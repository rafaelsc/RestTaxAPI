namespace RestTaxAPI.Commands
{
    using System;
    using System.Diagnostics;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    /// <summary>
    /// TODO
    /// </summary>
    public interface IPostCalculateTaxAndExchangeRatesCommand : ICommand<InvoiceRequest>
    {
    }

    internal class PostCalculateTaxAndExchangeRatesCommand : IPostCalculateTaxAndExchangeRatesCommand
    {
        public IActionResult Execute(InvoiceRequest requestData)
        {
            Debug.Assert(requestData != null, "requestData != null");
            Debug.Assert(requestData.PreTaxAmountInCents != null, "requestData.PreTaxAmountInCents != null");

            var exchangeRate = 1M; //TODO
            var taxOfCurrency = 0.09M; //TODO

            var preTaxTotal = (long)Math.Ceiling(requestData.PreTaxAmountInCents.Value * exchangeRate);
            var calculatedTax = (long)Math.Ceiling(requestData.PreTaxAmountInCents.Value * taxOfCurrency);
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
    }
}
