using BrockAllen.MembershipReboot.Nh;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace WebFramework.Data.Mappings.Security
{
    public class RoleMap : ClassMapping<Role>
    {
        public RoleMap()
        {
            this.Table("Roles");
            this.Id(x => x.Id, idm => idm.Generator(Generators.GuidComb));
            this.Property(x => x.Name, pm => { pm.NotNullable(true); pm.Length(100); });
            this.Property(x => x.Description, pm => pm.Length(150));
            Set(x => x.Permissions, collectionMapping =>
            {
                collectionMapping.Table("RolePermissions");
                collectionMapping.Key(k =>
                {
                    k.Column("RoleId");
                    k.ForeignKey("FK_RolePermissions_Roles");
                });
                collectionMapping.Cascade(Cascade.None);
            },
            map => map.ManyToMany(p =>
            {
                p.Column("PermissionId");
                p.ForeignKey("FK_RolePermissions_Permissions");
            }));
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
