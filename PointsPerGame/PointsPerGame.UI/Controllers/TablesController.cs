using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.UI.Controllers
{
    public class TablesController : Controller
    {
        public async Task<ActionResult> Index(int? id = 0)
        {
            if (id.HasValue == false || id == 0)
            {
                var links = Enum.GetValues(typeof(League))
                                    .Cast<League>()
                                    .ToDictionary(l => (int)l, GetDescription);

                return View("List", links);
            }

            var league = (League)id;

            var scraper = new GuardianScraper();
            var teams = await scraper.GetResults(league);
            
            ViewBag.Title = GetDescription(league);

            return View(teams);
        }

        private string GetDescription(League league)
        {
            var val = league.GetType().GetMember(league.ToString()).Single();

            return val.GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}