namespace PointsPerGame.Core.Models;

public static class TeamResultsExtensions
{
	public static IReadOnlyList<TeamResults> SortTeams(
		this IEnumerable<TeamResults> values,
		int pointsForWin)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pointsForWin);

		// Teams are sorted by points per game descending (highest is best).
		var sortedValues = values.OrderByDescending(v => v.PointsPerGame)

			// Perfect-record teams are compared by goal difference below. For other PPG ties,
			// the team which has played fewer games is sorted higher.
			.ThenBy(v => HasMaximumPointsPerGame(v, pointsForWin) ? 0 : v.Played)

			// Remaining ties are resolved by goal difference, points in the bag, then team name.
			.ThenByDescending(v => v.GoalDifference)
			.ThenByDescending(v => v.Points)
			.ThenBy(v => v.TeamName);

		return [.. sortedValues];
	}

	private static bool HasMaximumPointsPerGame(TeamResults team, int pointsForWin) =>
		team.Played > 0 && team.Points == (long)team.Played * pointsForWin;
}
