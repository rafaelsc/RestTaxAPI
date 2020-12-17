namespace RestTaxAPI.Commands
{
    using RestTaxAPI.ViewModels;
    using Boxed.AspNetCore;

    public interface IPutCarCommand : IAsyncCommand<int, SaveCar>
    {
    }
}
