using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using System.Net;
using System.Runtime.Caching;

namespace PointsPerGame.Core.Web;

public class GuardianScraper(IHttpClientFactory httpClientFactory, GuardianTableParser tableParser)
    : BaseScraper(httpClientFactory) {
    
    private static readonly MemoryCache cache = new(nameof(GuardianScraper));
    private readonly GuardianTableParser tableParser = tableParser ?? throw new ArgumentNullException(nameof(tableParser));

    public GuardianScraper(IHttpClientFactory httpClientFactory) : this(httpClientFactory, new GuardianTableParser())
    {
    }

    public override async ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection)
    {
        if (TryGetCachedResults(tableSelection, out var cachedResults))
        {
            return cachedResults;
        }

        var leagueUri = GuardianLeagueMappings.GetUriForLeague(tableSelection);
        var (html, finalUri) = await GetPageHtmlAsync(leagueUri);

        if (IsGenericTablesPage(finalUri))
        {
            throw new HttpRequestException(
                $"The Guardian league page redirected to the generic tables page: {finalUri}",
                inner: null,
                HttpStatusCode.NotFound);
        }

        var teamData = tableParser.Parse(html);

        CacheResults(tableSelection, teamData);

        return teamData;
    }

    private static bool TryGetCachedResults(TableSelection tableSelection, out IReadOnlyList<TeamResults> results)
    {
        if (cache.Get(GetCacheKey(tableSelection)) is IReadOnlyList<TeamResults> cachedResults)
        {
            results = cachedResults;
            return true;
        }

        results = [];
        return false;
    }

    private static void CacheResults(TableSelection tableSelection, IReadOnlyList<TeamResults> results)
        => cache.Set(GetCacheKey(tableSelection), results, DateTimeOffset.Now.AddMinutes(5));

    private static string GetCacheKey(TableSelection tableSelection) => tableSelection.ToString();

    private static bool IsGenericTablesPage(Uri uri) =>
        uri.Host.Equals("www.theguardian.com", StringComparison.OrdinalIgnoreCase) &&
        uri.AbsolutePath.TrimEnd('/').Equals("/football/tables", StringComparison.OrdinalIgnoreCase);
}
