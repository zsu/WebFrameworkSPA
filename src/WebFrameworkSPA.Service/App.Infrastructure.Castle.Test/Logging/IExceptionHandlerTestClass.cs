using System.Collections.Generic;
using System.Collections;
namespace App.Infrastructure.Castle.Logging.Test
{
    public interface IExceptionHandlerTestClass {
        void ThrowException();
        float ThrowExceptionSilentWithReturn();
        void ThrowExceptionSilent();
        float ThrowExceptionSilentWithReturnWithLogAttribute();
        float ThrowBaseExceptionNoCatch();
        float ThrowNotImplementedExceptionCatch();
        void SkipPropertyShouldNotLog(string userName, string password, string senssitiveData, List<string> roles, Hashtable htRoles, List<Client> clients);
   }
}