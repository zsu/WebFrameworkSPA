using System;

namespace Web.Areas.Admin.Models
{
    public class RolePermissionModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasPermission { get; set; }
    }
}