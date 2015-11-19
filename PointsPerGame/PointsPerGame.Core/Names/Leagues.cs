using System.ComponentModel;

namespace PointsPerGame.Core.Names
{
    public enum Leagues
    {
        [Description("Premier League")]
        EnglishPremierLeague = 1,
        [Description("Championship")]
        EnglishChampionship = 2,
        [Description("League One")]
        EnglishLeagueOne = 3,
        [Description("League Two")]
        EnglishLeagueTwo = 4,
        [Description("Scottish Premier League")]
        SPL = 5,
        [Description("Scottish Division One")]
        DivisionOne = 6,
        [Description("Scottish Division Two")]
        DivisionTwo = 7,
        [Description("Scottish Division Three")]
        DivisionThree = 8,
        [Description("La Liga")]
        LaLiga = 9,
        [Description("Ligue 1")]
        Ligue1 = 10,
        [Description("Bundesliga")]
        Bundesliga = 11,
        [Description("Serie A")]
        SeriaA = 12
    }
}
