using BrockAllen.MembershipReboot.Nh;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Service
{
    public interface IRoleService : IService
    {
        IQueryable<Role> Query();
        Role GetById(Guid id);
        void AddUsersToRoles(List<Guid> userId, List<string> roleName);
        void CreateRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(string roleName);
        bool DeleteRole(Guid roleId);
        List<Role> GetAllRoles();
        List<Role> GetRolesForUser(Guid userId);
        List<Guid> GetUsersInRole(Guid roleId);
        List<Guid> GetUsersInRole(string roleName);
        bool IsUserInRole(Guid userId, string roleName);
        void RemoveUsersFromRoles(List<Guid> userId, List<Guid> roleId);
        void RemoveUsersFromRoles(List<Guid> userId, List<string> roleName);
        bool RoleExists(string roleName);
        IQueryable<UserRole> GetAllUserRoles();
    }
}
