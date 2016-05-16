using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain.Mappings
{
    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            Id(x => x.OrderID, idm => idm.Generator(Generators.Identity));
            Property(x => x.OrderDate);
            Property(x => x.ShipDate);
            ManyToOne(x => x.Customer, mm =>
            {
                mm.Column("CustomerId");
                mm.ForeignKey("FK_Orders_Customer");
            });
            Set(
                x => x.Items,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    spm.Key(
                    km =>
                    {
                        km.Column("OrderId");
                        km.ForeignKey("FK_Orders_OrderItems");
                    });
                }, r => r.OneToMany());
        }
    }
}