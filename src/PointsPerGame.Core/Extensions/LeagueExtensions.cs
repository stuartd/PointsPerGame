using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Extensions;

public static class LeagueExtensions {
	/// <summary>
	/// Is the 'league' a composite of actual leagues,
	/// for example 'All top divisions'
	/// </summary>
	/// <param name="tableSelection"></param>
	/// <returns></returns>
	public static bool IsMultiLeague(this TableSelection tableSelection) => 
		tableSelection is TableSelection.AllLeagues or TableSelection.AllTopDivisions or TableSelection.AllEnglishDivisions;
}
