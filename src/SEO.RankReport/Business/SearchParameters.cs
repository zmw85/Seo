using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO.RankReport.Business
{
    public class SearchParameters
    {
        /// <summary>
        /// search engine url includes url parameters
        /// {0}: keyword
        /// {1}: number of results per page
        /// </summary>
        public string SearchEngineUrl = "https://www.google.com.au/search?q={0}&num={1}";

        /// <summary>
        /// Regular Expression for matching all indexes
        /// </summary>
        public string RegExp_Indexes_String = "(<h3 class=\"r\">)(.*?)(</h3>)";

        /// <summary>
        /// Regular Expression for matching Index Title
        /// </summary>
        public string RegExp_Hit_Title = "<.*?>";

        /// <summary>
        /// Regular Expression for matching url
        /// </summary>
        public string RegExp_Hit_Url = @"http(.*?)(?=&amp)";
    }
}
