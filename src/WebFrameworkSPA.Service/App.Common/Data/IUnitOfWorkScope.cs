using System;

namespace App.Data
{
    ///<summary>
    ///</summary>
    public interface IUnitOfWorkScope : IDisposable
    {
        /// <summary>
        /// Event fired when the scope is comitting.
        /// </summary>
        event Action<IUnitOfWorkScope> ScopeComitting;

        /// <summary>
        /// Event fired when the scope is rollingback.
        /// </summary>
        event Action<IUnitOfWorkScope> ScopeRollingback;

        /// <summary>
        /// Gets the unique Id of the <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <value>A <see cref="Guid"/> representing the unique Id of the scope.</value>
        Guid ScopeId { get; }

        ///<summary>
        /// Commits the current running transaction in the scope.
        ///</summary>
        void Commit();

        /// <summary>
        /// Marks the scope as completed.
        /// Used for internally by application and should not be used by consumers.
        /// </summary>
        void Complete();
    }
}