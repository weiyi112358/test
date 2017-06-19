var dt_Table;
var reg = new RegExp("^[1-9]\\d*$");
$(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }

    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/InventorySearch/LoadBoxInfo',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aoColumns: [
            { data: 'BoxNo', title: "盒号", sClass: "center", sortable: false },
            { data: 'BeginCardNo', title: "开始卡号", sClass: "center", sortable: false },
            { data: 'EndCardNo', title: "结束卡号", sClass: "center", sortable: false },
            { data: 'CardNumIn', title: "总卡数", sClass: "center", sortable: false },
            { data: 'StoreName', title: "门店", sClass: "center", sortable: false },
            { data: 'UsedCard', title: "已使用卡数", sClass: "center", sortable: false },
            {
                data: null, title: "未使用卡数", sClass: "center", sortable: false, render: function (obj) {
                    return obj.CardNumIn - obj.UsedCard;
                }
            },
            
        ],
        fnFixData: function (d) {
            d.push({ name: 'BoxNo', value: $("#txtBoxNo").val() });
            d.push({ name: 'storeCode', value: $("#selStore").val() });
        }
    });

    //查询
    $("#btnSerach").bind("click", function () {
        var boxNo = $("#txtBoxNo").val().trim();
        if (boxNo != "" && (reg.test(boxNo) == false || boxNo.length > 14)) {
            $.dialog("请输入正确的盒号");
            return false;
        };
        dt_Table.fnDraw();
    });

   

    loadStore();

});



//加载门店
function loadStore() {
    var $store = $('#selStore');
    $store.empty();
    $.post('/PurchasesNew/LoadStore', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.StoreCode + ',' + data.StoreProvinceCode + '">' + data.StoreName + '/' + data.StoreCode + '<option>'
            });
            $store.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $store.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};