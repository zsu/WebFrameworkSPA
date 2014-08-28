using App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot.Nh;
using App.Common.Data;
using App.Data;
using App.Common.InversionOfControl;
using WebFramework.Data.Domain;
using System.Web;

namespace Service
{
    public class RoleService : IRoleService
    {
        private IRepository<Role,Guid> _roleRepository;
        private IRepository<NhUserAccount,Guid> _userRepository;
        private IActivityLogService _activityLogService;
        private enum ActivityType {AddRole,DeleteRole,UpdateRole,AddUserToRole,RemoveUserFromRole};
        public RoleService(IRepository<Role, Guid> roleRepository, IRepository<NhUserAccount, Guid> userRepository, IActivityLogService activityLogService)
        {
            _roleRepository = roleRepository;
            _userRepository=userRepository;
            _activityLogService = activityLogService;
        }
        public IQueryable<Role> Query()
        {
            return _roleRepository.Query;
        }
        public virtual Role GetById(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentNullException("Id");
            return _roleRepository.GetById(id);
        }
        public virtual void AddUsersToRoles(List<Guid> userId, List<string> roleName)
        {
            if (userId == null || userId.Count == 0 || roleName == null || roleName.Count == 0)
                return;
            StringBuilder message=new StringBuilder();
            var users = _userRepository.Query.Where(x => userId.Contains(x.ID)).ToList();
            var roles = _roleRepository.Query.Where(x => roleName.Contains(x.Name)).ToList();
            foreach (var userEntity in users)
            {
                if (userEntity.Roles != null && userEntity.Roles.Any())
                {
                    var newRoles = roles.Except(userEntity.Roles);

                    foreach (var role in newRoles)
                        userEntity.Roles.Add(role);
                    if(newRoles!=null && newRoles.Count()>0)
                        message.AppendFormat("User {0} is added to role(s) {1}.",userEntity.Username,string.Join(",", newRoles.Select(x => x.Name)));
                }
                else
                {
                    foreach (var role in roles)
                        userEntity.Roles.Add(role);
                    if(roles!=null && roles.Count()>0)
                        message.AppendFormat("User {0} is added to role(s) {1}.",userEntity.Username,string.Join(",", roles.Select(x => x.Name)));
                }
                using (var scope = new UnitOfWorkScope())
                {
                    _userRepository.Update(userEntity);
                    scope.Commit();
                }
            }
            foreach (var uid in userId)
            {
                if (!users.Any(u => u.ID == uid))
                {
                    var user = _userRepository.Query.Where(x => x.ID == uid).SingleOrDefault();
                    if (user != null)
                    {
                        user.Roles = roles;
                        using (var scope = new UnitOfWorkScope())
                        {
                            _userRepository.Update(user);
                            scope.Commit();
                        }
                        if(roles!=null && roles.Count()>0)
                            message.AppendFormat("User {0} is added to role(s) {1}.",user.Username,string.Join(",", roles.Select(x => x.Name)));
                    }
                }
            }
            if (message.Length > 0)
            {
                ActivityLog item = new ActivityLog(ActivityType.AddUserToRole.ToString(), message.ToString());
                _activityLogService.Add(item);
            }
        }

        public virtual void CreateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("Role");
            using (var scope = new UnitOfWorkScope())
            {
                _roleRepository.Add(role);
                scope.Commit();
            }
            StringBuilder message=new StringBuilder();
            message.AppendFormat("Role {0} is created.",role.Name);
            ActivityLog item=new ActivityLog(ActivityType.AddRole.ToString(),message.ToString());
            _activityLogService.Add(item);
        }
        public virtual bool UpdateRole(Role newRole)
        {
            var role = _roleRepository.Query.Where(x => x.Id == newRole.Id).SingleOrDefault();
            role.Name = newRole.Name;
            role.Description = newRole.Description;
            using (var scope = new UnitOfWorkScope())
            {
                _roleRepository.Update(role);
                scope.Commit();
            }
            StringBuilder message=new StringBuilder();
            message.AppendFormat("Role {0} is updated.",role.Name);
            ActivityLog item=new ActivityLog(ActivityType.UpdateRole.ToString(),message.ToString());
            _activityLogService.Add(item);

            return true;
        }
        public virtual bool DeleteRole(Guid id)
        {
            var role = _roleRepository.Query.Where(x => x.Id == id).SingleOrDefault();
            var users = _userRepository.Query.Where(x => x.Roles.Any(r => r.Id == id));
            if (users.Any())
            {
                throw new Exception(string.Format("Role {0} is being used and cannot be deleted.", role.Name));
            } 
            using (var scope = new UnitOfWorkScope())
            {
                _roleRepository.Delete(role);
                scope.Commit();
            }
            StringBuilder message=new StringBuilder();
            message.AppendFormat("Role {0} is deleted.",role.Name);
            ActivityLog item=new ActivityLog(ActivityType.DeleteRole.ToString(),message.ToString());
            _activityLogService.Add(item);

            return true;
        }
        public virtual bool DeleteRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return true;
            roleName = roleName.Trim();
            var users = _userRepository.Query.Where(x => x.Roles.Any(r => r.Name == roleName));
            var role = _roleRepository.Query.Where(x => x.Name==roleName).SingleOrDefault();
            if (users.Any())
            {
                throw new Exception(string.Format("Role {0} is being used and cannot be deleted.", roleName));
            }
            using (var scope = new UnitOfWorkScope())
            {
                _roleRepository.Delete(role);
                scope.Commit();
            }
            StringBuilder message=new StringBuilder();
            message.AppendFormat("Role {0} is deleted.",role.Name);
            ActivityLog item=new ActivityLog(ActivityType.DeleteRole.ToString(),message.ToString());
            _activityLogService.Add(item);

            return true;
        }

        public virtual List<Role> GetAllRoles()
        {
            var roles = _roleRepository.Query;
            return roles.ToList();
        }

        public virtual List<Role> GetRolesForUser(Guid userId)
        {
            var user = _userRepository.Query.Where(x => x.ID == userId).SingleOrDefault();
            if (user == null || user.Roles == null || user.Roles.Count == 0)
                return new List<Role>();
            return user.Roles.ToList<Role>();
        }
        public virtual List<Guid> GetUsersInRole(Guid roldId)
        {
            if (roldId == default(Guid))
                throw new Exception("RoleId cannot be emapty.");
            var role = _roleRepository.Query.Where(x => x.Id == roldId).SingleOrDefault();

            if (role == null)
            {
                throw new Exception(string.Format("Invalid roleId: {0}", roldId));
            }

            var users = _userRepository.Query.Where(x => x.Roles.Any(r => r.Id == roldId));

            if (users == null || !users.Any())
                return new List<Guid>();

            return users.Select(u => u.ID).ToList();
        }
        public virtual List<Guid> GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new Exception("Rolename cannot be emapty.");
            roleName = roleName.Trim();
            var role = _roleRepository.Query.Where(x => x.Name == roleName).SingleOrDefault();

            if (role == null)
            {
                throw new Exception(string.Format("Invalid rolename: {0}", roleName));
            }

            var users = _userRepository.Query.Where(x=>x.Roles.Any(r => r.Name == roleName));

            if (users == null || !users.Any())
                return new List<Guid>();

            return users.Select(u => u.ID).ToList();
        }

        public virtual bool IsUserInRole(Guid userId, string roleName)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("UserId");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("RoleName");
            int count=_userRepository.Query.Where(x => x.ID == userId && x.Roles.Any(r => r.Name == roleName.Trim())).Count();
            return count>0?true:false;
        }

        public virtual void RemoveUsersFromRoles(List<Guid> userId, List<string> roleName)
        {
            if (userId == null || roleName == null || userId.Count == 0 || roleName.Count == 0)
                return;
            var users = _userRepository.Query.Where(x => userId.Contains(x.ID)).ToList();
            var roles = _roleRepository.Query.Where(x => roleName.Contains(x.Name)).ToList();
            RemoveUsersFromRoles(users, roles);
        }
        public virtual void RemoveUsersFromRoles(List<Guid> userId, List<Guid> roleId)
        {
            if (userId == null || roleId == null || userId.Count == 0 || roleId.Count == 0)
                return;
            var users = _userRepository.Query.Where(x => userId.Contains(x.ID)).ToList();
            var roles = _roleRepository.Query.Where(x => roleId.Contains(x.Id)).ToList();
            RemoveUsersFromRoles(users, roles);
        }

        public virtual bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("RoleName");
            int count=_roleRepository.Query.Where(x => x.Name == roleName.Trim()).Count();
            return count > 0 ? true : false;
        }

        public virtual IQueryable<UserRole> GetAllUserRoles()
        {
            var repository=IoC.GetService<App.Common.Data.IRepository<UserRole,Guid>>();
            var userRoles = repository.Query;
            return userRoles;
        }
        private void RemoveUsersFromRoles(List<NhUserAccount> users, List<Role> roles)
        {
            if (users == null || users.Count == 0 || roles == null || roles.Count == 0)
                return;
            StringBuilder message=new StringBuilder();
            using (var scope = new UnitOfWorkScope())
            {
                foreach (var userEntity in users)
                {
                    if (userEntity.Roles != null && userEntity.Roles.Any())
                    {
                        int oldCount = userEntity.Roles.Count;
                        var matchedRoles = roles.Intersect(userEntity.Roles);
                        message.AppendFormat("Role(s) {0} are removed from user {1}.", string.Join(",", matchedRoles.Select(x => x.Name)), userEntity.Username);

                        foreach (var matchedRole in matchedRoles)
                            userEntity.Roles.Remove(matchedRole);

                        if (oldCount != userEntity.Roles.Count)
                        {
                            _userRepository.Update(userEntity);
                        }
                    }
                }
                scope.Commit();
            }
            if (message.Length > 0)
            {
                ActivityLog item = new ActivityLog(ActivityType.RemoveUserFromRole.ToString(), message.ToString());
                _activityLogService.Add(item);
            }

        }
    }
}
