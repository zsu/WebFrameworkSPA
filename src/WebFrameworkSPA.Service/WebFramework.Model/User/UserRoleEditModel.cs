using BrockAllen.MembershipReboot.Nh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Model
{
    public class UserRoleEditModel
    {
        IList<UserRoleModel> _roles = new List<UserRoleModel>();
        public NhUserAccount User { get; set; }
        public IList<UserRoleModel> Roles { get { return _roles; } set { _roles = value; } }
    }
}