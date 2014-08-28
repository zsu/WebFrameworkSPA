using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using App.Common.Data;

namespace App.Infrastructure.EntityFramework
{
    public class EFFetchingRepository<TEntity, TId, TReleated> : RepositoryWrapperBase<EFRepository<TEntity, TId>, TEntity, TId>, IEFFetchingRepository<TEntity, TId, TReleated> where TEntity : Entity<TId>
    {
        readonly string _fetchingPath;

        public EFFetchingRepository(EFRepository<TEntity,TId> repository, string fetchingPath) : base(repository)
        {
            _fetchingPath = fetchingPath;
        }

        public string FetchingPath
        {
            get { return _fetchingPath; }
        }
    }
}