using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;
using WebFramework.Data.Mappings.Log;
using WebFramework.Data.Domain;

namespace WebFramework.Data.Mappings.Log
{
    //public class ElmahLogMap : ClassMapping<ElmahLog>
    //{

    //    public ElmahLogMap()
    //    {
    //        Table("ELMAH_Error");
    //        Id(x => x.Id, map => { map.Generator(Generators.GuidComb); map.Column("ErrorId"); });
    //        Property(x => x.Application, map => { map.NotNullable(true); });
    //        Property(x => x.Host);
    //        Property(x => x.Type);
    //        Property(x => x.Source);
    //        Property(x => x.Message);
    //        Property(x => x.User, map => { map.Column("UserName"); });
    //        Property(x => x.StatusCode);
    //        Property(x => x.Time, map => { map.NotNullable(true); map.Column("TimeUtc"); });
    //        Property(x => x.Allxml);
    //    }
    //}
}
