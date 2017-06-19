var dtMemberInfo;
$(function () {
    $("#txtStartTime,#txtEndTime").datepicker();
});
loadMemberInfo();
function loadMemberInfo() {
    if (!dtMemberInfo) {
        dtMemberInfo = $("#tbMemberInfo").dataTable({
            sAjaxSource: '/MemberStoreCode/GetStoreCodeMembers',
            bSort: true,   //是否排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aaSorting: [[7, "desc"]],//index从0开始 即按第八行列
            aoColumns: [
                { data: 'RegisterStoreName', title: "注册门店", sortable: false },
                { data: "StoreName", title: "授权门店", sortable: false },
                { data: 'Str_Attr_3', title: "姓名", sortable: false, },
                { data: 'Str_Attr_2', title: "卡号", sortable: false, },
                { data: 'Str_Attr_4', title: "手机号码", sortable: false, },
                { data: 'Str_Attr_7', title: "性别", sortable: false, },
                { data: 'AddedUser', title: "授权人", sortable: false },
                { data: 'AddedDate', title: "授权时间", sortable: false },

                //{ data: 'Str_Attr_100', title: "邮箱", sortable: false, },

            ],
            fnFixData: function (d) {
                d.push({ name: 'memberName', value: $("#txtMemberName").val() });
                d.push({ name: 'stroeName', value: $("#txtStoreName").val() });
                d.push({ name: 'startTime', value: $("#txtStartTime").val() });
                d.push({ name: 'endTime', value: $("#txtEndTime").val() });
            }
        });
    } else {
        dtMemberInfo.fnDraw();
    }
}

$("#btnQuery").on("click", function () {
    dtMemberInfo.fnDraw();
});
