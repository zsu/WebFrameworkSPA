using App.Infrastructure.NHibernate.Test.HRDomain.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.HRDomain.Mappings
{
    public class DepartmentMap : ClassMapping<Department>
    {
        public DepartmentMap()
        {
            Table("Departments");
            Id(x => x.Id,idm => idm.Generator(Generators.Identity));
            this.Property(x => x.Name);
        }
    }
}