using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain.Mappings
{
	public class ProductMap : ClassMapping<Product>
	{
		public ProductMap()
		{
			Table("Products");
            Id(x => x.ProductID, idm => idm.Generator(Generators.Identity));
            Property(x => x.Name);
			Property(x => x.Description);
		}
	}
}