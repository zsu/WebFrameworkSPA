//Author: Zhicheng Su
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Text;
using System.Linq;
using Castle.DynamicProxy;
using App.Common;
using App.Common.Attributes;

namespace App.Infrastructure.Castle.Logging
{
    public class LogInterceptor : IInterceptor
    {
        private readonly IMethodLogger methodLogger;

        public LogInterceptor(IMethodLogger methodLogger)
        {
            Check.IsNotNull(methodLogger, "methodLogger");

            this.methodLogger = methodLogger;
        }

        #region IInterceptor Members
        /// <summary>
        /// Intercept method call to add logging information
        /// </summary>
        /// <param name="invocation">Method to be called</param>
        /// <remarks>
        /// Method level settings override Class level settings; Class level settings override Assembly level settings
        /// </remarks>
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            //Method level settings override Class level settings; Class level settings override Assembly level settings
            var assemblyLogAttributes =
                (LogAttribute[])methodInfo.ReflectedType.Assembly.GetCustomAttributes(typeof(LogAttribute), false);
            var classLogAttributes =
                (LogAttribute[])methodInfo.ReflectedType.GetCustomAttributes(typeof(LogAttribute), false);
            var methodLogAttributes =
                (LogAttribute[])methodInfo.GetCustomAttributes(typeof(LogAttribute), false);

            if (assemblyLogAttributes.Length == 0 && classLogAttributes.Length == 0 && methodLogAttributes.Length == 0)
            {
                invocation.Proceed();
            }
            else
            {
                LogAttribute logAttribute = GetLoggingLevels(assemblyLogAttributes, classLogAttributes,
                                                                             methodLogAttributes);
                methodLogger.LogEntry(methodInfo, invocation.Arguments, logAttribute);
                try
                {
                    invocation.Proceed();
                }
                catch (Exception err)
                {
                    methodLogger.LogException(methodInfo, err, invocation.Arguments, logAttribute);
                    throw;
                }
                methodLogger.LogSuccess(methodInfo, invocation.ReturnValue, logAttribute.SuccessLevel, logAttribute.LogType);
            }
        }

        #endregion

        private LogAttribute GetLoggingLevels(LogAttribute[] assemblyLogAttributes,
                                                      LogAttribute[] classLogAttributes,
                                                      LogAttribute[] methodLogAttributes)
        {
            LogAttribute logAttribute = null;
            if (methodLogAttributes.Length > 0)
                logAttribute = GetLoggingLevels(methodLogAttributes);
            else if (classLogAttributes.Length > 0)
                logAttribute = GetLoggingLevels(classLogAttributes);
            if (assemblyLogAttributes.Length > 0)
                logAttribute = GetLoggingLevels(assemblyLogAttributes);

            return logAttribute;
        }

        private LogAttribute GetLoggingLevels(LogAttribute[] logAttributes)
        {
            LogAttribute currentLogAttribute = null;
            if (logAttributes != null && logAttributes.Count() > 0)
                currentLogAttribute = logAttributes[0];

            foreach (LogAttribute logAttribute in logAttributes)
            {
                if (logAttribute.EntryLevel < currentLogAttribute.EntryLevel)
                {
                    currentLogAttribute.EntryLevel = logAttribute.EntryLevel;
                }
                if (logAttribute.SuccessLevel < currentLogAttribute.SuccessLevel)
                {
                    currentLogAttribute.SuccessLevel = logAttribute.SuccessLevel;
                }
                if (logAttribute.ExceptionLevel < currentLogAttribute.ExceptionLevel)
                {
                    currentLogAttribute.ExceptionLevel = logAttribute.ExceptionLevel;
                }
                if (logAttribute.LogType != null && logAttribute.LogType.Trim() != string.Empty)
                {
                    currentLogAttribute.LogType = logAttribute.LogType;
                }
            }

            return currentLogAttribute;
        }
        private string CreateInvocationLogString(IInvocation invocation, LogAttribute logAttribute)
        {
            StringBuilder sb = new StringBuilder(100);
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }
            sb.AppendFormat("Called: {0}.{1}(", invocation.TargetType.Name, methodInfo.Name);
            ParameterInfo[] parameters = methodInfo.GetParameters();
            string[] settings = null;
            if (logAttribute.SkipArguments != null && logAttribute.SkipArguments.Trim() != string.Empty)
            {
                settings = logAttribute.SkipArguments.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                object argument = invocation.Arguments[i];
                if (settings != null && settings.Contains(parameters[i].Name))
                {
                    sb.Append("[***],");
                    continue;
                }
                String argumentDescription = argument == null ? "null" : DumpObject(argument);
                sb.Append(argumentDescription).Append(",");
            }
            if (invocation.Arguments.Length > 0) sb.Length--;
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