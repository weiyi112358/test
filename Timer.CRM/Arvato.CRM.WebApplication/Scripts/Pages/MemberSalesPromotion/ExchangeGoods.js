var dt_Table;
$(function () {

    $("#txtStartDate,#txtEndDate").datepicker();

    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/MemberSalesPromotion/LoadRuleList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[0, "desc"]],
        aoColumns: [
           {
               data: 'Code', title: "单号", sClass: "center", sortable: true
           },
            {
                data: 'Status', title: "状态", sClass: "center", sortable: false, render: function (d) {
                    var msg = "";
                    if (d == "1") {
                        msg = "已审核";
                    }
                    else if (d == "2") {
                        msg = "已撤销";
                    }
                    else {
                        msg = "未审核";
                    }
                    return msg
                }
            },
            {
                data: 'ModifiedUser', title: "最后修改人", sClass: "center", sortable: false
            },
            {
                data: 'ExchangeType', title: "兑奖类型", sClass: "center", sortable: false
            },
            {
                data: 'StartDate', title: "开始日期", sClass: "center", sortable: false, render: function (d) {
                    return d.substring(0,10)

                }
            },
            {
                data: 'EndDate', title: "结束日期", sClass: "center", sortable: false, render: function (d) {
                    return d.substring(0, 10)

                }
            },
            
            {
                data: 'Remark', title: "说明", sClass: "center", sortable: false
            },
            {
                data: null, title: "最后修改时间", sClass: "center", sortable: false, render: function (obj) {
                    return obj.ModifiedDate.substr(0, 10);
                }
            },
             {
                 data: null, title: "操作", sClass: "center", sortable: false,
                 render: function (obj) {
                     var action = '';
                     //未审核，可以删除
                     if (obj.Status == 0) {
                         action += '<button class=\"btn\ modifyRule" onclick="Edit(\'' + obj.ExchangeID + '\',1)"  >编辑</button>';
                         action += '<button class=\"btn btn-danger\ deleteRule" billid=\'' + obj.ExchangeID + '\'  >删除</button>';
                     }
                     else if (obj.Status == 1) {
                         action += '<button class=\"btn\ modifyRule" onclick="Edit(\'' + obj.ExchangeID + '\',2)"  >查看</button>';
                     }
                     else if (obj.Status == 2) {
                         action += '<button class=\"btn btn-danger\ deleteRule" billid=\'' + obj.ExchangeID + '\'  >删除</button>';
                     }
                     else
                         action = action.format(obj.CouponListID, '');

              
                     return action;
                 }
             }
        ],
        fnFixData: function (d) {
            d.push({ name: 'strmodel', value: getSearchModel() });
        }
    });

    ////重置
    //$("#btnRemove").bind("click", function () {
    //    $("#SearchOddNumber").val(''),
    //    $("#searchCouponInfo").val(''),
    //    $("#Status").selectIndex = 1
    //});
    //查询
    $("#btnSerach").bind("click", function () {
        dt_Table.fnDraw();
    });
    //新建
    $("#btnAdd").bind("click", function () {
        $("#hideID").val('');
        $("#hideInfo").val('');
        $("#form1").submit();
    });
    ////审核
    //$('#dt_Table').on('click', '.approveRule', function () {
    //    var id = $(this).attr('billid');
    //    $.dialog("确认审核此购物券派送吗?", {
    //        footer: {
    //            closebtn: '取消',
    //            okbtn: '确认'
    //        }
    //    }, function () {
    //        ajax("/CouponSend/ApproveCouponSendById", { Id: id, active: 1 }, showResult);
    //    })
    //});
    ////作废
    //$('#dt_Table').on('click', '.voidRule', function () {
    //    var id = $(this).attr('billid');
    //    $.dialog("确认作废此规则吗?", {
    //        footer: {
    //            closebtn: '取消',
    //            okbtn: '确认'
    //        }
    //    }, function () {
    //        ajax("/CouponSend/ApproveCouponSendById", { Id: id, active: 2 }, showResult);
    //    })
    //});
    //删除
    $('#dt_Table').on('click', '.deleteRule', function () {
        var id = $(this).attr('billid');
        $.dialog("确认删除吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax("/MemberSalesPromotion/DeleteExchangeRuleById", { Id: id }, showResult);
        })
    });
    

});


//获取查询对象
function getSearchModel() {
    


    var model = {
        ExchangeID: $("#ExchangeRuleCode").val(),
        ExchangeType: $("#ExchangeType").val(),
        Status: $("#Status").val(),
        Remark: $("#Remark").val(),
        StartDate: $("#txtStartDate").val() == "" ? "0001/1/1 0:00:00" : $("#txtStartDate").val(),
        EndDate: $("#txtEndDate").val() == "" ? "0001/1/1 0:00:00" : $("#txtEndDate").val(),
    };
    return JSON.stringify(model);
}

//修改
function Edit(id, isinfo) {
    $("#hideID").val(id);
    $("#hideInfo").val(isinfo);
    $("#form1").submit();
}
//显示结果
function showResult(res) {
    if (res.IsPass) {
        $.dialog(res.MSG);
        var start = dt_Table.fnSettings()._iDisplayStart;
        var length = dt_Table.fnSettings()._iDisplayLength;
        dt_Table.fnPageChange(start / length, true);
    } else { $.dialog(res.MSG); }
}

