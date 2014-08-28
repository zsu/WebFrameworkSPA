
using App.Common.Logging;
using App.Common.Attributes;
using System.Collections.Generic;
using System.Collections;
using System;
namespace App.Infrastructure.Castle.Logging.Test
{
    public class LogTestClass : ILogTestClass {
        #region ILogTestClass Members

        [Log]
        public int Method(string name, int val) {
            return val;
        }

        [Log(EntryLevel = LogLevel.Info)]
        public virtual int VirtualMethod(string name, int val) {
            return val;
        }

        public virtual int NotLogged(string name, int val) {
            return val;
        }
        [Log(EntryLevel = LogLevel.Off, SuccessLevel = LogLevel.Off, ExceptionLevel = LogLevel.Off)]
        public void LogAttributeWithOffValues()
        {
        }
        [Log(SkipArguments = "password,senssitiveData")]
        public void SkipPropertyShouldNotLog(string userName, string password, string senssitiveData, List<string> roles, Hashtable htRoles, List<Client> clients)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Log(EntryLevel = LogLevel.Info, SuccessLevel = LogLevel.Info, ExceptionLevel = LogLevel.Error, LogType = "CustomLogType")]
    public class LogTestClass2
    {
        [Log(EntryLevel = LogLevel.Error, SuccessLevel = LogLevel.Error, ExceptionLevel = LogLevel.Error, LogType = "MethodLevelLogType")]
        public string GetMessage(string message)
        {
            return message;
        }

        [Log(EntryLevel = LogLevel.Error, SuccessLevel = LogLevel.Error, ExceptionLevel = LogLevel.Error, LogType = "MethodLevelLogType")]
        public virtual string VirtualMethod(string message)
        {
            return message;
        }

        public virtual string GetMessageWithoutAttributes(string message)
        {
            return message;
        }
        [Log(EntryLevel = LogLevel.Off, SuccessLevel = LogLevel.Off, ExceptionLevel = LogLevel.Off)]
        public virtual void LogAttributeWithOverideOffValues()
        {
        }
    }
}