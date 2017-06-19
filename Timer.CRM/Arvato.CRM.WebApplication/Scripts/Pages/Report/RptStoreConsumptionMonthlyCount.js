//门店消费月统计

$(function () {
    $("#txtConsumptionStartDate,#txtConsumptionEndDate").datepicker({ format: "yyyy-mm" });
    $("#drpChannel,#drpArea,#drpCity,#drpStores").chosen();

    $("#drpCity").change(function () {
        $('#drpStores').empty();
        var opt = "<option value=''>全部</option>";
        ajax("/Report/GetStoreByCity", { cityCode: $("#drpCity").val() }, function (data) {
            for (var i = 0; i < data.length; i++) {
                opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
            }
            $('#drpStores').append(opt).chosen().trigger("liszt:updated");
        });

    });
    $("#btnSearch").bind("click", function () {
        StoreConsumptionMonCount.SearchStoreConsumptionMonCount();
    });

    //查询
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportStoreConsumptionMonthlyCount";
        $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm #exprDtConsumptionStart").val($("#txtConsumptionStartDate").val()+"-01");
        $("#exportForm #exprDtConsumptionEnd").val($("#txtConsumptionEndDate").val() + "-" + getLastDay($("#txtConsumptionEndDate").val().substring(0, 4), $("#txtConsumptionEndDate").val().substring(5, 7)));
        $("#exportForm")[0].submit();
    });
});

var dtSearch;
var StoreConsumptionMonCount = {
    isNum: function (num) {
        num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
        return num;
    },
    AmountAccount: function () {
        var $TotalSales; //总销售额
        var $AmountOfSalesMem; //会员销售额
        var $SalesContributionMem; //会员贡献率
        var $SalesAmplificationHbMem; //环比增幅
        var $GuestPriceMem; //客单价
        var $GuestOrdersMem;//客单量
        var $TurnoverRateMem;//交易额增长率
        var $TBAmount, $HBAmount;
        var tabstr = "";
        $("#dt_search tr").each(function () {
            var $this = $(this);
            $TotalSales = StoreConsumptionMonCount.isNum($TotalSales) + StoreConsumptionMonCount.isNum($this.find(".TotalSales").html());
            $AmountOfSalesMem = StoreConsumptionMonCount.isNum($AmountOfSalesMem) + StoreConsumptionMonCount.isNum($this.find(".AmountOfSales_Mem").html());
            $SalesContributionMem = StoreConsumptionMonCount.isNum($SalesContributionMem) + StoreConsumptionMonCount.isNum($this.find(".SalesContribution_Mem").html());
            $SalesAmplificationHbMem = StoreConsumptionMonCount.isNum($SalesAmplificationHbMem) + StoreConsumptionMonCount.isNum($this.find(".SalesAmplificationHB_Mem").html());
            $GuestPriceMem = StoreConsumptionMonCount.isNum($GuestPriceMem) + StoreConsumptionMonCount.isNum($this.find(".GuestPrice_Mem").html());
            $GuestOrdersMem = StoreConsumptionMonCount.isNum($GuestOrdersMem) + StoreConsumptionMonCount.isNum($this.find(".GuestOrders_Mem").html());
            $TurnoverRateMem = StoreConsumptionMonCount.isNum($TurnoverRateMem) + StoreConsumptionMonCount.isNum($this.find(".TurnoverRate_Mem").html());
            $HBAmount = StoreConsumptionMonCount.isNum($HBAmount) + StoreConsumptionMonCount.isNum($this.find(".HBAmount").html());
            $TBAmount = StoreConsumptionMonCount.isNum($TBAmount) + StoreConsumptionMonCount.isNum($this.find(".TBAmount").html());
        });
        tabstr += "<tr>" +
            "<td colspan='2'  style='font-weight:bold;text-align:center'>合计</td>" +
            "<td>" + $TotalSales.toFixed(2) + "</td>" +
            "<td>" + $AmountOfSalesMem.toFixed(2) + "</td>" +
            "<td>" + $SalesContributionMem.toFixed(2) + "</td>" +
            "<td>" + ($SalesAmplificationHbMem > 0 ? (($AmountOfSalesMem - $SalesAmplificationHbMem) / $SalesAmplificationHbMem).toFixed(2) : 0.00) + "</td>" +
            "<td>" + ($TurnoverRateMem > 0 ? (($AmountOfSalesMem - $TurnoverRateMem) / $TurnoverRateMem).toFixed(2) : 0.00) + "</td>" +
            "<td>" + $GuestPriceMem.toFixed(2) + "</td>" +
            "<td>" + $GuestOrdersMem.toFixed(2) + "</td>" +
            "</tr>";
        $('#dt_search tbody').append(tabstr);
    },

    SearchStoreConsumptionMonCount: function () {
        qryopt = {
            consumptiondateStart: $("#txtConsumptionStartDate").val()+"-01", //消费起期
            consumptiondateEnd: $("#txtConsumptionEndDate").val() + "-" + getLastDay($("#txtConsumptionEndDate").val().substring(0, 4), $("#txtConsumptionEndDate").val().substring(5, 7)),
            channel: addqout($("#drpChannel").val()).toString(), //渠道
            area: $("#drpArea").val(), //大区
            city: $("#drpCity").val(), //城市
            store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString() //店铺
        };
        if (!dtSearch) {
            dtSearch = $('#dt_search').dataTable({
                sAjaxSource: '/Report/GetStoreConsumptionMonthlyCount',
                bSort: false, //不排序
                bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true, //每次请求后台数据
                bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 20, //
                aoColumns: [
                    { data: "City", title: "城市", sortable: false },
                    { data: "Store", title: "店铺", sortable: false },
                    {
                        data: "TotalSales", title: "总销售额", sortable: false, sClass: "TotalSales", render: function (r) {
                            return formatNumber(r, true);
                        }
                    },
                    {
                        data: "AmountOfSales_Mem", title: "会员销售额", sortable: false, sClass: "AmountOfSales_Mem", render: function (r) {
                            return formatNumber(r, true);
                        }
                    },
                    {
                        data: "SalesContribution_Mem", title: "会员销售贡献率", sortable: false, sClass: "AmountOfSales_Mem", render: function (r) {
                            return convertRateData(r);
                        }
                    },
                    {
                        data: "SalesAmplificationHB_Mem", title: "会员销售环比", sortable: false, sClass: "SalesAmplificationHB_Mem", render: function (r) {
                            return convertRateData(r);
                        }
                    },
                    {
                        data: "TurnoverRate_Mem", title: "会员销售同比", sortable: false, sClass: "TurnoverRate_Mem", render: function (r) {
                            return convertRateData(r);
                        }
                    },
                    {
                        data: "GuestPrice_Mem", title: "会员客单价", sortable: false, sClass: "GuestPrice_Mem", render: function (r) {
                            return formatNumber(r, true);
                        }
                    },
                    {
                        data: "GuestOrders_Mem", title: "会员客单量", sortable: false, sClass: "GuestOrders_Mem", render: function (r) {
                            return formatNumber(r,true);
                        }
                    },
                    { data: "HBAmount", bVisible: false, sClass: "HBAmount" },
                    { data: "TBAmount", bVisible: false, sClass: "TBAmount" }
                ],
                fnDrawCallback: function () {
                   // StoreConsumptionMonCount.AmountAccount();
                },
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
};