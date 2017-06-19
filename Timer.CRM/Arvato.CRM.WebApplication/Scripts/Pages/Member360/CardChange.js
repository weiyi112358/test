$(function () {

    //dtCardChangeLoad();
    //搜索
    $("#btnSearch").click(function () {
        //if ($("#CardNo").val() == "" && $("#dp_single").val() == "" && $("#dp_single2").val() == "") {
        //    processErrs(["请至少输入一个查询条件!"]);
        //}
        //else {
        dtCardChangeLoad();
        //}
    });

    $("#dp_single,#dp_single2").datepicker();
});
var dtCardChange;
function dtCardChangeLoad() {
    if (!dtCardChange) {
        dtCardChange = $('#dt_CardChange').dataTable({
            sAjaxSource: '/Member360/CardChangeSearch',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            //aaSorting: [[4, "desc"]],
            aoColumns: [
                //{ data: 'LogID', title: "规则名称", sortable: false },
                { data: 'tranId', title: "单号", sortable: false },
                { data: 'CardNo', title: "会员卡号", sortable: false },
                {
                    data: null , title: "状态", sortable: false, render: function (obj) {
                       return obj.isConfirm== true ? '已审核' : '未审核'
                    }
                },
                { data: 'OptionText', title: "类型", sortable: true },
                { data: 'CusAndPhone', title: "手机", sortable: true },
                { data: 'UserName', title: "最后修改人", sortable: false },
                {
                    data: 'AddedDate', title: "最后修改时间", sortable: true, render: function (r) {
                        return r.substr(0, 10);
                    }
                },
                //{ data: 'ChangePlace', title: "变更地点", sortable: true },

            ],
            fnFixData: function (d) {
                d.push({ name: 'orderNo', value: $("#txtOrderNo").val() });
                d.push({ name: 'cardNo', value: $("#txtCardNo").val() });
                d.push({ name: 'phone', value: $("#txtPhone").val() });
                d.push({ name: 'status', value: $("#selStatus").val() });
                d.push({ name: 'type', value: $("#selType").val() });
                d.push({ name: 'startTime', value: $("#dp_single").val() });
                d.push({ name: 'endTime', value: $("#dp_single2").val() });
            }
        });
    }
    else {
        dtCardChange.fnDraw();
    }
}