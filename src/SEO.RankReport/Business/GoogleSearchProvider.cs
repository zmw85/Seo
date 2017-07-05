using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO.RankReport.Business
{
    public class GoogleSearchProvider : SearchProviderBase
    {
        public static GoogleSearchProvider GetInstance(SearchParameters parameters = null)
        {
            instance = instance ?? new GoogleSearchProvider();
            if (parameters != null)
            {
                instance.SearchParameters = parameters;
            }
            return instance;
        }

        private static GoogleSearchProvider instance = null;
    }
}
