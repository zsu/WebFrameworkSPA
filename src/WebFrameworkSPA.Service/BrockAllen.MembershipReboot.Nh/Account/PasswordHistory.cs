using App.Common.Data;
using BrockAllen.MembershipReboot.Nh;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockAllen.MembershipReboot.Nh
{
    public class PasswordHistory:Entity<Guid>
    {
        public virtual NhUserAccount User { get; set; }
        [Required]
        [MaxLength(100)]
        public virtual string Username { get; set; }
        public virtual DateTime DateChanged { get; set; }
        [Required]
        public virtual string PasswordHash { get; set; }
    }
}
