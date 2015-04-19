using BrockAllen.MembershipReboot.Nh;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace Service
{
    public interface IUserService:IService
    {
        IQueryable<NhUserAccount> Query();
        NhUserAccount GetById(Guid id);
        NhUserAccount FindById(Guid id);
        NhUserAccount FindBy(Expression<Func<NhUserAccount, bool>> expression);
        void Update(NhUserAccount user);
        void Save(NhUserAccount user);
        bool Delete(Guid id);
        NhUserAccount CreateAccountWithTempPassword(NhUserAccount item);
        NhUserAccount CreateAccount(NhUserAccount item);
        void ResetPassword(Guid id);
        void ResetPassword(string email);
        void ChangePassword(Guid id, string oldPassword, string newPassword);
        bool ChangePasswordFromResetKey(string key, string newPassword, out NhUserAccount account);
        void VerifyEmailFromKey(string key, string password, out NhUserAccount account);
        void CancelVerification(string key, out bool accountClosed);
    }
}
