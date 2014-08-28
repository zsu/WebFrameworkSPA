using System;
using App.Data;
using NHibernate;
using App.Common;

namespace App.Infrastructure.NHibernate
{
    /// <summary>
    /// Implements the <see cref="IUnitOfWorkFactory"/> interface to provide an implementation of a factory
    /// that creates <see cref="NHUnitOfWork"/> instances.
    /// </summary>
    public class NHUnitOfWorkFactory : IUnitOfWorkFactory
    {
        NHSessionResolver _sessionResolver = new NHSessionResolver();

        /// <summary>
        /// Registers a <see cref="Func{T}"/> of type <see cref="ISessionFactory"/> provider with the unit of work factory.
        /// </summary>
        /// <param name="factoryProvider">A <see cref="Func{T}"/> of type <see cref="ISessionFactory"/> instance.</param>
        public void RegisterSessionFactoryProvider(Func<ISessionFactory> factoryProvider)
        {
            Check.Assert<ArgumentNullException>(factoryProvider != null,
                                                 "Invalid session factory provider registration. " +
                                                 "Expected a non-null Func<ISessionFactory> instance.");
            _sessionResolver.RegisterSessionFactoryProvider(factoryProvider);
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork Create()
        {
            Check.Assert<InvalidOperationException>(
                _sessionResolver.SessionFactoriesRegistered != 0,
                "No session factory providers have been registered. You must register ISessionFactory providers using " +
                "the RegisterSessionFactoryProvider method or use App.Common.Configure class to configure NHibernate " +
                "using the NHConfiguration class and register ISessionFactory instances using the WithSessionFactory method.");
            return new NHUnitOfWork(_sessionResolver);
        }
    }
}