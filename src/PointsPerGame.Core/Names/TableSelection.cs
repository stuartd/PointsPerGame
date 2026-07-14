using System.ComponentModel;
// League names can be abbreviations or multilingual
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace PointsPerGame.Core.Names;

/// <summary>
/// Represent one or many tables for display
/// </summary>
public enum TableSelection
{
    [Description("Premier League")]
    EnglishPremierLeague = 1,

    [Description("Championship")]
    EnglishChampionship,

    [Description("League One")]
    EnglishLeagueOne,

    [Description("League Two")]
    EnglishLeagueTwo,

    [Description("Women's Super League")]
    WSL,

    [Description("Scottish Premier League")]
    SPL,

    [Description("Scottish Championship")]
    ScottishChampionship,

    [Description("Scottish League One")]
    ScottishLeagueOne,

    [Description("Scottish League Two")]
    ScottishLeagueTwo,

    [Description("La Liga")]
    LaLiga,

    [Description("Ligue 1")]
    Ligue1,

    [Description("Bundesliga")]
    Bundesliga,

    [Description("Serie A")]
    SerieA,
    
    [Description("All leagues")]
    AllLeagues,
    
    [Description("All top divisions")]
    AllTopDivisions,
    
    [Description("All English divisions")]
    AllEnglishDivisions,
}
