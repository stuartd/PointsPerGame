using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using System.Runtime.Caching;

namespace PointsPerGame.Core.Web;

public class GuardianScraper(IHttpClientFactory httpClientFactory, GuardianTableParser tableParser)
    : BaseScraper(httpClientFactory) {
    
    private static readonly MemoryCache cache = new(nameof(GuardianScraper));
    private readonly GuardianTableParser tableParser = tableParser ?? throw new ArgumentNullException(nameof(tableParser));

    public GuardianScraper(IHttpClientFactory httpClientFactory) : this(httpClientFactory, new GuardianTableParser())
    {
    }

    public override async ValueTask<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league)
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

    private static bool TryGetCachedResults(League league, out IReadOnlyList<TeamResultDisplaySet> results)
    {
        if (cache.Get(GetCacheKey(league)) is IReadOnlyList<TeamResultDisplaySet> cachedResults)
        {
            results = cachedResults;
            return true;
        }

        results = [];
        return false;
    }

    private static void CacheResults(League league, IReadOnlyList<TeamResultDisplaySet> results) 
        => cache.Set(GetCacheKey(league), results, DateTimeOffset.Now.AddMinutes(5));

    private static string GetCacheKey(League league) => league.ToString();
}
