using System;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Mappings
{
    public static class GuardianLeagueMappings
    {
        public static string GetUriForLeague(Leagues league)
        {
            switch (league)
            {
                case Leagues.EnglishPremierLeague:
                    return "http://www.theguardian.com/football/premierleague/table";

                case Leagues.EnglishChampionship:
                    return "http://www.theguardian.com/football/championship/table";

                case Leagues.EnglishLeagueOne:
                    return "http://www.theguardian.com/football/leagueonefootball/table";

                case Leagues.EnglishLeagueTwo:
                    return "http://www.theguardian.com/football/leaguetwofootball/table";

                case Leagues.SPL:
                    return "http://www.theguardian.com/football/scottishpremierleague/table";

                case Leagues.DivisionOne:
                    return "http://www.theguardian.com/football/scottish-division-one/table";

                case Leagues.DivisionTwo:
                    return "http://www.theguardian.com/football/scottish-division-two/table";

                case Leagues.DivisionThree:
                    return "http://www.theguardian.com/football/scottish-division-three/table";

                case Leagues.LaLiga:
                    return "http://www.theguardian.com/football/laligafootball/table";

                case Leagues.Ligue1:
                    return "http://www.theguardian.com/football/ligue1football/table";

                case Leagues.Bundesliga:
                    return "http://www.theguardian.com/football/bundesligafootball/table";

                case Leagues.SeriaA:
                    return "http://www.theguardian.com/football/serieafootball/table";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
