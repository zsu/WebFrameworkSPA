//Author: Zhicheng Su
using System;
using System.Reflection;
using Castle.Core.Interceptor;
using App.Infrastructure.Castle.Logging;
using App.Common;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Castle.DynamicProxy;
using App.Common.Attributes;

namespace App.Infrastructure.Castle.Logging
{
    public class ExceptionHandlerInterceptor : IInterceptor
    {
        private readonly IExceptionLogger exceptionLogger;

        public ExceptionHandlerInterceptor(IExceptionLogger exceptionLogger)
        {
            Check.IsNotNull(exceptionLogger, "exceptionLogger");

            this.exceptionLogger = exceptionLogger;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            //we take the settings from the first attribute we find searching method first
            //If there is at least one attribute, the call gets wrapped with an exception handler
            var assemblyAttributes =
                (ExceptionHandlerAttribute[])
                methodInfo.ReflectedType.Assembly.GetCustomAttributes(typeof(ExceptionHandlerAttribute), false);
            var classAttributes =
                (ExceptionHandlerAttribute[])
                methodInfo.ReflectedType.GetCustomAttributes(typeof(ExceptionHandlerAttribute), false);
            var methodAttributes =
                (ExceptionHandlerAttribute[])methodInfo.GetCustomAttributes(typeof(ExceptionHandlerAttribute), false);

            if (assemblyAttributes.Length == 0 && classAttributes.Length == 0 && methodAttributes.Length == 0)
            {
                invocation.Proceed();
            }
            else
            {
                ExceptionHandlerAttribute exceptionHandlerAttribute =
                    GetExceptionHandlerAttribute(assemblyAttributes, classAttributes, methodAttributes);
                try
                {
                    invocation.Proceed();
                }
                catch (Exception err)
                {
                    string message = null;
                    try
                    {
                        message = CreateInvocationLogString(invocation, exceptionHandlerAttribute);
                    }
                    catch (Exception exception)
                    {
                        message = "Failed to create invocation information for the method that threw the exception" + Environment.NewLine + exception.Message;
                    }
                    exceptionLogger.LogException(err, exceptionHandlerAttribute.IsSilent,
                                                 methodInfo.ReflectedType.FullName, message);
                    if (exceptionHandlerAttribute.IsSilent)
                    {
                        if (exceptionHandlerAttribute.ExceptionType == null ||
                            exceptionHandlerAttribute.ExceptionType == err.GetType())
                        {
                            invocation.ReturnValue = exceptionHandlerAttribute.ReturnValue;
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        #endregion

        private ExceptionHandlerAttribute GetExceptionHandlerAttribute(
            ExceptionHandlerAttribute[] assemblyAttributes, ExceptionHandlerAttribute[] classAttributes,
            ExceptionHandlerAttribute[] methodAttributes)
        {
            if (methodAttributes.Length > 0)
            {
                return methodAttributes[0];
            }
            if (classAttributes.Length > 0)
            {
                return classAttributes[0];
            }
            return assemblyAttributes[0];
        }

        private string CreateInvocationLogString(IInvocation invocation, ExceptionHandlerAttribute exceptionHandlerAttribute)
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
            if (exceptionHandlerAttribute.SkipArguments != null && exceptionHandlerAttribute.SkipArguments.Trim() != string.Empty)
            {
                settings = exceptionHandlerAttribute.SkipArguments.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            try
            {
                using (XmlWriter writer = XmlWriter.Create(serialXml))
                {
                    serializer.WriteObject(writer, argument);
                    writer.Flush();
                    return serialXml.ToString();
                }
            }
            catch (Exception exception)
            {
                serialXml.AppendFormat("Error occurred while creating invocation information: {0}", exception.Message);
                return serialXml.ToString();
            }
            //StringBuilder objString = new StringBuilder("{");
            //DoDumpObject(argument, objString);
            //string temp = objString.ToString().Trim();
            //if (temp.EndsWith(","))
            //    temp.Remove(temp.Length - 1);
            //temp += "}";
            //return temp;
        }

        //private void DoDumpObject(object argument, StringBuilder objString)
        //{
        //    if (objString == null)
        //        objString = new StringBuilder("{");
        //    Type objtype = argument.GetType();
        //    if (objtype == typeof(String) || objtype.IsPrimitive || !objtype.IsClass)
        //        objString.AppendFormat("{0}:{1},", objtype.Name, objtype.ToString());
        //    else
        //    {
        //        PropertyInfo[] properties = objtype.GetProperties();
        //        objString.AppendFormat("{0}:{", objtype.Name);
        //        foreach (PropertyInfo property in properties)
        //        {
        //            DoDumpObject(property.GetValue(argument, null), objString);
        //        }
        //        objString.Append("},");
        //    }
        //}
    }
}