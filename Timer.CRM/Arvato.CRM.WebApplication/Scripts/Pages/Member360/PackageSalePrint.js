var dt_ActHistoryData;
var printPackageList = [];//打印套餐信息
var printPacDetail = [];//条目信息
$(function () {
    $('#txtStartDate,#txtEndDate').datepicker();
    loadActHistoryList();
    $('#btnSearch').click(function () {
        loadActHistoryList();
    })
})

//加载储值历史信息
function loadActHistoryList() {
    if (!dt_ActHistoryData) {
        dt_ActHistoryData = $('#dt_ActHistoryData').dataTable({
            sAjaxSource: '/Member360/GetPackageSaleHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aaSorting: [[5, "desc"]],
            aoColumns: [
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerName", title: "会员名称", sortable: false },
                { data: "CustomerMobile", title: "会员手机号", sortable: false },
                { data: "PackageDecTrade", title: "操作方式", sortable: true, sWidth: "100px" },
                { data: "PackageTotalPriceTrade", title: "总金额", sortable: true, sWidth: "100px" },
                {
                    data: "AddedDate", title: "购买时间", sortable: false, sWidth: "100px", render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "AddedUser", title: "操作人", sortable: true, sWidth: "100px" },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="print(\'' + obj.MemberCardNo + '\',\'' + obj.CustomerName + '\',\'' + obj.PackageTotalPriceTrade + '\',\'' + obj.CarNo + '\',\'' + obj.VIN + '\',\'' + obj.AddedUser + '\',\'' + obj.TradeID + '\',\'' + obj.AddedDate + '\')">打印</button>';
                        return str;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'memName', value: $("#txtName").val() });
                d.push({ name: 'memMobile', value: $("#txtMobile").val() });
                d.push({ name: 'vehicleNo', value: $("#txtVehicle").val() });
                d.push({ name: 'startDate', value: $("#txtStartDate").val() });
                d.push({ name: 'endDate', value: $("#txtEndDate").val() });
            }
        });
    }
    else {
        dt_ActHistoryData.fnDraw();
    }
}

function print(a, b, c, d, e, f, g, h) {
    var tradeId = g;
    //通过订单Id获取购买套餐明细
    GetPakcgeHistory(tradeId);
    var orderMaster = {
        OrderDate: h,//零售单日期
        //OrderNumber: pendingLoadOrderObject.OrderCode,//流水号
        OrderSumAmt: Number(c).toFixed(2),//总金额
        actAmt: Number(c).toFixed(2),//实付金额
        totalAmt: Number(c).toFixed(2),//找零金额   
        storeCode: $("#hdnStoreName").val(),//门店名称
        userCode: f,//员工名字
        cardNo: a,//会员卡号
        memName: b,
        veNo: utility.isNull(d) ? '' : d,
        vinNo: utility.isNull(e) ? '' : e,
        PrintDate: (new Date()).toLocaleDateString(),
    };
    //传输数据
    var data = {
        Type: "PackageSale",
        //Store: store,
        OrderMaster: orderMaster,
        PackageList: printPackageList,
        Printer: $("#hdnPrinter").val()
    }
    printPage("../Print.html", data, function (res) {
        printPackageList = [];
        printPacDetail = [];
    });
}
function GetPakcgeHistory(t) {
    ajaxSync("/Member360/GetPakcgeHistoryByTradeID", { tradeId: t }, function (res) {
        if (res.length > 0) {
            for (var i = 0; i < res.length; i++) {
                getItemDetailList(res[i].PackageIDDetail);
                printPackageList.push({ pacName: res[i].PackageNameDetail, pacNum: res[i].PackageQtyDetail, pacPrice: res[i].PackagePriceDetail, itemlist: printPacDetail })
            }

        }
    });
}
function getItemDetailList(pid) {
    printPacDetail = [];
    ajaxSync('/BaseData/GetPackageDetailList1', { packageId: pid }, function (res) {
        for (var i = 0; i < res.length; i++) {
            printPacDetail.push({ itemName: res[i].ItemName, itemQty: res[i].Qty });
        }
    })
}