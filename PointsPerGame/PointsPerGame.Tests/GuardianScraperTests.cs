using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.Tests;

public class GuardianScraperTests
{
	[Test]
	public async Task GetResultsAsync_Reads_Team_Details_From_Table_Structure_Not_Generated_Css_Classes()
	{
		const string rowHtml = """
			<td class="dcr-sz4gcj">1</td><th scope="row"><div class="dcr-2ny8cj"><picture class="dcr-twj9nh"><img srcset="https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&amp;dpr=1&amp;s=none&amp;crop=none, https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&amp;dpr=2&amp;s=none&amp;crop=none 2x" src="https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&amp;dpr=1&amp;s=none&amp;crop=none" alt="" class="dcr-a1c492"></picture><a href="/football/birminghamcityfc" class="dcr-1d5op0q">Birmingham</a></div></th><td>0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td>0</td><td><b class="dcr-or6g55">0</b></td><td><div class="dcr-1lbgi1b"></div></td>
			""";
		var html = $"<html><body><table><tbody><tr>{rowHtml}</tr></tbody></table></body></html>";
		var scraper = new GuardianScraper(new StubHttpClientFactory(html));

		var results = await scraper.GetResultsAsync(League.EnglishChampionship);

		results.Should().ContainSingle();
		var team = results.Single();
		team.TeamName.Should().Be("Birmingham");
		team.TeamUrl.Should().Be("https://www.theguardian.com/football/birminghamcityfc/fixtures");
		team.TeamCrest.Should().Be("https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&dpr=1&s=none&crop=none");
		team.Played.Should().Be(0);
		team.Points.Should().Be(0);
	}

	private sealed class StubHttpClientFactory : IHttpClientFactory
	{
		private readonly string html;

		public StubHttpClientFactory(string html)
		{
			this.html = html;
		}

		public HttpClient CreateClient(string name)
		{
			return new(new StubHttpMessageHandler(html));
		}
	}

	private sealed class StubHttpMessageHandler : HttpMessageHandler
	{
		private readonly string html;

		public StubHttpMessageHandler(string html)
		{
			this.html = html;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(html, Encoding.UTF8, "text/html"),
			});
		}
	}
}
