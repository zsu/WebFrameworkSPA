using System;
using App.Common.Logging;
using Castle.Core.Interceptor;

namespace App.Infrastructure.Castle.Logging {
    public class ExceptionLogger : IExceptionLogger {
        private readonly ILogFactory _logFactory;
        public ExceptionLogger(ILogFactory logFactory)
        {
            _logFactory = logFactory;
        }
        #region IExceptionLogger Members

        public void LogException(Exception err, bool isSilent, string logType, string message){
            LogException(err, isSilent, logType, message, LogLevel.Error); 
        }

        public void LogException(Exception err, bool isSilent, string logType, string message, LogLevel exceptionLevel)
        {
            ILog logger = _logFactory.Create(logType);
            if (ShouldLog(logger, exceptionLevel, logType))
            {
                if (isSilent)
                {
                    message = "[SILENT]";
                }
                logger.Log(exceptionLevel, message, err);
            }
        }

        private bool ShouldLog(ILog logger, LogLevel logLevel, string logType)
        {
            if (logType != null && logType.Trim() != string.Empty)
            {
                return logger.IsLoggingEnabled(logLevel);
            }

            return false;
        }
        #endregion
    }
}