using System.Collections;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;
using NUnit.Framework;

namespace PointsPerGame.UnitTests;

[Explicit]
[TestFixture]
public class When_Retrieving_Tables_From_Guardian_Website
{
	private readonly IDataSource dataSource = new GuardianScraper(TestHttpClientFactory.Instance);

	[TestCaseSource(typeof(TableTestCaseSource), nameof(TableTestCaseSource.Tables))]
	[Test]
	public async Task All_Tables_Should_Be_Retrievable(League league)
	{
		var teams = await dataSource.GetResultsAsync(league);

		// If the link is wrong, the table list page is returned, which then only returns 4 values.
		Assert.That(teams.Count, Is.GreaterThan(4));
	}

	private sealed class TestHttpClientFactory : IHttpClientFactory
	{
		public static readonly IHttpClientFactory Instance = new TestHttpClientFactory();

		private TestHttpClientFactory()
		{
		}

		public HttpClient CreateClient(string name) => new();
	}

	private class TableTestCaseSource
	{
		public static IEnumerable Tables
		{
			get
			{
				foreach (var league in Enum.GetValues<League>())
				{
					if (league is League.AllTopDivisions or League.All)
					{
						continue;
					}

					yield return league;
				}
			}
		}
	}
}