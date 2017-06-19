var dtMemberList;

$(function () {
    $("#txtMobile").bind("keyup", function () {
        $(this).val($(this).val().replace(/[\D]/g, ""));
    });

    $("#dtMemberList").resize(function () {
        $("#dtMemberList").css({ "width": "115%" });
    });

    //查询操作
    $("#btnSearch").click(function () {
        searchMember();
    });

    $("#dtMemberList").delegate("tbody tr", "click", function (e) {
        if (dtMemberList) {
            var memberID = dtMemberList.fnGetData(this).MemberID.trim();
            window.open("/member360/MemberDetailForStore?mid=" + memberID);
        }
    });
});

// 搜索会员
function searchMember() {
    //destory datatable资源之后重新加载新资源
    if (dtMemberList) {
        dtMemberList.fnDestroy();
    }

    dtMemberList = $('#dtMemberList').dataTable({
        sAjaxSource: '/Member360/GetMembersByPage',
        sScrollX: "100%",
        sScrollXInner: "115%",
        bScrollCollapse: true,
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 30,
        aaSorting: [[8, "desc"]],
        aoColumns: [
            { data: "MemberCardNo", title: "会员编号", sWidth: "100", sortable: false },
            { data: "CustomerName", title: "姓名", sWidth: "120", sortable: false },
            { data: "CustomerMobile", title: "手机", sWidth: "100", sortable: false },
            { data: "CustomerLevelText", title: "会员等级", sWidth: "80", sortable: false },
            { data: "City", title: "所在城市", sWidth: "100", sortable: false },
            { data: "Channel", title: "入会渠道", sWidth: "100", sortable: false },
            { data: "CustomerSource", title: "来源", sWidth: "100", sortable: false },
            { data: "RegisterStoreName", title: "注册门店", sWidth: "100", sortable: false },
            { data: "AvailPoint", title: "可用积分", sWidth: "100", sortable: true },
            { data: "RegisterDate", title: "注册时间", sWidth: "140", sortable: false },
            { data: "ConsumeAmount", title: "消费额", sWidth: "80", sortable: true }
        ],
        fnFixData: function (d) {
            d.push({ name: "customerMobile", value: $("#txtMobile").val() });
        }

    });
}
