var dtMember360;
var dtSubMember360;
var urlpara;
$(function () {
    $(".datepicker").datepicker();
    $("#txtRegDateStart").datepicker();
    $("#txtRegDateEnd").datepicker();
    // $(".chzn_a").chosen({
    //    allow_single_deselect: true
    //});
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }
    $('#dtMember360').resize(function () {
        $('#dtMember360').css({ "width": "130%" });
    })

    urlpara = utility.getUrlParameters();
    if (urlpara["sStart"] || urlpara["sLen"]) {
        $("#txtMemNo").val(urlpara["mNo"]);
        $("#txtName").val(decodeURI(urlpara["mName"]));
        $("#txtMobile").val(urlpara["mbl"]);
        $("#drpCustomerLevel").val(urlpara["grd"]);
        $("#drpStores").val(urlpara["storeCode"]);
        $("#drpArea").val(urlpara["storeArea"]);
        $("#drpChannel").val(urlpara["storeChan"]);
        $("#txtConsumeAmountStart").val(urlpara["amountStart"]);
        $("#txtConsumeAmountEnd").val(urlpara["amountEnd"]);
        $("#txtConsumePointStart").val(urlpara["pointStart"]);
        $("#txtConsumePointEnd").val(urlpara["pointEnd"]);
        $(".chzn_a").trigger("liszt:updated");
        searchMem();
    }

    //查询操作
    $("#btnSearch").click(function () {
        searchMem();
    });

    $("#btnExport").click(function () {
        exportMem();
    });

    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    $('#txtAgent').empty();
    

    $.post('/PurchasesNew/LoadCompany', {}, function (result) {
        $('#txtAgent').empty();
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CompanyCode + '">' + data.CompanyName + '/' + data.CompanyCode + '<option>'
            });
            $('#txtAgent').append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#txtAgent').append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });

    //查询条件中的分公司
    $('#txtAgent').change(function () {
        $('#drpStore').html("");
        $.ajax({
            type: 'post',
            url: '/MemberTransform/GetStoreList',
            dataType: 'json',
            data: { company: $('#txtAgent').val() },
            success: function (result) {
                if (result.length > 0) {
                    var opt = "";
                    opt += "<option value=''>请选择</option>";
                    for (var i = 0; i < result.length; i++) {
                        opt += '<option value=' + result[i].StoreCode + '>' + result[i].StoreName + '/' + result[i].StoreCode+ '</option>';
                    }
                    $('#drpStore').append(opt);
                    $(".chzn_a").trigger("liszt:updated");
                }
                else {
                    opt = "<option value=''>请选择</option>";
                    $('#drpStore').append(opt);
                    $(".chzn_a").trigger("liszt:updated");
                }
            },
            error: function (e) {
                e.responseText;
            }
        })
    });
});
//var columnList;
// 搜索会员
function searchMem() {
    qryopt = {
        MemberNo: $("#txtMemNo").val().trim(),
        CustomerName: $("#txtName").val().trim(),
        CustomerMobile: $("#txtMobile").val().trim(),
        //CustomerLevel: $("#drpCustomerLevel").val(),
        Agent: $("#txtAgent").val(),
        Store: $("#drpStore").val(),
        //RegStoreArea: $("#drpArea").val(),
        //RegStoreChan: $("#drpChannel").val(),
        //RegStartDate: $("#txtRegDateStart").val(),
        //RegEndDate: $("#txtRegDateEnd").val(),
        //ConsumeAmountStart: $("#txtConsumeAmountStart").val(),
        //ConsumeAmountEnd: $("#txtConsumeAmountEnd").val(),
        //ConsumePointStart: $("#txtConsumePointStart").val(),
        //ConsumePointEnd: $("#txtConsumePointEnd").val(),
        //CustomerSource: $("#drpCustomerSource").val()
    };

    if (qryopt.MemberNo == "" && qryopt.CustomerName == "" && qryopt.CustomerMobile == "" && qryopt.Agent == "" && qryopt.Store == "") {//&& qryopt.CustomerLevel == "" && qryopt.RegisterStoreCode == "" && qryopt.RegStoreArea == "" && qryopt.RegStoreChan == "" && qryopt.ConsumeAmountStart == "" && qryopt.ConsumeAmountEnd == "" && qryopt.ConsumePointStart == "" && qryopt.ConsumePointEnd == "" && qryopt.CustomerSource == "" && qryopt.RegStartDate == "" && qryopt.RegEndDate == "") {
        $.dialog("查询条件不能为空！");
        return;
    }
    if (checkStr(qryopt.MemberNo)) {
        $.dialog("会员卡号中不能包含特殊字符！");
        return;
    }
    if (checkStr(qryopt.CustomerName)) {
        $.dialog("姓名中不能包含特殊字符！");
        return;
    }
    if (checkStr(qryopt.Agent)) {
        $.dialog("代理商中不能包含特殊字符！");
        return;
    }
    //if (qryopt.RegStartDate != "" && qryopt.RegEndDate != "") {
    //    if (qryopt.RegStartDate > qryopt.RegEndDate) {
    //        $.dialog("结束时间不能小于注册时间！");
    //        return;
    //    }
    //}


    //ajaxSync("/Member360/GetMembersByPageColumn", { memberNo: qryopt.MemberNo, customerName: qryopt.CustomerName, customerMobile: qryopt.CustomerMobile }, function (res) {
    //    if (res) {
    //        columnList = res;
    //    }
    //});
    var con = [];
    if (qryopt.MemberNo != "") { con.push({ FieldAlias: "CardNo", Value: qryopt.MemberNo, TableName: "TM_Mem_Card", Condition: "like" }); }
    if (qryopt.CustomerName != "") { con.push({ FieldAlias: "Str_Attr_3", Value: qryopt.CustomerName, TableName: "TM_Mem_Ext", Condition: "like" }); }
    if (qryopt.CustomerMobile != "") { con.push({ FieldAlias: "Str_Attr_4", Value: qryopt.CustomerMobile, TableName: "TM_Mem_Ext", Condition: "like" }); }
    if (qryopt.Agent != "") { con.push({ FieldAlias: "Str_Attr_52", Value: qryopt.Agent, TableName: "TM_Mem_Ext", Condition: "like" }); }
    if (qryopt.Store != "") { con.push({ FieldAlias: "Str_Attr_5", Value: qryopt.Store, TableName: "TM_Mem_Ext", Condition: "like" }); }

    condition = JSON.stringify(con);
    ajaxSync("/Member360/GetColumnByPage", { tablename: "TM_Mem_Ext", blockcode: "0" }, function (res) {
        if (res) {
            columnList = res;

        }
    });
    if (!dtMember360) {
        dtMember360 = $('#dtMember360').dataTable({
            sAjaxSource: '/Member360/GetTabInfo',
            sScrollX: "100%",
            sScrollXInner: "130%",
            bScrollCollapse: true,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aoColumns:
                columnList,
            fnFixData: function (d) {
                d.push({ name: 'tablename', value: "TM_Mem_Ext" });
                d.push({ name: 'blockcode', value: "0" });
                d.push({ name: 'parm', value: condition });
                //d.push({ name: 'memberNo', value: $("#txtMemNo").val().trim() });
                //d.push({ name: 'customerName', value: $("#txtName").val().trim() });
                //d.push({ name: 'customerMobile', value: $("#txtMobile").val().trim() });
            },
            fnDrawCallback: function () {
                $('#dtMember360 tbody tr').addClass("rowlink").bind('click', function () {
                    var mid = $(this).find('td:first').text();
                    window.open("/member360/MemberDetail?mid=" + mid);
                })
            },
            //    [
            //        { data: "MemberCardNo", title: "会员编号", sWidth: "100", sortable: false },
            //        { data: "CustomerName", title: "姓名", sWidth: "120", sortable: false },
            //        { data: "CustomerMobile", title: "手机", sWidth: "100", sortable: false },
            //        { data: "CustomerLevelText", title: "会员等级", sWidth: "80", sortable: false },
            //        {
            //            data: null, title: "所在城市", sWidth: "100", sortable: false, render: function (r) {
            //                var show = r.City == null ? "" : r.City;
            //                if (r.City != null) {
            //                    show = show.length > 3 ? (show.substring(0, 3) + "...") : show;
            //                }
            //                return "<span title='" + r.City + "'>" + show + "</span>"
            //            }
            //        },
            //        { data: "Channel", title: "入会渠道", sWidth: "100", sortable: false },
            //        { data: "CustomerSource", title: "来源", sWidth: "100", sortable: false },
            //        {
            //            data: null, title: "注册门店", sWidth: "150", sortable: false, render: function (r) {
            //                var show = r.RegisterStoreName == null ? "" : r.RegisterStoreName;
            //                if (r.RegisterStoreName != null) {
            //                    show = show.length > 10 ? (show.substring(0, 10) + "...") : show;
            //                }
            //                return "<span title='" + r.RegisterStoreName + "'>" + show + "</span>"
            //            }
            //        },
            //        { data: "AvailPoint", title: "可用积分", sWidth: "100", sortable: true },
            //        {
            //            data: null, title: "注册时间", sWidth: "140", sortable: false, render: function (r) {
            //                return r.RegisterDate.substring(0, 10);
            //            }
            //        },
            //        {
            //            data: "ConsumeAmount", title: "消费额", sWidth: "80", sortable: true,
            //        },
            //        {
            //            data: null, title: "历史消费额", sWidth: "100", sortable: false, render: function (r) {
            //                return r.HistoryConsumeAmount == null ? 0 : r.HistoryConsumeAmount;
            //            }
            //        },
            //          {
            //              data: null, title: "调整金额", sWidth: "100", sortable: false, render: function (r) {
            //                  return r.HistoryConsumeModify == null ? 0 : r.HistoryConsumeModify;
            //              }
            //          }
            //        { data: "AvailPoint", title: "有效积分", sortable: false }
            //    ],
            //    fnServerData: function (sSource, aoData, fnCallback, aoSearchFilter) {
            //        var d = $.extend({}, fixData(aoData), qryopt);
            //        if (urlpara.length > 1 && !aoSearchFilter._bInitComplete) {
            //            d.iDisplayLength = urlpara["sLen"];
            //            d.iDisplayStart = urlpara["sStart"];
            //            aoSearchFilter._iDisplayStart = parseInt(urlpara["sStart"]);
            //            aoSearchFilter._iDisplayEnd = urlpara["sLen"];
            //        }
            //        ajax(sSource, d, function (data) {
            //            fnCallback(data);
            //            $('#dtMember360 tbody tr').addClass("rowlink").click(function () {
            //                var mid = dtMember360.fnGetData(this).MemberID.trim();
            //                var sStart = dtMember360.fnSettings()._iDisplayStart;
            //                var sLen = dtMember360.fnSettings()._iDisplayLength;
            //                var memberNo = $("#txtMemNo").val().trim();
            //                var memName = $("#txtName").val().trim();
            //                var mobile = $("#txtMobile").val().trim();
            //                var grade = $("#drpCustomerLevel").val();
            //                var storeCode = $("#drpStores").val();
            //                var storeArea = $("#drpArea").val();
            //                var storeChan = $("#drpChannel").val();
            //                var amountStart = $("#txtConsumeAmountStart").val();
            //                var amountEnd = $("#txtConsumeAmountEnd").val();
            //                var pointStart = $("#txtConsumePointStart").val();
            //                var pointEnd = $("#txtConsumePointEnd").val();
            //                var cutomerSource = $("#drpCustomerSource").val();
            //                window.open("/member360/MemberDetail?mid=" + mid + "&sStart=" + sStart + "&sLen=" + sLen + "&mNo=" + memberNo + "&mName=" + memName + "&mbl=" + mobile + "&grd=" + grade + "&storeCode=" + storeCode + "&storeArea=" + storeArea + "&storeChan=" + storeChan + "&amountStart=" + amountStart + "&amountEnd=" + amountEnd + "&pointStart=" + pointStart + "&pointEnd=" + pointEnd);
            //            });
            //        });
            //    }
        });
    }
    else {
        dtMember360.fnDraw();
    }

    //$('#dtMember360 tbody tr').addClass("rowlink").bind('click', function () {
    //    var mid = $(this).find('td:first').text();
    //    window.open("/member360/MemberDetail?mid=" + mid);
    //})
}

function fixData(d) {
    var ndata = {};
    for (var i in d) {
        ndata[d[i].name] = d[i].value;
    }
    return ndata;
}

function checkStr(str) {
    var pattern = new RegExp("[%--`~!#$^&*()=|{}':;',\\[\\].<>/?~！#￥……&*（）——| {}【】‘；：”“'。，、？]");
    return pattern.test(str);
}


function exportMem() {

    qryopt = {
        MemberNo: $("#txtMemNo").val().trim(),
        CustomerName: $("#txtName").val().trim(),
        CustomerMobile: $("#txtMobile").val().trim(),
        //CustomerLevel: $("#drpCustomerLevel").val(),
        Agent: $("#txtAgent").val().trim(),

    };

    if (qryopt.MemberNo == "" && qryopt.CustomerName == "" && qryopt.CustomerMobile == "" && qryopt.Agent == "") {//&& qryopt.CustomerLevel == "" && qryopt.RegisterStoreCode == "" && qryopt.RegStoreArea == "" && qryopt.RegStoreChan == "" && qryopt.ConsumeAmountStart == "" && qryopt.ConsumeAmountEnd == "" && qryopt.ConsumePointStart == "" && qryopt.ConsumePointEnd == "" && qryopt.CustomerSource == "" && qryopt.RegStartDate == "" && qryopt.RegEndDate == "") {
        $.dialog("查询条件不能为空！");
        return;
    }
    if (checkStr(qryopt.MemberNo)) {
        $.dialog("会员卡号中不能包含特殊字符！");
        return;
    }
    if (checkStr(qryopt.CustomerName)) {
        $.dialog("姓名中不能包含特殊字符！");
        return;
    }
    if (checkStr(qryopt.Agent)) {
        $.dialog("代理商中不能包含特殊字符！");
        return;
    }

    var con = [];
    if (qryopt.MemberNo != "") { con.push({ FieldAlias: "CardNo", Value: qryopt.MemberNo, TableName: "TM_Mem_Card", Condition: "like" }); }
    if (qryopt.CustomerName != "") { con.push({ FieldAlias: "Str_Attr_3", Value: qryopt.CustomerName, TableName: "TM_Mem_Ext", Condition: "like" }); }
    if (qryopt.CustomerMobile != "") { con.push({ FieldAlias: "Str_Attr_4", Value: qryopt.CustomerMobile, TableName: "TM_Mem_Ext", Condition: "like" }); }
    if (qryopt.Agent != "") { con.push({ FieldAlias: "Str_Attr_52", Value: qryopt.Agent, TableName: "TM_Mem_Ext", Condition: "like" }); }

    condition = JSON.stringify(con);


    $('#resultExportForm')[0].action = "/Member360/ExportMember360List";
    //$('#resultExportForm #memCard').val(qryopt.MemberNo);
    //$('#resultExportForm #memName').val(qryopt.CustomerName);
    //$('#resultExportForm #memMobile').val(qryopt.CustomerMobile);
    //$('#resultExportForm #memIdNo').val(qryopt.CerNo);
    $('#resultExportForm #condition').val(condition);
    $('#resultExportForm')[0].submit();







    qryopt = {
        MemberNo: $("#txtMemNo").val().trim(),
        CustomerName: $("#txtName").val().trim(),
        CustomerMobile: $("#txtMobile").val().trim(),
        //CustomerLevel: $("#drpCustomerLevel").val(),
        CerNo: $("#txtCerNo").val(),

    };

    if (qryopt.MemberNo == "" && qryopt.CustomerName == "" && qryopt.CustomerMobile == "" && qryopt.CerNo == "") {//&& qryopt.CustomerLevel == "" && qryopt.RegisterStoreCode == "" && qryopt.RegStoreArea == "" && qryopt.RegStoreChan == "" && qryopt.ConsumeAmountStart == "" && qryopt.ConsumeAmountEnd == "" && qryopt.ConsumePointStart == "" && qryopt.ConsumePointEnd == "" && qryopt.CustomerSource == "" && qryopt.RegStartDate == "" && qryopt.RegEndDate == "") {
        $.dialog("查询条件不能为空！");
        return;
    }
    if (checkStr(qryopt.MemberNo)) {
        $.dialog("会员卡号中不能包含特殊字符！");
        return;
    }
    if (checkStr(qryopt.CustomerName)) {
        $.dialog("姓名中不能包含特殊字符！");
        return;
    }
    if (checkStr(qryopt.CerNo)) {
        $.dialog("身份证号中不能包含特殊字符！");
        return;
    }

    var con = [];
    if (qryopt.MemberNo != "") { con.push({ FieldAlias: "CardNo", Value: qryopt.MemberNo, TableName: "TM_Mem_Card", Condition: "like" }); }
    if (qryopt.CustomerName != "") { con.push({ FieldAlias: "Str_Attr_3", Value: qryopt.CustomerName, TableName: "TM_Mem_Ext", Condition: "like" }); }
    if (qryopt.CustomerMobile != "") { con.push({ FieldAlias: "Str_Attr_4", Value: qryopt.CustomerMobile, TableName: "TM_Mem_Ext", Condition: "like" }); }
    if (qryopt.CerNo != "") { con.push({ FieldAlias: "Str_Attr_9", Value: qryopt.CerNo, TableName: "TM_Mem_Ext", Condition: "like" }); }

    condition = JSON.stringify(con);
    ajaxSync("/Member360/GetColumnByPage", { tablename: "TM_Mem_Ext", blockcode: "0" }, function (res) {
        if (res) {
            columnList = res;
        }
    });








}
