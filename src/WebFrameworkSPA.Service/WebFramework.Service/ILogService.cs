using WebFramework.Data.Domain;
using App.Common.Data;
using Elmah;
using System;
using System.Linq;
namespace Service
{
    public interface ILogService:IService
    {
        IQueryable<Logs> Query();
        Logs GetById(long id);
    }
}
