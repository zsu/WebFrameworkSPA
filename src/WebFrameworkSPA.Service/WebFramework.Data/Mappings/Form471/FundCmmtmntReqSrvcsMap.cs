using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;
using NHibernate;


namespace WebFramework.Data.Mappings
{
    
    
    public class FundCmmtmntReqSrvcsMap : ClassMapping<FundCmmtmntReqSrvcs> {
        
        public FundCmmtmntReqSrvcsMap() {
			Table("FUND_CMMTMNT_REQ_SRVCS");
			Lazy(true);
			ComposedId(compId =>
				{
					compId.Property(x => x.FundReqId, m => m.Column("FUND_REQ_ID"));
                    compId.Property(x => x.ServiceDtlId, m => m.Column("SERVICE_DTL_ID"));
				});
			Property(x => x.ExistingSrvcCt, map => map.Column("EXISTING_SRVC_CT"));
			Property(x => x.TotalSrvcCt, map => map.Column("TOTAL_SRVC_CT"));
			Property(x => x.ExistingSrvcSpeed, map => {map.Column("EXISTING_SRVC_SPEED"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.TotalSrvcSpeed, map => {map.Column("TOTAL_SRVC_SPEED"); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
            Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
			Property(x => x.ExistingSrvcText, map => {map.Column("EXISTING_SRVC_TEXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.TotalSrvcText, map => {map.Column("TOTAL_SRVC_TEXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.LbrTotalSrvcCt, map => map.Column("LBR_TOTAL_SRVC_CT"));
			ManyToOne(x => x.FundCmmtmntReqSlc, map => 
			{
				map.Column("FUND_REQ_ID");
				map.PropertyRef("FundReqId");
				map.Cascade(Cascade.None);
			});

            //ManyToOne(x => x.ServiceSlcs, map => 
            //{
            //    map.Column("SERVICE_DTL_ID");
            //    map.PropertyRef("ServiceDtlId");
            //    map.Cascade(Cascade.None);
            //});

        }
    }
}
