using WebFramework.Data.Domain;
using App.Common;
using App.Common.InversionOfControl;
using App.Common.Security;
using App.Data;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Nh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        private App.Common.Data.IRepository<NhUserAccount,Guid> _userRepository;
        private UserAccountService<NhUserAccount> _userAccountService;
        private IActivityLogService _activityLogService;
        private MembershipRebootConfiguration<NhUserAccount> _membershipConfiguration;
        private enum ActivityType { AddUser, DeleteUser, UpdateUser};
        public UserService(App.Common.Data.IRepository<NhUserAccount, Guid> userRepository, UserAccountService<NhUserAccount> userAccountService,
            MembershipRebootConfiguration<NhUserAccount> membershipConfiguration,IActivityLogService activityLogService)
        {
            _userRepository = userRepository;
            _userAccountService = userAccountService;
            _activityLogService = activityLogService;
            _membershipConfiguration = membershipConfiguration;
        }
        public virtual IQueryable<NhUserAccount> Query()
        {
            return _userRepository.Query;//.FindAll();
        }
        public virtual NhUserAccount GetById(Guid id)
        {
            if (id==default(Guid))
                throw new ArgumentNullException("Id");
            return _userRepository.GetById(id);//.Get<Guid>(id);
        }
        public virtual NhUserAccount FindById(Guid id)
        {
            if (id==default(Guid))
                throw new ArgumentNullException("Id");
            return _userRepository.Query.SingleOrDefault(x => x.ID == id);
        }
        public virtual NhUserAccount FindBy(Expression<Func<NhUserAccount, bool>> expression)
        {
            return _userRepository.Query.FirstOrDefault(expression);
        }
        public virtual void Update(NhUserAccount user)
        {
            if (user == null)
                throw new ArgumentNullException("User cannot be empty.");
            if(string.IsNullOrEmpty(user.Username))
                throw new Exception("Username cannot be empty.");
            if(string.IsNullOrEmpty(user.Email))
                throw new Exception("User email cannot be empty.");
            if (_membershipConfiguration.UsernamesUniqueAcrossTenants)
            {
                var account = _userRepository.Query.SingleOrDefault(x => x.Username == user.Username);
                if (account != null && account.ID != user.ID)
                {
                    throw new Exception(string.Format("Username {0} is not available.", user.Username));;
                }
            }
            else
            {
                var account = _userRepository.Query.SingleOrDefault(x => x.Username == user.Username);
                if (account != null && account.ID != user.ID)
                {
                    throw new Exception(string.Format("Username {0} is not available.", user.Username)); ;
                }
            }
            using (var scope = new UnitOfWorkScope())
            {
                _userRepository.Update(user);
                scope.Commit();
            }
        }
        public virtual void Save(NhUserAccount user)
        {
            if (user == null)
                throw new ArgumentNullException("User cannot be empty.");
            if (string.IsNullOrEmpty(user.Username))
                throw new Exception("Username cannot be empty.");
            if (string.IsNullOrEmpty(user.Email))
                throw new Exception("User email cannot be empty.");
            if (_membershipConfiguration.UsernamesUniqueAcrossTenants)
            {
                var account = _userRepository.Query.SingleOrDefault(x => x.Username == user.Username);
                if (account != null && account.ID != user.ID)
                {
                    throw new Exception(string.Format("Username {0} is not available.", user.Username)); ;
                }
            }
            else
            {
                var account = _userRepository.Query.SingleOrDefault(x => x.Username == user.Username);
                if (account!=null && (user.ID==default(Guid) || account.ID != user.ID))
                {
                    throw new Exception(string.Format("Username {0} is not available.", user.Username)); ;
                }
            }
            using (var scope = new UnitOfWorkScope())
            {
                _userRepository.Add(user);//.Save(user);
                scope.Commit();
            }
            StringBuilder message = new StringBuilder();
            message.AppendFormat("User {0} is created.", user.Username);
            ActivityLog activityItem = new ActivityLog(ActivityType.AddUser.ToString(), message.ToString());
            _activityLogService.Add(activityItem);
        }
        public virtual bool Delete(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentNullException("User id cannot be empty.");
            NhUserAccount user = _userRepository.Query.SingleOrDefault(x => x.ID == id);
            if(user==null)
                throw new Exception(string.Format("Cannot find user {0}",id));
            App.Common.Data.IRepository<WebFramework.Data.Domain.PasswordHistory,Guid> passwordRepository = IoC.GetService<App.Common.Data.IRepository<WebFramework.Data.Domain.PasswordHistory,Guid>>();
            List<WebFramework.Data.Domain.PasswordHistory> passwordHistories = passwordRepository.Query.Where(x => x.User.ID == id).ToList();
            using (var scope = new UnitOfWorkScope())
            {
                foreach (WebFramework.Data.Domain.PasswordHistory item in passwordHistories)
                {
                    passwordRepository.Delete(item);
                }
                _userRepository.Delete(user);
                scope.Commit();
            }
            StringBuilder message = new StringBuilder();
            message.AppendFormat("User {0} is deleted.", user.Username);
            ActivityLog activityItem = new ActivityLog(ActivityType.DeleteUser.ToString(), message.ToString());
            _activityLogService.Add(activityItem);
            return true;
        }

        public virtual NhUserAccount CreateAccountWithTempPassword(NhUserAccount item)
        {
            if (item == null)
                throw new Exception("User cannot be empty.");
            if (string.IsNullOrEmpty(item.Username))
                throw new Exception("Username cannot be empty.");
            if (string.IsNullOrEmpty(item.Email))
               throw new Exception("User email cannot be empty.");
            item.Username = item.Username.Trim();
            item.Email = string.IsNullOrEmpty(item.Email) ? null : item.Email.Trim();
            item.FirstName = string.IsNullOrEmpty(item.FirstName) ? null : item.FirstName.Trim();
            item.LastName = string.IsNullOrEmpty(item.LastName) ? null : item.LastName.Trim();
            if (string.IsNullOrEmpty(item.HashedPassword))
            {
                var passwordGenerator = IoC.GetService<IPasswordGenerator>();
                item.HashedPassword = passwordGenerator.GeneratePassword();
            }
            if (_membershipConfiguration.UsernamesUniqueAcrossTenants)
            {
                var account = _userRepository.Query.SingleOrDefault(x => x.Username == item.Username);
                if (account != null)
                {
                    throw new Exception(string.Format("Username {0} is not available.", item.Username)); ;
                }
            }
            else
            {
                var account = _userRepository.Query.SingleOrDefault(x => x.Tenant == item.Tenant && x.Username == item.Username);
                if (account != null)
                {
                    throw new Exception(string.Format("Username {0} is not available.", item.Username)); ;
                }
            }
            NhUserAccount newAccount;
            using (var scope = new UnitOfWorkScope())
            {
                newAccount = _userAccountService.CreateAccountWithTempPassword(item.Tenant, item.Username, item.HashedPassword, item.Email, item.FirstName, item.LastName);
                scope.Commit();
            }
            using (var scope = new UnitOfWorkScope())
            {
                _userAccountService.SetRequiresPasswordReset(newAccount.ID, true);
                scope.Commit();
                return newAccount;
            }
        }
        public virtual NhUserAccount CreateAccount(NhUserAccount item)
        {
            NhUserAccount newAccount;
            using (var scope = new UnitOfWorkScope())
            {
                newAccount = _userAccountService.CreateAccount(item.Username, item.HashedPassword, item.Email, item.FirstName, item.LastName);
                scope.Commit();
            }
            return newAccount;
        }
    }
}
