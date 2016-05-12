using System.Collections.Generic;

namespace Web.Infrastructure
{
    public class SearchModel:PagingOptions
    {
        public string Filters { get; set; }
        public List<string> SortField { get; set; }
        public List<string> SortDirection { get; set; }
    }
}