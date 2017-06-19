var tableMemberInfo;
$(function () {
    //搜索会员
    $("#btnSearch").click(function () {
        //至少有一个条件才能搜索
        if ($("#txtCardNo").val() == '' && $("#txtNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '') {
            $.dialog("至少输入一个条件以供查询");
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
    });
    //新建账户
    $("#btnAddAct").click(function () {
        var memId = $("#hdnMemberId").val();
        if (memId != '') {
            $("#table_EditHistoryAmount h3").html('新增历史金额调整');
            var historyAmount = $("#spnHistoryAmount").text();
            var adjustAmount = $("#spnAdjustAmount").text();
            var amount = Number(historyAmount) + Number(adjustAmount);
         
            $("#txtHistoryAmount").val(amount).attr('disabled', 'disabled');
            $("#txtNumber").val('');
            $("#txtId").val('');
            $("#txtAdjustReason").val('');
            $("#drpAdjustType").val('');
            if (amount == 0) {
                $("#drpAdjustType option[value='add']").attr("selected", true);
                $("#drpAdjustType").attr('disabled', 'disabled');
            } else {
                $("#drpAdjustType").attr('disabled', false);
            }
            $.colorbox({
                initialHeight: '0',
                initialWidth: '0',
                href: "#table_EditHistoryAmount",
                inline: true,
                opacity: '0.3',
                onComplete: function () {
                    $.colorbox.resize();
                }
            });
        } else {
            $.dialog('请先查询并选择会员！');
        }
    });
});


//保存调整信息
$("#frmEditHistoryAmount").submit(function (e) {
    e.preventDefault();
    if (DataValidatorAct.form()) {
        $("#btnSaveAjustAmount").attr('disabled', 'disabled');
        var value = $("#txtNumber").val();
        var historyAmount = $("#txtHistoryAmount").val();
        var type = $("#drpAdjustType").val();
        var memId = $("#hdnMemberId").val(); //会员id
      
        var ajustReason = encode($("#txtAdjustReason").val());
      
        if (memId == '') {
            $.dialog("请先选择会员，然后再添加账户");
            $("#btnSaveAjustAmount").attr('disabled', false);
            return;
        }
        if (type == "") {
            $.dialog('请选择调整类型！');
            $("#btnSaveAjustAmount").attr('disabled', false);
            return;
        }
        if (historyAmount == 0 && type == "sub") {
            $.dialog('可调整金额为0时，调整类型不能选择减少！');
            $("#btnSaveAjustAmount").attr('disabled', false);
            return;
        }
        if (value <= 0) {
            $.dialog('调整数值必须大于0！');
            $("#btnSaveAjustAmount").attr('disabled', false);
            return;
        }
        if (type == "sub") {
            if (historyAmount < value) {
                $.dialog("减少金额不能大于可调整金额");
                $("#btnSaveAjustAmount").attr('disabled', false);
                return;
            }
        }
     
        ajax("/Member360/SaveAjustAmountInfo", { ajustType: type, value: value, memId: memId, ajustReason: ajustReason }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                $.colorbox.close();
                loadAjustAmountList();
                detail(memId);
            } else {
                $.dialog(res.MSG);
            }
            $("#btnSaveAjustAmount").attr('disabled', false);
        });
    }
});

//新增时验证数据
var DataValidatorAct = $("#frmEditHistoryAmount").validate({
    //onSubmit: false,
    rules: {
        txtHistoryAmount: {
            required: true,
        },
        txtNumber: {
            required: true,
            isDecimal: true,
        },
        txtAdjustReason: {
            required: true,
            maxlength: 100
        },
        drpAdjustType: {
            reauired: true
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
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
            iDisplayLength: 5,
            aoColumns: [
                { data: "MemberCardNo", title: "会员编号", sortable: false, sWidth: "25%" },
                { data: "CustomerName", title: "会员名称", sortable: false },
                { data: "CustomerMobile", title: "手机", sortable: false },
                { data: "CustomerLevelText", title: "会员等级", sortable: false },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="detail(\'' + obj.MemberID + '\')">查看</button> ';
                        return str;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'cardNo', value: encode($.trim($("#txtCardNo").val())) });
                d.push({ name: 'memNo', value: encode($.trim($("#txtNo").val())) });
                d.push({ name: 'memName', value: encode($.trim($("#txtName").val())) });
                d.push({ name: 'memMobile', value: encode($("#txtMobile").val()) });
            }
        });
    }
    else {
        tableMemberInfo.fnDraw();
    }
}

//查看会员详细信息
function detail(id) {
    $.colorbox.close();
    $(".memInfoBlock").show();
    ajax('/Member360/GetMemberAmountByMemberId', { memId: id }, function (res) {
        if (res.IsPass) {
            $("#txtActId").val('');
            var memId = res.Obj[0].MemberID;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);
            $("#hdnMemberId").val(memId);
            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCode == null ? "" : res.Obj[0].MemberCode);
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);
            $("#spnHistoryAmount").text(res.Obj[0].HistoryConsumeAmount == null ? "0" : res.Obj[0].HistoryConsumeAmount);
            $("#spnAdjustAmount").text(res.Obj[0].TotalAdjustAmount == null ? "0" : res.Obj[0].TotalAdjustAmount);

            loadAjustAmountList();
        }
    })
}


//加载会员账户列表
function loadAjustAmountList() {
    ajax("/Member360/GetAjustHistoryAmountData", { memId: $("#hdnMemberId").val() }, function (res) {//  '0010024595984EB291D851C44EF091C6'
        var tbody = $("#dtHistoryAmountDetail tbody");
        $('#txtActId').val('');
        tbody.empty();
        var data1 = res.Obj;
        if (data1 == null) {
            tbody.append('<tr class="odd"><td class="dataTables_empty" valign="top" colspan="6">没有记录</td></tr>');
        }
        else {
            var data = data1[0];
            if (data.length > 0) {
                $('#txtActId').val(data[0].ID);
                for (var i in data) {
                    var reason = data[i].AdjustReason;
                    if (reason.length > 50) {
                        reason = reason.substring(0, 47) + "...";
                    }
                    var type = "";
                    if (data[i].AdjustWay == "add")
                        type = "增加";
                    else {
                        type = "减少";
                    }
                    var tr = $('<tr></tr>');
                    tr.append('<td>' + type + '</td>')
                    .append('<td>' + data[i].AdjustAmount + '</td>')
                    .append('<td>' + data[i].AdjustDate + '</td>')
                    .append('<td title=' + data[i].AdjustReason + '>' + reason + '</td>')
                    .appendTo(tbody);
                }
            } else {
                tbody.append('<tr class="odd"><td class="dataTables_empty" valign="top" colspan="6">没有记录</td></tr>');
            }
        }
    });
}
