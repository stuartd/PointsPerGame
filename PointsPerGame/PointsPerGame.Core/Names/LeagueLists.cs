using System;
using System.Linq;

namespace PointsPerGame.Core.Names;

public static class LeagueLists
{
	public static League[] AllLeagues => Enum.GetValues(typeof(League))
		.Cast<League>()
		.Except([League.All, League.AllTopDivisions]).ToArray();

	public static readonly League[] AllTopDivisions =
	[
		League.EnglishPremierLeague,
		League.SPL,
		League.LaLiga,
		League.Ligue1,
		League.Bundesliga,
		League.SeriaA,
	];
}