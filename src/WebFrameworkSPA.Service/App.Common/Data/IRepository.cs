using System.Linq;
using System.Web.UI.WebControls;
using App.Data;

namespace App.Common.Data
{
    public partial interface IRepository<T, TKey>
    {
        T GetById(TKey id);
        /// <summary>
        /// Adds a transient instance of <paramref name="entity"/> to be tracked
        /// and persisted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> to be persisted.</param>
        void Add(T entity);
        /// <summary>
        /// Update instance of <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> to be persisted.</param>
        void Update(T entity);
        /// <summary>
        /// Marks the changes of an existing entity to be saved to the store.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// updated in the database.</param>
        /// <remarks>Implementors of this method must handle the Update scneario. </remarks>
        void Delete(T entity);

        /// <summary>
        /// Detaches a instance from the repository.
        /// </summary>
        /// <param name="entity">The entity instance, currently being tracked via the repository, to detach.</param>
        /// <exception cref="NotSupportedException">Implentors should throw the NotImplementedException if Detaching
        /// entities is not supported.</exception>
        void Detach(T entity);

        /// <summary>
        /// Attaches a detached entity, previously detached via the <see cref="Detach"/> method.
        /// </summary>
        /// <param name="entity">The entity instance to attach back to the repository.</param>
        /// <exception cref="NotSupportedException">Implentors should throw the NotImplementedException if Attaching
        /// entities is not supported.</exception>
        void Attach(T entity);

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        /// <exception cref="NotSupportedException">Implementors should throw the NotImplementedException if Refreshing
        /// entities is not supported.</exception>
        void Refresh(T entity);
        IQueryable<T> Query { get; }
        IPagedList<T> SortAndPage(IQueryable<T> query,string sorExpression, SortDirection sortDirection, int pageIndex, int pageSize);
        K UnitOfWork<K>() where K : IUnitOfWork;
    }
}
