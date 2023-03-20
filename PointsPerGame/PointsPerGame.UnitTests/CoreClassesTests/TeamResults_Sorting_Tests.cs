using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Models;

namespace PointsPerGame.UnitTests.CoreClassesTests
{
    [TestFixture]
    public class TeamResults_Sorting_Tests
    {
        // "Order by: points per game (descending), then games played (ascending), 
        // then goal difference (descending), then name (ascending)"

       private List<ITeamResults> teams;

        [SetUp]
        public void Setup()
        {
            teams = new List<ITeamResults>();

           // So, setup 5 teams: 
            SetupTeam("team1", 2.567, 20, 50);

            // team 2 has the same PPG but lower games played, so is sorted higher of team 1
            SetupTeam("team2", 2.567, 19, 50);

            // team 3 has the same PPG and games played as team 2, but better goal difference, so is sorted higher
            SetupTeam("team3", 2.567, 19, 60);

            // team 4 has the same PPG and games and goal difference but a team name earlier in alphabet, so is sorted higher
            SetupTeam("_team4", 2.567, 19, 60);

            // team 5 has the highest PPG so gets sorted at the top.
            SetupTeam("team5", 2.789, 19, 60);
        }

        [Test]
        public void TeamsResults_Sort_Correctly()
        {
	        var sortedTeams = teams.SortTeams().ToList();
	        sortedTeams[0].Team.Should().Be("team5");
	        sortedTeams[1].Team.Should().Be("_team4");
	        sortedTeams[2].Team.Should().Be("team3");
	        sortedTeams[3].Team.Should().Be("team2");
	        sortedTeams[4].Team.Should().Be("team1");
        }

		private void SetupTeam(string name, double pointsPerGame, int played, int goalDifference)
        {
            teams.Add(new TestTeamResults(name, pointsPerGame, played, goalDifference));
        }

        private class TestTeamResults : ITeamResults
        {
	        public TestTeamResults(string name, double pointsPerGame, int played, int goalDifference)
	        {
		        Team = name;
		        PointsPerGame = pointsPerGame;
		        Played = played;
		        GoalDifference = goalDifference;
	        }

	        public string Url { get; }
	        public string Team { get; }
	        public int Won { get; }
	        public int Drawn { get; }
	        public int Lost { get; }
	        public int Played { get; }
	        public int GoalsScored { get; }
	        public int GoalsConceded { get; }
	        public int Points { get; }
	        public double GoalsPerGame { get; }
	        public string GoalsPerGameDisplay { get; }
	        public double PointsPerGame { get; }
	        public int GoalDifference { get; }
			public string Crest { get; }

			public override string ToString()
	        {
		        return Team;
	        }
        }
    }
}
