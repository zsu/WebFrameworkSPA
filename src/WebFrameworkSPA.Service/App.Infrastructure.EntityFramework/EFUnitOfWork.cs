using System;
using System.Collections.Generic;
using App.Data;
using App.Common;

namespace App.Infrastructure.EntityFramework
{
    /// <summary>
    /// Implements the <see cref="IUnitOfWork"/> interface to provide an implementation
    /// of a IUnitOfWork that uses Entity Framework to query and update the underlying store.
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        bool _disposed;
        readonly IEFSessionResolver _resolver;
        IDictionary<Guid, IEFSession> _openSessions = new Dictionary<Guid, IEFSession>();

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="EFUnitOfWork"/> class that uses the specified object context.
        /// </summary>
        /// <param name="resolver">An instance of <see cref="EFUnitOfWorkSettings"/> that contains settings for
        /// Entity Framework unit of work instances.</param>
        public EFUnitOfWork(IEFSessionResolver resolver)
        {
            Check.Assert<ArgumentNullException>(resolver != null,
                                                 "Expected a non-null EFUnitOfWorkSettings instance.");
            _resolver = resolver;
        }

        /// <summary>
        /// Gets a <see cref="IEFSession"/> that can be used to query and update the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="IEFSession"/> should be returned.</typeparam>
        /// <returns>An <see cref="IEFSession"/> that can be used to query and update the specified type.</returns>
        public IEFSession GetSession<T>()
        {
            Check.Assert<ObjectDisposedException>(!_disposed,
                                                   "The current EFUnitOfWork instance has been disposed. " + 
                                                   "Cannot get sessions from a disposed UnitOfWork instance.");

            var sessionKey = _resolver.GetSessionKeyFor<T>();
            if (_openSessions.ContainsKey(sessionKey))
                return _openSessions[sessionKey];

            //Opening a new session...
            var session = _resolver.OpenSessionFor<T>();
            _openSessions.Add(sessionKey, session);
            return session;
        }

        /// <summary>
        /// Flushes the changes made in the unit of work to the data store.
        /// </summary>
        public void Flush()
        {
            Check.Assert<ObjectDisposedException>(!_disposed,
                                                   "The current EFUnitOfWork instance has been disposed. " +
                                                   "Cannot get sessions from a disposed UnitOfWork instance.");

            _openSessions.ForEach(session => session.Value.SaveChanges());
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
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_openSessions != null && _openSessions.Count > 0)
                {
                    _openSessions.ForEach(session => session.Value.Dispose());
                    _openSessions.Clear();
                }
            }
            _openSessions = null;
            _disposed = true;
        }
    }
}