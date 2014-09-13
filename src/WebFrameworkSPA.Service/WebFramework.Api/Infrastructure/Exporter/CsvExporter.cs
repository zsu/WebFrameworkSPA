using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Web.Infrastructure.Extensions;
namespace Web.Infrastructure
{
    public class CsvExporter : IExporter
    {
        public string Export<T>(string reportName, IList<T> data, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            StringBuilder output = null;
            output = ExportList<T>(data, includeProperties, excludeProperties, addTimeStamp);
            return output.ToString();
        }
        public string Export<T>(string reportName, IList<T> data, string outputFilePath, List<string> includeProperties = null, List<string> excludePropertie = null, bool addTimeStamp = true)
        {
            string filePath = outputFilePath;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = Path.Combine(Path.GetTempPath(), string.Format("{0}_{1}_{2}.csv", App.Common.Util.MakeValidFileName(reportName), DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss"), App.Common.Util.GenerateRandomDigitCode(4)));
            }
            ExportList<T>(data, filePath, includeProperties, excludePropertie, addTimeStamp);
            return filePath;
        }
        public string Export(string reportName, DataSet data, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            StringBuilder output = null;
            output = ExportDataSet(data, includeProperties, excludeProperties, addTimeStamp);
            return output.ToString();
        }
        public string Export(string reportName, DataSet data, string outputFilePath, List<string> includeProperties = null, List<string> excludePropertie = null, bool addTimeStamp = true)
        {
            string filePath = outputFilePath;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = Path.Combine(Path.GetTempPath(), string.Format("{0}_{1}_{2}.csv", App.Common.Util.MakeValidFileName(reportName)), DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), App.Common.Util.GenerateRandomDigitCode(4));
            }
            ExportDataSet(data, filePath, includeProperties, excludePropertie, addTimeStamp);
            return filePath;
        }
        private string HandleSpecialChars(string s)
        {
            if (s == null)
                return "";
            return s.Contains(",") ? String.Format("\"{0}\"", s.Replace("\"", "\"\"")) : s;
        }
        private string HandleSpecialChars(string s, Type columnType)
        {
            if (s == null)
                return "";
            if (columnType == typeof(string))
            {
                s = string.Format("\"{0}\"", s.Contains("\"") ? s.Replace("\"", "\"\"") : s);
            }
            else
                s = s.Contains(",") ? String.Format("\"{0}\"", s.Replace("\"", "\"\"")) : s;
            return s;
        }
        private StringBuilder ExportList<T>(IList<T> data, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            //Get property collection and set selected property list
            PropertyInfo[] props = typeof(T).GetProperties();
            List<PropertyInfo> propList = GetSelectedProperties(props, includeProperties, excludeProperties);
            string formatQuoteWithComma="\"{0}\",",formatComma="{0},",formatQuote="\"{0}\"",format="{0}";
            for (int i = 0; i < propList.Count; i++)
            {
                string value=HandleSpecialChars(propList[i].Name);
                bool hasSYLKSymbol=i==0 && value!=null && value.StartsWith("ID");//Handle SYLK file symbol
                if (i < propList.Count - 1)
                    output.AppendFormat(hasSYLKSymbol ? formatQuoteWithComma : formatQuote, value);
                else
                {
                    output.AppendFormat(format, value);
                    output.AppendLine();
                }

            }

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < propList.Count; j++)
                {
                    if (j < propList.Count - 1)
                        output.AppendFormat(formatComma, HandleSpecialChars(propList[j].GetValue(data[i], null) == null ? string.Empty : propList[j].GetValue(data[i], null).ToString(), propList[j].PropertyType));
                    else
                    {
                        output.AppendFormat(format, HandleSpecialChars(propList[j].GetValue(data[i], null) == null ? string.Empty : propList[j].GetValue(data[i], null).ToString(), propList[j].PropertyType));
                        output.AppendLine();
                    }
                }
            }
            if (addTimeStamp)
                output.AppendFormat(Environment.NewLine + "This report was generated on {0}" + Environment.NewLine, HandleSpecialChars(DateTime.UtcNow.ToClientTime()));
            return output;
        }
        private void ExportList<T>(IList<T> data, string filePath, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            //Get property collection and set selected property list
            PropertyInfo[] props = typeof(T).GetProperties();
            List<PropertyInfo> propList = GetSelectedProperties(props, includeProperties, excludeProperties);
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", formatQuote = "\"{0}\"", format = "{0}";

            using (StreamWriter file = new StreamWriter(filePath))
            {
                for (int i = 0; i < propList.Count; i++)
                {
                    string value = HandleSpecialChars(propList[i].Name);
                    bool hasSYLKSymbol = i==0 && value != null && value.StartsWith("ID");//Handle SYLK file symbol
                    if (i < propList.Count - 1)
                        output.AppendFormat(hasSYLKSymbol ? formatQuoteWithComma : formatComma, value);
                    else
                    {
                        output.AppendFormat(format, value);
                        output.AppendLine();
                    }

                }
                file.Write(output.ToString());
                output.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < propList.Count; j++)
                    {
                        if (j < propList.Count - 1)
                            output.AppendFormat(formatComma, HandleSpecialChars(propList[j].GetValue(data[i], null) == null ? string.Empty : propList[j].GetValue(data[i], null).ToString(), propList[j].PropertyType));
                        else
                        {
                            output.AppendFormat(format, HandleSpecialChars(propList[j].GetValue(data[i], null) == null ? string.Empty : propList[j].GetValue(data[i], null).ToString(), propList[j].PropertyType));
                            output.AppendLine();
                            file.Write(output.ToString());
                            output.Clear();
                        }
                    }
                }
                if (addTimeStamp)
                {
                    output.AppendFormat(Environment.NewLine + "This report was generated on {0}" + Environment.NewLine, HandleSpecialChars(DateTime.UtcNow.ToClientTime()));
                    file.Write(output.ToString());
                    output.Clear();
                }
            }
        }
        private List<PropertyInfo> GetSelectedProperties(PropertyInfo[] props, List<string> include, List<string> exclude)
        {
            List<PropertyInfo> propList = new List<PropertyInfo>();
            if (include != null && include.Count > 0) //Do include first
            {
                foreach (var item in props)
                {
                    var propName = include.Where(a => a == item.Name.ToLower()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(propName))
                        propList.Add(item);
                }
            }
            else if (exclude != null && exclude.Count > 0) //Then do exclude
            {
                foreach (var item in props)
                {
                    var propName = exclude.Where(a => a == item.Name.ToLower()).FirstOrDefault();
                    if (string.IsNullOrEmpty(propName))
                        propList.Add(item);
                }
            }
            else //Default
            {
                propList.AddRange(props.ToList());
            }
            return propList;
        }
        private StringBuilder ExportDataSet(DataSet data, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", formatQuote = "\"{0}\"", format = "{0}";
            foreach (DataTable dt in data.Tables)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string value = HandleSpecialChars(dt.Columns[i].ColumnName);
                    bool hasSYLKSymbol = i == 0 && value != null && value.StartsWith("ID");//Handle SYLK file symbol
                    if (i < dt.Columns.Count - 1)
                        output.AppendFormat(hasSYLKSymbol ? formatQuoteWithComma : formatComma, value);
                    else
                    {
                        output.AppendFormat(format, value);
                        output.AppendLine();
                    }

                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (j < dt.Columns.Count - 1)
                            output.AppendFormat(formatComma, HandleSpecialChars(dt.Rows[i][j] == null ? string.Empty : dt.Rows[i][j].ToString()));
                        else
                        {
                            output.AppendFormat(format, HandleSpecialChars(dt.Rows[i][j] == null ? string.Empty : dt.Rows[i][j].ToString()));
                            output.AppendLine();
                        }
                    }
                }
            }
            if (addTimeStamp)
            {
                output.AppendFormat(Environment.NewLine + "This report was generated on {0}" + Environment.NewLine, HandleSpecialChars(DateTime.UtcNow.ToClientTime()));
            }
            return output;
        }
        private void ExportDataSet(DataSet data, string filePath, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", formatQuote = "\"{0}\"", format = "{0}";
            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach (DataTable dt in data.Tables)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string value = HandleSpecialChars(dt.Columns[i].ColumnName);
                        bool hasSYLKSymbol = i == 0 && value != null && value.StartsWith("ID");//Handle SYLK file symbol
                        if (i < dt.Columns.Count - 1)
                            output.AppendFormat(hasSYLKSymbol ? formatQuoteWithComma : formatComma, value);
                        else
                        {
                            output.AppendFormat(format, value);
                            output.AppendLine();
                        }
                    }
                    file.Write(output.ToString());
                    output.Clear();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {

                            if (j < dt.Columns.Count - 1)
                                output.AppendFormat(formatComma, HandleSpecialChars(dt.Rows[i][j] == null ? string.Empty : dt.Rows[i][j].ToString()));
                            else
                            {
                                output.AppendFormat(format, HandleSpecialChars(dt.Rows[i][j] == null ? string.Empty : dt.Rows[i][j].ToString()));
                                output.AppendLine();
                                file.Write(output.ToString());
                                output.Clear();
                            }

                        }
                    }
                }
                if (addTimeStamp)
                {
                    output.AppendFormat(Environment.NewLine + "This report was generated on {0}" + Environment.NewLine, HandleSpecialChars(DateTime.UtcNow.ToClientTime()));
                    file.Write(output.ToString());
                    output.Clear();
                }
            }
        }
    }
}