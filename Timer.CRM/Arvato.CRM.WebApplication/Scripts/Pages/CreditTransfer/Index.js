var dtTransfer;

$(function () {
    $("#txtStart,#txtEnd").datepicker();

    $("#btnSearch").click(function () {
        GetCreditTransfers();
    });
});

function add() {
    location.href = "/CreditTransfer/Add";
}

function GetCreditTransfers() {
    if (!dtTransfer) {
        dtTransfer = $('#dt_transfers').dataTable({
            sAjaxSource: '/CreditTransfer/GetCreditTransfers',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                {
                    data: "AddedDate", title: "转让日期", sortable: true, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "ChangeValue", title: "转入数量", sortable: false },
                {
                    data: "UsedEndDate", title: "有效期", sortable: true, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                { data: "ToName", title: "转入会员姓名", sortable: false },
                { data: "ToMobile", title: "转入会员手机", sortable: false },
                { data: "FromName", title: "转出会员姓名", sortable: false },
                { data: "FromMobile", title: "转出会员手机", sortable: false },
                //{ data: "CarNo", title: "车牌号", sortable: false },
                //{ data: "VIN", title: "车架号", sortable: false },
                //{
                //    data: null, title: "", sortable: false, render: function (obj) {
                //        return '<button id="btnSelectTo" class="btn" onclick="toMemberSelected(\'' + obj.MemberID + '\',\'' + obj.CustomerName + '\')">选中</button>';
                //    }
                //}
            ],
            fnFixData: function (d) {
                d.push({ name: 'name', value: $("#txtName").val() });
                d.push({ name: 'mobile', value: $("#txtMobile").val() });
                d.push({ name: 'start', value: $("#txtStart").val() });
                d.push({ name: 'end', value: $("#txtEnd").val() });
            }
        });
    }
    else {
        dtTransfer.fnDraw();
    }
}