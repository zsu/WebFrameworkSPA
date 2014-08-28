using System.Collections.Generic;
using System.Linq;
using App.Common.InversionOfControl;

namespace App.Common.Events
{
    public class SubscriptionService : ISubscriptionService
    {
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            return IoC.GetServices<IConsumer<T>>().ToList();
        }
    }
}
