$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    dt_CustomerLostRateTable = $('#dt_CustomerLostRateTable').dataTable({
        sAjaxSource: '/Report/CustomerLostRateQuery',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "一月来厂", sortable: false },
            { data: 'Sex', title: "二月来厂", sortable: false },
            { data: 'Sex', title: "三月来厂", sortable: false },
            { data: 'Sex', title: "四月来厂", sortable: false },
            { data: 'Sex', title: "五月来厂", sortable: false },
            { data: 'Sex', title: "六月来厂", sortable: false },
            { data: 'Sex', title: "七月来厂", sortable: false },
            { data: 'Sex', title: "八月来厂", sortable: false },
            { data: 'Sex', title: "九月来厂", sortable: false },
            { data: 'Sex', title: "十月来厂", sortable: false },
            { data: 'Sex', title: "十一月来厂", sortable: false },
            { data: 'Sex', title: "十二月来厂", sortable: false },
        ],
        fnFixData: function (d) {
            d.push({ name: 'txtMobile', value: $.trim($("#txtMobile").val()) });
            d.push({ name: 'txtStore', value: $.trim($("#txtStore").val()) });
            d.push({ name: 'txtYear', value: $.trim($("#txtYear").val()) });
        }
    });

    $("#export").click(function () {
        $("#exportForm")[0].action = "/Report/ExportCustomerLostRate";
        $("#exportForm #exportYear").val($("#txtYear").val());
        $("#exportForm #exportMobile").val($("#txtMobile").val());
        $("#exportForm #exportStore").val($("#txtStore").val());
        $("#exportForm")[0].submit();
    })
    $("#btnSerach").click(function () {
        dt_CustomerLostRateTable.fnDraw();
    })

});