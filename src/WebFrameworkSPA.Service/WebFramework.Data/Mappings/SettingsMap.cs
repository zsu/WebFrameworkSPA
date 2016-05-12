using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;

namespace WebFramework.Data.Mappings
{
    public class SettingsMap : ClassMapping<Setting>
    {
        public SettingsMap()
        {
            this.Table("Settings");
            Lazy(true);
            Id(x => x.Id, map => map.Generator(Generators.GuidComb));
            Property(x => x.Name, map => map.NotNullable(true));
            Property(x => x.Value, map => map.NotNullable(true));
        }
    }
}
