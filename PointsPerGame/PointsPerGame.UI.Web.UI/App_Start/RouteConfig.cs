using System.Web.Mvc;
using System.Web.Routing;

namespace PointsPerGame.UI {
	public class RouteConfig {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("Table", "{controller}/{id}",
				new { controller = "Tables", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}