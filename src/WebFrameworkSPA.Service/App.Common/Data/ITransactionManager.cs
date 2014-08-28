using System;

namespace App.Data
{
    /// <summary>
    /// Implemented by a transaction manager that manages unit of work transactions.
    /// </summary>
    public interface ITransactionManager : IDisposable
    {
        /// <summary>
        /// Returns the current <see cref="IUnitOfWork"/>.
        /// </summary>
        IUnitOfWork CurrentUnitOfWork { get;}

        /// <summary>
        /// Enlists a <see cref="UnitOfWorkScope"/> instance with the transaction manager,
        /// with the specified transaction mode.
        /// </summary>
        /// <param name="scope">The <see cref="IUnitOfWorkScope"/> to register.</param>
        /// <param name="mode">A <see cref="TransactionMode"/> enum specifying the transaciton
        /// mode of the unit of work.</param>
        void EnlistScope(IUnitOfWorkScope scope, TransactionMode mode);
    }
}