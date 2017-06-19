//会员招募
var dtSearch;//查询会员招募
var Recruit = {
    SelDateType: function () {
        var drpSearchDateTimeType = $("#drpSearchDateTimeType").val();
        if (drpSearchDateTimeType == "1") {
            $('#txtRegMon').show();
            $("#txtRegDate").hide();
        }
        if (drpSearchDateTimeType == "0") {
            $("#txtRegDate").show();
            $('#txtRegMon').hide();
        }
    },
    isNum: function (num) {
        num = isNaN(parseFloat(num)) ? 0 : parseFloat(num);
        return num;
    },
    AmountAccount: function (object) {
        var $ActualRecruitnumMem;//实际招募人数
        var $SameIncreaseMem;//环比增幅
        var $LastyearIncreaseMem;//同比增幅
        var $ArearatioMem;//区域占比
        var tabstr = "";

        $("#dt_search tr").each(function () {
            var $this = $(this);
            $ActualRecruitnumMem = Recruit.isNum($ActualRecruitnumMem) + Recruit.isNum($this.find(".ActualRecruitnumMem").html());
            $SameIncreaseMem = Recruit.isNum($SameIncreaseMem) + Recruit.isNum($this.find(".SameIncreaseMem").html());
            $LastyearIncreaseMem = Recruit.isNum($LastyearIncreaseMem) + Recruit.isNum($this.find(".LastyearIncreaseMem").html());
            $ArearatioMem = Recruit.isNum($ArearatioMem) + Recruit.isNum($this.find(".ArearatioMem").html());
        });

        //环比与同比算总计
        var $MemAllCount = object[0]._aData.MemAllCount;//全国会员数
        var $MemLastThisCount = 0;//上个月的这个时候的会员数
        var $MemLastYearThisCount = 0;//去年的这个时候的会员数
        var $MemThisCount = 0;//所选时间的会员数
        var $HB_ZJ = 0;
        var $TB_ZJ = 0;
        var $area_ZJ=0
        for (var i = 0; i < object.length; i++) {
            var obj = object[i]._aData;
            $MemLastThisCount = Recruit.isNum($MemLastThisCount) + Recruit.isNum(obj.MemLastThisCount);
            $MemLastYearThisCount = Recruit.isNum($MemLastYearThisCount) + Recruit.isNum(obj.MemLastYearThisCount);
            $MemThisCount = Recruit.isNum($MemThisCount) + Recruit.isNum(obj.MemThisCount);
        }
        //(tab.MemThisCount-tab.MemLastThisCount*1.0)/tab.MemLastThisCount*100)环比
        //(tab.MemThisCount-tab.MemLastYearThisCount*1.0)/tab.MemLastYearThisCount*100)同比

        //环比总计
        if ($MemLastThisCount == 0) {
            $HB_ZJ = 100
        }
        else {
            $HB_ZJ = Recruit.isNum((Recruit.isNum($MemThisCount) - Recruit.isNum($MemLastThisCount)))/ Recruit.isNum($MemLastThisCount)
        }
        //同比总计
        if ($MemLastYearThisCount == 0) {
            $TB_ZJ = 100
        }
        else {
            $TB_ZJ = Recruit.isNum((Recruit.isNum($MemThisCount) - Recruit.isNum($MemLastYearThisCount))) / Recruit.isNum($MemLastYearThisCount)
        }
        
        var $areadutysum = Recruit.isNum($ActualRecruitnumMem) / Recruit.isNum($MemAllCount);

        tabstr += "<tr>" +
            "<td colspan='3' style='font-weight:bold;text-align:center'>合计</td>" +
            "<td>" + $ActualRecruitnumMem + "</td>" +
            "<td>" + $HB_ZJ.toFixed(2) + "</td>" +
            "<td>" + $TB_ZJ.toFixed(2) + "</td>" +
            "<td>" + $ArearatioMem.toFixed(2) + "</td>" +
            "</tr>";

        $('#dt_search tbody').append(tabstr);
    },
    SearchRecruit: function () {
        var drpSearchDateTimeType = $("#drpSearchDateTimeType").val();
        var dateReg;
        if (drpSearchDateTimeType == "0")
             dateReg = $("#txtRegDate").val();
        else{
            dateReg = $("#txtRegMon").val() + "-1";
        }
        qryopt = {
            dateRegType: drpSearchDateTimeType,
            dateReg: dateReg, //$("#txtRegDate").val(),//注册日
          //  dateRegMon: (drpSearchDateTimeType == "1" ? ($("#txtRegMon").val() + "-1") : ""),//($("#txtRegMon").val() != null && $("#txtRegMon").val() != "") ? ($("#txtRegMon").val() + "-1") : "",//注册月
            channel: addqout($("#drpChannel").val()).toString(),//渠道
            area: $("#drpArea").val(),//大区
            city: $("#drpCity").val(),//城市
            store: $("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString()//店铺
        };
        if (!dtSearch) {

            dtSearch = $('#dt_search').dataTable({
                sAjaxSource: '/Report/GetMemRecruitCount',
                bSort: false,   //不排序
                bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true,  //每次请求后台数据
                bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 20,//
                aoColumns: [
                   { data: "Channel", title: "渠道明细", Width: "7%", sortable: false },
                   { data: "City", title: "城市", Width: "7%", sortable: false },
                   { data: "Store", title: "店铺", sidth: "12%", sortable: false },
                   //{ data: "RecruitTarget_Mem", title: "会员招募目标", sortable: false },
                   {
                       data: "Actual_Recruitnum_Mem", title: "实际招募人数", sClass: 'ActualRecruitnumMem', sortable: false, render: function (r) {
                           return formatNumber(r);
                       }
                   },
                  // { data: "Completion_rate_Mem", title: "完成率", sortable: false },
                   {
                       data: null, title: "环比增幅", sClass: 'SameIncreaseMem', sortable: false, render: function (r) {
                           return convertRateData(r.Same_Increase_Mem);
                       }
                   },
                   {
                       data: null, title: "同比增幅", sClass: 'LastyearIncreaseMem', sortable: false, render: function (r) {
                           return convertRateData(r.Lastyear_Increase_Mem);
                       }
                   },
                   {
                       data: null, title: "店铺区域占比", sClass: 'ArearatioMem', sortable: false, render: function (r) {
                           return convertRateData(r.Area_ratio_Mem);
                       }
                   },
                ],
                fnDrawCallback: function (nRow) {
                    var datalength = nRow.aoData.length;
                   
                    //if (datalength == 0) {
                    //    return;
                    //}
                    //var object = nRow.aoData;
                    //Recruit.AmountAccount(object);
                },
                fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                    var d = $.extend({}, fixData(aoData), qryopt);
                    ajax(sSource, d, function (data) {
                        fnCallback(data);
                    });
                }
            })
        }
        else {
            dtSearch.fnDraw();
        }
    }
}

//页面初始化
$(function () {
    $("#drpCity").chosen();
    $("#drpChannel").chosen();
    $("#drpArea").chosen();
    $("#drpStores").chosen();
    Recruit.SelDateType();
    $("#drpSearchDateTimeType").bind("change", function () {
        Recruit.SelDateType();
    });
    $("#txtRegDate").datepicker();
    $('#txtRegMon').datepicker({ format: "yyyy-mm" });
   
    $("#drpCity").change(function () {
        $('#drpStores').empty();

        ajax("/Report/GetStoreByCity", { cityCode: $("#drpCity").val() }, function (data) {
            var opt = "<option value=''>全部</option>";
            for (var i = 0; i < data.length; i++) {
                opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
            }
            $('#drpStores').append(opt).chosen().trigger("liszt:updated");
            //$("#drpStores").trigger("liszt:updated");
        });

    });

    //查询
    $("#btnSearch").bind("click", function () {
        Recruit.SearchRecruit();
    });

    //导出
    $("#btnExport").bind("click", function () {
        var drpSearchDateTimeType = $("#drpSearchDateTimeType").val();
        $("#exportForm")[0].action = "/Report/ExportMemRecruitCount";
        $("#exportForm #exprChannel").val(addqout($("#drpChannel").val()).toString());
        $("#exportForm #exprArea").val($("#drpArea").val());
        $("#exportForm #exprCity").val($("#drpCity").val());
        $("#exportForm #exprStore").val($("#drpStores").val() == null ? "" : addqout($("#drpStores").val()).toString());
        var dateReg;
        if (drpSearchDateTimeType == "0")
            dateReg = $("#txtRegDate").val();
        else 
            dateReg = $("#txtRegMon").val() + "-1";
        $("#exportForm #exprDtReg").val(dateReg);
        $("#exportForm #exprDtRegType").val(drpSearchDateTimeType);
        $("#exportForm")[0].submit();
    });
});






