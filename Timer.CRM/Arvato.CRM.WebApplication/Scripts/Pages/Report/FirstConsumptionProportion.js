$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    dt_FirstConsumptionProportionTable = $('#dt_FirstConsumptionProportionTable').dataTable({
        sAjaxSource: '/Report/FirstConsumptionProportionQuery',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "首次消费金额", sortable: false },
            { data: 'Name', title: "充值后金额", sortable: false },
            { data: 'Sex', title: "首次消费占比", sortable: false },
        ],
        fnFixData: function (d) {
            d.push({ name: 'txtMobile', value: $.trim($("#txtMobile").val()) });
            d.push({ name: 'txtGroupID', value: $.trim($("#txtGroupID").val()) });
            d.push({ name: 'txtStartDate', value: $.trim($("#txtStartDate").val()) });
            d.push({ name: 'txtEndDate', value: $.trim($("#txtEndDate").val()) });
        }
    });

    $("#btnSerach").click(function () {
        dt_FirstConsumptionProportionTable.fnDraw();
    })

    $("#export").click(function () {
        $("#exportForm")[0].action = "/Report/ExportFirstConsumptionProportion";
        $("#exportForm #exportStartDate").val($("#txtStartDate").val());
        $("#exportForm #exportEndDate").val($("#txtEndDate").val());
        $("#exportForm #exportStore").val($("#txtStore").val());
        $("#exportForm #exportMobile").val($("#txtMobile").val());
        $("#exportForm")[0].submit();
    })

});