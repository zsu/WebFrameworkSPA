using System.Data.Entity.ModelConfiguration;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Mappings
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Products");
            HasKey(x => x.ProductID);
            Property(x => x.ProductID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity).IsRequired();
            Property(x => x.Name);
            Ignore(x => x.Id);
        }
    }
}
