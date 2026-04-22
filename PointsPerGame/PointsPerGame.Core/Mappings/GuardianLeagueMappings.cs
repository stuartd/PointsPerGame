using System;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Mappings {
	public static class GuardianLeagueMappings {
		public static string GetUriForLeague(League league) {
			return league switch {
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
				League.SeriaA => "https://www.theguardian.com/football/serieafootball/table",
				League.AllTopDivisions => null,
				League.All => null,
				_ => throw new NotImplementedException(),
			};
		}
	}
}