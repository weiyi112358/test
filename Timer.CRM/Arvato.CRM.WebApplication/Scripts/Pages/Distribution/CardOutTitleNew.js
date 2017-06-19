var array = new Array();
var dt_Table;
var dt_DetailTable;
var identity = 0;
var destineNum = 0;
var idArray = new Array();
var oddId;
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }
    loadDataTable();
    loadApproveStatus();
    loadCompany();
    loadDetailParams();
    loadStore();
})

//时间查询条件
$("#queryCreateTime").datepicker({ dateFormat: "yyyy-MM-dd" });

//查询
$('#search').click(function () {
    var oddId = $("#queryOddIdNo").val().trim();
    if (oddId != "" && (reg.test(oddId) == false||oddId.length>14)) {
        $.dialog("请输入正确的单号");
        return false;
    };
    var boxNumIn = $("#queryBoxNum").val().trim();
    if (boxNumIn != "" && (reg.test(boxNumIn) == false||boxNumIn.length>7)) {
        $.dialog("请输入正确的盒数量");
        return false;
    };
    var boxNo = $('#queryBoxNo').val().trim()
    if (boxNo != "" && (reg.test(boxNo) == false||boxNo.length>12)) {
        $.dialog("请输入正确的盒号");
        return false;
    };
    var agent = $("#queryAcceptingUnit").val();
    var status = $("#queryStatus").val();
    var createTime = $("#queryCreateTime").val();
    var store = $("#queryStore").val();
    $('#txtExcelOddIdNo').val(oddId);
    $('#txtExcelBoxNum').val(boxNumIn);
    $('#txtExcelStatus').val(status);
    $('#txtExcelAcceptingUnit').val(agent);
    $('#txtExcelCreateTime').val(createTime);
    $('#txtExcelBoxNo').val(boxNo);
    $('#txtExcelStore').val(store);
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
    var info = "不通过审核";
    goStatus(status, info);
});


//列表
function loadDataTable() {
        dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Distribution/GetCardOutTitleNewList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[7, "desc"]],
        aoColumns: [
        { data: 'OddId', title: "单号", sClass: "center", bVisible: false },
          {
              data: 'OddIdNo', title: "单号", sortable: true, sClass: "center"
          },
           {
               data: 'Status', title: "状态", sClass: "center", bVisible: false
           },
         { data: 'StatusName', title: "状态", sClass: "center", sClass: "center" },
        { data: 'SendingUnit', title: "发送单位", sClass: "center", sortable: false, },
        //旧版
        //{ data: 'AcceptingUnit', title: "接收单位", sortable: false, sClass: "center" },
        { data: 'BoxNumber', title: "总盒数", sortable: false, sClass: "center" },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },
          {
              data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {
                  var html = '<button  onclick="goDetail(\'' + r.OddId + '\',\'' + r.Status + '\');" >详情</button>';
                  return html;
              }
          },
        ],
        fnFixData: function (d) {
            d.push({ name: 'oddId', value: $("#queryOddIdNo").val().trim() });
            d.push({ name: 'boxNum', value: $("#queryBoxNum").val().trim() });
            d.push({ name: 'status', value: $("#queryStatus").find('option:selected').val() });
            d.push({ name: 'acceptingUnit', value: $("#queryAcceptingUnit").find('option:selected').val() });
            d.push({ name: 'createTime', value: $("#queryCreateTime").val() });
            d.push({ name: 'boxNo', value: $("#queryBoxNo").val().trim() }); 
            d.push({ name: 'store', value: $("#queryStore").find('option:selected').val() });
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

//加载分公司
function loadCompany() {
    var $acceptingUnit = $('#queryAcceptingUnit');
    $acceptingUnit.empty();
    $.post('/PurchasesNew/LoadCompany', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CompanyCode + '">' + data.CompanyName + '/' + data.CompanyCode + '<option>'
            });
            $acceptingUnit.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $acceptingUnit.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};

//清理
function goClearIndex() {
    $('#queryCondition input:text').val('');
    loadCompany();
    loadStore();
    loadApproveStatus();
};

//打开弹框
function openColorBox() {
    $("#addBrand_dialog .heading h3").html("总部卡领出详情");
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addBrand_dialog",
        inline: true
    });
    $.colorbox.resize();
};

//详情页
function goDetail(id, status) {
    if (status != "0") {
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
    oddId = id;
    dt_DetailTable.fnDraw();
};


//加载详情list
function loadDetailParams() {
        dt_DetailTable=  $('#cardOutTitleDetailTable').dataTable({
        sAjaxSource: '/Distribution/CardOutTitleDetailsParams',
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
         
         { data: 'CardTypeName', title: "卡类型", sortable: false, sClass: "center" },
        //{ data: 'Agent', title: "供应商", sortable: false, sClass: "center" },
         { data: 'AcceptingUnit', title: "收货门店", sortable: false, sClass: "center" },
         { data: 'BoxNo', title: "盒号", sortable: false, sClass: "center" },
         { data: 'CardNumIn', title: "卡数量", sortable: false, sClass: "center" },
       { data: 'PurposeName', title: "盒用途", sortable: false, sClass: "center" },   
        //{
        //    data: 'IsOut', title: "分公司领出", sortable: false, sClass: "center", render: function (d) {
        //        var name = d=="0"?"未领出":"已领出";
        //        return name;
        //    }
        //},       
        ],
        fnFixData: function (d) {
            d.push({ name: 'oddId', value: oddId });
        }
    });
}

function goStatus(status, info) {
    $.dialog("确认" + info + "吗？", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        var postdata = {
            OddId: oddId,
            status: status
        };
        showLoading("正在" + info + "中")
        $.post('/Distribution/ChangeStatusCardOutTitle', postdata, function (result) {
            hideLoading();
            $.dialog(result.MSG);
            $.colorbox.close();
            dt_DetailTable.fnDraw();
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


//加载门店
function loadStore() {
    var $store = $('#queryStore');
    $store.empty();
    //旧版
    //var postdata = { companyCode: "直营店" };
    var postdata = { companyCode: "" };
    $.post('/Distribution/LoadStore', postdata, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.ShoppeCode + '">' + data.ShoppeName + '/' + data.ShoppeCode + '<option>'
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