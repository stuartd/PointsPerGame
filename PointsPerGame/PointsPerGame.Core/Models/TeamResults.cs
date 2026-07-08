namespace PointsPerGame.Core.Models;

/// <inheritdoc cref="ITeamResults"/>
public class TeamResults : ITeamResults
{
    public string TeamUrl { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public string TeamCrest { get; set; } = string.Empty;
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }
    public int GoalsScored { get; set; }
    public int GoalsConceded { get; set; }
    public int Played { get; set; }
    public int Points { get; set; }
}
