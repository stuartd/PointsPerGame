using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Models;

namespace PointsPerGame.UnitTests;

[TestFixture]
public class TeamResults_Sorting_Tests
{
	private List<TeamResultDisplaySet> teams = null!;

	[SetUp]
	public void Setup()
	{
		teams = [];

		SetupTeam("Whisky", points: 50, played: 20, goalDifference: 50);
		SetupTeam("Bravo", points: 40, played: 16, goalDifference: 50);
		// Insert the alphabetically later name first to prove the final name tie-break is applied.
		SetupTeam("Zulu", points: 40, played: 16, goalDifference: 60);
		SetupTeam("Alpha", points: 40, played: 16, goalDifference: 60);
		SetupTeam("Charlie", points: 42, played: 16, goalDifference: 60);
	}

	[Test]
	public void Teams_Are_Setup_Correctly()
	{
		var Whisky = teams.First();
		Whisky.TeamName.Should().Be("Whisky");
		Whisky.PointsPerGame.Should().Be(2.5);
		Whisky.Played.Should().Be(20);
		Whisky.GoalDifference.Should().Be(50);
	}

	[Test]
	public void TeamsResults_Sort_Correctly()
	{
		var sortedTeams = teams.SortTeams();
		sortedTeams[0].TeamName.Should().Be("Charlie");
		sortedTeams[1].TeamName.Should().Be("Alpha");
		sortedTeams[2].TeamName.Should().Be("Zulu");
		sortedTeams[3].TeamName.Should().Be("Bravo");
		sortedTeams[4].TeamName.Should().Be("Whisky");
	}

	private void SetupTeam(string name, int points, int played, int goalDifference)
	{
		const int baseGoals = 200;

		var teamResultSet = new TeamResults
		{
			TeamName = name,
			TeamUrl = string.Empty,
			TeamCrest = string.Empty,
			GoalsScored = baseGoals,
			GoalsConceded = baseGoals - goalDifference,
			Points = points,
			Played = played,
		};

		teams.Add(new(teamResultSet));
	}
}
