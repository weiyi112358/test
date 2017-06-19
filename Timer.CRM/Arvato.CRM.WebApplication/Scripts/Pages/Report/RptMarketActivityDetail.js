
// 页面初始化
$(function () {
    $("#marStartDate,#marEndDate,#comStartDate,#comEndDate").datepicker();
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
        SearchUseCouponCount();
    });

});

var dtSearch;
function SearchUseCouponCount() {
    qryopt = {
        marStartDate: $("#marStartDate").val(),
        marEndDate: $("#marEndDate").val(),
        comStartDate: $("#comStartDate").val(),
        comEndDate: $("#comEndDate").val(),
        grade: $("#drpGrade").val(),//会员等级
        area: $("#drpArea").val(),//大区
        city: $("#drpCity").val(),//城市
        store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString(),
        channel: $("#drpChannels").val() == null ? "" : addqout($("#drpChannels").val()).toString()
    };
    if (!dtSearch) {

        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetMarketActivityDetail',
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
               { data: "AreaActivityName", title: "活动名称", sortable: false },
               { data: "ActivityDate", title: "活动执行时间", sortable: false },
               {
                   data: "SegmentAmonut", title: "细分会员数量", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
                 {
                     data: "MsgPushAmount", title: "短信推送数量", sortable: false, render: function (r) {
                         return formatNumber(r);
                     }
                 },
               {
                   data: "RealtiveAmount", title: "拉动消费金额", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "ResponseRate", title: "响应率", sortable: false, render: function (r) {
                       return convertRateData(r);
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