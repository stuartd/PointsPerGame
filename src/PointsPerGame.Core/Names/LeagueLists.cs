namespace PointsPerGame.Core.Names;

public static class LeagueLists
{
    public static League[] AllLeagues => [.. Enum.GetValues<League>()
        .Where(l => l is not League.AllTopDivisions 
                        && l is not League.AllEnglishDivisions)
        .ToArray()];

    public static League[] AllTopDivisions => [
        League.EnglishPremierLeague,
        League.WSL,
        League.SPL,
        League.LaLiga,
        League.Ligue1,
        League.Bundesliga,
        League.SerieA,
    ];

    public static League[] AllEnglishDivisions => [
        League.EnglishPremierLeague,
        League.EnglishChampionship,
        League.EnglishLeagueOne,
        League.EnglishLeagueTwo,
    ];
}
