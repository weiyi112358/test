
// 页面初始化
$(function () {
    $("#txtStartDate, #txtEndDate").datepicker();
    $("#drpArea,#drpCity,#drpStores,#drpChannel").chosen();


    $("#drpCity").change(function () {
        $('#drpStores').empty();
        var opt = "<option value=''>全部</option>";
        ajax("/Report/GetStoreByCity", { cityCode: $("#drpCity").val() }, function (data) {

            for (var i = 0; i < data.length; i++) {
                opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
            }
            $('#drpStores').append(opt).chosen().trigger("liszt:updated");

            //$("#drpStores").trigger("liszt:updated");
        });

    });
});
$(document).ajaxSuccess(function (event, xhr, settings) {
    if (settings.url === "searchMem") {
        alert("数据填充完成");
    }
});

//查询操作
$("#btnSearch").on("click", function () {
    loadPriceSegmentDistribution();
});
var dtPriceSegmentDistributionOffline;
// 搜索会员
function loadPriceSegmentDistribution() {
    qryopt = {
        dateStart: $("#txtStartDate").val(),
        dateEnd: $("#txtEndDate").val(),
        channel: addqout($("#drpChannel").val()).toString(),
        city: addqout($("#drpCity").val()).toString(),
        area: addqout($("#drpArea").val()).toString(),
        store: addqout($("#drpStores").val()).toString()
    };
    if (!dtPriceSegmentDistributionOffline) {
        dtPriceSegmentDistributionOffline = $('#dtPriceSegmentDistributionOffline').dataTable({
            sAjaxSource: '/Report/GetPriceSegmentDistributionOffline',
            bAutoWidth: false,
            bSort: false,   //不排序
            bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: false, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
                { data: "AreaNameStore", title: "区域", sortable: false },
                { data: "City", title: "城市", sortable: false },
                { data: "Store", title: "店铺", sortable: false },
                {
                    data: "UnderHundred", title: "100以下", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "HundredOneToThree", title: "100-300", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "HundredThreeToSix", title: "300-600", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "HundredSixToTwelve", title: "600-1200", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "HundredTwelveToTwenty", title: "1200-2000", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "HundredTwentyToForty", title: "2000-4000", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "OverFortyHundred", title: "4000以上", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                }
            ],

            fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                var d = $.extend({}, fixData(aoData), qryopt);
                ajax(sSource, d, function (data) {
                    fnCallback(data);
                });
            }
        });
    } else {
        dtPriceSegmentDistributionOffline.fnDraw();
    }
}



$("#btnExport").on("click", function () {
    $("#exportForm")[0].action = "/Report/ExportPriceSegmentDistributionOffline";
    $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
    $("#exportForm #exprDtStart").val($("#txtStartDate").val());
    $("#exportForm #exprDtEnd").val($("#txtEndDate").val());
    $("#exportForm")[0].submit();
});


