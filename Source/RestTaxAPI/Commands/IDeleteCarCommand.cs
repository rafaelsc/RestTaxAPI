namespace RestTaxAPI.Commands
{
    using Boxed.AspNetCore;

    public interface IDeleteCarCommand : IAsyncCommand<int>
    {
    }
}
