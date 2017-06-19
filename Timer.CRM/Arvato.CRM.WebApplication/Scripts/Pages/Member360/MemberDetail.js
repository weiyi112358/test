var dtVehicle;
var isFirstLoadOrder = true;
var isFirstLoadCommu = true;
var isFirstLoadAct = true;
var isFirstLoadPoint = true;
var isFirstLoadCoupon = true;
var isFirstLoadExchange = true;
var CouponType = null;

var tabdetails;
var basicTags;
var tradeTags;
var meminfo;
var tagext;
var tabext;//Tab
var search;

var memactinfo = '<div class="memInfoBlock" style="padding: 0 0"><h2 class="pull-left memName" id="spnName" style="line-height: 40px;"></h2><div class="pull-left memBasic"><span id="spnGender">&nbsp;&nbsp;</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b id="spnLevel"></b></div><div class="span8 memInfoList"><div class="clearfix"><p class="sepH_a"><span class="muted sepV_b">会员卡号</span><strong id="spnCardNo"></strong></p><p class="sepH_a"><span class="muted sepV_b">手机号码</span><strong id="spnMobile"></strong></p><p class="sepH_a"><span class="muted sepV_b">当前可用</span><strong class="stgValidValue" id="stgValidValue11"></strong></p><p class="sepH_a"><span class="muted sepV_b">当前不可用</span><strong class="stgValidValue" id="stgValidValue33"></strong></p></div></div></div>';

$(document).ready(function ()
{
    hidemenu();//默认隐藏菜单

    loadPage();

    //$('#dt_ordtable').resize(function () {
    //    $("#dt_ordtable").css({ "width": "140%" })
    //})

    //$('#dt_commu').resize(function () {
    //    $("#dt_commu").css({ "width": "110%" })
    //})

    $("#sel_gender").val($("#spn_gender").text());

    //$("#btnGoback").click(function () {
    //    var urlstr = window.location.href;
    //    var startpos = urlstr.indexOf("&") + 1;
    //    var urlpara = urlstr.substr(startpos);
    //    window.location = "/member360?" + urlpara;
    //});


    $(".update").click(function ()
    {
        $("#divUpdateBtn").show();
        $(this).parent("span").hide();
        $(this).parent("span").siblings("input").show();
    });

    $(".chgregion").click(function ()
    {
        $("#divUpdateBtn").show();
        $(".chgregion").parent("span").hide();
        $(".chgregion").parent("span").siblings("select").show();
    });
    $(".chgsel").click(function ()
    {
        $("#divUpdateBtn").show();
        $(this).parent("span").hide();
        $(this).parent("span").siblings("select").show();
        if ($("#spn_gender").text()) $("#sel_gender option[text='" + $("#spn_gender").text() + "']").attr("selected", true);
        if ($("#spn_bb1gender").text())
        {
            $("#sel_bb1gender").val($("#spn_bb1gender").text());
        };
        if ($("#spn_bb2gender").text()) $("#sel_bb2gender").val($("#spn_bb2gender").text());

    });








    //加载省份
    loadProvince($("#hid_prv").val());
    //绑定所有城市
    $("#sel_province").change(function ()
    {
        var selectValue = $("#sel_province").val();
        getRegionCity(selectValue);
    });
    //绑定所有区
    $("#sel_city").change(function ()
    {
        var selectValue = $("#sel_city").val();
        getRegionRegion(selectValue);
    });
    getChartData();



    var buyfrequentdata = [
        { "Brand": "C&A", "Count": 170 },
        { "Brand": "Gap", "Count": 140 },
        { "Brand": "GUESS", "Count": 100 }, ];

    $("#buyfrequent").dxChart({
        dataSource: buyfrequentdata,
        commonSeriesSettings: {
            argumentField: 'Brand',
            type: 'bar'
        },
        series: [
            { valueField: "Count", name: "C&A", color: '#E3DEDD' }
        ],
        valueAxis: {
            min: 0,
            max: 140
        },

        tooltip: {
            enabled: true,
            border: 0,
            font: { color: '#59AED5', size: 14, family: '"Helvetica Neue","Microsoft YaHei",Helvetica,Arial,sans-serif', weight: '100' },
            shadow: {
                blur: 0,
                color: "#E7E7E7",
                offsetX: 0,
                offsetY: 3,
                opacity: 0.4
            }
        },
        legend: {
            visible: false
        },
    });

    var firstdata = [
        { continent: 'A', population: 10 },
       { continent: 'B', population: 50 },
    ];

    $(function ()
    {
        $("#brandfirst").dxPieChart({
            dataSource: firstdata,
            series: {
                type: 'doughnut',
                argumentField: 'continent',
                valueField: 'population',
                label: {
                    visible: false,
                },
                innerRadius: 0.8,
                hoverStyle: {
                    hatching: {
                        direction: 'none',
                    }
                }
            },
            legend: {
                visible: false
            },
            palette: ['#E3DEDD', '#EB6877'],
            tooltip: {
                enabled: false,
            }
        });
    });
    var seconddata = [
       { continent: 'A', population: 75 },
       { continent: 'B', population: 25 },
    ];

    $(function ()
    {
        $("#brandsecond").dxPieChart({
            dataSource: seconddata,
            series: {
                type: 'doughnut',
                argumentField: 'continent',
                valueField: 'population',
                label: {
                    visible: false,
                },
                innerRadius: 0.8,
                hoverStyle: {
                    hatching: {
                        direction: 'none',
                    }
                }
            },
            legend: {
                visible: false
            },
            palette: ['#FFBB50', '#E3DEDD'],
            tooltip: {
                enabled: false,
            }
        });
    });

    var thirddata = [
       { continent: 'A', population: 25 },
       { continent: 'B', population: 75 },
    ];

    $(function ()
    {
        $("#brandthird").dxPieChart({
            dataSource: thirddata,
            series: {
                type: 'doughnut',
                argumentField: 'continent',
                valueField: 'population',
                label: {
                    visible: false,
                },
                innerRadius: 0.8,
                hoverStyle: {
                    hatching: {
                        direction: 'none',
                    }
                }
            },
            legend: {
                visible: false
            },
            palette: ['#ACA6A6', '#E3DEDD'],
            tooltip: {
                enabled: false,
            }
        });


    });

    var brandliketypedata = [
       { "Brand": "服装", "Count": 210 },
       { "Brand": "箱包", "Count": 141 },
       { "Brand": "家电", "Count": 170 }, ];

    $("#brandliketype").dxChart({
        dataSource: brandliketypedata,
        commonSeriesSettings: {
            argumentField: 'Brand',
            type: 'bar'
        },
        series: [
            { valueField: "Count", color: '#E3DEDD' }
        ],
        valueAxis: {
            min: 0,
            max: 200
        },

        tooltip: {
            enabled: true,
            border: 0,
            font: { color: '#59AED5', size: 14, family: '"Helvetica Neue","Microsoft YaHei",Helvetica,Arial,sans-serif', weight: '100' },
            shadow: {
                blur: 0,
                color: "#E7E7E7",
                offsetX: 0,
                offsetY: 3,
                opacity: 0.4
            }
        },
        legend: {
            visible: false
        },
    });

    $(".chzn_a").chosen({
        allow_single_deselect: true
    });

    //短信模板——弹窗
    $("#btnSMS").bind("click", function ()
    {
        $("#txtAddSMS").val("");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#SMS_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
        });
    });
    //短信模板——保存
    $("#btnSMSSave").bind("click", function ()
    {
        var sms = $("#txtAddSMS").val();
        if (sms == '')
        {
            $.dialog("短信内容不能为空");
            return;
        }        
        var model = {
            MemberId: $("#hdnMemberId").val(),
            MemberInfo: JSON.stringify(meminfo),
            SMSInfo:encode(sms)
        };                       
        ajaxSync("/Member360/SaveMemberSMS", { model:model }, function (res)
        {
            $.colorbox.close();
            $.dialog(res.MSG);
        });
    });

});


setTimeout('$("html").removeClass("js")', 1000);
//* datepicker
$("#dt_orderstart, #dt_orderend, #commu_start, #commu_end, #dp_pointchgstart, #dp_pointchgend,#dp_couponstart,#dp_couponend").datepicker({ autoclose: true });
$("#inp_birthday,#inp_bb1birth, #inp_bb2birth").datepicker({ autoclose: true });


//加载省份
function loadProvince(provtxt)
{
    ajax("/BaseData/GetRegionByPID", {
        pid: 0,
        grade: 1
    }, function (res)
    {
        if (res.IsPass)
        {
            var optionstring = "<option value=''>请选择</option>";
            var prv = res.Obj[0];
            for (var i in prv)
            {
                optionstring += "<option value=\"" + prv[i].RegionID + "\" >" + prv[i].NameZH + "</option>";
            }
            $("#sel_province").html(optionstring);
            if (provtxt)
            {
                $("#sel_province option:contains('" + provtxt + "')").attr("selected", true);
                $("#sel_province").change();
            }
        } else
        {
            $.dialog(res.MSG);
        }
    })
}

//绑定市信息
function getRegionCity(pId)
{
    if (pId != '')
    {
        postUrl = "/BaseData/GetRegionByPId";
        ajax(postUrl, { "pid": pId, grade: 2 }, getRegionCityCallBack($("#hid_cty").val()));
    }
    else
    {
        //var optionstring = "<option value=''>请选择</option>";
        //$("#inp_city").html(optionstring);
        //$("#inp_city").html(optionstring);
    }

}
function getRegionCityCallBack(citytxt)
{
    return function (data)
    {
        if (data.IsPass)
        {
            var optionstring = "<option value=''>请选择</option>";
            $("#sel_dist").html(optionstring);
            var rdata = data.Obj[0];
            for (var i in rdata)
            {
                optionstring += "<option value=\"" + rdata[i].RegionID + "\" >" + rdata[i].NameZH + "</option>";
            }
            $("#sel_city").html(optionstring);
            if (citytxt)
            {
                $("#sel_city option:contains('" + citytxt + "')").attr("selected", true);
                $("#sel_city").change();
            }
        }
    }
}

//绑定地级区信息
function getRegionRegion(pId)
{
    if (pId != '')
    {
        var postUrl = "/BaseData/GetRegionByPId";
        ajax(postUrl, { "pid": pId, grade: 3 }, getRegionRegionCallBack($("#hid_dst").val()));
    }
    else
    {
        //var optionstring = "<option value=''>请选择</option>";
        //$("#drpRegion").html(optionstring);
    }

}
function getRegionRegionCallBack(disttxt)
{
    return function (data)
    {
        if (data.IsPass)
        {
            var optionstring = "<option value=''>请选择</option>";
            var rdata = data.Obj[0];
            for (var i in rdata)
            {
                optionstring += "<option value=\"" + rdata[i].RegionID + "\" >" + rdata[i].NameZH + "</option>";
            }
            $("#sel_dist").html(optionstring);
            if (disttxt) $("#sel_dist option:contains('" + disttxt + "')").attr("selected", true);
        }
    }
}


function checkEmailExist()
{
    var postUrl = "/Member360/CheckEmailExist";
    if ($("#CustomerEmail").val() != "")
    {
        ajax(postUrl, { mid: $("#hdnMemberId").val(), email: $("#CustomerEmail").val() },
        function (res)
        {
            if (res.IsPass)
            {

                if (res.Obj[0] && res.Obj[0].length > 0)
                {
                    $.dialog("邮箱已存在");
                    $("#btnsave").removeAttr("disabled");
                    return;
                }
                else
                {
                    if ($("#CustomerMobile").val() == "")
                    {
                        saveMemDetail();
                    }
                    else
                    {
                        var postmobileUrl = "/Member360/CheckMobileExist";
                        ajax(postmobileUrl, { mid: $("#hdnMemberId").val(), mobile: $("#CustomerMobile").val() },
                            function (res)
                            {
                                if (res.IsPass)
                                {
                                    if (res.Obj[0] && res.Obj[0].length > 0)
                                    {
                                        $.dialog("手机已存在");
                                        $("#btnsave").removeAttr("disabled");
                                    }
                                    else
                                    { saveMemDetail(); }
                                }
                                return;
                            });
                    }
                }
            }

            return;
        });
    }
    else
    {
        if ($("#CustomerMobile").val() == "")
        {
            saveMemDetail();
        }
        else
        {
            var postmobileUrl = "/Member360/CheckMobileExist";
            ajax(postmobileUrl, { mid: $("#hdnMemberId").val(), mobile: $("#CustomerMobile").val() },
                function (res)
                {
                    if (res.IsPass)
                    {
                        if (res.Obj[0] && res.Obj[0].length > 0)
                        {
                            $.dialog("手机已存在");
                            $("#btnsave").removeAttr("disabled");
                        }
                        else
                            saveMemDetail();
                    }
                    return;
                });
        }

    }

}

//获取图表信息
function getChartData()
{

    var postUrl = "/Member360/GetMemChartData";
    ajax(postUrl, { mid: $("#hdnMemberId").val() },
        function (res)
        {
            if (res.IsPass)
            {
                var data = res.Obj[0][0];

                var promarr = data.promotion ? data.promotion.split(',') : '';
                if (promarr != '')
                {
                    promarr.forEach(function (t)
                    {
                        var dataarr = t.split('|');
                        if (dataarr[0] == "折扣") $("#spn_p1").text(dataarr[1] + '%').parent('p').css('width', dataarr[1] + '%');
                        if (dataarr[0] == "赠品") $("#spn_p2").text(dataarr[1] + '%').parent('p').css('width', dataarr[1] + '%');
                        if (dataarr[0] == "组合套装") $("#spn_p3").text(dataarr[1] + '%').parent('p').css('width', dataarr[1] + '%');
                        if (dataarr[0] == "优惠券") $("#spn_p4").text(dataarr[1] + '%').parent('p').css('width', dataarr[1] + '%');
                    });
                }

                var timearr = data.shoptime ? data.shoptime.split(',') : '';
                var timedata = [];
                if (timearr == '')
                {
                    timedata.push({ continent: '无购物数据', population: 0.1 });
                } else
                {
                    timearr.forEach(function (t)
                    {
                        var dataarr = t.split('|');
                        timedata.push({ continent: dataarr[0], population: parseFloat(dataarr[1]) });
                    });
                }

                loadShoptimeChart(timedata);

                var storearr = data.shopstore ? data.shopstore.split(',') : '';
                var storedata = [];
                if (storearr == '')
                {
                    storedata.push({ continent: '无购物数据', population: 0.1 });
                } else
                {
                    storearr.forEach(function (t)
                    {
                        var dataarr = t.split('|');
                        storedata.push({ continent: dataarr[0], population: parseFloat(dataarr[1]) });
                    });
                }
                loadShopstoreChart(storedata);

                var brandarr = data.shopbrand ? data.shopbrand.split(',') : '';
                var branddata = [];
                if (brandarr == '')
                {
                    branddata.push({ continent: '无购物数据', population: 0.1 });
                } else
                {
                    brandarr.forEach(function (t)
                    {
                        var dataarr = t.split('|');
                        branddata.push({ continent: dataarr[0], population: parseFloat(dataarr[1]) });
                    });
                }
                loadShopbrandChart(branddata);

                var typearr = data.shoptype ? data.shoptype.split(',') : '';
                var typedata = [];
                if (typearr == '')
                {
                    typedata.push({ continent: '无购物数据', population: 0.1 });
                } else
                {
                    typearr.forEach(function (t)
                    {
                        var dataarr = t.split('|');
                        typedata.push({ continent: dataarr[0], population: parseFloat(dataarr[1]) });
                    });
                }

                loadShoptypeChart(typedata);
            }
        });

}

//加载购物时间chart
function loadShoptimeChart(chartdata)
{
    $("#chart_shoptime").dxPieChart({
        dataSource: chartdata,
        series: {
            type: 'doughnut',
            argumentField: 'continent',
            valueField: 'population',
            label: {
                visible: true,
                customizeText: function ()
                {
                    return this.argumentText;
                }
            },
            hoverStyle: {
                hatching: {
                    direction: 'none',
                }
            }
        },
        legend: {
            visible: false
        },
        palette: ['#B1ABBC', '#FDA316', '#C9C4D2', ],
        tooltip: {
            enabled: true,
            customizeText: function (point)
            {
                if (point.valueText == 0.1)
                {
                    return 0
                }
                else
                {
                    return point.valueText
                }

            }
        }
    });
}

//加载购物门店chart
function loadShopstoreChart(chartdata)
{
    $("#likestore").dxPieChart({
        dataSource: chartdata,
        series: {
            type: 'doughnut',
            argumentField: 'continent',
            valueField: 'population',
            label: {
                visible: true,
                customizeText: function ()
                {
                    return this.argumentText;
                }
            },
            hoverStyle: {
                hatching: {
                    direction: 'none',
                }
            }
        },
        legend: {
            visible: false
        },
        palette: ['#EB6877', '#B1ABBC', '#C9C4D2', ],
        tooltip: {
            enabled: true,
            customizeText: function (point)
            {
                if (point.valueText == 0.1)
                {
                    return 0
                }
                else
                {
                    return point.valueText
                }

            }
        }
    });
}

//加载购物品牌chart
function loadShopbrandChart(chartdata)
{
    $("#chart_shopband").dxPieChart({
        dataSource: chartdata,
        series: {
            type: 'doughnut',
            argumentField: 'continent',
            valueField: 'population',
            label: {
                visible: true,
                customizeText: function ()
                {
                    return this.argumentText;
                }
            },
            hoverStyle: {
                hatching: {
                    direction: 'none',
                }
            }
        },
        legend: {
            visible: false
        },
        palette: ['#B1ABBC', '#FDA316', '#C9C4D2', ],
        tooltip: {
            enabled: true,
            customizeText: function (point)
            {
                if (point.valueText == 0.1)
                {
                    return 0
                }
                else
                {
                    return point.valueText
                }

            }
        }
    });
}

//加载购物类型chart
function loadShoptypeChart(chartdata)
{
    $("#chart_shoptype").dxPieChart({
        dataSource: chartdata,
        series: {
            type: 'doughnut',
            argumentField: 'continent',
            valueField: 'population',
            label: {
                visible: true,
                customizeText: function ()
                {
                    return this.argumentText;
                }
            },
            hoverStyle: {
                hatching: {
                    direction: 'none',
                }
            }
        },
        legend: {
            visible: false
        },
        palette: ['#EB6877', '#B1ABBC', '#C9C4D2', ],
        tooltip: {
            enabled: true,
            customizeText: function (point)
            {
                if (point.valueText == 0.1)
                {
                    return 0
                }
                else
                {
                    return point.valueText
                }

            }
        }
    });
}



var dtorder, dtorderdet, dtorderpay, dtcommu, dtact, dtpointchg, dtcoupon, dtexchange;


var orderid = 0;
function getTradeDetail(oid, o)
{
    $("#order_detail").show();
    var a = $(o).parent().parent()[0].style.cssText;
    if (a.indexOf("table-row-select") == 1)
    {
        $(o).parent().parent()[0].className = "";
    }
    else
    {
        $('#dt_ordtable tr.table-row-select').removeClass('table-row-select');
        $(o).parent().parent()[0].className = 'table-row-select';
    }
    $("#lbl_orddet").show();
    if (oid) orderid = oid;
    if (dtorderdet)
    {
        dtorderdet.fnDraw();
    } else
    {
        dtorderdet = $('#dt_orddettable').dataTable({
            sAjaxSource: '/Member360/GetOrderDetail',
            bInfo: true,
            sScrollX: "100%",
            sScrollXInner: "100%",
            bScrollCollapse: true,
            bSort: true,   //不排序
            bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: false, //显示分页信息
            iDisplayLength: 1000,
            aoColumns: [
                { data: 'TradeDetailType', title: "类型", sortable: true },
                { data: 'GoodsCodeProduct', title: "商品编号", sortable: true },
                { data: 'ProductName', title: "商品名称", sortable: true },
                { data: 'ColorName', title: "颜色", sortable: true },
                { data: 'SizeCodeProduct', title: "尺码", sortable: true },
                { data: 'QuantityProduct', title: "数量", sortable: true },
                { data: 'PriceProduct', title: "单价", sortable: true },
                { data: 'AmountProduct', title: "金额", sortable: true }
            ],
            fnFixData: function (d)
            {
                d.push({ name: 'oid', value: orderid });
            }
        });
    }
    getTradePay(oid);
}

function getTradePay(oid)
{
    $("#lbl_ordpay").show();
    if (oid) orderid = oid;
    if (dtorderpay)
    {
        dtorderpay.fnDraw();
    } else
    {
        dtorderpay = $('#dt_ordpaytable').dataTable({
            sAjaxSource: '/Member360/GetOrderPayment',
            bInfo: true,
            sScrollX: "100%",
            sScrollXInner: "100%",
            bScrollCollapse: true,
            bSort: true,   //不排序
            bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: false, //显示分页信息
            iDisplayLength: 1000,
            aoColumns: [
                { data: 'PmtNamePayment', title: "支付方式", sortable: true },
                { data: 'PmtCardNoPayment', title: "支付卡号", sortable: true },
                { data: 'AmountPayment', title: "金额", sortable: true },
                { data: 'ReceivedAmountPayment', title: "实收金额", sortable: true },
                { data: 'IntegralCostPayment', title: "使用积分", sortable: true }
            ],
            fnFixData: function (d)
            {
                d.push({ name: 'oid', value: orderid });
            }
        });
    }
}



function getExchangeHistory()
{
    dtexchange = $('#dt_exchange').dataTable({
        sAjaxSource: '/Member360/GetMemAccountExchange',
        bInfo: true,

        bSort: true,   //不排序
        bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: false, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[4, "desc"]],
        aoColumns: [
            //{ data: 'TradeCode', title: "兑换编号", sortable: true },
            //{ data: 'TradeType', title: "兑换类型", sortable: true },
            //{ data: 'RefTradeID', title: "兑换关联编号", sortable: true },
            //{ data: 'RefTradeType', title: "兑换关联类型", sortable: true },
            { data: 'UseIntByCoupon', title: "使用积分", sortable: true },
            { data: 'CouponType', title: "优惠券类型", sortable: true },
             { data: 'ExchangeType', title: "交易类型", sortable: true },
             { data: 'ActName', title: "活动名称", sortable: true },
             { data: 'AddedDate', title: "兑换日期", sortable: true },
        ],
        fnFixData: function (d)
        {
            d.push({ name: 'mid', value: $("#hdnMemberId").val() });
        }
    });
}

function GetCouponType()
{
    ajax("/BaseData/GetOptionDataList?optType=CouponType", null, function (res)
    {
        if (res.length > 0)
        {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++)
            {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#sel_coupontype').append(opt);
            CouponType = res;
        } else
        {
            var opt = "<option value=''>-无-</option>";
            $('#sel_coupontype').append(opt);
        }
    })
}

function checkInputData()
{
    var mobile = $("#CustomerMobile").val();
    var email = $("#CustomerEmail").val();
    var certno = $("#pCertificateNo").val();
    //if (!name || (name.replace(/(^\s*)|(\s*$)/g, "") == "")) {
    //    $.dialog("姓名不能为空");
    //    return false;
    //}
    if (mobile)
    {
        if (utility.checkMobile(mobile))
        {
            $.dialog("手机格式错误");
            return false;
        }
    }
    if (email)
    {
        if (utility.checkEmail(email))
        {
            $.dialog("邮箱格式错误");
            return false;
        }
    }
    //if (zip) {
    //    if (!/^[0-9]+$/.test(zip)) {
    //        $.dialog("邮编格式错误");
    //        return false;
    //    }
    //}
    // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X  
    if (certno)
    {
        var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        if (reg.test(certno) === false)
        {
            $.dialog("身份证输入不合法");
            return false;
        }
    }
    //if (!birthday) {
    //    $.dialog("出生日期必填");
    //    return false;
    //}
    $("#btnsave").attr("disabled", "disabled");
    checkEmailExist();
}
function saveMemDetail()
{

    for (var i = 0; i < basicTags.length; i++)
    {
        if (basicTags[i].IsUpdate)
        {
            if (basicTags[i].Required && $("#" + basicTags[i].FieldAlias).val() == "")
            {
                $.dialog(basicTags[i].DisplayName + "不能为空");
                return;
            }
            var Reg = basicTags[i].FReg == null ? basicTags[i].BReg : basicTags[i].FReg;
            if (Reg != null && Reg != "")
            {
                if ($("#" + basicTags[i].FieldAlias).val() != "" && !($("#" + basicTags[i].FieldAlias).val().match(Reg)))
                {
                    var ErrorTip = basicTags[i].FErrorTip == null ? basicTags[i].BErrorTip : basicTags[i].FErrorTip;
                    $.dialog(basicTags[i].DisplayName + ErrorTip);
                    return;
                }
            }
            meminfo[basicTags[i].FieldAlias] = $("#" + basicTags[i].FieldAlias).val();
        }
    }
    meminfo["MemberID"] = $("#hdnMemberId").val();
    //for (var i = 0; i < tagext.length; i++) {
    //    for (var j = 0; j < basicTags.length; j++) {
    //        if (tagext[i].DisplayName == basicTags[j].DisplayName) {
    //            meminfo[tagext[i].FieldAlias] = $("#" + basicTags[j].FieldAlias).val();
    //        }
    //    }
    //}
    $.ajax({
        type: "POST",
        url: "/Member360/SaveMemberDetail",
        data: { meminfo: JSON.stringify(meminfo) },
        dataType: "json",
        success: function (res)
        {
            if (res.IsPass)
            {
                $.dialog(res.MSG);
                //cancelEditMem();
                $("#divUpdateBtn").hide();
                $(".client-table p span").show();
                $(".client-table p input,.client-table p select").hide();
                var input = $(".client-table p input");
                for (var i = 0; i < input.length; i++)
                {
                    input[i].parentNode.firstChild.firstChild.innerHTML = input[i].value;
                }
                var select = $(".client-table p select");
                for (var i = 0; i < select.length; i++)
                {
                    var index = select[i].selectedIndex;
                    select[i].parentNode.firstChild.firstChild.innerHTML = select[i].options[index].text == "请选择" ? "" : select[i].options[index].text;
                }

            } else
            {
                $.dialog(res.MSG);
            }
        },
        error: function (message)
        {
            $("#request-process-patent").html("提交数据失败！");
        }
    });
}
//function saveMemDetail() {
//    var mobile = $("#inp_mobile").val();
//    var email = $("#inp_email").val();
//    ajax("/Member360/SaveMemberDetail", {
//        mid: $("#hdnMemberId").val(),
//        name: $("#inp_name").val(),
//        nickname: $("#inp_nicname").val(),
//        mobile: mobile,
//        gender: $("#sel_gender").val(),
//        email: email,
//        certno: $("#inp_certno").val(),
//        birthday: $("#inp_birthday").val(),
//        tel: $("#inp_tel").val(),
//        provid: $("#sel_province").val(),
//        cityid: $("#sel_city").val(),
//        distid: $("#sel_dist").val(),
//        addr: $("#inp_addr").val(),
//        zip: $("#inp_zip").val(),
//        b1id: $("#inp_bb1id").val(),
//        b1name: $("#inp_bb1name").val(),
//        b1gender: $("#sel_bb1gender").val(),
//        b1birth: $("#inp_bb1birth").val(),
//        b1height: $("#inp_bb1height").val(),
//        b1weight: $("#inp_bb1weight").val(),
//        b2id: $("#inp_bb2id").val(),
//        b2name: $("#inp_bb2name").val(),
//        b2gender: $("#sel_bb2gender").val(),
//        b2birth: $("#inp_bb2birth").val(),
//        b2height: $("#inp_bb2height").val(),
//        b2weight: $("#inp_bb2weight").val(),
//    }, function (res) {
//        if (res.IsPass) {
//            $.dialog(res.MSG);
//            //cancelEditMem();
//            $("#divUpdateBtn").hide();
//            $(".client-table p span").show();
//            $(".client-table p input,.client-table p select").hide();
//            $('#inp_name').parent('p').children(":first").children(":first").text($('#inp_name').val());
//            $('#inp_nicname').parent('p').children(":first").children(":first").text($('#inp_nicname').val());
//            $('#inp_mobile').parent('p').children(":first").children(":first").text($('#inp_mobile').val());
//            $('#sel_gender').parent('p').children(":first").children(":first").text($('#sel_gender option:selected').text());
//            $('#inp_email').parent('p').children(":first").children(":first").text($('#inp_email').val());
//            $('#inp_certno').parent('p').children(":first").children(":first").text($('#inp_certno').val());
//            $('#inp_birthday').parent('p').children(":first").children(":first").text($('#inp_birthday').val());
//            $('#inp_tel').parent('p').children(":first").children(":first").text($('#inp_tel').val());
//            $('#sel_province').parent('p').children(":first").children(":first").text($("#sel_province option:selected").text());
//            $('#sel_city').parent('p').children(":first").children(":first").text($('#sel_city option:selected').text());
//            $('#sel_dist').parent('p').children(":first").children(":first").text($('#sel_dist option:selected').text());
//            $('#inp_addr').parent('p').children(":first").children(":first").text($('#inp_addr').val());
//            $('#inp_zip').parent('p').children(":first").children(":first").text($('#inp_zip').val());

//            $('#inp_bb1id').parent('p').children(":first").children(":first").text($('#inp_bb1id').val());
//            $('#inp_bb1name').parent('p').children(":first").children(":first").text($('#inp_bb1name').val());
//            $('#sel_bb1gender').parent('p').children(":first").children(":first").text($('#sel_bb1gender option:selected').text());
//            $('#inp_bb1birth').parent('p').children(":first").children(":first").text($('#inp_bb1birth').val());
//            $('#inp_bb1height').parent('p').children(":first").children(":first").text($('#inp_bb1height').val());
//            $('#inp_bb1weight').parent('p').children(":first").children(":first").text($('#inp_bb1weight').val());

//            $('#inp_bb2id').parent('p').children(":first").children(":first").text($('#inp_bb2id').val());
//            $('#inp_bb2name').parent('p').children(":first").children(":first").text($('#inp_bb2name').val());
//            $('#sel_bb2gender').parent('p').children(":first").children(":first").text($('#sel_bb2gender option:selected').text());
//            $('#inp_bb2birth').parent('p').children(":first").children(":first").text($('#inp_bb2birth').val());
//            $('#inp_bb2height').parent('p').children(":first").children(":first").text($('#inp_bb2height').val());
//            $('#inp_bb2weight').parent('p').children(":first").children(":first").text($('#inp_bb2weight').val());

//            if ($('#inp_bb1name').val() || $('#inp_bb1birth').val() || $('#sel_bb1gender').val() || $('#inp_bb1height').val() || $('#inp_bb1weight').val() || $('#inp_bb2name').val() || $('#inp_bb2birth').val() || $('#sel_bb2gender').val() || $('#inp_bb2height').val() || $('#inp_bb2weight').val()) {
//                $('#lbl_haschild').text('是');
//            } else {
//                $('#lbl_haschild').text('否');
//            }
//        } else {
//            $.dialog(res.MSG);
//        }
//        $("#btnsave").removeAttr("disabled");
//    })
//}

function updateLabel()
{
    //$('.basic-row input').parent('p').children(":first").children(":first").text($('.basic-row input').val());
}
function cancelEditMem()
{
    $("#divUpdateBtn").hide();
    $(".client-table p span").show();
    $(".client-table p input,.client-table p select").hide();
    $("#inp_name").val($("#inp_name").parent('p').children(":first").children(":first").text());
    $("#inp_nicname").val($("#inp_nicname").parent('p').children(":first").children(":first").text());
    $("#inp_mobile").val($("#inp_mobile").parent('p').children(":first").children(":first").text());
    $("#inp_email").val($("#inp_email").parent('p').children(":first").children(":first").text());
    $("#inp_certno").val($("#inp_certno").parent('p').children(":first").children(":first").text());
    $("#inp_birthday").val($("#inp_birthday").parent('p').children(":first").children(":first").text());
    $("#inp_tel").val($("#inp_tel").parent('p').children(":first").children(":first").text());
    $("#inp_addr").val($("#inp_addr").parent('p').children(":first").children(":first").text());
    $("#inp_zip").val($("#inp_zip").parent('p').children(":first").children(":first").text());

    $("#inp_bb1name").val($("#inp_bb1name").parent('p').children(":first").children(":first").text());
    $("#inp_bb1birth").val($("#inp_bb1birth").parent('p').children(":first").children(":first").text());
    $("#inp_bb1height").val($("#inp_bb1height").parent('p').children(":first").children(":first").text());
    $("#inp_bb1weight").val($("#inp_bb1weight").parent('p').children(":first").children(":first").text());
    $("#inp_bb2name").val($("#inp_bb2name").parent('p').children(":first").children(":first").text());
    $("#inp_bb2birth").val($("#inp_bb2birth").parent('p').children(":first").children(":first").text());
    $("#inp_bb2height").val($("#inp_bb2height").parent('p').children(":first").children(":first").text());
    $("#inp_bb2weight").val($("#inp_bb2weight").parent('p').children(":first").children(":first").text());
}

//加载账户积分现金信息
function loadCashPoint(id)
{
    ajax("/Member360/GetMemAccountInfo", { mid: id }, function (data)
    {
        $(".stgValidValue").text(0);
        if (data.length > 0)
        {
            $("#stgValidValue11").text(data[0].ValidValue);//可用积分
            $("#stgValidValue33").text(data[0].FrozenValue);//冻结积分
            $("#stgValidValue44").text(Math.round(data[0].UnValidValue));//失效积分
            $("#stgValidValue88").text(Math.round(data[0].TotalValue));//总积分
            ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res)
            {
                if (res.IsPass)
                {
                    //给页面上详细信息栏赋值
                    $("#spnName").text(res.Obj[0].CustomerName);
                    $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
                    $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "" : res.Obj[0].CustomerLevelText);
                    $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
                    $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

                    //$('#dt_account_detail').dataTable({
                    //    sAjaxSource: '/Member360/GetActDetail',
                    //    bInfo: true,

                    //    bSort: false,   //不排序
                    //    bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                    //    bServerSide: false,  //每次请求后台数据
                    //    bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                    //    bPaginate: true, //显示分页信息
                    //    iDisplayLength: 1,
                    //    aaSorting: [[2, "desc"]],
                    //    bFilter: false, //过滤功能
                    //    aoColumns: [
                    //        { data: null, title: "账户类型", sortable: false },
                    //        {
                    //            data: null, title: "可用积分", sortable: false, render: function (r) {
                    //                return Math.round(r.DetailValue);
                    //            }
                    //        },
                    //        { data: 'SpecialDate1', title: "起始日期", sortable: true },
                    //        { data: 'SpecialDate2', title: "截止日期", sortable: false },

                    //    ],
                    //    fnFixData: function (d) {
                    //        d.push({ name: 'actid', value: res.Obj[0].AccountID });
                    //    },
                    //    "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    //        $('td:eq(0)', nRow).html(data[0].AccountTypeText);
                    //        var start = $('td:eq(3)', nRow).text();
                    //        var end = $('td:eq(2)', nRow).text();
                    //        start.length >= 10 ? $('td:eq(3)', nRow).text(start.substr(0, 10)) : "";
                    //        end.length >= 10 ? $('td:eq(2)', nRow).text(end.substr(0, 10)) : "";
                    //    },
                    //})
                }
            })
        }
    });

}

function loadchgtype()
{
    ajax("/BaseData/GetOptionDataList?optType=AccountChangeType", null, function (res)
    {
        if (res.length > 0)
        {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++)
            {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#sel_chgtype').append(opt);
            KPIType = res;
        } else
        {
            var opt = "<option value=''>-无-</option>";
            $('#sel_chgtype').append(opt);
        }

    });
}



var tags = "";
var d = new Array();
var s = new Array();
var p = new Array();
var ptag;
function loadPage()
{
    var mid = $("#hdnMemberId").val();
    ajax("/Member360/GetTagandMemInfo?memid=" + mid, null, function (res)
    {
        tags = res.taginfo;
        tabdetails = res.tabinfo;
        var queryResult = Enumerable.From(res.taginfo).OrderBy("$.Sort").GroupBy("$.BlockCode", null).ToArray();
        for (var i = 0; i < queryResult.length; i++)
        {
            if (queryResult[i].source[0].BlockCode == "Basic_Info")
            {
                basicTags = queryResult[i].source;
            }
            if (queryResult[i].source[0].BlockCode == "Personal_Info")
            {
                ptag = queryResult[i].source;
            }
            if (queryResult[i].source[0].BlockCode == "Table_Trade")
            {
                tradeTags = queryResult[i].source;
            }
            if (queryResult[i].source[0].BlockCode == "Basic_Info_Ext")
            {
                tagext = queryResult[i].source;
            }
            if (queryResult[i].source[0].BlockCode == "Search_Info")
            {
                search = queryResult[i].source;
            }
        }
        getBasicInfo(mid);
        loadTab(res);
        loadDNA(res);
        loadChartData(res);

    });
}

//获取基本信息
function getBasicInfo(mid)
{
    ajax("/Member360/GetBasicInfo?mid=" + mid, null, function (res)
    {
        meminfo = res.Obj[0];
        loadBasicInfo(res.Obj[0]);
        //loadPersonalInfo(ptag, res.Obj[0]);
        $(".update").click(function ()
        {
            $("#divUpdateBtn").show();
            $(this).parent("span").hide();
            $(this).parent("span").siblings().show();
        });
        for (var i = 0; i < d.length; i++)
        {
            $("#" + d[i].id).datepicker();//创建日历控件
        }
        for (var i = 0; i < s.length; i++)
        {
            creatselect(s[i].id, s[i].table, s[i].type, s[i].val)//创建下拉框
        }
        for (var i = 0; i < p.length; i++)
        {
            if (p[i].val != null)
            {
                transtag(p[i].id, p[i].table, p[i].type, p[i].val);
            }
        }
    });
}

//加载基本信息
function loadBasicInfo(o)
{
    var queryResult = Enumerable.From(tags).OrderBy("$.Sort").GroupBy("$.BlockCode", null).ToArray();
    for (var i = 0; i < queryResult.length; i++)
    {
        if (queryResult[i].source[0].BlockCode == "Basic_Info")
        {
            tags = queryResult[i].source;
        }
    }

    var html = "";
    createRow(html, tags, o);

}

//创建新一行的基本信息
function createRow(html, tags, basicinfo)
{
    var a = tags[0].Span;
    var b = new Array();
    var req = "<span class='f_req'>*</span>"
    html += "<tr class='basic-row'>";
    for (var i = 0; i < tags.length; i++)
    {
        if (tags[i].Span == tags[0].Span)
        {
            var show = basicinfo[tags[i].FieldAlias] == null ? "" : basicinfo[tags[i].FieldAlias];
            if (tags[i].IsUpdate)
            {//判断该字段是否可修改
                //添加修改类型
                var input = "";
                switch (tags[i].ControlType)
                {
                    case "input":
                        if (tags[i].MaxLength != null)
                        {
                            input = "<input id='" + tags[i].FieldAlias + "' class='hide' value='" + show + "' maxlength='" + tags[i].MaxLength + "' />";
                        }
                        else
                        {
                            input = "<input id='" + tags[i].FieldAlias + "' class='hide' value='" + show + "' />";
                        }

                        break;
                    case "select":
                        input = "<select class='hide' onchange='" + tags[i].FuncName + "'  id='" + tags[i].FieldAlias + "'></select>";
                        if (tags[i].FuncName)
                        {
                            input += "<script>" + tags[i].FuncContent + "</script>";
                        }
                        s.push({ id: tags[i].FieldAlias, table: tags[i].DictTableName, type: tags[i].DictTableType, val: basicinfo[tags[i].FieldAlias] });
                        //creatselect(tags[i].FieldAlias, tags[i].DictTableName, tags[i].DictTableType, basicinfo[tags[i].FieldAlias])
                        break;
                    case "date":
                        var date = basicinfo[tags[i].FieldAlias] == null ? (new Date().getFullYear() + "-" + (new Date().getMonth() + 1) + "-" + new Date().getDate()) : basicinfo[tags[i].FieldAlias].substr(0, 10);
                        show = show == null ? show : show.substr(0, 10);
                        input = "<input id='" + tags[i].FieldAlias + "' class='hide' value='" + date + "' />";
                        d.push({ id: tags[i].FieldAlias, val: basicinfo[tags[i].FieldAlias] });
                        break;
                    default:
                        input = "<input id='" + tags[i].FieldAlias + "' class='hide' value='" + show + "' />";
                        break;
                }
                if (tags[i].Required == true)
                {
                    html += "  <td colspan='" + 6 / tags[0].Span + "' class='td" + tags[0].Span + "'><p>" + tags[i].DisplayName + req + "</p><p class='two'><span><span id='p" + tags[i].FieldAlias + "'>" + show + "</span> [<span class='update'>修改</span>]</span>" + input + "</p></td>";
                }
                else
                {
                    html += "  <td colspan='" + 6 / tags[0].Span + "' class='td" + tags[0].Span + "'><p>" + tags[i].DisplayName + "</p><p class='two'><span><span id='p" + tags[i].FieldAlias + "'>" + show + "</span> [<span class='update'>修改</span>]</span>" + input + "</p></td>";
                }

            }
            else
            {
                if (tags[i].ControlType == "select")
                {
                    p.push({ id: tags[i].FieldAlias, table: tags[i].DictTableName, type: tags[i].DictTableType, val: basicinfo[tags[i].FieldAlias] });
                }
                if (tags[i].ControlType == "date")
                {
                    var date = basicinfo[tags[i].FieldAlias] == null ? (new Date().getFullYear() + "-" + (new Date().getMonth() + 1) + "-" + new Date().getDate()) : basicinfo[tags[i].FieldAlias].substr(0, 10);
                    show = show == null ? show : show.substr(0, 10);
                    input = "<input id='" + tags[i].FieldAlias + "' class='hide' value='" + date + "' />";
                    d.push({ id: tags[i].FieldAlias, val: show });
                }
                if (tags[i].Required == true)
                {
                    html += "<td colspan='" + 6 / tags[0].Span + "' class='td" + tags[0].Span + "'><p>" + tags[i].DisplayName + req + "</p><p class='two' id='p" + tags[i].FieldAlias + "'>" + show + "</p></td>";
                } else
                {
                    html += "<td colspan='" + 6 / tags[0].Span + "' class='td" + tags[0].Span + "'><p>" + tags[i].DisplayName + "</p><p class='two' id='p" + tags[i].FieldAlias + "'>" + show + "</p></td>";
                }





            }
            b.push(i);
            a = a - 1;
            if (a == 0)
            {
                break;
            }
        }
    }
    html += "</tr>";

    for (var i = b.length - 1; i >= 0; i--)
    {
        tags.splice(b[i], 1);
    }

    if (tags.length > 0)
    {
        createRow(html, tags, basicinfo);
    }
    else
    {
        $("#basic-info tbody").append(html);
    }
}

//为下拉框绑定值
function creatselect(id, table, dictType, val)
{
    if (dictType == "TrueOrFalse")
    {
        var opt = "";
        if (val == null)
        {
            opt = "<option value=''>请选择</option><option value='true'>是</option><option value='false'>否</option>";
            $("#p" + id).text("");
        }
        else
        {
            if (val)
            {
                $("#p" + id).text("是");
                opt = "<option value=''>请选择</option><option value='true' selected>是</option><option value='false'>否</option>";
            }
            else
            {
                $("#p" + id).text("否");
                opt = "<option value=''>请选择</option><option value='true' >是</option><option value='false' selected>否</option>";
            }
        }

        $("#" + id).append(opt);
    }
    else
    {
        if (table == null || dictType == null)
        {
            return;
        }
        ajax("/MemSubdivision/GetRightSelectDataByLimit", { fieldalias: id }, function (res)
        {//加载下拉框数据
            if (res.length > 0)
            {
                var opt = "<option value=''>请选择</option>";
                for (var i = 0; i < res.length; i++)
                {
                    if (val == res[i].OptionValue)
                    {
                        $("#p" + id).text(res[i].OptionText);//翻译显示值

                        opt += '<option value=' + res[i].OptionValue + ' selected>' + res[i].OptionText + '</option>';
                    }
                    else
                    {
                        opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
                    }
                }
            } else
            {
                var opt = "<option value=''>-无-</option>";
            }
            $("#" + id).append(opt);

        })
    }

}

//function LoadBasicInfoExt(tagext)
//{
//    for (var i = 0; i < tagext.length; i++) {
//        for (var j = 0; j < basicTags.length; j++) {
//            if (tagext[i].DisplayName == basicTags[j].DisplayName) {
//                $("#" + basicTags[j].FieldAlias).after("<input id='"+tagext[i].FieldAlias+"' type='hidden'>");
//            }
//        }
//    }
//}

//翻译非下拉框的选项
function transtag(id, table, dictType, val)
{
    if (table == null || dictType == null)
    {
        return;
    }
    ajax("/MemSubdivision/GetRightSelectData", { table: table, dictType: dictType }, function (res)
    {
        if (res.length > 0)
        {
            for (var i = 0; i < res.length; i++)
            {
                if (val == res[i].OptionValue)
                {
                    $("#p" + id).text(res[i].OptionText);
                }
            }
        }
    });
}



//加载Tab
function loadTab(o)
{
    var tabone = Enumerable.From(o.tabinfo).Where("$.Grade==1").OrderBy("$.Sort").ToArray();
    var tabtwo = Enumerable.From(o.tabinfo).Where("$.Grade==2").OrderBy("$.Sort").ToArray();
    for (var i = 0; i < tabone.length; i++)
    {
        $(".nav-tabs").append("<li><a href='#tab" + tabone[i].BlockTypeID + "' id='" + tabone[i].BlockType + "' data-toggle='tab'>" + tabone[i].BlockName + "</a></li>")

        //加载每个tab的html
        var a = "<div class='tab-pane' id='tab" + tabone[i].BlockTypeID + "'><div class='right-content'>";
        if (tabone[i].BlockType == "tab_gold")
        {
            a += memactinfo;

        }
        $(a + "<div style='padding: 0 10px;'><div class='row-fluid'></div></div><div><table class='table'></table></div></div></div>").appendTo($(".tab-content"));

        if (tabone[i].BlockType == "tab_gold")
        {
            $("#" + tabone[i].BlockType).bind('click', function ()
            {
                if (isFirstLoadPoint)
                {
                    loadCashPoint($("#hdnMemberId").val());
                    isFirstLoadPoint = false;
                }
            });

        }


        //加载tab的同时，加载本tab的搜索条件
        var searchobj = Enumerable.From(search).Where("$.BlockType==" + tabone[i].BlockTypeID).OrderBy("$.Sort").ToArray();
        if (searchobj.length > 0)
        {
            var strhtml = createSearchInfo(searchobj);
            $("#tab" + (tabone[i].BlockTypeID) + " .row-fluid").append(strhtml);
        } else
        {//如果没有搜索条件，则直接加载表格
            var tablename = tabone[i].TableName;
            if (!utility.isNull(tablename))
            {
                var conve = [];
                conve.push({ FieldAlias: "MemberID", Value: $("#hdnMemberId").val(), TableName: "TM_Mem_SubExt", Condition: "=" });
                var condition = JSON.stringify(conve); var aliaskey = tabone[i].AliasKey;
                var ishasdetail = tabone[i].IsHaveDetail; var aliassubkey = tabone[i].AliasSubKey;

                LoadDataTable(tablename, aliaskey, aliassubkey, ishasdetail, tabone[i].BlockTypeID, condition);
            }
        }
    }
    for (var i = 0; i < tabtwo.length; i++)
    {
        $("<div><p class='hide' style='font-size:14px;font-weight:700'>" + tabtwo[i].BlockName + "：</p><table class='table'></table></div>").appendTo($("#tab" + tabtwo[i].ParentID + " .right-content"));
    }
    for (var i = 0; i < ss.length; i++)
    {
        creatselect(ss[i].id, ss[i].table, ss[i].type)//创建下拉框
    }

    $(".dateinput").datepicker({ autoclose: true });
    loadStore();
    $(".btnSearch").bind("click", function ()
    {
        var id = $(this).prop("id");
        var con = [];//查询条件
        var total = [];//全部
        var tablefield = [];//表格返回字段
        var searchobj = Enumerable.From(search).Where("$.BlockType==" + id).OrderBy("$.Sort").ToArray();

        var tabobj = Enumerable.From(o.tabinfo).Where("$.BlockTypeID==" + id).ToArray();

        if (searchobj.length > 0)
        {
            for (var i = 0; i < searchobj.length; i++)
            {
                if (searchobj[i].ControlType != null)
                {
                    switch (searchobj[i].ControlType)
                    {
                        case "input":
                            if ($("#tab" + id + " #" + searchobj[i].FieldAlias).val() != "")
                            {
                                con.push({ FieldAlias: searchobj[i].FieldName, Value: $("#tab" + id + " #" + searchobj[i].FieldAlias).val(), TableName: searchobj[i].TableName, Condition: "like" });
                            }
                            break;

                        case "select":
                            if ($("#tab" + id + " #" + searchobj[i].FieldAlias).val() != "")
                            {
                                con.push({ FieldAlias: searchobj[i].FieldName, Value: $("#tab" + id + " #" + searchobj[i].FieldAlias).val(), TableName: searchobj[i].TableName, Condition: "=" });
                            }
                            break;
                        case "date":
                            if ($("#tab" + id + " #" + searchobj[i].FieldAlias + "Start").val() != "")
                            {
                                con.push({ FieldAlias: searchobj[i].FieldName, Value: $("#tab" + id + " #" + searchobj[i].FieldAlias + "Start").val(), TableName: searchobj[i].TableName, Condition: ">=" });
                            }
                            if ($("#tab" + id + " #" + searchobj[i].FieldAlias + "End").val() != "")
                            {
                                con.push({ FieldAlias: searchobj[i].FieldName, Value: addDate($("#tab" + id + " #" + searchobj[i].FieldAlias + "End").val(), 1), TableName: searchobj[i].TableName, Condition: "<=" });
                            }
                            break;
                        default:
                            con.push(null);
                            break;
                    }
                }
            }
        }
        con.push({ FieldAlias: "MemberID", Value: $("#hdnMemberId").val(), TableName: tabobj[0].TableName, Condition: "=" });
        //搜索条件
        //alert(JSON.stringify(con));
        //表+搜索条件
        //total.push({ TableName: tabobj[0].TableName, s: con });
        var tablename = tabobj[0].TableName; var condition = JSON.stringify(con); var aliaskey = tabobj[0].AliasKey;
        var ishasdetail = tabobj[0].IsHaveDetail; var aliassubkey = tabobj[0].AliasSubKey;

        LoadDataTable(tablename, aliaskey, aliassubkey, ishasdetail, id, condition);
    });
    $(".nav-tabs li:first").addClass("active");
}
function addDate(dd, dadd)
{
    var a = new Date(dd)
    a = a.valueOf()
    a = a + dadd * 24 * 60 * 60 * 1000
    a = new Date(a)
    return a.Format('yyyy-MM-dd');
}
Date.prototype.Format = function (fmt)
{ //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
//加载主表格
function LoadDataTable(tablename, aliaskey, aliassubkey, ishasdetail, blockcode, con)
{
    ajaxSync("/Member360/GetColumnByPage", { tablename: tablename, blockcode: blockcode }, function (res)
    {//{ mid: $("#hdnMemberId").val() }
        if (res)
        {
            columnList = res;
        }
    });

    if (dtcommu)
        dtcommu.fnDestroy();
    dtcommu = $("#tab" + blockcode + " .table:first").dataTable({
        sAjaxSource: '/Member360/GetTabInfo',
        bInfo: true,
        //sScrollX: "100%",
        //sScrollXInner: "110%",
        //bScrollCollapse: true,
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        //aaSorting: [[3, "desc"]],
        //sDom: "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
        aoColumns: columnList,
        fnDrawCallback: function ()
        {
            if (ishasdetail)
            {
                $("#tab" + blockcode + " .table:first tbody tr").addClass("rowlink").bind('click', function ()
                {
                    var tid = $(this).find('td:first').text();
                    var tabdetail = Enumerable.From(tabdetails).Where("$.ParentID==" + blockcode + "& $.Grade==2").OrderBy("$.Sort").ToArray();
                    loadTabDetailDataTable(tabdetail, tid);
                })

            }
        },
        fnFixData: function (d)
        {
            d.push({ name: 'tablename', value: tablename });
            d.push({ name: 'aliaskey', value: aliaskey });
            d.push({ name: 'aliassubkey', value: aliassubkey });
            d.push({ name: 'blockcode', value: blockcode });
            d.push({ name: 'parm', value: con });
        }
    });
}
//加载明细表格
function loadTabDetailDataTable(tabdetail, tid)
{
    for (var i = 0; i < tabdetail.length; i++)
    {
        var tablename = tabdetail[i].TableName; var aliaskey = tabdetail[i].AliasKey; var aliassubkey = tabdetail[i].AliasSubKey;
        var pid = tabdetail[i].ParentID + '_' + i; var blockcode = tabdetail[i].ParentID;
        loadDetailDataTable(tablename, aliaskey, aliassubkey, tid, pid, i + 1, blockcode)
    }
}
var dtDetail;
function loadDetailDataTable(tablename, aliaskey, aliassubkey, tid, pid, index, blockcode)
{
    var con = [];//查询条件
    con.push({ FieldAlias: "TradeID", Value: tid, TableName: tablename, Condition: "=" });
    $("#tab" + blockcode + " p").show();
    ajaxSync("/Member360/GetColumnByPage", { tablename: tablename, blockcode: pid, }, function (res)
    {//{ mid: $("#hdnMemberId").val() }
        if (res)
        {
            columnList = res;
        }
    });

    if (dtDetail)
        dtDetail.fnDraw();
    dtDetail = $("#tab" + blockcode + " .table:eq(" + index + ")").dataTable({
        sAjaxSource: '/Member360/GetTabInfo',
        bInfo: true,
        //sScrollX: "100%",
        //sScrollXInner: "110%",
        //bScrollCollapse: true,
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        //aaSorting: [[3, "desc"]],
        //sDom: "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",
        aoColumns: columnList,
        fnFixData: function (d)
        {
            d.push({ name: 'tablename', value: tablename });
            d.push({ name: 'aliaskey', value: aliaskey });
            d.push({ name: 'aliassubkey', value: aliassubkey });
            d.push({ name: 'blockcode', value: pid });
            d.push({ name: 'parm', value: JSON.stringify(con) });
        }
    });
}

var ss = new Array();

function createSearchInfo(searchobj)
{
    var html = "";
    html += "<div class='form-horizontal'><div class='control-group'>";
    for (var i = 0; i < searchobj.length; i++)
    {
        switch (searchobj[i].ControlType)
        {
            case "input":
                html += "<label class='control-label-long'>" + searchobj[i].DisplayName + "：</label><div class='controls-text'><input type='text' class='span12' id='" + searchobj[i].FieldAlias + "'></div>";
                break;

            case "select":
                //creatselect(tags[i].FieldAlias, tags[i].DictTableName, tags[i].DictTableType, basicinfo[tags[i].FieldAlias])
                //creatselect(searchobj[i].FieldAlias, searchobj[i].DisplayName, searchobj[i].DictTableType);
                html += "<label class='control-label-long'>" + searchobj[i].DisplayName + "：</label><div class='controls-text'><select id='" + searchobj[i].FieldAlias + "' class='chzn_a select drpstore' name='drp_select'></select></div>";
                ss.push({ id: searchobj[i].FieldAlias, table: searchobj[i].DictTableName, type: searchobj[i].DictTableType });
                break;
            case "date":
                html += "<label class='control-label-long'>" + searchobj[i].DisplayName + "：</label><div class='controls-text'><input type='text' class='span12 dateinput' id='" + searchobj[i].FieldAlias + "Start' readonly='readonly'><div class='btn-date-clear'></div></div><label class='control-label-short'>-</label><div class='controls-text'><input type='text' class='span12 dateinput' id='" + searchobj[i].FieldAlias + "End' readonly='readonly'><div class='btn-date-clear'></div></div>";

                break;
            default:
                html += "<input type='button' id='" + searchobj[i].BlockType + "' class='btn querry btnSearch' value='" + searchobj[i].DisplayName + "' />";
                break;
        }
    }
    html += "</div></div>";
    return html;
}


function loadStore()
{
    ajax("/MemSubdivision/GetStores", null, function (res)
    {//加载下拉框数据
        if (res.length > 0)
        {
            var opt = "";
            for (var i = 0; i < res.length; i++)
            {

                opt += '<option value=' + res[i].StoreCode + ' >' + res[i].StoreName + '</option>';
            }
        } else
        {
            var opt = "<option value=''>-无-</option>";
        }
        $("#StoreCode").append(opt).chosen({
            allow_single_deselect: true
        });

    })
}
/*绑定会员标签*/
function loadDNA(o)
{
    for (var i = 0; i < o.taginfo.length; i++)
    {
        if (o.taginfo[i].BlockCode == "DNA_1")
        {
            var showdata = o.meminfo[o.taginfo[i].FieldAlias] == null ? "" : o.meminfo[o.taginfo[i].FieldAlias];
            $(".taglib").append("<li><span class='text-ellipsis'>" + o.taginfo[i].DisplayName
                + "</span><span class='r text-ellipsis'>" + showdata + "</span></li>")
        }
    }
}

/*绑定饼图*/
//加载饼图数据
function loadChartData(o)
{
    var querResult = Enumerable.From(o.taginfo).GroupBy("$.BlockCode", null).ToArray();
    //去掉非饼图的字段
    for (var i = querResult.length - 1; i >= 0; i--)
    {
        if (querResult[i].source[0].BlockCode.indexOf('Pie') < 0)
        {
            querResult.splice(i, 1);
        }
    }
    //将饼图的字段分组
    for (var i = 0; i < querResult.length; i++)
    {
        var showdata = new Array();
        for (var j = 0; j < querResult[i].source.length; j++)
        {
            var str = o.meminfo[querResult[i].source[j].FieldAlias] == null ? "" : o.meminfo[querResult[i].source[j].FieldAlias];
            if (str.length > 0)
            {
                var dataarr = str.split("|");
                showdata.push({ continent: dataarr[0], population: parseFloat(dataarr[1]) })
            }
        }
        if (showdata.length == 0)
        {
            showdata.push({ continent: '无购物数据', population: 100 });
        }
        if (i % 2 == 0)
        {
            loadChart(querResult[i].source[0].BlockCode, querResult[i].source[0].DisplayName, showdata);
        }
        else
        {
            loadChart(querResult[i].source[0].BlockCode, querResult[i].source[0].DisplayName, showdata, true);
        }

    }
}

//加载饼图
function loadChart(block, desc, data, isfirst)
{
    var first = isfirst ? "" : "first";
    $(".shop-habits").append("<div class='horizon " + first + "'><div style='width: 100%; text-align: center;'><h3>" + desc + "</h3>"
        + "</div><div class='shop-habit-chart'><div id='" + block + "' style='height: 100%; width: 100%;'></div>"
                                    + "</div></div>")
    showChart(block, data, isfirst);
}

//设置饼图
function showChart(id, data, isfirst)
{
    $("#" + id).dxPieChart({
        dataSource: data,
        series: {
            type: 'doughnut',
            argumentField: 'continent',
            valueField: 'population',
            //label: {
            //    visible: true,
            //    customizeText: function ()
            //    {
            //        return this.argumentText;
            //    }
            //},
            hoverStyle: {
                hatching: {
                    direction: 'none',
                }
            }
        },
        legend: {
            visible: false
        },
        palette: getPieColor(isfirst),
        tooltip: {
            enabled: true
        }
    });
}

//设置饼图颜色
function getPieColor(isfirst)
{
    if (isfirst)
    {
        return ['#EB6877', '#B1ABBC', '#C9C4D2', ];
    }
    else
    {
        return ['#B1ABBC', '#FDA316', '#C9C4D2', ];
    }
}

//加载右侧个人信息
function loadPersonalInfo(ptags, data)
{
    for (var i = 0; i < ptags.length; i++)
    {
        switch (ptags[i].Sort)
        {
            case 1:
                $(".name").text(data[ptags[i].FieldAlias]);
                $(".name").attr("title", data[ptags[i].FieldAlias]);
                break;
            case 2:
                if (data[ptags[i].FieldAlias] == "男")
                {
                    $("#sex").addClass("male")
                }
                else
                {
                    $("#sex").addClass("female")
                }
                break;
            case 3:
                $(".member span").text(data[ptags[i].FieldAlias] == null ? "" : data[ptags[i].FieldAlias])
                break;
            case 4:
                $(".integral span").text(data[ptags[i].FieldAlias] == null ? "" : data[ptags[i].FieldAlias])
                break;
            default:

        }
    }
}

/*绑定订单*/

//绑定订单查询
function loadTradeSearch()
{
    var serach = new Array();
    for (var i = 0; i < tags.length; i++)
    {
        if (tags[i].BlockCode == "Table_Trade" && tags[i].IsSearch)
        {
            serach.push(tags[i]);
        }
    }
    var html = "";
    for (var i = 0; i < serach.length; i++)
    {
        switch (serach[i].SearchType)
        {
            case "":
                break;
            case "":
                break;
            default:
                html += "";
                break;

        }
    }

}

//绑定订单表格
function loadTrade()
{

    $("#order_detail").hide();
    if (dtorder)
    {
        dtorder.fnDraw();
    } else
    {
        var col = [
            {
                data: null, sTitle: "订单编号", sortable: false, sWidth: "120", render: function (obj)
                {
                    return '<a href="#dt_orddettable" onclick="getTradeDetail(' + obj.TradeID + ',this)">' + obj.TradeCode + '</a>';
                }
            },
            { data: 'TradeType', title: "订单类型", sWidth: "120", sortable: true }, ]

        dtorder = $('#dt_ordtable').dataTable({
            sAjaxSource: '/Member360/GetOrderData',
            sScrollX: "100%",
            sScrollXInner: "140%",
            bScrollCollapse: true,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 7,
            aoColumns: addcol(col, tradeTags),
            fnFixData: function (d)
            {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
                d.push({ name: 'start', value: $("#dt_orderstart").val() });
                d.push({ name: 'end', value: $("#dt_orderend").val() });
                //d.push({ name: 'chan', value: $("#sel_channel").val() });
                d.push({ name: 'amount_start', value: $("#inp_ordamtstart").val() });
                d.push({ name: 'amount_end', value: $("#inp_ordamtend").val() });
                d.push({ name: 'ordercode', value: $("#txt_ordercode").val() });
                d.push({ name: 'storecode', value: $("#drp_storecode").val() });
            }
        });
    }
}

//为表格添加列
function addcol(cols, blocks)
{
    for (var i = 0; i < blocks.length; i++)
    {
        cols.push({ data: blocks[i].FieldAlias, title: blocks[i].DisplayName, sortable: true });
    }
    return cols;
}


