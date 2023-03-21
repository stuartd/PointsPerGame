using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;

namespace PointsPerGame.Core.Web {
	public class Scraper {

		static Scraper() {
			httpClient = new HttpClient();
		}

		protected static async Task<Stream> GetPageStreamAsync(string url) {

			var uri = new Uri(url);
			ServicePointManager.FindServicePoint(uri);

			var response = await httpClient.GetAsync(new Uri(url));

			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStreamAsync();
		}

		private static readonly HttpClient httpClient;
	}
}