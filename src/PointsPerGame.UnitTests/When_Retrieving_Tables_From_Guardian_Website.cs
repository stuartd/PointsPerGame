using System.Collections;
using System.Net;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;
using NUnit.Framework;
using PointsPerGame.Core.Extensions;
using Shouldly;

namespace PointsPerGame.UnitTests;

[Explicit]
[TestFixture]
public class When_Retrieving_Tables_From_Guardian_Website
{
    private static readonly TimeSpan LinkCheckDelay = TimeSpan.FromSeconds(1);
    private readonly IResultsDataSource dataSource = new GuardianScraper(TestHttpClientFactory.Instance);

    [TestCaseSource(typeof(TableTestCaseSource), nameof(TableTestCaseSource.Tables))]
    [Test]
    public async Task All_Tables_Should_Be_Retrievable(TableSelection tableSelection)
    {
        var teams = await dataSource.GetResultsAsync(tableSelection);

        // If the link is wrong, the table list page is returned, which then only returns 4 values.
        teams.Count.ShouldBeGreaterThan(4);
    }

    [TestCaseSource(typeof(TableTestCaseSource), nameof(TableTestCaseSource.Tables))]
    [Test]
    public async Task All_Team_Fixture_Links_Should_Be_Retrievable(TableSelection tableSelection)
    {
        var teams = await dataSource.GetResultsAsync(tableSelection);

        var uniqueTeams = teams
            .GroupBy(team => team.TeamUrl, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .OrderBy(team => team.TeamName)
            .ToList();

        using var handler = new HttpClientHandler { AllowAutoRedirect = false };
        using var httpClient = new HttpClient(handler);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; PointsPerGame link checker)");
        httpClient.DefaultRequestHeaders.Accept.ParseAdd("text/html");
        var failures = new List<string>();
        var checkedLinkCount = 0;
        string? rateLimitStatus = null;

        for (var index = 0; index < uniqueTeams.Count; index++)
        {
            if (index > 0)
            {
                await Task.Delay(LinkCheckDelay);
            }

            var team = uniqueTeams[index];
            var result = await GetLinkStatusAsync(httpClient, team);
            TestContext.Progress.WriteLine(result.Status);

            if (result.Outcome == LinkOutcome.Broken)
            {
                failures.Add(result.Status);
            }
            else if (result.Outcome == LinkOutcome.Unchecked)
            {
                rateLimitStatus = result.Status;
                break;
            }

            checkedLinkCount++;
        }

        var summary = $"Checked {checkedLinkCount} of {uniqueTeams.Count} unique fixture links.";

        if (rateLimitStatus is not null)
        {
            summary += $" Stopped after Guardian rate-limited the checker: {rateLimitStatus}";
        }

        TestContext.Progress.WriteLine(summary);

        failures.ShouldBeEmpty(
            $"Broken Guardian fixture links:{Environment.NewLine}{string.Join(Environment.NewLine, failures)}");
    }

    private static async Task<LinkStatus> GetLinkStatusAsync(HttpClient httpClient, TeamResults team)
    {
        if (!Uri.TryCreate(team.TeamUrl, UriKind.Absolute, out var uri))
        {
            return new(LinkOutcome.Broken, $"{team.TeamName}: missing or invalid URL '{team.TeamUrl}'");
        }

        try
        {
            using var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            var redirect = response.Headers.Location;
            var redirectText = redirect is null ? string.Empty : $" -> {new Uri(uri, redirect)}";
            var status = $"{team.TeamName}: {(int)response.StatusCode} {response.StatusCode} - {uri}{redirectText}";
            var isRedirect = response.StatusCode is >= HttpStatusCode.MultipleChoices and < HttpStatusCode.BadRequest;

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                return new(LinkOutcome.Unchecked, status);
            }

            var outcome = response.IsSuccessStatusCode || (isRedirect && redirect is not null)
                ? LinkOutcome.Valid
                : LinkOutcome.Broken;

            return new(outcome, status);
        }
        catch (HttpRequestException exception)
        {
            return new(LinkOutcome.Broken, $"{team.TeamName}: request failed - {uri} ({exception.Message})");
        }
    }

    private sealed record LinkStatus(LinkOutcome Outcome, string Status);

    private enum LinkOutcome
    {
        Valid,
        Broken,
        Unchecked,
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
                foreach (var league in Enum.GetValues<TableSelection>())
                {
                    if (league.IsMultiLeague())
                    {
                        continue;
                    }

                    yield return league;
                }
            }
        }
    }
}
