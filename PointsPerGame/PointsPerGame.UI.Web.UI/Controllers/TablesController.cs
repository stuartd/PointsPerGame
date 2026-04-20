using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.UI.Controllers {
	public class TablesController : Controller {
		public async Task<ActionResult> Index(int? id = 0) {
			if (id.HasValue == false || id == 0) {
				var links = Enum.GetValues(typeof(League))
					.Cast<League>()
					.ToDictionary(l => (int)l, GetDescription);

				ViewBag.Title = "Home";

				return View("List", links);
			}

			var league = (League)id;

			var scraper = new GuardianScraper();
			List<ITeamResults> results;
			string source = null;

			try {
				if (league == League.All) {
					results = await scraper.GetMultipleLeagueResults(LeagueLists.AllLeagues);
				}
				else if (league == League.AllTopDivisions) {
					results = await scraper.GetMultipleLeagueResults(LeagueLists.AllTopDivisions);
				}
				else {
					results = await GuardianScraper.GetResults(league);
					source = GuardianLeagueMappings.GetUriForLeague(league);
				}
			}
			catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				// Yes, I know 404 is wrong but not seeing a quick way of getting the error message back otherwise
				// TODO
				return new HttpNotFoundResult($"Error getting data using {nameof(GuardianScraper)}");
			}

			var leagueName = GetDescription(league);

			if (results.Count == 4) {
				// return "league page not found"
				return View("Missing", new MissingTable(leagueName));
			}

			ViewBag.Title = leagueName;
			if (!string.IsNullOrEmpty(source)) {
				ViewBag.Source = $"<a href= \"{source}\">Source data</a>";
			}

			return View(results);
		}

		private string GetDescription(League league) {
			var val = league.GetType().GetMember(league.ToString()).Single();

			return val.GetCustomAttribute<DescriptionAttribute>().Description;
		}
	}
}