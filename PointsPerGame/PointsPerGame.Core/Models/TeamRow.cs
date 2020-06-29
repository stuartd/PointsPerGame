using System;
using PointsPerGame.Core.Extensions;

namespace PointsPerGame.Core.Classes
{
    public class TeamRow : IComparable<TeamRow>
    {
        public string Team { get; set; }

        private readonly ResultSet home;
        private readonly ResultSet away;

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

        public double GoalsPerGameDisplay
        {
            get
            {
                if (Played != 0)
                {
                    return Math.Round(GoalsPerGame * 10, 1) / 10;
                }

                return 0;
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

        public double PointsPerGameDisplay
        {
            get
            {
                if (Played != 0)
                {
                    return Math.Round(PointsPerGame * 10, 1) / 10;
                }

                return 0;
            }
        }

        public int GoalDifference
        {
            get
            {
                return (home.GoalDifference + away.GoalDifference);
            }
        }

        public TeamRow(string teamName, ResultSet home, ResultSet away)
        {
            Team = teamName;
            this.home = home;
            this.away = away;
        }

        public int CompareTo(TeamRow other)
        {
            // Less than zero:    This instance is less than other. 
            // Zero:              This instance is equal to other. 
            // Greater than zero: This instance is greater than other. 

            // Order by: points per game, then games played (ascending), 
            // then goal difference descending, then name (ascending)

            if (PointsPerGame.IsNotEqualTo(other.PointsPerGame))
            {
                return PointsPerGame.CompareTo(other.PointsPerGame);
            }

            if (Played != other.Played)
            {
                return Played.CompareTo(other.Played);
            }

            if (GoalDifference != other.GoalDifference)
            {
                return GoalDifference.CompareTo(other.GoalDifference);
            }

            return String.Compare(Team, other.Team, StringComparison.Ordinal);
        }
    }
}