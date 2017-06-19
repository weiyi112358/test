
// 页面初始化
$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    $("#drpArea,#drpCity,#drpStores,#drpChannels").chosen();

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
        MemUpGrade.searchMemUpGrade();
    });

    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportMemUpGrade";
        //$("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprChannel").val($("#drpChannel").val());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        $("#exportForm #exprChannel").val($("#drpChannels").val() == null ? "" : addqout($("#drpChannels").val()).toString());
        $("#exportForm #exprDtStart").val($("#txtStartDate").val());
        $("#exportForm #exprDtEnd").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    });
});

var dtSearch;
// 搜索会员

var MemUpGrade = {
    isNum: function (num) {
        num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
        return num;
    },
    AmountAccount: function () {
        var $ComToCopper;//
        var $ActComToCopper;//
        var $CopperToSilver;//
        var $ActCopperToSilver;//
        var $SilverToGold, $ActSilverToGold, $GoldToPlatinum, $ActGoldToPlatinum;
        var tabstr = "";
        $("#dt_search tr").each(function () {
            var $this = $(this);
            $ComToCopper = MemUpGrade.isNum($ComToCopper) + MemUpGrade.isNum($this.find(".ComToCopper").html());
         //   $ActComToCopper = MemUpGrade.isNum($ActComToCopper) + MemUpGrade.isNum($this.find(".ActComToCopper").html());
            $CopperToSilver = MemUpGrade.isNum($CopperToSilver) + MemUpGrade.isNum($this.find(".CopperToSilver").html());
         //   $ActCopperToSilver = MemUpGrade.isNum($ActCopperToSilver) + MemUpGrade.isNum($this.find(".ActCopperToSilver").html());
            $SilverToGold = MemUpGrade.isNum($SilverToGold) + MemUpGrade.isNum($this.find(".SilverToGold").html());
          //  $ActSilverToGold = MemUpGrade.isNum($ActSilverToGold) + MemUpGrade.isNum($this.find(".ActSilverToGold").html());
            $GoldToPlatinum = MemUpGrade.isNum($GoldToPlatinum) + MemUpGrade.isNum($this.find(".GoldToPlatinum").html());
          //  $ActGoldToPlatinum = MemUpGrade.isNum($ActGoldToPlatinum) + MemUpGrade.isNum($this.find(".ActGoldToPlatinum").html());
        });
        tabstr += "<tr>" +
            "<td colspan='3'  style='font-weight:bold;text-align:center'>合计</td>" +
            "<td>" + $ComToCopper + "</td>" +
           // "<td>" + $ActComToCopper.toFixed(2) + "</td>" +
            "<td>" + $CopperToSilver + "</td>" +
           // "<td>" + $ActCopperToSilver.toFixed(2) + "</td>" +
            "<td>" + $SilverToGold + "</td>" +
          //  "<td>" + $ActSilverToGold.toFixed(2) + "</td>" +
            "<td>" + $GoldToPlatinum + "</td>" +
          //  "<td>" + $ActGoldToPlatinum.toFixed(2) + "</td>" +
            "</tr>";
        $('#dt_search tbody').append(tabstr);
    },
    searchMemUpGrade: function () {
        qryopt = {
            dateStart: $("#txtStartDate").val(),
            dateEnd: $("#txtEndDate").val(),
            area: $("#drpArea").val(),
            city: $("#drpCity").val(),
            store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString(),
            channel: $("#drpChannels").val() == null ? "" : addqout($("#drpChannels").val()).toString()
        };
        if (!dtSearch) {
            dtSearch = $('#dt_search').dataTable({
                sAjaxSource: '/Report/GetMemUpGrade',
                bSort: false, //不排序
                bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true, //每次请求后台数据
                bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 20, //
                aoColumns: [
                    { data: "Channel", title: "渠道明细", Width: "7%", sortable: false },
                    { data: "City", title: "城市", Width: "7%", sortable: false },
                    { data: "Store", title: "店铺", sidth: "12%", sortable: false },
                    {
                        data: "ComToCopper", title: "升铜卡", sortable: false, sClass: "ComToCopper", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                   // { data: "ActComToCopper", title: "累计普卡升铜卡", sortable: false, sClass: "ActComToCopper" },
                    {
                        data: "CopperToSilver", title: "升银卡", sortable: false, sClass: "CopperToSilver", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                  //  { data: "ActCopperToSilver", title: "累计铜卡升银卡", sortable: false, sClass: "ActCopperToSilver" },
                    {
                        data: "SilverToGold", title: "升金卡", sortable: false, sClass: "SilverToGold", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                  //  { data: "ActSilverToGold", title: "累计银卡升金卡", sortable: false, sClass: "ActSilverToGold" },
                    {
                        data: "GoldToPlatinum", title: "升白金卡", sortable: false, sClass: "GoldToPlatinum", render: function (r) {
                            return formatNumber(r);
                        }
                    }
                  //  { data: "ActGoldToPlatinum", title: "累计金卡升白金卡", sortable: false, sClass: "ActGoldToPlatinum" }
                ],
                fnDrawCallback: function () {
                    //MemUpGrade.AmountAccount();
                },
                fnServerData: function(sSource, aoData, fnCallback, aoSearchFilter) {
                    var d = $.extend({}, fixData(aoData), qryopt);
                    ajax(sSource, d, function(data) {
                        fnCallback(data);
                    });
                }

            });
        } else {
            dtSearch.fnDraw();
        }
    }


};