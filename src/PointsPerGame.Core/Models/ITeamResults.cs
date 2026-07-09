namespace PointsPerGame.Core.Models;

/// <summary>
/// Represents a team's results, as derived from the data source.
/// </summary>
public interface ITeamResults
{
	string TeamUrl { get; }
	string TeamName { get; }
	int Won { get; }
	int Drawn { get; }
	int Lost { get; }
	int Played { get; }
	int GoalsScored { get; }
	int GoalsConceded { get; }
	int Points { get; }
	string TeamCrest { get; }
}