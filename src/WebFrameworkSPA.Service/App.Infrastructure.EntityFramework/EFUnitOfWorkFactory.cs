using System;
using System.Data.Entity.Core.Objects;
using App.Data;
using App.Common;

namespace App.Infrastructure.EntityFramework
{
    /// <summary>
    /// Implements the <see cref="IUnitOfWorkFactory"/> interface to provide an implementation of a factory
    /// that creates <see cref="EFUnitOfWork"/> instances.
    /// </summary>
    public class EFUnitOfWorkFactory : IUnitOfWorkFactory
    {
        EFSessionResolver _resolver = new EFSessionResolver();
        
        /// Registers a <see cref="Func{T}"/> of type <see cref="ObjectContext"/> provider that can be used
        /// to resolve instances of <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="contextProvider">A <see cref="Func{T}"/> of type <see cref="ObjectContext"/>.</param>
        public void RegisterObjectContextProvider(Func<ObjectContext> contextProvider)
        {
            Check.Assert<ArgumentNullException>(contextProvider != null,
                                                 "Invalid object context provider registration. " +
                                                 "Expected a non-null Func<ObjectContext> instance.");
            _resolver.RegisterObjectContextProvider(contextProvider);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <returns>Instances of <see cref="EFUnitOfWork"/>.</returns>
        public IUnitOfWork Create()
        {
            Check.Assert<InvalidOperationException>(
               _resolver.ObjectContextsRegistered != 0,
               "No ObjectContext providers have been registered. You must register ObjectContext providers using " +
               "the RegisterObjectContextProvider method or use NCommon.Configure class to configure NCommon.EntityFramework " +
               "using the EFConfiguration class and register ObjectContext instances using the WithObjectContext method.");
            
            return new EFUnitOfWork(_resolver);
        }
    }
}
