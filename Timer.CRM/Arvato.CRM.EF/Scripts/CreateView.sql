USE [Kidsland_CRM]
GO
/****** Object:  View [dbo].[V_U_TM_Mem_Info]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_U_TM_Mem_Info]'))
DROP VIEW [dbo].[V_U_TM_Mem_Info]
GO
/****** Object:  View [dbo].[V_Sys_DataGroupRelation]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Sys_DataGroupRelation]'))
DROP VIEW [dbo].[V_Sys_DataGroupRelation]
GO
/****** Object:  View [dbo].[V_S_TM_Mem_Master]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_S_TM_Mem_Master]'))
DROP VIEW [dbo].[V_S_TM_Mem_Master]
GO
/****** Object:  View [dbo].[V_S_TM_Mem_Ext]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_S_TM_Mem_Ext]'))
DROP VIEW [dbo].[V_S_TM_Mem_Ext]
GO
/****** Object:  View [dbo].[V_S_TM_Loy_MemExt]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_S_TM_Loy_MemExt]'))
DROP VIEW [dbo].[V_S_TM_Loy_MemExt]
GO
/****** Object:  View [dbo].[V_M_TM_Mem_TradeDetail_sales_product]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_TradeDetail_sales_product]'))
DROP VIEW [dbo].[V_M_TM_Mem_TradeDetail_sales_product]
GO
/****** Object:  View [dbo].[V_M_TM_Mem_TradeDetail_sales_payment]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_TradeDetail_sales_payment]'))
DROP VIEW [dbo].[V_M_TM_Mem_TradeDetail_sales_payment]
GO
/****** Object:  View [dbo].[V_M_TM_Mem_Trade_sales]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_Trade_sales]'))
DROP VIEW [dbo].[V_M_TM_Mem_Trade_sales]
GO
/****** Object:  View [dbo].[V_M_TM_Mem_Trade_gift]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_Trade_gift]'))
DROP VIEW [dbo].[V_M_TM_Mem_Trade_gift]
GO
/****** Object:  View [dbo].[V_M_TM_Mem_Trade_coupon]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_Trade_coupon]'))
DROP VIEW [dbo].[V_M_TM_Mem_Trade_coupon]
GO
/****** Object:  View [dbo].[V_M_TM_Mem_SubExt_kid]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_SubExt_kid]'))
DROP VIEW [dbo].[V_M_TM_Mem_SubExt_kid]
GO
/****** Object:  View [dbo].[V_M_TM_Mem_SubExt_contact]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_SubExt_contact]'))
DROP VIEW [dbo].[V_M_TM_Mem_SubExt_contact]
GO
/****** Object:  View [dbo].[V_Loy_MemberAccount]    Script Date: 2016/1/3 1:01:51 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Loy_MemberAccount]'))
DROP VIEW [dbo].[V_Loy_MemberAccount]
GO
/****** Object:  View [dbo].[V_Loy_MemberAccount]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Loy_MemberAccount]'))
EXEC dbo.sp_executesql @statement = N'
create View [dbo].[V_Loy_MemberAccount] as 
 /**********************************
  ----arvarto system-----
  存储过程功能描述：应用于忠诚度积分积点账户可用的查询
  建 立 人：zyb
  建立时间：2015-05-19
  修 改 人：

  ***********************************/ 
select MemberID,
[RechargeAccountValue], 
[PointAccountValue],    
[CreditAccountValue],   
[CWAccountValue],       
[BXAccountValue],       
[SaleAccountValue],     
[ServiceAccountValue] 

from (
select memberid,
case when AccountType=1	then ''RechargeAccountValue''           
     when AccountType=2	then ''PointAccountValue''           
     when AccountType=3	then ''CreditAccountValue''           
     when AccountType=4	then ''CWAccountValue''         
     when AccountType=5	then ''BXAccountValue''         
     when AccountType=6	then ''SaleAccountValue''       
     when AccountType=7	then ''ServiceAccountValue''   end AccountType,Value1
from  TM_Mem_Account ) a
pivot ( 
 max(Value1)  for AccountType in (

[RechargeAccountValue], 
[PointAccountValue],    
[CreditAccountValue],   
[CWAccountValue],       
[BXAccountValue],       
[SaleAccountValue],     
[ServiceAccountValue])) b 







' 
GO
/****** Object:  View [dbo].[V_M_TM_Mem_SubExt_contact]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_SubExt_contact]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_M_TM_Mem_SubExt_contact] as 
		  select MemberSubExtID as MemberSubExtID,ExtType as ExtType,MemberID as MemberID,Str_Attr_1 as ContactName,Str_Attr_2 as ContactGender,Str_Attr_3 as ContactTel,Str_Attr_4 as ContactMobile,Str_Attr_5 as ContactEmail,Str_Attr_6 as ContactProvince,Str_Attr_7 as ContactCity,Str_Attr_8 as ContactDistrict,Str_Attr_9 as ContactZip,Str_Attr_10 as ContactJob,Str_Attr_11 as LovingCommunication,Str_Attr_12 as BestContactTime1,Str_Attr_13 as BestContactTime2,Str_Attr_14 as ContactPosition,Str_Attr_15 as ContactIdNo,Str_Attr_16 as ContactCertificateType,Str_Attr_17 as ContactAddress,Str_Attr_18 as ContactType,Str_Attr_19 as ContactProvinceName,Str_Attr_20 as ContactCityName,Str_Attr_21 as ContactDistrictName,Bool_Attr_50 as ContactHaveTrade from TM_Mem_SubExt where ExtType=''contact''' 
GO
/****** Object:  View [dbo].[V_M_TM_Mem_SubExt_kid]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_SubExt_kid]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_M_TM_Mem_SubExt_kid] as 
		  select MemberSubExtID as MemberSubExtID,ExtType as ExtType,MemberID as MemberID,Str_Attr_1 as BabyName,Str_Attr_2 as BabyGender,Str_Attr_3 as BabyHeight,Str_Attr_4 as BabyWeight,Date_Attr_1 as BabyBrithday from TM_Mem_SubExt where ExtType=''kid''' 
GO
/****** Object:  View [dbo].[V_M_TM_Mem_Trade_coupon]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_Trade_coupon]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_M_TM_Mem_Trade_coupon] as 
		  select TradeID as TradeID,DataGroupID as DataGroupID,TradeCode as TradeCode,TradeType as TradeType,RefTradeID as RefTradeID,RefTradeType as RefTradeType,MemberID as MemberID,NeedLoyCompute as NeedLoyCompute,NoNeedLoyComputeReaseon as NoNeedLoyComputeReaseon,Str_Attr_1 as CouponType,Str_Attr_2 as ExchangeType,Str_Attr_3 as ActName,Str_Attr_4 as CouponCodeTrade,Dec_Attr_1 as UseIntByCoupon,AddedDate as AddedDate,AddedUser as AddedUser,ModifiedDate as ModifiedDate,ModifiedUser as ModifiedUser from TM_Mem_Trade where TradeType=''coupon''' 
GO
/****** Object:  View [dbo].[V_M_TM_Mem_Trade_gift]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_Trade_gift]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_M_TM_Mem_Trade_gift] as 
		  select TradeID as TradeID,DataGroupID as DataGroupID,TradeCode as TradeCode,TradeType as TradeType,RefTradeID as RefTradeID,RefTradeType as RefTradeType,MemberID as MemberID,NeedLoyCompute as NeedLoyCompute,NoNeedLoyComputeReaseon as NoNeedLoyComputeReaseon,Str_Attr_1 as ProductCodeGift,Str_Attr_2 as ProductNameGift,Str_Attr_3 as IntergralGift,Dec_Attr_1 as SourcePriceGift,Int_Attr_1 as CountsGift,AddedDate as AddedDate,AddedUser as AddedUser,ModifiedDate as ModifiedDate,ModifiedUser as ModifiedUser from TM_Mem_Trade where TradeType=''gift''' 
GO
/****** Object:  View [dbo].[V_M_TM_Mem_Trade_sales]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_Trade_sales]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_M_TM_Mem_Trade_sales] as 
		  select TradeID as TradeID,DataGroupID as DataGroupID,TradeCode as TradeCode,TradeType as TradeType,RefTradeID as RefTradeID,RefTradeType as RefTradeType,MemberID as MemberID,NeedLoyCompute as NeedLoyCompute,NoNeedLoyComputeReaseon as NoNeedLoyComputeReaseon,Str_Attr_1 as BillNOSales,Str_Attr_2 as StoreCodeSales,Str_Attr_3 as MobileNumberSales,Str_Attr_4 as MarketNumberSales,Str_Attr_5 as MemoSales,Str_Attr_6 as SourceBillNOSales,Str_Attr_7 as GiftCouponNoSales,Str_Attr_8 as PayMethodSales,Str_Attr_9 as TradeSource,Str_Attr_10 as TradeChannelCode,Str_Attr_11 as StoreBrandTrade,Date_Attr_1 as ListDateSales,Dec_Attr_1 as MarketDiscountSales,Dec_Attr_2 as StandardAmountSales,Dec_Attr_3 as MarketDiscountRateSales,Dec_Attr_4 as DiscountAmountSales,Dec_Attr_5 as Amount,Dec_Attr_6 as Discount,Dec_Attr_7 as TotalMoneySales,Dec_Attr_8 as IntergralAccountingSales,Dec_Attr_9 as TradeAmoutSales,Int_Attr_1 as BirthdayprivilegeQtySales,Int_Attr_2 as IntegralRedemptionQtySales,Int_Attr_3 as PromotionGoodsQtySales,Int_Attr_4 as QuantitySales,Int_Attr_5 as ConsumeIntegralSales,Int_Attr_6 as TotalAmountTrade_Int,AddedDate as AddedDate,AddedUser as AddedUser,ModifiedDate as ModifiedDate,ModifiedUser as ModifiedUser,Str_Attr_182 as SalesTradeType from TM_Mem_Trade where TradeType=''sales''' 
GO
/****** Object:  View [dbo].[V_M_TM_Mem_TradeDetail_sales_payment]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_TradeDetail_sales_payment]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_M_TM_Mem_TradeDetail_sales_payment] as 
		  select TradeDetailID as TradeDetailID,TradeDetailType as TradeDetailType,TradeID as TradeID,TradeType as TradeType,Str_Attr_1 as PmtCodePayment,Str_Attr_2 as PmtNamePayment,Str_Attr_3 as PmtCardNoPayment,Str_Attr_4 as MemoPayment,Str_Attr_5 as CRMPayType,Dec_Attr_1 as AmountPayment,Dec_Attr_2 as ReceivedAmountPayment,Int_Attr_1 as IntegralCostPayment,Bool_Attr_1 as IsReturn from TM_Mem_TradeDetail where TradeType=''sales'' and TradeDetailType=''payment''' 
GO
/****** Object:  View [dbo].[V_M_TM_Mem_TradeDetail_sales_product]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_M_TM_Mem_TradeDetail_sales_product]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_M_TM_Mem_TradeDetail_sales_product] as 
		  select TradeDetailID as TradeDetailID,TradeDetailType as TradeDetailType,TradeID as TradeID,TradeType as TradeType,Str_Attr_1 as GoodsCodeProduct,Str_Attr_2 as ColorCodeProduct,Str_Attr_3 as SizeCodeProduct,Str_Attr_4 as DiscountBillNOProduct,Str_Attr_5 as SourceBillNoProduct,Str_Attr_6 as ChangeReasonProduct,Str_Attr_7 as ActivityCodeProduct,Str_Attr_8 as ActivityNameProduct,Str_Attr_9 as MemoProduct,Str_Attr_10 as SourceProduct,Str_Attr_11 as PromotionCodeProduct,Str_Attr_12 as ProductBrandName,Str_Attr_13 as ProductTypeName,Str_Attr_14 as ProductLineName1,Str_Attr_15 as ProductLineName2,Str_Attr_16 as ProductStatusName,Str_Attr_17 as UnitPriceTypeName,Str_Attr_18 as SinglePriceTypeName,Dec_Attr_1 as ReferencePriceProduct,Dec_Attr_2 as PriceProduct,Dec_Attr_3 as DiscountProduct,Dec_Attr_4 as QuantityProduct,Dec_Attr_5 as ReferenceAmountProduct,Dec_Attr_6 as AmountProduct,Dec_Attr_7 as MarketDiscProduct,Int_Attr_1 as StatusProduct from TM_Mem_TradeDetail where TradeType=''sales'' and TradeDetailType=''product''' 
GO
/****** Object:  View [dbo].[V_S_TM_Loy_MemExt]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_S_TM_Loy_MemExt]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_S_TM_Loy_MemExt] as 
		  select MemberID as MemberID,Str_Attr_1 as OftenBuyTime,Str_Attr_2 as MemberActivity,Dec_Attr_1 as HistoryConsumeAmount,Dec_Attr_2 as HistoryPoint,Dec_Attr_3 as ConsumptionYearly,Dec_Attr_4 as ActConsumption,Dec_Attr_5 as ActConsumption_3,Dec_Attr_6 as ActConsumptionCounts,Dec_Attr_7 as LastConsumeMoney,Dec_Attr_8 as EnableIntegrade,Dec_Attr_9 as ActConsumption_24,Dec_Attr_10 as CumulativeIntegral_6,Dec_Attr_11 as ConsumeIntegral_6,Dec_Attr_12 as UsedIntergral,Dec_Attr_13 as ComeMaturityIntergral,Dec_Attr_14 as UpdateIntergral,Dec_Attr_15 as AccConsumeIntegral,Dec_Attr_16 as AccCumulativeIntegral,Dec_Attr_17 as ConsumeIntegral_24,Dec_Attr_18 as AvgMemPrice,Dec_Attr_19 as AvgMemQty,Dec_Attr_20 as AvgMemPriceHis,Dec_Attr_21 as HistoryConsumeModify,Dec_Attr_22 as ActConsumptionLoy,Dec_Attr_23 as HistoryConsumeAmountModify,Date_Attr_1 as LastConsumeTime,Bool_Attr_1 as IsBirthMonth,Int_Attr_1 as ConsumptionCounts,Int_Attr_2 as ConsumptionCounts_24,AddedDate as AddedDate,AddedUser as AddedUser,ModifiedDate as ModifiedDate,ModifiedUser as ModifiedUser,Date_Attr_117 as RecentPurchaseDate,Str_Attr_118 as ProductColorLike,Str_Attr_119 as BrandLike,Str_Attr_120 as ProductTypeLike,Str_Attr_121 as StoreOftenGo,Str_Attr_122 as PromotionLike,Str_Attr_123 as BuySales_3,Str_Attr_124 as BestProductType,Str_Attr_125 as BestProductMaterial,Str_Attr_126 as OftenBuyTime_1N,Str_Attr_127 as OftenBuyTime_2N,Str_Attr_128 as OftenBuyTime_3N,Str_Attr_129 as BrandLike_1N,Str_Attr_130 as BrandLike_2N,Str_Attr_131 as BrandLike_3N,Str_Attr_132 as ProductTypeLike_1N,Str_Attr_133 as ProductTypeLike_2N,Str_Attr_134 as ProductTypeLike_3N,Str_Attr_135 as StoreOftenGo_1N,Str_Attr_136 as StoreOftenGo_2N,Str_Attr_137 as StoreOftenGo_3N,Str_Attr_138 as PromotionLike_1N,Str_Attr_139 as PromotionLike_2N,Str_Attr_140 as PromotionLike_3N,Str_Attr_141 as ProductTypeLike_4N,Str_Attr_142 as ProductTypeLike_5N,Str_Attr_143 as BrandLike_4N,Str_Attr_144 as BrandLike_5N,Str_Attr_145 as ProductColorLike_1N,Str_Attr_146 as ProductColorLike_2N,Str_Attr_147 as ProductColorLike_3N,Str_Attr_148 as ProductColorLike_4N,Str_Attr_149 as ProductColorLike_5N,Str_Attr_150 as OftenBuyTime_4N,Str_Attr_151 as OftenBuyTime_5N from TM_Loy_MemExt' 
GO
/****** Object:  View [dbo].[V_S_TM_Mem_Ext]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_S_TM_Mem_Ext]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_S_TM_Mem_Ext] as 
		  select MemberID as MemberID,Str_Attr_1 as CustomerStatus,Str_Attr_2 as MemberCardNo,Str_Attr_3 as CustomerName,Str_Attr_4 as CustomerMobile,Str_Attr_5 as RegisterStoreCode,Str_Attr_6 as NickName,Str_Attr_7 as Gender,Str_Attr_8 as CertificateType,Str_Attr_9 as CertificateNo,Str_Attr_10 as Profession,Str_Attr_11 as Province,Str_Attr_12 as City,Str_Attr_13 as District,Str_Attr_14 as Zip,Str_Attr_15 as Tel,Str_Attr_16 as CustomerType2,Str_Attr_17 as MembershipActivity,Str_Attr_18 as Address1,Str_Attr_19 as Address2,Str_Attr_20 as Job,Str_Attr_21 as FamilyIncome,Str_Attr_22 as Education,Str_Attr_23 as Hobbies,Str_Attr_24 as CouponTypeExt,Str_Attr_25 as MemberCardStatus,Str_Attr_26 as Corp,Str_Attr_27 as MemberLoyalty,Str_Attr_28 as RegChannelName,Str_Attr_29 as RecommenderCode,Str_Attr_30 as RecommenderName,Str_Attr_31 as POSCardNo,Str_Attr_32 as MemberCode,Str_Attr_33 as MemberIntroducer,Str_Attr_34 as BrandNameMember,Str_Attr_35 as AreaMember,Str_Attr_36 as IsWeChatSign,Str_Attr_37 as ProvinceCodeExt,Str_Attr_38 as CityCodeExt,Str_Attr_39 as DistrictCodeExt,Str_Attr_40 as IsMemberProtal,Str_Attr_100 as CustomerEmail,Str_Attr_101 as Remark,Str_Attr_50 as ChannelCodeMember,Str_Attr_51 as IsRegisteredFlag,Str_Attr_52 as IsdRecommendedFlag,Str_Attr_53 as CustomerSource,Date_Attr_1 as RegisterDate,Date_Attr_2 as Birthday,Date_Attr_3 as NearlyRemindTime,Date_Attr_4 as NearlyShopTime,Date_Attr_5 as RegisterDateProtal,Date_Attr_11 as MemberStartDate,Bool_Attr_1 as IsMessage,Bool_Attr_2 as IsEmail,Bool_Attr_3 as InfoCompletedFlag,Bool_Attr_4 as HasChild,Bool_Attr_5 as CellPhoneValidation,Bool_Attr_6 as IsBirthday,Bool_Attr_7 as IsModifyBirth,Date_Attr_60 as MemberEndDate,Bool_Attr_61 as MemberRightsLock,Date_Attr_62 as MemberLevelStartDate,Date_Attr_63 as MemberLevelEndDate,AddedDate as AddedDate,AddedUser as AddedUser,ModifiedDate as ModifiedDate,ModifiedUser as ModifiedUser from TM_Mem_Ext' 
GO
/****** Object:  View [dbo].[V_S_TM_Mem_Master]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_S_TM_Mem_Master]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_S_TM_Mem_Master] as 
		  select MemberID as MemberID,ParentMemberID as ParentMemberID,DataGroupID as DataGroupID,MemberGrade as MemberGrade,MemberLevel as CustomerLevel,MemberLevel2 as CustomerLevel2,Str_Key_1 as Mobile,Str_Key_2 as MemberAccount,Str_Key_3 as MemberPassword,Str_Key_4 as MemberOpenID,AddedDate as AddedDate,AddedUser as AddedUser,ModifiedDate as ModifiedDate,ModifiedUser as ModifiedUser from TM_Mem_Master' 
GO
/****** Object:  View [dbo].[V_Sys_DataGroupRelation]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Sys_DataGroupRelation]'))
EXEC dbo.sp_executesql @statement = N'
create view [dbo].[V_Sys_DataGroupRelation] as 
 /**********************************
  ----arvarto system-----
  存储过程功能描述：数据群组关系（群组下罗列其所有子群组，包含自身）
  建 立 人：zyb
  建立时间：2015-02-04
  修改内容: 

  ***********************************/

---0级分类
select  DataGroupID,DataGroupID  SubDataGroupID,DataGroupGrade SubDataGroupGrade,DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
where a.DataGroupGrade=0
union all
select  a.DataGroupID,b.DataGroupID SubDataGroupID,b.DataGroupGrade SubDataGroupGrade,b.DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
inner join TM_SYS_DataGroup b on a.DataGroupID=b.ParentDataGroupID
where a.DataGroupGrade=0 and b.DataGroupGrade=1
union all
select  a.DataGroupID,c.DataGroupID SubDataGroupID,c.DataGroupGrade SubDataGroupGrade,c.DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
inner join TM_SYS_DataGroup b on a.DataGroupID=b.ParentDataGroupID
inner join TM_SYS_DataGroup c on b.DataGroupID=c.ParentDataGroupID
where a.DataGroupGrade=0 and b.DataGroupGrade=1 and c.DataGroupGrade=2
union all
select  a.DataGroupID,d.DataGroupID SubDataGroupID,d.DataGroupGrade SubDataGroupGrade,d.DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
inner join TM_SYS_DataGroup b on a.DataGroupID=b.ParentDataGroupID
inner join TM_SYS_DataGroup c on b.DataGroupID=c.ParentDataGroupID
inner join TM_SYS_DataGroup d on c.DataGroupID=d.ParentDataGroupID
where a.DataGroupGrade=0 and b.DataGroupGrade=1 and c.DataGroupGrade=2  and d.DataGroupGrade=3
----1级分类
union all
select  DataGroupID,DataGroupID  SubDataGroupID,DataGroupGrade SubDataGroupGrade,DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
where a.DataGroupGrade=1
union all
select  a.DataGroupID,b.DataGroupID SubDataGroupID,b.DataGroupGrade SubDataGroupGrade,b.DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
inner join TM_SYS_DataGroup b on a.DataGroupID=b.ParentDataGroupID
where a.DataGroupGrade=1 and b.DataGroupGrade=2
union all
select  a.DataGroupID,c.DataGroupID SubDataGroupID,c.DataGroupGrade SubDataGroupGrade,c.DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
inner join TM_SYS_DataGroup b on a.DataGroupID=b.ParentDataGroupID
inner join TM_SYS_DataGroup c on b.DataGroupID=c.ParentDataGroupID
where a.DataGroupGrade=1 and b.DataGroupGrade=2 and c.DataGroupGrade=3
--2级分类
union all
select  DataGroupID,DataGroupID  SubDataGroupID,DataGroupGrade SubDataGroupGrade,DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
where a.DataGroupGrade=2
union all
select  a.DataGroupID,b.DataGroupID SubDataGroupID,b.DataGroupGrade SubDataGroupGrade,b.DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
inner join TM_SYS_DataGroup b on a.DataGroupID=b.ParentDataGroupID
where a.DataGroupGrade=2 and b.DataGroupGrade=3
---3级分类
union all
select  DataGroupID,DataGroupID  SubDataGroupID,DataGroupGrade SubDataGroupGrade,DataGroupName SubDataGroupName
from TM_SYS_DataGroup a
where a.DataGroupGrade=3






' 
GO
/****** Object:  View [dbo].[V_U_TM_Mem_Info]    Script Date: 2016/1/3 1:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_U_TM_Mem_Info]'))
EXEC dbo.sp_executesql @statement = N'create view [dbo].[V_U_TM_Mem_Info] as 
		  select a.MemberID as MemberID,a.ParentMemberID as ParentMemberID,a.DataGroupID as DataGroupID,a.MemberGrade as MemberGrade,a.MemberLevel as CustomerLevel,a.MemberLevel2 as CustomerLevel2,a.Str_Key_1 as Mobile,a.Str_Key_2 as MemberAccount,a.Str_Key_3 as MemberPassword,a.Str_Key_4 as MemberOpenID,a.AddedDate as AddedDate,a.AddedUser as AddedUser,a.ModifiedDate as ModifiedDate,a.ModifiedUser as ModifiedUser,b.Str_Attr_1 as CustomerStatus,b.Str_Attr_2 as MemberCardNo,b.Str_Attr_3 as CustomerName,b.Str_Attr_4 as CustomerMobile,b.Date_Attr_1 as RegisterDate,b.Str_Attr_5 as RegisterStoreCode,b.Str_Attr_100 as CustomerEmail,b.Str_Attr_7 as Gender,b.Str_Attr_8 as CertificateType,b.Str_Attr_9 as CertificateNo,b.Str_Attr_10 as Profession,b.Str_Attr_11 as Province,b.Str_Attr_12 as City,b.Str_Attr_13 as District,b.Str_Attr_14 as Zip,b.Str_Attr_15 as Tel,b.Date_Attr_2 as Birthday,b.Str_Attr_18 as Address1,b.Str_Attr_19 as Address2,b.Str_Attr_20 as Job,b.Str_Attr_21 as FamilyIncome,b.Str_Attr_22 as Education,b.Str_Attr_23 as Hobbies,b.Str_Attr_24 as CouponTypeExt,b.Str_Attr_101 as Remark,b.Str_Attr_16 as CustomerType2,b.Str_Attr_17 as MembershipActivity,b.Str_Attr_25 as MemberCardStatus,b.Str_Attr_26 as Corp,b.Str_Attr_27 as MemberLoyalty,b.Date_Attr_3 as NearlyRemindTime,b.Date_Attr_4 as NearlyShopTime,b.Bool_Attr_1 as IsMessage,b.Bool_Attr_2 as IsEmail,b.Date_Attr_11 as MemberStartDate,b.Date_Attr_60 as MemberEndDate,b.Bool_Attr_61 as MemberRightsLock,b.Date_Attr_62 as MemberLevelStartDate,b.Date_Attr_63 as MemberLevelEndDate,b.Str_Attr_53 as CustomerSource,b.Str_Attr_50 as ChannelCodeMember,b.Str_Attr_6 as NickName,b.Str_Attr_28 as RegChannelName,b.Str_Attr_29 as RecommenderCode,b.Str_Attr_30 as RecommenderName,b.Str_Attr_31 as POSCardNo,b.Bool_Attr_3 as InfoCompletedFlag,b.Bool_Attr_4 as HasChild,b.Bool_Attr_5 as CellPhoneValidation,b.Str_Attr_32 as MemberCode,b.Str_Attr_33 as MemberIntroducer,b.Str_Attr_34 as BrandNameMember,b.Str_Attr_35 as AreaMember,b.Bool_Attr_6 as IsBirthday,b.Str_Attr_51 as IsRegisteredFlag,b.Str_Attr_52 as IsdRecommendedFlag,b.Bool_Attr_7 as IsModifyBirth,b.Str_Attr_36 as IsWeChatSign,b.Str_Attr_37 as ProvinceCodeExt,b.Str_Attr_38 as CityCodeExt,b.Str_Attr_39 as DistrictCodeExt,b.Date_Attr_5 as RegisterDateProtal,b.Str_Attr_40 as IsMemberProtal,c.Str_Attr_1 as OftenBuyTime,c.Dec_Attr_1 as HistoryConsumeAmount,c.Dec_Attr_2 as HistoryPoint,c.Str_Attr_118 as ProductColorLike,c.Date_Attr_117 as RecentPurchaseDate,c.Int_Attr_1 as ConsumptionCounts,c.Dec_Attr_3 as ConsumptionYearly,c.Dec_Attr_4 as ActConsumption,c.Str_Attr_119 as BrandLike,c.Str_Attr_120 as ProductTypeLike,c.Str_Attr_121 as StoreOftenGo,c.Str_Attr_122 as PromotionLike,c.Dec_Attr_5 as ActConsumption_3,c.Dec_Attr_6 as ActConsumptionCounts,c.Str_Attr_123 as BuySales_3,c.Dec_Attr_7 as LastConsumeMoney,c.Date_Attr_1 as LastConsumeTime,c.Dec_Attr_10 as CumulativeIntegral_6,c.Dec_Attr_11 as ConsumeIntegral_6,c.Str_Attr_2 as MemberActivity,c.Dec_Attr_12 as UsedIntergral,c.Dec_Attr_13 as ComeMaturityIntergral,c.Dec_Attr_14 as UpdateIntergral,c.Str_Attr_124 as BestProductType,c.Str_Attr_125 as BestProductMaterial,c.Dec_Attr_15 as AccConsumeIntegral,c.Dec_Attr_16 as AccCumulativeIntegral,c.Str_Attr_126 as OftenBuyTime_1N,c.Str_Attr_127 as OftenBuyTime_2N,c.Str_Attr_128 as OftenBuyTime_3N,c.Str_Attr_129 as BrandLike_1N,c.Str_Attr_130 as BrandLike_2N,c.Str_Attr_131 as BrandLike_3N,c.Str_Attr_132 as ProductTypeLike_1N,c.Str_Attr_133 as ProductTypeLike_2N,c.Str_Attr_134 as ProductTypeLike_3N,c.Str_Attr_135 as StoreOftenGo_1N,c.Str_Attr_136 as StoreOftenGo_2N,c.Str_Attr_137 as StoreOftenGo_3N,c.Str_Attr_138 as PromotionLike_1N,c.Str_Attr_139 as PromotionLike_2N,c.Str_Attr_140 as PromotionLike_3N,c.Str_Attr_141 as ProductTypeLike_4N,c.Str_Attr_142 as ProductTypeLike_5N,c.Str_Attr_143 as BrandLike_4N,c.Str_Attr_144 as BrandLike_5N,c.Str_Attr_145 as ProductColorLike_1N,c.Str_Attr_146 as ProductColorLike_2N,c.Str_Attr_147 as ProductColorLike_3N,c.Str_Attr_148 as ProductColorLike_4N,c.Str_Attr_149 as ProductColorLike_5N,c.Str_Attr_150 as OftenBuyTime_4N,c.Str_Attr_151 as OftenBuyTime_5N,c.Dec_Attr_17 as ConsumeIntegral_24,c.Bool_Attr_1 as IsBirthMonth,c.Dec_Attr_8 as EnableIntegrade,c.Int_Attr_2 as ConsumptionCounts_24,c.Dec_Attr_9 as ActConsumption_24,c.Dec_Attr_18 as AvgMemPrice,c.Dec_Attr_19 as AvgMemQty,c.Dec_Attr_20 as AvgMemPriceHis,c.Dec_Attr_21 as HistoryConsumeModify,c.Dec_Attr_22 as ActConsumptionLoy,c.Dec_Attr_23 as HistoryConsumeAmountModify 
			  from TM_Mem_Master a
              left join TM_Mem_Ext b on a.MemberID=b.MemberID
              left join TM_Loy_MemExt c on a.MemberID=c.MemberID ' 
GO
