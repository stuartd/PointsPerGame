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

			// Perfect-record teams favour points in the bag, so the team which has played
			// more games is sorted higher. Other PPG ties still favour fewer games played.
			.ThenBy(v => HasMaximumPointsPerGame(v, pointsForWin) ? -(long)v.Played : v.Played)

			// Remaining ties are resolved by goal difference, points, then team name.
			.ThenByDescending(v => v.GoalDifference)
			.ThenByDescending(v => v.Points)
			.ThenBy(v => v.TeamName);

		return [.. sortedValues];
	}

	private static bool HasMaximumPointsPerGame(TeamResults team, int pointsForWin) =>
		team.Played > 0 && team.Points == (long)team.Played * pointsForWin;
}
