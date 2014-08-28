using App.Common.Data;
namespace App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain
{
    public class Product : Entity<int>
    {
        public virtual int ProductID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}