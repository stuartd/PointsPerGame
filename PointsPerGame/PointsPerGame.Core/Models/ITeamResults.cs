namespace PointsPerGame.Core.Models {
	public interface ITeamResults {
		string Url { get; }
		string Team { get; }
		int Won { get; }
		int Drawn { get; }
		int Lost { get; }
		int Played { get; }
		int GoalsScored { get; }
		int GoalsConceded { get; }
		int Points { get; }
		double GoalsPerGame { get; }
		string GoalsPerGameDisplay { get; }
		double PointsPerGame { get; }
		int GoalDifference { get; }
		string Crest { get; }
	}
}