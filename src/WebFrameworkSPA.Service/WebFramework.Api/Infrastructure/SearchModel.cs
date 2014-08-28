using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public class SearchModel:PagingOptions
    {
        public string Filters { get; set; }
        public List<string> SortField { get; set; }
        public List<string> SortDirection { get; set; }
    }
}