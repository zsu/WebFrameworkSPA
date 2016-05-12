using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Web.Infrastructure.Extensions;
namespace Web.Infrastructure
{
    public class CsvExporter : IExporter
    {
        public string Export(string reportName, IDataReader data, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            StringBuilder output = null;
            output = DoExport(data, includeProperties, excludeProperties, addTimeStamp);
            return output.ToString();
        }
        public string Export(string reportName, IDataReader data, string outputFilePath, List<string> includeProperties = null, List<string> excludePropertie = null, bool addTimeStamp = true)
        {
            string filePath = outputFilePath;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = Path.Combine(Path.GetTempPath(), string.Format("{0}_{1}_{2}.csv", App.Common.Util.MakeValidFileName(reportName), DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), App.Common.Util.GenerateRandomDigitCode(4)));
            }
            DoExport(data, filePath, includeProperties, excludePropertie, addTimeStamp);
            return filePath;
        }
        public string Export<T>(string reportName, IList<T> data, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            StringBuilder output = null;
            output = DoExport<T>(data, includeProperties, excludeProperties, addTimeStamp);
            return output.ToString();
        }
        public string Export<T>(string reportName, IList<T> data, string outputFilePath, List<string> includeProperties = null, List<string> excludePropertie = null, bool addTimeStamp = true)
        {
            string filePath = outputFilePath;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = Path.Combine(Path.GetTempPath(), string.Format("{0}_{1}_{2}.csv", App.Common.Util.MakeValidFileName(reportName), DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss"), App.Common.Util.GenerateRandomDigitCode(4)));
            }
            DoExport<T>(data, filePath, includeProperties, excludePropertie, addTimeStamp);
            return filePath;
        }
        public string Export(string reportName, DataSet data, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            StringBuilder output = null;
            output = DoExport(data, includeProperties, excludeProperties, addTimeStamp);
            return output.ToString();
        }
        public string Export(string reportName, DataSet data, string outputFilePath, List<string> includeProperties = null, List<string> excludePropertie = null, bool addTimeStamp = true)
        {
            string filePath = outputFilePath;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = Path.Combine(Path.GetTempPath(), string.Format("{0}_{1}_{2}.csv", App.Common.Util.MakeValidFileName(reportName), DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), App.Common.Util.GenerateRandomDigitCode(4)));
            }
            DoExport(data, filePath, includeProperties, excludePropertie, addTimeStamp);
            return filePath;
        }
        private string HandleSpecialChars(string s)
        {
            if (s == null)
                return "";
            return s.Contains(",") ? String.Concat("\"", s.Replace("\"", "\"\""), "\"") : s;
        }
        private string HandleSpecialChars(string s, Type columnType)
        {
            if (s == null)
                return "";
            if (columnType == typeof(string))
            {
                s = String.Concat("\"", s.Replace("\"", "\"\""), "\"");
            }
            else
                s = s.Contains(",") ? String.Concat("\"", s.Replace("\"", "\"\""), "\"") : s;
            return s;
        }
        private StringBuilder DoExport(IDataReader data, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            //Get property collection and set selected property list
            List<int> propList = GetSelectedProperties(data, includeProperties, excludeProperties);
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", format = "{0}";


            foreach (int i in propList)
            {
                string columnName = data.GetName(i);
                string value = HandleSpecialChars(columnName);
                bool hasSYLKSymbol = i == propList[0] && value != null && value.StartsWith("ID");//Handle SYLK file symbol
                if (i < propList.Count - 1)
                    output.AppendFormat(hasSYLKSymbol ? formatQuoteWithComma : formatComma, value);
                else
                {
                    output.AppendFormat(format, value);
                    output.AppendLine();
                }

            }
            while (data.Read())
            {
                for (int i = 0; i < propList.Count; i++)
                {
                    if (!data.IsDBNull(propList[i]))
                    {
                        string columnValue = Convert.ToString(data.GetValue(propList[i]));
                        if (i < propList.Count - 1)
                            output.AppendFormat(formatComma, HandleSpecialChars(columnValue == null ? string.Empty : columnValue, data.GetFieldType(propList[i])));
                        else
                        {
                            output.AppendFormat(format, HandleSpecialChars(columnValue == null ? string.Empty : columnValue, data.GetFieldType(propList[i])));
                            output.AppendLine();
                        }
                    }
                    else
                    {
                        if (i == propList[propList.Count - 1])
                        {
                            output.AppendLine();
                        }
                        else
                            output.Append(",");
                    }
                }
                if (addTimeStamp)
                {
                    output.AppendFormat(Environment.NewLine + "This report was generated on {0}" + Environment.NewLine, HandleSpecialChars(DateTime.UtcNow.ToClientTime()));
                }
            }
            return output;
        }
        private void DoExport(IDataReader data, string filePath, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            //Get property collection and set selected property list
            List<int> propList = GetSelectedProperties(data, includeProperties, excludeProperties);
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", format = "{0}";

            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach (int i in propList)
                {
                    string columnName = data.GetName(i);
                    string value = HandleSpecialChars(columnName);
                    bool hasSYLKSymbol = i == propList[0] && value != null && value.StartsWith("ID");//Handle SYLK file symbol
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
                while (data.Read())
                {
                    for (int i = 0; i < propList.Count; i++)
                    {
                        if (!data.IsDBNull(propList[i]))
                        {
                            string columnValue = Convert.ToString(data.GetValue(propList[i]));
                            if (i < propList.Count - 1)
                                output.AppendFormat(formatComma, HandleSpecialChars(columnValue == null ? string.Empty : columnValue, data.GetFieldType(propList[i])));
                            else
                            {
                                output.AppendFormat(format, HandleSpecialChars(columnValue == null ? string.Empty : columnValue, data.GetFieldType(propList[i])));
                                output.AppendLine();
                                file.Write(output.ToString());
                                output.Clear();
                            }
                        }
                        else
                        {
                            if (i == propList[propList.Count - 1])
                            {
                                output.AppendLine();
                                file.Write(output.ToString());
                                output.Clear();
                            }
                            else
                                output.Append(",");
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
        private StringBuilder DoExport<T>(IList<T> data, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            //Get property collection and set selected property list
            PropertyInfo[] props = typeof(T).GetProperties();
            List<PropertyInfo> propList = GetSelectedProperties(props, includeProperties, excludeProperties);
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", formatQuote = "\"{0}\"", format = "{0}";
            for (int i = 0; i < propList.Count; i++)
            {
                string value = HandleSpecialChars(propList[i].Name);
                bool hasSYLKSymbol = i == 0 && value != null && value.StartsWith("ID");//Handle SYLK file symbol
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
        private void DoExport<T>(IList<T> data, string filePath, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            //Get property collection and set selected property list
            PropertyInfo[] props = typeof(T).GetProperties();
            List<PropertyInfo> propList = GetSelectedProperties(props, includeProperties, excludeProperties);
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", format = "{0}";

            using (StreamWriter file = new StreamWriter(filePath))
            {
                for (int i = 0; i < propList.Count; i++)
                {
                    string value = HandleSpecialChars(propList[i].Name);
                    bool hasSYLKSymbol = i == 0 && value != null && value.StartsWith("ID");//Handle SYLK file symbol
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
                    var propName = include.Where(a => a.ToLower() == item.Name.ToLower()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(propName))
                        propList.Add(item);
                }
            }
            else if (exclude != null && exclude.Count > 0) //Then do exclude
            {
                foreach (var item in props)
                {
                    var propName = exclude.Where(a => a.ToLower() == item.Name.ToLower()).FirstOrDefault();
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
        private List<int> GetSelectedProperties(IDataReader data, List<string> include, List<string> exclude)
        {
            List<int> propList = new List<int>();
            if (include != null && include.Count > 0) //Do include first
            {
                for (int i = 0; i < data.FieldCount; i++)
                {
                    var propName = include.Where(a => a.ToLower() == data.GetName(i).ToLower()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(propName))
                        propList.Add(i);
                }
            }
            else if (exclude != null && exclude.Count > 0) //Then do exclude
            {
                for (int i = 0; i < data.FieldCount; i++)
                {
                    var propName = exclude.Where(a => a.ToLower() == data.GetName(i).ToLower()).FirstOrDefault();
                    if (string.IsNullOrEmpty(propName))
                        propList.Add(i);
                }
            }
            else //Default
            {
                for (int i = 0; i < data.FieldCount; i++)
                {
                    propList.Add(i);
                }
            }
            return propList;
        }
        private StringBuilder DoExport(DataSet data, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", format = "{0}";
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
        private void DoExport(DataSet data, string filePath, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp)
        {
            StringBuilder output = new StringBuilder();
            string formatQuoteWithComma = "\"{0}\",", formatComma = "{0},", format = "{0}";
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