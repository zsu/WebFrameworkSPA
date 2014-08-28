using FluentNHibernate.Mapping;
using App.Infrastructure.NHibernate.Test.HRDomain.Domain;

namespace App.Infrastructure.NHibernate.Test.HRDomain.Mappings
{
    public class DepartmentMap : ClassMap<Department>
    {
        public DepartmentMap()
        {
            Table("Departments");
            Id(x => x.Id)
                .GeneratedBy.Identity();
            Map(x => x.Name);
        }
    }
}