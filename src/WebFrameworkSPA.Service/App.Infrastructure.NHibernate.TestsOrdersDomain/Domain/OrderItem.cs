namespace App.Infrastructure.NHibernate.Test.OrdersDomain
{
    public class OrderItem
    {
        public virtual int OrderItemID { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }
		public virtual string Store { get; set; }
        public virtual decimal TotalPrice
        {
            get { return Quantity * Price; }
        }
    }
}