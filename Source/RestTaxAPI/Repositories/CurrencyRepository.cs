namespace RestTaxAPI.Repositories
{
    using System.Collections.Generic;
    using RestTaxAPI.Models;
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> GetAllAllowed();
    }
    
    public class CurrencyRepository : ICurrencyRepository
    {
        private static readonly List<Currency> AllowedCurrencies = new List<Currency>()
        {
            new Currency() { Code = "EUR", Number = 978, Name = "Euro" },
            new Currency() { Code = "CAD", Number = 124, Name = "Canadian dollar" },
            new Currency() { Code = "USD", Number = 840, Name = "United States dollar" },
        };

        public IEnumerable<Currency> GetAllAllowed() => AllowedCurrencies;
    }
}
