using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Models;

namespace PointsPerGame.Tests;

[TestFixture]
public class TeamResults_Sorting_Tests
{
	private List<TeamResultDisplaySet> teams = null!;

	[SetUp]
	public void Setup()
	{
		teams = [];

		SetupTeam("team1", points: 50, played: 20, goalDifference: 50);
		SetupTeam("team2", points: 40, played: 16, goalDifference: 50);
		SetupTeam("team3", points: 40, played: 16, goalDifference: 60);
		SetupTeam("_team4", points: 40, played: 16, goalDifference: 60);
		SetupTeam("team5", points: 42, played: 16, goalDifference: 60);
	}

	[Test]
	public void Teams_Are_Setup_Correctly()
	{
		var team1 = teams.First();
		team1.TeamName.Should().Be("team1");
		team1.PointsPerGame.Should().Be(2.5);
		team1.Played.Should().Be(20);
		team1.GoalDifference.Should().Be(50);
	}

	[Test]
	public void TeamsResults_Sort_Correctly()
	{
		var sortedTeams = teams.SortTeams().ToList();
		sortedTeams[0].TeamName.Should().Be("team5");
		sortedTeams[1].TeamName.Should().Be("_team4");
		sortedTeams[2].TeamName.Should().Be("team3");
		sortedTeams[3].TeamName.Should().Be("team2");
		sortedTeams[4].TeamName.Should().Be("team1");
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