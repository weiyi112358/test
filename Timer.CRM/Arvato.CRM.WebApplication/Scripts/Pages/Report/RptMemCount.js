
// 页面初始化
$(function () {
    $("#txtStartDate, #txtEndDate").datepicker();
    $("#drpChannel,#drpArea,#drpCity,#drpStores").chosen();

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
    //查询操作
    $("#btnSearch").click(function () {
        searchMem();
    });

    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportMemCount";
        $("#exportForm #exprChannel").val($("#drpChannel").val() == null ? "" : addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm #exprDtStart").val($("#txtStartDate").val());
        $("#exportForm #exprDtEnd").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    });

});
$(document).ajaxSuccess(function (event, xhr, settings) {
    if (settings.url === "searchMem") {
        alert("数据填充完成");
    }
});
var dtSearch;
// 搜索会员
function searchMem() {
    qryopt = {
        dateStart: $("#txtStartDate").val(),
        dateEnd: $("#txtEndDate").val(),
        channel: $("#drpChannel").val() == null ? "" : addqout($("#drpChannel").val()).toString(),
        area: $("#drpArea").val(),
        city: $("#drpCity").val(),
        store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString(),
    };
    if (!dtSearch) {
        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetMemCount',
            bAutoWidth: false,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,//
            aoColumns: [
               { data: "Channel", title: "渠道明细", "sClass": "center", sortable: false },
               { data: "City", title: "城市", "sClass": "center", sortable: false },
               { data: "Store", title: "店铺", "sClass": "center", sortable: false },
               {
                   data: "Com_Mem", title: "普通会员", "sClass": "center Com_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Copper_Mem", title: "铜卡", "sClass": "center Copper_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Silver_Mem", title: "银卡", "sClass": "center Silver_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Gold_Mem", title: "金卡", "sClass": "center Gold_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Platinum_Mem", title: "白金卡", "sClass": "center Platinum_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Total_Mem", title: "会员总数", "sClass": "center Total_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Com_Mem_New", title: "普通会员", "sClass": "center Com_Mem_New", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               , render: function (r) {
                   return formatNumber(r);
               }
               },
               {
                   data: "Copper_Mem_New", title: "铜卡", "sClass": "center Copper_Mem_New", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Silver_Mem_New", title: "银卡", "sClass": "center Silver_Mem_New", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Gold_Mem_New", title: "金卡", "sClass": "center Gold_Mem_New", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Platinum_Mem_New", title: "白金卡", "sClass": "center Platinum_Mem_New", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Total_Mem_New", title: "会员总数", "sClass": "center Total_Mem_New", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
                {
                    data: null, title: "会员增长率", "sClass": "center Percent_Mem", sortable: false, render: function (obj) {
                        if (obj.Total_Mem_New == 0 || obj.Total_Mem_New==null) {
                            return "0.00%";
                        } 
                        else if (obj.Total_Mem_New == obj.Total_Mem) {
                            return "100.00%";
                        } else {
                            return ((obj.Total_Mem_New / (obj.Total_Mem - obj.Total_Mem_New)) * 100).toFixed(2) + "%";
                        }
                    }
                },
               {
                   data: "Active_Mem", title: "活跃会员量", "sClass": "center Active_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "WillLose_Mem", title: "潜在流失会员量", "sClass": "center WillLose_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Lose_Mem", title: "流失会员量", "sClass": "center Lose_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
            ],
            fnDrawCallback: function (nRow) {
                var datalength = nRow.aoData.length;
                if (datalength == 0) {
                    return;
                }

                //AmountAccount();
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

        })
    } else {
        dtSearch.fnDraw();
    }
}

function AmountAccount() {
    var $Com_Mem;
    var $Copper_Mem;
    var $Silver_Mem;
    var $Gold_Mem;
    var $Platinum_Mem;
    var $Total_Mem;
    var $Com_Mem_New;
    var $Copper_Mem_New;
    var $Silver_Mem_New;
    var $Gold_Mem_New;
    var $Platinum_Mem_New;
    var $Total_Mem_New;
    var $Percent_Mem;
    var $Active_Mem;
    var $WillLose_Mem;
    var $Lose_Mem;
    var tabstr = "";

    $("#dt_search tr").each(function () {
        var $this = $(this);
        $Com_Mem = isNum($Com_Mem) + isNum($this.find(".Com_Mem").html());
        $Copper_Mem = isNum($Copper_Mem) + isNum($this.find(".Copper_Mem").html());
        $Silver_Mem = isNum($Silver_Mem) + isNum($this.find(".Silver_Mem").html());
        $Gold_Mem = isNum($Gold_Mem) + isNum($this.find(".Gold_Mem").html());
        $Platinum_Mem = isNum($Platinum_Mem) + isNum($this.find(".Platinum_Mem").html());
        $Total_Mem = isNum($Total_Mem) + isNum($this.find(".Total_Mem").html());
        $Com_Mem_New = isNum($Com_Mem_New) + isNum($this.find(".Com_Mem_New").html());
        $Copper_Mem_New = isNum($Copper_Mem_New) + isNum($this.find(".Copper_Mem_New").html());
        $Silver_Mem_New = isNum($Silver_Mem_New) + isNum($this.find(".Silver_Mem_New").html());
        $Gold_Mem_New = isNum($Gold_Mem_New) + isNum($this.find(".Gold_Mem_New").html());
        $Platinum_Mem_New = isNum($Platinum_Mem_New) + isNum($this.find(".Platinum_Mem_New").html());
        $Total_Mem_New = isNum($Total_Mem_New) + isNum($this.find(".Total_Mem_New").html());
        //  $Percent_Mem =  // isNum($Percent_Mem) + isNum($this.find(".Percent_Mem").html());
        $Active_Mem = isNum($Active_Mem) + isNum($this.find(".Active_Mem").html());
        $WillLose_Mem = isNum($WillLose_Mem) + isNum($this.find(".WillLose_Mem").html());
        $Lose_Mem = isNum($Lose_Mem) + isNum($this.find(".Lose_Mem").html());
    });
    tabstr += "<tr>" +
        "<td colspan='3'  style='font-weight:bold;text-align:center'>合计</td>" +
        "<td style='text-align:center'>" + $Com_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Copper_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Silver_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Gold_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Platinum_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Total_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Com_Mem_New.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Copper_Mem_New.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Silver_Mem_New.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Gold_Mem_New.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Platinum_Mem_New.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Total_Mem_New.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + ($Total_Mem - $Total_Mem_New > 0 ? $Total_Mem_New / ($Total_Mem - $Total_Mem_New).toFixed(2) : 0) + "</td>" +
        "<td style='text-align:center'>" + $Active_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $WillLose_Mem.toFixed(0) + "</td>" +
        "<td style='text-align:center'>" + $Lose_Mem.toFixed(0) + "</td>" +
        "</tr>";
    $('#dt_search tbody').append(tabstr);
}

function isNum(num) {
    num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
    return num;
}


