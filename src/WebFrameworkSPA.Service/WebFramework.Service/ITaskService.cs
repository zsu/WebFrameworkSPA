using System;
using System.Collections.Generic;
using System.Linq;
using WebFramework.Data.Domain;
using App.Common.Tasks;
namespace Service
{
    public interface ITaskService:IService
    {
        IQueryable<ScheduleTask> Query();
        bool Exists(string name);
        void Add(ScheduleTask item);
        bool Delete(Guid id);
        bool Delete(ScheduleTask item);
        List<ScheduleTask> GetAll();
        ScheduleTask GetById(Guid id);
        void Update(ScheduleTask item);
    }
}
