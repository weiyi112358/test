
// 页面初始化
$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    $("#drpArea,#drpCity,#drpStores,#drpChannels,#drpGrade").chosen();

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




    //查询操作
    $("#btnSearch").click(function () {
        SearchCouponDetail();
    });

});

var dtSearch;
function SearchCouponDetail() {
    qryopt = {
        sendCoupondateStart: $("#txtSendCouponStartDate").val(),//发券起期
        sendCoupondateEnd: $("#txtSendCouponEndDate").val(),//发券止期
        grade: $("#drpGrade").val(),//会员等级
        area: $("#drpArea").val(),//大区
        city: $("#drpCity").val(),//城市
        store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString(),
        channel: $("#drpChannels").val() == null ? "" : addqout($("#drpChannels").val()).toString()
    };
    if (!dtSearch) {

        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetCouponDetail',
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
               { data: "Area", title: "大区", sortable: false },
               { data: "Channel", title: "渠道", sortable: false },
               { data: "City", title: "城市", sortable: false },
               { data: "phonenumber", title: "会员手机号", sortable: false },
               { data: "CouponName", title: "优惠券名称", sortable: false },
               { data: "CouponCode", title: "优惠券号", sortable: false },

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