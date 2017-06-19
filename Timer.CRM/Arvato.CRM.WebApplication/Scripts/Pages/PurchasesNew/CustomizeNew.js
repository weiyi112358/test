﻿var dt_Table;
var reg12 = new RegExp("^\\d{12}$");
var reg5 = new RegExp("/\b\d{5}\b/");
var reg = new RegExp("^[1-9]\\d*$");
var dt_DetailTable;
var customizeOddId;

$(document).ready(function () {
  
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }
    loadAgent();
    loadApproveStatus();
    loadCustomizeDataTable();
    loadDetailParams();
});

//时间查询条件
$("#queryCreateTime").datepicker({ dateFormat: "yyyy-MM-dd" });

//查询
$('#search').click(function () {
    var oddId = $("#queryOddIdNo").val().trim();
    if (oddId != "" && (reg.test(oddId) == false||oddId.length>14)) {
        $.dialog("请输入正确的单号");
        return false;
    };
    var boxNumIn = $("#queryBoxNumIn").val().trim();
    if (boxNumIn != "" && (reg.test(boxNumIn) == false||boxNumIn.length>7)) {
        $.dialog("请输入正确的盒数量");
        return false;
    };
    var cardNumIn = $("#queryCardNumIn").val().trim();
    if (cardNumIn != "" && (reg.test(cardNumIn) == false||cardNumIn.length>7)) {
        $.dialog("请输入正确的卡数量");
        return false;
    };
    var agent = $("#queryAgent").find('option:selected').val();
    var status = $("#queryStatus").find('option:selected').val();
    var createTime = $("#queryCreateTime").val();
    var isRetrieve = $('#queryIsRetrieve').val();
    $('#txtExcelOddIdNo').val(oddId);
    $('#txtExcelAgent').val(agent);
    $('#txtExcelStatus').val(status);
    $('#txtExcelBoxNumIn').val(boxNumIn);
    $('#txtExcelCardNumIn').val(cardNumIn);
    $('#txtExcelCreateTime').val(createTime);
    $('#txtExcelIsRetrieve').val(isRetrieve);
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
            $(".chzn_a").trigger("liszt:updated");
        }
    })
};

//加载列表
function loadCustomizeDataTable() {
        dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/PurchasesNew/CustomizeCardList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[10, "desc"]],
        aoColumns: [
        { data: 'OddId', title: "单号", sortable: true, bVisible: false },
        {
            data: 'OddIdNo', title: "单号", sortable: true, sClass: "center"
        },
         {
             data: 'Status', title: "状态码", sortable: true, bVisible: false },
           {
               data: 'StatusName', title: "状态", sortable: false, sClass: "center"
           },          
        { data: 'Agent', title: "供应商", sortable: false, sClass: "center" },
        { data: 'AcceptingUnit', title: "收货单位", sortable: false, sClass: "center" },
        { data: 'BoxNumIn', title: "定卡盒数", sortable: false, sClass: "center" },
        { data: 'CardNumIn', title: "定卡张数", sortable: false, sClass: "center" },
        {
            data: 'IsRetrieve', title: "是否已收卡", sortable: false, sClass: "center", render: function (data) {
                return data == null ? "未收卡" : "已收卡";
            }
        },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },
        {
            data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {
                var html = '<button  onclick="goDetail(\'' + r.OddId + '\',\''+r.Status+'\');" >详情</button>';
                return html;
            }
        },
        ],
        fnFixData: function (d) {
            d.push({ name: 'oddIdNo', value: $("#queryOddIdNo").val().trim() });
            d.push({ name: 'agent', value: $("#queryAgent").find('option:selected').val() });
            d.push({ name: 'status', value: $("#queryStatus").find('option:selected').val() });
            d.push({ name: 'boxNumIn', value: $("#queryBoxNumIn").val().trim() });
            d.push({ name: 'cardNumIn', value: $("#queryCardNumIn").val().trim() });
            d.push({ name: 'createTime', value: $("#queryCreateTime").val() });
            d.push({ name: 'isRetrieve', value: $("#queryIsRetrieve").val() });
        }
    })
};

//详情页
function goDetail(id, status) {
    if (status!="0") {
        $('#btnStatusPass').hide();
        $('#btnStatusReturn').hide();
        $('#btnExcelOut').show();
    }
    else {
        $('#btnExcelOut').hide();
        $('#btnStatusPass').show();
        $('#btnStatusReturn').show();
    }
    openColorBox();
    $('#customizeOddId').empty().val(id);
    customizeOddId = id;
    dt_DetailTable.fnDraw(); 
}


//清理
function goClearIndex() {
    $('#queryCondition input:text').val('');
    loadAgent();
    loadApproveStatus();
    $('#queryIsRetrieve').empty().append('<option value="">==请选择==</option><option value="0">否</option><option value="1">是</option>');
};

//打开弹框
function openColorBox()
{
    $("#addBrand_dialog .heading h3").html("定卡详情");
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

//加载详情list
function loadDetailParams() {
        dt_DetailTable = $('#showParams').dataTable({
        sAjaxSource: '/PurchasesNew/CustomizeDetailsParams',
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
  
         { data: 'BoxNo', title: "盒号", sortable: false, sClass: "center" },
         { data: 'BeginCardNo', title: "起始卡号", sortable: false, sClass: "center" },
         { data: 'EndCardNo', title: "截止卡号", sortable: false, sClass: "center" },
      
        ],
        fnFixData: function (d) {
            d.push({ name: 'customizeOddId', value: customizeOddId });
        }
    });
}

function goStatus(status,info)
{
    $.dialog("确认" + info + "吗？", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        var postdata = {
            customizeOddId: customizeOddId,
            status: status
        };
        $.post('/PurchasesNew/ChangeStatus', postdata, function (result) {
            $.dialog(result.MSG);
            $.colorbox.close();
            dt_Table.fnDraw();
        }, 'json');
    });
}
//清理详情
function goClearDetail()
{
    $('#showParams').empty();
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

