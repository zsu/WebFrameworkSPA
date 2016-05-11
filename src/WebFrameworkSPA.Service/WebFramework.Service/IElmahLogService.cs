using App.Common.Data;
using Elmah;
using System;
using System.Linq;
using WebFramework.Data.Domain;
namespace Service
{
    public interface IElmahLogService:IService
    {
        IQueryable<ElmahLog> Query();
        ErrorLogEntry GetElmahLogById(string id);
    }
}
