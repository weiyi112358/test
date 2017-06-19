//门店消费占比统计

var dtSearch;
var StoreConsumptionDutyCount = {
    isNum: function (num) {
        num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
        return num;
    },
    //AmountAccount: function () {
    //var $ExpendtureMem;//会员消费金额
    //var $ConsumptionMoneySumMem;//会员消费累计金额
    //var $DutyTotal;//占比1
    //var $ConsumptionTotalMoney;//消费总金额
    //var $DutyExpendture;//占比2
    //var tabstr = "";

    //$("#dt_search tr").each(function () {
    //    var $this = $(this);
    //    $ExpendtureMem =StoreConsumptionDutyCount.isNum($ExpendtureMem) + StoreConsumptionDutyCount.isNum($this.find(".ExpendtureMem").html());
    //    $ConsumptionMoneySumMem =StoreConsumptionDutyCount.isNum($ConsumptionMoneySumMem) + StoreConsumptionDutyCount.isNum($this.find(".ConsumptionMoneySumMem").html());
    //    $DutyTotal = ($ConsumptionMoneySumMem == 0) ? 0 : ((StoreConsumptionDutyCount.isNum($ExpendtureMem) /StoreConsumptionDutyCount.isNum($ConsumptionMoneySumMem)) * 100);
    //    $ConsumptionTotalMoney = StoreConsumptionDutyCount.isNum($ConsumptionTotalMoney) + StoreConsumptionDutyCount.isNum($this.find(".ConsumptionTotalMoney").html());
    //    $DutyExpendture = $ConsumptionTotalMoney == 0 ? 0 : ((StoreConsumptionDutyCount.isNum($ExpendtureMem) / StoreConsumptionDutyCount.isNum($ConsumptionTotalMoney)) * 100);
    //});
    //tabstr += "<tr>" +
    //    "<td colspan='2'  style='font-weight:bold;text-align:center'>合计</td>" +
    //    "<td>" + $ExpendtureMem.toFixed(2) + "</td>" +
    //    "<td>" + $ConsumptionMoneySumMem.toFixed(2) + "</td>" +
    //    "<td>" + convertRateData($DutyTotal) + "</td>" +
    //    "<td>" + $ConsumptionTotalMoney.toFixed(2) + "</td>" +
    //    "<td>" + convertRateData($DutyExpendture) + "</td>" +
    //    "</tr>";

    //$('#dt_search tbody').append(tabstr);
    //},
    SearchStoreConsumptionDutyCount: function () {
        qryopt = {
            searchdateStart: $("#txtStartDate").val(),//消费起期
            searchdateEnd: $("#txtEndDate").val(),
            channel: addqout($("#drpChannel").val()).toString(),//渠道
            area: $("#drpArea").val(),//大区
            city: $("#drpCity").val(),//城市
            store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString()//店铺
        };
        if (!dtSearch) {
            dtSearch = $('#dt_search').dataTable({
                sAjaxSource: '/Report/GetStoreConsumptionDutyCount',
                bSort: false,   //不排序
                bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true,  //每次请求后台数据
                bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 20,//
                aoColumns: [
                   { data: "City", title: "城市", sortable: false },
                   { data: "Store", title: "店铺", sortable: false },
                   //{
                   //    data: "Expendture_Mem", title: "会员消费金额", sortable: false, sClass: "ExpendtureMem", render: function (r) {
                   //        return formatNumber(r);
                   //    }
                   //},
                   {
                       data: "ConsumptionMoneySum_Mem", title: "会员消费金额（区间）", sortable: false, sClass: "ConsumptionMoneySumMem", render: function (r) {
                           return formatNumber(r, true);
                       }
                   },
                   {
                       data: "DutyTotal", title: "会员消费额在各门店占比", sortable: false, sClass: "DutyTotal", render: function (r) {
                           return convertRateData(r);
                       }
                   },
                   {
                       data: "ConsumptionTotalMoney", title: "总消费金额", sortable: false, sClass: "ConsumptionTotalMoney", render: function (r) {
                           return formatNumber(r, true);
                      }
                   },
                   {
                       data: "DutyExpendture", title: "门店会员贡献率", sortable: false, sClass: "DutyExpendture", render: function (r) {
                           return convertRateData(r);
                       }
                   }
                ],
                fnDrawCallback: function (nRow) {
                    var datalength = nRow.aoData.length;
                    if (datalength == 0) {
                        return;
                    }

                    //  StoreConsumptionDutyCount.AmountAccount();
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
    }//查询会员招募
};

//页面初始化
$(function () {
    //var times = new Date();
    //var year = times.getFullYear();
    //loadAllStore();
    $("#txtStartDate,#txtEndDate").datepicker({
        format: "yyyy-mm-dd",
        //startDate: year + "-01",
        //endDate: year + "-12"
        // startView: 1,
    });


    $("#drpCity").chosen();
    $("#drpChannel").chosen();
    $("#drpArea").chosen();
    $("#drpStores").chosen();

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
    //查询
    $("#btnSearch").bind("click", function () {
        StoreConsumptionDutyCount.SearchStoreConsumptionDutyCount();
    });

    //导出
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportStoreConsumptionDutyCount";
        $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm #exprDtStart").val($("#txtStartDate").val());
        $("#exportForm #exprDtEnd").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    });
});



