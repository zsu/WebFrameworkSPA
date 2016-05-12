using System.Collections.Generic;
namespace App.Common
{
    public interface IReportConfig
    {
        Dictionary<string, ReportSection> ItemSections { get; }
        ReportSection this[string name] { get; }
    }
}
