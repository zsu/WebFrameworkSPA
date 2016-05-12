using App.Common.Data;
using System.Linq;
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
