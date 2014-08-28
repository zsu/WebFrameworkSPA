using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{
    
    
    public class FundReqReviewAssignmentMap : ClassMapping<FundReqReviewAssignment> {
        
        public FundReqReviewAssignmentMap() {
			Table("FUND_REQ_REVIEW_ASSIGNMENT");
			Lazy(true);
            Id(x => x.FundReqId, map => { map.Column("FUND_REQ_ID"); map.Generator(Generators.Assigned); });
            //ComposedId(compId =>
            //    {
            //        compId.Property(x => x.FundReqId, m => m.Column("FUND_REQ_ID"));
            //        compId.Property(x => x.ReviewCategoryCd, m => m.Column("REVIEW_CATEGORY_CD"));
            //        compId.Property(x => x.ReviewLevelCd, m => m.Column("REVIEW_LEVEL_CD"));
            //    });
			Property(x => x.AssignedDt, map => { map.Column("ASSIGNED_DT"); map.NotNullable(true); });
			Property(x => x.CompletedDt, map => map.Column("COMPLETED_DT"));
			Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
			Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });

            Property(x => x.ReviewCategoryCd, m => m.Column("REVIEW_CATEGORY_CD"));
            Property(x => x.ReviewLevelCd, m => m.Column("REVIEW_LEVEL_CD"));
            ManyToOne(x => x.FundCmmtmntReqSlc, map =>
            {
                map.Column("FUND_REQ_ID");
                map.Cascade(Cascade.None);
            });

            //ManyToOne(x => x.ReviewType, map => map.Columns(new Action<IColumnMapper>[] { x => x.Name("REVIEW_CATEGORY_CD"), x => x.Name("REVIEW_LEVEL_CD") }));
            //ManyToOne(x => x.SwPerson, map => 
            //{
            //    map.Column("SWPERSONID");
            //    map.PropertyRef("Swpersonid");
            //    map.Cascade(Cascade.None);
            //});

        }
    }
}
