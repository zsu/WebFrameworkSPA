using System.Linq;
using App.Data;
using System.Web.UI.WebControls;

namespace App.Common.Data
{
    /// <summary>
    /// Fetching strategy wrapper for a IRepository implemenation.
    /// </summary>
    /// <typeparam name="TRepository">The type of repository to wrap.</typeparam>
    /// <typeparam name="TEntity">The entity type of the repository.</typeparam>
    public abstract class RepositoryWrapperBase<TRepository, TEntity, TId> : IRepository<TEntity,TId> where TRepository : IRepository<TEntity,TId>
    {
        readonly TRepository _rootRootRepository;

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="RepositoryWrapperBase{TRepository,TEntity}"/> class.
        /// </summary>
        /// <param name="rootRootRepository">The <see cref="IRepository{TEntity}"/> instance to wrap.</param>
        protected RepositoryWrapperBase(TRepository rootRootRepository)
        {
            _rootRootRepository = rootRootRepository;
        }

        ///<summary>
        /// Gets the <see cref="IRepository{TEntity}"/> instnace that this RepositoryWrapperBase wraps.
        ///</summary>
        /// <value>The wrapped <see cref="IRepository{TEntity}"/> instance</value>
        public virtual TRepository RootRepository
        {
            get { return _rootRootRepository; }
        }

        /// <summary>
        /// Gets the a <see cref="IUnitOfWork"/> of <typeparamref name="T"/> that
        /// the repository will use to query the underlying store.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IUnitOfWork"/> implementation to retrieve.</typeparam>
        /// <returns>The <see cref="IUnitOfWork"/> implementation.</returns>
        public virtual K UnitOfWork<K>() where K : IUnitOfWork
        {
            return _rootRootRepository.UnitOfWork<K>();
        }

        /// <summary>
        /// Adds a transient instance of <paramref name="entity"/> to be tracked
        /// and persisted by the repository.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity)
        {
            _rootRootRepository.Add(entity);
        }
        /// <summary>
        /// Update instance of <paramref name="entity"/>y.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            _rootRootRepository.Update(entity);
        }
        /// <summary>
        /// Marks the changes of an existing entity to be saved to the store.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// updated in the database.</param>
        /// <remarks>Implementors of this method must handle the Update scneario. </remarks>
        public virtual void Delete(TEntity entity)
        {
            _rootRootRepository.Delete(entity);
        }

        /// <summary>
        /// Detaches a instance from the repository.
        /// </summary>
        /// <param name="entity">The entity instance, currently being tracked via the repository, to detach.</param>
        /// <exception cref="NotSupportedException">Implentors should throw the NotImplementedException if Detaching
        /// entities is not supported.</exception>
        public virtual void Detach(TEntity entity)
        {
            _rootRootRepository.Detach(entity);
        }

        /// <summary>
        /// Attaches a detached entity, previously detached via the <see cref="IRepository{TEntity}.Detach"/> method.
        /// </summary>
        /// <param name="entity">The entity instance to attach back to the repository.</param>
        /// <exception cref="NotSupportedException">Implentors should throw the NotImplementedException if Attaching
        /// entities is not supported.</exception>
        public virtual void Attach(TEntity entity)
        {
            _rootRootRepository.Attach(entity);
        }

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        /// <exception cref="NotSupportedException">Implementors should throw the NotImplementedException if Refreshing
        /// entities is not supported.</exception>
        public virtual void Refresh(TEntity entity)
        {
            _rootRootRepository.Refresh(entity);
        }
        public virtual TEntity GetById(TId id)
        {
            return _rootRootRepository.GetById(id);
        }
        /// <summary>
        /// Querries the repository based on the provided specification and returns results that
        /// are only satisfied by the specification.
        /// </summary>
        /// <param name="specification">A <see cref="ISpecification{TEntity}"/> instnace used to filter results
        /// that only satisfy the specification.</param>
        /// <returns>A <see cref="IEnumerable{TEntity}"/> that can be used to enumerate over the results
        /// of the query.</returns>
        public virtual IQueryable<TEntity> Query
        {
            get { return _rootRootRepository.Query; }
        }
        public virtual IPagedList<TEntity> SortAndPage(IQueryable<TEntity> query, string sortExpression, SortDirection sortDirection, int startRowIndex, int pageSize)
        {
            return _rootRootRepository.SortAndPage(query, sortExpression, sortDirection, startRowIndex, pageSize);
        }
    }
}