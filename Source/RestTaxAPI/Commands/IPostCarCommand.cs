namespace RestTaxAPI.Commands
{
    using RestTaxAPI.ViewModels;
    using Boxed.AspNetCore;

    public interface IPostCarCommand : IAsyncCommand<SaveCar>
    {
    }
}
