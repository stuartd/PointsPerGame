using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PointsPerGame.Core.Web {
	public class Scraper {

		static Scraper() {
			httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:128.0) Gecko/20100101 Firefox/128.0");
		}

		protected static async Task<Stream> GetPageStreamAsync(string url) {

			var uri = new Uri(url);
			ServicePointManager.FindServicePoint(uri);

			var response = await httpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStreamAsync();
		}

		// Yes, I know. Can use this still as not expecting DNS changes
		private static readonly HttpClient httpClient;
	}
}