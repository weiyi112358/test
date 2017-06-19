var dt_RuleTable;
$(function () {
    //加载数据表格
    loadRuleList();
    //加载数据类型列表
    LoadRuleType();
    //加载规则表
    if ($("#hideRuleName").val() != "") $("#txtRuleName").val($("#hideRuleName").val());
    if ($("#hideRuleType").val() != "") $("#drpRuleType").val($("#hideRuleType").val());
    if ($("#hideEnable").val() != "") $("#drpIsEnable").val($("#hideEnable").val());
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
    $("#hideRuleName").val($("#txtRuleName").val());
    $("#hideRuleType").val($("#drpRuleType").val()); 
    $("#hideEnable").val($("#drpIsEnable").val());
    $("#form1").submit();
}

//修改规则
function modifyRule(ruleID) {
    $("#hideRuleID").val(ruleID);
    $("#hideRuleName").val($("#txtRuleName").val());
    $("#hideRuleType").val($("#drpRuleType").val());
    $("#hideEnable").val($("#drpIsEnable").val());
    $("#form1").submit();
}

$("#button").click(function () {
    $("#effect").toggleClass("newClass", 1000);
    return false;
});

//启用条目
function active(id) {
    $.dialog("确认启用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Loyalty/ActiveRuleById", { ruleId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                var start = dt_RuleTable.fnSettings()._iDisplayStart;
                var length = dt_RuleTable.fnSettings()._iDisplayLength;
                dt_RuleTable.fnPageChange(start / length, true);
                //loadRuleList();
            } else { $.dialog(res.MSG); }
        });
    })
}

//禁用条目
function Inactive(id) {
    $.dialog("确认禁用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Loyalty/InActiveRuleById", { ruleId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                //loadRuleList();

                var start = dt_RuleTable.fnSettings()._iDisplayStart;
                var length = dt_RuleTable.fnSettings()._iDisplayLength;
                dt_RuleTable.fnPageChange(start / length, true);
            } else { $.dialog(res.MSG); }
        });
    })
}

//删除规则
function deleteRule(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Loyalty/DeleteRuleById", { ruleId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                loadRuleList();
            } else { $.dialog(res.MSG); }
        });
    })
}

//加载规则类型
function LoadRuleType() {
    //$('#drpRuleType').empty();
    ajax('/Loyalty/GetRuleTypeList', null, function (res) {
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
            sAjaxSource: '/Loyalty/GetRuleData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aaSorting: [[3, "asc" ]],
            aoColumns: [
                { data: 'RuleName', title: "规则名称", sortable: false },
                { data: 'TypeDesc', title: "规则类型", sortable: false },
                { data: 'SubTypeDesc', title: "规则子类型", sortable: false },
                { data: 'RunIndex', title: "优先级", sortable: true },
                {
                    data: 'StartDate', title: "生效日期", sortable: true, render: function (r) {
                        return r.substr(0,10);
                    }
                },
                { data: 'AddedDate', title: "创建时间", sortable: true },
                //{
                //    data: 'Enable', title: "是否启用", sortable: false, render: function (r) {
                //        return r == true ? "启用" : "未启用";
                //    }
                //},
                { data: 'Cycle', title: "周期", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        if (obj.Enable)
                            return "<button class=\"btn\" id=\"btnModify\"  onclick=\"modifyRule(" + obj.RuleID + ")\">编辑</button><button class=\"btn btn-danger\" onclick=\"Inactive(" + obj.RuleID + ")\">禁用</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteRule(" + obj.RuleID + ")\">删除</button>";
                        else
                            return "<button class=\"btn\" id=\"btnModify\"  onclick=\"modifyRule(" + obj.RuleID + ")\">编辑</button><button class=\"btn btn-info\" onclick=\"active(" + obj.RuleID + ")\">启用</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteRule(" + obj.RuleID + ")\">删除</button>";

                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'ruleName', value: $("#txtRuleName").val() });
                d.push({ name: 'ruleType', value: $("#drpRuleType").val() });
                d.push({ name: 'enable', value: $("#drpIsEnable").val() });
            }
        });
    }
    else {
        dt_RuleTable.fnDraw();
    }
}