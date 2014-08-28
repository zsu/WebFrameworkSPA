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
    }
}
