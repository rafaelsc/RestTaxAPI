namespace RestTaxAPI.Constants
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class CurrenciesControllerRoute
    {
        public const string OptionsCurrencies = ControllerName.Currencies + nameof(OptionsCurrencies);
        public const string HeadCurrencies = ControllerName.Currencies + nameof(HeadCurrencies);
        public const string GetCurrencies = ControllerName.Currencies + nameof(GetCurrencies);
    }

    /// <summary>
    /// TODO
    /// </summary>
    public static class InvoiceCurrencyExchangeControllerRoute
    {
        public const string Options = ControllerName.InvoiceCurrencyExchange + nameof(Options);
        public const string CalculateTaxAndExchangeRates = ControllerName.InvoiceCurrencyExchange + nameof(CalculateTaxAndExchangeRates);
    }
}
