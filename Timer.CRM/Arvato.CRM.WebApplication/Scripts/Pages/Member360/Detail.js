var salesTradeId = "";
var serviceTradeId = "";
var rechargeTradeId = "";
var packageTradeId = "";
var dtCarInfo, dtLevelHistory, dlgMemInfoChangeHistory, dtSales_insurance_TH, dtSales_pack_TH, dtSales_oilcard_TH, dtSales_coupon1_TH, dtSales_servercard_TH;
var dtSales_TH, dtService_TH, dtSales_dress_TH, dtSales_others_TH;
var dtSales_package_TH, dtSales_coupon2_TH, dtSales_result_TH, dtSales_config_TH, dtService_repairitem_TH, dtService_salesmaterial_TH, dtService_addition_TH, dtService_coupon_TH, dtService_package_TH, dtService_result_TH, dtServiceRights;
var dtPackage, dtPackageDetail_P, dtPackageLimit_P, dtContactsInfo, dtAccountHistory, dtAccountDetailHistory, dtAccountCouponList, dtChange_BHP;
var dtSMS_CH, dtEmail_CH, dtReserve_CH, dtService_invoice_TH;
var dtRecharge_TH, dtPackage_TH, dtRecharge_invoice_TH, dtPackage_detail_TH, dtPackage_invoice_TH; dtPackageHistory;
$(function () {
    $(".datepicker").datepicker();
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }

    //getTabAccountInfo();
    GetTabContactsInfo();
    //main tab导航
    $("#navMain li a").click(function (e) {
        e.preventDefault();
        if ($(this).attr("loaded") != "1") {
            var tabId = $(this).attr("href");
            if (tabId == "#tabCarInfo") {
                loadCarInfoList();
            }
            else if (tabId == "#tabTradeHistory") {
                getTabSales_TH();
            }
            else if (tabId == "#tabPackageInfo") {
                loadPackages();
            }
            else if (tabId == "#tabAccountDetail") {
                getAccountCoupon();
            }
            else if (tabId == "#tabCommunicateHistory") {
                getCommunicationHistory();
            }
            $(this).attr("loaded", "1");
        }
    });
    //交易历史数据导航
    $("#navTradeHistory li a").click(function (e) {
        e.preventDefault();
        var tabId = $(this).attr("href");
        if (tabId == "#tabSales_TH") {
            getTabSales_TH();
        }
        else if (tabId == "#tabService_TH") {
            getTabService_TH();
        }
        else if (tabId == "#tabRecharge_TH") {
            getTabRecharge_TH();
        }
        else if (tabId == "#tabPackage_TH") {
            getTabPackage_TH();
        }
    });

    //交易历史查询 
    $("#btnSearchTradeHistory").click(function (e) {
        e.preventDefault();
        getTabSales_TH();
    });
    //套餐查询
    $("#btnSearchPackage").click(function (e) {
        e.preventDefault();
        loadPackages();
    });
});

function loadCarInfoList() {
    if (!dtCarInfo) {
        dtCarInfo = $('#dtCarInfo').dataTable({
            sAjaxSource: '/Member360/GetMemberCarInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "CarNo", title: "车牌号", sortable: false },
                {
                    data: "VihicleBrandName", title: "品牌", sortable: false
                },
                {
                    data: null, title: "车系", sortable: false, render: function (obj) {
                        return utility.isNull(obj.VehicleSeriesNameService) ? obj.VehicleSeriesNameSales : obj.VehicleSeriesNameService
                    }
                },
                {
                    data: null, title: "车型", sortable: false, render: function (obj) {
                        return utility.isNull(obj.VehicleLevelNameService) ? obj.VehicleLevelNameSales : obj.VehicleLevelNameService
                    }
                },
                {
                    data: null, title: "车辆颜色", sortable: false, render: function (obj) {
                        return utility.isNull(obj.VehicleColorNameService) ? obj.VehicleColorNameSales : obj.VehicleColorNameService
                    }
                },
                { data: "InteriorName", title: "内饰", sortable: false },
                { data: "DriveDistinct3", title: "行驶里程", sortable: false },
                { data: "VIN", title: "车架号", sortable: false },
                {
                    data: null, title: "购车时间", sortable: false, render: function (obj) {
                        return "" //!obj ? "" : obj.substr(0, 10);
                    }
                },
                //{ data: null, title: "使用年限", sortable: false },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="detailCarInfo(' + obj.MemberSubExtID + ')">明细</button> ';
                        return str;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
            }
        });
    }
    else {
        dtCarInfo.fnDraw();
    }
}
function detailCarInfo(id) {
    clearFormData("frmEditCarInfo");
    if (id) {
        $("#hdnCarInfoId").val(id);
        ajax("/Member360/GetMemberCarInfoById", { id: id }, function (data) {
            if (data.IsPass) {
                var car = data.Obj[0];
                $("#txtCarNo").val(car.CarNo);
                $("#txtInsuranceDate").val(car.InsuranceDate == null ? "" : car.InsuranceDate.substr(0, 10));
                $("#txtValidateDate").val(car.ValidateDate == null ? "" : car.ValidateDate.substr(0.10));
                $("#txtVIN").val(car.VIN);
                $("#txtVehicleProperties").val(car.VehicleProperties);
                $("#txtBrand").val(car.VihicleBrandName);
                $("#txtEngineNo").val(car.EngineNo);
                $("#txtVehicleSeriesName").val(utility.isNull(car.VehicleSeriesNameService) ? car.VehicleSeriesNameSales : car.VehicleSeriesNameService);
                $("#txtVehicleSeriesCode").val(utility.isNull(car.VehicleSeriesCodeService) ? car.VehicleSeriesCodeSales : car.VehicleSeriesCodeService);
                $("#txtVehicleLevelName").val(utility.isNull(car.VehicleLevelNameService) ? car.VehicleLevelNameSales : car.VehicleLevelNameService);
                $("#txtVehicleLevelCode").val(utility.isNull(car.VehicleLevelCodeService) ? car.VehicleLevelCodeSales : car.VehicleLevelCodeService);
                $("#txtVehicleColorName").val(utility.isNull(car.VehicleColorNameService) ? car.VehicleColorNameSales : car.VehicleColorNameService);
                $("#txtVehicleColorCode").val(utility.isNull(car.VehicleColorCodeService) ? car.VehicleColorCodeSales : car.VehicleColorCodeService);
                $("#txtVehicleYear").val(car.VehicleYear);
                $("#txtDriveDistinct1").val(car.DriveDistinct1);
                $("#txtDriveDistinct2").val(car.DriveDistinct2);
                $("#txtDriveDistinct3").val(car.DriveDistinct3);
                $("#txtInteriorName").val(car.InteriorName);
                $("#txtInteriorCode").val(car.InteriorCode);
                $("#txtVehicleSpecial").val(car.VehicleSpecial);
                $("#txtSourceNo").val(car.SourceNo);
                $("#txtMaintenanceStartTime").val(car.MaintenanceStartTime == null ? "" : car.MaintenanceStartTime.substr(0, 10));
                $("#txtLastInFactoryDate").val(car.LastInFactoryDate == null ? "" : car.LastInFactoryDate.substr(0, 10));
                $("#txtNextMaintenanceDate").val(car.NextMaintenanceDate == null ? "" : car.NextMaintenanceDate.substr(0, 10));
                $("#txtDriveDistinct4").val(car.DriveDistinct4);
                var curDate = new Date();
                $("#txtUseAge").val(utility.isNull(car.BuyVehicleDate) ? "" : utility.floatCalculate(curDate.getFullYear(), new Date(car.BuyVehicleDate).getFullYear(), "-"));
                $("#txtPurchasingDate").val(utility.isNull(car.BuyVehicleDate) ? "" : car.BuyVehicleDate.substr(0, 10));
            }
            else {
                $.dialog(data.MSG);
            }
        });
    }
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#dlgEditCarInfo",
        inline: true
    });
}
function getCorpInfo(corpId) {
    clearFormData("dlgCorpInfo");
    if (corpId) {
        ajax("/BaseData/GetCorpById", { corpId: corpId }, function (data) {
            $("#txtCorpName_Corp").val(data.CorpName);
            $("#txtCorpContract_Corp").val(data.CorpContract);
            $("#txtCorpPhoneNo_Corp").val(data.CorpPhoneNo);
            $("#txtAddress_Corp").val(data.CorpAddress);
        });
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            overlayClose: false,
            opacity: '0.3',
            //title: '素材',
            href: "#dlgCorpInfo",
            inline: true
        });
    }
}

function getLevelHistory() {
    if (!dtLevelHistory) {
        dtLevelHistory = $('#dtLevelHistory').dataTable({
            sAjaxSource: '/Member360/GetMemLevelHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "LevelChangeTypeText", title: "变更类型", sortable: false },
                { data: "ChangeLevelFrom", title: "变更前等级", sortable: false },
                { data: "StartDateFrom", title: "变更前等级有效开始", sortable: false },
                { data: "EndDateFrom", title: "变更前等级有效结束", sortable: false },
                { data: "ChangeLevelToText", title: "新等级", sortable: false },
                { data: "StartDateTo", title: "变更后等级有效开始", sortable: false },
                { data: "EndDateTo", title: "变更后等级有效结束", sortable: false },
                { data: "AddedDate", title: "变更时间", sortable: false },
                { data: "ChangeUserName", title: "变更者", sortable: false },
                { data: "ChangeReason", title: "调整原因", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
            }
        });
    }
    else {
        dtLevelHistory.fnDraw();
    }
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#dlgLevelHistory",
        inline: true
    });
}
function getMemInfoChangeHistory() {
    if (!dlgMemInfoChangeHistory) {
        dlgMemInfoChangeHistory = $('#dlgMemInfoChangeHistory').dataTable({
            sAjaxSource: '/Member360/GetMemInfoChangeHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: null, title: "字段", sortable: false },
                { data: null, title: "修改前", sortable: false },
                { data: null, title: "修改后", sortable: false },
                { data: null, title: "修改时间", sortable: false },
                { data: null, title: "修改人", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'start', value: $("#dateChgStart_InfoHty").val() });
                d.push({ name: 'end', value: $("#dateChgEnd_infoHty").val() });
            }
        });
    }
    else {
        dlgMemInfoChangeHistory.fnDraw();
    }
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#dlgLevelHistory",
        inline: true
    });
}
function showChangeHistory_BHP() {
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#dlgChange_BHP",
        inline: true
    });
}
function getChangeHistory_BHP() {
    if (!dtChange_BHP) {
        dtChange_BHP = $("#dtChange_BHP").dataTable({
            sAjaxSource: '/Service/GetLogsByMainIdAndType',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "FieldName", title: "字段", sortable: false },
                { data: "OrgValue", title: "修改前", sortable: false },
                { data: "ChangeValueTo", title: "修改后", sortable: false },
                {
                    data: "AddedDate", title: "修改时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "AddedUserName", title: "修改人", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mainId', value: $("#hdnMemberId").val() });
                d.push({ name: 'mainType', value: "MemberExt" });
                d.push({ name: 'start', value: $("#dateChgStart_BHP").val() });
                d.push({ name: 'end', value: $("#dateChgEnd_BHP").val() });
            }
        });
    }
    else {
        dtChange_BHP.fnDraw();
    }
}

//加载交易历史数据
//购车订单
function getTabSales_TH() {
    if (!dtSales_TH) {
        dtSales_TH = $('#dtSales_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            bScrollCollapse: true,
            sScrollX: '1200px',
            aoColumns: [
                { data: "TradeCodeSales", title: "流转单号", sortable: false, sWidth: "100px" },
                {
                    data: null, title: "操作", sortable: false, sWidth: "100px", render: function (obj) {
                        return '<button class="btn" onclick="showSalesDetail(this,' + obj.TradeID + ')">查看明细</button></td>';
                    }
                },
                { data: "PayNameSales", title: "开票名称", sortable: false, sWidth: "100px" },
                { data: "ConSultantSales", title: "销售顾问", sortable: false, sWidth: "100px" },
                { data: "SalesChannelSales", title: "销售渠道", sortable: false, sWidth: "100px" },
                { data: "VihicleBrandName", title: "品牌", sortable: false, sWidth: "100px" },
                { data: "VehicleSeriesNameSales", title: "车系", sortable: false, sWidth: "100px" },
                { data: "VehicleLevelNameSales", title: "车型", sortable: false, sWidth: "100px" },
                //{ data: "", title: "开票价格", sortable: false },
                { data: "InsurancePaysSales", title: "保险预估自付", sortable: false, sWidth: "100px" },
                //{ data: "", title: "保险公司", sortable: false },
                //{
                //    data: "", title: "投保期限", sortable: false, render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                { data: "ServerCardMoneySales", title: "服务卡总金额", sortable: false, sWidth: "100px" },
                { data: "ServerCardPaysSales", title: "服务卡自付", sortable: false, sWidth: "100px" },
                { data: "ServerCardDiscountSales", title: "服务卡折让", sortable: false, sWidth: "100px" },
                { data: "Coupon1MoneySales", title: "抵用券总金额", sortable: false, sWidth: "100px" },
                { data: "Coupon1PaysSales", title: "抵用券自付", sortable: false, sWidth: "100px" },
                { data: "Coupon1DiscountSales", title: "抵用券折让", sortable: false, sWidth: "100px" },
                { data: "OtherMoneySales", title: "其它总金额", sortable: false, sWidth: "100px" },
                { data: "OtherPaysSales", title: "其它自付", sortable: false, sWidth: "100px" },
                { data: "OtherDiscountSales", title: "其它折让", sortable: false, sWidth: "100px" }
                //{ data: "", title: "贷款金额", sortable: false },
                //{ data: "", title: "贷款是否按揭（月）", sortable: false },
                //{ data: "", title: "金融公司/银行", sortable: false },

            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales' });
                addSearch_TH(d);
            },
            fnInitComplete: function () {
                $("#dtSales_TH").parent("div[class='dataTables_scrollBody']").css({ "width": "1800px" });
                $("#dtSales_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHead']").css({ "width": "1800px" });
                $("#dtSales_TH").css({ "width": "100%" });
                $("#dtSales_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").css({ "width": "100%" });
                $("#dtSales_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").find("table").css({ "width": "100%" });
                $("#dtSales_TH").parents("div[class='dataTables_scroll']").css({ "width": "100%", "overflow-x": "auto" });
            }
        });
    }
    else {
        dtSales_TH.fnDraw();
    }
}
function showSalesDetail(btn, tid) {
    salesTradeId = tid;
    $(btn).parent().parent().css('background-color', '#EBEBEB');
    $(btn).parent().parent().siblings("tr").css('background-color', '');
    if (!tid) {
        $.dialog("参数错误，请刷新后重试");
    }
    $("#navSales_TH li.active a").click();
}
//售后订单
function getTabService_TH() {
    if (!dtService_TH) {
        dtService_TH = $('#dtService_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            bScrollCollapse: true,
            sScrollX: '1600px',
            aoColumns: [
                //{ data: "TradeCode", title: "结算单号", sortable: false },
                { data: "TradeCodeService", title: "委托书号", sortable: true, sWidth: "150px" },
                {
                    data: null, title: "操作", sortable: false, sWidth: "100px", render: function (obj) {
                        return '<button class="btn" onclick="showServiceDetail(this,' + obj.TradeID + ')">查看明细</button></td>';
                    }
                },
                { data: "CarNo", title: "车牌号", sortable: false, },
                { data: "VIN", title: "车架号", sortable: false, },
                { data: "EngineNo", title: "发动机号", sortable: false, },
                { data: "VehicleProperties", title: "车辆性质", sortable: false, },
                { data: "VihicleBrandName", title: "品牌", sortable: false, },
                { data: "VehicleSeriesNameService", title: "车系", sortable: false, },
                { data: "VehicleLevelNameService", title: "车型", sortable: false, },
                { data: "VehicleColorNameService", title: "车辆颜色", sortable: false, },
                //{ data: "DriveDistinct1", title: "表显里程", sortable: false, },
                //{ data: "DriveDistinct2", title: "换表里程", sortable: false, },
                //{ data: "DriveDistinct3", title: "总里程", sortable: false, },
                ////{ data: "", title: "是否换表", sortable: false },
                ////{ data: "", title: "三包", sortable: false },
                ////{
                ////    data: "MaintenanceStartTime", title: "保修开始日期", sortable: false, render: function (obj) {
                ////        return !obj ? "" : obj.substr(0, 10);
                ////    }
                ////},
                //{
                //    data: "LastInFactoryDate", title: "上次进厂日期", sortable: false, sWidth: "100px", render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                //{
                //    data: "NextMaintenanceDate", title: "下次保养日期", sortable: false, sWidth: "100px", render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                { data: "DriveDistinct4", title: "下次保养里程", sWidth: "100px", sortable: false },
                {
                    data: "TradeOpenDateService", title: "开单时间", sortable: false, sWidth: "100px", render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "TradeTypeService", title: "单据类型", sortable: false, sWidth: "100px" },
                { data: "RepairTypeService", title: "维修类别", sortable: false, sWidth: "100px" },
                { data: "ConSultantService", title: "服务顾问", sortable: false, sWidth: "100px" }
                //{ data: "RemarkService", title: "备注", sortable: false },

            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'service' });
                addSearch_TH(d);
            },
            fnInitComplete: function () {
                $("#dtService_TH").parent("div[class='dataTables_scrollBody']").css({ "width": "1800px" });
                $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHead']").css({ "width": "1800px" });
                $("#dtService_TH").css({ "width": "100%" });
                $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").css({ "width": "100%" });
                $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").find("table").css({ "width": "100%" });
                $("#dtService_TH").parents("div[class='dataTables_scroll']").css({ "width": "100%", "overflow-x": "auto" });
            }
        });
    }
    else {
        dtService_TH.fnDraw();
    }
}
function showServiceDetail(btn, tid) {
    serviceTradeId = tid;
    $(btn).parent().parent().css('background-color', '#EBEBEB');
    $(btn).parent().parent().siblings("tr").css('background-color', '');
    if (!tid) {
        $.dialog("参数错误，请刷新后重试");
    }
    $("#navService_TH li.active a").click();

}
//购车订单-装潢
function getTabSales_dress_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_dress_TH) {
        dtSales_dress_TH = $('#dtSales_dress_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "NameDress", title: "装潢名称", sortable: false },
                { data: "UnitPriceDress", title: "装潢单价", sortable: false },
                { data: "AmountDress", title: "装潢数量", sortable: false },
                { data: "PriceDress", title: "装潢价格", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_dress' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_dress_TH.fnDraw();
    }
}
//购车订单-保险
function getTabSales_insurance_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_insurance_TH) {
        dtSales_insurance_TH = $('#dtSales_insurance_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "CodeInsurance", title: "保单号", sortable: false },
                { data: "TypeInsurance", title: "险种", sortable: false },
                { data: "CompanyNameInsurance", title: "保险公司名称", sortable: false },
                { data: "MoneyInsurance", title: "保额", sortable: false },
                { data: "TimesInsurance", title: "期数", sortable: false },
                {
                    data: "StartTimeInsurance", title: "开始时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: "EndTimeInsurance", title: "结束时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "RemarkInsurance", title: "备注", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_insurance' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_insurance_TH.fnDraw();
    }
}
//购车订单-附件
function getTabSales_pack_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_pack_TH) {
        dtSales_pack_TH = $('#dtSales_pack_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "NamePack", title: "附件名称", sortable: false },
                { data: "UnitPricePack", title: "附件单价", sortable: false },
                { data: "AmountPack", title: "附件数量", sortable: false },
                { data: "PricePack", title: "附件价格", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_pack' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_pack_TH.fnDraw();
    }
}
//购车订单-油卡
function getTabSales_oilcard_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_oilcard_TH) {
        dtSales_oilcard_TH = $('#dtSales_oilcard_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "NameOilcard", title: "油卡名称", sortable: false },
                { data: "MoneyOilcard", title: "油卡金额", sortable: false },
                { data: "PersonNameOilcard", title: "受益人姓名", sortable: false },
                { data: "PersonIdentifyOilcard", title: "受益人证件号码", sortable: false },
                { data: "PersonMobileOilcard", title: "受益人电话号码", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_oilcard' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_oilcard_TH.fnDraw();
    }
}
//购车订单-抵用券
function getTabSales_coupon1_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_coupon1_TH) {
        dtSales_coupon1_TH = $('#dtSales_coupon1_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "NameCoupon1", title: "券种类", sortable: false },
                { data: "UnitPriceCoupon1", title: "券单价", sortable: false },
                { data: "AmountCoupon1", title: "券数量", sortable: false },
                { data: "MoneyCoupon1", title: "券金额", sortable: false },
                { data: "PersonNameCoupon1", title: "受益人姓名", sortable: false },
                { data: "PersonIdentifyCoupon1", title: "受益人证件号码", sortable: false },
                { data: "PersonMobileCoupon1", title: "受益人电话号码", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_coupon1' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_coupon1_TH.fnDraw();
    }
}
//购车订单-服务卡
function getTabSales_servercard_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_servercard_TH) {
        dtSales_servercard_TH = $('#dtSales_servercard_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "NameServercard", title: "卡种类", sortable: false },
                { data: "MoneyServercard", title: "卡金额", sortable: false },
                { data: "PersonNameServercard", title: "受益人姓名", sortable: false },
                { data: "PersonIdentifyServercard", title: "受益人证件号码", sortable: false },
                { data: "PersonMobileServercard", title: "受益人电话号码", sortable: false },
                //{
                //    data: "", title: "生效期", sortable: false, render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                //{
                //    data: "", title: "到期日期", sortable: false, render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //}
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_servercard' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_servercard_TH.fnDraw();
    }
}
//购车订单-套餐
function getTabSales_package_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_package_TH) {
        dtSales_package_TH = $('#dtSales_package_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "PackageIdSales", title: "套餐编号", sortable: false },
                { data: "PackageAmountSales", title: "套餐使用数量", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_package' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_package_TH.fnDraw();
    }
}
//购车订单-优惠券
function getTabSales_coupon2_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_coupon2_TH) {
        dtSales_coupon2_TH = $('#dtSales_coupon2_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "IDCoupon2", title: "优惠券编号", sortable: false },
                { data: "CouponAmountSales", title: "优惠券使用数量", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_coupon2' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_coupon2_TH.fnDraw();
    }
}
//购车订单-结算单
function getTabSales_result_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_result_TH) {
        dtSales_result_TH = $('#dtSales_result_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "InvoiceMoneySalesRt", title: "开票价格", sortable: false },
                { data: "DiscountMoneySalesRt", title: "车价低开", sortable: false },
                { data: "PayableMoneySalesRt", title: "应收总金额", sortable: false },
                { data: "PaymentMoneySalesRt", title: "实收总金额", sortable: false },
                { data: "StatementStatusSalesRt", title: "结算单状态", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_result' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_result_TH.fnDraw();
    }
}
//购车订单-选装
function getTabSales_config_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_config_TH) {
        dtSales_config_TH = $('#dtSales_config_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "ConfigName1", title: "选装名称1", sortable: false },
                { data: "ConfigName2", title: "选装名称2", sortable: false },
                { data: "ConfigName3", title: "选装名称3", sortable: false },
                { data: "ConfigName4", title: "选装名称4", sortable: false },
                { data: "ConfigName5", title: "选装名称5", sortable: false },
                { data: "ConfigName6", title: "选装名称6", sortable: false },
                { data: "ConfigName7", title: "选装名称7", sortable: false },
                { data: "ConfigName8", title: "选装名称8", sortable: false },
                { data: "ConfigName9", title: "选装名称9", sortable: false },
                { data: "ConfigName10", title: "选装名称10", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_config' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_config_TH.fnDraw();
    }
}
//购车订单-其它
function getTabSales_others_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtSales_others_TH) {
        dtSales_others_TH = $('#dtSales_others_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "NameOthers", title: "种类", sortable: false },
                { data: "UnitPriceOthers", title: "单价", sortable: false },
                { data: "AmountOthers", title: "数量", sortable: false },
                { data: "MoneyOthers", title: "金额", sortable: false },
                { data: "PersonNameOthers", title: "受益人姓名", sortable: false },
                { data: "PersonIdentifyOthers", title: "受益人证件号码", sortable: false },
                { data: "PersonMobileOthers", title: "受益人电话号码", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_others' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtSales_others_TH.fnDraw();
    }
}
//售后-维修项目
function getTabService_repairitem_TH() {
    if (!serviceTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtService_repairitem_TH) {
        dtService_repairitem_TH = $('#dtService_repairitem_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "RepairItemCodeRepairitem", title: "项目代码", sortable: false },
                { data: "RepairItemNameRepairitem", title: "项目名称", sortable: false },
                { data: "LaborHourRepairitem", title: "标准工时", sortable: false },
                { data: "LaborHourARepairitem", title: "派工工时", sortable: false },
                { data: "LaborCostRepairitem", title: "工时费", sortable: false },
                { data: "ChargeTypeRepairitem", title: "收费区分", sortable: false },
                { data: "WorkModeRepairitem", title: "工种", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'service_repairitem' });
                d.push({ name: 'tid', value: serviceTradeId });
            }
        });
    }
    else {
        dtService_repairitem_TH.fnDraw();
    }
}
//售后-维修销售材料
function getTabService_salesmaterial_TH() {
    if (!serviceTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtService_salesmaterial_TH) {
        dtService_salesmaterial_TH = $('#dtService_salesmaterial_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "MaterialTypeMaterial", title: "材料类型", sortable: false },
                { data: "PartFormat", title: "配附件格式", sortable: false },
                { data: "PartName", title: "配件名称", sortable: false },
                { data: "PartCode", title: "配件代码", sortable: false },
                { data: "PinYinCode", title: "拼音代码", sortable: false },
                { data: "MaterialPriceType", title: "价格类型", sortable: false },
                { data: "ChargeType", title: "收费区分", sortable: false },
                { data: "ReserveMaterial", title: "补差材料", sortable: false },
                { data: "SalesPriceMaterial", title: "销售价", sortable: false },
                { data: "CostPriceMaterial", title: "成本价", sortable: false },
                { data: "AverageCostMaterial", title: "移动加权平均成本价", sortable: false },
                { data: "NewCostMaterial", title: "最新进价", sortable: false },
                { data: "MaterialUnitPrice", title: "单价", sortable: false },
                { data: "PriceRatio", title: "价格系数", sortable: false },
                { data: "MaterialAmount", title: "数量", sortable: false },
                { data: "ReserveMaterialPrice", title: "补差材料销售价", sortable: false },
                { data: "ReserveMoney", title: "补差金额", sortable: false },
                { data: "DiscountRateMaterial", title: "折扣率", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'service_salesmaterial' });
                d.push({ name: 'tid', value: serviceTradeId });
            }
        });
    }
    else {
        dtService_salesmaterial_TH.fnDraw();
    }
}
//售后-附加项目
function getTabService_addition_TH() {
    if (!serviceTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtService_addition_TH) {
        dtService_addition_TH = $('#dtService_addition_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                //{ data: "AdditionItemCodeAddition", title: "附加项目编号", sortable: false },
                { data: "AdditionItemNameAddition", title: "附加项目名称", sortable: false },
                { data: "MoneyAddition", title: "金额", sortable: false },
                { data: "ChargeTypeAddition", title: "收费区分", sortable: false },
                { data: "DiscountRateAddition", title: "折扣率", sortable: false },
                { data: "RemarkAddition", title: "备注", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'service_addition' });
                d.push({ name: 'tid', value: serviceTradeId });
            }
        });
    }
    else {
        dtService_addition_TH.fnDraw();
    }
}
//售后-优惠券
function getTabService_coupon_TH() {
    if (!serviceTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtService_coupon_TH) {
        dtService_coupon_TH = $('#dtService_coupon_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "CouponIdService", title: "优惠券编号", sortable: false },
                { data: "CouponAmountService", title: "优惠券使用数量", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'service_coupon' });
                d.push({ name: 'tid', value: serviceTradeId });
            }
        });
    }
    else {
        dtService_coupon_TH.fnDraw();
    }
}
//售后-套餐
function getTabService_package_TH() {
    if (!serviceTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtService_package_TH) {
        dtService_package_TH = $('#dtService_package_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "PackageIdService", title: "套餐编号", sortable: false },
                { data: "PackageAmountService", title: "套餐使用数量", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'service_package' });
                d.push({ name: 'tid', value: serviceTradeId });
            }
        });
    }
    else {
        dtService_package_TH.fnDraw();
    }
}
//售后-结算单
function getTabService_result_TH() {
    if (!serviceTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtService_result_TH) {
        dtService_result_TH = $('#dtService_result_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                //{ data: "TradeCode", title: "流转单号", sortable: false },
                { data: "PayNameServiceRt", title: "付费方名称", sortable: false },
                { data: "LaborHourMoneyServiceRt", title: "实收工时费", sortable: false },
                { data: "RepairMaterialMoneyServiceRt", title: "实收维修材料费", sortable: false },
                { data: "SalesMaterialMoneyServiceRt", title: "实收销售材料费", sortable: false },
                { data: "AdditionItemMoneyServiceRt", title: "实收附加项目费", sortable: false },
                { data: "TaxMoneyServiceRt", title: "税额", sortable: false },
                { data: "LaborCouponMoneyServiceRt", title: "工时抵用券", sortable: false },
                { data: "PartCouponMoneyServiceRt", title: "零件抵用券", sortable: false },
                { data: "StoreIntergralMoneyServiceRt", title: "门店积分金额", sortable: false },
                { data: "StoreIntegralServiceRt", title: "门店积分", sortable: false },
                { data: "PayableMoneyServiceRt", title: "应收账款", sortable: false },
                { data: "RemoveOddMoneyServiceRt", title: "去零", sortable: false },
                { data: "DiscountMoneyServiceRt", title: "减免汇总金额", sortable: false },
                { data: "ActPayableMoneyServiceRt", title: "实际应收账款", sortable: false },
                { data: "StatementStatusServiceRt", title: "结算单状态", sortable: false },
                { data: "InvoiceTypeService", title: "开票状态", sortable: false },
                { data: "InvoiceMoneyServiceRt", title: "已开票金额", sortable: false },
                { data: "PaymentMoneyServiceRt", title: "已收款金额", sortable: false },
                { data: "OweMoneyService", title: "赊款金额", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'service_result' });
                d.push({ name: 'tid', value: serviceTradeId });
            }
        });
    }
    else {
        dtService_result_TH.fnDraw();
    }
}
//权益明细
function getTabServiceRights() {
    if (!serviceTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtServiceRights) {
        dtServiceRights = $('#dtServiceRights').dataTable({
            sAjaxSource: '/Member360/GetTradeRightsDetail',//todo
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "RightsName", title: "权益名称", sortable: false },
                //{ data: "RightsCode", title: "权益编号", sortable: false },
                { data: "RightsType", title: "权益类型", sortable: false },
                { data: "RightsSubType", title: "权益子类型", sortable: false },
                { data: "RightsTotalValue", title: "抵用总金额", sortable: false },
                { data: "RightsHourValue", title: "抵用工时费", sortable: false },
                { data: "RightsSalesValue", title: "抵用材料费", sortable: false },
                { data: "RightsOtherValue", title: "抵用其他", sortable: false },
                { data: "RightsCount", title: "权益数量", sortable: false }

            ],
            fnFixData: function (d) {
                d.push({ name: 'tradeId', value: serviceTradeId });
            }
        });
    }
    else {
        dtServiceRights.fnDraw();
    }

}

//售前-开票
function getTabSales_invioce_TH() {
    if (!salesTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtService_invoice_TH) {
        dtService_invoice_TH = $('#dtSales_invoice_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "InvoiceNo", title: "发票号码", sortable: false },
                { data: "InvoiceStatus", title: "发票状态", sortable: false },
                { data: "InvoiceMoney", title: "发票金额", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'sales_invioce' });
                d.push({ name: 'tid', value: salesTradeId });
            }
        });
    }
    else {
        dtService_invoice_TH.fnDraw();
    }
}
//添加交易历史搜索条件
function addSearch_TH(d) {
    d.push({ name: 'mid', value: $("#hdnMemberId").val() });
    d.push({ name: 'tradeCode', value: $("#txtTradeCode_TH").val() });
    d.push({ name: 'start', value: $("#txtStart_TH").val() });
    d.push({ name: 'end', value: $("#txtEnd_TH").val() });
    d.push({ name: 'store', value: $("#drpStore_TH").val() });
    d.push({ name: 'corp', value: $("#drpCorp_TH").val() });
}

//交易历史-充值
function getTabRecharge_TH() {
    if (!dtRecharge_TH) {
        dtRecharge_TH = $('#dtRecharge_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            bScrollCollapse: true,
            //sScrollX: '1500px',
            aoColumns: [
                //{ data: "TradeCode", title: "交易编号", sortable: true, sWidth: "100px" },
                { data: "AccountType", title: "账户类型", sortable: true, sWidth: "150px" },
                //{
                //    data: null, title: "操作", sortable: false, sWidth: "100px", render: function (obj) {
                //        return '<button class="btn" onclick="showRecharge_THDetail(this,' + obj.TradeID + ')">查看明细</button></td>';
                //    }
                //},
                { data: "StoreNameRechargeTrade", title: "充值门店", sortable: false, sWidth: "200px" },
                { data: "SaleMan", title: "操作人", sortable: false, sWidth: "200px" },
                { data: "ChargeValueTrade", title: "充值金额", sortable: false, sWidth: "100px" },
                //{
                //    data: "IsInvoiceCharge", title: "是否开票", sortable: false, sWidth: "100px", render: function (obj) {
                //        return obj == true ? "是" : "否";
                //    }
                //},
                {
                    data: "RechargeTimeTrade", title: "充值时间", sortable: false, sWidth: "100px", render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'recharge' });
                addSearch_TH(d);
            },
            //fnInitComplete: function () {
            //    $("#dtService_TH").parent("div[class='dataTables_scrollBody']").css({ "width": "1800px" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHead']").css({ "width": "1800px" });
            //    $("#dtService_TH").css({ "width": "100%" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").css({ "width": "100%" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").find("table").css({ "width": "100%" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").css({ "width": "100%", "overflow-x": "auto" });
            //}
        });
    }
    else {
        dtRecharge_TH.fnDraw();
    }
}
function showRecharge_THDetail(btn, tid) {
    rechargeTradeId = tid;
    $(btn).parent().parent().css('background-color', '#EBEBEB');
    $(btn).parent().parent().siblings("tr").css('background-color', '');
    if (!tid) {
        $.dialog("参数错误，请刷新后重试");
    }
    $("#navRecharge_TH li.active a").click();
}
//交易历史-充值-开票
function getTabRecharge_invoice_TH() {
    if (!rechargeTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtRecharge_invoice_TH) {
        dtRecharge_invoice_TH = $('#dtRecharge_invoice_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "InvoiceNoCharge", title: "发票号码", sortable: false },
                { data: "InvoiceMoneyCharge", title: "发票金额", sortable: false },
                {
                    data: "IsRedInvoiceRecharge", title: "是否红冲", sortable: false, render: function (obj) {
                        return obj == true ? "是" : "否";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'recharge_invoice' });
                d.push({ name: 'tid', value: rechargeTradeId });
            }
        });
    }
    else {
        dtRecharge_invoice_TH.fnDraw();
    }
}

//交易历史-套餐
function getTabPackage_TH() {
    if (!dtPackage_TH) {
        dtPackage_TH = $('#dtPackage_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            bScrollCollapse: true,
            //sScrollX: '1500px',
            aoColumns: [
                //{ data: "PackageName", title: "套餐名称", sortable: true, sWidth: "100px" },
                //{ data: "ItemName", title: "条目名称", sortable: true, sWidth: "100px" },
                //{ data: "ChangeValue", title: "操作数量", sortable: true, sWidth: "100px" },
                //{ data: "ChangeReason", title: "操作方式", sortable: true, sWidth: "100px" },
                //{
                //    data: "AddedDate", title: "操作时间", sortable: false, sWidth: "100px", render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                //{ data: "AddedUser", title: "操作人", sortable: true, sWidth: "100px" },

                //{ data: "StoreNamePackageTrade", title: "购买门店", sortable: true, sWidth: "200px" },
                { data: "PackageDecTrade", title: "操作方式", sortable: true, sWidth: "100px" },
                { data: "PackageTotalPriceTrade", title: "总金额", sortable: true, sWidth: "100px" },
                {
                    data: "AddedDate", title: "操作时间", sortable: false, sWidth: "100px", render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "AddedUser", title: "操作人", sortable: true, sWidth: "100px" },
                {
                    data: null, title: "操作", sortable: false, sWidth: "100px", render: function (obj) {
                        return '<button class="btn" onclick="showPackage_THDetail(this,' + obj.TradeID + ')">查看明细</button></td>';
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'package1' });
                addSearch_TH(d);
            },
            //fnInitComplete: function () {
            //    $("#dtService_TH").parent("div[class='dataTables_scrollBody']").css({ "width": "1800px" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHead']").css({ "width": "1800px" });
            //    $("#dtService_TH").css({ "width": "100%" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").css({ "width": "100%" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").find("table").css({ "width": "100%" });
            //    $("#dtService_TH").parents("div[class='dataTables_scroll']").css({ "width": "100%", "overflow-x": "auto" });
            //}
        });
    }
    else {
        dtPackage_TH.fnDraw();
    }
}
function showPackage_THDetail(btn, tid) {
    packageTradeId = tid;
    $(btn).parent().parent().css('background-color', '#EBEBEB');
    $(btn).parent().parent().siblings("tr").css('background-color', '');
    if (!tid) {
        $.dialog("参数错误，请刷新后重试");
    }
    $("#navPackage_TH li.active a").click();

}
//交易历史-套餐-明细
function getTabPackage_detail_TH() {
    if (!packageTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtPackage_detail_TH) {
        dtPackage_detail_TH = $('#dtPackage_detail_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "PackageNameDetail", title: "套餐名称", sortable: false },
                { data: "PackageDecDetail", title: "套餐描述", sortable: false },
                { data: "PackagePriceDetail", title: "购买价格", sortable: false },
                { data: "PackageQtyDetail", title: "数量", sortable: false },
                {
                    data: "PackageEndDateDetail", title: "有效截止日期", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'package_detail' });
                d.push({ name: 'tid', value: packageTradeId });
            }
        });
    }
    else {
        dtPackage_detail_TH.fnDraw();
    }
}
//交易历史-套餐-开票
function getTabPackage_invoice_TH() {
    if (!packageTradeId) {
        $.dialog("请先选中一条交易记录");
        return;
    }
    if (!dtPackage_invoice_TH) {
        dtPackage_invoice_TH = $('#dtPackage_invoice_TH').dataTable({
            sAjaxSource: '/Member360/GetTradeHistoryDetail',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "InvoiceNoPackage", title: "发票号码", sortable: false },
                { data: "InvoiceMoneyPackage", title: "发票金额", sortable: false },
                {
                    data: "IsRedInvoicePackage", title: "是否红冲", sortable: false, render: function (obj) {
                        return obj == true ? "是" : "否";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'tabName', value: 'package_invioce' });
                d.push({ name: 'tid', value: packageTradeId });
            }
        });
    }
    else {
        dtPackage_invoice_TH.fnDraw();
    }
}

//加载套餐信息
function loadPackages() {
    if (!dtPackage) {
        dtPackage = $("#dtPackage").dataTable({
            sAjaxSource: '/Member360/GetMemPackages',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "PackageName", title: "套餐名称", sortable: false },
                { data: "PurchasePrice1", title: "套餐价格(元)", sortable: false },
                { data: "PurchasePrice2", title: "购买价格", sortable: false },
                { data: "AccountLimit", title: "限制条件", sortable: false },
                {
                    data: "StartDate", title: "生效日期", sortable: true, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: "EndDate", title: "到期日期", sortable: true, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: "AddedDate", title: "购买时间", sortable: true,
                },
                {
                    data: "UserName", title: "操作员工", sortable: true,
                },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        return '<button class="btn" onclick="showPackageDetail(this,' + obj.PackageInstanceID + ')">查看明细</button></td>';
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'name', value: $("#txtName_Package").val() });
                d.push({ name: 'start', value: $("#txtStart_Package").val() });
                d.push({ name: 'end', value: $("#txtEnd_Package").val() });
                d.push({ name: 'valid', value: $("#cbValid_Package")[0].checked });
            }
        });
    }
    else {
        dtPackage.fnDraw();
    }
}
function showPackageDetail(e, piid) {
    $(e).parent().parent().css('background-color', '#EBEBEB');
    $(e).parent().parent().siblings("tr").css('background-color', '');
    //$(e).parent().parent().remove();
    if (!piid) {
        $.dialog("参数错误，请刷新后重试");
    }
    $("#navPackage li a").click(function () {
        var tabId = $(this).attr("href");
        if (tabId == "#tabPackageDetail_P") {
            GetPackageDetail(piid);
        }
        else if (tabId == "#tabPackageLimit_P") {
            GetPackageLimit(piid);
        }
    });
    GetPackageDetail(piid);
}
function GetPackageDetail(piid) {

    $("#hdnPiid").val(piid);
    if (!dtPackageDetail_P) {
        dtPackageDetail_P = $("#dtPackageDetail_P").dataTable({
            sAjaxSource: '/Member360/GetPackageDetailsById',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "ItemID", title: "条码", sortable: false },
                { data: "ItemName", title: "条目名称", sortable: false },
                { data: "OrgQty", title: "初始数量", sortable: true },
                { data: "Qty", title: "可用数量", sortable: true },
                //{
                //    data: "StartDate", title: "生效日期", sortable: true, render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                //{
                //    data: "EndDate", title: "到期日期", sortable: true, render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                //{
                //    data: null, title: "操作", sortable: false, render: function (obj) {
                //        return '<button class="btn" onclick="showPackageHistory(' + obj.PackageInstanceDetailID + ')">查看历史</button></td>';
                //    }
                //}
            ],
            fnFixData: function (d) {
                d.push({ name: 'piid', value: $("#hdnPiid").val() });
            }
        });
    }
    else {
        dtPackageDetail_P.fnDraw();
    }
}
//套餐使用历史
function showPackageHistory(piid) {
    if (!dtPackageHistory) {
        dtPackageHistory = $('#dtPackageHistory').dataTable({
            sAjaxSource: '/Member360/GetMemPackageHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "ItemName", title: "明细名称", sortable: false },
                //{ data: "ChangeLevelFrom", title: "使用门店", sortable: false },
                { data: "ChangeValue", title: "使用数量", sortable: false },
                { data: "AddedDate", title: "使用时间", sortable: false },
                { data: "ChangeReason", title: "备注", sortable: false },
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'piid', value: piid });
            }
        });
    }
    else {
        dtPackageHistory.fnDraw();
    }
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#packageUsedHistory",
        inline: true
    });
}
function GetPackageLimit(piid) {
    if (!dtPackageLimit_P) {
        dtPackageLimit_P = $("#dtPackageLimit_P").dataTable({
            sAjaxSource: '/Member360/GetPackageLimitsById',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "LimitType", title: "限制类型", sortable: false },
                { data: "LimitValue", title: "限制值", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'piid', value: piid });
            }
        });
    }
    else {
        dtPackageLimit_P.fnDraw();
    }
}

//加载联系人信息
function GetTabContactsInfo() {
    if (!dtContactsInfo) {
        dtContactsInfo = $("#dtContactsInfo").dataTable({
            sAjaxSource: '/Member360/GetMemContacts',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            bScrollCollapse: true,
            sScrollX: "1200px",
            aoColumns: [
                { data: "ContactName", title: "姓名", sWidth: "100px", sortable: true },
                {
                    data: null, title: "操作", sortable: false, sWidth: "100px", render: function (obj) {
                        return '<Linkbutton class="btn" onclick="showContactDetail(this,' + obj.MemberSubExtID + ')">查看明细</button></td>';
                    }
                },
                { data: "ContactGender", title: "性别", sortable: false, sWidth: "50px" },
                {
                    data: null, title: "联系人类型", sortable: false, sWidth: "100px", render: function (obj) {
                        return obj.ContactType == 0 ? "车主联系人" : obj.ContactType == 1 ? "送修人联系人" : ""
                    }
                },
                { data: "ContactCertificateType", title: "证件类型", sortable: false, sWidth: "80px" },
                { data: "ContactIdNo", title: "证件号码", sortable: false, sWidth: "150px" },
                { data: "ContactTel", title: "电话", sortable: false, sWidth: "50px" },
                { data: "ContactMobile", title: "手机", sortable: false, sWidth: "50px" },
                { data: "ContactEmail", title: "Email", sortable: false, sWidth: "100px" },
                { data: "ContactProvinceName", title: "省份", sortable: false, sWidth: "100px" },
                { data: "ContactCityName", title: "城市", sortable: false, sWidth: "100px" },
                { data: "ContactDistrictName", title: "区县", sortable: false, sWidth: "100px" },
                { data: "ContactZip", title: "邮编", sortable: false, sWidth: "100px" },
                { data: "ContactAddress", title: "邮寄地址", sortable: false, sWidth: "200px" },
                { data: "ContactJob", title: "职业", sortable: false, sWidth: "100px" },
                { data: "ContactPosition", title: "职务", sortable: false, sWidth: "100px" },
                { data: "LovingCommunication", title: "喜欢的联络方式", sortable: false, sWidth: "100px" },
                {
                    data: null, title: "最佳联系时间开始", sortable: false, sWidth: "120px", render: function (obj) {
                        return utility.isNull(obj.BestContactTime1) ? "" : obj.BestContactTime1.substr(0, 8);
                    }
                },
                {
                    data: null, title: "最佳联系时间结束", sortable: false, sWidth: "120px", render: function (obj) {
                        return utility.isNull(obj.BestContactTime2) ? "" : obj.BestContactTime2.substr(0, 8);
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
            },
            fnInitComplete: function () {
                $("#dtContactsInfo").parent("div[class='dataTables_scrollBody']").css({ "width": "2500px" });
                $("#dtContactsInfo").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHead']").css({ "width": "2500px" });
                $("#dtContactsInfo").css({ "width": "100%" });
                $("#dtContactsInfo").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").css({ "width": "100%" });
                $("#dtContactsInfo").parents("div[class='dataTables_scroll']").find("div[class='dataTables_scrollHeadInner']").find("table").css({ "width": "100%" });
                $("#dtContactsInfo").parents("div[class='dataTables_scroll']").css({ "width": "100%", "overflow-x": "auto" });
            }
        });
    }
    else {
        dtContactsInfo.fnDraw();
    }

}
function showContactDetail(e, cid) {
    clearFormData("dlgMemContactDetail");
    ajax("/Member360/GetMemContactDetail", { cid: cid }, function (data) {
        if (data.IsPass) {
            var info = data.Obj[0];
            $("#txtName_Contact").val(info.ContactName);
            $("#txtType_Contact").val(info.ContactType == 0 ? "车主联系人" : info.ContactType == 1 ? "送修人联系人" : "");
            $("#txtGender_Contact").val(info.ContactGender);
            $("#txtCertificateType_Contact").val(info.ContactCertificateType);
            $("#txtCertificateNo_Contact").val(info.ContactIdNo);
            $("#txtTel_Contact").val(info.ContactTel);
            $("#txtMobile_Contact").val(info.ContactMobile);
            $("#txtEmail_Contact").val(info.ContactEmail);
            $("#txtProvince_Contact").val(info.ContactProvinceName);
            $("#txtCity_Contact").val(info.ContactCityName);
            $("#txtDistrict_Contact").val(info.ContactDistrictName);
            $("#txtZip_Contact").val(info.ContactZip);
            $("#txtAddress_Contact").val(info.ContactAddress);
            $("#txtJob_Contact").val(info.ContactJob);
            $("#txtPosition_Contact").val(info.ContactPosition);
            $("#txtFavoriteType_Contact").val(info.LovingCommunication);
            $("#txtFavoriteStart_Contact").val(utility.isNull(info.BestContactTime1) ? "" : info.BestContactTime1.substr(0, 8));
            $("#txtFavoriteEnd_Contact").val(utility.isNull(info.BestContactTime1) ? "" : info.BestContactTime1.substr(0, 8));
        }
        else {
            $.dialog(data.MSG);
        }
    });
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#dlgMemContactDetail",
        inline: true
    });
}

//加载账户信息
//function getTabAccountInfo() {
//    ajax("/Member360/GetMemAccountInfo", { mid: $("#hdnMemberId").val() }, function (data) {
//        $("#stgValidValue1").text(0);
//        $("#stgValidValue3").text(0);
//        if (data.length > 0) {
//            for (var i in data) {
//                $("#spnTotalValue" + data[i].AccountType).html(data[i].TotalValue);
//                $("#spnValidValue" + data[i].AccountType).html(data[i].ValidValue);
//                $("#spnFrozenValue" + data[i].AccountType).html(data[i].FrozenValue);
//                $("#stgValidValue" + data[i].AccountType).html(data[i].ValidValue);
//                $("#btnAccountDetail" + data[i].AccountType).attr("disabled", false);
//            }
//        }
//    });
//}
function showAccountDetail(accType) {
    if (accType == "1" || accType == "2" || accType == "3" || accType == "4" || accType == "5" || accType == "6" || accType == "7") {
        ajax("/Member360/GetMemActDetails", { mid: $("#hdnMemberId").val(), accType: accType }, function (res) {

            var tbody = $("#dtAccountDetail tbody");
            tbody.empty();
            var data1 = res.Obj;
            if (data1 == null) {
                tbody.append('<tr class="odd"><td class="dataTables_empty" valign="top" colspan="6">没有记录</td></tr>');
            }
            else {
                var data = data1[0];
                if (data.length > 0) {
                    //var accountId = "", type = "", limittd = "", typetd = "", valuetd = "", limitCount = 0, typeCount = 0;
                    $('#txtActId').val(data[0].AccountID);
                    for (var i in data) {
                        //var tValue = data[i].AccountDetailType == "value1" ? data[i].Value1 : data[i].Value2;

                        var tr = $('<tr></tr>');
                        //if (data[i].AccountID != accountId) {
                        //    accountId = data[i].AccountID;
                        //    type = data[i].AccountDetailType;
                        //    limitCount = 1;
                        //    typeCount = 1;
                        //    limittd = $('<td valign="middle">' + data[i].AccountLimit + '</td>');
                        //    typetd = $('<td valign="middle">' + data[i].DetailTypeText + '</td>');
                        //    valuetd = $('<td valign="middle">' + tValue + '</td>');
                        //    tr.append(limittd).append(typetd).append(valuetd);
                        //}
                        //else {
                        //    limitCount += 1;
                        //    limittd.attr("rowspan", limitCount);
                        //    //tbody.find("tr[accountid=" + limittr + "] td")[0].attr("rowspan", limitCount);
                        //    if (data[i].AccountDetailType != type) {
                        //        type = data[i].AccountDetailType;
                        //        typeCount = 1;
                        //        typetd = $('<td valign="middle">' + data[i].DetailTypeText + '</td>');
                        //        valuetd = $('<td valign="middle">' + tValue + '</td>');
                        //        tr.append(typetd).append(valuetd);
                        //    }
                        //    else {
                        //        typeCount += 1;
                        //        typetd.attr("rowspan", typeCount);
                        //        valuetd.attr("rowspan", typeCount);
                        //        //$(typetr).find("td")[1].attr("rowspan", typeCount);
                        //        //$(typetr).find("td")[2].attr("rowspan", typeCount);
                        //    }
                        //}
                        //tr.append('<td>' + data[i].DetailValue + '</td>')
                        //    .append('<td>' + sDate + '</td>')
                        //    .append('<td>' + eDate + '</td>')
                        //    .append('<td><button class="btn" onclick="editAccountDetail(' + data[i].AccountDetailID + ')">编辑</button></td>').appendTo(tbody);
                        //if (data[i].DetailTypeText == '可用') {
                        //    var eDate = !data[i].SpecialDate1 ? "" : data[i].SpecialDate1.substr(0, 10);
                        //    var sDate = !data[i].SpecialDate2 ? "" : data[i].SpecialDate2.substr(0, 10);
                        //    tr.append('<td>' + data[i].AccountLimit + '</td>')
                        //        .append('<td>' + data[i].DetailTypeText + '</td>')
                        //        .append('<td>' + data[i].DetailValue + '</td>')
                        //        .append('<td>' + sDate + '</td>')
                        //        .append('<td>' + eDate + '</td>').appendTo(tbody);
                        //        //.append('<td><button class="btn" onclick="editAccountDetail(' + data[i].AccountDetailID + ')">编辑</button></td>').appendTo(tbody);
                        //} else {
                        var sDate = !data[i].SpecialDate1 ? "" : data[i].SpecialDate1.substr(0, 10);
                        var eDate = !data[i].SpecialDate2 ? "" : data[i].SpecialDate2.substr(0, 10);
                        tr.append('<td>' + data[i].AccountLimit + '</td>')
                            .append('<td>' + data[i].DetailTypeText + '</td>')
                            .append('<td>' + data[i].DetailValue + '</td>')
                            .append('<td>' + sDate + '</td>')
                            .append('<td>' + eDate + '</td>').appendTo(tbody);
                        //.append('<td><button class="btn" onclick="editAccountDetail(' + data[i].AccountDetailID + ')">编辑</button></td>').appendTo(tbody);

                        //}
                    }
                }
            }
            //var tbody = $("#dtAccountDetail tbody");
            //tbody.empty();
            //var data1 = res.Obj;
            //if (data1 == null) {
            //    tbody.append('<tr class="odd"><td class="dataTables_empty" valign="top" colspan="6">没有记录</td></tr>');
            //}
            //else {
            //    var data = data1[0];
            //    //var accountId = "", type = "", limittd = "", typetd = "", valuetd = "", limitCount = 0, typeCount = 0;
            //    for (var i in data) {
            //        //var tValue = data[i].AccountDetailType == "value1" ? data[i].Value1 : data[i].Value2;
            //        var sDate = !data[i].SpecialDate1 ? "" : data[i].SpecialDate1.substr(0, 10);
            //        var eDate = !data[i].SpecialDate2 ? "" : data[i].SpecialDate2.substr(0, 10);
            //        var tr = $('<tr></tr>');
            //        //if (data[i].AccountID != accountId) {
            //        //    accountId = data[i].AccountID;
            //        //    type = data[i].AccountDetailType;
            //        //    limitCount = 1;
            //        //    typeCount = 1;
            //        //    limittd = $('<td valign="middle">' + data[i].AccountLimit + '</td>');
            //        //    typetd = $('<td valign="middle">' + data[i].DetailTypeText + '</td>');
            //        //    valuetd = $('<td valign="middle">' + tValue + '</td>');
            //        //    tr.append(limittd).append(typetd).append(valuetd);
            //        //}
            //        //else {
            //        //    limitCount += 1;
            //        //    limittd.attr("rowspan", limitCount);
            //        //    //tbody.find("tr[accountid=" + limittr + "] td")[0].attr("rowspan", limitCount);
            //        //    if (data[i].AccountDetailType != type) {
            //        //        type = data[i].AccountDetailType;
            //        //        typeCount = 1;
            //        //        typetd = $('<td valign="middle">' + data[i].DetailTypeText + '</td>');
            //        //        valuetd = $('<td valign="middle">' + tValue + '</td>');
            //        //        tr.append(typetd).append(valuetd);
            //        //    }
            //        //    else {
            //        //        typeCount += 1;
            //        //        typetd.attr("rowspan", typeCount);
            //        //        valuetd.attr("rowspan", typeCount);
            //        //        //$(typetr).find("td")[1].attr("rowspan", typeCount);
            //        //        //$(typetr).find("td")[2].attr("rowspan", typeCount);
            //        //    }
            //        //}
            //        //tr.append('<td>' + data[i].DetailValue + '</td>').append('<td>' + sDate + '</td>').append('<td><button class="btn" onclick="showAccountDetailHistory(' + data[i].AccountDetailID + ')">查看历史</button></td>').appendTo(tbody);
            //        tr.append('<td>' + data[i].AccountLimit + '</td>')
            //        .append('<td>' + data[i].DetailTypeText + '</td>')
            //        .append('<td>' + data[i].DetailValue + '</td>')
            //        .append('<td>' + sDate + '</td>')
            //        .append('<td>' + eDate + '</td>').appendTo(tbody);;
            //        //.append('<td><button class="btn" onclick="editAccountDetail(' + data[i].AccountDetailID + ')">编辑</button></td>').appendTo(tbody);

            //    }
            //}
        });
    }
    else {
        $.dialog("参数错误");
    }
    $("#accountDet1").show();
    $("#accountDet2").hide();
}
function showAccountHistory(accType) {
    //$("#dtAccountHistory tbody").html('');
    //$("#dtAccountHistory_info").html('暂无记录');
    //$("#dtAccountHistory_paginate").html('<ul><li class="prev disabled"><a href="#">上一页</a></li><li class="next disabled"><a href="#">下一页</a></li></ul>');
    $("#hidAccType").val(accType);
    if (!dtAccountHistory) {
        dtAccountHistory = $("#dtAccountHistory").dataTable({
            sAjaxSource: '/Member360/GetMemAccountChangeHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 7,
            aaSorting: [[0, "desc"]],
            aoColumns: [
                {
                    data: "AddedDate", title: "变更时间", sortable: false,
                    //render: function (obj) {
                    //    return !obj ? "" : obj.substr(0, 10);
                    //}
                },
                { data: "ChangeValue", title: "变更值", sortable: false },
                { data: "ChangeTypeText", title: "变更类型", sortable: false },
                { data: "ChangeReason", title: "变更原因", sortable: false },
                { data: "ReferenceNo", title: "委托书号", sortable: false },
                { data: "ChangeUserName", title: "变更者", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'accType', value: $("#hidAccType").val() });
            }
        });
    }
    else {
        dtAccountHistory.fnDraw();
    }
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#dlgAccountHistory",
        inline: true
    });
}
function showAccountDetailHistory(detailId) {
    if (!dtAccountDetailHistory) {
        dtAccountDetailHistory = $("#dtAccountDetailHistory").dataTable({
            sAjaxSource: '/Member360/GetMemAccountDetailChangeHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: "AddedDate", title: "变更时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "ChangeValue", title: "变更值", sortable: false },
                { data: "ChangeTypeText", title: "变更类型", sortable: false },
                { data: "ChangeReason", title: "变更原因", sortable: false },
                { data: "ReferenceNo", title: "关联单号", sortable: false },
                { data: "ChangeUserName", title: "变更者", sortable: false }
            ],
            fnFixData: function (d) {
                d.push({ name: 'detailId', value: detailId });
            }
        });
    }
    else {
        dtAccountDetailHistory.fnDraw();
    }
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#dlgAccountDetailHistory",
        inline: true
    });
}
function getAccountCoupon() {
    ajax("/Member360/GetMemAccountCoupon", { mid: $("#hdnMemberId").val() }, function (data) {
        $("#spnValidCoupon").text(data.Obj[0]);
        $("#spnUsedCoupon").text(data.Obj[1]);
        $("#spnExpiredCoupon").text(data.Obj[2]);
    });
}
function showAccountCouponList() {
    if (!dtAccountCouponList) {
        dtAccountCouponList = $("#dtAccountCouponList").dataTable({
            sAjaxSource: '/Member360/GetMemAccountCouponList',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: "CouponName", title: "优惠券名称", sortable: false
                },
                { data: "CouponTypeText", title: "优惠券类型", sortable: false },
                { data: "LimitText", title: "限制条件", sortable: false, sWidth: "20%" },

                {
                    data: "StartDate", title: "开始日期", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: "EndDate", title: "结束日期", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: "AddedDate", title: "添加时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: null, title: "是否可用", sortable: false, render: function (obj) {
                        var myDate = new Date();
                        var datenow = myDate.format(("yyyy-MM-dd hh:mm:ss"));
                        var b = utility.compareDate(obj.StartDate, datenow);

                        var c = utility.compareDate(datenow, obj.EndDate);

                        if (obj.Enable && !obj.IsUsed && b && c)
                            return "可用";
                        else return "不可用";
                    }
                },
                {
                    data: null, title: "是否过期", sortable: false, render: function (obj) {
                        var myDate = new Date();
                        var datenow = myDate.format(("yyyy-MM-dd hh:mm:ss"));
                        var b = utility.compareDate(obj.StartDate, datenow);

                        var c = utility.compareDate(datenow, obj.EndDate);

                        if (c)
                            return "未过期";
                        else return "过期";
                    }
                },
                {
                    data: "Enable", title: "是否有效", sortable: false, render: function (obj) {
                        return obj == true ? "有效" : "无效";
                    }
                },
                {
                    data: "IsUsed", title: "是否使用", sortable: false, render: function (obj) {
                        return obj == true ? "已使用" : "未使用";
                    }
                },
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
            }
        });
    }
    else {
        dtAccountCouponList.fnDraw();
    }
    $("#accountDet1").hide();
    $("#accountDet2").show();
}

//加载沟通历史
function getCommunicationHistory() {
    if (!dtSMS_CH) {
        dtSMS_CH = $("#dtSMS_CH").dataTable({
            sAjaxSource: '/Member360/GetMemberCommunicateHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: "OccurTime", title: "沟通时间", sortable: false, sWidth: "10%", render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "TempletCategoryText", title: "沟通类型", sortable: false, sWidth: "10%" },
                { data: "TempletName", title: "模板名称", sortable: false, sWidth: "20%" },
                { data: "ContentDesc", title: "沟通内容", sortable: false },
                { data: "StatusText", title: "状态", sortable: false, sWidth: "10%" },
                //{ data: "ReferenceNo", title: "关联活动", sortable: false },
                //{ data: "ChangeUserName", title: "活动有效期", sortable: false },
                {
                    data: "Direction", title: "方向", sortable: false, sWidth: "10%", render: function (obj) {
                        return obj == true ? "发送" : "接收";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'cType', value: "SMS" });
            }
        });
    }
    else {
        dtSMS_CH.fnDraw();
    }

    if (!dtEmail_CH) {
        dtEmail_CH = $("#dtEmail_CH").dataTable({
            sAjaxSource: '/Member360/GetMemberCommunicateHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: "OccurTime", title: "沟通时间", sortable: false, sWidth: "10%", render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "TempletCategoryText", title: "沟通类型", sortable: false, sWidth: "10%" },
                { data: "TempletName", title: "模板名称", sortable: false, sWidth: "20%" },
                { data: "ContentDesc", title: "沟通内容", sortable: false },
                { data: "StatusText", title: "状态", sortable: false, sWidth: "10%" },
                //{ data: "ReferenceNo", title: "关联活动", sortable: false },
                //{ data: "ChangeUserName", title: "活动有效期", sortable: false },
                {
                    data: "Direction", title: "方向", sortable: false, sWidth: "10%", render: function (obj) {
                        return obj == true ? "发送" : "接收";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'cType', value: "Email" });
            }
        });
    }
    else {
        dtEmail_CH.fnDraw();
    }


    if (!dtReserve_CH) {
        dtReserve_CH = $("#dtReserve_CH").dataTable({
            sAjaxSource: '/Member360/GetMemberCommunicateHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: "ReserveDate", title: "预约日期", sortable: false, sWidth: "8%", render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "ReserveTimeArea", title: "预约时段", sortable: false, sWidth: "9%" },
                {
                    data: "ReserveType", title: "预约类型", sortable: false, sWidth: "7%", render: function (obj) {
                        var type = "";
                        if (obj == "1") {
                            type = "试驾预约";
                        }
                        else if (obj == "2") {
                            type = "服务预约";
                        }
                        return type;
                    }
                },
                { data: "StoreCode", title: "预约门店", sortable: false },
                //{ data: "", title: "预约内容", sortable: false },
                {
                    data: "ReserveStatus", title: "预约状态", sortable: false, sWidth: "7%", render: function (obj) {
                        var status = "";
                        if (obj == "0") {
                            status = "待确认";
                        }
                        else if (obj == "1") {
                            status = "已确认";
                        }
                        else if (obj == "2") {
                            status = "已完成";
                        }
                        else if (obj == "-1") {
                            status = "已取消";
                        }
                        else if (obj == "-2") {
                            status = "过期未到";
                        }
                        return status;
                    }
                },
                { data: "CarBrandName", title: "车辆品牌", sortable: false, sWidth: "8%" },
                { data: "CarSeriesName", title: "车系 ", sortable: false, sWidth: "10%" },
                { data: "CarLevelName", title: "车型", sortable: false, sWidth: "15%" },
                { data: "ServiceType", title: "服务类型", sortable: false, sWidth: "7%" },
                //{ data: "ReferenceNo", title: "关联活动", sortable: false },
                //{ data: "ChangeUserName", title: "活动有效期", sortable: false },
                //{
                //    data: "Direction", title: "方向", sortable: false, sWidth: "10%", render: function (obj) {
                //        return obj == true ? "发送" : "接收";
                //    }
                //}
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'cType', value: "Reserve" });
            }
        });
    }
    else {
        dtReserve_CH.fnDraw();
    }
}

//加载会员卡变更历史
function getMemberCardHistory(cardNo) {
    $("#dtCard" + cardNo).dataTable({
        sAjaxSource: '/Member360/GetMemberCardChangeHistory',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[1, "desc"]],
        aoColumns: [
            { data: "ChangeTypeText", title: "状态", sortable: false },
            {
                data: "ChangeTime", title: "变更时间", sortable: true,
                render: function (obj) {
                    return !obj ? "" : obj.substr(0, 19);
                }
            },
            //{ data: "TempletName", title: "变更原因", sortable: false },
            //{ data: "ChangePlace", title: "变更门店", sortable: false },
            { data: "ChangeUserName", title: "变更者", sortable: false }
        ],
        fnFixData: function (d) {
            d.push({ name: 'cardNo', value: cardNo });
        }
    });
}

//清空表单数据
function clearFormData(formId) {
    var form = $("#" + formId);
    if (form) {
        form.find("input[type!=button]").val("");
        form.find("select").val("");
    }
}