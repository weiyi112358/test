$(function () {
    searchMem();

    $("#btnSerach").click(function () {
        searchMem();
    })


    $("#export").click(function () {
        $("#exportForm")[0].action = "/Report/ExportServiceIncomeCount";
        $("#exportForm #exportMobile").val($("#txtMobile").val());
        $("#exportForm #exportStore").val($("#txtStore").val());
        $("#exportForm #exportStartDate").val($("#txtStartDate").val());
        $("#exportForm #exportEndDate").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    })
})
var dtSearch;
function searchMem() {
    qryopt = {
        txtStartDate: $("#txtStartDate").val(),
        txtEndDate: $("#txtEndDate").val(),
        txtMobile: $("#txtMobile").val(),
        txtStore: $("#txtStore").val(),
    };
    if (!dtSearch) {
        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetServiceIncomeCount',
            bAutoWidth: false,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,//
            aoColumns: [
               {
                   data: "ID", title: "一般维修", "sClass": "center Com_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Name", title: "保养", "sClass": "center Copper_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "自采机油保养台次", "sClass": "center Silver_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Name", title: "事故", "sClass": "center Gold_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Sex", title: "维修+保养", "sClass": "center Gold_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "ID", title: "其他", "sClass": "center Platinum_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "合计", "sClass": "center Total_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "一般维修", "sClass": "center Com_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "保养", "sClass": "center Copper_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "自采机油保养台次", "sClass": "center Silver_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Name", title: "事故", "sClass": "center Gold_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "维修+保养", "sClass": "center Gold_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "其他", "sClass": "center Platinum_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "合计", "sClass": "center Total_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               }, {
                   data: "Age", title: "一般维修", "sClass": "center Com_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "保养", "sClass": "center Copper_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "自采机油保养台次", "sClass": "center Silver_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
                {
                    data: "Name", title: "事故", "sClass": "center Gold_Mem", sortable: false, render: function (r) {
                        return formatNumber(r);
                    }
                },
               {
                   data: "Age", title: "维修+保养", "sClass": "center Gold_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "其他", "sClass": "center Platinum_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: "Age", title: "合计", "sClass": "center Total_Mem", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
            ],
            fnDrawCallback: function (nRow) {
                var datalength = nRow.aoData.length;
                if (datalength == 0) {
                    return;
                }

                //AmountAccount();
            },
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                /* Append the grade to the default row class name */
                //数据加载完成后的最后一行处理

                return nRow;
            },
            fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
                var d = $.extend({}, fixData(aoData), qryopt);
                ajax(sSource, d, function (data) {
                    fnCallback(data);
                });
            }

        })
    } else {
        dtSearch.fnDraw();
    }
}