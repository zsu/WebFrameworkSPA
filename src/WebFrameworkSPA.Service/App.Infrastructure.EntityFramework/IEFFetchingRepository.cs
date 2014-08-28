using App.Common.Data;
namespace App.Infrastructure.EntityFramework
{
    public interface IEFFetchingRepository<TEntity, TId, TFetch> : IRepository<TEntity, TId> where TEntity : Entity<TId>
    {
        EFRepository<TEntity,TId> RootRepository { get; }

        string FetchingPath { get; }
    }
}