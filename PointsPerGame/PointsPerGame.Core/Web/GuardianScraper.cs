using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Web {
	public class GuardianScraper : Scraper {
		private static readonly MemoryCache cache = new MemoryCache(nameof(GuardianScraper));

		public async Task<List<ITeamResults>> GetMultipleLeagueResults(IEnumerable<League> leagues) {
			var allResults = new List<ITeamResults>();

			foreach (var league in leagues) {
				allResults.AddRange(await GetResults(league));
			}

			return allResults.SortTeams().ToList();
		}

		public static async Task<List<ITeamResults>> GetResults(League league) {
#if RELEASE
			if (cache.Contains(league.ToString())) {
				return cache[league.ToString()] as List<ITeamResults>;
			}
#endif
			var teams = new List<ITeamResults>();

			var doc = new HtmlDocument();

			var stream = await GetPageStreamAsync(GuardianLeagueMappings.GetUriForLeague(league));

			doc.Load(stream, Encoding.UTF8);

			var table = doc.DocumentNode.SelectNodes("//table").FirstOrDefault();

			if (table == null) {
				throw new InvalidOperationException("Guardian have changed their site again - can't find table.");
			}

			var body = table.SelectNodes(".//tbody").FirstOrDefault();

			if (body == null) {
				throw new InvalidOperationException("Guardian have changed their site again - can't find table body.");
			}

			var rows = body.SelectNodes(".//tr");

			if (rows == null) {
				throw new InvalidOperationException("Guardian have changed their site again - can't find rows.");
			}

			var foundAnyTeams = false;

			/*

				Row format squashed (May 2025)

<p>1</p>
			   <div>
			   <div><img src="https://sport.guim.co.uk/football/crests/60/9.png" alt="" /></div>
			   <a href="/football/liverpool">Liverpool</a></div>
			   <p>362583833746<strong>83</strong></p>
			   <div><span title="Won 2-1 against West Ham">Won 2-1 against West Ham</span><span title="Won 1-0 against Leicester">Won 1-0 against Leicester</span><span title="Won 5-1 against Spurs">Won 5-1 against Spurs</span><span title="Lost 1-3 to Chelsea">Lost 1-3 to Chelsea</span><span title="Drew 2-2 with Arsenal">Drew 2-2 with Arsenal</span></div>


			 */

			foreach (var row in rows) {
				var results = new ResultSet();
				var cells = row.SelectNodes(".//td");

				if (cells == null) {
					throw new InvalidOperationException("Guardian have changed their site again - can't find cells.");
				}

				/*

				Team name: dcr-aq6qi6

				<a href="/football/liverpool" class= "dcr-aq6qi6">Liverpool</a>

				<td class=\ "dcr-sz4gcj\">1</td>
				   <th scope=\ "row\">
					   <div class=\ "dcr-1bx3yx1\">
						   <div class=\ "dcr-1rdax43\"><img src=\ "https://sport.guim.co.uk/football/crests/60/9.png\" alt=\ "\" class=\ "dcr-swmlxg\"></div><a href=\ "/football/liverpool\" class=\ "dcr-aq6qi6\">Liverpool</a></div>
				   </th>
				   <td>36</td>
				   <td class=\ "dcr-sicfl9\">25</td>
				   <td class=\ "dcr-sicfl9\">8</td>
				   <td class=\ "dcr-sicfl9\">3</td>
				   <td class=\ "dcr-sicfl9\">83</td>
				   <td class=\ "dcr-sicfl9\">37</td>
				   <td>46</td>
				   <td><b class=\ "dcr-or6g55\">83</b></td>
				   <td>
					   <div class=\ "dcr-q129gn\"><span title=\ "Won 2-1 against West Ham\" class=\ "dcr-1x3aqs8\"><span class=\"dcr-kh6f2l\">Won 2-1 against West Ham</span></span><span title=\ "Won 1-0 against Leicester\" class=\ "dcr-1x3aqs8\"><span class=\"dcr-kh6f2l\">Won 1-0 against Leicester</span></span><span title=\ "Won 5-1 against Spurs\" class=\ "dcr-1x3aqs8\"><span class=\"dcr-kh6f2l\">Won 5-1 against Spurs</span></span><span title=\ "Lost 1-3 to Chelsea\" class=\ "dcr-1pmhvnj\"><span class=\"dcr-kh6f2l\">Lost 1-3 to Chelsea</span></spa n><span title=\ "Drew 2-2 with Arsenal\" class=\ "dcr-1utiqk7\"><span class=\"dcr-kh6f2l\">Drew 2-2 with Arsenal</span></span>
					   </div>
				   </td>


			*/

				string team, url = null;
				var anchor = row.SelectSingleNode(".//a[contains(@class, 'dcr-aq6qi6')]");

				if (anchor == null) {
					team ="Grauniad bug (reported 13/5/2025)";
				}
				else {
					team = anchor?.InnerText.Trim(); // "Arsenal"
					url = anchor.GetAttributeValue("href", null);
					if (string.IsNullOrEmpty(url) == false) {
						url = $"https://www.theguardian.com/{url}/fixtures";
					}
				}

				if (string.IsNullOrEmpty(team)) {
					if (foundAnyTeams) {
						Trace.TraceInformation($"Didn't find team name in {row.InnerHtml}");
						continue;
					}

					throw new Exception("Failed to find team names, has The Guardian changed it's site again?");
				}

				var crestSource = row.SelectNodes(".//img")[0];
				string crest = crestSource.GetAttributeValue("src", null);

				results.Played = int.Parse(cells[1].InnerText);
				results.Won = int.Parse(cells[2].InnerText);
				results.Drawn = int.Parse(cells[3].InnerText);
				results.Lost = int.Parse(cells[4].InnerText);
				results.GoalsScored = int.Parse(cells[5].InnerText);
				results.GoalsConceded = int.Parse(cells[6].InnerText);
				results.GoalDifference = int.Parse(cells[7].InnerText);
				results.Points = int.Parse(cells[8].InnerText);

				results.TeamUrl = url;
				results.Crest = crest;

				teams.Add(new CombinedTeamResult(team, results));

				foundAnyTeams = true;
			}

			var result = teams.SortTeams().ToList();
			cache.Add(league.ToString(), result, DateTimeOffset.Now.AddMinutes(5));

			return result;
		}
	}
}