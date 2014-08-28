using System;
using System.Collections.Generic;
using App.Common.InversionOfControl;
using System.Web.Http.Dependencies;

namespace App.Common.Test
{
    public class DependencyResolverTestDouble : ICustomDependencyResolver
    {
        public object Container
        { get { throw new NotImplementedException(); } }
        #region IServiceResolver Members

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServiceRegistrar Members

        public IServiceRegistrar RegisterType<TInterface,TClass>(LifetimeType lifetime)
        {
            throw new NotImplementedException();
        }
        
        public IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime)
        {
            throw new NotImplementedException();
        }

        public IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance,LifetimeType lifetime)
        {
            throw new NotImplementedException();
        }
        public IServiceRegistrar RegisterType<TInterface, TClass>()
        {
            throw new NotImplementedException();
        }

        public IServiceRegistrar RegisterType(Type serviceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public IServiceRegistrar RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime)
        {
            throw new NotImplementedException();
        }

        public IServiceRegistrar RegisterInstance<TInterface>(object instance)
        {
            throw new NotImplementedException();
        }

        public IServiceRegistrar RegisterInstance<TInterface>(object instance, LifetimeType lifetime)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IServiceInjector Members

        public void Inject(object instance)
        {
            throw new NotImplementedException();
        }

        #endregion
        public IDependencyScope BeginScope()
        {
            throw new NotImplementedException();
        }
        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}