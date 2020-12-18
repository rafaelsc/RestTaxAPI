namespace RestTaxAPI.Commands
{
    using System.Diagnostics;
    using System.Linq;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private ICurrencyRepository Repository { get; }
        private CalculateExchangeService ExchangeService { get; }

        public PostCalculateTaxAndExchangeRatesCommand(ICurrencyRepository repository, CalculateExchangeService exchangeService)
        {
            this.Repository = repository;
            this.ExchangeService = exchangeService;
        }

        public IActionResult Execute(InvoiceRequest requestData)
        {
            Debug.Assert(requestData != null, "requestData != null");

            var invoiceRequest = NormalizeData(requestData);

            var sourceCurrency = invoiceRequest.PreTaxAmountCurrencyCode;
            var destinationCurrency = invoiceRequest.PaymentCurrencyCode;

            //TODO Improve Model Validation
            if (this.ValidateModel(sourceCurrency, destinationCurrency, out var badRequestActionResult))
                return badRequestActionResult; //BUG: This return is not in the same format that the Swagger is defined. Need more studies how to solve.

            var responseData = this.ExchangeService.CalculateExchange(requestData, invoiceRequest);

            return new OkObjectResult(responseData);
        }

        private bool ValidateModel(string sourceCurrency, string destinationCurrency, out IActionResult badRequestActionResult)
        {
            badRequestActionResult = null;

            var allowedCurrencies = this.Repository.GetAllAllowed().ToArray();
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
