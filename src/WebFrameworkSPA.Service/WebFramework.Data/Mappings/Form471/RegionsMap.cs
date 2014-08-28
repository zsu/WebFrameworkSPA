using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{
    public class RegionsMap : ClassMapping<Regions> {
        
        public RegionsMap() {
			Lazy(true);
			Id(x => x.RegionCd, map => { map.Column("REGION_CD"); map.Generator(Generators.Assigned); });
			Property(x => x.RegionDesc, map => map.Column("REGION_DESC"));
			Bag(x => x.StateSlc, colmap =>  { colmap.Key(x => x.Column("REGION_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
