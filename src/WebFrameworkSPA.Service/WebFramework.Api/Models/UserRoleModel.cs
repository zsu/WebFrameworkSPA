using System;

namespace Web.Areas.Admin.Models
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