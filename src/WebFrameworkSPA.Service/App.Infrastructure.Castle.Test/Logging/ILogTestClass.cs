using System.Collections.Generic;
using System.Collections;
namespace App.Infrastructure.Castle.Logging.Test
{
    public interface ILogTestClass {
        int Method(string name, int val);
        int VirtualMethod(string name, int val);
        int NotLogged(string name, int val);
        void LogAttributeWithOffValues();
        void SkipPropertyShouldNotLog(string userName, string password, string senssitiveData, List<string> roles, Hashtable htRoles, List<Client> clients);
    }
}