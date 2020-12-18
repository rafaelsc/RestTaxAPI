namespace RestTaxAPI.Services
{
    using System;
    using System.Diagnostics;
    using Models;

    internal class CalculateExchangeService
    {
        private IExchangeRateService ExchangeRateService { get; }
        private ITaxCalculatorService TaxCalculatorService { get; }

        public CalculateExchangeService(IExchangeRateService exchangeRateService, ITaxCalculatorService taxCalculatorService)
        {
            this.ExchangeRateService = exchangeRateService;
            this.TaxCalculatorService = taxCalculatorService;
        }

        public InvoiceResponse CalculateExchange(InvoiceRequest requestData, InvoiceRequest invoiceRequest)
        {
            Debug.Assert(invoiceRequest != null, "invoiceRequest != null");
            Debug.Assert(invoiceRequest.Date != null, "invoiceRequest.Date != null");
            Debug.Assert(invoiceRequest.PreTaxAmountInCents != null, "invoiceRequest.PreTaxAmountInCents != null");

            var sourceCurrency = invoiceRequest.PreTaxAmountCurrencyCode;
            var destinationCurrency = invoiceRequest.PaymentCurrencyCode;

            var exchangeRate = this.ExchangeRateService.GetExchangeRate(invoiceRequest.Date.Value, sourceCurrency, destinationCurrency);
            var taxOfCurrency = this.TaxCalculatorService.GetTaxPercentage(destinationCurrency);

            var preTaxTotal = (long) Math.Round(invoiceRequest.PreTaxAmountInCents.Value * exchangeRate, MidpointRounding.AwayFromZero);
            var calculatedTax = (long) Math.Round(preTaxTotal * taxOfCurrency, MidpointRounding.AwayFromZero);
            var grandTotal = preTaxTotal + calculatedTax;

            var responseData = new InvoiceResponse()
            {
                CurrencyCode = requestData.PaymentCurrencyCode,
                ExchangeRate = exchangeRate,
                PreTaxTotalInCents = preTaxTotal,
                TaxAmountInCents = calculatedTax,
                GrandTotalInCents = grandTotal,
            };

            return responseData;
        }
    }
}
