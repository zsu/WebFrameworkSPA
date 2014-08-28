using System;

namespace App.Data
{
    /// <summary>
    /// A unit of work contract that that encapsulates the Unit of Work pattern.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Flushes the changes made in the unit of work to the data store.
        /// </summary>
        void Flush();
    }
}