var dt_ActHistoryData;
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
            sAjaxSource: '/Member360/GetActHistory',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aaSorting: [[7, "desc"]],
            aoColumns: [
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerName", title: "会员名称", sortable: false },
                { data: "ChangeValueBefore", title: "充值前余额", sortable: false },
                { data: "ChangeValue", title: "充值金额", sortable: false },

                { data: "ChangeAfter", title: "充值后余额", sortable: false },
                { data: "CarNo", title: "车牌号", sortable: false },
                    { data: "VIN", title: "车架号", sortable: false },
                    { data: "UserName", title: "操作人", sortable: false },
                    { data: "AddedDate", title: "充值日期", sortable: true },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="print(\'' + obj.MemberCardNo + '\',\'' + obj.CustomerName + '\',\'' + obj.ChangeValue + '\',\'' + obj.ChangeAfter + '\',\'' + obj.CarNo + '\',\'' + obj.VIN + '\',\'' + obj.UserName + '\',\'' + obj.AddedDate + '\')">打印</button> ';
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

function print(a,b,c,d,e,f,g,h) {
    var orderMaster = {
        OrderDate: h,//零售单日期
        //OrderNumber: pendingLoadOrderObject.OrderCode,//流水号
        OrderSumAmt: Number(d).toFixed(2),//总金额
        actAmt: Number(c).toFixed(2),//实付金额
        totalAmt: Number(d).toFixed(2),//找零金额   
        storeCode: $("#hdnStoreName").val(),//门店名称
        userCode: g,//员工名字
        cardNo: a,//会员卡号
        memName: b,
        veNo: e,
        vinNo: f,
        PrintDate: (new Date()).toLocaleDateString(),
    };
    //传输数据
    var data = {
        Type: "StoreValue",
        //Store: store,
        OrderMaster: orderMaster,
        Printer: $("#hdnPrinter").val()
    }
    printPage("../Print.html", data, null);
}