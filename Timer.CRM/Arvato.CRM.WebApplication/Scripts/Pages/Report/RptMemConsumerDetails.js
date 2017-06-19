//会员消费明细
var ConsumerDetail = {
    isNum: function (num) {
        num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
        return num;
    },
    AmountAccount: function () {
        var $Num;//数量
        var $PayMoney;//应付金额
        var $DiscountMoney;//折扣金额
        var $SettleMoney;//结算金额
        var tabstr = "";

        $("#dt_search tr").each(function () {
            var $this = $(this);
            $Num = ConsumerDetail.isNum($Num) + ConsumerDetail.isNum($this.find(".Num").html());
            $PayMoney = ConsumerDetail.isNum($PayMoney) + ConsumerDetail.isNum($this.find(".PayMoney").html());
            $DiscountMoney = ConsumerDetail.isNum($DiscountMoney) + ConsumerDetail.isNum($this.find(".DiscountMoney").html());
            $SettleMoney = ConsumerDetail.isNum($SettleMoney) + ConsumerDetail.isNum($this.find(".SettleMoney").html());
        });

        tabstr += "<tr>" +
            "<td colspan='6' style='font-weight:bold;text-align:center'>合计</td>" +
            "<td>" + $Num + "</td>" +
            "<td>" + $PayMoney.toFixed(2) + "</td>" +
            "<td>" + $DiscountMoney.toFixed(2) + "</td>" +
            "<td>" + $SettleMoney.toFixed(2) + "</td>" +
            "</tr>";

        $('#dt_search tbody').append(tabstr);
    },
}
//页面初始化
$(function () {
    $("#drpCity").chosen();
    $("#drpChannel").chosen();
    $("#drpStores").chosen();
    $("#drpArea").chosen();
    $("#drpCity").change(function () {
        $('#drpStores').empty();

        ajax("/Report/GetStoreByCity", { cityCode: $("#drpCity").val() }, function (data) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < data.length; i++) {
                opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
            }
            $('#drpStores').append(opt).chosen().trigger("liszt:updated");
        });

    });

    $("#txtConsumerStartDate,#txtConsumerEndDate").datepicker();

    //查询
    $("#btnSearch").bind("click", function () {
        SearchConsumerDetail();
    });

    //导出
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportMemConsumerDetails";
        $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm #exprDtReg").val($("#txtConsumerStartDate").val());
        $("#exportForm #exprDtRegMon").val($("#txtConsumerEndDate").val());
        $("#exportForm")[0].submit();
    });
});
var dtSearch;
//查询会员招募
function SearchConsumerDetail() {
    qryopt = {
        dateConsumptionStart: $("#txtConsumerStartDate").val(),//消费起期
        dateConsumptionEnd: $("#txtConsumerEndDate").val(),//消费止期
        channel: addqout($("#drpChannel").val()).toString(),//渠道
        area: $("#drpArea").val(),//大区
        city: $("#drpCity").val(),//城市
        store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString()//店铺
    };
    if (!dtSearch) {
        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetMemConsumerDetails',
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
               { data: "ConsumptionDate", title: "消费日期", Width: "7%", sortable: false },
               { data: "ConsumptionStore", title: "消费店铺", Width: "7%", sortable: false },
               { data: "ConsumptionChannel", title: "消费渠道", Width: "12%", sortable: false },
               { data: "Mobile", title: "手机号", sortable: false },
               { data: "Name", title: "姓名", sortable: false },
               { data: "No", title: "单据号", sortable: false },
               {
                   data: "Num", title: "数量", sClass: 'Num', sortable: false
               },
               {
                   data: "StandardAmountSales", title: "标准金额", sClass: 'StandardAmountSales', sortable: false, render: function (r) {
                       return formatNumber(r, true);
                   }
               }, {
                   data: "TradeAmoutSales", title: "订单金额", sClass: 'TradeAmoutSales', sortable: false, render: function (r) {
                       return formatNumber(r, true);
                   }
               },
               {
                   data: "PayMoney", title: "应付金额", sClass: 'PayMoney', sortable: false, render: function (r) {
                       return formatNumber(r,true);
                   }
               },
               {
                   data: "DiscountMoney", title: "折扣金额", sClass: 'DiscountMoney', sortable: false, render: function (r) {
                       return formatNumber(r,true);
                   }
               },
               {
                   data: "SettleMoney", title: "结算金额", sClass: 'SettleMoney', sortable: false, render: function (r) {
                       return formatNumber(r,true);
                   }
               },
                
            ],
            fnDrawCallback: function (nRow) {
                //var datalength = nRow.aoData.length;
                //if (datalength == 0) {
                //    return;
                //}
                //ConsumerDetail.AmountAccount();
            },
            fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                var d = $.extend({}, fixData(aoData), qryopt);
                ajax(sSource, d, function (data) {
                    fnCallback(data);
                });
            }
        })
    }
    else {
        dtSearch.fnDraw();
    }

}