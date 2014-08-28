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
    
    
    public class FundCmmtmntReqSrvcPrvdrMap : ClassMapping<FundCmmtmntReqSrvcPrvdr> {
        
        public FundCmmtmntReqSrvcPrvdrMap() {
			Table("FUND_CMMTMNT_REQ_SRVC_PRVDR");
			Lazy(true);
			Id(x => x.SequenceNo, map => { map.Column("SEQUENCE_NO"); map.Generator(Generators.Assigned); });
			Property(x => x.ContractNbr, map => {map.Column("CONTRACT_NBR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ContractAwardDt, map => {map.Column("CONTRACT_AWARD_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.ContractExpirationDt, map => {map.Column("CONTRACT_EXPIRATION_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.TariffedRateInd, map => {map.Column("TARIFFED_RATE_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ServiceStartDt, map => {map.Column("SERVICE_START_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.EstmtdOneTimeCost, map => {map.Column("ESTMTD_ONE_TIME_COST");});
			Property(x => x.EstmtdMonthlyCost, map => {map.Column("ESTMTD_MONTHLY_COST");});
			Property(x => x.EstmtdAnnualCost, map => {map.Column("ESTMTD_ANNUAL_COST");});
			Property(x => x.DiscountPct, map => {map.Column("DISCOUNT_PCT");});
			Property(x => x.CommittedAmt, map => {map.Column("COMMITTED_AMT");});
            Property(x => x.CommitmentStatusCd, map => { map.Column("COMMITMENT_STATUS_CD"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.SrvcPrvdrLegalNm, map => {map.Column("SRVC_PRVDR_LEGAL_NM"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.PartialFundingRsnCd, map => {map.Column("PARTIAL_FUNDING_RSN_CD"); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.SrvcOrdApprovalStatusCd, map => { map.Column("SRVC_ORD_APPROVAL_STATUS_CD"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.SrvcOrdApprovalStatusDt, map => {map.Column("SRVC_ORD_APPROVAL_STATUS_DT"); map.Type(NHibernateUtil.Date); });
            Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
            Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); map.Type(NHibernateUtil.Date); });
			Property(x => x.PreviousBasicPhoneOnlyInd, map => {map.Column("PREVIOUS_BASIC_PHONE_ONLY_IND"); map.Type(NHibernateUtil.AnsiString); });
            Property(x => x.CommitmentProcessTwiceInd, map => { map.Column("COMMITMENT_PROCESS_TWICE_IND"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.FundCmmtmntNtfLtrSndDt, map => {map.Column("FUND_CMMTMNT_NTF_LTR_SND_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FundCmmtmntEntAnnualCost, map => {map.Column("FUND_CMMTMNT_ENT_ANNUAL_COST");});
			Property(x => x.InvReqReviewInd, map => {map.Column("INV_REQ_REVIEW_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.FrnCancelInd, map => {map.Column("FRN_CANCEL_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.SrvcActualStartDt, map => {map.Column("SRVC_ACTUAL_START_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FrnTermnDt, map => {map.Column("FRN_TERMN_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.RetroactivePmtInd, map => {map.Column("RETROACTIVE_PMT_IND"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.FrnInvActPmtAmt, map => {map.Column("FRN_INV_ACT_PMT_AMT");});
			Property(x => x.FrnCntrctTermnDtCd, map => {map.Column("FRN_CNTRCT_TERMN_DT_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.InvoiceFrnStatusCd, map => {map.Column("INVOICE_FRN_STATUS_CD"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.FundCmmtmntExtndCostAmt, map => {map.Column("FUND_CMMTMNT_EXTND_COST_AMT");});
			Property(x => x.SrvcNtfctnLtrCreateDt, map => {map.Column("SRVC_NTFCTN_LTR_CREATE_DT"); map.Type(NHibernateUtil.Date); });
            Property(x => x.AppealsInd, map => { map.Column("APPEALS_IND"); map.NotNullable(true); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.SrvcPrvdr471RaLtrDt, map => {map.Column("SRVC_PRVDR_471RA_LTR_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.CommittedDt, map => {map.Column("COMMITTED_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.WaveNo, map => {map.Column("WAVE_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.CommitmentRecapAmt, map => {map.Column("COMMITMENT_RECAP_AMT");});
			Property(x => x.BillingAcctNbr, map => {map.Column("BILLING_ACCT_NBR"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ServiceDescCode, map => {map.Column("SERVICE_DESC_CODE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.TotalMonthlyCost, map => {map.Column("TOTAL_MONTHLY_COST");});
			Property(x => x.TotalIneligibleMonthlyCost, map => {map.Column("TOTAL_INELIGIBLE_MONTHLY_COST");});
			Property(x => x.TotalOneTimeCost, map => {map.Column("TOTAL_ONE_TIME_COST");});
			Property(x => x.TotalOneTimeIneligibleCost, map => {map.Column("TOTAL_ONE_TIME_INELIGIBLE_COST"); });
			Property(x => x.F500SrvcNtfctnLtrCreateDt, map => {map.Column("F500_SRVC_NTFCTN_LTR_CREATE_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.ReasonDesc, map => {map.Column("REASON_DESC"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.FrnPiaStatusCommentsTxt, map => {map.Column("FRN_PIA_STATUS_COMMENTS_TXT"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ServiceEndDtInd, map => {map.Column("SERVICE_END_DT_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.ApplCmmtmntNtfctnDt, map => {map.Column("APPL_CMMTMNT_NTFCTN_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.FrnPassedValidationInd, map => {map.Column("FRN_PASSED_VALIDATION_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.KeyedServiceMonthsQty, map => map.Column("KEYED_SERVICE_MONTHS_QTY"));
			Property(x => x.KeyedAnnualPrediscRecurAmt, map => {map.Column("KEYED_ANNUAL_PREDISC_RECUR_AMT");});
			Property(x => x.KeyedCommitmentRequestedAmt, map => {map.Column("KEYED_COMMITMENT_REQUESTED_AMT"); });
			Property(x => x.Form486ProcessInd, map => {map.Column("FORM_486_PROCESS_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.TnTestReqd, map => {map.Column("TN_TEST_REQD"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.UnderReviewInd, map => {map.Column("UNDER_REVIEW_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.AppealsCommittedDt, map => {map.Column("APPEALS_COMMITTED_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.AppealsWaveNo, map => {map.Column("APPEALS_WAVE_NO"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.AppealsApplRevisedFcdlDt, map => {map.Column("APPEALS_APPL_REVISED_FCDL_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.AppealsSpRevisedFcdlDt, map => {map.Column("APPEALS_SP_REVISED_FCDL_DT"); map.Type(NHibernateUtil.Date); });
            Property(x => x.Appletterdt, map => {map.Type(NHibernateUtil.Date); });
			Property(x => x.Sprvdrletterdt,map=>{map.Type(NHibernateUtil.Date); });
			Property(x => x.FunctionType, map => {map.Column("FUNCTION_TYPE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ProductType, map => {map.Column("PRODUCT_TYPE"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.DuplicateFrn, map => map.Column("DUPLICATE_FRN"));
			Property(x => x.TMtmContractInd, map => {map.Column("T_MTM_CONTRACT_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.StateMasterContractInd, map => {map.Column("STATE_MASTER_CONTRACT_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.MultYrContractFrn, map => map.Column("MULT_YR_CONTRACT_FRN"));
			Property(x => x.MultipleBillingAcctNbrInd, map => {map.Column("MULTIPLE_BILLING_ACCT_NBR_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.BasicServiceInd, map => {map.Column("BASIC_SERVICE_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.AppealId, map => {map.Column("APPEAL_ID"); map.Type(NHibernateUtil.AnsiString); });
			Property(x => x.ApplLate486LtrDt, map => {map.Column("APPL_LATE_486_LTR_DT"); map.Type(NHibernateUtil.Date); });
			Property(x => x.DeobligationAmt, map => {map.Column("DEOBLIGATION_AMT");});
			Property(x => x.VoipInd, map => {map.Column("VOIP_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.UnlitDarkFiberInd, map => {map.Column("UNLIT_DARK_FIBER_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.BmicInd, map => {map.Column("BMIC_IND"); map.Type(NHibernateUtil.YesNo); });
			Property(x => x.BroadbandInd, map => {map.Column("BROADBAND_IND"); map.Type(NHibernateUtil.YesNo); });
            //ManyToOne(x => x.RequestingOrgSlc, map => 
            //{
            //    map.Column("BUS_PARTY_ID");
            //    map.PropertyRef("BusPartyId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            //ManyToOne(x => x.ApplSlc, map => 
            //{
            //    map.Column("APPLICATION_ID");
            //    map.PropertyRef("ApplicationId");
            //    map.Cascade(Cascade.None);
            //});

            //ManyToOne(x => x.SlcServiceGrp, map => 
            //{
            //    map.Column("SERVICE_ID");
            //    map.PropertyRef("ServiceId");
            //    map.Cascade(Cascade.None);
            //});

            //ManyToOne(x => x.SystemUsers, map => 
            //{
            //    map.Column("REVIEWED_BY_ID");
            //    map.PropertyRef("BusPartyId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

			ManyToOne(x => x.FundCmmtmntReqSlc, map => 
			{
				map.Column("FUND_REQ_ID");
				map.PropertyRef("FundReqId");
				map.Cascade(Cascade.None);
			});

            //ManyToOne(x => x.UsacServiceProviders, map => 
            //{
            //    map.Column("USAC_SPIN");
            //    map.PropertyRef("OrganizationId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            //ManyToOne(x => x.InvoiceEditRule, map => 
            //{
            //    map.Column("INV_EDIT_RULE_ID");
            //    map.PropertyRef("InvEditRuleId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            //ManyToOne(x => x.FundCmmtmntReqGrp, map => 
            //{
            //    map.Column("FUND_CMMTMNT_REQ_GRP_SEQ_NO");
            //    map.PropertyRef("FundCmmtmntReqGrpSeqNo");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            //ManyToOne(x => x.ProcessStatusReason, map => 
            //{
            //    map.Column("REASON_ID");
            //    map.PropertyRef("ReasonId");
            //    map.NotNullable(true);
            //    map.Cascade(Cascade.None);
            //});

            //Bag(x => x.CipaLibFrn486MasterList, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.CipaLibFrn486MasterList, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.DisasterReliefFrn, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.DisasterReliefFrn, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F486DtlReviewNotes, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F486DtlReviewNotes, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.InvoiceDetail, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.InvoiceDetail, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.InvDtlHist, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.InvDtlHist, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestReviewHistories, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestReviewHistories, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestReviewHistories, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.RequestReviewHistories, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5ConnServices, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5ConnServices, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Bl5ConnServicesHist, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Bl5ConnServicesHist, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5ConnHist, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5ConnHist, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5Conn, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.F471Block5Conn, colmap =>  { colmap.Key(x => x.Column("FCR_SRVC_PRVDR_SEQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnExceptValid, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnExceptValid, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnInvoiceExtension, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnInvoiceExtension, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnModify, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnModify, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnRecaptureFund, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnRecaptureFund, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnStaffPreAssignment, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnStaffPreAssignment, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundRescindChecks, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundRescindChecks, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntStatusLog, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FundCmmtmntStatusLog, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnObligationTxn, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnObligationTxn, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnReviewNote, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnReviewNote, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AppealsInAccess, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.AppealsInAccess, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.InvoiceDetail, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.InvoiceDetail, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.CipaLibFrn486MasterList, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.CipaLibFrn486MasterList, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnExceptValid, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnExceptValid, colmap =>  { colmap.Key(x => x.Column("FUND_REQ_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnInvoiceExtension, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            //Bag(x => x.FrnInvoiceExtension, colmap =>  { colmap.Key(x => x.Column("SEQUENCE_NO")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
