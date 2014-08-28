using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockAllen.MembershipReboot
{
    public class PasswordHistory
    {
        //public virtual Guid UserId { get; set; }
        public virtual DateTime DateChanged { get; set; }
        [Required]
        public virtual string PasswordHash { get; set; }
    }
}
