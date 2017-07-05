using SEO.RankReport.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEO.RankReport.Business
{
    public abstract class SearchProviderBase
    {
        private SearchParameters searchParameters = null;

        public SearchParameters SearchParameters
        {
            set { searchParameters = value; }
        }

        public SearchProviderBase(SearchParameters parameters = null)
        {
            if (parameters != null)
            {
                searchParameters = parameters;
            }
            else
            {
                // default to google search engine parameters
                searchParameters = new SearchParameters
                {
                    SearchEngineUrl = "https://www.google.com.au/search?q={0}&num={1}",
                    RegExp_Indexes_String = "(<h3 class=\"r\">)(.*?)(</h3>)",
                    RegExp_Hit_Title = "<.*?>",
                    RegExp_Hit_Url = @"http(.*?)(?=&amp)"
                };
            }
        }

        public virtual IList<SearchIndex> PerformSearch(string keyword, string urlPrefix, int? limit = 100)
        {
            // merge the url with "keyword" & "page limit"
            string searchUrl = string.Format(searchParameters.SearchEngineUrl, WebUtility.UrlEncode(keyword), limit);

            // making the http request
            var httpResponse = MakeHttpRequest(searchUrl);

            // parse the results
            IList<SearchIndex> results = new List<SearchIndex>();

            string responseText = string.Empty;

            using (var responseStream = httpResponse.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                responseText = streamReader.ReadToEnd();
            }

            results = ParseResponse(responseText, urlPrefix);
            return results;
        }

        public virtual IList<SearchIndex> ParseResponse(string response, string targetUrl)
        {
            var indexes = new List<SearchIndex>();

            var RegExp_Hits = new Regex(searchParameters.RegExp_Indexes_String, RegexOptions.IgnoreCase);
            var match = RegExp_Hits.Match(response);

            int index = 0;

            while (match.Success && match.Groups.Count == 4)
            {
                index++;
                var uriString = match.Groups[1].Value;

                var decodedUrl = Regex.Match(match.Value, searchParameters.RegExp_Hit_Url, RegexOptions.Singleline).Value;
                var decodedTitle = Regex.Replace(match.Value, searchParameters.RegExp_Hit_Title, string.Empty);

                Uri uri;

                if (Uri.TryCreate(decodedUrl, UriKind.Absolute, out uri))
                {
                    var searchIndex = new SearchIndex
                    {
                        Index = index,
                        Text = decodedTitle,
                        Url = uri.AbsoluteUri
                    };

                    searchIndex.Hit = Regex.IsMatch(searchIndex.Url, string.Format(@"http[s]://{0}(.*?)", targetUrl), RegexOptions.Singleline);

                    indexes.Add(searchIndex);
                }

                match = match.NextMatch();
            }

            return indexes;
        }

        public HttpWebResponse MakeHttpRequest(string url)
        {
            var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            var responseTask = request.GetResponseAsync();
            responseTask.Wait();
            var httpResponse = (HttpWebResponse)responseTask.Result;

            return httpResponse;
        }
    }
}
