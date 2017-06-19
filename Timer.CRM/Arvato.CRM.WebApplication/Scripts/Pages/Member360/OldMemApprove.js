var tableMemberInfo, tableVehcileInfo;
$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
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
        loadMemInfoList();

        $("#txtHdnMid").val('');
    })

    $("#btnAddSave").click(function () {
        //判断是否修改逻辑
        var total = $("#txtTotalGold").val();
        var noinvoice = $("#txtNoInvoiceGold").val();
        var recharge = $("#txtRechargeGold").val();
        var send = $("#txtSendGold").val();
        if (utility.isNull(total) || utility.isNull(noinvoice) || utility.isNull(recharge) || utility.isNull(send)) {
            $.dialog("四个金额都不能为空");
            return;
        }
        if (!utility.isNull(total) && !utility.isNull(noinvoice)) {
            if ((noinvoice - total) > 0) {
                $.dialog("未开票金额不能超过累计充值金额");
                return;
            }
        }
        //if (!utility.isNull(total) && !utility.isNull(noinvoice) && !utility.isNull(recharge) && !utility.isNull(send)) {
        //    var histotal = $("#txtHdnTotal").val();
        //    var hisnoinvoice = $("#txtHdnNoInvoice").val();
        //    var hisrecharge = $("#txtHdnRecharge").val();
        //    var hissend = $("#txtHdnSend").val();
        //    if (total == histotal && noinvoice == hisnoinvoice && recharge == hisrecharge && send == hissend) {
        //        $.dialog("调整后金额和调整前金额一样，请修改");
        //        return;
        //    }
        //} else if (utility.isNull(total) && utility.isNull(noinvoice) && utility.isNull(recharge) && utility.isNull(send)) {

        //}else {
        //    $.dialog("调整后金额要么都为空，要么都有值");
        //    return;
        //}
        ajax('/Member360/UpdateOldMemStatus', { mid: $("#txtHdnMemMid").val(), total: total, noinvoice: noinvoice, recharge: recharge, send: send }, function (res) {
            if (res.IsPass) {
                $.colorbox.close();
                loadMemInfoList();
                $.dialog(res.MSG);
            }
        })
    })
})
//加载会员信息
function loadMemInfoList() {
    if (!tableMemberInfo) {
        tableMemberInfo = $('#tableMemberInfo').dataTable({
            sAjaxSource: '/Member360/GetOldMembersByPage',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aoColumns: [
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerName", title: "会员名称", sortable: false },

                { data: "CustomerMobile", title: "手机", sortable: false },
                {
                    data: "CustomerStatus", title: "会员状态", sortable: false
                },
                {
                    data: "CustomerLevel", title: "会员等级", sortable: false
                },
                {
                    data: "NoInvoiceAccount", title: "未开票金额", sortable: false
                },
                {
                    data: "OldTotalValue", title: "总金额", sortable: false
                },
                {
                    data: "OverRechargeValue", title: "剩余购买金币", sortable: false
                },
                {
                    data: "OverSendValue", title: "剩余赠送金币", sortable: false
                },
                {
                    data: "StatusChange", title: "审批状态", sortable: true, render: function (obj) {
                        if (obj == '0') return "未审批"; else return "已审批";
                    }
                },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        if (obj.StatusChange == '0')
                            return '<button class="btn" onclick="vehcile(\'' + obj.MemberID + '\',this)">查看车辆</button><button class="btn btn-danger" onclick="detail(\'' + obj.MemberID + '\',\'' + obj.MemberCardNo + '\',\'' + obj.CustomerName + '\')">审批</button> ';
                        else
                            return '<button class="btn" onclick="vehcile(\'' + obj.MemberID + '\',this)">查看车辆</button>';
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'memNo', value: $("#txtCardNo").val() });
                d.push({ name: 'memName', value: $("#txtName").val() });
                d.push({ name: 'memMobile', value: $("#txtMobile").val() });
                d.push({ name: 'status', value: $("#txtStatus").val() });
                d.push({ name: 'startDate', value: $("#txtStartDate").val() });
                d.push({ name: 'endDate', value: $("#txtEndDate").val() });
            }
        });
    }
    else {
        tableMemberInfo.fnDraw();
    }
}

//审批
function detail(id, cardno, name) {
    $("#txtHdnMemMid").val(id);
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
    $("#txtTotalGold").val('');
    $("#txtNoInvoiceGold").val('');
    $("#txtRechargeGold").val('');
    $("#txtSendGold").val('');

    $("#txtMemNo").val(cardno);
    $("#txtMemName").val(name);

    //ajax("/Member360/GetOldMemActInfo", { mid: id }, function (data) {

    //    //$("#txtTotalGold").val(data.TotalAct);
    //    //$("#txtNoInvoiceGold").val(data.NoInvoiceAct);
    //    //$("#txtRechargeGold").val(data.RechargeAct);
    //    //$("#txtSendGold").val(data.SendAct);


    //    $("#txtHdnTotal").val(data.TotalAct);
    //    $("#txtHdnNoInvoice").val(data.NoInvoiceAct);
    //    $("#txtHdnRecharge").val(data.RechargeAct);
    //    $("#txtHdnSend").val(data.SendAct);


    //});

}
//查看车辆
function vehcile(id, obj) {
    //$(obj).parent().parent().css('background-color', 'grey').siblings().css('background-color', '');

    $("#txtHdnMid").val(id);
    //$('#tableVehcileInfo_wrapper').show();
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


function checkStr(str) {
    var pattern = new RegExp("[%--`~!#$^&*()=|{}':;',\\[\\].<>/?~！#￥……&*（）——| {}【】‘；：”“'。，、？]");
    return pattern.test(str);
}
