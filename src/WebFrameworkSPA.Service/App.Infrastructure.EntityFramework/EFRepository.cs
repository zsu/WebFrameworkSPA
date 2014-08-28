using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using App.Common.Data;

namespace App.Infrastructure.EntityFramework
{
    /// <summary>
    /// Inherits from the <see cref="RepositoryBase{TEntity}"/> class to provide an implementation of a
    /// Repository that uses Entity Framework.
    /// </summary>
    public class EFRepository<T,TId> : RepositoryBase<T,TId> where T : Entity<TId>
    {
        private IEFSession _session;
        readonly List<string> _includes = new List<string>();

        /// <summary>
        /// Creates a new instance of the <see cref="EFRepository{TEntity}"/> class.
        /// </summary>
        public EFRepository()
        {
            //var sessions = ServiceLocator.Current.GetAllInstances<IEFSession>();
            //if (sessions != null && sessions.Count() > 0)
            //    _privateSession = sessions.First();
        }

        /// <summary>
        /// Gets the <see cref="ObjectContext"/> to be used by the repository.
        /// </summary>
        private IEFSession Session
        {
            get
            {
                if(_session==null)
                    _session= UnitOfWork<EFUnitOfWork>().GetSession<T>();
                return _session;

            }
        }
        public override T GetById(TId id)
        {
            return Query.Where(x => x.Id.Equals(id)).SingleOrDefault();
        }
        /// <summary>
        /// Gets the <see cref="IQueryable{TEntity}"/> used by the <see cref="RepositoryBase{TEntity}"/> 
        /// to execute Linq queries.
        /// </summary>
        /// <value>A <see cref="IQueryable{TEntity}"/> instance.</value>
        public override IQueryable<T> Query
        {
            get
            {
                var query = Session.CreateQuery<T>();
                if (_includes.Count > 0)
                    _includes.ForEach(x => query = query.Include(x));
                return query;
            }
        }

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to be tracked
        /// and persisted by the repository.
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(T entity)
        {
            Session.Add(entity);
        }

        /// <summary>
        /// Update instance of <typeparamref cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            //if (!this._context.IsAttached(entity))
            //    this._entities.Attach(entity);

            Session.SaveChanges();
        }
        /// <summary>
        /// Marks the entity instance to be deleted from the store.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be deleted.</param>
        public override void Delete(T entity)
        {
            Session.Delete(entity);
        }

        /// <summary>
        /// Detaches a instance from the repository.
        /// </summary>
        /// <param name="entity">The entity instance, currently being tracked via the repository, to detach.</param>
        /// <exception cref="NotImplementedException">Implentors should throw the NotImplementedException if Detaching
        /// entities is not supported.</exception>
        public override void Detach(T entity)
        {
            Session.Detach(entity);
        }

        /// <summary>
        /// Attaches a detached entity, previously detached via the <see cref="IRepository{TEntity}.Detach"/> method.
        /// </summary>
        /// <param name="entity">The entity instance to attach back to the repository.</param>
        /// <exception cref="NotImplementedException">Implentors should throw the NotImplementedException if Attaching
        /// entities is not supported.</exception>
        public override void Attach(T entity)
        {
            Session.Attach(entity);
        }

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        /// <exception cref="NotImplementedException">Implementors should throw the NotImplementedException if Refreshing
        /// entities is not supported.</exception>
        public override void Refresh(T entity)
        {
            Session.Refresh(entity);
        }

        internal void AddInclude(string includePath)
        {
            _includes.Add(includePath);
        }
    }
}
