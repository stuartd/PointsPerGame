using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PointsPerGame.Core.Extensions;
using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Web
{
    public class GuardianScraper : Scraper
    {
        private const string root = "http://www.theguardian.com";

        public async Task<List<ITeamResults>> GetResults(League league)
        {
            var teams = new List<ITeamResults>();

            var doc = new HtmlDocument();

            var stream = await GetPageStream(GuardianLeagueMappings.GetUriForLeague(league));

            doc.Load(stream, Encoding.UTF8);

            var table = doc.DocumentNode.SelectNodes("//table").FirstOrDefault();

            if (table == null)
            {
                throw new InvalidOperationException("Guardian have changed their site again - can't find table.");
            }

            var body = table.SelectNodes(".//tbody").FirstOrDefault();

            if (body == null)
            {
                throw new InvalidOperationException("Guardian have changed their site again - can't find table body.");
            }

            var rows = body.SelectNodes(".//tr");

            if (rows == null)
            {
                throw new InvalidOperationException("Guardian have changed their site again - can't find rows.");
            }

            foreach (var row in rows)
            {
                var results = new CombinedResultSet();
                var cells = row.SelectNodes(".//td");

                if (cells == null)
                {
                    throw new InvalidOperationException("Guardian have changed their site again - can't find cells.");
                }

                var team = cells[1].InnerText;
                var link = "Link is broken";

                var spans = cells[1].SelectNodes(".//span");
                var linkCells = spans?.First().SelectNodes(".//a");

                var href = linkCells?.First().Attributes["href"];

                if (href != null)
                {
                    link = string.Concat(root, href.Value, "/fixtures");
                }

                /*

                Pos Team 	    P 	W 	D 	L 	F 	A 	GD 	Pts 	Form
                1 	Arsenal 	1 	1 	0 	0 	2 	1 	1 	3 	

                 <tr class="">
                  <td class="table-column--sub">3</td> <-- TD 0 -->
                  <td class="table-column--main"> <-- TD 1 -->
                    <span class="team-name" data-abbr="ARS">
                      <a href="/football/arsenal" data-link-name="View team" class="team-name__long">
                        Arsenal
                      </a>
                    </span>
                  </td>
                  <td>1</td>  <-- TD 2, played, which is calculated -->
                  <td class="table-column--importance-1">1</td> <-- TD 3 -->
                  <td class="table-column--importance-1">0</td> <-- TD 4 -->
                  <td class="table-column--importance-1">0</td> <-- TD 5 -->
                  <td class="table-column--importance-1">2</td> <-- TD 6 -->
                  <td class="table-column--importance-1">1</td> <-- TD 7 -->
                  <td class="table-column--importance-3">1</td> <-- TD 8 -->
                  <td>
                    <b>3</b>  <-- TD 9 - note b tag -->
                  </td>
                  <td class="table-column--importance-2 football-stat--form team__results--thin"><-- TD 10 - form - TODO -->
                    <div class="team__results">
                      <span class="team-result team-result--won" data-foe="C Palace" data-score="2" data-score-foe="1" title="Won 2-1 against C Palace">
                        <span class="u-h">Won against C Palace</span>
                      </span>
                    </div>
                  </td>
                </tr>
            */
                // ReSharper disable UnusedVariable
                var played = int.Parse(cells[2].InnerText);
                var gd = int.Parse(cells[8].InnerText);
                var points = int.Parse(cells[9].InnerText);
                // ReSharper restore UnusedVariable

                results.Won = int.Parse(cells[3].InnerText);
                results.Drawn = int.Parse(cells[4].InnerText);
                results.Lost = int.Parse(cells[5].InnerText);
                results.GoalsScored = int.Parse(cells[6].InnerText);
                results.GoalsConceded = int.Parse(cells[7].InnerText);

                teams.Add(new CombinedTeamResult(team, link, results));
            }

            return teams.SortTeams().ToList();
        }
    }
}

