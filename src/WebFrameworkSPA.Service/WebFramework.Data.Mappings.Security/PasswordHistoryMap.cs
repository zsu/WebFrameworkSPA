using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings.Security
{
    public class PasswordHistoryMap : ClassMapping<PasswordHistory>
    {
        public PasswordHistoryMap()
        {
            this.Table("PasswordHistories");
            this.Id(x => x.Id, idm => idm.Generator(Generators.GuidComb));
            ManyToOne(x => x.User, map =>
            {
                map.Column("UserId");
                map.NotNullable(true);
                map.ForeignKey("FK_PWDHistories_UserAccount");
                map.Cascade(Cascade.None);
            });
            this.Property(x => x.Username, pm => { pm.NotNullable(true); pm.Length(100); });
            this.Property(x => x.DateChanged, pm => { pm.NotNullable(true); });
            this.Property(x => x.PasswordHash, pm => { pm.NotNullable(true); });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }
}
