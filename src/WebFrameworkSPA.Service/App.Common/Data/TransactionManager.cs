using System;
using System.Collections.Generic;
using App.Common;
using App.Common.Logging;
using App.Common.InversionOfControl;

namespace App.Data
{
    /// <summary>
    /// Default implementation of <see cref="ITransactionManager"/> interface.
    /// <remarks>To avoid "The connection object can not be enlisted in transaction scope" problem, the same TrasactionManager must be used for nested <see cref="UnitofWorkScope"/></remarks>
    /// </summary>
    public class TransactionManager : ITransactionManager, IDisposable
    {
        bool _disposed;
        readonly Guid _transactionManagerId = Guid.NewGuid();
        readonly LinkedList<UnitOfWorkTransaction> _transactions = new LinkedList<UnitOfWorkTransaction>();

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="TransactionManager"/> class.
        /// </summary>
        public TransactionManager()
        {
            Logger.Log(LogLevel.Debug,string.Format("New instance of TransactionManager with Id {0} created.", _transactionManagerId));
        }

        /// <summary>
        /// Gets the current <see cref="IUnitOfWork"/> instance.
        /// </summary>
        public IUnitOfWork CurrentUnitOfWork
        {
            get 
            {
                return CurrentTransaction == null ? null : CurrentTransaction.UnitOfWork;
            }
        }

        /// <summary>
        /// Gets the current <see cref="UnitOfWorkTransaction"/> instance.
        /// </summary>
        public UnitOfWorkTransaction CurrentTransaction
        {
            get
            {
                return _transactions.Count == 0 ? null : _transactions.First.Value;
            }
        }

        /// <summary>
        /// Enlists a <see cref="UnitOfWorkScope"/> instance with the transaction manager,
        /// with the specified transaction mode.
        /// </summary>
        /// <param name="scope">The <see cref="IUnitOfWorkScope"/> to register.</param>
        /// <param name="mode">A <see cref="TransactionMode"/> enum specifying the transaciton
        /// mode of the unit of work.</param>
        public void EnlistScope(IUnitOfWorkScope scope, TransactionMode mode)
        {
            Logger.Log(LogLevel.Debug,string.Format("Enlisting scope {0} with transaction manager {1} with transaction mode {2}",
                                scope.ScopeId,
                                _transactionManagerId,
                                mode));

            var uowFactory = IoC.GetService<IUnitOfWorkFactory>();
            if (_transactions.Count == 0 || 
                mode == TransactionMode.New ||
                mode == TransactionMode.Supress)
            {
                Logger.Log(LogLevel.Debug,string.Format("Enlisting scope {0} with mode {1} requires a new TransactionScope to be created.", scope.ScopeId, mode));
                var txScope = TransactionScopeHelper.CreateScope(UnitOfWorkSettings.DefaultIsolation, mode);
                var unitOfWork = uowFactory.Create();
                var transaction = new UnitOfWorkTransaction(unitOfWork, txScope);
                transaction.TransactionDisposing += OnTransactionDisposing;
                transaction.EnlistScope(scope);
                _transactions.AddFirst(transaction);
                return;
            }
            CurrentTransaction.EnlistScope(scope);
        }

        /// <summary>
        /// Handles a Dispose signal from a transaction.
        /// </summary>
        /// <param name="transaction"></param>
        void OnTransactionDisposing(UnitOfWorkTransaction transaction)
        {
            Logger.Log(LogLevel.Debug,string.Format("UnitOfWorkTransaction {0} signalled a disposed. Unregistering transaction from TransactionManager {1}",
                                    transaction.TransactionId, _transactionManagerId));

            transaction.TransactionDisposing -= OnTransactionDisposing;
            var node = _transactions.Find(transaction);
            if (node != null)
                _transactions.Remove(node);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Internal dispose.
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Logger.Log(LogLevel.Debug,string.Format("Disposing off transction manager {0}", _transactionManagerId));
                if (_transactions != null && _transactions.Count > 0)
                {
                    _transactions.ForEach(tx =>
                    {
                        tx.TransactionDisposing -= OnTransactionDisposing;
                        tx.Dispose();
                    });
                    _transactions.Clear();
                }
            }
            _disposed = true;
        }
    }
}