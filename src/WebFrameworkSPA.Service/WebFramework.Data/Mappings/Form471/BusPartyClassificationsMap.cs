using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{      
    public class BusPartyClassificationsMap : ClassMapping<BusPartyClassifications> {
        
        public BusPartyClassificationsMap() {
			Table("BUS_PARTY_CLASSIFICATIONS");
			Lazy(true);
			Id(x => x.BusPartyClassification, map => { map.Column("BUS_PARTY_CLASSIFICATION"); map.Generator(Generators.Assigned); });
            //Bag(x => x.EligibleFacility, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_CLASSIFICATION")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestingOrgSlc, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_CLASSIFICATION")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.EligibleFacility, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_CLASSIFICATION")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestingOrgSlc, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_CLASSIFICATION")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
