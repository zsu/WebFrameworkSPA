using System;
using System.Data.Entity.Core.Objects;
using App.Common.Configuration;
using App.Common;
using App.Common.Data;
using App.Data;
using App.Common.InversionOfControl;

namespace App.Infrastructure.EntityFramework
{
    /// <summary>
    /// Implementation of <see cref="IDataConfiguration"/> for Entity Framework.
    /// </summary>
    public class EFConfiguration : IDataConfiguration
    {
        readonly EFUnitOfWorkFactory _factory = new EFUnitOfWorkFactory();

        /// <summary>
        /// Configures unit of work instances to use the specified <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="objectContextProvider">A <see cref="Func{T}"/> of type <see cref="ObjectContext"/>
        /// that can be used to construct <see cref="ObjectContext"/> instances.</param>
        /// <returns><see cref="EFConfiguration"/></returns>
        public EFConfiguration WithObjectContext(Func<ObjectContext> objectContextProvider)
        {
            Check.Assert<ArgumentNullException>(objectContextProvider != null,
                                                 "Expected a non-null Func<ObjectContext> instance.");
            _factory.RegisterObjectContextProvider(objectContextProvider);
            return this;
        }

        /// <summary>
        /// Called by NCommon <see cref="Configure"/> to configure data providers.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="IContainerAdapter"/> instance that allows
        /// registering components.</param>
        public void Configure(ICustomDependencyResolver containerAdapter)
        {
            containerAdapter.RegisterInstance<IUnitOfWorkFactory>(_factory);
            containerAdapter.RegisterType(typeof(IRepository<,>), typeof(EFRepository<,>));
        }
    }
}