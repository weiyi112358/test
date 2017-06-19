//市场活动跟踪

$(function () {
    $("#txtStartDate,#txtEndDate").datepicker({ format: "yyyy-mm" });

    $("#btnSearch").bind("click", function () {
        SearchMarketActivityTracking();
    });

    //查询
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportMarketActivityTracking";
        $("#exportForm #expStartDate").val($("#txtStartDate").val() + "-01");
        $("#exportForm #expEndedDate").val($("#txtEndDate").val() + "-" + getLastDay($("#txtEndDate").val().substring(0, 4), $("#txtEndDate").val().substring(5, 7)));
        $("#exportForm")[0].submit();
    });
});

var dtSearch;
function SearchMarketActivityTracking() {
    qryopt = {
        startDate: $("#txtStartDate").val()+"-01",//查询时间
        endDate: $("#txtEndDate").val() + "-" + getLastDay($("#txtEndDate").val().substring(0, 4), $("#txtEndDate").val().substring(5, 7)),
    };
    if (!dtSearch) {
        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetMarketActivityTracking',
            sScrollX: "100%",
            sScrollXInner: "100%",
            bSort: false,   //不排序
            bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
               { data: "ActDate", title: "活动时间", Width: "57px", sortable: false },
               { data: "ActName", title: "活动名称", sortable: false },
               {
                   data: "ConsumerNumber", title: "细分会员群数量", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "SmsNumber", title: "短信推送数量", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "TotActMoney", title: "回归销售额累计", sortable: false, render: function (r) {
                       return formatNumber(r,true);
                   }
               },
               {
                   data: "TotActRate", title: "占总体销售额比（会员和非会员）", sortable: false, render: function (r) {
                       return convertRateData(r);
                   }
               },
               {
                   data: "TradeRate", title: "占总体买单数比", sortable: false, render: function (r) {
                       return convertRateData(r);
                   }
               },
               {
                   data: "ManNumber", title: "男性会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "WomenNumber", title: "女性会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "SecretNumber", title: "保密会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age1Number", title: "18-22周岁会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age2Number", title: "23-29周岁会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age3Number", title: "30-35周岁会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age4Number", title: "35-39周岁会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age5Number", title: "40+周岁会员数", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "UnknownNumber", title: "未知年龄会员", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
            ],
            fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                var d = $.extend({}, fixData(aoData), qryopt);
                ajax(sSource, d, function (data) {
                    fnCallback(data);
                });
            }
        });
    } else {
        dtSearch.fnDraw();
    }
}