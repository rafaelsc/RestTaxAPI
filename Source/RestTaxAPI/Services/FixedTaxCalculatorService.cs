namespace RestTaxAPI.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO
    /// </summary>
    public interface ITaxCalculatorService
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        decimal GetTaxPercentage(string currency);
    }

    internal class FixedTaxCalculatorService : ITaxCalculatorService
    {
        private static readonly IDictionary<string, decimal> TaxRates = new Dictionary<string, decimal>(3) { { "CAD", 0.11M }, { "USD", 0.10M }, { "EUR", 0.09M } }; //TODO: Move to a Configuration file
        public decimal GetTaxPercentage(string currency) => TaxRates[currency];
    }
}
