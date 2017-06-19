//变量
var dictTableList1 = ["TM_Mem_Master", "TM_Mem_Ext", "TM_Loy_MemExt"];
var dictTableList2 = ["TM_Mem_Trade", "TM_Mem_SubExt"];
var dictTableList3 = ["TM_Mem_TradeDetail"];
var masterData;
var actions = new Array();
var actionsResult = new Array();
var leftValueList;//规则左值取值
var leftValueHtml;//左值Html
var aliasHtml;
var tr;//编辑的行
var trNum;

var filterRightVals;
var ModifidAct;
var ModifiActResult;
var NameCode;//计算规则别名
var dataTypeList = { int: ["3"], dec: ["7", "8"], string: ["1", "2"], date: ["5", "6"], bool: ["4"] };

var strRightDateSelValue = "";//规则右值时间取值字典值
var rightDateSelObj = new Array();//右值时间下拉框取值

var numOperatorHtml = "";//数字关系Html
var strOperatorHtml = "";//字符关系Html

var operatorObj = [];//关系符号

var numActionOperatorHtml = "<select id='selOp' class='span12'>" +
                         "<option value='+='>累加(减)</option>" +
                         "<option value='='>设置为</option></select>";
var strActionOperatorHtml = "<select id='selOp' class='span12'>" +
                         "<option value='='>设置为</option></select>";
var strActTypeOperatorHtml = "<select id='selActType' class='span12'>" +
                         "<option value=''>请选择</option><option value='1'>现金</option><option value='2'>积点</option><option value='3'>积分</option></select>";
var strActLimitOperatorHtml = "<select id='selActLimit'  multiple data-placeholder='请选择...' class='chzn_b span12' style='width:166px'></select>";
//"<option value='vehicle'>车辆</option><option value='store'>门店</option><option value='brand'>品牌</option></select>";
var strActionOffSetErrHtml = "<select id='selOffset' class='span12'>" +
                         "<option value='+'>加(减)</option>" +
                         "<option value='*'>乘以</option></select>";
//页面初始
$(function () {
    /****************************** main start ***********************************/
    $.ajaxSetup({
        async: false
    });
    //注册保存按钮
    $("#ActionForm").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            saveAction();
        }
        else {
            tabActive(1);
        }
    });

    if ($("#txtRuleID").val() == "") {
        $("#btnActive,#btnActive1").hide();
    };

    var executeStatus = $("#cbExecuteStatus").val();
    if (executeStatus == '休眠中') {
        $('#btnActive').prop('disabled', false).prop('value', 1).text('唤醒');
    }
    else
        $('#btnActive').prop('disabled', false).prop('value', 0).text('休眠');


    //执行状态
    $("#btnActive").click(function (e) {
        var isWakeup = $(this).prop('value');

        var postUrl = "/MemberSalesPromotion/ActiveRuleById";
        ajax(postUrl, { "ruleId": $("#txtRuleID").val(), "IsWakeUp": isWakeup == 1 }, ActiveRuleCallBack);
    });

    var approveStatus = $('#cbApproveStatus').val();
    if (approveStatus == '未审核') {
        $('#btnApprove').prop('disabled', false).prop('value', 1).text('审核');
    }
    else if (approveStatus == "已审核") {
        $('#btnApprove').prop('disabled', false).prop('value', 2).text('作废');
    }
    else
        $('#btnApprove').prop('disabled', true).prop('value', 1).text('审核');

    //审核
    $("#btnApprove").click(function (e) {
        var status = $(this).prop('value');
        var postUrl = "/MemberSalesPromotion/ApproveRuleById";
        ajax(postUrl, { "ruleId": $("#txtRuleID").val(), "active": status }, ActiveRuleCallBack1);
    });

    //返回
    //$("#btnReturn").click(function (e) {
    //    window.location.href = "/Loyalty/Rule";
    //});

    //$("#selPromotionType").click(function () {
    //    var type = $("#selPromotionType").val();
    //    if (type == "1") {
    //        $("#rdCycle").prop("checked", false).prop("disabled", true);
    //        $("#rdRealTime").prop("checked", true).prop("disabled", true);
    //        $("#rdRealTime").change();
    //    } else {
    //        $("#rdCycle").prop("checked", true).prop("disabled", false);
    //        $("#rdRealTime").prop("checked", false).prop("disabled", false);
    //        $("#rdCycle").change();
    //    }
    //})

    /****************************** main end ***********************************/
    /****************************** tab1 start ***********************************/
    //加载日期控件
    $("#txtStartDate,#txtEndDate").datepicker();
    //加载时间插件
    $("#txtTime").timepicker();
    //注册周期类型选择事件
    $("#rdCycle,#rdRealTime").change(function () {
        cycleChanged(this);
    });
    //注册日类型选择事件
    $("#rd1st,#rdlst,#rdday").change(function () {
        dayChanged(this);
    });
    ////周期子类型改变事件
    //$("#selScheduleSubType").change(function () {
    //    subCycleChanged();
    //});
    //规则类型改变事件
    $("#selPromotionType").change(function () {
        //  ruleTypeChanged();
    });


    //加载调度
    if ($("#hideSchedule").val() != "") {
        var str = $("#hideSchedule").val();
        var s = eval("(" + str + ")");
        var rdType = s.Type == "cycle" ? $("#rdCycle").attr("checked", true)[0] : $("#rdRealTime").attr("checked", true)[0];
        //rdType.attr("checked", true);
        cycleChanged(rdType);
        $("#selScheduleSubType").val(s.SubType);
        subCycleChanged();
        if (s.Date == "1st") { $("#rd1st,#rdlst,#rdday").attr("checked", false); $("#rd1st").attr("checked", true); }
        else if (s.Date == "lst") { $("#rd1st,#rdlst,#rdday").attr("checked", false); $("#rdlst").attr("checked", true); }
        else if (s.Date != "") {
            $("#rd1st,#rdlst,#rdday").attr("checked", false);
            $("#rdday").attr("checked", true);
            if (s.SubType == "week") {
                $("#selWeekCycle").val(s.Date);
            }
            else {
                $("#selMonthCycle").val(s.Date);
            }
        }
        $("#txtTime").val(s.Time);
        $("#txtRemark").val(s.Remark);
    }
    else {
        $("#txtTime").val("11:00 PM");
    };

    /****************************** tab1 end ***********************************/

    /****************************** tab2 start ***********************************/

    //日期右值下拉框取值
    getDateSelValue("SysDateValue");
    //根节点+号单击事件
    $("#hyRoot").bind("click", function () {
        showRootReletion(this);
    });

    $("#hyAddSubRoot").bind("click", function () {
        addSubRootReletion();
    });

    $("#hySubRoot").live("click", function () {
        showRootReletion(this);
    });

    $("#hyDelSubReletion").live("click", function () {
        deleteSubReletion(this);
    });

    $("#hyAddFilter").live("click", function () {
        addFilter(this);
    });

    $("#hyLeftValue").live("click", function () {
        showLeftValueMenu(this);
    });

    $("#hyDelFilter").live("click", function () {
        deleteFilter(this);
    });

    $("#hyOperation").live("click", function () {
        selectOperator(this);
    });

    $("#hyRightValue").live("click", function () {
        selectRightValue(this);
    });

    /****************************** tab2 end ***********************************/


    /****************************** tab3 end ***********************************/
    //获取字段别名列表
    LoadAliasKeyList();
    //$(".chzn_a").chosen();
    //字段关键字改变，名称也改变
    $("#txtKey").change(function () {
        var key = $("#txtKey").val();
        var keyCode = $("#txtKey").find('option:selected').text();
        if (key != '') {
            NameCode = key + '_' + Math.round((Math.random() * (99999 - 10000) + 10000));
            $("#txtNameCode").val(NameCode);
        } else {
            $("#txtNameCode").val('');
        }
    });
});

/****************************** main start ***********************************/
function ActiveRuleCallBack(data) {
    $.dialog(data.MSG, {
        footer: {
            closebtn: '确认',
        }
    });
    if (data.IsPass) {
        $("#cbEnable").val("已激活");
        $('#btnActive').prop('disabled', true);
        $('#btnActive1').prop('disabled', false);
    };
}

function ActiveRuleCallBack1(data) {
    $.dialog(data.MSG, {
        footer: {
            closebtn: '确认',
        }
    });
    if (data.IsPass) {
        $("#cbEnable").val("未激活");
        $('#btnActive').prop('disabled', false);
        $('#btnActive1').prop('disabled', true);
    };
}

//新增时验证数据
var DataValidator = $("#ActionForm").validate({
    //onSubmit: false,
    rules: {
        selPromotionType: {
            required: true,
        },
        txtRunIndex: {
            required: true,
            number: true,
        },
        txtStartDate: {
            required: true,
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存
function saveAction() {
    if (actions.length == 0) {
        $.dialog("请配置动作", {
            footer: {
                closebtn: '确认'
            }
        });
        tabActive(3);
        return false;
    }
    var condition = getFilterJson();
    if (condition == "notsave") {
        return false;
    }
    var ruleMaster = {
        BillCode: encode($("#txtBillCode").val()),
        RuleID: $("#txtRuleID").val(),
        RuleType: $("#selPromotionType").val(),
        RunIndex: $("#txtRunIndex").val(),
        Condition: condition,
        ConditionResult: JSON.stringify(actionsResult),
        Actions: JSON.stringify(actions),
        DataGroupID: $("#selDataGroup").val(),
        StartDate: $("#txtStartDate").val(),
        EndDate: $("#txtEndDate").val(),
        ScheduleType: $("#rdRealTime").attr("checked") ? "realtime" : "cycle",
        ScheduleSubType: $("#selScheduleSubType").val(),
        ScheduleDay: getScheduleDay(),
        ScheduleTime: $("#txtTime").val(),
        Remark: $("#txtRemark").val(),
        LastExecTime: $("#txtLastExecTime").val()
    };
    if (ruleMaster.EndDate != '') {
        if (!utility.compareDate(ruleMaster.StartDate, ruleMaster.EndDate)) {
            $.dialog("起始时间不能大于结束时间");
            return;
        }
    }
    if (ruleMaster.RuleType == '') {
        $.dialog("规则类型不能为空");
        return;
    }
    if (ruleMaster.RunIndex == '') {
        $.dialog("优先级不能为空");
        return;
    }
    if (ruleMaster.StartDate == '') {
        $.dialog("开始时间不能为空");
        return;
    }
    if (ruleMaster.DataGroupID == '') {
        $.dialog("请选择数据群组");
        return;
    }
    var postUrl = "/Loyalty/SaveRule";
    ajax(postUrl, { "ruleMaster": ruleMaster }, saveActionCallBack);
};

function saveActionCallBack(data) {
    if (data.IsPass) {
        $("#txtRuleID").val(data.Obj[0]);
    };
    $.dialog(data.MSG);
};

//获取调度日
function getScheduleDay() {
    if ($("#selScheduleSubType").val() != "week") {
        if ($("#rd1st").attr("checked")) return "1st";
        if ($("#rdlst").attr("checked")) return "lst";
        if ($("#rdday").attr("checked")) return $("#selMonthCycle").val();
    }
    else {
        return $("#selWeekCycle").val();
    }
}

//激活选项卡
function tabActive(index) {
    $("#li1,#li2,#li3,#tab1,#tab2,#tab3").removeClass("active");
    $("#li" + index + ",#tab" + index).addClass("active");
};

// 把细分规则拼成json字符串
function getFilterJson() {
    if (checkFilter()) {
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
            }
            else {
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

function checkFilter() {
    var setFlag = true;
    $(".filter-item").each(function (i, val) {
        if (utility.isNull($(val).attr("l")) || utility.isNull($(val).attr("e"))) {// || utility.isNull($(val).attr("r"))
            setFlag = false;
            $.dialog("请把所有的细分规则设置完成!");
            return false;
        }
    });
    return setFlag;
}
/****************************** main end ***********************************/

/****************************** tab1 start ***********************************/


//规则类型改变事件
function cycleChanged(rd) {
    //反选
    var choice = $('#rdCycle,#rdRealTime').not("#" + rd.id).attr("checked", false);
    if ($(rd).val() == "cycle") {
        $("#divCycle").show();
    }
    else {
        $("#divCycle").hide();
    }
};

//日类型改变事件
function dayChanged(rd) {
    //反选
    var choice = $("#rd1st,#rdlst,#rdday").not("#" + rd.id).attr("checked", false);

};

//周期子类型改变事件
function subCycleChanged() {
    switch ($("#selScheduleSubType").val()) {
        case "day":
            $("#divWeek").hide();
            $("#divDay").hide();
            $("#divMonth").hide();
            break;
        case "week":
            $("#divWeek").show();
            $("#divDay").hide();
            $("#divMonth").hide();
            break;
        case "month":
            $("#divWeek").hide();
            $("#divDay").show();
            $("#divMonth").show();
            break;
        case "year":
            $("#divWeek").hide();
            $("#divDay").show();
            $("#divMonth").hide();
            break;
    }
};
/****************************** tab1 end ***********************************/

/****************************** tab2 start ***********************************/
//增加子关系菜单
function addSubRootReletion() {
    $("#ulSubRootReletion").append("<li class='sub-rootreletion' r='and'><span class='dynatree-connector'></span><i class='splashy-diamonds_1'></i>&nbsp;<a id='hySubRoot' href='#'>并且</a>&nbsp;<a id='hyAddFilter' href='#'><i class='splashy-add_small'></i></a>&nbsp;<a id='hyDelSubReletion' href='#'><i class='splashy-gem_remove'></i></a><ul id='ulFilter'></ul></li>");
    setLastRootClass();
}

//删除子关系菜单
function deleteSubReletion(me) {
    deleteParentLi(me);
}

//新增规则
function addFilter(me) {
    $(me).parent().find("#ulFilter").append("<li class='filter-item'><span class='dynatree-connector'></span><a id='hyLeftValue' href='#'>选择左值</a>&nbsp;<a id='hyOperation' href='#'>选择操作符</a>&nbsp;<a id='hyRightValue' href='#'>选择右值</a>&nbsp;<i class='splashy-menu'></i>&nbsp;<a id='hyDelFilter' href='#'><i class='splashy-gem_remove'></i></a></li>");
    setLastRootClass();
}

//删除规则
function deleteFilter(me) {
    deleteParentLi(me);
}

function deleteParentLi(me) {
    $(me).parent().remove();
    setLastRootClass();
}

function setLastRootClass() {
    $(".sub-rootreletion").removeClass("lastNode");
    $(".sub-rootreletion").last().addClass("lastNode");
    $(".sub-rootreletion .filter-item").removeClass("lastNode");
    $(".sub-rootreletion .filter-item:last-child").addClass("lastNode");
}

// 选择右值
function selectRightValue(me) {
    $("#btnSelectOK").unbind();
    $("#btnDelTemplet").hide();
    var dialogID = "select_dialog";
    var dataType = $(me).parent().attr("datatype");
    if (utility.isNull(dataType)) {
        $.dialog("请先选择左值！");
        return;
    }

    var operator = $(me).parent().attr("e");
    if (utility.isNull(operator)) {
        $.dialog("请先选择操作符！");
        return;
    }
    var fieldalias = $(me).parent().attr("l");
    var controlType = $(me).parent().attr("controltype");
    var enumvalue = $(me).parent().attr("enumvalue");
    var enumobj = eval("(" + enumvalue + ")");
    var type = $(me).parent().attr("datatype");
    var regular = $(me).parent().attr("reg");
    if (regular != "null") {
        regular = new RegExp(regular);
    }
    var tableName = $(me).parent().attr("TableName");
    var dictTableName = $(me).parent().attr("dictTableName");
    var dictTableType = $(me).parent().attr("dictTableType");
    $("#selectDialogTitle").text("选择右值");
    $("#selDialogError").text("");
    var curRightValue = $(me).html();
    var rightValueHtml = getRightValueHtml(controlType, dictTableName, dictTableType, type, fieldalias, tableName);
    $("#dvOption").css("min-height", "100px");
    $("#dvOption").html(rightValueHtml);
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
    setRightValueHtml(controlType, type, curRightValue);

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
                        if (enumobj) {
                            if (enumobj.Type == 'Vehicle') {
                                get5RelatedCarModel(k);
                            } else if (enumobj.Type == 'Corp') {
                                get5RelatedCorp(k);
                            }
                        }
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
                    else {
                        $(me).text($(selObj).text());
                        $(me).parent().removeAttr("r");
                        $(me).parent().attr("r", $(selObj).val());
                        e.preventDefault();
                        $.colorbox.close();
                    }
                });
            }
            //输入框
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
                            if (dataType == dataTypeList.int || dataType == dataTypeList.dec[0] || dataType == dataTypeList.dec[1] || dataType == dataTypeList.string[0] || dataType == dataTypeList.string[1]) {
                                if (operator == "=" && !utility.isNull(regular) && !filterText.match(regular)) {
                                    $("#selDialogError").text("格式不正确");
                                    $.colorbox.resize();
                                    return;
                                }
                            }
                            $(me).text(filterText);
                            $(me).parent().removeAttr("r");
                            $(me).parent().attr("r", filterText);
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
                        if (!utility.isNull(filterDate) && (utility.isNull(regular) || filterDate.match(regular))) {
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

                            $(me).text(dateTextValue);
                            $(me).parent().removeAttr("r");
                            $(me).parent().attr("r", jsonDateValue);
                            e.preventDefault();
                            $.colorbox.close();
                        }
                    }
                    else if ($("#rdo_DateRight2").attr("checked") == "checked") {
                        var filterDate = $("#tbFilterDate").val();
                        if (!utility.isNull(filterDate) && (utility.isNull(regular) || filterDate.match(regular))) {
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
                        if (!utility.isNull(filterDate) && (utility.isNull(regular) || filterDate.match(regular))) {
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

                            $(me).text(dateTextValue);
                            $(me).parent().removeAttr("r");
                            $(me).parent().attr("r", jsonDateValue);
                            e.preventDefault();
                            $.colorbox.close();
                        }
                    }
                    else if ($("#rdo_DateRight2").attr("checked") == "checked") {
                        var filterDate = $("#tbFilterDate").val() + " " + changeTimeFormat($("#tbFilterTime").val(), 1);
                        if (!utility.isNull(filterDate) && (utility.isNull(regular) || filterDate.match(regular))) {
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

}

//获取日期类型下拉框字典值
function getDateSelValue(type) {
    var postUrl = "/MemSubdivision/GetCommonOptionData";
    ajaxSync(postUrl, { type: type }, function (res) {
        if (utility.isNull(res)) {
            return;
        }
        else {
            rightDateSelObj = res;
            var objHtml = "<select id='dateRight-menu1' class='dateRight-menu1 span4'>";
            for (var i = 0; i < res.length; i++) {
                if (i == 0)
                    objHtml += "<option value='" + res[i].OptionValue + "' selected='selected'>" + res[i].OptionText + "</option>";
                else
                    objHtml += "<option value='" + res[i].OptionValue + "'>" + res[i].OptionText + "</option>";
            }
            objHtml += "</select>"
        }
        strRightDateSelValue = objHtml;
    });
}


function setRightValueHtml(controlType, type, curRightValue) {

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

            if (type == dataTypeList.int || type == dataTypeList.dec[0] || type == dataTypeList.dec[1]) {
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
}
//设置下拉框值i
function setSelect(selid, s) {
    sl = document.getElementById(selid);
    if (s == null)
        s = "";
    for (i = 0; i < sl.length; i++) {
        if (sl[i].value == $.trim(s)) {
            sl[i].selected = true;
        }
        else
            sl[i].selected = false;
    }
}
function setSelectText(selid, s) {
    sl = document.getElementById(selid);
    if (s == null)
        s = "";
    for (i = 0; i < sl.length; i++) {
        var seltxt = sl[i].text;
        if (seltxt == s.trim()) {
            sl[i].selected = true;
        }
        else
            sl[i].selected = false;

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
            var datatype = "";
            var controltype = "";
            var enumvalue = "";
            var enumobj = null;
            var regular = "";
            var descname = "";
            var l = subVal.l;
            var e = subVal.e;
            var r = subVal.r;
            var rText = r == "" ? "空" : r;
            var dictTableName = "";
            var dictTableType = "";
            leftValueList.forEach(function (item, index, array) {
                if (item.FieldAlias == l) {

                    datatype = item.FieldType;
                    controltype = item.ControlType;
                    enumvalue = item.EnumValue;
                    regular = item.Reg;
                    descname = item.FieldDesc;
                    dictTableName = item.dictTableName;
                    dictTableType = item.dictTableType;
                    //if (enumvalue) {
                    //    enumobj = eval("(" + enumvalue + ")");
                    //    if (enumobj.Type == "Fixed") {
                    //        arrEnum = enumobj.Code.split("|");
                    //    } else {
                    //        arrEnum = filterRightVals[l];
                    //    }
                    //}
                    if (item.DataType = "select") {
                        if (!utility.isNull(filterRightVals))
                            arrEnum = filterRightVals[l];
                    }
                    if (controltype == "select") {
                        arrEnum.forEach(function (item, index, array) {
                            if (item.sv == r) {
                                rText = item.st;
                                return false;
                            }
                        });
                    }
                    else if (controltype == "selesearch") {
                        arrEnum.forEach(function (item, index, array) {
                            if (item.sv == r) {
                                rText = item.st;
                                return false;
                            }
                        });

                    }
                    else if (controltype == "date") {
                        var rTextLower = rText.toString().toLowerCase();
                        if (rTextLower.indexOf("dateadd") >= 0) {
                            var subtxt1 = "";
                            if (rTextLower.indexOf("'") >= 0)
                                subtxt1 = rTextLower.substr(rTextLower.indexOf("dateadd") + 8, rTextLower.indexOf(")") - rTextLower.indexOf("dateadd") - 9);
                            else
                                subtxt1 = rTextLower.substr(rTextLower.indexOf("dateadd") + 8, rTextLower.indexOf(")") - rTextLower.indexOf("dateadd") - 8);
                            if (utility.isNull(subtxt1))
                                rText = "";
                            else {
                                var arr = new Array();
                                arr = subtxt1.split(",");
                                if (arr.length != 3)
                                    rText = "";
                                else {
                                    var dateSel = arr[0];
                                    var addValue = arr[1];
                                    var orgDate = arr[2].replace(/\'/g, "").trim();
                                    var rightDateText = "";
                                    if (rightDateSelObj != null) {
                                        for (var i = 0; i < rightDateSelObj.length; i++) {
                                            if (rightDateSelObj[i].OptionValue.toLowerCase() == orgDate.toLowerCase())
                                                rightDateText = rightDateSelObj[i].OptionText;
                                        }
                                    }
                                    leftValueList.forEach(function (item, index, array) {
                                        if (item.FieldAlias.toLowerCase() == orgDate) {
                                            rightDateText = item.FieldDesc;
                                        }
                                    });
                                    if (rightDateText != "") {
                                        var dateTxt = "";
                                        if (dateSel == "day")
                                            dateTxt = "天";
                                        else if (dateSel == "month")
                                            dateTxt = "月";
                                        else if (dateSel == "hour")
                                            dateTxt = "时";
                                        else if (dateSel == "minute")
                                            dateTxt = "分";
                                        rText = rightDateText + "+ " + addValue + dateTxt;
                                    }
                                    else {
                                        var dateTxt = "";
                                        if (dateSel == "day")
                                            dateTxt = "天";
                                        else if (dateSel == "month")
                                            dateTxt = "月";
                                        else if (dateSel == "hour")
                                            dateTxt = "时";
                                        else if (dateSel == "minute")
                                            dateTxt = "分";
                                        rText = orgDate + "+ " + addValue + dateTxt;
                                    }
                                }
                            }
                        }
                        else {
                            if (rightDateSelObj != null) {
                                for (var i = 0; i < rightDateSelObj.length; i++) {
                                    if (rightDateSelObj[i].OptionValue.toLowerCase() == rText.toLowerCase())
                                        rText = rightDateSelObj[i].OptionText;
                                }
                            }
                            leftValueList.forEach(function (item, index, array) {
                                if (item.FieldAlias == rText) {
                                    rText = item.FieldDesc;
                                }
                            });
                        }
                    }
                    else {
                        if (datatype == dataTypeList.int || datatype == dataTypeList.dec[0] || datatype == dataTypeList.dec[1]) {
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
                            }
                        }
                        else {
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
            subRootReletionHtml += "<li class='filter-item' l='" + l + "' e='" + e + "' r='" + r + "' datatype='" + datatype + "' controltype='" + controltype + "' enumvalue='" + enumvalue + "' reg='" + regular + "' dictTableName='" + dictTableName + "' dictTableType='" + dictTableType + "'><span class='dynatree-connector'></span>"
                + "<a id='hyLeftValue' href='javascript:;'>" + descname + "</a>&nbsp;<a id='hyOperation' href='javascript:;'>" + getOperatorName(e) + "</a>&nbsp;<a id='hyRightValue' href='javascript:;'>" + rText + "</a>&nbsp;<i class='splashy-menu'></i>&nbsp;<a id='hyDelFilter' href='javascript:;'><i class='splashy-gem_remove'></i></a></li>";
        });
        subRootReletionHtml += "</ul></li>";
    });
    if (!utility.isNull(subRootReletionHtml)) {
        $("#ulSubRootReletion").html(subRootReletionHtml);
    }
    setLastRootClass();
}

// 获取连接符名称
function getOperatorName1(sign) {
    var operatorName = "";
    switch (sign) {
        case "=":
            operatorName = "设置为";
            break;
        case "+":
            operatorName = "累加(减)";
            break;
        case "+=":
            operatorName = "累加(减)";
            break;
        case "*=":
            operatorName = "累乘";
            break;
        case "*":
            operatorName = "乘以";
            break;
        case "sum":
            operatorName = "求和";
            break;
        case "avg":
            operatorName = "平均值";
            break;
        case "max":
            operatorName = "最大值";
            break;
        case "min":
            operatorName = "最小值";
            break;
        case "Normal":
            operatorName = "普卡会员";
            break;
        case "Copper":
            operatorName = "铜卡会员";
            break;
        case "Silver":
            operatorName = "银卡会员";
            break;
        case "Gold":
            operatorName = "金卡会员";
            break;
        case "Platinum":
            operatorName = "白金卡会员";
            break;
        case "Account":
            operatorName = "账户";
            break;
        case "DateNow":
            operatorName = "当前日期";
            break;
        case "year":
            operatorName = "年";
            break;
        case "month":
            operatorName = "月";
            break;
        case "day":
            operatorName = "日";
            break;
        default:
            operatorName = sign;
            break;
    }
    return operatorName;
};


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
// 获取过滤条件右边值
function getRightValueHtml(controlType, dictTableName, dictTableType, type, fieldalias, tableName) {
    var rightValueHtml = "";
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
            compareLeftHtml += "<option value='" + item.FieldAlias + "' l='" + item.FieldAlias + "' f='" + item.FieldName + "' datatype='"
                + item.FieldType + "' controltype='" + item.ControlType + "' enumvalue='" + item.EnumValue + "' reg='"
                + item.Reg + "' dictTableName='" + item.DictTableName + "' dictTableType='" + item.DictTableType + "' tableName='" + item.TableName + "'>" + item.FieldDesc + "</option>";
        }
    });
    compareLeftHtml += "</select>";

    switch (controlType) {
        //单选下拉框
        case "select":
            var arrEnum;
            var valAttr,
                textAttr;

            rightValueHtml = "<ul name='selRightValue' id='selRightValue' data-placeholder='选择右值...' class='span14 chzn_a'>";

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
            rightValueHtml = "<select name='selSearchRightValue' id='selSearchRightValue' data-placeholder='选择右值...' class='span12 chzn_a'>";
            arrEnum = filterRightVals[fieldalias];
            $.each(arrEnum, function (i, n) {
                textAttr = n["st"];
                valAttr = n["sv"];
                rightValueHtml += "<option value='" + valAttr + "'>" + textAttr + "</option>";
            });
            rightValueHtml += "</select><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />";
            break;
        case "date":
            if (type == dataTypeList.date[1]) {
                rightValueHtml = "<ul><li><input type='radio' id='rdo_DateRight1' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight1'> 特定值 </label>" + strRightDateSelValue;
                rightValueHtml += "+<input type='number' class='span2' id='tbFilterNumber2' />";
                rightValueHtml += "<select id='selDateAddValue2' class='selDateAddValue2 span2'><option value='day' selected='selected'>天</option><option value='month'>月</option><option value='hour'>时</option><option value='minute'>分</option></select>"
                rightValueHtml += "<li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设置值 </label>";
                rightValueHtml += "<input type='text' placeholder='选择日期' class='input-small' id='tbFilterDate' />";
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
            }
            else {
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
            rightValueHtml += "<input type='text' placeholder='选择日期' class='input-small' id='tbFilterDate' />";
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

            if (type == dataTypeList.string[0] || type == dataTypeList.string[1]) {
                rightValueHtml += "<ul><li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设定值 </label>"
                rightValueHtml += "<input type='text' class='span14' id='tbFilterText' />";
                rightValueHtml += "</li>";

                rightValueHtml += "<li><input type='radio' id='rdo_DateRight3' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight3'> 选定值 </label>";
                rightValueHtml += compareLeftHtml;
                rightValueHtml += "</li>";
                rightValueHtml += "</ul>";
            }
            else {
                rightValueHtml += "<ul><li><input type='radio' id='rdo_DateRight2' name='dateRightRdoGroup'  style='float:left;margin-right:5px' /><label for='rdo_DateRight2'> 设定值 </label>"
                rightValueHtml += "<input type='text' class='span14' id='tbFilterText' />";
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
    return rightValueHtml;
}

//选择连接符
function selectOperator(me) {
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
                var selVal = "";
                var selText = "";
                $(".operator-menu li").each(function (index, item) {
                    if ($(this).children("input").attr("checked") == "checked") {
                        selVal = $(this).children("input").val();
                        selText = $(this).children("input").attr("title");
                    }
                })
                if (utility.isNull(selVal)) {//$(".operator-menu").val()
                    $("#selDialogError").text("请选择操作符");
                    $.colorbox.resize();
                }
                else {
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

//展示关系菜单
function showRootReletion(me) {
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

                var selVal = "";
                var selText = "";
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

    $("#txtLeftValueSearch").bind("keyup", function (e) {
        //e.preventDefault();
        var reg = "[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]";
        var valueserach = $("#txtLeftValueSearch").val();
        if (valueserach.match(reg)) {
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
                    if (curtext.indexOf(name) < 0) {
                        $(this).hide();
                    }
                });
                $.colorbox.resize();
            }
            else {
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
                var selText = "";
                var tid = "";
                var isHasValue = false;
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
                }
                else {
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
                    $(me).parent().attr("l", $(selObj).attr("l"));
                    $(me).parent().attr("datatype", $(selObj).attr("datatype"));
                    $(me).parent().attr("controltype", $(selObj).attr("controltype"));
                    $(me).parent().attr("enumvalue", $(selObj).attr("enumvalue"));
                    $(me).parent().attr("type", $(selObj).attr("type"));
                    $(me).parent().attr("reg", $(selObj).attr("reg"));
                    $(me).parent().attr("dictTableName", $(selObj).attr("dictTableName"));
                    $(me).parent().attr("dictTableType", $(selObj).attr("dictTableType"));
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

// 获取过滤条件右值
function getFilterRightValues(rightValCfg) {
    var postData = { "rightValCfgs": JSON.stringify(rightValCfg) },
        postUrl = "/MemSubdivision/GetMemSubdRightValues";
    ajax(postUrl, postData, getFilterRightValueCallback);
}

function getFilterRightValueCallback(data) {
    if (data) {
        filterRightVals = eval("(" + data.Data + ")");
    }
    //如果隐藏的Condition不空，加载数据
    if ($("#hideCondition").val() != "") {
        convertFilterJsonToForm($("#hideCondition").val());
    };
}





/****************************** tab2 end ***********************************/

/****************************** tab3 start ***********************************/


function selRightValue1(obj) {
    var val = $(obj).val();
    if (val != '') {
        $("#txtRightValue2").val('');
    }
}
function selRightValue2(obj) {
    var val = $(obj).val();
    if (val != '') {
        $("#txtRightValue1").val('');
        //判断选中的值是否是actionResult中的数据，则没有上下限
        if (Enumerable.from(actionsResult).where("($.FieldAlias == '" + val + "')").count() != 0) {
            $("#txtRightMax,#txtRightMin").val('').prop("disabled", "disabled");
        } else {
            $("#txtRightMax,#txtRightMin").prop("disabled", false);
        }
    }
}
function selRightValueChange(obj) {
    var val = $(obj).val();
    if (val == '1') {//可选值
        $("#divRightValue3").show(); $("#txtRightValue2").val('');
        $("#divRightValue4").show(); $("#txtRightValueFilterAlias").val('');
        $("#divRightValue5").show(); $("#txtRightMax").val('');
        $("#divRightValue6").show(); $("#txtRightMin").val('');

        $("#divRightValue8").show().html("<label>偏移表达式</label>" + strActionOffSetErrHtml); $("#selOffset").val('+');
        $("#divRightValue9").show(); $("#txtOffsetValue").val('');

        $("#divRightValue7").remove();
        $("#divRightValue10").show(); $("#selActLimit").val('').trigger("liszt:updated");
    } else {//固定值
        $("#divRightValue3").hide(); $("#txtRightValue2").val('');
        $("#divRightValue4").hide(); $("#txtRightValueFilterAlias").val('');
        $("#divRightValue5").hide(); $("#txtRightMax").val('');
        $("#divRightValue6").hide(); $("#txtRightMin").val('');

        $("#divRightValue8").hide().html(''); //$("#selOffset").val('+');
        $("#divRightValue9").hide(); $("#txtOffsetValue").val('');
        $("#divRightValue8").before("<div id='divRightValue7' class='span3'><label>右值(固定值)</label><input id='txtRightValue1' type='text' class='span12' onkeydown='selRightValue1(this)' onkeyup=\"value=value.replace(/[^\\d.]/g,'')\" /></div>");

        $("#divRightValue10").hide(); $("#selActLimit").val('').trigger("liszt:updated");
    }
}

//弹出层添加事件
function addAct() {
    if ($("#selActionLeft").val() != "") {
        var curLeftValue = $("#selActionLeft").val();
        if (ModifidAct != $("#selActionLeft").val() && curLeftValue != "Account") {
            if (Enumerable.from(actions).where("($.LeftValue.ExtName == '" + curLeftValue + "')").count() != 0) {
                $.dialog("已经添加过此条目");
                return;
            }
        }
        var curLeftText = $("#selActionLeft").find("option:selected").text();
        var curOpText = $("#selOp").find("option:selected").text();
        var curOpValue = $("#selOp").val();
        var curRightText;
        var curRightValue;


        if ($("#selActionLeft").val() == "Account") {
            var drp = $("#drpRightValue").val();
            if (drp == '1') {
                curRightValue = $("#txtRightValue2").val();
                var rightText = $("#txtRightValue2").find("option:selected").text();
                curRightText = rightText + $("#selOffset").val() + $("#txtOffsetValue").val();
            }
            else {
                curRightValue = $("#txtRightValue1").val();
                var rightText = $("#txtRightValue1").val();
                curRightText = rightText;//+ $("#selOffset").val() + $("#txtOffsetValue").val();
            }

            if (curRightValue == '') {
                $.dialog("右值不能为空");
                return;
            }
            if (drp == '1') {
                if (!utility.isNumber($("#txtOffsetValue").val())) {
                    $.dialog("偏移量格式不正确");
                    return;
                }
            }
            //判断上下限大小
            var Maximum = $("#txtRightMax").val();
            var Minimum = $("#txtRightMin").val();
            if (Maximum != '' && Minimum != '') {
                if (parseInt(Maximum) <= parseInt(Minimum)) {
                    $.dialog("上限值必须大于下限值");
                    return;
                }
            }
            //关于偏移日期
            if ($("#txtFreezeValue").val() == "" && $("#drpFreezeUnit").val() != "") {
                $("#txtFreezeValue").val('0');
            }
            if ($("#txtAvailabeValue").val() == "" && $("#drpAvailabeUnit").val() != "") {
                $("#txtAvailabeValue").val('0');
            }
        }
        else if ($("#selRightValue1").val() == "fixed") {
            curRightText = $("#txtRightValue").val();
            curRightValue = $("#txtRightValue").val();
            if (curRightValue == "") {
                $.dialog("请填写固定值");
                return;
            }
        }
        else if ($("#selRightValue1").val() == "DateNow") {
            //curRightText = $("#selRightValue1").find("option:selected").text() + '+' + ($("#txtOffsetValue").val() == "" ? 0 : $("#txtOffsetValue").val()) + $("#selDateUnit").find("option:selected").text();
            curRightText = $("#txtOffsetValue").val() == "" ? $("#selRightValue1").find("option:selected").text() : ($("#selRightValue1").find("option:selected").text() + "+" + $("#txtOffsetValue").val() + $("#selDateUnit").find("option:selected").text());
            curRightValue = $("#selRightValue1").val();
        }
        else if ($("#selRightValue1").val() == "NotNow") {
            curRightText = $("#txtRightValue").val();
            curRightValue = $("#txtRightValue").val();
        }
        else if ($("#selRightValue1").val() == undefined) {
            curRightText = $("#txtRightValue").val();
            curRightValue = $("#txtRightValue").val();
        }
        else {
            curRightValue = $("#selRightValue1").val();
            curRightText = $("#selRightValue1").find("option:selected").text();
        }

        //添加或保存
        if (ModifidAct == "") {
            $("#dt_act").find("tbody").append("<tr><td>" + curLeftText + "</td><td>" + curOpText + "</td><td>" + curRightText + "</td><td l='" + curLeftValue + "'><button class='btn' type='button' onclick='modifyAct(this)'>编辑</button> <button onclick='removeAct(this)' class='btn btn-danger'>删除</button></td></tr>");
        }
        else {
            //actions = Enumerable.from(actions).where("($.LeftValue.ExtName != '" + ModifidAct + "')").toArray();
            //var line = $("#dt_act").find("td[l='" + ModifidAct + "']").parent();
            var line = $(tr).parent().parent('tr');
            line.html("<td>" + curLeftText + "</td><td>" + curOpText + "</td><td>" + curRightText + "</td><td l='" + curLeftValue + "'><button class='btn' type='button' onclick='modifyAct(this)'>编辑</button> <button onclick='removeAct(this)' class='btn btn-danger'>删除</button></td>");
        }
        //actions.push({ l: curLeftValue, e: curOpValue, r: curRightValue, r1: $("#selRightValue1").val(), r2: $("#txtRightValue").val() == undefined ? "" : $("#txtRightValue").val(), s: actions.length == 0 ? 1 : (actions[actions.length - 1].s + 1), rt: curRightText });

        //账户限制条件
        var actLimit = new Array();
        $("#selActLimit").find("option:selected").each(function (i, data) {
            var value = data.value;
            actLimit[i] = value;
        });
        var extType = $("#selActType") == undefined ? "" : $("#selActType").val()//.find("option:selected").text();
        if (ModifidAct == "") {
            //新版本
            actions.push({
                LeftValue: { ExtName: curLeftValue, ExtType: extType, ExtLimitList: actLimit }, Expression: curOpValue, RightValue: curRightValue, RightValueFilterAlias: $("#txtRightValueFilterAlias").val() == undefined ? "" : $("#txtRightValueFilterAlias").val(), RightValueMax: $("#txtRightMax").val() == undefined ? "" : $("#txtRightMax").val(), RightValueMin: $("#txtRightMin").val() == undefined ? "" : $("#txtRightMin").val(), OffsetExpression: $("#selOffset").val() == undefined ? "" : $("#selOffset").val(), OffsetValue: $("#txtOffsetValue").val() == undefined ? "" : ($("#txtOffsetValue").val() == "" ? 0 : $("#txtOffsetValue").val()), OffsetUnit: $("#selDateUnit").val() == undefined ? "" : $("#selDateUnit").val(), FreezeValue: $("#txtFreezeValue").val() == undefined ? "" : $("#txtFreezeValue").val(), FreezeUnit: $("#drpFreezeUnit").val() == undefined ? "" : $("#drpFreezeUnit").val(), AvailabeValue: $("#txtAvailabeValue").val() == undefined ? "" : $("#txtAvailabeValue").val(), AvailabeUnit: $("#drpAvailabeUnit").val() == undefined ? "" : $("#drpAvailabeUnit").val(), OffsetDay: $("#drpAvailUnit2").val() == undefined ? "" : $("#drpAvailUnit2").val(), OffsetMonth
        : $("#drpAvailUnit1").val() == undefined ? "" : $("#drpAvailUnit1").val(), Sort: actions.length == 0 ? 1 : (actions[actions.length - 1].Sort + 1)
            });
        } else {

            actions[trNum] = {
                LeftValue: { ExtName: curLeftValue, ExtType: extType, ExtLimitList: actLimit }, Expression: curOpValue, RightValue: curRightValue, RightValueFilterAlias: $("#txtRightValueFilterAlias").val() == undefined ? "" : $("#txtRightValueFilterAlias").val(), RightValueMax: $("#txtRightMax").val() == undefined ? "" : $("#txtRightMax").val(), RightValueMin: $("#txtRightMin").val() == undefined ? "" : $("#txtRightMin").val(), OffsetExpression: $("#selOffset").val() == undefined ? "" : $("#selOffset").val(), OffsetValue: $("#txtOffsetValue").val() == undefined ? "" : ($("#txtOffsetValue").val() == "" ? 0 : $("#txtOffsetValue").val()), OffsetUnit: $("#selDateUnit").val() == undefined ? "" : $("#selDateUnit").val(), FreezeValue: $("#txtFreezeValue").val() == undefined ? "" : $("#txtFreezeValue").val(), FreezeUnit: $("#drpFreezeUnit").val() == undefined ? "" : $("#drpFreezeUnit").val(), AvailabeValue: $("#txtAvailabeValue").val() == undefined ? "" : $("#txtAvailabeValue").val(), AvailabeUnit: $("#drpAvailabeUnit").val() == undefined ? "" : $("#drpAvailabeUnit").val(), OffsetDay: $("#drpAvailUnit2").val() == undefined ? "" : $("#drpAvailUnit2").val(), OffsetMonth
    : $("#drpAvailUnit1").val() == undefined ? "" : $("#drpAvailUnit1").val(), Sort: trNum + 1
            };
        }
    }
    $.colorbox.close();
}

//移除行
function removeAct(me) {
    var key = $(me).closest("td").attr("l");
    var i = $(me).parent().parent('tr').prevAll().length;//当前行的坐标
    //actions[i] = [];
    actions.splice(i, 1);
    //actions = Enumerable.from(actions).where("($.LeftValue.ExtName != '" + key + "')").toArray();
    $(me).closest("tr").remove();
};

//修改行
function modifyAct(me) {
    $("#divOp,#divRightValue1,#divRightValue2,#divRightValue3,#divRightValue4,#divRightValue5,#divRightValue6").html("");
    $("#divRightValue7,#divRightValue8,#divRightValue9,#divRightValue10,#divRightValue11,#divRightValue12").html("");
    $("#divAtten").hide();
    tr = me;
    var key = $(me).closest("td").attr("l");
    var i = $(me).parent().parent('tr').prevAll().length;//当前行的坐标
    trNum = i;
    //var selact = Enumerable.from(actions).first("($.LeftValue.ExtName == '" + key + "')");
    $("#selActionLeft").val(key);
    $("#selActionLeft").change();
    //actionLeftChanged();
    $("#selOp").val(actions[i].Expression);
    $("#selActType").val(actions[i].LeftValue.ExtType);

    if (key == "Account" && utility.isNumber(actions[i].RightValue)) {
        $("#drpRightValue").val('2').change();
    }
    if (key == "Account" && !utility.isNumber(actions[i].RightValue)) {
        $("#drpRightValue").val('1').change();
    }

    $("#txtRightValue").val(actions[i].RightValue);

    if (actions[i].RightValue == "DateNow")
        $("#selRightValue1").val(actions[i].RightValue).change();
    else if (actions[i].RightValue == "NotNow")
        $("#selRightValue1").val("NotNow").change();
    else {
        $("#selRightValue1").val(actions[i].RightValue);
    }
    //右值赋值
    if (utility.isNumber(actions[i].RightValue)) {
        $("#txtRightValue1").val(actions[i].RightValue);
    } else {
        $("#txtRightValue2").val(actions[i].RightValue);//.change();
    }
    $("#txtRightMax").val(actions[i].RightValueMax);
    $("#txtRightMin").val(actions[i].RightValueMin);
    $("#txtRightValueFilterAlias").val(actions[i].RightValueFilterAlias);
    $("#txtFreezeValue").val(actions[i].FreezeValue);
    $("#drpFreezeUnit").val(actions[i].FreezeUnit);
    $("#txtAvailabeValue").val(actions[i].AvailabeValue);
    $("#drpAvailabeUnit").val(actions[i].AvailabeUnit).change();

    $("#drpAvailUnit1").val(actions[i].OffsetMonth);
    $("#drpAvailUnit2").val(actions[i].OffsetDay);

    $("#selActLimit").val(actions[i].LeftValue.ExtLimitList);
    $("#selActLimit").trigger("liszt:updated");

    $("#selOffset").val(actions[i].OffsetExpression);
    $("#txtOffsetValue").val(actions[i].OffsetValue);

    $("#selDateUnit").val(actions[i].OffsetUnit);
    $("#btnAddAction").text("保存");

    $("#tbAction h3").text("编辑动作");
    ModifidAct = $("#selActionLeft").val();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#tbAction",
        inline: true,
        opacity: '0.3',
        onComplete: function () {
        }
    });
};

//可输入下拉框方法
function writeSelect(obj) {
    obj.options[0].selected = "select";
    obj.options[0].text = obj.options[0].text + String.fromCharCode(event.keyCode);
    event.returnValue = false;
    return obj.options[0].text;
}
function deleteText(obj) {
    if (event.keyCode == 8) {
        obj.options[0].text = '';
    }
    return obj.options[0].text;
}
//加载右值列表
function LoadRightValueList() {

    //加载规则
    if (actionsResult.length > 0) {
        var opt = '';
        for (i = 0; i < actionsResult.length; i++) {
            opt += '<option value=' + actionsResult[i].FieldAlias + '>' + actionsResult[i].NameCode + '</option>'
        }
        $('#txtRightValue2').append(opt);
    }
    //加载明细
    $.post("/Loyalty/GetTradeAliasKeyList1", null, function (res) {
        if (res.length > 0) {
            //var opt = "<option value=''>请选择</option>";
            var opt = '';
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].FieldAlias + '>' + res[i].FieldDesc + '</option>';
            }
            $('#txtRightValue2').append(opt);
        } else {
            var opt = '';//"<option value=''>-无-</option>";
            $('#txtRightValue2').append(opt);
        }
    });
}
//加载右值累计过滤
function LoadRightValueFilterAlias() {
    //加载明细
    $.post("/Loyalty/GetTradeAliasKeyList", { type1: 'MemberExt', type2: '' }, function (res) {
        if (res.length > 0) {
            //var opt = "<option value=''>请选择</option>";
            var opt = '';
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].FieldAlias + '>' + res[i].FieldDesc + '</option>';
            }
            $('#txtRightValueFilterAlias').append(opt);
        } else {
            var opt = '';//"<option value=''>-无-</option>";
            $('#txtRightValueFilterAlias').append(opt);
        }
    });
}

//加载使用限制
function GetLimitList() {
    $.post("/Member360/GetActLimitList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#selActLimit').append(opt);
            $(".chzn_b").trigger("liszt:updated");

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#selActLimit').append(opt);
        }
    });
}
/****************************** tab3 end ***********************************/



//移除交易订单计算行
function removeActResult(me) {
    var key = $(me).closest("td").attr("l");
    actionsResult = Enumerable.from(actionsResult).where("($.FieldAlias != '" + key + "')").toArray();
    $(me).closest("tr").remove();
};

//修改交易订单计算行
function modifyActResult(me) {
    var key = $(me).closest("td").attr("l");
    var selact = Enumerable.from(actionsResult).first("($.FieldAlias == '" + key + "')");


    $("#txtKey").val(key);
    $("#txtKey").trigger("liszt:updated");
    $("#txtCeiling").val(selact.Maximum);
    $("#txtFloor").val(selact.Minimum);
    $("#drpOffsetExpression").val(selact.OffsetExpression);
    //$("#drpGroupFunc").find("option:selected").text();
    //$("#drpOffsetExpression").find("option:selected").text();
    $("#drpGroupFunc").val(selact.GroupFunc);
    $("#txtShift").val(selact.OffsetValue);
    $("#txtNameCode").val(selact.NameCode);

    $("#btnAddComRule").text("保存");
    ModifiActResult = $("#txtKey").val();

    $("#table_addcomputer h3").html("设定规则编辑");
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#table_addcomputer",
        inline: true,
        opacity: '0.3',
        onComplete: function () {
        }
    });
};

//加载关键字列表
function LoadAliasKeyList() {
    ajax("/Loyalty/GetTradeAliasKeyList", { type1: 'MemberTradeDetail', type2: '' }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].FieldAlias + '>' + res[i].FieldDesc + '</option>';
            }
            $('#txtKey').append(opt);
            $(".chzn_a").chosen();
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#txtKey').append(opt);
        }
    });
}

//加载关键字时，转换
function GetAliasDescByValue(v) {
    var desc = '';
    $.post("/Loyalty/GetAliasDescByValue", { value: v }, function (res) {
        if (res.length > 0) {
            desc = res[0].FieldDesc;
        } else {
            desc = v;
        }
    });
    return desc;
}
/****************************** tab4 end ***********************************/



