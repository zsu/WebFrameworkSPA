using System;
using System.Collections.Generic;
using App.Common;
using NHibernate;

namespace App.Infrastructure.NHibernate.Test
{
    public class NHTestData : IDisposable
    {
        readonly ISession _session;
        readonly IList<object> _entitiesPersisted;

        public NHTestData(ISession session)
        {
            _session = session;
            _entitiesPersisted = new List<object>();
        }

        public ISession Session
        {
            get { return _session; }
        }

        public IList<object> EntitiesPersisted
        {
            get { return _entitiesPersisted; }
        }

        public void Batch(Action<NHTestDataActions> action)
        {
            using (var nhTx = _session.BeginTransaction())
            {
                var dataActions = new NHTestDataActions(this);
                action(dataActions);
                nhTx.Commit();
            }
        }

        public void Dispose()
        {
            if (_entitiesPersisted.Count <= 0) 
                return;

            _entitiesPersisted.ForEach(x => _session.Delete(x));
            _session.Flush();
        }
    }
}