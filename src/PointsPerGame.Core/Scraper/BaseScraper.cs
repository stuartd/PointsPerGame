using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Web;

public abstract class BaseScraper(IHttpClientFactory httpClientFactory) : IResultsDataSource
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    protected async Task<string> GetPageHtmlAsync(string url)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new("text/html"));
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:128.0) Gecko/20100101 Firefox/128.0");

        using var response = await httpClient.GetAsync(new Uri(url));
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public abstract ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection);
}
