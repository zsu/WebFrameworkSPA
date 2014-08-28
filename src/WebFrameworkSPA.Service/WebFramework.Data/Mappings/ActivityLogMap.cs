using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;
using WebFramework.Data.Domain;

namespace WebFramework.Data.Mappings
{
    public class ActivityLogMap : ClassMapping<ActivityLog>
    {
        public ActivityLogMap()
        {
            this.Table("ActivityLogs");
            Lazy(true);
            this.Id(x => x.Id, idm => idm.Generator(Generators.GuidComb));
            this.Property(x => x.Application, pm => { pm.NotNullable(true); pm.Length(50); pm.Index("IX_ActivityLogs_Application"); });
            this.Property(x => x.UserName, pm => { pm.NotNullable(false); pm.Length(100); pm.Index("IX_ActivityLogs_UserName"); });
            this.Property(x => x.CreatedDate, pm => { pm.NotNullable(true); pm.Index("IX_ActivityLogs_CreatedDate"); });
            this.Property(x => x.Activity, pm => { pm.NotNullable(true); });
            this.Property(x => x.Detail);
            this.Property(x => x.ClientIP);

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }
}
