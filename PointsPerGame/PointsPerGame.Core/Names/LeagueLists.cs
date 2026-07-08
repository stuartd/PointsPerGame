namespace PointsPerGame.Core.Names;

public static class LeagueLists
{
    public static League[] AllLeagues => Enum.GetValues<League>()
        .Except([League.All, League.AllTopDivisions])
        .ToArray();

    public static readonly League[] AllTopDivisions =
    [
        League.EnglishPremierLeague,
        League.WSL,
        League.SPL,
        League.LaLiga,
        League.Ligue1,
        League.Bundesliga,
        League.SerieA,
    ];
}
