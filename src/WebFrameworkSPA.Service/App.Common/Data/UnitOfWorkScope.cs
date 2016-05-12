﻿using System;
using App.Common.Logging;
using App.Common;

namespace App.Data
{
    /// <summary>
    /// Helper class that allows starting and using a unit of work like:
    /// <![CDATA[
    ///     using (UnitOfWorkScope scope = new UnitOfWorkScope()) {
    ///         //Do some stuff here.
    ///         scope.Commit();
    ///     }
    /// 
    /// ]]>
    /// </summary>
    public class UnitOfWorkScope : IUnitOfWorkScope
    {
        bool _disposed;
        bool _commitAttempted;
        bool _completed;
        readonly Guid _scopeId = Guid.NewGuid();

        /// <summary>
        /// Event fired when the scope is comitting.
        /// </summary>
        public event Action<IUnitOfWorkScope> ScopeComitting;

        /// <summary>
        /// Event fired when the scope is rollingback.
        /// </summary>
        public event Action<IUnitOfWorkScope> ScopeRollingback;

        /// <summary>
        /// Default Constuctor.
        /// Creates a new <see cref="UnitOfWorkScope"/> with the <see cref="System.Data.IsolationLevel.Serializable"/> 
        /// transaction isolation level.
        /// </summary>
        public UnitOfWorkScope() : this(TransactionMode.Default) { }

        /// <summary>
        /// Overloaded Constructor.
        /// Creates a new instance of the <see cref="UnitOfWorkScope"/> class.
        /// </summary>
        /// <param name="newTransaction">To create a new scope that does not enlist in an existing ambient 
        /// <see cref="UnitOfWorkScope"/> or <see cref="TransactionScope"/>, specify new, otherwise specify false.</param>
        [Obsolete("Use UnitOfWorkScope(TransactionMode) constructor instead. This will be removed in final 1.1 release.")]
        public UnitOfWorkScope(bool newTransaction)
        {
            Logger.Log(LogLevel.Debug,string.Format("New UnitOfWorkScope {0} started with newTransaction setting as : {1}", _scopeId, newTransaction));
            UnitOfWorkManager.CurrentTransactionManager.EnlistScope(this, TransactionMode.New);
        }

        /// <summary>
        /// Overloaded Constructor.
        /// Creates a new instance of the <see cref="UnitOfWorkScope"/> class.
        /// </summary>
        /// <param name="mode">A <see cref="TransactionMode"/> enum specifying the transation mode
        /// of the unit of work.</param>
        public UnitOfWorkScope(TransactionMode mode)
        {
            UnitOfWorkManager.CurrentTransactionManager.EnlistScope(this, mode);
        }

        /// <summary>
        /// Gets the unique Id of the <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <value>A <see cref="Guid"/> representing the unique Id of the scope.</value>
        public Guid ScopeId
        {
            get { return _scopeId; }
        }

        /// <summary>
        /// Gets the current unit of work that the scope participates in.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IUnitOfWork"/> to retrieve.</typeparam>
        /// <returns>A <see cref="IUnitOfWork"/> instance of type <typeparamref name="T"/> that
        /// the scope participates in.</returns>
        public T CurrentUnitOfWork<T>()
        {
            var currentUow = UnitOfWorkManager.CurrentUnitOfWork;
            Check.Assert<InvalidOperationException>(currentUow != null,
                                                     "No compatible UnitOfWork was found. Please start a compatible UnitOfWorkScope before " +
                                                     "using the repository.");

            Check.IsNotTypeOf<T>(currentUow,
                            "The current unit of work is not compatible with expected type" + typeof(T).FullName +
                            ", instead the current unit of work is of type " + currentUow.GetType().FullName + ".");
            return (T) currentUow;
        }

        ///<summary>
        /// Commits the current running transaction in the scope.
        ///</summary>
        public void Commit()
        {
            Check.Assert<ObjectDisposedException>(!_disposed,
                                                   "Cannot commit a disposed UnitOfWorkScope instance.");
            Check.Assert<InvalidOperationException>(!_completed,
                                                     "This unit of work scope has been marked completed. A child scope participating in the " +
                                                     "transaction has rolledback and the transaction aborted. The parent scope cannot be commit.");

            
            _commitAttempted = true;
            OnCommit();
        }

        /// <summary>
        /// Marks the scope as completed.
        /// Used for internally by application and should not be used by consumers.
        /// </summary>
        public void Complete()
        {
            _completed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void OnCommit()
        {
            Logger.Log(LogLevel.Debug,string.Format("UnitOfWorkScope {0} Comitting.", _scopeId));
            if (ScopeComitting != null)
                ScopeComitting(this);
        }

        /// <summary>
        /// 
        /// </summary>
        void OnRollback()
        {
            Logger.Log(LogLevel.Debug, string.Format("UnitOfWorkScope {0} Rolling back.", _scopeId));
            if (ScopeRollingback != null)
                ScopeRollingback(this);
        }

        /// <summary>
        /// Disposes off the <see cref="UnitOfWorkScope"/> insance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and un-managed resources used.
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    if (_completed)
                    {
                        //Scope is marked as completed. Nothing to do here...
                        _disposed = true;
                        return;
                    }

                    if (!_commitAttempted && UnitOfWorkSettings.AutoCompleteScope)
                        //Scope did not try to commit before, and auto complete is switched on. Trying to commit.
                        //If an exception occurs here, the finally block will clean things up for us.
                        OnCommit(); 
                    else
                        //Scope either tried a commit before or auto complete is turned off. Trying to rollback.
                        //If an exception occurs here, the finally block will clean things up for us.
                        OnRollback();
                }
                finally
                {
                    ScopeComitting = null;
                    ScopeRollingback = null;
                    _disposed = true;
                }
            }
        }
    }
}
