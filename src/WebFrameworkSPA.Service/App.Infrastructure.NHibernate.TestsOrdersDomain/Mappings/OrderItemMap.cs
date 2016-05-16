using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace App.Infrastructure.NHibernate.Test.OrdersDomain.Mappings
{
    public class OrderItemMap : ClassMapping<OrderItem>
    {
        public OrderItemMap()
        {
            Table("OrderItems");
            Id(x => x.OrderItemID, idm => idm.Generator(Generators.Identity));
            Property(x => x.Price);
            Property(x => x.Quantity);
            Property(x => x.Store);
            ManyToOne(x => x.Product,
                mm =>
                {
                    mm.Column("ProductId");
                    mm.Cascade(Cascade.Persist);
                    mm.ForeignKey("FK_OrderItems_Product");
                });
            ManyToOne(x => x.Order,
            mm =>
            {
                mm.Column("OrderId");
                mm.ForeignKey("FK_OrderItems_Order");
            });
        }
    }
}