//会员消费频次统计
var MemConsumptionFre = {
    isNum: function (num) {
        num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
        return num;
    },
    AmountAccount: function () {
        var $FrequencyEqZero;//频次=0
        var $FrequencyEqOne;//频次=1
        var $FrequencyEqTwo;//频次=2
        var $FrequencyEqThree;//频次=3
        var $FrequencyEqFour;//频次=4
        var $FrequencyEqFive;//频次=5
        var $FrequencyEqFiveGoTen;//频次=6-10
        var $FrequencyThanTen;//频次10
        var tabstr = "";

        $("#dt_search tr").each(function () {
            var $this = $(this);
            $FrequencyEqZero = MemConsumptionFre.isNum($FrequencyEqZero) + MemConsumptionFre.isNum($this.find(".FrequencyEqZero").html());
            $FrequencyEqOne = MemConsumptionFre.isNum($FrequencyEqOne) + MemConsumptionFre.isNum($this.find(".FrequencyEqOne").html());
            $FrequencyEqTwo = MemConsumptionFre.isNum($FrequencyEqTwo) + MemConsumptionFre.isNum($this.find(".FrequencyEqTwo").html());
            $FrequencyEqThree = MemConsumptionFre.isNum($FrequencyEqThree) + MemConsumptionFre.isNum($this.find(".FrequencyEqThree").html());
            $FrequencyEqFour = MemConsumptionFre.isNum($FrequencyEqFour) + MemConsumptionFre.isNum($this.find(".FrequencyEqFour").html());
            $FrequencyEqFive = MemConsumptionFre.isNum($FrequencyEqFive) + MemConsumptionFre.isNum($this.find(".FrequencyEqFive").html());
            $FrequencyEqFiveGoTen = MemConsumptionFre.isNum($FrequencyEqFiveGoTen) + MemConsumptionFre.isNum($this.find(".FrequencyEqFiveGoTen").html());
            $FrequencyThanTen = MemConsumptionFre.isNum($FrequencyThanTen) + MemConsumptionFre.isNum($this.find(".FrequencyThanTen").html());
        });

        tabstr += "<tr>" +
            "<td style='font-weight:bold;text-align:center'>合计</td>" +
            "<td style='text-align:center'>" + $FrequencyEqZero + "</td>" +
            "<td style='text-align:center'>" + $FrequencyEqOne + "</td>" +
            "<td style='text-align:center'>" + $FrequencyEqTwo + "</td>" +
            "<td style='text-align:center'>" + $FrequencyEqThree + "</td>" +
            "<td style='text-align:center'>" + $FrequencyEqFour + "</td>" +
            "<td style='text-align:center'>" + $FrequencyEqFive + "</td>" +
            "<td style='text-align:center'>" + $FrequencyEqFiveGoTen + "</td>" +
            "<td style='text-align:center'>" + $FrequencyThanTen + "</td>" +
            "</tr>";

        $('#dt_search tbody').append(tabstr);
    },
}
$(function () {
    $("#drpChannel").chosen();
    $("#drpBrand").chosen();
    $("#drpCategory").chosen();
    
    $("#txtConsumptionStartDate,#txtConsumptionEndDate").datepicker();

    //查询
    $("#btnSearch").bind("click", function () {
        searchMemConsumptionFreCount();
    });

    //导出
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportMemConsumptionFreCount";
        $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprBrand").val($("#drpBrand").val());
        $("#exportForm #exprCategory").val($("#drpCategory").val());
        $("#exportForm #expDtConsumptionStart").val($("#txtConsumptionStartDate").val());
        $("#exportForm #expDtConsumptionEnd").val($("#txtConsumptionEndDate").val());
        $("#exportForm")[0].submit();
    });

})

var dtSearch;
//会员消费频次统计
function searchMemConsumptionFreCount() {
    qryopt = {
        dateConsumptionStart: $("#txtConsumptionStartDate").val(),//消费日期起期
        dateConsumptionEnd: $("#txtConsumptionEndDate").val(),//消费日期止期
        channel: addqout($("#drpChannel").val()).toString(),//渠道
        brand: $("#drpBrand").val(),//品牌
        category: $("#drpCategory").val()//品类
    };
    if (!dtSearch) {
        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetMemConsumptionFreCount',
            bAutoWidth: true,
            bSort: false,   //不排序
            bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: false, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
               { data: "member", title: "会员等级", Width: "7%", "sClass": "center", sortable: false },
               {
                   data: "FrequencyEqZero", title: "频次=0", "sClass": "center FrequencyEqZero", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "FrequencyEqOne", title: "频次=1", "sClass": "center FrequencyEqOne", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "FrequencyEqTwo", title: "频次=2", "sClass": "center FrequencyEqTwo", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "FrequencyEqThree", title: "频次=3", "sClass": "center FrequencyEqThree", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "FrequencyEqFour", title: "频次=4", "sClass": "center FrequencyEqFour", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "FrequencyEqFive", title: "频次 5", "sClass": "center FrequencyEqFive", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
                {
                    data: "FrequencyEqFiveGoTen", title: "频次=6-10", "sClass": "center FrequencyEqFiveGoTen", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
               {
                   data: "FrequencyThanTen", title: "频次>10", "sClass": "center FrequencyThanTen", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               }
            ],
            fnDrawCallback: function (nRow) {
                //var datalength = nRow.aoData.length;
                //if (datalength == 0) {
                //    return;
                //}
                //MemConsumptionFre.AmountAccount();
            },
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                //数据加载完成后的最后一行处理
                if (aData.Channel == "合计") {
                    $('td:eq(0)', nRow).remove();
                    $('td:eq(1)', nRow).remove();
                }
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
        dtSearch.fnDraw();
    }

}

