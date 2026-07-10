using PointsPerGame.Core.Names;
using PointsPerGame.Core.Extensions;

namespace PointsPerGame.Core.Mappings;

public static class GuardianLeagueMappings
{
    public static string GetUriForLeague(League league) => league switch
    {
        League.EnglishPremierLeague => "https://www.theguardian.com/football/premierleague/table",
        League.EnglishChampionship => "https://www.theguardian.com/football/championship/table",
        League.EnglishLeagueOne => "https://www.theguardian.com/football/leagueonefootball/table",
        League.EnglishLeagueTwo => "https://www.theguardian.com/football/leaguetwofootball/table",
        League.WSL => "https://www.theguardian.com/football/womens-super-league/table",
        League.SPL => "https://www.theguardian.com/football/scottish-premiership/table",
        League.ScottishChampionship => "https://www.theguardian.com/football/scottish-championship/table",
        League.ScottishLeagueOne => "https://www.theguardian.com/football/scottish-league-one/table",
        League.ScottishLeagueTwo => "https://www.theguardian.com/football/scottish-league-two/table",
        League.LaLiga => "https://www.theguardian.com/football/laligafootball/table",
        League.Ligue1 => "https://www.theguardian.com/football/ligue1football/table",
        League.Bundesliga => "https://www.theguardian.com/football/bundesligafootball/table",
        League.SerieA => "https://www.theguardian.com/football/serieafootball/table",
        _ when league.IsMultiLeague() =>
            throw new ArgumentException($"{league} is an aggregate league and does not have a Guardian table URL.", nameof(league)),
        _ => throw new ArgumentOutOfRangeException(nameof(league), league, "No Guardian table URL is configured for this league."),
    };
}
