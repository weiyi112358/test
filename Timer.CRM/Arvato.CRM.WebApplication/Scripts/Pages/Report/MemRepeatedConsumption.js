
// 页面初始化
$(function () {
    $("#drpCity").chosen();
    $('#drpStores').chosen();
    $('#drpChannel').chosen();
    $("#txtStartDate, #txtEndDate").datepicker({
        format: 'yyyy-mm' 
    });
    $("#drpCity").chosen().change(function () {
        $('#drpStores').empty();
        var opt = "<option value=''>全部</option>";
        ajax("/Report/GetStoreByCity", { cityCode: $("#drpCity").val() }, function (data) { 
            for (var i = 0; i < data.length; i++) {
                opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
            }
            $('#drpStores').append(opt).chosen().trigger("liszt:updated");
        });
        
    });

    $("#btnExport").on("click", function () {
        $("#exportForm")[0].action = "/Report/ExportRepeatedConsumption";
        $("#exportForm #exprDtStart").val($("#txtStartDate").val()+"-01");
        $("#exportForm #exprDtEnd").val($("#txtEndDate").val() + "-" + getLastDay($("#txtEndDate").val().substring(0, 4), $("#txtEndDate").val().substring(5, 7)));
        $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm")[0].submit();
    });

    //查询操作
    $("#btnSearch").on("click", function () {
        loadRepeatedConsumption();
    });

});
var dtRepeatedConsumption;
// 搜索会员
function loadRepeatedConsumption() {
   
    if (!dtRepeatedConsumption) {
        dtRepeatedConsumption = $('#dtRepeatedConsumption').dataTable({
            sAjaxSource: '/Report/GetMemRepeatedConsumption',
            bAutoWidth: true,
            bSort: false, //不排序
            bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true, //每次请求后台数据
            bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 20, //
            aoColumns: [
                { data: "MonthDate", title: "月份", "sClass": "center", sortable: false },
                {
                    data: "FirstSpendMoney", title: "首次消费金额", "sClass": "center FirstSpendMoney", sortable: false, render: function (r) {
                        return formatNumber(r, true);
                    }
                },
                {
                    data: "FirstSpendTradeNumber", title: "首次消费交易数", "sClass": "center FirstSpendTradeNumber", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "RepeatedSpendMoney", title: "重复消费金额", "sClass": "center RepeatedSpendMoney", sortable: false, render: function (r) {
                        return formatNumber(r, true);
                    }
                },
                {
                    data: "RepeatedSpendTradeNumber", title: "重复消费交易数", "sClass": "center RepeatedSpendTradeNumber", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "RepeatedSpendPeople", title: "重复消费人数", "sClass": "center RepeatedSpendPeople", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "TotalSpendMoney", title: "总消费金额", "sClass": "center TotalSpendMoney", sortable: false, render: function (r) {
                        return formatNumber(r, true);
                    }
                },
                {
                    data: "TotalTradePeople", title: "总交易人数(去重)", "sClass": "center TotalTradePeople", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "TotalTradeNumber", title: "总交易数", "sClass": "center TotalTradeNumber", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
                {
                    data: "FristTradeGuestUnitPrice", title: "首次消费客单价", "sClass": "center FristTradeGuestUnitPrice", sortable: false, render: function (r) {
                        return formatNumber(r, true);
                    }
                },
                {
                    data: "RepeatedTradeGuestUnitPrice", title: "重复消费客单价", "sClass": "center RepeatedTradeGuestUnitPrice", sortable: false, render: function (r) {
                        return formatNumber(r, true);
                    }
                },
                {
                    data: "RepeatedTradeProportion", title: "重复消费占比", "sClass": "center RepeatedTradeProportion", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                }
            ],
            fnDrawCallback: function (nRow) {
                var datalength = nRow.aoData.length;
                if (datalength == 0) {
                    return;
                }  
            },
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                /* Append the grade to the default row class name */
                //数据加载完成后的最后一行处理
             
                return nRow;
            },
            fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                var qryopt = {
                    dateStart: $("#txtStartDate").val()+ "-01",
                    dateEnd: $("#txtEndDate").val() +"-"+ getLastDay($("#txtEndDate").val().substring(0, 4), $("#txtEndDate").val().substring(5, 7)),
                    channel: addqout($("#drpChannel").val()).toString(),
                    area: $("#drpArea").val(),
                    city: $("#drpCity").val(),
                    store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString()
                };
                var d = $.extend({}, fixData(aoData), qryopt);
                ajax(sSource, d, function (data) {
                    fnCallback(data);
                });
            }
        });
    } else {
        dtRepeatedConsumption.fnDraw();
    }
}


 