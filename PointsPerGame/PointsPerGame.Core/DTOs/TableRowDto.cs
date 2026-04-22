using PointsPerGame.Core.Models;

namespace PointsPerGame.Core.DTOs;

// DTO used by the Blazor UI
public record TableRowDto(
	string TeamName,
	int Played,
	int Won,
	int Drawn,
	int Lost,
	int GoalsScored,
	int GoalsConceded,
	int GoalDifference,
	int Points,
	string PointsPerGame,
	string TeamUrl,
	string TeamCrest
)
{
	public static TableRowDto FromResultSet(TeamResultDisplaySet teamResults)
	{
		return new(
			teamResults.TeamName,
			teamResults.Played,
			teamResults.Won,
			teamResults.Drawn,
			teamResults.Lost,
			teamResults.GoalsScored,
			teamResults.GoalsConceded,
			teamResults.GoalDifference,
			teamResults.Points,
			teamResults.PointsPerGame.ToString("F2"),
			teamResults.TeamUrl ?? string.Empty,
			teamResults.TeamCrest ?? string.Empty
		);
	}
}