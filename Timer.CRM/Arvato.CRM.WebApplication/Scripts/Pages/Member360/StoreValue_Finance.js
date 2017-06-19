var tableMemberInfo, tableVehcileInfo;
var dtStoreData;
var total, addvalue, memname, memcardno, memveno, memvinkey;//账户总金额,充值金额,会员名字,会员卡号,车牌号,车架号
var flag = 0;//打印标记
$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    LoadStoreList();
    LoadPayType();

    $("#txtStatus").val('1');
    //搜索会员
    $("#btnSearch").click(function () {
        //至少有一个条件才能搜索
        //if ($("#txtCardNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '' && $("#txtStatus").val() == '' && $("#txtStartDate").val() == '' && $("#txtEndDate").val() == '') {
        //    $.dialog("至少输入一个条件以供查询");
        //    return;
        //}
        if (checkStr($("#txtCardNo").val())) {
            $.dialog("会员卡号中不能包含特殊字符！");
            return;
        }
        if (checkStr($("#txtName").val())) {
            $.dialog("姓名中不能包含特殊字符！");
            return;
        }



        LoadStoreValueRecord()
    });

    $("#drpStoreLevel2").change(function () {
        var le = $("#drpStoreLevel2").val();
        if (le == "0") {
            $("#divCash2").removeClass("hide");
        } else {

            $("#divCash2").addClass("hide");
        }
    });

    //储值保存
    $("#btnSubmit").click(function () {
        var id = $("#hdnMemberId").val();
        if (id != '') {
            var changeValue = '';
            if ($("#drpStoreLevel").val() != "0") {
                changeValue = $("#drpStoreLevel").val();
            } else {
                changeValue = $("#txtValue").val();
            }
            if (changeValue == '') {
                $.dialog("请选择或者填写储值金额");
                return;
            }
            if (!utility.isMoney(changeValue)) {
                $.dialog("储值金额格式不正确");
                return;
            }
            if ($("#drpStore").val() == '') {
                $.dialog("请选择门店");
                return;
            }
            if ($("#spnCardStat").text() == '停用') {
                $.dialog("该会员已停用,请联系管理员！");
                return;
            }

            if ($("#drpStoreLevel").val() == "0" && $("#txtValue").val() >= 2000) {
                $.dialog("自定义充值金额不能大于2000");
                return;
            }

            addvalue = changeValue;
            //var startDate = $("#txtStartDate").val();
            //var date = utility.getCurrentDate();
            //if (startDate == '' || utility.compareDate(startDate, date)) {
            //    startDate = GetDateStr(1);
            //}
            //储值操作
            ajax('/Member360/SaveStoreValue', { mid: id, changeValue: changeValue, storeCode: $("#drpStore").val() }, function (res) {
                if (res.IsPass) {
                    flag = 1;
                    loadCashPoint(id);
                    LoadStoreValueRecord();
                    $("#txtValue").val('');
                    $('#drpStore').val('');
                    $.dialog(res.MSG);
                } else {
                    $.dialog(res.MSG);
                }
            });
        } else {
            $.dialog("请先选择会员");
        }
    });
    $("#drpStoreLevel2").change(function () {
        if ($("#drpStoreLevel2").val() != "0") {
            changeValue = $("#drpStoreLevel2").val();
            $("#divCash2").addClass("hide");
        } else {
            changeValue = $("#txtValue2").val();
            $("#divCash2").removeClass("hide");
        }
        $("#txtInvoice").val(changeValue);
    })
    //储值修改
    $("#btnSubmit2").click(function () {
        var id = $("#hdnTradeID").val();
        var isChecked = $("#isStore")[0].checked;
        if (id != '') {
            var changeValue = '';
            if ($("#drpStoreLevel2").val() != "0") {
                changeValue = $("#drpStoreLevel2").val();
            } else {
                changeValue = $("#txtValue2").val();
            }
            if (changeValue == '') {
                $.dialog("请选择或者填写储值金额");
                return;
            }
            if (!utility.isMoney(changeValue)) {
                $.dialog("储值金额格式不正确");
                return;
            }
            if ($("#drpStore2").val() == '') {
                $.dialog("请选择门店");
                return;
            }
            addvalue = changeValue;
            if ($("#txtDate2").val() == '') {
                $.dialog("请选择储值日期");
                return;
            }
            if ($("#txtInvoice").val() == '') {
                $.dialog("请填写开票金额");
                return;
            }
            //if ($("#txtInvoice").val() - changeValue!= 0) {
            //    $.dialog("开票金额和储值金额不一致");
            //}
            if ($("#dropPayType").val() == '') {
                $.dialog("请选择支付方式");
                return;
            }
            //储值操作
            ajax('/Member360/SaveEditValue', { mid: id, changeValue: changeValue, storeCode: $("#drpStore2").val(), date: $("#txtDate2").val(), invoice: $("#txtInvoice").val(), payment: $("#dropPayType").val(),isStore:isChecked }, function (res) {
                if (res.IsPass) {
                    flag = 1;
                    LoadStoreValueRecord();
                    if ($("#txtInvoice").val() - changeValue != 0) {
                        $.dialog("修改成功,开票金额和储值金额不一致");
                    }
                    $.colorbox.close();
                } else {
                    $.dialog(res.MSG);
                    $.colorbox.close();
                }
            });
        }
    });
});

//加载会员信息
function loadMemInfoList() {
    if (!tableMemberInfo) {
        tableMemberInfo = $('#tableMemberInfo').dataTable({
            sAjaxSource: '/Member360/GetMembersNameByPage',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 4,
            aoColumns: [
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerName", title: "会员名称", sortable: false },

                { data: "CustomerMobile", title: "手机", sortable: false },
                //{ data: "CustomerStatusText", title: "会员状态", sortable: false },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="detail(\'' + obj.MemberID + '\')">查看</button> ';
                        return str;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'cardNo', value: $("#txtCardNo").val() });
                d.push({ name: 'memName', value: $("#txtName").val() });
                d.push({ name: 'memMobile', value: $("#txtMobile").val() });

                //d.push({ name: 'vehicleNo', value: $("#txtVehicle").val() });
                //d.push({ name: 'vehicleVIN', value: $("#txtVIN").val() });
            }
        });
    }
    else {
        tableMemberInfo.fnDraw();
    }
}


function LoadStoreValueRecord() {
    if (!dtStoreData) {
        dtStoreData = $('#dt_StoreData').dataTable({
            sAjaxSource: '/Member360/GetStoreValueRecord_Finance',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aaSorting: [[3, "asc"]],
            aoColumns: [
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "AmountRecharge", title: "储值金额", sortable: false },
                //{ data: "PlateNumVehicle", title: "车牌号", sortable: false },
                { data: "RechargeDate", title: "储值时间", sortable: true },
                { data: "StatusSalesText", title: "审批状态", sortable: true },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        if (obj.StatusRecharge == "1")
                            return "<button class=\"btn\" onclick=\"vehcile('" + obj.MemberID + "')\">查看车辆</button><button class=\"btn btn-default\" onclick=\"Edit(" + obj.TradeID + ")\">编辑</button>&nbsp;&nbsp;<button class=\"btn btn-danger\" onclick=\"Inactive(" + obj.TradeID + ")\">审批</button>";
                        else
                            return "<button class=\"btn\" onclick=\"vehcile('" + obj.MemberID + "')\">查看车辆</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'cardNo', value: $("#txtCardNo").val() });
                d.push({ name: 'name', value: $("#txtName").val() });
                d.push({ name: 'mobile', value: $("#txtMobile").val() });
                d.push({ name: 'status', value: $("#txtStatus").val() });
                d.push({ name: 'startDate', value: $("#txtStartDate").val() });
                d.push({ name: 'endDate', value: $("#txtEndDate").val() });
            }
        });
    }
    else {
        dtStoreData.fnDraw();
    }
}
//查看车辆
function vehcile(id) {

    $("#txtHdnMid").val(id);
    loadVehcileList();

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_Vehcile",
        inline: true
    });
}
function loadVehcileList() {
    if (!tableVehcileInfo) {
        tableVehcileInfo = $('#tableVehcileInfo').dataTable({
            sAjaxSource: '/Member360/GetMemberCarInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aoColumns: [
                { data: "PlateNumVehicle", title: "车牌号", sortable: false },
                { data: "VINVehicle", title: "车架号", sortable: false },
                {
                    data: "BrandName", title: "品牌", sortable: false
                },
                {
                    data: "SeriesName", title: "车系", sortable: false
                },
                {
                    data: "LevelName", title: "车型", sortable: false
                },
                {
                    data: "ColorName", title: "车辆颜色", sortable: false
                },
                { data: "TrimNameBase", title: "内饰", sortable: false },
                { data: "DriveDistinct", title: "行驶里程", sortable: false },
                {
                    data: "BuyDate", title: "购车时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtHdnMid").val() });
            }
        });
    }
    else {
        tableVehcileInfo.fnDraw();
    }
}
function Edit(id) {
    $.colorbox({
        initialHeight: '200',
        initialWidth: '400',
        href: "#table_Edit",
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $(".memInfoBlock").show();
            GetEditById(id);
            $.colorbox.resize();
        }
    });
}

function GetEditById(id) {
    $("#txtDate2").attr("readonly", "readonly");
    $("#drpStoreLevel2").val("");
    $("#divCash2").hide();
    $("#txtValue2").val("");
    $("#txtDate2").val("");
    $("#drpStore2").val("");
    $("#hdnTradeID").val("");
    $("#txtInvoice").val("");
    $("#dropPayType").val("");
    ajax('/Member360/GetEditById', { mid: id }, function (res) {
        if (res != null) {
            var isLevel = false;
            var storelevelV = res.Obj[0].AmountRecharge;
            for (var i = 0; i < document.getElementById("drpStoreLevel2").options.length; i++) {
                if (document.getElementById("drpStoreLevel2").options[i].value == storelevelV) {
                    $("#drpStoreLevel2").val(storelevelV);
                    isLevel = true;
                    break;
                }
            }
            if (!isLevel) {
                $("#divCash2").show();
                $("#txtValue2").val(res.Obj[0].AmountRecharge);
                $("#drpStoreLevel2").val("0");
            } else {
                $("#divCash2").hide();
            }
            $("#txtDate2").val(res.Obj[0].RechargeDate);
            $("#drpStore2").val(res.Obj[0].StoreCodeRecharge);
            $("#hdnTradeID").val(id);

            $("#txtInvoice").val(res.Obj[0].InvoiceRecharge1 == null ? res.Obj[0].AmountRecharge : res.Obj[0].InvoiceRecharge1);
            $("#dropPayType").val(res.Obj[0].RechargePmtCode);
            if (res.Obj[0].IsOwnStore == true)
            {
                $("#isStore").attr("checked", true);
            }
            else
                $("#isStore").attr("checked", false);


            $("#drpStoreLevel2").change(function () {
                storeLevelOnChange($("#drpStoreLevel2").val());

            })
        }
    });
}

//查看会员详细信息
function detail(id) {

    $(".memInfoBlock").show();
    flag = 0;
    $.colorbox.close();
    $("#hdnMemberId").val(id);
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            //memberId = mid;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);

            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "v1" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].CustomerStatus == 1 ? "正常" : "停用");
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

            memname = res.Obj[0].CustomerName;
            memcardno = res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo;
            memvinkey = res.Obj[0].VinKey == null ? "" : res.Obj[0].VinKey;


            //加载审批记录
            LoadStoreValueRecord();
        }
    })
    loadCashPoint(id);
}
//审批记录
function Inactive(id) {
    $.dialog("确认通过审批吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Member360/InActiveStoreValueById", { itemId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                loadCashPoint($("#hdnMemberId").val());
                var start = dtStoreData.fnSettings()._iDisplayStart;
                var length = dtStoreData.fnSettings()._iDisplayLength;
                dtStoreData.fnPageChange(start / length, true);
            } else { $.dialog(res.MSG); }
        });
    })
}
//加载账户积分现金信息
function loadCashPoint(id) {
    ajax("/Member360/GetMemIsBackAccountInfo", { mid: id }, function (data) {
        $("#stgValidValue1").text(0);
        $("#stgValidValue2").text(0);
        if (data.length > 0) {
            $("#stgValidValue1").text(data[0].Value2);
            $("#stgValidValue3").text(data[0].Value1);
            $("#stgValidValue2").text(data[0].NoBackAccount == null ? "0" : data[0].NoBackAccount);
            $("#txtCash").val(data[0].Total);
            total = data[0].Total;
        }
    });
}

//加载门店
function LoadStoreList() {
    ajax('/Member360/GetActLimitStoreList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drpStore').append(opt);
            $('#drpStore2').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStore').append(opt);
            $('#drpStore2').append(opt);
        }
    });
}

function GetDateStr(AddDayCount) {
    var dd = new Date();
    dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1;//获取当前月份的日期
    var d = dd.getDate();
    return y + "-" + m + "-" + d;
}

function checkStr(str) {
    var pattern = new RegExp("[%--`~!#$^&*()=|{}':;',\\[\\].<>/?~！#￥……&*（）——| {}【】‘；：”“'。，、？]");
    return pattern.test(str);
}

function storeLevelOnChange(val) {
    if (val == 0) {
        $("#divCash2").show();
    }
    else
        $("#divCash2").hide();

}

//加载支付类型
function LoadPayType() {
    ajaxSync('/BaseData/GetOptionDataList', { optType: "PayType" }, function (res) {
        $('#dropPayType').html("");
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#dropPayType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#dropPayType').append(opt);
        }
    });
}