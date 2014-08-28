using App.Common.Data;
namespace App.Infrastructure.NHibernate.Test.HRDomain.Domain
{
    public class SalesPerson:Entity<int>
    {
        //public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Department Department { get; set; }
        public virtual float SalesQuota { get; set; }
        public virtual decimal SalesYTD { get; set;}
        public virtual SalesTerritory Territory { get; set; }
    }
}