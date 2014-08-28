using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using App.Common;
using App.Common.InversionOfControl;
using System.Reflection;
using App.Common.Logging;
using System.Web.Http.Dependencies;

namespace App.Infrastructure.Castle
{
    public class WindsorDependencyResolver : CustomDependencyResolver
    {
        #region Fields
        private static readonly Dictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> propertyCache = new Dictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly object propertyCacheLock = new object();
        private IWindsorContainer _container;
        #endregion
        #region Properties
       
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public override object Container
        {
            get { return _container; }
        }
        #endregion Properties
        #region Ctor

        public WindsorDependencyResolver()
            : this(new WindsorContainer())
        {
        }

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            Check.IsNotNull(container, "container");

            _container = container;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Registers the service and its implementation as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterType<TInterface, TClass>()
        {
            return RegisterType(typeof(TInterface).FullName, typeof(TInterface), typeof(TClass), LifetimeType.Singleton);
        }
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterType<TInterface,TClass>(LifetimeType lifetime)
        {
            return RegisterType(typeof(TInterface).FullName, typeof(TInterface), typeof(TClass), lifetime);
        }
        /// <summary>
        /// Registers the service and its implementation as singleton.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterType(Type serviceType, Type implementationType)
        {
            return RegisterType(serviceType.FullName, serviceType, implementationType, LifetimeType.Singleton);
        }
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime)
        {
            return RegisterType(serviceType.FullName, serviceType, implementationType, lifetime);
        }
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime)
        {
            Check.IsNotNull(serviceType, "serviceType");
            Check.IsNotNull(implementationType, "implementationType");

            key = key ?? MakeKey(serviceType, implementationType);

            LifestyleType lifestyle = (lifetime == LifetimeType.PerRequest) ?
                                      LifestyleType.PerWebRequest :
                                      ((lifetime == LifetimeType.Singleton) ?
                                      LifestyleType.Singleton :
                                      LifestyleType.Transient);

            _container.Register(Component.For(serviceType).ImplementedBy(implementationType).Named(key).LifeStyle.Is(lifestyle));

            return this;
        }

        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterInstance<TInterface>(object instance)
        {
            return RegisterInstance(typeof(TInterface).FullName, typeof(TInterface), instance, LifetimeType.Singleton);
        }
        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="TInterface">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterInstance<TInterface>(object instance, LifetimeType lifetime)
        {
            return RegisterInstance(typeof(TInterface).FullName, typeof(TInterface), instance, lifetime);
        }

        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public override IServiceRegistrar RegisterInstance(string key, Type serviceType, object instance, LifetimeType lifetime)
        {
            Check.IsNotNull(serviceType, "serviceType");
            Check.IsNotNull(instance, "instance");

            key = key ?? MakeKey(serviceType, instance.GetType());
            LifestyleType lifestyle = (lifetime == LifetimeType.PerRequest) ?
                          LifestyleType.PerWebRequest :
                          ((lifetime == LifetimeType.Singleton) ?
                          LifestyleType.Singleton :
                          LifestyleType.Transient);
            _container.Register(Component.For(serviceType).Named(key).Instance(instance).LifeStyle.Is(lifestyle));

            return this;
        }

        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public override void Inject(object instance)
        {
            if (instance != null)
            {
                GetProperties(instance.GetType(), _container).ForEach(property => property.SetValue(instance, _container.Resolve(property.PropertyType), null));
            }
        }

        /// <summary>
        /// Implementation of <see cref="IServiceProvider.GetService"/>.
        /// </summary>
        /// <param name="serviceType">The requested service.</param>
        /// <returns>The requested object.</returns>
        public override object GetService(Type serviceType)
        {
            return GetService(serviceType, null);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override object GetService(Type serviceType, string key)
        {
            try
            {
                return DoGetService(serviceType, key);
            }
            catch(Exception exception)
            {
                Logger.Log(LogLevel.Warn, exception);
                // Eat the exception, the ASP.NET MVC Framework expects a null service when the underlying container
                // cannot resolve.
                return null;
            }
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return DoGetServices(serviceType);
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.Warn, exception);
                // Eat the exception, the ASP.NET MVC Framework expects an empty enumerable when the underlying container
                // cannot resolve.
                return Enumerable.Empty<object>();
            }
        }

        public override IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_container);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected virtual object DoGetService(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key) ? _container.Resolve(serviceType) : _container.Resolve(key, serviceType);
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected virtual IEnumerable<object> DoGetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        protected override void DisposeCore()
        {
            _container.Dispose();
        }
        #endregion

        #region Private Methods
        private IEnumerable<PropertyInfo> GetProperties(Type type, IWindsorContainer container)
        {
            RuntimeTypeHandle typeHandle = type.TypeHandle;
            IEnumerable<PropertyInfo> properties;

            if (!propertyCache.TryGetValue(typeHandle, out properties))
            {
                lock (propertyCacheLock)
                {
                    if (!propertyCache.TryGetValue(typeHandle, out properties))
                    {
                        Func<PropertyInfo, bool> filter = property =>
                        {
                            if (property.CanWrite)
                            {
                                MethodInfo setMethod = property.GetSetMethod();

                                if ((setMethod != null) && (setMethod.GetParameters().Length == 1))
                                {
                                    if (container.Kernel.HasComponent(property.PropertyType))
                                    {
                                        return true;
                                    }
                                }
                            }

                            return false;
                        };

                        properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                                         .Where(filter)
                                         .ToList();

                        propertyCache.Add(typeHandle, properties);
                    }
                }
            }

            return properties;
        }

        private string MakeKey(Type serviceType, Type implementationType)
        {
            return serviceType.FullName + "->" + implementationType.FullName;
        }
        #endregion Private Methods
    }
}
