using System;
using System.Collections.Generic;
using App.Common;
using App.Common.Data;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain
{
    public class Order:Entity<int>
    {
        public Order()
        {
            Items = new HashSet<OrderItem>();   
        }
        public virtual int OrderID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DateTime OrderDate { get; set; }
        public virtual DateTime ShipDate { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }

        /// <summary>
        /// Simple method to calculate total of all items in the order.
        /// </summary>
        /// <returns></returns>
        public virtual decimal CalculateTotal ()
        {
            decimal total = 0;
            Items.ForEach(x => total += x.TotalPrice);
            return total;
        }
    }
}
