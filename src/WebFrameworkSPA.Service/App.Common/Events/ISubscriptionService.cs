using System.Collections.Generic;

namespace App.Common.Events
{
    public interface ISubscriptionService
    {
        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
