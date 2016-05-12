using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;

namespace WebFramework.Data.Mappings
{
    public class MessageTemplatesMap : ClassMapping<MessageTemplate>
    {

        public MessageTemplatesMap()
        {
            this.Table("MessageTemplates");
            Lazy(true);
            Id(x => x.Id, map => map.Generator(Generators.GuidComb));
            Property(x => x.Name, map => map.NotNullable(true));
            Property(x => x.BccEmailAddresses);
            Property(x => x.Subject);
            Property(x => x.Body);
            Property(x => x.IsActive, map => map.NotNullable(true));
        }
    }
}
