using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{    
    public class StateSlcMap : ClassMapping<StateSlc> {
        
        public StateSlcMap() {
			Table("STATE_SLC");
			Lazy(true);
			Id(x => x.StateCd, map => { map.Column("STATE_CD"); map.Generator(Generators.Assigned); });
			Property(x => x.StateNm, map => { map.Column("STATE_NM"); map.NotNullable(true); });
			Property(x => x.StateDiscMatrixInd, map => { map.Column("STATE_DISC_MATRIX_IND"); map.NotNullable(true); });
			Property(x => x.FipsStateNbr, map => map.Column("FIPS_STATE_NBR"));
			Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
			Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });
			Property(x => x.TariffSectNo, map => map.Column("TARIFF_SECT_NO"));
			ManyToOne(x => x.Regions, map => 
			{
				map.Column("REGION_CD");
                //map.PropertyRef("RegionCd");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});

            //Bag(x => x.ApplReqOrgSlc, colmap =>  { colmap.Key(x => x.Column("LOCATED_IN_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ApplSlc, colmap =>  { colmap.Key(x => x.Column("INCLUDES_ENTITIES_IN_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.EligibleFacility, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.StateAreaCode, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestingOrgSlc, colmap =>  { colmap.Key(x => x.Column("MAILING_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FcrSlcHist, colmap =>  { colmap.Key(x => x.Column("CONTACT_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Form486, colmap =>  { colmap.Key(x => x.Column("CRTFCTN_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ApplSlcContactInfo, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ApplReqOrgSlc, colmap =>  { colmap.Key(x => x.Column("LOCATED_IN_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ApplSlc, colmap =>  { colmap.Key(x => x.Column("INCLUDES_ENTITIES_IN_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.EligibleFacility, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.MsaGoldsmith, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.SrvcPrvdrCrtfctn, colmap =>  { colmap.Key(x => x.Column("CONTACT_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Form486, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.MsaGoldsmith, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqSlc, colmap =>  { colmap.Key(x => x.Column("CONTACT_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.SrvcPrvdrCrtfctn, colmap =>  { colmap.Key(x => x.Column("CONTACT_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.StateAreaCode, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ApplSlcContactInfo, colmap =>  { colmap.Key(x => x.Column("STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqSlc, colmap =>  { colmap.Key(x => x.Column("CONTACT_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestingOrgSlc, colmap =>  { colmap.Key(x => x.Column("MAILING_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Form486, colmap =>  { colmap.Key(x => x.Column("CRTFCTN_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FcrSlcHist, colmap =>  { colmap.Key(x => x.Column("CONTACT_STATE_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
