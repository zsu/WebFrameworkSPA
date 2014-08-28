using BrockAllen.MembershipReboot.Nh;
using System;
using System.Linq;
namespace Service
{
    public interface IPermissionService : IService
    {
        IQueryable<Permission> Query();
        void AssignPermissionsToRole(Guid roleId, System.Collections.Generic.List<string> permission);
        void AssignPermissionsToRole(string roleName, System.Collections.Generic.List<string> permission);
        Permission CreatePermission(Permission permission);
        bool UpdatePermission(Permission newPermission);
        bool DeletePermission(Guid id);
        bool DeletePermission(string name);
        bool CanDelete(Permission item);
        bool CanDelete(string name);
        bool CanDelete(Guid id);
        bool IsRoleAssignedPermission(Guid roleId, string permissionName);
        System.Collections.Generic.List<Permission> GetAllPermissions();
        void RemoveAllPermissionsFromRole(Guid roleId);
        void RemovePermissionsFromRole(Guid roleId, System.Collections.Generic.List<string> permission);
        void RemovePermissionsFromRole(string roleName, System.Collections.Generic.List<string> permission);
        bool PermissionExists(string permissionName);
    }
}
