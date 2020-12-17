namespace RestTaxAPI.Commands
{
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// TODO
    /// </summary>
    public interface IPostCalculateTaxAndExchangeRatesCommand : ICommand
    {
    }

    internal class PostCalculateTaxAndExchangeRatesCommand : IPostCalculateTaxAndExchangeRatesCommand
    {
        public IActionResult Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
