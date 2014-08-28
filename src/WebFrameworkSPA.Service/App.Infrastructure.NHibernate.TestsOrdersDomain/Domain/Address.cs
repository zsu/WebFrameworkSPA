namespace App.Infrastructure.NHibernate.Test.OrdersDomain
{
    public class Address
    {
        public virtual string StreetAddress1 { get; set; }
        public virtual string StreetAddress2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
    }
}