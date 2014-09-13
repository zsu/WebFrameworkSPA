using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public interface IExporter
    {
        /// <summary>
        /// Generate report in memory
        /// </summary>
        /// <param name="reportName">Report key</param>
        /// <param name="exportType">Type of report to generate</param>
        /// <param name="data">Data to export</param>
        /// <param name="includeProperties">Property list to be included</param>
        /// <param name="excludeProperties">Property list to be excluded</param>
        /// <param name="addTimeStamp">indicator to add timestamp</param>
        /// <returns>Report content</returns>
        string Export<T>(string reportName, IList<T> data, List<string> includeProperties, List<string> excludeProperties,bool addTimeStamp);
        /// <summary>
        /// Generate report file
        /// </summary>
        /// <param name="reportName">Report key</param>
        /// <param name="data">Data to export</param>
        /// <param name="outputFilePath">Output file path</param>
        /// <param name="includeProperties">Property list to be included</param>
        /// <param name="excludeProperties">Property list to be excluded</param>
        /// <param name="addTimeStamp">indicator to add timestamp</param>
        /// <returns>Report file path</returns>
        string Export<T>(string reportName, IList<T> data, string outputFilePath, List<string> includeProperties, List<string> excludePropertie, bool addTimeStamp);
        /// <summary>
        /// Generate report in memory
        /// </summary>
        /// <param name="reportName">Report key</param>
        /// <param name="exportType">Type of report to generate</param>
        /// <param name="data">Data to export</param>
        /// <param name="includeProperties">Property list to be included</param>
        /// <param name="excludeProperties">Property list to be excluded</param>
        /// <param name="addTimeStamp">indicator to add timestamp</param>
        /// <returns>Report content</returns>
        string Export(string reportName, DataSet data, List<string> includeProperties, List<string> excludeProperties, bool addTimeStamp);
        /// <summary>
        /// Generate report file
        /// </summary>
        /// <param name="reportName">Report key</param>
        /// <param name="data">Data to export</param>
        /// <param name="outputFilePath">Output file path</param>
        /// <param name="includeProperties">Property list to be included</param>
        /// <param name="excludeProperties">Property list to be excluded</param>
        /// <param name="addTimeStamp">indicator to add timestamp</param>
        /// <returns>Report file path</returns>
        string Export(string reportName, DataSet data, string outputFilePath, List<string> includeProperties, List<string> excludePropertie, bool addTimeStamp);
    }
}