using System.Security.Policy;

namespace PointsPerGame.Core.Models
{
   public class ResultSet
    {
        public string TeamUrl { get; set; }
        public string Crest { get; set; }

        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }

        public int Played => Won + Drawn + Lost;

        public int GoalDifference => GoalsScored - GoalsConceded;

        public int Points => ((Won * 3) + Drawn);
	}
}