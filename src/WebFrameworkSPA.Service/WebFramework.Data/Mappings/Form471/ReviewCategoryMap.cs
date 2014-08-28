using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{


    public class ReviewCategoryMap : ClassMapping<ReviewCategory>
    {

        public ReviewCategoryMap()
        {
            Table("REVIEW_CATEGORY");
            Lazy(true);
            Id(x => x.ReviewCategoryCd, map => { map.Column("REVIEW_CATEGORY_CD"); map.Generator(Generators.Assigned); });
            Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
            Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
            Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
            Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });
        }
    }
}
