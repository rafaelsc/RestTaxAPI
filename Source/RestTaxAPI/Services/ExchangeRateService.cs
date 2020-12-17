namespace RestTaxAPI.Services
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using FixerSharp;
    using Models;
    using Repositories;

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
        public ExchangeRateService()
        {
            Fixer.SetApiKey("70917b78c21d431cd1f5f16e2763a551");
        }

        public decimal GetExchangeRate(string sourceCurrency, string destinationCurrency)
        {
            if (sourceCurrency == destinationCurrency)
                return 1;

            var rate = Fixer.Rate(sourceCurrency, Symbols.GBP);
            var exchangeRate = rate.Convert(1);
            return (decimal)exchangeRate;
        }
    }

    internal class CachedExchangeRateService : IExchangeRateService //TODO Test this Cached Service.
    {
        private static TimeSpan CacheTimeout => TimeSpan.FromHours(1); //TODO: Add this to a config file.

        private static readonly ConcurrentDictionary<(string, string), (DateTimeOffset, decimal)> Cache = new();

        private ExchangeRateService ExchangeRateService { get; }

        public CachedExchangeRateService(ExchangeRateService exchangeRateService, ICurrencyRepository currencyRepository)
        {
            this.ExchangeRateService = exchangeRateService;
            this.FillCache(currencyRepository.GetAllAllowed().ToArray());
        }

        private void FillCache(Currency[] currencies)
        {
            var allCommutations = from c1 in currencies
                                  from c2 in currencies
                                  where c1.Code != c2.Code
                                  select (Source: c1.Code, Destination: c2.Code);

            foreach (var curr in allCommutations)
            {
                this.GetExchangeRate(curr.Source, curr.Destination);
            }
        }

        public decimal GetExchangeRate(string sourceCurrency, string destinationCurrency)
        {
            if (sourceCurrency == destinationCurrency)
                return 1;

            var cachedData = Cache.GetOrAdd((sourceCurrency, destinationCurrency), (i) => (DateTimeOffset.UtcNow, this.ExchangeRateService.GetExchangeRate(i.Item1, i.Item2)));
            if (DateTimeOffset.UtcNow - cachedData.Item1 < CacheTimeout)
                return cachedData.Item2; //Return the Cached rate;

            //TODO, BUG, If the ExchangeRateService.GetExchangeRate return slower than CacheTimeoutInSeconds a cache miss will happen.

            var currentRate = this.ExchangeRateService.GetExchangeRate(sourceCurrency, destinationCurrency);
            Cache[(sourceCurrency, destinationCurrency)] = (DateTimeOffset.UtcNow, currentRate); //Update rate in the Cache
            return currentRate;
        }
    }
}
