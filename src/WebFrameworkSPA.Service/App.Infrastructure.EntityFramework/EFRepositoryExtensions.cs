using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using App.Common;
using App.Common.Data;
using App.Common.Expressions;

namespace App.Infrastructure.EntityFramework
{
    public static class EFRepositoryExtensions
    {
        public static IEFFetchingRepository<TEntity,TId,TReleated> Fetch<TEntity, TId,TReleated>(this IRepository<TEntity, TId> repository, Expression<Func<TEntity, TReleated>> selector) where TEntity : Entity<TId>
        {
            Check.Assert<ArgumentNullException>(repository != null, "Expected a non-null IRepository<> instance.");

            var efRepository = repository as EFRepository<TEntity,TId>;
            Check.Assert<InvalidOperationException>(efRepository != null,
                "Cannot use Entity Framework's Fetch extension on the underlying repository. The repository " +
                "does not inherit or is not a EFRepository<> instance. The Entity Framework's fetching extensions can " +
                "only be used by entity framework's repository EFRepository<>.");

            var visitor = new MemberAccessPathVisitor();
            visitor.Visit(selector);
            efRepository.AddInclude(visitor.Path);

            return (IEFFetchingRepository<TEntity,TId, TReleated>)
                Activator.CreateInstance(typeof(EFFetchingRepository<TEntity, TId,TReleated>), efRepository, visitor.Path);
        }

        public static IEFFetchingRepository<TEntity, TId, TReleated> FetchMany<TEntity, TId, TReleated>(this IRepository<TEntity, TId> repository, Expression<Func<TEntity, IEnumerable<TReleated>>> selector) where TEntity : Entity<TId>
        {
            Check.Assert<ArgumentNullException>(repository != null, "Expected a non-null IRepository<> instance.");

            var efRepository = repository as EFRepository<TEntity, TId>;
            Check.Assert<InvalidOperationException>(efRepository != null,
                "Cannot use Entity Framework's FetchMany extension on the underlying repository. The repository " +
                "does not inherit or is not a EFRepository<> instance. The Entity Framework's fetching extensions can " +
                "only be used by entity framework's repository EFRepository<>.");

            var visitor = new MemberAccessPathVisitor();
            visitor.Visit(selector);
            efRepository.AddInclude(visitor.Path);

            return (IEFFetchingRepository<TEntity, TId, TReleated>)
                Activator.CreateInstance(typeof(EFFetchingRepository<TEntity, TId, TReleated>), efRepository, visitor.Path);
        }

        public static IEFFetchingRepository<TEntity, TId, TReleated> ThenFetch<TEntity, TId, TFetch, TReleated>(this IEFFetchingRepository<TEntity, TId, TFetch> repository, Expression<Func<TFetch, TReleated>> selector) where TEntity : Entity<TId>
        {
            Check.Assert<ArgumentNullException>(repository != null, "Expected a non-null IEFFetchingRepository<> instance.");

            var visitor = new MemberAccessPathVisitor();
            visitor.Visit(selector);
            var includePath = repository.FetchingPath + "." + visitor.Path;
            repository.RootRepository.AddInclude(includePath);

            return (IEFFetchingRepository<TEntity, TId, TReleated>)
                Activator.CreateInstance(typeof(EFFetchingRepository<TEntity, TId, TReleated>), repository.RootRepository, includePath);
        }

        public static IEFFetchingRepository<TEntity, TId, TReleated> ThenFetchMany<TEntity, TId, TFetch, TReleated>(this IEFFetchingRepository<TEntity, TId, TFetch> repository, Expression<Func<TFetch, IEnumerable<TReleated>>> selector) where TEntity : Entity<TId>
        {
            Check.Assert<ArgumentNullException>(repository != null, "Expected a non-null IEFFetchingRepository<> instance.");

            var visitor = new MemberAccessPathVisitor();
            visitor.Visit(selector);
            var includePath = repository.FetchingPath + "." + visitor.Path;
            repository.RootRepository.AddInclude(includePath);

            return (IEFFetchingRepository<TEntity, TId, TReleated>)
                Activator.CreateInstance(typeof(EFFetchingRepository<TEntity, TId, TReleated>), repository.RootRepository, includePath);
        }
    }
}