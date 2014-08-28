using FluentNHibernate.Mapping;
using App.Infrastructure.NHibernate.Test.HRDomain.Domain;

namespace App.Infrastructure.NHibernate.Test.HRDomain.Mappings
{
    public class SalesPersonMap : ClassMap<SalesPerson>
    {
        public SalesPersonMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Identity();
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.SalesQuota);
            Map(x => x.SalesYTD);
            References(x => x.Department)
                .Column("DepartmentId");
            References(x => x.Territory)
                .Column("TerritoryId");
        }
    }
}