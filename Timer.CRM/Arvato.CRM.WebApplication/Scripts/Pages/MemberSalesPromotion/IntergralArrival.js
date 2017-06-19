var dt_Table;
$(function () {
    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/CouponUseRule/LoadCouponUseRule',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aoColumns: [
            { data: 'ID', title: "购物券编号", sClass: "center", sortable: true },
            { data: 'CouponName', title: "购物券名称", sClass: "center", sortable: false },
            { data: 'CouponValue', title: "面额", sClass: "center", sortable: false },
            {
                data: null, title: "Logo", sClass: "center", sortable: false, render: function (obj) {
                    return '<img src="' + obj.LogoPath + '" style="width:20px;heigth:20px" />';
                }
            },
            {
                data: null, title: "开始时间", sClass: "center", sortable: false, render: function (obj) {
                    return obj.StartDate.substr(0, 10);
                }
            },
            {
                data: null, title: "结束时间", sClass: "center", sortable: false, render: function (obj) {
                    return obj.EndDate.substr(0, 10);
                }
            },
            {
                data: null, title: "是否会员券", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.IsMember) {
                        return "<span class=\"label label-success\">是</span>";
                    } else {
                        return "<span class=\"label label-danger\">不是</span>";
                    }
                    return result;
                }
            },
            {
                data: null, title: "状态", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.IsEnble) {
                        return "<span class=\"label label-success\">启用</span>";
                    } else {
                        return "<span class=\"label label-danger\">禁用</span>";
                    }
                    return result;
                }
            },
             {
                 data: null, title: "操作", sClass: "center", sortable: false,
                 render: function (obj) {
                     var result = "";
                     result += "<button class=\"btn\" id=\"btnModify\"  onclick=\"EditRule(" + obj.ID + ")\">编辑</button>&nbsp;&nbsp;";
                     if (obj.IsEnble)
                         result += "<button class=\"btn\" onclick=\"goDisabled(" + obj.ID + ")\">禁用</button>&nbsp;&nbsp;";
                     else
                         result += "<button class=\"btn\"  onclick=\"goEnable(" + obj.ID + ")\">启用</button>&nbsp;&nbsp;";
                     result += "<button class=\"btn btn-danger\" id=\"btnDelete\" onclick=\"goDelete(" + obj.ID + ")\">删除</button>";
                     return result;
                 }
             }
        ],
        fnFixData: function (d) {
            d.push({ name: 'ID', value: $("#txtCouponId").val() });
            d.push({ name: 'CouponName', value: encode($("#txtCouponName").val()) });
        }
    });
    //查询
    $("#btnSerach").bind("click", function () {
        dt_Table.fnDraw();
    });
    //新建
    $("#btnAdd").bind("click", function () {
        AddRule();
    });

});
//新增规则
function AddRule() {
    $("#hideID").val('');
    $("#form1").submit();
}
//修改规则 
function EditRule(id) {
    $("#hideID").val(id);
    $("#form1").submit();
}
//禁用优惠券
function goDisabled(id) {
    $.dialog("确认禁用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/CouponUseRule/ChangeCouponStatus", { Id: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_Table.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}
//启用优惠券
function goEnable(id) {
    $.dialog("确认启用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/CouponUseRule/ChangeCouponStatus", { Id: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_Table.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}
//删除优惠券
function goDelete(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/CouponUseRule/DeleteCouponById", { Id: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_Table.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

