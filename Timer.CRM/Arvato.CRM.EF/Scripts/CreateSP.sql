USE [Kidsland_CRM]
GO
/****** Object:  UserDefinedFunction [dbo].[IntTo36HexStr]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IntTo36HexStr]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IntTo36HexStr]
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ViewCreate]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ViewCreate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Sys_ViewCreate]
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ReturnAvaiAliasColumn]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ReturnAvaiAliasColumn]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Sys_ReturnAvaiAliasColumn]
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ReturnAuthValueForAccountChange]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ReturnAuthValueForAccountChange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Sys_ReturnAuthValueForAccountChange]
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ReturnAuthValue]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ReturnAuthValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Sys_ReturnAuthValue]
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_MemViewCreate]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_MemViewCreate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Sys_MemViewCreate]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_WXSign]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_WXSign]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_WXSign]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_UseCoupon_Count]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_UseCoupon_Count]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_UseCoupon_Count]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_StoreConsumptionMonthly_Count]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_StoreConsumptionMonthly_Count]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_StoreConsumptionMonthly_Count]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_StoreConsumptionDuty_Count]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_StoreConsumptionDuty_Count]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_StoreConsumptionDuty_Count]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_PriceSegmentDistributionOffline_Result]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_PriceSegmentDistributionOffline_Result]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_PriceSegmentDistributionOffline_Result]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Prd_WeChatSign]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Prd_WeChatSign]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Prd_WeChatSign]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemUpGrade]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemUpGrade]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_MemUpGrade]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemRepeatedConsumption_Result]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemRepeatedConsumption_Result]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_MemRepeatedConsumption_Result]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemMonthConsum]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemMonthConsum]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_MemMonthConsum]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemContributionRate_Result]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemContributionRate_Result]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_MemContributionRate_Result]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Recruit_Count_test]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Recruit_Count_test]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_Recruit_Count_test]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Recruit_Count]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Recruit_Count]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_Recruit_Count]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_MemToNonMemSalesDutyCount]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_MemToNonMemSalesDutyCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_MemToNonMemSalesDutyCount]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_IssuingConsumption_Result]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_IssuingConsumption_Result]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_IssuingConsumption_Result]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Count_test]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Count_test]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_Count_test]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Count]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Count]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_Count]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_ContributionRate_Statistics]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_ContributionRate_Statistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_ContributionRate_Statistics]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_ConsumptionFrequency_Count]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_ConsumptionFrequency_Count]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_ConsumptionFrequency_Count]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_ConsumerDetails]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_ConsumerDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_ConsumerDetails]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Activitycount]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Activitycount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_Mem_Activitycount]
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MarketActivityTracking]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MarketActivityTracking]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rpt_MarketActivityTracking]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadPoint3]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadPoint3]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_WillDeadPoint3]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadPoint]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadPoint]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_WillDeadPoint]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadCreditYear]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadCreditYear]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_WillDeadCreditYear]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadCredit3]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadCredit3]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_WillDeadCredit3]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadCredit]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadCredit]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_WillDeadCredit]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnTradeInfo]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnTradeInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_ReturnTradeInfo]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnSplitDetail]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnSplitDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_ReturnSplitDetail]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnSecondSplitDetail]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnSecondSplitDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_ReturnSecondSplitDetail]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnFirstSplitDetail]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnFirstSplitDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_ReturnFirstSplitDetail]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_FreezeToThawAccountUpdate]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_FreezeToThawAccountUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_FreezeToThawAccountUpdate]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_DisabledAccountUpdate]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_DisabledAccountUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_DisabledAccountUpdate]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountValueUpdate]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountValueUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_AccountValueUpdate]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRuleNoTrade]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRuleNoTrade]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_AccountComputeForActRuleNoTrade]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRule1008]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRule1008]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_AccountComputeForActRule1008]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRule0604]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRule0604]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_AccountComputeForActRule0604]
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRule]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Loy_AccountComputeForActRule]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Pro_InsertMemberID]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Pro_InsertMemberID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_Pro_InsertMemberID]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_MemGradeAdjust]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_MemGradeAdjust]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_MemGradeAdjust]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_UpNeedPoint]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_UpNeedPoint]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_Mem_UpNeedPoint]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_SMSSend]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_SMSSend]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_Mem_SMSSend]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_SearchForAccountChange]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_SearchForAccountChange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_Mem_SearchForAccountChange]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_Search1]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_Search1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_Mem_Search1]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_Search]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_Mem_Search]
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Act_InstanceMemberid]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Act_InstanceMemberid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CRM_Act_InstanceMemberid]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_WX_GetActivityList]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_WX_GetActivityList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_WX_GetActivityList]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivitySubdivision]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivitySubdivision]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivitySubdivision]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityRecordStep]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityRecordStep]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityRecordStep]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityRecordExpired]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityRecordExpired]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityRecordExpired]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushWechat]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushWechat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPushWechat]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushSurvey]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushSurvey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPushSurvey]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushSMS]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushSMS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPushSMS]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushOB]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushOB]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPushOB]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushNormal]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushNormal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPushNormal]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushEmail]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPushEmail]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushCoupon]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushCoupon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPushCoupon]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullWechat]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullWechat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPullWechat]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullSurvey]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullSurvey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPullSurvey]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullSMS]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullSMS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPullSMS]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullOB]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullOB]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPullOB]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullNormal]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullNormal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPullNormal]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullEmail]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPullEmail]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullCoupon]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullCoupon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityPullCoupon]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityNextSetp]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityNextSetp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityNextSetp]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityFinish]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityFinish]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityFinish]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityActive]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityActive]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_MarketActivityActive]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_GetSubdivision]    Script Date: 2015/12/24 14:24:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_GetSubdivision]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Act_GetSubdivision]
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_GetSubdivision]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_GetSubdivision]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[sp_Act_GetSubdivision] 
(
@ActivityID int=null,
@ActivityName nvarchar(50) = '''',
@Enable  bit=null,
@PlanStartDateFrom datetime=null,
@PlanStartDateTo datetime=null,
@PlanEndDateFrom datetime=null,
@PlanEndDateTo datetime=null,
@SearchGroupID int,
@SearchStore nvarchar(20),
@UserGroupID int,
@AuthStores nvarchar(1000),
@AuthGroups nvarchar(100),
@PageIndex int = 1,
@PageSize int = 10,
@RecordCount int out
)
as
 /**********************************
  ----arvarto system-----
  存储过程功能描述：获取市场活动信息及会员细分实例名
  建 立 人：zyb
  建立时间：2014-07-10
  修 改 人：
  修改时间：
  修改内容:
  ***********************************/    
begin

	declare
			@Sql varchar(max) = '''',
	        @Sql_Search varchar(max) = '' where 1=1 '',
			@Sql_Count nvarchar(max)

	if (isnull(@ActivityID, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.ActivityID= ''''''+convert(nvarchar(100),@ActivityID)+'''''' ''     
	end


	if (isnull(@ActivityName, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.ActivityName like ''''%''+@ActivityName+''%'''' ''    
	end

	if (isnull(convert(nvarchar(100),@Enable), '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.Enable =  ''''''+convert(nvarchar(100),@Enable)+'''''' ''   
	end

	if (isnull(@PlanStartDateFrom,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and convert(NVARCHAR(10),a.PlanStartDate,121)>= ''''''+convert( NVARCHAR(10),@PlanStartDateFrom,121)+'''''' ''
	end
	if (isnull(@PlanStartDateTo,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and convert(NVARCHAR(10),a.PlanStartDate,121)<= ''''''+convert( NVARCHAR(10),@PlanStartDateTo,121)+'''''' ''
	end
	if (isnull(@PlanEndDateFrom,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and convert(NVARCHAR(10),a.PlanEndDate,121)>= ''''''+convert( NVARCHAR(10),@PlanEndDateFrom,121)+'''''' ''
	end
	if (isnull(@PlanEndDateTo,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and convert(NVARCHAR(10),a.PlanEndDate,121)<= ''''''+convert( NVARCHAR(10),@PlanEndDateTo,121)+'''''' ''
	end

	if (isnull(@SearchGroupID, 0) <> 0 )
	begin
		set @Sql_Search = @Sql_Search + '' and a.DataGroupID = ''+Convert(varchar,@SearchGroupID) 
	end
	else if(isnull(@UserGroupID, 0) <> 0 )
	begin
		if (isnull(@AuthGroups, '''') <> '''' )--权限群组
		begin
			set @Sql_Search = @Sql_Search + '' and CharIndex(''''''+@AuthGroups+'''''',a.DataGroupID)>=0 ''    
		end
		--else
		--begin
		--	set @Sql_Search = @Sql_Search + '' and a.DataGroupID = ''+Convert(varchar,@UserGroupID)   
		--end
	end

	if (isnull(@SearchStore, '''') <> '''' )--查询门店
	begin
		set @Sql_Search = @Sql_Search + '' and a.StoreCode = ''''''+@SearchStore+'''''' ''    
	end

	if (isnull(@AuthGroups, '''') <> '''' )--权限群组
	begin
		set @Sql_Search = @Sql_Search + '' and CharIndex(''''''+@AuthGroups+'''''',a.DataGroupID)>=0 ''    
	end
	else 
	if (isnull(@AuthStores, '''') <> '''' )--权限门店
	begin
		set @Sql_Search = @Sql_Search + '' and CharIndex(''''''+@AuthStores+'''''',a.StoreCode)>=0 ''     
	end


--清空游标
truncate table TE_Mem_TableName

DECLARE MyCursor CURSOR 
FOR 
   select TableName
   from TM_Act_Master a
   left join (
             select * from
                        (select m.*,ROW_NUMBER() OVER (partition by m.ActivityID ORDER BY m.addeddate desc) serial
                         from TM_Act_Instance m) t
              where serial=1 ) b on a.ActivityID=b.ActivityID

--打开一个游标	
OPEN MyCursor
--循环一个游标
DECLARE @TableName nvarchar(2000) 	
FETCH NEXT FROM  MyCursor INTO @TableName
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql1 nvarchar(max);
    set @sql1 =''
	          insert into TE_Mem_TableName
              select ''''''+@tablename+'''''',count(distinct memberid) 
              from [''+@tablename+''] where IsSelected=1''
   exec (@sql1)

FETCH NEXT FROM  MyCursor INTO @TableName
END	
--关闭游标
CLOSE MyCursor
--释放资源
DEALLOCATE MyCursor

    set @Sql='' from TM_Act_Master  a
  left join ( 
 select * from 
 (select m.*,ROW_NUMBER() OVER (partition by m.ActivityID ORDER BY m.addeddate desc) serial 
  from TM_Act_Instance m) t
 where  serial=1 )
   b on a.ActivityID=b.ActivityID
 left join TE_Mem_TableName c on b.tablename=c.tablename
 left join TM_SYS_DataGroup d on a.DataGroupID=d.DataGroupID 
 left join V_M_TM_SYS_BaseData_store e on a.StoreCode=e.StoreCode 
   ''+@Sql_Search
	    
	set @Sql_Count = ''select @ct = count(a.ActivityID) '' + @Sql 
	exec sp_executesql @Sql_Count,N''@ct int output'',@RecordCount output 

	set @Sql = ''
       select a.ActivityID,a.DataGroupID,a.ActivityName,a.PlanStartDate,a.PlanEndDate,a.Workflow,a.WfRootId,a.Schedule,
       a.Enable,a.status,a.remark,a.DataGroupName,a.StoreName,
       a.AddedDate,a.ModifiedDate,a.SubdivisionName,Memcnt
	   from (
	   select a.ActivityID,a.DataGroupID,a.ActivityName,a.PlanStartDate,a.PlanEndDate,a.Workflow,a.WfRootId,a.Schedule,
       a.Enable,a.status,a.remark,d.DataGroupName,e.StoreName,
       b.AddedDate,b.ModifiedDate,
	   SubdivisionName = STUFF(( SELECT '''','''' + e.SubdivisionName
         FROM TM_Act_InstanceSubdivision c 
        inner join TM_Mem_SubdivisionInstance d on c.SubdivisionInstanceID=d.SubdivisionInstanceID
        inner join TM_Mem_Subdivision e on d.SubdivisionID=e.SubdivisionID 
         WHERE  b.ActivityInstanceID=c.ActivityInstanceID
          FOR
         XML PATH('''''''')
          ), 1, 1, ''''''''),c.memcnt,
       ROW_NUMBER () over(order by a.addeddate desc) RowID
	''+@Sql+'') a  ''



if (@PageSize is not null and @PageSize>0) 
	begin 
		set @Sql=@Sql+''where RowID > ''+convert(nvarchar(100),@PageIndex)+'' and RowID <= ''+convert(nvarchar(100),@PageIndex+@PageSize)+''  order by a.RowID''

      	print @Sql
        exec (@Sql)
	end 
	else 
	begin
		declare @tbl table(
			[ActivityID] [int] NOT NULL,
			[DataGroupID] [int] NOT NULL,
			[ActivityName] [nvarchar](50) NOT NULL,
			[PlanStartDate] [datetime] NOT NULL,
			[PlanEndDate] [datetime] NULL,
			[Workflow] [nvarchar](max) NOT NULL,
			[WfRootId] [uniqueidentifier] NULL,
			[Schedule] [nvarchar](1000) NOT NULL,
			[Enable] [bit] NOT NULL,
			[Status] [smallint] NOT NULL,
			[Remark] [nvarchar](100) NULL,
			[AddedDate] [datetime] NULL,
			[ModifiedDate] [datetime] NULL,
			[SubdivisionName] [nvarchar](max) NULL,
			[Memcnt] [int]  NULL,
			[DataGroupName] [nvarchar](100),
			[StoreName] [nvarchar](100)
		)
		SET @RecordCount = 0
		SELECT * FROM @tbl
	end


end















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityActive]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityActive]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_Act_MarketActivityActive]
	@activityId AS INT,
	@selectType as nvarchar(10),
	@selectRate as float,
	@selectAmount as int
AS
BEGIN TRY

--set @activityId=10
--set @selectType=''Rate''
--set @selectRate=100.0

--创建事务
BEGIN TRANSACTION

	DECLARE @tableName AS nvarchar(100)
	DECLARE @activityInstanceId AS bigint
	SET @tableName = ''TM_Act_InstanceSubdivisionResult_'' + CAST(NEWID() AS nvarchar(50))
	--print @tableName

	--实例化主表
	INSERT INTO TM_ACT_INSTANCE  
		(
			ACTIVITYID
			,DataGroupID
			,ACTIVITYNAME
			,PLANSTARTDATE
			,PlanEndDate
			,WORKFLOW
			,WfRootId
			,SCHEDULE
			,ENABLE
			,STATUS
			,TABLENAME
			,IsTranslated
			,REMARK
			,ADDEDDATE
			,ADDEDUSER
			,MODIFIEDDATE
			,MODIFIEDUSER
		)
	SELECT 
		ACTIVITYID
		,DataGroupID
		,ACTIVITYNAME
		,PLANSTARTDATE
		,PlanEndDate
		,WORKFLOW
		,WfRootId
		,SCHEDULE
		,ENABLE
		,1
		,@tableName
		,0
		,REMARK
		,GetDate() ADDEDDATE
		,ADDEDUSER
		,GetDate() MODIFIEDDATE
		,MODIFIEDUSER
	FROM TM_ACT_MASTER
	WHERE ACTIVITYID = @activityId

	--declare @activityInstanceId bigint
	SET @activityInstanceId = @@IDENTITY
	--print @activityInstanceId

	--实例化会员细分表
	INSERT INTO TM_Act_InstanceSubdivision
	SELECT @activityInstanceId
		,M.CurSubdivisionInstanceID
	FROM TM_Act_Subdivision A
		INNER JOIN TM_Mem_Subdivision M ON A.SubdivisionID = M.SubdivisionID
	WHERE a.ActivityID = @activityId 
		AND M.CurSubdivisionInstanceID IS NOT NULL

	--创建细分明细表结构
	IF OBJECT_ID(N''TM_Act_InstanceSubdivisionResult'', N''U'') IS NOT NULL
		DROP TABLE TM_Act_InstanceSubdivisionResult

	CREATE TABLE TM_Act_InstanceSubdivisionResult
	(
		ISRID BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		MemberId CHAR(32) NOT NULL,
		WfRootId UNIQUEIDENTIFIER NOT NULL,
		IsSelected BIT NOT NULL default (0),
		Status SMALLINT NOT NULL DEFAULT (0),
		Reply NVARCHAR(100),
		StartDate DATETIME,
		ExpiredDate DATETIME,
		Remark NVARCHAR(100),
		AddedDate DATETIME NOT NULL DEFAULT GETDATE(),
		ModifiedDate DATETIME NOT NULL DEFAULT GETDATE()
	)
	EXEC sp_rename ''TM_Act_InstanceSubdivisionResult'', @tableName

	--初始化细分明细表数据
	DECLARE @wfRid UNIQUEIDENTIFIER
	DECLARE @tblMName NVARCHAR(100)
	DECLARE @sql nvarchar(MAX)
	DECLARE RdCursor CURSOR FOR 
	SELECT I.WfRootId,B.TableName
	FROM TM_ACT_INSTANCE I
		INNER JOIN TM_Act_InstanceSubdivision S ON I.ActivityInstanceID = S.ActivityInstanceID
		INNER JOIN TM_Mem_SubdivisionInstance B ON S.SubdivisionInstanceID = B.SubdivisionInstanceID
	WHERE I.ActivityInstanceID = @activityInstanceId

	DECLARE @i AS INT
	SET @i = 0
	OPEN RdCursor
	FETCH NEXT FROM RdCursor INTO @wfRid, @tblMName
	WHILE (@@FETCH_STATUS = 0)
		BEGIN 
			IF @i = 0
				BEGIN
					SET @sql = N''INSERT ''+ QUOTENAME(@tableName) + N'' 
									(MemberId
									,WfRootId) 
								SELECT MemberId
									,'''''' + CAST(@wfRid AS NVARCHAR(50)) + N'''''' 
								FROM '' + QUOTENAME(@tblMName)
				END
			ELSE
				BEGIN
					SET @sql = @sql + N'' UNION SELECT 
											MemberId
											,'''''' + CAST(@wfRid AS NVARCHAR(50)) + N'''''' 
											FROM '' + QUOTENAME(@tblMName)
				END
			SET @i = @i + 1
			FETCH NEXT FROM RdCursor INTO @wfRid, @tblMName
		END
	IF @sql IS NOT NULL
		BEGIN 
			EXEC sp_executesql @sql
		END
	CLOSE RdCursor
	DEALLOCATE RdCursor

	--设置执行比例
	DECLARE @usql AS NVARCHAR(MAX)
	SET @usql = N''UPDATE '' + QUOTENAME(@tableName) + N'' 
				  SET IsSelected = 1
					  ,Status = 1
					  ,StartDate = GETDATE() 
				  WHERE ISRID IN (SELECT TOP ''
	IF @selectType = ''Rate''
		SET @usql = @usql + CAST(@selectRate AS NVARCHAR) + N'' PERCENT ISRID FROM '' + QUOTENAME(@tableName) + N'' ORDER BY NEWID()) ''
	ELSE IF @selectType=''Amount''
		SET @usql = @usql + CAST(@selectAmount AS NVARCHAR) + N'' ISRID FROM '' + QUOTENAME(@tableName) + N'' ORDER BY NEWID()) ''
	ELSE 
		SET @usql = N''UPDATE '' + QUOTENAME(@tableName) + N'' 
					  SET IsSelected = 1
						  ,Status = 1
						  ,StartDate = GETDATE() ''
	EXEC sp_executesql @usql

	--更新主表信息
	UPDATE TM_ACT_INSTANCE
	SET STATUS = 2
	WHERE ActivityInstanceID = @activityInstanceId

COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               );  
ROLLBACK TRANSACTION	--事物事务

END CATCH










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityFinish]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityFinish]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[sp_Act_MarketActivityFinish]
AS 
BEGIN 
	DECLARE @aiId BIGINT
	DECLARE @tblName NVARCHAR(100)
	DECLARE @sql NVARCHAR(max)

	DECLARE insCur CURSOR FOR 
	SELECT ActivityInstanceID,TableName FROM TM_Act_Instance
	WHERE STATUS = 2

	OPEN insCur
	FETCH NEXT FROM insCur INTO @aiId, @tblName
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		SET @sql = N''IF NOT EXISTS 
						(
							SELECT * FROM '' + QUOTENAME(@tblName) + N'' 
							WHERE IsSelected = 1 
								AND STATUS IN (0,1,2,3)
						) 
					UPDATE TM_Act_Instance SET STATUS = 3
						, ModifiedDate=GetDate() 
					WHERE ActivityInstanceID = @aiId''
		EXEC sp_executesql @sql, N''@aiId BIGINT'', @aiId
		FETCH NEXT FROM insCur INTO @aiId,@tblName
	END
	CLOSE insCur
	DEALLOCATE insCur
END 










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityNextSetp]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityNextSetp]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




CREATE PROCEDURE [dbo].[sp_Act_MarketActivityNextSetp]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@resultType NVARCHAR(10),
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)

	--DECLARE @tmpTables TABLE
	--	(
	--		[ISRID] BIGINT NOT NULL,
	--		[MemberID] [CHAR](32) NOT NULL,
	--		[Reply] NVARCHAR(100) NOT NULL
	--	)
	IF OBJECT_ID(''tempdb..#tmpTableStep'') IS NOT NULL
		DROP TABLE #tmpTableStep
	CREATE TABLE #tmpTableStep
	(
		[ISRID] BIGINT NOT NULL,
		[MemberID] [CHAR](32) NOT NULL,
		[Reply] NVARCHAR(100) NOT NULL
	)

	--insert into @tmpTables
	--(ISRID,MemberID,Reply)
	--select top cast(@maxCount as nvarchar) a.ISRID,a.MemberId,a.Reply from TM_Act_InstanceSubdivisionResult a
	--where a.IsSelected=1 and a.ExpiredDate=0 and a.Status=3 and a.WfRootId=@wfRootId
	SET @sql = ''''
	SET @sql = N''INSERT INTO #tmpTableStep
					(ISRID
					,MemberID
					,Reply)
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + '' 
					a.ISRID
					,a.MemberId
					,a.Reply
				FROM '' + QUOTENAME(@tblName) + '' a
				WHERE a.IsSelected = 1 
					AND a.ExpiredDate IS NULL
					AND a.Status = 3 
					AND a.WfRootId = @wfRootId''
					--AND NOT EXISTS
					--	(
					--		SELECT * 
					--		FROM '' + QUOTENAME(@tblName) + N'' b
					--		WHERE b.MemberId = a.MemberId
					--			AND b.WfRootId = @wfRootId
					--	)''
		
	IF @sql IS NOT NULL AND @sql != ''''
		EXEC sp_executesql @sql, N''@wfRootId uniqueidentifier'', @wfRootId = @wfRootId
		
	IF EXISTS(SELECT * FROM #tmpTableStep)
		BEGIN

			--更新记录状态
			SET @sql = ''''
			SET @sql = N''UPDATE '' + QUOTENAME(@tblName) + N''
						SET Status = 4
							,ModifiedDate = GETDATE()
						FROM '' + QUOTENAME(@tblName) + '' a
							,#tmpTableStep t
						WHERE a.ISRID = t.ISRID''
			IF @sql IS NOT NULL AND @sql != ''''
				BEGIN
					EXEC sp_executesql @sql
				END

			--获取当前步骤的子步骤
			--DECLARE @childSteps TABLE
			IF OBJECT_ID(''tempdb..#childSteps'') IS NOT NULL
				DROP TABLE #childSteps
			CREATE TABLE #childSteps
			(
				[InstanceStepID] [uniqueidentifier] NOT NULL,
				[Category] [nvarchar](20) NOT NULL,
				[Condition] [nvarchar](50) NULL
			)
			INSERT INTO #childSteps
				(InstanceStepID
				,Category
				,Condition)
			SELECT InstanceStepID
				,Category
				,Condition
				--CASE @resultType 
				--	WHEN ''Number'' THEN CASE CHARINDEX('','', Condition) 
				--		WHEN 0 THEN '' = '' + Condition 
				--	ELSE '' BETWEEN '' + REPLACE(Condition, '','', '' AND '') 
				--	END
				--ELSE Condition 
				--END
			FROM TM_Act_InstanceStep 
			WHERE ActivityInstanceID = @instanceId 
				AND ParentInstanceStepID = @wfRootId

			DECLARE @sqlInsert NVARCHAR(MAX)
			SET @sqlInsert = N''INSERT INTO '' + QUOTENAME(@tblName) + N''
							(
							   [MemberId]
							   ,[WfRootId]
							   ,[Status]
							   ,[IsSelected]
							)
							SELECT t.MemberID
								,c.InstanceStepID
								,1
								,1 
							FROM #childSteps c
								,'' + CASE @resultType WHEN ''Number'' THEN N''(SELECT * FROM #tmpTableStep WHERE ISNUMERIC(Reply) = 1)'' ELSE N''#tmpTableStep'' END + N'' t
							WHERE NOT EXISTS
									(
										SELECT * 
										FROM '' + QUOTENAME(@tblName) + N'' r
										WHERE r.MemberId = t.MemberID
											AND r.WfRootId = c.InstanceStepID
									) ''

			IF EXISTS(SELECT * FROM #childSteps)
				BEGIN
					IF NOT EXISTS(SELECT * FROM #childSteps WHERE Category = ''Branch'')
						BEGIN
							--insert into TM_Act_InstanceSubdivisionResult
							--(
							--   [MemberId]
							--   ,[WfRootId]
							--   ,[Status]
							--   ,[AddedDate]
							--   ,[ModifiedDate]
							--   ,[IsSelected]
							--)
							--select t.MemberID,c.InstanceStepID,1,GETDATE(),GETDATE(),1
							--from @childSteps c,@tmpTables t
							IF @sqlInsert IS NOT NULL AND @sqlInsert != ''''
								BEGIN
									--DECLARE @sqlIstNor NVARCHAR(MAX)
									--SET @sqlIstNor = @sqlInsert + N'' FROM #childSteps c, #tmpTableStep t ''
									--IF @sqlIstNor IS NOT NULL AND @sqlIstNor != ''''
									EXEC sp_executesql @sqlInsert
								END
						END
					ELSE
						BEGIN
							IF @resultType = ''Number''
								BEGIN
									DECLARE @sqlIstNum NVARCHAR(MAX)
									IF @sqlInsert IS NOT NULL AND @sqlInsert != ''''
										BEGIN
											DECLARE @condition NVARCHAR(200)
											DECLARE RdCursor CURSOR FOR 
												SELECT Condition FROM #childSteps WHERE Category = ''Branch''
											OPEN RdCursor
											FETCH NEXT FROM RdCursor INTO @condition
												WHILE (@@FETCH_STATUS = 0)
													BEGIN 
														SET @sqlIstNum = ''''
														SET @sqlIstNum = @sqlInsert + N'' AND t.Reply '' + CASE CHARINDEX('','', @condition) 
																											WHEN 0 THEN '' = '' + @condition 
																											ELSE '' BETWEEN '' + REPLACE(@condition, '','', '' AND '') 
																											END
														IF @sqlIstNum IS NOT NULL AND @sqlIstNum != ''''
															EXEC sp_executesql @sqlIstNum				
							
														FETCH NEXT FROM RdCursor INTO @condition
													END
						
										END
									----更新回复不符合规范的记录状态
									--set @sql=''''
									--set @sql=''update ''+@tblName+''
									--			set Status=6,ModifiedDate=GETDATE(),ExpiredDate=1
									--			where ISRID in (select ISRID from @tmpTables where ISNUMERIC(Reply)=0)''
									--if @sql is not null and @sql!=''''
									--	EXEC sp_executesql @sql
								END
							ELSE
								BEGIN
									--insert into TM_Act_InstanceSubdivisionResult
									--(
									--	[MemberId]
									--   ,[WfRootId]
									--   ,[Status]
									--   ,[AddedDate]
									--   ,[ModifiedDate]
									--   ,[IsSelected]
									--) 
									--select t.MemberID,c.InstanceStepID,1,GETDATE(),GETDATE(),1
									--from @childSteps c,@tmpTables t
									--where LOWER('',''+c.Condition+'','') like LOWER(''%,''+t.Reply+'',%'') 

									--结果类型为Varchar类型
									--set @sqlInsert=@sqlInsert+''where LOWER('''',''''+c.Condition+'''','''') like LOWER(''''%,''''+t.Reply+'''',%'''') ''
									IF @sqlInsert IS NOT NULL AND @sqlInsert != ''''
										BEGIN
											DECLARE @sqlIstVar NVARCHAR(MAX)
											SET @sqlIstVar = ''''
											SET @sqlIstVar = @sqlInsert + N'' AND LOWER('''','''' + c.Condition + '''','''') LIKE LOWER(''''%,'''' + t.Reply + '''',%'''') ''
											IF @sqlIstVar IS NOT NULL AND @sqlIstVar != ''''
												EXEC sp_executesql @sqlIstVar
										END
								END
						END
				END
		 
			
		END	
	
COMMIT TRANSACTION	--事务提交
--SELECT @sql,@sqlInsert sqlInsert,@sqlIstNum sqlIstNum,@sqlIstVar sqlIstVar

--SELECT * FROM #childSteps

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               );  
--SELECT ERROR_NUMBER() AS ERRORNUMBER, ERROR_MESSAGE() AS ERROR_MESSAGE, ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH












' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullCoupon]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullCoupon]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPullCoupon]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--declare @tmpMemIds TABLE
	--	(
	--		[MemberID] [char](32) NOT NULL
	--	)

	SET @sql = N''WITH C AS
				(
					SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' r.* 
					FROM '' + QUOTENAME(@tblName) + N'' r
						INNER JOIN TM_Mem_CouponPool cp ON r.MemberId = cp.MemberID AND r.WfRootId = cp.WorkflowID
					WHERE r.WfRootId = @wfRootId 
						AND cp.ActInstanceID = @instanceId 
						AND r.ExpiredDate IS NULL
						AND r.Status = 2
						AND r.IsSelected = 1
				)

				UPDATE C
				SET Status = 3
					,ModifiedDate = GETDATE()
					,Reply = ''''1''''''

	 IF @sql IS NOT NULL AND @sql != ''''
	 BEGIN
		EXEC sp_executesql @sql, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER'', @instanceId = @instanceId, @wfRootId = @wfRootId
	 END

	--update TM_Act_InstanceSubdivisionResult
	--set Status=3,ModifiedDate=GETDATE(),Reply=''1''
	--where WfRootId=@wfRootId 
	--	and MemberId in (select * from TM_Mem_CouponPool
	--		where ActInstanceID=@instanceId and WorkflowID=@wfRootId
	--	)
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               );  
--SELECT ERROR_NUMBER() AS ERRORNUMBER
ROLLBACK TRANSACTION	--事物事务

END CATCH









' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullEmail]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPullEmail]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)

	SET @sql = N''WITH C AS
			(
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' r.* 
				FROM '' + QUOTENAME(@tblName) + N'' r
					INNER JOIN TM_Sys_EmailSendingQueue t ON r.MemberId = t.MemberID AND r.WfRootId = t.WorkflowID
				WHERE r.WfRootId = @wfRootId 
					AND t.ActInstanceID = @instanceId
					AND r.Status = 2 
					AND r.IsSelected = 1 
					AND r.ExpiredDate IS NULL
					AND t.IsSent = 1
			)

			 UPDATE C
			 SET Status = 3
				,ModifiedDate = GETDATE()
				,Reply = ''''1''''''

	 IF @sql IS NOT NULL AND @sql != ''''
	 BEGIN
		EXEC sp_executesql @sql, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER'', @instanceId = @instanceId, @wfRootId = @wfRootId
	 END

	--with C as
	--(
	--	select top 1000 r.* from TM_Act_InstanceSubdivisionResult r
	--	inner join TM_Sys_EmailSendingQueue t
	--		on r.MemberId=t.MemberID and r.WfRootId=t.WorkflowID
	--	where r.WfRootId=@wfRootId and t.ActInstanceID=@instanceId
	--)
 
	
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER--,ERROR_MESSAGE() AS ERROR_MESSAGE,ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullNormal]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullNormal]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPullNormal]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	
	SET @sql = N''WITH C AS
				(
					SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' r.* 
					FROM ''+ QUOTENAME(@tblName) + N'' r
					WHERE r.WfRootId = @wfRootId 
						AND r.Status = 2
						AND r.IsSelected = 1 
						AND r.ExpiredDate IS NULL
				)

				UPDATE C
				SET Status = 3
					,ModifiedDate = GETDATE()
					,Reply = ''''1''''''

	 IF @sql IS NOT NULL AND @sql!=''''
	 BEGIN
		EXEC sp_executesql @sql, N''@wfRootId UNIQUEIDENTIFIER'', @wfRootId = @wfRootId
	 END

	--with C as
	--(
	--	select top 1000 r.* from TM_Act_InstanceSubdivisionResult r
	--	where r.WfRootId=@wfRootId and r.Status=2
	--	and r.IsSelected=1 and r.ExpiredDate=0
	--)
	--update C
	--set Status=3,ModifiedDate=GETDATE(),Reply=''1''
	
 
	
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() as ERRORNUMBER
ROLLBACK TRANSACTION	--事物事务

END CATCH










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullOB]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullOB]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPullOB]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--DECLARE @tmpMemIds TABLE
	--	(
	--		[MemberID] [CHAR](32) NOT NULL
	--	)

	SET @sql = N''UPDATE '' + QUOTENAME(@tblName) + N''
				SET Status = 3
					,ModifiedDate = GETDATE()
					,Reply = ob.Feedback
				FROM '' + QUOTENAME(@tblName) + '' r
					, TM_Mem_OB ob
				WHERE r.MemberId = ob.MemberID 
					AND r.WfRootId = ob.WorkflowID
					AND ob.ActInstanceID = @instanceId 
					AND ob.Feedback IS NOT NULL
					AND ob.Feedback != ''''''''
					AND r.ISRID IN 
						(
							SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' ISRID 
							FROM '' + QUOTENAME(@tblName) + N'' rs
							WHERE rs.WfRootId = @wfRootId 
								AND rs.Status = 2
								AND rs.IsSelected = 1 
								AND rs.ExpiredDate IS NULL
						)''

	 IF @sql IS NOT NULL AND @sql != ''''
	 BEGIN
		EXEC sp_executesql @sql, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER'', @instanceId = @instanceId, @wfRootId = @wfRootId
	 END

	--with C as
	--(
	--	select top 1000 r.* from TM_Act_InstanceSubdivisionResult r
	--	inner join TM_Sys_EmailSendingQueue t
	--		on r.MemberId=t.MemberID and r.WfRootId=t.WorkflowID
	--	where r.WfRootId=@wfRootId and t.ActInstanceID=@instanceId
	--)

 
	
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() as ERRORNUMBER
ROLLBACK TRANSACTION	--事物事务

END CATCH










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullSMS]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullSMS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPullSMS]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@resultType NVARCHAR(10),
	@conditions NVARCHAR(MAX),
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)

	IF @resultType IS NULL OR @resultType = '''' OR @conditions IS NULL OR @conditions = ''''
		BEGIN
			SET @sql = N''WITH C AS
						(
							SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' r.* 
							FROM '' + QUOTENAME(@tblName) + N'' r
								INNER JOIN TM_Sys_SMSSendingQueue t ON r.MemberId = t.MemberID AND r.WfRootId = t.WorkflowID
							WHERE r.WfRootId = @wfRootId 
								AND t.ActInstanceID = @instanceId
								AND r.Status = 2 
								AND r.IsSelected = 1 
								AND r.ExpiredDate IS NULL
								AND t.IsSent = 1
						)

						UPDATE C
						SET Status = 3
							,ModifiedDate = GETDATE()
							,Reply = ''''1''''''

			 IF @sql IS NOT NULL AND @sql! = ''''
			 BEGIN
				EXEC sp_executesql @sql, N''@wfRootId UNIQUEIDENTIFIER, @instanceId BIGINT'', @wfRootId = @wfRootId, @instanceId = @instanceId
			 END
		 
			--with C as
			--(
			--	select top 1000 r.* from TM_Act_InstanceSubdivisionResult r
			--	inner join TM_Sys_SMSSendingQueue t
			--		on r.MemberId=t.MemberID and r.WfRootId=t.WorkflowID
			--	where r.WfRootId=@wfRootId and t.ActInstanceID=@instanceId
			--)
		END
	ELSE
		BEGIN
			--DECLARE @tmpTables TABLE
			--	(
			--		[ISRID] bigint not null,
			--		[MemberID] [char](32) NOT NULL,
			--		[ReceivedID] bigint not null,
			--		[Reply] nvarchar(100) not null
			--	)
			IF OBJECT_ID(''tempdb..#tmpTable'') IS NOT NULL
				DROP TABLE #tmpTable
			CREATE TABLE #tmpTable
			(
				[ISRID] bigint not null,
				[MemberID] [char](32) NOT NULL,
				[ReceivedID] bigint not null,
				[Reply] nvarchar(100) not null
			)
			--insert into @tmpTables
			--(ISRID,MemberID,ReceivedID,Reply)
			--select top 1000 a.ISRID,a.MemberId,r.ID,r.Message from TL_Sys_SMSReceivedList r
			--	inner join TM_Sys_SMSSendingQueue s on r.Mobile=s.Mobile
			--	inner join TM_Act_InstanceSubdivisionResult a on s.MemberID=a.MemberId and s.WorkflowID=a.WfRootId
			--where r.ReceivedTime>=s.AddedDate
			--	and r.ActInstanceID is null and r.MemberID is null and r.WorkflowID is null
			--	and s.ActInstanceID=@instanceId and s.IsSent=1
			--	and a.IsSelected=1 and a.ExpiredDate=0 and a.Status=2
			--	and LOWER(@conditions) like LOWER(''%,''+r.Message+'',%'')
			SET @sql = ''''
			SET @sql = N''INSERT INTO #tmpTable
							(ISRID
							,MemberID
							,ReceivedID
							,Reply)
						SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + '' 
							a.ISRID
							,a.MemberId
							,r.ID
							,r.Message 
						FROM TL_Sys_SMSReceivedList r
							INNER JOIN TM_Sys_SMSSendingQueue s ON r.Mobile = s.Mobile
							INNER JOIN '' + QUOTENAME(@tblName) + N'' a ON s.MemberID = a.MemberId AND s.WorkflowID = a.WfRootId
						WHERE r.ReceivedTime >= s.AddedDate
							AND r.ActInstanceID IS NULL 
							AND r.MemberID IS NULL 
							AND r.WorkflowID IS NULL
							AND s.ActInstanceID = @instanceId 
							AND s.IsSent = 1
							AND a.IsSelected = 1 
							AND a.ExpiredDate IS NULL 
							AND a.Status = 2 
							AND a.WfRootId = @wfRootId 
							AND LOWER(@conditions) LIKE LOWER(''''%,'''' + r.Message + '''',%'''')''
		
			IF @sql IS NOT NULL AND @sql != ''''
				BEGIN
					EXEC sp_executesql @sql, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER, @conditions NVARCHAR(MAX)'', @instanceId = @instanceId, @wfRootId = @wfRootId, @conditions = @conditions
				END
		
			IF EXISTS(SELECT * FROM #tmpTable)
				BEGIN

					--更新记录
					SET @sql = ''''
					SET @sql = ''UPDATE '' + QUOTENAME(@tblName) + N''
								SET Status = 3
									,ModifiedDate = GETDATE()
									,Reply = t.Reply
								FROM '' + QUOTENAME(@tblName) + '' a
									, #tmpTable t
								WHERE a.ISRID = t.ISRID''

					 IF @sql IS NOT NULL AND @sql!=''''
						 BEGIN
							EXEC sp_executesql @sql
						 END

					--回写SMS接收表
					UPDATE TL_Sys_SMSReceivedList
					SET ActInstanceID = @instanceId
						,WorkflowID = @wfRootId
						,MemberID = t.MemberID
					FROM TL_Sys_SMSReceivedList r
						,#tmpTable t
					WHERE r.ID = t.ReceivedID
	
				
				END
		END

	--with C as
	--(
	--	select top 1000 r.* from TM_Act_InstanceSubdivisionResult r
	--	inner join TM_Sys_EmailSendingQueue t
	--		on r.MemberId=t.MemberID and r.WfRootId=t.WorkflowID
	--	where r.WfRootId=@wfRootId and t.ActInstanceID=@instanceId
	--)

 
	
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() as ERRORNUMBER,ERROR_MESSAGE() as ERROR_MESSAGE
ROLLBACK TRANSACTION	--事物事务

END CATCH











' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullSurvey]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullSurvey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPullSurvey]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(max)
	--DECLARE @tmpMemIds TABLE
	--	(
	--		[MemberID] [CHAR](32) NOT NULL
	--	)

	SET @sql = N''UPDATE '' + QUOTENAME(@tblName) + N''
				SET Status = 3
					,ModifiedDate = GETDATE()
					,Reply = CAST(q.Score AS NVARCHAR)
				FROM '' + QUOTENAME(@tblName) + N'' r
					, TM_Mem_Question q
				WHERE r.MemberId = q.MemberID 
					AND r.WfRootId = q.WorkflowID
					AND q.ActInstanceID = @instanceId 
					AND q.Status > 0
					AND r.ISRID IN 
						(
							SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' ISRID 
							FROM '' + QUOTENAME(@tblName) + N'' rs
							WHERE rs.WfRootId = @wfRootId 
								AND rs.Status = 2
								AND rs.IsSelected = 1 
								AND rs.ExpiredDate IS NULL
						)''

	 IF @sql IS NOT NULL AND @sql != ''''
	 BEGIN
		EXEC sp_executesql @sql, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER'', @instanceId = @instanceId, @wfRootId = @wfRootId
	 END

	--with C as
	--(
	--	select top 1000 r.* from TM_Act_InstanceSubdivisionResult r
	--	inner join TM_Sys_EmailSendingQueue t
	--		on r.MemberId=t.MemberID and r.WfRootId=t.WorkflowID
	--	where r.WfRootId=@wfRootId and t.ActInstanceID=@instanceId
	--)

	--update TM_Act_InstanceSubdivisionResult
	--set Status=3,ModifiedDate=GETDATE(),Reply=cast(q.Score as nvarchar)
	--from TM_Act_InstanceSubdivisionResult r, TM_Mem_Question q
	--where r.MemberId=q.MemberID and r.WfRootId=q.WorkflowID
	--	and q.ActInstanceID=@instanceId 
	--	and r.ISRID in (select top 1000 ISRID 
	--		from TM_Act_InstanceSubdivisionResult rs
	--		where rs.WfRootId=@wfRootId and rs.Status=3
	--			and rs.IsSelected=1 and rs.ExpiredDate=0)
 
	
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() as ERRORNUMBER
ROLLBACK TRANSACTION	--事物事务

END CATCH










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPullWechat]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPullWechat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPullWechat]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)

	SET @sql = N''WITH C AS
			(
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' r.* 
				FROM '' + QUOTENAME(@tblName) + N'' r
					INNER JOIN TM_Sys_WechatSendingQueue t ON r.MemberId = t.MemberID AND r.WfRootId = t.WorkflowID
				WHERE r.WfRootId = @wfRootId 
					AND t.ActInstanceID = @instanceId
					AND r.Status = 2 
					AND r.IsSelected = 1 
					AND r.ExpiredDate IS NULL
					AND t.IsSent = 1
			)

			 UPDATE C
			 SET Status = 3
				,ModifiedDate = GETDATE()
				,Reply = ''''1''''''

	 IF @sql IS NOT NULL AND @sql != ''''
	 BEGIN
		EXEC sp_executesql @sql, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER'', @instanceId = @instanceId, @wfRootId = @wfRootId
	 END

	--with C as
	--(
	--	select top 1000 r.* from TM_Act_InstanceSubdivisionResult r
	--	inner join TM_Sys_EmailSendingQueue t
	--		on r.MemberId=t.MemberID and r.WfRootId=t.WorkflowID
	--	where r.WfRootId=@wfRootId and t.ActInstanceID=@instanceId
	--)
 
	
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER--,ERROR_MESSAGE() AS ERROR_MESSAGE,ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH













' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushCoupon]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushCoupon]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushCoupon]    Script Date: 2015/4/16 9:58:43 ******/
CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPushCoupon]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@templetId INT,
	@couponType NVARCHAR(20),
	@couponStart datetime,
	@couponEnd datetime,
	@offNumber int,
	@unit NVARCHAR(20),
	@IsSMS BIT,
	@IsEmail BIT,
	@templateSMS NVARCHAR(500),
	@emailSubject NVARCHAR(200),
	@templateEmail NVARCHAR(MAX),
	@maxCount INT,
	@isOthers bit
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--declare @tmpMemIds TABLE
	--	(
	--		[MemberID] [char](32) NOT NULL
    --    )
    --IF OBJECT_ID(''tempdb..#tmpTableCoupon'') is not null
    --drop table #tmpTableCoupon
    IF OBJECT_ID(''tempdb..#tmpMemIdsCoupon'') IS NOT NULL
        DROP TABLE #tmpMemIdsCoupon
    CREATE TABLE #tmpMemIdsCoupon
    (   [ID]     [int] IDENTITY(1,1) ,
        [ISRID] BIGINT NOT NULL,
        [MemberID] [CHAR](32) NOT NULL,
        [CouponCode] [nvarchar](50) NULL
    )

    Declare @couponName nvarchar(200)
    SET @sql = ''INSERT INTO #tmpMemIdsCoupon
                    (ISRID
                    ,MemberID
                    )
                SELECT TOP '' + CAST(@maxCount AS NVARCHAR(10)) + N'' 
                    ISRID
                    ,MemberId
            
				FROM '' + QUOTENAME(@tblName) + N''
				WHERE IsSelected = 1 
					AND ExpiredDate IS NULL 
					AND Status = 1 
					AND WfRootId = @wfRootId ''

	EXEC sp_executesql @sql, N''@wfRootId UNIQUEIDENTIFIER'', @wfRootId = @wfRootId

	select @couponName= Name from TM_Act_CommunicationTemplet where TempletID=@templetId
	
	DECLARE @dataGroupID int 
	select @dataGroupID=DataGroupID from TM_Act_Instance where ActivityInstanceID=@instanceId

   if EXISTS(SELECT 1 FROM #tmpMemIdsCoupon)
		BEGIN
			if(@isOthers = 1)
				begin
					declare @execSqlForOtherCoupon nvarchar(max)
					set @execSqlForOtherCoupon = ''
					select top ''+ CAST(@maxCount as nvarchar(10)) +'' ROW_NUMBER() over(order by newid()) as indexID,* 
					into #tempPoolList 
					from TM_Mem_CouponPool
					where memberid is null and TempletID = ''+convert(nvarchar(50),@templetId)+''

					select ROW_NUMBER() over(order by newid()) as indexID,*
					into #tempMemberToCoupon
					from #tmpMemIdsCoupon

					update t1 
					set t1.CouponCode = t3.CouponCode
					from #tmpMemIdsCoupon t1
					inner join #tempMemberToCoupon t2 on t1.ISRID = t2.ISRID
					inner join #tempPoolList t3 on t2.indexID = t3.indexID

					update t5
					set t5.MemberID = t8.MemberID,
						t5.WorkflowID = @wfRootId,
						t5.ActInstanceID = @instanceId
					from TM_Mem_CouponPool t5
					inner join #tempPoolList t6 on t5.CouponID = t6.CouponID
					inner join #tempMemberToCoupon t7 on t6.indexID = t7.indexID
					inner join  #tmpMemIdsCoupon t8 on t7.ISRID = t8.ISRID''
					
					exec sp_executesql @execSqlForOtherCoupon, N''@wfRootId UNIQUEIDENTIFIER, @instanceId BIGINT'', 
						@wfRootId = @wfRootId,@instanceId=@instanceId
				end
			else
				begin
				DECLARE @sqlCoupon AS nvarchar(max)
						--生成优惠券
						set @sqlCoupon=''
						truncate table TE_SYS_CouponCode 
						insert into TE_SYS_CouponCode
						select  top ''+ CAST(@maxCount as nvarchar(10)) +'' *  
						from TD_SYS_CouponCode
						where Isused=0 
						
						
						
						
						INSERT INTO [dbo].[TM_Mem_CouponPool]
							   ([CouponCode]
							   ,[MemberID]
							   ,[TempletID]
							   ,[CouponType]
							   ,[StartDate]
							   ,[EndDate]
							   ,[Enable]
							   ,[IsUsed]
							   ,[AddedDate]
							   ,[ActInstanceID]
							   ,[WorkflowID]                   
							   )

						SELECT  b.CouponCode
							   ,MemberID
							   ,@templetId
							   ,@couponType
							   ,@couponStart
							   ,@couponEnd
							   ,1
							   ,0
							   ,GETDATE()
							   ,@instanceId
							   ,@wfRootId
                  
						 from #tmpMemIdsCoupon  a
						 inner join  (select b.*,ROW_NUMBER() over(order by serial )  ID
									  from   TE_SYS_CouponCode b
									  )   b on a.id=b.id  

						------------更新临时表中的CouponCode 

						update  #tmpMemIdsCoupon set CouponCode=b.CouponCode 
						from (select b.*,ROW_NUMBER() over(order by serial )  ID
									  from   TE_SYS_CouponCode b
									  ) b

						where #tmpMemIdsCoupon.id=b.id  

						--insert into tmpMemIdsCoupon
						--select * from #tmpMemIdsCoupon 
						 -----------更新优惠券编码已使用的状态

						 update TD_SYS_CouponCode set Isused=1 
						 from TE_SYS_CouponCode b 
						 where TD_SYS_CouponCode.Isused=0  and TD_SYS_CouponCode.CouponCode=b.CouponCode 

						 ''
							 --print @unit
							 --print @offNumber
							 --print @sqlCoupon
			 
								   --, Convert(datetime,Convert(varchar(10),dateadd(day,1,getdate()),121))
								   --,DATEADD(''+@unit+'',@offNumber, Convert(datetime,Convert(varchar(10),dateadd(day,1,getdate()),121)))
							 if @offNumber is not null and @unit is not null and @unit != ''''
							 begin
								 if @couponStart is null or @couponStart < GETDATE()
									 set @couponStart = GETDATE()
								 declare @tempDate datetime
								 declare @sqlDate nvarchar(200)
								 set @sqlDate = N''set @tempDate = DATEADD('' + @unit + N'',@offNumber,@couponStart)''
								 EXEC sp_executesql @sqlDate, N''@tempDate datetime output, @offNumber int, @couponStart datetime'', @tempDate = @tempDate output, @offNumber = @offNumber, @couponStart = @couponStart
								 if @tempDate is not null and @couponEnd is not null and @tempDate < @couponEnd
									 set @couponEnd = @tempDate
							 end

							 set @couponEnd=convert(varchar(10),@couponEnd,111)+'' 23:59:59''
			 
							 EXEC sp_executesql @sqlCoupon, N''@templetId INT, @couponType NVARCHAR(20), @instanceId BIGINT,@wfRootId UNIQUEIDENTIFIER, @couponStart datetime, @couponEnd datetime'', @templetId = @templetId,@couponType=@couponType,@instanceId=@instanceId, @wfRootId = @wfRootId, @couponStart = @couponStart, @couponEnd = @couponEnd

							--Delete by Richard 20151116
							--INSERT INTO [dbo].[TM_Mem_CouponLimit] ([CouponID],[LimitType],[LimitValue],[AddedDate]) 
							--SELECT A.CouponID,B.LimitType,B.LimitValue,GETDATE() 
							--FROM TD_SYS_CouponLimit B 
							--LEFT JOIN [dbo].[TM_Mem_CouponPool] A ON B.CouponID=A.TempletID and A.CouponID IS NOT NULL
							--WHERE B.CouponID=@templetId and A.WorkflowID = @wfRootId AND A.ActInstanceID = @instanceId

							-- Append By Leo,2015-7-1 Start-----
							--DECLARE @limitType nvarchar(50)
							--DECLARE @limitValues nvarchar(500)
							--DECLARE @limitValue nvarchar(50)
							--DECLARE @limitIndex int
			
							--IF OBJECT_ID(''tempdb..#tmpdataLimit'') IS NOT NULL
							--	DROP TABLE #tmpdataLimit
							--CREATE TABLE #tmpdataLimit
							--(
							--	LimitType [nvarchar](50) NOT NULL,
							--	LimitValue [nvarchar](50) NOT NULL
							--)

							--SELECT @limitType = [LimitType], @limitValues = [LimitValue] FROM [dbo].[TM_Act_InstanceStep] WHERE [InstanceStepID] = @wfRootId AND [ActivityInstanceID] = @instanceId

							--IF @limitType IS NOT NULL AND @limitValues IS NOT NULL
							--BEGIN 
							--	SET @limitIndex = CHARINDEX('','',@limitValues)
							--	WHILE @limitIndex <> 0
							--	BEGIN
							--		SET @limitValue = SUBSTRING(@limitValues,1,@limitIndex - 1)
							--		SET @limitValues = STUFF(@limitValues,1,@limitIndex,'''')
							--		INSERT INTO #tmpdataLimit VALUES(@limitType,@limitValue)
							--		SET @limitIndex = CHARINDEX('','',@limitValues)
							--	END
							--	INSERT INTO #tmpdataLimit VALUES(@limitType,@limitValues)
							--END

							--INSERT INTO [dbo].[TM_Mem_CouponLimit] ([CouponID],[LimitType],[LimitValue],[AddedDate]) 
							--SELECT A.CouponID,B.LimitType,B.LimitValue,GETDATE() 
							--FROM #tmpdataLimit B 
							--LEFT JOIN [dbo].[TM_Mem_CouponPool] A ON A.CouponID IS NOT NULL
							--WHERE A.WorkflowID = @wfRootId AND A.ActInstanceID = @instanceId
							-- Append By Leo,2015-7-1 End-----
			end
			
            --print 132
            --替换短信和邮件内容的通配符
            IF @templateSMS IS NULL
                SET @templateSMS=''''
            IF @templateEmail IS NULL
                SET @templateEmail=''''
            IF @IsSMS = 1 AND @templateSMS != '''' OR @IsEmail = 1 AND @templateEmail != ''''
                BEGIN
                    DECLARE @sqlSMS AS nvarchar(max)
                    DECLARE @sqlEmail AS nvarchar(max)
                   SET @sqlSMS = N''INSERT INTO [dbo].[TM_Sys_SMSSendingQueue]
										([Mobile]
										,[Message]
										,[MsgPara]
										,[MemberID]
										,[ActInstanceID]
										,[WorkflowID]
										,[TempletID]
										,[AddedDate]
										,[AddedUser]
										,[Remark]
										,[Channel]
										,[IsSent])
									SELECT v_u.CustomerMobile
										,'''''' + @templateSMS + N''''''
										,''''{"CouponName":"''+@couponName+''"}''''
										,v_u.MemberID
										,@instanceId
										,@wfRootId
										,@templetId
										,GETDATE()
										,''''ActEngine''''
										,''''市场活动''''
										,''''2''''
										,0
									FROM V_U_TM_Mem_Info v_u 
										INNER JOIN (select * from #tmpMemIdsCoupon where CouponCode is not null and CouponCode <>'''''''') m ON v_u.MemberID = m.MemberID ''
					SET @sqlEmail = N''INSERT INTO [dbo].[TM_Sys_EmailSendingQueue]
										  ([Email]
										  ,[Subject]
										  ,[Message]
										  ,[MemberID]
										  ,[ActInstanceID]
										  ,[WorkflowID]
										  ,[TempletID]
										  ,[AddedDate]
										  ,[AddedUser]
										  ,[IsSent])
									  SELECT v_u.CustomerEmail,
										  '''''' + @emailSubject + N''''''
										  ,'''''' + @templateEmail + N''''''
										  ,v_u.MemberID
										  ,@instanceId
										  ,@wfRootId
										  ,@templetId
										  ,GETDATE()
										  ,''''ActEngine'''' 
										  ,0
									  FROM V_U_TM_Mem_Info v_u 
										  INNER JOIN (select * from #tmpMemIdsCoupon where CouponCode is not null and CouponCode <>'''''''' ) m ON v_u.MemberID=m.MemberID ''
					--替换短信和邮件内容中的CouponCode
					IF @templateSMS LIKE ''%{CouponName}%'' OR @templateEmail LIKE ''%{CouponName}%''
						BEGIN
							SET @sqlSMS = REPLACE(@sqlSMS, N''{CouponCode}'', N'''''' + m.CouponCode + '''''')
							SET @sqlEmail = REPLACE(@sqlEmail, N''{CouponCode}'', N'''''' + m.CouponCode + '''''')
							
							SET @sqlSMS = REPLACE(@sqlSMS, N''{CouponName}'', @couponName)
							SET @sqlEmail = REPLACE(@sqlEmail, N''{CouponName}'', @couponName)
						END
					--替换短信和邮件内容中的会员信息
					IF @templateSMS LIKE ''%{%'' OR @templateEmail LIKE ''%{%''
						BEGIN
							DECLARE @fieldAlias AS nvarchar(50)
							DECLARE @dictName AS nvarchar(50)
							DECLARE @dictType AS nvarchar(50)
                            DECLARE @i AS int
                            DECLARE @tempName AS nvarchar(20)
                            SET @i = 1
                            DECLARE RdCursor CURSOR FOR
                                SELECT [FieldAlias], [DictTableName], [DictTableType] 
                                FROM TD_SYS_FieldAlias 
                                WHERE IsCommunicationTemplet = 1
                            OPEN RdCursor
                                FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
                            WHILE (@@FETCH_STATUS = 0)
                                BEGIN
                                    if @dictName = ''TD_SYS_BizOption''
                                        BEGIN
                                            SET @tempName = ''d'' + CAST(@i AS nvarchar(10))
											SET @sqlSMS = REPLACE(@sqlSMS, N''{'' + @fieldAlias + ''}'', N'''''' + CASE WHEN '' + @tempName + N''.OptionText IS NULL THEN '''''''' ELSE '' + @tempName + N''.OptionText END + '''''')
											SET @sqlSMS = @sqlSMS + N'' LEFT JOIN (SELECT * FROM TD_SYS_BizOption WHERE DataGroupID=''''''+Convert(varchar,@dataGroupID)+'''''' and OptionType = '''''' + @dictType + N'''''') '' + @tempName + N'' ON v_u.CustomerLevel = '' + @tempName + N''.OptionValue''
											SET @sqlEmail = REPLACE(@sqlEmail, ''{'' + @fieldAlias + ''}'', N'''''' + CASE WHEN '' + @tempName + N''.OptionText IS NULL THEN '''''''' ELSE '' + @tempName + N''.OptionText END + '''''')
											SET @sqlEmail = @sqlEmail+N'' LEFT JOIN (SELECT * FROM TD_SYS_BizOption WHERE DataGroupID=''''''+Convert(varchar,@dataGroupID)+'''''' and OptionType = '''''' + @dictType + N'''''') '' + @tempName + N'' ON v_u.CustomerLevel = '' + @tempName + N''.OptionValue''
										END
									ELSE
										BEGIN
											SET @sqlSMS = REPLACE(@sqlSMS, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN '''''''' ELSE CAST(v_u.'' + @fieldAlias + N'' AS NVARCHAR(MAX)) END + '''''')
											SET @sqlEmail = REPLACE(@sqlEmail, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN '''''''' ELSE CAST(v_u.'' + @fieldAlias + N'' AS NVARCHAR(MAX)) END + '''''')
										END
									SET @i = @i + 1
									FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
								END
							CLOSE RdCursor
							DEALLOCATE RdCursor
						END
				END

			--发送短信
			IF @IsSMS = 1 AND @templateSMS IS NOT NULL AND @templateSMS!=''''
			BEGIN
				SET @sqlSMS=@sqlSMS + '' WHERE v_u.CustomerMobile IS NOT NULL AND v_u.CustomerMobile != ''''''''''
				print @sqlSMS
				EXEC sp_executesql @sqlSMS, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER, @templetId INT'', @instanceId = @instanceId, @wfRootId = @wfRootId, @templetId = @templetId
			END

			--发送邮件
			IF @IsEmail = 1 AND @templateEmail IS NOT NULL AND @templateEmail != ''''
			BEGIN
				SET @sqlEmail = @sqlEmail + '' WHERE v_u.CustomerEmail IS NOT NULL AND v_u.CustomerEmail != ''''''''''
				EXEC sp_executesql @sqlEmail, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER, @templetId INT'', @instanceId = @instanceId, @wfRootId = @wfRootId, @templetId = @templetId
			END

			--修改状态
			DECLARE @sqlUpdate AS nvarchar(max)
			SET @sqlUpdate = N''UPDATE ''+ QUOTENAME(@tblName) + N'' 
							 SET Status = 2
								 , ModifiedDate = GETDATE()
								 , StartDate = GETDATE()
							 WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsCoupon)''
			EXEC sp_executesql @sqlUpdate
	
		END
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER,ERROR_MESSAGE() AS ERROR_MESSAGE,ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH









' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushEmail]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPushEmail]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@templetId INT,
	@subjectEmail NVARCHAR(200),
	@templateEmail NVARCHAR(MAX),
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--declare @tmpMemIds TABLE
	--	(
	--		[MemberID] [char](32) NOT NULL
	--	)
	--IF OBJECT_ID(''tempdb..#tmpTableCoupon'') is not null
	--drop table #tmpTableCoupon
	IF OBJECT_ID(''tempdb..#tmpMemIdsEmail'') IS NOT NULL
		DROP TABLE #tmpMemIdsEmail
	CREATE TABLE #tmpMemIdsEmail
	(
		[ISRID] BIGINT NOT NULL,
		[MemberID] [CHAR](32) NOT NULL,
		[HasEmail] [BIT] NOT NULL
	)

	SET @sql = N''INSERT INTO #tmpMemIdsEmail
					(ISRID
					,MemberID
					,HasEmail)
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' 
					t.ISRID 
					,t.MemberId 
					,CASE ISNULL(v_u.CustomerEmail,'''''''') WHEN '''''''' THEN 0 ELSE 1 END
				FROM [''+@tblName+N''] t
					INNER JOIN V_U_TM_Mem_Info v_u ON t.MemberID = v_u.MemberID
				WHERE t.IsSelected = 1 
					AND t.ExpiredDate IS NULL 
					AND t.Status = 1 
					AND t.WfRootId = @wfRootId ''
					PRINT @sql
	EXEC sp_executesql @sql, N''@wfRootId UNIQUEIDENTIFIER'', @wfRootId = @wfRootId

    DECLARE @dataGroupID int 
	select @dataGroupID=DataGroupID from TM_Act_Instance where ActivityInstanceID=@instanceId

	IF EXISTS(SELECT * FROM #tmpMemIdsEmail)
		BEGIN
	

			--declare @subjectEmail NVARCHAR(100)
			--declare @templateEmail NVARCHAR(MAX)
			--declare @maxCount INT
			--set @maxCount=1000
			--set @subjectEmail=N''yyb邮件会员关怀3''
			--set @templateEmail=N''<p>yyb邮件会员关怀3</p>
			--					<p>&nbsp;</p>
			--					<p>yyb邮件会员关怀3</p>
			--					<p>&nbsp;</p>
			--					<p>&nbsp;</p>
			--					<p>yyb邮件会员关怀3</p>
			--					<p>&nbsp;</p>
			--					<p>yyb邮件会员关怀3</p>
			--					<p>&nbsp;</p>
			--					<p>{CustomerLevel}</p>
			--					<p>{MemberCardNo}</p>
			--					<p>{RegisterDate}</p>
			--					<p>{CustomerEmail}</p>''
			--	set @maxCount = 1000

			--生成发送邮件记录的sql
			DECLARE @sqlEmail AS NVARCHAR(MAX)
			SET @sqlEmail = N''INSERT INTO [dbo].[TM_Sys_EmailSendingQueue]
								([Email]
								,[Subject]
								,[Message]
								,[MemberID]
								,[ActInstanceID]
								,[WorkflowID]
								,[TempletID]
								,[IsSent]
								,[AddedDate]
								,[AddedUser])
							SELECT v_u.CustomerEmail
								,'''''' + @subjectEmail + N''''''
								,'''''' + @templateEmail + N''''''
								,v_u.MemberID
								,@instanceId
								,@wfRootId
								,@templetId
								,0,GETDATE()
								,''''ActEngine'''' 
							FROM V_U_TM_Mem_Info v_u 
								INNER JOIN #tmpMemIdsEmail m ON v_u.MemberID = m.MemberID ''

			--替换邮件内容的通配符
			IF @subjectEmail LIKE ''%{%'' OR @templateEmail LIKE ''%{%''
				BEGIN
					DECLARE @fieldAlias AS NVARCHAR(50)
					DECLARE @dictName AS NVARCHAR(50)
					DECLARE @dictType AS NVARCHAR(50)
					DECLARE @fieldType AS NVARCHAR(10)
					DECLARE @i AS INT
					DECLARE @tempName AS NVARCHAR(10)
					SET @i = 1
					DECLARE RdCursor CURSOR FOR
						SELECT [FieldAlias], [DictTableName], [DictTableType], [FieldType]
						FROM TD_SYS_FieldAlias 
						WHERE IsCommunicationTemplet = 1
					OPEN RdCursor
						FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
					WHILE (@@FETCH_STATUS = 0)
						BEGIN 
							IF @dictName=''TD_SYS_BizOption''
								BEGIN
									SET @tempName = ''d'' + CAST(@i AS NVARCHAR(5))
									SET @sqlEmail = REPLACE(@sqlEmail, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @tempName + N''.OptionText IS NULL THEN '''''''' ELSE '' + @tempName + N''.OptionText END + '''''')
									SET @sqlEmail = @sqlEmail + N'' LEFT JOIN (SELECT * FROM TD_SYS_BizOption WHERE DataGroupID=''''''+Convert(varchar,@dataGroupID)+'''''' and  OptionType = '''''' + @dictType + N'''''') '' + @tempName + N'' ON v_u.CustomerLevel = '' + @tempName + N''.OptionValue''
								END
							ELSE
								BEGIN
									IF(@fieldType =''5'' or @fieldType=''6'')
									BEGIN 
										SET @sqlEmail = REPLACE(@sqlEmail, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN ''''''''  ELSE CONVERT(varchar(10),v_u.'' + @fieldAlias + N'' ,111) END + '''''')
									END
									ELSE
									BEGIN
										SET @sqlEmail = REPLACE(@sqlEmail, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN '''''''' ELSE CAST(v_u.'' + @fieldAlias + N'' AS NVARCHAR(MAX)) END + '''''')
									END
								END
							SET @i = @i + 1
							FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
						END
					CLOSE RdCursor
					DEALLOCATE RdCursor
				END	


			--发送邮件
			--SET @sqlEmail=@sqlEmail+N'' WHERE v_u.CustomerEmail IS NOT NULL AND v_u.CustomerEmail!=''''''''''
			print @sqlEmail
			SET @sqlEmail = @sqlEmail + '' WHERE m.HasEmail = 1''
			EXEC sp_executesql @sqlEmail, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER, @templetId INT'', @instanceId = @instanceId, @wfRootId = @wfRootId, @templetId = @templetId
		
			--修改状态
			DECLARE @sqlUpdate AS NVARCHAR(MAX)
			SET @sqlUpdate = N''UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 2
									, ModifiedDate = GETDATE()
									, StartDate = GETDATE()
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsEmail WHERE HasEmail = 1)

								UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 5
									, ModifiedDate = GETDATE()
									, Remark = ''''发送过期-电子邮箱为空''''
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsEmail WHERE HasEmail != 1)''
			EXEC sp_executesql @sqlUpdate
	
		END
COMMIT TRANSACTION	--事务提交
SELECT 1--, @sql Sql, @sqlEmail SqlEmail, @sqlUpdate SqlUpdate
--select @sqlEmail SqlEmail, @sqlUpdate SqlUpdate

--select * from #tmpMemIdsEmail

END TRY
BEGIN CATCH	
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER, ERROR_MESSAGE() AS ERROR_MESSAGE, ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH









' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushNormal]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushNormal]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPushNormal]
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--declare @tmpMemIds TABLE
	--	(
	--		[MemberID] [char](32) NOT NULL
	--	)
	--IF OBJECT_ID(''tempdb..#tmpTableCoupon'') is not null
	--drop table #tmpTableCoupon
	IF OBJECT_ID(''tempdb..#tmpMemIdsSMS'') IS NOT NULL
		DROP TABLE #tmpMemIdsNormal
	CREATE TABLE #tmpMemIdsNormal
	(
		[ISRID] BIGINT NOT NULL,
		[MemberID] [CHAR](32) NOT NULL
	)

	SET @sql = N''INSERT INTO #tmpMemIdsNormal
					 (ISRID
					 ,MemberID)
				 SELECT TOP ''+CAST(@maxCount AS NVARCHAR) + N'' 
					 ISRID
					 ,MemberId 
				 FROM '' + QUOTENAME(@tblName) + N''
				 WHERE IsSelected = 1 
					 AND ExpiredDate IS NULL 
					 AND Status = 1 
					 AND WfRootId = @wfRootId''

	EXEC sp_executesql @sql, N''@wfRootId uniqueidentifier'', @wfRootId = @wfRootId

	IF EXISTS(SELECT * FROM #tmpMemIdsNormal)
		BEGIN

			--修改状态
			DECLARE @sqlUpdate AS NVARCHAR(MAX)
			SET @sqlUpdate=N''UPDATE ['' + @tblName + N''] 
							SET Status = 2
								, ModifiedDate = GETDATE()
								, StartDate = GETDATE()
							WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsNormal)''
			EXEC sp_executesql @sqlUpdate
	
		END
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH

	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER,ERROR_MESSAGE() AS ERROR_MESSAGE,ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH









' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushOB]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushOB]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPushOB]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@obContent NVARCHAR(MAX),
	@foreignTempletID INT,
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(max)
	--DECLARE @tmpMemIds TABLE
	--	(
	--		[MemberID] [char](32) NOT NULL
	--	)
	IF OBJECT_ID(''tempdb..#tmpMemIdsOB'') IS NOT NULL
		DROP TABLE #tmpMemIdsOB

	--创建临时表，用来存储要处理的MemberId
	CREATE TABLE #tmpMemIdsOB
	(
		[ISRID] BIGINT NOT NULL,
		[MemberID] [CHAR](32) NOT NULL,
		[HasMobile] [BIT] NOT NULL
	)

	--SET @sql = N''INSERT INTO #tmpMemIdsOB
	--			SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' MemberId 
	--			FROM '' + QUOTENAME(@tblName) + ''
	--			WHERE IsSelected = 1 
	--				AND ExpiredDate IS NULL 
	--				AND Status = 1 
	--				AND WfRootId = @wfRootId ''
	SET @sql = N''INSERT INTO #tmpMemIdsOB
				(ISRID
				,MemberID
				,HasMobile)
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N''
					t.ISRID 
					,t.MemberId 
					,CASE ISNULL(v_u.CustomerMobile,'''''''') WHEN '''''''' THEN 0 ELSE 1 END
				FROM '' + QUOTENAME(@tblName) + N'' t
					INNER JOIN V_U_TM_Mem_Info v_u ON t.MemberID = v_u.MemberID
				WHERE t.IsSelected = 1 
					AND t.ExpiredDate IS NULL 
					AND t.Status = 1 
					AND t.WfRootId = @wfRootId ''
					--AND v_u.CustomerMobile IS NOT NULL 
					--AND v_u.CustomerMobile != ''''''''''

	EXEC sp_executesql @sql, N''@wfRootId uniqueidentifier'', @wfRootId = @wfRootId

	IF EXISTS(SELECT * FROM #tmpMemIdsOB)
		BEGIN

			--INSERT INTO [dbo].[TM_Mem_OB]
			--	   ([MemberID]
			--	   ,[PhoneNo]
			--	   ,[ActInstanceID]
			--	   ,[WorkflowID]
			--	   ,[OBContent]
			--	   ,[Status]
			--	   ,[AddedDate]
			--	   ,[AddedUser]
			--	   ,[ModifiedDate]
			--	   ,[ModifiedUser])
			--SELECT v_u.MemberID
			--		,v_u.CustomerMobile
			--		,@instanceId
			--		,@wfRootId
			--		,@obContent
			--		,0
			--		,GETDATE()
			--		,''ActEngine''
			--		,GETDATE()
			--		,''ActEngine''
			--FROM V_U_TM_Mem_Info v_u 
			--INNER JOIN #tmpMemIdsOB m ON v_u.MemberID = m.MemberID

			--替换外呼内容的通配符
			DECLARE @sqlOB AS NVARCHAR(MAX)
			SET @sqlOB = N''INSERT INTO [dbo].[TM_Mem_OB]
								([MemberID]
								,[PhoneNo]
								,[ActInstanceID]
								,[WorkflowID]
								,[OBContent]
								,[Status]
								,[AddedDate]
								,[AddedUser]
								,[ModifiedDate]
								,[ModifiedUser])
							SELECT v_u.MemberID
								,v_u.CustomerMobile
								,@instanceId
								,@wfRootId
								,''''''+@obContent+''''''
								,0
								,GETDATE()
								,''''ActEngine''''
								,GETDATE()
								,''''ActEngine''''
							FROM V_U_TM_Mem_Info v_u 
								INNER JOIN #tmpMemIdsOB m ON v_u.MemberID = m.MemberID ''
	
			DECLARE @fieldAlias AS NVARCHAR(50)
			DECLARE @dictName AS NVARCHAR(50)
			DECLARE @dictType AS NVARCHAR(50)
			DECLARE @i AS INT
			DECLARE @tempName AS NVARCHAR(10)
			SET @i = 1
			DECLARE RdCursor CURSOR FOR
			SELECT [FieldAlias], [DictTableName], [DictTableType] FROM TD_SYS_FieldAlias WHERE IsCommunicationTemplet = 1
			OPEN RdCursor
			FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
			WHILE (@@FETCH_STATUS = 0)
				BEGIN 
					IF @dictName=''TD_SYS_BizOption''
							BEGIN
								SET @tempName = ''d'' + CAST(@i AS NVARCHAR(5))
								SET @sqlOB = REPLACE(@sqlOB, N''{'' + @fieldAlias + N''}'', N''''''+CASE WHEN '' + @tempName + N''.OptionText IS NULL THEN '''''''' ELSE '' + @tempName + N''.OptionText END + '''''')
								SET @sqlOB = @sqlOB + N'' LEFT JOIN (SELECT * FROM TD_SYS_BizOption WHERE OptionType = ''''''+@dictType + N'''''') '' + @tempName + N'' ON v_u.CustomerLevel = '' + @tempName + N''.OptionValue ''
							END
						ELSE
							BEGIN
								SET @sqlOB = REPLACE(@sqlOB, ''{'' + @fieldAlias + ''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN '''''''' ELSE CAST(v_u.'' + @fieldAlias + N'' AS NVARCHAR(MAX)) END + '''''')
							END
					SET @i = @i + 1
					FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
				END
			CLOSE RdCursor
			DEALLOCATE RdCursor

			--发送外呼
			--set @sqlOB = @sqlOB+'' where v_u.CustomerMobile is not null and v_u.CustomerMobile!=''''''''''
			SET @sqlOB = @sqlOB + '' WHERE m.HasMobile = 1''
			EXEC sp_executesql @sqlOB, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER'', @instanceId = @instanceId, @wfRootId = @wfRootId

			--发送问卷
			IF @foreignTempletID IS NOT NULL 
				AND EXISTS(SELECT * FROM TM_Act_CommunicationTemplet WHERE TempletID = @foreignTempletID AND Type = ''Question'')
			BEGIN
				--问卷内容
				DECLARE @question AS NVARCHAR(MAX)
				SELECT @question = BasicContent 
					FROM TM_Act_CommunicationTemplet
					WHERE TempletID = @foreignTempletID
				IF @question IS NULL OR @question = ''''
					SET @question = ''null''
				SET @question = N''{"questions":'' + @question +''}''

				--发送
				INSERT INTO [dbo].[TM_Mem_Question]
				   ([MemberID]
				   ,[ActInstanceID]
				   ,[WorkflowID]
				   ,[SendChannel]
				   ,[TempletId]
				   ,[Question]
				   ,[Score]
				   ,[Status]
				   ,[AddedDate]
				   ,[AddedUser]
				   ,[ModifiedDate]
				   ,[ModifiedUser])
			    SELECT MemberID
				   ,@instanceId
				   ,@wfRootId
				   ,''1''
				   ,@foreignTempletID
				   ,@question
				   ,0
				   ,0
				   ,GETDATE()
				   ,''ActEngine''
				   ,GETDATE()
				   ,''ActEngine''
			    FROM #tmpMemIdsOB
			END

			--修改状态
			DECLARE @sqlUpdate AS NVARCHAR(MAX)
			SET @sqlUpdate = N''UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 2
									, ModifiedDate = GETDATE()
									, StartDate = GETDATE()
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsOB WHERE HasMobile = 1)

								UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 5
									, ModifiedDate = GETDATE()
									, Remark = ''''发送过期-手机号为空''''
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsOB WHERE HasMobile != 1)''
			EXEC sp_executesql @sqlUpdate
	
		end
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH

	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() as ERRORNUMBER
ROLLBACK TRANSACTION	--事物事务

END CATCH









' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushSMS]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushSMS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
--select * from TM_Sys_SMSSendingQueue where MsgPara is not null 

--select * from  TM_Act_Instance where ActivityName=''yyb6.5市场活动003''
--select templetid  from TM_Act_Master where Activityid=93

--1000264   TM_Act_InstanceSubdivisionResult_55253BB9-1944-49E4-9A98-0F724F651EE5

--select * from TM_Act_CommunicationTemplet 



/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushSMS]    Script Date: 2015/4/16 9:58:43 ******/
CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPushSMS]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,   
	@templetId INT,   
	@templateSMS NVARCHAR(500),
	@templatePara NVARCHAR(500)='''',
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--declare @tmpMemIds TABLE
	--	(
	--		[MemberID] [char](32) NOT NULL
	--	)
	IF OBJECT_ID(''tempdb..#tmpMemIdsSMS'') IS NOT NULL
		DROP TABLE #tmpMemIdsSMS
	CREATE TABLE #tmpMemIdsSMS
	(
		[ISRID] BIGINT NOT NULL,
		[MemberID] [CHAR](32) NOT NULL,
		[HasMobile] [BIT] NOT NULL
	)

	
	SET @sql = N''INSERT INTO #tmpMemIdsSMS
					(ISRID
					,MemberID
					,HasMobile)
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N''
					t.ISRID 
					,t.MemberId 
					,CASE ISNULL(v_u.CustomerMobile,'''''''') WHEN '''''''' THEN 0 ELSE 1 END
				FROM '' + QUOTENAME(@tblName) + N'' t
					INNER JOIN V_U_TM_Mem_Info v_u ON t.MemberID = v_u.MemberID
				WHERE t.IsSelected = 1 
					AND t.ExpiredDate IS NULL 
					AND t.Status = 1 
					AND t.WfRootId = @wfRootId ''

    PRINT @sql

	EXEC sp_executesql @sql, N''@wfRootId uniqueidentifier'', @wfRootId = @wfRootId


	
	DECLARE @dataGroupID int 
	select @dataGroupID=DataGroupID from TM_Act_Instance where ActivityInstanceID=@instanceId


	IF EXISTS(SELECT * FROM #tmpMemIdsSMS)
		BEGIN

			--declare @templateSMS nvarchar(500)
			--set @templateSMS=''会员关怀yyb模板3\n\n{CustomerLevelEndDate}{CustomerEmail}{RegisterDate}''
			IF @templateSMS IS NULL
				SET @templateSMS = ''''
		
			--生成发送短信记录的sql
			DECLARE @sqlSMS AS NVARCHAR(MAX)
			SET @sqlSMS = N''INSERT INTO [dbo].[TM_Sys_SMSSendingQueue]
								([Mobile]
								,[Message]
								,[MsgPara]
								,[MemberID]
								,[ActInstanceID]
								,[WorkflowID]
								,[TempletID]
								,[AddedDate]
								,[AddedUser]
								,[Remark]
								,[Channel])
							SELECT v_u.CustomerMobile
								,'''''' + @templateSMS + N''''''
								,'''''' + @templatePara + N''''''
								,v_u.MemberID,@instanceId
								,@wfRootId
								,@templetId
								,GETDATE()
								,''''ActEngine''''
								,''''市场活动''''
								,''''2''''
							FROM V_U_TM_Mem_Info v_u 
								INNER JOIN #tmpMemIdsSMS m ON v_u.MemberID = m.MemberID
								 ''


			--替换短信内容的通配符
			IF @templateSMS LIKE ''%{%''
				BEGIN
					DECLARE @fieldAlias AS NVARCHAR(50)
					DECLARE @dictName AS NVARCHAR(50)
					DECLARE @dictType AS NVARCHAR(50)
					DECLARE @fieldType AS NVARCHAR(10)
					DECLARE @i AS INT
					DECLARE @tempName AS NVARCHAR(10)
					SET @i = 1
					--CLOSE RdCursor
					--DEALLOCATE RdCursor
					DECLARE RdCursor CURSOR FOR
						SELECT [FieldAlias]
							,[DictTableName]
							,[DictTableType] 
							,[FieldType]
						FROM TD_SYS_FieldAlias 
						WHERE IsCommunicationTemplet = 1
					OPEN RdCursor
					FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType,@fieldType
					WHILE (@@FETCH_STATUS = 0)
						BEGIN 
							IF @dictName = ''TD_SYS_BizOption''
								BEGIN
									SET @tempName = ''d'' + CAST(@i AS NVARCHAR(5))
									SET @sqlSMS = REPLACE(@sqlSMS, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @tempName + N''.OptionText IS NULL THEN '''''''' ELSE '' + @tempName + N''.OptionText END + '''''')
									SET @sqlSMS = @sqlSMS + N'' LEFT JOIN (SELECT * FROM TD_SYS_BizOption WHERE DataGroupID=''''''+Convert(varchar,@dataGroupID)+'''''' and OptionType = '''''' + @dictType + N'''''') '' + @tempName + N'' ON v_u.CustomerLevel = '' + @tempName + N''.OptionValue''
								END
							ELSE
								BEGIN
									IF(@fieldType =''5'' or @fieldType=''6'')
									BEGIN 
										SET @sqlSMS = REPLACE(@sqlSMS, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN '''''''' ELSE CONVERT(varchar(10),v_u.'' + @fieldAlias + N'' ,111) END + '''''') --CONVERT(varchar(100), GETDATE(), 120)
									END
									ELSE
									BEGIN
										SET @sqlSMS = REPLACE(@sqlSMS, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN '''''''' ELSE CAST(v_u.'' + @fieldAlias + N'' AS NVARCHAR(MAX)) END + '''''')
									END
								END
							SET @i = @i + 1
							FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType,@fieldType
							print @sqlSMS
						END
					CLOSE RdCursor
					DEALLOCATE RdCursor
				END
				

			--发送短信
			--SET @sqlSMS=@sqlSMS+N'' WHERE v_u.CustomerMobile IS NOT NULL AND v_u.CustomerMobile!=''''''''''
			--SET @sqlSMS = @sqlSMS + N'' where not exists(select 1 from [dbo].[TM_Sys_SMSSendingQueue] where MemberID=m.MemberID and ActInstanceID=@instanceId) AND m.HasMobile = 1 ''
			SET @sqlSMS = @sqlSMS + N'' Where m.HasMobile = 1 ''
			print @sqlSMS
			EXEC sp_executesql @sqlSMS, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER, @templetId INT'',
			                   @instanceId = @instanceId, @wfRootId = @wfRootId, @templetId = @templetId
			

			--修改状态
			DECLARE @sqlUpdate AS NVARCHAR(MAX)
			SET @sqlUpdate = N''UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 2
									, ModifiedDate = GETDATE()
									, StartDate = GETDATE()
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsSMS WHERE HasMobile = 1)

								UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 5
									, ModifiedDate = GETDATE()
									, Remark = ''''发送过期-手机号为空''''
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsSMS WHERE HasMobile != 1)''
			
			PRINT  @sqlUpdate 
			EXEC sp_executesql @sqlUpdate
	
		END
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER--,ERROR_MESSAGE() AS ERROR_MESSAGE,ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH











' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushSurvey]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushSurvey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPushSurvey]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@templetId INT,
	@sendChannel NVARCHAR(20),
	@question NVARCHAR(MAX),
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--declare @tmpMemIds TABLE
	--	(
	--		[MemberID] [char](32) NOT NULL
	--	)
	IF OBJECT_ID(''tempdb..#tmpMemIdsServey'') IS NOT NULL
		DROP TABLE #tmpMemIdsServey

	--创建临时表，用来存储要处理的MemberId
	CREATE TABLE #tmpMemIdsServey
	(
		[ISRID] BIGINT NOT NULL,
		[MemberID] [CHAR](32) NOT NULL
	)

	IF @question IS NULL OR @question = ''''
		SET @question = N''null''

	SET @question = N''{"questions":'' + @question + N''}''

	SET @sql = N''INSERT INTO #tmpMemIdsServey
					(ISRID
					,MemberID)
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR)+'' 
					ISRID
					,MemberId 
				FROM '' + QUOTENAME(@tblName) + ''
				WHERE IsSelected = 1 
					AND ExpiredDate IS NULL 
					AND Status = 1 
					AND WfRootId = @wfRootId''

	EXEC sp_executesql @sql, N''@wfRootId uniqueidentifier'', @wfRootId = @wfRootId

	IF EXISTS(SELECT * FROM #tmpMemIdsServey)
		BEGIN

			--发送问卷
			INSERT INTO [dbo].[TM_Mem_Question]
				   ([MemberID]
				   ,[ActInstanceID]
				   ,[WorkflowID]
				   ,[SendChannel]
				   ,[TempletId]
				   ,[Question]
				   ,[Score]
				   ,[Status]
				   ,[AddedDate]
				   ,[AddedUser]
				   ,[ModifiedDate]
				   ,[ModifiedUser])
			 SELECT MemberID
				   ,@instanceId
				   ,@wfRootId
				   ,@sendChannel
				   ,@templetId
				   ,@question
				   ,0
				   ,0
				   ,GETDATE()
				   ,''ActEngine''
				   ,GETDATE()
				   ,''ActEngine''
			 FROM #tmpMemIdsServey

			--修改状态
			DECLARE @sqlUpdate AS NVARCHAR(MAX)
			SET @sqlUpdate = N''UPDATE '' + QUOTENAME(@tblName) + ''
								SET Status = 2
									, ModifiedDate = GETDATE()
									, StartDate = GETDATE()
								WHERE 
									ISRID IN 
									(
										SELECT ISRID 
										FROM #tmpMemIdsServey
									)''
			EXEC sp_executesql @sqlUpdate
	
		
		END
COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH

	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER,ERROR_MESSAGE() AS ERROR_MESSAGE,ERROR_LINE() AS ERROR_LINE
	ROLLBACK TRANSACTION	--事物事务

END CATCH










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityPushWechat]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityPushWechat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Act_MarketActivityPushWechat]
	@instanceId BIGINT,
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@templetId INT,
	@contentType NVARCHAR(20),
	@contentPara NVARCHAR(500),
	@maxCount INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)

	IF OBJECT_ID(''tempdb..#tmpMemIdsWechat'') IS NOT NULL
		DROP TABLE #tmpMemIdsWechat
	CREATE TABLE #tmpMemIdsWechat
	(
		[ISRID] BIGINT NOT NULL,
		[MemberID] [CHAR](32) NOT NULL,
		[OpenId] nvarchar(100) NOT NULL
	)

	SET @sql = N''INSERT INTO #tmpMemIdsWechat
					(ISRID
					,MemberID
					,OpenId)
				SELECT TOP '' + CAST(@maxCount AS NVARCHAR) + N'' 
					t.ISRID 
					,t.MemberId 
					,v_u.[MemberOpenID]
				FROM [''+@tblName+N''] t
					INNER JOIN V_S_TM_Mem_Master v_u ON t.MemberID = v_u.MemberID
				WHERE t.IsSelected = 1 
					AND t.ExpiredDate IS NULL 
					AND t.Status = 1 
					AND t.WfRootId = @wfRootId
					AND v_u.MemberOpenID is not null ''
					PRINT @sql
	EXEC sp_executesql @sql, N''@wfRootId UNIQUEIDENTIFIER'', @wfRootId = @wfRootId

 --   DECLARE @dataGroupID int 
	--select @dataGroupID=DataGroupID from TM_Act_Instance where ActivityInstanceID=@instanceId

	IF EXISTS(SELECT * FROM #tmpMemIdsWechat)
		BEGIN

			--生成发送邮件记录的sql
			DECLARE @sqlWechat AS NVARCHAR(MAX)
			SET @sqlWechat = N''INSERT INTO [dbo].[TM_Sys_WechatSendingQueue]
								([MemberOpenID]
								,[ContentType]
								,[ContentPara]
								,[MemberID]
								,[ActInstanceID]
								,[WorkflowID]
								,[TempletID]
								,[IsSent]
								,[AddedDate]
								,[AddedUser]
								,[PlanSendDate])
							SELECT OpenID
								,'''''' + @contentType + N''''''
								,'''''' + @contentPara + N''''''
								,MemberID
								,@instanceId
								,@wfRootId
								,@templetId
								,0
								,GETDATE()
								,''''ActEngine''''
								,GETDATE()
							FROM #tmpMemIdsWechat m ''

			----替换内容的通配符
			--IF @contentPara LIKE ''%{%}%''
			--	BEGIN
			--		DECLARE @fieldAlias AS NVARCHAR(50)
			--		DECLARE @dictName AS NVARCHAR(50)
			--		DECLARE @dictType AS NVARCHAR(50)
			--		DECLARE @i AS INT
			--		DECLARE @tempName AS NVARCHAR(10)
			--		SET @i = 1
			--		DECLARE RdCursor CURSOR FOR
			--			SELECT [FieldAlias], [DictTableName], [DictTableType] 
			--			FROM TD_SYS_FieldAlias 
			--			WHERE IsCommunicationTemplet = 1
			--		OPEN RdCursor
			--			FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
			--		WHILE (@@FETCH_STATUS = 0)
			--			BEGIN 
			--				IF @dictName=''TD_SYS_BizOption''
			--					BEGIN
			--						SET @tempName = ''d'' + CAST(@i AS NVARCHAR)
			--						SET @sqlWechat = REPLACE(@sqlWechat, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @tempName + N''.OptionText IS NULL THEN '''''''' ELSE '' + @tempName + N''.OptionText END + '''''')
			--						SET @sqlWechat = @sqlWechat + N'' LEFT JOIN (SELECT * FROM TD_SYS_BizOption WHERE DataGroupID=''''''+Convert(varchar,@dataGroupID)+'''''' and  OptionType = '''''' + @dictType + N'''''') '' + @tempName + N'' ON v_u.CustomerLevel = '' + @tempName + N''.OptionValue''
			--					END
			--				ELSE
			--					BEGIN
			--						SET @sqlWechat = REPLACE(@sqlWechat, N''{'' + @fieldAlias + N''}'', N'''''' + CASE WHEN '' + @fieldAlias + N'' IS NULL THEN '''''''' ELSE CAST(v_u.'' + @fieldAlias + N'' AS NVARCHAR(MAX)) END + '''''')
			--					END
			--				SET @i = @i + 1
			--				FETCH NEXT FROM RdCursor INTO @fieldAlias, @dictName, @dictType
			--			END
			--		CLOSE RdCursor
			--		DEALLOCATE RdCursor
			--	END	


			--插入记录
			
			SET @sqlWechat = @sqlWechat + '' WHERE OpenId is not null and OpenId != ''''''''''
			print @sqlWechat
			EXEC sp_executesql @sqlWechat, N''@instanceId BIGINT, @wfRootId UNIQUEIDENTIFIER, @templetId INT'', @instanceId = @instanceId, @wfRootId = @wfRootId, @templetId = @templetId
		
			--修改状态
			DECLARE @sqlUpdate AS NVARCHAR(MAX)
			SET @sqlUpdate = N''UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 2
									, ModifiedDate = GETDATE()
									, StartDate = GETDATE()
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsWechat WHERE OpenId is not null and OpenId != '''''''')

								UPDATE ''+ QUOTENAME(@tblName) + N''
								SET Status = 5
									, ModifiedDate = GETDATE()
									, Remark = ''''发送过期-OpenId为空''''
								WHERE ISRID IN (SELECT ISRID FROM #tmpMemIdsWechat WHERE OpenId is null or OpenId = '''''''')''
								print @sqlupdate
			EXEC sp_executesql @sqlUpdate
	
		END
COMMIT TRANSACTION	--事务提交
SELECT 1--, @sql Sql, @sqlWechat SqlEmail, @sqlUpdate SqlUpdate
--select @sqlWechat SqlEmail, @sqlUpdate SqlUpdate

--select * from #tmpMemIdsWechat

END TRY
BEGIN CATCH	
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() AS ERRORNUMBER, ERROR_MESSAGE() AS ERROR_MESSAGE, ERROR_LINE() AS ERROR_LINE
ROLLBACK TRANSACTION	--事物事务

END CATCH












' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityRecordExpired]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityRecordExpired]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityRecordExpired]    Script Date: 2015/4/16 9:58:43 ******/
CREATE PROCEDURE [dbo].[sp_Act_MarketActivityRecordExpired]
	@tblName NVARCHAR(100),
	@wfRootId UNIQUEIDENTIFIER,
	@curStatus INT
AS
BEGIN TRY

--创建事务
BEGIN TRANSACTION
	DECLARE @sql NVARCHAR(MAX)
	--set @memberIds= replace(@memberIds,'','','''''','''''')
	SET @sql = N''UPDATE '' + QUOTENAME(@tblName) + N'' 
				 SET Status = 5
				 	,ExpiredDate = GETDATE()
					,ModifiedDate = GETDATE() 
					,Remark = CASE @curStatus WHEN 1 THEN ''''发送过期'''' WHEN 2 THEN ''''获取结果过期'''' ELSE '''''''' END
				 WHERE IsSelected = 1
					 AND Status = @curStatus
					 AND ExpiredDate IS NULL
					 AND WfRootId = @wfRootId''
	EXEC sp_executesql @sql, N''@curStatus INT, @wfRootId UNIQUEIDENTIFIER'', @curStatus = @curStatus, @wfRootId = @wfRootId

COMMIT TRANSACTION	--事务提交
SELECT 1

END TRY
BEGIN CATCH

DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
   
    SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,  -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState     -- State.  
               ); 
--SELECT ERROR_NUMBER() as ERRORNUMBER
ROLLBACK TRANSACTION	--事物事务

END CATCH

--update [TM_Act_InstanceSubdivisionResult_0197D387-EE48-48F2-9CC2-88931061E0F9]
--set ModifiedDate=GETDATE(),ExpiredDate=GETDATE()









' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivityRecordStep]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivityRecordStep]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROC [dbo].[sp_Act_MarketActivityRecordStep]
	@instanceID AS BIGINT,
	@tblName AS NVARCHAR(100),
	@status AS INT 
AS 
BEGIN 
	IF @tblName IS NULL OR @tblName = ''''
		BEGIN
			DECLARE @tmpResult TABLE
				(
					[InstanceStepID] [uniqueidentifier] NOT NULL,
					[ActivityInstanceID] [bigint] NOT NULL,
					[ParentInstanceStepID] [uniqueidentifier] NULL,
					[Category] [nvarchar](20) NOT NULL,
					[Wait] [float] NOT NULL,
					[ResultType] [nvarchar](20) NULL,
					[Condition] [nvarchar](50) NULL,
					[SendMail] [bit] NOT NULL,
					[SendSMS] [bit] NOT NULL,
					[TemplateId] [int] NULL,
					[SendChannel] [nvarchar](20) NULL,
					[ValidDay] [float] NOT NULL,
					[RunDate] [datetime] NOT NULL,
					[ExpiredDate] [datetime] NULL,
					[AddedDate] [datetime] NOT NULL,
					[ModifiedDate] [datetime] NOT NULL,
					[DataLimit] [bit] NULL,
					[LimitType] [nvarchar](20) NULL,
					[LimitValue] [nvarchar](500) NULL
				)
			SELECT * FROM @tmpResult
			RETURN
		END 
	
	DECLARE @sql NVARCHAR(max)
	--declare @tblName as nvarchar(1000)
	--declare @maxCnt AS INT 
	--set @tblName = ''TM_Act_InstanceSubdivisionResult_9974975F-3A78-471E-BB65-2C8BAC353B14''
	--set	@maxCnt= 1000

	SET @sql = N''SELECT * FROM TM_Act_InstanceStep 
				WHERE ActivityInstanceID = @instanceId
				AND RunDate <= GETDATE()
				AND InstanceStepID IN (
					SELECT DISTINCT WfRootId 
					FROM '' + QUOTENAME(@tblName) + N'' 
					WHERE IsSelected = 1 AND ExpiredDate IS NULL AND Status = @status)''
	--print @sql
	EXEC sp_executesql @sql, N''@instanceId BIGINT, @status INT'', @instanceId = @instanceID, @status = @status
	--return 1
	--SET @sql=N''SELECT distinct WfRootId FROM [''+@tblName+N''] WHERE Status = ''+CAST(@status AS NVARCHAR)
	--if @sql is not null
	--	exec sp_executesql @sql
	--else
	--begin 
	--	DECLARE @tmpResult TABLE
	--	(
	--		[InstanceStepID] [uniqueidentifier] NOT NULL,
	--		[ActivityInstanceID] [bigint] NOT NULL,
	--		[ResultTableName] [nvarchar](100) NOT NULL,
	--		[ParentInstanceStepID] [uniqueidentifier] NULL,
	--		[Category] [nvarchar](20) NOT NULL,
	--		[Wait] [float] NOT NULL,
	--		[ResultType] [nvarchar](20) NULL,
	--		[Condition] [nvarchar](50) NULL,
	--		[SendMail] [bit] NOT NULL,
	--		[SendSMS] [bit] NOT NULL,
	--		[TemplateId] [int] NULL,
	--		[SendChannel] [nvarchar](20) NULL,
	--		[ValidDay] [float] NOT NULL,
	--		[RunDate] [datetime] NOT NULL,
	--		[ExpiredDateDate] [datetime] NULL,
	--		[IsFinished] [bit] NOT NULL,
	--		[AddedDate] [date] NOT NULL,
	--		[ModifiedDate] [date] NOT NULL
	--	)
	--	select * from @tmpResult
	--end
END 











' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_MarketActivitySubdivision]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_MarketActivitySubdivision]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE proc [dbo].[sp_Act_MarketActivitySubdivision]
 @actInsId bigint =0,
 @cardNo nvarchar(50) = '''',
 @name nvarchar(50) = '''',
 @subdivisionId nvarchar(500) =N'''',
 @pagesize int = 20,
 @pageindex int = 0,
 @pagecount int output
as 
/**********************************
----arvarto system-----
存储过程功能描述：sp_Act_MarketActivitySubdivision 获取市场活动指定实例的会员细分
建 立 人：Leo
建立时间：2014-04-23 15:25:45
修 改 人：Leo
修改时间：2014-05-04 13:58
修改内容：修改主键
***********************************/
begin
	--declare @actInsId bigint = 1000004
	--declare @cardNo nvarchar(50) = ''''
	--declare @name nvarchar(50) = ''''
	--declare @subdivisionId nvarchar(500) =N''''
	--declare @pagesize int = 20
	--declare @pageindex int = 0
	--declare @pagecount int 

		IF @actInsId IS NULL or @actInsId = ''''
	BEGIN 	    
		declare @tblActResult table (
            
		MemberId nvarchar(50),
		Name nvarchar(50),
		Mobile nvarchar(50),
		Email nvarchar(100),
		CardNo nvarchar(50),
		SubdivisionName nvarchar(1000),
		WfRootId nvarchar(1000),
		Templates nvarchar(1000)
		)
		select * from @tblActResult 
		return
	END

	declare @actSubdivTable nvarchar(100)	--活动细分表名
	--declare @memSubdivTable nvarchar(max)	--会员细分表名

	--活动细分表
	select @actSubdivTable = TableName from [TM_Act_Instance] where ActivityInstanceID = @actInsId

	--会员细分表
	declare cur2 cursor for 
	select c.TableName from [TM_Act_InstanceSubdivision] b 
	inner join [TM_Mem_SubdivisionInstance] c on c.SubdivisionInstanceID = b.SubdivisionInstanceID
	where b.ActivityInstanceID = @actInsId and (@subdivisionId is null or @subdivisionId = '''' or charindex(@subdivisionId,SubdivisionID,0)>0)

	declare @memTables nvarchar(max) = ''select ''''0'''' MemberId''
	declare @tblName nvarchar(100)
	 
	OPEN cur2
	FETCH NEXT FROM cur2 INTO @tblName
	WHILE (@@FETCH_STATUS = 0)
	BEGIN 
		SET @memTables = @memTables + N'' union select MemberId FROM [''+@tblName+'']''
		FETCH NEXT FROM cur2 INTO @tblName
	END
	CLOSE cur2
	DEALLOCATE cur2

	--会员细分表
	declare cur cursor for 
	select b.TableName,c.SubdivisionName
	from [TM_Act_InstanceSubdivision] a
	inner join [TM_Mem_SubdivisionInstance] b on a.SubdivisionInstanceID = b.SubdivisionInstanceID
	inner join [TM_Mem_Subdivision] c on b.SubdivisionID = c.SubdivisionID
	where a.ActivityInstanceId = @actInsId

	--会员细分分类
	declare @sql nvarchar(max)
	--declare @tblName nvarchar(100)
	declare @subdivisionName nvarchar(50)

	IF OBJECT_ID(N''tempdb..##tblResult'',N''U'') IS NOT NULL DROP TABLE ##tblResult
	create table ##tblResult(
		MemberId nvarchar(50),
		Name nvarchar(20),
		Mobile nvarchar(11),
		Email nvarchar(100),
		CardNo nvarchar(20),
		SubdivisionName nvarchar(1000),
		WfRootId nvarchar(1000),
		Templates nvarchar(1000)
	)


	IF OBJECT_ID(@actSubdivTable,N''U'') IS NOT NULL
	BEGIN 
		SET @sql =		  '' insert into #tmp_Mem_SubdivideResultType''
		SET @sql = @sql + '' select b.MemberID,b.SubdivisionName from ['' + @actSubdivTable + ''] a''
		SET @sql = @sql + '' inner join ( ''
		SET @sql = @sql + '' select ''''0'''' MemberId,''''测试会员'''' SubdivisionName''

		OPEN cur
		FETCH NEXT FROM cur INTO @tblName,@subdivisionName
		WHILE (@@FETCH_STATUS = 0)
		BEGIN 
			SET @sql = @sql + N'' union select MemberId,'''''' + @subdivisionName + '''''' SubdivisionName FROM [''+@tblName+''] ''
			FETCH NEXT FROM cur INTO @tblName,@subdivisionName
		END
		CLOSE cur
		DEALLOCATE cur
		SET @sql = @sql + '' ) b on a.MemberId = b.MemberId ''

		IF OBJECT_ID(N''tempdb..#tmp_Mem_SubdivideResultType'',N''U'') IS NOT NULL DROP TABLE #tmp_Mem_SubdivideResultType
		CREATE TABLE #tmp_Mem_SubdivideResultType(
			MemberID nvarchar(50),
			SubdivisionName NVARCHAR(50)
		)
		print @sql
		EXEC sp_executesql @sql

		--分组后的会员细分分类
		SET @sql =		  '' insert into #tmp_Mem_SubdivideResultTypeGrouped''
		SET @sql = @sql + '' select a.MemberId,''
		SET @sql = @sql + '' (''
		SET @sql = @sql + ''		select distinct SubdivisionName+'''',''''''
		SET @sql = @sql + ''		from #tmp_Mem_SubdivideResultType ''
		SET @sql = @sql + ''		where MemberId = a.MemberId ''
		SET @sql = @sql + ''		for xml path('''''''')''
		SET @sql = @sql + '' ) SubdivisionName''
		SET @sql = @sql + '' from #tmp_Mem_SubdivideResultType a''
		SET @sql = @sql + '' inner join (''+@memTables+'') b on a.MemberId = b.MemberID''
		SET @sql = @sql + '' group by a.MemberId''
		IF OBJECT_ID(N''tempdb..#tmp_Mem_SubdivideResultTypeGrouped'',N''U'') IS NOT NULL DROP TABLE #tmp_Mem_SubdivideResultTypeGrouped
		CREATE TABLE #tmp_Mem_SubdivideResultTypeGrouped(
			MemberID nvarchar(50),
			SubdivisionName NVARCHAR(1000)
		)
		print @sql
		EXEC sp_executesql @sql

		--市场细分记录
		SET @sql =		  '' insert into #tmp_Act_InstanceSubdivisionResult''
		SET @sql = @sql + '' select MemberId,''
		SET @sql = @sql + '' (''
		SET @sql = @sql + ''		select cast(WfRootId as nvarchar(50))+''''|''''+cast(Status as nvarchar)+'''',''''''
		SET @sql = @sql + ''		from ['' + @actSubdivTable + '']''
		SET @sql = @sql + ''		where p.MemberId = MemberId''
		SET @sql = @sql + ''		for xml path('''''''')''
		SET @sql = @sql + '' ) WfRootId''
		SET @sql = @sql + '' from ['' + @actSubdivTable + ''] p''
		SET @sql = @sql + '' where p.IsSelected=1 group by MemberId''
		IF OBJECT_ID(N''tempdb..#tmp_Act_InstanceSubdivisionResult'',N''U'') IS NOT NULL DROP TABLE #tmp_Act_InstanceSubdivisionResult
		CREATE TABLE #tmp_Act_InstanceSubdivisionResult(
			MemberId NVARCHAR(50),
			WfRootId NVARCHAR(1000)
		)
		print @sql
		EXEC sp_executesql @sql		

		SET @sql =		  '' insert into ##tblResult''
		SET @sql = @sql + '' select a.MemberId,c.CustomerName,c.CustomerMobile,c.CustomerEmail,c.MemberCardNo,b.SubdivisionName,a.WfRootId,NULL ''
		SET @sql = @sql + '' from #tmp_Act_InstanceSubdivisionResult a''
		SET @sql = @sql + '' inner join #tmp_Mem_SubdivideResultTypeGrouped b on a.MemberId = b.MemberID''
		SET @sql = @sql + '' inner join V_U_TM_Mem_Info c on c.MemberId = b.MemberID''
		--SET @sql = @sql + '' inner join TM_Mem_Master c on c.MemberId = b.MemberID''
		--SET @sql = @sql + '' left join TM_Mem_Card d on c.MemberID = d.MemberID and d.Active = 1''
		SET @sql = @sql + '' where 1=1 ''
		if @cardNo is not null and @cardNo != '''' SET @sql = @sql + '' and c.MemberCardNo like ''''%''+@cardNo+''%''''''
		if @name is not null and @name != '''' SET @sql = @sql + '' and c.CustomerName like ''''%''+@name+''%''''''
	
		print @sql
		EXEC sp_executesql @sql


		DROP TABLE #tmp_Mem_SubdivideResultType
		DROP TABLE #tmp_Mem_SubdivideResultTypeGrouped
		DROP TABLE #tmp_Act_InstanceSubdivisionResult
	END
	if @pagesize>0 
	begin
		select @pagecount = count(1) from ##tblResult
	
		SELECT A.MemberId,A.Name,A.Mobile,A.Email,A.CardNo,A.SubdivisionName,A.WfRootId,A.Templates 
		FROM (SELECT row_number() OVER (ORDER BY Name DESC) RowIdx,* FROM ##tblResult) A 
		WHERE A.RowIdx>@pageindex AND A.RowIdx<@pagesize+@pageindex+1
	end
	ELSE 
	begin
		set @pagecount = 0
		select * from ##tblResult
	end
end










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Act_WX_GetActivityList]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Act_WX_GetActivityList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Act_WX_GetActivityList] 
	-- Add the parameters for the stored procedure here
	@dataGroupId AS int, 
	@memberId AS char(32)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

	IF @dataGroupId IS NULL OR @memberId IS NULL OR @memberId = ''''
	BEGIN
		DECLARE @tbl TABLE 
		(
			[ActivityInstanceID] [bigint],
			[ActivityName] [nvarchar](50),
			[TableName] [nvarchar](100),
			[AddedDate] [datetime],
			TempletID int,
			ExpiredDate [datetime]
		)
		SELECT * FROM @tbl
		RETURN
	END

    -- Insert statements for procedure here
	IF OBJECT_ID(''tempdb..#tmp'') IS NOT NULL
		DROP TABLE #tmp
	CREATE TABLE #tmp
	(
		[ActivityInstanceID] [bigint],
		[ActivityName] [nvarchar](50),
		[TableName] [nvarchar](100),
		[AddedDate] [datetime],
		TempletID int,
		ExpiredDate [datetime]
	)

	INSERT INTO #tmp
	(
			[ActivityInstanceID],
			[ActivityName],
			[TableName],
			[AddedDate],
			TempletID,
			ExpiredDate
	)
	SELECT 
		[ActivityInstanceID],
		[ActivityName],
		[TableName],
		[AddedDate],
		TempletID,
		ExpiredDate
	FROM (
			SELECT m.ActivityInstanceID, m.ActivityName, m.TableName, m.AddedDate, c.TempletID, s.ExpiredDate, ROW_NUMBER() OVER (PARTITION BY m.ActivityID ORDER BY m.addeddate DESC) serial
			FROM TM_Act_Instance m
				INNER JOIN TM_Act_InstanceStep s ON m.ActivityInstanceID = s.ActivityInstanceID
				INNER JOIN TM_Act_CommunicationTemplet c ON s.TemplateId = c.TempletID
			WHERE m.DataGroupID = @dataGroupId 
				--AND m.Status = 2
				AND m.IsTranslated = 1
				AND s.Category = ''WeChat'' 
				AND c.Type = ''WeChat''
		 ) t
	WHERE t.serial = 1
	
	--SELECT * FROM #tmp

	DECLARE MyCursor CURSOR 
		FOR SELECT ActivityInstanceID, TableName FROM #tmp
	OPEN MyCursor
	DECLARE @tableName nvarchar(100) 
	DECLARE @InstanceID bigint
	FETCH NEXT FROM  MyCursor INTO @InstanceID, @tableName
	WHILE @@FETCH_STATUS = 0	
		BEGIN
			DECLARE @sql nvarchar(max);
			--SET @sql = N''IF NOT EXISTS(SELECT * FROM '' + QUOTENAME(@tableName) + N'' WHERE MemberId = '''''' + @memberId + '''''' AND IsSelected = 1 )
			--begin
			--select 1
			--end''
			--DELETE #tmp WHERE ActivityInstanceID = '' + CAST( @InstanceID AS nvarchar)
			SET @sql = N''DELETE #tmp WHERE ActivityInstanceID = '' + CAST( @InstanceID AS nvarchar) + ''
							AND NOT EXISTS(SELECT * FROM '' + QUOTENAME(@tableName) + N'' WHERE MemberId = '''''' + @memberId + '''''' AND IsSelected = 1 )''
							
			EXEC (@sql)
		FETCH NEXT FROM  MyCursor INTO @InstanceID, @tableName
		END
	--关闭游标
	CLOSE MyCursor
	--释放资源
	DEALLOCATE MyCursor

	SELECT * FROM #tmp

	--select @sql
END










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Act_InstanceMemberid]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Act_InstanceMemberid]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[sp_CRM_Act_InstanceMemberid] 
as
 /**********************************
  ----arvarto system-----
  存储过程功能描述：市场KPI发出通知的会员数

  ***********************************/    
  BEGIN


--清除游标数据
truncate table TR_Mem_MarketActivity


--声明游标

DECLARE MyCursor CURSOR 
FOR 
select a.ActivityInstanceID,a.ActivityID,a.ActivityName,a.TableName,b.ProStartDate,b.ProEndDate 
from TM_Act_Instance a inner join  TM_Act_Master b
on a.ActivityID = b.ActivityID 


 --打开一个游标	
OPEN MyCursor
--循环一个游标
DECLARE @ActivityInstanceID  nvarchar(50) ,@ActivityID  nvarchar(50),@ActivityName nvarchar(50),@TableName nvarchar(2000),@ProStartDate nvarchar(50),@ProEndDate nvarchar(50) 
FETCH NEXT FROM  MyCursor INTO @ActivityInstanceID,@ActivityID,@ActivityName,@TableName,@ProStartDate,@ProEndDate
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql nvarchar(max);
    set @sql =''
	          insert into TR_Mem_MarketActivity
			  (memberid, ActivityInstanceID,ActivityID,ActivityName,StartDate, EndDate)
              select memberid,''''''+@ActivityInstanceID+'''''',''''''+@ActivityID+'''''',  ''''''+@ActivityName+'''''',''''''+@ProStartDate+'''''',''''''+@ProEndDate+''''''
              from [''+@TableName+'']''
   print @sql		    
   exec (@sql)

FETCH NEXT FROM  MyCursor INTO @ActivityInstanceID,@ActivityID,@ActivityName,@TableName,@ProStartDate,@ProEndDate
END	
--关闭游标
CLOSE MyCursor
--释放资源
DEALLOCATE MyCursor

end














' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_Search]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_Search]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_CRM_Mem_Search]
	@dataGroupId int,
	@pageIds nvarchar(100)='''', 
	@dataRoleIds nvarchar(100)='''',
	@memberNo nvarchar(20),              --会员卡号
	@customerName nvarchar(20),
	@customerMobile nvarchar(20),
	@customerLevel nvarchar(20),
	@RegisterStoreCode nvarchar(20),
	@RegStoreArea nvarchar(20),
	@RegStoreChan nvarchar(20),
	@ConsumeAmountStart decimal(18,2),
	@ConsumeAmountEnd decimal(18,2),
	@ConsumePointStart decimal(18,2),
	@ConsumePointEnd decimal(18,2),
	@membercode nvarchar(20),
	@CustomerSource nvarchar(20),
	@RegisterDateStart nvarchar(20),
	@RegisterDateEnd nvarchar(20),
	@OrderBy nvarchar(20),
	@PageIndex int,
    @PageSize int,
	@RecordCount int out	
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：会员360查找
  建 立 人：
  建立时间：2015-03-03
  修 改 人： 
  20151120 yhq 增加注册时间、消费额、积分、来源四个条件的查询，增加来源、注册门店、可用积分、注册时间返回列，增加排序字段
  20151028 yhq 会员编号查询改成会员卡号查询
  20151102 zyb 修改会员的入会渠道参数取值及显示的入会渠道名称（会员来源改为渠道名称）,以及会员的大区域参数
  20151103 zyb 修改城市取值从v_s_tm_mem_ext.City 
  20151103 zyb 新增membercode的输入参数查询；

  ***********************************/ 


BEGIN
IF @pageIds IS NULL or @pageIds = ''''
BEGIN 

declare @sp_CRM_Mem_Search_Result table (
        MemberID char(32),
		DataGroupID int,
        MemberCardNo nvarchar(20),
        CustomerName nvarchar(100),
        CustomerMobile nvarchar(20),
        CustomerLevel nvarchar(20),
        CustomerLevelText nvarchar(20),
        City nvarchar(20),
		Channel nvarchar(100) ,
		ConsumeAmount decimal(18,2),         --维度添加好后需加入此字段
		HistoryConsumeAmount decimal(18,2),  --维度添加好后需加入此字段
		AvailPoint decimal(18,2) ,           --维度添加好后需加入此字段
		CustomerSource nvarchar(20),
		RegisterDate nvarchar(20),
		RegisterStoreName nvarchar(20),
		HistoryConsumeModify decimal(18,2)

)
select * from @sp_CRM_Mem_Search_Result 
return
END

 -----生成输入的角色对应的权限过滤值
    exec sp_Sys_ReturnAuthValue @dataGroupId,@pageIds,@dataRoleIds 



	declare
			@Sql nvarchar(max) = '''',
			@Sql1 nvarchar(max) = '''',

			
			----管理员无法查看会员，其他人员可查看本身下的数据
	        --@Sql_Search nvarchar(max) = '' where 1=1 and g.dataGroupId in (select   subdataGroupId
			      --                                                        from V_Sys_DataGroupRelation
									--									  where  dataGroupId<>6
									--									    and  dataGroupId=''+cast(@dataGroupId as nvarchar(1000))+'') '',

	        @Sql_Search nvarchar(max) = '' where 1=1 
	and a.dataGroupId =''+cast(@dataGroupId as nvarchar(1000))+'' '',
			@Sql_VehicleNo  nvarchar(max)=''where 1=1 '',
			@Sql_Count nvarchar(max),
			@Sql_Choose nvarchar(max)='' ''



----先期查看性能如何，可考虑改为等值关联


	if exists (select value from TE_AUTH_TypeFilterValue with (nolock) where type=''store'')

	--begin
	--	set @Sql_Search = @Sql_Search + '' and  a.RegisterStoreCode  in (select value from TE_AUTH_TypeFilterValue where type=''''store'''')''
	--end

	begin
		set @Sql_Search = @Sql_Search + '' and exists (select t.memberid,t.RegisterStoreCode 
		                                                   from V_R_TM_Mem_StoreCode  t 														  
												           where  t.RegisterStoreCode  in (select value 
                                                                                            from 
																							TE_AUTH_TypeFilterValue  with (nolock) where type=''''store'''')
														      and a.memberid=t.memberid 								      
												                        ) ''
	end

----待有品牌约束时，恢复
	if exists (select value from TE_AUTH_TypeFilterValue  with (nolock) where type=''brand'')

	begin
		set @Sql_Search = @Sql_Search + ''  and exists (select t.memberid,t.RegisterStoreCode 
		                                                   from V_R_TM_Mem_StoreCode  t 														  
												           where  t.RegisterStoreCode in (select StoreCode 
														                                  from V_M_TM_SYS_BaseData_store 
																						  where  StorebrandCode  in (select value 
																						                             from TE_AUTH_TypeFilterValue with (nolock) where type=''''brand''''))
														      and a.memberid=t.memberid 								      
												                        )  ''
	end


	
	if (isnull(@memberNo, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.MemberCardNo like  ''''%''+@memberNo+''%'''' ''   
	end
	

	if (isnull(@customerName, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerName like   ''''%''+ @customerName + ''%'''' ''    
	end


	if (isnull(@customerMobile, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerMobile like ''''%''+@customerMobile+''%'''' ''    
	end


	if (isnull(@customerLevel, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerLevel    =  ''''''+@customerLevel+'''''' ''    
	end
	

	if (isnull(@RegisterStoreCode, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.RegisterStoreCode =  ''''''+@RegisterStoreCode+'''''' ''     
	end


	if (isnull(@RegStoreArea, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.AreaMember =  ''''''+@RegStoreArea+'''''' ''     
	end

 
	if (isnull(@RegStoreChan, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.ChannelCodeMember =  ''''''+@RegStoreChan+'''''' ''     
	end




	if (@ConsumeAmountStart is not null )
	begin
		set @Sql_Search = @Sql_Search + '' and a.ActConsumption >=  ''+ cast(@ConsumeAmountStart as varchar(10))+'' ''     
	end

	if (@ConsumeAmountEnd  is not null )
	begin
		set @Sql_Search = @Sql_Search + '' and a.ActConsumption <=  ''+cast(@ConsumeAmountEnd as varchar(10))+'' ''     
	end

	if (@ConsumePointStart is not null )
	begin
		set @Sql_Search = @Sql_Search + '' and f.value1 >=  ''+ cast(@ConsumePointStart as varchar(10))+'' ''     
	end

	if (@ConsumePointEnd is not null )
	begin
		set @Sql_Search = @Sql_Search + '' and f.value1 <=  ''+cast(@ConsumePointEnd as varchar(10))+'' ''     
	end

	if(@RegisterDateStart<>'''')
	begin
	    set @Sql_Search=@Sql_Search+'' and a.RegisterDate >= ''''''+@RegisterDateStart+'''''' '' 
	end

	if(@RegisterDateEnd<>'''')
	begin
    	set @Sql_Search=@Sql_Search+'' and a.RegisterDate <= ''''''+@RegisterDateEnd+'''''' '' 
	end

	if(@CustomerSource<>'''')
	begin
    	set @Sql_Search=@Sql_Search+'' and a.CustomerSource = ''''''+@CustomerSource+'''''' '' 
	end

 --(select OptionValue,OptionType TD_SYS_BizOption where datagroupid=''+@dataGroupId+'')


 	if (isnull(@membercode, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.membercode like ''''%''+@membercode+''%'''' ''    
	end




set @Sql1=''
           truncate table TE_Sys_VehiclenoSearch 

           insert into TE_Sys_VehiclenoSearch
           select distinct a.MemberID,---a.VIN,
		 VIN = STUFF(
                 ( SELECT '''','''' +VIN
                   from V_M_TM_Mem_SubExt_vehicle w 
                   where w.MemberID=a.MemberID
                  FOR XML PATH('''''''')
                  ), 1, 1, ''''''''),
               CarNo = STUFF(
                 ( SELECT '''','''' +CarNo
                   from V_M_TM_Mem_SubExt_vehicle t 
                    ''+@Sql_VehicleNo+''  and t.MemberID=a.MemberID
                  FOR XML PATH('''''''')
                  ), 1, 1, '''''''')
         from V_M_TM_Mem_SubExt_vehicle a with (nolock)
         ''+@Sql_VehicleNo+'' 


          ''





set @Sql='' 
from V_U_TM_Mem_Info a with (nolock) 
left join  (select OptionValue,OptionType,OptionText from TD_SYS_BizOption with (nolock) 
where datagroupid=''+cast(@dataGroupId as nvarchar(2))+'') e on a.CustomerLevel=e.OptionValue and e.OptionType=''''CustomerLevel''''  
   left join 	V_M_TM_SYS_BaseData_store g with (nolock) on a.RegisterStoreCode=g.StoreCode 
   ---left join TD_SYS_Region h with (nolock) on ltrim(rtrim(a.City)) = cast(h.RegionID as varchar(10)) 
   left join TM_Mem_Account f with (nolock) on (a.memberid=f.memberid and f.accounttype=''''3'''') 
 ''+@Sql_Search	

	set @Sql_Count = ''select @ct = count(a.MemberID) '' + @Sql 
	exec sp_executesql @Sql_Count,N''@ct int output'',@RecordCount output 


	set @Sql = '' 
select  memberid,MemberCode as MemberCardNo,CustomerName,CustomerMobile,
        CustomerLevel,CustomerLevelText,DataGroupId,
		City, Channel,isnull(ActConsumption,0) ConsumeAmount, HistoryConsumeAmount,AvailPoint,HistoryConsumeModify,
		convert(varchar(20),isnull(RegisterDate,''''''''),20) as RegisterDate,RegisterStoreCode,StoreName RegisterStoreName,CustomerSource
from  
(
select  a.memberid,a.MemberCode,a.CustomerName,a.CustomerMobile,
        a.CustomerLevel,e.OptionText CustomerLevelText,a.DataGroupId, 
		a.City City,a.RegChannelName Channel, a.HistoryConsumeAmount,a.ActConsumption,a.HistoryConsumeModify,
		a.RegisterDate,a.RegisterStoreCode,g.StoreName,f.value1 AvailPoint,a.CustomerSource,
		ROW_NUMBER() OVER (ORDER BY a.RegisterDate desc) RowID

	''+@Sql+'') a  ''

	if (@PageSize is not null and @PageSize>1) 
	begin 
		set @Sql=@Sql+
	''where (RowID-1)/''+convert(nvarchar(100),@PageSize)+''+1 = ''+convert(nvarchar(100),@PageIndex)+''+1 ''
	end 
	if(@OrderBy<>'''')
	begin
	set @Sql=@Sql+''order by ''+@OrderBy
	end

	--set @Sql=@Sql+''order by a.RowID''
      	print @Sql

        exec (@Sql)

END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_Search1]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_Search1]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_CRM_Mem_Search1]
	@dataGroupId int,
	@pageIds nvarchar(100)='''', 
	@dataRoleIds nvarchar(100)='''',
	@customerStatus nvarchar(20), 
	@memberCardNo nvarchar(20),
	@customerName nvarchar(20),
	@customerMobile nvarchar(20),
	@customerLevel nvarchar(20),
	@carNo nvarchar(20),
	@validateDateStart DateTime,
	@validateDateEnd DateTime,
	@insuranceDateStart DateTime,
	@insuranceDateEnd DateTime,
	@RegisterDateStart DateTime,
	@RegisterDateEnd DateTime,
	@RegisterStoreCode nvarchar(20),
	@PageIndex int,
    @PageSize int,
	@RecordCount int out	
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：会员360查找
  建 立 人：
  建立时间：2015-03-03
  修 改 人： 
  1.会员的车辆含有多个？只显示其中一个的信息
  2.会员状态含有多个？如何在内容中展示
  3.@dataGroupId in or = ??@pageRoleIds,@dataRoleIds
  4.门店回头改为等值关联

  20150310 zyb 对于车辆只显示车辆信息，以逗号隔开；增加权限设置的约束


  ***********************************/ 


BEGIN


 -----生成输入的角色对应的权限过滤值
    exec sp_Sys_ReturnAuthValue @dataGroupId,@pageIds,@dataRoleIds 


	declare
			@Sql nvarchar(max) = '''',
			----管理员无法查看会员，其他人员可查看其子项下的数据
	        @Sql_Search nvarchar(max) = '' where 1=1 and g.dataGroupId in (select   subdataGroupId
			                                                              from V_Sys_DataGroupRelation
																		  where  dataGroupId<>6
																		    and  dataGroupId=''+cast(@dataGroupId as nvarchar(1000))+'') '',
			@Sql_VehicleNo  nvarchar(max)=''where 1=1 '',
			@Sql_Count nvarchar(max),
			@Sql_Choose nvarchar(max)='' ''



------先期查看性能如何，可考虑改为等值关联


	if exists (select value from TE_AUTH_TypeFilterValue where type=''store'')

	begin
		set @Sql_Search = @Sql_Search + '' and  a.RegisterStoreCode  in (select value from TE_AUTH_TypeFilterValue where type=''''store'''')''
	end



------待有品牌约束时，恢复
	--if exists (select value from TE_AUTH_TypeFilterValue where type=''brand'')

	--begin
	--	set @Sql_Search = @Sql_Search + '' and  a.brand  in (select value from TE_AUTH_TypeFilterValue where type=''''brand'''')''
	--end


    if (isnull(@customerStatus, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerStatus=  ''''''+@customerStatus+'''''' ''
	end

	
	if (isnull(@memberCardNo, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.MemberCardNo=  ''''''+@memberCardNo+'''''' ''
	end
	

	if (isnull(@customerName, '''') <> '''' )
	begin
		set @Sql_Search = @customerName + '' and a.customerName like   ''''%''+ @customerName + ''%'''' ''    
	end


	if (isnull(@customerMobile, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerMobile    =  ''''''+@customerMobile+'''''' ''    
	end


	if (isnull(@carNo, '''') <> '''' )
	begin
		---set @Sql_Search = @Sql_Search + '' and f.CarNo  like   ''''%''+ @carNo + ''%'''' ''     
		set @Sql_VehicleNo = @Sql_VehicleNo+'' and carNo   like   ''''%''+ @carNo + ''%'''' ''   
	end


	if (isnull(@validateDateStart, '''') <> '''' )
	begin
		---set @Sql_Search = @Sql_Search + '' and f.ValidateDate>= ''''''+convert( NVARCHAR(20),@validateDateStart,120)+'''''' ''    
		set @Sql_VehicleNo = @Sql_VehicleNo+ '' and ValidateDate>= ''''''+convert( NVARCHAR(20),@validateDateStart,120)+'''''' ''    
	end


	if (isnull(@validateDateEnd, '''') <> '''' )
	begin
		--set @Sql_Search = @Sql_Search +    '' and f.ValidateDate< ''''''+convert( NVARCHAR(20),DATEADD(day,1,@validateDateEnd),120)+'''''' ''
		set @Sql_VehicleNo = @Sql_VehicleNo+ '' and ValidateDate< ''''''+convert( NVARCHAR(20),DATEADD(day,1,@validateDateEnd),120)+'''''' ''  
	end



	if (isnull(@insuranceDateStart, '''') <> '''' )
	begin
		---set @Sql_Search = @Sql_Search + '' and f.InsuranceDate>= ''''''+convert( NVARCHAR(20),@insuranceDateStart,120)+'''''' ''    
		set @Sql_VehicleNo = @Sql_VehicleNo+ '' and InsuranceDate>= ''''''+convert( NVARCHAR(20),@insuranceDateStart,120)+'''''' ''    
	end


	if (isnull(@insuranceDateEnd, '''') <> '''' )
	begin
		--set @Sql_Search = @Sql_Search +    '' and f.InsuranceDate< ''''''+convert( NVARCHAR(20),DATEADD(day,1,@insuranceDateEnd),120)+'''''' ''
		set @Sql_VehicleNo = @Sql_VehicleNo+ '' and InsuranceDate< ''''''+convert( NVARCHAR(20),DATEADD(day,1,@insuranceDateEnd),120)+'''''' ''  
	end





	if (isnull(@RegisterDateStart,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.RegisterDate>= ''''''+convert( NVARCHAR(20),@RegisterDateStart,120)+'''''' ''
	end


	if (isnull(@RegisterDateEnd,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.RegisterDate< ''''''+convert( NVARCHAR(20),DATEADD(day,1,@RegisterDateEnd),120)+'''''' ''
	end


	

	if (isnull(@RegisterStoreCode, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.RegisterStoreCode =  ''''''+@RegisterStoreCode+'''''' ''     
	end


 

	if (isnull(@carNo, '''') = ''''   
	    and isnull(@validateDateStart, '''') = ''''    and isnull(@validateDateEnd, '''') = '''' 
	    and isnull(@insuranceDateStart, '''') = ''''  and isnull(@insuranceDateEnd, '''') = ''''   
		)
	begin
		set @Sql_Choose = @Sql_Choose + '' left  join ''     
    end
	else 
	begin 
		set @Sql_Choose = @Sql_Choose + '' inner  join ''     
	 end  
print @Sql_Choose
print @Sql_Search



set @Sql='' 
from V_U_TM_Mem_Info a 
inner join  TD_SYS_BizOption c on a.CustomerStatus=c.OptionValue and c.OptionType=''''CustomerStatus''''
inner join  TD_SYS_BizOption e on a.CustomerLevel=e.OptionValue and e.OptionType=''''CustomerLevel''''
''+ @Sql_Choose +''
 (
         select distinct  a.MemberID,
               CarNo = STUFF(
                 ( SELECT '''','''' +CarNo
                   from V_M_TM_Mem_SubExt_vehicle t 
                    ''+@Sql_VehicleNo+''  and t.MemberID=a.MemberID
                  FOR XML PATH('''''''')
                  ), 1, 1, '''''''')
         from V_M_TM_Mem_SubExt_vehicle a
         ''+@Sql_VehicleNo+'' ) f on a.MemberID=f.MemberID 
left join 	V_M_TM_SYS_BaseData_store g on a.RegisterStoreCode=g.StoreCode
 ''+@Sql_Search	

	set @Sql_Count = ''select @ct = count(a.MemberID) '' + @Sql 
	exec sp_executesql @Sql_Count,N''@ct int output'',@RecordCount output 

	set @Sql = '' 
select  memberid,MemberCardNo,CustomerStatus, CustomerStatusText,CustomerName,CustomerMobile,
        CustomerLevel,CustomerLevelText,
		CarNo,
		---ValidateDate,InsuranceDate,
		RegisterDate,RegisterStoreCode,StoreName RegisterStoreName
from  
(
select  a.memberid,a.MemberCardNo,a.CustomerStatus,c.OptionText CustomerStatusText,a.CustomerName,a.CustomerMobile,
        a.CustomerLevel,e.OptionText CustomerLevelText,
		f.CarNo,
		a.RegisterDate,a.RegisterStoreCode,g.StoreName ,
		ROW_NUMBER() OVER (ORDER BY a.MemberID) RowID

	''+@Sql+'') a  ''

	if (@PageSize is not null and @PageSize>1) 
	begin 
		set @Sql=@Sql+
	''where (RowID-1)/''+convert(nvarchar(100),@PageSize)+''+1 = ''+convert(nvarchar(100),@PageIndex)+''+1 ''
	end 

	set @Sql=@Sql+''order by a.RowID''

      	print @Sql

        exec (@Sql)

END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_SearchForAccountChange]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_SearchForAccountChange]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_CRM_Mem_SearchForAccountChange]
	@dataGroupId int,
	@pageIds nvarchar(100)='''', 
	@dataRoleIds nvarchar(100)='''',
	@customerStatus nvarchar(20),
	@memberCardNo nvarchar(20),
	@customerName nvarchar(20),
	@customerMobile nvarchar(20),
	@carNo nvarchar(20),
	@vin  nvarchar(20),
	@PageIndex int,
    @PageSize int,
	@RecordCount int out	
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：会员账户调整会员信息查找
  建 立 人：
  建立时间：2015-04-08
  修 改 人： 20150717 zyb  增加参数@customerStatus
             20150717 zyb  限制总部会员，增加字段输出  

  ---输入参数：卡号，姓名，手机，车牌，VIN
  ---输出值  ：卡号，名称，手机 


  ***********************************/ 


BEGIN
IF @pageIds IS NULL or @pageIds = ''''
BEGIN 

declare @sp_CRM_Mem_SearchForAccountChange_Result table (
        Memberid char(32),
		 
        MemberCardNo nvarchar(20),
        CustomerName nvarchar(100),
        CustomerMobile nvarchar(20),
		CustomerStatus nvarchar(100),
		CertificateNoKey nvarchar(100),
		CustomerStatusText nvarchar(100),
        CustomerLevel nvarchar(20),
        CustomerLevelText nvarchar(20),
        RegisterDate datetime,
        RegisterStoreCode nvarchar(100),
        RegisterStoreName nvarchar(100)

)
select * from @sp_CRM_Mem_SearchForAccountChange_Result 
return
END

 -----生成输入的角色对应的权限过滤值
    exec sp_Sys_ReturnAuthValue @dataGroupId,@pageIds,@dataRoleIds 


	declare
			@Sql nvarchar(max) = '''',
			----管理员无法查看会员，其他人员可查看本身下的数据
	        --@Sql_Search nvarchar(max) = '' where 1=1 and g.dataGroupId in (select   subdataGroupId
			      --                                                        from V_Sys_DataGroupRelation
									--									  where  dataGroupId<>6
									--									    and  dataGroupId=''+cast(@dataGroupId as nvarchar(1000))+'') '',

	        @Sql_Search nvarchar(max) = '' where 1=1 
			                                    and exists (select t.memberid,t.RegisterStoreCode from V_R_TM_Mem_StoreCode  t 
												           where  a.memberid=t.memberid 
														      and isnull(a.RegisterStoreCode,''''0'''')=isnull(t.RegisterStoreCode,''''0'''')
												                        )
                                                and a.dataGroupId<>1 
			                                    and a.dataGroupId =''+cast(@dataGroupId as nvarchar(1000))+''
												 '',
			@Sql_VehicleNo  nvarchar(max)=''where 1=1 '',
			@Sql_Count nvarchar(max),
			@Sql_Choose nvarchar(max)='' ''

select * from  V_Sys_DataGroupRelation  

------先期查看性能如何，可考虑改为等值关联


	--if exists (select value from TE_AUTH_TypeFilterValue where type=''store'')

	--begin
	--	set @Sql_Search = @Sql_Search + '' and  a.RegisterStoreCode  in (select value from TE_AUTH_TypeFilterValue where type=''''store'''')''
	--end


--	select * from V_R_TM_Mem_StoreCode 




----待有品牌约束时，恢复
	if exists (select value from TE_AUTH_TypeFilterValueForAccountChange where type=''brand'')

	begin
		set @Sql_Search = @Sql_Search + ''  and exists (select t.memberid,t.RegisterStoreCode 
		                                                   from V_R_TM_Mem_StoreCode  t 														  
												           where  t.RegisterStoreCode in (select StoreCode 
														                                  from V_M_TM_SYS_BaseData_store 
																						  where  StorebrandCode  in (select value 
																						                             from TE_AUTH_TypeFilterValueForAccountChange where type=''''brand''''))
														      and a.memberid=t.memberid 								      
												                        )  ''
	end


	if (isnull(@customerStatus, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerStatus=  ''''''+@customerStatus+'''''' ''
	end


	
	if (isnull(@memberCardNo, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.MemberCardNo like  ''''%''+@memberCardNo+''%'''' ''
	end
	

	if (isnull(@customerName, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerName like   ''''%''+ @customerName + ''%'''' ''    
	end


	if (isnull(@customerMobile, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.customerMobile    =  ''''''+@customerMobile+'''''' ''    
	end


	if (isnull(@carNo, '''') <> '''' )
	begin    
		set @Sql_VehicleNo = @Sql_VehicleNo+'' and carNo   like   ''''%''+ @carNo + ''%'''' ''   
	end

	if (isnull(@vin, '''') <> '''' )
	begin    
		set @Sql_VehicleNo = @Sql_VehicleNo+'' and carNo   like   ''''%''+ @vin + ''%'''' ''   
	end


	if (isnull(@carNo, '''') = ''''   and isnull(@vin, '''') = '''' )
	begin
		set @Sql_Choose = @Sql_Choose + '' left  join ''     
    end
	else 
	begin 
		set @Sql_Choose = @Sql_Choose + '' inner  join ''     
	 end  



set @Sql='' 
from V_U_TM_Mem_Info a 
left join  TD_SYS_BizOption c with (nolock) on a.CustomerStatus=c.OptionValue and c.OptionType=''''CustomerStatus''''
left join  (select OptionValue,OptionType,OptionText from TD_SYS_BizOption with (nolock) 
where datagroupid=''+cast(@dataGroupId as nvarchar(2))+'') e on a.CustomerLevel=e.OptionValue and e.OptionType=''''CustomerLevel''''
left join 	V_M_TM_SYS_BaseData_store g with (nolock) on a.RegisterStoreCode=g.StoreCode
''+ @Sql_Choose +''
 (
         select   a.MemberID              
         from V_M_TM_Mem_SubExt_vehicle a
         ''+@Sql_VehicleNo+''
		 group by a.MemberID  ) f on a.MemberID=f.MemberID 

 ''+@Sql_Search	


	set @Sql_Count = ''select @ct = count(a.MemberID) '' + @Sql 
	exec sp_executesql @Sql_Count,N''@ct int output'',@RecordCount output 



	set @Sql = '' 
select  memberid,MemberCardNo,CustomerName,CustomerMobile,
        CustomerStatus    ,
		CertificateNoKey  ,
		CustomerStatusText,
        CustomerLevel     ,
        CustomerLevelText ,
        RegisterDate      ,
        RegisterStoreCode ,
        RegisterStoreName 
from  
(
select  a.memberid,a.MemberCardNo,a.CustomerName,a.CustomerMobile,

       a.CustomerStatus,a.CertificateNoKey,c.OptionText CustomerStatusText,
        a.CustomerLevel,e.OptionText CustomerLevelText,
		a.RegisterDate,a.RegisterStoreCode,g.StoreName RegisterStoreName,


		ROW_NUMBER() OVER (ORDER BY a.modifieddate desc) RowID

	''+@Sql+'') a  ''

	if (@PageSize is not null and @PageSize>1) 
	begin 
		set @Sql=@Sql+
	''where (RowID-1)/''+convert(nvarchar(100),@PageSize)+''+1 = ''+convert(nvarchar(100),@PageIndex)+''+1 ''
	end 

	set @Sql=@Sql+''order by a.RowID''

      	print @Sql

        exec (@Sql)

END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_SMSSend]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_SMSSend]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE  PROCEDURE [dbo].[sp_CRM_Mem_SMSSend]
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：会员短信提醒
  建 立 人：zyb
  建立时间：2015-07-29 
  修 改 人： 会员升降级提醒以及优惠券套餐到期提醒
             20150805  程焕然 新增其他短信提醒及部分字段值得新增和调整
  ***********************************/ 

BEGIN


----升级提醒 (实时提醒)

insert into TM_Sys_SMSSendingQueue 
(Mobile          ,      
Message         ,
MsgPara         ,
MemberID        ,
ActInstanceID   ,
WorkflowID      ,
TempletID       ,
ReferenceNo     ,
IsSent          ,
AddedDate       ,
AddedUser       ,
Remark          ,
Classify        ,
ReferenceKey    ,
Channel         ,
ChannelResult   ,
PlanSendDate    ,
ActSendDate     ,
IsLogged        )

select 
b.CustomerMobile Mobile          ,      
''尊敬的''+c.BrandName+''会员：恭喜您已升级为''+d.OptionText+''等级会员！'' Message         ,
''{"memberLevel":"''+d.OptionText+''"}'' MsgPara         ,
b.MemberID        ,
null ActInstanceID   ,
null WorkflowID      ,
 (select top 1 TempletID from TM_Act_CommunicationTemplet where Type=''SMS'' AND SubType=''upGrade'' and IsSysInit=1 ) TempletID       ,
null ReferenceNo     ,
0  IsSent          ,
getdate() AddedDate       ,
1000  AddedUser       ,
''会员定时升级短信提醒''    Remark          ,
null Classify        ,
null ReferenceKey    ,
2  Channel         ,----渠道是否更改？
null ChannelResult   ,
null  PlanSendDate    ,
null  ActSendDate     ,
null  IsLogged     
from TL_Mem_LevelChange a
inner join V_U_TM_Mem_Info b on a.MemberID=b.MemberID
inner join V_M_TM_SYS_BaseData_brand c on b.DataGroupID=c.DataGroupID
inner join TD_SYS_BizOption d on d.OptionType=''CustomerLevel'' and b.DataGroupID=d.DataGroupID and b.CustomerLevel=d.OptionValue
where b.IsMessage=1  and b.DataGroupID=3
  and convert(nvarchar(8),a.AddedDate,112)=convert(nvarchar(8),dateadd(day ,-1,getdate()),112)
  and a.ChangeLevelTo>a.ChangeLevelFrom 



-----降级提醒（忠诚度定时执行）

insert into TM_Sys_SMSSendingQueue 
(Mobile          ,      
Message         ,
MsgPara         ,
MemberID        ,
ActInstanceID   ,
WorkflowID      ,
TempletID       ,
ReferenceNo     ,
IsSent          ,
AddedDate       ,
AddedUser       ,
Remark          ,
Classify        ,
ReferenceKey    ,
Channel         ,
ChannelResult   ,
PlanSendDate    ,
ActSendDate     ,
IsLogged        )

select 
b.CustomerMobile Mobile          ,      
''尊敬的''+c.BrandName+''会员：您的等级已变更为''+d.OptionText+''等级会员！'' Message         ,
''{"memberLevel":"''+d.OptionText+''"}'' MsgPara         ,
b.MemberID        ,
null ActInstanceID   ,
null WorkflowID      ,
(select top 1 TempletID from TM_Act_CommunicationTemplet where Type=''SMS'' AND SubType=''downGrade'' and IsSysInit=1 ) TempletID       ,
null ReferenceNo     ,
0  IsSent          ,
getdate() AddedDate       ,
1000  AddedUser       ,
''会员定时降级短信提醒''    Remark          ,
null Classify        ,
null ReferenceKey    ,
2  Channel         ,----渠道是否更改？
null ChannelResult   ,
null  PlanSendDate    ,
null  ActSendDate     ,
null  IsLogged     
from TL_Mem_LevelChange a
inner join V_U_TM_Mem_Info b on a.MemberID=b.MemberID
inner join V_M_TM_SYS_BaseData_brand c on b.DataGroupID=c.DataGroupID
inner join TD_SYS_BizOption d on d.OptionType=''CustomerLevel'' and b.DataGroupID=d.DataGroupID and b.CustomerLevel=d.OptionValue
where b.IsMessage=1   and b.DataGroupID=3
  and convert(nvarchar(8),a.AddedDate,112)=convert(nvarchar(8),dateadd(day ,-1,getdate()),112)
  and a.ChangeLevelTo<a.ChangeLevelFrom 



----优惠券到期提醒 (根据提醒日期前多少天发送进行修改时间天数)


insert into TM_Sys_SMSSendingQueue 
(Mobile          ,      
Message         ,
MsgPara         ,
MemberID        ,
ActInstanceID   ,
WorkflowID      ,
TempletID       ,
ReferenceNo     ,
IsSent          ,
AddedDate       ,
AddedUser       ,
Remark          ,
Classify        ,
ReferenceKey    ,
Channel         ,
ChannelResult   ,
PlanSendDate    ,
ActSendDate     ,
IsLogged        )


select 
b.CustomerMobile Mobile          ,  
''尊敬的''++c.BrandName+''会员：您的优惠券''+t.Name+''即将过期，有效期至''+convert(nvarchar(8),a.EndDate,112)+''，请您及时使用''     Message         ,
---''尊敬的''+c.BrandName+''会员：您有优惠券即将过期，有效期至''+convert(nvarchar(8),a.EndDate,112)+''，请您及时使用。'' Message         ,
null MsgPara         ,
b.MemberID        ,
null ActInstanceID   ,
null WorkflowID      ,
null TempletID       ,
null ReferenceNo     ,
0  IsSent          ,
getdate() AddedDate       ,
1000  AddedUser       ,
''优惠券到期短信提醒''    Remark          ,
null Classify        ,
null ReferenceKey    ,
2  Channel         ,----渠道是否更改？
null ChannelResult   ,
null  PlanSendDate    ,
null  ActSendDate     ,
null  IsLogged     
from TM_Mem_CouponPool a
inner join V_U_TM_Mem_Info b on a.MemberID=b.MemberID
inner join V_M_TM_SYS_BaseData_brand c on b.DataGroupID=c.DataGroupID
left join TM_Act_CommunicationTemplet t on a.TempletID=t.TempletID
where b.IsMessage=1   and b.DataGroupID=3
  and convert(nvarchar(8),a.EndDate,112)=convert(nvarchar(8),dateadd(day,30,getdate()),112)
  and a.Enable=1 and a.IsUsed=0  




----会员套餐到期提醒 


insert into TM_Sys_SMSSendingQueue 
(Mobile          ,      
Message         ,
MsgPara         ,
MemberID        ,
ActInstanceID   ,
WorkflowID      ,
TempletID       ,
ReferenceNo     ,
IsSent          ,
AddedDate       ,
AddedUser       ,
Remark          ,
Classify        ,
ReferenceKey    ,
Channel         ,
ChannelResult   ,
PlanSendDate    ,
ActSendDate     ,
IsLogged        )


select 
b.CustomerMobile Mobile          , 

''尊敬的''+c.BrandName+''会员：您的套餐''+a.PackageName+''即将过期，有效期至''+convert(nvarchar(8),a.EndDate,112)+''，请您及时使用''    Message         ,   
---''尊敬的''+c.BrandName+''会员：您有套餐即将过期，有效期至''+convert(nvarchar(8),a.EndDate,112)+''，请您及时使用。回复TD退订'' Message         ,
null MsgPara         ,
b.MemberID        ,
null ActInstanceID   ,
null WorkflowID      ,
null TempletID       ,
null ReferenceNo     ,
0  IsSent          ,
getdate() AddedDate       ,
1000  AddedUser       ,
''套餐到期短信提醒''    Remark          ,
null Classify        ,
null ReferenceKey    ,
2  Channel         ,----渠道是否更改？
null ChannelResult   ,
null  PlanSendDate    ,
null  ActSendDate     ,
null  IsLogged     
from TM_Mem_Package a
inner join (select PackageInstanceID
            from TM_Mem_PackageDetail
			group by PackageInstanceID having sum(Qty)>0 ) t on a.PackageInstanceID=t.PackageInstanceID
inner join V_U_TM_Mem_Info b on a.MemberID=b.MemberID
inner join V_M_TM_SYS_BaseData_brand c on b.DataGroupID=c.DataGroupID
where b.IsMessage=1   and b.DataGroupID=3
  and convert(nvarchar(8),a.EndDate,112)=convert(nvarchar(8),dateadd(day,30,getdate()),112)
  and a.Enable=1  

  select * from TM_Mem_Package 


  --会籍即将过期前，提醒会员

  
insert into TM_Sys_SMSSendingQueue 
(Mobile          ,      
Message         ,
MsgPara         ,
MemberID        ,
ActInstanceID   ,
WorkflowID      ,
TempletID       ,
ReferenceNo     ,
IsSent          ,
AddedDate       ,
AddedUser       ,
Remark          ,
Classify        ,
ReferenceKey    ,
Channel         ,
ChannelResult   ,
PlanSendDate    ,
ActSendDate     ,
IsLogged        )


select 
a.CustomerMobile,
''尊敬的''+c.brandName+''斯巴鲁会员：您的会籍期将于''+convert(nvarchar(8),a.MemberEndDate,112)+''到期！请及时与您的服务顾问联系续费事宜'',
''{"expireDate":"''+convert(nvarchar(8),a.MemberEndDate,112)+''"}'',
a.MemberID,
null ActInstanceID   ,
null WorkflowID      ,
(select top 1 TempletID from TM_Act_CommunicationTemplet where Type=''SMS'' AND SubType=''memberExpire'' and IsSysInit=1 ) TempletID ,
null ReferenceNo     ,
0  IsSent          ,
getdate() AddedDate       ,
1000  AddedUser       ,
''会籍即将过期前提醒''    Remark          ,
null Classify        ,
null ReferenceKey    ,
2  Channel         ,----渠道是否更改？
null ChannelResult   ,
null  PlanSendDate    ,
null  ActSendDate     ,
null  IsLogged     
 from V_S_TM_Mem_Ext a
 inner join V_S_TM_Mem_Master b on a.MemberID=b.MemberID
 inner join V_M_TM_SYS_BaseData_brand c on b.DataGroupID=c.DataGroupID
where convert(nvarchar(8),a.MemberEndDate,112)=convert(nvarchar(8),dateadd(day,30,getdate()),112)  and b.DataGroupID=3


  --会员降级前，提醒会员

  
insert into TM_Sys_SMSSendingQueue 
(Mobile          ,      
Message         ,
MsgPara         ,
MemberID        ,
ActInstanceID   ,
WorkflowID      ,
TempletID       ,
ReferenceNo     ,
IsSent          ,
AddedDate       ,
AddedUser       ,
Remark          ,
Classify        ,
ReferenceKey    ,
Channel         ,
ChannelResult   ,
PlanSendDate    ,
ActSendDate     ,
IsLogged        )


select 
a.CustomerMobile,
''尊敬的''+c.brandName+''斯巴鲁会员：根据目前您的消费，您的当前等级将于30天后降级！若您于''+convert(nvarchar(8),a.MemberLevelEndDate,112)+''之前再消费''
--+ d.EchelonConsumeAmt- d.MemAccPurchaseAmount_12+''元，将能成功保级''
,''{"date":"''+convert(nvarchar(8),a.MemberLevelEndDate,112)+''"}'',
a.MemberID,
null ActInstanceID   ,
null WorkflowID      ,
(select top 1 TempletID from TM_Act_CommunicationTemplet where Type=''SMS'' AND SubType=''beforeDownGrade'' and IsSysInit=1 ) TempletID ,
null ReferenceNo     ,
0  IsSent          ,
getdate() AddedDate       ,
1000  AddedUser       ,
''会员降级前提醒''    Remark          ,
null Classify        ,
null ReferenceKey    ,
2  Channel         ,----渠道是否更改？
null ChannelResult   ,
null  PlanSendDate    ,
null  ActSendDate     ,
null  IsLogged     
 from V_S_TM_Mem_Ext a
 inner join V_S_TM_Mem_Master b on a.MemberID=b.MemberID
 inner join V_M_TM_SYS_BaseData_brand c on b.DataGroupID=c.DataGroupID
 inner join V_S_TM_Loy_MemExt d on a.MemberID=d.MemberID
where convert(nvarchar(8),a.MemberLevelEndDate,112)=convert(nvarchar(8),dateadd(day,30,getdate()),112) 
--and d.EchelonConsumeAmt>d.MemAccPurchaseAmount_12  and b.DataGroupID=3



--会员生日醒
  
insert into TM_Sys_SMSSendingQueue 
(Mobile          ,      
Message         ,
MsgPara         ,
MemberID        ,
ActInstanceID   ,
WorkflowID      ,
TempletID       ,
ReferenceNo     ,
IsSent          ,
AddedDate       ,
AddedUser       ,
Remark          ,
Classify        ,
ReferenceKey    ,
Channel         ,
ChannelResult   ,
PlanSendDate    ,
ActSendDate     ,
IsLogged        )


select 
a.CustomerMobile,
''尊敬的''+c.brandName+''斯巴鲁会员：您的生日是''+ SUBSTRING(convert(nvarchar(10),a.Birthday,121),6,5)+''，斯巴鲁全体员工真诚的祝您：生日快乐！'',
''{"birthdayDate":"''+SUBSTRING(convert(nvarchar(10),a.Birthday,121),6,5)+''"}'',
a.MemberID,
null ActInstanceID   ,
null WorkflowID      ,
(select top 1 TempletID from TM_Act_CommunicationTemplet where Type=''SMS'' AND SubType=''birthday'' and IsSysInit=1 ) TempletID ,
null ReferenceNo     ,
0  IsSent          ,
getdate() AddedDate       ,
1000  AddedUser       ,
''会员生日提醒''    Remark          ,
null Classify        ,
null ReferenceKey    ,
2  Channel         ,----渠道是否更改？
null ChannelResult   ,
null  PlanSendDate    ,
null  ActSendDate     ,
null  IsLogged     
 from V_U_TM_Mem_Info a
 inner join V_M_TM_SYS_BaseData_brand c on a.DataGroupID=c.DataGroupID
where SUBSTRING(convert(nvarchar(10),a.Birthday,121),6,5)=SUBSTRING(convert(nvarchar(10),getdate(),121),6,5)
 and a.DataGroupID=3


end
















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Mem_UpNeedPoint]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Mem_UpNeedPoint]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_CRM_Mem_UpNeedPoint]

	@memberid nvarchar(100),
	@upneedpoint decimal(16,2) out 
	
	
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：查找返回会员本等级升级所需的积分数
  建 立 人：
  建立时间：2015-03-03
  修 改 人： 


  ***********************************/ 



BEGIN 


	declare
			@Sql nvarchar(max) = '''',
			@Sqlup nvarchar(max) = ''''
			

set @Sql=''from(
select a.MemberID,
 case when a.MemberLevel = ''''Normal'''' THEN  ''''Copper''''
  when a.MemberLevel = ''''Copper'''' THEN  ''''Silver''''
  when a.MemberLevel = ''''Silver'''' THEN  ''''Gold''''
  when a.MemberLevel = ''''Gold'''' THEN  ''''Platinum''''
 ELSE '''''''' 
 END up_level
 from tm_mem_master a 
 where a.MemberID = ''''''+@memberid+''''''
 )b left join V_M_TM_SYS_BaseData_customerlevel c
 on b.up_level = c.CustomerLevelBase
 left join V_S_TM_Loy_MemExt d
 on b.MemberID = d.MemberID
''

set @Sqlup = ''select @ct = case when c.LevelUpInt-isnull(d.ActConsumption,0)<0 then null
else c.LevelUpInt-isnull(d.ActConsumption,0)  
end  '' + @Sql
 exec sp_executesql @Sqlup,N''@ct int output'',@upneedpoint output

print @Sqlup

END



' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_MemGradeAdjust]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_MemGradeAdjust]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROC [dbo].[sp_CRM_MemGradeAdjust]
(
	@dataGroupId nvarchar(10),
	@vipCode nvarchar(40),
	@vipName nvarchar(20),
	@mobilNO nvarchar(20),
	@plateNO nvarchar(40),
	@PageIndex int,
    @PageSize int,
	@RecordCount int out	
)
as

	IF (@dataGroupId is null or @dataGroupId='''') or( (@vipCode IS NULL or @vipCode = '''') and ( @vipName IS NULL or @vipName = '''') and  (@mobilNO IS NULL or @mobilNO = '''') and (@plateNO IS NULL or @plateNO = ''''))
	BEGIN  
		declare @tblResult table (
		  VipCode nvarchar(50),
          VipName nvarchar(50),
		  MobilNO   nvarchar(40),
		  CurrLevel nvarchar(20),
		  DateStart DateTime,
		  DateEnd DateTime
		)

		select * from @tblResult 
		 return
	 END

	  declare 
	        @Sql_Search  nvarchar(max),
	        @Sql         nvarchar(max),
			@Sql_From         nvarchar(max),
			@Sql_Count nvarchar(max)

	set @Sql_Search = ''where 1=1 ''

	if (isnull(@vipCode,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + N'' and a.MemberCardNo = ''''''+@vipCode+'''''' ''
	end
	if (isnull(@vipName,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + N'' and a.CustomerName like ''''%''+@vipName+N''%''''''
	end
	if (isnull(@mobilNO,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + N'' and a.CustomerMobile = ''''''+@mobilNO+N'''''' ''
	end
	if (isnull(@plateNO,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + N'' and b.CarNo like ''''%''+ @plateNO+N''%'''' ''
	end
		set @Sql_Search = @Sql_Search + N'' and c.DataGroupID = ''''''+@dataGroupId+N'''''' ''

	 set @Sql_From=N'' 
from(
select distinct a.MemberCardNo VipCode,a.CustomerName VipName,a.CustomerMobile MobilNO,d.OptionText CurrLevel,
a.MemberLevelStartDate DateStart,a.MemberLevelEndDate DateEnd,ROW_NUMBER() OVER (ORDER BY a.MemberCardNo) RowID
from V_S_TM_Mem_Ext a 
left join V_M_TM_Mem_SubExt_vehicle b on a.MemberID=b.MemberID
inner join V_S_TM_Mem_Master c on a.MemberID=c.MemberID
inner join TD_SYS_BizOption d on c.CustomerLevel=d.OptionValue and d.DataGroupID=c.DataGroupID '' + @Sql_Search

set @Sql_From=@Sql_From+N'' ) tab ''

	set @Sql_Count = ''select @ct = count(tab.VipCode) '' + @Sql_From 
	exec sp_executesql @Sql_Count,N''@ct int output'',@RecordCount output 

begin

set @Sql=N''select * ''+ @Sql_From

 if (@PageSize is not null and @PageSize>1) 
	begin 
		set @Sql=@Sql+
	'' where (RowID-1)/''+convert(nvarchar(100),@PageSize)+''+1 = ''+convert(nvarchar(100),@PageIndex)+''+1''
	end 

	print @Sql
    exec (@Sql)
end










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRM_Pro_InsertMemberID]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CRM_Pro_InsertMemberID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE procedure [dbo].[sp_CRM_Pro_InsertMemberID] 
as
 /**********************************
  ----arvarto system-----
  存储过程功能描述：新增会员细分实例对应的会员写入到促销活动对应的会员明细表中（当天算采用的getdate()）
  建 立 人：zyb
  建立时间：2015-10-09
  修改内容: 20151009  zyb TM_POS_MemberPromotion中存放的会员是指促销活动对应的会员细分所对应的当前细分实例；
                          促销活动是一对多的关系，不可直接删除临时表细分对应促销活动ID在TM_POS_MemberPromotion中的数据，
						  及同一细分下对应的非当前实例的memberid；

 后续明确点：
 1.TM_POS_MemberPromotion显示的是当前会员可享受的优惠促销活动
 2.全量的当前的细分实例去插入，插入前删除TM_POS_MemberPromotion数据

 20151010 zyb 增加PromotionCode，修改PromotionID取会员的promotionID;
 20151027 zyb 增加FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF对应全部会员
 20151030 zyb 删除对应的全部会员的情况，KPI指标增量此约束；
  ***********************************/    
  BEGIN



	declare
			@Sql varchar(max) = ''''



--清空游标
truncate table TE_Mem_TableName_MemSubIns

---select * from TE_Mem_TableName_MemSubIns   

---

DECLARE MyCursor CURSOR 
FOR 

select a.SubdivisionID, a.TableName 
from TM_Mem_SubdivisionInstance   a  ,TM_Mem_Subdivision b
where  a.SubdivisionInstanceID=b.CurSubdivisionInstanceID  and b.CurSubdivisionInstanceID is not null 
union all
----CurSubdivisionInstanceID 为空时；
select t.SubdivisionID, t.TableName
from (
select a.SubdivisionID,a.TableName ,ROW_NUMBER() over (partition by a.SubdivisionID order by a.addeddate desc ) serial
from TM_Mem_SubdivisionInstance   a  ,TM_Mem_Subdivision b
where  a.SubdivisionID=b.SubdivisionID  and b.CurSubdivisionInstanceID is  null ) t 
 


  


--打开一个游标	
OPEN MyCursor
--循环一个游标
DECLARE @SubdivisionID  nvarchar(2000) 	 ,@TableName nvarchar(2000) 	
FETCH NEXT FROM  MyCursor INTO @SubdivisionID,@TableName
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql1 nvarchar(max);
    set @sql1 =''
	          insert into TE_Mem_TableName_MemSubIns
              select ''''''+@SubdivisionID+'''''',  ''''''+@tablename+'''''',memberid
              from [''+@tablename+''] 
			  group by memberid''   
   exec (@sql1)

FETCH NEXT FROM  MyCursor INTO @SubdivisionID, @TableName
END	
--关闭游标
CLOSE MyCursor
--释放资源
DEALLOCATE MyCursor


    set @Sql=''
truncate table TM_POS_MemberPromotion 

insert into TM_POS_MemberPromotion 
(memberid,membercode,PromotionID,AddedDate,ModifiedDate)
select a.memberid,c.MemberCode,d.PromotionID,getdate(),getdate()        
from   TE_Mem_TableName_MemSubIns    a 
inner join TR_SYS_Common b  on b.RelationGuidValue1=a.SubdivisionID  
inner join V_S_TM_Mem_Ext c on a.memberid=c.MemberID
inner join V_M_TM_SYS_BaseData_promotion d on b.RelationBigintValue1=d.BaseDataID
where b.relationtype=''''1''''


''
      	print @Sql
        exec (@Sql)


end


--union all
--select c.memberid,c.MemberCode,d.PromotionID,getdate(),getdate() 
--from   --TE_Mem_TableName_MemSubIns    a 
----inner join 
--TR_SYS_Common b  --on b.RelationGuidValue1=a.SubdivisionID  
--cross join V_S_TM_Mem_Ext c  --on a.memberid=c.MemberID
--inner join V_M_TM_SYS_BaseData_promotion d on b.RelationBigintValue1=d.BaseDataID
--where b.relationtype=''''1'''' and b.RelationGuidValue1=''''FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF''''



--create table TE_Mem_TableName_MemSubIns
--(
--SubdivisionID uniqueidentifier,
--TableName nvarchar(100),
--MemberID char(32)

--)


' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRule]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRule]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE  PROCEDURE [dbo].[sp_Loy_AccountComputeForActRule]
	@ActRuleTable nvarchar(3000) ,
	@TypeTable  nvarchar(3000)  
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：根据忠诚度的计算规则插入更新相应的交易账户相关表中的字段的值；(计算规则的得出,需tradeid)
  建 立 人：zyb
  建立时间：2015-04-14
  修 改 人：20150422 zyb 根据limit中的type去笛卡尔，插入更新相关的账户表，明细表中增加limittype,SpecialDate1等 （摒弃）
            20150505 zyb 根据limit中的type取笛卡尔，更新插入到帐户限制表，明细表增加账户冻结还是可用的类型；对于车辆的限制的是其车辆表主键ID；


  规则表：sample:TempActRule, tmpRuleAct125021237
  限制表：sample:TempLimitType 
  ***********************************/ 

BEGIN




---可用积分,冻结积分更新插入
exec (''MERGE INTO  TM_Mem_AccountDetail a                                                                                                                                                                                                                               
USING (select AccountID,fieldname AccountDetailType,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when OffsetExpression is null then ComputeValue end ) DetailValue,
               SpecialDate1, SpecialDate2 ,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from 	   ''+@ActRuleTable+''  a
      group by AccountID,fieldname,SpecialDate1,SpecialDate2) b                                                                                                               
ON ( a.AccountID=b.AccountID 
 and a.AccountDetailType=b.AccountDetailType
 and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(b.SpecialDate1,''''2000-01-01'''')  
 and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(b.SpecialDate2,''''2000-01-01'''')   
  )  
WHEN MATCHED THEN                                                                                                              
UPDATE                                                                                                                    
SET  
a.DetailValue         =a.DetailValue+b.DetailValue        ,
a.ModifiedDate                 =b.ModifiedDate                                                                                                                                                                                   
WHEN NOT MATCHED THEN                                                                                                          
INSERT                                                                                                                         
(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser)                                                                                                                            
values                                                                                                                         
(b.AccountID,b.AccountDetailType,b.DetailValue,b.SpecialDate1,b.SpecialDate2,b.AddedDate,b.AddedUser,b.ModifiedDate,b.ModifiedUser);  
'')




---更新插入账户限制  TM_Mem_AccountLimit

exec (''
MERGE INTO  TM_Mem_AccountLimit a                                                                                                                                                                                                                               
USING(select AccountID,AccountDetailID,LimitType, LimitValue, getdate()  AddedDate 
      from 
      (
       select a.* ,t.AccountDetailID,b.StoreCodeservice StoreCode,c.StoreBrandCode ,
	             d.Type LimitType,
       case   when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10))
              when type=''''store''''   then StoreCode 
			  when type=''''brand''''   then StoreBrandCode  end LimitValue 
			  	  
       from ''+@ActRuleTable+''  a
	   inner join TM_Mem_AccountDetail t on 
	              a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''')  
       inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
       inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
       inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
       cross  join  ''+@TypeTable+''    d 
	    )t
	   group by  AccountID,AccountDetailID,LimitType, LimitValue     )b                                                                                                               
ON ( a.AccountID=b.AccountID 
 and a.AccountDetailID=b.AccountDetailID
 and a.LimitType=b.LimitType
 and a.LimitValue=b.LimitValue 
  )                                                                                                                                                                                 
WHEN NOT MATCHED THEN                                                                                                          
INSERT                                                                                                                         
(AccountID,AccountDetailID,LimitType,LimitValue,AddedDate)                                                                                                                            
values                                                                                                                         
(b.AccountID,b.AccountDetailID,b.LimitType,b.LimitValue,b.AddedDate);  
'')



----更新账户表中积分账户的可用积分;
---删除AccountType=2，通过AccountID判别其为积分或者积点账户

exec(''
update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when OffsetExpression is null then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value1''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')


---更新账户表中积分账户的可用积分



exec(''
update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when OffsetExpression is null then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value2''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')



-----明细记录值插入 



exec(''
insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
AddedDate         ,
AddedUser          
)
select           
a.AccountID         ,
b.AccountDetailID   ,
''''loy'''' AccountChangeType ,  
(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                  when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			          when OffsetExpression is null then ComputeValue end )   ChangeValue,  
''''忠诚度发放积分'''' ChangeReason      ,
a.TradeID ReferenceNo       ,
0 HasReverse        ,  ------？？？？
getdate() AddedDate         ,
''''1000'''' AddedUser          
from   ''+@ActRuleTable+''  a
inner join TM_Mem_AccountDetail b  with (nolock)
  on   a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType 
    and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(b.SpecialDate1,''''2000-01-01'''')  
    and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(b.SpecialDate2,''''2000-01-01'''')  

'')

END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRule0604]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRule0604]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE  PROCEDURE [dbo].[sp_Loy_AccountComputeForActRule0604]
	@ActRuleTable nvarchar(3000) ,
	@TypeTable  nvarchar(3000)  
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：根据忠诚度的计算规则插入更新相应的交易账户相关表中的字段的值；(计算规则的得出,需tradeid)
  建 立 人：zyb
  建立时间：2015-04-14
  修 改 人：20150422 zyb 根据limit中的type去笛卡尔，插入更新相关的账户表，明细表中增加limittype,SpecialDate1等 （摒弃）
            20150505 zyb 根据limit中的type取笛卡尔，更新插入到帐户限制表，明细表增加账户冻结还是可用的类型；对于车辆的限制的是其车辆表主键ID；
			20150604 zyb 增加约束，交易的积分的账户限定类型，以及限定类型的值不同要创建不同的账户明细ID
			20150604 zyb 修改when isnull(OffsetExpression,'''''''')='''''''' 改为when isnull(OffsetExpression,'''''''')=''''''''

  规则表：sample:TempActRule, tmpRuleAct125021237
  限制表：sample:TempLimitType 可能无品牌的限制
  ***********************************/ 

BEGIN


---可用积分,冻结积分更新插入
exec (''

 if exists (select type  from  ''+@TypeTable+'')   ---非空时 

-----更新部分

update TM_Mem_AccountDetail 
set TM_Mem_AccountDetail.DetailValue         =TM_Mem_AccountDetail.DetailValue+temp.DetailValue        ,
    TM_Mem_AccountDetail.ModifiedDate        =getdate()      

from  (
	 
	      select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue 
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) StoreCode,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			cross  join  ''+@TypeTable+'' d 
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         inner join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
           
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2  ) temp 
where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID 


--------插入部分
insert into TM_Mem_AccountDetail 
(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser) 


	      select m.AccountID,m.AccountDetailType,t.DetailValue,
		         case when t.SpecialDate1=''''2000-01-01'''' then null else t.SpecialDate1 end SpecialDate1,
				 case when t.SpecialDate2=''''2000-01-01'''' then null else t.SpecialDate2 end SpecialDate2,
		        t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) StoreCode,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			cross  join  ''+@TypeTable+'' d 
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         left join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
     
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2 
    where n.AccountID is null
	 and  n.AccountDetailType  is null
	 and  n.SpecialDate1 is null   
	 and  n.SpecialDate2 is null 
     and  n.vehicle is null 
	 and  n.StoreCode is null 
	 and  n.StoreBrandCode is null 


 if not exists (select type  from  ''+@TypeTable+'')   ---空时 


 
update TM_Mem_AccountDetail 
set TM_Mem_AccountDetail.DetailValue         =TM_Mem_AccountDetail.DetailValue+temp.DetailValue        ,
    TM_Mem_AccountDetail.ModifiedDate        =getdate()      

from  (
	 
	      select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue 
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          , ''''0'''' vehicle, ''''0''''  StoreCode, ''''0''''  StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         inner join 

     ---------------历史账户明细限制值 (left 存在不作限制的账户明细ID)  
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
                   
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2  ) temp 
where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID 


--------插入部分
insert into TM_Mem_AccountDetail 
(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser) 


	      select m.AccountID,m.AccountDetailType,t.DetailValue,
		         case when t.SpecialDate1=''''2000-01-01'''' then null else t.SpecialDate1 end SpecialDate1,
				 case when t.SpecialDate2=''''2000-01-01'''' then null else t.SpecialDate2 end SpecialDate2,
		        t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,''''0'''' vehicle, ''''0''''  StoreCode, ''''0''''  StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         left join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
          
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2 
    where n.AccountID is null
	 and  n.AccountDetailType  is null
	 and  n.SpecialDate1 is null   
	 and  n.SpecialDate2 is null 
     and  n.vehicle is null 
	 and  n.StoreCode is null 
	 and  n.StoreBrandCode is null 		   		 
'')




---更新插入账户限制  TM_Mem_AccountLimit

exec (''  insert into TM_Mem_AccountLimit
        (AccountID,AccountDetailID,LimitType,LimitValue,AddedDate)    
---------------行转列，去掉值为0的数据
         select AccountID,AccountDetailID, attribute LimitType,value LimitValue ,getdate()
		 from 

	     ( select m.AccountID,m.AccountDetailID,
		         cast(m.vehicle as nvarchar(20)) vehicle ,
				 cast(m.store as nvarchar(20)) store ,
				 cast(m.brand as nvarchar(20)) brand  
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,t.AccountDetailID,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) store,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) brand 
			from   ''+@ActRuleTable+''  a
		    inner join   
			            ------由于限制类型尚未添加，只能取同accountid,AccountDetailType,SpecialDate1,SpecialDate2 刚刚创建插入的那条数据记录
			            (select x.*  from 
			            (select x.*,ROW_NUMBER() over(partition by accountid,AccountDetailType,SpecialDate1,SpecialDate2  order by addeddate desc ) serial 
			            from TM_Mem_AccountDetail x  )  x
						where serial=1 ) t on 
	              a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''')
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			cross  join  ''+@TypeTable+'' d 
			group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
	     left join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) store,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) brand 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand		
    where n.AccountID is null
	 and  n.AccountDetailType  is null
	 and  n.SpecialDate1 is null   
	 and  n.SpecialDate2 is null 
     and  n.vehicle is null 
	 and  n.store is null 
	 and  n.brand is null ) t

 UNPIVOT
  (
    value FOR attribute IN (store, brand,vehicle)
  ) AS b
where value<>''''0''''
  
'')



----更新账户表中积分账户的可用积分;
---删除AccountType=2，通过AccountID判别其为积分或者积点账户

exec(''
update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value1''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')


---更新账户表中积分账户的可用积分



exec(''
update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value2''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')



-----明细记录值插入 

exec(''
if exists (select type  from  ''+@TypeTable+'')   ---非空时 

insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
RuleID,
AddedDate         ,
AddedUser          
)
select           
m.AccountID         ,
m.AccountDetailID   ,
''''loy'''' AccountChangeType ,  
(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                  when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			          when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end )   ChangeValue,  
''''忠诚度发放积分'''' ChangeReason      ,
t.TradeID ReferenceNo       ,
0 HasReverse                ,  
t.RuleID                    ,
getdate()  AddedDate        ,
''''1000''''   AddedUser          
from 

	      (  select * 
		  from (
	      (select a.AccountID,fieldname AccountDetailType,t.AccountDetailID,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle1,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) store1,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) brand1, f.store,f.brand,f.vehicle
			from   ''+@ActRuleTable+''   a
		    inner join 
		         TM_Mem_AccountDetail	t on 
	              a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''')
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			inner join   (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle
			              from (
			              select AccountID,AccountDetailID,isnull(store,''''0'''') store,isnull(brand,''''0'''') brand,isnull(vehicle,''''0'''') vehicle
		                  from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t 
						  group by  AccountID,AccountDetailID 
						  )  f 
						  on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID
			cross  join ''+@TypeTable+''  d 

			group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''') ,f.store,f.brand,f.vehicle )  ) w
			where store=store1 and brand=brand1 and vehicle=vehicle1)  m
	     inner join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) store,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) brand 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand	
		 inner join   ''+@ActRuleTable+''  t 
		 on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname    
		  and m.SpecialDate1=isnull(t.SpecialDate1,''''2000-01-01'''')  
		  and m.SpecialDate2=isnull(t.SpecialDate2,''''2000-01-01'''')



if not exists (select type  from  ''+@TypeTable+'')   ---空时 

insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
RuleID,
AddedDate         ,
AddedUser          
)
select           
m.AccountID         ,
m.AccountDetailID   ,
''''loy'''' AccountChangeType ,  
(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                  when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			          when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end )   ChangeValue,  
''''忠诚度发放积分'''' ChangeReason      ,
t.TradeID ReferenceNo       ,
0 HasReverse                ,  
t.RuleID                    ,
getdate()  AddedDate        ,
''''1000''''   AddedUser          
from 

	     (  select w.*
		  from 
	      (select a.AccountID,fieldname AccountDetailType,t.AccountDetailID,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,''''0''''   vehicle1,     ''''0''''  store1,	    ''''0''''  brand1,isnull(f.store,0) store, isnull(f.brand,0) brand,isnull(f.vehicle,0) vehicle
			from   ''+@ActRuleTable+''  a
	          inner join TM_Mem_AccountDetail t on 
			      a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''') 

			left join (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle
			              from (
			              select AccountID,AccountDetailID,isnull(store,''''0'''') store,isnull(brand,''''0'''') brand,isnull(vehicle,''''0'''') vehicle
		                  from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t 
						  group by  AccountID,AccountDetailID 
						  )  f 
						  on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID
			group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''') ,f.store,f.brand,f.vehicle  )w
			where  store=store1 and brand=brand1 and vehicle=vehicle1 )  m
	     inner join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) store,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) brand 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand	
		 inner join   ''+@ActRuleTable+''  t 
		 on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname    
		  and m.SpecialDate1=isnull(t.SpecialDate1,''''2000-01-01'''')  
		  and m.SpecialDate2=isnull(t.SpecialDate2,''''2000-01-01'''')

'')




		   	


END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRule1008]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRule1008]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE  PROCEDURE [dbo].[sp_Loy_AccountComputeForActRule1008]
	@ActRuleTable nvarchar(3000) ,
	@TypeTable  nvarchar(3000)  
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：根据忠诚度的计算规则插入更新相应的交易账户相关表中的字段的值；(计算规则的得出,需tradeid)
  建 立 人：zyb
  建立时间：2015-04-14
  修 改 人：20150422 zyb 根据limit中的type去笛卡尔，插入更新相关的账户表，明细表中增加limittype,SpecialDate1等 （摒弃）
            20150505 zyb 根据limit中的type取笛卡尔，更新插入到帐户限制表，明细表增加账户冻结还是可用的类型；对于车辆的限制的是其车辆表主键ID；
			20150604 zyb 增加约束，交易的积分的账户限定类型，以及限定类型的值不同要创建不同的账户明细ID
			20150604 zyb 修改when isnull(OffsetExpression,'''''''')='''''''' 改为when isnull(OffsetExpression,'''''''')=''''''''

  规则表：sample:TempActRule, tmpRuleAct125021237
  限制表：sample:TempLimitType 可能无品牌的限制
  ***********************************/ 

BEGIN


---可用积分,冻结积分更新插入
exec (''

 if exists (select type  from  ''+@TypeTable+'')   ---非空时 

-----更新部分

update TM_Mem_AccountDetail 
set TM_Mem_AccountDetail.DetailValue         =TM_Mem_AccountDetail.DetailValue+temp.DetailValue        ,
    TM_Mem_AccountDetail.ModifiedDate        =getdate()      

from  (
	 
	      select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue 
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) StoreCode,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			cross  join  ''+@TypeTable+'' d 
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         inner join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
           
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2  ) temp 
where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID 


--------插入部分
insert into TM_Mem_AccountDetail 
(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser) 


	      select m.AccountID,m.AccountDetailType,t.DetailValue,
		         case when t.SpecialDate1=''''2000-01-01'''' then null else t.SpecialDate1 end SpecialDate1,
				 case when t.SpecialDate2=''''2000-01-01'''' then null else t.SpecialDate2 end SpecialDate2,
		        t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) StoreCode,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			cross  join  ''+@TypeTable+'' d 
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         left join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
     
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2 
    where n.AccountID is null
	 and  n.AccountDetailType  is null
	 and  n.SpecialDate1 is null   
	 and  n.SpecialDate2 is null 
     and  n.vehicle is null 
	 and  n.StoreCode is null 
	 and  n.StoreBrandCode is null 


 if not exists (select type  from  ''+@TypeTable+'')   ---空时 


 
update TM_Mem_AccountDetail 
set TM_Mem_AccountDetail.DetailValue         =TM_Mem_AccountDetail.DetailValue+temp.DetailValue        ,
    TM_Mem_AccountDetail.ModifiedDate        =getdate()      

from  (
	 
	      select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue 
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          , ''''0'''' vehicle, ''''0''''  StoreCode, ''''0''''  StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         inner join 

     ---------------历史账户明细限制值 (left 存在不作限制的账户明细ID)  
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
                   
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2  ) temp 
where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID 


--------插入部分
insert into TM_Mem_AccountDetail 
(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser) 


	      select m.AccountID,m.AccountDetailType,t.DetailValue,
		         case when t.SpecialDate1=''''2000-01-01'''' then null else t.SpecialDate1 end SpecialDate1,
				 case when t.SpecialDate2=''''2000-01-01'''' then null else t.SpecialDate2 end SpecialDate2,
		        t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,''''0'''' vehicle, ''''0''''  StoreCode, ''''0''''  StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         left join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
          
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2 
    where n.AccountID is null
	 and  n.AccountDetailType  is null
	 and  n.SpecialDate1 is null   
	 and  n.SpecialDate2 is null 
     and  n.vehicle is null 
	 and  n.StoreCode is null 
	 and  n.StoreBrandCode is null 		   		 
'')




---更新插入账户限制  TM_Mem_AccountLimit

exec (''  insert into TM_Mem_AccountLimit
        (AccountID,AccountDetailID,LimitType,LimitValue,AddedDate)    
---------------行转列，去掉值为0的数据
         select AccountID,AccountDetailID, attribute LimitType,value LimitValue ,getdate()
		 from 

	     ( select m.AccountID,m.AccountDetailID,
		         cast(m.vehicle as nvarchar(20)) vehicle ,
				 cast(m.store as nvarchar(20)) store ,
				 cast(m.brand as nvarchar(20)) brand  
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,t.AccountDetailID,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) store,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) brand 
			from   ''+@ActRuleTable+''  a
		    inner join   
			            ------由于限制类型尚未添加，只能取同accountid,AccountDetailType,SpecialDate1,SpecialDate2 刚刚创建插入的那条数据记录
			            (select x.*  from 
			            (select x.*,ROW_NUMBER() over(partition by accountid,AccountDetailType,SpecialDate1,SpecialDate2  order by addeddate desc ) serial 
			            from TM_Mem_AccountDetail x  )  x
						where serial=1 ) t on 
	              a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''')
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			cross  join  ''+@TypeTable+'' d 
			group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
	     left join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) store,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) brand 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand		
    where n.AccountID is null
	 and  n.AccountDetailType  is null
	 and  n.SpecialDate1 is null   
	 and  n.SpecialDate2 is null 
     and  n.vehicle is null 
	 and  n.store is null 
	 and  n.brand is null ) t

 UNPIVOT
  (
    value FOR attribute IN (store, brand,vehicle)
  ) AS b
where value<>''''0''''
  
'')



----更新账户表中积分账户的可用积分;
---删除AccountType=2，通过AccountID判别其为积分或者积点账户

exec(''
update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value1''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')


---更新账户表中积分账户的可用积分



exec(''
update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value2''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')



-----明细记录值插入 

exec(''
if exists (select type  from  ''+@TypeTable+'')   ---非空时 

insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
RuleID,
AddedDate         ,
AddedUser          
)
select           
m.AccountID         ,
m.AccountDetailID   ,
''''loy'''' AccountChangeType ,  
(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                  when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			          when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end )   ChangeValue,  
''''忠诚度发放积分'''' ChangeReason      ,
t.TradeID ReferenceNo       ,
0 HasReverse                ,  
t.RuleID                    ,
getdate()  AddedDate        ,
''''1000''''   AddedUser          
from 

	      (  select * 
		  from (
	      (select a.AccountID,fieldname AccountDetailType,t.AccountDetailID,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,max(case when type=''''vehicle'''' then cast(e.MemberSubExtID  as nvarchar(10)) else ''''0'''' end ) vehicle1,
			           max(case when type=''''store'''' then StoreCode  else ''''0'''' end ) store1,
					   max(case when type=''''brand'''' then StoreBrandCode  else ''''0'''' end ) brand1, f.store,f.brand,f.vehicle
			from   ''+@ActRuleTable+''   a
		    inner join 
		         TM_Mem_AccountDetail	t on 
	              a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''')
			inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID
            inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode 
            inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID
			inner join   (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle
			              from (
			              select AccountID,AccountDetailID,isnull(store,''''0'''') store,isnull(brand,''''0'''') brand,isnull(vehicle,''''0'''') vehicle
		                  from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t 
						  group by  AccountID,AccountDetailID 
						  )  f 
						  on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID
			cross  join ''+@TypeTable+''  d 

			group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''') ,f.store,f.brand,f.vehicle )  ) w
			where store=store1 and brand=brand1 and vehicle=vehicle1)  m
	     inner join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) store,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) brand 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand	
		 inner join   ''+@ActRuleTable+''  t 
		 on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname    
		  and m.SpecialDate1=isnull(t.SpecialDate1,''''2000-01-01'''')  
		  and m.SpecialDate2=isnull(t.SpecialDate2,''''2000-01-01'''')



if not exists (select type  from  ''+@TypeTable+'')   ---空时 

insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
RuleID,
AddedDate         ,
AddedUser          
)
select           
m.AccountID         ,
m.AccountDetailID   ,
''''loy'''' AccountChangeType ,  
(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                  when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			          when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end )   ChangeValue,  
''''忠诚度发放积分'''' ChangeReason      ,
t.TradeID ReferenceNo       ,
0 HasReverse                ,  
t.RuleID                    ,
getdate()  AddedDate        ,
''''1000''''   AddedUser          
from 

	     (  select w.*
		  from 
	      (select a.AccountID,fieldname AccountDetailType,t.AccountDetailID,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,''''0''''   vehicle1,     ''''0''''  store1,	    ''''0''''  brand1,isnull(f.store,0) store, isnull(f.brand,0) brand,isnull(f.vehicle,0) vehicle
			from   ''+@ActRuleTable+''  a
	          inner join TM_Mem_AccountDetail t on 
			      a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''') 

			left join (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle
			              from (
			              select AccountID,AccountDetailID,isnull(store,''''0'''') store,isnull(brand,''''0'''') brand,isnull(vehicle,''''0'''') vehicle
		                  from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t 
						  group by  AccountID,AccountDetailID 
						  )  f 
						  on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID
			group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''') ,f.store,f.brand,f.vehicle  )w
			where  store=store1 and brand=brand1 and vehicle=vehicle1 )  m
	     inner join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) store,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) brand 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand	
		 inner join   ''+@ActRuleTable+''  t 
		 on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname    
		  and m.SpecialDate1=isnull(t.SpecialDate1,''''2000-01-01'''')  
		  and m.SpecialDate2=isnull(t.SpecialDate2,''''2000-01-01'''')

'')




		   	


END



















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountComputeForActRuleNoTrade]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountComputeForActRuleNoTrade]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE  PROCEDURE [dbo].[sp_Loy_AccountComputeForActRuleNoTrade]
	@ActRuleTable nvarchar(3000) ,
	@TypeTable  nvarchar(3000)  
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：根据忠诚度的计算规则插入更新相应的交易账户相关表中的字段的值；(计算规则只针对账户，不针对交易)  
  建 立 人：zyb
  建立时间：2015-06-05
  修 改 人：

  规则表：sample:TempActRule, tmpRuleAct125021237
  限制表：sample:TempLimitType 可能无品牌的限制  
  ***********************************/ 

BEGIN


---可用积分,冻结积分更新插入
exec (''


update TM_Mem_AccountDetail 
set TM_Mem_AccountDetail.DetailValue         =TM_Mem_AccountDetail.DetailValue+temp.DetailValue        ,
    TM_Mem_AccountDetail.ModifiedDate        =getdate()      

from  (
	 
	      select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue 
		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          , ''''0'''' vehicle, ''''0''''  StoreCode, ''''0''''  StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         inner join 

     ---------------历史账户明细限制值 (left 存在不作限制的账户明细ID)  
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
                   
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2  ) temp 
where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID 


--------插入部分
insert into TM_Mem_AccountDetail 
(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser) 


	      select m.AccountID,m.AccountDetailType,t.DetailValue,
		         case when t.SpecialDate1=''''2000-01-01'''' then null else t.SpecialDate1 end SpecialDate1,
				 case when t.SpecialDate2=''''2000-01-01'''' then null else t.SpecialDate2 end SpecialDate2,
		        t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser

		  from 
	 --------------本次账户相关内容及限定值
		  (
	      	select a.AccountID,fieldname AccountDetailType,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,''''0'''' vehicle, ''''0''''  StoreCode, ''''0''''  StoreBrandCode 
			from   ''+@ActRuleTable+''  a
			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''')    ) m
         left join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) StoreCode,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) StoreBrandCode 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode
		
		 inner join 
	-----------------本次需要更新的账户数据 

		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(SpecialDate2,''''2000-01-01'''')  SpecialDate2,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			           when isnull(OffsetExpression,'''''''')=''''''''  then ComputeValue end ) DetailValue,
                
                    getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
           from 	   ''+@ActRuleTable+''  a
           group by AccountID,fieldname,isnull(SpecialDate1,''''2000-01-01'''') , isnull(SpecialDate2,''''2000-01-01'''')  ) t 
		on  m.AccountID=t.AccountID 
        and m.AccountDetailType=t.AccountDetailType
		and m.SpecialDate1=t.SpecialDate1  
		and m.SpecialDate2=t.SpecialDate2 
    where n.AccountID is null
	 and  n.AccountDetailType  is null
	 and  n.SpecialDate1 is null   
	 and  n.SpecialDate2 is null 
     and  n.vehicle is null 
	 and  n.StoreCode is null 
	 and  n.StoreBrandCode is null 		   		 
'')


----不存在账户限制值


----更新账户表中积分账户的可用积分;
---删除AccountType=2，通过AccountID判别其为积分或者积点账户

exec(''
update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value1''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')


---更新账户表中积分账户的可用积分

exec(''
update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate() 
from ( 	 

     select AccountID,
              sum(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                   when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			            when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end ) ChangeValue,
              getdate() AddedDate,''''1000'''' AddedUser,getdate() ModifiedDate,''''1000'''' ModifiedUser
      from  ''+@ActRuleTable+''  a
      where fieldname=''''value2''''
	  group by AccountID ) temp 
where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  
'')



-----明细记录值插入 

exec(''
insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
RuleID,
AddedDate         ,
AddedUser          
)
select           
m.AccountID         ,
m.AccountDetailID   ,
''''loy'''' AccountChangeType ,  
(case when isnull(OffsetExpression,'''''''')=''''*'''' then  ComputeValue* (OffsetValue) 
	                  when isnull(OffsetExpression,'''''''')=''''+'''' then  ComputeValue+ (OffsetValue)
			          when isnull(OffsetExpression,'''''''')='''''''' then ComputeValue end )   ChangeValue,  
''''忠诚度发放积分'''' ChangeReason      ,
t.TradeID ReferenceNo       ,
0 HasReverse                ,  
t.RuleID                    ,
getdate()  AddedDate        ,
''''1000''''   AddedUser          
from 

	     (  select w.*
		  from 
	      (select a.AccountID,fieldname AccountDetailType,t.AccountDetailID,
			       isnull(a.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(a.SpecialDate2,''''2000-01-01'''')  SpecialDate2
			          ,''''0''''   vehicle1,     ''''0''''  store1,	    ''''0''''  brand1,isnull(f.store,0) store, isnull(f.brand,0) brand,isnull(f.vehicle,0) vehicle
			from   ''+@ActRuleTable+''  a
	          inner join TM_Mem_AccountDetail t on 
			      a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType
              and isnull(a.SpecialDate1,''''2000-01-01'''')=isnull(t.SpecialDate1,''''2000-01-01'''')  
              and isnull(a.SpecialDate2,''''2000-01-01'''')=isnull(t.SpecialDate2,''''2000-01-01'''') 

			left join (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle
			              from (
			              select AccountID,AccountDetailID,isnull(store,''''0'''') store,isnull(brand,''''0'''') brand,isnull(vehicle,''''0'''') vehicle
		                  from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t 
						  group by  AccountID,AccountDetailID 
						  )  f 
						  on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID
			group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''''2000-01-01''''), isnull(a.SpecialDate2,''''2000-01-01'''') ,f.store,f.brand,f.vehicle  )w
			where  store=store1 and brand=brand1 and vehicle=vehicle1 )  m
	     inner join 

     ---------------历史账户明细限制值   
	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,
		          isnull(b.SpecialDate1,''''2000-01-01'''') SpecialDate1, isnull(b.SpecialDate2,''''2000-01-01'''')  SpecialDate2,
		               max(case when Limittype=''''vehicle'''' then LimitValue else ''''0'''' end ) vehicle,
			           max(case when Limittype=''''store''''   then LimitValue else ''''0'''' end ) store,
					   max(case when Limittype=''''brand''''   then LimitValue else ''''0'''' end ) brand 
		   from ''+@ActRuleTable+'' a
		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType
		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID
		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''''2000-01-01''''), isnull(b.SpecialDate2,''''2000-01-01'''') ) n

	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  
		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand	
		 inner join   ''+@ActRuleTable+''  t 
		 on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname    
		  and m.SpecialDate1=isnull(t.SpecialDate1,''''2000-01-01'''')  
		  and m.SpecialDate2=isnull(t.SpecialDate2,''''2000-01-01'''')
'')


END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_AccountValueUpdate]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_AccountValueUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE  PROCEDURE [dbo].[sp_Loy_AccountValueUpdate]
	@OutTable  nvarchar(1000)  -----输出结果表名
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：根据返回忠诚度的交易的计算明细值更新相应的交易账户表的值；
  建 立 人：zyb
  建立时间：2015-04-09
  修 改 人：20150507 zyb AccountChangeType,ChangeReason默认值赋值
            20150729 zyb   增加约束 and b.AccountChangeType=''loy''
  ***********************************/ 


BEGIN

------将能够关联到的tradeid的log数据插入抵冲数据
exec(''
insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
AddedDate         ,
AddedUser          
)
select           
AccountID         ,
AccountDetailID   ,
''''loy''''AccountChangeType ,  
-1*ChangeValue       ,
''''反算对冲'''' ChangeReason      ,
ReferenceNo       ,
1 HasReverse        ,
getdate() AddedDate         ,
''''1000'''' AddedUser          
from ''+@OutTable+'' a
inner join TL_Mem_AccountChange b with (nolock)  on a.tradeid=b.ReferenceNo 
where HasReverse=0  and b.AccountChangeType=''''loy''''  '')


-----更新TM_Mem_AccountDetail中需要更新的数值根据AccountID，AccountDetailID
exec(''
update TM_Mem_AccountDetail
set DetailValue =DetailValue+temp.ChangeValue,ModifiedDate=getdate(),ModifiedUser=''''1000'''' 
from 
(select b.AccountID,b.AccountDetailID,sum(-1*ChangeValue) ChangeValue
 from  ''+@OutTable+'' a
inner join TL_Mem_AccountChange b with (nolock)  on a.tradeid=b.ReferenceNo 
where HasReverse=0 
group by b.AccountID,b.AccountDetailID) temp 
where TM_Mem_AccountDetail.AccountID =temp.AccountID and TM_Mem_AccountDetail.AccountDetailID =temp.AccountDetailID '')



---更新TM_Mem_Account中Value1，Value2，Value3中数值按照AccountID
exec(''
update TM_Mem_Account
set TM_Mem_Account.Value1 =TM_Mem_Account.Value1+temp.value1, 
    TM_Mem_Account.Value2 =TM_Mem_Account.Value2+temp.value2,
	TM_Mem_Account.Value3 =TM_Mem_Account.Value3+temp.value3,
	TM_Mem_Account.ModifiedDate=getdate() 
from 
(
select AccountID,isnull(value1,0)  value1,isnull(value2,0)  value2,isnull(value3,0)  value3
from 
(
select  m.AccountID,n.AccountDetailType,m.ChangeValue
from 
(select b.AccountID,b.AccountDetailID,sum(-1*ChangeValue) ChangeValue
 from  ''+@OutTable+'' a
inner join TL_Mem_AccountChange b with (nolock)  on a.tradeid=b.ReferenceNo 
where HasReverse=0 
group by b.AccountID,b.AccountDetailID) m
inner join  TM_Mem_AccountDetail  n  with (nolock)  on m.AccountID=n.AccountID and m.AccountDetailID=n.AccountDetailID 
group by m.AccountID,n.AccountDetailType,m.ChangeValue) t 
 pivot (max(ChangeValue) for AccountDetailType in ([value1],[value2],[value3])) w   ) temp
 where TM_Mem_Account.AccountID=temp.AccountID '') 



 -----更新能够关联到的tradeid的log数据HasReverse 
exec(''
update TL_Mem_AccountChange set HasReverse=1 ,ChangeReason=''''反算对冲''''  
from ''+@OutTable+'' a
inner join TL_Mem_AccountChange b with (nolock)  on a.tradeid=b.ReferenceNo 
where HasReverse=0 and b.AccountChangeType=''''loy'''''')



END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_DisabledAccountUpdate]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_DisabledAccountUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

create  PROCEDURE [dbo].[sp_Loy_DisabledAccountUpdate]
AS


 /**********************************
  ----arvarto system-----
  存储过程功能描述：根据积分积点的失效日期SpecialDate2，每天定时失效积分积点，更新账户相关表；
  建 立 人：zyb
  建立时间：2015-05-13
  修 改 人：

  ***********************************/ 





BEGIN
----根据当天需要失效的积分积点账户的失效数

update TM_Mem_Account
set Value1=Value1-ChangeValue,
    Value3=Value3+ChangeValue
from (select AccountID,sum(DetailValue) ChangeValue 
      from TM_Mem_AccountDetail  
      where AccountDetailType=''value1'' and  convert(nvarchar(8),SpecialDate2,112)=convert(nvarchar(8),getdate(),112)
      group by AccountID) temp  
where TM_Mem_Account.AccountID=temp.AccountID 






---失效积分增加日志记录；


insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
AddedDate         ,
AddedUser          
)
select           
a.AccountID         ,
a.AccountDetailID   ,
''loy''AccountChangeType ,  
-a.DetailValue   ChangeValue       ,
''账户权益失效'' ChangeReason      ,
null  ReferenceNo       ,
0 HasReverse        ,
getdate() AddedDate         ,
''1000'' AddedUser          
from TM_Mem_AccountDetail a with (nolock)
where   a.AccountDetailType=''value1'' 
 and  convert(nvarchar(8),a.SpecialDate2,112)=convert(nvarchar(8),getdate(),112)


---将账户明细中即将失效的可用的积分更改为0

update TM_Mem_AccountDetail  
set DetailValue=0,ModifiedDate=getdate() 
where AccountDetailType=''value1'' 
 and  convert(nvarchar(8),SpecialDate2,112)=convert(nvarchar(8),getdate(),112)



END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_FreezeToThawAccountUpdate]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_FreezeToThawAccountUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE  PROCEDURE [dbo].[sp_Loy_FreezeToThawAccountUpdate]
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：根据冻结积分积点的解冻日期SpecialDate1，每天定时解冻积分，其类型更改为value1,更新账户相关表；
  建 立 人：zyb
  建立时间：2015-05-07
  修 改 人：20150605 zyb  业务情况解冻之后再红冲，解冻反算对冲后，解冻值中不含tradeid，造成红冲时无法关联交易单号红冲；
            20150729 zyb   增加约束 and b.AccountChangeType=''loy''
  ***********************************/ 


BEGIN
----根据当天需要解冻的积分积点账户的解冻数据扣减冻结额度数，增加可用额度数

update TM_Mem_Account
set Value1=Value1+ChangeValue,
    Value2=Value2-ChangeValue
from (select AccountID,sum(DetailValue) ChangeValue 
      from TM_Mem_AccountDetail  
      where AccountDetailType=''value2'' and  convert(nvarchar(8),SpecialDate1,112)=convert(nvarchar(8),getdate(),112)
      group by AccountID) temp 
where TM_Mem_Account.AccountID=temp.AccountID


---更新反算账户变动日志表TL_Mem_AccountChange中数据；

insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
AddedDate         ,
AddedUser          
)
select           
b.AccountID         ,
b.AccountDetailID   ,
''loy''AccountChangeType ,  
-1*ChangeValue       ,
''反算对冲'' ChangeReason      ,
b.ReferenceNo       ,
1 HasReverse        ,
getdate() AddedDate         ,
''1000'' AddedUser          
from TM_Mem_AccountDetail a with (nolock)
inner join TL_Mem_AccountChange b with (nolock)  on a.AccountID=b.AccountID  and a.AccountDetailID=b.AccountDetailID
where b.HasReverse=0   and b.AccountChangeType=''loy''
 and  a.AccountDetailType=''value2'' 
 and  convert(nvarchar(8),a.SpecialDate1,112)=convert(nvarchar(8),getdate(),112)






update TL_Mem_AccountChange set HasReverse=1  , ChangeReason=''忠诚度发放积分需反算对冲''
from  TM_Mem_AccountDetail b 
where HasReverse=0 
 and  TL_Mem_AccountChange.AccountID=b.AccountID  
 and  TL_Mem_AccountChange.AccountDetailID=b.AccountDetailID
 and b.AccountDetailType=''value2''  and TL_Mem_AccountChange.AccountChangeType=''loy''
 and  convert(nvarchar(8),b.SpecialDate1,112)=convert(nvarchar(8),getdate(),112)






---反算对应完毕之后，新增可用账户积分积点账户的新增日志记录；
----20150605 修改解冻不取汇总值 ，取非反算对冲的明细值 ；

insert into TL_Mem_AccountChange
(
AccountID         ,
AccountDetailID   ,
AccountChangeType ,
ChangeValue       ,
ChangeReason      ,
ReferenceNo       ,
HasReverse        ,
AddedDate         ,
AddedUser          
)
select           
a.AccountID         ,
a.AccountDetailID   ,
''loy''AccountChangeType ,  
b.ChangeValue   ChangeValue       ,
''账户解冻'' ChangeReason      ,
b.ReferenceNo       ,
0 HasReverse        ,
getdate() AddedDate         ,
''1000'' AddedUser          
from TM_Mem_AccountDetail a with (nolock) 
inner join TL_Mem_AccountChange b with (nolock)  on a.AccountID=b.AccountID  and a.AccountDetailID=b.AccountDetailID 
where   a.AccountDetailType=''value2'' 
 and  convert(nvarchar(8),a.SpecialDate1,112)=convert(nvarchar(8),getdate(),112)
 and  b.ChangeReason<>''反算对冲''  



---将账户明细中的账户类型冻结更改为可用


update TM_Mem_AccountDetail 
set AccountDetailType=''value1'',ModifiedDate=getdate() 
where AccountDetailType=''value2'' 
 and  convert(nvarchar(8),SpecialDate1,112)=convert(nvarchar(8),getdate(),112)


END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnFirstSplitDetail]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnFirstSplitDetail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Loy_ReturnFirstSplitDetail]
	@TempNodeSplit  nvarchar(3000),  ----需拆分的明细表 ，表样式如： TempNodeSplit
	@TETable1  nvarchar(1000),  -----输入临时表1
	@TETable2  nvarchar(1000),  -----输入临时表2
	@TETable3  nvarchar(1000),   -----输入临时表3
    @OutSplitTable  nvarchar(1000)   -----输出结果表名
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：返回第一次拆分的结果明细表 
  建 立 人：zyb
  建立时间：2015-04-21
  修 改 人：
			 
  ***********************************/ 


BEGIN

---根据需要拆分的明细表生成其按会员分类统计的累积明细值以及坎点和忠诚度累计值（不含本次统计的）得差值；
exec(''select a.* ,
       ROW_NUMBER() OVER (partition by a.Memberid ORDER BY a.addeddate asc) serial ,
	    SUM(ComputeValue) OVER(
	   partition by a.Memberid 
	   ORDER BY a.addeddate 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) accvalue ,
       a.NodeValue-a.LoyAccBeforeValue DiffValue 
into ''+@TETable1+''
from ''+@TempNodeSplit+''   a '' )


  
----需要拆分的会员对应的明细值信息
exec(''
select *  
into ''+@TETable2+''  
from (
---无需拆分数值
select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue, ComputeValue SplitValue ,0 sort
from ''+@TETable1+''  a
where accvalue<=DiffValue     
union all
---拆分坎数下一等级的数额
select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,accvalue-DiffValue   SplitValue ,2 sort
from ''+@TETable1+''  a
where accvalue>DiffValue  
union all 
--拆分坎数本等级的数额
select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,ComputeValue-(accvalue-DiffValue)   SplitValue ,1 sort
from ''+@TETable1+''  a
where accvalue>DiffValue ) t 
 '' )



----拆分的会员明细的整合
EXEC(''
select * 
into ''+@TETable3+''   
from (
select a.* 
from  ''+@TETable2+''    a
left join  (
select  Memberid,TradeID  
from ''+@TETable2+''   a
where SplitValue<0 
group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID
where b.Memberid is null and b.TradeID is null  
union all
select  a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue,sum(SplitValue) SplitValue,0 sort
from  ''+@TETable2+''    a
inner join  (
select  Memberid,TradeID  
from ''+@TETable2+''   a
where SplitValue<0 
group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID
group by a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue ) t 
order by memberid ,tradeid ,sort '')


---最终返回结果值 （拆分值和根据会员，拆分值的累计）
exec (''select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue,
	    a.LoyAccBeforeValue+SUM(SplitValue) OVER(
	   partition by a.Memberid 
	   ORDER BY a.serial  ,sort 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AccSplitValue 
into    ''+@OutSplitTable+''   
from  ''+@TETable3+''  a  '')  




END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnSecondSplitDetail]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnSecondSplitDetail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Loy_ReturnSecondSplitDetail]
	@TempNodeSplit2  nvarchar(3000),  ----需拆分的明细表 ，表样式如： TempNodeSplit
	@TETable1  nvarchar(1000),  -----输入临时表1
    @OutSplitTable2  nvarchar(1000)   -----输出结果表名
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：返回第二次拆分的结果明细表 
  建 立 人：zyb
  建立时间：2015-04-21
  修 改 人：
			 
  ***********************************/ 


BEGIN

---根据需要拆分的明细表生成其按会员分类统计的累积明细值以及坎点和忠诚度累计值（不含本次统计的）得差值；
exec(''
 select * into ''+@TETable1+'' 
 from (
 ---二次需拆分的会员无需拆分数据
  select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue,
	    SplitValue SplitValue1 
 from  ''+@TempNodeSplit2+''   a 
 inner join 
  (select a.Memberid
 from  ''+@TempNodeSplit2+''  a 
 where  LoyAccBeforeValue>NodeValue
 group by  Memberid )   b  on a.Memberid=b.Memberid
 where LoyAccBeforeValue<=NodeValue
 union all
  -----拆分下一等级
 select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue,
	    a.LoyAccBeforeValue-a.NodeValue  SplitValue1 
 from   ''+@TempNodeSplit2+''  a 
 where  LoyAccBeforeValue>NodeValue   
 union all
  ----拆分本等级
 select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue,
	    a.SplitValue-(a.LoyAccBeforeValue-a.NodeValue)  SplitValue1 
 from   ''+@TempNodeSplit2+''  a 
 where  LoyAccBeforeValue>NodeValue  ) t 
 order by memberid,TradeID'' )   


 exec('' delete from ''+@TETable1+'' 
        where memberid in (select memberid from  ''+@TETable1+''  where SplitValue1=0 )'' )

---最终返回结果值 （拆分值和根据会员，拆分值的累计）
exec ('' select * into ''+@OutSplitTable2+''  
 from (
 select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,SplitValue 
 from ''+@TempNodeSplit2+''    a
 left join (select memberid  from ''+@TETable1+''  group by memberid ) b on a.Memberid=b.Memberid
 where b.Memberid is null
 union all
 select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue1  SplitValue
 from  ''+@TETable1+''  a ) t 
 order by Memberid,TradeID  '')  




END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnSplitDetail]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnSplitDetail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Loy_ReturnSplitDetail]
	@TempNodeSplit  nvarchar(3000),  ----需拆分的明细表 ，表样式如： TempNodeSplit
	@TETable1  nvarchar(1000),       -----输入临时表1
	@TETable2  nvarchar(1000),       -----输入临时表2
	@TETable3  nvarchar(1000),       -----输入临时表3
    @OutSplitTable  nvarchar(1000)   -----输出结果表名
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：返回第一次拆分的结果明细表 
  建 立 人：zyb
  建立时间：2015-04-21
  修 改 人：
  20150423 zyb 一次二次拆分兼容，删除拆分结果值为0的；
			 
  ***********************************/ 


BEGIN

---对忠诚度的累计值进行计算（所给的累积是含本次要计算的，将本次要计算的减去）

exec(''update ''+@TempNodeSplit+'' set LoyAccBeforeValue=LoyAccBeforeValue-temp.accamt
from  (select Memberid,sum(ComputeValue) accamt
       from  ''+@TempNodeSplit+''
	   group by Memberid ) temp 
where ''+@TempNodeSplit+''.Memberid=temp.Memberid'')



---根据需要拆分的明细表生成其按会员分类统计的累积明细值以及坎点和忠诚度累计值（不含本次统计的）得差值；
exec(''select a.* ,
       ROW_NUMBER() OVER (partition by a.Memberid ORDER BY a.addeddate asc) serial ,
	    SUM(ComputeValue) OVER(
	   partition by a.Memberid 
	   ORDER BY a.addeddate 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) accvalue ,
       a.NodeValue-a.LoyAccBeforeValue DiffValue 
into ''+@TETable1+''
from ''+@TempNodeSplit+''   a '' )


--拆分规则:(accvalue>DiffValue)
--1.按照序号的累计值-坎数的差额=拆分下一等级的积分数
--2.按照序号的对应值-拆分下一等级的积分数=拆分本等级的积分数
  
----需要拆分的会员对应的明细值信息
exec(''
select *  
into ''+@TETable2+''  
from (
---无需拆分数值,按序号的累计值小于等于忠诚度累计值和坎数的差值
select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue, ComputeValue SplitValue ,0 sort
from ''+@TETable1+''  a
where accvalue<=DiffValue     
union all
---拆分坎数下一等级的数额 ,按序号的累计值大于忠诚度累计值和坎数的差值
select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,accvalue-DiffValue   SplitValue ,2 sort
from ''+@TETable1+''  a
where accvalue>DiffValue  
union all 
--拆分坎数本等级的数额,按序号的累计值大于忠诚度累计值和坎数的差值
select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,ComputeValue-(accvalue-DiffValue)   SplitValue ,1 sort
from ''+@TETable1+''  a
where accvalue>DiffValue ) t 
 '' )

---删除拆分值中为0的数据明细
exec(''delete from ''+@TETable2+'' where  SplitValue=0 '')

----拆分的会员明细的整合
EXEC(''
select * 
into ''+@TETable3+''   
from (
---针对拆分值非负数的会员,订单数据处理
select a.* 
from  ''+@TETable2+''    a
left join  (
----拆分值小于0
select  Memberid,TradeID  
from ''+@TETable2+''   a
where SplitValue<0 
group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID
where b.Memberid is null and b.TradeID is null  
union all
---针对拆分值负数的会员,订单数据处理
select  a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue,sum(SplitValue) SplitValue,0 sort
from  ''+@TETable2+''    a
inner join  (
select  Memberid,TradeID  
from ''+@TETable2+''   a
where SplitValue<0 
group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID
group by a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue ) t 
order by memberid ,tradeid ,sort '')


---最终返回结果值 （拆分值和根据会员，拆分值的累计）
exec (''select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue,
	    a.LoyAccBeforeValue+SUM(SplitValue) OVER(
	   partition by a.Memberid 
	   ORDER BY a.serial  ,sort 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AccSplitValue 
into    ''+@OutSplitTable+''   
from  ''+@TETable3+''  a  '')  




END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_ReturnTradeInfo]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_ReturnTradeInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Loy_ReturnTradeInfo]
	@TradeSQL  nvarchar(3000),  ----tradedetailid的输出值集 ，集形式如：(select tradedetailid from tm_mem_tradedetail) 
	@InTable   nvarchar(1000),  -----aliasid等信息的值表名
	@OutTable  nvarchar(1000),  -----输出结果表名
	@TETable1  nvarchar(1000),  -----输入临时表1
	@TETable2  nvarchar(1000),  -----输入临时表2
	@TETable3  nvarchar(1000)   -----输入临时表3
AS
 /**********************************
  ----arvarto system-----
  存储过程功能描述：返回忠诚度的交易的计算明细值
  建 立 人：zyb
  建立时间：2015-04-02
  修 改 人：20150402 zyb @TradeSQL作为tradedetailid的输出值表
            20150403 zyb 增加临时表输入和结果值输出表名参数 ；支持并发执行操作；临时表要动态生成 ；
			             增加参数@TETable1，@TETable2，@TETable3 ，保证其唯一性；			           
            20150408 zyb 采用支持参数变量值输出的sp_executesql
			20150413 zyb 计算指标包容TM_Mem_Trade中的别名字段 
			20150416 zyb AliasID改为 FieldAlias；MAX MIN的修改约束最终的计算结果值低得上下限
			 
  ***********************************/ 


BEGIN



            exec(''IF OBJECT_ID(N''''SAISC_4S_CRM.dbo.''+@TETable1+'''''') IS NOT NULL

            BEGIN
                DROP TABLE ''+@TETable1+''
            END'' )

           exec(''IF OBJECT_ID(N''''SAISC_4S_CRM.dbo.''+@TETable2+'''''') IS NOT NULL

            BEGIN
                DROP TABLE ''+@TETable2+''
            END'' )

		   exec(''IF OBJECT_ID(N''''SAISC_4S_CRM.dbo.''+@TETable3+'''''') IS NOT NULL

            BEGIN
                DROP TABLE ''+@TETable3+''
            END'' )

            exec(''IF OBJECT_ID(N''''SAISC_4S_CRM.dbo.''+@OutTable+'''''') IS NOT NULL

            BEGIN
                DROP TABLE ''+@OutTable+''
            END'' )

----生成用于存放SQL脚本的临时表
           exec(''select cast('''''''' as nvarchar(max)) AS SQL into ''+@TETable1+'''')
		   
		 ---  update TE_SYS_TableSQL set sql= null   

	declare
			@Sqlx nvarchar(max) ,
			@Sqly nvarchar(max) ,
			
			@Sqlz nvarchar(max) ,
			@Sqlw nvarchar(max) ,

			@Sql  nvarchar(max) ,
			@ReturnSql  nvarchar(max) ,
			@ReturnValue  nvarchar(max)


exec(''
DECLARE TradeCursor CURSOR 
FOR 
select a.FieldAlias,a.groupfunc,a.offsetexpression,a.offsetvalue,a.namecode,a.isabs,
  case when b.TableName=''''TM_Mem_TradeDetail'''' and b.AliasKey is not null and  b.AliasSubKey is not  null 
       then  ''''V_M_''''+B.TableName+''''_''''+b.AliasKey+''''_''''+b.AliasSubKey 
	   when b.TableName=''''TM_Mem_Trade'''' and b.AliasKey is not null and  b.AliasSubKey is   null 
       then  ''''V_M_''''+B.TableName+''''_''''+b.AliasKey 
  end V_TableName 
from  ''+@InTable+''  a   
inner join TD_SYS_FieldAlias b on a.FieldAlias=b.FieldAlias  
 '' )


--打开一个游标	
OPEN TradeCursor
--循环一个游标
DECLARE @FieldAlias nvarchar(200) ,
  --      @maximum 	decimal(18,2) ,
		--@minimum 	decimal(18,2), 
		@groupfunc nvarchar(100) ,
        @offsetexpression 	nvarchar(100) ,
		@offsetvalue 	nvarchar(100),
		@namecode nvarchar(100) ,
		@isabs  nvarchar(2) ,
        @V_TableName 	nvarchar(100) 
FETCH NEXT FROM  TradeCursor INTO @FieldAlias,@groupfunc,@offsetexpression,@offsetvalue,@namecode,@isabs,@V_TableName
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql1 nvarchar(max) , 
        @sql_search1 nvarchar(max)=''where 1=1 '' ,
		@sql_search2a nvarchar(max)='''' ,
		@sql_search2b nvarchar(max)='''' ,
		@sql_search3a nvarchar(max)='''' ,
		@sql_search3b nvarchar(max)='''' ,
		@sql_search4 nvarchar(max)='''' ,
		@sql_search5 nvarchar(max)='''' 


    if (@isabs =''1'')
	begin
		set @sql_search2a = @sql_search2a  + ''abs(''   
		set @sql_search2b = @sql_search2b  + '')'' 
	end
 
    if (@groupfunc is not null )
	begin
		set @sql_search3a = @sql_search3a  + @groupfunc+''(''   
		set @sql_search3b = @sql_search3b  + '')'' 
	end 

    if (@offsetexpression is not null )
	begin
		set @sql_search4 = @sql_search4+@offsetexpression   
		
	end 

    if (@offsetvalue is not null )
	begin
		set @sql_search5 = @sql_search5+@offsetvalue   
		
	end 	
	   
---20150416 修改
	--if (@maximum is not null  )
	--begin
	--	set @sql_search1 = @sql_search1  + '' and a.''+@FieldAlias+ ''   <=  ''+convert(nvarchar(20),@maximum) +''''    
	--end

	--if (@minimum is not null  )
	--begin
	--	set @sql_search1 = @sql_search1  + '' and a.''+@FieldAlias+ ''   >=  ''+convert(nvarchar(20),@minimum) +''''   
	--end



set @sql1='' 
union all
select a.tradeid ,''''''''''+@namecode+''''''''''  as type,
''+@sql_search2a+''''+@sql_search3a+@FieldAlias+@sql_search3b+@sql_search4+''''+@sql_search5+''''+@sql_search2b+''  as amt      
from ''+@V_TableName+''  a
inner join ''+@TradeSQL+'' b  on a.tradedetailid=b.tradedetailid 
''+@sql_search1+''
group by a.tradeid 
union all
select 0 tradeid,''''''''''+@namecode+'''''''''' as type, 0 amt 
''

exec(''update ''+@TETable1+'' set sql= isnull(sql,'''''''')''+''+  ''''''+@sql1+'''''''')

print(''update ''+@TETable1+'' set sql= isnull(sql,'''''''')''+''+ ''''''+@sql1+'''''''')

FETCH NEXT FROM  TradeCursor INTO  @FieldAlias,@groupfunc,@offsetexpression,@offsetvalue,@namecode,@isabs,@V_TableName

END	
--关闭游标
CLOSE TradeCursor
--释放资源
DEALLOCATE TradeCursor




----处理SQL 将第一个union all 去掉；
--exec(''update ''+@TETable1+'' set sql=  stuff(sql,1,12,'''''''')'')
---print (''update ''+@TETable1+'' set sql=  stuff(sql,1,12,'''''''')'')


print(''update ''+@TETable1+'' set sql= ''''select * into ''+@TETable2+'' from (''''+ stuff(sql,1,12,'''''''') +'''') t'''' '')
exec (''update ''+@TETable1+'' set sql= ''''select * into ''+@TETable2+'' from (''''+ stuff(sql,1,12,'''''''') +'''') t'''' '')

----将表中的字段值作为变量值输出并执行；

	set @sqlx = ''select @ct=sql from ''+@TETable1 
	
	exec sp_executesql @sqlx,N''@ct nvarchar(max) output'',@ReturnSql output 
	print @ReturnSql
	exec(@ReturnSql)




----将行列转换的列名作为变量值输出


set @sqly='' select @ct1 = isnull(@ct1 + ''''],['''' ,'''''''') + type  from ''+@TETable2+''  group by type '' 

exec sp_executesql @sqly,N''@ct1 nvarchar(max) output'',@ReturnValue output 


set @ReturnValue = ''['' + @ReturnValue + '']''  
print @ReturnValue  


---20150416 修改

EXEC(''update ''+@TETable2+'' 
set amt =
case when amt is not null and temp.maximum is null  and temp.minimum  is null  then amt 
     when amt is not null and temp.maximum is not null  and temp.minimum  is not  null  and  amt>temp.maximum  then temp.maximum
	 when amt is not null and temp.maximum is not null  and temp.minimum  is not  null  and  amt<temp.minimum  then temp.minimum
	 when amt is not null and temp.maximum is not null  and temp.minimum  is not  null  and  amt<=temp.maximum  and   amt>=temp.minimum  then amt
	 when amt is not null and temp.maximum is not null  and temp.minimum  is null       and  amt>temp.maximum  then temp.maximum 
	 when amt is not null and temp.maximum is not null  and temp.minimum  is null       and  amt<=temp.maximum then amt  
	 when amt is not null and temp.maximum is     null  and temp.minimum  is not null   and  amt>temp.minimum  then amt  
	 when amt is not null and temp.maximum is     null  and temp.minimum  is not null   and  amt<temp.minimum  then temp.minimum     
     when amt is null then  amt  end 
from ''+@InTable+'' temp
where TYPE=temp.NameCode   '')



----生成行列转换后的表
exec (''select * into ''+@TETable3+''
       from (select * from ''+@TETable2+'') a pivot (max(amt) for type in ('' + @ReturnValue + '')) b'')  




---生成最终输出值得结果
set @sql='' select b.memberid,a.* 
	   into ''+@OutTable+''
	   from ''+@TETable3+''   a
	   inner join (select c.TradeID,c.memberid 
	               from  ''+@TradeSQL+''  a
				   inner join TM_Mem_TradeDetail b   on a.tradedetailid=b.tradedetailid
				   inner join TM_Mem_Trade c on b.tradeid=c.tradeid
				   group by c.TradeID,c.memberid  ) b on a.tradeid=b.TradeID 
''

exec(@sql)
PRINT(@SQL)




END


















' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadCredit]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadCredit]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[sp_Loy_WillDeadCredit]
@MemberList nvarchar(max),
@DatetimeNow nvarchar(8),
@Switch nvarchar(2)

as 


  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Loy_WillDeadCredit 返回会员1个月后的当天将过期的可用积分
  建 立 人：zyb
  建立时间：2015-7-29
  修 改 人：
  修改时间：
  修改内容:
  ***********************************/
begin


--清空临时表
truncate table TE_Mem_WillDeadPoint
truncate table TE_Mem_WillDeadDate
truncate table TE_Mem_Account1
truncate table TE_Mem_Account2
truncate table TE_Mem_Account3
truncate table TE_Mem_Account4 


  


--取正值数据排序累计
declare @Sql nvarchar(max)
set @Sql=''insert into TE_Mem_Account1
          select a.MemberID,a.AccountID,b.ChangeValue,c.SpecialDate2 EndDate,b.AddedDate
                 ,ROW_NUMBER() OVER (partition by b.AccountID ORDER BY b.addeddate asc) serial
          from TM_Mem_Account a
          inner join TL_Mem_AccountChange b on a.AccountID=b.AccountID
		  inner join TM_Mem_AccountDetail c on b.AccountID=b.AccountID and b.AccountDetailID=c.AccountDetailID
          where    a.AccountType=''''3'''' 
              and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)
	          and b.ChangeValue>0
			  and b.HasReverse=0
			  and c.AccountDetailType=''''value1''''

			  ''


--select *　from TL_Mem_AccountChange 
--select *,a.SpecialDate1,a.SpecialDate2,a.AccountDetailType　from TM_Mem_AccountDetail  a
--select * from TD_SYS_BizOption where OptionType=''AccountDetailType''

--value1	可用
--value2	冻结


exec (@Sql)
print @Sql

insert into TE_Mem_Account2
select a.serial,a.MemberID,a.AccountID,a.ChangeValue, 
       SUM(ChangeValue) OVER(
	   partition by a.AccountID 
	   ORDER BY AccountID, SERIAL 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)  accchangevalue,
       a.EndDate,a.AddedDate   
from TE_Mem_Account1 a
order by a.AccountID,serial 
 

----已用积分负值累加
set @Sql=''insert into TE_Mem_Account3
          select a.MemberID,a.AccountID,-sum(isnull(b.ChangeValue,0)) ChangeValue
          from TM_Mem_Account a
          left join (select * 
		             from TL_Mem_AccountChange 
					 where ChangeValue<0 
					   and HasReverse=0	)b on a.AccountID=b.AccountID
          where    a.AccountType=''''3''''
               and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	   	  
          group by a.MemberID,a.AccountID'' 

exec (@Sql)


----正负差值比较，找到serial这个节点

insert into TE_Mem_Account4
select m.*, n.ChangeValue ChangeValue1 ,m.accchangevalue-n.ChangeValue diffamt
from  TE_Mem_Account2  m
inner join     ( select a.MemberID,a.AccountID,min(isnull(serial,0)) serial,min(isnull(b.ChangeValue,0)) ChangeValue
                 from  TE_Mem_Account2 a
				 inner join  TE_Mem_Account3 b on a.MemberID=b.MemberID
				 where  a.accchangevalue>=b.ChangeValue 
				 group by a.MemberID,a.AccountID) n  on m.MemberID=n.MemberID and  m.serial=n.serial





----以节点为起点，查找其后在三个月内即将过期的积分
set @Sql=''insert into TE_Mem_WillDeadPoint
          select t.MemberID,sum(isnull(m.changevalue,0)) WillDeadPoint
          from TM_Mem_Master t
          left join 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where a.EndDate>=''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and dateadd(day ,-30,a.EndDate) =''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and a.serial>=b.serial
                    ) m on t.MemberID =m.MemberID
         where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	 
         group by t.MemberID''  

exec (@Sql)

set @Sql=''insert into TE_Mem_WillDeadDate
          select memberid ,dateadd(day ,30,''''''+CAST(@DatetimeNow AS NVARCHAR(20))+'''''')   
		  from TE_Mem_WillDeadPoint 
		  where willdeadpoint>0 ''
          

exec (@Sql)



end 










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadCredit3]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadCredit3]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[sp_Loy_WillDeadCredit3]
@MemberList nvarchar(max),
@DatetimeNow nvarchar(8),
@Switch nvarchar(2)

as 



  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Loy_WillDeadCredit3 返回会员3个月内容即将过期的可用积分
  建 立 人：zyb
  建立时间：2015-4-30
  修 改 人：
  修改时间：
  修改内容:
  ***********************************/
begin


--清空临时表
truncate table TE_Mem_WillDeadPoint
truncate table TE_Mem_WillDeadDate
truncate table TE_Mem_Account1
truncate table TE_Mem_Account2
truncate table TE_Mem_Account3
truncate table TE_Mem_Account4 


  


--取正值数据排序累计
declare @Sql nvarchar(max)
set @Sql=''insert into TE_Mem_Account1
          select a.MemberID,a.AccountID,b.ChangeValue,c.SpecialDate2 EndDate,b.AddedDate
                 ,ROW_NUMBER() OVER (partition by b.AccountID ORDER BY b.addeddate asc) serial
          from TM_Mem_Account a
          inner join TL_Mem_AccountChange b on a.AccountID=b.AccountID
		  inner join TM_Mem_AccountDetail c on b.AccountID=b.AccountID and b.AccountDetailID=c.AccountDetailID
          where    a.AccountType=''''3'''' 
              and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)
	          and b.ChangeValue>0
			  and b.HasReverse=0
			  and c.AccountDetailType=''''value1''''

			  ''


--select *　from TL_Mem_AccountChange 
--select *,a.SpecialDate1,a.SpecialDate2,a.AccountDetailType　from TM_Mem_AccountDetail  a
--select * from TD_SYS_BizOption where OptionType=''AccountDetailType''

--value1	可用
--value2	冻结


exec (@Sql)
print @Sql

insert into TE_Mem_Account2
select a.serial,a.MemberID,a.AccountID,a.ChangeValue, 
       SUM(ChangeValue) OVER(
	   partition by a.AccountID 
	   ORDER BY AccountID, SERIAL 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)  accchangevalue,
       a.EndDate,a.AddedDate   
from TE_Mem_Account1 a
order by a.AccountID,serial 
 

----已用积分负值累加
set @Sql=''insert into TE_Mem_Account3
          select a.MemberID,a.AccountID,-sum(isnull(b.ChangeValue,0)) ChangeValue
          from TM_Mem_Account a
          left join (select * 
		             from TL_Mem_AccountChange 
					 where ChangeValue<0 
					   and HasReverse=0	)b on a.AccountID=b.AccountID
          where    a.AccountType=''''3''''
               and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	   	  
          group by a.MemberID,a.AccountID'' 

exec (@Sql)


----正负差值比较，找到serial这个节点

insert into TE_Mem_Account4
select m.*, n.ChangeValue ChangeValue1 ,m.accchangevalue-n.ChangeValue diffamt
from  TE_Mem_Account2  m
inner join     ( select a.MemberID,a.AccountID,min(isnull(serial,0)) serial,min(isnull(b.ChangeValue,0)) ChangeValue
                 from  TE_Mem_Account2 a
				 inner join  TE_Mem_Account3 b on a.MemberID=b.MemberID
				 where  a.accchangevalue>=b.ChangeValue 
				 group by a.MemberID,a.AccountID) n  on m.MemberID=n.MemberID and  m.serial=n.serial





----以节点为起点，查找其后在三个月内即将过期的积分
set @Sql=''insert into TE_Mem_WillDeadPoint
          select t.MemberID,sum(isnull(m.changevalue,0)) WillDeadPoint
          from TM_Mem_Master t
          left join 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where a.EndDate>=''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and dateadd(month ,-3,a.EndDate)<=''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and a.serial>=b.serial
                    ) m on t.MemberID =m.MemberID
         where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	 
         group by t.MemberID''  

exec (@Sql)

set @Sql=''insert into TE_Mem_WillDeadDate
          select t.MemberID, m.WillDeadDate  
          from TM_Mem_Master t
          left join (select w.MemberID ,min(w.enddate) WillDeadDate from 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where 
                      a.serial>=b.serial
                    )  w where w.changevalue<>0 
					group by w.MemberID
					)m on t.MemberID =m.MemberID
        where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)''


exec (@Sql)




end 








' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadCreditYear]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadCreditYear]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[sp_Loy_WillDeadCreditYear]
@MemberList nvarchar(max),
@DatetimeNow nvarchar(8),
@Switch nvarchar(2)

as 



  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Loy_WillDeadCredityear 返回本年度会员即将过期的积分；
  建 立 人：zyb
  建立时间：2015-10-28
  修 改 人：
  修改时间：
  修改内容:
  ***********************************/
begin


--清空临时表
truncate table TE_Mem_WillDeadPoint
truncate table TE_Mem_WillDeadDate
truncate table TE_Mem_Account1
truncate table TE_Mem_Account2
truncate table TE_Mem_Account3
truncate table TE_Mem_Account4 


  


--取正值数据排序累计
declare @Sql nvarchar(max)
set @Sql=''insert into TE_Mem_Account1
          select a.MemberID,a.AccountID,b.ChangeValue,c.SpecialDate2 EndDate,b.AddedDate
                 ,ROW_NUMBER() OVER (partition by b.AccountID ORDER BY b.addeddate asc) serial
          from TM_Mem_Account a
          inner join TL_Mem_AccountChange b on a.AccountID=b.AccountID
		  inner join TM_Mem_AccountDetail c on b.AccountID=b.AccountID and b.AccountDetailID=c.AccountDetailID
          where    a.AccountType=''''3'''' 
              and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)
	          and b.ChangeValue>0
			  and b.HasReverse=0
			  and c.AccountDetailType=''''value1''''

			  ''


--select *　from TL_Mem_AccountChange 
--select *,a.SpecialDate1,a.SpecialDate2,a.AccountDetailType　from TM_Mem_AccountDetail  a
--select * from TD_SYS_BizOption where OptionType=''AccountDetailType''

--value1	可用
--value2	冻结


exec (@Sql)
print @Sql

insert into TE_Mem_Account2
select a.serial,a.MemberID,a.AccountID,a.ChangeValue, 
       SUM(ChangeValue) OVER(
	   partition by a.AccountID 
	   ORDER BY AccountID, SERIAL 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)  accchangevalue,
       a.EndDate,a.AddedDate   
from TE_Mem_Account1 a
order by a.AccountID,serial 
 

----已用积分负值累加
set @Sql=''insert into TE_Mem_Account3
          select a.MemberID,a.AccountID,-sum(isnull(b.ChangeValue,0)) ChangeValue
          from TM_Mem_Account a
          left join (select * 
		             from TL_Mem_AccountChange 
					 where ChangeValue<0 
					   and HasReverse=0	)b on a.AccountID=b.AccountID
          where    a.AccountType=''''3''''
               and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	   	  
          group by a.MemberID,a.AccountID'' 

exec (@Sql)


----正负差值比较，找到serial这个节点

insert into TE_Mem_Account4
select m.*, n.ChangeValue ChangeValue1 ,m.accchangevalue-n.ChangeValue diffamt
from  TE_Mem_Account2  m
inner join     ( select a.MemberID,a.AccountID,min(isnull(serial,0)) serial,min(isnull(b.ChangeValue,0)) ChangeValue
                 from  TE_Mem_Account2 a
				 inner join  TE_Mem_Account3 b on a.MemberID=b.MemberID
				 where  a.accchangevalue>=b.ChangeValue 
				 group by a.MemberID,a.AccountID) n  on m.MemberID=n.MemberID and  m.serial=n.serial





----以节点为起点，查找其后在三个月内即将过期的积分
set @Sql=''insert into TE_Mem_WillDeadPoint
          select t.MemberID,sum(isnull(m.changevalue,0)) WillDeadPoint
          from TM_Mem_Master t
          left join 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where a.EndDate>=''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
					 and substring(convert(nvarchar(8),a.EndDate,112),1,4)=''''''+substring(CAST(@DatetimeNow AS NVARCHAR(20)),1,4)+''''''
                     and a.serial>=b.serial
                    ) m on t.MemberID =m.MemberID
         where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	 
         group by t.MemberID''  

exec (@Sql)

set @Sql=''insert into TE_Mem_WillDeadDate
          select t.MemberID, m.WillDeadDate  
          from TM_Mem_Master t
          left join (select w.MemberID ,min(w.enddate) WillDeadDate from 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where 
                      a.serial>=b.serial
                    )  w where w.changevalue<>0 
					group by w.MemberID
					)m on t.MemberID =m.MemberID
        where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)''


exec (@Sql)




end 








' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadPoint]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadPoint]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[sp_Loy_WillDeadPoint]
@MemberList nvarchar(max),
@DatetimeNow nvarchar(8),
@Switch nvarchar(2)

as 



  /**********************************
  ----arvarto system-----
  存储过程功能描述：[sp_Loy_WillDeadPoint] 返回会员1个月后的当天将过期的可用积点
  建 立 人：zyb
  建立时间：2015-7-29
  修 改 人：
  修改时间：
  修改内容:
  ***********************************/
begin


--清空临时表
truncate table TE_Mem_WillDeadPoint
truncate table TE_Mem_WillDeadDate
truncate table TE_Mem_Account1
truncate table TE_Mem_Account2
truncate table TE_Mem_Account3
truncate table TE_Mem_Account4 


  


--取正值数据排序累计
declare @Sql nvarchar(max)
set @Sql=''insert into TE_Mem_Account1
          select a.MemberID,a.AccountID,b.ChangeValue,c.SpecialDate2 EndDate,b.AddedDate
                 ,ROW_NUMBER() OVER (partition by b.AccountID ORDER BY b.addeddate asc) serial
          from TM_Mem_Account a
          inner join TL_Mem_AccountChange b on a.AccountID=b.AccountID
		  inner join TM_Mem_AccountDetail c on b.AccountID=b.AccountID and b.AccountDetailID=c.AccountDetailID
          where    a.AccountType=''''2'''' 
              and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)
	          and b.ChangeValue>0
			  and b.HasReverse=0
			  and c.AccountDetailType=''''value1''''

			  ''


--select *　from TL_Mem_AccountChange 
--select *,a.SpecialDate1,a.SpecialDate2,a.AccountDetailType　from TM_Mem_AccountDetail  a
--select * from TD_SYS_BizOption where OptionType=''AccountDetailType''

--value1	可用
--value2	冻结


exec (@Sql)

insert into TE_Mem_Account2
select a.serial,a.MemberID,a.AccountID,a.ChangeValue, 
       SUM(ChangeValue) OVER(
	   partition by a.AccountID 
	   ORDER BY AccountID, SERIAL 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)  accchangevalue,
       a.EndDate,a.AddedDate   
from TE_Mem_Account1 a
order by a.AccountID,serial 
 

----已用积分负值累加
set @Sql=''insert into TE_Mem_Account3
          select a.MemberID,a.AccountID,-sum(isnull(b.ChangeValue,0)) ChangeValue
          from TM_Mem_Account a
          left join (select * 
		             from TL_Mem_AccountChange 
					 where ChangeValue<0 
					   and HasReverse=0	)b on a.AccountID=b.AccountID
          where    a.AccountType=''''2''''
               and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	   	  
          group by a.MemberID,a.AccountID'' 

exec (@Sql)


----正负差值比较，找到serial这个节点

insert into TE_Mem_Account4
select m.*, n.ChangeValue ChangeValue1 ,m.accchangevalue-n.ChangeValue diffamt
from  TE_Mem_Account2  m
inner join     ( select a.MemberID,a.AccountID,min(isnull(serial,0)) serial,min(isnull(b.ChangeValue,0)) ChangeValue
                 from  TE_Mem_Account2 a
				 inner join  TE_Mem_Account3 b on a.MemberID=b.MemberID
				 where  a.accchangevalue>=b.ChangeValue 
				 group by a.MemberID,a.AccountID) n  on m.MemberID=n.MemberID and  m.serial=n.serial





----以节点为起点，查找其后在30天后的当天即将过期的积点
set @Sql=''insert into TE_Mem_WillDeadPoint
          select t.MemberID,sum(isnull(m.changevalue,0)) WillDeadPoint
          from TM_Mem_Master t
          left join 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where a.EndDate>=''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and dateadd(day ,-30,a.EndDate) =''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and a.serial>=b.serial
                    ) m on t.MemberID =m.MemberID
         where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	 
         group by t.MemberID''  

exec (@Sql)

set @Sql=''insert into TE_Mem_WillDeadDate
          select memberid ,dateadd(day ,30,''''''+CAST(@DatetimeNow AS NVARCHAR(20))+'''''')   
		  from TE_Mem_WillDeadPoint 
		  where willdeadpoint>0 ''


exec (@Sql)




end 








' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Loy_WillDeadPoint3]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Loy_WillDeadPoint3]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[sp_Loy_WillDeadPoint3]
@MemberList nvarchar(max),
@DatetimeNow nvarchar(8),
@Switch nvarchar(2)

as 



  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Loy_WillDeadPoint3 返回会员3个月内容即将过期的可用积点
  建 立 人：zyb
  建立时间：2015-4-30
  修 改 人：
  修改时间：
  修改内容:
  ***********************************/
begin


--清空临时表
truncate table TE_Mem_WillDeadPoint
truncate table TE_Mem_WillDeadDate
truncate table TE_Mem_Account1
truncate table TE_Mem_Account2
truncate table TE_Mem_Account3
truncate table TE_Mem_Account4 


  


--取正值数据排序累计
declare @Sql nvarchar(max)
set @Sql=''insert into TE_Mem_Account1
          select a.MemberID,a.AccountID,b.ChangeValue,c.SpecialDate2 EndDate,b.AddedDate
                 ,ROW_NUMBER() OVER (partition by b.AccountID ORDER BY b.addeddate asc) serial
          from TM_Mem_Account a
          inner join TL_Mem_AccountChange b on a.AccountID=b.AccountID
		  inner join TM_Mem_AccountDetail c on b.AccountID=b.AccountID and b.AccountDetailID=c.AccountDetailID
          where    a.AccountType=''''2'''' 
              and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)
	          and b.ChangeValue>0
			  and b.HasReverse=0
			  and c.AccountDetailType=''''value1''''

			  ''


--select *　from TL_Mem_AccountChange 
--select *,a.SpecialDate1,a.SpecialDate2,a.AccountDetailType　from TM_Mem_AccountDetail  a
--select * from TD_SYS_BizOption where OptionType=''AccountDetailType''

--value1	可用
--value2	冻结


exec (@Sql)

insert into TE_Mem_Account2
select a.serial,a.MemberID,a.AccountID,a.ChangeValue, 
       SUM(ChangeValue) OVER(
	   partition by a.AccountID 
	   ORDER BY AccountID, SERIAL 
	   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)  accchangevalue,
       a.EndDate,a.AddedDate   
from TE_Mem_Account1 a
order by a.AccountID,serial 
 

----已用积分负值累加
set @Sql=''insert into TE_Mem_Account3
          select a.MemberID,a.AccountID,-sum(isnull(b.ChangeValue,0)) ChangeValue
          from TM_Mem_Account a
          left join (select * 
		             from TL_Mem_AccountChange 
					 where ChangeValue<0 
					   and HasReverse=0	)b on a.AccountID=b.AccountID
          where    a.AccountType=''''2''''
               and (a.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	   	  
          group by a.MemberID,a.AccountID'' 

exec (@Sql)


----正负差值比较，找到serial这个节点

insert into TE_Mem_Account4
select m.*, n.ChangeValue ChangeValue1 ,m.accchangevalue-n.ChangeValue diffamt
from  TE_Mem_Account2  m
inner join     ( select a.MemberID,a.AccountID,min(isnull(serial,0)) serial,min(isnull(b.ChangeValue,0)) ChangeValue
                 from  TE_Mem_Account2 a
				 inner join  TE_Mem_Account3 b on a.MemberID=b.MemberID
				 where  a.accchangevalue>=b.ChangeValue 
				 group by a.MemberID,a.AccountID) n  on m.MemberID=n.MemberID and  m.serial=n.serial





----以节点为起点，查找其后在三个月内即将过期的积分
set @Sql=''insert into TE_Mem_WillDeadPoint
          select t.MemberID,sum(isnull(m.changevalue,0)) WillDeadPoint
          from TM_Mem_Master t
          left join 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where a.EndDate>=''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and dateadd(month ,-3,a.EndDate)<=''''''+CAST(@DatetimeNow AS NVARCHAR(20))+''''''
                     and a.serial>=b.serial
                    ) m on t.MemberID =m.MemberID
         where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)	 
         group by t.MemberID''  

exec (@Sql)

set @Sql=''insert into TE_Mem_WillDeadDate
          select t.MemberID, m.WillDeadDate  
          from TM_Mem_Master t
          left join (select w.MemberID ,min(w.enddate) WillDeadDate from 
                  (
                   select a.MemberID,a.AccountID,a.serial,
                          case when a.serial=b.serial 
						       then b.diffamt else a.changevalue end changevalue ,a.enddate
                   from  TE_Mem_Account2 a
                   inner join  TE_Mem_Account4 b on a.MemberID=b.MemberID
                   where 
                      a.serial>=b.serial
                    )  w where w.changevalue<>0 
					group by w.MemberID
					)m on t.MemberID =m.MemberID
        where (t.MemberID in (''+@MemberList+'') or ''+@Switch+''=1)''


exec (@Sql)




end 








' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MarketActivityTracking]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MarketActivityTracking]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Rpt_MarketActivityTracking]
	@searchdate DateTime,--查询月份
    @PageIndex int,--当前页
    @PageSize int,--总页数
	@RecordCount int out	--总数
AS
BEGIN

declare @Sql  varchar(max)

set @Sql= ''
select ''''2015-10-09'''' as ActivityTime,
''''促销测试'''' as ActivityName,
''''100'''' as SubdivisionCustomersNumber,
''''1000'''' as SmsPushNumber,
''''20000000'''' as ReturnSalesCumulative,
''''10%'''' as OccupyOverallSalesB,
''''16%'''' as OccupyOverallPayB,
''''19%'''' as OccupyOverallPayHB,
''''1666'''' as Male_MemNum,
''''14445'''' as Female_MemNum,
''''1000'''' as EighteenToTwentytwo_MemNum,
''''20000'''' as TwentythreeToTwentynine_MemNum,
''''1544'''' as ThirtyToThirtyfive_MemNum,
''''343'''' as ThirtyfiveToThirtynine_MemNum,
''''43'''' as FortyThan_MemNum,
''''34'''' as Unknown_MemNum
union all
select ''''2015-10-09'''' as ActivityTime,
''''促销测试1'''' as ActivityName,
''''100'''' as SubdivisionCustomersNumber,
''''1000'''' as SmsPushNumber,
''''20000000'''' as ReturnSalesCumulative,
''''10%'''' as OccupyOverallSalesB,
''''16%'''' as OccupyOverallPayB,
''''19%'''' as OccupyOverallPayHB,
''''1666'''' as Male_MemNum,
''''14445'''' as Female_MemNum,
''''1000'''' as EighteenToTwentytwo_MemNum,
''''20000'''' as TwentythreeToTwentynine_MemNum,
''''1544'''' as ThirtyToThirtyfive_MemNum,
''''343'''' as ThirtyfiveToThirtynine_MemNum,
''''43'''' as FortyThan_MemNum,
''''34'''' as Unknown_MemNum''
	   print @Sql
	   exec (@Sql)
END





' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Activitycount]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Activitycount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
=============================================
Author:		<shifenglong>
Create date: <Create Date,,>
Description:	<市场活动跟踪,,>
Change:去除订单的环比，加上了促销活动信息
       20151103 lqw  作大的修改
=============================================
*/

CREATE PROCEDURE [dbo].[sp_Rpt_Mem_Activitycount]
	@dateBegin2 DateTime ,--开始时间
    @dateEnd2   DateTime,--结束时间
	--@channel	nvarchar(100),--渠道
	--@area	nvarchar(100),--大区
	--@city	nvarchar(100), --城市
	--@store nvarchar(100),--门店
	@PageIndex int,--第几页,0开始
    @PageSize int,-- 一页几行
	@RecordCount int out	 -- 总数据量
AS
BEGIN

	if @PageIndex is null or @PageIndex = -1
	begin
	declare @tabActFollow table(
	 ActDate nvarchar(1000),--活动时间
	 ActName nvarchar(2000),--活动名称
	 ConsumerNumber int ,--细分会员群数量
	 SmsNumber int ,--短信推送数量
	 TotActMoney decimal(18,2),--回归销售统计额
	 TotActRate decimal(18,2),--占总体销售额比（会员与非会员）
	 TradeRate decimal(18,2),--占总体买单数比
	 --CircleTradeRate decimal(18,2), --总体买单数环比   --要删除
	 ManNumber int ,--男性会员数
	 WomenNumber int ,--女性会员数
	 SecretNumber int,--保密会员数
	 Age1Number int ,--18-22岁会员数
	 Age2Number int ,--23-29岁会员数
     Age3Number int ,--30-34岁会员数
     Age4Number int ,--35-39岁会员数
	 Age5Number int ,--40以上会员数
	 UnknownNumber int --UNKNOWN会员数  改为未知年龄会员
)
		select * from @tabActFollow
		return
		end


declare @Sql  varchar(max) 
declare @Sql_Search varchar(max)
set @Sql_Search =''''

declare
@sql1 nvarchar(max) = '''',
@sql2 nvarchar(max) = '''',
@sql3 nvarchar(max) = '''',
@sql4 nvarchar(max) = '''';



declare 
@dateBegin nvarchar(100),
@dateEnd   nvarchar(100);
set @dateBegin = convert(nvarchar, @dateBegin2, 23);
set @dateEnd = convert(nvarchar, @dateEnd2, 23);


/*
    --大区暂时没有,条件不写 
	if ( isnull(@channel, '''') <> ''''  )
	begin
		set @Sql_Search = @Sql_Search + '' and ChannelCodeBase =  ''+@channel+'' ''   
	end
	if ( isnull(@area, '''') <> ''''  )
	begin
		set @Sql_Search = @Sql_Search + '' and AreaCodeStore =  ''+@area+'' ''   
	end
	 
	if (isnull(@store, '''') <> ''''and @store <> ''null'' )
	begin
		set @Sql_Search = @Sql_Search + '' and storecode =   ''+ @store + '' ''    
	end

	if (isnull(@city, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and CityCodeStore =   ''+ @city + '' ''    
	end
*/



	 set @sql1 =''select 
     ActDate ,--活动时间
	 ActName ,--活动名称
	 ConsumerNumber  ,--细分会员群数量
	 SmsNumber  ,--短信推送数量
	 TotActMoney ,--回归销售统计额
	 TotActRate,--占总体销售额比（会员与非会员）
	 TradeRate ,--占总体买单数比
	 ManNumber  ,--男性会员数
	 WomenNumber  ,--女性会员数
	 SecretNumber,--保密会员数
	 Age1Number  ,--18-22岁会员数
	 Age2Number  ,--23-29岁会员数
     Age3Number  ,--30-34岁会员数
     Age4Number  ,--35-39岁会员数
	 Age5Number  ,--40以上会员数
	 UnknownNumber--,  --UNKNOWN会员数 --未知年龄会员
	 --rn 
	 from	
(
select 
     ActDate ,--活动时间
	 ActName ,--活动名称
	 ConsumerNumber  ,--细分会员群数量
	 SmsNumber  ,--短信推送数量
	 TotActMoney ,--回归销售统计额
	 TotActRate,--占总体销售额比（会员与非会员）
	 TradeRate ,--占总体买单数比
	 ManNumber  ,--男性会员数
	 WomenNumber  ,--女性会员数
	 SecretNumber,--保密会员数
	 Age1Number  ,--18-22岁会员数
	 Age2Number  ,--23-29岁会员数
     Age3Number  ,--30-34岁会员数
     Age4Number  ,--36-39岁会员数
	 Age5Number  ,--40以上会员数
	 UnknownNumber,  --UNKNOWN会员数	
	 Row_number() OVER (ORDER BY ActDate) rn --排序
from
(select 
n.activityid,
convert(nvarchar(20),n.StartDate,112)+''''-''''+convert(nvarchar(20),n.EndDate,112) ActDate,
--''''2015-10'''' ActDate,
n.ActivityName ActName,
n.ConsumerNumber,
isnull(t.SmsNumber,0) SmsNumber,
m.TotActMoney TotActMoney,
cast (cast (m.TotActMoney as float)/tot_money as decimal(18,2)) TotActRate ,
cast (cast (act_trade as float)/tot_trade as decimal(18,2)) TradeRate ,
n.ManNumber,
n.WomenNumber,
n.SecretNumber,
n.Age1Number,
n.Age2Number,
n.Age3Number,
n.Age4Number,
n.Age5Number,
n.UnknownNumber
  from 
(select c.activityid, count(c.tradeid) act_trade,sum(c.totalmoneysales) TotActMoney,
tot_trade=(select count(tradeid) from V_M_TM_Mem_Trade_sales where listdatesales >= convert(nvarchar(20),''''''+@dateBegin+'''''',112)
                                                               and listdatesales <  convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)
),
tot_money=(select sum(totalmoneysales) from V_M_TM_Mem_Trade_sales where listdatesales >= convert(nvarchar(20),''''''+@dateBegin+'''''',112)
                                                                     and listdatesales <  convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)
)
 from  ( select a.activityid,b.tradeid,b.totalmoneysales from 
 (select distinct memberid,activityid,EndDate,StartDate from  
TR_Mem_MarketActivity) a 
 inner join V_M_TM_Mem_Trade_sales b 
     on a.memberid= b.memberid
  where listdatesales >= convert(nvarchar(20),''''''+@dateBegin+'''''',112)
    and listdatesales <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)
    and ( ( a.StartDate >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and a.StartDate < convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
	    or( a.EndDate   >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and a.StartDate < convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) )
	)c
 group by c.activityid) m
'';
set @sql2 = ''
 left join (select b.ActivityID,b.ActivityName,b.StartDate,b.EndDate,
                                             sum(case when datediff(year,birthday,registerdate)>=18 and datediff(year,birthday,registerdate)<=22 then 1 else 0 end) Age1Number,
                                             sum(case when datediff(year,birthday,registerdate)>=23 and datediff(year,birthday,registerdate)<=29 then 1 else 0 end) Age2Number,
		                                     sum(case when datediff(year,birthday,registerdate)>=30 and datediff(year,birthday,registerdate)<=34 then 1 else 0 end) Age3Number,
		                                     sum(case when datediff(year,birthday,registerdate)>=35 and datediff(year,birthday,registerdate)<=39 then 1 else 0 end) Age4Number,
		                                     sum(case when datediff(year,birthday,registerdate)>=40  then 1 else 0 end ) Age5Number,
                                             sum(case when birthday is null or datediff(year,birthday,registerdate)<18 then 1 else 0 end ) UnknownNumber,
		                                     sum(case when a.Gender = ''''男'''' then 1 else 0 end ) ManNumber,
		                                     sum(case when a.Gender = ''''女'''' then 1 else 0 end ) WomenNumber,
											 sum(case when a.Gender = ''''保密'''' then 1 else 0 end ) SecretNumber,
		                                     count(distinct b.memberid) ConsumerNumber
		                                     from V_S_TM_Mem_Ext a inner join (select t.memberid,t.ActivityID,t.ActivityName,t.StartDate,t.EndDate 
                                                                                 from TR_Mem_MarketActivity t 
																				 where ( t.StartDate >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and t.StartDate < convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
	                                                                                or ( t.EndDate   >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and t.StartDate < convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
                                                                                 group by t.memberid,t.ActivityID,t.ActivityName,t.StartDate,t.EndDate)b
                                                                    on a.MemberID = b.MemberID
                                                                     group by b.ActivityID,b.ActivityName,b.StartDate,b.EndDate) n
      on m.activityid = n.activityid

	  left join (select ActivityID ,count(*) SmsNumber from  TM_Sys_SMSSendingQueue a inner join (select ActivityID,t.activityinstanceid
                                  from TR_Mem_MarketActivity t
								   where ( t.StartDate >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and t.StartDate < convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
	                                  or ( t.EndDate   >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and t.StartDate < convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
				  group by t.ActivityID,t.activityinstanceid)b 
	on a.actinstanceid = b.activityinstanceid
	group by ActivityID) t
	on m.activityid = t.ActivityID

	union all
'';
set @sql3 = ''
select
n.promotionid,  
convert(nvarchar(20),n.startdatepromotion,112)+''''-''''+convert(nvarchar(20),n.enddatepromotion,112) ActDate,
--''''2015-10'''' ActDate,
n.promotionname ActName,
n.ConsumerNumber,
isnull(t.SmsNumber,0),
m.TotActMoney,
cast (cast (m.TotActMoney as float)/tot_money as decimal(18,2)) TotActRate ,
cast (cast (act_trade as float)/tot_trade as decimal(18,2)) TradeRate ,
n.ManNumber,
n.WomenNumber,
n.SecretNumber,
n.Age1Number,
n.Age2Number,
n.Age3Number,
n.Age4Number,
n.Age5Number,
n.UnknownNumber
  from 
(select c.promotionid, count(c.tradeid) act_trade,sum(c.totalmoneysales) TotActMoney,
tot_trade=(select count(tradeid) from V_M_TM_Mem_Trade_sales where listdatesales >= convert(nvarchar(20),''''''+@dateBegin+'''''',112)
                                                               and listdatesales <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)
),
tot_money=(select sum(totalmoneysales) from V_M_TM_Mem_Trade_sales where listdatesales >= convert(nvarchar(20),''''''+@dateBegin+'''''',112)
                                                                     and listdatesales <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)
)
 from  ( select a.promotionid,b.tradeid,b.totalmoneysales from 
 (select distinct a.memberid,a.promotionid,b.promotionname,b.startdatepromotion,b.enddatepromotion 
from TM_POS_MemberPromotion a left join V_M_TM_SYS_BaseData_promotion b on a.promotionid = b.promotionid) a 
 inner join V_M_TM_Mem_Trade_sales b 
     on a.memberid= b.memberid
  where listdatesales >= convert(nvarchar(20),''''''+@dateBegin+'''''',112)
    and listdatesales <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)
    and ( ( a.startdatepromotion >= convert(nvarchar(20), ''''''+@dateBegin+'''''',112) and a.startdatepromotion <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
	   or ( a.enddatepromotion   >= convert(nvarchar(20),''''''+@dateBegin+'''''',112)  and a.enddatepromotion   <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) )
	)c
 group by c.promotionid) m
 '';
set @sql4 = ''
 left join (select b.promotionid,b.promotionname,b.startdatepromotion,b.enddatepromotion,
                                             sum(case when datediff(year,birthday,registerdate)>=18 and datediff(year,birthday,registerdate)<=22 then 1 else 0 end) Age1Number,
                                             sum(case when datediff(year,birthday,registerdate)>=23 and datediff(year,birthday,registerdate)<=29 then 1 else 0 end) Age2Number,
		                                     sum(case when datediff(year,birthday,registerdate)>=30 and datediff(year,birthday,registerdate)<=34 then 1 else 0 end) Age3Number,
		                                     sum(case when datediff(year,birthday,registerdate)>=35 and datediff(year,birthday,registerdate)<=39 then 1 else 0 end) Age4Number,
		                                     sum(case when datediff(year,birthday,registerdate)>=40  then 1 else 0 end ) Age5Number,
                                             sum(case when birthday is null or datediff(year,birthday,registerdate)<18 then 1 else 0 end ) UnknownNumber,
		                                     sum(case when a.Gender = ''''男'''' then 1 else 0 end ) ManNumber,
		                                     sum(case when a.Gender = ''''女'''' then 1 else 0 end ) WomenNumber,
											 sum(case when a.Gender = ''''保密'''' then 1 else 0 end ) SecretNumber,
		                                     count(distinct b.memberid) ConsumerNumber
		                                     from V_S_TM_Mem_Ext a inner join (select t.memberid,t.promotionid,n.promotionname,n.startdatepromotion,n.enddatepromotion 
                                                                                 from TM_POS_MemberPromotion t left join V_M_TM_SYS_BaseData_promotion n on t.promotionid = n.promotionid
																				 where ( n.startdatepromotion >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and n.startdatepromotion <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
	                                                                                or ( n.enddatepromotion   >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and n.enddatepromotion   <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
                                                                                 group by t.memberid,t.promotionid,n.promotionname,n.startdatepromotion,n.enddatepromotion)b
                                                                    on a.MemberID = b.MemberID
                                                                     group by b.promotionid,b.promotionname,b.startdatepromotion,b.enddatepromotion) n
      on m.promotionid = n.promotionid

	  left join (select promotionid ,count(*) SmsNumber from  TM_Sys_SMSSendingQueue a inner join (select  t.memberid,t.promotionid
                                                                                                         from TM_POS_MemberPromotion t left join V_M_TM_SYS_BaseData_promotion n 
																										  on t.promotionid = n.promotionid
								   where ( n.startdatepromotion >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and  n.startdatepromotion <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
	                                  or ( n.enddatepromotion   >= convert(nvarchar(20),''''''+@dateBegin+'''''',112) and  n.enddatepromotion   <= convert(nvarchar(20),dateadd(day,1,''''''+@dateEnd+''''''),112)) 
				  group by t.promotionid,t.memberid)b 
	on a.memberid = b.memberid
	group by promotionid) t
	on m.promotionid = t.promotionid
	) t
	)a
   WHERE rn >= ''''''+ convert(varchar(20),@PageIndex  * @PageSize)+''''''
       AND rn <= ''''''+convert(varchar(20),(@PageIndex+1) * @PageSize)+''''''
		''+@Sql_Search;

  print @sql1;
  print @sql2;
  print @sql3;
  print @sql4;

  set @sql = @sql1 + @sql2 + @sql3 + @sql4;

  declare @tabActFollow_result table(
	 ActDate nvarchar(1000),--活动时间
	 ActName nvarchar(2000),--活动名称
	 ConsumerNumber int ,--细分会员群数量
	 SmsNumber int ,--短信推送数量
	 TotActMoney decimal(18,2),--回归销售统计额
	 TotActRate decimal(18,2),--占总体销售额比（会员与非会员）
	 TradeRate decimal(18,2),--占总体买单数比
	 --CircleTradeRate decimal(18,2), --总体买单数环比   --要删除
	 ManNumber int ,--男性会员数
	 WomenNumber int ,--女性会员数
	 SecretNumber int,--保密会员数
	 Age1Number int ,--18-22岁会员数
	 Age2Number int ,--23-29岁会员数
     Age3Number int ,--30-34岁会员数
     Age4Number int ,--35-39岁会员数
	 Age5Number int ,--40以上会员数
	 UnknownNumber int --UNKNOWN会员数  改为未知年龄会员
)
		 	  
declare  @sqlCount nvarchar(max)
 set @sqlCount = ''select @ct = count(1) from( '' + @Sql +'')T''
 
    exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 
	 --  print @sqlCount
	   --print @Sql
	   insert into @tabActFollow_result exec (@Sql)
	  select * from @tabActFollow_result
 end
 --end' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_ConsumerDetails]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_ConsumerDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_Rpt_Mem_ConsumerDetails]
	@channel	nvarchar(200),--渠道
	@area	nvarchar(100),--大区
	@city	nvarchar(100), --城市
	@store nvarchar(max), --门店decimal(18, 2)
	@dateConsumptionStart DateTime,--查询消费起日
	@dateConsumptionEnd	DateTime, --查询消费止日
    @PageIndex int,--当前页
    @PageSize int,--总页数
	@RecordCount int out	--总数
AS
/**********************************
  ----arvarto system-----
  存储过程功能描述：会员消费明细报表
  建 立 人：
  建立时间：
  修改内容: 20151218 zyb修改消费时间addeddate改为Date_Attr_1 ，增加订单类型标识；
  ***********************************/
BEGIN
	 
declare @tabMemConsumerDetails table(
	 rowid nvarchar(20),
	 ConsumptionDate datetime,--消费日期
	 ConsumptionStore nvarchar(100),--消费店铺
	 ConsumptionChannel nvarchar(100),--消费渠道
	 Mobile nvarchar(30),--手机
	 Name nvarchar(20),--姓名
	 [No] nvarchar(50),--单据号
	 Num nvarchar(30),--数量
	 PayMoney decimal(18, 2),--应付金额
	 DiscountMoney decimal(18, 2),--折扣金额
	 SettleMoney decimal(18, 2)--结算金额
	
)

declare @Sql  varchar(max),
 @SqlSearchCondition  nvarchar(max)='' where 1=1 and tradetype=''''sales'''' '',
 @rowBegin int=@PageIndex*@PageSize+1,
 @rowEnd int=(@PageIndex+1)*@PageSize,
 @Sql_Count nvarchar(max)

begin
 
	if (isnull(@channel, '''') <> '''' )--渠道
	begin
		--set @SqlSearchCondition=@SqlSearchCondition + '' and channel.ChannelCodeBase  =''''''+ @channel +'''''' ''
		set @SqlSearchCondition=@SqlSearchCondition + '' and channel.ChannelCodeBase  in (''+ @channel +'') ''
	end
	
	if (isnull(@area, '''') <> '''' )--大区
	begin
		set @SqlSearchCondition=@SqlSearchCondition + '' and v.AreaCodeStore  = '''''' +@area+'''''' ''
	end
	
	if (isnull(@city, '''') <> '''' )--城市
	begin
		set @SqlSearchCondition=@SqlSearchCondition + '' and v.CityCodeStore  =''''''+@city +'''''' '' 
	end
	
	if (isnull(@store, '''') <> '''' )--门店
	begin
		--set @SqlSearchCondition=@SqlSearchCondition + '' and tmt.Str_Attr_2  = '''''' +@store+'''''' ''

        if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
           begin
              drop table #TE_Rpt_StoreCode;
           end
        
        select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
        into #TE_Rpt_StoreCode;
        
        if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
           begin
              drop table #TE_Rpt_StoreCode2;
           end
        
        select replace(b.StoreCode, '''''''', '''') as StoreCode
        into #TE_Rpt_StoreCode2
         from #TE_Rpt_StoreCode a
        outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;
        
        set @SqlSearchCondition = @SqlSearchCondition + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = tmt.Str_Attr_2) '';

	end
	
	if (isnull(@dateConsumptionStart, '''') <> '''' )--查询消费起日
	begin
		set @SqlSearchCondition=@SqlSearchCondition + '' and tmt.Date_Attr_1  >=  ''''''+convert(NVARCHAR(10),@dateConsumptionStart,21)+'''''' '' 
	end
	
	if (isnull(@dateConsumptionEnd, '''') <> '''' )--查询消费止日
	begin
		set @SqlSearchCondition=@SqlSearchCondition + '' and tmt.Date_Attr_1 <=  ''''''+convert(NVARCHAR(10),@dateConsumptionEnd,21)+'''''' ''  
	end
	
end

set @Sql=''select ROW_NUMBER() OVER (ORDER BY tab.ConsumptionDate desc) as rowid,tab.* from (select
convert(NVARCHAR(20),tmt.Date_Attr_1,120) as ConsumptionDate,--消费日期
 v.StoreName as ConsumptionStore, --消费店铺
 channel.ChannelNameBase as ConsumptionChannel,--消费渠道
 tmt.Str_Attr_3 as Mobile,--手机号
 tme.Str_Attr_3 as Name,--姓名
 tmt.Str_Attr_1 as No,--单据编号
 CAST(tmt.Int_Attr_4 as varchar(10)) as Num,--数量
 CAST(convert(decimal(38, 2),tmt.Dec_Attr_5) as varchar(20)) as PayMoney,--应付金额
 CAST(convert(decimal(38, 2),tmt.Dec_Attr_4) as varchar(20)) as DiscountMoney,--折扣金额
 CAST(convert(decimal(38, 2),tmt.Dec_Attr_7) as varchar(20)) as SettleMoney--结算金额
 from TM_Mem_Trade as tmt with(nolock) 
 left join TM_Mem_Ext tme with(nolock) on tmt.MemberID=tme.MemberID 
 left join V_M_TM_SYS_BaseData_store as v with(nolock) 
on tmt.Str_Attr_2=v.StoreCode 
left join V_M_TM_SYS_BaseData_channel as channel on v.ChannelTypeCodeStore=channel.ChannelCodeBase
''+@SqlSearchCondition+''
) as tab
''
    set @Sql_Count = ''select @ct = count(*) from( '' + @Sql +'')T''

	exec sp_executesql @Sql_Count,N''@ct int output'',@RecordCount output 
	
    set @Sql=''select * from (''+@Sql+'') as tab where rowid between ''+cast(@rowBegin as nvarchar(10))+'' and ''+cast(@rowEnd as nvarchar(10))+''''
	   
    print @Sql
	insert into @tabMemConsumerDetails exec (@Sql)
	select * from @tabMemConsumerDetails
END

--exec  [sp_Rpt_Mem_ConsumerDetails] '''','''','''','''','''','''',0,2,''''' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_ConsumptionFrequency_Count]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_ConsumptionFrequency_Count]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Rpt_Mem_ConsumptionFrequency_Count]
	@channel nvarchar(200) ,--渠道
    @brand  nvarchar(100),--品牌
	@category	nvarchar(100),--品类
	@dateConsumptionStart	DateTime,--消费起日
	@dateConsumptionEnd	DateTime --消费止日
AS
BEGIN

	declare @tblMemQstResult table (
		    member nvarchar(20),
			FrequencyEqZero int,
			FrequencyEqOne int,
			FrequencyEqTwo int,
			FrequencyEqThree int,
			FrequencyEqFour int,
			FrequencyEqFive int,
			FrequencyEqFiveGoTen int,
			FrequencyThanTen int 
		)
	 
declare @Sql  varchar(max)
declare @SalesSql varchar(max)
declare @Sql_Search varchar(max),@Sql_Search1 varchar(max)=''where 1=1''

set @Sql_Search = '' where 1 = 1 '' 
 		set @Sql_Search = @Sql_Search + '' and ListDateSales >= ''''''+convert( NVARCHAR(20),@dateConsumptionStart,120)+'''''' ''    
	  
		set @Sql_Search = @Sql_Search + '' and ListDateSales <= ''''''+convert( NVARCHAR(20),@dateConsumptionEnd,120)+'''''' ''    
	if ( isnull(@channel,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and ChannelCodeBase in( ''+@channel+'') ''   
	end
	
	if ( isnull(@brand,'''') <> '''' )
	begin
		set @Sql_Search1 = @Sql_Search1 + '' and vb.ProductBrandCode in( ''''''+@brand+'''''') ''   
	end
	if ( isnull(@category,'''') <> '''' )
	begin
		set @Sql_Search1 = @Sql_Search1 + '' and vb.CategoryCode in( ''''''+@category+'''''') ''   
	end
	
  
set @SalesSql = ''select MemberID,count(1) cnt from V_M_TM_Mem_Trade_sales a with (nolock)
left join V_M_TM_SYS_BaseData_store b on a.StoreCodeSales = b.StoreCode
left join V_M_TM_SYS_BaseData_channel c on b.ChannelTypeCodeStore =  c.ChannelCodeBase 
right join 
(
select vt.TradeID from V_M_TM_Mem_TradeDetail_sales_product vt
left join V_M_TM_SYS_BaseData_product vb
on
vt.GoodsCodeProduct=vb.ProductCode
''+@Sql_Search1+''
group by vt.TradeID
) as tab
on 
a.TradeID=tab.TradeID
''+ @Sql_Search +'' group by MemberID''

set @sql =''
select 
c.CustomerLevelNameBase as Member,
sum(case when b.cnt = 0 then 1 else 0 end) as FrequencyEqZero, 
sum(case when b.cnt = 1 then 1 else 0 end) as FrequencyEqOne, 
sum(case when b.cnt = 2 then 1 else 0 end) as FrequencyEqTwo, 
sum(case when b.cnt = 3 then 1 else 0 end) as FrequencyEqThree, 
sum(case when b.cnt = 4 then 1 else 0 end) as FrequencyEqFour, 
sum(case when b.cnt = 5 then 1 else 0 end) as FrequencyEqFive, 
sum(case when b.cnt >5 and b.cnt<=10  then 1 else 0 end) as FrequencyEqFiveGoTen,  
sum(case when b.cnt >10  then 1 else 0 end) as FrequencyThanTen
 from V_S_TM_Mem_Master a
left join (''+ @SalesSql +'') b on a.MemberID = b.MemberID
left join  V_M_TM_SYS_BaseData_customerlevel c on c.CustomerLevelBase = a.CustomerLevel 
group by c.CustomerLevelNameBase 
 ''	print @sql
	  insert into @tblMemQstResult exec (@Sql)
	  select * from @tblMemQstResult
	 
END 





' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_ContributionRate_Statistics]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_ContributionRate_Statistics]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
 *********************************************
 Author:        Chris.W.Lang
 Create date:   2015-11-02
 Description:    会员贡献率统计
 Change:        20151103传入日期参数由varchar修改为datetime类型，yyyy-mm-dd
                20151104实体临时表改为虚拟临时表|修改参数传入机制
 *********************************************
*/
CREATE PROCEDURE [dbo].[sp_Rpt_Mem_ContributionRate_Statistics]
    @datetime_year datetime,
    @datetime_month datetime
    --@flag nvarchar(10)--, --0传入年份,如：2015 1传入年月,如：2015-11
    --@PageIndex int,  --当前页
    --@PageSize int,  --总页数
    --@RecordCount int out  --总数
AS
BEGIN

if 1 = 2
begin
declare @Mem_Contri_Rate_Stat table(  
   MemberLevel            nvarchar(100),  --会员等级
   Spending               decimal(18, 2), --消费额
   SpendingTB             decimal(18, 4), --消费额同比
   SpendingHB             decimal(18, 4), --消费额环比
   GuestUnitPrice         decimal(18, 2), --客单价
   GuestUnitPriceTB       decimal(18, 4), --客单价同比
   GuestUnitPriceHB       decimal(18, 4), --客单价环比
   GuestUnitCount         decimal(18, 2), --客单量
   GuestUnitCountTB       decimal(18, 4), --客单量同比
   GuestUnitCountHB       decimal(18, 4), --客单量环比
   SpendingRiseRate       decimal(18, 4), --消费额增长率
   ContributionRateMember decimal(18, 4), --线下消费贡献率（占会员）
   ContributionRateTotal  decimal(18, 4)  --线下消费贡献率（占总体）
   );
select * from @Mem_Contri_Rate_Stat;
return
end

declare
@year nvarchar(100),
@month nvarchar(100),
@begindate_dq nvarchar(100),
@enddate_dq nvarchar(100),
@begindate_tb nvarchar(100),
@enddate_tb nvarchar(100),
@begindate_hb nvarchar(100),
@enddate_hb nvarchar(100),
@month_2 datetime;


--if @flag = ''0''
if isnull(@datetime_year, '''') <> ''''
begin

set @year = substring(convert(nvarchar(100), @datetime_year, 23), 1, 4);

--当期
set @begindate_dq = @year + ''-01-01 00:00:00'';
set @enddate_dq = @year + ''-12-31 23:59:59'';
print @begindate_dq;
print @enddate_dq;

--环比
set @begindate_hb = cast(@year-1 as nvarchar) + ''-01-01 00:00:00'';
set @enddate_hb = cast(@year-1 as nvarchar) + ''-12-31 23:59:59'';
print @begindate_hb;
print @enddate_hb;

--同比
set @begindate_tb = cast(@year-1 as nvarchar) + ''-01-01 00:00:00'';
set @enddate_tb = cast(@year-1 as nvarchar) + ''-12-31 23:59:59'';
print @begindate_tb;
print @enddate_tb;

end

--if @flag = ''1''
if isnull(@datetime_month, '''') <> ''''
begin

set @month = substring(convert(nvarchar(100), @datetime_month, 23), 1, 7);

set @month_2 = cast(@month + ''-01'' as datetime);
print @month_2;

--当期
set @begindate_dq = convert(varchar(100), dateadd(dd, -day(@month_2) + 1, @month_2), 23) + '' 00:00:00'';
set @enddate_dq = convert(nvarchar(100), dateadd(dd, -day(dateadd(month, 1, @month_2)), dateadd(month, 1, @month_2)), 23) + '' 23:59:59'';
print @begindate_dq;
print @enddate_dq;

--环比
set @begindate_hb = convert(varchar(100), dateadd(dd, -day(dateadd(month, -1, @month_2)) + 1, dateadd(month, -1, @month_2)), 23) + '' 00:00:00'';
set @enddate_hb = convert(varchar(100), dateadd(dd, -day(@month_2), @month_2), 23) + '' 23:59:59'';
print @begindate_hb;
print @enddate_hb;

--同比
set @begindate_tb = convert(nvarchar, dateadd(yy, -1, @begindate_dq), 23) + '' 00:00:00'';
set @enddate_tb = convert(nvarchar, dateadd(yy, -1, @enddate_dq), 23) + '' 23:59:59'';
print @begindate_tb;
print @enddate_tb;

end


--当期
if object_id(''tempdb.dbo.#TE_kdj_dq'') is not null
   drop table #TE_kdj_dq;

select isnull(t1.CustomerLevel, ''合计'') CustomerLevel, --会员等级
       sum(t2.TotalMoneySales) xfe, --消费额
       --count(t2.TradeID) jyl, --交易订单量
       sum(t2.TotalMoneySales) / count(t2.TradeID) kdj --客单价
  into #TE_kdj_dq
  from V_S_TM_Mem_Master t1
  join V_M_TM_Mem_Trade_sales t2 on t1.MemberID = t2.MemberID
 where t2.ListDateSales >= @begindate_dq
   and t2.ListDateSales <= @enddate_dq
 group by cube(t1.CustomerLevel)
 order by CustomerLevel;

if object_id(''tempdb.dbo.#TE_kdl_dq'') is not null
   drop table #TE_kdl_dq;

select isnull(t1.CustomerLevel, ''合计'') CustomerLevel, --会员等级
       --sum(t3.QuantityProduct) xfzl, --消费总量
       --count(distinct t3.TradeID) jyl, --交易订单量
       sum(t3.QuantityProduct) / count(distinct t3.TradeID) kdl --客单量
  into #TE_kdl_dq
  from V_S_TM_Mem_Master t1
  join V_M_TM_Mem_Trade_sales t2 on t1.MemberID = t2.MemberID
  join V_M_TM_Mem_TradeDetail_sales_product t3 on t2.TradeID = t3.TradeID
 where t2.ListDateSales >= @begindate_dq
   and t2.ListDateSales <= @enddate_dq
 group by cube(t1.CustomerLevel)
 order by CustomerLevel;


--环比
if object_id(''tempdb.dbo.#TE_kdj_hb'') is not null
   drop table #TE_kdj_hb;

select isnull(t1.CustomerLevel, ''合计'') CustomerLevel,
       sum(t2.TotalMoneySales) xfe,
       --count(t2.TradeID) jyl,
       sum(t2.TotalMoneySales) / count(t2.TradeID) kdj
  into #TE_kdj_hb
  from V_S_TM_Mem_Master t1
  join V_M_TM_Mem_Trade_sales t2 on t1.MemberID = t2.MemberID
 where t2.ListDateSales >= @begindate_hb
   and t2.ListDateSales <= @enddate_hb
 group by cube(t1.CustomerLevel)
 order by CustomerLevel;

if object_id(''tempdb.dbo.#TE_kdl_hb'') is not null
   drop table #TE_kdl_hb;

select isnull(t1.CustomerLevel, ''合计'') CustomerLevel,
       --sum(t3.QuantityProduct) xfzl,
       --count(distinct t3.TradeID) jyl,
       sum(t3.QuantityProduct) / count(distinct t3.TradeID) kdl
  into #TE_kdl_hb
  from V_S_TM_Mem_Master t1
  join V_M_TM_Mem_Trade_sales t2 on t1.MemberID = t2.MemberID
  join V_M_TM_Mem_TradeDetail_sales_product t3 on t2.TradeID = t3.TradeID
 where t2.ListDateSales >= @begindate_hb
   and t2.ListDateSales <= @enddate_hb
 group by cube(t1.CustomerLevel)
 order by CustomerLevel;


--同比
if object_id(''tempdb.dbo.#TE_kdj_tb'') is not null
   drop table #TE_kdj_tb;

select isnull(t1.CustomerLevel, ''合计'') CustomerLevel,
       sum(t2.TotalMoneySales) xfe,
       --count(t2.TradeID) jyl,
       sum(t2.TotalMoneySales) / count(t2.TradeID) kdj
  into #TE_kdj_tb
  from V_S_TM_Mem_Master t1
  join V_M_TM_Mem_Trade_sales t2 on t1.MemberID = t2.MemberID
 where t2.ListDateSales >= @begindate_tb
   and t2.ListDateSales <= @enddate_tb
 group by cube(t1.CustomerLevel)
 order by CustomerLevel;

if object_id(''tempdb.dbo.#TE_kdl_tb'') is not null
   drop table #TE_kdl_tb;

select isnull(t1.CustomerLevel, ''合计'') CustomerLevel,
       --sum(t3.QuantityProduct) xfzl,
       --count(distinct t3.TradeID) jyl,
       sum(t3.QuantityProduct) / count(distinct t3.TradeID) kdl
  into #TE_kdl_tb
  from V_S_TM_Mem_Master t1
  join V_M_TM_Mem_Trade_sales t2 on t1.MemberID = t2.MemberID
  join V_M_TM_Mem_TradeDetail_sales_product t3 on t2.TradeID = t3.TradeID
 where t2.ListDateSales >= @begindate_tb
   and t2.ListDateSales <= @enddate_tb
 group by cube(t1.CustomerLevel)
 order by CustomerLevel;


--线下消费贡献率（占会员）
if object_id(''tempdb.dbo.#TE_xfe_hy_xx'') is not null
   drop table #TE_xfe_hy_xx;

select isnull(t1.CustomerLevel, ''合计'') CustomerLevel, --会员等级
       sum(t2.TotalMoneySales) xfe
  into #TE_xfe_hy_xx
  from V_S_TM_Mem_Master t1
  join V_M_TM_Mem_Trade_sales t2 on t1.MemberID = t2.MemberID
  join V_S_TM_Mem_Ext t4 on t1.MemberID = t4.MemberID and t4.CustomerSource = ''线下''
 where t2.ListDateSales >= @begindate_dq
   and t2.ListDateSales <= @enddate_dq
 group by cube(t1.CustomerLevel)
 order by CustomerLevel;

--线下消费贡献率（占总体）
if object_id(''tempdb.dbo.#TE_xfe_zt_xx'') is not null
   drop table #TE_xfe_zt_xx;

select sum(t.TotalMoneySales) as xfe
  into #TE_xfe_zt_xx
  from V_M_TM_Mem_Trade_sales t
 where t.ListDateSales >= @begindate_dq
   and t.ListDateSales <= @enddate_dq
   and t.TradeSource = ''线下'';

--insert into @Mem_Contri_Rate_Stat
select
t1.CustomerLevel as MemberLevel, --会员等级
t1.xfe as Spending, --消费额
t1.xfe / t5.xfe * 100 as SpendingTB, --消费额同比
t1.xfe / t3.xfe * 100 as SpendingHB, --消费额环比
t1.kdj as GuestUnitPrice, --客单价
t1.kdj / t5.kdj * 100 as GuestUnitPriceTB, --客单价同比
t1.kdj / t3.kdj * 100 as GuestUnitPriceHB, --客单价环比
t2.kdl as GuestUnitCount, --客单量
t2.kdl / t6.kdl * 100 as GuestUnitCountTB, --客单量同比
t2.kdl / t4.kdl * 100 as GuestUnitCountHB, --客单量环比
(t1.xfe - t3.xfe) / t3.xfe * 100 as SpendingRiseRate, --消费额增长率
t7.xfe / (select xfe from #TE_xfe_hy_xx where CustomerLevel = ''合计'') * 100 as ContributionRateMember, --线下消费贡献率（占会员）
t7.xfe / (select xfe from #TE_xfe_zt_xx) * 100 as ContributionRateTotal --线下消费贡献率（占总体）
from #TE_kdj_dq t1
join #TE_kdl_dq t2    on t1.CustomerLevel = t2.CustomerLevel
join #TE_kdj_hb t3    on t1.CustomerLevel = t3.CustomerLevel
join #TE_kdl_hb t4    on t1.CustomerLevel = t4.CustomerLevel
join #TE_kdj_tb t5    on t1.CustomerLevel = t5.CustomerLevel
join #TE_kdl_tb t6    on t1.CustomerLevel = t6.CustomerLevel
join #TE_xfe_hy_xx t7 on t1.CustomerLevel = t7.CustomerLevel;

/*
select MemberLevel,
       Spending,
       SpendingTB,
       SpendingHB,
       GuestUnitPrice,
       GuestUnitPriceTB,
       GuestUnitPriceHB,
       GuestUnitCount,
       GuestUnitCountTB,
       GuestUnitCountHB,
       SpendingRiseRate,
       ContributionRateMember,
       ContributionRateTotal
  from @Mem_Contri_Rate_Stat;
*/

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Count]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Count]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Rpt_Mem_Count]
    @dateBegin DateTime ,--开始时间
    @dateEnd   DateTime,--结束时间
    @channel    nvarchar(200),--渠道
    @area    nvarchar(100),--大区
    @city    nvarchar(100), --城市
    @store nvarchar(max),--门店
    @PageIndex int,--第几页,0开始
    @PageSize int,--一页几行
    @RecordCount int out  --总数据量
AS
/**********************************
  ----arvarto system-----
  存储过程功能描述：会员升级分析报表
  建 立 人：<hui.yang>
  建立时间：
  修改内容: 20151222  zyb 修改合计项，增加临界时间值的修改；
            20151222  zyb 会员增长率待修改后续由SP算出；
  ***********************************/


BEGIN
    if @PageIndex is null or @PageIndex = -1
    begin
    declare @tblMemQstResult table (
          City nvarchar(500),
          Channel nvarchar(500),
          ChannelCodeBase nvarchar(500),
          CityCodeStore nvarchar(500),
          AreaCodeStore nvarchar(500),
          Store nvarchar(500),
          rn int,
          StoreCode nvarchar(500),
          Will_Mem   int,
          Com_Mem   int,
          Copper_Mem   int,
          Silver_Mem   int,
          Gold_Mem   int,
          Platinum_Mem   int,
          Total_Mem   int,
          Will_Mem_New   int,
          Com_Mem_New   int,
          Copper_Mem_New   int,
          Silver_Mem_New   int,
          Gold_Mem_New   int,
          Platinum_Mem_New   int,
          Total_Mem_New   int,
          Percent_Mem   int,
          Active_Mem   int,
          WillLose_Mem   int,
          Lose_Mem   int
        )
        select * from @tblMemQstResult
        return
        end

    --大区暂时没有,条件不写 
declare 
 --@Sql1  nvarchar(max),
 -- @Sql2  nvarchar(max),
@Sql  nvarchar(max) 
declare
@Sql_Search nvarchar(max)='''',
@sqlCount nvarchar(max)='''',
@sql_temp nvarchar(max) = ''''

set @Sql_Search =''''
    if ( isnull(@channel, '''') <> '''')
    begin
        --set @Sql_Search = @Sql_Search + '' and ChannelTypeCodeStore =  ''''''+@channel+'''''' ''
        set @Sql_Search = @Sql_Search + '' and ChannelTypeCodeStore in ('' + @channel + '') ''
    end
    if ( isnull(@area, '''') <> '''')
    begin
        set @Sql_Search = @Sql_Search + '' and AreaCodeStore =  ''''''+@area+'''''' ''
    end
     
if (isnull(@store, '''') <> '''')
begin
--set @Sql_Search = @Sql_Search + '' and storecode =   ''''''+ @store + '''''' ''

if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
   begin
      drop table #TE_Rpt_StoreCode;
   end

select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
into #TE_Rpt_StoreCode;

if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
   begin
      drop table #TE_Rpt_StoreCode2;
   end

select replace(b.StoreCode, '''''''', '''') as StoreCode
into #TE_Rpt_StoreCode2
 from #TE_Rpt_StoreCode a
outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;

set @Sql_Search = @Sql_Search + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = V_M_TM_SYS_BaseData_store.StoreCode) '';

end

    if (isnull(@city, '''') <> '''')
    begin
        set @Sql_Search = @Sql_Search + '' and CityCodeStore =   ''''''+ @city + '''''' ''    
    end

    set @Sql=''
	select 
  City                       ,    
 Channel                    ,    
 ChannelCodeBase            ,    
 CityCodeStore              ,    
 AreaCodeStore              ,    
 Store                      ,    
 Row_number() OVER ( ORDER BY StoreCode desc ) rn,                          
 StoreCode                  ,    
 Will_Mem                   ,    
 Com_Mem                    ,    
 Copper_Mem                 ,    
 Silver_Mem                 ,    
 Gold_Mem                   ,    
 Platinum_Mem               ,    
 Total_Mem                  ,    
 Will_Mem_New               ,    
 Com_Mem_New                ,    
 Copper_Mem_New             ,    
 Silver_Mem_New             ,    
 Gold_Mem_New               ,    
 Platinum_Mem_New           ,    
 Total_Mem_New              ,    
 Percent_Mem                ,    
 Active_Mem                 ,    
 WillLose_Mem               ,    
 Lose_Mem                        

 from (
SELECT CityStore City,ChannelNameBase Channel,ChannelCodeBase,CityCodeStore,AreaCodeStore,StoreName Store,
---Row_number() OVER ( ORDER BY RegisterStoreCode ) rn1,
RegisterStoreCode AS StoreCode, 0 AS Will_Mem, 
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0  END) AS Com_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem,
0 AS Will_Mem_New,
Sum(CASE WHEN RegisterDate >=''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+''''''AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0 END) AS Com_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem_New,
0 as Percent_Mem,
Sum(CASE WHEN day1 = 3 THEN 1 ELSE 0 END)AS Active_Mem,
Sum(CASE WHEN day1 = 12 THEN 1 ELSE 0 END)AS WillLose_Mem,
Sum(CASE WHEN day1 > 12 THEN 1 ELSE 0 END)AS Lose_Mem
 FROM (select CityStore ,ChannelTypeCodeStore,CityCodeStore,AreaCodeStore,StoreName,StoreCode from V_M_TM_SYS_BaseData_store with(nolock)
     where 1=1 ''+@Sql_Search+'' )a 
 left join V_M_TM_SYS_BaseData_channel e on e.ChannelCodeBase = a.ChannelTypeCodeStore
 left join (
 select RegisterStoreCode,RegisterDate,CustomerLevel,c.MemberID,day1 from V_S_TM_Mem_Ext  c  with (nolock)
 inner JOIN V_S_TM_Mem_Master b with (nolock) ON c.MemberID = b.MemberID
 left join(
SELECT TradeID,MemberID,ListDateSales,Datediff(mm, ListDateSales, Getdate()) as day1 FROM (
       SELECT TradeID,MemberID,ListDateSales, Row_number()OVER (partition BY MemberId ORDER BY ListDateSales desc) rn FROM 
       V_M_TM_Mem_Trade_sales with(nolock)) mm
 WHERE  rn = 1) sales
 on sales.MemberID=c.MemberID) basedata
 on a.StoreCode=basedata.RegisterStoreCode
 group by CityStore,ChannelNameBase,ChannelCodeBase,CityCodeStore,RegisterStoreCode,AreaCodeStore,StoreName
 union all
 
SELECT '''''''' City,''''总计'''' Channel,''''''''ChannelCodeBase,''''''''CityCodeStore,''''''''AreaCodeStore,'''''''' Store,
''''''''  StoreCode, 0 AS Will_Mem, 
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0  END) AS Com_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem,
0 AS Will_Mem_New,
Sum(CASE WHEN RegisterDate >=''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+''''''AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0 END) AS Com_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem_New,
0 as Percent_Mem,
Sum(CASE WHEN day1 = 3 THEN 1 ELSE 0 END)AS Active_Mem,
Sum(CASE WHEN day1 = 12 THEN 1 ELSE 0 END)AS WillLose_Mem,
Sum(CASE WHEN day1 > 12 THEN 1 ELSE 0 END)AS Lose_Mem
 FROM (select CityStore ,ChannelTypeCodeStore,CityCodeStore,AreaCodeStore,StoreName,StoreCode from V_M_TM_SYS_BaseData_store with(nolock)
     where 1=1 ''+@Sql_Search+'' )a 
 left join V_M_TM_SYS_BaseData_channel e on e.ChannelCodeBase = a.ChannelTypeCodeStore
 left join (
 select RegisterStoreCode,RegisterDate,CustomerLevel,c.MemberID,day1 from V_S_TM_Mem_Ext  c  with (nolock)
 inner JOIN V_S_TM_Mem_Master b with (nolock) ON c.MemberID = b.MemberID
 left join(
SELECT TradeID,MemberID,ListDateSales,Datediff(mm, ListDateSales, Getdate()) as day1 FROM (
       SELECT TradeID,MemberID,ListDateSales, Row_number()OVER (partition BY MemberId ORDER BY ListDateSales desc) rn FROM 
       V_M_TM_Mem_Trade_sales with(nolock)) mm
 WHERE  rn = 1) sales
 on sales.MemberID=c.MemberID) basedata
 on a.StoreCode=basedata.RegisterStoreCode ) t 

 ''
 


---	set @Sql=@sql1+@sql2 

    set @sqlCount = ''select @ct = count(rn) from( '' + @Sql +'')T''
    exec sp_executesql @sqlCount,N''@ct int output'',@RecordCount output 
    set @Sql=''select * from(''+@Sql+'')as RowId where rn between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize) +''''
     print @Sql

    insert into @tblMemQstResult   exec (@Sql)
    select *   from @tblMemQstResult
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Count_test]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Count_test]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Rpt_Mem_Count_test]
    @dateBegin DateTime ,--开始时间
    @dateEnd   DateTime,--结束时间
    @channel    nvarchar(200),--渠道
    @area    nvarchar(100),--大区
    @city    nvarchar(100), --城市
    @store nvarchar(max),--门店
    @PageIndex int,--第几页,0开始
    @PageSize int,--一页几行
    @RecordCount int out  --总数据量
AS
/**********************************
  ----arvarto system-----
  存储过程功能描述：会员升级分析报表
  建 立 人：<hui.yang>
  建立时间：
  修改内容: 20151222  zyb 修改合计项，增加临界时间值的修改；
  ***********************************/


BEGIN
    if @PageIndex is null or @PageIndex = -1
    begin
    declare @tblMemQstResult table (
          City nvarchar(500),
          Channel nvarchar(500),
          ChannelCodeBase nvarchar(500),
          CityCodeStore nvarchar(500),
          AreaCodeStore nvarchar(500),
          Store nvarchar(500),
          rn int,
          StoreCode nvarchar(500),
          Will_Mem   int,
          Com_Mem   int,
          Copper_Mem   int,
          Silver_Mem   int,
          Gold_Mem   int,
          Platinum_Mem   int,
          Total_Mem   int,
          Will_Mem_New   int,
          Com_Mem_New   int,
          Copper_Mem_New   int,
          Silver_Mem_New   int,
          Gold_Mem_New   int,
          Platinum_Mem_New   int,
          Total_Mem_New   int,
          Percent_Mem   int,
          Active_Mem   int,
          WillLose_Mem   int,
          Lose_Mem   int
        )
        select * from @tblMemQstResult
        return
        end

    --大区暂时没有,条件不写 
declare 
 --@Sql1  nvarchar(max),
 -- @Sql2  nvarchar(max),
@Sql  nvarchar(max) 
declare
@Sql_Search nvarchar(max)='''',
@sqlCount nvarchar(max)='''',
@sql_temp nvarchar(max) = ''''

set @Sql_Search =''''
    if ( isnull(@channel, '''') <> '''')
    begin
        --set @Sql_Search = @Sql_Search + '' and ChannelTypeCodeStore =  ''''''+@channel+'''''' ''
        set @Sql_Search = @Sql_Search + '' and ChannelTypeCodeStore in ('' + @channel + '') ''
    end
    if ( isnull(@area, '''') <> '''')
    begin
        set @Sql_Search = @Sql_Search + '' and AreaCodeStore =  ''''''+@area+'''''' ''
    end
     
if (isnull(@store, '''') <> '''')
begin
--set @Sql_Search = @Sql_Search + '' and storecode =   ''''''+ @store + '''''' ''

if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
   begin
      drop table #TE_Rpt_StoreCode;
   end

select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
into #TE_Rpt_StoreCode;

if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
   begin
      drop table #TE_Rpt_StoreCode2;
   end

select replace(b.StoreCode, '''''''', '''') as StoreCode
into #TE_Rpt_StoreCode2
 from #TE_Rpt_StoreCode a
outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;

set @Sql_Search = @Sql_Search + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = V_M_TM_SYS_BaseData_store.StoreCode) '';

end

    if (isnull(@city, '''') <> '''')
    begin
        set @Sql_Search = @Sql_Search + '' and CityCodeStore =   ''''''+ @city + '''''' ''    
    end

    set @Sql=''

SELECT CityStore City,ChannelNameBase Channel,ChannelCodeBase,CityCodeStore,AreaCodeStore,StoreName Store,
Row_number() OVER ( ORDER BY RegisterStoreCode ) rn,RegisterStoreCode AS StoreCode, 0 AS Will_Mem, 
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0  END) AS Com_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem,
0 AS Will_Mem_New,
Sum(CASE WHEN RegisterDate >=''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+''''''AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0 END) AS Com_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem_New,
0 as Percent_Mem,
Sum(CASE WHEN day1 = 3 THEN 1 ELSE 0 END)AS Active_Mem,
Sum(CASE WHEN day1 = 12 THEN 1 ELSE 0 END)AS WillLose_Mem,
Sum(CASE WHEN day1 > 12 THEN 1 ELSE 0 END)AS Lose_Mem
 FROM (select CityStore ,ChannelTypeCodeStore,CityCodeStore,AreaCodeStore,StoreName,StoreCode from V_M_TM_SYS_BaseData_store with(nolock)
     where 1=1 ''+@Sql_Search+'' )a 
 left join V_M_TM_SYS_BaseData_channel e on e.ChannelCodeBase = a.ChannelTypeCodeStore
 left join (
 select RegisterStoreCode,RegisterDate,CustomerLevel,c.MemberID,day1 from V_S_TM_Mem_Ext  c  with (nolock)
 inner JOIN V_S_TM_Mem_Master b with (nolock) ON c.MemberID = b.MemberID
 left join(
SELECT TradeID,MemberID,ListDateSales,Datediff(mm, ListDateSales, Getdate()) as day1 FROM (
       SELECT TradeID,MemberID,ListDateSales, Row_number()OVER (partition BY MemberId ORDER BY ListDateSales desc) rn FROM 
       V_M_TM_Mem_Trade_sales with(nolock)) mm
 WHERE  rn = 1) sales
 on sales.MemberID=c.MemberID) basedata
 on a.StoreCode=basedata.RegisterStoreCode
 group by CityStore,ChannelNameBase,ChannelCodeBase,CityCodeStore,RegisterStoreCode,AreaCodeStore,StoreName
 union all
 
SELECT '''''''' City,''''总计'''' Channel,''''''''ChannelCodeBase,''''''''CityCodeStore,''''''''AreaCodeStore,'''''''' Store,
100000000 rn,''''''''  StoreCode, 0 AS Will_Mem, 
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0  END) AS Com_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem,
Sum(CASE WHEN RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem,
0 AS Will_Mem_New,
Sum(CASE WHEN RegisterDate >=''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+''''''AND CustomerLevel = ''''Normal'''' THEN 1 ELSE 0 END) AS Com_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Copper'''' THEN 1 ELSE 0 END) AS Copper_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Silver'''' THEN 1 ELSE 0 END) AS Silver_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Gold'''' THEN 1 ELSE 0 END) AS Gold_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' AND CustomerLevel = ''''Platinum'''' THEN 1 ELSE 0 END) AS Platinum_Mem_New,
Sum(CASE WHEN RegisterDate >= ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND RegisterDate < ''''''+ convert( NVARCHAR(20),dateadd(day,1,@dateEnd),120)+'''''' THEN 1 ELSE 0 END) AS Total_Mem_New,
0 as Percent_Mem,
Sum(CASE WHEN day1 = 3 THEN 1 ELSE 0 END)AS Active_Mem,
Sum(CASE WHEN day1 = 12 THEN 1 ELSE 0 END)AS WillLose_Mem,
Sum(CASE WHEN day1 > 12 THEN 1 ELSE 0 END)AS Lose_Mem
 FROM (select CityStore ,ChannelTypeCodeStore,CityCodeStore,AreaCodeStore,StoreName,StoreCode from V_M_TM_SYS_BaseData_store with(nolock)
     where 1=1 ''+@Sql_Search+'' )a 
 left join V_M_TM_SYS_BaseData_channel e on e.ChannelCodeBase = a.ChannelTypeCodeStore
 left join (
 select RegisterStoreCode,RegisterDate,CustomerLevel,c.MemberID,day1 from V_S_TM_Mem_Ext  c  with (nolock)
 inner JOIN V_S_TM_Mem_Master b with (nolock) ON c.MemberID = b.MemberID
 left join(
SELECT TradeID,MemberID,ListDateSales,Datediff(mm, ListDateSales, Getdate()) as day1 FROM (
       SELECT TradeID,MemberID,ListDateSales, Row_number()OVER (partition BY MemberId ORDER BY ListDateSales desc) rn FROM 
       V_M_TM_Mem_Trade_sales with(nolock)) mm
 WHERE  rn = 1) sales
 on sales.MemberID=c.MemberID) basedata
 on a.StoreCode=basedata.RegisterStoreCode





 ''
 


---	set @Sql=@sql1+@sql2 

    set @sqlCount = ''select @ct = count(rn) from( '' + @Sql +'')T''
    exec sp_executesql @sqlCount,N''@ct int output'',@RecordCount output 
    set @Sql=''select * from(''+@Sql+'')as RowId where rn between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize) +''''
     print @Sql

    insert into @tblMemQstResult   exec (@Sql)
    select *   from @tblMemQstResult
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_IssuingConsumption_Result]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_IssuingConsumption_Result]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE  PROCEDURE  [dbo].[sp_Rpt_Mem_IssuingConsumption_Result]
	@dateBegin DateTime ,
    @dateEnd   DateTime,
	@channel nvarchar(200),
	@area  nvarchar(100),
	@city	nvarchar(100), 
	@store nvarchar(max) ,
	@PageIndex int,
    @PageSize int,
	@RecordCount int out
AS
BEGIN
declare @IssuingConsumption table(
	      rowIndex int,
          Channel nvarchar(100),
          City nvarchar(100),
          Store  nvarchar(100),
          Com_Mem_Get int,
          Copper_Mem_Get int,
          Silver_Mem_Get int,
          Gold_Mem_Get int,
          Platinum_Mem_Get int,
          Total_Mem_Get int,
          Com_Mem_Cost int,
          Copper_Mem_Cost int,
          Silver_Mem_Cost int,
          Gold_Mem_Cost int,
          Platinum_Mem_Cost int,
          Total_Mem_Cost int,
          Com_Mem_Left int,
          Copper_Mem_Left int,
          Silver_Mem_Left int,
          Gold_Mem_Left int,
          Platinum_Mem_Left int,
          Total_Mem_Left int
)
declare @Sql  varchar(max)='''',@SqlCount nvarchar(max)='''',
   @SqlSearchChange varchar(max)='''',@SqlSearchValue varchar(max)='''',@SqlSearch nvarchar(max)=''''
   if (ISNULL(@area,'''')<>'''')
   begin 
   set @SqlSearch = @SqlSearch+ '' and AreaCodeStore in(''''''+@area+'''''')''
   end 
   
   if (ISNULL(@city,'''')<>'''')
   begin 
   set @SqlSearch = @SqlSearch+ '' and CityCodeStore in(''''''+@city+'''''')''
   end 
   
   if (ISNULL(@store,'''')<>'''')
   begin 
   --set @SqlSearch = @SqlSearch+ '' and storecode in(''''''+@store+'''''')''

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
      begin
         drop table #TE_Rpt_StoreCode;
      end

   select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
   into #TE_Rpt_StoreCode;
   
   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
      begin
         drop table #TE_Rpt_StoreCode2;
      end
   
   select replace(b.StoreCode, '''''''', '''') as StoreCode
   into #TE_Rpt_StoreCode2
    from #TE_Rpt_StoreCode a
   outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;
   
   set @SqlSearch = @SqlSearch + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = V_M_TM_SYS_BaseData_store.StoreCode) '';


   end 

   if(isnull(@dateBegin,'''')<>'''')
   begin
   set @SqlSearchChange=@SqlSearchChange+'' and addeddate>=''''''+convert(varchar(20), @dateBegin,120)+''''''''  
   set @SqlSearchValue=@SqlSearchValue+'' and SpecialDate1>=''''''+convert(varchar(20),@dateBegin,120)+''''''''

   end
   if(isnull(@dateEnd,'''')<>'''')
   begin
   set @SqlSearchChange=@SqlSearchChange+'' and addeddate<''''''+convert(varchar(20),DATEADD(day,1, @dateEnd))+''''''''
     set @SqlSearchValue=@SqlSearchValue+'' and SpecialDate2<''''''+convert(varchar(20),DATEADD(day,1, @dateEnd))+''''''''
   end

set @Sql= ''select ROW_NUMBER() over(order by channel) as rowIndex, channel as  Channel ,city ,StoreName as store,
	sum( case when MemberLevel= ''''Normal'''' then changeget else 0 end) as  Com_Mem_Get,
	sum( case when MemberLevel= ''''Copper'''' then changeget else 0 end) as  Copper_Mem_Get ,
    sum( case when MemberLevel= ''''Silver'''' then changeget else 0 end) as  Silver_Mem_Get ,
	sum( case when MemberLevel= ''''Gold'''' then changeget else 0 end)  as   Gold_Mem_Get ,
    sum( case when MemberLevel= ''''Platinum'''' then changeget else 0 end)  as Platinum_Mem_Get ,
    sum(changeget) as Total_Mem_Get ,
    sum( case when MemberLevel= ''''Normal'''' then changecost else 0 end) as  Com_Mem_Cost ,
    sum( case when MemberLevel= ''''Copper'''' then changecost else 0 end) as  Copper_Mem_Cost ,
    sum( case when MemberLevel= ''''Silver'''' then changecost else 0 end) as  Silver_Mem_Cost ,
    sum( case when MemberLevel= ''''Gold'''' then changecost else 0 end)   as Gold_Mem_Cost ,
    sum( case when MemberLevel= ''''Platinum'''' then changecost else 0 end) as Platinum_Mem_Cost ,
    sum(changecost) as  Total_Mem_Cost,
    sum( case when MemberLevel= ''''Normal'''' then acccanuse else 0 end) as  Com_Mem_Left ,
    sum( case when MemberLevel= ''''Copper'''' then acccanuse else 0 end) as  Copper_Mem_Left ,
    sum( case when MemberLevel= ''''Silver'''' then acccanuse else 0 end) as  Silver_Mem_Left ,
    sum( case when MemberLevel= ''''Gold'''' then acccanuse else 0 end)  as  Gold_Mem_Left ,
    sum( case when MemberLevel= ''''Platinum'''' then acccanuse else 0 end)  as Platinum_Mem_Left ,
    sum(acccanuse) as   Total_Mem_Left 
 from
       (select StoreCode,StoreName,CityStore as city,ChannerTypeNameStore as channel,AreaNameStore as area, acc.AccountID,AccountType,memmaster.MemberID,Value1,chancost.changecost,chanGet.changeget,acccanuse, MemberLevel,memExt.Str_Attr_5 as stroeCode
	   from TM_Mem_Master memmaster with(nolock)
inner join 
	 TM_Mem_Ext memExt with(nolock) on memmaster.MemberID=memExt.MemberID
right join   
		(select AccountID,AccountType,MemberID,Value1 from TM_Mem_Account with(nolock) where AccountType=3) acc
		 on acc.MemberID=memmaster.MemberID
left join
       (select AccountID,sum(DetailValue) as acccanuse from TM_Mem_AccountDetail with(nolock) where AccountDetailType=''''value1'''' ''+@SqlSearchValue+'' group by AccountID) canuse
	   on acc.AccountID=canuse.AccountID
left join (
			select AccountID,sum(ChangeValue) as changeget from TL_Mem_AccountChange with(nolock) where changevalue>0 and HasReverse=0 ''+@SqlSearchChange+'' group by AccountID) chanGet
			on acc.AccountID =chanGet.AccountID
left join (
			select AccountID,sum(ChangeValue) as changecost from TL_Mem_AccountChange with(nolock) where changevalue<0 and HasReverse=0 ''+@SqlSearchChange+'' group by AccountID)
			chancost on acc.AccountID=chancost.AccountID
left join (select StoreCode,StoreName,CityStore ,ChannerTypeNameStore ,AreaNameStore  from V_M_TM_SYS_BaseData_store with(nolock) where 1=1 ''+@SqlSearch+'')  store  on store.StoreCode=memExt.Str_Attr_5)
as tbResult
group by city ,channel,StoreName ''

set @SqlCount = ''select @ct = count(rowIndex) from( '' + @Sql +'')T''
	exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 
	set @Sql=''select * from(''+@Sql+'')as RowId where rowIndex between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize) +''''
     print @Sql

	insert into @IssuingConsumption   exec (@Sql)
	select  Channel, City, Store , Com_Mem_Get,Copper_Mem_Get,Silver_Mem_Get, Gold_Mem_Get,Platinum_Mem_Get,
          Total_Mem_Get, Com_Mem_Cost,Copper_Mem_Cost, Silver_Mem_Cost,Gold_Mem_Cost,Platinum_Mem_Cost,
          Total_Mem_Cost, Com_Mem_Left , Copper_Mem_Left , Silver_Mem_Left , Gold_Mem_Left , Platinum_Mem_Left , Total_Mem_Left 
		    from @IssuingConsumption

END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_MemToNonMemSalesDutyCount]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_MemToNonMemSalesDutyCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[sp_Rpt_Mem_MemToNonMemSalesDutyCount]
    @dateConsumptionStart	DateTime,--查询消费起日
	@dateConsumptionEnd	DateTime, --查询消费止日
	@customSource nvarchar(20)--消费来源
	as 
	begin 
	declare @tableSalesDutyCount table(
	Type_Mem nvarchar(20),
	Expendture_Mem varchar(20),
	ExpendtureSum_Mem varchar(20),
	ConsumptionContribution_Mem varchar(20),
	ConsumptionContributionSum_Mem varchar(20)
	)

	

	declare @Sql  nvarchar(max)='''',	 @SqlSearch nvarchar(max)='''',@sqlCustomSource nvarchar(300)='''',
	@SqlSearchNonMem nvarchar(max)='''',
 	@NonMemberSpend decimal(18,2), @MemberSpend decimal(18,2),
	@NonMemberSpendAll decimal(18,2),@MemberSpendAll decimal(18,2),
	@NonMemberSpendRate decimal(18,2),@MemberSpendRate decimal(18,2),
	@NonMemberSpendAllRate decimal(18,2),@MemberSpendAllRate decimal(18,2)
   if(isnull(@customSource,'''')<>'''')
   begin
      set @sqlCustomSource=@sqlCustomSource+'' and Str_Attr_53=''''''+@customSource+'''''' ''
   end

	if(ISNULL(@dateConsumptionStart,'''')<>'''')
	begin
	 set @SqlSearch=@SqlSearch+'' and AddedDate>='''''' +convert(nvarchar(20),@dateConsumptionStart,120)+'''''' ''
     set @SqlSearchNonMem=@SqlSearchNonMem+'' and BusinessDate>='''''' +convert(nvarchar(20),@dateConsumptionStart,120)+'''''' ''
	 
	end
	if(ISNULL(@dateConsumptionEnd,'''')<>'''')
	begin
	 set @SqlSearch=@SqlSearch+'' and AddedDate<'''''' +convert(nvarchar(20),dateadd(day,1,@dateConsumptionEnd),120)+'''''' ''
	  set @SqlSearchNonMem=@SqlSearchNonMem+'' and BusinessDate<'''''' +convert(nvarchar(20),dateadd(day,1,@dateConsumptionEnd),120)+'''''' ''
	end
	 
	 declare  @sqlCount nvarchar(max)
	    set @sqlCount='' select @ct= isnull(sum(sumamount),0) from (select MemberID from TM_Mem_Ext with(nolock) where 1=1''+@sqlCustomSource+'') ext
                inner join (select sum(amount) as sumamount,MemberID from   V_M_TM_Mem_Trade_sales with(nolock) where 1=1 '' +@SqlSearch+''
                group by MemberID) sales on ext.MemberID=sales.MemberID''

    exec sp_executesql @sqlCount,N''@ct int output'',@MemberSpend output 
 
		set @sqlCount = ''select @ct =sum(amount)  from TD_POS_ConsumeBill with(nolock)  where 1=1'' +@SqlSearchNonMem
    exec sp_executesql @sqlCount,N''@ct int output'',@NonMemberSpend output 

	  set @sqlCount='' select @ct= isnull(sum(sumamount),0) from (select MemberID from TM_Mem_Ext with(nolock) where 1=1''+@sqlCustomSource+'') ext
                inner join (select sum(amount) as sumamount,MemberID from   V_M_TM_Mem_Trade_sales with(nolock) where 1=1 
                group by MemberID) sales on ext.MemberID=sales.MemberID''

     exec sp_executesql @sqlCount,N''@ct int output'',@MemberSpendAll output 
	print @MemberSpendAll
	 set @NonMemberSpendAll= (select sum(amount) from TD_POS_ConsumeBill with(nolock))
	 if @MemberSpend+@NonMemberSpend>0

	 begin
	 set @NonMemberSpendRate= @NonMemberSpend/(@MemberSpend+@NonMemberSpend)
	 set @MemberSpendRate=  @MemberSpend/(select(@MemberSpend+@NonMemberSpend))
	 end
	 if @NonMemberSpendAll+@MemberSpendAll>0
	 begin
	 set @NonMemberSpendAllRate=@NonMemberSpendAll/(select(@NonMemberSpendAll+@MemberSpendAll))
	 set @MemberSpendAllRate=@MemberSpendAll/(select(@NonMemberSpendAll+@MemberSpendAll)) 
	 end
insert into @tableSalesDutyCount
	select ''非会员''  as Type_Mem,cast(@NonMemberSpend as varchar) as Expendture_Mem,cast(@NonMemberSpendAll as varchar) as ExpendtureSum_Mem,
    cast(@NonMemberSpendRate as varchar) as ConsumptionContribution_Mem,cast(@NonMemberSpendAllRate as varchar)as ConsumptionContributionSum_Mem
	union all
	select ''会员''  as Type_Mem,cast(@MemberSpend as varchar) as Expendture_Mem,cast(@MemberSpendAll as varchar) as ExpendtureSum_Mem,
    cast(@MemberSpendRate  as varchar)as ConsumptionContribution_Mem,cast(@MemberSpendAllRate as varchar) as ConsumptionContributionSum_Mem
   
	select * from  @tableSalesDutyCount
	end' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Recruit_Count]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Recruit_Count]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_Rpt_Mem_Recruit_Count]
    @dateRegType nvarchar(100) ,--注册类型:日 0,月1
    @dateRegDate  DateTime,--注册日期
    @channel    nvarchar(200),--渠道
    @area    nvarchar(100),--大区
    @city    nvarchar(100), --城市
    @store nvarchar(max), --门店
    @PageIndex int,--当前页
    @PageSize int,--总页数
    @RecordCount int out    --总数
AS
/**********************************
  ----arvarto system-----
  存储过程功能描述：会员招募率统计
  建 立 人：<hui.yang>
  建立时间：
  修改内容: 20151222  zyb 修改合计项，增加临界时间值的修改；修改注册时间取Date_Attr_1 (RegisterDate)
            20151222  zyb 所有SQL代码重新编写；
            20151224  zyb 更改@dateRegType及@dateRegDate输入参数类型及名称
  ***********************************/

BEGIN
     declare @tabRecruitCount table
     (
        rowId int,
        StoreCode nvarchar(100),
        Channel nvarchar(100),
        City nvarchar(100),
        Store nvarchar(100),
        --RecruitTarget_Mem nvarchar(20),--招募目标
        Actual_Recruitnum_Mem int,--实际招募人数
        ---Completion_rate_Mem nvarchar(20),--decimal(18, 2),完成率
        Same_Increase_Mem nvarchar(20),--环比(当前月与上一个月比)
        Lastyear_Increase_Mem nvarchar(20),--同比(当前月与去年的这个月比)
        Area_ratio_Mem nvarchar(20) ---区域占比( 区域占比 = 店铺新增注册会员实际数据/全国新增注册会员实际数据)
        --MemThisCount int,--当前这个时间的总会员
        --MemLastYearThisCount int,--上一年的总会员
        --MemLastThisCount int,--上一个月的总会员
        --MemAllCount int
     )
	-- select * from @tabRecruitCount
declare @Sql varchar(max),
        @sqlCount nvarchar(max),
        @search1 varchar(max)='''',
        @search_theperiod nvarchar(max),
        @search_parallel nvarchar(max),
        @search_lastperiod nvarchar(max) 


 begin
   if (ISNULL(@channel,'''')<>'''')
   begin 
   --set @search1 = @search1+ '' and v.ChannelTypeCodeStore=''''''+@channel+''''''''
   set @search1 = @search1+ '' and c.ChannelTypeCodeStore in (''+@channel+'') ''
   end 
   if (ISNULL(@area,'''')<>'''')
   begin 
   set @search1 = @search1+ '' and b.AreaCodeStore= ''''''+@area+''''''''
   end 
   if (ISNULL(@city,'''')<>'''')
   begin 
   set @search1 = @search1+ '' and b.CityCodeStore= ''''''+@city+''''''''
   end 
   if (ISNULL(@store,'''')<>'''')
   begin 
   --set @search1 = @search1+ '' and v.StoreCode= ''''''+@store+''''''''

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
      begin
         drop table #TE_Rpt_StoreCode;
      end

   select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
   into #TE_Rpt_StoreCode;

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
      begin
         drop table #TE_Rpt_StoreCode2;
      end

   select replace(b.StoreCode, '''''''', '''') as StoreCode
   into #TE_Rpt_StoreCode2
    from #TE_Rpt_StoreCode a
   outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;

   set @search1 = @search1 + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = b.StoreCode) '';



   end 
   if (ISNULL(@dateRegType,'''')=''0'')
   begin 
   set @search_theperiod='' CONVERT(varchar(10),RegisterDate ,21)= CONVERT(varchar(10),''''''+convert(NVARCHAR(10),@dateRegDate,120)+'''''' ,21)''
   set @search_parallel=''  CONVERT(varchar(10),RegisterDate ,21)= CONVERT(varchar(10),DATEADD(YEAR,-1,''''''+convert(NVARCHAR(10),@dateRegDate,120)+'''''') ,21)''
   set @search_lastperiod='' CONVERT(varchar(10),RegisterDate ,21)= CONVERT(varchar(10),DATEADD(MONTH,-1,''''''+convert(NVARCHAR(10),@dateRegDate,120)+''''''),21)''

   end 
    if (ISNULL(@dateRegType,'''')=''1'')
   begin 
  -- set @search2 = @search2+ '' and convert(varchar(7),RegisterDate,21) = ''''''+convert(varchar(7),@dateRegMon,21)+''''''''
  -- set @colshow=''
        --(select COUNT(*) from TM_Mem_Ext where CONVERT(varchar(7),Date_Attr_1 ,21)= CONVERT(varchar(7),''''''+convert(varchar(10),@dateRegMon,21)+'''''' ,21) and Str_Attr_5=tab.StoreCode ) as MemThisCount,
        --(select COUNT(*) from TM_Mem_Ext where CONVERT(varchar(7),Date_Attr_1 ,21)= CONVERT(varchar(7),DATEADD(YEAR,-1,''''''+convert(varchar(10),@dateRegMon,21)+''''''),21) and Str_Attr_5=tab.StoreCode ) as MemLastYearThisCount,
        --(select COUNT(*) from TM_Mem_Ext where CONVERT(varchar(7),Date_Attr_1 ,21)= CONVERT(varchar(7),DATEADD(MONTH,-1,''''''+convert(varchar(10),@dateRegMon,21)+''''''),21) and Str_Attr_5=tab.StoreCode ) as MemLastThisCount
  -- ''

   set @search_theperiod='' CONVERT(varchar(7),RegisterDate ,21)= CONVERT(varchar(7),''''''+convert(NVARCHAR(10),@dateRegDate,120)+'''''' ,21)''
   set @search_parallel='' CONVERT(varchar(7),RegisterDate ,21)= CONVERT(varchar(7),DATEADD(YEAR,-1,''''''+convert(NVARCHAR(10),@dateRegDate,120)+'''''') ,21)''
   set @search_lastperiod='' CONVERT(varchar(7),RegisterDate ,21)= CONVERT(varchar(7),DATEADD(MONTH,-1,''''''+convert(NVARCHAR(10),@dateRegDate,120)+''''''),21)''



   end 
   end




set @sql=''
 select ROW_NUMBER() OVER (ORDER BY  StoreCode desc) as rowId, *
 from (
select b.StoreCode,channelnamebase, citystore,storename ,
       sum(case when ''+@search_theperiod+''  then 1 else 0 end )  Actual_Recruitnum_Mem,    --招募人数
       case when sum(case when ''+@search_lastperiod+''  then 1 else 0 end )=0 then 0 else 
       round(cast( (sum(case when ''+@search_theperiod+''  then 1 else 0 end )- sum(case when ''+@search_lastperiod+''  then 1 else 0 end )  ) as float)/
         sum(case when ''+@search_lastperiod+''  then 1 else 0 end ) ,4)  end   Same_Increase_Mem,---环比 增幅

        case when sum(case when ''+@search_parallel+''  then 1 else 0 end )=0 then 0 else       
       round(cast( (sum(case when ''+@search_theperiod+''  then 1 else 0 end )- sum(case when ''+@search_parallel+''  then 1 else 0 end )  ) as float)/
         sum(case when ''+@search_parallel+''  then 1 else 0 end ) ,4) end   Lastyear_Increase_Mem,  ---同比增幅

        case when      (select count(*)   ---全国所有新增会员
         from V_S_TM_Mem_Ext 
         where 1=1 and ''+@search_theperiod+'')  =0 then 0 else 
       round( cast(sum(case when ''+@search_theperiod+''  then 1 else 0 end ) as float) /
        (select count(*)   ---全国所有新增会员
         from V_S_TM_Mem_Ext 
         where 1=1 and ''+@search_theperiod+'')  ,4)  end   Area_ratio_Mem  --区域占比

from V_S_TM_Mem_Ext a with(nolock)  
left join  V_M_TM_SYS_BaseData_store b with(nolock)  on a.RegisterStoreCode=b.StoreCode
left join  V_M_TM_SYS_BaseData_channel c  with(nolock) on  b.ChannelTypeCodeStore=c.ChannelCodeBase
where 1=1  ''+@search1+''
group by StoreCode,channelnamebase, citystore,storename 
union all

select  '''''''' StoreCode,''''总计'''' channelnamebase, '''''''' citystore,'''''''' storename ,
       sum(case when ''+@search_theperiod+''  then 1 else 0 end )  Actual_Recruitnum_Mem,    --招募人数
       case when sum(case when ''+@search_lastperiod+''  then 1 else 0 end )=0 then 0 else 
       round(cast( (sum(case when ''+@search_theperiod+''  then 1 else 0 end )- sum(case when ''+@search_lastperiod+''  then 1 else 0 end )  ) as float)/
         sum(case when ''+@search_lastperiod+''  then 1 else 0 end ) ,4)  end   Same_Increase_Mem,---环比 增幅

        case when sum(case when ''+@search_parallel+''  then 1 else 0 end )=0 then 0 else       
       round(cast( (sum(case when ''+@search_theperiod+''  then 1 else 0 end )- sum(case when ''+@search_parallel+''  then 1 else 0 end )  ) as float)/
         sum(case when ''+@search_parallel+''  then 1 else 0 end ) ,4) end   Lastyear_Increase_Mem,  ---同比增幅

        case when      (select count(*)   ---全国所有新增会员
         from V_S_TM_Mem_Ext 
         where 1=1 and ''+@search_theperiod+'')  =0 then 0 else 
       round( cast(sum(case when ''+@search_theperiod+''  then 1 else 0 end ) as float) /
        (select count(*)   ---全国所有新增会员
         from V_S_TM_Mem_Ext 
         where 1=1 and ''+@search_theperiod+'')  ,4)  end   Area_ratio_Mem  --区域占比

from V_S_TM_Mem_Ext a with(nolock)  
left join  V_M_TM_SYS_BaseData_store b with(nolock)  on a.RegisterStoreCode=b.StoreCode
left join  V_M_TM_SYS_BaseData_channel c  with(nolock) on  b.ChannelTypeCodeStore=c.ChannelCodeBase
where 1=1  ''+@search1+''




) t ''


--select * from V_M_TM_SYS_BaseData_store

--select * from V_M_TM_SYS_BaseData_channel


        set @sqlCount = ''select @ct = count(rowId) from( '' + @Sql +'')T''
        exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 

        set @sql=''select * from (''+@sql+'') as tab 
        where rowId between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize)+''''

          print @Sql 

       insert into @tabRecruitCount exec (@Sql)
       select * from @tabRecruitCount
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Mem_Recruit_Count_test]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Mem_Recruit_Count_test]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Rpt_Mem_Recruit_Count_test]
	@dateReg DateTime ,--注册日
    @dateRegMon  DateTime,--注册月
	@channel	nvarchar(200),--渠道
	@area	nvarchar(100),--大区
	@city	nvarchar(100), --城市
	@store nvarchar(max), --门店
    @PageIndex int,--当前页
    @PageSize int,--总页数
	@RecordCount int out	--总数
AS
/**********************************
  ----arvarto system-----
  存储过程功能描述：会员招募率统计
  建 立 人：<hui.yang>
  建立时间：
  修改内容: 20151222  zyb 修改合计项，增加临界时间值的修改；修改注册时间取Date_Attr_1 (RegisterDate)
            20151222  zyb 所有SQL代码重新编写；
  ***********************************/

BEGIN
	 declare @tabRecruitCount table
	 (
		rowId int,
		StoreCode nvarchar(100),
		Channel nvarchar(100),
		City nvarchar(100),
		Store nvarchar(100),
		--RecruitTarget_Mem nvarchar(20),--招募目标
		Actual_Recruitnum_Mem int,--实际招募人数
		---Completion_rate_Mem nvarchar(20),--decimal(18, 2),完成率
		Same_Increase_Mem nvarchar(20),--环比(当前月与上一个月比)
		Lastyear_Increase_Mem nvarchar(20),--同比(当前月与去年的这个月比)
		Area_ratio_Mem nvarchar(20) ---区域占比( 区域占比 = 店铺新增注册会员实际数据/全国新增注册会员实际数据)
		--MemThisCount int,--当前这个时间的总会员
		--MemLastYearThisCount int,--上一年的总会员
		--MemLastThisCount int,--上一个月的总会员
		--MemAllCount int
	 )
declare @Sql varchar(max),
        @sqlCount nvarchar(max),
		@search1 varchar(max)='''',
		@search_theperiod nvarchar(max),
		@search_parallel nvarchar(max),
		@search_lastperiod nvarchar(max) 


 begin
   if (ISNULL(@channel,'''')<>'''')
   begin 
   --set @search1 = @search1+ '' and v.ChannelTypeCodeStore=''''''+@channel+''''''''
   set @search1 = @search1+ '' and c.ChannelTypeCodeStore in (''+@channel+'') ''
   end 
   if (ISNULL(@area,'''')<>'''')
   begin 
   set @search1 = @search1+ '' and b.AreaCodeStore= ''''''+@area+''''''''
   end 
   if (ISNULL(@city,'''')<>'''')
   begin 
   set @search1 = @search1+ '' and b.CityCodeStore= ''''''+@city+''''''''
   end 
   if (ISNULL(@store,'''')<>'''')
   begin 
   --set @search1 = @search1+ '' and v.StoreCode= ''''''+@store+''''''''

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
      begin
         drop table #TE_Rpt_StoreCode;
      end
   
   select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
   into #TE_Rpt_StoreCode;
   
   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
      begin
         drop table #TE_Rpt_StoreCode2;
      end
   
   select replace(b.StoreCode, '''''''', '''') as StoreCode
   into #TE_Rpt_StoreCode2
    from #TE_Rpt_StoreCode a
   outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;
   
   set @search1 = @search1 + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = b.StoreCode) '';



   end 
   if (ISNULL(@dateReg,'''')<>'''')
   begin 
   set @search_theperiod='' CONVERT(varchar(10),RegisterDate ,21)= CONVERT(varchar(10),''''''+convert(NVARCHAR(10),@dateReg,120)+'''''' ,21)''
   set @search_parallel=''  CONVERT(varchar(10),RegisterDate ,21)= CONVERT(varchar(10),DATEADD(YEAR,-1,''''''+convert(NVARCHAR(10),@dateReg,120)+'''''') ,21)''
   set @search_lastperiod='' CONVERT(varchar(10),RegisterDate ,21)= CONVERT(varchar(10),DATEADD(MONTH,-1,''''''+convert(NVARCHAR(10),@dateReg,120)+''''''),21)''

   end 
    if (ISNULL(@dateRegMon,'''')<>'''')
   begin 
  -- set @search2 = @search2+ '' and convert(varchar(7),RegisterDate,21) = ''''''+convert(varchar(7),@dateRegMon,21)+''''''''
  -- set @colshow=''
		--(select COUNT(*) from TM_Mem_Ext where CONVERT(varchar(7),Date_Attr_1 ,21)= CONVERT(varchar(7),''''''+convert(varchar(10),@dateRegMon,21)+'''''' ,21) and Str_Attr_5=tab.StoreCode ) as MemThisCount,
		--(select COUNT(*) from TM_Mem_Ext where CONVERT(varchar(7),Date_Attr_1 ,21)= CONVERT(varchar(7),DATEADD(YEAR,-1,''''''+convert(varchar(10),@dateRegMon,21)+''''''),21) and Str_Attr_5=tab.StoreCode ) as MemLastYearThisCount,
		--(select COUNT(*) from TM_Mem_Ext where CONVERT(varchar(7),Date_Attr_1 ,21)= CONVERT(varchar(7),DATEADD(MONTH,-1,''''''+convert(varchar(10),@dateRegMon,21)+''''''),21) and Str_Attr_5=tab.StoreCode ) as MemLastThisCount
  -- ''

   set @search_theperiod='' CONVERT(varchar(7),RegisterDate ,21)= CONVERT(varchar(7),''''''+convert(NVARCHAR(10),@dateReg,120)+'''''' ,21)''
   set @search_parallel='' CONVERT(varchar(7),RegisterDate ,21)= CONVERT(varchar(7),DATEADD(YEAR,-1,''''''+convert(NVARCHAR(10),@dateReg,120)+'''''') ,21)''
   set @search_lastperiod='' CONVERT(varchar(7),RegisterDate ,21)= CONVERT(varchar(7),DATEADD(MONTH,-1,''''''+convert(NVARCHAR(10),@dateReg,120)+''''''),21)''



   end 
   end




set @sql=''
 select ROW_NUMBER() OVER (ORDER BY  StoreCode desc) as rowId, *
 from (
select b.StoreCode,channelnamebase, citystore,storename ,
       sum(case when ''+@search_theperiod+''  then 1 else 0 end )  Actual_Recruitnum_Mem,    --招募人数
	   case when sum(case when ''+@search_lastperiod+''  then 1 else 0 end )=0 then 0 else 
	   round(cast( (sum(case when ''+@search_theperiod+''  then 1 else 0 end )- sum(case when ''+@search_lastperiod+''  then 1 else 0 end )  ) as float)/
	     sum(case when ''+@search_lastperiod+''  then 1 else 0 end ) ,4)  end   Same_Increase_Mem,---环比 增幅
		
		case when sum(case when ''+@search_parallel+''  then 1 else 0 end )=0 then 0 else  	 
	   round(cast( (sum(case when ''+@search_theperiod+''  then 1 else 0 end )- sum(case when ''+@search_parallel+''  then 1 else 0 end )  ) as float)/
	     sum(case when ''+@search_parallel+''  then 1 else 0 end ) ,4) end   Lastyear_Increase_Mem,  ---同比增幅

	    case when      (select count(*)   ---全国所有新增会员
		 from V_S_TM_Mem_Ext 
		 where 1=1 and ''+@search_theperiod+'')  =0 then 0 else 
	   round( cast(sum(case when ''+@search_theperiod+''  then 1 else 0 end ) as float) /
		(select count(*)   ---全国所有新增会员
		 from V_S_TM_Mem_Ext 
		 where 1=1 and ''+@search_theperiod+'')  ,4)  end   Area_ratio_Mem  --区域占比
       
from V_S_TM_Mem_Ext a with(nolock)  
left join  V_M_TM_SYS_BaseData_store b with(nolock)  on a.RegisterStoreCode=b.StoreCode
left join  V_M_TM_SYS_BaseData_channel c  with(nolock) on  b.ChannelTypeCodeStore=c.ChannelCodeBase
where 1=1  ''+@search1+''
group by StoreCode,channelnamebase, citystore,storename ) t ''
	 

--select * from V_M_TM_SYS_BaseData_store

--select * from V_M_TM_SYS_BaseData_channel

	   
        set @sqlCount = ''select @ct = count(rowId) from( '' + @Sql +'')T''
	    exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 
    
		set @sql=''select * from (''+@sql+'') as tab 
		where rowId between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize)+''''

		  print @Sql 
    
	   insert into @tabRecruitCount exec (@Sql)
	   select * from @tabRecruitCount
END

--exec [dbo].[sp_Rpt_Mem_Recruit_Count] '''','''','''','''','''','''',0,5,null




' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemContributionRate_Result]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemContributionRate_Result]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'  create procedure [dbo].[sp_Rpt_MemContributionRate_Result]
  @yearDate datetime,
  @endDate datetime
  as
  begin declare @Sql varchar(max)
  set @Sql=''
  select ''''普通'''' as MemberLevel,230 as Spending,20 as SpendingTB,30 as SpendingHB,
  59 as GuestUnitPrice, 22 as GuestUnitPriceTB, 20 as GuestUnitPriceHB,
  23 as GuestUnitCount,21 as GuestUnitCountTB,14 as GuestUnitCountHB,
  7 as SpendingRiseRate,1 as ContributionRateMember,2 as ContributionRateTotal,
  2000 as SpendingTotal,12 as SpendingTotalRiseRate,10 as SpendingTotalConRate
  union all
  select ''''tongpai'''' as MemberLevel,230 as Spending,10 as SpendingTB,24 as SpendingHB,
  39 as GuestUnitPrice, 22 as GuestUnitPriceTB, 14 as GuestUnitPriceHB,
  23 as GuestUnitCount,21 as GuestUnitCountTB,21 as GuestUnitCountHB,
  7 as SpendingRiseRate,1 as ContributionRateMember,21 as ContributionRateTotal,
  4000 as SpendingTotal,12 as SpendingTotalRiseRate,20 as SpendingTotalConRate
  union all
  select ''''yinpai'''' as MemberLevel,230 as Spending,20 as SpendingTB,30 as SpendingHB,
  59 as GuestUnitPrice, 12 as GuestUnitPriceTB, 18 as GuestUnitPriceHB,
  23 as GuestUnitCount,11 as GuestUnitCountTB,13 as GuestUnitCountHB,
  7 as SpendingRiseRate,11 as ContributionRateMember,2.1 as ContributionRateTotal,
  3000 as SpendingTotal,21 as SpendingTotalRiseRate,30 as SpendingTotalConRate
  ''

  print @Sql
  exec (@Sql)
  end



' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemMonthConsum]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemMonthConsum]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<hui.yang>
-- Create date: <Create Date,,>
-- Description:	<会员月消费区间统计,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Rpt_MemMonthConsum]
	@dateBegin DateTime ,
    @dateEnd   DateTime,
	@channel	nvarchar(200),
	@area	nvarchar(100),
	@city	nvarchar(100), 
	@store nvarchar(max),
	@PageIndex int,
    @PageSize int,
	@RecordCount int out	
AS
BEGIN

if @PageIndex = null or @PageIndex = -1 
begin
	declare @tblMemQstResult table (
		MemType nvarchar(128),
		StatTime nvarchar(128),
		Cnt1 int,
		Cnt2 int,
		Cnt3 int,
		Cnt4 int,
		Cnt5 int,
		Cnt6 int,
		Cnt7 int
		)
		select * from @tblMemQstResult 
		return
		end

		
declare @Sql  varchar(max)
declare @Sql_Search varchar(max)

 
	create table #mmmm
(
MemType nvarchar(128),
StatTime nvarchar(128),
Cnt1 int,
Cnt2 int,
Cnt3 int,
Cnt4 int,
Cnt5 int,
Cnt6 int,
Cnt7 int
)

set @Sql_Search =''''
	if ( isnull(@channel,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and c.ChannelCodeBase in (''+@channel+'') ''   
	end
	if ( isnull(@area, '''') <> ''''  )
	begin
		set @Sql_Search = @Sql_Search + '' and b.AreaCodeStore in (''+@area+'')  ''   
	end
	if (isnull(@store, '''') <> '''' and @store <> ''null'' )
	begin
		--set @Sql_Search = @Sql_Search + '' and a.StoreCodeSales in (''+ @store + '')  ''

    if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
       begin
          drop table #TE_Rpt_StoreCode;
       end
    
    select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
    into #TE_Rpt_StoreCode;
    
    if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
       begin
          drop table #TE_Rpt_StoreCode2;
       end
    
    select replace(b.StoreCode, '''''''', '''') as StoreCode
    into #TE_Rpt_StoreCode2
     from #TE_Rpt_StoreCode a
    outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;
    
    set @Sql_Search = @Sql_Search + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = a.StoreCodeSales) '';

	end

	if (isnull(@city, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and b.CityCodeStore in (''+ @city + '')  ''    
	end

 
set @Sql ='' 
 select MemType,StatTime,Cnt1,Cnt2 ,Cnt3 ,Cnt4 ,Cnt5 ,Cnt6 ,Cnt7  from (
 select Row_number() OVER ( ORDER BY SUBSTRING( b.ListDateSales,0,7),c.Str_Attr_12) rn, c.Str_Attr_12 as MemType,
SUBSTRING( b.ListDateSales,0,7) StatTime,
 sum(case when b.Amount >=1 and b.Amount <=100 then b.cnt else 0 end) as Cnt1 , 
 sum(case when b.Amount >= 101 and b.Amount <=300 then b.cnt else 0 end)  as Cnt2, 
 sum(case when b.Amount >= 301 and b.Amount <=500  then b.cnt else 0 end)  as Cnt3, 
 sum(case when b.Amount >= 501 and b.Amount <=1000  and b.cnt<=5  then b.cnt else 0 end)  as Cnt4, 
 sum(case when b.Amount >= 1001 and b.Amount <=2000  then b.cnt else 0 end)  as Cnt5, 
 sum(case when b.cnt >=2001 and b.cnt <= 5000 then b.cnt else 0 end)  as Cnt6, 
 sum(case when b.cnt >5000 then b.cnt else 0 end)   as Cnt7
 from
   V_S_TM_Mem_Master a left join 
 (
--分组 会员,消费,日期,计数
select MemberID,Amount,CONVERT(varchar(100), ListDateSales, 112) ListDateSales,count(1) cnt  from V_M_TM_Mem_Trade_sales a with (nolock)
left join V_M_TM_SYS_BaseData_store b on a.StoreCodeSales = b.StoreCode
left join V_M_TM_SYS_BaseData_channel c on b.ChannelTypeCodeStore = c.ChannelCodeBase 
WHERE ListDateSales >=  ''''''+convert( NVARCHAR(20),@dateBegin,120)+'''''' AND ListDateSales<=  ''''''+convert( NVARCHAR(20),@dateEnd,120)+''''''
''+ @Sql_Search +''
 group by MemberID,Amount, CONVERT(varchar(100), ListDateSales, 112)
) b on a.MemberID = b.MemberID
left join  TM_SYS_BaseData c on c.Str_Attr_11 = a.CustomerLevel 
where isnull(c.Str_Attr_12,'''''''') <> '''''''' and b.ListDateSales is not null 
group by c.Str_Attr_12,SUBSTRING( b.ListDateSales,0,7) 
 ) tt  ''

	   
declare  @sqlCount nvarchar(max)
 set @sqlCount = ''select @ct = count(1) from( '' + @Sql +'')T''
    exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 
	set @sql = @Sql +'' where rn >= ''''''+ convert(varchar(20),@PageIndex  * @PageSize)+''''''
       AND rn <= ''''''+convert(varchar(20),(@PageIndex+1) * @PageSize)+'''''' ''
	   print @sql
	 insert into #mmmm exec (@Sql)
	 print @RecordCount
select * from (
select * from #MMMM
 union all
 select MemType,''合计'' StatTime,sum(Cnt1)Cnt1,sum(Cnt2)Cnt2 ,sum(Cnt3) Cnt3,sum(Cnt4) Cnt4,sum(Cnt5) Cnt5,sum(Cnt6) Cnt6,sum(Cnt7) Cnt7 from #MMMM 
 group by MemType with rollup ) a where a.MemType is not null order by StatTime ,MemType desc
  
	    
	 
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemRepeatedConsumption_Result]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemRepeatedConsumption_Result]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<hui.yang>
-- Create date: <Create Date,,>
-- Description:	<会员重复消费统计,,>
-- =============================================
CREATE  PROCEDURE  [dbo].[sp_Rpt_MemRepeatedConsumption_Result]
	@dateBegin DateTime ,
    @dateEnd   DateTime,
	@channel	nvarchar(200),
	@area	nvarchar(100),
	@city	nvarchar(100), 
	@store nvarchar(max) ,
    @PageIndex int,
    @PageSize int,
	@RecordCount int out
	as
	begin 

	if @PageIndex = null or @PageIndex = -1 
	begin
	declare @tblMemQstResult table (
			rn int,
			MonthDate nvarchar(128),
			FirstSpendMoney decimal(18,2),
			FirstAllSpendMoney decimal(18,2),
			FirstSpendTradeNumber decimal(18,2),
			RepeatedSpendMoney decimal(18,2),
			RepeatedSpendTradeNumber decimal(18,2),
			RepeatedSpendPeople decimal(18,2),
			TotalSpendMoney decimal(18,2),
			TotalTradePeople decimal(18,2),
			TotalTradeNumber decimal(18,2),
			FristTradeGuestUnitPrice decimal(18,2),
			RepeatedTradeGuestUnitPrice decimal(18,2),
			RepeatedTradeProportion decimal(18,2) 
			) 
		select * from @tblMemQstResult 
		return
	end


	declare @Sql varchar(max)
	declare @Sql_Search varchar(max) 
	set @Sql_Search =''''
	if ( isnull(@channel,'''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.ChannelTypeCodeStore in ( ''+@channel+'') ''   
	end
	if ( isnull(@area, '''') <> ''''  )
	begin
		set @Sql_Search = @Sql_Search + '' and a.AreaCodeStore in (''+@area+'')  ''   
	end
	if (isnull(@store, '''') <> '''' and @store <> ''null'' )
	begin
		--set @Sql_Search = @Sql_Search + '' and a.StoreCode in (''+ @store + '')  ''

    if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
       begin
          drop table #TE_Rpt_StoreCode;
       end
    
    select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
    into #TE_Rpt_StoreCode;
    
    if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
       begin
          drop table #TE_Rpt_StoreCode2;
       end
    
    select replace(b.StoreCode, '''''''', '''') as StoreCode
    into #TE_Rpt_StoreCode2
     from #TE_Rpt_StoreCode a
    outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;
    
    set @Sql_Search = @Sql_Search + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = a.StoreCode) '';

	end


	if (isnull(@city, '''') <> '''' )
	begin
		set @Sql_Search = @Sql_Search + '' and a.CityCodeStore in (''+ @city + '')  ''    
	end
	declare @Sql_left varchar(max)
	set @Sql_left = ''''
	if @Sql_Search <> ''''
	begin
		set @Sql_left = ''left join V_M_TM_SYS_BaseData_store a  on a.StoreCode = b.StoreCodeSales''
	end
 
	set @Sql =''select 0 as rn,''''''+
	SUBSTRING( CONVERT(nvarchar(20),@dateBegin,112),0,7) + ''--''+SUBSTRING( CONVERT(nvarchar(20),@dateEnd,112),0,7)
	+'''''' MonthDate, n.*,
ROUND(
case when n.FirstSpendTradeNumber = 0 then 0 else 
n.FirstAllSpendMoney/cast(n.FirstSpendTradeNumber  as float) end ,2)as FristTradeGuestUnitPrice,--首次消费客单价
ROUND(
case when n.RepeatedSpendPeople =0 then 0 else
n.RepeatedSpendMoney /cast(n.RepeatedSpendPeople as float) end,2) as RepeatedTradeGuestUnitPrice,--重复消费客单价 
ROUND(
case when n.TotalTradePeople = 0 then 0 else
n.RepeatedSpendPeople  / cast(n.TotalTradePeople as float) end ,2)*100 as RepeatedTradeProportion--重复消费占比
from (
select 
 Sum(case when AllRn = 1 then amount else 0 end) as FirstSpendMoney,--首次交易金额
 Sum(case when MemRn = 1 then amount else 0 end)as FirstAllSpendMoney, --首次交易总金额
 Sum(case when MemRn = 1 then 1 else 0 end) as FirstSpendTradeNumber,--首次消费交易数
 Sum( case when MemRn <> 1 then Amount else 0 end) as RepeatedSpendMoney,--重复消费金额
 Sum(case when MemRn <>1 then 1 else 0 end) as RepeatedSpendTradeNumber, --重复消费交易数
 Sum(case when MemRn = 2 then 1 else 0 end) AS RepeatedSpendPeople,--重复消费人数
 Sum(amount) as TotalSpendMoney,--总消费金额
 Sum(case when MemRn = 1 then 1 else 0 end) as TotalTradePeople,--总交易人数(去重)
 COUNT(1) as TotalTradeNumber --总交易数 
from (
select
ROW_NUMBER() over (order by b.ListDateSales) as AllRn , --总计数
ROW_NUMBER() over ( PARTITION BY b.StoreCodeSales  order by b.ListDateSales) as StoreRn , --门店计数
ROW_NUMBER() over( PARTITION BY b.Memberid  order by b.ListDateSales) as MemRn ,--门店消费会员计数
StoreCodeSales,
b.Memberid, b.Amount
from V_M_TM_Mem_Trade_sales b  
''+ @Sql_left +''
--left join V_M_TM_SYS_BaseData_store a  on a.StoreCode = b.StoreCodeSales
where 1=1 and ListDateSales between ''''''+CONVERT(nvarchar(20),@dateBegin,112)+ ''''''and ''''''+CONVERT(nvarchar(20),@dateEnd,112)+'''''' '' + @Sql_Search +''
) m ) n''


	declare  @sqlCount nvarchar(max)
	set @sqlCount = ''select @ct = count(1) from( '' + @Sql +'')T''
    exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 
	print @RecordCount 
	print @Sql 
	 insert into @tblMemQstResult exec (@Sql)
	  select * from @tblMemQstResult
	   


	end
 ' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_MemUpGrade]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_MemUpGrade]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_Rpt_MemUpGrade]
	@dateBegin DateTime,
    @dateEnd   DateTime, 
	@area	nvarchar(100),
	@city	nvarchar(100), 
	@store nvarchar(max),
	@PageIndex int,
    @PageSize int,
	@RecordCount int out
as
/**********************************
  ----arvarto system-----
  存储过程功能描述：会员升级分析报表
  建 立 人：
  建立时间：
  修改内容: 20151218 zyb 修改会员存在跨级升级现象，后续跟维松确认，会员逐级升级，且跟simon确认取查询区间会员最后 一次升级记录进行统计；
            20151222 zyb 修改合计项
 
  ***********************************/

 begin
   declare @tableMemUpGrade table(
      rowIndex int,
	  Store nvarchar(100),
	  City nvarchar(100),
	  area nvarchar(100),
      Channel nvarchar(100),
	  ComToCopper int ,
	  CopperToSilver int,
	  SilverToGold int,
	  GoldToPlatinum int
   )
 declare @Sql nvarchar(max)='''', @SqlCount nvarchar(max)='''',@SqlSearch nvarchar(max)='''',
  @sqlSearchTime nvarchar(max)='''',@sqlSearchDataCondition nvarchar(max)=''''
   if (ISNULL(@area,'''')<>'''')
   
   begin 
   set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and AreaCodeStore in(''''''+@area+'''''')''
   end 
   
   if (ISNULL(@city,'''')<>'''')
   begin 
   set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and CityCodeStore in( ''''''+@city+'''''')''
   end 
   
   if (ISNULL(@store,'''')<>'''')
   begin 
   --set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and storecode in( ''''''+@store+'''''')''

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
      begin
         drop table #TE_Rpt_StoreCode;
      end
   
   select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
   into #TE_Rpt_StoreCode;
   
   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
      begin
         drop table #TE_Rpt_StoreCode2;
      end
   
   select replace(b.StoreCode, '''''''', '''') as StoreCode
   into #TE_Rpt_StoreCode2
    from #TE_Rpt_StoreCode a
   outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;
   
   set @sqlSearchDataCondition = @sqlSearchDataCondition + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = V_M_TM_SYS_BaseData_store.StoreCode) '';

   end 

 if(isnull(@dateBegin,'''')<>'''')
 begin
 set @sqlSearchTime=@sqlSearchTime+'' and AddedDate>= ''''''+CONVERT(nvarchar(20),@dateBegin,120)+'''''' ''
 end 
 if(isnull(@dateEnd,'''')<>'''')
 set @sqlSearchTime=@sqlSearchTime+'' and AddedDate< ''''''+CONVERT(nvarchar(20),dateadd(day,1,@dateEnd),120)+'''''' ''

 set @Sql=''

 select ROW_NUMBER() over(order by Store desc) as rowIndex,* from( 

select  
    StoreName as Store,city,area,channel,
	 --case when  GROUPING(StoreName) = 1 THEN  ''''总计''''  else StoreName end Store,
	 --case when  GROUPING(city) = 1 THEN  null  else city end city,
	 --case when  GROUPING(area) = 1 THEN  null  else area end area,
	 --case when  GROUPING(channel) = 1 THEN  null  else channel end channel,

	sum(ComToCopper)as ComToCopper,
	sum(CopperToSilver)as CopperToSilver,sum(SilverToGold)as SilverToGold,
	 sum(GoldToPlatinum)as GoldToPlatinum
from(select  dbStore.StoreName,city,area,channel,
	 isnull(ComToCopper,0)as ComToCopper,isnull(CopperToSilver,0) as CopperToSilver,
	isnull(SilverToGold,0) as SilverToGold,isnull(GoldToPlatinum,0) as GoldToPlatinum 	
from (select str_attr_12 ,Str_Attr_35 ,Str_Attr_50,Str_Attr_5 ,memberid from  TM_Mem_Ext with(nolock)) ext inner join 
      (select memberid,case when ChangeLevelTo=''''2'''' then 1 else 0 end  ComToCopper,
                case when ChangeLevelTo=''''3'''' then 1 else 0 end  CopperToSilver,
				case when ChangeLevelTo=''''4'''' then 1 else 0 end  SilverToGold,
				case when ChangeLevelTo=''''5'''' then 1 else 0 end  GoldToPlatinum
from (
select t.*,ROW_NUMBER() over (partition by t.memberid order by t.addeddate desc ) serial 
from (
select memberid,addeddate,
      case when ChangeLevelFrom=''''Normal'''' then 1
            when ChangeLevelFrom=''''Copper'''' then 2
			when ChangeLevelFrom=''''Silver'''' then 3
			when ChangeLevelFrom=''''Gold''''   then 4
			when ChangeLevelFrom=''''Platinum'''' then 5 end ChangeLevelFrom,
       case when ChangeLevelTo=''''Normal'''' then 1
            when ChangeLevelTo=''''Copper'''' then 2
			when ChangeLevelTo=''''Silver'''' then 3
			when ChangeLevelTo=''''Gold''''   then 4
			when ChangeLevelTo=''''Platinum'''' then 5 end ChangeLevelTo
from TL_Mem_LevelChange with(nolock) 
where 1=1 ''+@sqlSearchTime+'' ) t 
where ChangeLevelTo>ChangeLevelFrom  ) m
where serial=1  ) lc 
	on ext.MemberID=lc.MemberID		
right join(select StoreCode,StoreName,CityStore as city,ChannerTypeNameStore as channel,AreaNameStore as area from V_M_TM_SYS_BaseData_store with(nolock) where 1=1 ''+@sqlSearchDataCondition+'') dbStore on ext.Str_Attr_5=dbStore.StoreCode)TM
group by  StoreName,city,area,channel 
union all

select  
      '''''''' Store,'''''''' city,'''''''' area,''''总计'''' channel,

	sum(ComToCopper)as ComToCopper,
	sum(CopperToSilver)as CopperToSilver,sum(SilverToGold)as SilverToGold,
	 sum(GoldToPlatinum)as GoldToPlatinum
from(select  dbStore.StoreName,city,area,channel,
	 isnull(ComToCopper,0)as ComToCopper,isnull(CopperToSilver,0) as CopperToSilver,
	isnull(SilverToGold,0) as SilverToGold,isnull(GoldToPlatinum,0) as GoldToPlatinum 	
from (select str_attr_12 ,Str_Attr_35 ,Str_Attr_50,Str_Attr_5 ,memberid from  TM_Mem_Ext with(nolock)) ext inner join 
      (select memberid,case when ChangeLevelTo=''''2'''' then 1 else 0 end  ComToCopper,
                case when ChangeLevelTo=''''3'''' then 1 else 0 end  CopperToSilver,
				case when ChangeLevelTo=''''4'''' then 1 else 0 end  SilverToGold,
				case when ChangeLevelTo=''''5'''' then 1 else 0 end  GoldToPlatinum
from (
select t.*,ROW_NUMBER() over (partition by t.memberid order by t.addeddate desc ) serial 
from (
select memberid,addeddate,
      case when ChangeLevelFrom=''''Normal'''' then 1
            when ChangeLevelFrom=''''Copper'''' then 2
			when ChangeLevelFrom=''''Silver'''' then 3
			when ChangeLevelFrom=''''Gold''''   then 4
			when ChangeLevelFrom=''''Platinum'''' then 5 end ChangeLevelFrom,
       case when ChangeLevelTo=''''Normal'''' then 1
            when ChangeLevelTo=''''Copper'''' then 2
			when ChangeLevelTo=''''Silver'''' then 3
			when ChangeLevelTo=''''Gold''''   then 4
			when ChangeLevelTo=''''Platinum'''' then 5 end ChangeLevelTo
from TL_Mem_LevelChange with(nolock) 
where 1=1 ''+@sqlSearchTime+'' ) t 
where ChangeLevelTo>ChangeLevelFrom  ) m
where serial=1  ) lc 
	on ext.MemberID=lc.MemberID		
right join(select StoreCode,StoreName,CityStore as city,ChannerTypeNameStore as channel,AreaNameStore as area from V_M_TM_SYS_BaseData_store with(nolock) where 1=1 ''+@sqlSearchDataCondition+'') dbStore on ext.Str_Attr_5=dbStore.StoreCode)TM

) t 


''	
 set @SqlCount = ''select @ct = count(rowIndex) from( '' + @Sql +'')T''
	exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 
	set @Sql=''select * from(''+@Sql+'')as RowId where rowIndex between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize) +''''
     print @Sql
       -- exec (@Sql)
	   insert into @tableMemUpGrade exec(@Sql)
	   select * from @tableMemUpGrade
 end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_Prd_WeChatSign]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_Prd_WeChatSign]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
=============================================
Author:        lqw
Create date:   2015-11-25
Description:   二维码微信签到统计
Change:
=============================================
*/
CREATE PROCEDURE [dbo].[sp_Rpt_Prd_WeChatSign]
@dateBegin nvarchar(20),
@dateEnd   nvarchar(20),
@ActionCode nvarchar(20),
@PageIndex int,  --当前页 第0页开始
@PageSize int, --每页多少行
@RecordCount int out --总数

AS

begin

if @PageIndex is null or @PageIndex =-1
begin
declare @TE_WX_Mem_Act_result table(
ActionName nvarchar(50), --活动名称
SignDate nvarchar(20), --签到日期
fscs int, --发送次数
bpcqdcs int, --本批次签到次数
qdcs int, --签到次数
hyqdcs int, --会员签到次数
fhyqdcs int, --非会员签到次数
qdl float --签到率
);
select * from @TE_WX_Mem_Act_result;
return
end


if object_id(N''dbo.TE_WX_Mem_Act_lqw'') is not null
begin
   drop table TE_WX_Mem_Act_lqw;
end

select distinct t1.ActivityID, t1.ActivityName, t2.TableName, t1.ProStartDate, t1.ProEndDate, t1.ReferenceNo
into TE_WX_Mem_Act_lqw
from TM_Act_Master t1
left join TM_Act_Instance t2 on t1.ActivityID = t2.ActivityID and t2.Enable = 1
where t1.ProStartDate >= @dateBegin
  and t1.ProStartDate <= @dateEnd
  and t1.Enable = 1
  and t1.ReferenceNo = @ActionCode;

  declare @activtycount int
 select @activtycount=count(1) from TE_WX_Mem_Act_lqw
 if @activtycount=0
 begin
 select * from @TE_WX_Mem_Act_result
 return
 end

declare c_cursor cursor scroll for
select ReferenceNo, TableName from TE_WX_Mem_Act_lqw;
open c_cursor

declare
@ReferenceNo nvarchar(20),
@TableName nvarchar(100);

fetch next from c_cursor into @ReferenceNo, @TableName
declare @sql nvarchar(4000),
@cnt int,
@min_date nvarchar(20);

set @sql = ''select @cnt = count(1), @min_date = min(convert(nvarchar, AddedDate, 23)) from ['' + @TableName + ''] '';
exec sp_executesql @sql, N''@cnt int output, @min_date nvarchar(20) output'', @cnt output, @min_date output

set @sql = ''
select
t1.ActionName ActionName,
convert(nvarchar, t1.SignDate, 23) SignDate,
(case when convert(nvarchar, t1.SignDate, 23) >= '''''' + @min_date + '''''' then '' + cast(@cnt as nvarchar(20)) + '' else 0 end) fscs,
count(t1.MemberID) bpcqdcs,
count(case when isnull(t3.MemberID, '''''''') <> '''''''' then 1 end) qdcs,
count(case when isnull(t4.MemberID, '''''''') <> '''''''' then 1 end) hyqdcs,
count(case when isnull(t4.MemberID, '''''''') = '''''''' then 1 end) fhyqdcs,
round(cast(count(case when isnull(t3.MemberID, '''''''') <> '''''''' then 1 end) as float) / '' + cast(@cnt as nvarchar(20)) + '', 4) qdl into ##temp
from TL_WX_MemberSign t1
left join TE_WX_Mem_Act_lqw t2 on t2.ReferenceNo = t1.ActionCode
left join ['' + @TableName + ''] t3 on t1.MemberID = t3.MemberID
left join V_S_TM_Mem_Ext t4 on t1.MemberID = t4.MemberID
where t1.SignDate >= '''''' + @dateBegin + ''''''
and t1.SignDate <= '''''' + @dateEnd + ''''''
and t1.ActionCode = '' + cast(@ReferenceNo as nvarchar(10)) + ''
group by t1.ActionCode, t1.ActionName, convert(nvarchar, t1.SignDate, 23)
'';

 exec (@sql);
close c_cursor
deallocate c_cursor

select @RecordCount = (case when t.cnt > 0 then t.cnt + 1 else t.cnt end) from (
select count(1) cnt from ##temp
) t;

select
a.ActionName, a.SignDate, a.fscs, a.bpcqdcs, a.qdcs, a.hyqdcs, a.fhyqdcs, a.qdl
from (
select
case when isnull(t.ActionName, '''') <> '''' then t.ActionName else ''合计'' end ActionName,
t.SignDate, t.fscs, t.bpcqdcs, t.qdcs, t.hyqdcs, t.fhyqdcs, t.qdl,
row_number() over(order by ActionName desc) r_num
from (
select ActionName, SignDate, max(fscs) fscs, sum(bpcqdcs) bpcqdcs,
sum(qdcs) qdcs, sum(hyqdcs) hyqdcs, sum(fhyqdcs) fhyqdcs, sum(qdl) qdl
from ##temp
group by cube(ActionName, SignDate)
) t
where (isnull(t.ActionName, '''') = '''' and isnull(t.SignDate, '''') = '''')
or (isnull(t.ActionName, '''') <> '''' and isnull(t.SignDate, '''') <> '''')
) a
where a.r_num >= @PageIndex * @PageSize + 1
  and a.r_num <= (@PageIndex + 1) * @PageSize

  drop table ##temp

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_PriceSegmentDistributionOffline_Result]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_PriceSegmentDistributionOffline_Result]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_Rpt_PriceSegmentDistributionOffline_Result]
	@dateBegin DateTime ,
    @dateEnd   DateTime,
	@channel	nvarchar(200)
AS
/**********************************
  ----arvarto system-----
  存储过程功能描述：价位消费区间统计
  建 立 人：
  建立时间：
  修改内容: 20151221 zyb 修改订单日期listdatesales，约束中加转换约束，避免临界值的少统计；
            20151221 zyb 整体代码待优化， 金额取值待确认；
  ***********************************/
BEGIN
	 declare @tabPriceSegmentDistributionOffline table
	 (
		AreaCodeStore nvarchar(100),
	    Area nvarchar(100),
	    UnderHundred int,
	    HundredOneToThree int,
	    HundredThreeToSix int,
	    HundredSixToTwelve int,
	    HundredTwelveToTwenty int,
	    HundredTwentyToForty int,
	    OverFortyHundred int
	 )
	 
declare @Sql varchar(max),
	 @SqlSearchCondition varchar(max)=''''

begin
		 if (isnull(@dateBegin, '''') <> '''' )--查询消费起日
		begin
			set @SqlSearchCondition=@SqlSearchCondition + '' and convert(NVARCHAR(10),tmt.listdatesales,120)  >=  ''''''+convert(NVARCHAR(10),@dateBegin,120)+'''''' '' 
		end
		
		if (isnull(@dateEnd, '''') <> '''' )--查询消费止日
		begin
			set @SqlSearchCondition=@SqlSearchCondition + '' and convert(NVARCHAR(10),tmt.listdatesales,120) <=  ''''''+convert(NVARCHAR(10),@dateEnd,120)+'''''' ''  
		end
		
		if (isnull(@channel, '''') <> '''' )--渠道
		begin
			--set @SqlSearchCondition=@SqlSearchCondition + '' and vd.ChannelTypeCodeStore =  ''''''+convert(NVARCHAR(10),@channel,120)+'''''' ''
			set @SqlSearchCondition=@SqlSearchCondition + '' and vd.ChannelTypeCodeStore in ('' + @channel + '') ''
		end
	 end

set @Sql='' select tab.AreaCodeStore as AreaCodeStore ,tab.AreaNameStore as Area,
     (select COUNT(1) from V_M_TM_Mem_Trade_sales tmt with(nolock)
		left join V_M_TM_SYS_BaseData_store vd with(nolock)
		on tmt.StoreCodeSales=vd.StoreCode
		where tmt.TotalMoneySales <100 and vd.AreaCodeStore=tab.AreaCodeStore 
		''+@SqlSearchCondition+''
		) as UnderHundred,
		
		(select COUNT(1) from V_M_TM_Mem_Trade_sales tmt with(nolock)
		left join V_M_TM_SYS_BaseData_store vd with(nolock)
		on tmt.StoreCodeSales=vd.StoreCode
		where tmt.TotalMoneySales >=100 and tmt.TotalMoneySales <300 and vd.AreaCodeStore=tab.AreaCodeStore
		''+@SqlSearchCondition+''
		) as HundredOneToThree,
		
		(select COUNT(1) from V_M_TM_Mem_Trade_sales tmt with(nolock)
		left join V_M_TM_SYS_BaseData_store vd with(nolock)
		on tmt.StoreCodeSales=vd.StoreCode
		where tmt.TotalMoneySales >=300 and tmt.TotalMoneySales <600 and vd.AreaCodeStore=tab.AreaCodeStore
		''+@SqlSearchCondition+''
		) as HundredThreeToSix,
		
		(select COUNT(1) from V_M_TM_Mem_Trade_sales tmt with(nolock)
		left join V_M_TM_SYS_BaseData_store vd with(nolock)
		on tmt.StoreCodeSales=vd.StoreCode
		where tmt.TotalMoneySales >=600 and tmt.TotalMoneySales <1200 and vd.AreaCodeStore=tab.AreaCodeStore
		''+@SqlSearchCondition+''
		) as HundredSixToTwelve,
		
		(select COUNT(1) from V_M_TM_Mem_Trade_sales tmt with(nolock)
		left join V_M_TM_SYS_BaseData_store vd with(nolock)
		on tmt.StoreCodeSales=vd.StoreCode
		where tmt.TotalMoneySales >=1200 and tmt.TotalMoneySales <2000 and vd.AreaCodeStore=tab.AreaCodeStore
		''+@SqlSearchCondition+''
		) as HundredTwelveToTwenty,
		
		(select COUNT(1) from V_M_TM_Mem_Trade_sales tmt with(nolock)
		left join V_M_TM_SYS_BaseData_store vd with(nolock)
		on tmt.StoreCodeSales=vd.StoreCode
		where tmt.TotalMoneySales >=2000 and tmt.TotalMoneySales <=4000 and vd.AreaCodeStore=tab.AreaCodeStore
		''+@SqlSearchCondition+''
		) as HundredTwentyToForty,
		
		(select COUNT(1) from V_M_TM_Mem_Trade_sales tmt with(nolock)
		left join V_M_TM_SYS_BaseData_store vd with(nolock)
		on tmt.StoreCodeSales=vd.StoreCode
		where tmt.TotalMoneySales >4000 and vd.AreaCodeStore=tab.AreaCodeStore
		''+@SqlSearchCondition+''
		) as OverFortyHundred
		from
		(
			select tmt.*,vd.AreaCodeStore,vd.StoreCode,vd.AreaNameStore from V_M_TM_Mem_Trade_sales tmt with(nolock)
			left join V_M_TM_SYS_BaseData_store vd with(nolock)
			on tmt.StoreCodeSales=vd.StoreCode
		) as tab  group by tab.AreaNameStore,tab.AreaCodeStore order by tab.AreaCodeStore''
	
	 print @Sql
	 insert into @tabPriceSegmentDistributionOffline exec (@Sql)
	 select * from @tabPriceSegmentDistributionOffline
END

--exec  sp_Rpt_PriceSegmentDistributionOffline_Result null,null,null' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_StoreConsumptionDuty_Count]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_StoreConsumptionDuty_Count]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_Rpt_StoreConsumptionDuty_Count]
    @channel    nvarchar(200),--渠道
    @area    nvarchar(100),--大区
    @city    nvarchar(100), --城市
    @store nvarchar(max), --门店
    @dateConsumptionStart    DateTime,--查询消费起日
    @dateConsumptionEnd    DateTime, --查询消费止日
    @PageIndex int,--当前页
    @PageSize int,--总页数
    @RecordCount int out    --总数
AS
/**********************************
  ----arvarto system-----
  存储过程功能描述：门店消费占比
  建 立 人：
  建立时间：
  修改内容: 20151221 zyb 更改所有的时间转换，避免临界值数据少取，更改累计消费金额的时间约束（反了）
            20151221 zyb 合计项待修改；
			

      select * from [dbo].[TD_SYS_FieldAlias] where tablename=''tm_mem_trade'' 
    Dec_Attr_7    TotalMoneySales   

  ***********************************/




BEGIN

declare @tabStoreConsumptionDuty table(
    rowId int,
    storecode nvarchar(100),
    Expendture_Mem decimal(18, 2),--会员消费金额
    ConsumptionMoneySum_Mem decimal(18, 2),--会员累计消费金额
    City nvarchar(100),
    Store nvarchar(100),
    ConsumptionTotalMoney decimal(18, 2),--消费总金额
    DutyTotal decimal(18, 2),--占比
    DutyExpendture decimal(18, 2)--占比
)

declare @Sql  varchar(max),
@sqlSearchDataCondition varchar(max)='' where 1=1'',
 @sqlCount nvarchar(max)

 begin
   if (ISNULL(@channel,'''')<>'''')
   begin 
   --set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and s.ChannelTypeCodeStore=''''''+@channel+''''''''
   set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and s.ChannelTypeCodeStore in ('' + @channel + '') ''
   end 

   if (ISNULL(@area,'''')<>'''')
   begin 
   set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and s.AreaCodeStore= ''''''+@area+''''''''
   end 

   if (ISNULL(@city,'''')<>'''')
   begin 
   set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and s.CityCodeStore= ''''''+@city+''''''''
   end 

   if (ISNULL(@store,'''')<>'''')
   begin 
   --set @sqlSearchDataCondition = @sqlSearchDataCondition+ '' and tab.storecode= ''''''+@store+''''''''

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
      begin
         drop table #TE_Rpt_StoreCode;
      end

   select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
   into #TE_Rpt_StoreCode;

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
      begin
         drop table #TE_Rpt_StoreCode2;
      end

   select replace(b.StoreCode, '''''''', '''') as StoreCode
   into #TE_Rpt_StoreCode2
    from #TE_Rpt_StoreCode a
   outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;

   set @sqlSearchDataCondition = @sqlSearchDataCondition + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = tab.storecode) '';

   end 
end



set @Sql= '' 
select ROW_NUMBER() over(order by tab.storecode desc) as rowId,tab.* from 
(
    select 
        tab.*,
        s.CityStore as City,
        s.StoreName as Store,
        CAST(convert(decimal(38, 2),tpc.amount) as varchar(20))as ConsumptionTotalMoney,
        
		            CAST(convert(decimal(38, 2),isnull((tab.Expendture_Mem/tab.ConsumptionMoneySum_Mem) ,0)*100) 
					as varchar(20)) 
					as DutyTotal,
    
		 CAST(Convert(decimal(38,2), isnull((tab.Expendture_Mem/tpc.amount) ,0)*100) as varchar(20))
		             as DutyExpendture
    from
    (
        select tmt.Str_Attr_2 as storecode,--门店代码
        CAST(convert(decimal(38, 2),SUM(tmt.Dec_Attr_7)) as varchar(20)) as Expendture_Mem,--会员消费金额
        (select SUM(t.Dec_Attr_7) from TM_Mem_Trade t with(nolock)
        where 
        convert(NVARCHAR(20),t.Date_Attr_1,120)<=''''''+convert(NVARCHAR(20),@dateConsumptionEnd ,120) +'''''' 
        and convert(NVARCHAR(20),t.Date_Attr_1,120)>=''''''+convert(NVARCHAR(20),@dateConsumptionStart,120) +''''''
        and t.Str_Attr_2= tmt.Str_Attr_2) as ConsumptionMoneySum_Mem--会员累计消费金额
        from TM_Mem_Trade tmt with(nolock)
        group by tmt.Str_Attr_2
     ) as tab
     left join V_M_TM_SYS_BaseData_store as s with(nolock)
     on
     tab.storecode=s.StoreCode 
     left join 
     (
          select tp.StoreCode,SUM(tp.Amount) as amount from TD_POS_ConsumeBill tp with(nolock)
          where tp.BusinessDate>=''''''+convert(NVARCHAR(20),@dateConsumptionStart,120)+''''''
           and tp.BusinessDate<=''''''+convert(NVARCHAR(20),@dateConsumptionEnd,120)+'''''' 
          group by tp.StoreCode
      ) tpc
     on
     tab.storecode=tpc.StoreCode ''+@sqlSearchDataCondition+''
 ) as tab''

    set @sqlCount = ''select @ct = count(rowId) from( '' + @Sql +'')T''
    exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 

    --print(@sqlCount)

    set @sql=''select * from (''+@sql+'') as tab 
    where rowId between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize)+''''

    print @Sql
    insert into @tabStoreConsumptionDuty exec (@Sql)
    select * from  @tabStoreConsumptionDuty

END

--exec  [sp_Rpt_StoreConsumptionDuty_Count] '''','''','''','''','''','''',1,2,null




' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_StoreConsumptionMonthly_Count]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_StoreConsumptionMonthly_Count]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_Rpt_StoreConsumptionMonthly_Count]
   @channel    nvarchar(200),--渠道
    @area    nvarchar(100),--大区
    @city    nvarchar(100), --城市
    @store nvarchar(max), --门店
    @consumptiondateStart DateTime,--查询消费起日
    @consumptiondateEnd DateTime, --查询消费止日
    @PageIndex int,--当前页
    @PageSize int,--总页数
    @RecordCount int out    --总数
as 
/**********************************
  ----arvarto system-----
  存储过程功能描述：门店消费月统计
  建 立 人：
  建立时间：
  修改内容: 20151222 zyb  更改同比环比的使用函数，环比改为按天推算；且改为包含日期的临界值；修改时间的取值字段；
            20151123 zyb  合计值新增；判断分母为0的情况，修改数值型由字符改为数值，合计需使用；
			20151124 zyb  修改storecode的数据类型，int改为nvarchar;

  ***********************************/

begin
 declare @tablemonthcount table(
        rowIndexs int,
        StoreCode nvarchar(50),
        Store nvarchar(50), 
        City nvarchar(50),
        TotalSales decimal(18,2), 
        AmountOfSales_Mem decimal(18,2),
        SalesContribution_Mem decimal(18,4),
        SalesAmplificationHB_Mem decimal(18,4),
        GuestPrice_Mem decimal(18,2),
        GuestOrders_Mem decimal(18,2),
        TurnoverRate_Mem decimal(18,4),
        HBAmount decimal(18,2),
        TBAmount decimal(18,2)
 )
-- select * from @tablemonthcount
 declare @Sql nvarchar(max)='''',@SqlSearch nvarchar(max)='''',@sqlCount nvarchar(max),
 @SqlTime nvarchar(max)='''',@SqlTimeHB nvarchar(max)='''',
 @SqlTimeTB nvarchar(max)='''',
 @DateTimeHBStart datetime,
 @DateTimeHBEnd datetime,@DateTimeTBStart datetime,
 @DateTimeTBEnd datetime,@Interval int
 if(isnull(@area,'''')<>'''')
    begin
      set @SqlSearch=  @SqlSearch+'' and store.AreaCodeStore=''''''+@area+'''''' ''
    end
    if(ISNULL(@city,'''')<>'''')
    begin
      set   @SqlSearch=  @SqlSearch+'' and store.CityCodeStore=''''''+@city+'''''' '' 
    end
 if(ISNULL(@channel,'''')<>'''')
 begin
   --set @SqlSearch=  @SqlSearch+'' and chanel.ChannelCodeBase=''''''+@channel+'''''' ''
   set @SqlSearch=  @SqlSearch+'' and chanel.ChannelCodeBase in ('' + @channel + '') ''
 end
 if(ISNULL(@store,'''')<>'''')
  begin
   --set @SqlSearch=  @SqlSearch+'' and store.StoreCode=''''''+@store+'''''' ''

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode'') is not null
      begin
         drop table #TE_Rpt_StoreCode;
      end

   select StoreCode = @store, convert(xml, ''<root><v>'' + replace(@store, '','', ''</v><v>'')+ ''</v></root>'') as Xml_StoreCode
   into #TE_Rpt_StoreCode;

   if object_id(N''tempdb.dbo.#TE_Rpt_StoreCode2'') is not null
      begin
         drop table #TE_Rpt_StoreCode2;
      end

   select replace(b.StoreCode, '''''''', '''') as StoreCode
   into #TE_Rpt_StoreCode2
    from #TE_Rpt_StoreCode a
   outer apply (select N.v.value(''.'', ''nvarchar(100)'') as StoreCode from a.Xml_StoreCode.nodes(''/root/v'') N(v) ) b;

   set @SqlSearch = @SqlSearch + ''and exists (select 1 from #TE_Rpt_StoreCode2 te where te.StoreCode = store.StoreCode) '';

 end

 if(ISNULL(@consumptiondateStart,'''')<>''''and ISNULL(@consumptiondateEnd,'''')<>'''')
 begin
   --set @Interval=DATEDIFF(month,@consumptiondateEnd,@consumptiondateStart)

   --set @DateTimeHBStart=DATEADD(MONTH,@Interval,@consumptiondateStart)
   --set @DateTimeHBEnd=dateadd(month,@interval,@consumptiondateEnd)
   --set  @DateTimeTBStart=dateadd(year,1,@consumptiondateStart)
   --set @DateTimeTBEnd=dateadd(year,1,@consumptiondateEnd)


   set @Interval=DATEDIFF(day,@consumptiondateEnd,@consumptiondateStart)

   set @DateTimeHBStart=DATEADD(day,@Interval,@consumptiondateStart)
   set @DateTimeHBEnd=dateadd(day,@interval,@consumptiondateEnd)
   set  @DateTimeTBStart=dateadd(year,-1,@consumptiondateStart)
   set @DateTimeTBEnd=dateadd(year,-1,@consumptiondateEnd)






--set @SqlTime=@SqlTime+'' and addeddate >=''''''+convert(nvarchar(20),@consumptiondateStart,120)+'''''' and addeddate<''''''+convert(nvarchar(20),@consumptiondateEnd,120)+'''''' ''

--set @SqlTimeHB=@SqlTimeHB+'' and addeddate >=''''''+convert(nvarchar(20),@DateTimeHBStart,120)+'''''' and addeddate<''''''+convert(nvarchar(20),@DateTimeHBEnd,120)+'''''' ''

--set @SqlTimeTB=@SqlTimeTB+'' and addeddate>=''''''+convert(nvarchar(20),@DateTimeTBStart,120)+''''''and addeddate<''''''+convert(nvarchar(20),@DateTimeTBEnd,120)+'''''' ''


set @SqlTime=@SqlTime+'' and convert(nvarchar(20),ListdateSales ,120)>=''''''+convert(nvarchar(20),@consumptiondateStart,120)+'''''' and convert(nvarchar(20),ListdateSales ,120)<=''''''+convert(nvarchar(20),@consumptiondateEnd,120)+'''''' ''

set @SqlTimeHB=@SqlTimeHB+'' and  convert(nvarchar(20),ListdateSales ,120) >=''''''+convert(nvarchar(20),@DateTimeHBStart,120)+'''''' and convert(nvarchar(20),ListdateSales ,120)<=''''''+convert(nvarchar(20),@DateTimeHBEnd,120)+'''''' ''

set @SqlTimeTB=@SqlTimeTB+'' and  convert(nvarchar(20),ListdateSales ,120)>=''''''+convert(nvarchar(20),@DateTimeTBStart,120)+''''''and convert(nvarchar(20),ListdateSales ,120)<=''''''+convert(nvarchar(20),@DateTimeTBEnd,120)+'''''' ''



end
  set @Sql=''

  select ROW_NUMBER() over(order by StoreCode desc ) as rowIndexs,* 
  from (


  select  StoreCode,StoreName as Store,CityStore as City,allamount as TotalSales,memberamount as AmountOfSales_Mem,
case when isnull(allamount,0)=0 then 0 else  round(isnull(memberamount,0)/allamount ,4)  end SalesContribution_Mem,
case when isnull(hbamount,0)=0 then 0 else  round((isnull(memberamount,0)-isnull(hbamount,0))/hbamount ,4)  end  SalesAmplificationHB_Mem,
case when isnull(membercount,0)=0   then 0 else  round(isnull(memberamount,0)/membercount ,2) end  GuestPrice_Mem,
case when isnull(membercount,0)=0   then 0 else  round(isnull(memberqty,0)/membercount ,2) end   GuestOrders_Mem,
case when isnull(tbamount,0)=0   then 0 else  round((isnull(memberamount,0)-isnull(tbamount,0))/tbamount ,2) end   TurnoverRate_Mem,
hbamount,tbamount
 from (select store.StoreCode,store.StoreName,store.CityStore from V_M_TM_SYS_BaseData_store store with (nolock) left join V_M_TM_SYS_BaseData_channel chanel with (nolock)
on store.ChannelCodeStore=chanel.ChannelCodeBase 
where 1=1 '' +@SqlSearch+'') basedata  
left join 
(select * from (select count(1)as membercount, StoreCodeSales as memberStoreCode,sum(QuantitySales)as memberqty,sum(amount) as memberamount from V_M_TM_Mem_Trade_sales with(nolock)
   where 1=1 ''+@SqlTime+'' group by StoreCodeSales ) member  left join 
 (select allStoreCode,sum(qty)as allqty,sum(amount) as allamount  from 
 (select StoreCode as allStoreCode,sum(Qty) as qty,sum(Amount)as amount from TD_POS_ConsumeBill group by StoreCode
union all
select  StoreCodeSales as allStoreCode,sum(QuantitySales)as qty,sum(amount) as amount from V_M_TM_Mem_Trade_sales with(nolock)
group by StoreCodeSales) allamount 
group by allStoreCode)  amountall  
on amountall.allStoreCode=member.memberStoreCode
left join 
(select StoreCodeSales as tbStoreCode,sum(QuantitySales)as tbqty,sum(amount) as tbamount from V_M_TM_Mem_Trade_sales with(nolock)
where 1=1 ''+@SqlTimeTB+''
group by StoreCodeSales) membertb 
on membertb.tbStoreCode=member.memberStoreCode
left join 
(select StoreCodeSales as hbStoreCode,sum(QuantitySales)as hbqty,sum(amount) as hbamount from V_M_TM_Mem_Trade_sales with(nolock)
where 1=1  ''+@SqlTimeHB+''
group by StoreCodeSales) memberhb 
on memberhb.hbStoreCode=member.memberStoreCode)
shuju on shuju.memberStoreCode=basedata.StoreCode

union all
select '''''''' StoreCode,'''''''' Store,''''总计'''' City,sum(allamount) as TotalSales,sum(memberamount) as AmountOfSales_Mem,
case when sum(isnull(allamount,0))=0 then 0 else  round(sum(isnull(memberamount,0))/sum(allamount) ,4)  end SalesContribution_Mem,
case when sum(isnull(hbamount,0))=0 then 0 else  round((sum(isnull(memberamount,0))-sum(isnull(hbamount,0)))/sum(hbamount) ,4)  end  SalesAmplificationHB_Mem,
case when sum(isnull(membercount,0))=0   then 0 else  round(sum(isnull(memberamount,0))/sum(membercount) ,2) end  GuestPrice_Mem,
case when sum(isnull(membercount,0))=0   then 0 else  round(sum(isnull(memberqty,0))/sum(membercount) ,2) end   GuestOrders_Mem,
case when sum(isnull(tbamount,0))=0   then 0 else  round((sum(isnull(memberamount,0))-sum(isnull(tbamount,0)))/sum(tbamount) ,2) end   TurnoverRate_Mem,
 sum(hbamount) hbamount ,sum(tbamount) tbamount     
from (
  select  StoreCode,StoreName as Store,CityStore as City,allamount ,memberamount  ,membercount,memberqty,
hbamount,tbamount
 from (select store.StoreCode,store.StoreName,store.CityStore from V_M_TM_SYS_BaseData_store store with (nolock) left join V_M_TM_SYS_BaseData_channel chanel with (nolock)
on store.ChannelCodeStore=chanel.ChannelCodeBase 
where 1=1 '' +@SqlSearch+'') basedata  
left join 
(select * from (select count(1)as membercount, StoreCodeSales as memberStoreCode,sum(QuantitySales)as memberqty,sum(amount) as memberamount from V_M_TM_Mem_Trade_sales with(nolock)
   where 1=1 ''+@SqlTime+'' group by StoreCodeSales ) member  left join 
 (select allStoreCode,sum(qty)as allqty,sum(amount) as allamount  from 
 (select StoreCode as allStoreCode,sum(Qty) as qty,sum(Amount)as amount from TD_POS_ConsumeBill group by StoreCode
union all
select  StoreCodeSales as allStoreCode,sum(QuantitySales)as qty,sum(amount) as amount from V_M_TM_Mem_Trade_sales with(nolock)
group by StoreCodeSales) allamount 
group by allStoreCode)  amountall  
on amountall.allStoreCode=member.memberStoreCode
left join 
(select StoreCodeSales as tbStoreCode,sum(QuantitySales)as tbqty,sum(amount) as tbamount from V_M_TM_Mem_Trade_sales with(nolock)
where 1=1 ''+@SqlTimeTB+''
group by StoreCodeSales) membertb 
on membertb.tbStoreCode=member.memberStoreCode
left join 
(select StoreCodeSales as hbStoreCode,sum(QuantitySales)as hbqty,sum(amount) as hbamount from V_M_TM_Mem_Trade_sales with(nolock)
where 1=1  ''+@SqlTimeHB+''
group by StoreCodeSales) memberhb 
on memberhb.hbStoreCode=member.memberStoreCode)
shuju on shuju.memberStoreCode=basedata.StoreCode ) m 
) t

''

  set @sqlCount = ''select @ct = count(rowIndexs) from( '' + @Sql +'')T''
    exec sp_executesql @SqlCount,N''@ct int output'',@RecordCount output 
    set @Sql=''select * from(''+@Sql+'')as RowId where rowIndexs between ''+Convert(nvarchar(20),@PageIndex*@PageSize+1)+'' and ''+Convert(nvarchar(20),(@PageIndex+1)*@PageSize) +''''
        print @Sql
      insert into @tablemonthcount  exec (@Sql)

      select * from @tablemonthcount
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_UseCoupon_Count]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_UseCoupon_Count]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE proc [dbo].[sp_Rpt_UseCoupon_Count]
 @area varchar(100),
 @city varchar(100),
 @store  varchar(max),
 @couponname varchar(100),
 @datesendcouponStart	datetime,
 @datesendcouponEnd datetime,
 @PageIndex int,
 @PageSize int ,
 @RecordCount int out
 as
  begin 
    declare @tableCoupon table(  
        --rowIndexs int,
        UseNumber int,
		CouponName nvarchar(500),
        SendNumber int,
        UseRate decimal(18,2),
		ReceiveNumber int
		
  )
 -- select * from @tableCoupon
	declare @sql nvarchar(max)
	       ,@Sql_Count nvarchar(max)
		   ,@Sql_Total nvarchar(max)
	set @sql = ''
	select tct.AddedDate as AddedDate ,tct.Name as CouponName,count(*) as SendNumber
	,Sum(case when tmc.MemberID is not null then 1 else 0 end) as ReceiveNumber
	,sum(case when IsUsed =1 then 1 else 0 end) as UseNumber 
	,cast(cast(sum(case when IsUsed =1 then 1 else 0 end)as decimal(18,2))/cast(count(*) as decimal(18,2)) as decimal(18,2))  as UseRate
	from TM_Act_CommunicationTemplet tct
	inner join TM_Mem_CouponPool tmc on tct.TempletID=tmc.TempletID
	inner join V_U_TM_Mem_Info m on tmc.memberid = m.MemberID
	left join V_M_TM_Mem_Trade_sales t on t.MemberID=m.MemberID
	left join V_M_TM_SYS_BaseData_store s on s.StoreCode = t.StoreCodeSales
	where Type = ''''Coupon'''' ''
	begin
	if (ISNULL(@area,'''')<>'''')
	set @sql = @sql+'' and s.AreaCodeStore = ''+@area+''''
	
	if (ISNULL(@city,'''')<>'''')
	set @sql = @sql+'' and s.CityCodeStore = ''+@city+''''

	if (ISNULL(@store,'''')<>'''')
	set @sql = @sql+'' and s.StoreCode in (''+@store+'')''
 
 	if (ISNULL(@couponname,'''')<>'''')
	set @sql = @sql+'' and tct.Name like ''''%''+@couponname+''%''''''

	if (ISNULL(@datesendcouponStart,'''')<>'''')
	set @sql = @sql+'' and tmc.AddedDate > ''''''+cast(@datesendcouponStart as nvarchar(20))+''''''''

	if (ISNULL(@datesendcouponStart,'''')<>'''')
	set @sql = @sql+'' and tmc.AddedDate < ''''''+cast(@datesendcouponEnd as nvarchar(20))+''''''''
	end

	set @sql =@sql + ''group by tct.AddedDate,tct.Name,tmc.TempletID''

	set @Sql_Total = ''select  ''''2099-12-31'''' AddedDate,''''总计'''' CouponName,sum(SendNumber) as SendNumber ,sum(ReceiveNumber) as ReceiveNumber,sum(UseNumber) as UseNumber , avg(UseRate) as UseRate from ('' + @sql +'')o''
	
	
	set  @sql= @sql + '' union all '' +@Sql_Total
	set @Sql_Count= ''select @ct =count(1) from (''+@sql+'')p''
	exec sp_executesql @Sql_Count,N''@ct int output'',@RecordCount out
	set @sql = ''select * from (select Row_number() OVER ( ORDER BY AddedDate) rn ,* from (''+@sql+'')r)rr where rn > ''''''+ convert(varchar(20),@PageIndex  * @PageSize)+''''''
																	AND rn <= ''''''+convert(varchar(20),(@PageIndex+1) * @PageSize)+'''''' ''

	


	exec (@sql)
	
	
	
  


	     
 end





' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rpt_WXSign]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rpt_WXSign]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N' CREATE PROC [dbo].[sp_Rpt_WXSign](
 @StartDate nvarchar(20),
 @EndDate nvarchar(20),
 @ActionName nvarchar(20),
 @PageIndex int,--第几页,0开始
 @PageSize int,--一页几行
 @RecordCount int out	 -- 总数据量
 ) 
AS
Begin

	if @PageIndex is null or @PageIndex =-1
	begin
	declare @tblResult table (
		 ReferenceNo nvarchar(20),
		ActivityName nvarchar(50) ,
		SendCount int ,
		SignCountAll int,
		SignCountSend int,
		SignCountMem int,
		SignCountNOTMem int
		)
		select * from @tblResult
		return
		end
		declare @sqlcursor nvarchar(2000),
		 @sqlsearch nvarchar(2000)=''''
		
		if (ISNULL(@StartDate,'''')<>'''')
		begin
		set @sqlsearch=@sqlsearch+'' AND a.PlanStartDate>=''''''+@StartDate+''''''''
	
		end
		if (ISNULL(@EndDate,'''')<>'''')
		begin
		set @sqlsearch=@sqlsearch+'' AND a.PlanEndDate<=''''''+@EndDate+''''''''
	
		end
		if (ISNULL(@ActionName,'''')<>'''')
		begin
		set @sqlsearch=@sqlsearch+'' AND a.ActivityName like ''''%''+@ActionName+''%''''''
		end

set @sqlcursor=''
  SELECT top ''+cast(@PageSize as nvarchar(10))+''
         a.ActivityName,
         a.ReferenceNo,
         b.TableName
		 into ##temp 
  FROM TM_Act_Master a
  LEFT JOIN TM_Act_Instance b ON a.ActivityID=b.ActivityID  WHERE a.ReferenceNo <>''''''''''+@sqlsearch+''
   AND a.ActivityID not in(
  SELECT top ''+cast(@PageIndex*@PageSize as nvarchar(10))+'' a.ActivityID
  FROM TM_Act_Master a
  LEFT JOIN TM_Act_Instance b ON a.ActivityID=b.ActivityID  WHERE a.ReferenceNo <>''''''''''+@sqlsearch+''
   order by a.PlanStartDate DESC)
  order by a.PlanStartDate DESC''

  exec (@sqlcursor)
declare @sqlCount nvarchar(2000)

 set @sqlCount= ''SELECT @ct=count(1)
  FROM TM_Act_Master a
  LEFT JOIN TM_Act_Instance b ON a.ActivityID=b.ActivityID  WHERE a.ReferenceNo <>''''''''''+@sqlsearch


  DECLARE MyCursor
  CURSOR
  FOR
  select * from ##temp

  OPEN MyCursor 
  DECLARE @ActivityName nvarchar(50),
          @ReferenceNo nvarchar(100),
          @TableName nvarchar(2000) 
		  FETCH NEXT
  FROM MyCursor INTO @ActivityName,
                     @ReferenceNo,
                     @TableName WHILE @@FETCH_STATUS =0 BEGIN DECLARE @sql nvarchar(MAX);
SET @sql =''select ''''''+@ReferenceNo+'''''' ReferenceNo,''''''+@ActivityName+'''''' ActivityName,( select count(distinct memberid) from [''+@tablename+'']) SendCount,(select count(1) from TL_WX_MemberSign where ActionCode=''''''+@ReferenceNo+'''''') SignCountAll,(select count(1) from TL_WX_MemberSign where ActionCode=''''''+@ReferenceNo+'''''' and MemberID in (select MemberID from [''+@TableName+''])) SignCountSend,(select count(1) from TL_WX_MemberSign where ActionCode=''''''+@ReferenceNo+'''''' and MemberID not in (select MemberID from [''+@TableName+'']))SignCountMem,(select count(1) from TL_WX_MemberSign where ActionCode=''''''+@ReferenceNo+'''''' and MemberID <>'''''''')SignCountNOTMem''  
insert into @tblResult EXEC (@sql) 
FETCH NEXT
FROM MyCursor INTO @ActivityName,
                   @ReferenceNo,
                   @TableName END
CLOSE MyCursor
DEALLOCATE MyCursor
drop table ##temp
exec sp_executesql @sqlCount,N''@ct int output'',@RecordCount output 
select *   from @tblResult

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_MemViewCreate]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_MemViewCreate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[sp_Sys_MemViewCreate]
AS
  /**********************************
  ----arvarto system-----
  存储过程功能描述：生成TM_Mem_Master,TM_Mem_Ext,TM_Loy_MemExt联合视图
  建 立 人：zyb
  建立时间：2015-03-05 
  修改内容: 

  ***********************************/

BEGIN

----------------视图删除----------------
drop view  V_U_TM_Mem_Info

----------------视图创建------------------

		
DECLARE @sql1 nvarchar(max)
       

    IF OBJECT_ID(N''tempdb.dbo.#TE_View_Table'') IS NOT NULL
            BEGIN
                DROP TABLE #TE_View_Table;
            END;

    IF OBJECT_ID(N''tempdb.dbo.#TE_View_Table1'') IS NOT NULL
            BEGIN
                DROP TABLE #TE_View_Table1;
            END;

    IF OBJECT_ID(N''tempdb.dbo.#TE_View_Table2'') IS NOT NULL
            BEGIN
                DROP TABLE #TE_View_Table2;
            END;

    IF OBJECT_ID(N''tempdb.dbo.#TE_View_Table3'') IS NOT NULL
            BEGIN
                DROP TABLE #TE_View_Table3;
            END;


select a.Table_name, a.colid,a.Column_name, b.FieldAlias 
into #TE_View_Table
from 

(select  a.colid,
        b.name as Table_name, 
        a.name as Column_name,
        c.name as Type_name,
        columnproperty(a.id,a.name,''precision'') as Column_lengh,
        isnull(COLUMNPROPERTY(a.id,a.name,''Scale''),0) Column_decimal 
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (''TM_Mem_Master'') 
  and b.xtype = ''u''
  and b.name<>''dtproperties'')  a
inner join (   select AliasKey FieldName,AliasKey  FieldAlias
			   from TD_Sys_FilterMapping 
			   where Type=''Union'' and   TableName=''TM_Mem_Master''
			   union all           
               select  a.FieldName ,a.FieldAlias
               from TD_SYS_FieldAlias a
               where --IsSingle=1  
			   AliasKey is  null 
               and TableName=''TM_Mem_Master''
             ) b on a.Column_name=b.FieldName

----TM_Mem_Ext
      
               select TableName Table_name, a.FieldName Column_name ,a.FieldAlias
			   into #TE_View_Table1
               from TD_SYS_FieldAlias a
               where --IsSingle=1  
			         AliasKey is  null 
               and TableName=''TM_Mem_Ext''


----TM_Loy_MemExt
      
               select  TableName Table_name,a.FieldName Column_name ,a.FieldAlias
			   into #TE_View_Table2
               from TD_SYS_FieldAlias a
               where --IsSingle=1  
			       AliasKey is  null 
               and TableName=''TM_Loy_MemExt''


---V_Sys_Mem

     SELECT * INTO #TE_View_Table3
	 FROM (
                 select ''V_Sys_Mem'' Table_name, 1 serial ,
			      SQL=STUFF(
                         ( SELECT '','' +( +''a.''+t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table t
                   where t.Table_name=m.Table_name
                   order by t.colid
                   FOR XML PATH('''')), 1, 1, '''')  
                   from #TE_View_Table  m
                   group by Table_name 
				   union all
                    select ''V_Sys_Mem'' Table_name, 2 serial ,
			      SQL=STUFF(
                         ( SELECT '','' +( +''b.''+t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table1 t
                   where t.Table_name=m.Table_name
                 
                   FOR XML PATH('''')), 1, 1, '''')  
                   from #TE_View_Table1  m
                   group by Table_name 
				   union all
                    select ''V_Sys_Mem'' Table_name, 3 serial ,
			      SQL=STUFF(
                         ( SELECT '','' +( +''c.''+t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table2 t
                   where t.Table_name=m.Table_name
                 
                   FOR XML PATH('''')), 1, 1, '''')  
                   from #TE_View_Table2  m
                   group by Table_name ) T 

set @sql1=( 
select 
         sql=''create view V_U_TM_Mem_Info as 
		  select '' +STUFF(
                 ( SELECT '','' +( t.SQL)
                   from #TE_View_Table3 t
                   where t.Table_name=m.Table_name
                   order by t.serial
                   FOR XML PATH('''')
                 ), 1, 1, '''')  
            +'' 
			  from TM_Mem_Master a
              left join TM_Mem_Ext b on a.MemberID=b.MemberID
              left join TM_Loy_MemExt c on a.MemberID=c.MemberID '' 

from #TE_View_Table3  m
group by Table_name )
  


print @sql1
exec(@sql1)



END










' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ReturnAuthValue]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ReturnAuthValue]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[sp_Sys_ReturnAuthValue]
	@dataGroupId int,
	@pageIds nvarchar(100)='''',
	@dataRoleIds nvarchar(100)=''''
AS
  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Sys_ReturnAuthValue 返回角色所对应的类型权限值
  建 立 人：zyb
  建立时间：2015-03-09 
  修改内容: 
  ***********************************/

BEGIN



declare
			@Sql nvarchar(max) = '''',
			@Sql_Search1 nvarchar(max) = ''where 1=1 ''



---清空表值

 truncate table TE_AUTH_TypeFilterValue  

----role控制的store,brand


    if (isnull(@dataGroupId, '''') <> '''' )
	begin  
		set @Sql_Search1 = @Sql_Search1 + '' and dataGroupId= ''+ cast(@dataGroupId as nvarchar(1000))+'' ''
	end

set @Sql=''
insert into TE_AUTH_TypeFilterValue
select distinct a.RangeType,a.RangeValue  
from [dbo].[TM_AUTH_DataLimit]  a
inner join TM_AUTH_Role b on a.HierarchyValue=b.RoleID
where a.HierarchyType=''''role'''' 
  and b.DataGroupID in 
                   (select   subdataGroupId
			        from V_Sys_DataGroupRelation
					  ''+@Sql_Search1+'')
					 				 
  and a.HierarchyValue in  ''+@dataRoleIds+''
  and a.PageID=''+@pageIds+'' ''

 
   print (@sql)
   exec (@sql)


END











' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ReturnAuthValueForAccountChange]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ReturnAuthValueForAccountChange]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[sp_Sys_ReturnAuthValueForAccountChange]
	@dataGroupId int,
	@pageIds nvarchar(100)='''',
	@dataRoleIds nvarchar(100)=''''
AS
  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Sys_ReturnAuthValueForAccountChange 返回角色所对应的类型权限值
  建 立 人：zyb
  建立时间：2015-04-09 
  修改内容: 
  ***********************************/

BEGIN



declare
			@Sql nvarchar(max) = '''',
			@Sql_Search1 nvarchar(max) = ''where 1=1 ''



---清空表值

 truncate table TE_AUTH_TypeFilterValueForAccountChange
 
    

----role控制的store,brand


    if (isnull(@dataGroupId, '''') <> '''' )
	begin  
		set @Sql_Search1 = @Sql_Search1 + '' and dataGroupId= ''+ cast(@dataGroupId as nvarchar(1000))+'' ''
	end

set @Sql=''
insert into TE_AUTH_TypeFilterValueForAccountChange
select distinct a.RangeType,a.RangeValue  
from [dbo].[TM_AUTH_DataLimit]  a
inner join TM_AUTH_Role b on a.HierarchyValue=b.RoleID
where a.HierarchyType=''''role'''' 
  and b.DataGroupID in 
                   (select   subdataGroupId
			        from V_Sys_DataGroupRelation
					  ''+@Sql_Search1+'')
					 				 
  and a.HierarchyValue in  ''+@dataRoleIds+''
  and a.PageID=''+@pageIds+'' ''

 
   print (@sql)
   exec (@sql)


END











' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ReturnAvaiAliasColumn]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ReturnAvaiAliasColumn]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[sp_Sys_ReturnAvaiAliasColumn]
@TableName nvarchar(50),
@ExtCode nvarchar(20),
@AliasType nvarchar(20),
@AliasKey nvarchar(50),
@AliasSubKey nvarchar(50)
AS
  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Sys_ReturnAvaiAliasColumn 返回可用的别名列
  建 立 人：zyb
  建立时间：2015-2-12
  修改内容: 20150304 zyb 增加传参@AliasType，@AliasKey，@AliasSubKey
            20150328 zyb 增加无返回值自动创建字段（建议C+本表最大字段数），数据库可能存在已超过字段数的动态字段；
  ***********************************/

 


BEGIN

if exists ( 
select a.Column_name
from 
(select b.name as Table_name, 
        a.name as Column_name,
        c.name as Type_name,
        columnproperty(a.id,a.name,''precision'') as Column_lengh,
        isnull(COLUMNPROPERTY(a.id,a.name,''Scale''),0) Column_decimal 
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (@TableName) 
  and b.xtype = ''u''
  and b.name<>''dtproperties''
  and a.name not in (''MemberID'',''AddedDate'',''AddedUser'',''ModifiedDate'',''ModifiedUser'',''DataGroupID'')) a

inner join  
(select OptionValue,
    case when OptionText=''字符串(20)''      then ''nvarchar'' 
         when OptionText=''字符串(100)''     then ''nvarchar'' 
	     when OptionText=''整型''            then ''int'' 
	     when OptionText=''布尔型''          then ''bit'' 
	     when OptionText=''日期型(长)''      then ''datetime'' 
	     when OptionText=''日期型(短)''      then ''date'' 
	     when OptionText=''十进制(2位小数)'' then ''decimal'' 
	     when OptionText=''十进制(4位小数)'' then ''decimal''  
	     end  Type_name,
	 case when OptionText=''字符串(20)''      then 20 
          when OptionText=''字符串(100)''     then 100 
	      when OptionText=''整型''            then 10 
	      when OptionText=''布尔型''          then 1
	      when OptionText=''日期型(长)''      then 23 
	      when OptionText=''日期型(短)''      then 10 
	      when OptionText=''十进制(2位小数)'' then 18 
	      when OptionText=''十进制(4位小数)'' then 18 
	      end  Column_lengh,
	 case when OptionText=''字符串(20)''      then 0 
          when OptionText=''字符串(100)''     then 0 
	      when OptionText=''整型''            then 0 
	      when OptionText=''布尔型''          then 0
	      when OptionText=''日期型(长)''      then 3 
	      when OptionText=''日期型(短)''      then 0 
	      when OptionText=''十进制(2位小数)'' then 2 
	      when OptionText=''十进制(4位小数)'' then 4 
	      end  Column_decimal
 from [dbo].[TD_Sys_BizOption] 
 where OptionType=''DBFieldType'' and  OptionValue in (@ExtCode) ) b  
on a.Type_name=b.Type_name and a.Column_lengh=b.Column_lengh and a.Column_decimal=b.Column_decimal 
where Column_name not in (select FieldName 
                          from [dbo].[TD_Sys_FieldAlias]  
						  where Tablename in (@TableName)
						    and isnull(AliasType,'''') =@AliasType
							and isnull(AliasKey,'''')  =isnull(@AliasKey,'''')
							and isnull(AliasSubKey,'''') =isnull(@AliasSubKey,'''') ) )


begin 

 
select a.Column_name
from 
(select b.name as Table_name, 
        a.name as Column_name,
        c.name as Type_name,
        columnproperty(a.id,a.name,''precision'') as Column_lengh,
        isnull(COLUMNPROPERTY(a.id,a.name,''Scale''),0) Column_decimal 
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (@TableName) 
  and b.xtype = ''u''
  and b.name<>''dtproperties''
  and a.name not in (''MemberID'',''AddedDate'',''AddedUser'',''ModifiedDate'',''ModifiedUser'',''DataGroupID'')) a

inner join  
(select OptionValue,
    case when OptionText=''字符串(20)''      then ''nvarchar'' 
         when OptionText=''字符串(100)''     then ''nvarchar'' 
	     when OptionText=''整型''            then ''int'' 
	     when OptionText=''布尔型''          then ''bit'' 
	     when OptionText=''日期型(长)''      then ''datetime'' 
	     when OptionText=''日期型(短)''      then ''date'' 
	     when OptionText=''十进制(2位小数)'' then ''decimal'' 
	     when OptionText=''十进制(4位小数)'' then ''decimal''  
	     end  Type_name,
	 case when OptionText=''字符串(20)''      then 20 
          when OptionText=''字符串(100)''     then 100 
	      when OptionText=''整型''            then 10 
	      when OptionText=''布尔型''          then 1
	      when OptionText=''日期型(长)''      then 23 
	      when OptionText=''日期型(短)''      then 10 
	      when OptionText=''十进制(2位小数)'' then 18 
	      when OptionText=''十进制(4位小数)'' then 18 
	      end  Column_lengh,
	 case when OptionText=''字符串(20)''      then 0 
          when OptionText=''字符串(100)''     then 0 
	      when OptionText=''整型''            then 0 
	      when OptionText=''布尔型''          then 0
	      when OptionText=''日期型(长)''      then 3 
	      when OptionText=''日期型(短)''      then 0 
	      when OptionText=''十进制(2位小数)'' then 2 
	      when OptionText=''十进制(4位小数)'' then 4 
	      end  Column_decimal
 from [dbo].[TD_Sys_BizOption] 
 where OptionType=''DBFieldType'' and  OptionValue in (@ExtCode) ) b  
on a.Type_name=b.Type_name and a.Column_lengh=b.Column_lengh and a.Column_decimal=b.Column_decimal 
where Column_name not in (select FieldName 
                          from [dbo].[TD_Sys_FieldAlias]  
						  where Tablename in (@TableName)
						    and isnull(AliasType,'''') =@AliasType
							and isnull(AliasKey,'''')  =isnull(@AliasKey,'''')
							and isnull(AliasSubKey,'''') =isnull(@AliasSubKey,'''') )  
end 
else 
begin

declare @sql nvarchar(1000),
        @colid nvarchar(6)


select @colid=cast(max(a.colid)+1  as nvarchar(6) )
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (@TableName) 
  and b.xtype = ''u''
  and b.name<>''dtproperties''

     
set @sql=(
select 
''alter table''+''  ''+ @TableName +'' add  ''
+ (case when @ExtCode=''1''  then  ''Str_Attr_''      
     when @ExtCode=''2''  then  ''Str_Attr_''      
     when @ExtCode=''3''  then  ''Int_Attr_''      
     when @ExtCode=''4''  then  ''Bool_Attr_''      
     when @ExtCode=''5''  then  ''Date_Attr_''     
     when @ExtCode=''6''  then  ''Date_Attr_''    
     when @ExtCode=''7''  then  ''Dec_Attr_''       
     when @ExtCode=''8''  then  ''Dec_Attr_''end)+@colid  +''  ''+           
+ case when @ExtCode=''1''  then  ''nvarchar(20)''
    when @ExtCode=''2''  then  ''nvarchar(100)''
    when @ExtCode=''3''  then  ''int''
    when @ExtCode=''4''  then  ''bit''
    when @ExtCode=''5''  then  ''datetime''
    when @ExtCode=''6''  then  ''date''
    when @ExtCode=''7''  then  ''decimal(18,2)''
    when @ExtCode=''8''  then  ''decimal(18,4)''   end  
+ '' null '' )


exec(@sql)  


select a.Column_name
from 
(select b.name as Table_name, 
        a.name as Column_name,
        c.name as Type_name,
        columnproperty(a.id,a.name,''precision'') as Column_lengh,
        isnull(COLUMNPROPERTY(a.id,a.name,''Scale''),0) Column_decimal 
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (@TableName) 
  and b.xtype = ''u''
  and b.name<>''dtproperties''
  and a.name not in (''MemberID'',''AddedDate'',''AddedUser'',''ModifiedDate'',''ModifiedUser'',''DataGroupID'')) a

inner join  
(select OptionValue,
    case when OptionText=''字符串(20)''      then ''nvarchar'' 
         when OptionText=''字符串(100)''     then ''nvarchar'' 
	     when OptionText=''整型''            then ''int'' 
	     when OptionText=''布尔型''          then ''bit'' 
	     when OptionText=''日期型(长)''      then ''datetime'' 
	     when OptionText=''日期型(短)''      then ''date'' 
	     when OptionText=''十进制(2位小数)'' then ''decimal'' 
	     when OptionText=''十进制(4位小数)'' then ''decimal''  
	     end  Type_name,
	 case when OptionText=''字符串(20)''      then 20 
          when OptionText=''字符串(100)''     then 100 
	      when OptionText=''整型''            then 10 
	      when OptionText=''布尔型''          then 1
	      when OptionText=''日期型(长)''      then 23 
	      when OptionText=''日期型(短)''      then 10 
	      when OptionText=''十进制(2位小数)'' then 18 
	      when OptionText=''十进制(4位小数)'' then 18 
	      end  Column_lengh,
	 case when OptionText=''字符串(20)''      then 0 
          when OptionText=''字符串(100)''     then 0 
	      when OptionText=''整型''            then 0 
	      when OptionText=''布尔型''          then 0
	      when OptionText=''日期型(长)''      then 3 
	      when OptionText=''日期型(短)''      then 0 
	      when OptionText=''十进制(2位小数)'' then 2 
	      when OptionText=''十进制(4位小数)'' then 4 
	      end  Column_decimal
 from [dbo].[TD_Sys_BizOption] 
 where OptionType=''DBFieldType'' and  OptionValue in (@ExtCode) ) b  
on a.Type_name=b.Type_name and a.Column_lengh=b.Column_lengh and a.Column_decimal=b.Column_decimal 
where Column_name not in (select FieldName 
                          from [dbo].[TD_Sys_FieldAlias]  
						  where Tablename in (@TableName)
						    and isnull(AliasType,'''') =@AliasType
							and isnull(AliasKey,'''')  =isnull(@AliasKey,'''')
							and isnull(AliasSubKey,'''') =isnull(@AliasSubKey,'''') )  
end 




END












' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Sys_ViewCreate]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Sys_ViewCreate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[sp_Sys_ViewCreate]
AS
  /**********************************
  ----arvarto system-----
  存储过程功能描述：sp_Sys_ViewCreate 生成视图
  建 立 人：zyb
  建立时间：2014-12-05 
  修改内容: zyb 20150304 增加子视图的创建以及非动态字段的配置表，级别对应匹配的字段配置；
            zyb 20150305 子视图增加字段的关联以及缺少的参数新增，维护字段改为只需维护过滤字段，取值字段到别名表中取；

  ***********************************/

BEGIN

----------------初始化时视图及视图语句表删除-----------------


DECLARE ViewCursor CURSOR 
FOR 
   select ViewTable  from  TM_Sys_ViewTable 

  
--打开一个游标	
OPEN ViewCursor
--循环一个游标
DECLARE @ViewTable nvarchar(200) 	
FETCH NEXT FROM  ViewCursor INTO @ViewTable
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql nvarchar(max)
      
set @sql=''drop view ''+@ViewTable+'' ''

print @sql
exec(@sql)

FETCH NEXT FROM  ViewCursor INTO @ViewTable
END	
--关闭游标
CLOSE ViewCursor
--释放资源
DEALLOCATE ViewCursor


---对历史的数据做记录
truncate table TE_Sys_ViewTable
insert into TE_Sys_ViewTable  select * from TM_Sys_ViewTable

---清空数据记录
truncate table TM_Sys_ViewTable


----------------单表单视图的创建及数据插入-----------------------
--循环一个游标


DECLARE SingleCursor CURSOR 
FOR 
   select distinct  TableName
   from  TD_SYS_FieldAlias a
   where  ----IsSingle=1 
   AliasKey is  null 
  
--打开一个游标	
OPEN SingleCursor
--循环一个游标
DECLARE @TableName nvarchar(100) 	
FETCH NEXT FROM  SingleCursor INTO @TableName
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql1 nvarchar(max)
       

    IF OBJECT_ID(N''tempdb.dbo.#TE_View_Table'') IS NOT NULL
            BEGIN
                DROP TABLE #TE_View_Table;
            END;


select a.Table_name, a.colid,a.Column_name, b.FieldAlias 
into #TE_View_Table
from 

(select  a.colid,
        b.name as Table_name, 
        a.name as Column_name,
        c.name as Type_name,
        columnproperty(a.id,a.name,''precision'') as Column_lengh,
        isnull(COLUMNPROPERTY(a.id,a.name,''Scale''),0) Column_decimal 
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (@TableName) 
  and b.xtype = ''u''
  and b.name<>''dtproperties'')  a
inner join (   select AliasKey FieldName,AliasKey  FieldAlias
			   from TD_Sys_FilterMapping 
			   where Type=''Union'' and   TableName=@TableName
			   union all           
               select  a.FieldName ,a.FieldAlias
               from TD_SYS_FieldAlias a
               where --IsSingle=1  
			   AliasKey is  null 
               and TableName=@TableName
             ) b on a.Column_name=b.FieldName 

set @sql1=(
select 
         sql=''create view V_S_''+m.Table_name+ '' as 
		  select '' +STUFF(
                 ( SELECT '','' +( t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table t
                   where t.Table_name=m.Table_name
                   order by t.colid
                   FOR XML PATH('''')
                 ), 1, 1, '''')  
            +'' from '' + m.Table_name

from #TE_View_Table  m
group by Table_name )



print @sql1
exec(@sql1)


insert into TM_Sys_ViewTable
(TableName,ViewTable,IsSingle,ViewSQL,Addeddate,AddedUser,ModifiedDate,ModifiedUser) 
select m.Table_name,''V_S_''+m.Table_name,1 ,
         sql=''create view V_S_''+m.Table_name+ '' as 
		  select '' +STUFF(
                 ( SELECT '','' +( t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table t
                   where t.Table_name=m.Table_name
                   order by t.colid
                   FOR XML PATH('''')
                 ), 1, 1, '''')  
            +'' from '' + m.Table_name,
		getdate(),''system'',getdate(),''system''

from #TE_View_Table  m
group by Table_name 

FETCH NEXT FROM  SingleCursor INTO @TableName
END	
--关闭游标
CLOSE SingleCursor
--释放资源
DEALLOCATE SingleCursor


  -----------------单表1级多视图的创建及数据插入-----------------------

DECLARE MultipleCursor CURSOR 
FOR 
   select distinct  TableName,AliasKey 
   from  TD_SYS_FieldAlias a
   where --- IsSingle=0 and 
          AliasKey is not null  and AliasSubKey is null 
  
--打开一个游标	
OPEN MultipleCursor
--循环一个游标
DECLARE @TableName1 nvarchar(100) ,
        @AliasKey1 	nvarchar(100) 
FETCH NEXT FROM  MultipleCursor INTO @TableName1,@AliasKey1
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql2 nvarchar(max)
 

    IF OBJECT_ID(N''tempdb.dbo.#TE_View_Table1'') IS NOT NULL
            BEGIN
                DROP TABLE #TE_View_Table1;
            END;


select a.Table_name, a.colid,a.Column_name, b.FieldAlias ,b.AliasKey,b.AliasKeyFilter
into #TE_View_Table1
from 

(select  a.colid,
        b.name as Table_name, 
        a.name as Column_name,
        c.name as Type_name,
        columnproperty(a.id,a.name,''precision'') as Column_lengh,
        isnull(COLUMNPROPERTY(a.id,a.name,''Scale''),0) Column_decimal 
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (@TableName1) 
  and b.xtype = ''u''
  and b.name<>''dtproperties'')  a
inner join (   select AliasKey FieldName,AliasKey  FieldAlias,'''' AliasKey,'''' AliasKeyFilter
			   from TD_Sys_FilterMapping 
			   where Type=''Union'' and   TableName=@TableName1
			   union all 
               select  a.FieldName ,a.FieldAlias,a.AliasKey,b.AliasKeyFilter
               from TD_SYS_FieldAlias a
			   inner join (select * 
			               from TD_Sys_FilterMapping a 
			               where Type=''Filter'' and level=''1'') b
			               on a.TableName=b.TableName  
               where --IsSingle=0  
			       a.AliasKey is not null  and a.AliasSubKey is null 
               and a.TableName=@TableName1 
			   and a.AliasKey=@AliasKey1 ) b on a.Column_name=b.FieldName

set @sql2=( 
select 
         sql=''create view V_M_''+m.Table_name+''_''+max(m.AliasKey)+ '' as 
		  select '' +STUFF(
                 ( SELECT '','' +( t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table1 t
                   where t.Table_name=m.Table_name
                   order by t.colid
                   FOR XML PATH('''')
                 ), 1, 1, '''')  
            +'' from '' + m.Table_name+ '' where ''+ max(m.AliasKeyFilter)+''=''+''''''''+@AliasKey1+''''''''

from #TE_View_Table1  m
group by Table_name )



print @sql2
exec(@sql2)


insert into TM_Sys_ViewTable
(TableName,ViewTable,IsSingle,ViewSQL,Addeddate,AddedUser,ModifiedDate,ModifiedUser) 
select m.Table_name,''V_M_''+m.Table_name+''_''+max(m.AliasKey),0 ,
         sql=''create view V_M_''+m.Table_name+''_''+max(m.AliasKey)+ '' as
		  select '' +STUFF(
                 ( SELECT '','' +( t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table1 t
                   where t.Table_name=m.Table_name
                   order by t.colid
                   FOR XML PATH('''')
                 ), 1, 1, '''')  
            +'' from '' + m.Table_name+ '' where ''+ max(m.AliasKeyFilter)+''=''+''''''''+@AliasKey1+'''''''',
		getdate(),''system'',getdate(),''system''

from #TE_View_Table1  m
group by Table_name 

FETCH NEXT FROM  MultipleCursor INTO @TableName1,@AliasKey1
END	
--关闭游标
CLOSE MultipleCursor
--释放资源
DEALLOCATE MultipleCursor


  -----------------单表2级多视图子视图的创建及数据插入-----------------------

DECLARE MultipleSubCursor CURSOR 
FOR 
   select distinct  TableName,AliasKey ,AliasSubKey
   from  TD_SYS_FieldAlias a
   where --- IsSingle=0 and 
          AliasKey is not null  and AliasSubKey is not null 
  
--打开一个游标	
OPEN MultipleSubCursor
--循环一个游标
DECLARE @TableName2 nvarchar(100) ,
        @AliasKey2 	nvarchar(100) ,
		@AliasSubKey2 	nvarchar(100) 
FETCH NEXT FROM  MultipleSubCursor INTO @TableName2,@AliasKey2,@AliasSubKey2
WHILE @@FETCH_STATUS =0	
BEGIN		
DECLARE @sql3 nvarchar(max)
 

    IF OBJECT_ID(N''tempdb.dbo.#TE_View_Table2'') IS NOT NULL
            BEGIN
                DROP TABLE #TE_View_Table2;
            END;


select a.Table_name, a.colid,a.Column_name, b.FieldAlias ,b.AliasKey,b.AliasKeyFilter,b.AliasSubKey,b.AliasSubKeyFilter
into #TE_View_Table2
from 

(select  a.colid,
        b.name as Table_name, 
        a.name as Column_name,
        c.name as Type_name,
        columnproperty(a.id,a.name,''precision'') as Column_lengh,
        isnull(COLUMNPROPERTY(a.id,a.name,''Scale''),0) Column_decimal 
from dbo.syscolumns a
inner join dbo.sysobjects b on a.id = b.id
left join dbo.systypes c on a.xtype = c.xusertype
where b.name in (@TableName2) 
  and b.xtype = ''u''
  and b.name<>''dtproperties'')  a
inner join (   select AliasKey FieldName,AliasKey  FieldAlias,'''' AliasKey,'''' AliasKeyFilter,'''' AliasSubKey,'''' AliasSubKeyFilter
			   from TD_Sys_FilterMapping 
			   where Type=''Union'' and   TableName=@TableName2
			   union all 
               select  a.FieldName ,a.FieldAlias,a.AliasKey,b.AliasKeyFilter,a.AliasSubKey,b.AliasSubKeyFilter
               from TD_SYS_FieldAlias a
			   inner join (select * from TD_Sys_FilterMapping where Type=''Filter'' and level=''2'') b
			               on a.TableName=b.TableName 
               where --IsSingle=0  
			       a.AliasKey is not null  and a.AliasSubKey is not null 
               and a.TableName=@TableName2 
			   and a.AliasKey=@AliasKey2
			   and a.AliasSubKey=@AliasSubKey2 ) b on a.Column_name=b.FieldName

set @sql3=( 
select 
         sql=''create view V_M_''+m.Table_name+''_''+max(m.AliasKey)+''_''+max(m.AliasSubKey)+ '' as 
		  select '' +STUFF(
                 ( SELECT '','' +( t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table2 t
                   where t.Table_name=m.Table_name
                   order by t.colid
                   FOR XML PATH('''')
                 ), 1, 1, '''')  
            +'' from '' + m.Table_name
			+ '' where ''
			+ max(m.AliasKeyFilter)+''=''+''''''''+@AliasKey2+''''''''
			+ '' and ''
			+max(m.AliasSubKeyFilter)+''=''+''''''''+@AliasSubKey2+''''''''

from #TE_View_Table2  m
group by Table_name )



print @sql3
exec(@sql3)


insert into TM_Sys_ViewTable
(TableName,ViewTable,IsSingle,ViewSQL,Addeddate,AddedUser,ModifiedDate,ModifiedUser) 
select m.Table_name,''V_M_''+m.Table_name+''_''+max(m.AliasKey)+''_''+max(m.AliasSubKey),0 ,
         sql=''create view V_M_''+m.Table_name+''_''+max(m.AliasKey)+''_''+max(m.AliasSubKey)+ '' as
		  select '' +STUFF(
                 ( SELECT '','' +( t.Column_name+'' as ''+t.FieldAlias)
                   from #TE_View_Table2 t
                   where t.Table_name=m.Table_name
                   order by t.colid
                   FOR XML PATH('''')
                 ), 1, 1, '''')  
            +'' from '' + m.Table_name
			+ '' where ''
			+ max(m.AliasKeyFilter)+''=''+''''''''+@AliasKey2+''''''''
			+ '' and ''
			+max(m.AliasSubKeyFilter)+''=''+''''''''+@AliasSubKey2+'''''''',
		getdate(),''system'',getdate(),''system''

from #TE_View_Table2  m
group by Table_name 

FETCH NEXT FROM  MultipleSubCursor INTO @TableName2,@AliasKey2,@AliasSubKey2
END	
--关闭游标
CLOSE MultipleSubCursor
--释放资源
DEALLOCATE MultipleSubCursor




----与历史数据记录做比对，更新上次的viewsql，并与现在的做比对，是否发生了变更；

update TM_Sys_ViewTable 
set LastViewSql =a.LastViewSql,
    IsChange=a.IsChange
from  (select  a.ViewTable,b.ViewSQL  LastViewSql,
               case when b.ViewTable is null then 1 
			        when b.ViewTable is not  null and a.ViewSQL=b.ViewSQL then 0
					when b.ViewTable is not  null and a.ViewSQL<>b.ViewSQL then 1 end IsChange
       from TM_Sys_ViewTable a
	   left join TE_Sys_ViewTable b on a.ViewTable=b.ViewTable ) a
where TM_Sys_ViewTable.ViewTable=a.ViewTable

END










' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[IntTo36HexStr]    Script Date: 2015/12/24 14:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IntTo36HexStr]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[IntTo36HexStr](@value INT) 
RETURNS VARCHAR(6) 
AS
BEGIN
DECLARE @seq CHAR(36) 
DECLARE @result VARCHAR(6) 
DECLARE @digit CHAR(1) 
SET @seq=''0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ''
SET @result=SUBSTRING(@seq, (@value%36)+1, 1) 
WHILE @value>0
BEGIN
SET @digit=SUBSTRING(@seq, ((@value/36)%36)+1, 1) 
SET @value=@value/36
IF @value<>0
SET @result=@digit+@result
END

while len(@result)<6
begin
set @result=''0''+@result
end
RETURN @result
END

' 
END

GO
