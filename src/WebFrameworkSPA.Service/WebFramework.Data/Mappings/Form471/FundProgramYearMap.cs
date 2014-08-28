using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using WebFramework.Data.Domain;


namespace WebFramework.Data.Mappings
{
    
    
    public class FundProgramYearMap : ClassMapping<FundProgramYear> {
        
        public FundProgramYearMap() {
			Table("FUND_PROGRAM_YEAR");
            Lazy(true);
			Id(x => x.Year, map => 
				{
					map.Column("YEAR");
				});
			Property(x => x.ApplAcceptedDt, map => { map.Column("APPL_ACCEPTED_DT"); map.NotNullable(true); });
			Property(x => x.TotalFundAmt, map => { map.Column("TOTAL_FUND_AMT"); map.NotNullable(true); });
			Property(x => x.ServiceStartDt, map => { map.Column("SERVICE_START_DT"); map.NotNullable(true); });
			Property(x => x.ServiceEndDate, map => { map.Column("SERVICE_END_DATE"); map.NotNullable(true); });
			Property(x => x.Total6MnthAmt, map => map.Column("TOTAL_6_MNTH_AMT"));
			Property(x => x.TotalSlcLessNeedCmmitedAm, map => map.Column("TOTAL_SLC_LESS_NEED_CMMITED_AM"));
			Property(x => x.TotalSlcHiNeedCmmitedAmt, map => map.Column("TOTAL_SLC_HI_NEED_CMMITED_AMT"));
			Property(x => x.Cumulative6MnthEstimatedAm, map => map.Column("CUMULATIVE_6_MNTH_ESTIMATED_AM"));
			Property(x => x.TotalRhcCommtedAmt, map => map.Column("TOTAL_RHC_COMMTED_AMT"));
			Property(x => x.LastCommitmentRunDt, map => map.Column("LAST_COMMITMENT_RUN_DT"));
			Property(x => x.WindowCloseDt, map => map.Column("WINDOW_CLOSE_DT"));
			Property(x => x.FundYrStartDt, map => { map.Column("FUND_YR_START_DT"); map.NotNullable(true); });
			Property(x => x.FundYrEndDt, map => { map.Column("FUND_YR_END_DT"); map.NotNullable(true); });
			Property(x => x.SrvcNtfctnLtrLastRunDt, map => map.Column("SRVC_NTFCTN_LTR_LAST_RUN_DT"));
			Property(x => x.TotalAvailForCommitmentAmt, map => map.Column("TOTAL_AVAIL_FOR_COMMITMENT_AMT"));
			Property(x => x.FundCmmtmntMinAmt, map => map.Column("FUND_CMMTMNT_MIN_AMT"));
			Property(x => x.WindowDays, map => map.Column("WINDOW_DAYS"));
			Property(x => x.FundCloseDt, map => map.Column("FUND_CLOSE_DT"));
			Property(x => x.CommitedAmt, map => map.Column("COMMITED_AMT"));
			Property(x => x.InvAcptbEndDt, map => map.Column("INV_ACPTB_END_DT"));
			Property(x => x.CusBilParmLrnDt, map => map.Column("CUS_BIL_PARM_LRN_DT"));
			Property(x => x.CusBilParmHrnDt, map => map.Column("CUS_BIL_PARM_HRN_DT"));
			Property(x => x.ContractExpLrnDt, map => map.Column("CONTRACT_EXP_LRN_DT"));
			Property(x => x.AppealsFundAmt, map => map.Column("APPEALS_FUND_AMT"));
			Property(x => x.AppealsCommittedAmt, map => map.Column("APPEALS_COMMITTED_AMT"));
			Property(x => x.AppealsCloseDt, map => map.Column("APPEALS_CLOSE_DT"));
			Property(x => x.SrvcContTermnExtndDt, map => map.Column("SRVC_CONT_TERMN_EXTND_DT"));
			Property(x => x.TotalCmmtmntRecapAmt, map => map.Column("TOTAL_CMMTMNT_RECAP_AMT"));
			Property(x => x.CusInvGrcpdParmDt, map => map.Column("CUS_INV_GRCPD_PARM_DT"));
			Property(x => x.ContractWaitDayCt, map => { map.Column("CONTRACT_WAIT_DAY_CT"); map.NotNullable(true); });
			Property(x => x.CmmtmntPct, map => map.Column("CMMTMNT_PCT"));
			Property(x => x.YearNo, map => { map.Column("YEAR_NO"); map.NotNullable(true); });
			Property(x => x.EarlyNotificationEndDate, map => map.Column("EARLY_NOTIFICATION_END_DATE"));
			Property(x => x.F500SrvcNtfctnLtrRunDt, map => map.Column("F500_SRVC_NTFCTN_LTR_RUN_DT"));
			Property(x => x.ElectronicFiledCertReqDt, map => map.Column("ELECTRONIC_FILED_CERT_REQ_DT"));
			Property(x => x.PiaCertificationCutoffDt, map => map.Column("PIA_CERTIFICATION_CUTOFF_DT"));
			Property(x => x.PiaIcDiscountCutoffPct, map => map.Column("PIA_IC_DISCOUNT_CUTOFF_PCT"));
			Property(x => x.OpenForPiaReviewInd, map => { map.Column("OPEN_FOR_PIA_REVIEW_IND"); map.NotNullable(true); });
			Property(x => x.AttachmentRcvdCutoffDt, map => map.Column("ATTACHMENT_RCVD_CUTOFF_DT"));
			Property(x => x.Certification470CutoffDt, map => map.Column("CERTIFICATION_470_CUTOFF_DT"));
			Property(x => x.CreateUserId, map => { map.Column("CREATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.CreateTs, map => { map.Column("CREATE_TS"); map.NotNullable(true); });
			Property(x => x.LastUpdateUserId, map => { map.Column("LAST_UPDATE_USER_ID"); map.NotNullable(true); });
			Property(x => x.LastUpdateTs, map => { map.Column("LAST_UPDATE_TS"); map.NotNullable(true); });
			Property(x => x.PiaFrnIcDiscountCutoffCd, map => map.Column("PIA_FRN_IC_DISCOUNT_CUTOFF_CD"));
			Property(x => x.CommittedAmountPerEntity, map => map.Column("COMMITTED_AMOUNT_PER_ENTITY"));
			Property(x => x.CommittedAmountDisbPct, map => map.Column("COMMITTED_AMOUNT_DISB_PCT"));
			Property(x => x.InvEstOneTimeCostEndDt, map => map.Column("INV_EST_ONE_TIME_COST_END_DT"));
			Property(x => x.AdlTemplateDir, map => map.Column("ADL_TEMPLATE_DIR"));
			Property(x => x.AdlDraftDir, map => map.Column("ADL_DRAFT_DIR"));
			Property(x => x.AdlLetterDir, map => map.Column("ADL_LETTER_DIR"));
			Property(x => x.BelowBucketReqdInd, map => map.Column("BELOW_BUCKET_REQD_IND"));
			Property(x => x.AstarsExceptionReadyInd, map => { map.Column("ASTARS_EXCEPTION_READY_IND"); map.NotNullable(true); });
			Property(x => x.F486TpaSamplingParm, map => map.Column("F486_TPA_SAMPLING_PARM"));
			Property(x => x.DeobligationAmt, map => map.Column("DEOBLIGATION_AMT"));
			Property(x => x.RolloverFundAmt, map => map.Column("ROLLOVER_FUND_AMT"));
			Property(x => x.AdminFees, map => map.Column("ADMIN_FEES"));
			Property(x => x.ThresholdFundingAbovePct, map => { map.Column("THRESHOLD_FUNDING_ABOVE_PCT"); map.NotNullable(true); });
			Property(x => x.ThresholdDeniedBelowPct, map => { map.Column("THRESHOLD_DENIED_BELOW_PCT"); map.NotNullable(true); });
			Property(x => x.ThresholdComments, map => { map.Column("THRESHOLD_COMMENTS"); map.NotNullable(true); });
			Property(x => x.Item21CloseDt, map => map.Column("ITEM21_CLOSE_DT"));
			Property(x => x.HardWindowCloseDt, map => map.Column("HARD_WINDOW_CLOSE_DT"));
        }
    }
}
