///Author: Zhicheng Su 
///Date: 02/24/2010
using System;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using log4net;
using log4net.Config;
using App.Common;
using App.Common.Logging;
using System.Threading;
namespace App.Infrastructure.Log4net
{


    public class Log4netAdapter : App.Common.Logging.ILog
    {
        //private readonly string _configFilePath;
        private readonly int _frameToSkip = 2;
        private string _loggerName = null;

        /// <summary>
        /// Log4net adapter
        /// </summary>
        /// <remarks>
        /// Logger's name is the name of the class in which the function is called.
        /// </remarks>
        public Log4netAdapter()
        {
            if (log4net.ThreadContext.Properties["SessionId"] == null)
                if (HttpContext.Current == null)
                    log4net.ThreadContext.Properties["SessionId"] = Thread.CurrentThread.ManagedThreadId;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                log4net.ThreadContext.Properties["SessionId"] = HttpContext.Current.Session.SessionID;
        }

        /// <summary>
        /// Log4net adapter
        /// </summary>
        /// <param name="loggerName">Logger's name to overide the default name</param>
        public Log4netAdapter(string loggerName)
            : this()
        {
            if (loggerName != null && loggerName.Trim() != string.Empty)
            {
                _loggerName = loggerName;
            }
        }

        //public Log4netLogger(string configFilePath)
        //{
        //    Check.IsNotEmpty(configFilePath, "configFilePath");
        //    _configFilePath = Util.GetFullPath(configFilePath);
        //    XmlConfigurator.ConfigureAndWatch(new FileInfo(_configFilePath));
        //    if (log4net.ThreadContext.Properties["SessionId"] == null)
        //        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        //            log4net.ThreadContext.Properties["SessionId"] = HttpContext.Current.Session.SessionID;
        //        else
        //            log4net.ThreadContext.Properties["SessionId"] = Thread.CurrentThread.ManagedThreadId;
        //}

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Log(LogLevel logLevel, string message)
        {
            Check.IsNotEmpty(message, "message");

            log4net.ILog logger;
            if (_loggerName != null && _loggerName.Trim() != string.Empty)
                logger = LogManager.GetLogger(_loggerName);
            else
            {
                Type type = new StackFrame(_frameToSkip, false).GetMethod().DeclaringType;
                logger = LogManager.GetLogger(type);
            }
            Log(logger, logLevel, message, null);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Log(LogLevel logLevel, Exception exception)
        {
            Check.IsNotNull(exception, "exception");

            log4net.ILog logger;
            if (_loggerName != null && _loggerName.Trim() != string.Empty)
                logger = LogManager.GetLogger(_loggerName);
            else
            {
                Type type = new StackFrame(_frameToSkip, false).GetMethod().DeclaringType;
                logger = LogManager.GetLogger(type);
            }
            Log(logger, logLevel, null, exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            if ((message == null || message.Trim() == string.Empty) && exception == null)
                return;

            log4net.ILog logger;
            if (_loggerName != null && _loggerName.Trim() != string.Empty)
                logger = LogManager.GetLogger(_loggerName);
            else
            {
                Type type = new StackFrame(_frameToSkip, false).GetMethod().DeclaringType;
                logger = LogManager.GetLogger(type);
            }
            Log(logger, logLevel, message, exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool IsLoggingEnabled(LogLevel logLevel)
        {
            log4net.ILog logger;
            if (_loggerName != null && _loggerName.Trim() != string.Empty)
                logger = LogManager.GetLogger(_loggerName);
            else
            {
                Type type = new StackFrame(_frameToSkip, false).GetMethod().DeclaringType;
                logger = LogManager.GetLogger(type);
            }
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return logger.IsDebugEnabled;
                case LogLevel.Info:
                    return logger.IsInfoEnabled;
                case LogLevel.Warn:
                    return logger.IsWarnEnabled;
                case LogLevel.Error:
                    return logger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return logger.IsFatalEnabled;
                case LogLevel.Off:
                    return false;
                default:
                    throw new Exception(string.Format("Unrecognized log level '{0}'.", logLevel.ToString()));
            }
        }

        private void Log(log4net.ILog logger, LogLevel logLevel, string message, Exception excpetion)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    if (logger.IsDebugEnabled)
                        logger.Debug(message, excpetion);
                    break;
                case LogLevel.Info:
                    if (logger.IsInfoEnabled)
                        logger.Info(message, excpetion);
                    break;
                case LogLevel.Warn:
                    if (logger.IsWarnEnabled)
                        logger.Warn(message, excpetion);
                    break;
                case LogLevel.Error:
                    if (logger.IsErrorEnabled)
                        logger.Error(message, excpetion);
                    break;
                case LogLevel.Fatal:
                    if (logger.IsFatalEnabled)
                        logger.Fatal(message, excpetion);
                    break;
                case LogLevel.Off:
                    break;
                default:
                    throw new Exception(string.Format("Unrecognized log level '{0}'.", logLevel.ToString()));
            }
        }
    }
}