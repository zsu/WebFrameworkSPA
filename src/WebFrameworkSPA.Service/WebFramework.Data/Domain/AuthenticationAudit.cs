using System;

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
