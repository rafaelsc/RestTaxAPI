namespace RestTaxAPI.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using RestTaxAPI.Commands;
    using RestTaxAPI.Constants;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using Models;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// The Currencies Controller
    /// </summary>
    [Route("invoice/currencyExchange")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class InvoiceCurrencyExchangeController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>
        ///     A 200 OK response.
        /// </returns>
        [HttpOptions(Name = InvoiceCurrencyExchangeControllerRoute.Options)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(HeaderNames.Allow, HttpMethods.Post, HttpMethods.Options);
            return this.Ok();
        }

        /// <summary>
        /// //TODO
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <returns>
        ///     A 200 OK response //TODO 
        ///     A 400 BadRequest response //TODO
        /// 
        /// </returns>
        [HttpPost("", Name = InvoiceCurrencyExchangeControllerRoute.CalculateTaxAndExchangeRates)]
        [SwaggerResponse(StatusCodes.Status200OK, "The Invoice with tax and exchanged", typeof(Currency[]))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The Invoice data is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public IActionResult PostPage([FromServices] IPostCalculateTaxAndExchangeRatesCommand command) => command.Execute();
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
