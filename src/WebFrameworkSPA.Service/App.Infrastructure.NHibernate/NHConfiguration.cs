using System;
using NHibernate;
using App.Common;
using App.Common.InversionOfControl;
using App.Data;
using App.Common.Data;
using App.Common.Configuration;

namespace App.Infrastructure.NHibernate
{
    /// <summary>
    /// Implementation of <see cref="IDataConfiguration"/> that configures application to use NHibernate.
    /// </summary>
    public class NHConfiguration : IDataConfiguration
    {
        Type _defaultRepositoryType = typeof (NHRepository<,>);
        readonly NHUnitOfWorkFactory _factory = new NHUnitOfWorkFactory();

        /// <summary>
        /// Registers a <see cref="Func{T}"/> of type <see cref="ISessionFactory"/> provider that can be
        /// used to get instances of <see cref="ISessionFactory"/>.
        /// </summary>
        /// <param name="factoryProvider">An instance of <see cref="Func{T}"/> of type <see cref="ISessionFactory"/></param>
        /// <returns><see cref="NHConfiguration"/></returns>
        public NHConfiguration WithSessionFactory(Func<ISessionFactory> factoryProvider)
        {
            Check.Assert<ArgumentNullException>(factoryProvider != null,
                                                 "Expected a non-null Func<ISessionFactory> instance.");
            _factory.RegisterSessionFactoryProvider(factoryProvider);
            return this;
        }

        /// <summary>
        /// Called by application <see cref="Configure"/> to configure data providers.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="IContainerAdapter"/> instance that allows
        /// registering components.</param>
        public void Configure(ICustomDependencyResolver containerAdapter)
        {
            containerAdapter.RegisterInstance<IUnitOfWorkFactory>(_factory);
            containerAdapter.RegisterType(typeof(IRepository<,>), _defaultRepositoryType,LifetimeType.Transient);
        }
    }
}