using HtmlAgilityPack;
using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PointsPerGame.Core.Web;

public class GuardianScraper : BaseScraper
{
	private static readonly MemoryCache cache = new(nameof(GuardianScraper));
	private readonly GuardianTableParser tableParser;

	public GuardianScraper(IHttpClientFactory httpClientFactory) : this(httpClientFactory, new GuardianTableParser())
	{
	}

	public GuardianScraper(IHttpClientFactory httpClientFactory, GuardianTableParser tableParser) : base(httpClientFactory)
	{
		this.tableParser = tableParser ?? throw new ArgumentNullException(nameof(tableParser));
	}

	public async Task<List<TeamResultDisplaySet>> GetMultipleLeagueResults(IEnumerable<League> leagues)
	{
		var allResults = new List<ITeamResults>();

		foreach (var league in leagues)
		{
			var results = await GetResultsAsync(league);
			allResults.AddRange(results);
		}

		return allResults.Select(a => new TeamResultDisplaySet(a)).SortTeams().ToList();
	}

	public override async Task<List<TeamResultDisplaySet>> GetResultsAsync(League league)
	{
		if (TryGetCachedResults(league, out var cachedResults))
		{
			return cachedResults;
		}

		var doc = new HtmlDocument();
		var leagueUri = GuardianLeagueMappings.GetUriForLeague(league);
		var stream = await GetPageStreamAsync(leagueUri);

		doc.Load(stream, Encoding.UTF8);

		var teamData = tableParser.Parse(doc)
			.Select(results => new TeamResultDisplaySet(results))
			.SortTeams()
			.ToList();

		CacheResults(league, teamData);

		return teamData;
	}

	private static bool TryGetCachedResults(League league, out List<TeamResultDisplaySet> results)
	{
		if (cache.Get(GetCacheKey(league)) is List<TeamResultDisplaySet> cachedResults)
		{
			results = cachedResults;
			return true;
		}

		results = [];
		return false;
	}

	private static void CacheResults(League league, List<TeamResultDisplaySet> results)
	{
		cache.Set(GetCacheKey(league), results, DateTimeOffset.Now.AddMinutes(5));
	}

	private static string GetCacheKey(League league)
	{
		return league.ToString();
	}
}
