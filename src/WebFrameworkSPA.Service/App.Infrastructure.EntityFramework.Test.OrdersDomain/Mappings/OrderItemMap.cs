using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Mappings
{
    public class OrderItemMap: EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            ToTable("OrderItems");
            HasKey(x => x.OrderItemID);
            Property(x => x.OrderItemID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity).IsRequired();
            Property(x => x.Price);
            Property(x => x.Quantity);
            Property(x => x.Store);
            HasRequired(o => o.Order)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(o => o.OrderId);
            HasRequired(o => o.Product)
                .WithMany()
               .HasForeignKey(o => o.ProductId); 
            Ignore(x => x.Id);
        }
    }
}
