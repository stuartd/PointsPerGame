using System.Collections.Generic;
using NUnit.Framework;
using PointsPerGame.Core.Models;
using Rhino.Mocks;

namespace PointsPerGame.UnitTests.CoreClassesTests
{
    [TestFixture]
    public class TeamResults_Sorting_Tests
    {
        // "Order by: points per game (descending), then games played (ascending), 
        // then goal difference (descending), then name (ascending)"

        private ITeamResults team1;
        private ITeamResults team2;
        private ITeamResults team3;
        private ITeamResults team4;
        private ITeamResults team5;

        [SetUp]
        public void Setup()
        {
            team1 = MockRepository.GenerateMock<ITeamResults>();
            team2 = MockRepository.GenerateMock<ITeamResults>();
            team3 = MockRepository.GenerateMock<ITeamResults>();
            team4 = MockRepository.GenerateMock<ITeamResults>();
            team5 = MockRepository.GenerateMock<ITeamResults>();

            // So, setup 5 teams: 
            SetupTeam(team1, 2.567, 20, 50, "team1");

            // team 2 has the same PPG but lower games played, so is sorted ahead of team 1
            SetupTeam(team2, 2.567, 19, 50, "team2");

            // team 3 has the same PPG and games played as team 2, but better goal difference, so is sorted higher
            SetupTeam(team3, 2.567, 19, 60, "team3");

            // team 4 has the same PPG and games and goal difference but a team name earlier in alphabet, so is sorted higher
            SetupTeam(team4, 2.567, 19, 60, "steam4");

            // team 5 has the highest PPG so gets sorted at the top.
            SetupTeam(team5, 2.789, 19, 60, "team5");
        }

        [Test]
        public void TeamsResults_Sort_Correctly()
        {
            var list = new List<ITeamResults> { team1, team2, team3, team4, team5 };

            list.Sort(new TeamResultsComparer());

            Assert.AreEqual("team5", list[0].Team);
            Assert.AreEqual("team4", list[1].Team);
            Assert.AreEqual("team3", list[2].Team);
            Assert.AreEqual("team2", list[3].Team);
            Assert.AreEqual("team1", list[4].Team);
        }


        private void SetupTeam(ITeamResults team, double pointsPerGame, int played, int goalDifference, string name)
        {
            team.Stub(m => m.PointsPerGame).Return(pointsPerGame);
            team.Stub(m => m.Played).Return(played);
            team.Stub(m => m.GoalDifference).Return(goalDifference);
            team.Stub(m => m.Team).Return(name);
        }
    }
}
