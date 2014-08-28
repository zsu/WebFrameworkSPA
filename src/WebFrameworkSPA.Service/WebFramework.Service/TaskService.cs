using App.Common;
using App.Common.Caching;
using App.Common.Data;
using App.Common.Events;
using App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFramework.Data.Domain;
using App.Common.Tasks;

namespace Service
{
    public class TaskService : ITaskService
    {
        #region Fields
        private readonly IRepository<ScheduleTask, Guid> _repository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private IActivityLogService _activityLogService;
        #endregion
        private enum ActivityType { AddTask, DeleteTask, UpdateTask};

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="itemRepository">item repository</param>
        public TaskService(ICacheManager cacheManager, IRepository<ScheduleTask, Guid> repository, IEventPublisher eventPublisher,IActivityLogService activityLogService)
        {
            this._cacheManager = cacheManager;
            this._repository = repository;
            this._eventPublisher = eventPublisher;
            _activityLogService = activityLogService;
        }

        #endregion

        #region Methods
        public IQueryable<ScheduleTask> Query()
        {
            return _repository.Query;
        }
        public virtual bool Exists(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            int count = _repository.Query.Where(x => x.Name == name.Trim()).Count();
            return count > 0 ? true : false;
        }
        /// <summary>
        /// Adds a item
        /// </summary>
        /// <param name="item">item</param>
        public virtual void Add(ScheduleTask item)
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
            TaskManager.Instance.Refresh();
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Task {0} is created.", item.Name);
            ActivityLog activityItem = new ActivityLog(ActivityType.AddTask.ToString(), message.ToString());
            _activityLogService.Add(activityItem); 
        }

        /// <summary>
        /// Updates a item
        /// </summary>
        /// <param name="item">item</param>
        public virtual void Update(ScheduleTask item)
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
            TaskManager.Instance.Refresh();
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Task {0} is updated.", item.Name);
            ActivityLog activityItem = new ActivityLog(ActivityType.UpdateTask.ToString(), message.ToString());
            _activityLogService.Add(activityItem); 
        }

        /// <summary>
        /// Gets a item by identifier
        /// </summary>
        /// <param name="itemId">item identifier</param>
        /// <returns>item</returns>
        public virtual ScheduleTask GetById(Guid id)
        {
            if (id == default(Guid))
                return null;

            var item = _repository.GetById(id);
            return item;
        }

        public virtual bool Delete(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentNullException("id");
            ScheduleTask item = _repository.Query.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return false;
            using (var scope = new UnitOfWorkScope())
            {
                _repository.Delete(item);
                scope.Commit();
            }

            //event notification
            _eventPublisher.EntityDeleted(item);
            TaskManager.Instance.Refresh();
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Task {0} is deleted.", item.Name);
            ActivityLog activityItem = new ActivityLog(ActivityType.DeleteTask.ToString(), message.ToString());
            _activityLogService.Add(activityItem); 
            return true;
        }
        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">item</param>
        public virtual bool Delete(ScheduleTask item)
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
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Task {0} is deleted.", item.Name);
            ActivityLog activityItem = new ActivityLog(ActivityType.DeleteTask.ToString(), message.ToString());
            _activityLogService.Add(activityItem); 
            return true;
        }

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>item collection</returns>
        /// Changed by: Zhicheng Su
        public virtual List<ScheduleTask> GetAll()
        {
            return _repository.Query.ToList();
        }
        #endregion Methods
    }
}
