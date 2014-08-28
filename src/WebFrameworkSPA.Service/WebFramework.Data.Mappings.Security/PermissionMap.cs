using BrockAllen.MembershipReboot.Nh;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace WebFramework.Data.Mappings.Security
{
    public class PermissionMap : ClassMapping<Permission>
    {
        public PermissionMap()
        {
            this.Table("Permissions");
            this.Id(x => x.Id, idm => idm.Generator(Generators.GuidComb));
            this.Property(x => x.Name, pm => { pm.NotNullable(true); pm.Length(100); });
            this.Property(x => x.Description, pm => pm.Length(150));
            Set(x => x.Roles, collectionMapping =>
            {
                collectionMapping.Table("RolePermissions");
                collectionMapping.Key(key => key.Column("PermissionId"));
                collectionMapping.Cascade(Cascade.None);
            },
                    map => map.ManyToMany(p => p.Column("RoleId")));
            this.Version(
            x => x.Version,
            vm =>
            {
                vm.Generated(VersionGeneration.Never);
                vm.Type(new Int64Type());
            });
            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }
}
