using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace App.Common.InversionOfControl
{


    public static class IoC
    {
        private static ICustomDependencyResolver _resolver;

        [DebuggerStepThrough]
        public static void InitializeWith(IDependencyResolverFactory factory)
        {
            Check.IsNotNull(factory, "factory");

            _resolver = factory.CreateInstance();
        }
        public static T GetResolver<T>()
        { return (T)_resolver; }
        public static T GetContainer<T>()
        { return (T)_resolver.Container; }
        public static object GetService(Type serviceType)
        {
            return _resolver.GetService(serviceType);
        }
        public static T GetService<T>()
        {
            return GetService<T>(typeof(T), null);
        }
        public static T GetService<T>(Type serviceType)
        {
            return GetService<T>(serviceType, null);
        }
        public static T GetService<T>(string key)
        {
            return GetService<T>(typeof(T), key);
        }
        public static T GetService<T>(Type serviceType, string key)
        {
            object service;
            if (key == null)
                service = _resolver.GetService(serviceType);
            else
                service = _resolver.GetService(serviceType, key);
            if (service == null)
                return default(T);
            return (T)service;
        }
        public static IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolver.GetServices(serviceType);
        }
        public static IEnumerable<T> GetServices<T>()
        {
            return GetServices<T>(typeof(T));
        }
        public static IEnumerable<T> GetServices<T>(Type serviceType)
        {
            return (IEnumerable<T>)_resolver.GetServices(serviceType);
        }
        public static void Inject(object instance)
        {
            _resolver.Inject(instance);
        }
        public static IServiceRegistrar RegisterType<TInterface, TClass>(LifetimeType lifetime)
        {
            return _resolver.RegisterType<TInterface,TClass>(lifetime);
        }
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public static IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime)
        {
            return _resolver.RegisterType(key, serviceType, implementationType, lifetime);
        }
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IServiceRegistrar RegisterInstance<TInterface>(object instance)
        {
            return _resolver.RegisterInstance<TInterface>(instance);
        }
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IServiceRegistrar RegisterInstance<TInterface>(object instance, LifetimeType lifetime)
        {
            return _resolver.RegisterInstance<TInterface>(instance, lifetime);
        }
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance, LifetimeType lifetime)
        {
            return _resolver.RegisterInstance(key, serviceType, instance,lifetime);
        }

        public static void Reset()
        {
            _resolver.Dispose();
        }
    }
}