using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public class ExporterManager
    {
        public static string Export<T>(string reportName, ExporterType exportType, IList<T> data, List<string> includeProperties = null, List<string> excludeProperties = null,bool addTimeStamp=true)
        {
            IExporter exporter;
            switch(exportType)
            {
                case ExporterType.CSV:
                    exporter = new CsvExporter();
                    break;
                default:
                    throw new NotImplementedException(exportType.ToString());
            }
            return exporter.Export(reportName,data,includeProperties,excludeProperties,addTimeStamp);
        }
        public static string Export<T>(string reportName, ExporterType exportType, IList<T> data, string outputFilePath, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            IExporter exporter;
            switch (exportType)
            {
                case ExporterType.CSV:
                    exporter = new CsvExporter();
                    break;
                default:
                    throw new NotImplementedException(exportType.ToString());
            }
            return exporter.Export(reportName, data, outputFilePath, includeProperties, excludeProperties, addTimeStamp);
        }
        public static string Export(string reportName, ExporterType exportType, DataSet data, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            IExporter exporter;
            switch (exportType)
            {
                case ExporterType.CSV:
                    exporter = new CsvExporter();
                    break;
                default:
                    throw new NotImplementedException(exportType.ToString());
            }
            return exporter.Export(reportName, data, includeProperties, excludeProperties, addTimeStamp);
        }
        public static string Export(string reportName, ExporterType exportType, DataSet data, string outputFilePath, List<string> includeProperties = null, List<string> excludeProperties = null, bool addTimeStamp = true)
        {
            IExporter exporter;
            switch (exportType)
            {
                case ExporterType.CSV:
                    exporter = new CsvExporter();
                    break;
                default:
                    throw new NotImplementedException(exportType.ToString());
            }
            return exporter.Export(reportName, data, outputFilePath, includeProperties, excludeProperties, addTimeStamp);
        }
    }
}