var tableMemberInfo, tableMembPackage;
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

        }
        
    })
    loadMemPackageList(id);
}

//加载会员套餐列表
function loadMemPackageList(id) {
    if (!tableMembPackage) {
        tableMembPackage = $('#dt_package').dataTable({
            sAjaxSource: '/Member360/GetMemPackageList',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [

            { data: 'PackageInstanceID', title: "编号", sortable: true },
                { data: 'PackageName', title: "套餐名称", sortable: false },
                { data: 'PackageDesc', title: "套餐描述", sortable: false },

                { data: 'PurchasePrice2', title: "购买价格", sortable: false },

                {
                    data: 'AddedDate', title: "购买日期", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },

                {
                    data: 'IsPresented', title: "是否赠送", sortable: false, render: function (obj) {
                        return obj==false ? "是" : "否";
                    }
                },
                { data: 'UserName', title: "操作员工", sortable: false },
            {
                data: "StartDate", title: "有效起始日期", sortable: false, render: function (obj) {
                    return !obj ? "" : obj.substr(0, 10);
                }
            },
                {
                    data: "EndDate", title: "有效结束日期", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: null, title: "操作", sClass: "center", sWidth: "20%", sortable: false,
                    render: function (obj) {
                        return "<button class=\"btn\" onclick=\"deleteMempackage(" + obj.PackageInstanceID + ")\">删除</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: id });
            }
        });
    }
    else {
        tableMembPackage.fnDraw();
    }
}

//删除会员套餐
function deleteMempackage(pId) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Member360/DeleteMemPackage", { packageID: pId, mid: $("#hdnMemberId").val() }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                loadMemPackageList();
            } else { $.dialog(res.MSG); }
        });
    })
}