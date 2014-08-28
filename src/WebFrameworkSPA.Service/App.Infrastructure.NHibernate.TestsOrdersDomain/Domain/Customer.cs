using System.Collections.Generic;
using App.Common.Data;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain
{
    public class Customer:Entity<int>
    {
        public virtual int CustomerID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}