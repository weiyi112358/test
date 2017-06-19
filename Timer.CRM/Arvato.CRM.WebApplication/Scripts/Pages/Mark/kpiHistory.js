var dtKpiHistory;

$(document).ready(function () {
    loadKPIResultHistory();
    $("#dt_ActivityKPIHistory").resize(function () {
        $("#dt_ActivityKPIHistory").css({ "width": "100%" })
    });
});

function loadKPIResultHistory() {
    //destory datatable资源之后重新加载新资源
    if (dtKpiHistory) {
        dtKpiHistory.fnDestroy();
    }

    dtKpiHistory = $("#dt_ActivityKPIHistory").dataTable({
        sAjaxSource: "/Mark/SearchActivityKPIResultForPage",
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
        aaSorting: [[10, "desc"]],
        aoColumns: [
            { data: "KPIResultID", title: "计算结果ID", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "KPIID", title: "KPIID", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "KPIName", title: "指标参数", sortable: false, "sClass": "center", sWidth: "15%", bVisible: true },
            { data: "TargetValueType", title: "目标值类型", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "TargetValue", title: "目标值", sortable: false, "sClass": "center", sWidth: "15%" },
            {
                data: null, title: "实际值", sortable: false, "sClass": "center", sWidth: "15%", render: function (r) {
                    if (r.Unit=="%") {
                        return r.CurrentValue * 100;
                    }
                    else {
                        return r.CurrentValue;
                    }
                }
            },
            { data: "Unit", title: "单位", sortable: false, "sClass": "center", sWidth: "10%", bVisible: true },
            { data: "KPIType", title: "计划类型ID", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "KPITypeValue", title: "计划类型名称", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false },
            { data: "ComputeTime", title: "计算时间", sortable: false, "sClass": "center", sWidth: "20%", bVisible: true },
            { data: "ComputeTime", title: "计算时间", sortable: false, "sClass": "center", sWidth: "1%", bVisible: false }
        ],
        fnFixData: function (d) {
            d.push({ name: 'activityID', value: $("#hidActivityIDForKpiHistory").val() });
        }
    });
}