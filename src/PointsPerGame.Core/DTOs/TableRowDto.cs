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
    public static TableRowDto FromResultSet(TeamResultDisplaySet teamResults) => new(
            TeamName: teamResults.TeamName,
            Played: teamResults.Played,
            Won: teamResults.Won,
            Drawn: teamResults.Drawn,
            Lost: teamResults.Lost,
            GoalsScored: teamResults.GoalsScored,
            GoalsConceded: teamResults.GoalsConceded,
            GoalDifference: teamResults.GoalDifference,
            Points: teamResults.Points,
            PointsPerGame: teamResults.PointsPerGame.ToString("F2"),
            TeamUrl: teamResults.TeamUrl ?? string.Empty,
            TeamCrest: teamResults.TeamCrest ?? string.Empty
        );
}