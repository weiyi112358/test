$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    dt_DiscountRateTable = $('#dt_DiscountRateTable').dataTable({
        sAjaxSource: '/Report/DiscountRateQuery',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "充值现金金额", sortable: false },
            { data: 'Name', title: "充值积分", sortable: false },
            { data: 'Sex', title: "消费赠送积分", sortable: false },
             { data: 'Birthday', title: "代金券", sortable: false },
              { data: 'age', title: "折扣率", sortable: false },
        ],
        fnFixData: function (d) {
            d.push({ name: 'txtStartDate', value: $.trim($("#txtStartDate").val()) });
            d.push({ name: 'txtEndDate', value: $.trim($("#txtEndDate").val()) });
        }
    });

    $("#export").click(function () {
        $("#exportForm")[0].action = "/Report/ExportDiscountRate";
        $("#exportForm #exportStartDate").val($("#txtStartDate").val());
        $("#exportForm #exportEndDate").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    })
    $("#btnSerach").click(function () {
        dt_DiscountRateTable.fnDraw();
    })

});