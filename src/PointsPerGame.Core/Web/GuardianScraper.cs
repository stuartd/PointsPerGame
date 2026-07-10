using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using System.Runtime.Caching;

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

    public override async Task<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league)
    {
        if (TryGetCachedResults(league, out var cachedResults))
        {
            return cachedResults;
        }

        var leagueUri = GuardianLeagueMappings.GetUriForLeague(league);
        var html = await GetPageHtmlAsync(leagueUri);

        var teamData = tableParser.Parse(html)
            .Select(results => new TeamResultDisplaySet(results))
            .SortTeams();

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

    private static void CacheResults(League league, IReadOnlyList<TeamResultDisplaySet> results) => cache.Set(GetCacheKey(league), results, DateTimeOffset.Now.AddMinutes(5));

    private static string GetCacheKey(League league) => league.ToString();
}
