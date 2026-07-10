using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using PointsPerGame.Core.Web;

namespace PointsPerGame.UnitTests;

public class LeagueTableServiceTests
{
	[Test]
	public async Task GetResultsAsync_For_Single_League_Requests_That_League()
	{
		var dataSource = new StubDataSource();
		dataSource.SetResults(League.EnglishChampionship, Team("Championship", points: 30, played: 10));
		var service = new LeagueTableService(dataSource);

		var results = await service.GetResultsAsync(League.EnglishChampionship);

		dataSource.RequestedLeagues.Should().Equal(League.EnglishChampionship);
		results.Should().ContainSingle(r => r.TeamName == "Championship");
	}

	[Test]
	public async Task GetResultsAsync_For_All_Requests_Every_Source_League()
	{
		var dataSource = new StubDataSource();
		foreach (var league in LeagueLists.AllLeagues)
		{
			dataSource.SetResults(league, Team(league.ToString(), points: 30, played: 10));
		}
		var service = new LeagueTableService(dataSource);

		var results = await service.GetResultsAsync(League.All);

		dataSource.RequestedLeagues.Should().Equal(LeagueLists.AllLeagues);
		results.Should().HaveCount(LeagueLists.AllLeagues.Length);
	}

	[Test]
	public async Task GetResultsAsync_For_AllTopDivisions_Requests_Top_Division_Leagues()
	{
		var dataSource = new StubDataSource();
		foreach (var league in LeagueLists.AllTopDivisions)
		{
			dataSource.SetResults(league, Team(league.ToString(), points: 30, played: 10));
		}
		var service = new LeagueTableService(dataSource);

		var results = await service.GetResultsAsync(League.AllTopDivisions);

		dataSource.RequestedLeagues.Should().Equal(LeagueLists.AllTopDivisions);
		results.Should().HaveCount(LeagueLists.AllTopDivisions.Length);
	}

	[Test]
	public async Task GetResultsAsync_Sorts_Combined_Results()
	{
		var dataSource = new StubDataSource();
		dataSource.SetResults(League.EnglishPremierLeague,
			Team("lower", points: 30, played: 10),
			Team("higher", points: 31, played: 10));
		var service = new LeagueTableService(dataSource);

		var results = await service.GetResultsAsync(League.EnglishPremierLeague);

		results.Select(r => r.TeamName).Should().Equal("higher", "lower");
	}

    private static TeamResultDisplaySet Team(string name, int points, int played) => new(new TeamResults
    {
        TeamName = name,
        TeamUrl = string.Empty,
        TeamCrest = string.Empty,
        Played = played,
        Points = points,
    });

    private sealed class StubDataSource : IDataSource
	{
		private readonly Dictionary<League, IReadOnlyList<TeamResultDisplaySet>> resultsByLeague = [];

		public List<League> RequestedLeagues { get; } = [];

        public void SetResults(League league, params TeamResultDisplaySet[] results) => resultsByLeague[league] = [.. results];

        public Task<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league)
		{
			RequestedLeagues.Add(league);

			if (resultsByLeague.TryGetValue(league, out var leagueResults))
			{
				return Task.FromResult<IReadOnlyList<TeamResultDisplaySet>>([.. leagueResults]);
			}

			return Task.FromResult<IReadOnlyList<TeamResultDisplaySet>>([]);
		}
    }
}
