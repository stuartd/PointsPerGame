using System.Collections.Generic;
using System.Linq;

namespace PointsPerGame.Core.Models;

public static class TeamResultsExtensions
{
	public static IReadOnlyList<TeamResultDisplaySet> SortTeams(this IEnumerable<TeamResultDisplaySet> values)
	{
		// Teams are sorted by points per game descending (highest is best) 
		var sortedValues = values.OrderByDescending(v => v.PointsPerGame)

			// then by the number of games played, so if two teams have the same points per game
			// the team which has played the fewer games is sorted higher.
			.ThenBy(v => v.Played)

			// If both of those values are equal, teams are sorted by goal difference (descending, so highest first)
			// and then by team name. 
			.ThenByDescending(v => v.GoalDifference)
			.ThenBy(v => v.TeamName);

			return [.. sortedValues];
	}
}