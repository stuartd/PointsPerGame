using NUnit.Framework;
using PointsPerGame.Core.Models;
using Shouldly;

namespace PointsPerGame.UnitTests;

[TestFixture]
public class TeamResults_Sorting_Tests
{
	private const int PointsForWin = 3;
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
		Whisky.TeamName.ShouldBe("Whisky");
		Whisky.PointsPerGame.ShouldBe(2.5);
		Whisky.Played.ShouldBe(20);
		Whisky.GoalDifference.ShouldBe(50);
	}

	[Test]
	public void TeamsResults_Sort_Correctly()
	{
		var sortedTeams = teams.SortTeams(PointsForWin);
		sortedTeams[0].TeamName.ShouldBe("Charlie");
		sortedTeams[1].TeamName.ShouldBe("Alpha");
		sortedTeams[2].TeamName.ShouldBe("Zulu");
		sortedTeams[3].TeamName.ShouldBe("Bravo");
		sortedTeams[4].TeamName.ShouldBe("Whisky");
	}

	[Test]
	public void Perfect_Record_Teams_Are_Sorted_By_Goal_Difference()
	{
		TeamResultDisplaySet[] perfectTeams =
		[
			CreateTeam("Aston Villa", points: 3, played: 1, goalDifference: 1),
			CreateTeam("Arsenal", points: 6, played: 2, goalDifference: 4),
			CreateTeam("Liverpool", points: 6, played: 2, goalDifference: 3),
			CreateTeam("Leicester", points: 9, played: 3, goalDifference: 8),
			CreateTeam("Everton", points: 9, played: 3, goalDifference: 5),
		];

		var sortedTeams = perfectTeams.SortTeams(PointsForWin);

		sortedTeams.Select(t => t.TeamName).ShouldBe([
			"Leicester",
			"Everton",
			"Arsenal",
			"Liverpool",
			"Aston Villa",
		]);
	}

	[Test]
	public void Perfect_Record_Teams_With_Equal_Goal_Difference_Favour_Points_In_The_Bag()
	{
		TeamResultDisplaySet[] perfectTeams =
		[
			CreateTeam("One game", points: 3, played: 1, goalDifference: 4),
			CreateTeam("Three games", points: 9, played: 3, goalDifference: 4),
			CreateTeam("Two games", points: 6, played: 2, goalDifference: 4),
		];

		var sortedTeams = perfectTeams.SortTeams(PointsForWin);

		sortedTeams.Select(t => t.TeamName).ShouldBe(["Three games", "Two games", "One game"]);
	}

	[Test]
	public void Non_Perfect_PPG_Ties_Still_Favour_Fewer_Games_Played()
	{
		TeamResultDisplaySet[] tiedTeams =
		[
			CreateTeam("Long schedule", points: 6, played: 4, goalDifference: 100),
			CreateTeam("Short schedule", points: 3, played: 2, goalDifference: 0),
		];

		var sortedTeams = tiedTeams.SortTeams(PointsForWin);

		sortedTeams.Select(t => t.TeamName).ShouldBe(["Short schedule", "Long schedule"]);
	}

	[Test]
	public void Maximum_PPG_Uses_The_Configured_Points_For_A_Win()
	{
		TeamResultDisplaySet[] perfectTeams =
		[
			CreateTeam("Two games", points: 4, played: 2, goalDifference: 1),
			CreateTeam("One game", points: 2, played: 1, goalDifference: 5),
		];

		var sortedTeams = perfectTeams.SortTeams(pointsForWin: 2);

		sortedTeams.Select(t => t.TeamName).ShouldBe(["One game", "Two games"]);
	}

	[TestCase(0)]
	[TestCase(-1)]
	public void Points_For_A_Win_Must_Be_Positive(int pointsForWin)
	{
		var exception = Should.Throw<ArgumentOutOfRangeException>(() => teams.SortTeams(pointsForWin));

		exception.ParamName.ShouldBe(nameof(pointsForWin));
	}

	private void SetupTeam(string name, int points, int played, int goalDifference)
	{
		teams.Add(CreateTeam(name, points, played, goalDifference));
	}

	private static TeamResultDisplaySet CreateTeam(string name, int points, int played, int goalDifference)
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

		return new(teamResultSet);
	}
}
