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
using SEO.RankReport.Business;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SEO.RankReport.Api.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        // GET api/search
        [HttpGet]
        public IEnumerable<SearchIndex> Get(
            [FromQuery]string keyword, 
            [FromQuery]string urlPrefix, 
            [FromQuery]int? limit)
        {
            // default value initialization if values are missing
            // didn't implement request validation for the simplicity
            keyword = string.IsNullOrWhiteSpace(keyword) ? "online title search" : keyword.Trim();
            urlPrefix = (string.IsNullOrWhiteSpace(urlPrefix) ? "www.infotrack.com" : urlPrefix.Trim()).Replace("http://", string.Empty).Replace("https://", string.Empty);

            var googleSearch = new GoogleSearchProvider();
            var indexes = googleSearch.PerformSearch(keyword, urlPrefix, limit);

            if (indexes.Any())
            {
                indexes = indexes.Where(i => i.Hit).ToList();
            }

            return indexes;
        }
    }
}
