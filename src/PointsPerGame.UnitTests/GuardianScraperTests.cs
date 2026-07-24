using NUnit.Framework;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;
using Shouldly;
using System.Net;

namespace PointsPerGame.UnitTests;

public class GuardianScraperTests
{
    [Test]
    public async Task GetResultsAsync_Treats_The_Generic_Tables_Page_As_NotFound()
    {
        var responseUri = new Uri("https://www.theguardian.com/football/tables");
        var scraper = new GuardianScraper(new StubHttpClientFactory(responseUri));

        var exception = await Should.ThrowAsync<HttpRequestException>(
            async () => await scraper.GetResultsAsync(TableSelection.SerieA));

        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private sealed class StubHttpClientFactory(Uri responseUri) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new(new StubHttpMessageHandler(responseUri));
    }

    private sealed class StubHttpMessageHandler(Uri responseUri) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("<html><body></body></html>"),
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, responseUri),
            };

            return Task.FromResult(response);
        }
    }
}
