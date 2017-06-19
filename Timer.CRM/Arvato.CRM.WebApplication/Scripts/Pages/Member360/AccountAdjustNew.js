var tableMemberInfo;
$(function () {
    //$('#txtStartDate').datepicker({ startDate: '0' });
    $("#txtStartDate,#txtEndDate").datepicker();

    //加载门店
    loadStore();
    LoadBusinessChildType();
    //搜索会员
    $("#btnSearch").click(function () {
        //至少有一个条件才能搜索
        if ($("#txtCardNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '') {
            $.dialog("至少输入一个条件以供查询");
            return;
        }
        if (checkStr($("#txtCardNo").val())) {
            $.dialog("会员卡号中不能包含特殊字符！");
            return;
        }
        if (checkStr($("#txtName").val())) {
            $.dialog("姓名中不能包含特殊字符！");
            return;
        }
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#table_Member",
            inline: true,
            opacity: '0.3',
            onComplete: function () {
                loadMemInfoList();
                $.colorbox.resize();
            }
        });
        //$.colorbox.resize();
    })
    $("#btnSearchTrade").click(function () {
        if ($("#txtMemId").val() == "") {
            $.dialog("请先选择会员");
            return;
        }
        if ($("#spnCardStat").text() == '停用') {
            $.dialog("该会员已停用,请联系管理员！");
            return;
        }
        LoadStoreValueRecord();
    })
    //保存
    $("#frmAddStore").submit(function (e) {
        e.preventDefault();

        var id = $("#hdnMemberId").val();
        if (id != '') {
            var changeValue = $("#txtValue").val();
            if (!utility.isMoney(changeValue)) {
                $.dialog("调整金币数量格式不正确");
                return;
            }
            if (changeValue == 0) {
                $.dialog("调整金币数量不能为零");
                return;
            }
            if ($("#drpBusiChildType").val() == '') {
                $.dialog("请选择调整原因");
                return;
            }
            if (DataValidatorAdd.form()) {
                //储值操作
                ajax('/Member360/SaveAccountChange', { mid: id, tid: $("#txtTradeId").val(), changeValue: changeValue, changereson: $("#drpBusiChildType").find("option:selected").text(),reasoncode:$("#drpBusiChildType").val(), remark: encode($("#txtRemark").val()), storecode: $("#drpStore").val() }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        loadCashPoint(id);
                        LoadStoreValueRecord();
                        $.dialog(res.MSG);
                    } else { $.dialog(res.MSG); }
                })
            } else { $.dialog("请先选择会员"); }
        }
    })

})
//新增时验证数据
var DataValidatorAdd = $("#frmAddStore").validate({
    //onSubmit: false,
    rules: {
        drpBusiChildType: {
            required: true,

        },
        txtValue: {
            required: true,
            maxlength: 10,
            number: true,

        },
        drpStore: {
            required: true,
        },

    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
var dtStoreData;
function LoadStoreValueRecord() {
    if (!dtStoreData) {
        dtStoreData = $('#dt_StoreData').dataTable({
            sAjaxSource: '/Member360/GetAccountChangeRecord',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            order: [[3, "desc"]],
            aoColumns: [
                { data: "ActChangeValue", title: "调整金币数量", sortable: false },

                { data: "ActChangeReason", title: "调整原因", sortable: false },
                { data: "AddedDate", title: "调整时间", sortable: true },
                {
                    data: "StatusChange", title: "审批状态", sortable: true, render: function (obj) {
                        if (obj == '1') return "待审批"; else return "已审批";
                    }
                },
                { data: "UserName", title: "操作人", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        if (obj.StatusChange == "1")
                            return "<button class=\"btn btn-default\" onclick=\"Edit(" + obj.TradeID + ")\">编辑</button>&nbsp;&nbsp;";
                        else
                            return "";

                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'startdate', value: $("#txtStartDate").val() });
                d.push({ name: 'enddate', value: $("#txtEndDate").val() });
            }
        });
    }
    else {
        dtStoreData.fnDraw();
    }
}
function Edit(v) {
    clearData();
    $("#txtTradeId").val(v);
    if ($("#txtStatus").val() == "0") {
        $.dialog("会员状态为停用，不允许编辑结算");
        return;
    }
    $("#addStore_dialog .heading h3").html("编辑");
    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
    ajax("/Member360/GetActChangeEditInfo", { tid: v }, function (data) {
        if (data.IsPass) {
            var item = data.Obj[0];
            if (item != null) {

                $("#txtValue").val(item.ActChangeValue);
                $("#drpBusiChildType").val(item.ActChangeReasonCode);
                //$("#drpBusiChildType option[text='" + item.ActChangeReason + "']").attr("selected", true);
                $("#drpStore").val(item.ChangeStoreCode);
                $("#txtRemark").val(item.Remark);
            }
        }
    });
}
//清空数据
function clearData() {

    $("#txtValue").val('');
    $("#drpBusiChildType").val('');
    $("#drpStore").val('');
    $("#txtRemark").val('');

    $('.error-block').html('');
}
//新建条目
function add() {
    $("#txtTradeId").val('');
    clearData();
    if ($("#txtStatus").val() == "0") {
        $.dialog("会员状态为停用，不允许新建");
        return;
    }

    $("#addStore_dialog .heading h3").html("新建");
    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
}
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
                d.push({ name: 'memNo', value: $("#txtCardNo").val() });
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

//查看会员详细信息
function detail(id) {

    $(".memInfoBlock").show();
    $(".divsearch").show();
    $.colorbox.close();
    $("#hdnMemberId").val(id);
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            //memberId = mid;
            //给页面上详细信息栏赋值
            $("#txtStatus").val(res.Obj[0].CustomerStatus);
            $("#spnName").text(res.Obj[0].CustomerName);

            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "v1" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].CustomerStatus == 1 ? "正常" : "停用");
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

            //加载审批记录
            LoadStoreValueRecord();
        }
    })
    loadCashPoint(id);
}
function loadCashPoint(id) {
    ajax("/Member360/GetMemIsBackAccountInfo", { mid: id }, function (data) {
        $("#stgValidValue1").text(0);
        $("#stgValidValue2").text(0);
        if (data.length > 0) {
            $("#stgValidValue1").text(data[0].Value2);
            $("#stgValidValue3").text(data[0].Total);
            $("#stgValidValue2").text(data[0].NoBackAccount == null ? "0" : data[0].NoBackAccount);
            $("#txtCash").val(data[0].Total);
            total = data[0].Total;
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


//加载子业务类型
function LoadBusinessChildType() {
    ajaxSync('/BaseData/GetTypeBListAccount', null, function (res) {
        $('#drpBusiChildType').html("");
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].TypeBCodeBase + '>' + res[i].TypeBNameBase + '</option>';
            }
            $('#drpBusiChildType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpBusiChildType').append(opt);
        }
    });
}

function loadStore() {
    ajax('/Member360/GetStoreListByGroupID', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drpStore').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStore').append(opt);
        }
    });
}