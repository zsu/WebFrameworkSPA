using App.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFramework.Data.Domain
{

    public class AuthenticationAudit
    {
        public virtual Guid Id { get; set; }
        public virtual string Application { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string Activity { get; set; }
        public virtual string Detail { get; set; }
        public virtual string ClientIP { get; set; }
    }
}
