namespace RestTaxAPI.Commands
{
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Repositories;

    public interface IGetCurrenciesCommand : ICommand
    {
    }

    internal class GetCurrenciesCommand : IGetCurrenciesCommand
    {
        private readonly ICurrencyRepository currencyRepository;

        public GetCurrenciesCommand(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public IActionResult Execute()
        {
            var result = this.currencyRepository.GetAllAllowed();
            return new OkObjectResult(result);
        }
    }
}
