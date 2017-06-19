var dt_RuleTable;
$(function () {
    //加载数据表格
    loadRuleList();
    //加载数据类型列表
    LoadRuleType();
    //加载规则表
    if ($("#hideCode").val() != "") $("#txtCode").val($("#hideCode").val());
    if ($("#hideRuleType").val() != "") $("#drpRuleType").val($("#hideRuleType").val());
    if ($("#hideApproveStatus").val() != "") $("#drpApproveStatus").val($("#hideApproveStatus").val());
    if ($("#hideExecuteStatus").val() != "") $("#drpExecuteStatus").val($("#hideExecuteStatus").val());
    //查询
    $("#btnSearch").click(function () {
        loadRuleList();
    })
    //注册新增按钮
    $("#btnAdd").click(function (e) {
        AddRule();
    });
})

//新增规则
function AddRule() {
    $("#hideRuleID").val('');
    $("#hideCode").val($("#txtCode").val());
    $("#hideRuleType").val($("#drpRuleType").val()); 
    $("#hideApproveStatus").val($("#drpApproveStatus").val());
    $("#hideExecuteStatus").val($("#drpExecuteStatus").val());
    $("#form1").submit();
}

//修改规则 
$('#dt_Rule').on('click', '.modifyRule,.billcode', function () {
    var ruleID = $(this).attr('billid');
    $("#hideRuleID").val(ruleID);
    $("#hideCode").val($("#txtCode").val());
    $("#hideRuleType").val($("#drpRuleType").val());
    $("#hideApproveStatus").val($("#drpApproveStatus").val());
    $("#hideExecuteStatus").val($("#drpExecuteStatus").val());
    $("#form1").submit();
})

$("#button").click(function () {
    $("#effect").toggleClass("newClass", 1000);
    return false;
});


//-----------动作-------------------------------------
function showResult(res) {
    if (res.IsPass) {
        $.dialog(res.MSG); 
        var start = dt_RuleTable.fnSettings()._iDisplayStart;
        var length = dt_RuleTable.fnSettings()._iDisplayLength;
        dt_RuleTable.fnPageChange(start / length, true);
    } else { $.dialog(res.MSG); }
}
//唤醒条目
$('#dt_Rule').on('click', '.wakeupRule', function () {
    var id = $(this).attr('billid');
    $.dialog("确认唤醒吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/MemberSalesPromotion/ActiveRuleById", { ruleId: id, IsWakeUp: true }, showResult);
    })
});

//休眠条目
$('#dt_Rule').on('click', '.sleepRule', function () {
    var id = $(this).attr('billid');
    $.dialog("确认休眠吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/MemberSalesPromotion/ActiveRuleById", { ruleId: id, IsWakeUp: false }, showResult);
    })
});

//审核条目
$('#dt_Rule').on('click', '.approveRule', function () {
    var id = $(this).attr('billid');
    $.dialog("确认审核此规则吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/MemberSalesPromotion/ApproveRuleById", { ruleId: id, active: 1 }, showResult);
    })
});
//作废条目
$('#dt_Rule').on('click', '.voidRule', function () {
    var id = $(this).attr('billid');
    $.dialog("确认作废此规则吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/MemberSalesPromotion/ApproveRuleById", { ruleId: id, active: 2 }, showResult);
    })
});
//删除条目
$('#dt_Rule').on('click', '.deleteRule', function () {
    var id = $(this).attr('billid');
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/MemberSalesPromotion/DeleteRuleById", { ruleId: id }, showResult);
    })
});




//-----------加载规则-------------------------------------

//加载规则模板
function LoadRuleType() {
    //$('#drpRuleType').empty();
    ajax('/MemberSalesPromotion/GetRuleTypeList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpRuleType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpRuleType').append(opt);
        }
    });
}

function loadRuleList() {
    if (!dt_RuleTable) {
        dt_RuleTable = $('#dt_Rule').dataTable({
            sAjaxSource: '/MemberSalesPromotion/GetRuleData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aaSorting: [[1, "desc" ]],
            aoColumns: [
                {
                    data: null, title: "促销单号", sortable: false, render: function (obj) {
                        return "<a class='billcode' billid='{0}' href='#' >{1}</a>".format(obj.BillID, obj.BillCode);
                    }
                },
                { data: 'BillCode', title: "促销单号", visible: false, },
                { data: 'ExecuteStatus', title: "执行状态", sortable: true },
                { data: 'TypeDesc', title: "模板", sortable: true },
                { data: 'ApproveStatus', title: "审核状态", sortable: true },
                
                { data: 'Days', title: "促销日期", sortable: true },
                
                //{ data: 'Remark', title: "说明", sortable: false },//需要改成 textarea
                 {
                     data: 'Remark', title: "说明", sortable: false, render: function (obj) {
                         if (obj == null)
                             obj = '';
                         return "<textarea readonly>{0}</textarea>".format(obj);
                     }
                 },
                { data: 'CreateInfo', title: "创建信息", sortable: false },
                { data: 'UpdateInfo', title: "修改信息", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        var action = '<button class=\"btn\ modifyRule" billid="{0}"  {1}  >编辑</button>';
                        if (obj.ExecuteStatus == '执行中')
                            action += '<button class=\"btn btn-danger\ sleepRule" billid="{0}" {1}  >休眠</button>';
                        else if (obj.ExecuteStatus == '休眠中')
                            action += '<button class=\"btn btn-info\ wakeupRule" billid="{0}"  {1}  >唤醒</button>';
                        //未审核，可以删除
                        if (obj.ApproveStatus == "未审核") {
                            action += '<button class=\"btn btn-info\ approveRule" billid="{0}"  >审核</button>';
                            action += '<button class=\"btn btn-danger\ deleteRule" billid="{0}"  >删除</button>';
                        }
                        else if (obj.ApproveStatus == "已审核") { 
                            action += '<button class=\"btn btn-danger\ voidRule" billid="{0}"  >作废</button>';
                        }
                       if (obj.ApproveStatus == "已作废") {
                           //action = action.format(obj.BillID, "disabled");
                           return '';
                       }
                        else 
                           action = action.format(obj.BillID, '');
                        

                        return action;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'Code', value: $("#txtCode").val() });
                d.push({ name: 'ruleType', value: $("#drpRuleType").val() });
                d.push({ name: 'approveStatus', value: $("#drpApproveStatus").val() });
                d.push({ name: 'executeStatus', value: $("#drpExecuteStatus").val() });
            }
        });
    }
    else {
        dt_RuleTable.fnDraw();
    }
}