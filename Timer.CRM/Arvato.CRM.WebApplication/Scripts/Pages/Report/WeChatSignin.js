var tb_result,
    dt_detail;

$(function () {
    var time = new Date();
    var month = "00" + time.getMonth();
    var datestart = time.getFullYear() + "-" + month.substring(month.length-2,2) + "-01";
    var dateend = time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate();
    $("#txtStartDate").val(datestart)
    $("#txtEndDate").val(dateend)
    $("#txtStartDate").datepicker({
    });
    $("#txtEndDate").datepicker();
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    //查询操作
    $("#btnSearch").click(function () {
        searchMem();
    });

})

function searchMem() {
    if ($("#drpactionname").val() == "") {
        $.dialog("请选择活动名称！");
        return;
    }
    if (tb_result) {
        tb_result.fnDraw();
    } else {
        tb_result = $("#dt_search").dataTable({
            sAjaxSource: '/Report/GetWXReport',
            bInfo: false,
            bServerSide: true,
            bLengthChange: false,
            bPaginate: true,
            bFilter: false,
            iDisplayLength: 20,
            select: true,
            bSort: false,
            aoColumns: [
                {
                    data: "ActionName", title: "活动名称", sortable: false,
                },
                 {
                     data: "SignDate", title: "签到日期", sortable: false,
                 },
                {
                    data: "fscs", title: "发送次数", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "bpcqdcs", title: "本批次签到次数", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "qdcs", title: "签到次数", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "hyqdcs", title: "会员签到次数", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "fhyqdcs", title: "非会员签到次数", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "qdl", title: "签到率", sortable: false,render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: null, title: "操作", sortable: false, render: function (r) {
                        return "<button class='btn' onclick='showdetail(\"" + r.SignDate + "\")'>查看明细</button>&nbsp;<button class='btn export btn-gebo' onclick='Export(\"" + r.SignDate + "\",\"" + r.ActionName + "\")'>导出</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'StartTime', value: $("#txtStartDate").val() });
                d.push({ name: 'EndTime', value: $("#txtEndDate").val() });
                d.push({ name: 'ActCode', value: $("#drpactionname").val() });
            },
           
        });
    }
}

function showdetail(date) {
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_detail",
        inline: true
    });
    $("#hddate").val(date);
    if (dt_detail) {
        dt_detail.fnDraw();
    }
    else {
        dt_detail = $("#dt_detail").dataTable({
            sAjaxSource: '/Report/GetWXbyActionCode',
            bInfo: false,
            bServerSide: true,
            bLengthChange: false,
            bSort: false,
            bPaginate: true,
            bFilter: false,
            iDisplayLength: 8,
            select: true,
            //aaSorting: [[0, "desc"]],
            aoColumns: [
             { data: 'Mobile', title: "手机号", sortable: false, sWidth: "120" },
             { data: 'OpenID', title: "微信号", sortable: false, sWidth: "120" },
             { data: 'SignDate', title: "签到时间", sortable: false, sWidth: "200" },
              {
                  data: 'IsMem', title: "是否会员", sortable: false, sWidth: "120",
                  
              },
              {
                  data: 'IsAttendActive', title: "是否本次活动", sortable: false, sWidth: "120",
              }

            ],
            fnFixData: function (d) {
                if ($("#hddate").val()=="null") {
                    d.push({ name: 'startDate', value: $("#txtStartDate").val() + " 00:00:00" });
                    d.push({ name: 'endDate', value: $("#txtEndDate").val() + " 23:59:59" });
                    d.push({ name: 'ActionCode', value: $("#drpactionname").val() });
                }
                else {
                    d.push({ name: 'startDate', value: $("#hddate").val() + " 00:00:00" });
                    d.push({ name: 'endDate', value: $("#hddate").val() + " 23:59:59" });
                    d.push({ name: 'ActionCode', value: $("#drpactionname").val() });
                }               
            },
        });
    }
}

function Export(date,actionName) {
    $("#exportForm")[0].action = "/Report/ExportWxSignDetail";
    $("#exportForm #expractionCode").val($("#drpactionname").val());
    if (actionName=="合计") {
        $("#exportForm #exprstartdate").val($("#txtStartDate").val() + " 00:00:00");
        $("#exportForm #exprenddate").val($("#txtStartDate").val() + " 23:59:59");
    }
    else {
        $("#exportForm #exprstartdate").val(date+" 00:00:00");
        $("#exportForm #exprenddate").val(date+" 23:59:59");
    }
    $("#exportForm")[0].submit();
};
