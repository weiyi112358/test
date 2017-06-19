$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    dt_RenewalInsuranceRateTable = $('#dt_RenewalInsuranceRateTable').dataTable({
        sAjaxSource: '/Report/RenewalInsuranceRateQuery',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'C201601', title: "1月首保/续保台次", sortable: false },
            { data: 'C201602', title: "2月首保/续保台次", sortable: false },
            { data: 'C201602', title: "3月首保/续保台次", sortable: false },
            { data: 'C201602', title: "4月首保/续保台次", sortable: false },
            { data: 'C201602', title: "5月首保/续保台次", sortable: false },
            { data: 'C201602', title: "6月首保/续保台次", sortable: false },
            { data: 'C201602', title: "7月首保/续保台次", sortable: false },
            { data: 'C201602', title: "8月首保/续保台次", sortable: false },
            { data: 'C201602', title: "9月首保/续保台次", sortable: false },
            { data: 'C201602', title: "10月首保/续保台次", sortable: false },
            { data: 'C201602', title: "11月首保/续保台次", sortable: false },
            { data: 'C201602', title: "12月首保/续保台次", sortable: false },
        ],
        fnFixData: function (d) {
            d.push({ name: 'txtMobile', value: $.trim($("#txtMobile").val()) });
            d.push({ name: 'txtStore', value: $.trim($("#txtStore").val()) });
            d.push({ name: 'txtStartDate', value: $.trim($("#txtStartDate").val()) });
            d.push({ name: 'txtEndDate', value: $.trim($("#txtEndDate").val()) });
        }
    });

    $("#export").click(function () {
        $("#exportForm")[0].action = "/Report/ExportRenewalInsuranceRate";
        $("#exportForm #exportStartDate").val($("#txtStartDate").val());
        $("#exportForm #exportEndDate").val($("#txtEndDate").val());
        $("#exportForm #exportMobile").val($("#txtMobile").val());
        $("#exportForm #exportStore").val($("#txtStore").val());
        $("#exportForm")[0].submit();
    })
    $("#btnSerach").click(function () {
        dt_RenewalInsuranceRateTable.fnDraw();
    })

});