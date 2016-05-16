
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain.Mappings
{
	public class CustomerMap : ClassMapping<Customer>
	{
		public CustomerMap()
		{
			Table("Customers");
            Id(x => x.CustomerID, idm => idm.Generator(Generators.Identity));
			this.Property(x => x.FirstName);
			this.Property(x => x.LastName);
			Component(x => x.Address, component =>
			{
				component.Property(x => x.StreetAddress1);
				component.Property(x => x.StreetAddress2);
				component.Property(x => x.City);
				component.Property(x => x.State);
				component.Property(x => x.ZipCode);
			});
            this.Set(
            x => x.Orders,
            spm =>
            {
                            spm.Inverse(true);
                            spm.Key(
                    km =>
                    {
                        km.Column("CustomerId");
                        km.ForeignKey("FK_Customer_Orders");
                    });
            },
            r => r.OneToMany());
        }
	}
}