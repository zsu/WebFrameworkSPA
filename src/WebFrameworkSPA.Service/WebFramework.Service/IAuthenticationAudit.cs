using System;
using System.Collections.Generic;
using System.Linq;
using WebFramework.Data.Domain;
namespace Service
{
    public interface IAuthenticationAuditService:IService
    {
        IQueryable<AuthenticationAudit> Query();
        //bool Exists(string name);
        void Add(AuthenticationAudit item);
        bool Delete(Guid id);
        bool Delete(AuthenticationAudit item);
        List<AuthenticationAudit> GetAll();
        AuthenticationAudit GetById(Guid id);
        void Update(AuthenticationAudit item);
    }
}
