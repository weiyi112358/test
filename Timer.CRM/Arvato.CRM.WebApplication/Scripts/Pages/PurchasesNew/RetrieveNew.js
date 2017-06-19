var dt_Table;
var dt_DetailParamsTable;
var reg12 = new RegExp("^\\d{12}$");
var reg5 = new RegExp("/\b\d{5}\b/");
var reg = new RegExp("^[1-9]\\d*$");
var retrieveId;
$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }
    loadAgent();
    loadApproveStatus();
    loadRetrieveDataTable();
    loadDetailParams();
});

//时间查询条件
$("#queryCreateTime").datepicker({ dateFormat: "yyyy-MM-dd" });

//查询
$('#search').click(function () {
    var customizeOddId = $("#queryCustomizeOddId").val().trim();
    if (customizeOddId != "" && (reg.test(customizeOddId) == false||customizeOddId.length>14)) {
        $.dialog("请输入正确的定卡单单号");
        return false;
    }; 
    var retrieveOddId = $("#queryRetrieveOddId").val().trim();
    if (retrieveOddId != "" && (reg.test(retrieveOddId) == false||retrieveOddId.length>14)) {
        $.dialog("请输入正确收卡单单号");
        return false;
    };
    var reserveBox = $('#queryReserveBox').val();
    if (reserveBox != "" && (reg.test(reserveBox) == false||reserveBox.length>7)) {
        $.dialog("请输入正确收盒数量");
        return false;
    };
    var agent = $("#queryAgent").find('option:selected').val();
    var status = $("#queryStatus").find('option:selected').val();
    var createTime = $("#queryCreateTime").val();
    $('#txtExcelCustomizeOddIdNo').val(customizeOddId);
    $('#txtExcelRetrieveOddIdNo').val(retrieveOddId);
    $('#txtExcelAgent').val(agent);
    $('#txtExcelStatus').val(status);
    $('#txtExcelReserveBox').val(reserveBox);
    $('#txtExcelCreateTime').val(createTime);
    dt_Table.fnDraw();
});

//清理
$('#btnRemove').click(function () {
    goClearIndex();
})


//审核通过
$('#btnStatusPass').click(function () {
    var status = "1";
    var info = "通过审核";
    goStatus(status, info);
});

//审核驳回
$('#btnStatusReturn').click(function () {
    var status = "2";
    var info = "驳回审核";
    goStatus(status, info);
});


//加载供应商
function loadAgent() {
    var $agent = $('#queryAgent');
    $agent.empty();
    $.post('/PurchasesNew/LoadAgent', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.AgentCode + '">' + data.AgentName + '<option>'
            });
            $agent.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $agent.append('<option value="">==无==<option>');
        }
    })
};

//加载状态
function loadApproveStatus() {
    var $status = $('#queryStatus');
    $status.empty();
    var postdata = { optionType: "ApproveStatus" };
    $.post('/PurchasesNew/LoadBizOption', postdata, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '<option>'
            });
            $status.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $status.append('<option value="">==无==<option>');
        }
    })
};


//加载定卡单号下拉框
function loadCustomizeOddIdNo()
{
    var $customizeOddIdNo = $('#customizeOddIdNo');
    $customizeOddIdNo.empty();
    $.post('/PurchasesNew/loadCustomizeOddIdNo', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CustomizeOddId + '">' + data.CustomizeOddIdNo + '<option>'
            });
            $customizeOddIdNo.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $customizeOddIdNo.append('<option value="">==无==<option>');
        }
    })
}

//清理
function goClearIndex() {
    $('#queryCondition input:text').val('');
    loadAgent();
    loadApproveStatus();
};

//加载列表
function loadRetrieveDataTable() {
        dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/PurchasesNew/RetrieveCardList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[9, "desc"]],
        aoColumns: [
        { data: 'RetrieveOddId', title: "单号", sortable: true, bVisible: false },
        {
            data: 'RetrieveOddIdNo', title: "收卡单号", sortable: true, sClass: "center"
        },
         {
             data: 'Status', title: "状态码", sortable: true, bVisible: false
         },
           {
               data: 'StatusName', title: "状态", sortable: false, sClass: "center"            
           },
        { data: 'AgentName', title: "供应商", sortable: false, sClass: "center" },
        { data: 'CustomizeOddIdNo', title: "定卡单号", sortable: false, sClass: "center" },
        { data: 'ReserveBoxNumber', title: "收卡盒数", sortable: false, sClass: "center" },
        { data: 'ReserveCardNumber', title: "收卡张数", sortable: false, sClass: "center" },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },
        {
            data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {
                var html = '<button  onclick="goDetail(\'' + r.RetrieveOddId + '\',\'' + r.Status + '\');" >详情</button>';
                return html;
            }
        },
        ],
        fnFixData: function (d) {
            d.push({ name: 'retrieveOddIdNo', value: $("#queryRetrieveOddId").val().trim() });
            d.push({ name: 'agent', value: $("#queryAgent").find('option:selected').val() });
            d.push({ name: 'status', value: $("#queryStatus").find('option:selected').val() });
            d.push({ name: 'customizeOddIdNo', value: $("#queryCustomizeOddId").val().trim() });
            d.push({ name: 'createTime', value: $("#queryCreateTime").val() });
            d.push({ name: 'reserveBoxNumber', value: $("#queryReserveBox").val().trim() });
        }
    })
};

//打开弹框
function openColorBox() {
    $("#addBrand_dialog .heading h3").html("收卡详情");
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addBrand_dialog",
        inline: true
    });
    $.colorbox.resize();
}

function goDetail(id,status)
{
    if (status != "0") {
        $('#btnStatusPass').hide();
        $('#btnStatusReturn').hide();  
    }
    else {
        $('#btnStatusPass').show();
        $('#btnStatusReturn').show();
    }
    openColorBox();
    retrieveId = id;
    dt_DetailParamsTable.fnDraw();

}

//加载详情list
function loadDetailParams() {
    dt_DetailParamsTable= $('#retrieveDetailTable').dataTable({
        sAjaxSource: '/PurchasesNew/RetrieveDetailsParams',
        bAutoWidth: true,
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 5,
        order: [[0, "desc"]],
        aoColumns: [
        {
            data: 'OddIdNo', title: "单号", sortable: true, sClass: "center"
        },
           {
               data: 'StatusName', title: "状态", sortable: false, sClass: "center"
           },
     
         { data: 'BoxNumber', title: "盒号", sortable: false, sClass: "center" },
         { data: 'BeginCardNo', title: "起始卡号", sortable: false, sClass: "center" },
         { data: 'EndCardNo', title: "截止卡号", sortable: false, sClass: "center" },
     
        ],
        fnFixData: function (d) {
            d.push({ name: 'retrieveOddId', value: retrieveId });
        }
    });
}

//审核
function goStatus(status,info)
{
    $.dialog("确认" + info + "吗？", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        var postdata = {
            retrieveOddId: retrieveId,
            status: status
        };
        showLoading("正在"+info+"中")
        $.post('/PurchasesNew/ChangeStatusRetrieve', postdata, function (result) {
            hideLoading();
            $.dialog(result.MSG);
            $.colorbox.close();
            dt_DetailParamsTable.fnDraw();
            dt_Table.fnDraw();
        }, 'json');
    });
}

//遮罩层
function showLoading(desc) {
    $("#txtspan").html(desc);
    $("#txtspan").css("color", "#ffffff");


    $.openPopupLayer({
        name: "processing",
        width: 500,
        target: "processingdiv"
    });
};
//遮罩层
function hideLoading() {
    $.closePopupLayer('processing');
};