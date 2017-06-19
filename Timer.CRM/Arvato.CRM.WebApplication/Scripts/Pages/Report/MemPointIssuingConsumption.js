var dtIssuingConsumption;
$(function () {
    $("#txtStartDate, #txtEndDate").datepicker();
    $("#drpChannel,#drpArea,#drpCity,#drpStores").chosen();
    //loadIssuingConsumption();
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

    $('#dtMemIntegralGrant').resize(function () {
        $('#dtMemIntegralGrant').css({ "width": "140%" });
        $('#dtMemIntegralGrant').css({ "overflow-x": "scroll" });
    })
});

var IssUingConsumption = {
    isNum: function (num) {
        num = isNaN(parseInt(num)) ? 0 : parseInt(num);
        return num;
    },
    AmountAccount: function () {
        var $ComMemGet, $CopperMemGet, $SilverMemGet, $GoldMemGet, $PlatinumMemGet, $TotalMemGet;//
        var $ComMemCost, $CopperMemCost, $SilverMemCost, $GoldMemCost, $PlatinumMemCost, $TotalMemCost;//
        var $ComMemLeft, $CopperMemLeft, $SilverMemLeft, $GoldMemLeft, $PlatinumMemLeft, $TotalMemLeft;//
        var tabstr = "";
        $("#dtMemIntegralGrant tr").each(function () {
            var $this = $(this);
            $ComMemGet = IssUingConsumption.isNum($ComMemGet) + IssUingConsumption.isNum($this.find(".Com_Mem_Get").html());
            $CopperMemGet = IssUingConsumption.isNum($CopperMemGet) + IssUingConsumption.isNum($this.find(".Copper_Mem_Get").html());
            $GoldMemGet = IssUingConsumption.isNum($GoldMemGet) + IssUingConsumption.isNum($this.find(".Gold_Mem_Get").html());
            $SilverMemGet = IssUingConsumption.isNum($SilverMemGet) + IssUingConsumption.isNum($this.find(".Silver_Mem_Get").html());
            $PlatinumMemGet = IssUingConsumption.isNum($PlatinumMemGet) + IssUingConsumption.isNum($this.find(".Platinum_Mem_Get").html());
            $TotalMemGet = IssUingConsumption.isNum($TotalMemGet) + IssUingConsumption.isNum($this.find(".Total_Mem_Get").html());

            $ComMemCost = IssUingConsumption.isNum($ComMemCost) + IssUingConsumption.isNum($this.find(".Com_Mem_Cost").html());
            $CopperMemCost = IssUingConsumption.isNum($CopperMemCost) + IssUingConsumption.isNum($this.find(".Copper_Mem_Cost").html());
            $SilverMemCost = IssUingConsumption.isNum($SilverMemCost) + IssUingConsumption.isNum($this.find(".Silver_Mem_Cost").html());
            $GoldMemCost = IssUingConsumption.isNum($GoldMemCost) + IssUingConsumption.isNum($this.find(".Gold_Mem_Cost").html());
            $PlatinumMemCost = IssUingConsumption.isNum($PlatinumMemCost) + IssUingConsumption.isNum($this.find(".Platinum_Mem_Cost").html());
            $TotalMemCost = IssUingConsumption.isNum($TotalMemCost) + IssUingConsumption.isNum($this.find(".Total_Mem_Cost").html());

            $ComMemLeft = IssUingConsumption.isNum($ComMemLeft) + IssUingConsumption.isNum($this.find(".Com_Mem_Left").html());
            $CopperMemLeft = IssUingConsumption.isNum($CopperMemLeft) + IssUingConsumption.isNum($this.find(".Copper_Mem_Left").html());
            $SilverMemLeft = IssUingConsumption.isNum($SilverMemLeft) + IssUingConsumption.isNum($this.find(".Silver_Mem_Left").html());
            $GoldMemLeft = IssUingConsumption.isNum($GoldMemLeft) + IssUingConsumption.isNum($this.find(".Gold_Mem_Left").html());
            $PlatinumMemLeft = IssUingConsumption.isNum($PlatinumMemLeft) + IssUingConsumption.isNum($this.find(".Platinum_Mem_Left").html());
            $TotalMemLeft = IssUingConsumption.isNum($TotalMemLeft) + IssUingConsumption.isNum($this.find(".Total_Mem_Left").html());
        });
        tabstr += "<tr>" +
            "<td colspan='3'  style='font-weight:bold;text-align:center'>合计</td>" +
            "<td>" + $ComMemGet + "</td>" +
            "<td>" + $CopperMemGet + "</td>" +
            "<td>" + $GoldMemGet + "</td>" +
            "<td>" + $SilverMemGet + "</td>" +
            "<td>" + $PlatinumMemGet + "</td>" +
            "<td>" + $TotalMemGet + "</td>" +

            "<td>" + $ComMemCost + "</td>" +
            "<td>" + $CopperMemCost + "</td>" +
            "<td>" + $SilverMemCost + "</td>" +
            "<td>" + $GoldMemCost + "</td>" +
            "<td>" + $PlatinumMemCost + "</td>" +
            "<td>" + $TotalMemCost + "</td>" +

            "<td>" + $ComMemLeft + "</td>" +
            "<td>" + $CopperMemLeft + "</td>" +
            "<td>" + $SilverMemLeft + "</td>" +
            "<td>" + $GoldMemLeft + "</td>" +
            "<td>" + $PlatinumMemLeft + "</td>" +
            "<td>" + $TotalMemLeft + "</td>" +
            "</tr>";
        $('#dtMemIntegralGrant tbody').append(tabstr);
    },
    loadIssuingConsumption: function () {
        qryopt = {
            channel: addqout($("#drpChannel").val()).toString(),
            area: $("#drpArea").val(),
            city: $("#drpCity").val(),
            store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString(),
            dateStart: $("#txtStartDate").val(),
            dateEnd: $("#txtEndDate").val()
        }
        if (!dtIssuingConsumption) {
            dtIssuingConsumption = $('#dtMemIntegralGrant').dataTable({
                sAjaxSource: '/Report/GetMemIssuingConsumption',
                sScrollX: "100%",
                sScrollXInner: "140%",
                bScrollCollapse: true,
                bAutoWidth: false,
                bSort: false, //不排序
                bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true, //每次请求后台数据
                bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 20, //
                aoColumns: [
                    { data: "Channel", title: "渠道明细", sortable: false },
                    { data: "City", title: "城市", sortable: false },
                    { data: "Store", title: "店铺", sortable: false },
                    {
                        data: "Com_Mem_Get", title: "普通会员", sortable: false, sClass: "Com_Mem_Get", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Copper_Mem_Get", title: "铜卡", sortable: false, sClass: "Copper_Mem_Get", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Silver_Mem_Get", title: "银卡", sortable: false, sClass: "Silver_Mem_Get", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Gold_Mem_Get", title: "金卡", sortable: false, sClass: "Gold_Mem_Get", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Platinum_Mem_Get", title: "白金卡", sortable: false, sClass: "Platinum_Mem_Get", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Total_Mem_Get", title: "合计", sortable: false, sClass: "Total_Mem_Get", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Com_Mem_Cost", title: "普通会员", sortable: false, sClass: "Com_Mem_Cost", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Copper_Mem_Cost", title: "铜卡", sortable: false, sClass: "Copper_Mem_Cost", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Silver_Mem_Cost", title: "银卡", sortable: false, sClass: "Silver_Mem_Cost", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Gold_Mem_Cost", title: "金卡", sortable: false, sClass: "Gold_Mem_Cost", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Platinum_Mem_Cost", title: "白金卡", sortable: false, sClass: "Platinum_Mem_Cost", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Total_Mem_Cost", title: "合计", sortable: false, sClass: "Total_Mem_Cost", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Com_Mem_Left", title: "普通会员", sortable: false, sClass: "Com_Mem_Left", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Copper_Mem_Left", title: "铜卡", sortable: false, sClass: "Copper_Mem_Left", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Silver_Mem_Left", title: "银卡", sortable: false, sClass: "Gold_Mem_Left", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Gold_Mem_Left", title: "金卡", sortable: false, sClass: "Gold_Mem_Left", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Platinum_Mem_Left", title: "白金卡", sortable: false, sClass: "Platinum_Mem_Left", render: function (r) {
                            return formatNumber(r);
                        }
                    },
                    {
                        data: "Total_Mem_Left", title: "合计", sortable: false, sClass: "Total_Mem_Left", render: function (r) {
                            return formatNumber(r);
                        }
                    }
                ],
                fnDrawCallback: function () {
                    //IssUingConsumption.AmountAccount();
                },
                fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                    var d = $.extend({}, fixData(aoData), qryopt);
                    ajax(sSource, d, function (data) {
                        fnCallback(data);
                    });
                }

            });
        } else {
            dtIssuingConsumption.fnDraw();
        }
    }
}

$("#btnSearch").on("click", function () {
    IssUingConsumption.loadIssuingConsumption();
});

$("#btnExport").on("click", function () {
    $("#exportForm")[0].action = "/Report/ExportMemIssuingConsumption";
    $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
    $("#exportForm #exprArea").val($("#drpArea").val());
    $("#exportForm #exprCity").val($("#drpCity").val());
    $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
    $("#exportForm #exprDtStart").val($("#txtStartDate").val());
    $("#exportForm #exprDtEnd").val($("#txtEndDate").val());
    $("#exportForm")[0].submit();
});