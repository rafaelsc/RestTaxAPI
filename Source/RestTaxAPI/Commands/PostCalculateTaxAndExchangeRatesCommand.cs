namespace RestTaxAPI.Commands
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Models;
    using Repositories;
    using Services;

    /// <summary>
    /// TODO
    /// </summary>
    public interface IPostCalculateTaxAndExchangeRatesCommand : ICommand<InvoiceRequest>
    {
    }

    internal class PostCalculateTaxAndExchangeRatesCommand : IPostCalculateTaxAndExchangeRatesCommand
    {
        private IExchangeRateService ExchangeRateService { get; }
        private ITaxCalculatorService TaxCalculatorService { get; }
        private readonly ICurrencyRepository CurrencyRepository;

        private IObjectModelValidator ModelValidator { get; }

        public PostCalculateTaxAndExchangeRatesCommand(IExchangeRateService exchangeRateService, ITaxCalculatorService taxCalculatorService, IObjectModelValidator modelValidator, ICurrencyRepository currencyRepository)
        {
            this.ExchangeRateService = exchangeRateService;
            this.TaxCalculatorService = taxCalculatorService;
            this.ModelValidator = modelValidator;
            this.CurrencyRepository = currencyRepository;
        }

        public IActionResult Execute(InvoiceRequest requestData)
        {
            Debug.Assert(requestData != null, "requestData != null");

            var invoiceRequest = NormalizeData(requestData);

            Debug.Assert(invoiceRequest != null, "invoiceRequest != null");
            Debug.Assert(invoiceRequest.Date != null, "invoiceRequest.Date != null");
            Debug.Assert(invoiceRequest.PreTaxAmountInCents != null, "invoiceRequest.PreTaxAmountInCents != null");

            var sourceCurrency = invoiceRequest.PreTaxAmountCurrencyCode;
            var destinationCurrency = invoiceRequest.PaymentCurrencyCode;

            if (this.ValidateModel(sourceCurrency, destinationCurrency, out var badRequestActionResult))
                return badRequestActionResult; //BUG: This return is not in the same format that the Swagger is defined. Need more studies how to solve.

            var exchangeRate = this.ExchangeRateService.GetExchangeRate(invoiceRequest.Date.Value, sourceCurrency, destinationCurrency);
            var taxOfCurrency = this.TaxCalculatorService.GetTaxPercentage(destinationCurrency);

            var preTaxTotal = (long)Math.Round(invoiceRequest.PreTaxAmountInCents.Value * exchangeRate, MidpointRounding.AwayFromZero);
            var calculatedTax = (long)Math.Round(preTaxTotal * taxOfCurrency, MidpointRounding.AwayFromZero);
            var grandTotal = preTaxTotal + calculatedTax;

            var responseData = new InvoiceResponse()
            {
                CurrencyCode = requestData.PaymentCurrencyCode,
                ExchangeRate = exchangeRate,
                PreTaxTotalInCents = preTaxTotal,
                TaxAmountInCents = calculatedTax,
                GrandTotalInCents = grandTotal,
            };

            return new OkObjectResult(responseData);
        }

        private bool ValidateModel(string sourceCurrency, string destinationCurrency, out IActionResult badRequestActionResult)
        {
            badRequestActionResult = null;

            var allowedCurrencies = this.CurrencyRepository.GetAllAllowed().ToArray();
            if (allowedCurrencies.Select(c => c.Code).Contains(sourceCurrency) == false)
            {
                var validationModel = new ModelStateDictionary();
                validationModel.AddModelError("PreTaxAmountCurrencyCode", $"The currency '{sourceCurrency}' is not allowed by this API.");

                badRequestActionResult = new BadRequestObjectResult(validationModel);
            }

            if (allowedCurrencies.Select(c => c.Code).Contains(destinationCurrency) == false)
            {
                var validationModel = new ModelStateDictionary();
                validationModel.AddModelError("PaymentCurrencyCode", $"The currency '{destinationCurrency}' is not allowed by this API.");

                badRequestActionResult = new BadRequestObjectResult(validationModel);
            }

            return badRequestActionResult != null;
        }

        private static InvoiceRequest NormalizeData(InvoiceRequest requestData) =>
            requestData with
            {
                PaymentCurrencyCode = requestData.PaymentCurrencyCode.ToUpperInvariant(),
                PreTaxAmountCurrencyCode = requestData.PreTaxAmountCurrencyCode.ToUpperInvariant()
            };
    }
}
