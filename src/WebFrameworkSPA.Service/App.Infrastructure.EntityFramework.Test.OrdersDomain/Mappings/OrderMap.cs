using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Mappings
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Orders");
            HasKey(x => x.OrderID);
            Property(x => x.OrderID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();        
            Property(x => x.OrderDate);
            Property(x => x.ShipDate);
            HasRequired(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);
            Ignore(x => x.Id);
        }
    }
}
