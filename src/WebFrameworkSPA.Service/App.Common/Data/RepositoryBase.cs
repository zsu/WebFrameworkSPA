using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using App.Data;
using System.Linq.Expressions;
using System.Collections;

namespace App.Common.Data
{
    public abstract class RepositoryBase<T,TId>:IRepository<T,TId>
    {
        #region IRepository<T,TKey> Members
        /// <summary>
        /// Gets the <see cref="IQueryable{TEntity}"/> used by the <see cref="RepositoryBase{TEntity}"/> 
        /// to execute Linq queries.
        /// </summary>
        /// <value>A <see cref="IQueryable{TEntity}"/> instance.</value>
        /// <remarks>
        /// Inheritors of this base class should return a valid non-null <see cref="IQueryable{TEntity}"/> instance.
        /// </remarks>
        public abstract IQueryable<T> Query { get; }

        public abstract T GetById(TId id);

        /// <summary>
        /// Adds a transient instance of <paramref name="entity"/> to be tracked
        /// and persisted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be added.</param>
        public abstract void Add(T entity);
        /// <summary>
        /// Update instance of <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be added.</param>
        public abstract void Update(T entity);
        /// <summary>
        /// Marks the entity instance to be deleted from the store.
        /// </summary>
        /// <param name="entity">An instance of <paramref name="entity"/> that should be deleted.</param>
        public abstract void Delete(T entity);

        /// <summary>
        /// Detaches a instance from the repository.
        /// </summary>
        /// <param name="entity">The entity instance, currently being tracked via the repository, to detach.</param>
        public abstract void Detach(T entity);

        /// <summary>
        /// Attaches a detached entity, previously detached via the <see cref="IRepository{TEntity}.Detach"/> method.
        /// </summary>
        /// <param name="entity">The entity instance to attach back to the repository.</param>
        public abstract void Attach(T entity);

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        public abstract void Refresh(T entity);

        public virtual IPagedList<T> SortAndPage(IQueryable<T> query,string sortExpression, SortDirection sortDirection, int startRowIndex, int pageSize)
        {
            if (query != null)
            {
                if (!sortExpression.IsNullOrWhiteSpace())
                {
                    StringBuilder orderby = new StringBuilder();
                    string direction = null;
                    if (!sortExpression.IsNullOrWhiteSpace())
                    {
                        if (sortDirection == SortDirection.Descending)
                            direction = "DESC";
                        else
                            direction = "ASC";
                        orderby.AppendFormat("{0} {1}", sortExpression, direction);
                    }
                    query=query.OrderBy(orderby.ToString());
                }
                return new PagedList<T>(query, startRowIndex>0 && pageSize>0?startRowIndex/pageSize:0, pageSize);//Gridview passes in startRowIndex rather than pageIndex
            }
            return null;
        }

        /// <summary>
        /// Gets the a <see cref="IUnitOfWork"/> of <typeparamref name="T"/> that
        /// the repository will use to query the underlying store.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IUnitOfWork"/> implementation to retrieve.</typeparam>
        /// <returns>The <see cref="IUnitOfWork"/> implementation.</returns>
        public virtual K UnitOfWork<K>() where K : IUnitOfWork
        {
            var currentScope = UnitOfWorkManager.CurrentUnitOfWork;
            Check.Assert<InvalidOperationException>(currentScope != null,
                                                     "No compatible UnitOfWork was found. Please start a compatible UnitOfWorkScope before " +
                                                     "using the repository.");

            Check.IsNotTypeOf<K>(currentScope,
                                              "The current UnitOfWork instance is not compatible with the repository. " +
                                              "Please start a compatible unit of work before using the repository.");
            return ((K)currentScope);
        }
        #endregion
    }
}
