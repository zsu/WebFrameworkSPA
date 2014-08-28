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
    
    
    public class FundCmmtmntReqSlcMap : ClassMapping<FundCmmtmntReqSlc> {
        
        public FundCmmtmntReqSlcMap() {
			Table("FUND_CMMTMNT_REQ_SLC");
			Lazy(true);
			Id(x => x.FundReqId, map => 
				{
					map.Column("FUND_REQ_ID");
                    map.Generator(Generators.Native, g => g.Params(new { sequence = "FCAPPLID_SEQ" }));
				});
            Property(x => x.FundReqDt, map => { map.Column("FUND_REQ_DT"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
            Property(x => x.ApplNm, map => { map.Column("APPL_NM"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.FundReqReceivedDt, map => {map.Column("FUND_REQ_RECEIVED_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FundReqCompleteDt, map => {map.Column("FUND_REQ_COMPLETE_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FundReqCanceledDt, map => {map.Column("FUND_REQ_CANCELED_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FundReqCrtfctnDt, map => {map.Column("FUND_REQ_CRTFCTN_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FundReqReceiptNtfctnDt, map => {map.Column("FUND_REQ_RECEIPT_NTFCTN_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FundReqCmmtmntStatusDt, map =>{ map.Column("FUND_REQ_CMMTMNT_STATUS_DT"); map.Type(NHibernateUtil.Date); });
            Property(x => x.FundReqCmmtmntStatusCd, map => { map.Column("FUND_REQ_CMMTMNT_STATUS_CD"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.AuthorizedToSubmitInd, map => {map.Column("AUTHORIZED_TO_SUBMIT_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplStreetAddr1, map => {map.Column("APPL_STREET_ADDR1"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplStreetAddr2, map => {map.Column("APPL_STREET_ADDR2"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplCityNm, map => {map.Column("APPL_CITY_NM"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplZip5Cd, map => {map.Column("APPL_ZIP5_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplZip4Cd, map => {map.Column("APPL_ZIP4_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplZip2Cd, map => {map.Column("APPL_ZIP2_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplAreaCd, map => {map.Column("APPL_AREA_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplPhoneNo, map => {map.Column("APPL_PHONE_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplPhoneExt, map => {map.Column("APPL_PHONE_EXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplEmailAddr, map => {map.Column("APPL_EMAIL_ADDR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.BusPartyCatg, map => {map.Column("BUS_PARTY_CATG"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactNm, map => {map.Column("CONTACT_NM"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactStreetAddr, map =>{map.Column("CONTACT_STREET_ADDR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactStreetAddr2, map => {map.Column("CONTACT_STREET_ADDR2"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactCityNm, map => {map.Column("CONTACT_CITY_NM"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactZip5Cd, map => {map.Column("CONTACT_ZIP5_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactZip4Cd, map => {map.Column("CONTACT_ZIP4_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactZip2Cd, map => {map.Column("CONTACT_ZIP2_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactAreaCd, map => {map.Column("CONTACT_AREA_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactPhoneNo, map => {map.Column("CONTACT_PHONE_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactPhoneExt, map => {map.Column("CONTACT_PHONE_EXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactFaxAreaCd, map => {map.Column("CONTACT_FAX_AREA_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactFaxNo, map => {map.Column("CONTACT_FAX_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactFaxExt, map => {map.Column("CONTACT_FAX_EXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactEmailAddr, map => {map.Column("CONTACT_EMAIL_ADDR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactPreferredMode, map => {map.Column("CONTACT_PREFERRED_MODE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnDt, map => {map.Column("CRTFCTN_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.CrtfctnSignedInd, map => {map.Column("CRTFCTN_SIGNED_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnRecvdDt, map => {map.Column("CRTFCTN_RECVD_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.CrtfctnNm, map => {map.Column("CRTFCTN_NM"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnTitle, map => {map.Column("CRTFCTN_TITLE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CmmtmntAmt, map => {map.Column("CMMTMNT_AMT");  });
			Property(x => x.CmmtmntNtfctnDt, map => {map.Column("CMMTMNT_NTFCTN_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.IncIneligOrgsInd, map => {map.Column("INC_INELIG_ORGS_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.RegionOfStateInd, map => {map.Column("REGION_OF_STATE_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.StatewideInd, map => {map.Column("STATEWIDE_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.MultiStateInd, map => {map.Column("MULTI_STATE_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.StateEdAgncyInd, map => {map.Column("STATE_ED_AGNCY_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.LocalEdAgncyInd, map => {map.Column("LOCAL_ED_AGNCY_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.EdSrvcAgncyInd, map => {map.Column("ED_SRVC_AGNCY_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ExtngCntrctInd, map => {map.Column("EXTNG_CNTRCT_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplFiled470Ind, map =>{ map.Column("APPL_FILED_470_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ReqPostedInd, map => {map.Column("REQ_POSTED_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.MinorModInd, map => {map.Column("MINOR_MOD_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.StdntCt, map => map.Column("STDNT_CT"));
			Property(x => x.PtrnCt, map => map.Column("PTRN_CT"));
			Property(x => x.BuildingsCt, map => map.Column("BUILDINGS_CT"));
			Property(x => x.RoomsCt, map => map.Column("ROOMS_CT"));
			Property(x => x.ServiceListInd, map => {map.Column("SERVICE_LIST_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ServiceListDesc, map => {map.Column("SERVICE_LIST_DESC"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.SlcCalc6MnthDscntAmt, map => {map.Column("SLC_CALC_6_MNTH_DSCNT_AMT");});
			Property(x => x.SlcCalcDscntAmt, map => {map.Column("SLC_CALC_DSCNT_AMT");});
			Property(x => x.EstNextYrPrediscntAmt, map =>{ map.Column("EST_NEXT_YR_PREDISCNT_AMT");});
			Property(x => x.BasicPhoneOnlyInd, map => {map.Column("BASIC_PHONE_ONLY_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.EligSchlInd, map => {map.Column("ELIG_SCHL_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.EligLibrInd, map => {map.Column("ELIG_LIBR_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.IndividualTechPlanInd, map => {map.Column("INDIVIDUAL_TECH_PLAN_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.HighLevelTechPlanInd, map => {map.Column("HIGH_LEVEL_TECH_PLAN_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.TechPlanApprvdInd, map => {map.Column("TECH_PLAN_APPRVD_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.TechPlanToBeStApprvdInd, map => {map.Column("TECH_PLAN_TO_BE_ST_APPRVD_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CalcTotalAmt, map => {map.Column("CALC_TOTAL_AMT");});
			Property(x => x.DenialReasonCd, map => {map.Column("DENIAL_REASON_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.TechPlanToBeSlcApprvdInd, map => {map.Column("TECH_PLAN_TO_BE_SLC_APPRVD_IND"); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.FundReqStatusCd, map => { map.Column("FUND_REQ_STATUS_CD"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.FundReqStatusPiaCd, map => { map.Column("FUND_REQ_STATUS_PIA_CD"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.FundReqStatusPiaDt, map => {map.Column("FUND_REQ_STATUS_PIA_DT"); map.Type(NHibernateUtil.Date); });
            Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
            Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
			Property(x => x.BusPartyIdSubmittedBy, map => map.Column("BUS_PARTY_ID_SUBMITTED_BY"));
			Property(x => x.LockId, map => {map.Column("LOCK_ID"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.InvReqReviewInd, map => {map.Column("INV_REQ_REVIEW_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.FundReqSecurityNo, map => { map.Column("FUND_REQ_SECURITY_NO"); map.NotNullable(true); });
			Property(x => x.DscntWorksheetReceiptInd, map => {map.Column("DSCNT_WORKSHEET_RECEIPT_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.DscntWorksheetSubmitCd, map => {map.Column("DSCNT_WORKSHEET_SUBMIT_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.SrvcListReceiptInd, map => {map.Column("SRVC_LIST_RECEIPT_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplFormCode, map => {map.Column("APPL_FORM_CODE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.FormCode, map => {map.Column("FORM_CODE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplFaxAreaCd, map => {map.Column("APPL_FAX_AREA_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplFaxNo, map => {map.Column("APPL_FAX_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplFaxExt, map => {map.Column("APPL_FAX_EXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnAreaCd, map => {map.Column("CRTFCTN_AREA_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnPhoneNo, map => {map.Column("CRTFCTN_PHONE_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnPhoneExt, map => {map.Column("CRTFCTN_PHONE_EXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContactAlternateInfoTxt, map => {map.Column("CONTACT_ALTERNATE_INFO_TXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.AttachmentReceivedDt, map => {map.Column("ATTACHMENT_RECEIVED_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.MpsFailInd, map => {map.Column("MPS_FAIL_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CrtfctnRecordId, map => map.Column("CRTFCTN_RECORD_ID"));
			Property(x => x.ApplicationHeadsDownInd, map => {map.Column("APPLICATION_HEADS_DOWN_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CoveredByNoTechPlanInd, map => {map.Column("COVERED_BY_NO_TECH_PLAN_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.StatusNoTechPlanInd, map => {map.Column("STATUS_NO_TECH_PLAN_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.SubmitIpAddr, map => {map.Column("SUBMIT_IP_ADDR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.TnTestReqd, map => {map.Column("TN_TEST_REQD"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.FundReqReceiptOowNtfctnDt, map => {map.Column("FUND_REQ_RECEIPT_OOW_NTFCTN_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.CrtfctnEmailAddr, map => {map.Column("CRTFCTN_EMAIL_ADDR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnFaxAreaCd, map => {map.Column("CRTFCTN_FAX_AREA_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnFaxNo, map => {map.Column("CRTFCTN_FAX_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnStreetAddr, map => {map.Column("CRTFCTN_STREET_ADDR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnStreetAddr2, map => {map.Column("CRTFCTN_STREET_ADDR2"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnCityNm, map => {map.Column("CRTFCTN_CITY_NM"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnStateCd, map => {map.Column("CRTFCTN_STATE_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnZip5Cd, map => {map.Column("CRTFCTN_ZIP5_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CrtfctnZip4Cd, map => {map.Column("CRTFCTN_ZIP4_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ExemptFromProcedure, map => {map.Column("EXEMPT_FROM_PROCEDURE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.MpsRejectLtrDt, map => {map.Column("MPS_REJECT_LTR_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.AppealsRevisedFcdlDt, map => {map.Column("APPEALS_REVISED_FCDL_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.CertEligForSupportInd, map => {map.Column("CERT_ELIG_FOR_SUPPORT_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertSecuredAccessInd, map => {map.Column("CERT_SECURED_ACCESS_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertIneligAllocBudgetAmt, map => {map.Column("CERT_INELIG_ALLOC_BUDGET_AMT");});
			Property(x => x.CertFundRecvdInd, map => {map.Column("CERT_FUND_RECVD_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertTechPlanInd, map => {map.Column("CERT_TECH_PLAN_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.Cert470FiledInd, map => {map.Column("CERT_470_FILED_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertEntityRevwInd, map => {map.Column("CERT_ENTITY_REVW_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertSrvcForEdInd, map => {map.Column("CERT_SRVC_FOR_ED_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertComplyProgRuleInd, map => {map.Column("CERT_COMPLY_PROG_RULE_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertDiscCondInd, map => {map.Column("CERT_DISC_COND_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertRetainDocInd, map => {map.Column("CERT_RETAIN_DOC_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertOrderSrvcInd, map => {map.Column("CERT_ORDER_SRVC_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertCrimeViolationInd, map => {map.Column("CERT_CRIME_VIOLATION_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertAllocCostInd, map => {map.Column("CERT_ALLOC_COST_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertReqIcSrvcInd, map => {map.Column("CERT_REQ_IC_SRVC_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CertNonDiscCostPdSpInd, map => {map.Column("CERT_NON_DISC_COST_PD_SP_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.CrtfctnEmployerNm, map => {map.Column("CRTFCTN_EMPLOYER_NM"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.NsiqaRevwInd, map => {map.Column("NSIQA_REVW_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.SldqaRevwInd, map => {map.Column("SLDQA_REVW_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.No471CertWarnLtrDt, map => {map.Column("NO_471_CERT_WARN_LTR_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.ApplDmgTypePrivateInd, map => {map.Column("APPL_DMG_TYPE_PRIVATE_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.ApplDmgTypePublicInd, map => {map.Column("APPL_DMG_TYPE_PUBLIC_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.ApplDmgTypeCharterInd, map =>{ map.Column("APPL_DMG_TYPE_CHARTER_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.ApplDmgTypeTribalInd, map => {map.Column("APPL_DMG_TYPE_TRIBAL_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.ApplDmgTypeHeadstartInd, map => {map.Column("APPL_DMG_TYPE_HEADSTART_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.ApplDmgTypeStateagencyInd, map => {map.Column("APPL_DMG_TYPE_STATEAGENCY_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.ConsultantAsAuthorisedInd, map => {map.Column("CONSULTANT_AS_AUTHORISED_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.FccRegistrationNo, map => {map.Column("FCC_REGISTRATION_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.StatewidePubSchoolInd, map => {map.Column("STATEWIDE_PUB_SCHOOL_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.StatewideNonpubSchdistInd, map => {map.Column("STATEWIDE_NONPUB_SCHDIST_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.StatewideLibrariesInd, map => {map.Column("STATEWIDE_LIBRARIES_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.StatewideStateCode, map => {map.Column("STATEWIDE_STATE_CODE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.NoItem21LtrSentDt, map => {map.Column("NO_ITEM21_LTR_SENT_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.ApplDmgTypeFederalentInd, map =>{ map.Column("APPL_DMG_TYPE_FEDERALENT_IND"); map.Type(NHibernateUtil.YesNo); });

            ManyToOne(x => x.RequestingOrgSlc, map =>
            {
                map.Column("BUS_PARTY_ID");
                //map.PropertyRef("BusPartyId");
                map.Cascade(Cascade.None);
            });

            //ManyToOne(x => x.StateSlc, map => map.Columns(new Action<IColumnMapper>[] { x => x.Name("LOCATED_IN_STATE_CD"), x => x.Name("CONTACT_STATE_CD") }));
            //ManyToOne(x => x.SystemUsers, map =>
            //{
            //    map.Column("REVIEWED_BY_ID");
            //    map.PropertyRef("BusPartyId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            ManyToOne(x => x.FundProgramYear, map =>
            {
                map.Column("FUND_REQ_YEAR");
                map.NotNullable(true);
                map.Cascade(Cascade.None);
            });

            //ManyToOne(x => x.InvoiceEditRule, map =>
            //{
            //    map.Column("INV_EDIT_RULE_ID");
            //    map.PropertyRef("InvEditRuleId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            //ManyToOne(x => x.SlcConsultantInfo, map =>
            //{
            //    map.Column("CONSULTANT_ID");
            //    map.PropertyRef("ConsultantId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            //Bag(x => x.AsFundCmmtmntStatusLog, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AsFundReqExceptValid, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AsFundReqReviewNote, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.CipaLibFrn486MasterList, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AsFundCmmtmntReqSrvcPrvdr, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestReviewHistories, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.SelectiveLtrCcToContact, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestReviewHistories, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5ConnServices, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Bl5ConnServicesHist, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5ConnHist, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5Conn, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnModify, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqExceptValid, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqReviewNote, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqService, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundReqAutoAssign, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            Bag(x => x.FundReqPendingReason, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            Bag(x => x.FundReqReviewAssignment, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            Bag(x => x.FundCmmtmntReqExceptWatch, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundReqExceptReturn, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });

            //Bag(x => x.FundCmmtmntReqSrvcs, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqReqOrg, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntReqSrvcPrvdr, colmap => { colmap.Key(x => x.Column("FUND_REQ_ID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
