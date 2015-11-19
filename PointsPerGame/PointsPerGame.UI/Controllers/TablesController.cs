using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.UI.Controllers
{
    public class TablesController : Controller
    {
        public ActionResult Index(int? id = 0)
        {
            if (id.HasValue == false || id == 0)
            {
                Dictionary<int, string> links = Enum.GetValues(typeof(Leagues))
                                    .Cast<Leagues>()
                                    .ToDictionary(l => (int)l, GetDescription);

                return View("List", links);
            }

            var league = (Leagues)id;

            var scraper = new GuardianScraper();
            var teams = scraper.GetResults(league);
            
            ViewBag.Title = GetDescription(league);

            return View(teams);
        }

        private string GetDescription(Leagues league)
        {
            MemberInfo val = league.GetType().GetMember(league.ToString()).Single();

            return val.GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}