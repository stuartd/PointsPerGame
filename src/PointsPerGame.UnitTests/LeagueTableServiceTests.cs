using NUnit.Framework;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using Shouldly;

namespace PointsPerGame.UnitTests;

public class LeagueTableServiceTests
{
	[Test]
	public async Task GetResultsAsync_For_Single_League_Requests_That_League()
	{
		var dataSource = new StubDataSource();
		dataSource.SetResults(TableSelection.EnglishChampionship, Team("Championship", points: 30, played: 10));
		var service = new LeagueTableService(dataSource);

		var results = await service.GetResultsAsync(TableSelection.EnglishChampionship);

		dataSource.RequestedLeagues.ShouldBe([TableSelection.EnglishChampionship]);
		results.Count(r => r.TeamName == "Championship").ShouldBe(1);
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

		var results = await service.GetResultsAsync(TableSelection.AllLeagues);

		dataSource.RequestedLeagues.ShouldBe(LeagueLists.AllLeagues);
		results.Count.ShouldBe(LeagueLists.AllLeagues.Length);
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

		var results = await service.GetResultsAsync(TableSelection.AllTopDivisions);

		dataSource.RequestedLeagues.ShouldBe(LeagueLists.AllTopDivisions);
		results.Count.ShouldBe(LeagueLists.AllTopDivisions.Length);
	}

	[Test]
	public async Task GetResultsAsync_Sorts_Combined_Results()
	{
		var dataSource = new StubDataSource();
		dataSource.SetResults(TableSelection.EnglishPremierLeague,
			Team("lower", points: 30, played: 10),
			Team("higher", points: 31, played: 10));
		var service = new LeagueTableService(dataSource);

		var results = await service.GetResultsAsync(TableSelection.EnglishPremierLeague);

		results.Select(r => r.TeamName).ShouldBe(["higher", "lower"]);
	}

	private static TeamResults Team(string name, int points, int played) => new()
	{
		TeamName = name,
		TeamUrl = string.Empty,
		TeamCrest = string.Empty,
		Played = played,
		Points = points,
	};

	private sealed class StubDataSource : IResultsDataSource
	{
		private readonly Dictionary<TableSelection, IReadOnlyList<TeamResults>> resultsByLeague = [];

		public List<TableSelection> RequestedLeagues { get; } = [];

		public void SetResults(TableSelection tableSelection, params TeamResults[] results) => resultsByLeague[tableSelection] = [.. results];

		public ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection)
		{
			RequestedLeagues.Add(tableSelection);

			if (resultsByLeague.TryGetValue(tableSelection, out var leagueResults))
			{
				return ValueTask.FromResult<IReadOnlyList<TeamResults>>([.. leagueResults]);
			}

			return ValueTask.FromResult<IReadOnlyList<TeamResults>>([]);
		}
    }
}
