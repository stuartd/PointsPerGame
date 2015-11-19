using NUnit.Framework;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.UnitTests
{
    [TestFixture]
    public class When_Retrieving_Tables_From_Guardian_Website
    {
        private GuardianScraper scraper;

        [SetUp]
        public void Setup()
        {
            scraper = new GuardianScraper();
        }

        [Test]
        public void The_Table_Should_Be_Retrieveable()
        {
            var teams = scraper.GetResults(Leagues.EnglishPremierLeague);

            Assert.AreEqual(20, teams.Count);
        }
    }
}
