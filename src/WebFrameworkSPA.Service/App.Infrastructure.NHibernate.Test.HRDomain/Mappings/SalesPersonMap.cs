using App.Infrastructure.NHibernate.Test.HRDomain.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.HRDomain.Mappings
{
    public class SalesPersonMap : ClassMapping<SalesPerson>
    {
        public SalesPersonMap()
        {
            Id(x => x.Id, idm => idm.Generator(Generators.Identity));
            this.Property(x => x.FirstName);
            this.Property(x => x.LastName);
            this.Property(x => x.SalesQuota);
            this.Property(x => x.SalesYTD);
            this.ManyToOne(x => x.Department, mm =>
            {
                mm.Column("DepartmentId");
            });
            this.ManyToOne(
                        x => x.Territory,
                        mm =>
                        {
                            mm.Column("TerritoryId");
                        });
        }
    }
}