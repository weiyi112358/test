
// 页面初始化
$(function () {
    $("#drpChannel,#drpArea,#drpCity,#drpStores").chosen();

    $("#txtStartDate, #txtEndDate").datepicker(
        {
            format: "yyyy-mm",
            autoClose: true,
        });
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
        searchMemMonthConsum();
    });

    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportMemMonthConsum";
        $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm #exprDtStart").val($("#txtStartDate").val());
        $("#exportForm #exprDtEnd").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    });

});

var dtSearch;
// 查询数据
function searchMemMonthConsum() {
    qryopt = {
        dateStart: $("#txtStartDate").val(),
        dateEnd: $("#txtEndDate").val(),
        channel: addqout($("#drpChannel").val()).toString(),
        area: $("#drpArea").val(),
        city: $("#drpCity").val(),
        store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString(),
    };
    if (!dtSearch) {
        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetMemMonthConsum',
            bAutoWidth: false,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength:1000,//
            aoColumns: [
               { data: "StatTime", title: "统计年月", "sClass": "center", Width: "7%", sortable: false },
               { data: "MemType", title: "会员类型", "sClass": "center", sortable: false },
               {
                   data: "Cnt1", title: "1-100元", "sClass": "center", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Cnt2", title: "101-300元", "sClass": "center", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Cnt3", title: "301-500元", "sClass": "center", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Cnt4", title: "501-1000元", "sClass": "center", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Cnt5", title: "1001-2000元", "sClass": "center", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Cnt6", title: "2001-5000元", "sClass": "center", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Cnt7", title: "5000以上", "sClass": "center", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
            ],
            fnDrawCallback: function () {
                $('#dt_search').rowspan(0);
            },
            fnRowCallback: function (nRow, aData, iDisplayIndex) {

                return nRow;
            },
            fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                var d = $.extend({}, fixData(aoData), qryopt);
                ajax(sSource, d, function (data) {
                    fnCallback(data);
                });
            }

        })
    } else {
        dtSearch.fnDraw();
    }
}

jQuery.fn.rowspan = function (colIdx) { //封装的一个JQuery小插件 
    return this.each(function () {
        var that;
        $('tr', this).each(function (row) {
            $('td:eq(' + colIdx + ')', this).filter(':visible').each(function (col) {
                if (that != null && $(this).html() == $(that).html()) {
                    rowspan = $(that).attr("rowSpan");
                    if (rowspan == undefined) {
                        $(that).attr("rowSpan", 1);
                        rowspan = $(that).attr("rowSpan");
                    }
                    rowspan = Number(rowspan) + 1;
                    $(that).attr("rowSpan", rowspan);
                    var h = $(that).height();

                    $(that).css({ "line-height": h + "px", "align:": "center" });
                    $(this).hide();
                } else {
                    that = this;
                }
            });
        });
    });
}