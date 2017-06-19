var dtPackageDetail, tableMemberInfo;
var total, addvalue, memname, memcardno, memveno, memvinkey;//账户总金额,充值金额,会员名字,会员卡号,车牌号,车架号
var flag = 0;//打印标记
var printPackageList = [];//打印套餐信息
var printPacDetail = [];//条目信息
$(function () {


    $(".chzn_store").chosen();
    GetActLimitStoreList();//记载限制门店

    //搜索会员
    $("#btnSearch").click(function () {
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#table_Member",
            inline: true,
            opacity: '0.3',
            onComplete: function () {
                loadMemInfoList();
                $.colorbox.resize();
            }
        });
        //$.colorbox.resize();
    })
    //搜索套餐
    $("#btnSerachPackage").click(function () {
        dt_package.fnDraw();
    })

    //计算总价
    $("#btnCompute").click(function () {
        printPackageList = [];
        printPacDetail = [];
        var trObj = $("#dt_package tbody tr");
        var total = 0;
        for (var i = 0; i < trObj.length; i++) {
            //套餐价格
            var pacId = trObj[i].cells[8].getElementsByTagName("input")[0].value;//trObj[i].cells[2].innerText;
            //购买数量
            var qty = trObj[i].cells[7].getElementsByTagName("input")[0].value;

            var isBuy = trObj[i].cells[6].getElementsByTagName("input")[0].checked;
            if (isBuy) {
                total += parseInt(pacId) * qty;
            }
        }
        $("#txtTotal").val(total);
    })


    //确认购买套餐
    $("#btnEnter").click(function () {
        //限制门店
        //var limitList = new Array();
        //$("#drpLimitStore").find("option:selected").each(function (i, data) {
        //    var value = data.value;
        //    limitList.push({ LimitType: "store", LimitValue: value });
        //    //limitList[i] = value;
        //});
        //购买列表
        var packageList = [];
        var trObj = $("#dt_package tbody tr");
        for (var i = 0; i < trObj.length; i++) {
            //套餐编号
            var test1 = trObj[i].cells[0];
            var pacId = test1.innerText;
            if (pacId == undefined) { pacId = test1.textContent }
            //套餐名称
            var test2 = trObj[i].cells[1];
            var pacName = test2.innerText;
            if (pacName == undefined) { pacName = test2.textContent }
            //购买数量
            var qty = trObj[i].cells[7].getElementsByTagName("input")[0].value;
            //价格
            var price = trObj[i].cells[8].getElementsByTagName("input")[0].value;
            var mId = $("#hdnMemberId").val();
            var isBuy = trObj[i].cells[6].getElementsByTagName("input")[0].checked;
            if (isBuy) {
                packageList.push({ PackageId: pacId, PackageDetailId: 0, MemberId: mId, Qty: qty, Price: price, IsPresented: price == 0 ? false : true });

                var pid = trObj[i].cells[9].getElementsByTagName("input")[0].value;
                getItemDetailList(pid);

                printPackageList.push({ pacName: pacName, pacNum: qty, pacPrice: parseFloat(price) * parseInt(qty), itemlist: printPacDetail })
            }

        }
        if ($("#hdnMemberId").val() == "") {
            $.dialog("请先选择会员");
            return;
        }
        if ($("#txtTotal").val() == "") {
            $.dialog("请先计算总价");
            return;
        }
        if (!utility.isMoney($("#txtTotal").val())) {
            $.dialog("总价格式不正确");
            return;
        }
        if (packageList.length < 0) {
            $.dialog("请选择套餐");
            return;
        }
        total = $("#txtTotal").val();//打印总价
        ajax('/Member360/SavePackageSaleData', { packageList: packageList, mid: mId, total: $("#txtTotal").val() }, function (res) {
            if (res.IsPass) {
                flag = 1;
                $.dialog(res.MSG);
            } else $.dialog(res.MSG);
        });
    })

    dt_package = $('#dt_package').dataTable({
        sAjaxSource: '/Member360/GetPackageList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 5,
        aoColumns: [
            { data: 'PackageID', title: "编号", sortable: true },
            { data: 'PackageName', title: "套餐名称", sortable: false },
            { data: 'Price1', title: "套餐价格", sortable: false },

            { data: 'limitName', title: "限定条件", sortable: false },
            //{ data: 'Price2', title: "内部结算价", sortable: false },
            {
                data: "StartDate", title: "有效起始日期", sortable: true, render: function (obj) {
                    return !obj ? "" : obj.substr(0, 10);
                }
            },
                {
                    data: "EndDate", title: "有效结束日期", sortable: true, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: null, title: "购买", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<input type='checkbox' />";
                    }
                },
                {
                    data: null, title: "购买数量", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<input type='text'  class='input-medium span6' onkeyup=\"value=value.replace(/[^\\d.]/g,'')\"/>";
                    }
                },

            {
                data: null, title: "购买价格", sortable: false,
                render: function (obj) {
                    return "<input type='text'  class='input-medium span6' value='" + (obj.Price1 == null ? "0" : obj.Price1) + "' onkeyup=\"value=value.replace(/[^\\d.]/g,'')\"/>";
                }
            },
            {
                data: null, title: "操作", sClass: "center", sWidth: "15%", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" onclick=\"goPackageDetail(" + obj.PackageID + ")\">明细</button><input type='hidden'  class='input-medium' value='" + obj.PackageID + "'/>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'pName', value: $("#txtPackageName").val() });
        }
    });

    //打印
    $("#btnPrint").click(function () {
        if (flag == 0) {
            $.dialog("没有套餐购买信息，请先购买套餐");
            return;
        }

        //获取车牌号
        ajaxSync("/Member360/GetActLimitVehicleList", { memId: $("#hdnMemberId").val() }, function (res) {
            if (res.length > 0) {
                memveno = res[0].CarNo;
            } else {
                memveno = "";
            }
        });

        var orderMaster = {
            OrderDate: (new Date()).toLocaleDateString(),//零售单日期
            //OrderNumber: pendingLoadOrderObject.OrderCode,//流水号
            OrderSumAmt: Number(total).toFixed(2),//总金额
            actAmt: Number(addvalue).toFixed(2),//实付金额
            totalAmt: Number(total).toFixed(2),//找零金额
            //salesPersonCode: salesPerson,//销售员代码
            //salesPersonName: salesPersonName,//销售员名字     
            storeCode: $("#hdnStoreName").val(),//门店名称
            //cashboxCode: userLoginInfo.CashboxCode,//pos机号
            userCode: $("#hdnUserName").val(),//员工名字
            cardNo: memcardno,//会员卡号
            memName: memname,
            veNo: utility.isNull(memveno) ? '' : memveno,
            vinNo: utility.isNull(memvinkey) ? '' : memvinkey,
            PrintDate: (new Date()).toLocaleDateString(),
            //memberPointValue: memberPointValue,//积分余额
            //memberAccountValue: memberAccountValue,//账户余额
            //RePrintFlag: XPRePrintFlag
        };

        //传输数据
        var data = {
            Type: "PackageSale",
            //Store: store,
            OrderMaster: orderMaster,
            //OrderDetailList: orderDetailList,
            //OrderPaymentList: orderPaymentList,
            PackageList: printPackageList,
            Printer: $("#hdnPrinter").val()
        }
        printPage("../Print.html", data, function (res) {
            printPackageList = [];
            printPacDetail = [];
        });
    })
})
//加载会员信息
function loadMemInfoList() {
    if (!tableMemberInfo) {
        tableMemberInfo = $('#tableMemberInfo').dataTable({
            sAjaxSource: '/Member360/GetMembersNameByPage',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 4,
            aoColumns: [
                { data: "MemberCardNo", title: "会员卡号", sortable: false, sWidth: "33%" },
                { data: "CustomerName", title: "会员名称", sortable: false },
                { data: "CustomerMobile", title: "手机", sortable: false },
                    { data: "CustomerStatusText", title: "会员状态", sortable: false },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="detail(\'' + obj.MemberID + '\')">查看</button> ';
                        return str;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'cardNo', value: $("#txtCardNo").val() });
                d.push({ name: 'memName', value: $("#txtName").val() });
                d.push({ name: 'memMobile', value: $("#txtMobile").val() });
                d.push({ name: 'vehicleNo', value: $("#txtVehicle").val() });
                //d.push({ name: 'vehicleVIN', value: $("#txtVIN").val() });
            }
        });
    }
    else {
        tableMemberInfo.fnDraw();
    }
}

//查看会员详细信息
function detail(id) {
    //新会员时，清空上次的信息
    printPackageList = [];
    printPacDetail = [];
    $("#txtTotal").val('');
    dt_package.fnDraw();

    flag = 0;
    $.colorbox.close();
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            //memberId = mid;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);
            $("#hdnMemberId").val(mid);
            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].OptionText == null ? "" : res.Obj[0].OptionText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].MemberCardStatus == null ? "未开卡" : res.Obj[0].MemberCardStatus);
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

            memname = res.Obj[0].CustomerName;
            memcardno = res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo;
            memvinkey = res.Obj[0].VinKey == null ? "" : res.Obj[0].VinKey;
        }
    })
}
function getItemDetailList(pid) {
    printPacDetail = [];
    ajaxSync('/BaseData/GetPackageDetailList1', { packageId: pid }, function (res) {
        for (var i = 0; i < res.length; i++) {
            printPacDetail.push({ itemName: res[i].ItemName, itemQty: res[i].Qty });
        }
    })
}
//查看明细
function goPackageDetail(pId) {
    $("#hdnPid").val(pId);
    //加载明细
    if (!dtPackageDetail) {
        dtPackageDetail = $("#dtPackageDetail").dataTable({
            sAjaxSource: '/BaseData/GetPackageDetailList',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                //{ data: "PackageDetailID", title: "套餐明细编号", sortable: false },
                { data: "ItemName", title: "条目名称", sortable: false },
                { data: "ItemDesc", title: "条目描述", sortable: false },
                { data: "Qty", title: "数量", sortable: false },
            ],
            fnFixData: function (d) {
                d.push({ name: 'packageId', value: $("#hdnPid").val() });
            }
        });
    }
    else {
        dtPackageDetail.fnDraw();
    }
}

//加载具体门店使用限制
function GetActLimitStoreList() {
    ajax("/Member360/GetActLimitStoreList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drpLimitStore').append(opt);
            $(".chzn_store").trigger("liszt:updated");

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpLimitStore').append(opt);
        }
    });
}