using System;
using System.Globalization;
using System.Security.Policy;
using PointsPerGame.Core.Utilities;

namespace PointsPerGame.Core.Classes
{
    public class CombinedTeamResult : ITeamResults
    {
        private readonly ResultSet results;

        public CombinedTeamResult(string teamName, string url, CombinedResultSet results)
        {
            CheckParameter.RequireNotNullOrEmpty(() => teamName);
            CheckParameter.RequireNotNull(() => results);

            Team = teamName;
            this.results = results;
            this.Url = new Url(url);
        }

        public Url Url { get; private set; }

        public string Team { get; private set; }

        public int Won
        {
            get { return results.Won; }
        }

        public int Drawn
        {
            get { return results.Drawn; }
        }

        public int Lost
        {
            get { return results.Lost; }
        }

        public int Played
        {
            get { return results.Played; }
        }

        public int GoalsScored
        {
            get { return results.GoalsScored; }
        }

        public int GoalsConceded
        {
            get { return results.GoalsConceded; }
        }

        public int GoalDifference
        {
            get { return results.GoalDifference; }
        }


        public int Points
        {
            get { return results.Points; }
        }

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