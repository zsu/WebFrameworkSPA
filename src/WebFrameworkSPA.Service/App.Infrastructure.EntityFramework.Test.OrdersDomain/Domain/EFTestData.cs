using System;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using App.Common;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain
{
    public class EFTestData:IDisposable
    {
        readonly ObjectContext _context;
        readonly IList<object> _entitiesPersisted;

        public EFTestData(ObjectContext context)
        {
            _context = context;
            _context.ContextOptions.LazyLoadingEnabled = true;
            _entitiesPersisted = new List<object>();
        }

        public ObjectContext Context
        {
            get { return _context; }
        }
        public IList<object> EntitiesPersisted
        {
            get { return _entitiesPersisted; }
        }
        public void Refresh(object entity)
        {
            _context.Refresh(RefreshMode.StoreWins, entity);
        }

        public T Get<T>(Func<T, bool> predicate) where T : class
        {
            return _context.CreateObjectSet<T>().Where(predicate).FirstOrDefault();
        }

        public void Batch(Action<EFTestDataActions> action)
        {
            var dataActions = new EFTestDataActions(this);
            action(dataActions);
            _context.SaveChanges();
        }
        public void Dispose()
        {
            if (_entitiesPersisted.Count <= 0)
                return;

            _entitiesPersisted.ForEach(x => _context.DeleteObject(x));
            _context.SaveChanges();
        }
    }
}