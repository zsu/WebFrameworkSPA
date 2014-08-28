using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using App.Common.Tasks;

namespace WebFramework.Data.Mappings
{
    public class ScheduleTaskMap : ClassMapping<ScheduleTask>
    {
        public ScheduleTaskMap()
        {
            this.Table("ScheduleTasks");
            Lazy(true);
            Id(x => x.Id, map => map.Generator(Generators.GuidComb));
            Property(x => x.Name, map => map.NotNullable(true));
            Property(x => x.Seconds, map => map.NotNullable(true));
            Property(x => x.Type, map => map.NotNullable(true));
            Property(x => x.Enabled, map => map.NotNullable(true));
            Property(x => x.StopOnError, map => map.NotNullable(true));
            Property(x => x.LastStartUtc);
            Property(x => x.LastEndUtc);
            Property(x => x.LastSuccessUtc);
        }
    }
}
