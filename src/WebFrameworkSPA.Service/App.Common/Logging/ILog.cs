using System;
namespace App.Common.Logging
{
    public interface ILog
    {
        void Log(LogLevel logLevel, string message);

        void Log(LogLevel logLevel, Exception exception);

        void Log(LogLevel logLevel, string message, Exception exception);

        bool IsLoggingEnabled(LogLevel logLevel);
    }
}