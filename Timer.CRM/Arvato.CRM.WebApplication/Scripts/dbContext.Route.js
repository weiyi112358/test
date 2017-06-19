var Route = [
    {
        Uri: '/Login',
        Req: ['userCode', 'password', 'storeID', 'cashboxID'],
        Fun: 'Auth_UserBal.Login'
    },
    {
        Uri: '/Login/GetAuth',
        Req: ['userId', 'pageStr'],
        Fun: 'Auth_UserBal.GetAuth'
    },
    {
        Uri: '/Member/GetMemberList',
        Fun: 'Member_Offline.GetMemberList'
    },
    {
        Uri: '/Member/AddMember',
        Fun: 'Member_Offline.Add'
    },
    {
        Uri: '/Member/EditMember',
        Fun: 'Member_Offline.Edit'
    },
    {
        Uri: '/Login/GetLimitCashbox',
        Req: ['storeID'],
        Fun: 'Auth_UserBal.GetLimitCashbox'
    },
    {
        Uri: '/Product/ScanProduct',
        Req: ['productCode', 'storeID'],
        Fun: 'Sales_Offline.ScanProduct'
    },
    {
        Uri: '/Share/GetProvince',
        Fun: 'Member_Offline.GetProvince'
    },
    {
        Uri: '/Share/GetCity',
        Req: ['provinceID'],
        Fun: 'Member_Offline.GetCity'
    },
    {
        Uri: '/Sales/AddOrders',
        Fun: 'Sales_Offline.AddOrders'
    },
    {
        Uri: '/Member/GetMemberByCard',
        Req: ['cardNo'],
        Fun: 'Member_Offline.GetMemberByCard'
    },
    {
        Uri: '/Sales/SearchOrders',
        Fun: 'Sales_Offline.SearchOrders'
    },
    {
        Uri: '/Sales/OrderPay',
        Fun: 'Sales_Offline.OrderPay'
    },
    {
        Uri: '/Login/GetLimitStoreByUser',
        Req: ['userCode'],
        Fun: 'Auth_UserBal.GetLimitStoreByUser'
    },
    {
        Uri: '/Login/GetMenuList',
        Req: ['userID'],
        Fun: 'Auth_UserBal.GetMenuList'
    },
    {
        Uri: '/Login/GetCurPageList',
        Fun: 'Auth_UserBal.GetCurPageList'
    },
    {
        Uri: '/Member/GetMemberPackage',
    },
    {
        Uri: '/Member/GetMemberCoupon',
    },
    {
        Uri: '/Sales/GetSalesPerson',
        Fun: 'Sales_Offline.GetSalesPerson'
    },
    {
        Uri: '/Member/GetCommonCoupon',
    },
    {
        Uri: '/Share/GetPaymentType',
        Fun: 'Sales_Offline.GetPaymentType'
    },
    {
        Uri: '/Share/GetCurStoreMSG',
        Fun: 'Member_Offline.GetCurStoreMSG'
    },
    {
        Uri: '/Sales/GetCashFlow',
        Fun: 'Sales_Offline.GetCashFlow'
    },
    {
        Uri: '/Member/GetPackageByCode',
    },
    {
        Uri: '/Sales/PreOrderPay'
    },
    {
        Uri: '/Sales/SearchPreOrders'
    },
    {
        Uri: '/Sales/CancelPreOrder'
    },
    {
        Uri: '/Sales/ReturnOrder'
    },
    {
        Uri: '/Sales/SearchReturnOrders'
    },
    {
        Uri: '/Sales/SearchOrderByCode'
    },
    {
        Uri: '/Member/GetVehicleBrand',
        Fun: 'Member_Offline.GetVehicleBrand'
    },
    {
        Uri: '/Member/GetVehicleSeries',
        Fun: 'Member_Offline.GetVehicleSeries'
    },
    {
        Uri: '/Member/GetVehicleType',
        Fun: 'Member_Offline.GetVehicleType'
    },
    {
        Uri: '/Member/GetMemberInfoByCardNo',
        Fun: 'Member_Offline.GetMemberInfoByCardNo'
    },
    {
        Uri: '/Share/GetCounty',
        Fun: 'Member_Offline.GetCounty'
    },
    {
        Uri: '/Member/GetPackageHistory'
    },
    {
        Uri: '/Member/GetMemberOrder'
    },
    {
        Uri: '/Member/GetMemberOrderDetail'
    },
    {
        Uri: '/Member/GetMemberOrderPayment'
    },
    {
        Uri: '/Member/UpdatePassword'
    },
    {
        Uri: '/Member/AddMemberSubExt'
    },
    {
        Uri: '/Member/EditMemberSubExt'
    },
    {
        Uri: '/Member/DeleteMemberSubExt'
    },
    {
        Uri: '/Member/ChangeOrSupplyCard'
    },
    {
        Uri: '/Member/LoseOrRelaseCard'
    },
    {
        Uri: '/Member/GetAccountInfo'
    },
    {
        Uri: '/Login/SaveOnDuty',
        Fun: 'Member_Offline.SaveOnDuty'
    },
    {
        Uri: '/Login/IsOnDuty',
        Fun: 'Auth_UserBal.IsOnDuty'
    },
    {
        Uri: '/Login/GetOnDutyData',
        Fun: 'Member_Offline.GetOnDutyData'
    },
    {
        Uri: '/Login/GetShiftDutyData',
        Fun: 'Member_Offline.GetShiftDutyData'
    },
    {
        Uri: '/Login/SaveShiftDutyData',
        Fun: 'Member_Offline.SaveShiftDutyData'
    },
    {
        Uri: '/Login/GetDailyDebitData',
        Fun: 'Member_Offline.GetDailyDebitData'
    },
    {
        Uri: '/Login/GetSummaryDebitData',
        Fun: 'Member_Offline.GetSummaryDebitData'
    },
    {
        Uri: '/Login/SaveDailyDebitData',
        Fun: 'Member_Offline.SaveDailyDebitData'
    },
    {
        Uri: '/Login/IsShiftDuty',
        Fun: 'Member_Offline.IsShiftDuty'
    },
    {
        Uri: '/Login/SaveCashboxAmt',
        Fun: 'Member_Offline.SaveCashboxAmt'
    },
    {
        Uri: '/Login/IsDailyDebit',
        Fun: 'Member_Offline.IsDailyDebit'
    },
    {
        Uri: '/Member/ShiftGiftCardToMember'
    },
    {
        Uri: '/Share/GetInterestOption'
    },
    {
        Uri: '/Member/GetMemberGiftReceive'
    },
    {
        Uri: '/Sales/ReceiveGiftAddOrders'
    },
    {
        Uri: '/Share/GetVehicleColorOption',
        Fun: 'Member_Offline.GetVehicleColorOption'
    },
    {
        Uri: '/Share/GetChannelOption',
        Fun: 'Member_Offline.GetChannelOption'
    },
    {
        Uri: '/Share/GetIncomeOption'
    },
    {
        Uri: '/Share/GetPostOption'
    },
    {
        Uri: '/Share/GetIndustryOption'
    },
    {
        Uri: '/Share/GetCertifyOption',
        Fun: 'Sales_Offline.GetCertifyOption'
    },
    {
        Uri: '/Sales/GetPackageByBarcode'
    },
    {
        Uri: '/Login/AuthorizeOperation'
    },
    {
        Uri: '/Login/UpdateLoginPassword'
    },
    {
        Uri: '/Member/LockMemberByCard'
    },
    {
        Uri: '/Member/AddPotential'
    },
    {
        Uri: '/Member/GetPotentialByID'
    },
    {
        Uri: '/Member/GetCorpByID'
    },
    {
        Uri: '/Member/GetMemGrageChangeHistory'
    },
    {
        Uri: '/Member/ResetPassword'
    },
    {
        Uri: '/Product/GetProductCategoryA'
    },
    {
        Uri: '/Product/GetProductCategoryB'
    },
    {
        Uri: '/Product/GetProductCategoryC'
    },
    {
        Uri: '/Product/GetProduct'
    },
    {
        Uri: '/Share/GetSpentMinuteStore'
    },
    {
        Uri: '/Share/GetVehicleLevel'
    },
    {
        Uri: '/Share/GetStore'
    },
    {
        Uri: '/Member/GetActivityList'
    },
    {
        Uri: '/Sales/CancelTempOrder'
    },
    {
        Uri: '/Member/GetCorporationByID'
    },
    {
        Uri: '/Share/GetPaymentDict',
        Fun: 'Sales_Offline.GetPaymentDict'
    },
    {
        Uri: '/Share/GetPrintTemplate'
    },
    {
        Uri: '/Member/isChangedCard'
    },
    {
        Uri: '/Login/LogOut'
    },
    {
        Uri: '/Member/GetVehicleInfo'
    },
    {
        Uri: '/Sales/GetOrderPrintNumber'
    },
    {
        Uri: '/Sales/GetNoMarkCoupon'
    },
    {
        Uri: '/Message/GetMessageList',
    },
    {
        Uri: '/Message/GetMessageCount',
    },
    {
        Uri: '/Message/ReadMessage',
    },
    {
        Uri: '/Member/GetMemAppointMement',
    },
    {
        Uri: '/Member/UpdateMemAppointStatus',
    },
    {
        Uri: '/Order/GetOrderList',
    },
    {
        Uri: '/Order/SearchOrderDetail',
    },
    {
        Uri: '/Order/SearchOrders',
    },
    {
        Uri: '/Member/CombineMember',
    },
    {
        Uri: '/Member/GetMemberVehicle',
    },
    {
        Uri: '/Sales/GetMemMaintains',
    },
    {
        Uri: '/Member/GetMemMaintainsByCarNo',
    }
    
];
