namespace RestTaxAPI.Commands
{
    using RestTaxAPI.ViewModels;
    using Boxed.AspNetCore;

    public interface IGetCarPageCommand : IAsyncCommand<PageOptions>
    {
    }
}
