using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO.RankReport.Models
{
    public class SearchIndex
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public bool Hit { get; set; }
    }
}
