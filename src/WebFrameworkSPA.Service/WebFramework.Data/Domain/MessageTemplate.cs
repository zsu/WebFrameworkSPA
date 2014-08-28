using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace WebFramework.Data.Domain
{
    public class MessageTemplate {
        public virtual Guid Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual string BccEmailAddresses { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        [Required]
        public virtual bool IsActive { get; set; }
    }
}
