using System;
using System.Collections.Generic;
using App.Common.Data;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain
{
    public class Order : Entity<int>
    {
        ICollection<OrderItem> _orderItems;

        public virtual int OrderID { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DateTime OrderDate { get; set; }
        public virtual DateTime ShipDate { get; set; }
        public virtual ICollection<OrderItem> OrderItems
        {
            get { return _orderItems ?? (_orderItems = new HashSet<OrderItem>());}
            set { _orderItems = value; }
        }
    }
}