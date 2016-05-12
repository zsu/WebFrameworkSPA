using System;

namespace Web.Infrastructure.JqGrid
{
    public class JqGridSearchModel
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
        public WhereClause GenerateWhereClause(Type targetSearchType)
        {
            return new WhereClauseGenerator().Generate(_search, filters, targetSearchType);
        }
    }
}
