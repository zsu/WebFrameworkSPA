using App.Common.SessionMessage;
using System;
using System.Configuration;
namespace App.Common.SessionMessage
{


    public class SessionMessageFactory : ISessionMessageFactory
    {
        private readonly Type _type;

        public SessionMessageFactory(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                typeName = "App.Common.SessionMessage.CookieSessionMessageProvider,App.Common";

            _type = Type.GetType(typeName, true, true);
        }

        public SessionMessageFactory()
            : this(ConfigurationManager.AppSettings["sessionMessageFactoryTypeName"])
        {
        }

        public ISessionMessageProvider CreateInstance()
        {
            return Activator.CreateInstance(_type) as ISessionMessageProvider;
        }
    }
}