using System.Diagnostics;
using System;
using System.Runtime.CompilerServices;
using App.Common.InversionOfControl;

namespace App.Common.Logging
{
    public static class Logger
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, string message)
        {
            Check.IsNotEmpty(message, "message");
            ILog logger = GetLog();
            if (IsLoggingEnabled(logger,logLevel))
                logger.Log(logLevel, message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, string format, params object[] args)
        {
            Check.IsNotEmpty(format, "format");
            ILog logger = GetLog();
            if (IsLoggingEnabled(logger, logLevel))
                logger.Log(logLevel, Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, Exception exception)
        {
            Check.IsNotNull(exception, "exception");

            ILog logger = GetLog();
            if (IsLoggingEnabled(logger, logLevel))
                logger.Log(logLevel, exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, string message, Exception exception)
        {
            Check.IsNotNull(exception, "exception");

            ILog logger = GetLog();
            if (IsLoggingEnabled(logger, logLevel))
                logger.Log(logLevel, message, exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, string logType, string message)
        {
            Check.IsNotEmpty(message, "message");

            ILog logger = GetLog(logType);
            if (IsLoggingEnabled(logger, logLevel))
                logger.Log(logLevel, message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, string logType, string format, params object[] args)
        {
            Check.IsNotEmpty(format, "format");

            ILog logger = GetLog(logType);
            if (IsLoggingEnabled(logger, logLevel))
                logger.Log(logLevel, Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, Exception exception, string logType)
        {
            Check.IsNotNull(exception, "exception");

            ILog logger = GetLog(logType);
            if (IsLoggingEnabled(logger, logLevel))
                logger.Log(logLevel, exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Log(LogLevel logLevel, string logType, string message, Exception exception)
        {
            Check.IsNotNull(exception, "exception");

            ILog logger = GetLog(logType);
            if (IsLoggingEnabled(logger, logLevel))
                logger.Log(logLevel, message, exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static bool IsLoggingEnabled(LogLevel logLevel)
        {
            return GetLog().IsLoggingEnabled(logLevel);
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static bool IsLoggingEnabled(LogLevel logLevel, string logType)
        {
            return GetLog(logType).IsLoggingEnabled(logLevel);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ILog GetLog()
        {
            return IoC.GetService<ILogFactory>().Create(null);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ILog GetLog(string loggerName)
        {
            return IoC.GetService<ILogFactory>().Create(loggerName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsLoggingEnabled(ILog logger, LogLevel logLevel)
        {
            if (logger == null)
                return false;
            return logger.IsLoggingEnabled(logLevel);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string Format(string format, params object[] args)
        {
            Check.IsNotEmpty(format, "format");

            return string.Format(format,args);
        }
    }
}