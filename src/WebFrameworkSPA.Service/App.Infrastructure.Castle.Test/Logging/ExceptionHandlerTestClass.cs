using System;
using App.Common.Logging;
using System.Collections.Generic;
using System.Collections;
using App.Common.Attributes;

namespace App.Infrastructure.Castle.Logging.Test
{
    public class ExceptionHandlerTestClass : IExceptionHandlerTestClass {
        #region IExceptionHandlerTestClass Members

        [ExceptionHandler]
        public void ThrowException() {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        public float ThrowExceptionSilentWithReturn() {
            throw new NotImplementedException();
            //return 7f;
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        public void ThrowExceptionSilent() {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        [Log(ExceptionLevel = LogLevel.Error)]
        public float ThrowExceptionSilentWithReturnWithLogAttribute() {
            throw new NotImplementedException();
        }

        [ExceptionHandler(ExceptionType = typeof(NotImplementedException))]
        public float ThrowBaseExceptionNoCatch()
        {
            throw new Exception();
        }

        [ExceptionHandler(IsSilent=true, ExceptionType = typeof(NotImplementedException), ReturnValue = 6f)]
        public float ThrowNotImplementedExceptionCatch()
        {
            throw new NotImplementedException();
        }
        
        [ExceptionHandler(SkipArguments="password,senssitiveData")]
        public void SkipPropertyShouldNotLog(string userName,string password,string senssitiveData,List<string> roles, Hashtable htRoles, List<Client> clients)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class Client
    {
        public int Id
        {get;set;}
        public string Name
        { get; set; }
    }
}