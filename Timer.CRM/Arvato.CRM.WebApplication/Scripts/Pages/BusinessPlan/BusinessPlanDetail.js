$(document).ready(function () {
    searchBusinessPlanResultFromBack();
    loadBusinessPlanActivityList();
    hidemenu();//默认隐藏菜单
    $("#btnReturn").click(function () {
        window.location.href = "/BusinessPlan/Index";
    });
    $("#dt_BusinessplanDetail").resize(function () {
        $("#dt_BusinessplanDetail").css({ "width": "100%" })
    });
    //self.location = document.referrer;
});


var dtBusinessPlanDetail,
    dtBusinessPlanActivity;

function loadBusinessPlanResultList(businessPlanResultList) {
    //destory datatable资源之后重新加载新资源
    if (dtBusinessPlanDetail) {
        dtBusinessPlanDetail.fnDestroy();
    }

    dtBusinessPlanDetail = $("#dt_BusinessplanDetail").dataTable({
        sScrollX: "100%",
        sScrollXInner: "100%",
        bScrollCollapse: true,
        bInfo: true,
        bServerSide: false,
        bLengthChange: false,
        bPaginate: true,
        iDisplayLength: 10,
        aaData: businessPlanResultList,
        aaSorting: [[10, "desc"]],
        aoColumns: [
            { data: "KPIResultID", title: "计算结果ID", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "KPIID", title: "KPIID", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "KPIName", title: "指标参数", sortable: false, "sClass": "center", sWidth: "15%", bVisible: true },
            { data: "TargetValueType", title: "目标值类型", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "TargetValue", title: "目标值", sortable: false, "sClass": "center", sWidth: "15%" },
            { data: "CurrentValue", title: "实际值", sortable: false, "sClass": "center", sWidth: "15%" },
            { data: "Unit", title: "单位", sortable: false, "sClass": "center", sWidth: "10%", bVisible: true },
            { data: "KPIType", title: "计划类型ID", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "KPITypeValue", title: "计划类型名称", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "ComputeTime", title: "计算时间", sortable: false, "sClass": "center", sWidth: "20%", bVisible: true },
            { data: "ComputeTime", title: "计算时间", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false }
        ],
        fnFixData: function (d) {
        }
    });
}

function searchBusinessPlanResult() {
    var data = $("#hidBusinessPlanResult").val();
    if (data) {
        loadBusinessPlanResultList(JSON.parse(data));
    } else {
        loadBusinessPlanResultList(null);
    }
}

function searchBusinessPlanResultFromBack() {
    ajax("/BusinessPlan/SearchBusinessPlanResult", { planID: $("#hidBusinessPlanID").val() }, function (data) {
        loadBusinessPlanResultList(data);
    });
}

function loadBusinessPlanActivityList() {
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
            d.push({ name: "planID", value: $("#hidBusinessPlanID").val() });
        }
    });
}

