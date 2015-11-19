using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using PointsPerGame.Core.Extensions;
using PointsPerGame.Core.Utilities;

namespace PointsPerGame.Core.Classes
{
    public class HomeAndAwayTeamResults : ITeamResults
    {
        private readonly ResultSet away;
        private readonly ResultSet home;

        public HomeAndAwayTeamResults(string teamName, string url, HomeResultSet home, AwayResultSet away)
        {
            CheckParameter.RequireNotNullOrEmpty(() => teamName);
            CheckParameter.RequireNotNull(() => home);
            CheckParameter.RequireNotNull(() => away);

            Team = teamName;
            this.home = home;
            this.away = away;
            this.Url = new Url(url);
        }

        public Url Url { get; private set; }

        public string Team { get; private set; }

        public int Won
        {
            get { return home.Won + away.Won; }
        }

        public int Drawn
        {
            get { return home.Drawn + away.Drawn; }
        }

        public int Lost
        {
            get { return home.Lost + away.Lost; }
        }

        public int Played
        {
            get { return home.Played + away.Played; }
        }

        public int GoalsScored
        {
            get { return home.GoalsScored + away.GoalsScored; }
        }

        public int GoalsConceded
        {
            get { return home.GoalsConceded + away.GoalsConceded; }
        }

        public int Points
        {
            get { return home.Points + away.Points; }
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

        public int GoalDifference
        {
            get { return (home.GoalDifference + away.GoalDifference); }
        }

        public override string ToString()
        {
            return Team;
        }
    }

    public class TeamResultsComparer : IComparer<ITeamResults>
    {
        public int Compare(ITeamResults x, ITeamResults y)
        {
            // Less than zero:    This instance is less than y. 
            // Zero:              This instance is equal to y. 
            // Greater than zero: This instance is greater than y. 

            // Order by: points per game (descending), then games played (ascending), 
            // then goal difference (descending), then name (ascending)

            // For descending, compare this to y, for ascending,
            // compare y to this.

            if (x.PointsPerGame.IsNotEqualTo(y.PointsPerGame))
            {
                // desc
                return x.PointsPerGame.CompareTo(y.PointsPerGame);
            }

            if (x.Played != y.Played)
            {
                // asc
                return y.Played.CompareTo(x.Played);
            }

            if (x.GoalDifference != y.GoalDifference)
            {
                // desc
                return y.GoalDifference.CompareTo(x.GoalDifference);
            }

            // asc
            return String.Compare(x.Team, y.Team, StringComparison.Ordinal);
        }
    }
}