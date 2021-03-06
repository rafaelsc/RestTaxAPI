namespace RestTaxAPI
{
    using RestTaxAPI.Commands;
    using RestTaxAPI.Repositories;
    using RestTaxAPI.Services;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods add project services.
    /// </summary>
    /// <remarks>
    /// AddSingleton - Only one instance is ever created and returned.
    /// AddScoped - A new instance is created and returned for each request/response cycle.
    /// AddTransient - A new instance is created and returned each time.
    /// </remarks>
    internal static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services
                .AddSingleton<IGetCurrenciesCommand, GetCurrenciesCommand>()
                .AddSingleton<IPostCalculateTaxAndExchangeRatesCommand, PostCalculateTaxAndExchangeRatesCommand>();

        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services;

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<ICurrencyRepository, CurrencyRepository>();

        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>()
                .AddSingleton<IExchangeRateService, CachedExchangeRateService>()
                .AddSingleton<ExchangeRateService>()
                .AddSingleton<ITaxCalculatorService, FixedTaxCalculatorService>()
                .AddSingleton<CalculateExchangeService>();
    }
}
