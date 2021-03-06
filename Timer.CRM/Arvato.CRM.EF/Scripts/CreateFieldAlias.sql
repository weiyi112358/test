USE [Kidsland_CRM]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TD_SYS_FieldAliasParameter_IsRequired]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TD_SYS_FieldAliasParameter] DROP CONSTRAINT [DF_TD_SYS_FieldAliasParameter_IsRequired]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TD_SYS_FieldAlias_IsDynamicAlias]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TD_SYS_FieldAlias] DROP CONSTRAINT [DF_TD_SYS_FieldAlias_IsDynamicAlias]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TD_SYS_FieldAlias_IsFilterByCouponTemplet]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TD_SYS_FieldAlias] DROP CONSTRAINT [DF_TD_SYS_FieldAlias_IsFilterByCouponTemplet]
END

GO
/****** Object:  Table [dbo].[TD_SYS_FieldAliasParameter]    Script Date: 2016/1/28 16:54:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_SYS_FieldAliasParameter]') AND type in (N'U'))
DROP TABLE [dbo].[TD_SYS_FieldAliasParameter]
GO
/****** Object:  Table [dbo].[TD_SYS_FieldAlias]    Script Date: 2016/1/28 16:54:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_SYS_FieldAlias]') AND type in (N'U'))
DROP TABLE [dbo].[TD_SYS_FieldAlias]
GO
/****** Object:  Table [dbo].[TD_SYS_FieldAlias]    Script Date: 2016/1/28 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_SYS_FieldAlias]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TD_SYS_FieldAlias](
	[AliasID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](100) NOT NULL,
	[FieldName] [nvarchar](100) NOT NULL,
	[FieldType] [nvarchar](10) NOT NULL,
	[FieldAlias] [nvarchar](50) NOT NULL,
	[AliasType] [nvarchar](20) NOT NULL,
	[AliasKey] [nvarchar](50) NULL,
	[AliasSubKey] [nvarchar](50) NULL,
	[FieldDesc] [nvarchar](50) NOT NULL,
	[DictTableName] [nvarchar](50) NULL,
	[DictTableType] [nvarchar](50) NULL,
	[IsFilterBySubdivision] [bit] NOT NULL,
	[IsFilterByLoyRule] [bit] NOT NULL,
	[IsFilterByLoyActionLeft] [bit] NOT NULL,
	[IsFilterByLoyActionRight] [bit] NOT NULL,
	[IsCommunicationTemplet] [bit] NOT NULL,
	[ControlType] [nvarchar](10) NOT NULL,
	[Reg] [nvarchar](500) NULL,
	[ComputeScript] [nvarchar](max) NULL,
	[LogScript] [nvarchar](1000) NULL,
	[RunType] [smallint] NULL,
	[DataLimitType] [nvarchar](20) NULL,
	[AddedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ComputeSort] [int] NULL,
	[ParameterCount] [int] NULL,
	[IsFilterByCouponTemplet] [bit] NULL,
	[ErrorTip] [nvarchar](100) NULL,
	[IsDynamicAlias] [bit] NULL,
 CONSTRAINT [PK_TD_SYS_FieldAlias] PRIMARY KEY CLUSTERED 
(
	[AliasID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TD_SYS_FieldAliasParameter]    Script Date: 2016/1/28 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_SYS_FieldAliasParameter]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TD_SYS_FieldAliasParameter](
	[ParaID] [int] IDENTITY(1,1) NOT NULL,
	[AliasID] [int] NOT NULL,
	[ParaIndex] [smallint] NOT NULL,
	[Reg] [nvarchar](100) NOT NULL,
	[FieldType] [nvarchar](10) NOT NULL,
	[ControlType] [nvarchar](10) NOT NULL,
	[DictTableName] [nvarchar](50) NULL,
	[DictTableType] [nvarchar](50) NULL,
	[ParameterName] [nvarchar](20) NULL,
	[UIIndex] [int] NULL,
	[IsRequired] [bit] NULL,
	[GroupType] [nvarchar](1) NULL,
	[flag] [nvarchar](2) NULL,
 CONSTRAINT [PK_TD_SYS_FieldAliasParameter] PRIMARY KEY CLUSTERED 
(
	[ParaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[TD_SYS_FieldAlias] ON 

INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'StoreName', N'BaseDataType', N'store', NULL, N'门店名称', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4360151F63C AS DateTime), NULL, 20, NULL, 1, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'StoreAddress', N'BaseDataType', N'store', NULL, N'门店地址', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4360152612D AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (5, N'TM_SYS_BaseData', N'Str_Attr_3', N'2', N'StoreCode', N'BaseDataType', N'store', NULL, N'门店编号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, N'store', CAST(0x0000A43E00BFABFC AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (12, N'TM_Mem_Ext', N'Str_Attr_1', N'2', N'CustomerStatus', N'MemberExt', NULL, NULL, N'会员状态', N'TD_SYS_BizOption', N'CustomerStatus', 0, 1, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A44F0111722C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (13, N'TM_Mem_Master', N'MemberLevel', N'1', N'CustomerLevel', N'MemberKey', NULL, NULL, N'会员等级', N'TD_SYS_BizOption', N'CustomerLevel', 1, 1, 1, 1, 1, N'select', NULL, NULL, N'Insert into [TL_Mem_LevelChange]
(
	MemberID,ChangeLevelFrom,ChangeLevelTo,LevelChangeType,ChangeReason,AddedDate,AddedUser
)
Select {0}.MemberID,V_U_TM_Mem_Info.CustomerLevel,''{1}'',''loy'',''忠诚度计算'',''{2}'',''1000''
From {0} 
inner join V_U_TM_Mem_Info on {0}.MemberID = V_U_TM_Mem_Info.MemberID', NULL, NULL, CAST(0x0000A44F01125BF0 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (14, N'TM_Mem_Ext', N'Str_Attr_2', N'2', N'MemberCardNo', N'MemberExt', NULL, NULL, N'会员卡号', NULL, NULL, 1, 0, 0, 0, 1, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A44F011393C8 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (15, N'TM_Mem_Ext', N'Str_Attr_3', N'2', N'CustomerName', N'MemberExt', NULL, NULL, N'会员姓名', NULL, NULL, 1, 0, 0, 0, 1, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A44F0114CCD0 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (16, N'TM_Mem_Ext', N'Str_Attr_4', N'2', N'CustomerMobile', N'MemberExt', NULL, NULL, N'手机号', NULL, NULL, 1, 0, 0, 0, 1, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A44F01155006 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (21, N'TM_Mem_Ext', N'Date_Attr_1', N'5', N'RegisterDate', N'MemberExt', NULL, NULL, N'注册日期', NULL, NULL, 1, 0, 0, 0, 1, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A44F011AEC0A AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (22, N'TM_Mem_Ext', N'Str_Attr_5', N'2', N'RegisterStoreCode', N'MemberExt', NULL, NULL, N'注册门店(代码)', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, N'store', CAST(0x0000A44F0123BD07 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (23, N'TM_Mem_Ext', N'Str_Attr_100', N'2', N'CustomerEmail', N'MemberExt', NULL, NULL, N'会员邮箱', NULL, NULL, 0, 0, 0, 0, 1, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A44F0135C0AB AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (77, N'TM_Mem_Trade', N'Str_Attr_1', N'2', N'BillNOSales', N'MemberTrade', N'sales', NULL, N'单据编号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45101098838 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (177, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'BrandName', N'BaseDataType', N'brand', NULL, N'品牌名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45101107600 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (178, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'BrandCode', N'BaseDataType', N'brand', NULL, N'品牌代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4510110BC50 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (263, N'TM_Mem_Master', N'ParentMemberID', N'2', N'ParentMemberID', N'MemberKey', NULL, NULL, N'父会员编号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45200A9AEEF AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (264, N'TM_Mem_Master', N'MemberGrade', N'3', N'MemberGrade', N'MemberKey', NULL, NULL, N'会员层级', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45200AB81D7 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (268, N'TM_Mem_Ext', N'Str_Attr_7', N'2', N'Gender', N'MemberExt', NULL, NULL, N'性别', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4520118EE96 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (269, N'TM_Mem_Ext', N'Str_Attr_8', N'2', N'CertificateType', N'MemberExt', NULL, NULL, N'证件类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452011BA6A0 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (270, N'TM_Mem_Ext', N'Str_Attr_9', N'2', N'CertificateNo', N'MemberExt', NULL, NULL, N'证件号码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452011BBA00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (271, N'TM_Mem_Ext', N'Str_Attr_10', N'2', N'Profession', N'MemberExt', NULL, NULL, N'行业', NULL, NULL, 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452011E6972 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (272, N'TM_Mem_Ext', N'Str_Attr_11', N'2', N'Province', N'MemberExt', NULL, NULL, N'省', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452011EA949 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (275, N'TM_Mem_Ext', N'Str_Attr_12', N'2', N'City', N'MemberExt', NULL, NULL, N'市', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452011F94FB AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (276, N'TM_Mem_Ext', N'Str_Attr_13', N'2', N'District', N'MemberExt', NULL, NULL, N'区', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452011FF13B AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (277, N'TM_Mem_Ext', N'Str_Attr_14', N'2', N'Zip', N'MemberExt', NULL, NULL, N'邮编', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45201200FD4 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (278, N'TM_Mem_Ext', N'Str_Attr_15', N'2', N'Tel', N'MemberExt', NULL, NULL, N'电话', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4520121A0EB AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (280, N'TM_Mem_Ext', N'Date_Attr_2', N'5', N'Birthday', N'MemberExt', NULL, NULL, N'生日', NULL, NULL, 1, 1, 0, 0, 1, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45201231B75 AS DateTime), CAST(0x0000A52C011D38E3 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (283, N'TM_Mem_Ext', N'Str_Attr_18', N'2', N'Address1', N'MemberExt', NULL, NULL, N'现居地址', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4520123F0E4 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (284, N'TM_Mem_Ext', N'Str_Attr_19', N'2', N'Address2', N'MemberExt', NULL, NULL, N'身份证地址', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45201240592 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (285, N'TM_Mem_Ext', N'Str_Attr_20', N'2', N'Job', N'MemberExt', NULL, NULL, N'职务', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4520124292E AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (286, N'TM_Mem_Ext', N'Str_Attr_21', N'2', N'FamilyIncome', N'MemberExt', NULL, NULL, N'家庭月收入', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452012455D5 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (287, N'TM_Mem_Ext', N'Str_Attr_22', N'2', N'Education', N'MemberExt', NULL, NULL, N'学历', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45201248A41 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (288, N'TM_Mem_Ext', N'Str_Attr_23', N'2', N'Hobbies', N'MemberExt', NULL, NULL, N'爱好', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4520124A330 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (289, N'TM_Mem_Ext', N'Str_Attr_24', N'2', N'CouponTypeExt', N'MemberExt', NULL, NULL, N'当前优惠模式', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4520124C7BD AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (290, N'TM_Mem_Ext', N'Str_Attr_101', N'2', N'Remark', N'MemberExt', NULL, NULL, N'备注', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A452012504C3 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (315, N'TM_Mem_Ext', N'Str_Attr_16', N'2', N'CustomerType2', N'MemberExt', NULL, NULL, N'会员类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F3DD7F AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (316, N'TM_Mem_Ext', N'Str_Attr_17', N'2', N'MembershipActivity', N'MemberExt', NULL, NULL, N'会员活跃度', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F412A9 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (317, N'TM_Mem_Ext', N'Str_Attr_25', N'2', N'MemberCardStatus', N'MemberExt', NULL, NULL, N'会员卡状态', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F4280C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (318, N'TM_Mem_Ext', N'Str_Attr_26', N'2', N'Corp', N'MemberExt', NULL, NULL, N'所属企业', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F47126 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (319, N'TM_Mem_Ext', N'Str_Attr_27', N'2', N'MemberLoyalty', N'MemberExt', NULL, NULL, N'会员忠诚度', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F4CAA2 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (320, N'TM_Mem_Ext', N'Date_Attr_3', N'5', N'NearlyRemindTime', N'MemberExt', NULL, NULL, N'最近一次提醒时间', NULL, NULL, 0, 0, 0, 0, 0, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F5310C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (321, N'TM_Mem_Ext', N'Date_Attr_4', N'5', N'NearlyShopTime', N'MemberExt', NULL, NULL, N'最近一次消费时间', NULL, NULL, 0, 0, 0, 0, 0, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F55084 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (322, N'TM_Mem_Ext', N'Bool_Attr_1', N'4', N'IsMessage', N'MemberExt', NULL, NULL, N'是否接收短信', NULL, NULL, 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F5B5F7 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (323, N'TM_Mem_Ext', N'Bool_Attr_2', N'4', N'IsEmail', N'MemberExt', NULL, NULL, N'是否接收邮件', NULL, NULL, 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45300F5E197 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1378, N'TM_Mem_SubExt', N'Str_Attr_1', N'2', N'ContactName', N'MemberSubExt', N'contact', NULL, N'联系人姓名', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570134DFF4 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1380, N'TM_Mem_SubExt', N'Str_Attr_2', N'2', N'ContactGender', N'MemberSubExt', N'contact', NULL, N'联系人性别', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570135653F AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1381, N'TM_Mem_SubExt', N'Str_Attr_3', N'2', N'ContactTel', N'MemberSubExt', N'contact', NULL, N'联系人电话', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570135A10C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1382, N'TM_Mem_SubExt', N'Str_Attr_4', N'2', N'ContactMobile', N'MemberSubExt', N'contact', NULL, N'联系人手机', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570135AF37 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1383, N'TM_Mem_SubExt', N'Str_Attr_5', N'2', N'ContactEmail', N'MemberSubExt', N'contact', NULL, N'联系人Email', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570135C09F AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1384, N'TM_Mem_SubExt', N'Str_Attr_6', N'2', N'ContactProvince', N'MemberSubExt', N'contact', NULL, N'联系人省', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A457013625CC AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1385, N'TM_Mem_SubExt', N'Str_Attr_7', N'2', N'ContactCity', N'MemberSubExt', N'contact', NULL, N'联系人市', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45701365D8D AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1386, N'TM_Mem_SubExt', N'Str_Attr_8', N'2', N'ContactDistrict', N'MemberSubExt', N'contact', NULL, N'联系人区', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45701369E14 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1387, N'TM_Mem_SubExt', N'Str_Attr_9', N'2', N'ContactZip', N'MemberSubExt', N'contact', NULL, N'联系人邮编', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570136CD66 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1388, N'TM_Mem_SubExt', N'Str_Attr_10', N'2', N'ContactJob', N'MemberSubExt', N'contact', NULL, N'联系人职业', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45701371E62 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1389, N'TM_Mem_SubExt', N'Str_Attr_11', N'2', N'LovingCommunication', N'MemberSubExt', N'contact', NULL, N'喜欢的联络方式', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570137830D AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1390, N'TM_Mem_SubExt', N'Str_Attr_12', N'2', N'BestContactTime1', N'MemberSubExt', N'contact', NULL, N'最佳联系时间（起始）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570137B1BC AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1391, N'TM_Mem_SubExt', N'Str_Attr_13', N'2', N'BestContactTime2', N'MemberSubExt', N'contact', NULL, N'最佳联系时间（结束）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4570137C0C5 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1392, N'TM_Mem_SubExt', N'Str_Attr_14', N'2', N'ContactPosition', N'MemberSubExt', N'contact', NULL, N'联系人职务', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45701380715 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1393, N'TM_Mem_SubExt', N'Str_Attr_15', N'2', N'ContactIdNo', N'MemberSubExt', N'contact', NULL, N'联系人证件号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45800C1B8D0 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1397, N'TM_Mem_SubExt', N'Str_Attr_16', N'2', N'ContactCertificateType', N'MemberSubExt', N'contact', NULL, N'联系人证件类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45800000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1399, N'TM_Mem_SubExt', N'Str_Attr_17', N'2', N'ContactAddress', N'MemberSubExt', N'contact', NULL, N'邮寄地址', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45800000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1405, N'TM_Loy_MemExt', N'Str_Attr_1', N'1', N'OftenBuyTime', N'MemberExt', NULL, NULL, N'经常购买时间段', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1
--update TM_Loy_MemExt set [Attr]=temp.time_result
--from  
--(select MemberID ,time_result=stuff((select '',''+time_result from (
--select *,(isnull(w.time_part, '''') + ''|'' + isnull(w.time_rate, '''')) time_result
--from (
--select e.*,cast(cast(cast(e.time_count as float) /e.count_sum * 100  as int) as nvarchar(10)) time_rate from 
--(
--  select d.*,sum(time_count) over(partition by d.memberid) count_sum from (
--  select c.* ,row_number () over( partition by c.memberid order by c.time_count desc) time_order 
--   from (
        
--	 select b.memberid,b.time_part,count(b.time_part) time_count from (
--select a.memberid,
--case when 8< cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) 
--       and cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) <= 11 then ''早上''
--    when 11< cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) 
--       and cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) <= 14 then ''中午''
--    when 14< cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) 
--       and cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) <= 18 then ''下午''
--   when 18< cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) 
--       and cast(substring(convert(nvarchar(10),a.ListDateSales,108),0,3) as int) <= 22 then ''晚上''
--else ''其他''
--end  time_part
--------增加约束条件
--from (select m.memberid , m.ListDateSales 
--from V_M_TM_Mem_Trade_sales m inner join 
--(select  memberid ,max(ListDateSales) big_date
--        from V_M_TM_Mem_Trade_sales group by memberid) n 
--on m.memberid = n.memberid where m.ListDateSales>=dateadd(month,-2, n.big_date)) a

--)b
--group by b.memberid,b.time_part
--) c
--)d
--where d.time_order<=3
--)e
--)w
--)t where MemberID=w.MemberID for xml path('''')),1,1,'''')
--from TM_Loy_MemExt w 
--where ([Switch]=1 or w.MemberID in ([MemberList]))
--) temp 

--where TM_Loy_MemExt.MemberID=temp.MemberID
--and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 1, NULL, CAST(0x0000A45900B34EAA AS DateTime), CAST(0x0000A538013922A4 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1414, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'StoreSettingStoreCode', N'BaseDataType', N'storeSetting', NULL, N'门店编号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45F011070E2 AS DateTime), CAST(0x0000A45F011070E2 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1415, N'TM_SYS_BaseData', N'Dec_Attr_1', N'7', N'OrderMaxPoint', N'BaseDataType', N'storeSetting', NULL, N'每单积分使用上限', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45F0111046A AS DateTime), CAST(0x0000A45F0111046A AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1440, N'TM_Mem_Master', N'Str_Key_1', N'2', N'Mobile', N'MemberKey', NULL, NULL, N'手机号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46001413C7F AS DateTime), CAST(0x0000A46001413C7F AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1441, N'TM_Mem_Master', N'Str_Key_2', N'2', N'MemberAccount', N'MemberKey', NULL, NULL, N'会员账号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46001414C8C AS DateTime), CAST(0x0000A46001414C8C AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1442, N'TM_Mem_Master', N'Str_Key_3', N'2', N'MemberPassword', N'MemberKey', NULL, NULL, N'会员密码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46001415CA7 AS DateTime), CAST(0x0000A46001415CA7 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1486, N'TM_Mem_SubExt', N'Str_Attr_18', N'2', N'ContactType', N'MemberSubExt', N'contact', NULL, N'联系人类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46D010B15E5 AS DateTime), CAST(0x0000A46D010B15E5 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1489, N'TM_Mem_SubExt', N'Bool_Attr_50', N'4', N'ContactHaveTrade', N'MemberSubExt', N'contact', NULL, N'联系人是否存在交易单', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46D013F24AD AS DateTime), CAST(0x0000A46D013F24AD AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1492, N'TM_Mem_SubExt', N'Str_Attr_19', N'2', N'ContactProvinceName', N'MemberSubExt', N'contact', NULL, N'联系人省名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46E00B698C4 AS DateTime), CAST(0x0000A46E00B698C4 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1493, N'TM_Mem_SubExt', N'Str_Attr_20', N'2', N'ContactCityName', N'MemberSubExt', N'contact', NULL, N'联系人市名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46E00B6B499 AS DateTime), CAST(0x0000A46E00B6B499 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1494, N'TM_Mem_SubExt', N'Str_Attr_21', N'2', N'ContactDistrictName', N'MemberSubExt', N'contact', NULL, N'联系人区名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A46E00B6DB50 AS DateTime), CAST(0x0000A46E00B6DB50 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1502, N'TM_Mem_Ext', N'Date_Attr_11', N'6', N'MemberStartDate', N'MemberExt', NULL, NULL, N'会员有效开始日期', NULL, NULL, 0, 1, 1, 0, 0, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A47B00B3F8A9 AS DateTime), CAST(0x0000A47B00B3F8A9 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1503, N'TM_Mem_Ext', N'Date_Attr_60', N'6', N'MemberEndDate', N'MemberExt', NULL, NULL, N'会员有效截止日期', NULL, NULL, 0, 1, 1, 0, 0, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A47B00B40E6E AS DateTime), CAST(0x0000A47B00B40E6E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1504, N'TM_Mem_Ext', N'Bool_Attr_61', N'4', N'MemberRightsLock', N'MemberExt', NULL, NULL, N'会员权益锁定标志', NULL, NULL, 0, 1, 1, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A47B00B4842A AS DateTime), CAST(0x0000A47B00B4842A AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1505, N'TM_Mem_Ext', N'Date_Attr_62', N'6', N'MemberLevelStartDate', N'MemberExt', NULL, NULL, N'会员等级起始日期', NULL, NULL, 0, 1, 1, 0, 0, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A47B00B4CEFC AS DateTime), CAST(0x0000A47B00B4CEFC AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1506, N'TM_Mem_Ext', N'Date_Attr_63', N'6', N'MemberLevelEndDate', N'MemberExt', NULL, NULL, N'会员等级截止日期', NULL, NULL, 0, 1, 1, 0, 1, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A47B00B4E07D AS DateTime), CAST(0x0000A47B00B4E07D AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1560, N'TM_Mem_Ext', N'Str_Attr_53', N'1', N'CustomerSource', N'MemberExt', NULL, NULL, N'会员来源', N'TD_SYS_BizOption', N'CustomerSource', 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A48F00B724F3 AS DateTime), CAST(0x0000A48F00B724F3 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1612, N'TM_SYS_BaseData', N'Str_Attr_4', N'2', N'StoreFullName', N'BaseDataType', N'store', NULL, N'门店全称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1625, N'TM_SYS_BaseData', N'Dec_Attr_2', N'7', N'PointCashPec', N'BaseDataType', N'storeSetting', NULL, N'积分先进换算率', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E300000000 AS DateTime), CAST(0x0000A4E300000000 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1626, N'TM_Mem_Trade', N'Str_Attr_2', N'2', N'StoreCodeSales', N'MemberTrade', N'sales', NULL, N'消费门店名称', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1627, N'TM_Mem_Trade', N'Str_Attr_3', N'2', N'MobileNumberSales', N'MemberTrade', N'sales', NULL, N'手机号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1628, N'TM_Mem_Trade', N'Str_Attr_4', N'2', N'MarketNumberSales', N'MemberTrade', N'sales', NULL, N'商场卡号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1629, N'TM_Mem_Trade', N'Str_Attr_5', N'2', N'MemoSales', N'MemberTrade', N'sales', NULL, N'备注', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1630, N'TM_Mem_SubExt', N'Str_Attr_1', N'2', N'BabyName', N'MemberSubExt', N'kid', NULL, N'宝宝姓名', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1631, N'TM_Mem_SubExt', N'Str_Attr_2', N'2', N'BabyGender', N'MemberSubExt', N'kid', NULL, N'宝宝性别', N'TD_SYS_BizOption', N'SexType', 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1632, N'TM_Mem_SubExt', N'Date_Attr_1', N'6', N'BabyBrithday', N'MemberSubExt', N'kid', NULL, N'宝宝生日', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1633, N'TM_Mem_TradeDetail', N'Str_Attr_1', N'2', N'GoodsCodeProduct', N'MemberTradeDetail', N'sales', N'product', N'商品代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E5A528 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1634, N'TM_Mem_TradeDetail', N'Str_Attr_2', N'2', N'ColorCodeProduct', N'MemberTradeDetail', N'sales', N'product', N'颜色代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E60844 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1635, N'TM_Mem_TradeDetail', N'Str_Attr_3', N'2', N'SizeCodeProduct', N'MemberTradeDetail', N'sales', N'product', N'尺码代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E63E1D AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1637, N'TM_Mem_TradeDetail', N'Str_Attr_4', N'2', N'DiscountBillNOProduct', N'MemberTradeDetail', N'sales', N'product', N'折扣券号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E68612 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1638, N'TM_Mem_TradeDetail', N'Str_Attr_5', N'2', N'SourceBillNoProduct', N'MemberTradeDetail', N'sales', N'product', N'原单据编号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E6D7F0 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1639, N'TM_Mem_TradeDetail', N'Str_Attr_6', N'2', N'ChangeReasonProduct', N'MemberTradeDetail', N'sales', N'product', N'退换货原因', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E71FF2 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1640, N'TM_Mem_TradeDetail', N'Str_Attr_7', N'2', N'ActivityCodeProduct', N'MemberTradeDetail', N'sales', N'product', N'活动代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E74F59 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1641, N'TM_Mem_TradeDetail', N'Str_Attr_8', N'2', N'ActivityNameProduct', N'MemberTradeDetail', N'sales', N'product', N'活动名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E77D43 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1642, N'TM_Mem_TradeDetail', N'Str_Attr_9', N'2', N'MemoProduct', N'MemberTradeDetail', N'sales', N'product', N'摘要', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E7CE2E AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1645, N'TM_Mem_TradeDetail', N'Str_Attr_10', N'2', N'SourceProduct', N'MemberTradeDetail', N'sales', N'product', N'来源', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E81939 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1646, N'TM_Mem_TradeDetail', N'Dec_Attr_1', N'7', N'ReferencePriceProduct', N'MemberTradeDetail', N'sales', N'product', N'参考价', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E8CAB3 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1647, N'TM_Mem_TradeDetail', N'Dec_Attr_2', N'7', N'PriceProduct', N'MemberTradeDetail', N'sales', N'product', N'单品金额（订单明细）', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E9E85C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1649, N'TM_Mem_TradeDetail', N'Dec_Attr_3', N'7', N'DiscountProduct', N'MemberTradeDetail', N'sales', N'product', N'折扣', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EA190E AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1650, N'TM_Mem_TradeDetail', N'Dec_Attr_4', N'7', N'QuantityProduct', N'MemberTradeDetail', N'sales', N'product', N'数量', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EA65C6 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1651, N'TM_Mem_TradeDetail', N'Dec_Attr_5', N'7', N'ReferenceAmountProduct', N'MemberTradeDetail', N'sales', N'product', N'参考金额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EAB325 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1653, N'TM_Mem_TradeDetail', N'Dec_Attr_6', N'7', N'AmountProduct', N'MemberTradeDetail', N'sales', N'product', N'金额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EAE570 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1656, N'TM_Mem_TradeDetail', N'Dec_Attr_7', N'7', N'MarketDiscProduct', N'MemberTradeDetail', N'sales', N'product', N'扣率', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB3B02 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1658, N'TM_Mem_TradeDetail', N'Str_Attr_11', N'2', N'PromotionCodeProduct', N'MemberTradeDetail', N'sales', N'product', N'促销单号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1659, N'TM_Mem_TradeDetail', N'Int_Attr_1', N'3', N'StatusProduct', N'MemberTradeDetail', N'sales', N'product', N'状态', N'TD_SYS_BizOption', N'TradeDetailStatus', 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EBCFF6 AS DateTime), CAST(0x0000A53800F2388B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1660, N'TM_Mem_Trade', N'Str_Attr_6', N'2', N'SourceBillNOSales', N'MemberTrade', N'sales', NULL, N'原单据号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1662, N'TM_Mem_Trade', N'Str_Attr_7', N'2', N'GiftCouponNoSales', N'MemberTrade', N'sales', NULL, N'赠送券号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1663, N'TM_Mem_Trade', N'Dec_Attr_1', N'7', N'MarketDiscountSales', N'MemberTrade', N'sales', NULL, N'商场折扣', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1665, N'TM_Mem_Trade', N'Dec_Attr_2', N'7', N'StandardAmountSales', N'MemberTrade', N'sales', NULL, N'标准金额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1666, N'TM_Mem_Trade', N'Dec_Attr_3', N'7', N'MarketDiscountRateSales', N'MemberTrade', N'sales', NULL, N'扣率', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1667, N'TM_Mem_Trade', N'Dec_Attr_4', N'7', N'DiscountAmountSales', N'MemberTrade', N'sales', NULL, N'优惠小计', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1668, N'TM_Mem_Trade', N'Dec_Attr_5', N'7', N'Amount', N'MemberTrade', N'sales', NULL, N'支付金额', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1669, N'TM_Mem_Trade', N'Dec_Attr_6', N'7', N'Discount', N'MemberTrade', N'sales', NULL, N'折扣', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1670, N'TM_Mem_Trade', N'Int_Attr_1', N'3', N'BirthdayprivilegeQtySales', N'MemberTrade', N'sales', NULL, N'生日优惠数量', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1673, N'TM_Mem_Trade', N'Int_Attr_2', N'3', N'IntegralRedemptionQtySales', N'MemberTrade', N'sales', NULL, N'积分换购数量', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1674, N'TM_Mem_Trade', N'Int_Attr_3', N'3', N'PromotionGoodsQtySales', N'MemberTrade', N'sales', NULL, N'促销商品数量', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1675, N'TM_Mem_Trade', N'Int_Attr_4', N'3', N'QuantitySales', N'MemberTrade', N'sales', NULL, N'数量', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1676, N'TM_Mem_Trade', N'Date_Attr_1', N'5', N'ListDateSales', N'MemberTrade', N'sales', NULL, N'消费日期', NULL, NULL, 1, 0, 0, 0, 0, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51300000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1677, N'TM_Mem_TradeDetail', N'Str_Attr_1', N'2', N'PmtCodePayment', N'MemberTradeDetail', N'sales', N'payment', N'结算代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E5A528 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1678, N'TM_Mem_TradeDetail', N'Str_Attr_2', N'2', N'PmtNamePayment', N'MemberTradeDetail', N'sales', N'payment', N'结算名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E5A528 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1679, N'TM_Mem_TradeDetail', N'Str_Attr_3', N'2', N'PmtCardNoPayment', N'MemberTradeDetail', N'sales', N'payment', N'结算方式卡号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E5A528 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1680, N'TM_Mem_TradeDetail', N'Str_Attr_4', N'2', N'MemoPayment', N'MemberTradeDetail', N'sales', N'payment', N'摘要', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E5A528 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1681, N'TM_Mem_TradeDetail', N'Dec_Attr_1', N'7', N'AmountPayment', N'MemberTradeDetail', N'sales', N'payment', N'金额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E5A528 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1682, N'TM_Mem_TradeDetail', N'Dec_Attr_2', N'7', N'ReceivedAmountPayment', N'MemberTradeDetail', N'sales', N'payment', N'实收金额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400E5A528 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1683, N'TM_Mem_TradeDetail', N'Int_Attr_1', N'3', N'IntegralCostPayment', N'MemberTradeDetail', N'sales', N'payment', N'使用积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EBCFF6 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1684, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'PromotionID', N'BaseDataType', N'promotion', NULL, N'方案ID', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1686, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'PromotionCode', N'BaseDataType', N'promotion', NULL, N'方案代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1687, N'TM_SYS_BaseData', N'Str_Attr_3', N'2', N'PromotionName', N'BaseDataType', N'promotion', NULL, N'方案名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1688, N'TM_SYS_BaseData', N'Str_Attr_4', N'2', N'PromotionBillType', N'BaseDataType', N'promotion', NULL, N'方案单据类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1689, N'TM_SYS_BaseData', N'Str_Attr_5', N'2', N'PromotionType', N'BaseDataType', N'promotion', NULL, N'促销类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1690, N'TM_SYS_BaseData', N'Str_Attr_6', N'2', N'PromotionRemark', N'BaseDataType', N'promotion', NULL, N'备注', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1691, N'TM_SYS_BaseData', N'Date_Attr_1', N'5', N'PromotionStartDate', N'BaseDataType', N'promotion', NULL, N'起始日期', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1692, N'TM_SYS_BaseData', N'Date_Attr_2', N'5', N'PromotionEndDate', N'BaseDataType', N'promotion', NULL, N'结束日期', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1693, N'TM_SYS_BaseData', N'Bool_Attr_1', N'4', N'PromotionIsEnd', N'BaseDataType', N'promotion', NULL, N'是否终止', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1702, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'ProductCode', N'BaseDataType', N'product', NULL, N'产品代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1703, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'ProductName', N'BaseDataType', N'product', NULL, N'产品名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1704, N'TM_SYS_BaseData', N'Str_Attr_3', N'2', N'ProductCode_IPOS', N'BaseDataType', N'product', NULL, N'pos产品代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1705, N'TM_SYS_BaseData', N'Str_Attr_4', N'2', N'ProductLineCode1', N'BaseDataType', N'product', NULL, N'产品线1代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1706, N'TM_SYS_BaseData', N'Str_Attr_5', N'2', N'ProductLineName1Base', N'BaseDataType', N'product', NULL, N'产品线1名称
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1707, N'TM_SYS_BaseData', N'Str_Attr_6', N'2', N'ProductLineCode2', N'BaseDataType', N'product', NULL, N'产品线2代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1708, N'TM_SYS_BaseData', N'Str_Attr_7', N'2', N'ProductLineName2Base', N'BaseDataType', N'product', NULL, N'产品线2名称
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1709, N'TM_SYS_BaseData', N'Str_Attr_8', N'2', N'ProductLineCode3', N'BaseDataType', N'product', NULL, N'产品线3代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1710, N'TM_SYS_BaseData', N'Str_Attr_9', N'2', N'ProductLineName3', N'BaseDataType', N'product', NULL, N'产品线3名称
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1711, N'TM_SYS_BaseData', N'Str_Attr_10', N'2', N'CategoryCode', N'BaseDataType', N'product', NULL, N'大类代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1712, N'TM_SYS_BaseData', N'Str_Attr_11', N'2', N'CategoryName', N'BaseDataType', N'product', NULL, N'大类名称
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1713, N'TM_SYS_BaseData', N'Str_Attr_12', N'2', N'SubCategoryCode', N'BaseDataType', N'product', NULL, N'子类代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1714, N'TM_SYS_BaseData', N'Str_Attr_13', N'2', N'SubCategoryName', N'BaseDataType', N'product', NULL, N'子类名称
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1715, N'TM_SYS_BaseData', N'Str_Attr_14', N'2', N'OrginalSellPrice', N'BaseDataType', N'product', NULL, N'原始售价
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1716, N'TM_SYS_BaseData', N'Str_Attr_15', N'2', N'StopUsageFlag', N'BaseDataType', N'product', NULL, N'停止使用
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1717, N'TM_SYS_BaseData', N'Str_Attr_16', N'2', N'ProductBrandCode', N'BaseDataType', N'product', NULL, N'品牌代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1718, N'TM_SYS_BaseData', N'Str_Attr_17', N'2', N'ProductBrandNameBase', N'BaseDataType', N'product', NULL, N'品牌名称
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1719, N'TM_SYS_BaseData', N'Str_Attr_18', N'2', N'ProductStatusCode', N'BaseDataType', N'product', NULL, N'状态代码
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1720, N'TM_SYS_BaseData', N'Str_Attr_19', N'2', N'ProductStatusNameBase', N'BaseDataType', N'product', NULL, N'状态名称
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1721, N'TM_SYS_BaseData', N'Bool_Attr_1', N'4', N'ProductSyncFlag', N'BaseDataType', N'product', NULL, N'同步标志
', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1722, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'ColorCode', N'BaseDataType', N'colors', NULL, N'颜色代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1723, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'ColorName', N'BaseDataType', N'colors', NULL, N'颜色名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5140100ED00 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1724, N'TM_SYS_BaseData', N'Str_Attr_5', N'2', N'StoreCode_IPOS', N'BaseDataType', N'store', NULL, N'tt', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1725, N'TM_SYS_BaseData', N'Str_Attr_6', N'2', N'AreaCodeStore', N'BaseDataType', N'store', NULL, N'区域编号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1726, N'TM_SYS_BaseData', N'Str_Attr_7', N'2', N'AreaNameStore', N'BaseDataType', N'store', NULL, N'区域名称', N'TD_SYS_BizOption', N'StoreArea', 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1727, N'TM_SYS_BaseData', N'Str_Attr_8', N'2', N'ProvinceCodeStore', N'BaseDataType', N'store', NULL, N'省代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1728, N'TM_SYS_BaseData', N'Str_Attr_9', N'2', N'ProvinceStore', N'BaseDataType', N'store', NULL, N'省', N'TD_SYS_Province', N'RegionID,NameZH', 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, 1, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1729, N'TM_SYS_BaseData', N'Str_Attr_10', N'2', N'CityCodeStore', N'BaseDataType', N'store', NULL, N'市代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1730, N'TM_SYS_BaseData', N'Str_Attr_11', N'2', N'CityStore', N'BaseDataType', N'store', NULL, N'市', N'TD_SYS_City', N'RegionID,NameZH', 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, 1, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1731, N'TM_SYS_BaseData', N'Str_Attr_12', N'2', N'AddressStore', N'BaseDataType', N'store', NULL, N'门店地址', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1732, N'TM_SYS_BaseData', N'Str_Attr_13', N'2', N'ChannelCodeStore', N'BaseDataType', N'store', NULL, N'门店渠道代码2', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1733, N'TM_SYS_BaseData', N'Str_Attr_14', N'2', N'ChannelNameStore', N'BaseDataType', N'store', NULL, N'门店渠道2', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1734, N'TM_SYS_BaseData', N'Str_Attr_15', N'2', N'ChannelTypeCodeStore', N'BaseDataType', N'store', NULL, N'门店渠道代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1735, N'TM_SYS_BaseData', N'Str_Attr_16', N'2', N'ChannerTypeNameStore', N'BaseDataType', N'store', NULL, N'门店渠道', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, 1, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1736, N'TM_SYS_BaseData', N'Str_Attr_17', N'2', N'StoreType', N'BaseDataType', N'store', NULL, N'门店类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1737, N'TM_SYS_BaseData', N'Bool_Attr_1', N'2', N'OneLineFlag', N'BaseDataType', N'store', NULL, N'是否营业', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4E30111FD2E AS DateTime), CAST(0x0000A4E30111FD2E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1741, N'TM_Mem_Master', N'Str_Key_4', N'2', N'MemberOpenID', N'MemberKey', NULL, NULL, N'会员OpenID', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51A00EF3A5D AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1742, N'TM_Mem_Ext', N'Str_Attr_50', N'1', N'ChannelCodeMember', N'MemberExt', NULL, NULL, N'会员所属渠道（代码）', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51B00B6C9C3 AS DateTime), CAST(0x0000A51B00B6C9C3 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1743, N'TM_Loy_MemExt', N'Dec_Attr_1', N'7', N'HistoryConsumeAmount', N'MemberExt', NULL, NULL, N'历史消费额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A51D00BDE23B AS DateTime), CAST(0x0000A51D00BDE23B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1744, N'TM_Loy_MemExt', N'Dec_Attr_2', N'7', N'HistoryPoint', N'MemberExt', NULL, NULL, N'历史积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A51D00BE6A7B AS DateTime), CAST(0x0000A51D00BE6A7B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1745, N'TM_Mem_Ext', N'Str_Attr_6', N'2', N'NickName', N'MemberExt', NULL, NULL, N'会员昵称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BECBB8 AS DateTime), CAST(0x0000A51D00BECBB8 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1746, N'TM_Mem_Ext', N'Str_Attr_28', N'2', N'RegChannelName', N'MemberExt', NULL, NULL, N'注册渠道（名称）', N'V_M_TM_SYS_BaseData_channel', N'ChannelNameBase,ChannelNameBase', 1, 1, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BEE062 AS DateTime), CAST(0x0000A51D00BEE062 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1747, N'TM_Mem_Ext', N'Str_Attr_29', N'2', N'RecommenderCode', N'MemberExt', NULL, NULL, N'推荐人会员编号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BEFF6E AS DateTime), CAST(0x0000A51D00BEFF6E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1748, N'TM_Mem_Ext', N'Str_Attr_30', N'2', N'RecommenderName', N'MemberExt', NULL, NULL, N'推荐人姓名', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BF0B69 AS DateTime), CAST(0x0000A51D00BF0B69 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1749, N'TM_Mem_Ext', N'Str_Attr_31', N'2', N'POSCardNo', N'MemberExt', NULL, NULL, N'POS卡号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BF1EB8 AS DateTime), CAST(0x0000A51D00BF1EB8 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1750, N'TM_Mem_Ext', N'Bool_Attr_3', N'4', N'InfoCompletedFlag', N'MemberExt', NULL, NULL, N'信息填写是否完整', N'TD_SYS_BizOption', N'TrueOrFalse', 0, 1, 1, 1, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BF471A AS DateTime), CAST(0x0000A51D00BF471A AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1751, N'TM_Mem_Ext', N'Bool_Attr_4', N'4', N'HasChild', N'MemberExt', NULL, NULL, N'是否有孩子', NULL, NULL, 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BF5BD0 AS DateTime), CAST(0x0000A51D00BF5BD0 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1752, N'TM_Mem_Ext', N'Bool_Attr_5', N'4', N'CellPhoneValidation', N'MemberExt', NULL, NULL, N'手机是否验证', NULL, NULL, 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BF7CA2 AS DateTime), CAST(0x0000A51D00BF7CA2 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1753, N'TM_Mem_SubExt', N'Str_Attr_3', N'2', N'BabyHeight', N'MemberSubExt', N'kid', NULL, N'宝宝身高', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BF9DF4 AS DateTime), CAST(0x0000A51D00BF9DF4 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1754, N'TM_Mem_SubExt', N'Str_Attr_4', N'2', N'BabyWeight', N'MemberSubExt', N'kid', NULL, N'宝宝体重', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00BFAAB9 AS DateTime), CAST(0x0000A51D00BFAAB9 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1755, N'TM_SYS_BaseData', N'Str_Attr_11', N'1', N'CustomerLevelBase', N'BaseDataType', N'customerlevel', NULL, N'会员等级', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00E70F87 AS DateTime), CAST(0x0000A51D00E70F87 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1756, N'TM_SYS_BaseData', N'Str_Attr_12', N'1', N'CustomerLevelNameBase', N'BaseDataType', N'customerlevel', NULL, N'会员等级名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00E72003 AS DateTime), CAST(0x0000A51D00E72003 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1757, N'TM_SYS_BaseData', N'Dec_Attr_1', N'7', N'RateCustomerLevel', N'BaseDataType', N'customerlevel', NULL, N'折扣率', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00E73CF0 AS DateTime), CAST(0x0000A51D00E73CF0 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1759, N'TM_SYS_BaseData', N'Int_Attr_29', N'3', N'MaxIntergral', N'BaseDataType', N'customerlevel', NULL, N'最大抵用积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D00E7CB58 AS DateTime), CAST(0x0000A51D00E7CB58 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1760, N'TM_SYS_BaseData', N'Str_Attr_21', N'1', N'BrandCodeCustomerLevel', N'BaseDataType', N'customerlevel', NULL, N'所属品牌代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D011D22D4 AS DateTime), CAST(0x0000A51D011D22D4 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1761, N'TM_SYS_BaseData', N'Str_Attr_30', N'1', N'BrandNameCustomerLevel', N'BaseDataType', N'customerlevel', NULL, N'所属品牌名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51D011D3090 AS DateTime), CAST(0x0000A51D011D3090 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1762, N'TM_Loy_MemExt', N'Str_Attr_118', N'2', N'ProductColorLike', N'MemberExt', NULL, NULL, N'产品颜色偏好', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1 
--update TM_Loy_MemExt set [Attr]=temp.color_result
--from  
--(select MemberID ,color_result=stuff((select '',''+color_result from (
--select *,(isnull(w.colorname, '''') + ''|'' + isnull(w.color_rate, '''')) color_result
--from (
--select e.*,cast(cast(cast(e.color_count as float) /e.count_sum * 100  as int) as nvarchar(10)) color_rate from 
--(
--  select d.*,sum(color_count) over(partition by d.memberid) count_sum from (
--  select c.* ,row_number () over( partition by c.memberid order by c.color_count desc) color_order 
--   from (
        
--	  select a.memberid ,m.colorname,count(*) color_count
--        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
--		inner join V_M_TM_SYS_BaseData_colors m on b.colorcodeproduct = m.colorcode
--		----增加约束条件
--		group by a.memberid,m.colorname
--) c
--)d
--where d.color_order<=5
--)e
--)w
--)t where MemberID=w.MemberID for xml path('''')),1,1,'''')
--from TM_Loy_MemExt w 
--where ([Switch]=1 or w.MemberID in ([MemberList]))
--) temp 

--where TM_Loy_MemExt.MemberID=temp.MemberID
--and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 1, NULL, CAST(0x0000A51E011F31B6 AS DateTime), CAST(0x0000A5380139467E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1763, N'TM_Loy_MemExt', N'Date_Attr_117', N'6', N'RecentPurchaseDate', N'MemberExt', NULL, NULL, N'最近一次消费时间', NULL, NULL, 1, 0, 1, 0, 0, N'date', NULL, N'
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select a.memberid,max(a.ListDateSales) accamt
from   V_M_TM_Mem_Trade_sales a 
 where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
   and ([Switch]=1 or a.MemberID in ([MemberList]))
---   and convert(nvarchar(8),a.ListDateSales,112)>=''20150901''
group by a.MemberID
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 1, NULL, CAST(0x0000A51E011F77DB AS DateTime), CAST(0x0000A53D00B76FFE AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1764, N'TM_Loy_MemExt', N'Int_Attr_1', N'3', N'ConsumptionCounts', N'MemberExt', NULL, NULL, N'最近一年消费次数', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select  a.memberid ,isnull(b.accamt,0) accamt 
from tm_mem_master  a
left join (
         select a.MemberID ,count(*) accamt
         from   V_M_TM_Mem_Trade_sales  a
         where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
            and ([Switch]=1 or a.MemberID in ([MemberList]))
            and convert(nvarchar(8),a.ListDateSales,112)>=convert(nvarchar(8),dateadd(month,-12,cast([DatetimeNow] as datetime)),112)  
         group by a.MemberID ) b  on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A51E011FA892 AS DateTime), CAST(0x0000A54501215C93 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1765, N'TM_Loy_MemExt', N'Dec_Attr_3', N'7', N'ConsumptionYearly', N'MemberExt', NULL, NULL, N'最近一年消费额', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select  a.memberid ,isnull(b.accamt,0) accamt 
from tm_mem_master  a
left join (
           select a.MemberID ,sum(Amount) accamt
           from   V_M_TM_Mem_Trade_sales  a
           where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and ([Switch]=1 or a.MemberID in ([MemberList]))
              and convert(nvarchar(8),a.ListDateSales,112)>=convert(nvarchar(8),dateadd(month,-12,cast([DatetimeNow] as datetime)),112)  
           group by a.MemberID ) b  on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A51E011FE8B3 AS DateTime), CAST(0x0000A545012184A8 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1766, N'TM_Loy_MemExt', N'Dec_Attr_4', N'7', N'ActConsumption', N'MemberExt', NULL, NULL, N'累积消费额', NULL, NULL, 0, 1, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
    select a.MemberID ,sum(TotalMoneySales) accamt
    from (
          select a.memberid,TotalMoneySales
          from   V_M_TM_Mem_Trade_sales  a
          where convert(nvarchar(12),a.ListDateSales,112)<=[DatetimeNow]
          and (( convert(nvarchar(8),a.ListDateSales,112)>=''20150901'' and TradeSource=''线上'' ) or TradeSource=''线下'' ) 
			 and ([Switch]=1 or a.MemberID in ([MemberList]))
          union all
          select memberid,isnull(HistoryConsumeAmount,0)+isnull(HistoryConsumeModify,0) TotalMoneySales
          from V_S_TM_Loy_MemExt a
          where ([Switch]=1 or a.MemberID in ([MemberList])) 
          ) a
    group by a.MemberID
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A51E01209E27 AS DateTime), CAST(0x0000A56F01115ACF AS DateTime), 20, 0, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1767, N'TM_Loy_MemExt', N'Str_Attr_119', N'2', N'BrandLike', N'MemberExt', NULL, NULL, N'喜好品牌', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1 

--update TM_Loy_MemExt set [Attr]=temp.brand_result
--from  
--(select MemberID ,brand_result=stuff((select '',''+brand_result from (
--select *,(isnull(w.productbrandname, '''') + ''|'' + isnull(w.brand_rate, '''')) brand_result
--from (
--select e.*,cast(cast(cast(e.brand_count as float) /e.count_sum * 100  as int) as nvarchar(10)) brand_rate from 
--(
--  select d.*,sum(brand_count) over(partition by d.memberid) count_sum from (
--  select c.* ,row_number () over( partition by c.memberid order by c.brand_count desc) brand_order 
--   from (
--        select a.memberid ,c.productbrandname,count(*) brand_count
--        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
--		inner join V_M_TM_SYS_BaseData_product c on c.productcode =  b.goodscodeproduct
--        -------增加约束条件  
--       group by a.memberid,c.productbrandname
--) c
--)d
--where d.brand_order<=5
--)e
--)w
--)t where MemberID=w.MemberID for xml path('''')),1,1,'''')
--from TM_Loy_MemExt w 
--where ([Switch]=1 or w.MemberID in ([MemberList]))
--) temp 

--where TM_Loy_MemExt.MemberID=temp.MemberID
--and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 1, NULL, CAST(0x0000A51E01271224 AS DateTime), CAST(0x0000A53801396F3B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1768, N'TM_Loy_MemExt', N'Str_Attr_120', N'2', N'ProductTypeLike', N'MemberExt', NULL, NULL, N'喜欢的商品类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1 

--update TM_Loy_MemExt set [Attr]=temp.type_result
--from  
--(select MemberID ,type_result=stuff((select '',''+type_result from (
--select *,(isnull(w.CategoryName, '''') + ''|'' + isnull(w.type_rate, '''')) type_result
--from (
--select e.*,cast(cast(cast(e.type_count as float) /e.count_sum * 100  as int) as nvarchar(10)) type_rate from 
--(
--  select d.*,sum(type_count) over(partition by d.memberid) count_sum from (
--  select c.* ,row_number () over( partition by c.memberid order by c.type_count desc) type_order 
--   from (
--        select a.memberid ,c.CategoryName,count(*) type_count
--        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
--		inner join V_M_TM_SYS_BaseData_product c on c.productcode =  b.GoodsCodeProduct
--        -------增加约束条件  
--       group by a.memberid,c.CategoryName
--) c
--)d
--where d.type_order<=5
--)e
--)w
--)t where MemberID=w.MemberID for xml path('''')),1,1,'''')
--from TM_Loy_MemExt w 
--where ([Switch]=1 or w.MemberID in ([MemberList]))
--) temp 

--where TM_Loy_MemExt.MemberID=temp.MemberID
--and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 1, NULL, CAST(0x0000A51E0127E5F0 AS DateTime), CAST(0x0000A53801398CAD AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1769, N'TM_Loy_MemExt', N'Str_Attr_121', N'2', N'StoreOftenGo', N'MemberExt', NULL, NULL, N'习惯去的门店', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1 

--update TM_Loy_MemExt set [Attr]=temp.store_result
--from  
--(select MemberID, store_result=
--stuff((select '',''+store_result from (select *,(isnull(w.storename, '''') + ''|'' + isnull(w.store_rate, '''')) store_result
--from (
--select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) store_rate
--from 
--(
-- select *,sum(cnum) over(partition by memberid)  count_sum 
-- from (SELECT b.*,
--                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
--        FROM (SELECT a.memberid,m.storename,count(*) cnum
--              FROM V_M_TM_Mem_Trade_sales a
--			  -------增加约束条件  
--			  inner join V_M_TM_SYS_BaseData_store m
--			  on m.storecode = a.StoreCodeSales
--              group by a.memberid, m.storename) b
--	   ) b
-- where b.count_num <= 3  ) t  ) w) t where MemberID=w.MemberID for xml path('''')), 1, 1, '''') 
--from TM_Loy_MemExt w 
--where ([Switch]=1 or w.MemberID in ([MemberList]))
--) temp 

--where TM_Loy_MemExt.MemberID=temp.MemberID
--and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))

', NULL, 1, NULL, CAST(0x0000A51E012804B0 AS DateTime), CAST(0x0000A5380139AB6B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1770, N'TM_Loy_MemExt', N'Str_Attr_122', N'2', N'PromotionLike', N'MemberExt', NULL, NULL, N'促销偏好', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1 

--update TM_Loy_MemExt set [Attr]=temp.promotion_result
--from  
--(select MemberID, promotion_result=
--stuff((select '',''+promotion_result from (select *,(isnull(w.promotionname, '''') + ''|'' + isnull(w.promotion_rate, '''')) promotion_result
--from (
--select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) promotion_rate
--from 
--(
-- select *,sum(cnum) over(partition by memberid)  count_sum 
-- from (SELECT b.*,
--                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
--        FROM (SELECT a.memberid,m.promotionname,count(*) cnum
--              FROM TM_POS_MemberPromotion a
--			  -------增加约束条件  
--			  inner join V_M_TM_SYS_BaseData_promotion m
--			  on a.promotionid = m.promotionid
--              group by a.memberid, m.promotionname) b
--	   ) b
-- where b.count_num <= 3  ) t  ) w) t where MemberID=w.MemberID for xml path('''')), 1, 1, '''') 
--from TM_Loy_MemExt w 
--where ([Switch]=1 or w.MemberID in ([MemberList]))
--) temp 

--where TM_Loy_MemExt.MemberID=temp.MemberID
--and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))


', NULL, 1, NULL, CAST(0x0000A52000C1535F AS DateTime), CAST(0x0000A5380139C5AD AS DateTime), 20, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1771, N'TM_Mem_Ext', N'Str_Attr_32', N'2', N'MemberCode', N'MemberExt', NULL, NULL, N'会员代码', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52300000000 AS DateTime), CAST(0x0000A53300AE49B0 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (1772, N'TM_Mem_TradeDetail', N'Bool_Attr_1', N'2', N'IsReturn', N'MemberTradeDetail', N'sales', N'payment', N'是否退货', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52900000000 AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2772, N'TM_Mem_Trade', N'Dec_Attr_7', N'7', N'TotalMoneySales', N'MemberTrade', N'sales', NULL, N'积分计算金额', NULL, NULL, 0, 1, 1, 1, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52B00057B70 AS DateTime), CAST(0x0000A52B00057B70 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2773, N'TM_SYS_BaseData', N'Date_Attr_10', N'5', N'StartDatePromotion', N'BaseDataType', N'promotion', NULL, N'有效起始时间', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52C00F40F26 AS DateTime), CAST(0x0000A52C00F40F26 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2774, N'TM_SYS_BaseData', N'Date_Attr_31', N'5', N'EndDatePromotion', N'BaseDataType', N'promotion', NULL, N'有效结束时间', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52C00F420F8 AS DateTime), CAST(0x0000A52C00F420F8 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2775, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'ChannelNameBase', N'BaseDataType', N'channel', NULL, N'渠道名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4360151F63C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2776, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'ChannelCodeBase', N'BaseDataType', N'channel', NULL, N'渠道代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A4360151F63C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2777, N'TM_Loy_MemExt', N'Dec_Attr_5', N'7', N'ActConsumption_3', N'MemberExt', NULL, NULL, N'3个月内的消费额', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select  a.memberid ,isnull(b.accamt,0) accamt 
from tm_mem_master  a
left join (
           select a.MemberID ,sum(Amount) accamt
           from   V_M_TM_Mem_Trade_sales  a
           where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and ([Switch]=1 or a.MemberID in ([MemberList]))
              and convert(nvarchar(8),a.ListDateSales,112)>=convert(nvarchar(8),dateadd(month,-3,cast([DatetimeNow] as datetime)),112)  
           group by a.MemberID ) b  on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A52C011DD8B2 AS DateTime), CAST(0x0000A5450121CE85 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2778, N'TM_Loy_MemExt', N'Dec_Attr_6', N'7', N'ActConsumptionCounts', N'MemberExt', NULL, NULL, N'3个月内的消费次数', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'

update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select  a.memberid ,isnull(b.accamt,0) accamt 
from tm_mem_master  a
left join (
         select a.MemberID ,count(*) accamt
         from   V_M_TM_Mem_Trade_sales  a
         where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
            and ([Switch]=1 or a.MemberID in ([MemberList]))
            and convert(nvarchar(8),a.ListDateSales,112)>=convert(nvarchar(8),dateadd(month,-3,cast([DatetimeNow] as datetime)),112)  
         group by a.MemberID ) b  on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A52C011E3C5E AS DateTime), CAST(0x0000A5450121F60E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2779, N'TM_Loy_MemExt', N'Str_Attr_123', N'2', N'BuySales_3', N'MemberExt', NULL, NULL, N'三个月内购买的商品', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A52C011ECD4A AS DateTime), CAST(0x0000A52C011ECD4A AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2780, N'TM_Loy_MemExt', N'Dec_Attr_7', N'7', N'LastConsumeMoney', N'MemberExt', NULL, NULL, N'最近一次消费额', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select c.memberid,c.amount accamt from(
SELECT b.*, ROW_NUMBER() over(partition by b.memberid order by b.ListDateSales desc) count_num
				from V_M_TM_Mem_Trade_sales b
			) c
			where c.count_num=1
	   and ([Switch]=1 or c.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A52C011F1E16 AS DateTime), CAST(0x0000A53600F8AD45 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2781, N'TM_Loy_MemExt', N'Date_Attr_1', N'5', N'LastConsumeTime', N'MemberExt', NULL, NULL, N'最近一次消费时间(重复废除）', NULL, NULL, 0, 1, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A52C011F4317 AS DateTime), CAST(0x0000A52C011F4317 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2782, N'TM_Loy_MemExt', N'Dec_Attr_10', N'3', N'CumulativeIntegral_6', N'MemberExt', NULL, NULL, N'最近半年累积积分', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'----忠诚度的负值积分是需要减去的；
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accpoint
from  
(
select  a.MemberID  , sum(isnull(b.accpoint,0)) accpoint
from tm_mem_master  a
left join (
           select  b.MemberID  , a.changevalue  accpoint
           from TL_Mem_AccountChange a,tm_mem_account b
           where a.AccountID=b.AccountID
           and   convert(nvarchar(8),a.AddedDate,112)>=
                 convert(nvarchar(8),dateadd(month,-6,cast([DatetimeNow] as datetime)),112)
           and   convert(nvarchar(8),a.AddedDate,112)<=[DatetimeNow] 
           and a.changevalue>0
           and a.HasReverse=0 
           and b.AccountType=''3''
           and ([Switch]=1 or b.MemberID in ([MemberList]))
           union all
           select  b.MemberID  , a.changevalue  accpoint
           from TL_Mem_AccountChange a,tm_mem_account b
           where a.AccountID=b.AccountID
           and   convert(nvarchar(8),a.AddedDate,112)>=
                 convert(nvarchar(8),dateadd(month,-6,cast([DatetimeNow] as datetime)),112)
           and   convert(nvarchar(8),a.AddedDate,112)<=[DatetimeNow] 
           and a.changevalue<0
           and a.HasReverse=0 
           and b.AccountType=''3''
           and a.AccountChangeType=''loy''
           and ([Switch]=1 or b.MemberID in ([MemberList]))
           )b   on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
group by a.MemberID 
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A52C011FABF0 AS DateTime), CAST(0x0000A5450122204C AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2783, N'TM_Loy_MemExt', N'Dec_Attr_11', N'3', N'ConsumeIntegral_6', N'MemberExt', NULL, NULL, N'最近半年消耗积分', NULL, NULL, 1, 1, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accpoint
from  
(
select  a.memberid ,isnull(b.accpoint,0) accpoint
from tm_mem_master  a
left join (
           select  b.MemberID  , -sum(a.changevalue ) accpoint
           from TL_Mem_AccountChange a,tm_mem_account b
           where a.AccountID=b.AccountID
           and   convert(nvarchar(8),a.AddedDate,112)>=
                 convert(nvarchar(8),dateadd(month,-6,cast([DatetimeNow] as datetime)),112)
		   and   convert(nvarchar(8),a.AddedDate,112)<=[DatetimeNow] 
           and a.changevalue < 0
           and a.HasReverse=0 
           and b.AccountType=''3''
           and a.AccountChangeType<>''loy''
           and ([Switch]=1 or b.MemberID in ([MemberList]))
           group by b.MemberID  )  b on a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))

) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A52C011FE1E3 AS DateTime), CAST(0x0000A54501224BDB AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2784, N'TM_Mem_Trade', N'Int_Attr_5', N'3', N'ConsumeIntegralSales', N'MemberTrade', N'sales', NULL, N'订单消费积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52C01230146 AS DateTime), CAST(0x0000A52C01230146 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2785, N'TM_Mem_Trade', N'Str_Attr_8', N'2', N'PayMethodSales', N'MemberTrade', N'sales', NULL, N'支付方式', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52C01233A79 AS DateTime), CAST(0x0000A52C01233A79 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2786, N'TM_Mem_Trade', N'Dec_Attr_8', N'7', N'IntergralAccountingSales', N'MemberTrade', N'sales', NULL, N'积分占比', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52C012384E7 AS DateTime), CAST(0x0000A52C012384E7 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2787, N'TM_Mem_Trade', N'Str_Attr_182', N'1', N'SalesTradeType', N'MemberTrade', N'sales', NULL, N'订单类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52D0105CEF7 AS DateTime), CAST(0x0000A52D0105E05B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2788, N'TM_Mem_Ext', N'Str_Attr_33', N'2', N'MemberIntroducer', N'MemberExt', NULL, NULL, N'推荐人手机号', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52F00B73641 AS DateTime), CAST(0x0000A52F00B73641 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2789, N'TM_Mem_Trade', N'Str_Attr_1', N'2', N'ProductCodeGift', N'MemberTrade', N'gift', NULL, N'产品代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52F00ED9008 AS DateTime), CAST(0x0000A52F00ED9008 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2790, N'TM_Mem_Trade', N'Str_Attr_2', N'2', N'ProductNameGift', N'MemberTrade', N'gift', NULL, N'产品名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52F00EDA423 AS DateTime), CAST(0x0000A52F00EDA423 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2791, N'TM_Mem_Trade', N'Str_Attr_3', N'3', N'IntergralGift', N'MemberTrade', N'gift', NULL, N'所需积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52F00EDFBD4 AS DateTime), CAST(0x0000A52F00EE239E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2792, N'TM_Mem_Trade', N'Int_Attr_1', N'3', N'CountsGift', N'MemberTrade', N'gift', NULL, N'购买数量', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52F00EE1528 AS DateTime), CAST(0x0000A52F00EE1528 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2793, N'TM_Mem_Trade', N'Dec_Attr_1', N'7', N'SourcePriceGift', N'MemberTrade', N'gift', NULL, N'产品原价', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52F00EE695F AS DateTime), CAST(0x0000A52F00EE695F AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2794, N'TM_SYS_BaseData', N'Int_Attr_32', N'3', N'LevelUpInt', N'BaseDataType', N'customerlevel', NULL, N'升级积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A52F00F42386 AS DateTime), CAST(0x0000A52F00F42386 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2795, N'TM_Loy_MemExt', N'Str_Attr_2', N'1', N'MemberActivity', N'MemberExt', NULL, NULL, N'会员活跃度', N'TD_SYS_BizOption', N'MemActivity', 0, 0, 0, 0, 0, N'select', NULL, N'update TM_Loy_MemExt set [Attr]=temp.memactstatus
from  
(

select a.memberid,
case 
 when  (a.RecentPurchaseDate is null and  datediff(dd,a.RegisterDate,cast ([DatetimeNow] as datetime))>365 )
       or 
	   (a.RecentPurchaseDate is not null and  datediff(dd,a.RecentPurchaseDate,cast ([DatetimeNow] as datetime))>365 )
 then ''2''
 when  datediff(dd,isnull(a.RecentPurchaseDate,''1900-01-01''),cast ([DatetimeNow] as datetime)) >=182
   and datediff(dd,isnull(a.RecentPurchaseDate,''1900-01-01''),cast ([DatetimeNow] as datetime)) <=365 then ''1''
 when  datediff(dd,isnull(b.ListDateSales,''1900-01-01''),cast ([DatetimeNow] as datetime))<182  then  ''3''  end memactstatus 

from V_U_TM_Mem_Info a
left join  (select memberid,max(ListDateSales) ListDateSales
           from (
           (select  a.*,ROW_NUMBER() over (partition by memberid order by ListDateSales   ) serial 
           from V_M_TM_Mem_Trade_sales a )) t
           where serial<>1 
           group by memberid ) b on a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A53000EBC288 AS DateTime), CAST(0x0000A57F011D074F AS DateTime), 20, 0, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2796, N'TM_Loy_MemExt', N'Dec_Attr_12', N'3', N'UsedIntergral', N'MemberExt', NULL, NULL, N'会员已使用积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select d.memberid,c.sum_value accamt from (
select b.accountid,sum(b.changevalue) sum_value
from( 
select a.accountid , a.changevalue from TL_Mem_AccountChange a
where a.addeddate<=cast([DatetimeNow] as datetime)
and a.changevalue < 0
and a.HasReverse=0
------增加约束条件
)b
group by b.AccountID
)c
left join tm_mem_account d
on c.AccountID = d.accountid
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A53000EC0681 AS DateTime), CAST(0x0000A539016AF456 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2797, N'TM_Loy_MemExt', N'Dec_Attr_13', N'3', N'ComeMaturityIntergral', N'MemberExt', NULL, NULL, N'会员快到期积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A53000EC4CF4 AS DateTime), CAST(0x0000A53000EC4CF4 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2798, N'TM_Loy_MemExt', N'Dec_Attr_14', N'3', N'UpdateIntergral', N'MemberExt', NULL, NULL, N'升级所需积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A53100D9FFFF AS DateTime), CAST(0x0000A53100D9FFFF AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2799, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'AreaNameBase', N'BaseDataType', N'area', NULL, N'大区名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A53200ABE70A AS DateTime), CAST(0x0000A53200ABE70A AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2800, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'AreaCodeBase', N'BaseDataType', N'area', NULL, N'大区代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A53200ABF378 AS DateTime), CAST(0x0000A53200ABF378 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2801, N'TM_Mem_Ext', N'Str_Attr_34', N'2', N'BrandNameMember', N'MemberExt', NULL, NULL, N'会员注册品牌', NULL, NULL, 0, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45200000000 AS DateTime), CAST(0x0000A45200000000 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2802, N'TM_Mem_Ext', N'Str_Attr_35', N'2', N'AreaMember', N'MemberExt', NULL, NULL, N'会员所属大区（代码）', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A45200000000 AS DateTime), CAST(0x0000A45200000000 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2803, N'TM_Loy_MemExt', N'Str_Attr_124', N'2', N'BestProductType', N'MemberExt', NULL, NULL, N'产品种类偏好', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A53300DF200B AS DateTime), CAST(0x0000A53300DF200B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2804, N'TM_Loy_MemExt', N'Str_Attr_125', N'2', N'BestProductMaterial', N'MemberExt', NULL, NULL, N'产品材质偏好', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1 
--update TM_Loy_MemExt set [Attr]=temp.material_result
--from  
--(select MemberID ,material_result=stuff((select '',''+material_result from (
--select *,(isnull(w.materialname, '''') + ''|'' + isnull(w.material_rate, '''')) material_result
--from (
--select e.*,cast(cast(cast(e.material_count as float) /e.count_sum * 100  as int) as nvarchar(10)) material_rate from 
--(
--  select d.*,sum(material_count) over(partition by d.memberid) count_sum from (
--  select c.* ,row_number () over( partition by c.memberid order by c.material_count desc) material_order 
--   from (
        
--	  select a.memberid ,b.materialname,count(*) material_count
--        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		
--		----增加约束条件
--		group by a.memberid,b.materialname
--) c
--)d
--where d.material_order<=5
--)e
--)w
--)t where MemberID=w.MemberID for xml path('''')),1,1,'''')
--from TM_Loy_MemExt w 
--where ([Switch]=1 or w.MemberID in ([MemberList]))
--) temp 

--where TM_Loy_MemExt.MemberID=temp.MemberID
--and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
select 1', NULL, 1, NULL, CAST(0x0000A53300E18730 AS DateTime), CAST(0x0000A5380139E323 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2807, N'TM_Mem_TradeDetail', N'Str_Attr_12', N'2', N'ProductBrandName', N'MemberTradeDetail', N'sales', N'product', N'产品品牌（订单明细）', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', 1, 1, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2808, N'TM_Mem_TradeDetail', N'Str_Attr_13', N'2', N'ProductTypeName', N'MemberTradeDetail', N'sales', N'product', N'产品品类（订单明细）', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', 1, 1, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2809, N'TM_Mem_TradeDetail', N'Str_Attr_14', N'2', N'ProductLineName1', N'MemberTradeDetail', N'sales', N'product', N'产品线1（订单明细）', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', 1, 1, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2810, N'TM_Mem_TradeDetail', N'Str_Attr_15', N'2', N'ProductLineName2', N'MemberTradeDetail', N'sales', N'product', N'产品线2（订单明细）', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', 1, 1, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2811, N'TM_Mem_TradeDetail', N'Str_Attr_16', N'2', N'ProductStatusName', N'MemberTradeDetail', N'sales', N'product', N'产品状态（订单明细）', N'TD_SYS_BizOption', N'ProductStatus', 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2812, N'TM_Mem_TradeDetail', N'Str_Attr_17', N'2', N'UnitPriceTypeName', N'MemberTradeDetail', N'sales', N'product', N'客单价区间', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2813, N'TM_Mem_TradeDetail', N'Str_Attr_18', N'2', N'SinglePriceTypeName', N'MemberTradeDetail', N'sales', N'product', N'单品价格区间', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A51400EB7F3C AS DateTime), NULL, 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2814, N'TM_Loy_MemExt', N'Dec_Attr_15', N'3', N'AccConsumeIntegral', N'MemberExt', NULL, NULL, N'累计消费积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accpoint
from  
(
select  b.MemberID  , -sum(a.changevalue ) accpoint
from TL_Mem_AccountChange a,tm_mem_account b
where a.AccountID=b.AccountID
and   convert(nvarchar(8),a.AddedDate,112)<=[DatetimeNow] 
and a.changevalue < 0
and a.HasReverse=0 
and b.AccountType=''3''
and a.AccountChangeType<>''loy''
and ([Switch]=1 or b.MemberID in ([MemberList]))
group by b.MemberID 

) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 1, NULL, CAST(0x0000A53701403406 AS DateTime), CAST(0x0000A53E014C45CD AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2815, N'TM_Loy_MemExt', N'Dec_Attr_16', N'3', N'AccCumulativeIntegral', N'MemberExt', NULL, NULL, N'累计获取积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accpoint
from  
(
select  m.MemberID  , sum(m.accpoint ) accpoint from
(
select  b.MemberID  , a.changevalue  accpoint
from TL_Mem_AccountChange a,tm_mem_account b
where a.AccountID=b.AccountID
and   convert(nvarchar(8),a.AddedDate,112)<=[DatetimeNow] 
and a.changevalue>0
and a.HasReverse=0 
and b.AccountType=''3''
and ([Switch]=1 or b.MemberID in ([MemberList]))
union all
select  b.MemberID  , a.changevalue  accpoint
from TL_Mem_AccountChange a,tm_mem_account b
where a.AccountID=b.AccountID
and   convert(nvarchar(8),a.AddedDate,112)<=[DatetimeNow] 
and a.changevalue<0
and a.HasReverse=0 
and b.AccountType=''3''
and a.AccountChangeType=''loy''
and ([Switch]=1 or b.MemberID in ([MemberList]))
union all
select MemberID, HistoryPoint accpoint from V_S_TM_Loy_MemExt
where HistoryPoint<>0
and ([Switch]=1 or MemberID in ([MemberList]))
)m
group by m.MemberID 
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A53701405EAB AS DateTime), CAST(0x0000A5400170B349 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2816, N'TM_Mem_Ext', N'Bool_Attr_6', N'4', N'IsBirthday', N'MemberExt', NULL, NULL, N'是否享受生日优惠', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A53800EB59E1 AS DateTime), CAST(0x0000A53800EB59E1 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2817, N'TM_Mem_Ext', N'Str_Attr_51', N'1', N'IsRegisteredFlag', N'MemberExt', NULL, NULL, N'是否注册送积分', NULL, NULL, 0, 1, 1, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A538010EFF2C AS DateTime), CAST(0x0000A538010EFF2C AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2818, N'TM_Mem_Ext', N'Str_Attr_52', N'1', N'IsdRecommendedFlag', N'MemberExt', NULL, NULL, N'是否推荐人送积分', NULL, NULL, 0, 1, 1, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A538010F3CEB AS DateTime), CAST(0x0000A538010F3CEB AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2819, N'TM_Mem_Ext', N'Bool_Attr_7', N'4', N'IsModifyBirth', N'MemberExt', NULL, NULL, N'是否修改过生日', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5380110515E AS DateTime), CAST(0x0000A5380110515E AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2820, N'TM_Loy_MemExt', N'Str_Attr_126', N'2', N'OftenBuyTime_1N', N'MemberExt', NULL, NULL, N'经常购买时间段（名称1）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.day_result
from  
(select *,isnull(w.the_day, '''') + ''|'' + cast(w.day_count as nvarchar(10)) day_result
   ---(isnull(w.the_day, '''') + ''|'' + isnull(w.day_rate, '''')) day_result
from (
select e.*,cast(cast(cast(e.day_count as float) /e.count_sum * 100  as int) as nvarchar(10)) day_rate from 
(
  select d.*,sum(day_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.day_count desc) day_order 
   from (
        
	  select a.memberid ,a.the_day,count(*) day_count
        from (select m.memberid,m.ListDateSales, 
case n.the_day
      when ''星期一'' then ''周一''
	  when ''星期二'' then ''周二''
	  when ''星期三'' then ''周三''
	  when ''星期四'' then ''周四''
	  when ''星期五'' then ''周五''
	  when ''星期六'' then ''周六''
	  when ''星期日'' then ''周日'' 
	  end the_day
from V_M_TM_Mem_Trade_sales m
left join TL_Sys_time n
on convert(nvarchar(8),m.ListDateSales,112)= convert(nvarchar(8),n.the_date,112)
where convert(nvarchar(8),m.ListDateSales,112)<=[DatetimeNow]
and  ([Switch]=1 or m.MemberID in ([MemberList]))
)a
		----增加约束条件
		group by a.memberid,a.the_day
) c
)d
where d.day_order<=5
)e
)w
where day_order = 1
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500E4EB96 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2822, N'TM_Loy_MemExt', N'Str_Attr_127', N'2', N'OftenBuyTime_2N', N'MemberExt', NULL, NULL, N'经常购买时间段（名称2）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.day_result
from  
(select *,isnull(w.the_day, '''') + ''|'' + cast(w.day_count as nvarchar(10)) day_result
---(isnull(w.the_day, '''') + ''|'' + isnull(w.day_rate, '''')) day_result
from (
select e.*,cast(cast(cast(e.day_count as float) /e.count_sum * 100  as int) as nvarchar(10)) day_rate from 
(
  select d.*,sum(day_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.day_count desc) day_order 
   from (
        
	  select a.memberid ,a.the_day,count(*) day_count
        from (select m.memberid,m.ListDateSales, 
case n.the_day
      when ''星期一'' then ''周一''
	  when ''星期二'' then ''周二''
	  when ''星期三'' then ''周三''
	  when ''星期四'' then ''周四''
	  when ''星期五'' then ''周五''
	  when ''星期六'' then ''周六''
	  when ''星期日'' then ''周日'' 
	  end the_day
from V_M_TM_Mem_Trade_sales m
left join TL_Sys_time n
on convert(nvarchar(8),m.ListDateSales,112)= convert(nvarchar(8),n.the_date,112)
where convert(nvarchar(8),m.ListDateSales,112)<=[DatetimeNow]
and  ([Switch]=1 or m.MemberID in ([MemberList]))
)a
		----增加约束条件
		group by a.memberid,a.the_day
) c
)d
where d.day_order<=5
)e
)w
where day_order = 2
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500E4F90B AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2824, N'TM_Loy_MemExt', N'Str_Attr_128', N'2', N'OftenBuyTime_3N', N'MemberExt', NULL, NULL, N'经常购买时间段（名称3）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.day_result
from  
(select *,isnull(w.the_day, '''') + ''|'' + cast(w.day_count as nvarchar(10)) day_result
     ----(isnull(w.the_day, '''') + ''|'' + isnull(w.day_rate, '''')) day_result
from (
select e.*,cast(cast(cast(e.day_count as float) /e.count_sum * 100  as int) as nvarchar(10)) day_rate from 
(
  select d.*,sum(day_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.day_count desc) day_order 
   from (
        
	  select a.memberid ,a.the_day,count(*) day_count
        from (select m.memberid,m.ListDateSales, 
case n.the_day
      when ''星期一'' then ''周一''
	  when ''星期二'' then ''周二''
	  when ''星期三'' then ''周三''
	  when ''星期四'' then ''周四''
	  when ''星期五'' then ''周五''
	  when ''星期六'' then ''周六''
	  when ''星期日'' then ''周日'' 
	  end the_day
from V_M_TM_Mem_Trade_sales m
left join TL_Sys_time n
on convert(nvarchar(8),m.ListDateSales,112)= convert(nvarchar(8),n.the_date,112)
where convert(nvarchar(8),m.ListDateSales,112)<=[DatetimeNow]
and  ([Switch]=1 or m.MemberID in ([MemberList]))
)a
		----增加约束条件
		group by a.memberid,a.the_day
) c
)d
where d.day_order<=5
)e
)w
where day_order = 3
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500E50681 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2826, N'TM_Loy_MemExt', N'Str_Attr_129', N'2', N'BrandLike_1N', N'MemberExt', NULL, NULL, N'喜好品牌（名称1）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.brand_result
from  
(
select *,isnull(w.productbrandnamebase, '''') + ''|'' + cast(w.brand_count as nvarchar(10)) brand_result
         ---(isnull(w.productbrandnamebase, '''') + ''|'' + isnull(w.brand_rate, '''')) brand_result
from (
select e.*,cast(cast(cast(e.brand_count as float) /e.count_sum * 100  as int) as nvarchar(10)) brand_rate from 
(
  select d.*,sum(brand_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.brand_count desc) brand_order 
   from (
        select a.memberid ,c.productbrandnamebase,count(*) brand_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.productcode =  b.goodscodeproduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.productbrandnamebase
) c
)d
where d.brand_order<=5
)e
)w
where brand_order = 1
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DD22E5 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2828, N'TM_Loy_MemExt', N'Str_Attr_130', N'2', N'BrandLike_2N', N'MemberExt', NULL, NULL, N'喜好品牌（名称2）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.brand_result
from  
(
select *,isnull(w.productbrandnamebase, '''') + ''|'' + cast(w.brand_count as nvarchar(10)) brand_result
       ----(isnull(w.productbrandnamebase, '''') + ''|'' + isnull(w.brand_rate, '''')) brand_result
from (
select e.*,cast(cast(cast(e.brand_count as float) /e.count_sum * 100  as int) as nvarchar(10)) brand_rate from 
(
  select d.*,sum(brand_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.brand_count desc) brand_order 
   from (
        select a.memberid ,c.productbrandnamebase,count(*) brand_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.productcode =  b.goodscodeproduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.productbrandnamebase
) c
)d
where d.brand_order<=5
)e
)w
where brand_order = 2
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DD2FF8 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2830, N'TM_Loy_MemExt', N'Str_Attr_131', N'2', N'BrandLike_3N', N'MemberExt', NULL, NULL, N'喜好品牌（名称3）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.brand_result
from  
(
select *,isnull(w.productbrandnamebase, '''') + ''|'' + cast(w.brand_count as nvarchar(10)) brand_result
----(isnull(w.productbrandnamebase, '''') + ''|'' + isnull(w.brand_rate, '''')) brand_result
from (
select e.*,cast(cast(cast(e.brand_count as float) /e.count_sum * 100  as int) as nvarchar(10)) brand_rate from 
(
  select d.*,sum(brand_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.brand_count desc) brand_order 
   from (
        select a.memberid ,c.productbrandnamebase,count(*) brand_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.productcode =  b.goodscodeproduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.productbrandnamebase
) c
)d
where d.brand_order<=5
)e
)w
where brand_order = 3
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DD4117 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2832, N'TM_Loy_MemExt', N'Str_Attr_132', N'2', N'ProductTypeLike_1N', N'MemberExt', NULL, NULL, N'喜欢的商品类型（名称1）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.type_result
from  (
select *,isnull(w.CategoryName, '''') + ''|'' + cast(w.type_count as nvarchar(10)) type_result
    ----(isnull(w.CategoryName, '''') + ''|'' + isnull(w.type_rate, '''')) type_result
from (
select e.*,cast(cast(cast(e.type_count as float) /e.count_sum * 100  as int) as nvarchar(10)) type_rate from 
(
  select d.*,sum(type_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.type_count desc) type_order 
   from (
        select a.memberid ,c.CategoryName,count(*) type_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.ProductCode =  b.GoodsCodeProduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.CategoryName
) c
)d
where d.type_order<=5
)e
)w
 where type_order = 1
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DDE6D1 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2834, N'TM_Loy_MemExt', N'Str_Attr_133', N'2', N'ProductTypeLike_2N', N'MemberExt', NULL, NULL, N'喜欢的商品类型（名称2）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.type_result
from  (
select *,isnull(w.CategoryName, '''') + ''|'' + cast(w.type_count as nvarchar(10)) type_result
           ----(isnull(w.CategoryName, '''') + ''|'' + isnull(w.type_rate, '''')) type_result
from (
select e.*,cast(cast(cast(e.type_count as float) /e.count_sum * 100  as int) as nvarchar(10)) type_rate from 
(
  select d.*,sum(type_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.type_count desc) type_order 
   from (
        select a.memberid ,c.CategoryName,count(*) type_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.ProductCode =  b.GoodsCodeProduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.CategoryName
) c
)d
where d.type_order<=5
)e
)w
 where type_order = 2
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DE04B8 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2836, N'TM_Loy_MemExt', N'Str_Attr_134', N'2', N'ProductTypeLike_3N', N'MemberExt', NULL, NULL, N'喜欢的商品类型（名称3）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.type_result
from  (
select *,isnull(w.CategoryName, '''') + ''|'' + cast(w.type_count as nvarchar(10)) type_result
          ----(isnull(w.CategoryName, '''') + ''|'' + isnull(w.type_rate, '''')) type_result
from (
select e.*,cast(cast(cast(e.type_count as float) /e.count_sum * 100  as int) as nvarchar(10)) type_rate from 
(
  select d.*,sum(type_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.type_count desc) type_order 
   from (
        select a.memberid ,c.CategoryName,count(*) type_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.ProductCode =  b.GoodsCodeProduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.CategoryName
) c
)d
where d.type_order<=5
)e
)w
 where type_order = 3
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DE2397 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2838, N'TM_Loy_MemExt', N'Str_Attr_135', N'2', N'StoreOftenGo_1N', N'MemberExt', NULL, NULL, N'习惯去的门店（名称1）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.store_result
from  (
select *,isnull(w.storename, '''') + ''|'' + cast(w.cnum as nvarchar(10)) store_result 
           ----(isnull(w.storename, '''') + ''|'' + isnull(w.store_rate, '''')) store_result
from (
select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) store_rate
from 
(
 select *,sum(cnum) over(partition by memberid)  count_sum 
 from (SELECT b.*,
                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
        FROM (SELECT a.memberid,m.storename,count(*) cnum
              FROM V_M_TM_Mem_Trade_sales a
			  -------增加约束条件  
			  inner join V_M_TM_SYS_BaseData_store m
			  on m.storecode = a.StoreCodeSales
			  where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
              group by a.memberid, m.storename) b
	   ) b
 where b.count_num <= 3  ) t  ) w
 where count_num = 1
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DF0ABA AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2840, N'TM_Loy_MemExt', N'Str_Attr_136', N'2', N'StoreOftenGo_2N', N'MemberExt', NULL, NULL, N'习惯去的门店（名称2）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.store_result
from  (
select *,isnull(w.storename, '''') + ''|'' + cast(w.cnum as nvarchar(10)) store_result 
            ----(isnull(w.storename, '''') + ''|'' + isnull(w.store_rate, '''')) store_result
from (
select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) store_rate
from 
(
 select *,sum(cnum) over(partition by memberid)  count_sum 
 from (SELECT b.*,
                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
        FROM (SELECT a.memberid,m.storename,count(*) cnum
              FROM V_M_TM_Mem_Trade_sales a
			  -------增加约束条件  
			  inner join V_M_TM_SYS_BaseData_store m
			  on m.storecode = a.StoreCodeSales
			  where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
              group by a.memberid, m.storename) b
	   ) b
 where b.count_num <= 3  ) t  ) w
 where count_num = 2
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DF190C AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2842, N'TM_Loy_MemExt', N'Str_Attr_137', N'2', N'StoreOftenGo_3N', N'MemberExt', NULL, NULL, N'习惯去的门店（名称3）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.store_result
from  (
select *,isnull(w.storename, '''') + ''|'' + cast(w.cnum as nvarchar(10)) store_result 
         ----(isnull(w.storename, '''') + ''|'' + isnull(w.store_rate, '''')) store_result
from (
select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) store_rate
from 
(
 select *,sum(cnum) over(partition by memberid)  count_sum 
 from (SELECT b.*,
                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
        FROM (SELECT a.memberid,m.storename,count(*) cnum
              FROM V_M_TM_Mem_Trade_sales a
			  -------增加约束条件  
			  inner join V_M_TM_SYS_BaseData_store m
			  on m.storecode = a.StoreCodeSales
			  where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
              group by a.memberid, m.storename) b
	   ) b
 where b.count_num <= 3  ) t  ) w
 where count_num = 3
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500DF2929 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2845, N'TM_Loy_MemExt', N'Str_Attr_138', N'2', N'PromotionLike_1N', N'MemberExt', NULL, NULL, N'喜欢的促销方案类型（名称1）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=temp.promotion_result
from  (
select *,isnull(w.promotionname, '''') + ''|'' + cast(w.cnum as nvarchar(10)) promotion_result 
         ----(isnull(w.promotionname, '''') + ''|'' + isnull(w.promotion_rate, '''')) promotion_result
from (
select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) promotion_rate
from 
(
 select *,sum(cnum) over(partition by memberid)  count_sum 
 from (SELECT b.*,
                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
        FROM (SELECT a.memberid,n.promotionname,count(*) cnum
              FROM V_M_TM_Mem_Trade_sales a
			  -------增加约束条件  
			  inner join TM_POS_tradedetailPromotion m
			  on a.TradeID = m.TradeID
			  inner join V_M_TM_SYS_BaseData_promotion n
			  on m.promotionid = n.promotionid
			  where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
              group by a.memberid, n.promotionname) b
	   ) b
 where b.count_num <= 3  ) t  
 ) w
  where count_num = 1
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500E02700 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2847, N'TM_Loy_MemExt', N'Str_Attr_139', N'2', N'PromotionLike_2N', N'MemberExt', NULL, NULL, N'喜欢的促销方案类型（名称2）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.promotion_result
from  (
select *,isnull(w.promotionname, '''') + ''|'' + cast(w.cnum as nvarchar(10)) promotion_result 
----(isnull(w.promotionname, '''') + ''|'' + isnull(w.promotion_rate, '''')) promotion_result
from (
select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) promotion_rate
from 
(
 select *,sum(cnum) over(partition by memberid)  count_sum 
 from (SELECT b.*,
                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
        FROM (SELECT a.memberid,n.promotionname,count(*) cnum
              FROM V_M_TM_Mem_Trade_sales a
			  -------增加约束条件  
			  inner join TM_POS_tradedetailPromotion m
			  on a.TradeID = m.TradeID
			  inner join V_M_TM_SYS_BaseData_promotion n
			  on m.promotionid = n.promotionid
			  where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
              group by a.memberid, n.promotionname) b
	   ) b
 where b.count_num <= 3  ) t  
 ) w
  where count_num = 2
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500E03502 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2849, N'TM_Loy_MemExt', N'Str_Attr_140', N'2', N'PromotionLike_3N', N'MemberExt', NULL, NULL, N'喜欢的促销方案类型（名称3）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.promotion_result
from  (
select *,isnull(w.promotionname, '''') + ''|'' + cast(w.cnum as nvarchar(10)) promotion_result 
---(isnull(w.promotionname, '''') + ''|'' + isnull(w.promotion_rate, '''')) promotion_result
from (
select *,  cast(cast(cast(cnum as float) /count_sum * 100 as int) as nvarchar(10)) promotion_rate
from 
(
 select *,sum(cnum) over(partition by memberid)  count_sum 
 from (SELECT b.*,
                ROW_NUMBER() over(partition by b.memberid order by b.cnum desc) count_num
        FROM (SELECT a.memberid,n.promotionname,count(*) cnum
              FROM V_M_TM_Mem_Trade_sales a
			  -------增加约束条件  
			  inner join TM_POS_tradedetailPromotion m
			  on a.TradeID = m.TradeID
			  inner join V_M_TM_SYS_BaseData_promotion n
			  on m.promotionid = n.promotionid
			  where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
              group by a.memberid, n.promotionname) b
	   ) b
 where b.count_num <= 3  ) t  
 ) w
  where count_num = 3
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539013CD713 AS DateTime), CAST(0x0000A54500E041EF AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2851, N'TM_Loy_MemExt', N'Str_Attr_141', N'2', N'ProductTypeLike_4N', N'MemberExt', NULL, NULL, N'喜欢的商品类型（名称4）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.type_result
from  (
select *,isnull(w.CategoryName, '''') + ''|'' + cast(w.type_count as nvarchar(10)) type_result
            ----(isnull(w.CategoryName, '''') + ''|'' + isnull(w.type_rate, '''')) type_result
from (
select e.*,cast(cast(cast(e.type_count as float) /e.count_sum * 100  as int) as nvarchar(10)) type_rate from 
(
  select d.*,sum(type_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.type_count desc) type_order 
   from (
        select a.memberid ,c.CategoryName,count(*) type_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.ProductCode =  b.GoodsCodeProduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.CategoryName
) c
)d
where d.type_order<=5
)e
)w
 where type_order = 4
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A5390151DA50 AS DateTime), CAST(0x0000A54500DE33CC AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2855, N'TM_Loy_MemExt', N'Str_Attr_142', N'2', N'ProductTypeLike_5N', N'MemberExt', NULL, NULL, N'喜欢的商品类型（名称5）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.type_result
from  (
select *,isnull(w.CategoryName, '''') + ''|'' + cast(w.type_count as nvarchar(10)) type_result
           ----(isnull(w.CategoryName, '''') + ''|'' + isnull(w.type_rate, '''')) type_result
from (
select e.*,cast(cast(cast(e.type_count as float) /e.count_sum * 100  as int) as nvarchar(10)) type_rate from 
(
  select d.*,sum(type_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.type_count desc) type_order 
   from (
        select a.memberid ,c.CategoryName,count(*) type_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.ProductCode =  b.GoodsCodeProduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.CategoryName
) c
)d
where d.type_order<=5
)e
)w
 where type_order = 5
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A5390151DA50 AS DateTime), CAST(0x0000A54500DE47BD AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2856, N'TM_Loy_MemExt', N'Str_Attr_143', N'2', N'BrandLike_4N', N'MemberExt', NULL, NULL, N'喜好品牌（名称4）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.brand_result
from  
(
select *,isnull(w.productbrandnamebase, '''') + ''|'' + cast(w.brand_count as nvarchar(10)) brand_result
           ---(isnull(w.productbrandnamebase, '''') + ''|'' + isnull(w.brand_rate, '''')) brand_result
from (
select e.*,cast(cast(cast(e.brand_count as float) /e.count_sum * 100  as int) as nvarchar(10)) brand_rate from 
(
  select d.*,sum(brand_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.brand_count desc) brand_order 
   from (
        select a.memberid ,c.productbrandnamebase,count(*) brand_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.productcode =  b.goodscodeproduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.productbrandnamebase
) c
)d
where d.brand_order<=5
)e
)w
where brand_order = 4
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A53901524821 AS DateTime), CAST(0x0000A54500DD5260 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2858, N'TM_Loy_MemExt', N'Str_Attr_144', N'2', N'BrandLike_5N', N'MemberExt', NULL, NULL, N'喜好品牌（名称5）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.brand_result
from  
(
select *,isnull(w.productbrandnamebase, '''') + ''|'' + cast(w.brand_count as nvarchar(10)) brand_result
           ----(isnull(w.productbrandnamebase, '''') + ''|'' + isnull(w.brand_rate, '''')) brand_result
from (
select e.*,cast(cast(cast(e.brand_count as float) /e.count_sum * 100  as int) as nvarchar(10)) brand_rate from 
(
  select d.*,sum(brand_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.brand_count desc) brand_order 
   from (
        select a.memberid ,c.productbrandnamebase,count(*) brand_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_product c on c.productcode =  b.goodscodeproduct
        -------增加约束条件  
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
       group by a.memberid,c.productbrandnamebase
) c
)d
where d.brand_order<=5
)e
)w
where brand_order = 5
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A539015249DF AS DateTime), CAST(0x0000A54500DD6ADB AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2860, N'TM_Loy_MemExt', N'Str_Attr_145', N'2', N'ProductColorLike_1N', N'MemberExt', NULL, NULL, N'产品颜色偏好(名称1）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.color_result
from  
(
select *,isnull(w.colorname, '''') + ''|'' + cast(w.color_count as nvarchar(10)) color_result 
           ----(isnull(w.colorname, '''') + ''|'' + isnull(w.color_rate, '''')) color_result
from (
select e.*,cast(cast(cast(e.color_count as float) /e.count_sum * 100  as int) as nvarchar(10)) color_rate from 
(
  select d.*,sum(color_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.color_count desc) color_order 
   from (
        
	  select a.memberid ,m.colorname,count(*) color_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_colors m on b.colorcodeproduct = m.colorcode
		----增加约束条件
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
		group by a.memberid,m.colorname
) c
)d
where d.color_order<=5
)e
)w
where color_order = 1
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A5390152A1A1 AS DateTime), CAST(0x0000A54500E0F8B1 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2862, N'TM_Loy_MemExt', N'Str_Attr_146', N'2', N'ProductColorLike_2N', N'MemberExt', NULL, NULL, N'产品颜色偏好(名称2）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.color_result
from  
(
select *,isnull(w.colorname, '''') + ''|'' + cast(w.color_count as nvarchar(10)) color_result 
    -----(isnull(w.colorname, '''') + ''|'' + isnull(w.color_rate, '''')) color_result
from (
select e.*,cast(cast(cast(e.color_count as float) /e.count_sum * 100  as int) as nvarchar(10)) color_rate from 
(
  select d.*,sum(color_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.color_count desc) color_order 
   from (
        
	  select a.memberid ,m.colorname,count(*) color_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_colors m on b.colorcodeproduct = m.colorcode
		----增加约束条件
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
		group by a.memberid,m.colorname
) c
)d
where d.color_order<=5
)e
)w
where color_order = 2
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A5390152D27E AS DateTime), CAST(0x0000A54500E108B2 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2864, N'TM_Loy_MemExt', N'Str_Attr_147', N'2', N'ProductColorLike_3N', N'MemberExt', NULL, NULL, N'产品颜色偏好(名称3）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.color_result
from  
(
select *,isnull(w.colorname, '''') + ''|'' + cast(w.color_count as nvarchar(10)) color_result 
              ----(isnull(w.colorname, '''') + ''|'' + isnull(w.color_rate, '''')) color_result
from (
select e.*,cast(cast(cast(e.color_count as float) /e.count_sum * 100  as int) as nvarchar(10)) color_rate from 
(
  select d.*,sum(color_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.color_count desc) color_order 
   from (
        
	  select a.memberid ,m.colorname,count(*) color_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_colors m on b.colorcodeproduct = m.colorcode
		----增加约束条件
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
		group by a.memberid,m.colorname
) c
)d
where d.color_order<=5
)e
)w
where color_order = 3
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A5390152D61F AS DateTime), CAST(0x0000A54500E1180A AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2866, N'TM_Loy_MemExt', N'Str_Attr_148', N'2', N'ProductColorLike_4N', N'MemberExt', NULL, NULL, N'产品颜色偏好(名称4）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.color_result
from  
(
select *,isnull(w.colorname, '''') + ''|'' + cast(w.color_count as nvarchar(10)) color_result 
          ----(isnull(w.colorname, '''') + ''|'' + isnull(w.color_rate, '''')) color_result
from (
select e.*,cast(cast(cast(e.color_count as float) /e.count_sum * 100  as int) as nvarchar(10)) color_rate from 
(
  select d.*,sum(color_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.color_count desc) color_order 
   from (
        
	  select a.memberid ,m.colorname,count(*) color_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_colors m on b.colorcodeproduct = m.colorcode
		----增加约束条件
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
		group by a.memberid,m.colorname
) c
)d
where d.color_order<=5
)e
)w
where color_order = 4
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A5390152DAB3 AS DateTime), CAST(0x0000A54500E1267D AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2867, N'TM_Loy_MemExt', N'Str_Attr_149', N'2', N'ProductColorLike_5N', N'MemberExt', NULL, NULL, N'产品颜色偏好(名称5）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.color_result
from  
(
select *,isnull(w.colorname, '''') + ''|'' + cast(w.color_count as nvarchar(10)) color_result 
          -----(isnull(w.colorname, '''') + ''|'' + isnull(w.color_rate, '''')) color_result
from (
select e.*,cast(cast(cast(e.color_count as float) /e.count_sum * 100  as int) as nvarchar(10)) color_rate from 
(
  select d.*,sum(color_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.color_count desc) color_order 
   from (
        
	  select a.memberid ,m.colorname,count(*) color_count
        from  V_M_TM_Mem_Trade_sales a left join V_M_TM_Mem_Tradedetail_sales_product b on a.tradeid= b.tradeid
		inner join V_M_TM_SYS_BaseData_colors m on b.colorcodeproduct = m.colorcode
		----增加约束条件
		where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and  ([Switch]=1 or a.MemberID in ([MemberList]))
		group by a.memberid,m.colorname
) c
)d
where d.color_order<=5
)e
)w
where color_order = 5
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A5390152DB04 AS DateTime), CAST(0x0000A54500E136F3 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2869, N'TM_Loy_MemExt', N'Str_Attr_150', N'2', N'OftenBuyTime_4N', N'MemberExt', NULL, NULL, N'经常购买时间段（名称4）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.day_result
from  
(select *,isnull(w.the_day, '''') + ''|'' + cast(w.day_count as nvarchar(10)) day_result
           ----(isnull(w.the_day, '''') + ''|'' + isnull(w.day_rate, '''')) day_result
from (
select e.*,cast(cast(cast(e.day_count as float) /e.count_sum * 100  as int) as nvarchar(10)) day_rate from 
(
  select d.*,sum(day_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.day_count desc) day_order 
   from (
        
	  select a.memberid ,a.the_day,count(*) day_count
        from (select m.memberid,m.ListDateSales, 
case n.the_day
      when ''星期一'' then ''周一''
	  when ''星期二'' then ''周二''
	  when ''星期三'' then ''周三''
	  when ''星期四'' then ''周四''
	  when ''星期五'' then ''周五''
	  when ''星期六'' then ''周六''
	  when ''星期日'' then ''周日'' 
	  end the_day
from V_M_TM_Mem_Trade_sales m
left join TL_Sys_time n
on convert(nvarchar(8),m.ListDateSales,112)= convert(nvarchar(8),n.the_date,112)
where convert(nvarchar(8),m.ListDateSales,112)<=[DatetimeNow]
and  ([Switch]=1 or m.MemberID in ([MemberList]))
)a
		----增加约束条件
		group by a.memberid,a.the_day
) c
)d
where d.day_order<=5
)e
)w
where day_order = 4
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A53901535AAE AS DateTime), CAST(0x0000A54500E51250 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2871, N'TM_Loy_MemExt', N'Str_Attr_151', N'2', N'OftenBuyTime_5N', N'MemberExt', NULL, NULL, N'经常购买时间段（名称5）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=temp.day_result
from  
(select *,isnull(w.the_day, '''') + ''|'' + cast(w.day_count as nvarchar(10)) day_result
            ----(isnull(w.the_day, '''') + ''|'' + isnull(w.day_rate, '''')) day_result
from (
select e.*,cast(cast(cast(e.day_count as float) /e.count_sum * 100  as int) as nvarchar(10)) day_rate from 
(
  select d.*,sum(day_count) over(partition by d.memberid) count_sum from (
  select c.* ,row_number () over( partition by c.memberid order by c.day_count desc) day_order 
   from (
        
	  select a.memberid ,a.the_day,count(*) day_count
        from (select m.memberid,m.ListDateSales, 
case n.the_day
      when ''星期一'' then ''周一''
	  when ''星期二'' then ''周二''
	  when ''星期三'' then ''周三''
	  when ''星期四'' then ''周四''
	  when ''星期五'' then ''周五''
	  when ''星期六'' then ''周六''
	  when ''星期日'' then ''周日'' 
	  end the_day
from V_M_TM_Mem_Trade_sales m
left join TL_Sys_time n
on convert(nvarchar(8),m.ListDateSales,112)= convert(nvarchar(8),n.the_date,112)
where convert(nvarchar(8),m.ListDateSales,112)<=[DatetimeNow]
and  ([Switch]=1 or m.MemberID in ([MemberList]))
)a
		----增加约束条件
		group by a.memberid,a.the_day
) c
)d
where d.day_order<=5
)e
)w
where day_order = 5
  )temp
 where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A53901535C31 AS DateTime), CAST(0x0000A54500E51EF7 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2873, N'TM_Loy_MemExt', N'Dec_Attr_17', N'7', N'ConsumeIntegral_24', N'MemberExt', NULL, NULL, N'最近两年消耗积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accpoint
from  
(
select  a.memberid ,isnull(b.accpoint,0) accpoint
from tm_mem_master  a
left join (
           select  b.MemberID  , -sum(a.changevalue ) accpoint
           from TL_Mem_AccountChange a,tm_mem_account b
           where a.AccountID=b.AccountID
           and   convert(nvarchar(8),a.AddedDate,112)>=
                 convert(nvarchar(8),dateadd(month,-24,cast([DatetimeNow] as datetime)),112)
		   and   convert(nvarchar(8),a.AddedDate,112)<=[DatetimeNow] 
           and a.changevalue < 0
           and a.HasReverse=0 
           and b.AccountType=''3''
           and a.AccountChangeType<>''loy''
           and ([Switch]=1 or b.MemberID in ([MemberList]))
           group by b.MemberID  )  b on a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))

', NULL, 1, NULL, CAST(0x0000A53A00EDE51E AS DateTime), CAST(0x0000A5450122E5D0 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2874, N'TM_Loy_MemExt', N'Bool_Attr_1', N'4', N'IsBirthMonth', N'MemberExt', NULL, NULL, N'是否当月生日', N'TD_SYS_BizOption', N'TrueOrFalse', 1, 1, 0, 0, 0, N'select', NULL, N'update TM_Loy_MemExt set [Attr] = 0 where [Attr] is null 
update TM_Loy_MemExt set [Attr] = 
case when substring(convert(nvarchar(8),a.Birthday,112),5,2) = substring([DatetimeNow],5,2) then 1 else 0 end
from V_S_TM_Mem_Ext a
where TM_Loy_MemExt.MemberID = a.MemberID 
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList])) ', NULL, 2, NULL, CAST(0x0000A53A013D575C AS DateTime), CAST(0x0000A53A013D575C AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2875, N'TM_Loy_MemExt', N'Dec_Attr_8', N'7', N'EnableIntegrade', N'MemberExt', NULL, NULL, N'会员本年度到期的可用积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'truncate table TE_Mem_LoyFilter
insert into TE_Mem_LoyFilter  
select * from [MemberList] a

exec sp_Loy_WillDeadCreditYear  ''select * from TE_Mem_LoyFilter'',[DatetimeNow],[Switch];


update TM_Loy_MemExt set [Attr] = tmp.WillDeadPoint
from
TE_Mem_WillDeadPoint  tmp
where TM_Loy_MemExt .MemberID = tmp.MemberID and ([Switch]=1 or TM_Loy_MemExt .MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A53F01122101 AS DateTime), CAST(0x0000A53F01322F9E AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2876, N'TM_Mem_Trade', N'Int_Attr_6', N'3', N'TotalAmountTrade_Int', N'MemberTrade', N'sales', NULL, N'积分计算金额（去小数）', NULL, NULL, 0, 1, 1, 1, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A53F0113760A AS DateTime), CAST(0x0000A53F0113760A AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2877, N'TM_Mem_Ext', N'Str_Attr_36', N'2', N'IsWeChatSign', N'MemberExt', NULL, NULL, N'是否微信签到送积分', NULL, NULL, 0, 1, 1, 1, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54000C6393D AS DateTime), CAST(0x0000A54000C6393D AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2878, N'TM_Mem_Trade', N'Str_Attr_9', N'2', N'TradeSource', N'MemberTrade', N'sales', NULL, N'订单来源', N'TD_SYS_BizOption', N'CustomerSource', 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54000F538A6 AS DateTime), CAST(0x0000A54000F538A6 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2879, N'TM_Mem_Ext', N'Str_Attr_37', N'2', N'ProvinceCodeExt', N'MemberExt', NULL, NULL, N'省代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54100CF1957 AS DateTime), CAST(0x0000A54100CF1957 AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2880, N'TM_Mem_Ext', N'Str_Attr_38', N'2', N'CityCodeExt', N'MemberExt', NULL, NULL, N'市代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54100CF28BD AS DateTime), CAST(0x0000A54100CF28BD AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2881, N'TM_Mem_Ext', N'Str_Attr_39', N'2', N'DistrictCodeExt', N'MemberExt', NULL, NULL, N'区代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54100CF35EC AS DateTime), CAST(0x0000A54100CF35EC AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2882, N'TM_Loy_MemExt', N'Int_Attr_2', N'3', N'ConsumptionCounts_24', N'MemberExt', NULL, NULL, N'最近两年的消费次数', NULL, NULL, 0, 1, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select  a.memberid ,isnull(b.accamt,0) accamt 
from tm_mem_master  a
left join (
         select a.MemberID ,count(*) accamt
         from   V_M_TM_Mem_Trade_sales  a
         where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
            and ([Switch]=1 or a.MemberID in ([MemberList]))
            and convert(nvarchar(8),a.ListDateSales,112)>=convert(nvarchar(8),dateadd(month,-24,cast([DatetimeNow] as datetime)),112)  
         group by a.MemberID ) b  on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 1, NULL, CAST(0x0000A54100E3991E AS DateTime), CAST(0x0000A545012306BC AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2883, N'TM_Loy_MemExt', N'Dec_Attr_9', N'7', N'ActConsumption_24', N'MemberExt', NULL, NULL, N'最近两年的消费金额', NULL, NULL, 0, 1, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.accamt
from  
(
select  a.memberid ,isnull(b.accamt,0) accamt 
from tm_mem_master  a
left join (
           select a.MemberID ,sum(Amount) accamt
           from   V_M_TM_Mem_Trade_sales  a
           where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
              and ([Switch]=1 or a.MemberID in ([MemberList]))
              and convert(nvarchar(8),a.ListDateSales,112)>=convert(nvarchar(8),dateadd(month,-24,cast([DatetimeNow] as datetime)),112)  
           group by a.MemberID ) b  on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A54100E3ECED AS DateTime), CAST(0x0000A54501232B78 AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2884, N'TM_Loy_MemExt', N'Dec_Attr_18', N'7', N'AvgMemPrice', N'MemberExt', NULL, NULL, N'平均客单价（两年）', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'
update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.AvgMemPrice
from  
(
select  a.memberid ,isnull(b.AvgMemPrice,0) AvgMemPrice 
from tm_mem_master  a
left join (
            select a.MemberID ,
            case when count(*)<>0 then 
                     round( cast(sum(Amount)  as float )/count(*),2) else 0 end AvgMemPrice
            from   V_M_TM_Mem_Trade_sales  a
            where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
               and ([Switch]=1 or a.MemberID in ([MemberList]))
               and convert(nvarchar(8),a.ListDateSales,112)>=convert(nvarchar(8),dateadd(month,-24,cast([DatetimeNow] as datetime)),112)   
            group by a.MemberID ) b  on  a.memberid=b.memberid 
where ([Switch]=1 or a.MemberID in ([MemberList]))
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))
', NULL, 2, NULL, CAST(0x0000A54100E628B1 AS DateTime), CAST(0x0000A54501234E6C AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2885, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'ProductLine1Name', N'BaseDataType', N'productline1', NULL, N'产品线1名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54401016BC0 AS DateTime), CAST(0x0000A54401016BC0 AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2886, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'ProductLine1Code', N'BaseDataType', N'productline1', NULL, N'产品线1代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54401018336 AS DateTime), CAST(0x0000A54401018336 AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2887, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'ProductLine2Name', N'BaseDataType', N'productline2', NULL, N'产品线2名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54401019A89 AS DateTime), CAST(0x0000A54401019A89 AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2888, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'ProductLine2Code', N'BaseDataType', N'productline2', NULL, N'产品线2代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5440101A86C AS DateTime), CAST(0x0000A5440101A86C AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2889, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'ProductBrandNameBase2', N'BaseDataType', N'productBrand', NULL, N'产品品牌名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5440102A6DF AS DateTime), CAST(0x0000A5440102A6DF AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2890, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'ProductBrandCodeBase2', N'BaseDataType', N'productBrand', NULL, N'产品品牌代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5440102BAEA AS DateTime), CAST(0x0000A5440102BAEA AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2891, N'TM_SYS_BaseData', N'Str_Attr_1', N'2', N'ProductCategoryNameBase2', N'BaseDataType', N'productCategory', NULL, N'产品品类名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5440102D37C AS DateTime), CAST(0x0000A5440102D37C AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2892, N'TM_SYS_BaseData', N'Str_Attr_2', N'2', N'ProductCategoryCodeBase2', N'BaseDataType', N'productCategory', NULL, N'产品品类代码', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A5440102E1EE AS DateTime), CAST(0x0000A5440102E1EE AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2893, N'TM_SYS_BaseData', N'Str_Attr_18', N'2', N'StoreTel', N'BaseDataType', N'store', NULL, N'门店电话', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54401272539 AS DateTime), CAST(0x0000A54401272539 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2894, N'TM_Loy_MemExt', N'Dec_Attr_19', N'7', N'AvgMemQty', N'MemberExt', NULL, NULL, N'平均客单量', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.AvgMemcnt
from  
(
select a.MemberID ,
case when count(*)<>0 then 
         round( cast(sum(QuantitySales)  as float )/count(*),2) else 0 end AvgMemcnt 

from   V_M_TM_Mem_Trade_sales  a
where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
   and ([Switch]=1 or a.MemberID in ([MemberList]))
group by a.MemberID
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A544013C3901 AS DateTime), CAST(0x0000A5440149C62E AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2895, N'TM_Mem_Trade', N'Str_Attr_10', N'2', N'TradeChannelCode', N'MemberTrade', N'sales', NULL, N'消费渠道', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', 1, 1, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A544014C1EB2 AS DateTime), CAST(0x0000A544014C1EB2 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2896, N'TM_Mem_Trade', N'Dec_Attr_9', N'7', N'TradeAmoutSales', N'MemberTrade', N'sales', NULL, N'消费金额', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A54500000000 AS DateTime), CAST(0x0000A54500000000 AS DateTime), 20, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2897, N'TM_Loy_MemExt', N'Dec_Attr_20', N'7', N'AvgMemPriceHis', N'MemberExt', NULL, NULL, N'平均客单价', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.AvgMemPrice
from  
(

            select a.MemberID ,
            case when count(*)<>0 then 
                     round( cast(sum(Amount)  as float )/count(*),2) else 0 end AvgMemPrice
            from   V_M_TM_Mem_Trade_sales  a
            where convert(nvarchar(8),a.ListDateSales,112)<=[DatetimeNow]
               and ([Switch]=1 or a.MemberID in ([MemberList]))
            group by a.MemberID 
) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 2, NULL, CAST(0x0000A54600E912B7 AS DateTime), CAST(0x0000A54600E912B7 AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2898, N'TM_Mem_Ext', N'Date_Attr_5', N'5', N'RegisterDateProtal', N'MemberExt', NULL, NULL, N'门户注册时间', NULL, NULL, 1, 0, 0, 0, 0, N'date', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A55600000000 AS DateTime), CAST(0x0000A55600000000 AS DateTime), 20, NULL, 0, NULL, 0)
GO
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2902, N'TM_Mem_Master', N'MemberLevel2', N'1', N'CustomerLevel2', N'MemberKey', NULL, NULL, N'DPAM会员等级', N'TD_SYS_BizOption', N'CustomerLevel2', 1, 1, 1, 1, 1, N'select', NULL, NULL, N'Insert into [TL_Mem_LevelChange]
(
	MemberID,ChangeLevelFrom,ChangeLevelTo,LevelChangeType,ChangeReason,AddedDate,AddedUser
)
Select {0}.MemberID,V_U_TM_Mem_Info.CustomerLevel2,''{1}'',''loy'',''忠诚度计算'',''{2}'',''1000''
From {0} 
inner join V_U_TM_Mem_Info on {0}.MemberID = V_U_TM_Mem_Info.MemberID', NULL, NULL, CAST(0x0000A56200000000 AS DateTime), CAST(0x0000A56200000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2903, N'TM_SYS_BaseData', N'Str_Attr_19', N'2', N'BrandStore', N'BaseDataType', N'store', NULL, N'门店所属品牌', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A56700000000 AS DateTime), CAST(0x0000A56700000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2904, N'TM_Mem_TradeDetail', N'Str_Attr_5', N'2', N'CRMPayType', N'MemberTradeDetail', N'sales', N'payment', N'CRM结算方式', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A56800000000 AS DateTime), NULL, 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2906, N'TM_Loy_MemExt', N'Dec_Attr_21', N'7', N'HistoryConsumeModify', N'MemberExt', NULL, NULL, N'手工调整消费额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'  ', NULL, 2, NULL, CAST(0x0000A56800000000 AS DateTime), CAST(0x0000A56800000000 AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2908, N'TM_Loy_MemExt', N'Dec_Attr_22', N'7', N'ActConsumptionLoy', N'MemberExt', NULL, NULL, N'累计消费额（忠诚度计算）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'  ', NULL, 2, NULL, CAST(0x0000A56800000000 AS DateTime), CAST(0x0000A56800000000 AS DateTime), 99, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2909, N'TM_SYS_BaseData', N'Bool_Attr_1', N'4', N'ChannelIsEnableBase', N'BaseDataType', N'channel', NULL, N'是否可用', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A56800000000 AS DateTime), NULL, 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2910, N'TM_Mem_Trade', N'Str_Attr_11', N'2', N'StoreBrandTrade', N'MemberTrade', N'sales', NULL, N'门店所属品牌（交易单）', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A56900000000 AS DateTime), CAST(0x0000A56900000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2915, N'TE_Mem_DynamicDimension', N'Dynamic_ConsumeAmount', N'7', N'Dynamic_ConsumeAmount', N'MemberExt', NULL, NULL, N'消费额(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'----------------------------------=============1.消费额==========------------------------------------------------
---20151229 生日改为起止日期
---20151231 增加消费额的限定，修改金额的取值字段；
---20160104 修改生日为月份，传两位字符值

--大区[Parameter1]
--+省+[Parameter2]
--市+[Parameter3]
--区+[Parameter4]
--会员等级[Parameter5]
--+手机号[Parameter6]
--+生日起期[Parameter7]
--+性别[Parameter8]
--+宝宝性别[Parameter9]
--+会员来源[Parameter10]
--+注册渠道[Parameter11]
--+消费时间[Parameter12]  ----消费时间区间段
--+消费时间[Parameter13]
--+消费（线上线下）+[Parameter14]
--消费渠道[Parameter15]
--+消费门店[Parameter16]
--+产品品类[Parameter17]
--+产品品牌[Parameter18]
--+产品线1[Parameter19]
--+产品线2[Parameter20]
--+消费日期（区间型）或  [Parameter21],[Parameter22]
--消费日期（近多少日内）+消费额
--生日止期 [Parameter23]
--4个参数是否限定消费额[Parameter24]





declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,


        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end




   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end


    if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if ([Parameter24]=''1'' and len([Parameter17])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1''  and len([Parameter18])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1''  and len([Parameter19])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1''  and len([Parameter20])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode 
               ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0  
update  TE_Mem_DynamicDimension set  [Attr]=temp.Amount 
from (
select a.MemberID,isnull(b.Amount,0) Amount 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join 
        (select MemberID,sum(AmountProduct)  Amount 
         from V_M_TM_Mem_Trade_sales  a with(nolock)
		 inner join V_M_TM_Mem_TradeDetail_sales_product b   with(nolock) on a.tradeid=b.tradeid
		 inner join V_M_TM_SYS_BaseData_product t with(nolock) on  b.GoodsCodeProduct=t.ProductCode 
         where 1=1 ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID ) b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''

exec(@sql)', NULL, 1, NULL, CAST(0x0000A56B00F8EB58 AS DateTime), CAST(0x0000A583010159FC AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2916, N'TM_Loy_MemExt', N'Dec_Attr_23', N'7', N'HistoryConsumeAmountModify', N'MemberExt', NULL, NULL, N'手动调整后的累计历史消费额', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'update TM_Loy_MemExt set [Attr]=0 where [Attr] is null
update TM_Loy_MemExt set [Attr]=temp.TotalMoneySales
from  
(
          select memberid,isnull(HistoryConsumeAmount,0)+isnull(HistoryConsumeModify,0) TotalMoneySales
          from V_S_TM_Loy_MemExt a
          where ([Switch]=1 or a.MemberID in ([MemberList])) 

) temp
where TM_Loy_MemExt.MemberID=temp.MemberID
and ([Switch]=1 or TM_Loy_MemExt.MemberID in ([MemberList]))', NULL, 1, NULL, CAST(0x0000A56F0112F82D AS DateTime), CAST(0x0000A56F0112F82D AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2917, N'TM_Mem_Trade', N'Str_Attr_1', N'2', N'CouponType', N'MemberTrade', N'coupon', NULL, N'优惠券类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A56900000000 AS DateTime), CAST(0x0000A57000000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2922, N'TM_Mem_Trade', N'Str_Attr_2', N'2', N'ExchangeType', N'MemberTrade', N'coupon', NULL, N'兑换类型', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A57000000000 AS DateTime), CAST(0x0000A56100000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2923, N'TM_Mem_Trade', N'Str_Attr_3', N'2', N'ActName', N'MemberTrade', N'coupon', NULL, N'活动名称', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A57000000000 AS DateTime), CAST(0x0000A57000000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2924, N'TM_Mem_Trade', N'Str_Attr_4', N'2', N'CouponCodeTrade', N'MemberTrade', N'coupon', NULL, N'优惠券号', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A57000000000 AS DateTime), CAST(0x0000A57000000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2926, N'TM_Mem_trade', N'Dec_Attr_1', N'7', N'UseIntByCoupon', N'MemberTrade', N'coupon', NULL, N'使用积分', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A57000000000 AS DateTime), CAST(0x0000A57000000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2927, N'TE_Mem_DynamicDimension', N'Dynamic_MemPrice', N'7', N'Dynamic_MemPrice', N'MemberExt', NULL, NULL, N'客单价(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'-------------------------------------======2.客单价=============--------------------------------------------------
---20151231 修改生日起止日期
---20160104 修改生日传值月份 

declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
		@Sql_Search_kids varchar(max) = '''', ---孩子性别约束
		@Sql_Search_trade varchar(max) = '''', ---
		@Sql_Search_memownarea varchar(max) = '''',
		@Sql_Search_storeownarea varchar(max) = '''',
		@Sql_Search_DateAmt varchar(max) = '''' ,
		--@Sql_Search_memownareaexists1 varchar(max) = '''',
		--@Sql_Search_memownareaexists2 varchar(max) = '''',

		@Sql_Search_tradeexists1 varchar(max) = '''',
		@Sql_Search_tradeexists2 varchar(max) = '''' 

 

 -------------会员所属的区域字段

 if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end


     if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
	  and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
			  where a.memberid=b.memberid 
			  and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
	  set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
	  set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
	  set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
	  set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
	--  set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
	--    exists (select m.TradeID
	--		  from  V_M_TM_Mem_Trade_sales m with(nolock),
	--		        V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
	--				 V_M_TM_SYS_BaseData_store  t with(nolock) 
	--		  where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
	--  set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
	   len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
	  )
 begin
	  set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
	  exists (
              select m.TradeID
			  from  V_M_TM_Mem_Trade_sales m with(nolock),
			        V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
					V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
	   len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
	  )

 begin
	  set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end


 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0  
update  TE_Mem_DynamicDimension set  [Attr]=temp.AvgMemPrice 
from (
select a.MemberID,isnull(b.AvgMemPrice ,0) AvgMemPrice
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join 
        (select MemberID, 
		        case when count(*)<>0 then 
                     round( cast(sum(Amount)  as float )/count(*),2) else 0 end AvgMemPrice
		 from V_M_TM_Mem_Trade_sales  a with(nolock)
		 where 1=1 ''+@Sql_Search_DateAmt+
		 '' 
		 group by MemberID ) b on a.MemberID=b.MemberID
where (
       (1=1 
	   ''+@Sql_Search_memownarea+''
        )  
	   
       )
	   ''+@Sql_Search+''
	   ''+@Sql_Search_kids+''
	   ''+@Sql_Search_tradeexists1+''
	   ''+@Sql_Search_trade+''
	   ''+@Sql_Search_tradeexists2+''
				  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


------print @sql 
exec(@sql)
', NULL, 1, NULL, CAST(0x0000A56B00000000 AS DateTime), CAST(0x0000A57F011F6A9C AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (2928, N'TE_Mem_DynamicDimension', N'Dynamic_lastConsumeAmount', N'7', N'Dynamic_lastConsumeAmount', N'MemberExt', NULL, NULL, N'最近一次消费额(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'-----------------------------------------------====3.最近一次的消费额===-----------------------------------------
---20151231 修改生日起止日期
---20151231 增加消费额的约束参数
---20160104 修改生日传值月份



declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

     if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end



   if ([Parameter24]=''1'' and  len([Parameter17])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and  len([Parameter18])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and  len([Parameter19])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and  len([Parameter20])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0  
update  TE_Mem_DynamicDimension set  [Attr]=temp.Amount 
from (
select a.MemberID,isnull(b.Amount,0)  Amount 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock)  on a.RegisterStoreCode=t.StoreCode
left join
       ( 
        select MemberID, Amount
		from 
		 (select  MemberID, Amount,ROW_NUMBER() over(partition by a.memberid order by a.ListDateSales desc)  serial
         from V_M_TM_Mem_Trade_sales  a with(nolock)
		 inner join V_M_TM_Mem_TradeDetail_sales_product b   with(nolock) on a.tradeid=b.tradeid
		 inner join V_M_TM_SYS_BaseData_product t with(nolock) on  b.GoodsCodeProduct=t.ProductCode 
         where 1=1 ''+@Sql_Search_DateAmt+
         '' 
         ) t
		 where t.serial=1 
	    ) b on a.MemberID=b.MemberID



where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
       
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)
', NULL, 1, NULL, CAST(0x0000A56B00000000 AS DateTime), CAST(0x0000A5810169C79C AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3917, N'TE_Mem_DynamicDimension', N'Dynamic_lastConsumeDate', N'5', N'Dynamic_lastConsumeDate', N'MemberExt', NULL, NULL, N'最近一次消费时间(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'date', NULL, N'-------------------------------=======4.最近一次消费时间=======-----------------------------------
---20151231 修改生日起止日期
---20151231 增加最近一次消费时间的约束参数
---20160104 修改生日传值月份



declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

    if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if ([Parameter24]=''1'' and  len([Parameter17])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and len([Parameter18])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and len([Parameter19])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and len([Parameter20])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end

  


 set @sql=''
---update TE_Mem_DynamicDimension set [Attr]=null   
update  TE_Mem_DynamicDimension set  [Attr]=temp.lastconsumedate  
from (
select a.MemberID,b.lastconsumedate 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock)  on a.RegisterStoreCode=t.StoreCode
left join    
		 (select MemberID,max(a.ListDateSales ) lastconsumedate 
         from V_M_TM_Mem_Trade_sales  a with(nolock)
		 inner join V_M_TM_Mem_TradeDetail_sales_product b   with(nolock) on a.tradeid=b.tradeid
		 inner join V_M_TM_SYS_BaseData_product t with(nolock) on  b.GoodsCodeProduct=t.ProductCode 
         where 1=1 ''+@Sql_Search_DateAmt+ '' 
         group by MemberID ) b on a.MemberID=b.MemberID  
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D28FDE AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3919, N'TE_Mem_DynamicDimension', N'Dynamic_ConsumeCounts', N'3', N'Dynamic_ConsumeCounts', N'MemberExt', NULL, NULL, N'某时间段内的消费次数(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'-----------------------------------============5.某时间段内的消费次数=============------------------------------------；
--20151231 修改生日起止日期
--20151231 消费次数增加参数限定约束
--20160104 修改生日传值月份


declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

     if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if ([Parameter24]=''1'' and len([Parameter17])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and len([Parameter18])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and len([Parameter19])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if ([Parameter24]=''1'' and len([Parameter20])>0)
 begin
     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end

  


 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0
update  TE_Mem_DynamicDimension set  [Attr]=temp.consumecnt  
from (
select a.MemberID,isnull(b.consumecnt ,0) consumecnt
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock)  on a.RegisterStoreCode=t.StoreCode
left join    
	    (select MemberID,count(distinct a.TradeID) consumecnt 
         from V_M_TM_Mem_Trade_sales  a with(nolock)
		 inner join V_M_TM_Mem_TradeDetail_sales_product b   with(nolock) on a.tradeid=b.tradeid
		 inner join V_M_TM_SYS_BaseData_product t with(nolock) on  b.GoodsCodeProduct=t.ProductCode 
         where 1=1 ''+@Sql_Search_DateAmt+ '' 
         group by MemberID ) b on a.MemberID=b.MemberID  
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
       
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


----print @sql 
exec(@sql)
', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D2A2A7 AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3920, N'TE_Mem_DynamicDimension', N'Dynamic_MemCount', N'7', N'Dynamic_MemCount', N'MemberExt', NULL, NULL, N'客单量(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'----------------------------------------------=====6.客单量======---------------------------------------------
--20151231 修改生日的起止日期
--20160104 修改生日传值月份


declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end


     if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0  
update  TE_Mem_DynamicDimension set  [Attr]=temp.AvgMemCnt 
from (
select a.MemberID,isnull(b.AvgMemCnt,0)  AvgMemCnt
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join 
        (select MemberID, 
                case when count(distinct memberid)<>0 then 
                     round( cast(count(*)  as float )/count(distinct memberid),2) else 0 end AvgMemCnt
         from V_M_TM_Mem_Trade_sales  a with(nolock)
         where 1=1 ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID ) b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
       
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


-----print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D2B501 AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3922, N'TE_Mem_DynamicDimension', N'Dynamic_BuyProductCount', N'3', N'Dynamic_BuyProductCount', N'MemberExt', NULL, NULL, N'某时间段内购买商品件数(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'------------------------------=====7.某时间段内购买商品件数======------------------------
---20151231 修改生日的起止日期
---20160104 修改生日传值月份




declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

   if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end



 --  if (len([Parameter24])>0 and len([Parameter17])>0)
 --begin
 --     set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 --end


 --  if (len([Parameter24])>0 and len([Parameter18])>0)
 --begin
 --    set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 --end


 --  if (len([Parameter24])>0 and len([Parameter19])>0)
 --begin
 --    set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 --end


 --  if (len([Parameter24])>0 and len([Parameter20])>0)
 --begin
 --    set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 --end


-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end

 


 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0  
update  TE_Mem_DynamicDimension set  [Attr]=temp.QuantitySales 
from (
select a.MemberID,isnull(b.QuantitySales ,0) QuantitySales
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join 
        (select MemberID,sum(QuantitySales) QuantitySales
         from V_M_TM_Mem_Trade_sales  a with(nolock)
		 --inner join V_M_TM_Mem_TradeDetail_sales_product b   with(nolock) on a.tradeid=b.tradeid
		 --inner join V_M_TM_SYS_BaseData_product t with(nolock) on  b.GoodsCodeProduct=t.ProductCode 
         where 1=1 ''+@Sql_Search_DateAmt+ '' 
         group by MemberID ) b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D2CC7F AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3924, N'TE_Mem_DynamicDimension', N'Dynamic_EnableIntegral', N'7', N'Dynamic_EnableIntegral', N'MemberExt', NULL, NULL, N'当前可用积分(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'------------------------------=====8.当前可用积分======------------------------
--20151231 修改生日的起止日期 ,注意此标签无可用积分时间用户，生日止期参数为[Parameter21]
--20160104 修改生日传值月份

declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
       --- @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter21])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter21]+''''''''
 end


   if (len([Parameter22])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter22],'','',''","'')+''"'',''"'',''''''''))+'')''
 end




   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


--------消费区间取值约束


--   if (len([Parameter21])>0)
-- begin
--      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.addeddate,112)>=''''''+[Parameter21]+''''''''
-- end


-----

--   if (len([Parameter22])>0)
-- begin
--      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.addeddate,112)<=''''''+[Parameter22]+''''''''
-- end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
---update TE_Mem_DynamicDimension set [Attr]=0   
update  TE_Mem_DynamicDimension set  [Attr]=temp.avipoint  
from (
select a.MemberID,isnull(b.avipoint ,0)  avipoint 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock)  on a.RegisterStoreCode=t.StoreCode
left join    
         ( 
             select  a.MemberID  , a.value1  avipoint
             from tm_mem_account a 
             where 1=1 and accounttype=''''3''''
            
         )  b on a.MemberID=b.MemberID


where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
       
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D2E367 AS DateTime), NULL, 22, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3926, N'TE_Mem_DynamicDimension', N'Dynamic_UsedIntegral', N'7', N'Dynamic_UsedIntegral', N'MemberExt', NULL, NULL, N'某时间段内消耗积分(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'------------------------------=====9.某时间段内消耗积分======------------------------
---20151231 修改生日起止日期
---20160104 修改生日传值月份



declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

     if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.addeddate,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.addeddate,112)<=''''''+[Parameter22]+''''''''
 end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
update TE_Mem_DynamicDimension set [Attr]=0   
update  TE_Mem_DynamicDimension set  [Attr]=temp.accpoint  
from (
select a.MemberID,isnull(b.accpoint ,0)  accpoint
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock)  on a.RegisterStoreCode=t.StoreCode
left join    
         ( select  b.MemberID  , -sum(a.changevalue ) accpoint
           from TL_Mem_AccountChange a,tm_mem_account b
           where a.AccountID=b.AccountID
            ''+@Sql_Search_DateAmt+ ''  
           and a.changevalue < 0
           and a.HasReverse=0 
           and b.AccountType=''''3''''
           and a.AccountChangeType<>''''loy''''
           group by b.MemberID
         )  b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
       
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D30957 AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3928, N'TE_Mem_DynamicDimension', N'Dynamic_TradeDetailMaxPrice', N'7', N'Dynamic_TradeDetailMaxPrice', N'MemberExt', NULL, NULL, N'一笔订单单品最大金额（单品金额 动态）', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'------------------------------=====10.一笔订单单品最大金额（单品金额）======------------------------
--20151231 修改生日起止日期
--20160104 修改生日传值月份


declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 





 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

     if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end







-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0 
update  TE_Mem_DynamicDimension set  [Attr]=temp.AmountProduct 
from (
select a.MemberID,isnull(b.AmountProduct,0)  AmountProduct
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join (
   select m.memberid,max(AmountProduct) AmountProduct
     from 
        (select MemberID,a.tradeid,b.goodscodeproduct,sum(b.AmountProduct) AmountProduct 
         from V_M_TM_Mem_Trade_sales  a with(nolock)    
         inner join V_M_TM_Mem_TradeDetail_sales_product b with(nolock) on a.tradeid=b.tradeid   
         where 1=1   ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID,a.tradeid,b.goodscodeproduct)      m  
         group by memberid  
         ) b on a.MemberID=b.MemberID


where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
       
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


----print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D31A7B AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3930, N'TE_Mem_DynamicDimension', N'Dynamic_TradeMaxPrice', N'7', N'Dynamic_TradeMaxPrice', N'MemberExt', NULL, NULL, N'单笔最大消费金额(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'------------------------------=====11.单笔最大消费金额======------------------------
--20151231 修改生日为起止日期
--20160104 修改生日传值月份

declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

     if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
---update TE_Mem_DynamicDimension set [Attr]=0  
update  TE_Mem_DynamicDimension set  [Attr]=temp.MaxAmount 
from (
select a.MemberID,isnull(b.MaxAmount ,0)  MaxAmount
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join 
        (select MemberID, 
                max(Amount) MaxAmount
         from V_M_TM_Mem_Trade_sales  a with(nolock)
         where 1=1 ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID ) b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D32CBC AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3931, N'TE_Mem_DynamicDimension', N'Dynamic_CouponCounts', N'3', N'Dynamic_CouponCounts', N'MemberExt', NULL, NULL, N'某时间段内使用的优惠券次数(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'----------------------------=====12.某时间段内使用的优惠券次数======------------------------
---20151231 修改生日为起止日期
---20160104 修改生日传值月份

declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

     if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0  
update  TE_Mem_DynamicDimension set  [Attr]=temp.CouponCnt 
from (
select a.MemberID,isnull(b.CouponCnt,0) CouponCnt 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join 
        (select MemberID, 
                count(distinct b.PmtCardNoPayment )  CouponCnt
         from V_M_TM_Mem_Trade_sales  a with(nolock)
         inner join   V_M_TM_Mem_TradeDetail_sales_payment b with(nolock)
         where 1=1  and b.PmtCodePayment=''''888''''  ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID ) b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D34127 AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3932, N'TE_Mem_DynamicDimension', N'Dynamic_LikeToStore', N'2', N'Dynamic_LikeToStore', N'MemberExt', NULL, NULL, N'某时间段内最喜欢去的门店(动态)', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', 1, 0, 0, 0, 0, N'select', NULL, N'---------------------------------========13.某时间段内最喜欢去的门店=======------------------------
---20151231 修改生日为起止日期
---20160104 修改生日传值月份


declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 



 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter23])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter23]+''''''''
 end

     if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter24],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end




-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end



 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=null 
update  TE_Mem_DynamicDimension set  [Attr]=temp.StoreCodeSales 
from (
select a.MemberID,b.StoreCodeSales 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join (
   select m.memberid,m.StoreCodeSales
   from (
     select t.MemberID,StoreCodeSales,ordercnt,
	        ROW_NUMBER() over(partition by t.memberid order by t.ordercnt desc)  serial
	 from 
        (select MemberID,StoreCodeSales,count(*) ordercnt 
         from V_M_TM_Mem_Trade_sales  a with(nolock)		 
         where 1=1   ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID,StoreCodeSales ) t 
		  ) m  
	where m.serial=1
		 ) b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


--print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D353BD AS DateTime), NULL, 24, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3933, N'TE_Mem_DynamicDimension', N'Dynamic_LikeToChannel', N'7', N'Dynamic_LikeToChannel', N'MemberExt', NULL, NULL, N'某段时间某消费渠道的排名(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'-------------------------------=========14.某段时间某消费渠道的排名======-------------------------
-------------新增参数 Sql_Search_DateAppointed  Parameter23 
---20151231  修改生日为起止日期，生日止期参数Parameter24
---20160104 修改生日传值月份



declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' ,

         @Sql_Search_DateAppointed varchar(max) = '''' 





 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter24]+''''''''
 end

      if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if (len([Parameter23])>0)
 begin
      set @Sql_Search_DateAppointed=@Sql_Search_DateAppointed+'' and m.TradeChannelCode=''''''+[Parameter23]+''''''''
 end





-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end



 set @sql=''
---update TE_Mem_DynamicDimension set [Attr]=0 
update  TE_Mem_DynamicDimension set  [Attr]=temp.ChannelRank 
from (
select a.MemberID,b.ChannelRank   
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join (
   select m.memberid,m.serial  ChannelRank
   from (
     select t.MemberID,TradeChannelCode,ordercnt,amount,
            ROW_NUMBER() over(partition by t.memberid order by t.amount desc)  serial  ----or ordercnt
     from 
        (select MemberID,TradeChannelCode,count(*) ordercnt ,sum(amount) amount
         from V_M_TM_Mem_Trade_sales  a with(nolock)         
         where 1=1   ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID,TradeChannelCode )    t      
          ) m  
    where 1=1       ''+@Sql_Search_DateAppointed+'' 
         ) b on a.MemberID=b.MemberID
where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57700D3A822 AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3937, N'TE_Mem_DynamicDimension', N'Dynamic_LikeToBrand', N'7', N'Dynamic_LikeToBrand', N'MemberExt', NULL, NULL, N'某时间段某品类的排名(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'----------------------------------------=============15.某时间段某品类的排名==============---------------------------------
---20151231  修改生日为起止日期，生日止期参数Parameter24
---20160104 修改生日传值月份

declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' ,

		 @Sql_Search_DateAppointed varchar(max) = '''' 





 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter24]+''''''''
 end

      if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if (len([Parameter23])>0)
 begin
      set @Sql_Search_DateAppointed=@Sql_Search_DateAppointed+'' and m.CategoryCode=''''''+[Parameter23]+''''''''
 end





-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0 
update  TE_Mem_DynamicDimension set  [Attr]=temp.CategoryRank  
from (
select a.MemberID,b.CategoryRank  
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join (
   select m.memberid,m.serial  CategoryRank 
   from (
     select t.MemberID,TradeChannelCode,ordercnt,amount,
	        ROW_NUMBER() over(partition by t.memberid order by t.ordercnt desc)  serial 
	 from 
        (select MemberID,CategoryCode,count(distinct a.tradeid) ordercnt 
         from V_M_TM_Mem_Trade_sales  a with(nolock)	
		 inner join V_M_TM_Mem_TradeDetail_sales_product b with(nolock) on a.tradeid=b.tradeid  
		 inner join  V_M_TM_SYS_BaseData_product c with(nolock) on b.goodscodeproduct=c.productcode
         where 1=1   ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID,CategoryCode ) 	t	 
		  ) m  
	where 1=1       ''+@Sql_Search_DateAppointed+'' 
		 ) b on a.MemberID=b.MemberID


where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


--print @sql 
exec(@sql)', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57200000000 AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3938, N'TE_Mem_DynamicDimension', N'Dynamic_LikeToPromotion', N'7', N'Dynamic_LikeToPromotion', N'MemberExt', NULL, NULL, N'某时间段内某促销的排名(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'--------------------------------------------===========16.某时间段内某促销的排名========--------------------------------
 ---20151231  修改生日为起止日期，生日止期参数Parameter24
----20160104 修改生日传值月份


declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
       -- @Sql_Search_memownareaexists1 varchar(max) = '''',
       -- @Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' ,

		 @Sql_Search_DateAppointed varchar(max) = '''' 





 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter24]+''''''''
 end


       if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if (len([Parameter23])>0)
 begin
      set @Sql_Search_DateAppointed=@Sql_Search_DateAppointed+'' and m.PromotionType=''''''+[Parameter23]+''''''''
 end





-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
---update TE_Mem_DynamicDimension set [Attr]=0 
update  TE_Mem_DynamicDimension set  [Attr]=temp.PromotionTypeRank 
from (
select a.MemberID,b.PromotionTypeRank 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join (
   select m.memberid,m.serial  PromotionTypeRank
   from (
     select t.MemberID,promotionname,ordercnt,
	        ROW_NUMBER() over(partition by t.memberid order by t.ordercnt desc)  serial 
	 from 
        (select MemberID,CategoryCode,count(distinct a.tradeid) ordercnt 
         from V_M_TM_Mem_Trade_sales  a with(nolock)	
		 inner join TM_POS_tradedetailPromotion b with(nolock) on a.tradeid=b.tradeid 
		 inner join  V_M_TM_SYS_BaseData_promotion c with(nolock) on b.promotionid=c.promotionid 
         where 1=1   ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID,promotionname ) 		 t
		  ) m  
	where 1=1       ''+@Sql_Search_DateAppointed+'' 
		 ) b on a.MemberID=b.MemberID



where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


----print @sql 
exec(@sql)
', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57200000000 AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3939, N'TE_Mem_DynamicDimension', N'Dynamic_TradeDetailOnceMaxPrice', N'7', N'Dynamic_TradeDetailOnceMaxPrice', N'MemberExt', NULL, NULL, N'某时间段内一次性购买某品牌产品的最大金额(动态)', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'
-----------------------------------==========17.某时间段内一次性购买某品牌产品的最大金额=====------------
 ---20151231  修改生日为起止日期，生日止期参数Parameter24
 --20160104  修改生日传值月份

declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
        --@Sql_Search_memownareaexists1 varchar(max) = '''',
        --@Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' ,

		 @Sql_Search_DateAppointed varchar(max) = '''' 





 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter24])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter24]+''''''''
 end

      if (len([Parameter25])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter25],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if (len([Parameter23])>0)
 begin
      set @Sql_Search_DateAppointed=@Sql_Search_DateAppointed+'' and m.productbrandCode=''''''+[Parameter23]+''''''''
 end





-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
---update TE_Mem_DynamicDimension set [Attr]=0 
update  TE_Mem_DynamicDimension set  [Attr]=temp.AmountProduct 
from (
select a.MemberID,isnull(b.AmountProduct,0)  AmountProduct 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join (
     select MemberID,max(AmountProduct) AmountProduct
	 from 
	     ------同笔订单会存在同一品牌的多个产品明细，要根据品牌和订单进行汇总
        (select a.MemberID,a.Tradeid,c.productbrandCode,sum(b.AmountProduct) AmountProduct
         from V_M_TM_Mem_Trade_sales  a with(nolock)	
		 inner join V_M_TM_Mem_Tradedetail_sales_product b with(nolock) on a.tradeid=b.tradeid 
		 inner join  V_M_TM_SYS_BaseData_product c with(nolock) on b.goodscodeproduct=c.productcode 
         where 1=1   ''+@Sql_Search_DateAmt+
         '' 
         group by a.MemberID,a.Tradeid,c.productbrandCode 
		 ) m
	  where 1=1       ''+@Sql_Search_DateAppointed+''  
	  group by memberid	 
		  ) b on a.MemberID=b.MemberID


where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
       
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


--print @sql 
exec(@sql)
', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57200000000 AS DateTime), NULL, 25, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3940, N'TE_Mem_DynamicDimension', N'Dynamic_LikeToAllType', N'7', N'Dynamic_LikeToAllType', N'MemberExt', NULL, NULL, N'某时间段内购买某品类某品牌某产品的次数', NULL, NULL, 1, 0, 0, 0, 0, N'input', NULL, N'----------------------======18.某时间段内购买某品类某品牌某产品的次数========---------------
---20151231  修改生日为起止日期，生日止期参数Parameter26
---20160104  修改生日的传值月份Parameter27


declare @Sql varchar(max),
        @Sql_Search varchar(max) = '''',
        @Sql_Search_kids varchar(max) = '''', ---孩子性别约束
        @Sql_Search_trade varchar(max) = '''', ---
        @Sql_Search_memownarea varchar(max) = '''',
        @Sql_Search_storeownarea varchar(max) = '''',
        @Sql_Search_DateAmt varchar(max) = '''' ,
       -- @Sql_Search_memownareaexists1 varchar(max) = '''',
       -- @Sql_Search_memownareaexists2 varchar(max) = '''',

        @Sql_Search_tradeexists1 varchar(max) = '''',
        @Sql_Search_tradeexists2 varchar(max) = '''' 





 -------------会员所属的区域字段

   if (len([Parameter1])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.AreaCodeStore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.areacodestore  in (''+(select replace( ''"''+replace([Parameter1],'','',''","'')+''"'',''"'',''''''''))+'')''

 end


   if (len([Parameter2])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.ProvinceCodeStore  in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.ProvinceCodeStore in (''+(select replace( ''"''+replace([Parameter2],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter3])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  t.CityCodeStore  in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
	 set @Sql_Search_trade=@Sql_Search_trade+'' and  x.CityCodeStore   in (''+(select replace( ''"''+replace([Parameter3],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter4])>0)
 begin
     set @Sql_Search_memownarea=@Sql_Search_memownarea+'' and  a.DistrictCodeExt  in (''+(select replace( ''"''+replace([Parameter4],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ---------基本查询约束 

   if (len([Parameter5])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerLevel  in (''+(select replace( ''"''+replace([Parameter5],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter6])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.customermobile  like ''''''+(select replace([Parameter6],'''''''',''''))+''%''''''
 end

   if (len([Parameter7])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) >=''''''+[Parameter7]+''''''''
 end


   if (len([Parameter26])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   convert(nvarchar(8),a.Birthday,112) <=''''''+[Parameter26]+''''''''
 end


   if (len([Parameter27])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and   substring(convert(nvarchar(8),a.Birthday,112),5,2) in   (''+(select replace( ''"''+replace([Parameter27],'','',''","'')+''"'',''"'',''''''''))+'')''
 end




   if (len([Parameter8])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.Gender  in (''+(select replace( ''"''+replace([Parameter8],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


 ----小孩性别约束

   if (len([Parameter9])>0)
 begin
     set @Sql_Search_kids=@Sql_Search_kids+'' 
      and exists (select memberid 
              from V_M_TM_Mem_SubExt_kid b with(nolock) 
              where a.memberid=b.memberid 
              and b.BabyGender in (''+(select replace( ''"''+replace([Parameter9],'','',''","'')+''"'',''"'',''''''''))+''))''
 end


 ----基本查询约束 
   if (len([Parameter10])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.CustomerSource  in (''+(select replace( ''"''+replace([Parameter10],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter11])>0)
 begin
     set @Sql_Search=@Sql_Search+'' and  a.ChannelCodeMember  in (''+(select replace( ''"''+replace([Parameter11],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


-----区间内消费满足的查询约束

   if (len([Parameter12])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)>=''''''+[Parameter12]+''''''''
 end


---

   if (len([Parameter13])>0)
 begin
      set @Sql_Search_trade=@Sql_Search_trade+'' and convert(nvarchar(8),m.ListDateSales,112)<=''''''+[Parameter13]+''''''''
 end



   if (len([Parameter14])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeSource   in (''+(select replace( ''"''+replace([Parameter14],'','',''","'')+''"'',''"'',''''''''))+'')''
 end



   if (len([Parameter15])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.TradeChannelCode   in (''+(select replace( ''"''+replace([Parameter15],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter16])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  m.StoreCodeSales   in (''+(select replace( ''"''+replace([Parameter16],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter17])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.CategoryCode   in (''+(select replace( ''"''+replace([Parameter17],'','',''","'')+''"'',''"'',''''''''))+'')''
 end

   if (len([Parameter18])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductBrandCode   in (''+(select replace( ''"''+replace([Parameter18],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter19])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode1   in (''+(select replace( ''"''+replace([Parameter19],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


   if (len([Parameter20])>0)
 begin
     set @Sql_Search_trade=@Sql_Search_trade+'' and  t.ProductLineCode2   in (''+(select replace( ''"''+replace([Parameter20],'','',''","'')+''"'',''"'',''''''''))+'')''
 end


------消费区间取值约束


   if (len([Parameter21])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)>=''''''+[Parameter21]+''''''''
 end


---

   if (len([Parameter22])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and convert(nvarchar(8),a.ListDateSales,112)<=''''''+[Parameter22]+''''''''
 end


   if (len([Parameter23])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and c.CategoryCode=''''''+[Parameter23]+''''''''
 end


   if (len([Parameter24])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and c.productbrandCode=''''''+[Parameter24]+''''''''
 end

   if (len([Parameter25])>0)
 begin
      set @Sql_Search_DateAmt=@Sql_Search_DateAmt+'' and c.productcode=''''''+[Parameter25]+''''''''
 end





-----消费门店所属区域等参数存在判断

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists1=@Sql_Search_memownareaexists1+'' and 
 --       exists (select m.TradeID
 --             from  V_M_TM_Mem_Trade_sales m with(nolock),
 --                   V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
 --                    V_M_TM_SYS_BaseData_store  t with(nolock) 
 --             where  a.memberid=m.MemberID and m.tradeid=n.tradeid  and  m.StoreCodeSales=t.StoreCode ''
 --end

 --  if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0)
 --begin
 --     set @Sql_Search_memownareaexists2=@Sql_Search_memownareaexists2+'')''
 --end


------区间内消费内容等存在判断

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )
 begin
      set @Sql_Search_tradeexists1=@Sql_Search_tradeexists1+''  and 
      exists (
              select m.TradeID
              from  V_M_TM_Mem_Trade_sales m with(nolock),
                    V_M_TM_Mem_TradeDetail_sales_product n with(nolock),
                    V_M_TM_SYS_BaseData_product t with(nolock),
					 V_M_TM_SYS_BaseData_store  x with(nolock) 
              where  a.memberid=m.MemberID and m.tradeid=n.tradeid and n.GoodsCodeProduct=t.ProductCode 
			         and  m.StoreCodeSales=x.StoreCode  ''
 end

   if (len([Parameter1])>0 or len([Parameter2])>0 or len([Parameter3])>0 or
       len([Parameter12])>0 or len([Parameter13])>0 or len([Parameter14])>0 or
       len([Parameter15])>0 or len([Parameter16])>0 or len([Parameter17])>0 or
       len([Parameter18])>0 or len([Parameter19])>0 or len([Parameter20])>0  
      )

 begin
      set @Sql_Search_tradeexists2=@Sql_Search_tradeexists2+'')''
 end




 set @sql=''
--update TE_Mem_DynamicDimension set [Attr]=0 
update  TE_Mem_DynamicDimension set  [Attr]=temp.CategoryRank 
from (
select a.MemberID,b.CategoryRank 
from V_U_TM_Mem_Info  a with(nolock)
inner join V_M_TM_SYS_BaseData_store  t with(nolock) on a.RegisterStoreCode=t.StoreCode
left join (
     select t.MemberID,ordercnt 
     from 
        (select MemberID,c.CategoryCode, c.productbrandCode,c.productcode,count(distinct a.tradeid) ordercnt 
         from V_M_TM_Mem_Trade_sales  a with(nolock)    
         inner join V_M_TM_Mem_TradeDetail_sales_product b with(nolock) on a.tradeid=b.tradeid 
		 inner join  V_M_TM_SYS_BaseData_product c with(nolock) on b.goodscodeproduct=c.productcode  
         where 1=1   ''+@Sql_Search_DateAmt+
         '' 
         group by MemberID,c.CategoryCode, c.productbrandCode,c.productcode ) t
		 )         b on a.MemberID=b.MemberID

where (
       (1=1 
       ''+@Sql_Search_memownarea+''
        )  
        
       )
       ''+@Sql_Search+''
       ''+@Sql_Search_kids+''
       ''+@Sql_Search_tradeexists1+''
       ''+@Sql_Search_trade+''
       ''+@Sql_Search_tradeexists2+''
                  )   temp
where TE_Mem_DynamicDimension.MemberID=temp.MemberID  ''


---print @sql 
exec(@sql)




', NULL, 1, NULL, CAST(0x0000A57200000000 AS DateTime), CAST(0x0000A57200000000 AS DateTime), NULL, 27, 0, NULL, 1)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3941, N'TM_Mem_Ext', N'Str_Attr_40', N'2', N'IsMemberProtal', N'MemberExt', NULL, NULL, N'是否门户注册', N'TD_SYS_BizOption', N'TrueOrFalse', 1, 0, 0, 0, 0, N'select', NULL, NULL, NULL, NULL, NULL, CAST(0x0000A58200000000 AS DateTime), CAST(0x0000A58200000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3942, N'TM_Loy_MemExt', N'Dec_Attr_31', N'7', N'Backup1', N'MemberExt', NULL, NULL, N'预留数值型1', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3943, N'TM_Loy_MemExt', N'Dec_Attr_32', N'7', N'Backup2', N'MemberExt', NULL, NULL, N'预留数值型2', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3944, N'TM_Loy_MemExt', N'Dec_Attr_33', N'7', N'Backup3', N'MemberExt', NULL, NULL, N'预留数值型3', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3945, N'TM_Loy_MemExt', N'Dec_Attr_34', N'7', N'Backup4', N'MemberExt', NULL, NULL, N'预留数值型4', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3946, N'TM_Loy_MemExt', N'Dec_Attr_35', N'7', N'Backup5', N'MemberExt', NULL, NULL, N'预留数值型5', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3947, N'TM_Loy_MemExt', N'Dec_Attr_36', N'7', N'Backup6', N'MemberExt', NULL, NULL, N'预留数值型6', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3948, N'TM_Loy_MemExt', N'Dec_Attr_37', N'7', N'Backup7', N'MemberExt', NULL, NULL, N'预留数值型7', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3949, N'TM_Loy_MemExt', N'Dec_Attr_38', N'7', N'Backup8', N'MemberExt', NULL, NULL, N'预留数值型8', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3950, N'TM_Loy_MemExt', N'Dec_Attr_39', N'7', N'Backup9', N'MemberExt', NULL, NULL, N'预留数值型9', NULL, NULL, 0, 0, 0, 0, 0, N'Input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3963, N'TM_Loy_MemExt', N'Str_Attr_152', N'2', N'backup10', N'MemberExt', NULL, NULL, N'预留字符串1', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3964, N'TM_Loy_MemExt', N'Str_Attr_153', N'2', N'backup11', N'MemberExt', NULL, NULL, N'预留字符串2', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3965, N'TM_Loy_MemExt', N'Str_Attr_154', N'2', N'backup12', N'MemberExt', NULL, NULL, N'预留字符串3', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3966, N'TM_Loy_MemExt', N'Str_Attr_155', N'2', N'backup13', N'MemberExt', NULL, NULL, N'预留字符串4', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3967, N'TM_Loy_MemExt', N'Int_Attr_10', N'3', N'backup14', N'MemberExt', NULL, NULL, N'预留整型1', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3968, N'TM_Loy_MemExt', N'Int_Attr_11', N'3', N'backup15', N'MemberExt', NULL, NULL, N'预留整型2', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3969, N'TM_Loy_MemExt', N'Int_Attr_12', N'3', N'backup16', N'MemberExt', NULL, NULL, N'预留整型3', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3970, N'TM_Loy_MemExt', N'Int_Attr_13', N'3', N'backup17', N'MemberExt', NULL, NULL, N'预留整型4', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
INSERT [dbo].[TD_SYS_FieldAlias] ([AliasID], [TableName], [FieldName], [FieldType], [FieldAlias], [AliasType], [AliasKey], [AliasSubKey], [FieldDesc], [DictTableName], [DictTableType], [IsFilterBySubdivision], [IsFilterByLoyRule], [IsFilterByLoyActionLeft], [IsFilterByLoyActionRight], [IsCommunicationTemplet], [ControlType], [Reg], [ComputeScript], [LogScript], [RunType], [DataLimitType], [AddedDate], [ModifiedDate], [ComputeSort], [ParameterCount], [IsFilterByCouponTemplet], [ErrorTip], [IsDynamicAlias]) VALUES (3971, N'TM_Loy_MemExt', N'Int_Attr_14', N'3', N'backup18', N'MemberExt', NULL, NULL, N'预留整型5', NULL, NULL, 0, 0, 0, 0, 0, N'input', NULL, N'select 1', NULL, 1, NULL, CAST(0x0000A58D00000000 AS DateTime), CAST(0x0000A58D00000000 AS DateTime), 20, NULL, 0, NULL, 0)
SET IDENTITY_INSERT [dbo].[TD_SYS_FieldAlias] OFF
SET IDENTITY_INSERT [dbo].[TD_SYS_FieldAliasParameter] ON 

INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1827, 2927, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1828, 2927, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1829, 2927, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1830, 2927, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1831, 2927, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1832, 2927, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1833, 2927, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1834, 2927, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1835, 2927, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1836, 2927, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1837, 2927, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1838, 2927, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1839, 2927, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1840, 2927, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1841, 2927, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1842, 2927, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1843, 2927, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1844, 2927, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1845, 2927, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1846, 2927, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1847, 2927, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1848, 2927, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1849, 2927, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1875, 3917, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1876, 3917, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1877, 3917, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1878, 3917, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1879, 3917, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1880, 3917, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1881, 3917, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1882, 3917, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1883, 3917, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1884, 3917, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1885, 3917, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1886, 3917, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1887, 3917, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1888, 3917, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1889, 3917, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1890, 3917, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1891, 3917, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1892, 3917, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1893, 3917, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1894, 3917, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1895, 3917, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1896, 3917, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1897, 3917, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1898, 3917, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'TrueOrFalse', N'消费额是否计算标签', 24, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1899, 3919, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1900, 3919, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1901, 3919, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1902, 3919, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1903, 3919, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1904, 3919, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1905, 3919, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1906, 3919, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1907, 3919, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1908, 3919, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1909, 3919, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1910, 3919, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1911, 3919, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1912, 3919, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1913, 3919, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1914, 3919, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1915, 3919, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1916, 3919, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1917, 3919, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1918, 3919, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1919, 3919, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1920, 3919, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1921, 3919, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1922, 3919, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'TrueOrFalse', N'消费额是否计算标签', 24, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1923, 3920, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1924, 3920, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1925, 3920, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1926, 3920, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1927, 3920, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1928, 3920, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1929, 3920, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1930, 3920, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1931, 3920, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1932, 3920, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1933, 3920, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1934, 3920, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1935, 3920, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1936, 3920, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1937, 3920, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1938, 3920, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1939, 3920, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1940, 3920, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1941, 3920, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1942, 3920, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1943, 3920, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1944, 3920, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1945, 3920, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1947, 3922, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1948, 3922, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1949, 3922, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1950, 3922, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1951, 3922, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
GO
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1952, 3922, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1953, 3922, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1954, 3922, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1955, 3922, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1956, 3922, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1957, 3922, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1958, 3922, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1959, 3922, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1960, 3922, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1961, 3922, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1962, 3922, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1963, 3922, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1964, 3922, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1965, 3922, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1966, 3922, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1967, 3922, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1968, 3922, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1969, 3922, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1971, 3924, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1972, 3924, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1973, 3924, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1974, 3924, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1975, 3924, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1976, 3924, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1977, 3924, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1978, 3924, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1979, 3924, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1980, 3924, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1981, 3924, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1982, 3924, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1983, 3924, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1984, 3924, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1985, 3924, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1986, 3924, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1987, 3924, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1988, 3924, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1989, 3924, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1990, 3924, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1993, 3924, 21, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1995, 3926, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1996, 3926, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1997, 3926, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1998, 3926, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (1999, 3926, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2000, 3926, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2001, 3926, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2002, 3926, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2003, 3926, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2004, 3926, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2005, 3926, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2006, 3926, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2007, 3926, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2008, 3926, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2009, 3926, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2010, 3926, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2011, 3926, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2012, 3926, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2013, 3926, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2014, 3926, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2015, 3926, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2016, 3926, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2017, 3926, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2019, 3928, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2020, 3928, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2021, 3928, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2022, 3928, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2023, 3928, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2024, 3928, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2025, 3928, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2026, 3928, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2027, 3928, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2028, 3928, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2029, 3928, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2030, 3928, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2031, 3928, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2032, 3928, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2033, 3928, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2034, 3928, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2035, 3928, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2036, 3928, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2037, 3928, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2038, 3928, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2039, 3928, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2040, 3928, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2041, 3928, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2043, 3930, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2044, 3930, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2045, 3930, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2046, 3930, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2047, 3930, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2048, 3930, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2049, 3930, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2050, 3930, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2051, 3930, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2052, 3930, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2053, 3930, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2054, 3930, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2055, 3930, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2056, 3930, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2057, 3930, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
GO
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2058, 3930, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2059, 3930, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2060, 3930, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2061, 3930, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2062, 3930, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2063, 3930, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2064, 3930, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2065, 3930, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2067, 3931, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2068, 3931, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2069, 3931, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2070, 3931, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2071, 3931, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2072, 3931, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2073, 3931, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2074, 3931, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2075, 3931, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2076, 3931, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2077, 3931, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2078, 3931, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2079, 3931, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2080, 3931, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2081, 3931, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2082, 3931, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2083, 3931, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2084, 3931, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2085, 3931, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2086, 3931, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2087, 3931, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2088, 3931, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2089, 3931, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2091, 3932, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2092, 3932, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2093, 3932, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2094, 3932, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2095, 3932, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2096, 3932, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2097, 3932, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2098, 3932, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2099, 3932, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2100, 3932, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2101, 3932, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2102, 3932, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2103, 3932, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2104, 3932, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2105, 3932, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2106, 3932, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2107, 3932, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2108, 3932, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2109, 3932, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2110, 3932, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2111, 3932, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2112, 3932, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2113, 3932, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2115, 3933, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2116, 3933, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2117, 3933, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2118, 3933, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2119, 3933, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2120, 3933, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2121, 3933, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2122, 3933, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2123, 3933, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2124, 3933, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2125, 3933, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2126, 3933, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2127, 3933, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2128, 3933, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2129, 3933, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2130, 3933, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2131, 3933, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2132, 3933, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2133, 3933, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2134, 3933, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2135, 3933, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2136, 3933, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2137, 3933, 23, N' ', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'交易单渠道(左值相关）', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2139, 3937, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2140, 3937, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2141, 3937, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2142, 3937, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2143, 3937, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2144, 3937, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2145, 3937, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2146, 3937, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2147, 3937, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2148, 3937, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2149, 3937, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2150, 3937, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2151, 3937, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2152, 3937, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2153, 3937, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2154, 3937, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2155, 3937, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2156, 3937, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2157, 3937, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2158, 3937, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2159, 3937, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2160, 3937, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2161, 3937, 23, N' ', N'2', N'select', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类(左值相关）', 24, 0, NULL, NULL)
GO
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2163, 3938, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2164, 3938, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2165, 3938, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2166, 3938, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2167, 3938, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2168, 3938, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2169, 3938, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2170, 3938, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2171, 3938, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2172, 3938, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2173, 3938, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2174, 3938, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2175, 3938, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2176, 3938, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2177, 3938, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2178, 3938, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2179, 3938, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2180, 3938, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2181, 3938, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2182, 3938, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2183, 3938, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2184, 3938, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2185, 3938, 23, N' ', N'2', N'select', N'V_M_TM_SYS_BaseData_promotion', N'PromotionCode,PromotionName', N'促销(左值相关）', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2187, 3939, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2188, 3939, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2189, 3939, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2190, 3939, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2191, 3939, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2192, 3939, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2193, 3939, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2194, 3939, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2195, 3939, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2196, 3939, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2197, 3939, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2198, 3939, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2199, 3939, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2200, 3939, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2201, 3939, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2202, 3939, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2203, 3939, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2204, 3939, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2205, 3939, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2206, 3939, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2207, 3939, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2208, 3939, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2209, 3939, 24, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2211, 3940, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2212, 3940, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2213, 3940, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2214, 3940, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2215, 3940, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2216, 3940, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2217, 3940, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2218, 3940, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2219, 3940, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2220, 3940, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2221, 3940, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2222, 3940, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2223, 3940, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2224, 3940, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2225, 3940, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2226, 3940, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2227, 3940, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2228, 3940, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2229, 3940, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2230, 3940, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2231, 3940, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2232, 3940, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2233, 3940, 23, N' ', N'2', N'select', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类(左值相关）', 24, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2235, 2928, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2236, 2928, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2237, 2928, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2238, 2928, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2239, 2928, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2240, 2928, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2241, 2928, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2242, 2928, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2243, 2928, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2244, 2928, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2245, 2928, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2246, 2928, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2247, 2928, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2248, 2928, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2249, 2928, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2250, 2928, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2251, 2928, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2252, 2928, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2253, 2928, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2254, 2928, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2255, 2928, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2256, 2928, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2257, 2928, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2258, 2928, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'TrueOrFalse', N'消费额是否计算标签', 24, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2283, 2915, 1, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_area', N'AreaCodeBase,AreaNameBase', N'大区（交易）', 1, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2284, 2915, 2, N'', N'2', N'select', N'TD_SYS_Province', N'RegionID,NameZH', N'省（交易）', 2, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2285, 2915, 3, N'', N'2', N'select', N'TD_SYS_City', N'RegionID,NameZH', N'市（交易）', 3, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2286, 2915, 4, N'', N'2', N'select', N'TD_SYS_District', N'RegionID,NameZH', N'区（交易）', 4, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2287, 2915, 5, N'', N'2', N'mutisearch', N'TD_SYS_BizOption', N'Customerlevel', N'会员等级（会员）', 5, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2288, 2915, 6, N'', N'2', N'input', NULL, NULL, N'会员手机号（会员）', 6, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2289, 2915, 7, N'', N'6', N'date', NULL, NULL, N'生日起始（会员）', 7, 0, NULL, NULL)
GO
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2290, 2915, 8, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'性别（会员）', 9, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2291, 2915, 9, N'', N'2', N'select', N'TD_SYS_BizOption', N'SexType', N'宝宝性别（会员）', 10, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2292, 2915, 10, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'会员来源（会员）', 11, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2293, 2915, 11, N'', N'2', N'select', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'渠道（会员）', 12, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2294, 2915, 12, N'', N'6', N'date', NULL, NULL, N'起始日期（订单）', 13, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2295, 2915, 13, N'', N'6', N'date', NULL, NULL, N'结束日期（订单）', 14, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2296, 2915, 14, N'', N'2', N'select', N'TD_SYS_BizOption', N'CustomerSource', N'订单来源（订单）', 15, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2297, 2915, 15, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_channel', N'ChannelCodeBase,ChannelNameBase', N'订单渠道（订单）', 16, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2298, 2915, 16, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_store', N'StoreCode,StoreName', N'门店（订单）', 17, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2299, 2915, 17, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productCategory', N'ProductCategoryCodeBase2,ProductCategoryNameBase2', N'产品品类（订单）', 18, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2300, 2915, 18, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌（订单）', 19, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2301, 2915, 19, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline1', N'ProductLine1Code,ProductLine1Name', N'产品线1（订单）', 20, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2302, 2915, 20, N'', N'2', N'mutisearch', N'V_M_TM_SYS_BaseData_productline2', N'ProductLine2Code,ProductLine2Name', N'产品线2（订单）', 21, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2303, 2915, 21, N'', N'5', N'date', NULL, NULL, N'起始日期(左值相关）', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2304, 2915, 22, N'', N'5', N'date', NULL, NULL, N'结束日期(左值相关）', 23, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2305, 2915, 23, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2306, 2915, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'TrueOrFalse', N'消费额是否计算标签', 24, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2310, 2915, 25, N' ', N'2', N'mutisearch', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2311, 3940, 24, N' ', N'2', N'select', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌(左值相关）', 25, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2312, 3940, 25, N' ', N'2', N'select', N'V_M_TM_SYS_BaseData_product', N'ProductCode,ProductName', N'产品名(左值相关）', 26, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2313, 3940, 26, N' ', N'5', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2314, 3939, 23, N' ', N'2', N'select', N'V_M_TM_SYS_BaseData_productBrand', N'ProductBrandCodeBase2,ProductBrandNameBase2', N'产品品牌(左值相关）', 24, 1, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2315, 3940, 27, N' ', N'2', N'mutisearch', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 27, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2316, 3939, 25, N' ', N'2', N'mutisearch', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2318, 3938, 24, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2319, 3937, 24, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2320, 3933, 24, N' ', N'6', N'date', NULL, NULL, N'生日截止（会员）', 8, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2322, 3938, 25, N' ', N'2', N'mutisearch', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2323, 3937, 25, N' ', N'2', N'mutisearch', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2324, 3933, 25, N' ', N'2', N'mutisearch', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2325, 3932, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2326, 3931, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2327, 3930, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2328, 3928, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2329, 3926, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2330, 3924, 22, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 22, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2331, 3922, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2332, 3920, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2333, 3919, 25, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2334, 3917, 25, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2335, 2928, 25, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 25, 0, NULL, NULL)
INSERT [dbo].[TD_SYS_FieldAliasParameter] ([ParaID], [AliasID], [ParaIndex], [Reg], [FieldType], [ControlType], [DictTableName], [DictTableType], [ParameterName], [UIIndex], [IsRequired], [GroupType], [flag]) VALUES (2336, 2927, 24, N' ', N'2', N'select', N'TD_SYS_BizOption', N'BrithMonth', N'生日月份', 24, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[TD_SYS_FieldAliasParameter] OFF
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TD_SYS_FieldAlias_IsFilterByCouponTemplet]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TD_SYS_FieldAlias] ADD  CONSTRAINT [DF_TD_SYS_FieldAlias_IsFilterByCouponTemplet]  DEFAULT ((0)) FOR [IsFilterByCouponTemplet]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TD_SYS_FieldAlias_IsDynamicAlias]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TD_SYS_FieldAlias] ADD  CONSTRAINT [DF_TD_SYS_FieldAlias_IsDynamicAlias]  DEFAULT ((0)) FOR [IsDynamicAlias]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TD_SYS_FieldAliasParameter_IsRequired]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TD_SYS_FieldAliasParameter] ADD  CONSTRAINT [DF_TD_SYS_FieldAliasParameter_IsRequired]  DEFAULT ((0)) FOR [IsRequired]
END

GO
