var dt_Table;
$(function ()
{
    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/CouponUseRule/LoadCouponSendRule',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
           {
               data: 'ID', title: "购物券编号", sClass: "center", sortable: true, render: function (obj)
               {
                   return '<a class="billcode" href="#" onclick="EditRule(' + obj + ',0)">' + obj + '</a>';
               }
           },
            { data: 'CouponName', title: "购物券名称", sClass: "center", sortable: false },
            { data: 'CouponValue', title: "面额", sClass: "center", sortable: false },
            {
                data: 'CouponRemark', title: "购物券说明", sClass: "center", sortable: false, render: function (obj)
                {
                    if (obj == null)
                        obj = '';
                    return "<textarea readonly>{0}</textarea>".format(obj);
                }
            },
            {
                data: null, title: "开始时间", sClass: "center", sortable: false, render: function (obj)
                {
                    return obj.StartDate.substr(0, 10);
                }
            },
            {
                data: null, title: "结束时间", sClass: "center", sortable: false, render: function (obj)
                {
                    return obj.EndDate.substr(0, 10);
                }
            },
            {
                data: null, title: "是否会员券", sClass: "center", sortable: false,
                render: function (obj)
                {
                    if (obj.IsMember)
                    {
                        return "<span class=\"label label-success\">是</span>";
                    } else
                    {
                        return "<span class=\"label label-danger\">否</span>";
                    }
                    return result;
                }
            },
            { data: "ExeStatus", title: "执行状态", sClass: "center", sortable: false },
            { data: "AppStatus", title: "审核状态", sClass: "center", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj)
                {
                    var action = '<button class=\"btn\ modifyRule" onclick="EditRule(' + obj.ID + ',1)"  >编辑</button>';
                    if (obj.ExeStatus == '执行中')
                        action += '<button class=\"btn btn-danger\ sleepRule" billid="{0}" {1}  >休眠</button>';
                    else if (obj.ExeStatus == '休眠中')
                        action += '<button class=\"btn btn-info\ wakeupRule" billid="{0}"  {1}  >唤醒</button>';
                    //未审核，可以删除
                    if (obj.AppStatus == "未审核")
                    {
                        action += '<button class=\"btn btn-info\ approveRule" billid="{0}"  >审核</button>';
                        action += '<button class=\"btn btn-danger\ deleteRule" billid="{0}"  >删除</button>';
                    }
                    else if (obj.AppStatus == "已审核")
                    {
                        action += '<button class=\"btn btn-danger\ voidRule" billid="{0}"  >作废</button>';
                    }
                    if (obj.AppStatus == "已作废")
                    {
                        return '';
                    }
                    else
                        action = action.format(obj.ID, '');
                    return action;
                }
            }
        ],
        fnFixData: function (d)
        {
            d.push({ name: 'strmodel', value: getSearchModel() });
        }
    });
    //绑定时间
    $("#txtStartDate,#txtEndDate").datepicker();
    //重置
    $("#btnClear").bind("click", function ()
    {
        $("#txtCouponId").val(''),
        $("#txtCouponName").val(''),
        $("#txtCouponValue").val(''),
        $("#txtCouponRemark").val(''),
        $("#txtStartDate").val(''),
        $("#txtEndDate").val(''),
        $("#txtApproveStatus").val(''),
        $("#txtExecuteStatus").val('')
    });
    //查询
    $("#btnSerach").bind("click", function ()
    {
        var Id = $("#txtCouponId").val();
        var reg = /^(?!0)\d{1,10}$/;
        if (Id != '' && !reg.test(Id))
        {
            $.dialog("请输入10位以内的购物券编号");
            return
        }
        dt_Table.fnDraw();
    });
    //新建
    $("#btnAdd").bind("click", function ()
    {
        AddRule();
    });
    //唤醒
    $('#dt_Table').on('click', '.wakeupRule', function ()
    {
        var id = $(this).attr('billid');
        $.dialog("确认唤醒吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function ()
        {
            ajax("/CouponUseRule/ChangeCouponSendRuleStatus", { Id: id }, showResult);
        })
    });
    //休眠
    $('#dt_Table').on('click', '.sleepRule', function ()
    {
        var id = $(this).attr('billid');
        $.dialog("确认休眠吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function ()
        {
            ajax("/CouponUseRule/ChangeCouponSendRuleStatus", { Id: id }, showResult);
        })
    });
    //审核
    $('#dt_Table').on('click', '.approveRule', function ()
    {
        var id = $(this).attr('billid');
        $.dialog("确认审核此规则吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function ()
        {
            ajax("/CouponUseRule/ApproveCouponSendById", { Id: id, active: 1 }, showResult);
        })
    });
    //作废
    $('#dt_Table').on('click', '.voidRule', function ()
    {
        var id = $(this).attr('billid');
        $.dialog("确认作废此规则吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function ()
        {
            ajax("/CouponUseRule/ApproveCouponSendById", { Id: id, active: 2 }, showResult);
        })
    });
    //删除
    $('#dt_Table').on('click', '.deleteRule', function ()
    {
        var id = $(this).attr('billid');
        $.dialog("确认删除吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function ()
        {
            ajax("/CouponUseRule/DeleteCouponSendRuleById", { Id: id }, showResult);
        })
    });

});
//获取查询对象
function getSearchModel()
{
    var model = {
        ID: $("#txtCouponId").val(),
        CouponName: $("#txtCouponName").val(),
        CouponValue: $("#txtCouponValue").val(),
        CouponRemark: $("#txtCouponRemark").val(),
        StartDate: $("#txtStartDate").val(),
        EndDate: $("#txtEndDate").val(),
        ApproveStatus: $("#txtApproveStatus").val(),
        ExecuteStatus: $("#txtExecuteStatus").val()
    };
    return JSON.stringify(model);
}
//新增规则
function AddRule()
{
    $("#hideID").val('');
    $("#form1").submit();
}
//修改规则 
function EditRule(id,isinfo)
{
    $("#hideID").val(id);
    $("#hideInfo").val(isinfo);
    $("#form1").submit();
}
//显示结果
function showResult(res)
{
    if (res.IsPass)
    {
        $.dialog(res.MSG);
        var start = dt_Table.fnSettings()._iDisplayStart;
        var length = dt_Table.fnSettings()._iDisplayLength;
        dt_Table.fnPageChange(start / length, true);
    } else
    { $.dialog(res.MSG); }
}



