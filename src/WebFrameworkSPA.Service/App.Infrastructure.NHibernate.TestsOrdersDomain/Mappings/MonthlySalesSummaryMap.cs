using App.Infrastructure.NHibernate.Test.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain.Mappings
{
	public class MonthlySalesSummaryMap : ClassMapping<MonthlySalesSummary>
	{
        public MonthlySalesSummaryMap()
        {
            ComposedId(map =>
               {
                   map.Property(x => x.Year);
                   map.Property(x => x.Month);
                   map.Property(x => x.SalesPersonId);
               });
            Property(x => x.SalesPersonFirstName);
            Property(x => x.SalesPersonLastName);
            Component(x => x.TotalSale, component =>
            {
                component.Property(x => x.Amount);
                component.Property(x => x.Currency);
            });
        }
    }
}