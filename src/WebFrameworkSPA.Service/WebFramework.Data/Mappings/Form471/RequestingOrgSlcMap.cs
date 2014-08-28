using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{     
    public class RequestingOrgSlcMap : ClassMapping<RequestingOrgSlc> {
        
        public RequestingOrgSlcMap() {
			Table("REQUESTING_ORG_SLC");
			Lazy(true);
			Id(x => x.BusPartyId, map => { map.Column("BUS_PARTY_ID"); map.Generator(Generators.Assigned); });
			Property(x => x.BusPartyName, map => { map.Column("BUS_PARTY_NAME"); map.NotNullable(true); });
			Property(x => x.UnapprovedInd, map => { map.Column("UNAPPROVED_IND"); map.NotNullable(true); });
			Property(x => x.SlcBusPartyType, map => map.Column("SLC_BUS_PARTY_TYPE"));
			Property(x => x.RuralInd, map => map.Column("RURAL_IND"));
			Property(x => x.BusPartyPrimStreetAddr1, map => map.Column("BUS_PARTY_PRIM_STREET_ADDR1"));
			Property(x => x.BusPartyPrimStreetAddr2, map => map.Column("BUS_PARTY_PRIM_STREET_ADDR2"));
			Property(x => x.BusPartyPrimCntyNm, map => map.Column("BUS_PARTY_PRIM_CNTY_NM"));
			Property(x => x.BusPartyPrimZip5Cd, map => map.Column("BUS_PARTY_PRIM_ZIP5_CD"));
			Property(x => x.BusPartyPrimZip4Cd, map => map.Column("BUS_PARTY_PRIM_ZIP4_CD"));
			Property(x => x.BusPartyPrimZip2Cd, map => map.Column("BUS_PARTY_PRIM_ZIP2_CD"));
			Property(x => x.SecondaryGeoAddrInd, map => map.Column("SECONDARY_GEO_ADDR_IND"));
			Property(x => x.QedPin, map => map.Column("QED_PIN"));
			Property(x => x.NcesStateNbr, map => map.Column("NCES_STATE_NBR"));
			Property(x => x.FipsCountyNbr, map => map.Column("FIPS_COUNTY_NBR"));
			Property(x => x.NcesDistrictNbr, map => map.Column("NCES_DISTRICT_NBR"));
			Property(x => x.NcesBuildingNbr, map => map.Column("NCES_BUILDING_NBR"));
			Property(x => x.FipsCountyNm, map => map.Column("FIPS_COUNTY_NM"));
			Property(x => x.MailingAddr1, map => map.Column("MAILING_ADDR1"));
			Property(x => x.MailingCity, map => map.Column("MAILING_CITY"));
			Property(x => x.MailingZip5Cd, map => map.Column("MAILING_ZIP5_CD"));
			Property(x => x.MailingZip4Cd, map => map.Column("MAILING_ZIP4_CD"));
			Property(x => x.PhoneAreaCd, map => map.Column("PHONE_AREA_CD"));
			Property(x => x.PhoneNo, map => map.Column("PHONE_NO"));
			Property(x => x.PhoneExt, map => map.Column("PHONE_EXT"));
			Property(x => x.FaxAreaCd, map => map.Column("FAX_AREA_CD"));
			Property(x => x.FaxPhoneNo, map => map.Column("FAX_PHONE_NO"));
			Property(x => x.FaxExt, map => map.Column("FAX_EXT"));
			Property(x => x.QedMsa, map => map.Column("QED_MSA"));
			Property(x => x.QedMetroStatus, map => map.Column("QED_METRO_STATUS"));
			Property(x => x.GradeSpanCd, map => map.Column("GRADE_SPAN_CD"));
			Property(x => x.OrshanskyPctOfUniv, map => map.Column("ORSHANSKY_PCT_OF_UNIV"));
			Property(x => x.StudentComputerRatioCd, map => map.Column("STUDENT_COMPUTER_RATIO_CD"));
			Property(x => x.OnLineService, map => map.Column("ON_LINE_SERVICE"));
			Property(x => x.HomePageUrl, map => map.Column("HOME_PAGE_URL"));
			Property(x => x.TechCoordEmailAddr, map => map.Column("TECH_COORD_EMAIL_ADDR"));
			Property(x => x.TtlNbrStudents, map => map.Column("TTL_NBR_STUDENTS"));
			Property(x => x.TtlNbrBldings, map => map.Column("TTL_NBR_BLDINGS"));
			Property(x => x.TtlNbrRooms, map => map.Column("TTL_NBR_ROOMS"));
			Property(x => x.TtlNbrStudentsElgblFslp, map => map.Column("TTL_NBR_STUDENTS_ELGBL_FSLP"));
			Property(x => x.TtlNbrStudentsLastYr, map => map.Column("TTL_NBR_STUDENTS_LAST_YR"));
			Property(x => x.TtlNbrVolumes, map => map.Column("TTL_NBR_VOLUMES"));
			Property(x => x.ChapterIStudents, map => map.Column("CHAPTER_I_STUDENTS"));
			Property(x => x.ChapterIPct, map => map.Column("CHAPTER_I_PCT"));
			Property(x => x.PkUseNbrLibrPatrons, map => map.Column("PK_USE_NBR_LIBR_PATRONS"));
			Property(x => x.PopulationCd, map => map.Column("POPULATION_CD"));
			Property(x => x.BusPartyIdNearestSchlDist, map => map.Column("BUS_PARTY_ID_NEAREST_SCHL_DIST"));
			Property(x => x.BusPartyCatg, map => map.Column("BUS_PARTY_CATG"));
			Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
			Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });
			Property(x => x.RecordChngdInd, map => map.Column("RECORD_CHNGD_IND"));
			Property(x => x.LockId, map => map.Column("LOCK_ID"));
			Property(x => x.AllowedInBlock1Ind, map => map.Column("ALLOWED_IN_BLOCK1_IND"));
			Property(x => x.InvReqReviewInd, map => map.Column("INV_REQ_REVIEW_IND"));
			Property(x => x.InvEditRuleId, map => map.Column("INV_EDIT_RULE_ID"));
			Property(x => x.EligDiscountPct, map => map.Column("ELIG_DISCOUNT_PCT"));
			Property(x => x.PiaValidationInd, map => map.Column("PIA_VALIDATION_IND"));
			Property(x => x.StatusCd, map => map.Column("STATUS_CD"));
			Property(x => x.DuplicateOfBusPartyId, map => map.Column("DUPLICATE_OF_BUS_PARTY_ID"));
			Property(x => x.CipaReviewCommentsTxt, map => map.Column("CIPA_REVIEW_COMMENTS_TXT"));
			Property(x => x.EntityDiscountVerifiedInd, map => map.Column("ENTITY_DISCOUNT_VERIFIED_IND"));
			Property(x => x.Fccrn);
            ManyToOne(x => x.StateSlc, map => { map.Column("MAILING_STATE_CD"); map.Cascade(Cascade.None); });
			ManyToOne(x => x.BusPartyClassifications, map => 
			{
				map.Column("BUS_PARTY_CLASSIFICATION");
                //map.PropertyRef("BusPartyClassification");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});

            //Bag(x => x.ApplReqOrgSlc, colmap =>  { colmap.Key(x => x.Column("REQ_BY_BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ApplSlc, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.BenReviewerAssignment, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.CipaEntityYearInfo, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ContactAlternateInfo, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.CipaEntityYearInfo, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Form486, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AsFundCmmtmntReqSrvcPrvdr, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.InvHist, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FccrnFileDetails, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqGrp, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.ApplSlcEntityAreaGrp, colmap =>  { colmap.Key(x => x.Column("REQ_BY_BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqReqOrg, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID_NEAREST_DISTRICT")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Invoice, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqSlc, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Appeals, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AppealsInAccess, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AppealBenReviewerAssignment, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AppealDummyFrn, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqSrvcPrvdr, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.EntitySubtype, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.EntityStatusHist, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqReqOrg, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID_NEAREST_DISTRICT")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqSlc, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqSrvcPrvdr, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Invoice, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.Form486, colmap =>  { colmap.Key(x => x.Column("BUS_PARTY_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
