using System.Transactions;
using App.Data;
using App.Common.InversionOfControl;

namespace App.Common.Configuration
{
    ///<summary>
    /// Implementation of <see cref="IUnitOfWorkConfiguration"/>.
    ///</summary>
    public class DefaultUnitOfWorkConfiguration : IUnitOfWorkConfiguration
    {
        bool _autoCompleteScope = false;
        IsolationLevel _defaultIsolation = IsolationLevel.ReadCommitted;

        /// <summary>
        /// Configures <see cref="UnitOfWorkScope"/> settings.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="ICustomDependencyResolver"/> instance.</param>
        public void Configure(ICustomDependencyResolver containerAdapter)
        {
            containerAdapter.RegisterType<ITransactionManager, App.Data.TransactionManager>(LifetimeType.Transient);
            UnitOfWorkSettings.AutoCompleteScope = _autoCompleteScope;
            UnitOfWorkSettings.DefaultIsolation = _defaultIsolation;
        }

        /// <summary>
        /// Sets <see cref="UnitOfWorkScope"/> instances to auto complete when disposed.
        /// </summary>
        public IUnitOfWorkConfiguration AutoCompleteScope()
        {
            _autoCompleteScope = true;
            return this;
        }

        /// <summary>
        /// Sets the default isolation level used by <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <param name="isolationLevel"></param>
        public IUnitOfWorkConfiguration WithDefaultIsolation(IsolationLevel isolationLevel)
        {
            _defaultIsolation = isolationLevel;
            return this;
        }
    }
}