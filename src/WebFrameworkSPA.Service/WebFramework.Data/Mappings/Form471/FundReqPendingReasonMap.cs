using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{
    
    
    public class FundReqPendingReasonMap : ClassMapping<FundReqPendingReason> {
        
        public FundReqPendingReasonMap() {
			Table("FUND_REQ_PENDING_REASON");
			Lazy(true);
			Id(x => x.PendingReasonId, map => { map.Column("PENDING_REASON_ID"); map.Generator(Generators.Assigned); });
			Property(x => x.PendingStartDt, map => { map.Column("PENDING_START_DT"); map.NotNullable(true); });
			Property(x => x.PendingEndDt, map => map.Column("PENDING_END_DT"));
			Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
			Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });
			ManyToOne(x => x.FundCmmtmntReqSlc, map => 
			{
				map.Column("FUND_REQ_ID");
                //map.PropertyRef("FundReqId");
				map.Cascade(Cascade.None);
			});

            ManyToOne(x => x.ReviewCategory, map =>
            {
                map.Column("REVIEW_CATEGORY_CD");
                //map.PropertyRef("ReviewCategoryCd");
                map.Cascade(Cascade.None);
            });

            ManyToOne(x => x.ReviewPendingReason, map =>
            {
                map.Column("PENDING_REASON_CD");
                //map.PropertyRef("PendingReasonCd");
                map.Cascade(Cascade.None);
            });
            //ManyToOne(x => x.SwPerson, map => 
            //{
            //    map.Column("SWPERSONID");
            //    map.PropertyRef("Swpersonid");
            //    map.Cascade(Cascade.None);
            //});

        }
    }
}
