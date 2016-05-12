using System;
using Web.Infrastructure.JqGrid;

namespace Web.Infrastructure
{
    public class NgGridSearchModel
    {
        public string sidx { get; set; }
        public string sord { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public bool _search { get; set; }
        public string searchField { get; set; }
        public string searchOper { get; set; }
        public string searchString { get; set; }
        public string filters { get; set; }

        // ignore this for now... all will become clear
        public virtual WhereClause GenerateWhereClause(Type targetSearchType)
        {
            return new NgWhereClauseGenerator().Generate(_search, filters, targetSearchType);
        }
    }
}
