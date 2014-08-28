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
    
    
    public class FundCmmtmntReqReqOrgMap : ClassMapping<FundCmmtmntReqReqOrg> {
        
        public FundCmmtmntReqReqOrgMap() {
			Table("FUND_CMMTMNT_REQ_REQ_ORG");
			Lazy(true);
			ComposedId(compId =>
				{
					compId.Property(x => x.FundReqId, m => m.Column("FUND_REQ_ID"));
                    //compId.Property(x => x.FundCmmtmntReqGrpSeqNo, m => m.Column("FUND_CMMTMNT_REQ_GRP_SEQ_NO"));
					compId.Property(x => x.BusPartyId, m => m.Column("BUS_PARTY_ID"));
				});
            Property(x => x.ReqOrgNm, map => { map.Column("REQ_ORG_NM"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString);});
			Property(x => x.BusPartyIdNearestSchool, map => {map.Column("BUS_PARTY_ID_NEAREST_SCHOOL");});
            Property(x => x.NcesStateNbr, map => { map.Column("NCES_STATE_NBR"); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.NcesDistrictNbr, map => { map.Column("NCES_DISTRICT_NBR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.NcesBuildingNbr, map => {map.Column("NCES_BUILDING_NBR"); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.RuralInd, map => { map.Column("RURAL_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.StudentCt, map => map.Column("STUDENT_CT"));
			Property(x => x.NearestNcesStateNbr, map =>{ map.Column("NEAREST_NCES_STATE_NBR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.NearestNcesDistrictNbr, map => {map.Column("NEAREST_NCES_DISTRICT_NBR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.NearestNcesBuildingNbr, map => {map.Column("NEAREST_NCES_BUILDING_NBR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.StudentNslpCt, map => map.Column("STUDENT_NSLP_CT"));
            Property(x => x.SharedServiceInd, map => { map.Column("SHARED_SERVICE_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.DiscountPct, map => {map.Column("DISCOUNT_PCT"); map.Type(NHibernateUtil.Decimal); });
            Property(x => x.BusPartyCatg, map => { map.Column("BUS_PARTY_CATG"); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
            Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
			Property(x => x.NonMatrixDiscountInd, map => {map.Column("NON_MATRIX_DISCOUNT_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.PrekJuvenileInd, map => {map.Column("PREK_JUVENILE_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.AltDiscMechInd, map => {map.Column("ALT_DISC_MECH_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.DeCompleteInd, map => {map.Column("DE_COMPLETE_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypePrekInd, map => {map.Column("ENT_SUBTYPE_PREK_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeJuvenileInd, map =>{ map.Column("ENT_SUBTYPE_JUVENILE_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeHeadStartInd, map => {map.Column("ENT_SUBTYPE_HEAD_START_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeEsaInd, map => {map.Column("ENT_SUBTYPE_ESA_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeDormitoryInd, map => {map.Column("ENT_SUBTYPE_DORMITORY_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeNonInstfacInd, map => {map.Column("ENT_SUBTYPE_NON_INSTFAC_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeAdultEdInd, map => {map.Column("ENT_SUBTYPE_ADULT_ED_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeNewschConstInd, map => {map.Column("ENT_SUBTYPE_NEWSCH_CONST_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeAdminEntityInd, map => {map.Column("ENT_SUBTYPE_ADMIN_ENTITY_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.EntSubtypeNewlibConstInd, map => {map.Column("ENT_SUBTYPE_NEWLIB_CONST_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.SchoolDistrictName, map => {map.Column("SCHOOL_DISTRICT_NAME"); map.Type(NHibernateUtil.AnsiString); });
			ManyToOne(x => x.FundCmmtmntReqSlc, map => 
			{
				map.Column("FUND_REQ_ID");
				map.PropertyRef("FundReqId");
				map.Cascade(Cascade.None);
			});

            //ManyToOne(x => x.FundCmmtmntReqGrp, map => map.Columns(new Action<IColumnMapper>[] { x => x.Name("FUND_CMMTMNT_REQ_GRP_SEQ_NO"), x => x.Name("XREF_FC_REQ_GRP_SEQ_NO") }));
            //ManyToOne(x => x.RequestingOrgSlc, map => map.Columns(new Action<IColumnMapper>[] { x => x.Name("BUS_PARTY_ID"), x => x.Name("BUS_PARTY_ID_NEAREST_DISTRICT") }));
        }
    }
}
