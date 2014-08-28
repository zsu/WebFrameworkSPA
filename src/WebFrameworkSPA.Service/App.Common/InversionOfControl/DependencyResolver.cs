using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web.Http.Dependencies;

namespace App.Common.InversionOfControl
{
    public abstract class CustomDependencyResolver:ICustomDependencyResolver
    {
        private bool disposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="CustomDependencyResolver"/> is reclaimed by garbage collection.
        /// </summary>
        [DebuggerStepThrough]
        ~CustomDependencyResolver()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        protected virtual void DisposeCore()
        {
        }

        [DebuggerStepThrough]
        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                disposed = true;
                DisposeCore();
            }

            disposed = true;
        }
        public abstract object Container { get; }
        /// <summary>
        /// Registers the service and its implementation as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterType<TInterface, TClass>();
        /// <summary>
        /// Registers the service and its implementation as singleton.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterType(Type serviceType, Type implementationType);
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterType<TInterface, TClass>(LifetimeType lifetime);
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime);
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime);
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterInstance<TInterface>(object instance);
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterInstance<TInterface>(object instance, LifetimeType lifetime);
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance,LifetimeType lifetime);

        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public abstract void Inject(object instance);

        public abstract IDependencyScope BeginScope();
        #region IServiceResolver Members

        public abstract object GetService(Type serviceType);
        public abstract object GetService(Type serviceType, string key);
        public abstract IEnumerable<object> GetServices(Type serviceType);

        #endregion
}
}
