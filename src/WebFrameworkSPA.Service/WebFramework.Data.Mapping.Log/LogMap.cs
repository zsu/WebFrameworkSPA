using WebFramework.Data.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFramework.Data.Mapping.Log
{
    public class LogMap : ClassMapping<Logs>
    {
        public LogMap()
        {
            this.Table("Logs");
            this.Id(x => x.Id, idm => idm.Generator(Generators.Native, g => g.Params(new
            {
                sequence = "SQ_Logs_Id"
            })));
            this.Property(x => x.Application, pm => { pm.NotNullable(true); pm.Length(50); pm.Index("IX_Logs_Application"); });
            this.Property(x => x.UserName, pm => { pm.NotNullable(false); pm.Length(100); pm.Index("IX_Logs_User"); });
            this.Property(x => x.CreatedDate, pm => { pm.NotNullable(true); pm.Index("IX_Logs_CreatedDate"); });
            this.Property(x => x.Thread, pm => { pm.NotNullable(true); });
            this.Property(x => x.LogLevel, pm => { pm.NotNullable(true); pm.Index("IX_Logs_Level"); });
            this.Property(x => x.Logger, pm => { pm.NotNullable(true); });
            this.Property(x => x.Message, pm => { pm.NotNullable(true); pm.Length(2000); });
            this.Property(x => x.ClientIP, pm => { pm.NotNullable(false); pm.Index("IX_Logs_ClientIP"); });
            this.Property(x => x.SessionId, pm => { pm.NotNullable(false); pm.Index("IX_Logs_SessionId"); });
            this.Property(x => x.Host);

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }
}
