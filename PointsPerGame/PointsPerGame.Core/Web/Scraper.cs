using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PointsPerGame.Core.Web
{
    public class Scraper
    {
        protected static async Task<Stream> GetPageStream(string url)
        {
	        var uri = new Uri(url);

            return await new HttpClient().GetStreamAsync(uri);
        }
    }
}