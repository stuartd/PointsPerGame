using PointsPerGame.Core.Names;
using PointsPerGame.Core.Extensions;

namespace PointsPerGame.Core.Mappings;

public static class GuardianLeagueMappings
{
    public static string GetUriForLeague(TableSelection tableSelection) => tableSelection switch
    {
        TableSelection.EnglishPremierLeague => "https://www.theguardian.com/football/premierleague/table",
        TableSelection.EnglishChampionship => "https://www.theguardian.com/football/championship/table",
        TableSelection.EnglishLeagueOne => "https://www.theguardian.com/football/leagueonefootball/table",
        TableSelection.EnglishLeagueTwo => "https://www.theguardian.com/football/leaguetwofootball/table",
        TableSelection.WSL => "https://www.theguardian.com/football/womens-super-league/table",
        TableSelection.SPL => "https://www.theguardian.com/football/scottish-premiership/table",
        TableSelection.ScottishChampionship => "https://www.theguardian.com/football/scottish-championship/table",
        TableSelection.ScottishLeagueOne => "https://www.theguardian.com/football/scottish-league-one/table",
        TableSelection.ScottishLeagueTwo => "https://www.theguardian.com/football/scottish-league-two/table",
        TableSelection.LaLiga => "https://www.theguardian.com/football/laligafootball/table",
        TableSelection.Ligue1 => "https://www.theguardian.com/football/ligue1football/table",
        TableSelection.Bundesliga => "https://www.theguardian.com/football/bundesligafootball/table",
        TableSelection.SerieA => "https://www.theguardian.com/football/serieafootball/table",
        _ when tableSelection.IsMultiLeague() =>
            throw new ArgumentException($"{tableSelection} is an aggregate league and does not have a Guardian table URL.", nameof(tableSelection)),
        _ => throw new ArgumentOutOfRangeException(nameof(tableSelection), tableSelection, "No Guardian table URL is configured for this league."),
    };
}
