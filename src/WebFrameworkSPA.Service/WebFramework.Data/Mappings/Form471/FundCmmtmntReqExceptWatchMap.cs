using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{
    
    
    public class FundCmmtmntReqExceptWatchMap : ClassMapping<FundCmmtmntReqExceptWatch> {
        
        public FundCmmtmntReqExceptWatchMap() {
			Table("FUND_CMMTMNT_REQ_EXCEPT_WATCH");
			Lazy(true);
            Id(x => x.FundReqId, map => { map.Column("FUND_REQ_ID"); map.Generator(Generators.Assigned); });
            //ComposedId(compId =>
            //    {
            //        compId.Property(x => x.FundReqId, m => m.Column("FUND_REQ_ID"));
            //        compId.Property(x => x.ExceptionCd, m => m.Column("EXCEPTION_CD"));
            //        compId.Property(x => x.FundYear, m => m.Column("FUND_YEAR"));
            //        compId.Property(x => x.EffectiveFromDt, m => m.Column("EFFECTIVE_FROM_DT"));
            //    });
			Property(x => x.EffectiveToDt, map => map.Column("EFFECTIVE_TO_DT"));
			Property(x => x.Comments);
			Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
			Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });
            //ManyToOne(x => x.ExceptionFundYear, map => map.Columns(new Action<IColumnMapper>[] { x => x.Name("EXCEPTION_CD"), x => x.Name("FUND_YEAR") }));
        }
    }
}
