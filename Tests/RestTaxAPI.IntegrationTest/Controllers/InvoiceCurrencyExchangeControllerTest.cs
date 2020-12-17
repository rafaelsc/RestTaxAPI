namespace RestTaxAPI.IntegrationTest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    public class InvoiceCurrencyExchangeControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public InvoiceCurrencyExchangeControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Options_InvoiceCurrencyExchangeRoot_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "invoice/currencyExchange");
            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(new[] { HttpMethods.Post, HttpMethods.Options }, response.Content.Headers.Allow);
        }

        public async Task Get_InvoiceCurrencyExchangeRoot_Returns200OkAsync()
        {
            var data = new InvoiceRequest { Date = new DateTime(2020, 8, 5, 0, 0, 0), PreTaxAmountCurrencyCode = "EUR", PreTaxAmountInCents = 12345, PaymentCurrencyCode = "USD" };
            var expected = new InvoiceResponse { CurrencyCode = "USD", PreTaxTotalInCents = 14657, GrandTotalInCents = 16123, ExchangeRate = 1.187247m };

            var response = await this.client.PostAsJsonAsync(new Uri("invoice/currencyExchange", UriKind.Relative), data).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseData = await response.Content.ReadAsAsync<InvoiceResponse>(this.formatters).ConfigureAwait(false);

            Assert.NotNull(responseData);
            Assert.Equal(expected, responseData);
        }
    }
}
