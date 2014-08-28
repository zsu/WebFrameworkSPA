using App.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace Web.Infrastructure
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        //public WebApiExceptionLogger(TraceSource traceSource)
        //{
        //    _traceSource = traceSource;
        //}

        public override void Log(ExceptionLoggerContext context)
        {
            StringBuilder logMsg = new StringBuilder();
            if (Logger.IsLoggingEnabled(LogLevel.Error))   //If there is something wrong in the log4net configuration file,it won't throw exception but all log levels are disabled.
            {
                try
                {
                    //Some exception like file not found happened before the session object is created. 
                    if (HttpContext.Current.Session != null)
                        logMsg.AppendFormat("Session ID: {0}{1}", HttpContext.Current.Session.SessionID, System.Environment.NewLine);
                }
                catch
                {
                }
                try
                {
                    logMsg.AppendFormat("User ID: {0}{1}", HttpContext.Current.User.Identity.Name, System.Environment.NewLine);
                }
                catch
                {
                }
                try
                {
                    logMsg.AppendFormat("An error occurred in applicaton {0}; Method: {1}; Url: {2}.{3}", App.Common.Util.ApplicationConfiguration.AppFullName, 
                        context.Request.Method,context.Request.RequestUri,System.Environment.NewLine);
                }
                catch
                {
                }
                try
                {
                    logMsg.AppendFormat("Request Header: {0}", context.Request.Headers);
                }
                catch
                {
                }
                Logger.Log(LogLevel.Error, logMsg.ToString(), context.Exception);
            }
        }
    }

}