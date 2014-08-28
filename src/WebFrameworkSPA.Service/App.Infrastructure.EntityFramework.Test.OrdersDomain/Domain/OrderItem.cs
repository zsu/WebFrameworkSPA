﻿using App.Common.Data;
namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain
{
    public class OrderItem : Entity<int>
    {
        public virtual int OrderItemID { get; set; }
        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Store { get; set; }
        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}