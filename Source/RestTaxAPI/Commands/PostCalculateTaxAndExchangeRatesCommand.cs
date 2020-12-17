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

        public PostCalculateTaxAndExchangeRatesCommand(IExchangeRateService exchangeRateService)
        {
            this.ExchangeRateService = exchangeRateService;
        }

        public IActionResult Execute(InvoiceRequest requestData)
        {
            Debug.Assert(requestData != null, "requestData != null");

            var invoiceRequest = NormalizeData(requestData);

            Debug.Assert(invoiceRequest != null, "invoiceRequest != null");
            Debug.Assert(invoiceRequest.PreTaxAmountInCents != null, "invoiceRequest.PreTaxAmountInCents != null");

            var exchangeRate = this.ExchangeRateService.GetExchangeRate(invoiceRequest.PreTaxAmountCurrencyCode, invoiceRequest.PaymentCurrencyCode);
            var taxOfCurrency = 0.09M; //TODO

            var preTaxTotal = (long)Math.Ceiling(invoiceRequest.PreTaxAmountInCents.Value * exchangeRate);
            var calculatedTax = (long)Math.Ceiling(invoiceRequest.PreTaxAmountInCents.Value * taxOfCurrency);
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
