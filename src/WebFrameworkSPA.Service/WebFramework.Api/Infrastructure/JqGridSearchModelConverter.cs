using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Web.Infrastructure.JqGrid;

namespace Web.Infrastructure
{
    public class JqGridSearchModelConverter{
        public static JqGridSearchModel Convert(SearchModel from)
        {
            return ConvertFromNgGridWidthFieldFilter(from);
        }
        private static JqGridSearchModel ConvertFromNgGridWidthFieldFilter(SearchModel from)
        {
            JqGridSearchModel to = new JqGridSearchModel { page = from.PageNumber, rows = from.PageSize };
            JqGridFilter jqGridFilter = new JqGridFilter { groupOp = GroupOperations.AND };
            if (from.Filters != null)
            {
                to._search = true;
                jqGridFilter.rules = new List<JqGridRule>();
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(from.Filters);
                foreach (KeyValuePair<string, string> item in result)
                {
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value))
                    {
                        JqGridRule rule = new JqGridRule();
                        rule.field = item.Key;
                        rule.data = item.Value;
                        rule.op = SearchOperations.cn;
                        jqGridFilter.rules.Add(rule);
                    }
                }
                to.filters = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(jqGridFilter);
            }
            if (from.SortField != null)
            {
                StringBuilder sortQuery = new StringBuilder();
                for (int i = 0; i < from.SortField.Count - 1; i++)
                {
                    if (!string.IsNullOrWhiteSpace(from.SortField[i]))
                    {
                        if (!string.IsNullOrWhiteSpace(from.SortDirection[i]))
                            sortQuery.AppendFormat("{0} {1},", from.SortField[i], from.SortDirection[i]);
                        else
                            sortQuery.AppendFormat("{0},", from.SortField[i], from.SortDirection[i]);
                    }
                }
                if (!string.IsNullOrWhiteSpace(from.SortField[from.SortField.Count - 1]))
                {
                    sortQuery.Append(from.SortField[from.SortField.Count - 1]);
                }
                to.sidx = sortQuery.ToString();
                if (from.SortDirection != null && from.SortField.Count == from.SortDirection.Count)
                    to.sord = from.SortDirection[from.SortDirection.Count - 1];
            }
            return to;
        }
        private static JqGridSearchModel ConvertFromNgGrid(SearchModel from)
        {
            JqGridSearchModel to = new JqGridSearchModel { page = from.PageNumber, rows = from.PageSize };
            JqGridFilter jqGridFilter = new JqGridFilter { groupOp = GroupOperations.OR };
            if (from.Filters != null)
            {
                to._search = true;
                jqGridFilter.rules = new List<JqGridRule>();
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(from.Filters);
                foreach (KeyValuePair<string, string> item in result)
                {
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value))
                    {
                        JqGridRule rule = new JqGridRule();
                        rule.field = item.Key;
                        rule.data = item.Value;
                        rule.op = SearchOperations.cn;
                        jqGridFilter.rules.Add(rule);
                    }
                }
                to.filters = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(jqGridFilter);
            }
            if (from.SortField!=null)
            {
                StringBuilder sortQuery = new StringBuilder();
                for(int i=0;i<from.SortField.Count-1;i++)
                {
                    if (!string.IsNullOrWhiteSpace(from.SortField[i]))
                    {
                        if (!string.IsNullOrWhiteSpace(from.SortDirection[i]))
                            sortQuery.AppendFormat("{0} {1},", from.SortField[i], from.SortDirection[i]);
                        else
                            sortQuery.AppendFormat("{0},", from.SortField[i], from.SortDirection[i]);
                    }
                }
                if (!string.IsNullOrWhiteSpace(from.SortField[from.SortField.Count - 1]))
                {
                    sortQuery.Append(from.SortField[from.SortField.Count - 1]);
                }
                to.sidx = sortQuery.ToString();
                if (from.SortDirection != null && from.SortField.Count == from.SortDirection.Count)
                    to.sord = from.SortDirection[from.SortDirection.Count - 1];
            }
            return to;
        }
        private static JqGridSearchModel ConvertFromNgTable(SearchModel from)
        {
            JqGridSearchModel to = new JqGridSearchModel {page = from.PageNumber, rows = from.PageSize };
            JqGridFilter jqGridFilter=new JqGridFilter{groupOp=GroupOperations.OR};
            if (from.Filters != null)
            {
                to._search = true;
                jqGridFilter.rules = new List<JqGridRule>();
                JqGridRule rule = new JqGridRule();
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(from.Filters);
                foreach (KeyValuePair<string, string> item in result)
                {
                    rule.field = item.Key;
                    rule.data = item.Value;
                    rule.op = SearchOperations.cn;
                    jqGridFilter.rules.Add(rule);
                }
                to.filters = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(jqGridFilter);
            }
            if (from.SortField!=null && !string.IsNullOrWhiteSpace(from.SortField[0]))
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(from.SortField[0]);
                foreach (KeyValuePair<string, string> item in result)
                {
                    to.sidx = item.Key;
                    to.sord = item.Value;
                    break;
                }
            }
            return to;
        }
    }
}