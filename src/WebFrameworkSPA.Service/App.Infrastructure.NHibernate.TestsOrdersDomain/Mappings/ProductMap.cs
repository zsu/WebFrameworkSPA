using FluentNHibernate.Mapping;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain.Mappings
{
	public class ProductMap : ClassMap<Product>
	{
		public ProductMap()
		{
			Table("Products");
			Id(x => x.ProductID)
				.GeneratedBy.Identity();
			Map(x => x.Name);
			Map(x => x.Description);
		}
	}
}