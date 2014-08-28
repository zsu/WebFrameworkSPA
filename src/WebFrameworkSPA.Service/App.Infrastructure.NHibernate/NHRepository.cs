using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Common.Data;
using NHibernate;
using NHibernate.Linq;
using App.Common.InversionOfControl;

namespace App.Infrastructure.NHibernate
{
    public class NHRepository<T, TId> : RepositoryBase<T, TId>
    {
        private ISession _session;
        public NHRepository()
        {
            //var sessions = IoC.GetServices<ISession>();
            //if (sessions != null && sessions.Count() > 0)
            //    _session = sessions.FirstOrDefault();
        }
        /// <summary>
        /// Gets the <see cref="ISession"/> instnace that is used by the repository.
        /// </summary>
        private ISession Session
        {
            get
            {
                if (_session == null)
                    _session = UnitOfWork<NHUnitOfWork>().GetSession<T>();
                return _session;
            }
        }

        /// <summary>
        /// Gets the <see cref="IQueryable{TEntity}"/> used by the <see cref="RepositoryBase{TEntity}"/> 
        /// to execute Linq queries.
        /// </summary>
        /// <value>A <see cref="IQueryable{TEntity}"/> instance.</value>
        /// <remarks>
        /// Inheritors of this base class should return a valid non-null <see cref="IQueryable{TEntity}"/> instance.
        /// </remarks>
        public override IQueryable<T> Query
        {
            get
            {
                return Session.Query<T>();
            }
        }

        //public virtual IDbContext DbContext
        //{
        //    get
        //    {
        //        return new DbContext(_sessionFactory);
        //    }
        //}

        public override T GetById(TId id)
        {
            return Session.Get<T>(id);
        }

        /// <summary>
        /// Adds a transient instance of <see cref="TEntity"/> to be tracked
        /// and persisted by the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <remarks>
        /// The Add method replaces the existing <see cref="RepositoryBase{TEntity}.Save"/> method, which will
        /// eventually be removed from the public API.
        /// </remarks>
        public override void Add(T entity)
        {
            Session.SaveOrUpdate(entity);
        }
        /// <summary>
        /// Update instance of <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(T entity)
        {

            Session.Update(entity);

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
        public override void Detach(T entity)
        {
            Session.Evict(entity);
        }

        /// <summary>
        /// Attaches a detached entity, previously detached via the <see cref="IRepository{TEntity}.Detach"/> method.
        /// </summary>
        /// <param name="entity">The entity instance to attach back to the repository.</param>
        public override void Attach(T entity)
        {
            Session.Update(entity);
        }

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        public override void Refresh(T entity)
        {
            Session.Refresh(entity, LockMode.None);
        }

        //private readonly ISessionFactory _sessionFactory;
    }
}
