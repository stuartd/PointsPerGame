using System;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Mappings {
	public static class GuardianLeagueMappings {
		public static string GetUriForLeague(League league) {
			switch (league) {
				case League.EnglishPremierLeague:
					return "https://www.theguardian.com/football/premierleague/table";

				case League.EnglishChampionship:
					return "https://www.theguardian.com/football/championship/table";

				case League.EnglishLeagueOne:
					return "https://www.theguardian.com/football/leagueonefootball/table";

				case League.EnglishLeagueTwo:
					return "https://www.theguardian.com/football/leaguetwofootball/table";

				case League.WSL:
					return "https://www.theguardian.com/football/womens-super-league/table";

				case League.SPL:
					return "https://www.theguardian.com/football/scottish-premiership/table";

				case League.ScottishChampionship:
					return "https://www.theguardian.com/football/scottish-championship/table";

				case League.ScottishLeagueOne:
					return "https://www.theguardian.com/football/scottish-league-one/table";

				case League.ScottishLeagueTwo:
					return "https://www.theguardian.com/football/scottish-league-two/table";

				case League.LaLiga:
					return "https://www.theguardian.com/football/laligafootball/table";

				case League.Ligue1:
					return "https://www.theguardian.com/football/ligue1football/table";

				case League.Bundesliga:
					return "https://www.theguardian.com/football/bundesligafootball/table";

				case League.SeriaA:
					return "https://www.theguardian.com/football/serieafootball/table";

				case League.AllTopDivisions:
					return null;

				case League.All:
					return null;

				default:
					throw new NotImplementedException();
			}
		}
	}
}