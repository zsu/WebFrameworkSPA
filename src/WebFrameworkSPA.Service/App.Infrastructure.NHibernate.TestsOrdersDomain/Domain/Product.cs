using App.Common.Data;
namespace App.Infrastructure.NHibernate.Test.OrdersDomain
{
    public class Product:Entity<int>
    {
        public virtual int ProductID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}