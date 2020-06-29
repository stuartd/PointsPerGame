using System.ComponentModel;

namespace PointsPerGame.Core.Names
{
    public enum League
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
        [Description("Scottish Championship")]
        ScottishChampionship = 6,
        [Description("Scottish League One")]
        ScottishLeagueOne = 7,
        [Description("Scottish League Two")]
        ScottishLeagueTwo = 8,
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
