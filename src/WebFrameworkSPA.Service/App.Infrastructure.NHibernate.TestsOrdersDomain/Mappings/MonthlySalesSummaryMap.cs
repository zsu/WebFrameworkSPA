using FluentNHibernate.Mapping;
using App.Infrastructure.NHibernate.Test.Domain;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain.Mappings
{
	public class MonthlySalesSummaryMap : ClassMap<MonthlySalesSummary>
	{
		public MonthlySalesSummaryMap()
		{
			CompositeId()
				.KeyProperty(x => x.Year)
				.KeyProperty(x => x.Month)
				.KeyProperty(x => x.SalesPersonId);
			Map(x => x.SalesPersonFirstName);
			Map(x => x.SalesPersonLastName);
			Component(x => x.TotalSale, component =>
			{
				component.Map(x => x.Amount);
				component.Map(x => x.Currency);
			});
		}
	}
}