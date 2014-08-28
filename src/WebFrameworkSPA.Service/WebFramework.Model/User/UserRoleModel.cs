using BrockAllen.MembershipReboot.Nh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Model
{
    public class UserRoleModel
    {
        //public Role Role { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasRole { get; set; }
    }
}