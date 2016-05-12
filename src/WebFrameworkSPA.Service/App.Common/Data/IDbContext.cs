using System;

namespace App.Common.Data
{
    public interface IDbContext
    {
        IDisposable BeginTransaction();
        void CommitChanges();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
