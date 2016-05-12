using WebFramework.Data.Domain;
using App.Common.Data;
using App.Data;
using BrockAllen.MembershipReboot.Nh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class PermissionService : IPermissionService
    {
        private IRepository<Role,Guid> _roleRepository;
        private IRepository<Permission,Guid> _permissionRepository;
        private IActivityLogService _activityLogService;
        private enum ActivityType { AddPermission, DeletePermission, UpdatePermission, AddPermissionToRole, RemovePermissionFromRole };
        public PermissionService(IRepository<Permission, Guid> permissionRepository, 
            IRepository<Role,Guid> roleRepository,
            IActivityLogService activityLogService)
        {
            _roleRepository = roleRepository;
            _permissionRepository=permissionRepository;
            _activityLogService = activityLogService;
        }

        #region public
        public IQueryable<Permission> Query()
        {
            return _permissionRepository.Query;
        }
        public virtual List<Permission> GetAllPermissions()
        {

            return _permissionRepository.Query.ToList();
        }

        public virtual Permission CreatePermission(Permission permission)
        {
            Permission matchedPermission = _permissionRepository.Query.FirstOrDefault(x => x.Name == permission.Name);
            if (matchedPermission != null)
                throw new Exception(string.Format("Permission {0} already exist.", permission.Name));
            using (var scope = new UnitOfWorkScope())
            {
                _permissionRepository.Add(permission);
                scope.Commit();
            }
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Permission {0} is created.", permission.Name);
            ActivityLog item = new ActivityLog(ActivityType.AddPermission.ToString(), message.ToString());
            _activityLogService.Add(item); 
            return permission;
        }
        public virtual bool UpdatePermission(Permission newPermission)
        {
            var permission = _permissionRepository.Query.Where(x => x.Id == newPermission.Id).SingleOrDefault();
            permission.Name = newPermission.Name;
            permission.Description = newPermission.Description;
            using (var scope = new UnitOfWorkScope())
            {
                _permissionRepository.Update(permission);
                scope.Commit();
            }
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Permission {0} is updated.", permission.Name);
            ActivityLog item = new ActivityLog(ActivityType.UpdatePermission.ToString(), message.ToString());
            _activityLogService.Add(item);
            return true;
        }
        public virtual bool DeletePermission(Guid id)
        {
            if (id==default(Guid))
                throw new Exception("Permission id cannot be empty.");
            Permission permission = _permissionRepository.Query.FirstOrDefault(x => x.Id == id);
            var rolesWithPermissionCount = _roleRepository.Query.Where(x => x.Permissions.Any(y => y.Id == id)).Count();
            if (rolesWithPermissionCount > 0)
                throw new Exception(string.Format("Permission {0} is being used and cannot be deleted.", permission.Name));
            if (permission != null && rolesWithPermissionCount == 0)
            {
                using (var scope = new UnitOfWorkScope())
                {
                    _permissionRepository.Delete(permission);
                    scope.Commit();
                }
                StringBuilder message = new StringBuilder();
                message.AppendFormat("Permission {0} is deleted.", permission.Name);
                ActivityLog item = new ActivityLog(ActivityType.DeletePermission.ToString(), message.ToString());
                _activityLogService.Add(item);
                return true;
            }
            return false;
        }
        public virtual bool DeletePermission(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Permission name cannot be empty.");
            name = name.Trim();
            Permission permission = _permissionRepository.Query.FirstOrDefault(x => x.Name == name);
            var rolesWithPermissionCount = _roleRepository.Query.Where(x => x.Permissions.Any(y => y.Id == permission.Id)).Count();
            if (rolesWithPermissionCount > 0)
                throw new Exception(string.Format("Permission {0} is being used and cannot be deleted.", name));
            if (permission != null && rolesWithPermissionCount == 0)
            {
                using (var scope = new UnitOfWorkScope())
                {
                    _permissionRepository.Delete(permission);
                    scope.Commit();
                }
                StringBuilder message = new StringBuilder();
                message.AppendFormat("Permission {0} is deleted.", permission.Name);
                ActivityLog item = new ActivityLog(ActivityType.DeletePermission.ToString(), message.ToString());
                _activityLogService.Add(item); 
                return true;
            }
            return false;
        }
        public virtual bool CanDelete(Permission item)
        {
            if (item == null)
                throw new ArgumentNullException("Permission");
            if (item.Id == default(Guid))
                throw new Exception("Permission id cannot be empty.");
            bool canDelete = false;
            var rolesWithPermissionCount = _roleRepository.Query.Where(x => x.Permissions.Any(y => y.Id == item.Id)).Count();
            if (rolesWithPermissionCount <= 0)
                canDelete = true;
            return canDelete;
        }
        public virtual bool CanDelete(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name");
            name = name.Trim();
            var permission = _permissionRepository.Query.Where(x => x.Name==name).SingleOrDefault();
            if (permission == null)
                throw new Exception(string.Format("Permission {0} doesnot exist.", name));
            return CanDelete(permission);
        }
        public virtual bool CanDelete(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentNullException("Id");
            var permission = _permissionRepository.Query.Where(x => x.Id == id).SingleOrDefault();
            if (permission == null)
                throw new Exception(string.Format("Permission {0} doesnot exist.", id));
            return CanDelete(permission);
        }
        public virtual bool IsRoleAssignedPermission(Guid roleId, string permissionName)
        {
            if (roleId == Guid.Empty)
                throw new ArgumentNullException("RoleId");
            if (string.IsNullOrEmpty(permissionName))
                throw new ArgumentNullException("PermissionName");
            int count = _roleRepository.Query.Where(x => x.Id == roleId && x.Permissions.Any(r => r.Name == permissionName.Trim())).Count();
            return count > 0 ? true : false;
        }
        public virtual void AssignPermissionsToRole(string roleName, List<string> permission)
        {
            if (string.IsNullOrEmpty(roleName) || permission == null || permission.Count == 0)
                return;
            roleName = roleName.Trim();

            var role = _roleRepository.Query.FirstOrDefault(x => x.Name == roleName);
            var permissions = (from p in _permissionRepository.Query
                               where permission.Contains(p.Name)
                               select p).ToList();


            AddPermissionsToRole(permissions, role, (array, id) => { array.Add(id); return array; });

        }

        public virtual void AssignPermissionsToRole(Guid roleId, List<string> permission)
        {
            if (permission == null || permission.Count == 0)
                return;

            var role = _roleRepository.Query.FirstOrDefault(x => x.Id == roleId);
            var permissions = (from p in _permissionRepository.Query
                               where permission.Contains(p.Name)
                               select p).ToList();


            AddPermissionsToRole(permissions, role, (array, id) => { array.Add(id); return array; });
        }

        public virtual void RemovePermissionsFromRole(string roleName, List<string> permission)
        {
            if (string.IsNullOrEmpty(roleName) || permission == null || permission.Count == 0)
                return;
            roleName = roleName.Trim();

            var role = _roleRepository.Query.FirstOrDefault(x => x.Name == roleName);
            var permissions = (from p in _permissionRepository.Query
                               where permission.Contains(p.Name)
                               select p).ToList();


            RemovePermissionsFromRole(permissions, role, (array, id) => { array.Remove(id); return array; });
        }

        public virtual void RemovePermissionsFromRole(Guid roleId, List<string> permission)
        {
            if (permission == null || permission.Count == 0)
                return;
            var role = _roleRepository.Query.FirstOrDefault(x => x.Id == roleId);
            var permissions = (from p in _permissionRepository.Query
                               where permission.Contains(p.Name)
                               select p).ToList();


            RemovePermissionsFromRole(permissions, role, (array, id) => { array.Remove(id); return array; });
        }

        public virtual void RemoveAllPermissionsFromRole(Guid roleId)
        {
            StringBuilder message = new StringBuilder();
            var role = _roleRepository.Query.FirstOrDefault(x => x.Id == roleId);
            if (role.Permissions != null && role.Permissions.Count > 0)
            {
                message.AppendFormat("Permission(s) {0} are removed from role {1}.", string.Join(",", role.Permissions.Select(x => x.Name)), role.Name);
                role.Permissions.Clear();
            }
            using (var scope = new UnitOfWorkScope())
            {
                _roleRepository.Update(role);
                scope.Commit();
            }
            if (message.Length>0)
            {
                ActivityLog item = new ActivityLog(ActivityType.RemovePermissionFromRole.ToString(), message.ToString());
                _activityLogService.Add(item);
            }
        }

        public virtual bool PermissionExists(string permissionName)
        {
            if (string.IsNullOrEmpty(permissionName))
                throw new ArgumentNullException("PermissionName");
            int count = _permissionRepository.Query.Where(x => x.Name == permissionName.Trim()).Count();
            return count > 0 ? true : false;
        }

        #endregion


        #region private

        protected void AddPermissionsToRole(List<Permission> permissions, Role role, Func<ICollection<Permission>, Permission, ICollection<Permission>> changePermission)
        {
            if (permissions == null || permissions.Count == 0 || role == null)
                return;
            StringBuilder message = new StringBuilder(), items = new StringBuilder(); ;
            foreach (var perm in permissions)
            {
                if (perm != null)
                {
                    if (!role.Permissions.Contains<Permission>(perm))
                    {
                        changePermission(role.Permissions, perm);
                        items.AppendFormat("{0},",perm.Name);
                    }
                }
            }
            using (var scope = new UnitOfWorkScope())
            {
                _roleRepository.Update(role);
                scope.Commit();
            }
            if(items.Length>0)
            {
                items = items.Remove(items.Length - 1, 1);
                message.AppendFormat("Permission(s) {0} are added to role {1}.", items.ToString(),role.Name);
                ActivityLog item = new ActivityLog(ActivityType.AddPermissionToRole.ToString(), message.ToString());
                _activityLogService.Add(item);
            }
        }
        protected void RemovePermissionsFromRole(List<Permission> permissions, Role role, Func<ICollection<Permission>, Permission, ICollection<Permission>> changePermission)
        {
            if (permissions == null || permissions.Count == 0 || role == null)
                return;
            StringBuilder message = new StringBuilder(), items = new StringBuilder(); ;
            foreach (var perm in permissions)
            {
                if (perm != null)
                {
                    if (role.Permissions.Contains<Permission>(perm))
                    {
                        changePermission(role.Permissions, perm);
                        items.AppendFormat("{0},", perm.Name);
                    }
                }
            }
            using (var scope = new UnitOfWorkScope())
            {
                _roleRepository.Update(role);
                scope.Commit();
            }
            if (items.Length > 0)
            {
                items = items.Remove(items.Length - 1, 1);
                message.AppendFormat("Permission(s) {0} are removed from role {1}.", items.ToString(), role.Name);
                ActivityLog item = new ActivityLog(ActivityType.RemovePermissionFromRole.ToString(), message.ToString());
                _activityLogService.Add(item);
            }
        }
        #endregion
    }
}
