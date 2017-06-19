//结算分摊统计

var dtSearch;
var SettlementShare = {
    isNum: function (num) {
        num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
        return num;
    },
   

    
    SearchSettlementShare: function () {
        qryopt = {
            //searchdateStart: $("#txtStartDate").val(),//消费起期
            //searchdateEnd: $("#txtEndDate").val(),
            //channel: addqout($("#drpChannel").val()).toString(),//渠道
            //area: $("#drpArea").val(),//大区
            //city: $("#drpCity").val(),//城市
            store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString()//店铺

        };
        if (!dtSearch) {
            dtSearch = $('#dt_search').dataTable({
                sAjaxSource: '/Report/GetSettlementShareList',
                bSort: false,   //不排序
                bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true,  //每次请求后台数据
                bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 8,//
                aoColumns: [
                   { data: "customername", title: "姓名", sortable: false },
                   { data: "customermobile", title: "手机号", sortable: false },
                   { data: "Tradecode", title: "交易单号", sortable: false },
                   {
                       data: "OrderDate", title: "订单时间", sortable: false, render: function (r) {
                           return r.substring(0, 4) + "-" + r.substring(4, 6) + "-" + r.substring(6)
                       }
                   },
                   { data: "StoreName", title: "交易门店名称", sortable: false },
                   { data: "DeductType", title: "扣减类型", sortable: false },
                   { data: "DeductValue", title: "扣减金币", sortable: false },
                   { data: "SoureChargeOrderCode", title: "原储值单号", sortable: false },
                   { data: "ChargeValue", title: "原储值单储值金币", sortable: false },
                   { data: "PresentValue", title: "原储值单赠送金币", sortable: false },
                 
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
    //$("#txtStartDate,#txtEndDate").datepicker({
    //    format: "yyyy-mm-dd",
        //startDate: year + "-01",
        //endDate: year + "-12"
        // startView: 1,
    //});


    //$("#drpCity").chosen();
    //$("#drpChannel").chosen();
    //$("#drpArea").chosen();
    $("#drpStores").chosen();

    //$("#drpCity").change(function () {
    //    $('#drpStores').empty();
    //    var opt = "<option value=''>全部</option>";
    //    ajax("/Report/GetStoreByCity", { cityCode: $("#drpCity").val() }, function (data) {
    //        for (var i = 0; i < data.length; i++) {
    //            opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
    //        }
    //        $('#drpStores').append(opt).chosen().trigger("liszt:updated");
    //    });
    //});
    //查询
    $("#btnSearch").bind("click", function () {
        SettlementShare.SearchSettlementShare();
    });

    //导出
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportStoreConsumptionDutyCount";
        //$("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        //$("#exportForm #exprArea").val($("#drpArea").val());
        //$("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        //$("#exportForm #exprDtStart").val($("#txtStartDate").val());
        //$("#exportForm #exprDtEnd").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    });
});



