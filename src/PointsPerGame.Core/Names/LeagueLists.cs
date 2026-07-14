namespace PointsPerGame.Core.Names;
using PointsPerGame.Core.Extensions;

public static class LeagueLists
{
    public static TableSelection[] AllLeagues => [.. Enum.GetValues<TableSelection>()
        .Where(l => l.IsMultiLeague() == false)
        .ToArray()];

    public static TableSelection[] AllTopDivisions => [
        TableSelection.EnglishPremierLeague,
        TableSelection.WSL,
        TableSelection.SPL,
        TableSelection.LaLiga,
        TableSelection.Ligue1,
        TableSelection.Bundesliga,
        TableSelection.SerieA,
    ];

    public static TableSelection[] AllEnglishDivisions => [
        TableSelection.EnglishPremierLeague,
        TableSelection.EnglishChampionship,
        TableSelection.EnglishLeagueOne,
        TableSelection.EnglishLeagueTwo,
    ];
}
