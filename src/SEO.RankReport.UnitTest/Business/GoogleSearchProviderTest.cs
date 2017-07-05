using System;
using Xunit;
using SEO.RankReport.Business;

namespace SEO.RankReport.UnitTest
{
    public class GoogleSearchProviderTest
    {
        [Fact]
        public void Func_MakeHttpRequest_Should_Return_Ok()
        {
            string url = "https://www.google.com.au/search?q=online+title+search";
            var google = GoogleSearchProvider.GetInstance();

            var httpResponse = google.MakeHttpRequest(url);
            Assert.True(System.Net.HttpStatusCode.OK == httpResponse.StatusCode, "Generic google search should return STATUS 200 (OK)");
        }

        [Fact]
        public void Func_PerformSearch_Should_Return_Results()
        {
            var google = GoogleSearchProvider.GetInstance();

            var results = google.PerformSearch("Online Title Search", "www.infotrack.com", 10);

            Assert.True(results != null && results.Count > 0, "Google search should return some results");
        }
    }
}
