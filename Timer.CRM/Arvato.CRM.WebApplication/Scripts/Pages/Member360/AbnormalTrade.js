var dt_Table;
var reg = new RegExp("^[1-9]\\d*$");
var arrayTrue = new Array();
$(document).ready(function () {
    LoadDataTable();

});

function LoadDataTable()
{
    dt_Table = $('#abnormalTradeTable').dataTable({
        sAjaxSource: '/Member360/AbnormalTradeList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[7, "desc"]],
        aoColumns: [
        {
            data: null, title: '<input type="checkbox"  id="ckALL" />', sortable: false, sClass: "center", render: function (r) {
                var str = '<input type="checkbox" name="txtCK" allotId="' + r.Id + '" />';
                return str;
            }
        },
        { data: 'Id', title: "单号", sortable: false, bVisible: false },
        { data: 'TradeId', title: "贸易号", sortable: false, sClass: "center" },
        { data: 'MemberID', title: "会员号", sortable: false, bVisible: false },
        { data: 'StoreName', title: "门店名称", sortable: false, sClass: "center" },   
        { data: 'CustomerName', title: "代理商名称", sortable: false, sClass: "center" },
        { data: 'Mobile', title: "手机号", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },       
        ],
        fnFixData: function (d) {
            d.push({ name: 'tradeId', value: $("#txtTradeID").val().trim() });
        }
    });
}

//查询
$('#btnSearch').click(function () {
    var tradeId = $('#txtTradeID').val().trim();
    if (reg.test(tradeId)==false) {
        $.dialog("请输入正确的数字");
        return false;
    }
    dt_Table.fnDraw();
});


//选中
$("#ckALL").click(function () {
    $("[name=txtCK]:checkbox").prop("checked", this.checked);
});
$("input[name=txtCK]:checkbox").click(function () {
    var flag = true;
    $("input[name=txtCK]:checkbox").each(function () {
        if (this.checked == false) {
            flag = false;
        }
    });
    $("#ckALL").prop("checked", flag);
});

//删除
$('#btndelete').click(function () {
    $("input[name=txtCK]:checkbox").each(function () {
        if (this.checked == true) {
            var data = {
                Id: $(this).attr('allotId'),       
            }
            arrayTrue.push(data);
        }
    });
    if (arrayTrue.length == 0) {
        $.dialog("请勾选单号");
        return false;
    }
    var postdata = {
        jsonParam: JSON.stringify(arrayTrue)
    }
    $.post('/Member360/DeleteTrade', postdata, function (result) {
        if (result.IsPass) {
            $.dialog("操作成功");
            arrayTrue = [];
            dt_Table.fnDraw();
        }
        else {
            $.dialog("操作失败");
        }
    }, 'json')
})