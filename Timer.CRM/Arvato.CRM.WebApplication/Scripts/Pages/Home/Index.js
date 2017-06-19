var saleamount;
var newcustomeamount = {
    kpiactdata: 0,
    kpilastdata: 0,
};

$(document).ready(function () {
    //获取销售情况
    var OrderKPIID = 10006;//销售金额
    var data = [];
    $.getJSON("/Home/getOrderKPI?KPIID=" + OrderKPIID, function (json) {
        $("#kpiorder").html(json.kpiname);
        $("#kpiordertotaltitle").html(json.kpiname);
        $("#kpiordertotal").html("截至目前" + json.total);
        data = json.kpidata;

        //data.pop(0);
        //data.pop(0);
        //data.pop(0);
        //data.pop(0);
        

        $("#countrysale").dxChart({
            dataSource: data,

            commonSeriesSettings: {
                argumentField: 'Day',
                type: 'line'
            },
            series: [
                {
                    valueField: 'Count',
                }
            ],

            valueAxis: {

                valueType: "numeric",
                
                //constantLines: [{

                //    value: 150,

                //    width: 2,

                //    dashStyle: "dash",

                //    color: "#ff7c7c",

                //    label: {

                //        text: "警戒线"

                //    }

                //}],
            },
            tooltip: {
                enabled: true,
                customizeText: function (ev) {
                    return this.argumentText + "为" + this.originalValue;
                },
                border: 0,
                color: "#5F8B95",
                font: { color: '#fff', size: 14, family: '"Helvetica Neue","Microsoft YaHei",Helvetica,Arial,sans-serif', weight: '100' },
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
    });


   
    //var KPIID = 19;
    //var datasource1 = [];
    //$.getJSON("/Home/GetSalesKPI?KPIID=" + KPIID, function (json) {
    //    $("#menberprice2").dxPieChart({
    //        palette: "bright",
    //        dataSource: json.Obj[0][1],
    //        series: {
    //            argumentField: "name",
    //            valueField: "value",
    //            border: { visible: true },
    //            lable: {
    //                visible: true,
    //            }
    //        },
    //        onPointClick: function (e) {
    //            var point = e.target;
    
    //            toggleVisibility(point);
    //        },
    //        onLegendClick: function (e) {
    //            var arg = e.target;
    
    //            toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
    //        },
    //        title: 'Area of Continents',
    //        tooltip: { enabled: false }
    //    });
   
    //});

    //获取会员增长情况
    var MemberKPIID = 10008;//会员数量
    $.getJSON("/Home/getOrderKPI?KPIID=" + MemberKPIID, function (json) {
            var incresedata = [];
            $("#kpiorder").html(json.kpiname);
            $("#kpiordertotaltitle").html(json.kpiname);
    
            //$("#memberactivetitle").html(json.kpiname);
            //$("#memberactivenum").html((json.validNUm * 100).toFixed(2));

            $("#saleorder").html(json.kpiname);
            $("#orderamount").html(json.total+ "万元");
           // $("#kpiordertotal").html("截至目前" + json.totalall);
            incresedata = json.kpidata;

        $("#kpipotentialcustomername").html(json.kpiname);
        $("#menberincrease").dxChart({
            dataSource: incresedata,
            commonSeriesSettings: {
                argumentField: 'Day',
                type: 'line'
          
               
            },
            series: [
                {
                    valueField: 'Count',
                    color: "#90EE90",
                    hoverStyle: {
                        color: '#837899',
                    }
                }
            ],
            tooltip: {
                enabled: true,
            },
            argumentAxis: {
                tickInterval: 10
            },
            valueAxis: {
             
                valueType: "numeric",
                title: {
                    text: "万元"
                },
                format: {
                    type: "millions"
                }
                //constantLines: [{

                //    value: 150,

                //    width: 2,

                //    dashStyle: "dash",

                //    color: "#ff7c7c",

                //    label: {

                //        text: "警戒线",
                       
                //    }

                //}],
            },
            tooltip: {
                enabled: true,
                customizeText: function (ev) {
                    return this.argumentText + "为" + this.originalValue + "万元";
                },
                border: 0,
                color: "#837899",
               
                font: { color: '#fff', size: 14, family: '"Helvetica Neue","Microsoft YaHei",Helvetica,Arial,sans-serif', weight: '100' },
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
        }).dxChart("instance");;
    });



    //获取会员客单价情况
    //获取会员增长情况
    var MemberKPIID = 10010;//会员数量
    $.getJSON("/Home/getOrderKPI?KPIID=" + MemberKPIID, function (json) {
        var customerpricedata = [];
        $("#mem_price_title").html(json.kpiname);
        $("#mem_price_num").html("截至目前  " + json.total);

        $("#memberlosttitle").html(json.kpiname);
        $("#memberlostnum").html(json.total);
       // $("#kpimemberprice").html("截至目前  "+json.kpiname);
        
        //$("#kpiordertotal").html("截至目前" + json.total == "" ? "" : ((json.total / 1000000).toFixed(2)) + "百万");
        customerpricedata = json.kpidata;

        $("#menberprice").dxChart({
            dataSource: customerpricedata,
            commonSeriesSettings: {
                argumentField: 'Day',
                type: 'line'
            },
            series: [
                {
                    valueField: 'Count',
                    color: "#4EEE94",
                    hoverStyle: {
                        color: '#4295BB',
                    }
                }
            ],
            tooltip: {
                enabled: true,
            },
            //valueAxis: {
            //    valueType: "numeric",
               
            //},
            valueAxis: {
                
                valueType: "numeric",

                //constantLines: [{

                //    value: 150,

                //    width: 2,

                //    dashStyle: "dash",

                //    color: "#ff7c7c",

                //    label: {

                //        text: "警戒线"

                //    }

                //}],
            },
            tooltip: {
                enabled: true,
                customizeText: function (ev) {
                    return this.argumentText+"为"  + this.originalValue ;
                },
                border: 0,
                color: "#4295BB",
                font: { color: '#fff', size: 14, family: '"Helvetica Neue","Microsoft YaHei",Helvetica,Arial,sans-serif', weight: '100' },
                shadow: {
                    blur: 0,
                    color: "#E7E7E7",
                    offsetX: 0,
                    offsetY: 3,
                    opacity: 0.4
                }
            },
            legend: { visible: false },
        });
    })


    //获取柱状图
    var KPIID = 10;//二次回购时间占比
    $.getJSON("/Home/GetSalesKPI?KPIID=" + KPIID, function (json) {
      //  menberprice2

        var echartsPie;
        var option = {
            title: {
                text: json.MSG,

                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: json.Obj[0][0]
            },
            color: ['#FFEC8B', '#FFEBCD', '#87CEEB', '#F0E68C', '#E0FFFF', '#FFE1FF', '#9E9E9E', '#C1FFC1', '#D1EEEE', '#B0C4DE', '#8EE5EE', '#A1A1A1', '#EEC900', '#AEEEEE'],
            series: [
                {

                    type: 'pie',
                    radius: '55%',
                    center: ['40%', '50%'],
                    radius: ['30%', '70%'],
                    avoidLabelOverlap: false,
                    data: json.Obj[0][1],
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };
        echartsPie = echarts.init(document.getElementById('menberprice1'));
        echartsPie.setOption(option);

    })

    //获取新客数量
    var NewCustomerKPIID = 10006;//新客数量
    $.getJSON("/Home/getNewCustomerKPI?KPIID=" + NewCustomerKPIID, function (json) {
        newcustomeamount.kpiactdata = json.kpiactdata;
        newcustomeamount.kpilastdata = json.kpilastdata;
        $("#newcustomer_title").html(json.kpiname);
        if (json.kpiactdata == "" && json.kpilastdata == "") {
            $("#newcustomer_num").html("")
        }
        else {
            var show = newcustomeamount.kpiactdata == "" ? "" : (numberFormat(newcustomeamount.kpiactdata - newcustomeamount.kpilastdata));
            show = show == "" ? "" : show.toString();
            $("#newcustomer_num").html(newcustomeamount.kpiactdata)
            //$("#newcustomer_num").html(json.kpiactdata - json.kpilastdata)
        }

    })

 
    var membertotal = 10008;//会员总数
    $.getJSON("/Home/GetMemberActive?KPIID=" + membertotal, function (json) {
        $("#kpimemtotal").html(json.kpiname);
        var show = parseInt(json.validNUm)/10000;
      
        $("#kpimemtotalnum").html("截止目前" + show+"万元");
        $("#memberactivetitle").html(json.kpiname);
        $("#memberactivenum").html(show );
    })
})

//切换销售金额
function shiftorderamount(o, way) {
    if (way == "month") {
        $("#statisticway").html("本月累计");
        $("#orderamount").html(saleamount.totalmonth == "" ? "" : (numberFormat((saleamount.totalmonth / 10000).toFixed(2)) + "万"))
        $("#orderamountall").removeClass("active");
        $("#orderamountmonth").addClass("active");
    }
    else {
        $("#statisticway").html("累计总计");
        $("#orderamount").html(saleamount.totalall == "" ? "" : (numberFormat((saleamount.totalall / 10000).toFixed(2)) + "万"))
        $("#orderamountall").addClass("active");
        $("#orderamountmonth").removeClass("active");
    }
}

//切换新客数量
function shiftnewcustomeramount(o, way) {
    if (way == "month") {
        $("#newcustomer_sway").html("本月累计");
        var show = newcustomeamount.kpiactdata == "" ? "" : (numberFormat(newcustomeamount.kpiactdata - newcustomeamount.kpilastdata));
        show = show == "" ? "" : show.toString();
        $("#newcustomer_num").html(newcustomeamount.kpiactdata)
        $("#newcustomer_all").removeClass("active");
        $("#newcustomer_month").addClass("active");
    }
    else {
        $("#newcustomer_sway").html("累计总计");
        var show = newcustomeamount.kpiactdata == "" ? "" : (numberFormat(newcustomeamount.kpiactdata));
        show = show == "" ? "" : show.substr(0, (show.toString()).length - 3);
        $("#newcustomer_num").html(newcustomeamount.kpilastdata)
        $("#newcustomer_all").addClass("active");
        $("#newcustomer_month").removeClass("active");
    }
}




function numberFormat(number) {
    number = number.toString();
    number = number.replace(/,/g, "");
    var digit = number.indexOf("."); // 取得小数点的位置
    var int = number.substr(0, digit); // 取得小数中的整数部分
    var i;
    var mag = new Array();
    var word;
    if (number.indexOf(".") == -1) { // 整数时
        i = number.length; // 整数的个数
        while (i > 0) {
            word = number.substring(i, i - 3); // 每隔3位截取一组数字
            i -= 3;
            mag.unshift(word); // 分别将截取的数字压入数组
        }
        number = mag;
    } else { // 小数时
        i = int.length; // 除小数外，整数部分的个数
        while (i > 0) {
            word = int.substring(i, i - 3); // 每隔3位截取一组数字
            i -= 3;
            mag.unshift(word);
        }
        number = mag + number.substring(digit);
    }
    return number.toString();
}