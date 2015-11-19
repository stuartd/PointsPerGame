using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PointsPerGame.Core.Classes;
using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Web
{
    // ReSharper disable once UnusedMember.Global
    public class OldHomeAndAwayGuardianScraper : Scraper
    {
        private const string root = "http://www.theguardian.com";

        public List<ITeamResults> GetResults(Leagues league)
        {
            List<ITeamResults> teams = new List<ITeamResults>();

            HtmlDocument doc = new HtmlDocument();

            var stream =
                GetPageStream(GuardianLeagueMappings.GetUriForLeague(league));

            doc.Load(stream, Encoding.UTF8);

            var tables = doc.DocumentNode.SelectNodes("//table[@class='table-football']");

            var rows = tables[0].SelectNodes(".//tr");

            // Pos 	Team 	P 	W 	D 	L 	F 	A 	W 	D 	L 	F 	A 	W 	D 	L 	F 	A 	GD 	Pts
            // 0    1       2   3   4   5   6   7   8   9   10  11  12  13  14  15  16  17  19  20
            foreach (var row in rows)
            {
                HomeResultSet home = new HomeResultSet();
                AwayResultSet away = new AwayResultSet();
                HtmlNodeCollection cells = row.SelectNodes(".//td");

                if (cells == null)
                {
                    continue;
                }

                string team = cells[1].InnerText;
                // Cell has a td which has an a with a href..
                string link = string.Concat(root, cells[1].ChildNodes.Single().Attributes["href"].Value);

                home.Won = int.Parse(cells[3].InnerText);
                home.Drawn = int.Parse(cells[4].InnerText);
                home.Lost = int.Parse(cells[5].InnerText);
                home.GoalsScored = int.Parse(cells[6].InnerText);
                home.GoalsConceded = int.Parse(cells[7].InnerText);

                away.Won = int.Parse(cells[8].InnerText);
                away.Drawn = int.Parse(cells[9].InnerText);
                away.Lost = int.Parse(cells[10].InnerText);
                away.GoalsScored = int.Parse(cells[11].InnerText);
                away.GoalsConceded = int.Parse(cells[12].InnerText);

                teams.Add(new HomeAndAwayTeamResults(team, link, home, away));
            }

            teams.Sort(new TeamResultsComparer());

            return teams;
        }
    }
}
