using WebFramework.Data.Domain;
using System.Linq;
namespace Service
{
    public interface ILogService:IService
    {
        IQueryable<Logs> Query();
        Logs GetLogById(long id);
    }
}
