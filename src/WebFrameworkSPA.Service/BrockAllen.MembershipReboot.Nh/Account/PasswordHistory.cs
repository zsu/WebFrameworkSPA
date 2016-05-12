using App.Common.Data;
using System;
using System.ComponentModel.DataAnnotations;

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
