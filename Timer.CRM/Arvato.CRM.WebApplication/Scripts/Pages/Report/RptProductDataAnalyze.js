//产品数据分析

$(function () {
    $("#btnSearch").bind("click", function () {
        SearchProductDataAnalyze();
    });

    //导出
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Report/ExportProductDataAnalyze";
        $("#exportForm #ProductName").val($("#txt_ProductName").val());
        $("#exportForm #ProductCode").val($("#txt_ProductCode").val());
        $("#exportForm #BrandName").val($("#txt_ProductBrand").val());
        $("#exportForm #LineName1").val($("#txt_ProductLine1Name").val());
        $("#exportForm #LineName2").val($("#txt_ProductLine2Name").val());
        $("#exportForm")[0].submit();
    });
    $("#dt_search").resize(function () {
        $("#dt_search").css({ "width": "175%" });
    });
});

var dtSearch;
function SearchProductDataAnalyze() {
    qryopt = {
    };
    if (!dtSearch) {

        dtSearch = $('#dt_search').dataTable({
            sAjaxSource: '/Report/GetProductDataAnalyze',
            sScrollX: "100%",
            sScrollXInner: "175%",
            bScrollCollapse: true,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 20,//
            aoColumns: [
               { data: "ProductCode", title: "产品代码", sortable: false },
               {
                   data: null, title: "产品名称", sortable: false, render: function (r) {
                       var show = r.ProductName;

                       show = show.length > 11 ? (show.substr(0, 11) + "...") : show;

                       return "<span title='" + r.ProductName + "' style='white-space:nowrap'>" + show + "</span>"
                   }
               },
               { data: "ProductCode_IPOS", title: "pos产品代码", sortable: false },
               {
                   data: "ProductLineCode1", title: "产品线1代码", sortable: false
               },
               {
                   data: null, title: "产品线1名称", sortable: false, render: function (r) {
                       var show = r.ProductLineName1Base;

                       show = show.length > 8 ? (show.substr(0, 8) + "...") : show;

                       return "<span title='" + r.ProductLineName1Base + "' style='white-space:nowrap'>" + show + "</span>"
                   }
               },
               { data: "ProductLineCode2", title: "产品线2代码", sortable: false },
               {
                   data: null, title: "产品线2名称", sortable: false, render: function (r) {
                       var show = r.ProductLineName2Base;

                       show = show.length > 8 ? (show.substr(0, 8) + "...") : show;

                       return "<span title='" + r.ProductLineName2Base + "' style='white-space:nowrap'>" + show + "</span>"
                   }
               },
               //{ data: "ProductLineCode3", title: "产品线3代码", sortable: false },
               //{ data: "ProductLineName3", title: "产品线3名称", sortable: false },
               { data: "CategoryCode", title: "大类代码", sortable: false },
               { data: "CategoryName", title: "大类名称", sortable: false },
               { data: "SubCategoryCode", title: "子类代码", sortable: false },
               { data: "SubCategoryName", title: "子类名称", sortable: false },
               {
                   data: "OrginalSellPrice", title: "原始售价", sortable: false, render: function (r) {
                       return formatNumber(r);
                   }
               },
               {
                   data: null, title: "停止使用", sortable: false, render: function (r) {
                       if (r.StopUsageFlag=='true') {
                           return "是"
                       }
                       else {
                            return "否"
                       }
                   }
               },
               { data: "ProductBrandCode", title: "品牌代码", sortable: false },
               { data: "ProductBrandNameBase", title: "品牌名称", sortable: false },
               { data: "ProductStatusCode", title: "状态代码", sortable: false },
               { data: "ProductStatusNameBase", title: "状态名称", sortable: false },
               //{ data: "ProductSyncFlag", title: "同步标识", sortable: false }

            ],
            fnFixData: function (d) {
                d.push({ name: 'ProductName', value: $("#txt_ProductName").val() });
                d.push({ name: 'ProductCode', value: $("#txt_ProductCode").val() });
                d.push({ name: 'BrandName', value: $("#txt_ProductBrand").val() });
                d.push({ name: 'LineName1', value: $("#txt_ProductLine1Name").val() });
                d.push({ name: 'LineName2', value: $("#txt_ProductLine2Name").val() });
            },
        });
    }
    else {
        dtSearch.fnDraw();
    }

}