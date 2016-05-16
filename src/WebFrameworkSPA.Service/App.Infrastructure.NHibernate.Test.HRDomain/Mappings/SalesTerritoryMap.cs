using App.Infrastructure.NHibernate.Test.HRDomain.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.HRDomain.Mappings
{
    public class SalesTerritoryMap : ClassMapping<SalesTerritory>
    {
        public SalesTerritoryMap()
        {
            Id(x => x.Id, idm => idm.Generator(Generators.Identity));
            this.Property(x => x.Name);
            this.Property(x => x.Description);
            this.Set(
            x => x.SalesPersons,
            spm =>
            {
                //spm.Inverse(true);
                //spm.Cascade(Cascade.All);
                spm.Key(
                    km =>
                    {
                        km.Column("TerritoryId");
                    });
                //spm.Access(Accessor.ReadOnly);
                //spm.Access(Accessor.Field);
                spm.Access(Accessor.NoSetter);
            },
            r => r.OneToMany());
        }
    }
}