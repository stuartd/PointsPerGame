namespace PointsPerGame.Core.Models;

/// <summary>
/// Projects the underlying <see cref="ITeamResults">team result data</see>
/// into the display format
/// </summary>
public class TeamResultDisplaySet : ITeamResults
{
	private readonly ITeamResults teamResults;

	public TeamResultDisplaySet(ITeamResults teamResults)
	{
		this.teamResults = teamResults ?? throw new($"{nameof(teamResults)} must not be null");
	}

	public string TeamName => teamResults.TeamName;

	public int Won => teamResults.Won;

	public int Drawn => teamResults.Drawn;

	public int Lost => teamResults.Lost;

	public int Played => teamResults.Played;

	public int GoalsScored => teamResults.GoalsScored;

	public int GoalsConceded => teamResults.GoalsConceded;

	public int GoalDifference => teamResults.GoalsScored - teamResults.GoalsConceded;

	public string TeamCrest => teamResults.TeamCrest;

	public string TeamUrl => teamResults.TeamUrl;

	public int Points => teamResults.Points;

	// Don't know about this. Might be fun?
	// Give it a try

	//public double GoalsPerGame {
	//	get {
	//		if (Played != 0) {
	//			return GoalsScored / (double)Played;
	//		}

	//		return 0;
	//	}
	//}

	//public string GoalsPerGameDisplay {
	//	get {
	//		if (Played == 0) {
	//			return "0";
	//		}

	//		// One DP for display.
	//		var roundedValueInt = (int)(GoalsPerGame * 10);
	//		return (Math.Round((decimal)roundedValueInt, 1) / 10).ToString(CultureInfo.InvariantCulture);
	//	}
	//}

	public double PointsPerGame
	{
		get
		{
			if (Played == 0)
			{
				return 0;
			}

			return teamResults.Points / (double)Played;
		}
	}

	public override string ToString()
	{
		return TeamName;
	}
}