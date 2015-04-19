﻿namespace BrockAllen.MembershipReboot.Nh.Repository
{
    using System;
    using System.Linq;

    using BrockAllen.MembershipReboot;
    using App.Common.Data;
using App.Data;

    public class NhUserAccountRepository<TAccount> : QueryableUserAccountRepository<TAccount>
        where TAccount : NhUserAccount
    {
        private readonly IRepository<TAccount, Guid> accountRepository;

        public NhUserAccountRepository(IRepository<TAccount, Guid> accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        protected override IQueryable<TAccount> Queryable
        {
            get
            {
                return this.accountRepository.Query;//.FindAll();
            }
        }

        public override TAccount Create()
        {
            var account = Activator.CreateInstance<TAccount>();
            return account;
        }

        public override void Add(TAccount item)
        {

            using (var scope = new UnitOfWorkScope())
            {
                this.accountRepository.Add(item);//.Save(item);
                scope.Commit();
            }

        }

        public override void Remove(TAccount item)
        {
            using (var scope = new UnitOfWorkScope())
            {
                this.accountRepository.Delete(item);
                scope.Commit();
            }
        }

        public override void Update(TAccount item)
        {
            using (var scope = new UnitOfWorkScope())
            {
                this.accountRepository.Update(item);
                scope.Commit();
            }
        }

        public override TAccount GetByLinkedAccount(string tenant, string provider, string id)
        {
            var accounts = from a in this.accountRepository.Query//.FindAll()
                           where a.Tenant == tenant
                           from la in a.LinkedAccountsCollection
                           where la.ProviderName == provider && la.ProviderAccountID == id
                           select a;

            return accounts.SingleOrDefault();
        }

        public override TAccount GetByCertificate(string tenant, string thumbprint)
        {
            var accounts =
                from a in this.accountRepository.Query//.FindAll()
                where a.Tenant == tenant
                from c in a.CertificatesCollection
                where c.Thumbprint == thumbprint
                select a;
            return accounts.SingleOrDefault();
        }
        //public override void Refresh(TAccount item)
        //{
        //    accountRepository.Refresh(item);
        //}
    }

    public class NhUserAccountRepository : NhUserAccountRepository<NhUserAccount>
    {
        public NhUserAccountRepository(App.Common.Data.IRepository<NhUserAccount, Guid> accountRepository)
            : base(accountRepository)
        {
        }
    }
}