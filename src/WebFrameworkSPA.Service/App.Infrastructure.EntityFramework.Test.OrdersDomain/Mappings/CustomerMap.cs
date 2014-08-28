using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Mappings
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            ToTable("Customers");
            HasKey(c => c.CustomerID);
            Property(c=>c.CustomerID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(c => c.FirstName);
            Property(c => c.LastName);
            Property(c => c.StreetAddress1);
            Property(x => x.StreetAddress1);
            Property(x => x.StreetAddress2);
            Property(x => x.City);
            Property(x => x.State);
            Property(x => x.ZipCode);
            Ignore(x => x.Id);
        }
    }
}
