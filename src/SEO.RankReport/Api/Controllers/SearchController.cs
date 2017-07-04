using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Diagnostics;
using SEO.RankReport.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SEO.RankReport.Api.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private const string GOOGLE_URL = "https://www.google.com.au/search?q={0}&num={1}";
        //private static readonly Regex RX_SEARCH_HITS = new Regex(@"<h3 class=""r""><a href=""/.*?\?q=(.*?)"">(.*?)</a>", RegexOptions.IgnoreCase);
        private static readonly Regex RX_SEARCH_HITS = new Regex("(<h3 class=\"r\">)(.*?)(</h3>)", RegexOptions.IgnoreCase);
        
        // GET api/search
        [HttpGet]
        public IEnumerable<SearchHit> Get(
            [FromQuery]string keyword, 
            [FromQuery]string urlPrefix, 
            [FromQuery]int? limit)
        {
            // default value initialization if values are missing
            // didn't implement request validation for the simplicity
            keyword = string.IsNullOrWhiteSpace(keyword) ? "online title search" : keyword.Trim();
            urlPrefix = string.IsNullOrWhiteSpace(urlPrefix) ? "www.infotrack.com" : urlPrefix.Trim();
            limit = limit.GetValueOrDefault(100);

            int start = 0;

            string searchUrl = string.Format(GOOGLE_URL, WebUtility.UrlEncode(keyword), limit);

            //string pageData = new WebClient().DownloadString(searchUrl);
            var request = InstantiateWebRequest(new Uri(searchUrl));
            var responseTask = SendRequestAndRetrieveResponse(request);
            responseTask.Wait();
            var httpResponse = (HttpWebResponse)responseTask.Result;
            var hits = ProcessSearchResult(httpResponse);

            if (hits.Any())
            {
                hits = hits.Where(h => Regex.IsMatch(h.Url, string.Format(@"http[s]://{0}(.*?)", urlPrefix), RegexOptions.Singleline)).ToList();
            }

            return hits;
        }

        private HttpWebRequest InstantiateWebRequest(Uri uri)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            if (request == null)
            {
                throw new InvalidOperationException("Could not instantiate web request.");
            }

            // configure request
            //request.Headers.UserAgent = "FSBlog.GoogleSearch.GoogleClient";
            return request;
        }

        private Task<WebResponse> SendRequestAndRetrieveResponse(WebRequest webRequest)
        {
            var response = webRequest.GetResponseAsync();
            if (response == null)
            {
                throw new InvalidOperationException("Failed to retrieve response.");
            }

            return response;
        }

        private IList<SearchHit> ProcessSearchResult(WebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                var responseText = streamReader.ReadToEnd();
                var results = Parse(responseText);
                return results;
            }
        }

        private IList<SearchHit> Parse(string response)
        {
            var hits = new List<SearchHit>();

            var match = RX_SEARCH_HITS.Match(response);

            int index = 0;

            while (match.Success && match.Groups.Count == 4)
            {
                index++;
                var uriString = match.Groups[1].Value;

                var decodedUrl = Regex.Match(match.Value, @"http(.*?)(?=&amp)", RegexOptions.Singleline).Value;
                var decodedTitle = Regex.Replace(match.Value, "<.*?>", string.Empty);

                Uri uri;

                if (Uri.TryCreate(decodedUrl, UriKind.Absolute, out uri))
                {
                    hits.Add(new SearchHit
                    {
                        Index = index,
                        Text = decodedTitle,
                        Url = uri.AbsoluteUri
                    });
                }

                match = match.NextMatch();
            }

            return hits;
        }
    }
}
