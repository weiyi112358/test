var tableMemberInfo, dtPackage, dtPackageDetail_P, dtAccountCouponList, dt_CouponData;
$(function () {




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

    //加载优惠券
    loadCouponList();
    $("#btnCouponSearch").click(function () {
        loadCouponList();
    })
    $("#addCoupon").click(function () {
        addCoupon();
    })


})

function loadCouponList() {
    if (!dt_CouponData) {
        dt_CouponData = $('#dt_CouponData').dataTable({
            sAjaxSource: '/Member360/GetCouponAddData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: 'TempletID', title: "优惠券模板编号", sortable: true, },
                { data: 'CouponName', title: "优惠券名称", sortable: false, },
                { data: 'CouponType', title: "优惠券类型", sortable: false, },

                { data: 'limitName', title: "限定条件", sortable: false,sWidth:"20%" },
                {
                    data: 'StartDate', title: "生效时间", sortable: false, render: function (obj) {
                        return obj.substr(0, 10);
                    }
                },
                {
                    data: 'EndDate', title: "到期时间", sortable: false, render: function (obj) {
                        return obj.substr(0, 10);
                    }
                },
                 {
                     data: null, title: "操作", sClass: "center", sortable: false,
                     render: function (obj) {
                         return "<button class=\"btn\" id=\"btnChose\"  onclick=\"chose(" + obj.TempletID + ")\">选择</button>";
                     }
                 }
            ],
            fnFixData: function (d) {
                d.push({ name: 'couponName', value: $("#txtCouponName").val() });
            }
        });
    } else {
        dt_CouponData.fnDraw();
    }
}
var memberId;
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
    $.colorbox.close();
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            $("#txtActId").val('');
            var mid = res.Obj[0].MemberID;
            memberId = mid;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);
            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].OptionText == null ? "" : res.Obj[0].OptionText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].MemberCardStatus == null ? "未开卡" : res.Obj[0].MemberCardStatus);
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

            //$("#radioCash").prop("checked", true);
            //loadAccountList($("input[name='reg_det']:checked").val());
        }
    })
}

function chose(TempletID) {

    $("#txtTempleID").val(TempletID);
}

//加载优惠券
function showAccountCouponList() {
    if (!dtAccountCouponList) {
        dtAccountCouponList = $("#dtAccountCouponList").dataTable({
            sAjaxSource: '/Member360/GetMemAccountCouponList',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,

            aoColumns: [
                {
                    data: "CouponName", title: "优惠券名称", sortable: false
                },
                { data: "CouponTypeText", title: "优惠券类型", sortable: false },
                { data: "LimitText", title: "限制类型", sortable: false },
                { data: "Enable", title: "是否可用", sortable: false },
                {
                    data: "StartDate", title: "开始日期", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: "EndDate", title: "结束日期", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "IsUsed", title: "是否已用", sortable: false },
                {
                    data: "AddedDate", title: "添加时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#hdnMemberId").val() });
            }

        });
    }
    else {
        dtAccountCouponList.fnDraw();
    }
}


function addCoupon() {
    var templetID = $("#txtTempleID").val();
    if (memberId == '' || memberId == undefined) {
        $.dialog("请先查询并选择会员");
        return false;
    }

    //if ($("#txtCouponCode").val() == null || $("#txtCouponCode").val() == "") {
    //    $.dialog("请输入优惠券号");
    //    return false;
    //}
    if (!!isNaN(templetID)) {
        $.dialog("优惠券模板编号 请输入数字");
        return false;
    }
    if ($("#txtReason").val() == null || $("#txtReason").val() == "") {
        $.dialog("请输入添加原因");
        return false;
    }
    var couponCode = $("#txtCouponCode").val();
    var reason = $("#txtReason").val();
    ajax('/Member360/AddCouponByCode', { mid: memberId, templetID: Number(templetID), couponCode: couponCode, reason: reason }, function (res) {
        if (res.IsPass) {
            $.dialog(res.MSG);
        }
        else {
            $.dialog(res.MSG);
        }

    })

}