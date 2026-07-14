namespace PointsPerGame.Core.Models;

/// <summary>
/// Represents a team's result set, as derived from the data source.
/// </summary>
public sealed record TeamResults
{
	public string TeamUrl { get; init; } = string.Empty;
	public string TeamName { get; init; } = string.Empty;
	public string TeamCrest { get; init; } = string.Empty;
	public int Won { get; init; }
	public int Drawn { get; init; }
	public int Lost { get; init; }
	public int GoalsScored { get; init; }
	public int GoalsConceded { get; init; }
	public int Played { get; init; }
	public int Points { get; init; }

	public int GoalDifference => GoalsScored - GoalsConceded;

	public double PointsPerGame => Played == 0 ? 0 : Points / (double)Played;

	public override string ToString() => TeamName;
}
