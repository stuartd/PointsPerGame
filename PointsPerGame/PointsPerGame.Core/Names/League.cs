using System;
using System.ComponentModel;
using System.Linq;

namespace PointsPerGame.Core.Names {
	public enum League {
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
		SeriaA,

		[Description("All leagues")]
		All,

		[Description("All top divisions")]
		AllTopDivisions,
	}

	public static class LeagueLists {
		public static League[] AllLeagues = Enum.GetValues(typeof(League)).Cast<League>().Except(new[] { League.All, League.AllTopDivisions }).ToArray();
		public static readonly League[] AllTopDivisions = { League.EnglishPremierLeague, League.SPL, League.LaLiga, League.Ligue1, League.Bundesliga, League.SeriaA };
	}
}