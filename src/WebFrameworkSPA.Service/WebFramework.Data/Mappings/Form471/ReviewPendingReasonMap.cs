using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{
    
    
    public class ReviewPendingReasonMap : ClassMapping<ReviewPendingReason> {
        
        public ReviewPendingReasonMap() {
			Table("REVIEW_PENDING_REASON");
			Lazy(true);
			Id(x => x.PendingReasonCd, map => { map.Column("PENDING_REASON_CD"); map.Generator(Generators.Assigned); });
			Property(x => x.PendingReasonDesc, map => { map.Column("PENDING_REASON_DESC"); map.NotNullable(true); });
			Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
			Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });
			Property(x => x.IssueTypeDesc, map => map.Column("ISSUE_TYPE_DESC"));
			Bag(x => x.FundReqPendingReason, colmap =>  { colmap.Key(x => x.Column("PENDING_REASON_CD")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
