using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PointsPerGame.Core.Web
{
    public class Scraper
    {
        protected static Stream GetPageStream(string url)
        {
            var uri = new Uri(url);

            var client = new HttpClient();
            Task<Stream> task = client.GetStreamAsync(uri);
            task.Wait();

            return task.Result;
        }
    }
}