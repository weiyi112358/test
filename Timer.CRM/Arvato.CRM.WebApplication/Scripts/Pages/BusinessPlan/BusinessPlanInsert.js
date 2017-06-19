var dtkpi,
    dtBusinessPlanActivity,
    businessPlanID,
    businessPlanStatus,
    updateDate,
    kpi = [],
    parentWindow,
    kpiForPage = [];

$(document).ready(function () {
    parentWindow = window.opener;
    hidemenu();//默认隐藏菜单
    $("#txtPlanStartTime,#txtPlanEndTime").datepicker({ dateFormat: "yy-mm-dd", startDate: "-1" });
    businessPlanID = $("#hidBusinessPlanID").val();
    businessPlanStatus = $("#hidBusinessStatus").val();
    updateDate = $("#hidBusinessPlanUpdateDate").val();
    loadBusinessType();
    pageInit();

    getPOSpromotion(businessPlanID)

    $("#btnsave").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        insertBusinessPlan();
    });

    //$("#btnReturn").click(function () {
    //    window.location.href = "/BusinessPlan/Index";
    //});
});

function pageInit() {
    if (!utility.isNull(businessPlanID)) { //编辑页面
        if (!utility.isNull(businessPlanStatus) && businessPlanStatus != "1") { //如果状态不是提交申请,则直接跳到明细页面
            window.location.href = "/BusinessPlan/BusinessPlanDetail/" + businessPlanID;
        }
        loadBusinessPlan(businessPlanID);
        loadBusinessPlanActivityList(businessPlanID);
    }
}

function loadBusinessPlan(planID) {
    ajax("/BusinessPlan/SearchBusinessPlanTarget", { planID: planID }, function (data) {
        if (data == null) {
            return;
        } else {
            for (var i = 0; i < data.length; i++) {
                var o = {
                    "KPIID": data[i].KPIID, "KPIName": data[i].KPIName, "KPIType": data[i].KPIType, "TargetValueType": data[i].TargetValueType, "KPITypeValue": data[i].KPITypeValue,
                    "Unit": data[i].Unit, "IntValue1": data[i].IntValue1, "DecValue1": data[i].DecValue1, "DecValue2": data[i].DecValue2, "StrValue1": data[i].StrValue1
                }
                kpi.push(o);
            }
            loadBusinessPlanCallback();
        }
    });
};

function loadBusinessPlanCallback() {
    for (var i = kpi.length - 1; i >= 0; i--) {
        var currentKPI = kpi[i];
        var html = "";
        if (currentKPI.TargetValueType == "2") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.StrValue1 + "\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else if (currentKPI.TargetValueType == "3") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.IntValue1 + "\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        } else if (currentKPI.TargetValueType == "7") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtMinKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.DecValue1 + "\"/>--"
                + "<input type=\"text\" id=\"txtMaxKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.DecValue2 + "\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.StrValue1 + "\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        }
        $("#divKPIList").append(html);
    }
    kpiForPage = [];
    //if (kpi.length > 0) {
    //    $("#btnsave").show();
    //}
    $.colorbox.close();
}

function loadBusinessType() {
    ajax("/BaseData/GetBizOptionsByType", { optionType: "BusinessPlanType", enable: true }, function (data) {
        $("#drpPlanType").html("");
        var options = "";
        if (data && data.length > 1) {
            for (var i = 0; i < data.length; i++) {
                options += "<option value='" + data[i].OptionValue + "'>" + data[i].OptionText + "</option>";
            }
            $("#drpPlanType").html(options);
        }

        var businessPlanType = $("#hidBusinessPlanType").val();

        if (!utility.isNull(businessPlanType)) {
            $("#drpPlanType").val(businessPlanType);
        }
    });
}

function loadBusinessPlanActivityList(planID) {
    //destory datatable资源之后重新加载新资源
    if (dtBusinessPlanActivity) {
        dtBusinessPlanActivity.fnDestroy();
    }

    dtBusinessPlanActivity = $("#dt_BusinessPlanActivity").dataTable({
        sAjaxSource: "/BusinessPlan/SearchActivityByBusinessPlan",
        sScrollX: "100%",
        sScrollXInner: "100%",
        bScrollCollapse: true,                       //指定适当的时候缩起滚动视图
        bInfo: true,
        bAutoWidth: true,                                                     //是否自动计算表格各列宽度
        bDestroy: true,
        bRetrieve: true,
        bServerSide: true,
        bLengthChange: false,
        bPaginate: true,
        iDisplayLength: 8,
        aoColumns: [
            { data: "ActivityID", title: "活动编号", sortable: false, "sClass": "center", sWidth: "20%", bVisible: false },
            { data: "ActivityName", title: "活动名称", sortable: false, "sClass": "center", sWidth: "20%", bVisible: true },
            {
                data: null, title: "活动状态", sWidth: "10%", sortable: false,
                render: function (obj) {
                    return obj.Enable ? '审批通过' : '提交审批';
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: "planID", value: planID });
        }
    });
}

function loadKPI() {
    if (dtkpi) {
        dtkpi.fnDestroy();
    }

    dtkpi = $('#dt_KPIList').dataTable({
        sAjaxSource: "/BusinessPlan/GetKPIData",
        bInfo: true,
        bSort: true,
        bInfo: false,
        searching: true,
        bServerSide: false,
        bLengthChange: false,
        bLengthChange: false,
        bPaginate: true,
        iDisplayLength: 8,
        aoColumns: [
            { data: 'KPIName', title: "指标名称", sortable: false, "sClass": "center", sWidth: "55%" },
            {
                data: null, title: "选择", sortable: false, "sClass": "center", sWidth: "45%", render: function (r) {
                    var isExist = false;
                    for (var i = 0; i < kpi.length; i++) {
                        var currentKPI = kpi[i];
                        if (currentKPI.KPIID == r.KPIID) {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist) {
                        return "<input type=\"checkbox\" onchange=\"addkpi('" + r.KPIID + "','" + r.KPIName
                            + "','" + r.KPIType + "','" + r.Unit + "','" + r.TargetValueType + "')\" />";
                    } else {
                        return "";
                    }
                }
            }
        ]
    });
}

//加载指标列表页面
function showKPIdialog() {
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        title: '选择指标',
        href: "#addKPI_dialog",
        inline: true
    });
    kpiForPage = [];
    loadKPI();
}

//单个勾选  kpiForPage 页面显示的时候使用  kpi临时？
function addkpi(kpiID, kpiName, kpiType, unit, targetValueType) {
    for (var i = 0; i < kpi.length; i++) {
        if (kpi[i].KPIID == kpiID) {
            kpi.splice(i, 1);
            break;
        }
    }

    var o = { "KPIID": kpiID, "KPIName": kpiName, "KPIType": kpiType, "TargetValueType": targetValueType, "KPITypeValue": businessPlanID, "Unit": unit, "IntValue1": "", "DecValue1": "", "DecValue2": "", "StrValue1": "" }
    kpiForPage.push(o);
}

//将选择的指标显示到页面上
function showKPI() {
    for (var i = kpiForPage.length - 1; i >= 0; i--) {
        var currentKPI = kpiForPage[i];
        var html = "";
        if (currentKPI.TargetValueType == "2") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else if (currentKPI.TargetValueType == "3") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        } else if (currentKPI.TargetValueType == "7") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtMinKPI_" + currentKPI.KPIID + "\" required=\"required\"/>--"
                + "<input type=\"text\" id=\"txtMaxKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        }

        var flag = false; //如果不存在
        for (var index = 0; index < kpi.length; index++) {
            if (kpi[index].KPIID == currentKPI.KPIID) {
                flag = true;
            }
        }

        if (!flag) {
            kpi.push(currentKPI);
        }

        $("#divKPIList").append(html);

    }
    kpiForPage = [];
    if (kpi.length > 0) {
        $("#btnsave").show();
    }
    $.colorbox.close();
}

function deletekpi(o) {
    if (o.parentNode.parentNode) {
        var kpiID = o.parentNode.parentNode.id;
        if (kpiID) {
            kpiID = kpiID.substr(6);
        }
        //o.parentNode.parentNode.remove();
        $("#liKPD_" + kpiID).remove();

        for (var i = 0; i < kpi.length; i++) {
            if (kpi[i].KPIID == kpiID) {
                kpi.splice(i, 1);
                break;
            }
        }
    }
}

function insertBusinessPlan() {
    if (canInsertBusinessPlan()) {
        var planBaseInfo = {
            BusinessPlanID: businessPlanID,
            BusinessPlanName: $("#txtBusinessPlanName").val(),
            PlanStartTime: $("#txtPlanStartTime").val(),
            PlanEndTime: $("#txtPlanEndTime").val() + " 23:59:59",
            PlanType: $("#drpPlanType").val(),
            Remark: $("#txtRemark").val(),
            Status: "1",
            ModifiedDate: updateDate
        };

        if (kpi.length > 0) {
            for (var i = 0; i < kpi.length; i++) {
                var obj = kpi[i];
                if (obj.TargetValueType == "2") {
                    obj.StrValue1 = $("#txtKPI_" + obj.KPIID).val();
                } else if (obj.TargetValueType == "3") {
                    obj.IntValue1 = $("#txtKPI_" + obj.KPIID).val();
                } else if (obj.TargetValueType == "7") {
                    obj.DecValue1 = $("#txtMinKPI_" + obj.KPIID).val();
                    obj.DecValue2 = $("#txtMaxKPI_" + obj.KPIID).val();
                } else {
                    obj.StrValue1 = $("#txtKPI_" + obj.KPIID).val();
                }
            }
        }

        var postUrl = "/BusinessPlan/AddBusinessPlan";
        if (!utility.isNull(businessPlanID)) {
            postUrl = "/BusinessPlan/UpdateBusinessPlan";
        }

        ajax(postUrl, { businessPlan: JSON.stringify(planBaseInfo), businessPlanTargetList: JSON.stringify(kpi) }, function (data) {
            if (data && data.IsPass) {
                $.dialog("保存成功");
                parentWindow.location.reload();
                window.close();
                //window.location.href = "/BusinessPlan/Index";
            } else {
                $.dialog(data.MSG);

            }
        });
    }
}

function canInsertBusinessPlan() {
    if (utility.isNull($("#txtBusinessPlanName").val())) {
        $.dialog("计划名称不能为空");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (utility.isNull($("#txtPlanStartTime").val())) {
        $.dialog("计划开始时间不能为空");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (utility.isNull($("#txtPlanEndTime").val())) {
        $.dialog("计划结束时间不能为空");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (!utility.isDate($("#txtPlanStartTime").val())) {
        $.dialog("计划开始时间格式错误");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (!utility.isDate($("#txtPlanEndTime").val())) {
        $.dialog("计划结束时间格式错误");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (!utility.compareDate($("#txtPlanStartTime").val(), $("#txtPlanEndTime").val())) {
        $.dialog("时间范围错误");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (utility.isNull($("#drpPlanType").val())) {
        $.dialog("计划类型不能为空");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (utility.isNull($("#txtRemark").val())) {
        $.dialog("备注不能为空");
        $(".tabbable .nav-tabs a:eq(0)").tab("show");
        return false;
    }
    if (kpi.length == 0) {
        $.dialog("必须设置指标参数");
        $(".tabbable .nav-tabs a:eq(1)").tab("show");
        return false;
    } else {
        for (var i = 0; i < kpi.length; i++) {
            var obj = kpi[i];
            if (obj.TargetValueType == "2") {
                if (utility.isNull($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "不能为空");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
            } else if (obj.TargetValueType == "3") {
                if (utility.isNull($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "不能为空");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
                if (!utility.isInt($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "格式错误");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
            } else if (obj.TargetValueType == "7") {
                if (utility.isNull($("#txtMinKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最小值不能为空");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
                if (utility.isNull($("#txtMaxKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最大值不能为空");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
                if (!utility.isNumber($("#txtMinKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最小值格式错误");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
                if (!utility.isNumber($("#txtMaxKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最大值格式错误");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
                if (parseFloat($("#txtMaxKPI_" + obj.KPIID).val()) < parseFloat($("#txtMinKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "范围值错误");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }

            } else {
                if (utility.isNull($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "不能为空");
                    $(".tabbable .nav-tabs a:eq(1)").tab("show");
                    return false;
                }
            }
        }
    }

    return true;
}

function getPOSpromotion(id) {
    if (id == 0) {
        return;
    }
    $.getJSON("/Promotion/GetSysCommonBusPlanByID?planID=" + id, function (json) {
        if (json.length == 0) {
            return;
        }
        $("#posbind").show();
        var pos = "";
        for (var i = 0; i < json.length; i++) {
            pos += json[0].PromotionName + "、"
        }
        $("#posbind span").html(pos.substring(0, pos.length - 1));
    })
}