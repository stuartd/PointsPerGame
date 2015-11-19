using NUnit.Framework;
using NUnit.Framework.Internal;
using PointsPerGame.Core.Classes;

namespace PointsPerGame.UnitTests.CoreClassesTests
{
    [TestFixture]
    public class TeamResults_Tests
    {
        private HomeResultSet homeResultSet;
        private AwayResultSet awayResultSet;
        private HomeAndAwayTeamResults homeAndAwayTeamResults;

        [SetUp]
        public void Setup()
        {
            homeResultSet = new HomeResultSet
            {
                // Played is calculated from Drawn / Won / Lost
                Drawn = 2,
                Won = 5,
                Lost = 3,
                GoalsScored = 30,
                GoalsConceded = 20,
            };

            awayResultSet = new AwayResultSet
           {
               // Played is calculated from Drawn / Won / Lost
               Drawn = 0,
               Won = 2,
               Lost = 5,
               GoalsScored = 20,
               GoalsConceded = 50,
           };

            homeAndAwayTeamResults = new HomeAndAwayTeamResults("Team1", "http://www.example.com", homeResultSet, awayResultSet);
        }

        [Test]
        public void TeamResults_Calculates_Correctly()
        {
            Assert.AreEqual(17, homeResultSet.Points);
            Assert.AreEqual(6, awayResultSet.Points);

            Assert.AreEqual("Team1", homeAndAwayTeamResults.Team);
            Assert.AreEqual(7, homeAndAwayTeamResults.Won);
            Assert.AreEqual(2, homeAndAwayTeamResults.Drawn);
            Assert.AreEqual(8, homeAndAwayTeamResults.Lost);
            Assert.AreEqual(17, homeAndAwayTeamResults.Played);
            Assert.AreEqual(23, homeAndAwayTeamResults.Points);

            Assert.AreEqual(50, homeAndAwayTeamResults.GoalsScored);
            Assert.AreEqual(70, homeAndAwayTeamResults.GoalsConceded);
            Assert.AreEqual(-20, homeAndAwayTeamResults.GoalDifference);

            Assert.AreEqual(50 / 17D, homeAndAwayTeamResults.GoalsPerGame);
            Assert.AreEqual(23 / 17D, homeAndAwayTeamResults.PointsPerGame);
        }
    }
}
