using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Models;

namespace PointsPerGame.Tests;

[TestFixture]
public class TeamResults_Sorting_Tests {
	[SetUp]
	public void Setup() {
		teams = [];

		// So, setup 5 teams: 
		SetupTeam("team1", 2.567, 20, 50);

		// team 2 has the same PPG but lower games played, so is sorted higher of team 1
		SetupTeam("team2", 2.567, 19, 50);

		// team 3 has the same PPG and games played as team 2, but better goal difference, so is sorted higher
		SetupTeam("team3", 2.567, 19, 60);

		// team 4 has the same PPG and games and goal difference but a team name earlier in 'alphabet', so is sorted higher
		SetupTeam("steam4", 2.567, 19, 60);

		// team 5 has the highest PPG so gets sorted at the top.
		SetupTeam("team5", 2.789, 19, 60);
	}
	// "Order by: points per game (descending), then games played (ascending), 
	// then goal difference (descending), then name (ascending)"

	private List<TeamResultDisplaySet> teams = null!;

	private void SetupTeam(string name, double pointsPerGame, int played, int goalDifference) {
		const int baseGoals = 200;

		// Calculate:
		// GoalDifference (GD) = > total - conceded
		// PointsPerGame (PPG) = > points / played
		var teamResultSet = new TeamResults {
			TeamName = name,
			GoalsScored = baseGoals,
			GoalsConceded = baseGoals - goalDifference,
			Points = (int)pointsPerGame * played, // ugh rounding
			Played = played,
		};

		teams.Add(new (teamResultSet));
	}

	[Test]
	public void Teams_Are_Setup_Correctly() {
		var team1 = teams.First();
		team1.TeamName.Should().Be("team1");
		team1.PointsPerGame.Should().BeApproximately(2.567, 0.001);
		team1.Played.Should().Be(20);
		team1.GoalDifference.Should().Be(50);
	}

	[Test]
	public void TeamsResults_Sort_Correctly() {
		var sortedTeams = teams.SortTeams().ToList();
		sortedTeams[0].TeamName.Should().Be("team5");
		sortedTeams[1].TeamName.Should().Be("_team4");
		sortedTeams[2].TeamName.Should().Be("team3");
		sortedTeams[3].TeamName.Should().Be("team2");
		sortedTeams[4].TeamName.Should().Be("team1");
	}
}