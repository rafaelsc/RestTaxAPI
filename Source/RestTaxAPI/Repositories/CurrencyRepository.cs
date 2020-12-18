namespace RestTaxAPI.Repositories
{
    using System.Collections.Generic;
    using RestTaxAPI.Models;

    /// <summary>
    /// TODO
    /// </summary>
    public interface ICurrencyRepository
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        IEnumerable<Currency> GetAllAllowed();
    }

    internal class CurrencyRepository : ICurrencyRepository
    {
        private static readonly List<Currency> AllowedCurrencies = new()
        {
            new() { Code = "EUR", Number = 978, Name = "Euro" },
            new() { Code = "CAD", Number = 124, Name = "Canadian dollar" },
            new() { Code = "USD", Number = 840, Name = "United States dollar" },
        };

        public IEnumerable<Currency> GetAllAllowed() => AllowedCurrencies;
    }
}
