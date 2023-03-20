using System;
using System.Globalization;
using System.Security.Policy;
using PointsPerGame.Core.Utilities;

namespace PointsPerGame.Core.Models
{
    public class CombinedTeamResult : ITeamResults
    {
        private readonly ResultSet results;

        public CombinedTeamResult(string teamName, ResultSet results)
        {
			CheckParameter.RequireNotNullOrEmpty(() => teamName);
            CheckParameter.RequireNotNull(() => results);

            Team = teamName;
            this.results = results;
  }

        public string Team { get; }

        public int Won => results.Won;

        public int Drawn => results.Drawn;

        public int Lost => results.Lost;

        public int Played => results.Played;

        public int GoalsScored => results.GoalsScored;

        public int GoalsConceded => results.GoalsConceded;

        public int GoalDifference => results.GoalDifference;

		public string Crest => results.Crest;

		public string Url => results.TeamUrl;

		public int Points => results.Points;

        public double GoalsPerGame
        {
            get
            {
                if (Played != 0)
                {
                    return GoalsScored / (double)Played;
                }

                return 0;
            }
        }

        public string GoalsPerGameDisplay
        {
            get
            {
                if (Played != 0)
                {
                    // One DP for display.
                    var roundedValueInt = (int)(GoalsPerGame * 10);
                    return (Math.Round((decimal)roundedValueInt, 1) / 10).ToString(CultureInfo.InvariantCulture);
                }

                return "0";
            }
        }

        public double PointsPerGame
        {
            get
            {
                if (Played != 0)
                {
                    return Points / (double)Played;
                }

                return 0;
            }
        }

        public override string ToString()
        {
            return Team;
        }
    }
}