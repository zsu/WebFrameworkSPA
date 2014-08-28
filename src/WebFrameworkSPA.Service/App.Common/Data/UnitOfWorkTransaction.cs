using System;
using System.Collections.Generic;
using System.Transactions;
using App.Common.Logging;
using App.Common;
using System.Collections;

namespace App.Data
{
    /// <summary>
    /// Encapsulates a unit of work transaction.
    /// </summary>
    public class UnitOfWorkTransaction : IDisposable
    {
        bool _disposed;
        TransactionScope _transaction;
        IUnitOfWork _unitOfWork;
        IList<IUnitOfWorkScope> _attachedScopes = new List<IUnitOfWorkScope>();

        readonly Guid _transactionId = Guid.NewGuid();
        

        ///<summary>
        /// Raised when the transaction is disposing.
        ///</summary>
        public event Action<UnitOfWorkTransaction> TransactionDisposing;

        ///<summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="UnitOfWorkTransaction"/> class.
        ///</summary>
        ///<param name="unitOfWork">The <see cref="IUnitOfWork"/> instance managed by the 
        /// <see cref="UnitOfWorkTransaction"/> instance.</param>
        ///<param name="transaction">The <see cref="TransactionScope"/> instance managed by 
        /// the <see cref="UnitOfWorkTransaction"/> instance.</param>
        public UnitOfWorkTransaction(IUnitOfWork unitOfWork, TransactionScope transaction)
        {
            Check.Assert<ArgumentNullException>(unitOfWork != null,
                                                 "Expected a non-null UnitOfWork instance.");
            Check.Assert<ArgumentNullException>(transaction != null,
                                                 "Expected a non-null TransactionScope instance.");
            _unitOfWork = unitOfWork;
            _transaction = transaction;
            Logger.Log(LogLevel.Debug,string.Format("New UnitOfWorkTransction created with Id {0}", _transactionId));
        }

        ///<summary>
        /// Gets the unique transaction id of the <see cref="UnitOfWorkTransaction"/> instance.
        ///</summary>
        /// <value>A <see cref="Guid"/> representing the unique id of the <see cref="UnitOfWorkTransaction"/> instance.</value>
        public Guid TransactionId
        {
            get { return _transactionId; }
        }

        /// <summary>
        /// Gets the <see cref="IUnitOfWork"/> instance managed by the 
        /// <see cref="UnitOfWorkTransaction"/> instance.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Attaches a <see cref="UnitOfWorkScope"/> instance to the 
        /// <see cref="UnitOfWorkTransaction"/> instance.
        /// </summary>
        /// <param name="scope">The <see cref="UnitOfWorkScope"/> instance to attach.</param>
        public void EnlistScope(IUnitOfWorkScope scope)
        {
            Check.Assert<ArgumentNullException>(scope != null, "Expected a non-null IUnitOfWorkScope instance.");

            Logger.Log(LogLevel.Debug,string.Format("Scope {1} enlisted with transaction {1}", scope.ScopeId, _transactionId));
            _attachedScopes.Add(scope);
            scope.ScopeComitting += OnScopeCommitting;
            scope.ScopeRollingback += OnScopeRollingBack;
        }

        /// <summary>
        /// Callback executed when an enlisted scope has comitted.
        /// </summary>
        void OnScopeCommitting(IUnitOfWorkScope scope)
        {
            Check.Assert<ObjectDisposedException>(!_disposed,
                                                   "The transaction attached to the scope has already been disposed.");

            Logger.Log(LogLevel.Debug, string.Format("Commit signalled by scope {0} on transaction {1}.", scope.ScopeId, _transactionId));
           if (!_attachedScopes.Contains(scope))
           {
               Dispose();
               throw new InvalidOperationException("The scope being comitted is not attached to the current transaction.");
           }
            scope.ScopeComitting -= OnScopeCommitting;
            scope.ScopeRollingback -= OnScopeRollingBack;
            scope.Complete();
            _attachedScopes.Remove(scope);
            if (_attachedScopes.Count == 0)
            {
                Logger.Log(LogLevel.Debug, string.Format("All scopes have signalled a commit on transaction {0}. Flushing unit of work and comitting attached TransactionScope.", _transactionId));
                try
                {
                    _unitOfWork.Flush();
                    _transaction.Complete();
                }
                finally
                {
                    Dispose(); //Dispose the transaction after comitting.
                }
            }
        }

        /// <summary>
        /// Callback executed when an enlisted scope is rolledback.
        /// </summary>
        void OnScopeRollingBack(IUnitOfWorkScope scope)
        {
            Check.Assert<ObjectDisposedException>(!_disposed,
                                                   "The transaction attached to the scope has already been disposed.");
            Logger.Log(LogLevel.Debug, string.Format("Rollback signalled by scope {0} on transaction {1}.", scope.ScopeId, _transactionId));
            Logger.Log(LogLevel.Debug, string.Format("Detaching all scopes and disposing of attached TransactionScope on transaction {0}", _transactionId));

            scope.ScopeComitting -= OnScopeCommitting;
            scope.ScopeRollingback -= OnScopeRollingBack;
            scope.Complete();
            _attachedScopes.Remove(scope);
            Dispose();
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

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Logger.Log(LogLevel.Debug, string.Format("Disposing off transction {0}", _transactionId));
                if (_unitOfWork != null)
                    _unitOfWork.Dispose();

                if (_transaction != null)
                    _transaction.Dispose();

                if (TransactionDisposing != null)
                    TransactionDisposing(this);

                if (_attachedScopes != null && _attachedScopes.Count > 0)
                {
                    _attachedScopes.ForEach(scope =>
                    {
                        scope.ScopeComitting -= OnScopeCommitting;
                        scope.ScopeRollingback -= OnScopeRollingBack;
                        scope.Complete();
                    });
                    _attachedScopes.Clear();     
                }
            }
            TransactionDisposing = null;
            _unitOfWork = null;
            _transaction = null;
            _attachedScopes = null;
            _disposed = true;
        }
    }
}