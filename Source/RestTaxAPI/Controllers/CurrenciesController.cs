namespace RestTaxAPI.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using RestTaxAPI.Commands;
    using RestTaxAPI.Constants;
    using RestTaxAPI.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using Models;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class CurrenciesController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>
        ///     A 200 OK response.
        /// </returns>
        [HttpOptions(Name = CurrenciesControllerRoute.OptionsCurrencies)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(HeaderNames.Allow, HttpMethods.Get, HttpMethods.Head, HttpMethods.Options);
            return this.Ok();
        }

        /// <summary>
        /// Gets a collection of allowed Currencies.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <returns>
        ///     A 200 OK response containing a collection of All allowed Currencies in this API
        /// </returns>
        [HttpGet("", Name = CurrenciesControllerRoute.GetCurrencies)]
        [HttpHead("", Name = CurrenciesControllerRoute.HeadCurrencies)]
        [SwaggerResponse(StatusCodes.Status200OK, "A collection of all allowed Currencies.", typeof(Currency[]))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public IActionResult GetPage([FromServices] IGetCurrenciesCommand command) => command.Execute();
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
