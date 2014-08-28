using System;
using System.Reflection;
using App.Common.Logging;
using App.Common.Attributes;

namespace App.Infrastructure.Castle.Logging {
    public interface IMethodLogger {
        void LogEntry(MethodBase methodBase, object[] argumentValues, LogAttribute logAttribute);
        void LogSuccess(MethodBase methodBase, object returnValue, LogLevel successLevel, string logType);
        void LogException(MethodBase methodBase, Exception err, object[] argumentValues, LogAttribute logAttribute);
    }
}