using System;
using System.Collections.Generic;
using System.Linq;
using WebFramework.Data.Domain;
using App.Common.Tasks;
namespace Service
{
    public interface IActivityLogService:IService
    {
        IQueryable<ActivityLog> Query();
        //bool Exists(string name);
        void Add(ActivityLog item);
        bool Delete(Guid id);
        bool Delete(ActivityLog item);
        List<ActivityLog> GetAll();
        ActivityLog GetById(Guid id);
        void Update(ActivityLog item);
    }
}
