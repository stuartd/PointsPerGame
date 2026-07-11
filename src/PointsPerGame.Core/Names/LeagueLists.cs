namespace PointsPerGame.Core.Names;
using PointsPerGame.Core.Extensions;

public static class LeagueLists
{
    public static League[] AllLeagues => [.. Enum.GetValues<League>()
        .Where(l => l.IsMultiLeague() == false)
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
