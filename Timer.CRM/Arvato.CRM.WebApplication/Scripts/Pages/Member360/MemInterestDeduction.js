var tableAccountInfo, tableCouponInfo, tablePackageInfo, tableMemberInfo;
$(function () {
    //加载门店列表
    loadStoreList();

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

    //查询权益
    $("#btnSearch1").click(function () {
        if (!$("#txtMemId").val()) {
            $.dialog("会员编号不能为空");
            return;
        }
        if (!$("#txtTradeCode").val()) {
            $.dialog("委托书号不能为空");
            return;
        }

        //检验委托书号是否存在
        if (checkTradeCode) {
            //同时加载账户，优惠券，套餐列表
            loadAccountList();
            loadCouponList();
            loadPackageList();
        } else {
            $.dialog("委托书号不匹配");
        }
    })

    //添加公共优惠券
    $("#btnAddPuplic").click(function () {
        //优惠券列表添加公共优惠券
        ajax('/Member360/GetPublicCoupon', { couponCode: $("#txtPuplicCoupon").val() }, function (res) {
            if (res.IsPass) {
                //公共优惠券添加到优惠券列表
                //alert(res.Obj);
                var tbody = $("#dtCouponInfo tbody");
                var tr = $('<tr></tr>');
                tr.append('<td>' + res.Obj[0].Name + '</td>')
                        .append('<td>' + 1 + '</td>')
                        .append('<td></td>')
                        .append('<td><input type="checkbox" /></td>')
                        .append("<td><input type='text'  class='input-medium' onkeyup=\"value=value.replace(/[^\\d.]/g,'')\"/></td>").appendTo(tbody);

            } else $.dialog(res.MSG);
        });
    })

    //确认减扣
    $("#btnEnter").click(function () {
        var mId = $("#txtMemId").val();
        //账户
        var accountList = [];
        var trObj1 = $("#dtAccountInfo tbody tr");
        if (trObj1[0].cells[0].innerText != "没有记录") {
            for (var i = 0; i < trObj1.length; i++) {
                //账户编号
                var pacId = trObj1[i].cells[0].innerText;

                //扣减值
                var qty = trObj1[i].cells[3].getElementsByTagName("input")[0].value;
                var isBuy = trObj1[i].cells[2].getElementsByTagName("input")[0].checked;
                if (isBuy)
                    accountList.push({ AccountType: (i+1), MemberId: mId, ChangeValue: qty });

            }
        }
        //优惠券
        var couponList = [];
        var trObj2 = $("#dtCouponInfo tbody tr");
        if (trObj2[0].cells[0].innerText != "没有记录") {
            for (var i = 0; i < trObj2.length; i++) {
                //优惠券编号
                var pacId = trObj2[i].cells[0].innerText;
                //优惠券使用数量
                var qty = trObj2[i].cells[5].getElementsByTagName("input")[0].value;


                var isBuy = trObj2[i].cells[4].getElementsByTagName("input")[0].checked;
                if (isBuy)
                    couponList.push({ CouponId: pacId, MemberId: mId, Qty: qty });

            }
        }
        //套餐
        var packageList = [];
        var trObj3 = $("#dtPackageInfo tbody tr");
        if (trObj3[0].cells[0].innerText != "没有记录") {
            for (var i = 0; i < trObj3.length; i++) {
                //套餐条目编号
                var pacId = trObj3[i].cells[1].innerText;
                //购买数量
                var qty = trObj3[i].cells[6].getElementsByTagName("input")[0].value;

                var isBuy = trObj3[i].cells[5].getElementsByTagName("input")[0].checked;
                if (isBuy)
                    packageList.push({ PackageId: 0, PackageDetailId: pacId, MemberId: mId, Qty: qty });

            }
        }
        if (packageList.length <= 0 && couponList.length <= 0 && accountList.length <= 0) {
            $.dialog("请选择扣减项");
            return;
        }
        ajax('/Member360/SaveMemInterestDeduction', { packageList: packageList, couponList: couponList, accountList: accountList, mid: mId }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                //同时刷新账户，优惠券，套餐列表
                loadAccountList();
                loadCouponList();
                loadPackageList();
            } else $.dialog(res.MSG);
        });
    })


    //打印
    $("#btnPrint").click(function () {
        //打印数据

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
    $.colorbox.close();
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            //memberId = mid;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);
            $("#hdnMemberId").val(mid);
            $("#txtMemId").val(mid);
            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].OptionText == null ? "" : res.Obj[0].OptionText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].MemberCardStatus == null ? "未开卡" : res.Obj[0].MemberCardStatus);
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

        }
    })
}
//验证委托书单号
function checkTradeCode() {
    ajax('/Member360/CheckTradeCode', { mId: $("#txtMemId").val(), tradeCode: $("#txtTradeCode").val() }, function (res) {
        if (res.IsPass) {
            return true;
        } else return false;
    });
}
//加载账户
function loadAccountList() {
    if (!tableAccountInfo) {
        tableAccountInfo = $('#dtAccountInfo').dataTable({
            sAjaxSource: '/Member360/GetAccountInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 4,
            aoColumns: [
                //{ data: "memberCardNo", title: "账户类型", sortable: false, },
                { data: "OptionText", title: "名称", sortable: false },
                { data: "Value1", title: "可用数量", sortable: false },
                //{ data: "customerName", title: "有效截止日期", sortable: false },
                {
                    data: null, title: "使用", sortable: false, render: function (obj) {
                        return "<input type='checkbox' />";
                    }
                },
                {
                    data: null, title: "本次使用数量", sortable: false, render: function (obj) {
                        return "<input type='text'  class='input-medium' onkeyup=\"value=value.replace(/[^\\d.]/g,'')\"/>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtMemId").val() });
                
            }
        });
    }
    else {
        tableAccountInfo.fnDraw();
    }
}
//加载优惠券
function loadCouponList() {
    if (!tableCouponInfo) {
        tableCouponInfo = $('#dtCouponInfo').dataTable({
            sAjaxSource: '/Member360/GetCouponInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 4,
            aoColumns: [
                { data: "CouponID", title: "优惠券编号", sortable: false, },
                { data: "Name", title: "优惠券名称", sortable: false },
                { data: "Qty", title: "可用数量", sortable: false },
                { data: "EndDate", title: "有效截止日期", sortable: false },
                {
                    data: null, title: "使用", sortable: false, render: function (obj) {
                        return "<input type='checkbox' />";
                    }
                },
                {
                    data: null, title: "本次使用数量", sortable: false, render: function (obj) {
                        return "<input type='text'  class='input-medium' onkeyup=\"value=value.replace(/[^\\d.]/g,'')\"/>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtMemId").val() });
                d.push({ name: 'storeCode', value: $("#drpStore").val() });
            }
        });
    }
    else {
        tableCouponInfo.fnDraw();
    }
}
//加载套餐
function loadPackageList() {
    if (!tablePackageInfo) {
        tablePackageInfo = $('#dtPackageInfo').dataTable({
            sAjaxSource: '/Member360/GetPackageInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 4,
            aoColumns: [
                { data: "PackageName", title: "套餐名称", sortable: false, },
                { data: "PackageInstanceDetailID", title: "条目编号", sortable: false, },
                { data: "ItemDesc", title: "套餐明细", sortable: false },
                { data: "Qty", title: "可用数量", sortable: false },
                { data: "EndDate", title: "有效截止日期", sortable: false },
                {
                    data: null, title: "使用", sortable: false, render: function (obj) {
                        return "<input type='checkbox' />";
                    }
                },
                {
                    data: null, title: "本次使用数量", sortable: false, render: function (obj) {
                        return "<input type='text'  class='input-medium' onkeyup=\"value=value.replace(/[^\\d.]/g,'')\"/>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtMemId").val() });
                d.push({ name: 'storeCode', value: $("#drpStore").val() });
            }
        });
    }
    else {
        tablePackageInfo.fnDraw();
    }
}
//加载门店列表
function loadStoreList() {
    ajax("/Member360/GetActLimitStoreList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drpStore').append(opt);

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStore').append(opt);
        }
    });
}