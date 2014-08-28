using System;
using System.Reflection;
using System.Text;
using App.Common;
using App.Common.Logging;
using System.Runtime.Serialization;
using System.Xml;
using System.Linq;
using App.Common.Attributes;

namespace App.Infrastructure.Castle.Logging {
    public class MethodLogger : IMethodLogger {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly ILogFactory _logFactory;

        public MethodLogger(ILogFactory logFactory, IExceptionLogger exceptionLogger) {
            Check.IsNotNull(exceptionLogger, "exceptionLogger");
            Check.IsNotNull(logFactory, "logFactory");

            _exceptionLogger = exceptionLogger;
            _logFactory = logFactory;
        }

        #region IMethodLogger Members

        public void LogEntry(MethodBase methodBase, object[] argumentValues, LogAttribute logAttribute)
        {
            ILog logger=null;
            if(logAttribute.LogType==null || logAttribute.LogType.Trim()==string.Empty)
                logger = _logFactory.Create(methodBase.DeclaringType.FullName);
            else
                logger = _logFactory.Create(logAttribute.LogType);
            if (ShouldLog(logger, logAttribute.EntryLevel, methodBase)) {
                string message = null;
                try
                {
                    message = CreateInvocationLogString(methodBase, argumentValues, logAttribute);
                }
                catch (Exception exception)
                {
                    message = string.Format("Failed to create invocation information for method: {0}.{1}{2}{3}",methodBase.DeclaringType.FullName,methodBase.Name, Environment.NewLine,exception.Message);
                }
                logger.Log(logAttribute.EntryLevel, message);
            }
        }

        public void LogSuccess(MethodBase methodBase, object returnValue, LogLevel successLevel, string logType)
        {
            ILog logger = null;
            if (logType == null || logType.Trim() == string.Empty)
                logger = _logFactory.Create(methodBase.DeclaringType.FullName);
            else
                logger = _logFactory.Create(logType);
            if (ShouldLog(logger, successLevel, methodBase))
            {
                logger.Log(successLevel,
                           string.Format("{0} Returns:[{1}]", methodBase.Name,
                                         returnValue != null ? returnValue.ToString() : ""));
            }
        }

        public void LogException(MethodBase methodBase, Exception err, object[] argumentValues, LogAttribute logAttribute)
        {
            ILog logger = null;
            if (logAttribute.LogType == null || logAttribute.LogType.Trim() == string.Empty)
                logger = _logFactory.Create(methodBase.DeclaringType.FullName);
            else
                logger = _logFactory.Create(logAttribute.LogType);
            if (ShouldLog(logger, logAttribute.ExceptionLevel, methodBase))
            {
                string message = null;
                try
                {
                    message = CreateInvocationLogString(methodBase, argumentValues, logAttribute);
                }
                catch (Exception exception)
                {
                    message = string.Format("Failed to create invocation information for method: {0}.{1}{2}{3}", methodBase.DeclaringType.FullName, methodBase.Name, Environment.NewLine, exception.Message);
                }
                logger.Log(logAttribute.ExceptionLevel, message, err);
            }
        }

        #endregion

        private bool ShouldLog(ILog logger, LogLevel logLevel, MethodBase methodBase) {
            if (methodBase != null && methodBase.Name != null) {
                return logger.IsLoggingEnabled(logLevel);
            }

            return false;
        }
        private string CreateInvocationLogString(MethodBase methodBase, object[] argumentValues, LogAttribute logAttribute)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.AppendFormat("Called: {0}.{1}(", methodBase.DeclaringType.FullName, methodBase.Name);
            ParameterInfo[] parameters = methodBase.GetParameters();
            string[] settings = null;
            if (logAttribute.SkipArguments != null && logAttribute.SkipArguments.Trim() != string.Empty)
            {
                settings = logAttribute.SkipArguments.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 0; i < argumentValues.Length; i++)
            {
                object argument = argumentValues[i];
                if (settings != null && settings.Contains(parameters[i].Name))
                {
                    sb.Append("[***],");
                    continue;
                }
                String argumentDescription = argument == null ? "null" : DumpObject(argument);
                sb.Append(argumentDescription).Append(",");
            }
            if (argumentValues.Length > 0) sb.Length--;
            sb.Append(")");
            return sb.ToString();
        }

        private string DumpObject(object argument)
        {
            if (argument == null)
                return null;
            Type objtype = argument.GetType();
            if (objtype == typeof(String) || objtype.IsPrimitive || !objtype.IsClass)
                return argument.ToString();
            DataContractSerializer serializer = new DataContractSerializer(objtype);
            StringBuilder serialXml = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(serialXml))
            {
                serializer.WriteObject(writer, argument);
                writer.Flush();
                return serialXml.ToString();
            }
        }

    }
}