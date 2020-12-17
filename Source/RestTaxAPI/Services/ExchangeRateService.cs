namespace RestTaxAPI.Services
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

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

            throw new NotImplementedException(); //TODO Call the fixer.io API to get the current Rates
        }
    }

    internal class CachedExchangeRateService : IExchangeRateService //TODO Test this Cached Service.
    {
        private const int CacheTimeoutInSeconds = 60; //TODO: Add this to a config file.

        private ConcurrentDictionary<(string, string), (DateTimeOffset, decimal)> Cache = new ConcurrentDictionary<(string, string), (DateTimeOffset, decimal)>();
        
        private ExchangeRateService ExchangeRateService { get; }

        public CachedExchangeRateService(ExchangeRateService exchangeRateService)
        {
            this.ExchangeRateService = exchangeRateService;
        }

        public decimal GetExchangeRate(string sourceCurrency, string destinationCurrency)
        {
            if (sourceCurrency == destinationCurrency)
                return 1;

            var cachedData = this.Cache.GetOrAdd((sourceCurrency, destinationCurrency), (i) => (DateTimeOffset.UtcNow, this.ExchangeRateService.GetExchangeRate(i.Item1, i.Item2)));
            if (DateTimeOffset.UtcNow - cachedData.Item1 < TimeSpan.FromSeconds(CacheTimeoutInSeconds))
                return cachedData.Item2; //Return the Cached rate;

            //TODO, BUG, If the ExchangeRateService.GetExchangeRate return slower than CacheTimeoutInSeconds a cache miss will happen.

            var currentRate = this.ExchangeRateService.GetExchangeRate(sourceCurrency, destinationCurrency);
            this.Cache[(sourceCurrency, destinationCurrency)] = (DateTimeOffset.UtcNow, currentRate); //Update rate in Cache
            return currentRate;
        }
    }
}
