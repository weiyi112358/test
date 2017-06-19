//会员非会员销售占比统计


$(function () {
    $("#txtConsumptionStartDate,#txtConsumptionEndDate").datepicker({
        format: "yyyy-mm-dd"
    });
   

    //查询
    $("#btnSearch").bind("click", function () {
        if (validDate()) {
            SearchMemToNonMemSalesDutyCount();
        };
    });

    //导出
    $("#btnExport").bind("click", function () {
        if (validDate()) {
            $("#exportForm")[0].action = "/Report/ExportMemToNonMemSalesDutyCount";
            $("#exportForm #expDtConsumptionStart").val($("#txtConsumptionStartDate").val() + "-01");
            $("#exportForm #expDtConsumptionEnd").val($("#txtConsumptionEndDate").val()); // + "-" + getLastDay($("#txtConsumptionEndDate").val().substring(0, 4), $("#txtConsumptionEndDate").val().substring(5, 7))
            $("#exportForm #expdrCustomeSource").val($("#drCustomeSource").val());
            $("#exportForm")[0].submit();
        }
    });
});

var dtSearch;
function SearchMemToNonMemSalesDutyCount()
{
    qryopt = {
        dateConsumptionStart: $("#txtConsumptionStartDate").val()+"-01",//消费日期起期
        dateConsumptionEnd: $("#txtConsumptionEndDate").val(),//消费日期止期+ "-" + getLastDay($("#txtConsumptionEndDate").val().substring(0, 4), $("#txtConsumptionEndDate").val().substring(5, 7))
        customerSource: $("#drCustomeSource").val()
    };
    if (!dtSearch) {

        dtSearch=$('#dt_search').dataTable({
            sAjaxSource: '/Report/GetMemToNonMemSalesDutyCount',
            bSort: false,   //不排序
            bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: false, //显示分页信息
            iDisplayLength: 2,//
            aoColumns: [
               { data: "Type_Mem", title: "种类", sortable: false },
               {
                   data: "Expendture_Mem", title: "消费额", sortable: false, render: function (r) {
                       return formatNumber(r, true);
                   }
               },
               {
                   data: "ConsumptionContribution_Mem", title: "消费贡献率（占总体）", sortable: false, render: function (r) {
                       return convertRateData(r);
                   }
               },
               {
                   data: "ExpendtureSum_Mem", title: "总计消费额", sortable: false, render: function (r) {
                       return formatNumber(r, true);
                   }
               },
               {
                   data: "ConsumptionContributionSum_Mem", title: "总计消费贡献率（占总体）", sortable: false, render: function (r) {
                       return convertRateData(r);
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
function validDate() {
  var date=  $("#txtConsumptionStartDate").val()+"-01";
  var selectdate =  Date.parse(date.replace(/-/g, "/"))
  var datestart = "2015-09-01";
  var startDate = Date.parse(datestart.replace(/-/g, "/"))
    if (selectdate < startDate) {
        $.dialog("消费日期起期必须大于2015年9月1号");
        $("#txtConsumptionStartDate").val("2015-09-01");
        return false;
    }
    var endDate = new Date($("#txtConsumptionEndDate").val());
    if(endDate < startDate){
        $.dialog("消费日期起期不能小于消费止期");
        return false;
    }
    return true;
}