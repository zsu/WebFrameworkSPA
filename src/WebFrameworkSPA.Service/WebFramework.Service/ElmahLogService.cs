using App.Common;
using App.Common.Data;
using App.Common.InversionOfControl;
using Elmah;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebFramework.Data.Domain;

namespace Service
{
    public class ElmahLogService : IElmahLogService
    {
        public IQueryable<ElmahLog> Query() 
        {
            var db = IoC.GetService<IRepository<ElmahLog, Guid>>();
            return db.Query;
        }

        public ErrorLogEntry GetElmahLogById(string id)
        {
            var log = ErrorLog.GetDefault(HttpContext.Current);
            var errorEntry = log.GetError(id);
            return errorEntry;
        }
    }
}
