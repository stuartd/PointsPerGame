using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Extensions;

public static class LeagueExtensions {
	public static bool IsMultiLeague(this League league) => 
		league is League.AllLeagues or League.AllTopDivisions or League.AllEnglishDivisions;
}
