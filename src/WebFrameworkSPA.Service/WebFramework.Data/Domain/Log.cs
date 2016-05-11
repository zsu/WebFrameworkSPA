using App.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFramework.Data.Domain
{

    public class Logs
    {
        public virtual long Id { get; set; }
        public virtual string Application { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string Thread { get; set; }
        public virtual string LogLevel { get; set; }
        public virtual string Logger { get; set; }
        public virtual string Message { get; set; }
        public virtual string ClientIP { get; set; }
        public virtual string SessionId { get; set; }
        public virtual string Host { get; set; }
    }
}
