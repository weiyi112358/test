var dtContributionRate;
$(function () {
    $("#txtStartDate, #txtEndDate").datepicker();
    $("#drpCity").change(function () {
        $('#drpStores').empty();
        var opt = "<option value=''>全部</option>";
        ajax("/Report/GetStoreByCity", { cityCode: $("#drpCity").val() }, function (data) {

            for (var i = 0; i < data.length; i++) {
                opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
            }
            $('#drpStores').append(opt).trigger("liszt:updated");
        });
    });

    $("#drpChannel,#drpArea,#drpCity,#drpStores").chosen();
    $("#btnExport").bind("click", function () {
        var drpSearchDateTimeType = $("#drpSearchDateTimeType").val();
        $("#exportForm")[0].action = "/Report/ExportContributionRate";
        $("#exportForm #exprMonthDate").val(drpSearchDateTimeType == "1" ? ($("#txtMonthDate").val() + "-01") : "");
        $("#exportForm #exprYearDate").val(drpSearchDateTimeType == "2" ? ($("#txtYearDate").val() + "-01-01") : "");
        $("#exportForm")[0].submit();
    });

    $("#drpSearchDateTimeType").bind("change", function () {
        var drpSearchDateTimeType = $("#drpSearchDateTimeType").val();
        if (drpSearchDateTimeType == "1") {
            $('#txtMonthDate').show();
            $("#txtYearDate").hide();
        }
        if (drpSearchDateTimeType == "2") {
            $('#txtMonthDate').hide();
            $("#txtYearDate").show();
        }
    });
});
function loadContributionRate() {
    var drpSearchDateTimeType = $("#drpSearchDateTimeType").val();
    qryopt = {
        yearDate: $("#txtStartDate").val(),
        monthDate: $("#txtEndDate").val(),
        channel: $("#drpChannel").val(),
        area: $("#drpArea").val(),
        city: $("#drpCity").val(),
        store: $("#drpStores").val(),
        customerlevel: $("#drpCustomerLevel").val()
    }
    if (!dtContributionRate) {
        dtContributionRate = $('#dtContributionRate').dataTable({
            sAjaxSource: '/Report/GetContributionRate',
            bSort: false, //不排序
            bInfo: false, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true, //每次请求后台数据
            bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
            bPaginate: false, //显示分页信息
            iDisplayLength: 2, //
            aoColumns: [
                { data: "MemberLevel", title: "会员等级", Width: "7%", sortable: false },
                {
                    data: "Spending", title: "消费额", Width: "7%", sortable: false, render: function (r) {
                        return formatNumber(r,true);
                    }
                },
                {
                    data: "SpendingTB", title: "消费额同比", sidth: "12%", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "SpendingHB", title: "消费额环比", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "GuestUnitPrice", title: "客单价", sortable: false, render: function (r) {
                        return formatNumber(r, true);
                    }
                },
                {
                    data: "GuestUnitPriceTB", title: "客单价同比", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "GuestUnitPriceHB", title: "客单价环比", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "GuestUnitCount", title: "客单量", sortable: false, render: function (r) {
                        return formatNumber(r,true);
                    }
                },
                {
                    data: "GuestUnitCountTB", title: "客单量同比", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "GuestUnitCountHB", title: "客单量环比", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "SpendingRiseRate", title: "消费额环比增长率", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "ContributionRateMember", title: "线下消费贡献率(占会员)", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                {
                    data: "ContributionRateTotal", title: "线下消费贡献率(占总体)", sortable: false, render: function (r) {
                        return convertRateData(r);
                    }
                },
                //{ data: "SpendingTotal", title: "总计消费额", sortable: false },
                //{ data: "SpendingTotalRiseRate", title: "总计消费增长率", sortable: false },
                //{ data: "SpendingTotalConRate", title: "总计消费贡献率", sortable: false }
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
                var d = $.extend({}, fixData(aoData), qryopt);
                ajax(sSource, d, function (data) {
                    fnCallback(data);
                });
            }
        });
    } else {
        dtContributionRate.fnDraw();
    }
}



$("#btnSearch").on("click", function () {
    loadContributionRate();
});

