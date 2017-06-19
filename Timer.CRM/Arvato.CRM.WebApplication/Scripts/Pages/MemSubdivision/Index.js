var dictTableList1 = ["TM_Mem_Master", "TM_Mem_Ext", "TM_Loy_MemExt"],
    dictTableList2 = ["TM_Mem_Trade", "TM_Mem_SubExt"],
    dictTableList3 = ["TM_Mem_TradeDetail"],
    dataTypeList = { int: ["3"], dec: ["7", "8"], string: ["1", "2"], date: ["5", "6"], bool: ["4"] },
    validator = null,
    isFirstShow = "0",
    filterRightVals,
    leftValueList = new Object(),//规则左值取值
    leftValueHtml = "",//左值Html
    numOperatorHtml = "",//数字关系Html
    strOperatorHtml = "",//字符关系Html
    strRightDateSelValue = "",//规则右值时间取值字典值
    rightDateSelObj = new Array(),//右值时间下拉框取值
    operatorObj = [],//关系符号
    importObj = "",  //人工导入数据对象
    dtFilterResult,
    aliasParameterList,
    aliasParameterOptions,
    curDynamicParaCount = 0,
    curDynamicValue = "",
    dateController = new Array(),
    datetimeController = new Array(),
    paraHtml = "",
    curParentNode = "";


// 页面初始化
$(function () {
    refreshTree("");

    //会员细分类型下拉框
    $("#selSubType").chosen();

    //会员细分规则部分的遮罩
    //$("#divForTab2").resize(function () {
    //    hidemask();
    //    var flag = $("#flag").val();
    //    if (flag == 1) {
    //        showmask();
    //    }
    //});

    //新增会员细分类型的按钮点击事件
    $("#btnAddType").click(function () {
        resetFormByID("subdivisionTypeDialog");
        showEditDialogMessage("subdivisionTypeDialog", "btnSaveSubdivisionType", null, insertSubdivisionType);
    });

    //保存绑定事件
    $("#btnSaveAll").bind("click", function () {
        var oMemberSubd;
        if (checkForm()) {
            oMemberSubd = getBindMemberSubdFromForm();
            if ($("#selDataSubType").val() == "2") { //判断是否人工导入,如果是人工导入的话,则细分条件为空
                oMemberSubd.Condition = "";
                oMemberSubd.Enable = 1;
                editMemberSubdivision(oMemberSubd);
            } else {
                var filterJson = getFilterJson();
                if (filterJson != "notsave") {
                    oMemberSubd.Condition = filterJson;
                    editMemberSubdivision(oMemberSubd);
                }
            }
        }
        //initImportUpload();
        //return false;
    });

    $("#btnRefreshCurrentInfo").bind("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        if (!utility.isNull($("#hidSID").val())) {
            getMemberSubdToForm($("#hidSID").val());
        }
    });

    //绑定激活事件
    $("#btnActive").bind("click", function () {
        enableSubd(true);
    });

    //绑定取消激活事件
    $("#btnInactive").bind("click", function () {
        enableSubd(false);
    });

    //查找绑定事件
    $("#btnSearch").bind("click", function () {
        refreshTree();
    });

    // 绑定新建事件
    $("#btnAdd").bind("click", function () {
        initPageControlsDisabled();
        refreshTree();
        resetForm();
        initPageVisiable();
        $("#flag").val(0);
    });

    //展示动态参数配置
    $("#hyRightValue").live("mouseover", function () {
        $("#paradetail").hide();
        $("#paradetail ul li").remove();
        var top = $(this)[0].offsetTop + 10;
        var left = $(this)[0].offsetLeft + 40;
        var text = $(this).text();
        var isdynamic = $(this).parent().attr("isdynamicalias") == undefined ? "false" : $(this).parent().attr("isdynamicalias")
        if (isdynamic == "true" && text != "请选择右值") {
            var l = $(this).parent().attr("l") == undefined ? "" : $(this).parent().attr("l");
            var f = getAlias(l);
            var aps = new Array();
            for (i = 0; i < f.ParameterCount; i++) {
                var ap = getAliasParameterByParaIndex(l, i + 1);
                aps.push(ap);
            }
            var html = new Array();
            var r = $(this).parent().attr("r") == undefined ? "" : $(this).parent().attr("r");
            var rarray = r.split("|");
            for (var j = 0; j < aps.length; j++) {
                var item = {
                    html: "",
                    UIIndex: aps[j].UIIndex
                }
                if (rarray[j + 1] == "") {
                    item.html = "<li> <span class='item-key'>" + aps[j].ParameterName + ":</span><div class ='vcard-item'></div></li>"
                } else {
                    if (aps[j].ControlType == "select") {

                        for (var k = 0; k < aliasParameterOptions.length; k++) {
                            if (aliasParameterOptions[k].hasOwnProperty((aps[j].DictTableName + aps[j].DictTableType).replace(",", ""))) {
                                for (var m = 0; m < aliasParameterOptions[k][(aps[j].DictTableName + aps[j].DictTableType).replace(",", "")].length; m++) {

                                    if (rarray[j + 1] == aliasParameterOptions[k][(aps[j].DictTableName + aps[j].DictTableType).replace(",", "")][m].sv) {
                                        item.html = "<li> <span class='item-key'>" + aps[j].ParameterName + ":</span><div class ='vcard-item'>" + aliasParameterOptions[k][(aps[j].DictTableName + aps[j].DictTableType).replace(",", "")][m].st + "</div></li>"
                                        break;
                                    }
                                }
                                break;
                            }

                        }
                    }
                    else if (aps[j].ControlType == "mutisearch") {
                        var paravalue = "";
                        for (var k = 0; k < aliasParameterOptions.length; k++) {
                            if (aliasParameterOptions[k].hasOwnProperty((aps[j].DictTableName + aps[j].DictTableType).replace(",", ""))) {
                                for (var m = 0; m < aliasParameterOptions[k][(aps[j].DictTableName + aps[j].DictTableType).replace(",", "")].length; m++) {
                                    var mutiarray = rarray[j + 1].split(",");

                                    for (var n = 0; n < mutiarray.length; n++) {
                                        if (mutiarray[n] == aliasParameterOptions[k][(aps[j].DictTableName + aps[j].DictTableType).replace(",", "")][m].sv) {
                                            paravalue += aliasParameterOptions[k][(aps[j].DictTableName + aps[j].DictTableType).replace(",", "")][m].st + ","

                                        }

                                    }
                                }
                                break;
                            }
                        }
                        item.html = "<li> <span class='item-key'>" + aps[j].ParameterName + ":</span><div class ='vcard-item'>" + paravalue.substring(0, paravalue.length - 1) + "</div></li>"
                    }
                    else {
                        item.html = "<li> <span class='item-key'>" + aps[j].ParameterName + ":</span><div class ='vcard-item'>" + rarray[j + 1] + "</div></li>"
                    }


                }
                html.push(item);
            }
            //排序
            for (var i = 0; i < html.length - 1; i++) {
                for (var j = i + 1; j < html.length; j++) {
                    if (html[i].UIIndex > html[j].UIIndex) {
                        var temp = html[i];
                        html[i] = html[j];
                        html[j] = temp;
                    }
                }
            }
            var htmlshow = "";
            for (var i = 0; i < html.length; i++) {
                htmlshow += html[i].html;
            }
            $("#paradetail ul").append(htmlshow);
            $("#paradetail").css("top", top + "px")
            $("#paradetail").css("left", left + "px");
            $("#btnclose").css("top", (top - 23) + "px")
            $("#btnclose").css("left", (left + 284) + "px");
            $("#btnclose").show();
            $("#paradetail").show();
            $("#paradetail").focus();
        }
    });

    //隐藏动态参数配置
    $("#btnclose").live("click", function () {
        $("#paradetail").hide();
        $("#btnclose").hide();
    });

    //删除绑定事件
    $("#btnDelete").bind("click", function () {
        if (!$("#hidSID").val()) {
            $.dialog("请先选择要删除的会员细分");
            return false;
        }
        $.dialog("是否要删除该会员细分?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            deleteMemberSubd();
        });
    });

    $("#txtDate, #txtActStartDate, #txtActEndDate").datepicker();
    $("#txtTime, #txtCycleTime").timepicker();
    bindGroup();
    bindSubdivisionType();

    // 获得细分规则左值
    getMemberSubdLeftValuesAll();

    //获取操作符数据
    getOperatorData("StrOperator");
    getOperatorData("NumOperator");

    //日期右值下拉框取值
    getDateSelValue("SysDateValue"); 

    //细分数据类型取值
    getSubDevDataType("SubDevDataType");

    //获取门店数据
    getStoreAll();

    //加载模板按钮点击事件
    $("#btnLoadTemplet").bind("click", function () {
        getFilterAll();
    });

    //隐藏动态细分详情
    $("#aTab1").click(function () {
        $("#btnclose").click();
    })
    $("#tab_rule").click(function () {
        $("#btnclose").click();
    })
    $("#tab_result").click(function () {
        $("#btnclose").click();
    })
    $("#tab_any").click(function () {
        $("#btnclose").click();
    })

    //增加细分规则第一级
    $("#hyAddSubRoot").bind("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        addSubRootReletion();
    });

    //增加细分规则第二级
    $("#hyAddFilter").live("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        addFilter(this);
    });

    $("#hyLeftValue").live("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        showLeftValueMenu(this);
    });

    $("#hyDelFilter").live("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        deleteFilter(this);
    });

    //删除细分规则
    $("#hyDelSubReletion").live("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        deleteSubReletion(this);
    });

    // 细分规则链接
    $("#hyRoot").bind("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        showRootReletion(this);
    });

    //并且
    $("#hySubRoot").live("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        showRootReletion(this);
    });

    $("#hyOperation").live("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        selectOperator(this);
    });

    $("#hyRightValue").live("click", function () {
        if ($("#flag").val() == 1) {
            return;
        }
        selectRightValue(this);
    });

    //会员细分结果查询
    $("#btnSearchResult").bind("click", function () {
        var sid = $("#hidSID").val();
        if (!sid) {
            $.dialog("请选择会员细分！");
            return false;
        }
        //var buileTime = $("#drpBuildTime option:selected").val();
        //if (utility.isNull(buileTime)) {
        //    $.dialog("请选择执行时间！");
        //    return false;
        //}
        loadFilterResult();
    });

    //会员细分结果导出弹窗
    $("#btnResultExport").bind("click", function ()
    {
        $("#addSubExportCol").val('');
        loadSubExportSelect();
        loadSubExportTable();
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#addSubExport_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
            onLoad: function ()
            {
                dtSubExport.fnDraw();
            }
        });
    });

    //会员细分结果导出添加列
    $("#btnAddColumn").bind("click", function ()
    {
        if ($("#addSubExportCol").val() == '')
        {
            $.dialog("请选择要导出的列名");
            return;
        }
        var subdivisionID = $("#hidSID").val();
        var fieldAliasID = $("#addSubExportCol").val();
        //添加
        ajaxSync("/MemSubdivision/AddSubExport", { FieldAliasID: fieldAliasID, SubdivisionID: subdivisionID }, function (res)
        {
            if (res.IsPass)
            {
                $.dialog(res.MSG);
                dtSubExport.fnDraw();
            } else
            { $.dialog(res.MSG); }
        });
    });

    //绑定会员细分结果导出
    $("#btnSubExportSave").bind("click", function ()
    {
        var sid = $("#hidSID").val();
        var currsid = $("#hidCurSubdInstID").val();
        if (!sid)
        {
            $.dialog("请选择会员细分！");
            return false;
        }
        //currsid = utility.isNull(currsid) ? '00000000-0000-0000-0000-000000000000' : currsid;
        sid = utility.isNull(sid) ? '00000000-0000-0000-0000-000000000000' : sid;
        $('#resultExportForm')[0].action = "/MemSubdivision/ExportMemberSubdResult";
        $('#resultExportForm #currSubdId').val(sid);
        $('#resultExportForm #memCard').val($("#txtMemCard").val());
        $('#resultExportForm #memName').val($("#txtMemName").val());
        $('#resultExportForm #memMobile').val($("#txtMemMobile").val());
        $('#resultExportForm #registerStoreCode').val($("#drpRegStore option:selected").val());
        $('#resultExportForm #dynamicTable').val($("#drpBuildTime option:selected").val());
        $('#resultExportForm #subDevDataType').val($("#selDataSubType option:selected").val());
        $('#resultExportForm')[0].submit();
    });

    //绑定会员细分结果导出
    //$("#btnResultExport").bind("click", function () {
    //    var sid = $("#hidSID").val();
    //    var currsid = $("#hidCurSubdInstID").val();
    //    if (!sid) {
    //        $.dialog("请选择会员细分！");
    //        return false;
    //    }
    //    currsid = utility.isNull(currsid) ? '00000000-0000-0000-0000-000000000000' : currsid;
    //    //sid = utility.isNull(sid) ? '00000000-0000-0000-0000-000000000000' : sid;
    //    $('#resultExportForm')[0].action = "/MemSubdivision/ExportMemberSubdResult";
    //    $('#resultExportForm #currSubdId').val(currsid);
    //    $('#resultExportForm #memCard').val($("#txtMemCard").val());
    //    $('#resultExportForm #memName').val($("#txtMemName").val());
    //    $('#resultExportForm #memMobile').val($("#txtMemMobile").val());
    //    $('#resultExportForm #registerStoreCode').val($("#drpRegStore option:selected").val());
    //    $('#resultExportForm #dynamicTable').val($("#drpBuildTime option:selected").val());
    //    $('#resultExportForm #subDevDataType').val($("#selDataSubType option:selected").val());
    //    $('#resultExportForm')[0].submit();
    //});

    //细分数据类型
    $("#selDataSubType").bind("change", function () {
        if ($("#selDataSubType option:selected").val() == "2") {//手动导入 隐藏规则配置页
            $("#tab2 .form_validation_reg").hide();
            $("#btnResultImport").show();
            $("#btnDownTemplate").show();
        } else {
            $("#tab2 .form_validation_reg").show();
            $("#btnResultImport").hide();
            $("#btnDownTemplate").hide();
        }
    });

    //Excel导入
    initImportUpload();
    $("#btnResultImport").click(function () {
        if (checkForm()) {
            //initImportUpload();
            $("#tbFilePath").val("");
            $.colorbox({
                initialHeight: '0',
                initialWidth: '0',
                href: "#import_data",
                overlayClose: false,
                inline: true,
                opacity: '0.3',
                onComplete: function () {
                    $("#btnSaveImport").bind("click", function () {
                        ctrlUpload.startUpload();
                    });
                }
            });
        } else {
            return false;
        }

    });

    //模板下载
    $("#btnDownTemplate").click(function () {
        window.location = '/Upload/会员细分导入模板.xls';
    });

    //周期选择
    $("#drpCycle").bind("click", function changeCycle() {
        var cycle = $("#drpCycle").val();
        switch (cycle) {
            case "daily":
                $("#cycleDays").hide();
                $("#cycleWeek").hide();
                $("#cycleMonth").hide();
                break;
            case "weekly":
                $("#cycleWeek").show();
                $("#cycleDays").hide();
                $("#cycleMonth").hide();
                break;
            case "monthly":
                $("#cycleMonth").show();
                $("#cycleDays").show();
                $("#cycleWeek").hide();
                break;
            case "year":
                $("#cycleDays").show();
                $("#cycleMonth").hide();
                $("#cycleWeek").hide();
                break;
            default:
                break;
        }
    });

    $("input[name='cycle_days']").bind("click", function changeDay() {
        var chk = $("#rdSeleDay").prop("checked");
        if (chk == true) {
            $("#drpFixday").removeAttr('disabled');
        } else {
            $("#drpFixday").attr('disabled', "disabled");
        }
    });
    $("#liChart").hide();
    $("#litab3").hide();

    //$("#selGroup").change(function () {
    //    bindStore($("#selGroup option:selected").val());
    //});
});

//遮罩
function showmask() {
    $("#mask").height($("#mask").parent().height() + 24);
    $("#mask").width($("#mask").parent().width());
    $("#mask").css({ "top": $("#mask").parent()[0].offsetTop - 12 });
    $("#mask").show();
}

function hidemask() {
    $("#mask").hide();
}

function initImportUpload() {
    //ctrlUpload.initUpload("/MemSubdivision/ImportMemSubExcel", $("#hidSID").val(), uploadBack);
    ctrlUpload.initUpload("/MemSubdivision/ImportMemSubExcel", "", uploadBack);
}
//EXCEL上传回调
function uploadBack(data) {
    var ret = data.response;
    importObj = ret;
    $("#tbFilePath").val("");
    $.colorbox.close();

    var oMemberSubd = "";
    oMemberSubd = getBindMemberSubdFromForm();
    editMemberSubdivision(oMemberSubd);
}

// 清除提示
function clearHint() {
    $(".form_validation_reg .help-block").text("");
}

// 表单验证
function checkForm() {
    clearHint();
    if ($("#txtDesc").val().length > 100) {
        processErrs([{ 'txtDesc': '输入超长，最多100字符' }]);
        return false;
    }
    if ($("#txtName").val() == "") {
        processErrs([{ 'txtName': '名称必填' }]);
        return false;
    }
    if ($("#radScheduleNow").prop("checked") == false && $("#radScheduleFixed").prop("checked") == false
        && $("#radScheduleCycle").prop("checked") == false) {
        processErrs([{ 'chooseExeTime': '请选择执行时间' }]);
        return false;
    }
    if ($("#radScheduleFixed").is(":checked") &&
        (utility.trim($("#txtDate").val()) == "" || utility.trim($("#txtTime").val()) == "")) {
        processErrs([{ 'spnFixdayErr': '请选择时间' }]);
        return false;
    }
    if ($("#radScheduleCycle").is(":checked") && utility.trim($("#txtCycleTime").val()) == "") {
        processErrs([{ 'txtCycleTime': '请选择时间' }]);
        return false;
    }
    return true;
}

//获取操作符数据
function getOperatorData(type) {
    var postUrl = "/MemSubdivision/GetCommonOptionData";
    ajax(postUrl, { type: type }, function (res) {
        if (utility.isNull(res)) {
            return;
        } else {
            var objHtml = "<ul class='operator-menu'>";
            for (var i = 0; i < res.length; i++) {
                var isExist = false;
                if (operatorObj != null && operatorObj.length > 0) {
                    operatorObj.forEach(function (item, index) {
                        if (item.key == res[i].OptionValue)
                            isExist = true;
                    });
                }
                if (!isExist) {
                    operatorObj.push({
                        key: res[i].OptionValue,
                        value: res[i].OptionText
                    });
                }

                var tid = "radio" + i;
                objHtml += "<li itemid='" + res[i].OptionValue + "'>"
                    + "<input id='" + tid + "' type='radio' value='" + res[i].OptionValue + "' title='" + res[i].OptionText + "' name='operateGroup' style='float:left;margin-right:5px' />"
                    + "<label for='" + tid + "'>" + res[i].OptionText + "</label>"
                    + "</li>";
            }
            objHtml += "</ul>"
        }
        if (type == "StrOperator") {
            strOperatorHtml = objHtml;
        } else {
            numOperatorHtml = objHtml;
        }
    });
}

//获取日期类型下拉框字典值
function getDateSelValue(type) {
    var postUrl = "/MemSubdivision/GetCommonOptionData";
    ajax(postUrl, { type: type }, function (res) {
        if (utility.isNull(res)) {
            return;
        } else {
            rightDateSelObj = res;
            var objHtml = "<select id='dateRight-menu1' class='dateRight-menu1 span4'>";
            for (var i = 0; i < res.length; i++) {
                if (i == 0) {
                    objHtml += "<option value='" + res[i].OptionValue + "' selected='selected'>" + res[i].OptionText + "</option>";
                } else {
                    objHtml += "<option value='" + res[i].OptionValue + "'>" + res[i].OptionText + "</option>";
                }
            }
            objHtml += "</select>"
        }
        strRightDateSelValue = objHtml;
    });
}

//获取数据类型
function getSubDevDataType(type) {
    var postUrl = "/MemSubdivision/GetCommonOptionData";
    ajax(postUrl, { type: type }, function (res) {
        if (utility.isNull(res)) {
            return;
        } else {
            var objHtml = "";//<select class='selDataSubType'>
            for (var i = 0; i < res.length; i++) {
                if (i == 0) {
                    objHtml += "<option value='" + res[i].OptionValue + "' selected='selected'>" + res[i].OptionText + "</option>";
                } else {
                    objHtml += "<option value='" + res[i].OptionValue + "'>" + res[i].OptionText + "</option>";
                }
            }
            //objHtml += "</select>"
        }
        $("#selDataSubType").html(objHtml);
    });
}

//绑定群组
function bindGroup() {
    var postUrl = "/MemSubdivision/GetDataGroups";
    ajaxSync(postUrl, null, function (res) {
        if (utility.isNull(res)) {
            return;
        } else {
            var objHtml = "";
            for (var i = 0; i < res.length; i++) {
                if (i == 0) {
                    objHtml += "<option value='" + res[i].SubDataGroupID + "' selected='selected'>" + res[i].SubDataGroupName + "</option>";
                } else {
                    objHtml += "<option value='" + res[i].SubDataGroupID + "'>" + res[i].SubDataGroupName + "</option>";
                }
            }

            $("#selGroup").html(objHtml);
            if (res.length == 0) {
                $('#selGroup,#labstore').hide();
            }
            //bindStore(res[0].SubDataGroupID);
        }
    });
}

//绑定群组
//function bindStore(groupId) {
//    var postUrl = "/MemSubdivision/GetDataGroupStore";
//    ajaxSync(postUrl, { groupId: groupId }, function (res) {
//        $("#selStore").html("");
//        if (utility.isNull(res)) {
//            return;
//        } else {
//            var objHtml = "";
//            if (res.length > 1) {
//                objHtml = "<option value=''>请选择</option>";
//            }
//            for (var i = 0; i < res.length; i++) {
//                objHtml += "<option value='" + res[i].StoreCode + "'>" + res[i].StoreName + "</option>";
//            }
//            $("#selStore").html(objHtml);
//        }
//    });
//}

//绑定细分类型
function bindSubdivisionType() {
    ajax("/MemSubdivision/GetSubdivisionType", null, function (res) {
        var objHtml = "";
        if (!utility.isNull(res)) {
            for (var i = 0; i < res.length; i++) {
                if (i == 0) {
                    objHtml += "<option value='" + res[i].OptionValue + "' selected='selected'>" + res[i].OptionText + "</option>";
                } else {
                    objHtml += "<option value='" + res[i].OptionValue + "'>" + res[i].OptionText + "</option>";
                }
            }
        }
        $("#selSubType").html(objHtml);
        $("#selSubType").trigger("liszt:updated");
        var objHtml = "";
        ajax("/BaseData/GetSysClass", { "classID": null, "className": "", "classType": 1 }, function (data) {
            if (!utility.isNull(data)) {
                for (var index = 0; index < data.length; index++) {
                    objHtml += "<option value='" + data[index].ClassID + "'>" + data[index].ClassName + "</option>";
                }
                $("#selSubType").append(objHtml);
                $("#selSubType").trigger("liszt:updated");
            }
        });
    });
}

// ajax获取一个MemberSubdivision
function getMemberSubdToForm(memberSubdId) {
    if (memberSubdId.length < 32) {
        return;
    }
    var postData = 'msid=' + memberSubdId,
        postUrl = "/MemSubdivision/GetMemberSubdivisionById";
    ajaxSync(postUrl, postData, getMemberSubdToFormCallback);
}

function getMemberSubdToFormCallback(data) {
    if (utility.isNull(data)) {
        return;
    } else if (!utility.isNull(data.IsPass) && !data.IsPass) {
        $.dialog(data.MSG);
        return;
    }
    bindMemberSubdToForm(data);
}

//设置下拉框值i
function setSelect(selid, s) {
    sl = document.getElementById(selid);
    if (s == null) {
        s = "";
    }
    for (i = 0; i < sl.length; i++) {
        if (sl[i].value == $.trim(s)) {
            sl[i].selected = true;
        } else {
            sl[i].selected = false;
        }
    }
}

function setSelectText(selid, s) {
    sl = document.getElementById(selid);
    if (s == null) {
        s = "";
    }
    for (i = 0; i < sl.length; i++) {
        var seltxt = sl[i].text;
        if (seltxt == s.trim()) {
            sl[i].selected = true;
        } else {
            sl[i].selected = false;
        }
    }
}

//-----------------------------设置单个单个市场活动的数据
function bindMemberSubdToForm(oMemberSubd) {
    $("#btnSave, #btnSaveFilter").text("保存");
    var computerMethodPara = new Object();

    memberSubdId = oMemberSubd.SubdivisionID;
    $("#hidSID").val(oMemberSubd.SubdivisionID);
    $("#hidCurSubdInstID").val(oMemberSubd.CurSubdivisionInstanceID);
    $("#txtName").val(oMemberSubd.SubdivisionName),
    $("#txtActive").val(oMemberSubd.Enable == true ? "已激活" : "未激活");
    $("#txtComputeTime").val(oMemberSubd.ComputerTime);
    $("#txtLastCompute").val(oMemberSubd.LastComputerDate);
    $("#txtDesc").val(oMemberSubd.SubdivisionDesc);
    $("#tbRunStatus").val(oMemberSubd.RunStatus);
    ///$("#tbResultDesc").val(oMemberSubd.ResultDesc);
    //$("#txtHidBrand").val(oMemberSubd.DataGroupID);
    setSelect("selGroup", oMemberSubd.DataGroupID);
    //setSelect("selStore", oMemberSubd.StoreCode.trim());
    $("#selSubType").val(oMemberSubd.SubdivisionType);
    $("#selSubType").trigger("liszt:updated");
    setSelect("selDataSubType", oMemberSubd.SubDevDataType);

    if (oMemberSubd.SubDevDataType == "1") {
        //$("#span_tipMsg").html("");
        $("#tab2 .form_validation_reg").show();
    } else {
        //$("#span_tipMsg").html("手工导入的到会员细分结果页导入即可，无需设置规则！");
        $("#tab2 .form_validation_reg").hide();
    }

    var lastComputeDateText = "";
    if (!utility.isNull(oMemberSubd.LastComputerDate)) {
        lastComputeDateText = oMemberSubd.LastComputerDate.replace("T", " ");
    }
    $("#txtLastCompute").val(lastComputeDateText);
    $("#txtComputeTime").val(secondToHour(oMemberSubd.ComputerTime));

    var schedule = oMemberSubd.Schedule;

    initPageControlsDisabled();
    hidemask();
    $("#flag").val(0);

    if (!utility.isNull(schedule)) {
        schedule = JSON.parse(oMemberSubd.Schedule);
        switch (schedule.type) {
            case "immediately":
                $("input[name='radSchedule']:eq(0)").attr("checked", "checked");

                if (oMemberSubd.Enable) {
                    $("#flag").val(1);
                    disableForm("tab1");
                    disableForm("tab2");
                    $("#btnRefreshCurrentInfo").removeAttr("disabled");
                } else {
                    $("#btnSave,#btnSaveFilter").removeAttr("disabled");
                    $("#btnActive").removeAttr("disabled");
                    $("#btnInactive").attr("disabled", "disabled");
                    $("#btnRefreshCurrentInfo").attr("disabled", "disabled");
                }
                break;
            case "appointed":
                $("input[name='radSchedule']:eq(1)").attr("checked", "checked");
                $("#txtDate").val(schedule.ap.substr(0, 10));
                $("#txtTime").val(schedule.ap.substr(11));

                if (oMemberSubd.Enable) {
                    $("#flag").val(1);
                    disableForm("tab1");
                    disableForm("tab2");
                    $("#btnInactive").removeAttr("disabled");
                    $("#btnRefreshCurrentInfo").removeAttr("disabled");
                } else {
                    $("#btnSave,#btnSaveFilter").removeAttr("disabled");
                    $("#btnActive").removeAttr("disabled");
                    $("#btnInactive").attr("disabled", "disabled");
                    $("#btnRefreshCurrentInfo").attr("disabled", "disabled");
                }

                break;
            case "cycle":
                $("input[name='radSchedule']:eq(2)").attr("checked", "checked");
                $("#drpCycle").val(schedule.cycle);
                $("#drpCycle").click();
                $("#drpWeek").attr('value', schedule.d);
                var day = (schedule.d == "1st" || schedule.d == "last") ? schedule.d : 'fixed';
                $("input[name='cycle_days'][value='" + day + "']").attr("checked", "checked");
                if (day == "fixed") {
                    $("#drpFixday").removeAttr("disabled");
                    $("#drpFixday").attr('value', schedule.d);
                }
                $("#txtCycleTime").val(schedule.ap);

                if (oMemberSubd.Enable) {
                    $("#flag").val(1);
                    disableForm("tab1");
                    disableForm("tab2");
                    $("#btnInactive").removeAttr("disabled");
                    $("#btnRefreshCurrentInfo").removeAttr("disabled");
                } else {
                    $("#btnSave,#btnSaveFilter").removeAttr("disabled");
                    $("#btnActive").removeAttr("disabled");
                    $("#btnInactive").attr("disabled", "disabled");
                    $("#btnRefreshCurrentInfo").attr("disabled", "disabled");
                }
                break;
        }
    }

    $("#hdFilter").val(oMemberSubd.Condition);

    if (!utility.isNull(oMemberSubd.Condition)) {
        getSeleSearchItemsText(oMemberSubd.Condition);
    } else {
        $("#ulSubRootReletion").html("");
    }

    if (memberSubdId == "" || memberSubdId == "0") {
        isFirstShow = "0";
    } else {
        isFirstShow = "1";
    }

    getSubdDynamicTable();

    if ($("#radScheduleCycle").is(":checked")) {
        getSubdStatiscical();
        $("#chart_tile").html("细分群-" + oMemberSubd.SubdivisionName + "-趋势分析");
        $("#liChart").show();
    } else {
        $("#liChart").hide();
    }
    // 加载细分结果及活动
    loadFilterResult();
    // 加载细分活动
    //loadActivities();
    /*
    searchActivityList();
    */
}

// 获得门店列表
function getStoreAll() {
    var postUrl = "/MemSubdivision/GetStores";
    ajax(postUrl, null, getStoreAllCallback);
}

function getStoreAllCallback(data) {
    if (utility.isNull(data)) {
        return;
    }
    storeList = data;
    var opts = "<option value=''>请选择</option>";
    storeList.forEach(function (item, index, array) {
        opts += "<option value='" + item.StoreCode + "'>" + item.StoreName + "</option>";
    });
    $("#drpRegStore").append(opts);
}

//根据会员细分获取动态表数据
function getSubdDynamicTable() {
    var sid = $("#hidSID").val();
    if (!utility.isNull(sid)) {
        ajaxSync("/MemSubdivision/GetSubdDynamicTable", { subdId: sid }, function (res) {
            var data = "";
            $("#drpBuildTime").html("");
            var opts = "<option value=''>请选择</option>";
            if (res.IsPass) {
                data = res.Obj[0];
                if (data != null && data.length > 0) {
                    data.forEach(function (item, index, array) {
                        if (index == 0) {
                            opts += "<option value='" + item.TableName + "' selected='selected'>" + item.LastComputerDate.substr(0, 19) + "</option>";
                        } else {
                            opts += "<option value='" + item.TableName + "'>" + item.LastComputerDate.substr(0, 19) + "</option>";
                        }
                    });
                }
            }
            $("#drpBuildTime").append(opts);
        });
    }

}

// 会员细分过滤结果
//var dtFilterResult;
function loadFilterResult() {
    //if (dtFilterResult)
    //    dtFilterResult.fnClearTable();
    if (dtFilterResult) {
        dtFilterResult.fnClearTable();
        dtFilterResult.fnDraw();
    } else {
        //$('#dt_filter tbody').html("");
        dtFilterResult = $('#dt_filter').dataTable({
            sAjaxSource: '/MemSubdivision/GetMemberSubdResultByPage',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 30,
            aoColumns: [
                { data: 'MemberCardNo', title: "会员卡号", sortable: false, sWidth: "100px" },
                { data: 'CustomerName', title: "会员姓名", sortable: false, sWidth: "100px" },
                { data: 'Gender', title: "性别", sortable: true, sWidth: "50px" },
                {
                    data: null, title: "手机号", sortable: false, sWidth: "100px", render: function (obj) {
                        if (obj) {
                            return "<a onclick=\"redirectToMemberDetail('" + obj.MemberID + "')\">" + obj.CustomerMobile + "</a>";
                        }
                    }
                },
                { data: 'RegisterDate', title: "入会时间", sortable: true, sWidth: "100px" },
                { data: 'StoreName', title: "入会门店", sortable: true, sWidth: "100px" },
                { data: 'CustomerEmail', title: "邮箱", sortable: false, sWidth: "100px" },
            ],
            fnFixData: function (d) {
                var currsid = $("#hidCurSubdInstID").val(),
                    regstorecode = $("#drpRegStore option:selected").val(),
                    tablename = $("#drpBuildTime option:selected").val();
                var subDevDataType = $("#selDataSubType option:selected").val();
                currsid = utility.isNull(currsid) ? '00000000-0000-0000-0000-000000000000' : currsid;
                d.push({ name: 'currSubdId', value: currsid });
                d.push({ name: 'memCard', value: $("#txtMemCard").val() });
                d.push({ name: 'memName', value: $("#txtMemName").val() });
                d.push({ name: 'mobile', value: $("#txtMemMobile").val() });
                d.push({ name: 'registerStoreCode', value: regstorecode });
                d.push({ name: 'table', value: tablename });
                d.push({ name: 'subDevDataType', value: subDevDataType });
            }
        });

        //dtFilterResult.fnDraw();
    }
}

// 会员细分相关活动
var dtActivity;
function loadActivities() {
    if (dtActivity) {
        dtActivity.fnDraw();
    } else {
        dtActivity = $("#dt_activity").dataTable('/MemSubdivision/GetMemSubdActivitiesByPage', [
           { "mData": "ActivityName", "sTitle": "活动名称" },
           { "mData": "ActivityID", "sTitle": "活动编号" },
           {
               "mData": "PlanStartDate", "sTitle": "活动开始时间", "fnRender": function (obj) {
                   var startdate = obj.aData.PlanStartDate;
                   return utility.isNull(startdate) ? "" : startdate.replace("T", " ").substr(0, 16);
               }
           },
           {
               "mData": "PlanEndDate", "sTitle": "活动结束时间", "fnRender": function (obj) {
                   var enddate = obj.aData.PlanEndDate;
                   return utility.isNull(enddate) ? "" : enddate.replace("T", " ").substr(0, 16);
               }
           }
        ], function (sSource, aoData, fnCallback, aoSearchFilter) {
            var sid = $("#hidSID").val();
            sid = utility.isNull(sid) ? '00000000-0000-0000-0000-000000000000' : sid;
            var d = $.extend({}, fixData(aoData), { msId: sid, activityName: $("#txtActName").val(), dateStart: $("#txtActStartDate").val(), dateEnd: $("#txtActEndDate").val() });
            ajax(sSource, d, function (data) {
                fnCallback(toTableData(data));
            });
        })
    }
}


//根据会员细分获取动态表数据
function getSubdStatiscical() {
    var sid = $("#hidSID").val();
    if (!utility.isNull(sid)) {
        ajaxSync("/MemSubdivision/GetSubdStatiscicalResult", { subdId: sid }, function (res) {
            var data = "";
            if (res.IsPass) {
                data = res.Obj[0];
                if (data != null && data.length > 0) {
                    var arrD1 = [];
                    var arrD2 = new Array();
                    data.forEach(function (item, index, array) {
                        arrD1.push([new Date(item.ComputeDate.substr(0, 10)).getTime(), item.ComputeCount]);
                        arrD2.push([new Date(item.ComputeDate.substr(0, 10)).getTime(), item.ComputeCount]);
                    });
                    InitialSubdStatiscs(arrD1, arrD2);
                }
            }
        });
    }

}

function InitialSubdStatiscs(d1, d2) {
    //// Setup the placeholder reference
    var elem = $('#bar_chart');

    if (d2.length <= 1) {
        return;
    }

    //  d2.push(d1);

    for (var i = 0; i < d1.length; i++) {
        d1[i][0] += 60 * 120 * 1000
    };
    for (var index = 0; index < d2.length; index++) {
        d2[index][0] += 60 * 120 * 1000
    };

    var options = {
        label: "abc",
        series: {
            curvedLines: { active: true }
        },
        yaxes: [
            {
                min: 0,
                position: "left",
                labelWidth: 50
            },
            {
                min: 0,
                position: "right"
            }
        ],
        xaxis: {
            mode: "time",
            minTickSize: [1, "day"],
            autoscaleMargin: 0.10

        },
        grid: {
            hoverable: true
            //,
            //labelMargin: 30
        },
        legend: { position: 'nw' },
        colors: ["#8cc7e0", "#3ca0ca"]
    };

    // Setup the flot chart
    fl_d_plot = $.plot(elem,
        [
            {
                data: d1,
                label: "数量",
                bars: {
                    show: true,
                    barWidth: 60 * 360 * 1000,
                    align: "center",
                    fill: 1
                }
            },
            {
                data: d2,
                label: "数量",
                curvedLines: {
                    active: true,
                    show: true,
                    lineWidth: 3
                },
                yaxis: 2,
                points: { show: true },
                stack: null
            }
        ], options);

    // Create a tooltip on our chart
    elem.qtip({
        prerender: true,
        content: 'Loading...', // Use a loading message primarily
        position: {
            viewport: $(window), // Keep it visible within the window if possible
            target: 'mouse', // Position it in relation to the mouse
            adjust: { x: 7 } // ...but adjust it a bit so it doesn't overlap it.
        },
        show: false, // We'll show it programatically, so no show event is needed
        style: {
            classes: 'ui-tooltip-shadow ui-tooltip-tipsy',
            tip: false // Remove the default tip.
        }
    });

    // Bind the plot hover
    elem.on('plothover', function (event, coords, item) {
        // Grab the API reference
        var self = $(this),
            api = $(this).qtip(),
            previousPoint, content,

        // Setup a visually pleasing rounding function
        round = function (x) { return Math.round(x * 1000) / 1000; };

        // If we weren't passed the item object, hide the tooltip and remove cached point data
        if (!item) {
            api.cache.point = false;
            return api.hide(event);
        }

        // Proceed only if the data point has changed
        previousPoint = api.cache.point;
        if (previousPoint !== item.seriesIndex) {
            // Update the cached point data
            api.cache.point = item.seriesIndex;

            // Setup new content
            content = item.series.label + ': ' + round(item.datapoint[1]);

            // Update the tooltip content
            api.set('content.text', content);

            // Make sure we don't get problems with animations
            api.elements.tooltip.stop(1, 1);

            // Show the tooltip, passing the coordinates
            api.show(coords);
        }
    });
}

// 秒数转换
function secondToHour(second) {
    if (utility.isNull(second)) {
        return "";
    }
    var time = '';
    if (second >= 3600) {
        time += parseInt(second / 3600) + '小时';
        second %= 3600;
    }
    if (second >= 60) {
        time += parseInt(second / 60) + '分钟';
        second %= 60;
    }
    if (second > 0) {
        time += second + '秒';
    }
    if (second == 0) {
        time = '<1秒';
    }
    return time;
}

function setLastRootClass() {
    $(".sub-rootreletion").removeClass("lastNode");
    $(".sub-rootreletion").last().addClass("lastNode");
    $(".sub-rootreletion .filter-item").removeClass("lastNode");
    $(".sub-rootreletion .filter-item:last-child").addClass("lastNode");
}

// 获得下拉搜索值的名字
function getSeleSearchItemsText(filterJsonStr) {
    var filter = JSON.parse(filterJsonStr);
    var seletypelist = [];
    var isItemExist = false;
    $.each(filter.rfl, function (i, val) {
        var arrSubRootFilter = val.srfl;
        $.each(arrSubRootFilter, function (i, subVal) {
            var l = subVal.l;
            var r = subVal.r;
            leftValueList.forEach(function (item, index, array) {
                if (item.FieldAlias == l && item.ControlType == "selesearch") {
                    if (filterRightVals[l]) {
                        filterRightVals[l].forEach(function (fitem, findex, farray) {
                            if (fitem.sv == r) {
                                isItemExist = true;
                                return;
                            }
                        });
                    }
                    if (!isItemExist) seletypelist.push({ Type: l, Code: r });
                }
            });
        });
    });

    if (seletypelist.length > 0) {
        var postUrl = "/MemSubdivision/GetSeleSearchItemsText";
        ajax(postUrl, seletypelist, getSeleSearchItemsTextCallback(filterJsonStr));
    } else {
        convertFilterJsonToForm(filterJsonStr);
    }
}

function getSeleSearchItemsTextCallback(fJsonStr) {
    return function (data) {
        if (data != null) {
            var ts = eval('(' + data.Data + ')');
            for (var item in ts) {
                filterRightVals[item].push(ts[item][0]);
            }
        }
        convertFilterJsonToForm(fJsonStr);
    }
}

// 把过滤条件加载到页面中
function convertFilterJsonToForm(filterJsonStr) {
    if (!filterJsonStr) {
        return false;
    }
    var subRootReletionHtml = "";
    $("#ulSubRootReletion").html("");
    var filter = JSON.parse(filterJsonStr);
    var arrRootFilter = filter.rfl;

    $(".root-reletion").removeAttr("r");
    $(".root-reletion").attr("r", filter.r);
    $(".root-reletion #hyRoot").text(getOperatorName(filter.r));

    $.each(arrRootFilter, function (i, val) {
        subRootReletionHtml += "<li class='sub-rootreletion' r='" + val.r + "'><span class='dynatree-connector'></span><i class='splashy-diamonds_1'></i>&nbsp;<a id='hySubRoot' href='javascript:;'>" + getOperatorName(val.r) + "</a>&nbsp;<a id='hyAddFilter' href='javascript:;'><i class='splashy-add_small'></i></a>&nbsp;<a id='hyDelSubReletion' href='javascript:;'><i class='splashy-gem_remove'></i></a><ul id='ulFilter'>";
        var arrSubRootFilter = val.srfl;
        $.each(arrSubRootFilter, function (i, subVal) {
            var fa = getAlias(subVal.l);
            var rDynamic = "";
            if (fa.ControlType == "dynamic") {
                //var drv = eval(subVal.r);
                //rDynamic = drv.rightValue;
                rDynamic = subVal.r.split('|')[0];
            }
            var datatype = "",
                controltype = "",
                enumvalue = "",
                enumobj = null,
                regular = "",
                descname = "",
                l = subVal.l,
                e = subVal.e,
                r = subVal.r,
                rText = r == "" ? "空" : (rDynamic == "" ? r : rDynamic),
                dictTableName = "",
                IsDynamicAlias = false,
                dictTableType = "";

            leftValueList.forEach(function (item, index, array) {
                if (item.FieldAlias == l) {
                    datatype = item.FieldType;
                    controltype = item.ControlType;
                    enumvalue = item.EnumValue;
                    regular = item.Reg;
                    descname = item.FieldDesc;
                    dictTableName = item.dictTableName;
                    dictTableType = item.dictTableType;
                    IsDynamicAlias = item.IsDynamicAlias;
                    //if (enumvalue) {
                    //    enumobj = eval("(" + enumvalue + ")");
                    //    if (enumobj.Type == "Fixed") {
                    //        arrEnum = enumobj.Code.split("|");
                    //    } else {
                    //        arrEnum = filterRightVals[l];
                    //    }
                    //}
                    if (item.DataType = "select") {
                        if (!utility.isNull(filterRightVals)) {
                            arrEnum = filterRightVals[l];
                        }
                    }
                    if (controltype == "select") {
                        arrEnum.forEach(function (item, index, array) {
                            if (item.sv == r) {
                                rText = item.st;
                                return false;
                            }
                        });
                    } else if (controltype == "selesearch") {
                        arrEnum.forEach(function (item, index, array) {
                            if (item.sv == r) {
                                rText = item.st;
                                return false;
                            }
                        });
                    } else if (controltype == "date") {
                        var rTextLower = rText.toString().toLowerCase();
                        if (rTextLower.indexOf("dateadd") >= 0) {
                            var subtxt1 = "";
                            if (rTextLower.indexOf("'") >= 0) {
                                subtxt1 = rTextLower.substr(rTextLower.indexOf("dateadd") + 8, rTextLower.indexOf(")") - rTextLower.indexOf("dateadd") - 9);
                            } else {
                                subtxt1 = rTextLower.substr(rTextLower.indexOf("dateadd") + 8, rTextLower.indexOf(")") - rTextLower.indexOf("dateadd") - 8);
                                if (utility.isNull(subtxt1)) {
                                    rText = "";
                                } else {
                                    var arr = new Array();
                                    arr = subtxt1.split(",");
                                    if (arr.length != 3)
                                        rText = "";
                                    else {
                                        var dateSel = arr[0],
                                            addValue = arr[1],
                                            orgDate = arr[2].replace(/\'/g, "").trim(),
                                            rightDateText = "";
                                        if (rightDateSelObj != null) {
                                            for (var i = 0; i < rightDateSelObj.length; i++) {
                                                if (rightDateSelObj[i].OptionValue.toLowerCase() == orgDate.toLowerCase()) {
                                                    rightDateText = rightDateSelObj[i].OptionText;
                                                }
                                            }
                                        }
                                        leftValueList.forEach(function (item, index, array) {
                                            if (item.FieldAlias.toLowerCase() == orgDate) {
                                                rightDateText = item.FieldDesc;
                                            }
                                        });
                                        if (rightDateText != "") {
                                            var dateTxt = "";
                                            if (dateSel == "day") {
                                                dateTxt = "天";
                                            } else if (dateSel == "month") {
                                                dateTxt = "月";
                                            } else if (dateSel == "hour") {
                                                dateTxt = "时";
                                            } else if (dateSel == "minute") {
                                                dateTxt = "分";
                                            } else {
                                                dateTxt = "";
                                            }
                                            rText = rightDateText + "+ " + addValue + dateTxt;
                                        } else {
                                            var dateTxt = "";
                                            if (dateSel == "day") {
                                                dateTxt = "天";
                                            } else if (dateSel == "month") {
                                                dateTxt = "月";
                                            } else if (dateSel == "hour") {
                                                dateTxt = "时";
                                            } else if (dateSel == "minute") {
                                                dateTxt = "分";
                                            } else {
                                                dateTxt = "";
                                            }
                                            rText = orgDate + "+ " + addValue + dateTxt;
                                        }
                                    }
                                }
                            }
                        } else {
                            if (rightDateSelObj != null) {
                                for (var i = 0; i < rightDateSelObj.length; i++) {
                                    if (rightDateSelObj[i].OptionValue.toLowerCase() == rText.toLowerCase()) {
                                        rText = rightDateSelObj[i].OptionText;
                                    }
                                }
                            }
                            leftValueList.forEach(function (item, index, array) {
                                if (item.FieldAlias == rText) {
                                    rText = item.FieldDesc;
                                }
                            });
                        }
                    } else {
                        if (datatype == dataTypeList.int[0] || datatype == dataTypeList.dec[0] || datatype == dataTypeList.dec[1]) {
                            var addText = "", addNumber = "", curRightValue = "", rTextSel = "";
                            curRightValue = rText;
                            if (curRightValue.indexOf("+") >= 0) {
                                addText = "+";
                                rTextSel = curRightValue.substr(0, curRightValue.indexOf("+"));
                                addNumber = curRightValue.substr(curRightValue.indexOf("+") + 1, curRightValue.length - curRightValue.indexOf("+") - 1);
                            }
                            if (curRightValue.indexOf("*") >= 0) {
                                addText = "*";
                                rTextSel = curRightValue.substr(0, curRightValue.indexOf("*"));
                                addNumber = curRightValue.substr(curRightValue.indexOf("*") + 1, curRightValue.length - curRightValue.indexOf("+") - 1);
                            }
                            if (rTextSel != "") {
                                leftValueList.forEach(function (item, index, array) {
                                    if (item.FieldAlias == rTextSel) {
                                        rTextSel = item.FieldDesc;
                                    }
                                });
                                rText = rTextSel + addText + addNumber;
                            } else {
                                leftValueList.forEach(function (item, index, array) {
                                    if (item.FieldAlias == rText) {
                                        rText = item.FieldDesc;
                                    }
                                });
                            }
                        } else {
                            leftValueList.forEach(function (item, index, array) {
                                if (item.FieldAlias == rText) {
                                    rText = item.FieldDesc;
                                }
                            });
                        }
                    }
                    return false;
                }
            });
            subRootReletionHtml += "<li class='filter-item' l='" + l + "' e='" + e + "' r='" + r + "' datatype='" + datatype
                + "' controltype='" + controltype + "' fieldName='" + descname
                + "' enumvalue='" + enumvalue + "' reg='" + regular + "' dictTableName='" + dictTableName + "' dictTableType='" + dictTableType + "' isdynamicalias='" + IsDynamicAlias + "'>"
                + "<span class='dynatree-connector'></span>"
                + "<a id='hyLeftValue' href='javascript:;'>" + descname
                + "</a>&nbsp;<a id='hyOperation' href='javascript:;'>" + getOperatorName(e) + "</a>&nbsp;"
                + "<a id='hyRightValue' href='javascript:;'>" + rText.split("|")[0] + "</a>&nbsp;"
                + "<i class='splashy-menu'></i>&nbsp;"
                + "<a id='hyDelFilter' href='javascript:;'><i class='splashy-gem_remove'></i></a>"
                + "</li>";
        });
        subRootReletionHtml += "</ul></li>";
    });
    if (!utility.isNull(subRootReletionHtml)) {
        $("#ulSubRootReletion").html(subRootReletionHtml);
    }
    setLastRootClass();
}

// 获取页面会员细分
function getBindMemberSubdFromForm() {
    var schtype = $("input[name='radSchedule']:checked").val(),
        fixday = $("input[name='cycle_days']:checked").val(),
        schJson = {};

    fixday = fixday == "fixed" ? $("#drpFixday").val() : fixday;
    switch (schtype) {
        case "immediately":
            schJson = { type: schtype, cycle: "", d: "", ap: "" };
            break;
        case "appointed":
            schJson = { type: schtype, cycle: "", d: "", ap: $("#txtDate").val() + " " + $("#txtTime").val() };
            break;
        case "cycle":
            if ($("#drpCycle").val() == "weekly") {
                fixday = $("#drpWeek").val();
            }
            schJson = { type: schtype, cycle: $("#drpCycle").val(), d: fixday, ap: $("#txtCycleTime").val() };
            break;
        default:
            break;
    }
    var oMemberSubd = {
        SubdivisionID: $("#hidSID").val() == '' ? '00000000-0000-0000-0000-000000000000' : $("#hidSID").val(),
        SubdivisionName: encode($("#txtName").val()),
        CurSubdivisionInstanceID: $("#hidCurSubdInstID").val(),
        DataGroupID: $("#selGroup option:selected").val(),
        SubdivisionDesc: ($("#txtDesc").val() == "" ? "" : encode($("#txtDesc").val())),//SubdivisionDesc: ($("#txtDesc").val() == "" ? " " : encode($("#txtDesc").val())),
        Condition: "",
        Enable: (($("#txtActive").val() == "" || $("#txtActive").val() == "未激活") ? false : true),
        Schedule: JSON.stringify(schJson),
        SubdivisionType: $("#selSubType").val(),
        SubDevDataType: $("#selDataSubType option:selected").val()//,
        //StoreCode: $("#selStore option:selected").val().trim()
    }
    return oMemberSubd;
}

// 更新会员细分
function editMemberSubdivision(obj) {
    var postUrl = "/MemSubdivision/EditMemberSubdivision";
    ajaxSync(postUrl, { subdStr: JSON.stringify(obj), importObj: importObj }, editMemberSubdivisionCallback);
}

function editMemberSubdivisionCallback(data) {
    if (data.hasOwnProperty("IsPass") && data.IsPass) {
        importObj = "";
        refreshTree(data.Obj[0].toString());
        if ($("#hidSID").val() == '') {
            $("#hidSID").val(data.Obj[0].toString());
        }
        $("#btnActive").removeAttr("disabled");
        $("#btnSave").text("保存");
        if ($("#selDataSubType").val() == "1") {
            $("#btnResultImport").hide();
            $("#btnDownTemplate").hide();
        } else {
            $("#btnResultImport").show();
            $("#btnDownTemplate").show();
        }
    }
    $.dialog(data.MSG);
}

// 删除会员细分
function deleteMemberSubd() {
    var postData = 'sid=' + $("#hidSID").val(),
        postUrl = "/MemSubdivision/DeleMemberSubdivision";
    ajax(postUrl, postData, deleteMemberSubdCallback);
}

function deleteMemberSubdCallback(data) {
    if (data.hasOwnProperty("IsPass") && data.IsPass) {
        refreshTree(curParentNode);
        initPageControlsDisabled();
        resetForm();
        $("#flag").val(0);
        initPageVisiable();
    }
    $.dialog(data.MSG);
}

//激活、取消激活会员细分
function enableSubd(isenable) {
    if (!$("#hidSID").val()) {
        $.dialog("请选择会员细分！");
        return false;
    }
    var postData = { "sid": $("#hidSID").val(), "isEnable": isenable },
        postUrl = "/MemSubdivision/EnableMemberSubdivision";
    ajax(postUrl, postData, enableSubdCallback(isenable));
}

function enableSubdCallback(isEnable) {
    return function (data) {
        if (data.IsPass) {
            if (isEnable == true) {
                setTimeout(function () {
                    getMemberSubdToForm($("#hidSID").val());
                }, 2000);
                $("#btnActive").attr("disabled", "disabled");
                $("#btnInactive").removeAttr("disabled");
                $("#btnRefreshCurrentInfo").removeAttr("disabled");
                $("#btnSave,#btnSaveFilter").attr("disabled", "disabled");
                $("#flag").val("1");
                $.dialog($("#txtName").val() + " 已激活！");
                return;
            } else if (isEnable == false) {
                setTimeout(function () {
                    getMemberSubdToForm($("#hidSID").val());
                }, 2000);
                $("#btnActive").removeAttr("disabled");
                $("#btnRefreshCurrentInfo").attr("disabled", "disabled");
                $("#btnInactive").attr("disabled", "disabled");
                $("#btnSave,#btnSaveFilter").removeAttr("disabled");
                $("#flag").val("0");
                $.dialog($("#txtName").val() + " 已取消激活！");
                return;
            }
        } else {
            $("#flag").val("0");
            $.dialog(data.MSG);
        }
    }
}

// 刷新细分树
function refreshTree(snode) {
    var d = { key: $("#txtSearch").val() }, a = '/MemSubdivision/GetMemberSubdivisionByKey';
    ajax(a, d, function (data) {
        $('#ms_tree').dynatree({
            children: data,
            onActivate: function (node) {
                if (!node.data.isFolder) {
                    curParentNode = node.parent.data.key;
                    resetForm();
                    $("#aTab1").click();
                    $("#selDataSubType").attr("disabled", "disabled");
                    getMemberSubdToForm(node.data.key);
                    isFirstShow = "0";
                    isFirstShow = "1";
                    $("#litab3").show();
                    //showmask();
                } else {
                    $("#hidSID").val("");
                }
            },
            onPostInit: function (isReloading, isError, dtNode) {
                this.tnRoot.visit(function (dtNode) {
                    if (!utility.isNull(snode) && dtNode.data.key == snode) {
                        if (dtNode.data.isFolder) {
                            dtNode.expand(true);
                        } else {
                            dtNode.focus(true);
                            dtNode.activate(true);
                        }
                    }
                });
            }
        }).dynatree("getTree").reload();
    });

}


// 获取连接符名称
function getOperatorName(sign) {
    var operatorName = "";
    switch (sign) {
        case "or":
            operatorName = "或者";
            break;
        case "and":
            operatorName = "并且";
            break;
        default:
            if (operatorObj != null && operatorObj.length > 0) {
                operatorObj.forEach(function (item, index) {
                    if (item.key == sign) {
                        operatorName = item.value;
                    }
                })
            }
            break;
    }
    return operatorName;
}

//展示条件左值
function showLeftValueMenu(me) {
    $("#btnSelectOK").unbind();
    $("#selDialogError").text("");
    $("#selectDialogTitle").text("选择左值");
    //$("#selLeftValue li").show();
    $("#dvOption").css("min-height", "300px");
    $("#dvOption").html(leftValueHtml);
    $("#txtLeftValueSearch").bind("keypress", function (e) {
        //e.preventDefault();
        if (e.keyCode == 13) {
            return false;
        }
    });
    if ($("#headingpara").length > 0) {//清除动态参数
        $("#selDialogError").remove();
        $("#headingpara").remove();
        $("#formseppara").remove();
        $("#select_dialog .formSep").css("height", "380px");
    }
    $("#txtLeftValueSearch").bind("keyup", function (e) {
        //e.preventDefault();,
        var reg = "[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]";
        var valueserach = $("#txtLeftValueSearch").val();
        if (valueserach.match(reg)) {//禁止输入特殊字符
            $("#txtLeftValueSearch").val(valueserach.substr(0, valueserach.length - 1));
        }
        if (e.keyCode == 13) {
            return false;
        }
        var name = $("#txtLeftValueSearch").val();
        var regex = /^[A-Za-z\0-9\u4e00-\u9fa5]*$/g;
        if (name.match(regex)) {
            if (!utility.isNull(name)) {
                $("#selLeftValue li").each(function (index, item) {
                    var curtext = $(this).children("input").attr("title");
                    $(this).show();
                    if (!utility.isNull(curtext) && !utility.isNull(name)) {
                        if (curtext.toLowerCase().indexOf(name.toLowerCase()) < 0) {
                            $(this).hide();
                        }
                    } else {
                        if (curtext.indexOf(name) < 0) {
                            $(this).hide();
                        }
                    }
                });
                $.colorbox.resize();
            } else {
                $("#selLeftValue li").each(function (index, item) {
                    $(this).show();
                });
                $.colorbox.resize();
            }
        }
    });
    $("#btnDelTemplet").hide();

    var curLeftText = $(me).html();
    $("#selLeftValue li").each(function (index, item) {
        if ($(this).children("input").attr("title") == curLeftText) {
            $(this).children("input").attr("checked", "checked");
        }
    });
    //gebo_chosen.init();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#select_dialog",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#btnSelectOK").click(function (e) {
                //var selVal = "";
                var selText = "",
                    tid = "",
                    isHasValue = false;

                $("#selLeftValue li").each(function (index, item) {
                    if ($(this).children("input").attr("checked") == "checked") {
                        //selVal = $(this).children("input").val();
                        selText = $(this).children("input").attr("title");
                        tid = $(this).attr("id");
                        isHasValue = true;
                    }
                });

                if (!isHasValue) {
                    $("#selDialogError").text("请选择左值");
                    $.colorbox.resize();
                } else {
                    $("#selDialogError").text("");
                    $(me).siblings("#hyOperation").text("等于");
                    $(me).siblings("#hyRightValue").text("请选择右值");
                    $(me).parent().removeAttr("e").removeAttr("r");
                    $(me).parent().attr("e", "=");
                    var selObj = $("#" + tid);
                    $(me).parent().removeAttr("l");
                    $(me).parent().removeAttr("datatype");
                    $(me).parent().removeAttr("controltype");
                    $(me).parent().removeAttr("type");
                    $(me).parent().removeAttr("enumvalue");
                    $(me).parent().removeAttr("reg");
                    $(me).parent().removeAttr("dictTableName");
                    $(me).parent().removeAttr("dictTableType");
                    $(me).parent().removeAttr("fieldName");
                    $(me).parent().removeAttr("isdynamicalias");
                    $(me).parent().attr("l", $(selObj).attr("l"));
                    $(me).parent().attr("datatype", $(selObj).attr("datatype"));
                    $(me).parent().attr("controltype", $(selObj).attr("controltype"));
                    $(me).parent().attr("enumvalue", $(selObj).attr("enumvalue"));
                    $(me).parent().attr("type", $(selObj).attr("type"));
                    $(me).parent().attr("reg", $(selObj).attr("reg"));
                    $(me).parent().attr("dictTableName", $(selObj).attr("dictTableName"));
                    $(me).parent().attr("fieldName", $(selObj).attr("fieldName"));
                    $(me).parent().attr("isdynamicalias", $(selObj).attr("isdynamicalias"));

                    $(me).text(selText);
                    e.preventDefault();
                    $.colorbox.close();
                }
            });

            $("#btnCancelSelect").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });
        }
    });
}

//展示关系菜单
function showRootReletion(me) {
    if ($("#headingpara").length > 0) {//清除动态参数
        $("#selDialogError").remove();
        $("#headingpara").remove();
        $("#formseppara").remove();
        $("#select_dialog .formSep").css("height", "380px");
    }
    $("#btnSelectOK").unbind();
    $("#selectDialogTitle").text("选择关系");
    //var html = "<select name='selRootReletion' id='selRootReletion' data-placeholder='选择关系...' class='span14'><option value='and'>并且</option><option value='or'>或者</option></select>";

    var html = "<ul class='selRootReletion'>";
    html += "<li><input id='mainRelationAnd' type='radio' title='并且' name='relationRdoGroup' value='and' style='float:left;margin-right:5px'/>"
    html += "<label for='mainRelationAnd'>并且</label>";
    html += "</li>";
    html += "<li>";
    html += "<input id='mainRelationOr' type='radio' title='或者' name='relationRdoGroup' value='or' style='float:left;margin-right:5px' />"
    html += "<label for='mainRelationOr'>或者</label>";
    html += "</li></ul>";

    var curReletionText = $(me).html();
    $("#dvOption").css("min-height", "50px");
    $("#dvOption").html(html);

    $(".selRootReletion li").each(function (index, item) {
        if ($(this).children("input").attr("title") == curReletionText) {
            $(this).children("input").attr("checked", "checked")
        }
    })

    $("#btnDelTemplet").hide();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#select_dialog",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#btnSelectOK").click(function (e) {
                var selVal = "",
                    selText = "";
                $(".selRootReletion li").each(function (index, item) {
                    if ($(this).children("input").attr("checked") == "checked") {
                        selVal = $(this).children("input").val();
                        selText = $(this).children("input").attr("title");
                    }
                })

                //var selObj = $("#selRootReletion").find("option:selected");
                $(me).parent().removeAttr("r");
                $(me).parent().attr("r", selVal);
                $(me).text(selText);
                e.preventDefault();
                $.colorbox.close();
            });

            $("#btnCancelSelect").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });
        }
    });
}

//增加子关系菜单
function addSubRootReletion() {
    if ($("#headingpara").length > 0) {//清除动态参数
        $("#selDialogError").remove();
        $("#headingpara").remove();
        $("#formseppara").remove();
        $("#select_dialog .formSep").css("height", "380px");
    }
    $("#ulSubRootReletion").append("<li class='sub-rootreletion' r='and'>"
        + "<span class='dynatree-connector'></span><i class='splashy-diamonds_1'></i>&nbsp;"
        + "<a id='hySubRoot' href='javascript:;'>并且</a>&nbsp;"
        + "<a id='hyAddFilter' href='javascript:;'><i class='splashy-add_small'></i>" + "</a>&nbsp;"
        + "<a id='hyDelSubReletion' href='javascript:;'><i class='splashy-gem_remove'></i></a><ul id='ulFilter'></ul>"
        + "</li>");
    setLastRootClass();
}

//新增规则
function addFilter(me) {
    $(me).parent().find("#ulFilter").append("<li class='filter-item'>"
        + "<span class='dynatree-connector'></span>"
        + "<a id='hyLeftValue' href='javascript:;'>选择左值</a>&nbsp;"
        + "<a id='hyOperation' href='javascript:;'>选择操作符</a>&nbsp;"
        + "<a id='hyRightValue' href='javascript:;'>选择右值</a>&nbsp;"
        + "<i class='splashy-menu'></i>&nbsp;"
        + "<a id='hyDelFilter' href='javascript:;'><i class='splashy-gem_remove'></i></a>"
        + "</li>");
    setLastRootClass();
}

//删除子关系菜单
function deleteSubReletion(me) {
    deleteParentLi(me);
}

//删除规则
function deleteFilter(me) {
    deleteParentLi(me);
}

function deleteParentLi(me) {
    $(me).parent().remove();
    setLastRootClass();
}

/*---------------------------------细分规则相关-------------------------------------*/

// 加载全部模板
function getFilterAll() {
    var postUrl = "/MemSubdivision/GetMemSubdFilterAll";
    ajax(postUrl, null, getFilterAllCallback);
}

function getFilterAllCallback(rst) {
    $("#btnSelectOK").unbind();

    var html = "<select name='selFilterTemplet' id='selFilterTemplet' data-placeholder='选择模板...' class='chzn_a span12'>"
        + "<option value=''>请选择</option>",
        data = rst;

    if (!utility.isNull(data)) {
        data.forEach(function (item, index, array) {
            html += "<option value='" + item.Condition + "' tid='" + item.SubdivisionID + "'>" + item.SubdivisionName + "</option>";
        });
    }
    html += "</select>";
    if ($("#headingpara").length > 0) {
        $("#selDialogError").remove();
        $("#headingpara").remove();
        $("#formseppara").remove();
        $("#select_dialog .formSep").css("height", "380px");
    }
    $("#selectDialogTitle").text("选择模板");
    $("#dvOption").css("min-height", "50px");
    $("#dvOption").html(html);
    $("#selDialogError").text("");
    $.colorbox({
        initialHeight: '200',
        initialWidth: '200',
        href: "#select_dialog",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#btnSelectOK").bind("click", function (e) {
                if (utility.isNull($("#selFilterTemplet option:selected").attr("tid"))) {
                    $("#selDialogError").text("请选择细分规则模板");
                    $.colorbox.resize();
                }
                else {
                    var selObj = $("#selFilterTemplet").find("option:selected");
                    convertFilterJsonToForm($(selObj).attr("value"));
                    e.preventDefault();
                    $.colorbox.close();
                }
            });

            $("#btnCancelSelect").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });
        }
    });
}

// 获取会员细分过滤条件所有左值
function getMemberSubdLeftValuesAll() {
    var postUrl = "/MemSubdivision/GetMemSubdLeftValuesAll";
    ajaxSync(postUrl, null, getMemberSubdLeftValuesAllCallback);
}

function getMemberSubdLeftValuesAllCallback(data) {
    var vlist = eval(data.LeftAliasList);//左值列表
    aliasParameterList = eval(data.LeftDynamicParameters);//左值动态参数   
    aliasParameterOptions = eval(data.ParameterOption);//动态参数字典数据

    if (utility.isNull(vlist)) {
        return;
    }
    var item,
        rValCfg = [];

    $.each(vlist, function (i, val) {
        if (vlist[i].ControlType == "select" || vlist[i].ControlType == "selesearch") {
            rValCfg.push({
                FieldAlias: vlist[i].FieldAlias,
                TableName: vlist[i].DictTableName,
                TableType: vlist[i].DictTableType
            });
        }
    });
    if (rValCfg.length > 0) {
        getFilterRightValues(rValCfg);
    }

    leftValueList = vlist;
    leftValueHtml = "<input name=\"txtLeftValueSearch\" class=\"search_query\" id=\"txtLeftValueSearch\" type=\"text\" size=\"16\" placeholder=\"搜索...\" autocomplete=\"off\" />"
            + "<ul name='selLeftValue' id='selLeftValue' data-placeholder='选择左值...' class='chzn_a span12'>";

    vlist.forEach(function (item, index, array) {
        var tid = "leftRadio" + item.FieldAlias + index,
            lid = "leftLi" + item.FieldAlias + index;

        leftValueHtml += "<li id='" + lid + "' l='" + item.FieldAlias + "' f='" + item.FieldName + "' datatype='" + item.FieldType
            + "' controltype='" + item.ControlType + "' enumvalue='" + item.EnumValue + "' reg='" + item.Reg + "' fieldName='" + item.FieldDesc
            + "' dictTableName='" + item.DictTableName + "' dictTableType='" + item.DictTableType + "' tableName='" + item.TableName + "' isdynamicalias='" + item.IsDynamicAlias + "'>"
            + "<input id='" + tid + "'type='radio' title='" + item.FieldDesc + "' name='selLeftGroup' style='float:left;margin-right:5px' />"
            + "<label for='" + tid + "'>" + item.FieldDesc + "</label>"
            + "</li>"
    });

    leftValueHtml += "</ul>";

}

// 选择连接符
function selectOperator(me) {
    if ($("#headingpara").length > 0) {//清除动态参数
        $("#selDialogError").remove();
        $("#headingpara").remove();
        $("#formseppara").remove();
        $("#select_dialog .formSep").css("height", "380px");
    }
    $("#btnSelectOK").unbind();
    var dataType = $(me).parent().attr("datatype");
    if (utility.isNull(dataType)) {
        $.dialog("请先选择左值！");
        return;
    }
    $("#dvOption").css("min-height", "100px");
    if (dataType == dataTypeList.string[0] || dataType == dataTypeList.string[1]) {
        $("#dvOption").html(strOperatorHtml);
    } else {
        $("#dvOption").html(numOperatorHtml);
    }
    $("#selectDialogTitle").text("选择操作符");
    $("#selDialogError").text("");
    $("#btnDelTemplet").hide();

    var curOperatorText = $(me).html();
    $(".operator-menu li").each(function (index, item) {
        if ($(this).children("input").attr("title") == curOperatorText) {
            $(this).children("input").attr("checked", "checked");
        }
    });

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#select_dialog",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#btnSelectOK").click(function (e) {
                var selVal = "",
                    selText = "";
                $(".operator-menu li").each(function (index, item) {
                    if ($(this).children("input").attr("checked") == "checked") {
                        selVal = $(this).children("input").val();
                        selText = $(this).children("input").attr("title");
                    }
                });
                if (utility.isNull(selVal)) {//$(".operator-menu").val()
                    $("#selDialogError").text("请选择操作符");
                    $.colorbox.resize();
                } else {
                    $("#selDialogError").text("");
                    //var selObj = $(".operator-menu").find("option:selected");
                    $(me).text(selText); //$(me).text($(selObj).text());
                    $(me).parent().removeAttr("e");
                    $(me).parent().attr("e", selVal); //$(me).parent().attr("e", $(selObj).attr("value"));
                    e.preventDefault();
                    $.colorbox.close();
                }
            });

            $("#btnCancelSelect").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });

        }
    });
}

// 选择右值
function selectRightValue(me) {
    $("#btnSelectOK").unbind();
    $("#btnDelTemplet").hide();
    var dialogID = "select_dialog",
        dataType = $(me).parent().attr("datatype");
    if (utility.isNull(dataType)) {
        $.dialog("请先选择左值！");
        return;
    }

    var operator = $(me).parent().attr("e");
    if (utility.isNull(operator)) {
        $.dialog("请先选择操作符！");
        return;
    }

    curDynamicValue = $(me).parent().attr("r") == undefined ? "" : $(me).parent().attr("r");

    var fieldalias = $(me).parent().attr("l");
    var controlType = $(me).parent().attr("controltype");
    var IsDynamicAlias = $(me).parent().attr("isdynamicalias");

    var enumvalue = $(me).parent().attr("enumvalue");
    var enumobj = eval("(" + enumvalue + ")");
    var type = $(me).parent().attr("datatype");
    var regular = $(me).parent().attr("reg");
    if (regular != "null" || !utility.isNull(regular)) {
        regular = new RegExp(regular);
    }
    var tableName = $(me).parent().attr("TableName");
    var dictTableName = $(me).parent().attr("dictTableName");
    var dictTableType = $(me).parent().attr("dictTableType");
    $("#selectDialogTitle").text("选择右值");
    $("#selDialogError").text("");
    var curRightValue = $(me).parent().attr("r") == undefined ? "" : $(me).parent().attr("r");
    getRightValueHtml(controlType, dictTableName, dictTableType, type, fieldalias, tableName, IsDynamicAlias);
    $("#dvOption").css("min-height", "100px");
    $("#dvOption").html(rightValueHtml);
    if ($("#headingpara").length > 0) {
        $("#selDialogError").remove();
        $("#headingpara").remove();
        $("#formseppara").remove();
        $("#select_dialog .formSep").css("height", "380px");
    }
    if (IsDynamicAlias == 'true') {
        $(".formSep").css("height", "auto");
        $("#dvOption").css("height", "auto");
        paraHtml = "<div class='heading' id='headingpara'><h3><span id='selectDialogTitle'>参数设置</span></h3></div>";
        paraHtml += "<div class='formSep' id='formseppara'><div class='row-fluid'><div class='span12'><ul>";
        var f = getAlias(fieldalias);
        curDynamicParaCount = f.ParameterCount;
        var itemWidth = 0;
        var aps = new Array();
        for (i = 0; i < f.ParameterCount; i++) {
            var ap = getAliasParameter(fieldalias, i + 1);
            aps.push(ap);
        }
        dateController.length = 0;
        datetimeController.length = 0;
        getHTML(aps, aliasParameterOptions);
        paraHtml += "</ul></div> <span id='selDialogError' class='error' style='color:red'></span></div></div>"
        setTimeout(function () {
            $("#select_dialog .formSep").after(paraHtml);
            paraHtml = "";
        }, 200);

    }
    else {
        $("#dvOption").after(" <span id='selDialogError' class='error' style='color:red'></span>");
    }


    //多选控件
    $("select[id^=txtPara]").chosen();
    //.trigger("liszt:updated");
    if ($("#tbFilterDate").length > 0) {
        $("#tbFilterDate").datepicker();
    }
    if ($("#tbFilterTime").length > 0) {
        $("#tbFilterDate").datepicker();
        $("#tbFilterTime").timepicker();
    }

    if ($("#tbFilterDate").length > 0 || $("#tbFilterTime").length > 0) {
        $("#rdo_DateRight1").attr("checked", "checked");

        $("#tbFilterDate").attr("disabled", "disabled")
        $("#tbFilterNumber").attr("disabled", "disabled");
        $(".selDateAddValue").attr("disabled", "disabled");
        $("#tbFilterTime").attr("disabled", "disabled");
        $("#selCompareLeft").attr("disabled", "disabled");
        $("#tbFilterNumber3").attr("disabled", "disabled");
        $("#selDateAddValue3").attr("disabled", "disabled");

        $("#rdo_DateRight1").change(function () {
            if ($("#rdo_DateRight1").attr("checked") == "checked") {
                $(".dateRight-menu1").removeAttr("disabled");
                $("#tbFilterNumber2").removeAttr("disabled");
                $(".selDateAddValue2").removeAttr("disabled");

                $("#tbFilterDate").attr("disabled", "disabled")
                $("#tbFilterNumber").attr("disabled", "disabled");
                $(".selDateAddValue").attr("disabled", "disabled");
                $("#tbFilterTime").attr("disabled", "disabled");
                $("#selCompareLeft").attr("disabled", "disabled")
                $("#tbFilterNumber3").attr("disabled", "disabled");
                $("#selDateAddValue3").attr("disabled", "disabled");
            }
            else {
                $(".dateRight-menu1").attr("disabled", "disabled")
                $("#tbFilterNumber2").attr("disabled", "disabled");
                $(".selDateAddValue2").attr("disabled", "disabled");
            }
        })

        $("#rdo_DateRight2").change(function () {
            if ($("#rdo_DateRight2").attr("checked") == "checked") {
                $("#tbFilterDate").removeAttr("disabled");
                $("#tbFilterNumber").removeAttr("disabled");
                $(".selDateAddValue").removeAttr("disabled");
                $("#tbFilterTime").removeAttr("disabled");
                $(".dateRight-menu1").attr("disabled", "disabled")
                $("#tbFilterNumber2").attr("disabled", "disabled");
                $(".selDateAddValue2").attr("disabled", "disabled");
                $("#selCompareLeft").attr("disabled", "disabled")
                $("#tbFilterNumber3").attr("disabled", "disabled");
                $("#selDateAddValue3").attr("disabled", "disabled");
            }
            else {
                $("#tbFilterDate").attr("disabled", "disabled")
                $("#tbFilterNumber").attr("disabled", "disabled");
                $(".selDateAddValue").attr("disabled", "disabled");
                $("#tbFilterTime").attr("disabled", "disabled");
            }
        })

        $("#rdo_DateRight3").change(function () {
            if ($("#rdo_DateRight3").attr("checked") == "checked") {
                $("#selCompareLeft").removeAttr("disabled");
                $("#tbFilterNumber3").removeAttr("disabled");
                $("#selDateAddValue3").removeAttr("disabled");

                $("#tbFilterDate").attr("disabled", "disabled")
                $("#tbFilterNumber").attr("disabled", "disabled");
                $(".selDateAddValue").attr("disabled", "disabled");
                $("#tbFilterTime").attr("disabled", "disabled");
                $(".dateRight-menu1").attr("disabled", "disabled")
                $("#tbFilterNumber2").attr("disabled", "disabled");
                $(".selDateAddValue2").attr("disabled", "disabled");
            }
            else {
                $("#selCompareLeft").attr("disabled", "disabled");
                $("#tbFilterNumber3").attr("disabled", "disabled");
                $("#selDateAddValue3").attr("disabled", "disabled");
            }
        })
    }
    if ($("#tbFilterText").length > 0) {
        $("#rdo_DateRight2").attr("checked", "checked");
        $("#selCompareLeft").attr("disabled", "disabled");
        $("#tbFilterNumber3").attr("disabled", "disabled");
        $("#selNumberAddValue").attr("disabled", "disabled");

        $("#rdo_DateRight2").change(function () {
            if ($("#rdo_DateRight2").attr("checked") == "checked") {
                $("#tbFilterText").removeAttr("disabled");;
                $("#selCompareLeft").attr("disabled", "disabled")
                $("#tbFilterNumber3").attr("disabled", "disabled");
                $("#selNumberAddValue").attr("disabled", "disabled");
            }
            else {
                $("#tbFilterText").attr("disabled", "disabled")
            }
        })

        $("#rdo_DateRight3").change(function () {
            if ($("#rdo_DateRight3").attr("checked") == "checked") {
                $("#selCompareLeft").removeAttr("disabled");
                $("#tbFilterNumber3").removeAttr("disabled");
                $("#selNumberAddValue").removeAttr("disabled");
                $("#tbFilterText").attr("disabled", "disabled")
            }
            else {
                $("#selCompareLeft").attr("disabled", "disabled");
                $("#tbFilterNumber3").attr("disabled", "disabled");
                $("#selNumberAddValue").attr("disabled", "disabled");
            }
        })
    }

    setRightValueHtml(controlType, type, curRightValue, fieldalias, IsDynamicAlias);

    $("#txtRightValueSearch").bind("keypress", function (e) {
        //e.preventDefault();
        if (e.keyCode == 13) {
            return false;
        }
    });

    $("#txtRightValueSearch").bind("keyup", function (e) {
        //e.preventDefault();
        var reg = "[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]";
        var valueserach = $("#txtRightValueSearch").val();
        if (valueserach.match(reg)) {//禁止输入特殊字符
            $("#txtRightValueSearch").val(valueserach.substr(0, valueserach.length - 1));
        }
        if (e.keyCode == 13) {
            return false;
        }
        var name = $("#txtRightValueSearch").val();
        var regex = /^[A-Za-z\0-9\u4e00-\u9fa5]*$/g;
        if (name.match(regex)) {
            if (!utility.isNull(name)) {
                $("#selRightValue li").each(function (index, item) {
                    var curtext = $(this).children("input").attr("title");
                    $(this).show();
                    if (!utility.isNull(curtext) && !utility.isNull(name)) {
                        if (curtext.toLowerCase().indexOf(name.toLowerCase()) < 0) {
                            $(this).hide();
                        }
                    } else {
                        if (curtext.indexOf(name) < 0) {
                            $(this).hide();
                        }
                    }
                });
                $.colorbox.resize();
            } else {
                $("#selRightValue li").each(function (index, item) {
                    $(this).show();
                });
                $.colorbox.resize();
            }
        }
    });


    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#select_dialog",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            if (controlType == "selesearch") {
                $("#selSearchRightValue").chosen();
                $('#selSearchRightValue_chzn .chzn-search input').unbind("keyup keydown keypress").autocomplete({
                    source: function (request, response) {
                        var k = $('#selSearchRightValue_chzn .chzn-search input').val();
                        //if (enumobj) {
                        //    if (enumobj.Type == 'Vehicle') {
                        //        get5RelatedCarModel(k);
                        //    } else if (enumobj.Type == 'Corp') {
                        //        get5RelatedCorp(k);
                        //    }
                        //}
                        $('#selSearchRightValue_chzn .chzn-search input').val(k);
                    }
                });
            }

            //单选
            if ($("#selRightValue").length > 0) {
                //gebo_chosen.init();
                $("#btnSelectOK").click(function (e) {
                    var selVal = "";
                    var selText = "";
                    $("#selRightValue li").each(function (index, item) {
                        if ($(this).children("input").attr("checked") == "checked") {
                            selVal = $(this).children("input").val();
                            selText = $(this).children("input").attr("title");
                        }
                    })

                    //var selObj = $("#selRightValue").find("option:selected");
                    if (utility.isNull(selVal)) {
                        $("#selDialogError").text("请选择右值");
                        $.colorbox.resize();
                    }
                    if (IsDynamicAlias == 'true') {
                        checkDynamic(e, curDynamicParaCount, fieldalias, me, selText);
                        return;
                    }
                    else {
                        $(me).text(selText);
                        $(me).parent().removeAttr("r");
                        $(me).parent().attr("r", selVal);
                        e.preventDefault();
                        $.colorbox.close();
                    }

                    //var selObj = $("#selRightValue").find("option:selected");
                    //if (utility.isNull($(selObj).text())) {
                    //    $("#selDialogError").text("请选择右值");
                    //    $.colorbox.resize();
                    //}
                    //else {
                    //    $(me).text($(selObj).text());
                    //    $(me).parent().removeAttr("r");
                    //    $(me).parent().attr("r", $(selObj).val());
                    //    e.preventDefault();
                    //    $.colorbox.close();
                    //}
                });
            }
            //单选搜索
            if ($("#selSearchRightValue").length > 0) {
                $("#btnSelectOK").click(function (e) {
                    var selObj = $("#selSearchRightValue").find("option:selected");
                    if (utility.isNull($(selObj).text())) {
                        $("#selDialogError").text("请选择右值");
                        $.colorbox.resize();
                    }
                    if (IsDynamicAlias == 'true') {
                        checkDynamic(e, curDynamicParaCount, fieldalias, me, $(selObj).text());
                        return;
                    }
                    else {
                        $(me).text($(selObj).text());
                        $(me).parent().removeAttr("r");
                        $(me).parent().attr("r", $(selObj).val());
                        e.preventDefault();
                        $.colorbox.close();
                    }
                });
            }
            //输入框（含动态维度）
            if ($("#tbFilterText").length > 0) {
                $("#btnSelectOK").click(function (e) {
                    if ($("#rdo_DateRight2").attr("checked") == "checked") {
                        var filterText = $("#tbFilterText").val();
                        $("#selDialogError").text("");
                        if (utility.isNull(filterText)) {
                            $("#selDialogError").text("请输入右值");
                            $.colorbox.resize();
                            return;
                        }
                        else {
                            if (dataType == dataTypeList.int[0] || dataType == dataTypeList.dec[0] || dataType == dataTypeList.dec[1] || dataType == dataTypeList.string[0] || dataType == dataTypeList.string[1]) {
                                if (!utility.isNull(regular) && (operator == "=" || operator == "<>") && !filterText.match(regular)) {
                                    $("#selDialogError").text("格式不正确");
                                    $.colorbox.resize();
                                    return;
                                }
                            }

                            if (IsDynamicAlias == 'true') {
                                checkDynamic(e, curDynamicParaCount, fieldalias, me, filterText);
                                return;
                            }
                            else {
                                $(me).text(filterText);
                                $(me).parent().removeAttr("r");
                                $(me).parent().attr("r", filterText);
                            }
                            e.preventDefault();
                            $.colorbox.close();
                        }
                    }
                    else if ($("#rdo_DateRight3").attr("checked") == "checked") {
                        var filterText = $("#selCompareLeft option:selected").text();
                        var filterValue = $("#selCompareLeft option:selected").val();
                        var jsonValue = "";
                        var curTextValue = "";
                        if (!utility.isNull($("#tbFilterNumber3").val()) && $("#tbFilterNumber3").val() != 0) {
                            if (isNaN($("#tbFilterNumber3").val())) {
                                $("#selDialogError").text("数字不正确");
                                $.colorbox.resize();
                                return;
                            }
                            var selNumberAddVal = $(".selNumberAddValue option:selected").val();
                            jsonValue = filterValue + selNumberAddVal + $('#tbFilterNumber3').val();
                            curTextValue = filterText + " " + selNumberAddVal + $('#tbFilterNumber3').val();
                        }
                        if (IsDynamicAlias == 'true') {
                            checkDynamic(e, curDynamicParaCount, fieldalias, me, filterText);
                            return;
                        }
                        else {
                            jsonValue = filterValue;
                            curTextValue = filterText;
                        }

                        $(me).text(curTextValue);
                        $(me).parent().removeAttr("r");
                        $(me).parent().attr("r", jsonValue);
                        e.preventDefault();
                        $.colorbox.close();
                    }
                });
            }
            //日期框
            if ($("#tbFilterDate").length > 0) {
                //$("#tbFilterDate").datepicker();
                $("#btnSelectOK").click(function (e) {
                    if ($("#rdo_DateRight1").attr("checked") == "checked") {
                        var filterDate = $(".dateRight-menu1 option:selected").val();
                        var filterDateText = $(".dateRight-menu1 option:selected").text();
                        if (!utility.isNull(filterDate)) {
                            var jsonDateValue = "";
                            var dateTextValue = "";
                            if (!utility.isNull($("#tbFilterNumber2").val()) && $("#tbFilterNumber2").val() != 0) {
                                var selDateAddVal = $(".selDateAddValue2 option:selected").val();
                                var selDateAddText = $(".selDateAddValue2 option:selected").text();
                                jsonDateValue = "dateadd(" + selDateAddVal + ", " + $('#tbFilterNumber2').val() + ", " + filterDate + ")";
                                dateTextValue = filterDateText + "+ " + $('#tbFilterNumber2').val() + selDateAddText;
                            }
                            else {
                                jsonDateValue = filterDate;
                                dateTextValue = filterDateText;
                            }
                            if (IsDynamicAlias == 'true') {
                                checkDynamic(e, curDynamicParaCount, fieldalias, me, dateTextValue);
                                return;
                            }
                            $(me).text(dateTextValue);
                            $(me).parent().removeAttr("r");
                            $(me).parent().attr("r", jsonDateValue);
                            e.preventDefault();
                            $.colorbox.close();
                        }
                    }
                    else if ($("#rdo_DateRight2").attr("checked") == "checked") {
                        var filterDate = $("#tbFilterDate").val();
                        if (!utility.isNull(filterDate)) {
                            if (!utility.isNull(regular) && (operator == "=" || operator == "<>"))//操作符为等于或不等于的时候验证正则
                            {
                                if (!filterDate.match(regular)) {
                                    $("#selDialogError").text("日期格式不正确");
                                    $.colorbox.resize();
                                    return;
                                }
                            }
                            var jsonDateValue = "";
                            var dateTextValue = "";
                            if (!utility.isNull($("#tbFilterNumber").val()) && $("#tbFilterNumber").val() != 0) {
                                if (isNaN($("#tbFilterNumber").val())) {
                                    $("#selDialogError").text("数字不正确");
                                    $.colorbox.resize();
                                    return;
                                }
                                var selDateAddVal = $(".selDateAddValue option:selected").val();
                                var selDateAddText = $(".selDateAddValue option:selected").text();
                                jsonDateValue = "dateadd(" + selDateAddVal + ", " + $('#tbFilterNumber').val() + ", " + filterDate + ")";
                                dateTextValue = filterDate + "+ " + $('#tbFilterNumber').val() + selDateAddText;
                            }
                            else {
                                jsonDateValue = filterDate;
                                dateTextValue = filterDate;
                            }
                            if (IsDynamicAlias == 'true') {
                                checkDynamic(e, curDynamicParaCount, fieldalias, me, dateTextValue);
                                return;
                            }
                            $(me).text(dateTextValue);
                            $(me).parent().removeAttr("r");
                            $(me).parent().attr("r", jsonDateValue);
                            e.preventDefault();
                            $.colorbox.close();
                        } else {
                            $("#selDialogError").text("日期不正确");
                            $.colorbox.resize();
                            return;
                        }
                    }
                    else if ($("#rdo_DateRight3").attr("checked") == "checked") {
                        var filterDateText = $("#selCompareLeft option:selected").text();
                        var filterDateValue = $("#selCompareLeft option:selected").val();
                        var jsonDateValue = "";
                        var dateTextValue = "";
                        if (!utility.isNull($("#tbFilterNumber3").val()) && $("#tbFilterNumber3").val() != 0) {
                            if (isNaN($("#tbFilterNumber3").val())) {
                                $("#selDialogError").text("数字不正确");
                                $.colorbox.resize();
                                return;
                            }
                            var selDateAddVal = $(".selDateAddValue3 option:selected").val();
                            var selDateAddText = $(".selDateAddValue3 option:selected").text();
                            jsonDateValue = "dateadd(" + selDateAddVal + ", " + $('#tbFilterNumber3').val() + ", " + filterDateValue + ")";
                            dateTextValue = filterDateText + "+ " + $('#tbFilterNumber3').val() + selDateAddText;
                        }
                        else {
                            jsonDateValue = filterDateValue;
                            dateTextValue = filterDateText;
                        }
                        if (IsDynamicAlias == 'true') {
                            checkDynamic(e, curDynamicParaCount, fieldalias, me, dateTextValue);
                            return;
                        }

                        $(me).text(dateTextValue);
                        $(me).parent().removeAttr("r");
                        $(me).parent().attr("r", jsonDateValue);
                        e.preventDefault();
                        $.colorbox.close();
                    }
                });
            }
            //日期时间框
            if ($("#tbFilterTime").length > 0) {
                //$("#tbFilterDate").datepicker();
                //$("#tbFilterTime").timepicker();
                $("#btnSelectOK").click(function (e) {
                    if ($("#rdo_DateRight1").attr("checked") == "checked") {
                        var filterDate = $(".dateRight-menu1 option:selected").val();
                        var filterDateText = $(".dateRight-menu1 option:selected").text();
                        if (!utility.isNull(filterDate)) {
                            var jsonDateValue = "";
                            var dateTextValue = "";
                            if (!utility.isNull($("#tbFilterNumber2").val()) && $("#tbFilterNumber2").val() != 0) {
                                var selDateAddVal = $(".selDateAddValue2 option:selected").val();
                                var selDateAddText = $(".selDateAddValue2 option:selected").text();
                                jsonDateValue = "dateadd(" + selDateAddVal + ", " + $('#tbFilterNumber2').val() + ", " + filterDate + ")";
                                dateTextValue = filterDateText + "+ " + $('#tbFilterNumber2').val() + selDateAddText;
                            }
                            else {
                                jsonDateValue = filterDate;
                                dateTextValue = filterDateText;
                            }
                            if (IsDynamicAlias == 'true') {
                                checkDynamic(e, curDynamicParaCount, fieldalias, me, dateTextValue);
                                return;
                            }

                            $(me).text(dateTextValue);
                            $(me).parent().removeAttr("r");
                            $(me).parent().attr("r", jsonDateValue);
                            e.preventDefault();
                            $.colorbox.close();
                        }
                    }
                    else if ($("#rdo_DateRight2").attr("checked") == "checked") {
                        if (utility.isNull($("#tbFilterDate").val())) {
                            $("#selDialogError").text("日期时间不正确");
                            $.colorbox.resize();
                            return;
                        }
                        var filterDate = $("#tbFilterDate").val() + " " + changeTimeFormat($("#tbFilterTime").val(), 1);
                        if (!utility.isNull(filterDate)) {
                            if (!utility.isNull(regular) && (operator == "=" || operator == "<>"))//操作符为等于或不等于的时候验证正则
                            {
                                if (!filterDate.match(regular)) {
                                    $("#selDialogError").text("日期时间格式不正确");
                                    $.colorbox.resize();
                                    return;
                                }
                            }
                            var jsonDateValue = "";
                            var dateTextValue = "";
                            if (!utility.isNull($("#tbFilterNumber").val()) && $("#tbFilterNumber").val() != 0) {
                                var selDateAddVal = $(".selDateAddValue option:selected").val();
                                var selDateAddText = $(".selDateAddValue option:selected").text();
                                jsonDateValue = "dateadd(" + selDateAddVal + ", " + $('#tbFilterNumber').val() + ", " + filterDate + ")";
                                dateTextValue = filterDate + "+ " + $('#tbFilterNumber').val() + selDateAddText;
                            }
                            else {
                                jsonDateValue = filterDate;
                                dateTextValue = filterDate;
                            }
                            if (IsDynamicAlias == 'true') {
                                checkDynamic(e, curDynamicParaCount, fieldalias, me, dateTextValue);
                                return;
                            }
                            $(me).text(dateTextValue);
                            $(me).parent().removeAttr("r");
                            $(me).parent().attr("r", jsonDateValue);
                            e.preventDefault();
                            $.colorbox.close();
                        } else {
                            $("#selDialogError").text("日期时间不正确");
                            $.colorbox.resize();
                            return;
                        }
                    }
                    else if ($("#rdo_DateRight3").attr("checked") == "checked") {
                        var filterDateText = $("#selCompareLeft option:selected").text();
                        var filterDateValue = $("#selCompareLeft option:selected").val();
                        var jsonDateValue = "";
                        var dateTextValue = "";
                        if (!utility.isNull($("#tbFilterNumber3").val()) && $("#tbFilterNumber3").val() != 0) {
                            if (isNaN($("#tbFilterNumber3").val())) {
                                $("#selDialogError").text("数字不正确");
                                $.colorbox.resize();
                                return;
                            }
                            var selDateAddVal = $(".selDateAddValue3 option:selected").val();
                            var selDateAddText = $(".selDateAddValue3 option:selected").text();
                            jsonDateValue = "dateadd(" + selDateAddVal + ", " + $('#tbFilterNumber3').val() + ", " + filterDateValue + ")";
                            dateTextValue = filterDateText + "+ " + $('#tbFilterNumber3').val() + selDateAddText;
                        }
                        else {
                            jsonDateValue = filterDateValue;
                            dateTextValue = filterDateText;
                        }
                        if (IsDynamicAlias == 'true') {
                            checkDynamic(e, curDynamicParaCount, fieldalias, me, dateTextValue);
                            return;
                        }

                        $(me).text(dateTextValue);
                        $(me).parent().removeAttr("r");
                        $(me).parent().attr("r", jsonDateValue);
                        e.preventDefault();
                        $.colorbox.close();
                    }
                });
            }

            //取消
            $("#btnCancelSelect").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });


        }
    });
    if (IsDynamicAlias == "true") {
        showload();
    }

};

function showload() {
    var top = 0;
    var left = 0;
    top = ($(window).height() - 540) / 2 + 10;
    left = ($(window).width() - 470) / 2;
    var load = " <div id='load' style='height:550px;width: 475px;position: fixed;background-color: #000;opacity: 0.2;top: " + top + "px;left: " + left + "px;line-height:550px;z-index: 90000;'><img src='/img/ajax_loader.gif' alt='' style=' height:10px;width:32px;margin-left:220px'></div>";
    $("#subhead").before(load);
    setTimeout(function () {
        for (var i = 0; i < dateController.length; i++) {
            $("#" + dateController[i]).datepicker();
        }
        for (var i = 0; i < datetimeController.length; i++) {
            $("#" + datetimeController[i].date).datepicker();
            $("#" + datetimeController[i].time).timepicker({
                showMeridian: false,
            });
        }
        $("select[id^=txtPara]").chosen();
    }, 1200)
    //setInterval(function () { },1500)
    setTimeout(function () {
        $("#load").remove();
    }, 2000)

}

//验证动态参数是否符合正则
function checkDynamic(e, curDynamicParaCount, fieldalias, me, filterText) {
    curDynamicValue = "";
    for (var i = 0; i < curDynamicParaCount; i++) {
        var ap = getAliasParameterByParaIndex(fieldalias, i + 1);
        if (ap.IsRequired) {
            if ($("#txtPara" + ap.ParaIndex).val() == "") {
                $("#selDialogError").text(ap.ParameterName + "不能为空");
                $.colorbox.resize();
                return
            }

        }
        if (ap.ControlType == "datetime") {
            curDynamicValue += "|" + $("#txtPara" + ap.ParaIndex).val() + " " + $("#txtPara" + ap.ParaIndex + "time").val();
        }
        else if (ap.ControlType == "input" && ap.Reg != "" && $("#txtPara" + ap.ParaIndex).val() != "") {
            var reg = ap.Reg;
            if (!($("#txtPara" + ap.ParaIndex).val().match(reg))) {
                $("#selDialogError").text("请输入正确的" + ap.ParameterName);
                $.colorbox.resize();
                return;
            }
            curDynamicValue += "|" + ($("#txtPara" + ap.ParaIndex).val() == null ? "" : $("#txtPara" + ap.ParaIndex).val());

        }
        else {
            curDynamicValue += "|" + ($("#txtPara" + ap.ParaIndex).val() == null ? "" : $("#txtPara" + ap.ParaIndex).val());
        }
    }
    $(me).text(filterText);
    $(me).parent().removeAttr("r");
    $(me).parent().attr("r", filterText + curDynamicValue);
    e.preventDefault();
    $.colorbox.close();
}

function getAlias(aliasName) {
    for (i = 0; i < leftValueList.length; i++) {
        if (leftValueList[i].FieldAlias == aliasName) return leftValueList[i];
    }
};

var rightValueHtml = "";
// 获取过滤条件右边值
function getRightValueHtml(controlType, dictTableName, dictTableType, type, fieldalias, tableName, IsDynamicAlias) {
    rightValueHtml = "";
    var compareLeftHtml = "";
    compareLeftHtml = "<select name='selCompareLeft' id='selCompareLeft' data-placeholder='选择左值...' class='chzn_a span6' style='float:left;margin-right:5px'>";
    compareLeftHtml += "<option value=''>空</option>";
    var leftTableLevel = 1;
    if (dictTableList1.indexOf(tableName) >= 0)
        leftTableLevel = 1;
    else if (dictTableList2.indexOf(tableName) >= 0)
        leftTableLevel = 2
    else if (dictTableList3.indexOf(tableName) >= 0)
        leftTableLevel = 3
    leftValueList.forEach(function (item, index, array) {
        var thisTableLevel = 1;
        if (dictTableList1.indexOf(item.TableName) >= 0)
            thisTableLevel = 1;
        else if (dictTableList2.indexOf(item.TableName) >= 0)
            thisTableLevel = 2
        else if (dictTableList3.indexOf(item.TableName) >= 0)
            thisTableLevel = 3
        if (item.FieldAlias != fieldalias && item.ControlType == controlType && item.FieldType == type && thisTableLevel <= leftTableLevel) {
            compareLeftHtml += "<option value='" + item.FieldAlias + "' l='" + item.FieldAlias + "' f='" + item.FieldName + "' datatype='" + item.FieldType
                + "' controltype='" + item.ControlType + "' enumvalue='" + item.EnumValue + "' reg='" + item.Reg + "' fieldName='" + item.FieldDesc
                + "' dictTableName='" + item.DictTableName + "' dictTableType='" + item.DictTableType + "' tableName='" + item.TableName + "' isdynamicalias='" + item.IsDynamicAlias + "'>"
                + item.FieldDesc
                + "</option>";
        }
    });
    compareLeftHtml += "</select>";

    switch (controlType) {
        //单选下拉框
        case "select":
            var arrEnum;
            var valAttr,
                textAttr;

            rightValueHtml = "<input name=\"txtRightValueSearch\" class=\"search_query\" id=\"txtRightValueSearch\" type=\"text\" size=\"16\" placeholder=\"搜索...\" autocomplete=\"off\" />" +
                "<ul name='selRightValue' id='selRightValue' data-placeholder='选择右值...' class='span12 chzn_a'>";

            arrEnum = filterRightVals[fieldalias];
            $.each(arrEnum, function (i, n) {
                textAttr = n["st"];
                valAttr = n["sv"];
                var tid = "radio" + valAttr + i;
                rightValueHtml += "<li itemid='" + valAttr + "'><input id='" + tid + "' type='radio' value='" + valAttr + "' title='" + textAttr + "' name='selEnumGroup' style='float:left;margin-right:5px' /><label for='" + tid + "'>" + textAttr + "</label></li>";

            });

            rightValueHtml += "</ul>";
            break;
        case "selesearch":
            var arrEnum;
            var valAttr,
                textAttr;
            rightValueHtml = "<input name=\"txtRightValueSearch\" class=\"search_query\" id=\"txtRightValueSearch\" type=\"text\" size=\"16\" placeholder=\"搜索...\" autocomplete=\"off\" />" +
                "<select name='selSearchRightValue' multiple = 'multiple' data-placeholder='请选择...' id='selSearchRightValue' data-placeholder='选择右值...' class='span11 chzn_a'>";
            arrEnum = filterRightVals[fieldalias];
            $.each(arrEnum, function (i, n) {
                textAttr = n["st"];
                valAttr = n["sv"];
                rightValueHtml += "<option value='" + valAttr + "'>" + textAttr + "</option>";
            });
            rightValueHtml += "</select>";
            //$("#dvOption").css("overflow-y", "hidden");
            $("#dvOption").css("height", "380px");
            break;
        case "date":
            if (type == dataTypeList.date[1]) {
                rightValueHtml = "<ul><li><input type='radio' id='rdo_DateRight1' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight1'> 特定值 </label>" + strRightDateSelValue;
                rightValueHtml += "+<input type='number' class='span2' id='tbFilterNumber2' />";
                rightValueHtml += "<select id='selDateAddValue2' class='selDateAddValue2 span2'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>"
                rightValueHtml += "<li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设置值 </label>";
                rightValueHtml += "<input type='text' placeholder='选择日期' class='input-small' id='tbFilterDate' readonly='readonly' />";
                rightValueHtml += "<input type='text' placeholder='选择时间' class='ml10 input-small' id='tbFilterTime' />";
                rightValueHtml += "+<input type='number' class='span2' id='tbFilterNumber' />";
                rightValueHtml += "<select id='selDateAddValue' class='selDateAddValue span2'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>";
                rightValueHtml += "</li>";
                rightValueHtml += "<li><input type='radio' id='rdo_DateRight3' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight3'> 选定值 </label>";
                rightValueHtml += compareLeftHtml;
                rightValueHtml += "+<input type='number' class='span2' id='tbFilterNumber3' />";
                rightValueHtml += "<select id='selDateAddValue3' class='selDateAddValue3 span2'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>";
                rightValueHtml += "</li>";
                rightValueHtml += "</ul>";
            } else {
                rightValueHtml = "<ul><li><input type='radio' id='rdo_DateRight1' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight1'> 特定值 </label>" + strRightDateSelValue;
                rightValueHtml += "+<input type='number' class='span2' id='tbFilterNumber2' />";
                rightValueHtml += "<select id='selDateAddValue2' class='selDateAddValue2 span2'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>"
                rightValueHtml += "</li>";
                rightValueHtml += "<li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设置值 </label><input type='text' placeholder='选择日期' class='input-small' id='tbFilterDate' />" +
                    "+<input type='number' class='span2' id='tbFilterNumber' />" +
                    "<select id='selDateAddValue' class='selDateAddValue span2'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>"
                rightValueHtml += "</li>";
                rightValueHtml += "<li><input type='radio' id='rdo_DateRight3' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight3'> 选定值 </label>";
                rightValueHtml += compareLeftHtml;
                rightValueHtml += "+<input type='number' class='span2' id='tbFilterNumber3' />";
                rightValueHtml += "<select id='selDateAddValue3' class='selDateAddValue3 span2'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>";
                rightValueHtml += "</li>";
                rightValueHtml += "</ul>";
            }

            break;
        case "datetime":
            rightValueHtml = "<ul><li><input type='radio' id='rdo_DateRight1' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight1'> 特定值 </label>" + strRightDateSelValue;
            rightValueHtml += "+<input type='number' class='span3' id='tbFilterNumber2' />";
            rightValueHtml += "<select id='selDateAddValue2' class='selDateAddValue2 span3'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>"
            rightValueHtml += "<li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设置值 </label>";
            rightValueHtml += "<input type='text' placeholder='选择日期' class='input-small' id='tbFilterDate' readonly='readonly'/>";
            rightValueHtml += "<input type='text' placeholder='选择时间' class='ml10 input-small' id='tbFilterTime' />";
            rightValueHtml += "+<input type='number' class='span3' id='tbFilterNumber' />";
            rightValueHtml += "<select id='selDateAddValue' class='selDateAddValue span3'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>";
            rightValueHtml += "</li>";
            rightValueHtml += "<li><input type='radio' id='rdo_DateRight3' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight3'> 选定值 </label>";
            rightValueHtml += compareLeftHtml;
            rightValueHtml += "+<input type='number' class='span3' id='tbFilterNumber3' />";
            rightValueHtml += "<select id='selDateAddValue3' class='selDateAddValue3 span3'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>";
            rightValueHtml += "</li>";
            rightValueHtml += "</ul>";
            break;

        case "input":
            //日期型
            if (type == dataTypeList.string[0] || type == dataTypeList.string[1]) {
                rightValueHtml += "<ul><li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设定值 </label>"
                rightValueHtml += "<input type='text' class='span12' id='tbFilterText' />";
                rightValueHtml += "</li>";

                rightValueHtml += "<li><input type='radio' id='rdo_DateRight3' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight3'> 选定值 </label>";
                rightValueHtml += compareLeftHtml;
                rightValueHtml += "</li>";
                rightValueHtml += "</ul>";
            } else {
                rightValueHtml += "<ul><li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设定值 </label>"
                rightValueHtml += "<input type='text' class='span12' id='tbFilterText' />";
                rightValueHtml += "</li>";

                rightValueHtml += "<li><input type='radio' id='rdo_DateRight3' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight3'> 选定值 </label>";
                rightValueHtml += compareLeftHtml;
                rightValueHtml += "<select id='selNumberAddValue' class='selNumberAddValue span2' style='float:left;margin-right:5px'><option value='+' selected='selected'>+</option><option value='*'>*</option></select>";
                rightValueHtml += "<input type='number' class='span2' id='tbFilterNumber3' />";
                rightValueHtml += "</li>";
                rightValueHtml += "</ul>";
            }
            break;
    }
    //if (IsDynamicAlias == 'true') {
    //    rightValueHtml = rightValueHtml.substr(0, rightValueHtml.length - 5);
    //    var f = getAlias(fieldalias);
    //    curDynamicParaCount = f.ParameterCount;
    //    var itemWidth = 0;
    //    var aps = new Array();
    //    for (i = 0; i < f.ParameterCount; i++) {
    //        var ap = getAliasParameter(fieldalias, i + 1);
    //        aps.push(ap);
    //    }
    //    dateController.length = 0;
    //    datetimeController.length = 0;
    //    rightValueHtml += "<li style='width:100%;border-bottom:1px #dcdcdc dashed;margin:5px 0 10px'></li>";
    //    getHTML(aps, aliasParameterOptions);
    //    rightValueHtml += "</ul>";
    //}
};

//生成动态字段的HTML
function getHTML(aps, aliasParameterOptions) {
    var req = "<span class='f_req'>*</span>"
    var span = 3;
    paraHtml += "<li><div class='row-fluid'>";
    var u = new Array();
    for (var i = 0; i < aps.length; i++) {
        var html = "";
        if (0 < span && span < 3) {//生成每行的第二个和第三个控件
            if (aps[i].ControlType == "input" || aps[i].ControlType == "date") {//
                if (aps[i].GroupType != null && span == 1) {
                    if (aps[i - 1].GroupType == aps[i].GroupType) {
                        switch (aps[i].ControlType) {
                            case "input":
                                if (aps[i].flag == "1") {
                                    html = "<div class='span4'><label>" + aps[i].ParameterName + req + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                                } else {
                                    html = "<div class='span4'><label>" + aps[i].ParameterName + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                                }
                                break;
                            case "date":
                                if (aps[i].flag == "1") {
                                    html = "<div class='span4'><label>" + aps[i].ParameterName + req + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";

                                } else {
                                    html = "<div class='span4'><label>" + aps[i].ParameterName + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                                }
                                dateController.push("txtPara" + aps[i].ParaIndex);
                                break;
                        }
                        u.push(i);
                    }
                    span -= 1;
                } else {
                    switch (aps[i].ControlType) {
                        case "input":
                            if (aps[i].flag == "1") {
                                html = "<div class='span4'><label>" + aps[i].ParameterName + req + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                            }
                            else {
                                html = "<div class='span4'><label>" + aps[i].ParameterName + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                            }
                            span -= 1;
                            break;
                        case "date":
                            if (aps[i].flag == "1") {
                                html = "<div class='span4'><label>" + aps[i].ParameterName + req + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                            }
                            else {
                                html = "<div class='span4'><label>" + aps[i].ParameterName + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                            }
                            dateController.push("txtPara" + aps[i].ParaIndex);
                            span -= 1;
                            break;
                    }
                    u.push(i);
                }
            }

        }
        else {//生成每行的第一个控件
            switch (aps[i].ControlType) {
                case "input":
                    if (aps[i].flag == "1") {
                        html = "<div class='span4'><label>" + aps[i].ParameterName + req + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                    }
                    else {
                        html = "<div class='span4'><label>" + aps[i].ParameterName + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                    }
                    span -= 1;

                    break;
                case "select":
                    if (aps[i].flag == "1") {
                        html += "<div class='span12'><label>" + aps[i].ParameterName + req + "</label><select id='txtPara" + aps[i].ParaIndex + "' name='txtPara" + aps[i].ParaIndex + "' style='width:406px'>";
                    } else {
                        html += "<div class='span12'><label>" + aps[i].ParameterName + "</label><select id='txtPara" + aps[i].ParaIndex + "' name='txtPara" + aps[i].ParaIndex + "' style='width:406px'>";
                    }

                    html += "<option value=''>--请选择--</option>";
                    for (var k = 0; k < aliasParameterOptions.length; k++) {
                        if (aliasParameterOptions[k].hasOwnProperty((aps[i].DictTableName + aps[i].DictTableType).replace(",", ""))) {
                            for (var j = 0; j < aliasParameterOptions[k][(aps[i].DictTableName + aps[i].DictTableType).replace(",", "")].length; j++) {
                                html += "<option value='" + aliasParameterOptions[k][(aps[i].DictTableName + aps[i].DictTableType).replace(",", "")][j].sv + "'>" + aliasParameterOptions[k][(aps[i].DictTableName + aps[i].DictTableType).replace(",", "")][j].st + "</option>";
                            }
                        }
                    }
                    html += "</select></div>";
                    span -= 3;

                    break;
                case "mutisearch":
                    if (aps[i].flag == "1") {
                        html += "<div class='span12'><label>" + aps[i].ParameterName + req + "</label><select id='txtPara" + aps[i].ParaIndex + "' name='txtPara" + aps[i].ParaIndex + "' multiple = 'multiple' data-placeholder='请选择...' style='width:406px'>";
                    } else {
                        html += "<div class='span12'><label>" + aps[i].ParameterName + "</label><select id='txtPara" + aps[i].ParaIndex + "' name='txtPara" + aps[i].ParaIndex + "' multiple = 'multiple' data-placeholder='请选择...' style='width:406px'>";
                    }
                    for (var k = 0; k < aliasParameterOptions.length; k++) {
                        if (aliasParameterOptions[k].hasOwnProperty((aps[i].DictTableName + aps[i].DictTableType).replace(",", ""))) {
                            for (var j = 0; j < aliasParameterOptions[k][(aps[i].DictTableName + aps[i].DictTableType).replace(",", "")].length; j++) {
                                html += "<option value='" + aliasParameterOptions[k][(aps[i].DictTableName + aps[i].DictTableType).replace(",", "")][j].sv + "'>" + aliasParameterOptions[k][(aps[i].DictTableName + aps[i].DictTableType).replace(",", "")][j].st + "</option>";
                            }
                        }
                    }
                    html += "</select></div>";
                    span -= 3;

                    break;
                case "date":
                    if (aps[i].flag == "1") {
                        html = "<div class='span4'><label>" + aps[i].ParameterName + req + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";
                    } else {
                        html = "<div class='span4'><label>" + aps[i].ParameterName + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div>";

                    }
                    dateController.push("txtPara" + aps[i].ParaIndex);
                    span -= 1;
                    break;
                case "datetime":
                    if (aps[i].flag == "1") {
                        html = "<div class='span6'><label>" + aps[i].ParameterName + req + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div><div class='span6'><label>&nbsp;</label><p class='span1'>-<p><input id='txtPara" + aps[i].ParaIndex + "time' name = 'txtPara" + aps[i].ParaIndex + "time' type='text' class='span11'></input></div>";
                    } else {
                        html = "<div class='span6'><label>" + aps[i].ParameterName + "</label><input id='txtPara" + aps[i].ParaIndex + "' name = 'txtPara" + aps[i].ParaIndex + "' type='text' class='span12'></input></div><div class='span6'><label>&nbsp;</label><p class='span1'>-<p><input id='txtPara" + aps[i].ParaIndex + "time' name = 'txtPara" + aps[i].ParaIndex + "time' type='text' class='span11'></input></div>";

                    }
                    var datetime = {
                        date: "txtPara" + aps[i].ParaIndex,
                        time: "txtPara" + aps[i].ParaIndex + "time"
                    }
                    datetimeController.push(datetime);
                    span -= 3;
                    break;

            }
            u.push(i);
        }
        paraHtml += html;
        if (span == 0) {
            break;
        }
    }
    paraHtml += "</div></li>";

    for (var i = u.length - 1; i >= 0; i--) {
        aps.splice(u[i], 1);
    }
    if (aps.length > 0) {
        getHTML(aps, aliasParameterOptions);
    }
    else {
        return;
    }
}

function getAliasParameter(alias, index) {
    for (j = 0; j < aliasParameterList.length; j++) {
        if (aliasParameterList[j].FieldAlias == alias && aliasParameterList[j].UIIndex == index) {
            return aliasParameterList[j];
        }
    }
};

function getAliasParameterByParaIndex(alias, index) {
    for (j = 0; j < aliasParameterList.length; j++) {
        if (aliasParameterList[j].FieldAlias == alias && aliasParameterList[j].ParaIndex == index) {
            return aliasParameterList[j];
        }
    }
};

function setRightValueHtml(controlType, type, curRightValue, alias, IsDynamicAlias) {
    var curparavalue = curRightValue;
    if (IsDynamicAlias) {
        if (curRightValue.indexOf("请选择") < 0) {
            curRightValue = curDynamicValue.split("|")[0];
        }
    }
    if (controlType == "select") {
        if (curRightValue.indexOf("请选择") < 0) {
            $("#selRightValue li").each(function (index, item) {
                $(this).find("input[title='" + curRightValue + "']").attr("checked", "checked");
            });
        }
    }
    else if (controlType == "date" || controlType == "datetime") {
        if (curRightValue.indexOf("请选择") >= 0)
            return false;
        var dateStr = "", addText = "", addNumber = "", dateSelValue = "";
        if (curRightValue.indexOf("+") >= 0) {
            dateStr = curRightValue.substr(0, curRightValue.indexOf("+"));
            addText = curRightValue.substr(curRightValue.length - 1, 1);
            addNumber = curRightValue.substr(curRightValue.indexOf("+") + 1, curRightValue.length - curRightValue.indexOf("+") - 2);
        }
        else {
            dateStr = curRightValue;
        }
        if (dateStr != "") {
            if (rightDateSelObj != null) {
                for (var i = 0; i < rightDateSelObj.length; i++) {
                    if (rightDateSelObj[i].OptionText == dateStr)
                        dateSelValue = rightDateSelObj[i].OptionValue;
                }
            }
        }
        if (dateSelValue != "") {
            $("#rdo_DateRight1").attr("checked", "checked");
            setSelect("dateRight-menu1", dateSelValue);
            $("#tbFilterNumber2").val(addNumber);
            setSelectText("selDateAddValue2", addText);

            $("#selCompareLeft").attr("disabled", "disabled");
            $("#tbFilterNumber3").attr("disabled", "disabled");
            $("#selDateAddValue3").attr("disabled", "disabled");
            $("#tbFilterDate").attr("disabled", "disabled")
            $("#tbFilterNumber").attr("disabled", "disabled");
            $(".selDateAddValue").attr("disabled", "disabled");
            $("#tbFilterTime").attr("disabled", "disabled");

            $(".dateRight-menu1").removeAttr("disabled")
            $("#tbFilterNumber2").removeAttr("disabled");
            $(".selDateAddValue2").removeAttr("disabled");
        }
        else {
            var isCompareLeft = false;
            leftValueList.forEach(function (item, index, array) {
                if (item.FieldDesc == dateStr) {
                    isCompareLeft = true;
                }
            });
            if (isCompareLeft) {
                $("#rdo_DateRight3").attr("checked", "checked");
                setSelectText("selCompareLeft", dateStr);

                $("#selCompareLeft").removeAttr("disabled");
                $("#tbFilterNumber3").removeAttr("disabled");
                $("#selDateAddValue3").removeAttr("disabled");
                $("#tbFilterDate").attr("disabled", "disabled")
                $("#tbFilterNumber").attr("disabled", "disabled");
                $(".selDateAddValue").attr("disabled", "disabled");
                $("#tbFilterTime").attr("disabled", "disabled");
                $(".dateRight-menu1").attr("disabled", "disabled")
                $("#tbFilterNumber2").attr("disabled", "disabled");
                $(".selDateAddValue2").attr("disabled", "disabled");

                $("#tbFilterNumber3").val(addNumber);
                setSelectText("selDateAddValue3", addText);
            }
            else {
                $("#rdo_DateRight2").attr("checked", "checked");

                $("#selCompareLeft").attr("disabled", "disabled");
                $("#tbFilterNumber3").attr("disabled", "disabled");
                $("#selDateAddValue3").attr("disabled", "disabled");
                $("#tbFilterDate").removeAttr("disabled")
                $("#tbFilterNumber").removeAttr("disabled");
                $(".selDateAddValue").removeAttr("disabled");
                $("#tbFilterTime").removeAttr("disabled");

                $(".dateRight-menu1").attr("disabled", "disabled")
                $("#tbFilterNumber2").attr("disabled", "disabled");
                $(".selDateAddValue2").attr("disabled", "disabled");

                if (type == dataTypeList.date[1]) {
                    $("#tbFilterDate").val(dateStr.substr(0, 10));
                    $("#tbFilterTime").val(changeTimeFormat(dateStr.substr(10, dateStr.length - 10), 2));

                    //$("#tbFilterTime").val(dateStr.substr(10, dateStr.length - 10));
                    $("#tbFilterNumber").val(addNumber);
                    //$(".selDateAddValue").text(addText);
                    setSelectText("selDateAddValue", addText);
                }
                else {
                    $("#tbFilterDate").val(dateStr);
                    $("#tbFilterNumber").val(addNumber);
                    setSelectText("selDateAddValue", addText);
                }
            }
        }
    }
    else if (controlType == "input") {
        if (curRightValue.indexOf("请选择") < 0) {
            var filterText = curRightValue;

            if (type == dataTypeList.int[0] || type == dataTypeList.dec[0] || type == dataTypeList.dec[1]) {
                var addText = "", addNumber = "";
                if (curRightValue.indexOf("+") >= 0) {
                    addText = "+";
                    filterText = curRightValue.substr(0, curRightValue.indexOf("+")).trim();
                    addNumber = curRightValue.substr(curRightValue.indexOf("+") + 1, curRightValue.length - curRightValue.indexOf("+") - 1);
                }
                if (curRightValue.indexOf("*") >= 0) {
                    addText = "*";
                    filterText = curRightValue.substr(0, curRightValue.indexOf("*")).trim();
                    addNumber = curRightValue.substr(curRightValue.indexOf("*") + 1, curRightValue.length - curRightValue.indexOf("+") - 1);
                }
                $("#tbFilterNumber3").val(addNumber);
                setSelectText("selNumberAddValue", addText);
            }

            var isCompareLeft = false;
            leftValueList.forEach(function (item, index, array) {
                if (item.FieldDesc == filterText) {
                    isCompareLeft = true;
                }
            });
            if (isCompareLeft || curRightValue == "空") {
                $("#rdo_DateRight3").attr("checked", "checked");
                $("#selCompareLeft").removeAttr("disabled");
                $("#tbFilterNumber3").removeAttr("disabled");
                $("#selNumberAddValue").removeAttr("disabled");
                $("#tbFilterText").attr("disabled", "disabled");

                setSelectText("selCompareLeft", filterText);
            }
            else {
                $("#rdo_DateRight2").attr("checked", "checked");
                $("#tbFilterText").val(curRightValue);
                $("#tbFilterText").removeAttr("disabled");
                $("#selCompareLeft").attr("disabled", "disabled");
                $("#selNumberAddValue").attr("disabled", "disabled");
                $("#tbFilterNumber3").attr("disabled", "disabled");
            }
        }
    }
    if (IsDynamicAlias == 'true') {//动态字段赋值
        if (curparavalue.indexOf("请选择") < 0) {
            setTimeout(function () {
                var filterText = curparavalue;
                var arr = curDynamicValue.split('|');
                for (i = 0; i < arr.length - 1; i++) {
                    var fp = getAliasParameterByParaIndex(alias, i + 1);

                    switch (fp.ControlType) {
                        case "mutisearch":
                            $("#txtPara" + fp.ParaIndex).val(arr[i + 1].split(',')).trigger("liszt:updated");
                            break;
                        case "select":
                            $("#txtPara" + fp.ParaIndex).val(arr[i + 1].split(',')).trigger("liszt:updated");
                            break;
                        case "date":
                            $("#txtPara" + fp.ParaIndex).val(arr[i + 1]);
                            break;
                        case "datetime":
                            var datetime = arr[i + 1].split(' ')
                            $("#txtPara" + fp.ParaIndex).val(datetime[0]);
                            $("#txtPara" + fp.ParaIndex + "time").val(datetime[1]);
                            break;
                        default:
                            $("#txtPara" + fp.ParaIndex).val(arr[i + 1]);
                            break;
                    }
                }
                $("#tbFilterText").val(curRightValue);
                $("#tbFilterText").removeAttr("disabled");
                $("#selCompareLeft").attr("disabled", "disabled");
            }, 3000)
        }
    }
}

// 获取过滤条件右值
function getFilterRightValues(rightValCfg) {
    var postData = { "rightValCfgs": JSON.stringify(rightValCfg) },
        postUrl = "/MemSubdivision/GetMemSubdRightValues";
    ajaxSync(postUrl, postData, getFilterRightValueCallback);
}

function getFilterRightValueCallback(data) {
    if (data) {
        filterRightVals = eval("(" + data.Data + ")");
    }
}

// 把细分规则拼成json字符串
function getFilterJson() {
    if (checkFilter()) {
        /*var filter = { r: "", rfl: null };
        var rootFilter = { r: "", srfl: null };
        var subRootFilter = { l: "", e: "", r: "" };*/
        var result = true;
        var arrRootFilter = new Array();
        $(".sub-rootreletion").each(function (i, val) {
            var arrSubRootFilter = new Array();
            if ($(val).find("#ulFilter li").length > 0) {
                $(val).find("#ulFilter li").each(function (i, subVal) {
                    var lAttr = $(subVal).attr("l");
                    var eAttr = $(subVal).attr("e");
                    var rAttr = $(subVal).attr("r");
                    var subRootFilter = { l: lAttr, e: eAttr, r: rAttr };
                    arrSubRootFilter.push(subRootFilter);
                });
                var subRootFilter = { r: $(val).attr("r"), srfl: arrSubRootFilter };
                arrRootFilter.push(subRootFilter);
            } else {
                $.dialog("逻辑节点下必须有细分过滤规则!");
                result = false;
                return false;
            }
        });
        if (utility.isTrue(result)) {
            if (!utility.isNull(arrRootFilter)) {
                var filter = { r: $(".root-reletion").attr("r"), rfl: arrRootFilter };
                return JSON.stringify(filter);
            }
            else {
                return "";
            }
        }
        else {
            return "notsave";
        }
    }
    else {
        return "notsave";
    }
}

//会员细分规则数据验证重写 -----------------------------20151016
function checkFilter() {
    var setFlag = true;
    $(".filter-item").each(function (i, val) {
        var fieldType = $(val).attr("datatype"),
            fieldName = $(val).attr("fieldName"),
            leftname = $(val).attr("l"),
            opername = $(val).attr("e"),
            rightvalue = $(val).attr("r");

        //动态维度取制表符前第一个值
        var fa = getAlias(leftname);
        if (fa.IsDynamicAlias) {
            rightvalue = rightvalue.split('|')[0];
        }

        if (utility.isNull(leftname)) {
            setFlag = false;
            $.dialog("请选择左值!");
            return false;
        }
        if (utility.isNull(opername)) {
            setFlag = false;
            $.dialog("请选择操作符!");
            return false;
        }

        switch (fieldType) {
            case "1"://字符串(20)
                if (utility.isNull(rightvalue)) {
                    //$(val).attr("r", " ");
                    $(val).attr("r", "");
                    break;
                } else {
                    if (rightvalue.length > 20) {
                        setFlag = false;
                        $.dialog("会员细分规则中配置项'" + fieldName + "'的值最大长度为20!");
                        return false;
                    }
                }
            case "2"://字符串(100)
                if (utility.isNull(rightvalue)) {
                    //$(val).attr("r", " ");
                    $(val).attr("r", "");
                    break;
                } else {
                    if (rightvalue.length > 100) {
                        setFlag = false;
                        $.dialog("会员细分规则中配置项'" + fieldName + "'的值最大长度为100!");
                        return false;
                    }
                    break;
                }
            case "3": //整型
                if (utility.isNull(rightvalue)) {
                    setFlag = false;
                    $.dialog("会员细分规则中配置项'" + fieldName + "'的值不能为空!");
                    return false;
                }

                if (!utility.isNumber(rightvalue)) {
                    setFlag = false;
                    $.dialog("会员细分规则中配置项'" + fieldName + "'的值格式错误!");
                    return false;
                }
                break;
            case "4": //布尔型
            case "5": //日期型(长)
            case "6": //日期型(短)
                if (utility.isNull(rightvalue)) {
                    setFlag = false;
                    $.dialog("会员细分规则中配置项'" + fieldName + "'的值不能为空!");
                    return false;
                }
                break;
            case "7"://十进制(2位小数)
            case "8"://十进制(4位小数)
                if (utility.isNull(rightvalue)) {
                    setFlag = false;
                    $.dialog("会员细分规则中配置项'" + fieldName + "'的值不能为空!");
                    return false;
                }
                if (!utility.isNumber(rightvalue)) {
                    setFlag = false;
                    $.dialog("会员细分规则中配置项'" + fieldName + "'的值格式错误!");
                    return false;
                }
                break;
            default:
                if (utility.isNull($(val).attr("l")) || utility.isNull($(val).attr("e")) || utility.isNull($(val).attr("r"))) {
                    setFlag = false;
                    $.dialog("会员细分规则中配置项'" + fieldName + "'的值不能为空!");
                    return false;
                }
                break;
        }

    });
    return setFlag;
}
/*---------------------------------细分规则相关-------------------------------------*/

function insertSubdivisionType() {
    var postUrl = "/BaseData/InsertSysClass",
        classInfo = {
            ClassName: $("#txtAddSubdivisionType").val(),
            ClassType: "1",
            Sort: 1
        };

    if (utility.isNull($("#txtAddSubdivisionType").val())) {
        $.dialog("请填写细分类型名称");
        return;
    }

    ajax(postUrl, { "classInfo": JSON.stringify(classInfo) }, function (result) {
        if (utility.isNull(result)) {
            $.dialog("新增失败");
            return;
        } else if (!result.IsPass) {
            $.dialog(result.MSG);
            return;
        } else {
            bindSubdivisionType();
            refreshTree();
        }
    });
}



function redirectToMemberDetail(memberid) {
    window.open("/member360/MemberDetail?mid=" + memberid);
}

function disableForm(id) {
    $("#" + id + " :input").attr("disabled", "disabled");
    $("#selSubType").removeAttr("disabled");
}

function activeForm(id) {
    $("#" + id + " :input").removeAttr("disabled");
}

// 重置表单---数据初始化
function resetForm() {
    $("#hidSID").val('');
    $("#hidCurSubdInstID").val('');
    $(".help-block").html('');
    $(".form_validation_reg #txtName, #txtActive, #txtComputeTime, #txtLastCompute, #txtComputeResult, #txtDesc, #txtDate, #txtTime, #txtCycleTime").val("");
    $("input[name='radSchedule']").removeAttr("checked");
    $("#radScheduleNow").attr("checked", "checked");
    $("#btnRefreshCurrentInfo").attr("disabled", "disabled");
    $("#btnActive,#btnInactive").attr("disabled", "disabled");
    $("#btnSave, #btnSaveFilter").text("新增").removeAttr("disabled");
    $("#ulSubRootReletion").html("");
    //if (dtFilterResult) {
    //    var oTable = $('#dtFilter').dataTable();
    //    oTable.fnClearTable();
    //    //dtFilterResult.fnClearTable();
    //}
    //if (dtActivity)
    //    dtActivity.fnClearTable();
    setSelect("selGroup", 1);
    $("#selSubType").val("type1");
    $("#selSubType").trigger("liszt:updated");
    $("#selSubType").removeAttr("disabled");
    setSelect("selDataSubType", "1");
    $("#tab2 .form_validation_reg").show();
    $("#hyRoot").html("并且");
    //$("#span_tipMsg").html("");
}

function resetFormByID(id) {
    $("#" + id + " :input").not(":button, :submit, :reset").val("").removeAttr("checked").removeAttr("selected");
    $("#" + id + " .error").removeClass('error');
    $("#" + id + " .help-block").html('')
}

function initPageVisiable() {  //控件的隐藏显示
    $("#liChart").hide();
    $("#litab3").hide();
    $("#aTab1").click();
    $("#selDataSubType").removeAttr("disabled");
    $("#ms_tree li").each(function () {
        $(this).children("span").removeClass("dynatree-active");
    })
    $("#btnResultImport").hide();
    $("#btnDownTemplate").hide();
}

function initPageControlsDisabled() {//控件的是否可用性控制
    activeForm("tab1");
    activeForm("tab2");
    $("#txtActive").attr("disabled", "disabled");
    $("#txtComputeTime").attr("disabled", "disabled");
    $("#txtLastCompute").attr("disabled", "disabled");
}

//加载细分结果导出框的下拉框
function loadSubExportSelect()
{
    $('#addSubExportCol').empty();
    ajax('/MemSubdivision/loadSubExportSelect', null, function (res)
    {
        if (res.length > 0)
        {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++)
            {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#addSubExportCol').append(opt);
        } else
        {
            var opt = "<option value=''>无</option>";
            $('#addSubExportCol').append(opt);
        }
    });
}
//加载细分结果导出框的表格
function loadSubExportTable()
{
    dtSubExport = $('#dtSubExport').dataTable({
        sAjaxSource: '/MemSubdivision/loadSubExportTable',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "编号", sClass: "center", sortable: false },
            { data: 'Code', title: "代码", sClass: "center", sortable: false },
            { data: 'Name', title: "名称", sClass: "center", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj)
                {
                    return "<button type='button' class=\"btn btn-danger\" onclick=\"deleteSubExportCol('" + obj.ID + "');\">删除</button>";
                }
            }
        ],
        fnFixData: function (d)
        {
            d.push({ name: 'SubdivisionID', value: $("#hidSID").val() });
        }
    });
}
//删除细分结果导出框的表格数据
function deleteSubExportCol(id)
{
    $.dialog("确定要删除吗？", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function ()
    {
        ajaxSync("/MemSubdivision/deleteSubExportCol", { ID: id }, function (res)
        {
            if (res.IsPass)
            {
                $.dialog(res.MSG);
                dtSubExport.fnDraw();
            } else
            { $.dialog(res.MSG); }
        });
    });
}
