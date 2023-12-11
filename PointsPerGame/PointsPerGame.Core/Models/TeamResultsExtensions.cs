using System.Collections.Generic;
using System.Linq;

namespace PointsPerGame.Core.Models {
	public static class TeamResultsExtensions {
		public static IEnumerable<ITeamResults> SortTeams(this IEnumerable<ITeamResults> values) {
			/*

			 Teams are sorted by points per game, then by the number of games played.
			 This means if two teams have the same points per game, then the team which has played the fewer games is sorted higher.
			 If both of those values are equal, teams are sorted by goal difference, and then by team name. 

			 */

			return values.OrderByDescending(v => v.PointsPerGame)
				.ThenBy(v => v.Played)
				.ThenByDescending(v => v.GoalDifference)
				.ThenBy(v => v.Team);
		}
	}
}