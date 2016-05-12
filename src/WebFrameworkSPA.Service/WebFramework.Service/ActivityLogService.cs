using App.Common.Caching;
using App.Common.Data;
using App.Common.Events;
using App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFramework.Data.Domain;

namespace Service
{
    public class ActivityLogService : IActivityLogService
    {
        #region Fields
        private readonly IRepository<ActivityLog, Guid> _repository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="itemRepository">item repository</param>
        public ActivityLogService(ICacheManager cacheManager, IRepository<ActivityLog, Guid> repository, IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._repository = repository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods
        public IQueryable<ActivityLog> Query()
        {
            return _repository.Query;
        }
        //public virtual bool Exists(string name)
        //{
        //    if (string.IsNullOrEmpty(name))
        //        throw new ArgumentNullException("name");
        //    int count = _repository.Query.Where(x => x.Name == name.Trim()).Count();
        //    return count > 0 ? true : false;
        //}
        /// <summary>
        /// Adds a item
        /// </summary>
        /// <param name="item">item</param>
        public virtual void Add(ActivityLog item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            using (var scope = new UnitOfWorkScope())
            {
                _repository.Add(item);
                scope.Commit();
            }

            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates a item
        /// </summary>
        /// <param name="item">item</param>
        public virtual void Update(ActivityLog item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            using (var scope = new UnitOfWorkScope())
            {
                _repository.Update(item);
                scope.Commit();
            }

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Gets a item by identifier
        /// </summary>
        /// <param name="itemId">item identifier</param>
        /// <returns>item</returns>
        public virtual ActivityLog GetById(Guid id)
        {
            if (id == default(Guid))
                return default(ActivityLog);

            var item = _repository.GetById(id);
            return item;
        }

        public virtual bool Delete(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentNullException("id");
            ActivityLog item = _repository.Query.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return false;
            using (var scope = new UnitOfWorkScope())
            {
                _repository.Delete(item);
                scope.Commit();
            }

            //event notification
            _eventPublisher.EntityDeleted(item);
            return true;
        }
        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">item</param>
        public virtual bool Delete(ActivityLog item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            using (var scope = new UnitOfWorkScope())
            {
                _repository.Delete(item);
                scope.Commit();
            }

            //event notification
            _eventPublisher.EntityDeleted(item);
            return true;
        }

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>item collection</returns>
        /// Changed by: Zhicheng Su
        public virtual List<ActivityLog> GetAll()
        {
            return _repository.Query.ToList();
        }
        #endregion Methods
    }
}
