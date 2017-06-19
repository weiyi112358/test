/*--------------------------------------------------------------------------
* dbContext 实体类 for javascript
* ver 1.0.0.0
*
* 代码由代码生成器自动生成
* 请勿手动修改js代码
*--------------------------------------------------------------------------*/
var Templates={
	Auth_DataLimit:{
		HierarchyType:'STRING',
		HierarchyValue:'STRING',
		RangeType:'STRING',
		RangeValue:'STRING',
		PageID:'INT',
		Direction:'SHORT',
		AddedUser:'STRING',
		AddedDate:'DATETIME',
		ModifiedUser:'STRING',
		ModifiedDate:'DATETIME',
		ODA_Index:'STRING'
	},
	Auth_Role:{
		RoleID:'INT',
		RoleName:'STRING',
		RoleDesc:'STRING',
		RoleType:'STRING',
		Enable:'BOOL',
		AddedUser:'STRING',
		AddedDate:'DATETIME',
		ModifiedUser:'STRING',
		ModifiedDate:'DATETIME',
		ODA_Index:'STRING'
	},
	Auth_RolePageElementSettings:{
		RoleID:'INT',
		PageID:'INT',
		ElementValue:'STRING',
		ElementType:'STRING',
		SettingValue:'STRING',
		SettingType:'STRING',
		ODA_Index:'STRING'
	},
	Auth_RolePages:{
		RoleID:'INT',
		PageID:'INT',
		ODA_Index:'STRING'
	},
	Auth_User:{
		UserID:'INT',
		EmployeeNum:'STRING',
		UserName:'STRING',
		UserCode:'STRING',
		Password:'STRING',
		Email:'STRING',
		Mobile:'STRING',
		Ext:'STRING',
		Dept:'STRING',
		ExpiredDate:'DATETIME',
		Enable:'BOOL',
		AddedUser:'STRING',
		AddedDate:'DATETIME',
		ModifiedUser:'STRING',
		ModifiedDate:'DATETIME',
		ODA_Index:'STRING'
	},
	Auth_UserRole:{
		UserID:'INT',
		RoleID:'INT',
		ODA_Index:'STRING'
	},
	CRM_Member:{
		MemberID:'GUID',
		LastName:'STRING',
		FirstName:'STRING',
		Name:'STRING',
		StoreID:'INT',
		MemberCode:'STRING',
		RegisterDate:'DATETIME',
		Birthday:'DATETIME',
		Gender:'STRING',
		Mobile:'STRING',
		Email:'STRING',
		EDM:'BOOL',
		SMS:'BOOL',
		Source:'STRING',
		CrmKey:'LONG',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING',
		IsFirstCard:'BOOL',
		LastModifyPerson:'STRING',
		LastModifyTime:'DATETIME',
		LastModifySystem:'STRING'
	},
	CRM_MemberCard:{
		MemberID:'GUID',
		CardNo:'STRING',
		Type:'STRING',
		Active:'BOOL',
		Locked:'BOOL',
		Avai:'BOOL',
		CorpID:'INT',
		CorpName:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	CRM_MemberDiscountRate:{
		MemberLevel:'STRING',
		LeverName:'STRING',
		DiscountRate:'DECIMAL'
	},
	CRM_MemberExt:{
		MemberID:'GUID',
		Str_Attr_1:'STRING',
		Str_Attr_2:'STRING',
		Str_Attr_3:'STRING',
		Str_Attr_4:'STRING',
		Str_Attr_5:'STRING',
		Str_Attr_6:'STRING',
		Str_Attr_7:'STRING',
		Str_Attr_8:'STRING',
		Str_Attr_9:'STRING',
		Str_Attr_10:'STRING',
		Str_Attr_11:'STRING',
		Str_Attr_12:'STRING',
		Str_Attr_13:'STRING',
		Str_Attr_14:'STRING',
		Str_Attr_15:'STRING',
		Str_Attr_16:'STRING',
		Str_Attr_17:'STRING',
		Str_Attr_18:'STRING',
		Str_Attr_19:'STRING',
		Str_Attr_20:'STRING',
		Str_Attr_21:'STRING',
		Str_Attr_22:'STRING',
		Str_Attr_23:'STRING',
		Str_Attr_24:'STRING',
		Str_Attr_25:'STRING',
		Str_Attr_26:'STRING',
		Str_Attr_27:'STRING',
		Str_Attr_28:'STRING',
		Str_Attr_29:'STRING',
		Str_Attr_30:'STRING',
		Int_Attr_1:'INT',
		Int_Attr_2:'INT',
		Int_Attr_10:'LONG',
		Int_Attr_11:'LONG',
		Dec_Attr_1:'DECIMAL',
		Dec_Attr_2:'DECIMAL',
		Dec_Attr_10:'DECIMAL',
		Date_Attr_1:'DATETIME',
		Date_Attr_10:'DATETIME',
		Bool_Attr_1:'BOOL',
		Bool_Attr_2:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	CRM_MemberExtFieldAlias:{
		AliasID:'INT',
		TableName:'STRING',
		FieldName:'STRING',
		FieldType:'STRING',
		FieldAlias:'STRING',
		FieldDesc:'STRING',
		AliasType:'STRING',
		AliasKey:'STRING'
	},
	CRM_MemberLevelChanges:{
		MLCID:'LONG',
		MemberID:'GUID',
		PreLevel:'STRING',
		NextLevel:'STRING',
		ChangeType:'STRING',
		ChangeDate:'DATETIME',
		ChangeReason:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	CRM_MemberSubExt:{
		MSEID:'GUID',
		MemberID:'GUID',
		ExtCode:'STRING',
		ExtStringValue1:'STRING',
		ExtStringValue2:'STRING',
		ExtStringValue3:'STRING',
		ExtStringValue4:'STRING',
		ExtStringValue5:'STRING',
		ExtStringValue6:'STRING',
		ExtStringValue7:'STRING',
		ExtStringValue8:'STRING',
		ExtStringValue9:'STRING',
		ExtStringValue10:'STRING',
		ExtStringValue11:'STRING',
		ExtStringValue12:'STRING',
		ExtIntValue1:'INT',
		ExtIntValue2:'INT',
		ExtIntValue3:'INT',
		ExtBoolValue1:'BOOL',
		ExtBoolValue2:'BOOL',
		ExtBoolValue3:'BOOL',
		ExtDecValue1:'DECIMAL',
		ExtDecValue2:'DECIMAL',
		ExtDecValue3:'DECIMAL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	ODA_Sync:{
		SyncId:'INT',
		TableName:'STRING',
		ProcRise:'STRING',
		ProcAll:'STRING',
		ProcLen:'INT',
		IsActive:'BOOL',
		Type:'BYTE'
	},
	POS_Agent:{
		AgentID:'INT',
		AgentName:'STRING',
		AgentGrade:'STRING',
		AgentCode:'STRING',
		ParentAgentID:'INT',
		Remark:'STRING',
		ODA_Index:'STRING'
	},
	POS_CardApply:{
		ApplyID:'LONG',
		StoreID:'INT',
		ApplyDate:'DATETIME',
		ApplyPerson:'STRING',
		Reason:'STRING',
		Status:'SHORT',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING'
	},
	POS_CardApplyDetail:{
		ApplyID:'LONG',
		CardType:'STRING',
		ApplyQty:'INT',
		ApproveQty:'INT'
	},
	POS_CardCount:{
		CountID:'LONG',
		StoreID:'INT',
		CountPerson:'STRING',
		CountTime:'DATETIME',
		Status:'SHORT',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING'
	},
	POS_CardCountDetail:{
		CountID:'LONG',
		CardType:'STRING',
		OrgTotalQty:'INT',
		AfterCountTotalQty:'INT',
		Diff:'INT'
	},
	POS_CardTruck:{
		StoreID:'INT',
		CardType:'STRING',
		AvaiQty:'INT',
		FreezeQty:'INT',
		TotalQty:'INT'
	},
	POS_CardTruckHistory:{
		HistoryId:'LONG',
		StoreId:'LONG',
		CardType:'STRING',
		Qty:'INT',
		AddedDate:'DATETIME',
		AddedUser:'STRING'
	},
	POS_Cashbox:{
		CashboxID:'INT',
		CashboxCode:'STRING',
		StoreID:'INT',
		Enable:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING',
		StartNo:'STRING',
		Printer1:'STRING',
		Printer2:'STRING'
	},
	POS_CashFlow:{
		CashFlowID:'LONG',
		CashboxID:'INT',
		IO:'BOOL',
		Amount:'DECIMAL',
		ReferenceNo:'STRING',
		ReferenceType:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		Remark:'STRING',
		ODA_Index:'STRING'
	},
	POS_DailyDebit:{
		DailyDebitID:'LONG',
		StoreID:'INT',
		CashboxID:'INT',
		StartDate:'DATETIME',
		EndDate:'DATETIME',
		InitialAmount:'DECIMAL',
		OrderCount:'INT',
		ReturnCount:'INT',
		PreSaleCount:'INT',
		SaveTimes:'INT',
		DrawTimes:'INT',
		OrderAmt:'DECIMAL',
		ReturnAmt:'DECIMAL',
		SaveAmt:'DECIMAL',
		DrawAmt:'DECIMAL',
		CashAmount:'DECIMAL',
		NoCashAmount:'DECIMAL',
		DailyDebitDate:'DATETIME',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		IsCreated:'BOOL',
		IsDataValid:'BOOL',
		ODA_Index:'STRING'
	},
	POS_OnDuty:{
		OnDutyID:'INT',
		StoreID:'INT',
		CashboxID:'INT',
		InitialAmount:'DECIMAL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		Remark:'STRING',
		ODA_Index:'STRING'
	},
	POS_OrderDetail:{
		OrderID:'GUID',
		DetailIndex:'SHORT',
		ProductID:'INT',
		ProductCode:'STRING',
		Barcode:'STRING',
		ProductName:'STRING',
		CategoryA:'STRING',
		CategoryB:'STRING',
		CategoryC:'STRING',
		Qty:'INT',
		OrigPrice:'DECIMAL',
		OrigAmt:'DECIMAL',
		MemberPrice:'DECIMAL',
		ActPrice:'DECIMAL',
		ActAmt:'DECIMAL',
		DisAmt:'DECIMAL',
		IsGift:'BOOL',
		IsSuit:'BOOL',
		SuitCode:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING',
		Remark:'STRING'
	},
	POS_OrderMaster:{
		OrderID:'GUID',
		OrderCode:'STRING',
		Type:'STRING',
		ReferenceNo:'STRING',
		MemberID:'GUID',
		CardNo:'STRING',
		SalesGroup:'STRING',
		SalesPerson:'STRING',
		StoreID:'INT',
		CashboxID:'INT',
		OrigAmt:'DECIMAL',
		DCT:'DECIMAL',
		ACT:'DECIMAL',
		Remark:'STRING',
		Status:'SHORT',
		NoCountPoints:'BOOL',
		PotentialID:'LONG',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING',
		PrintNumber:'STRING'
	},
	POS_OrderPayment:{
		OrderID:'GUID',
		PaymentIndex:'SHORT',
		PaymentType:'STRING',
		SubPaymentType:'STRING',
		PayAmt:'DECIMAL',
		ReferenceNo:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_PreOrderDetail:{
		PreOrderID:'GUID',
		DetailIndex:'SHORT',
		ProductID:'INT',
		ProductCode:'STRING',
		Barcode:'STRING',
		ProductName:'STRING',
		Standard:'STRING',
		CategoryA:'STRING',
		CategoryB:'STRING',
		CategoryC:'STRING',
		Qty:'INT',
		OrigPrice:'DECIMAL',
		OrigAmt:'DECIMAL',
		ReachDate:'DATETIME',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		Remark:'STRING',
		ODA_Index:'STRING'
	},
	POS_PreOrderMaster:{
		PreOrderID:'GUID',
		PreOrderCode:'STRING',
		BookPerson:'STRING',
		Contaction:'STRING',
		CarType:'STRING',
		MemberID:'GUID',
		CardNo:'STRING',
		SalesGroup:'STRING',
		SalesPerson:'STRING',
		StoreID:'INT',
		CashboxID:'INT',
		TotalAmt:'DECIMAL',
		Remark:'STRING',
		Status:'SHORT',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_PreOrderPayment:{
		PreOrderID:'GUID',
		PaymentIndex:'SHORT',
		PaymentType:'STRING',
		SubPaymentType:'STRING',
		PayAmt:'DECIMAL',
		ReferenceNo:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_ProdCatgA:{
		CatgAID:'INT',
		CatgACode:'STRING',
		CatgAName:'STRING',
		Enable:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_ProdCatgB:{
		CatgBID:'INT',
		CatgBCode:'STRING',
		CatgBName:'STRING',
		CatgAID:'INT',
		Enable:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_ProdCatgC:{
		CatgCID:'INT',
		CatgCCode:'STRING',
		CatgCName:'STRING',
		CatgBID:'INT',
		Enable:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_Product:{
		ProductID:'INT',
		ProductCode:'STRING',
		Barcode:'STRING',
		ProductName:'STRING',
		CategoryA:'STRING',
		CategoryB:'STRING',
		CategoryC:'STRING',
		Standard:'STRING',
		Unit:'STRING',
		ProductType:'STRING',
		IsAutoCombo:'BOOL',
		BasicPrice:'DECIMAL',
		Enable:'BOOL',
		IsServiceProd:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_ProductPrice:{
		ProductID:'INT',
		StoreID:'INT',
		ProductCode:'STRING',
		Price1:'DECIMAL',
		Price2:'DECIMAL',
		Price3:'DECIMAL',
		IsServiceProd:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_ReturnOrderDetail:{
		ReturnOrderID:'GUID',
		OrderID:'GUID',
		OrderDetailIndex:'SHORT',
		ProductID:'INT',
		Barcode:'STRING',
		ProductCode:'STRING',
		ProductName:'STRING',
		CategoryA:'STRING',
		CategoryB:'STRING',
		CategoryC:'STRING',
		Qty:'INT',
		OrigPrice:'DECIMAL',
		OrigAmt:'DECIMAL',
		ActPrice:'DECIMAL',
		ActAmt:'DECIMAL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING'
	},
	POS_ReturnOrderMaster:{
		ReturnOrderID:'GUID',
		ReturnOrderCode:'STRING',
		OrderID:'GUID',
		StoreID:'INT',
		MemberID:'GUID',
		CardNo:'STRING',
		SalesGroup:'STRING',
		SalesPerson:'STRING',
		CashboxID:'INT',
		ReturnDate:'DATETIME',
		TotalAmt:'DECIMAL',
		Status:'SHORT',
		Remark:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING'
	},
	POS_ReturnOrderPayment:{
		ReturnOrderID:'GUID',
		PaymentIndex:'SHORT',
		PaymentType:'STRING',
		SubPaymentType:'STRING',
		PayAmt:'DECIMAL',
		ReferenceNo:'STRING',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING'
	},
	POS_SalesPerson:{
		PersonID:'STRING',
		Name:'STRING',
		StoreID:'INT',
		Enable:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifiedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_ShiftDuty:{
		ShiftDutyID:'INT',
		OnDutyID:'INT',
		StoreID:'INT',
		CashboxID:'INT',
		InitialAmount:'DECIMAL',
		CashAmount:'DECIMAL',
		NoCashAmount:'DECIMAL',
		OrderCount:'INT',
		DrawAmt:'DECIMAL',
		SaveAmt:'DECIMAL',
		OrderAmt:'DECIMAL',
		ReturnAmt:'DECIMAL',
		OnDutyDate:'DATETIME',
		AccountPayAmt:'DECIMAL',
		BankCardPayAmt:'DECIMAL',
		VoucherPayAmt:'DECIMAL',
		JiaotongPayAmt:'DECIMAL',
		LianhuaPayAmt:'DECIMAL',
		ShanDePayAmt:'DECIMAL',
		SmartCardPayAmt:'DECIMAL',
		ShiftDutyDate:'DATETIME',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ODA_Index:'STRING'
	},
	POS_Store:{
		StoreID:'INT',
		StoreCode:'STRING',
		StoreName:'STRING',
		Address:'STRING',
		RegionID:'INT',
		AgentID:'INT',
		Enable:'BOOL',
		Tel:'STRING',
		Fax:'STRING',
		IsHQ:'BOOL',
		AddedDate:'DATETIME',
		AddedUser:'STRING',
		ModifiedDate:'DATETIME',
		ModifedUser:'STRING',
		ODA_Index:'STRING'
	},
	PrintTemplate:{
		StoreID:'INT',
		PrintType:'STRING',
		TemplatePath:'STRING',
		Remark:'STRING'
	},
	Sys_Biz_Option:{
		ExtID:'INT',
		ExtType:'STRING',
		ExtCode:'STRING',
		ExtValue:'STRING',
		Sort:'SHORT',
		Remark:'STRING',
		Enable:'BOOL'
	},
	Sys_HotKey:{
		PageID:'INT',
		HotKey:'STRING',
		CallBack:'STRING',
		Type:'SHORT'
	},
	Sys_MenuList:{
		MenuID:'INT',
		System:'STRING',
		Layer:'SHORT',
		Type:'STRING',
		Sort:'SHORT',
		ParentMenuID:'INT',
		MenuIcon:'STRING',
		DispName:'STRING',
		MenuDesc:'STRING',
		Enable:'BOOL',
		ODA_Index:'STRING'
	},
	Sys_PageElement:{
		PageID:'INT',
		ElementValue:'STRING',
		ElementType:'STRING',
		ElementDispName:'STRING',
		SettingAvaiValues:'STRING',
		SettingType:'STRING',
		ODA_Index:'STRING'
	},
	Sys_PageList:{
		PageID:'INT',
		Sort:'SHORT',
		MenuID:'INT',
		Path:'STRING',
		DispName:'STRING',
		PageDesc:'STRING',
		OfflineSupport:'BOOL',
		Enable:'BOOL',
		Visable:'BOOL',
		Nav:'STRING',
		ODA_Index:'STRING'
	},
	Sys_PaymentDict:{
		PaymentType:'STRING',
		PaymentDesc:'STRING',
		ParentPaymentType:'STRING',
		IsCash:'BOOL',
		UsePlace:'STRING',
		TBMCode:'STRING',
		TBMID:'STRING'
	},
	Sys_RegionDict:{
		RegionID:'INT',
		ParentRegionID:'INT',
		RegionPath:'STRING',
		RegionGrade:'SHORT',
		NameZH:'STRING',
		NameEN:'STRING',
		SortIndex:'INT',
		Enable:'BOOL',
		ReferenceCode:'STRING',
		ODA_Index:'STRING'
	},
	Sys_VehicleDict:{
		VehicleID:'INT',
		ParentVehicleID:'INT',
		VehiclePath:'STRING',
		VehicleGrade:'SHORT',
		NameZH:'STRING',
		NameEN:'STRING',
		Enable:'BOOL',
		ODA_Index:'STRING',
		FirstLetter:'STRING'
	},
	View_UserAuth:{
		ID:'GUID',
		UserID:'INT',
		RoleID:'INT',
		PageID:'INT',
		Path:'STRING',
		ElementValue:'STRING',
		ElementType:'STRING',
		SettingValue:'STRING',
		SettingType:'STRING'
	}
},TemplateConfig={
	Auth_DataLimit:{
		Key:['HierarchyType','HierarchyValue','RangeType','RangeValue','PageID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Auth_Role:{
		Key:['RoleID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Auth_RolePageElementSettings:{
		Key:['RoleID','PageID','ElementValue','ElementType'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Auth_RolePages:{
		Key:['RoleID','PageID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Auth_User:{
		Key:['UserID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Auth_UserRole:{
		Key:['UserID','RoleID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	CRM_Member:{
		Key:['MemberID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	CRM_MemberCard:{
		Key:['CardNo'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	CRM_MemberDiscountRate:{
		Key:['MemberLevel'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	CRM_MemberExt:{
		Key:['MemberID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	CRM_MemberExtFieldAlias:{
		Key:['AliasID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	CRM_MemberLevelChanges:{
		Key:['MLCID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	CRM_MemberSubExt:{
		Key:['MSEID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	ODA_Sync:{
		Key:['SyncId'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_Agent:{
		Key:['AgentID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_CardApply:{
		Key:['ApplyID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_CardApplyDetail:{
		Key:['ApplyID','CardType'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_CardCount:{
		Key:['CountID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_CardCountDetail:{
		Key:['CountID','CardType'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_CardTruck:{
		Key:['StoreID','CardType'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_CardTruckHistory:{
		Key:['HistoryId'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_Cashbox:{
		Key:['CashboxID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_CashFlow:{
		Key:['CashFlowID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_DailyDebit:{
		Key:['DailyDebitID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_OnDuty:{
		Key:['OnDutyID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_OrderDetail:{
		Key:['OrderID','DetailIndex'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_OrderMaster:{
		Key:['OrderID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_OrderPayment:{
		Key:['OrderID','PaymentIndex'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_PreOrderDetail:{
		Key:['PreOrderID','DetailIndex'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_PreOrderMaster:{
		Key:['PreOrderID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_PreOrderPayment:{
		Key:['PreOrderID','PaymentIndex'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ProdCatgA:{
		Key:['CatgAID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ProdCatgB:{
		Key:['CatgBID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ProdCatgC:{
		Key:['CatgCID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_Product:{
		Key:['ProductID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ProductPrice:{
		Key:['StoreID','ProductCode'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ReturnOrderDetail:{
		Key:['ReturnOrderID','OrderID','OrderDetailIndex'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ReturnOrderMaster:{
		Key:['ReturnOrderID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ReturnOrderPayment:{
		Key:['ReturnOrderID','PaymentIndex'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_SalesPerson:{
		Key:['PersonID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_ShiftDuty:{
		Key:['ShiftDutyID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	POS_Store:{
		Key:['StoreID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	PrintTemplate:{
		Key:['StoreID','PrintType'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_Biz_Option:{
		Key:['ExtID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_HotKey:{
		Key:['PageID','HotKey'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_MenuList:{
		Key:['MenuID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_PageElement:{
		Key:['PageID','ElementValue','ElementType'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_PageList:{
		Key:['PageID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_PaymentDict:{
		Key:['PaymentType'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_RegionDict:{
		Key:['RegionID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	Sys_VehicleDict:{
		Key:['VehicleID'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	},
	View_UserAuth:{
		Key:['UserID','RoleID','PageID','Path'],
		Sync: {
			Add: '/DataSync/Add',     //Parameter:[Model]
			Edit: '/DataSync/Edit',		//Parameter:[Model]
			Del: '/DataSync/Remove',       //Parameter:[Model]
		}
	}
};
