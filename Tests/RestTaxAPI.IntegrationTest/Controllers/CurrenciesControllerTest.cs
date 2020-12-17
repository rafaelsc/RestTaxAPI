namespace RestTaxAPI.IntegrationTest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
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

    public class CurrenciesControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public CurrenciesControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Options_CurrencyRoot_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "currencies");

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(new[] { HttpMethods.Get, HttpMethods.Head, HttpMethods.Options }, response.Content.Headers.Allow);
        }

        [Fact]
        public async Task Get_Currencies_Returns200OkAsync()
        {
            var response = await this.client.GetAsync(new Uri("currencies", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = (await response.Content.ReadAsAsync<IEnumerable<Currency>>(this.formatters).ConfigureAwait(false))?.ToList();

            Assert.NotNull(data);
            Assert.NotEmpty(data);
            Assert.All(data, Assert.NotNull);
            Assert.Collection(data, i => Assert.NotEmpty(i.Code),
                                    i => Assert.InRange(i.Number, 0, 999),
                                    i => Assert.NotEmpty(i.Name));
        }
    }
}
