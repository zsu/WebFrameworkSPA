using System;
using App.Common.Logging;

namespace App.Infrastructure.Castle.Logging {
    public interface IExceptionLogger {
        void LogException(Exception err, bool isSilent, string logType, string message);
        void LogException(Exception err, bool isSilent, string logType, string message, LogLevel exceptionLevel);
    }
}