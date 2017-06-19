//优惠券使用统计

$(function () {
    $("#drpCoupon,#drpArea,#drpCity,#drpStores").chosen();
    
    $("#txtSendCouponStartDate,#txtSendCouponEndDate").datepicker();

    $("#btnSearch").bind("click", function () {
        SearchUseCouponCount();
    });

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
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportUseCouponCount";
        $("#exportForm #exprCouponName").val($("#drpCoupon").val());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm #exprDtSendCouponStart").val($("#txtSendCouponStartDate").val());
        $("#exportForm #exprDtSendCouponEnd").val($("#txtSendCouponEndDate").val());
        $("#exportForm")[0].submit();
    });

});

var dtSearch;
function SearchUseCouponCount()
{
    qryopt = {
        sendCoupondateStart: $("#txtSendCouponStartDate").val(),//发券起期
        sendCoupondateEnd: $("#txtSendCouponEndDate").val(),//发券止期
        couponname: $("#drpCoupon").val(),//券名
        area: $("#drpArea").val(),//大区
        city: $("#drpCity").val(),//城市
        store: $("#drpStores").val() == null ? "" : $("#drpStores").val() == null ? "" : $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString()//店铺
    };
    if (!dtSearch) {

        dtSearch= $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetUseCouponCount',
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
               { data: "CouponName", title: "券名称", sortable: false },
               {
                   data: "SendNumber", title: "发送数量", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
                 {
                     data: "ReceiveNumber", title: "领用数量", sortable: false, render: function (r) {
                         return formatNumber(r);
                     }
                 },
               {
                   data: "UseNumber", title: "使用数量", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "UseRate", title: "使用率", sortable: false, render: function (r) {
                       return convertRateData(r);
                   }
               },
               {
                   data: "RelativeAmount", title: "拉动消费金额", sortable: false, render: function (r) {
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
        dtSearch.fnDraw();
    }
}

