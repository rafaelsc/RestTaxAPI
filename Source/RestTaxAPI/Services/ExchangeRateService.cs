namespace RestTaxAPI.Services
{
    using System;
    using System.Collections.Concurrent;
    using FixerSharp;
    using Options;

    /// <summary>
    /// TODO
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="date"></param>
        /// <param name="sourceCurrency"></param>
        /// <param name="destinationCurrency"></param>
        /// <returns></returns>
        decimal GetExchangeRate(DateTime date, string sourceCurrency, string destinationCurrency);
    }

    internal class ExchangeRateService : IExchangeRateService
    {
        public ExchangeRateService(FixerOptions config) => Fixer.SetApiKey(config.ApiKey);

        public decimal GetExchangeRate(DateTime date, string sourceCurrency, string destinationCurrency)
        {
            if (sourceCurrency == destinationCurrency)
                return 1;

            var rate = Fixer.Rate(sourceCurrency, destinationCurrency, date);
            var exchangeRate = rate.Convert(1);
            return (decimal)exchangeRate;
        }
    }

    internal class CachedExchangeRateService : IExchangeRateService //TODO Test this Cached Service. //TODO A lot of improvement in this class.
    {
        private static TimeSpan CacheTimeout => TimeSpan.FromDays(1); //TODO: Add this to a config file.

        private static readonly ConcurrentDictionary<(DateTime, string, string), (DateTimeOffset, decimal)> Cache = new();

        private ExchangeRateService ExchangeRateService { get; }

        public CachedExchangeRateService(ExchangeRateService exchangeRateService) => this.ExchangeRateService = exchangeRateService;

        public decimal GetExchangeRate(DateTime date, string sourceCurrency, string destinationCurrency)
        {
            if (sourceCurrency == destinationCurrency)
                return 1;

            //TODO, Don't use the System Time, to enable a easy mock.

            var cachedData = Cache.GetOrAdd((date, sourceCurrency, destinationCurrency), (i) => (DateTimeOffset.UtcNow, this.ExchangeRateService.GetExchangeRate(date, sourceCurrency, destinationCurrency)));
            if (DateTimeOffset.UtcNow - cachedData.Item1 < CacheTimeout)
                return cachedData.Item2; //Return the Cached rate;

            //TODO, BUG, If the ExchangeRateService.GetExchangeRate return slower than CacheTimeoutInSeconds a cache miss will happen.

            var currentRate = this.ExchangeRateService.GetExchangeRate(date, sourceCurrency, destinationCurrency);
            Cache[(date, sourceCurrency, destinationCurrency)] = (DateTimeOffset.UtcNow, currentRate); //Update rate in the Cache
            return currentRate;
        }
    }
}
