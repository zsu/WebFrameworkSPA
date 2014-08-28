using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Mappings
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Products");
            HasKey(x => x.ProductID);
            Property(x => x.ProductID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(x => x.Name);
            Ignore(x => x.Id);
        }
    }
}
