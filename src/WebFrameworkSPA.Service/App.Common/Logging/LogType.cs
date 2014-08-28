using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Common.Logging
{
    /// <summary>
    /// Log types to categorize log information
    /// </summary>
    /// <value>
    /// Application -   General log information;
    /// LoginAudit  -   User login/logout information;
    /// Performance -   System profiling information;
    /// ReportUsage -   Report usage information;
    /// SecurityAudit   -   User/Permission CRUD information
    /// SystemEvents    -   Critical system information
    /// </value>
    public enum LogType
    {
        Application,
        LoginAudit,
        Performance,
        ReportUsage,
        SecurityAudit,
        SystemEvents
    }
}
