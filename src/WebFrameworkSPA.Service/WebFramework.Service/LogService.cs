using App.Common;
using App.Common.Data;
using App.Common.InversionOfControl;
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
    public class LogService : ILogService
    {
        private IRepository<Logs,long> _logsRepository;

        public LogService(IRepository<Logs, long> logsRepository)
        {
            _logsRepository = logsRepository;
        }
        public IQueryable<Logs> Query() 
        {
            return _logsRepository.Query;
        }

        public Logs GetLogById(long id)
        {
            if (id == default(long))
                return null;

            var log = _logsRepository.GetById(id);
            return log;

        }
    }
}
