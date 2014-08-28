using System;
namespace App.Common.InversionOfControl
{
    /// <summary>
    /// Represents an interface which is used to register services.
    /// </summary>
    public interface IServiceRegistrar
    {
        /// <summary>
        /// Registers the service and its implementation as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterType<TInterface, TClass>();
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterType<TInterface,TClass>(LifetimeType lifetime);
        /// <summary>
        /// Registers the service and its implementation as singleton.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterType(Type serviceType, Type implementationType);
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime);
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime);

        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterInstance<TInterface>(object instance);
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterInstance<TInterface>(object instance, LifetimeType lifetime);
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance,LifetimeType lifetime);
    }
}