using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using PointsPerGame.Core.Extensions;
using PointsPerGame.Core.Utilities;

namespace PointsPerGame.Core.Models
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
            Url = new Url(url);
        }

        public Url Url { get; }

        public string Team { get; }

        public int Won => home.Won + away.Won;

        public int Drawn => home.Drawn + away.Drawn;

        public int Lost => home.Lost + away.Lost;

        public int Played => home.Played + away.Played;

        public int GoalsScored => home.GoalsScored + away.GoalsScored;

        public int GoalsConceded => home.GoalsConceded + away.GoalsConceded;

        public int Points => home.Points + away.Points;

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

        public int GoalDifference => (home.GoalDifference + away.GoalDifference);

        public override string ToString()
        {
            return Team;
        }
    }

    public static class TeamResultsExtensions
    {
	    public static IEnumerable<ITeamResults> SortTeams(this IEnumerable<ITeamResults> values)
	    {
            /*

             Teams are sorted by points per game, then by the number of games played.
			 This means if two teams have the same points per game, then the team which has played the fewer games is sorted higher.
			 If both of those values are equal, teams are sorted by goal difference, and then by team name. 

             */

            return values.OrderByDescending(v => v.PointsPerGame)
	            .ThenBy(v => v.Played)
	            .ThenByDescending(v => v.GoalDifference)
	            .ThenBy(v => v.Team);

	    }
    }
}