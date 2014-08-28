using System.Transactions;
using App.Common.InversionOfControl;

namespace App.Common.Configuration
{
    /// <summary>
    /// Configuration settings for <see cref="UnitOfWorkScope"/> instances in application.
    /// </summary>
    public interface IUnitOfWorkConfiguration
    {
        /// <summary>
        /// Configures <see cref="UnitOfWorkScope"/> settings.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="IContainerAdapter"/> instance.</param>
        void Configure(ICustomDependencyResolver containerAdapter);

        /// <summary>
        /// Sets <see cref="UnitOfWorkScope"/> instances to auto complete when disposed.
        /// </summary>
        IUnitOfWorkConfiguration AutoCompleteScope();

        /// <summary>
        /// Sets the default isolation level used by <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <param name="isolationLevel"></param>
        IUnitOfWorkConfiguration WithDefaultIsolation(IsolationLevel isolationLevel);
    }
}