using FluentNHibernate.Mapping;
using App.Infrastructure.NHibernate.Test.HRDomain.Domain;

namespace App.Infrastructure.NHibernate.Test.HRDomain.Mappings
{
    public class SalesTerritoryMap : ClassMap<SalesTerritory>
    {
        public SalesTerritoryMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Description);
            HasMany(x => x.SalesPersons)
                .AsSet()
                .Access.ReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore)
                .KeyColumn("TerritoryId");
        }
    }
}