var dt_StoreData, tableMemberInfo;
$(function () {

    //搜索会员
    $("#btnSearch").click(function () {
        //至少有一个条件才能搜索
        if ($("#txtNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '') {
            $.dialog("至少输入一个条件以供查询");
            return;
        }

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
            iDisplayLength: 5,
            aoColumns: [
                { data: "MemberCardNo", title: "会员编号", sortable: false, sWidth: "25%" },

                { data: "CustomerName", title: "会员名称", sortable: false },

                    { data: "CustomerMobile", title: "手机", sortable: false },
                    //{ data: "CustomerLevelText", title: "会员等级", sortable: false },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="detail(\'' + obj.MemberID + '\')">查看</button> ';
                        return str;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'memNo', value: encode($("#txtNo").val()) });
                d.push({ name: 'memName', value: encode($("#txtName").val()) });
                d.push({ name: 'memMobile', value: encode($("#txtMobile").val()) });
            }
        });
    }
    else {
        tableMemberInfo.fnDraw();
    }
}


function detail(id) {
    $.colorbox.close();

    $("#txtMemId").val(id);
    $("#btnAdd").show();
    $(".memInfoBlock").show();
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            //memberId = mid;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);

            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "v1" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].CustomerStatus == 1 ? "正常" : "停用");
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

        }
    })
    loadCashPoint(id);

    loadVehcilefoList();
}

//加载账户积分现金信息
function loadCashPoint(id) {
    ajax("/Member360/GetMemIsBackAccountInfo", { mid: id }, function (data) {
        $("#stgValidValue1").text(0);
        $("#stgValidValue2").text(0);
        if (data.length > 0) {
            $("#stgValidValue1").text(data[0].Value2);
            $("#stgValidValue3").text(data[0].Value1);
            $("#stgValidValue2").text(data[0].NoBackAccount == null ? "0" : data[0].NoBackAccount);
            $("#txtCash").val(data[0].Total);
            total = data[0].Total;
        }
    });
}
//加载会员信息
function loadVehcilefoList() {
    if (!dt_StoreData) {
        dt_StoreData = $('#dt_StoreData').dataTable({
            sAjaxSource: '/Member360/GetCouponListByMid',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: "CouponID", title: "优惠券ID", sortable: false },
                { data: "CouponCode", title: "优惠券代码", sortable: false },
                //{ data: "TempletID", title: "模板ID", sortable: false },
                { data: "Name", title: "优惠券名称", sortable: false },
                { data: "StartDate", title: "开始时间", sortable: false },
                { data: "EndDate", title: "结束时间", sortable: false },
                {
                    data: "Enable", title: "是否可用", sortable: false, render: function (obj) {
                        return obj == true ? "是" : "否";
                    }
                },
                {
                    data: "IsUsed", title: "是否已使用", sortable: false, render: function (obj) {
                        return obj == true ? "是" : "否";
                    }
                },
                //{ data: "Counts", title: "数量", sortable: false },
                
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        if (obj.IsUsed == 0) {
                            return str = "<button class=\"btn\" id=\"btnModify\"  onclick=\"Active(" + obj.CouponID + ")\">使用</button>";
                        }
                        else return "";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtMemId").val() });
            }
        });
    }
    else {
        dt_StoreData.fnDraw();
    }
}


function goClear() {
    
}


//审批
function Active(id, obj) {

    $.dialog("确认使用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Member360/UsingCoupon", { cid: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreData.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

