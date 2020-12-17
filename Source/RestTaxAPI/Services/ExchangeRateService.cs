namespace RestTaxAPI.Services
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="destinationCurrency"></param>
        /// <returns></returns>
        decimal GetExchangeRate(string sourceCurrency, string destinationCurrency);
    }

    internal class ExchangeRateService : IExchangeRateService
    {
        public decimal GetExchangeRate(string sourceCurrency, string destinationCurrency)
        {
            if (sourceCurrency == destinationCurrency)
                return 1;
            return 0; //TODO
        }
    }
}
