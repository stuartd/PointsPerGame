using HtmlAgilityPack;
using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PointsPerGame.Core.Web;

public class GuardianScraper : BaseScraper
{
	private static readonly MemoryCache cache = new(nameof(GuardianScraper));

	public GuardianScraper(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
	{
	}

	public async Task<List<TeamResultDisplaySet>> GetMultipleLeagueResults(IEnumerable<League> leagues)
	{
		var allResults = new List<ITeamResults>();

		foreach (var league in leagues)
		{
			var results = await GetResultsAsync(league);

			// ok closing in now
			// we get the raw results below
			allResults.AddRange(results);
		}

		return allResults.Select(a => new TeamResultDisplaySet(a)).SortTeams().ToList();
	}

	public override async Task<List<TeamResultDisplaySet>> GetResultsAsync(League league)
	{
// #if RELEASE
#if DEBUG
		if (cache.Contains(league.ToString()))
		{
			return cache[league.ToString()] as List<TeamResultDisplaySet>;
		}
#endif
		List<TeamResultDisplaySet> teamData = [];

		var doc = new HtmlDocument();

		// ok this is all mixed up
		// (thanks, dotnet/modernizer)

		// For a 'league' (on or many tables)
		// construct a data set of:
		// Team / pld / won / pts etc


		//if (league == League.All) {
		//	teams = await GetMultipleLeagueResults(LeagueLists.AllLeagues);
		//}
		//else if (league == League.AllTopDivisions)
		//{
		//	teams = await GetMultipleLeagueResults(LeagueLists.AllTopDivisions);
		//}
		//else
		//{
		//	teams = await GetResults(league);
		//	leagueUri = GuardianLeagueMappings.GetUriForLeague(league);
		//}

		string leagueUri = GuardianLeagueMappings.GetUriForLeague(league);

		var stream = await GetPageStreamAsync(leagueUri);

		doc.Load(stream, Encoding.UTF8);

		var table = doc.DocumentNode.SelectNodes("//table").FirstOrDefault();

		if (table == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find table.");
		}

		var body = table.SelectNodes(".//tbody").FirstOrDefault();

		if (body == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find table body.");
		}

		var rows = body.SelectNodes(".//tr");

		if (rows == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find rows.");
		}

		foreach (var row in rows)
		{
			teamData.Add(new(ParseTeamResults(row)));
		}

		cache.Add(league.ToString(), teamData, DateTimeOffset.Now.AddMinutes(5));

		return teamData.SortTeams().ToList();
	}

	private static TeamResults ParseTeamResults(HtmlNode row)
	{
		var cells = row.SelectNodes("./td");

		if (cells == null || cells.Count < 9)
		{
			throw new InvalidOperationException("Guardian have changed their site again - expected at least 9 table cells in a team row.");
		}

		var teamHeader = row.SelectSingleNode("./th[@scope='row']") ?? row.SelectSingleNode("./th");

		if (teamHeader == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find the team row header.");
		}

		// Guardian's dcr-* class names are generated, so parse from table semantics instead.
		var anchor = teamHeader.SelectSingleNode(".//a[@href]");

		if (anchor == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find the team link.");
		}

		var teamName = WebUtility.HtmlDecode(anchor.InnerText).Trim();

		if (string.IsNullOrWhiteSpace(teamName))
		{
			throw new InvalidOperationException("Guardian have changed their site again - found a team link without a team name.");
		}

		var crestSource = teamHeader.SelectSingleNode(".//img[@src]");
		var crest = crestSource?.GetAttributeValue("src", string.Empty) ?? string.Empty;
		var teamUrl = anchor.GetAttributeValue("href", string.Empty);

		return new()
		{
			TeamName = teamName,
			TeamUrl = CreateTeamFixturesUri(teamUrl).ToString(),
			TeamCrest = WebUtility.HtmlDecode(crest),
			Played = GetCellInt(cells, 1, "played"),
			Won = GetCellInt(cells, 2, "won"),
			Drawn = GetCellInt(cells, 3, "drawn"),
			Lost = GetCellInt(cells, 4, "lost"),
			GoalsScored = GetCellInt(cells, 5, "goals scored"),
			GoalsConceded = GetCellInt(cells, 6, "goals conceded"),
			Points = GetCellInt(cells, 8, "points"),
		};
	}

	private static int GetCellInt(HtmlNodeCollection cells, int index, string columnName)
	{
		var value = WebUtility.HtmlDecode(cells[index].InnerText).Trim();

		if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			throw new InvalidOperationException($"Guardian have changed their site again - couldn't parse {columnName} value '{value}'.");
		}

		return result;
	}

	private static Uri CreateTeamFixturesUri(string teamUrl)
	{
		if (string.IsNullOrWhiteSpace(teamUrl))
		{
			throw new InvalidOperationException("Guardian have changed their site again - found a team link without a URL.");
		}

		var baseUri = new Uri("https://www.theguardian.com/");
		var uri = Uri.TryCreate(teamUrl, UriKind.Absolute, out var absoluteUri)
			? absoluteUri
			: new Uri(baseUri, teamUrl.TrimStart('/'));

		if (!uri.AbsolutePath.EndsWith("/fixtures", StringComparison.OrdinalIgnoreCase))
		{
			uri = new Uri(uri, $"{uri.AbsolutePath.TrimEnd('/')}/fixtures");
		}

		return CreateUri(uri.ToString());
	}

	private static Uri CreateUri(string url)
	{
		var uri = new Uri(url);

		// convert // to /
		string normalizedPath = string.Join("/",
			uri.AbsolutePath.Split(['/'], StringSplitOptions.RemoveEmptyEntries)
		);

		var builder = new UriBuilder(uri)
		{
			Path = normalizedPath,
		};

		return builder.Uri;
	}
}
