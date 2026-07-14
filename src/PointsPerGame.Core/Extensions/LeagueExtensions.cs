using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Extensions;

public static class LeagueExtensions {
	/// <summary>
	/// Is the 'league' a composite of actual leagues,
	/// for example 'All top divisions'
	/// </summary>
	/// <param name="league"></param>
	/// <returns></returns>
	public static bool IsMultiLeague(this League league) => 
		league is League.AllLeagues or League.AllTopDivisions or League.AllEnglishDivisions;
}
