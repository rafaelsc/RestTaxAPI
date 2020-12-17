namespace RestTaxAPI.Commands
{
    using System;
    using System.Diagnostics;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Models;
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


        public PostCalculateTaxAndExchangeRatesCommand(IExchangeRateService exchangeRateService, ITaxCalculatorService taxCalculatorService)
        {
            this.ExchangeRateService = exchangeRateService;
            this.TaxCalculatorService = taxCalculatorService;
        }

        public IActionResult Execute(InvoiceRequest requestData)
        {
            Debug.Assert(requestData != null, "requestData != null");

            var invoiceRequest = NormalizeData(requestData);

            Debug.Assert(invoiceRequest != null, "invoiceRequest != null");
            Debug.Assert(invoiceRequest.PreTaxAmountInCents != null, "invoiceRequest.PreTaxAmountInCents != null");

            var sourceCurrency = invoiceRequest.PreTaxAmountCurrencyCode;
            var destinationCurrency = invoiceRequest.PaymentCurrencyCode;

            var exchangeRate = this.ExchangeRateService.GetExchangeRate(sourceCurrency, destinationCurrency);
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

        private static InvoiceRequest NormalizeData(InvoiceRequest requestData) =>
            requestData with
            {
                PaymentCurrencyCode = requestData.PaymentCurrencyCode.ToUpperInvariant(),
                PreTaxAmountCurrencyCode = requestData.PreTaxAmountCurrencyCode.ToUpperInvariant()
            };
    }
}
