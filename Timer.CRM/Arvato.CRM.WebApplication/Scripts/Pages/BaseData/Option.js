var aliasID = 0;//标记判断是增加还是编辑
var paracount = 0;

$(function () {
    //绑定新增删除 Para 按钮事件
    ViewDataBind();

    //加载数据表格
    dt_OptionTable = $('#dt_OptionTable').dataTable({
        sAjaxSource: '/BaseData/GetOptionData',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'AliasDegeeID', title: "编号", sortable: true },
            { data: 'OptionText', title: "类型", sortable: true },
            { data: 'FieldDesc', title: "参数名", sortable: false },
            { data: 'FieldDesc2', title: "目标参数名", sortable: false },
            { data: 'Sort', title: "排序编号", sortable: true },
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"goEdit(" + obj.AliasDegeeID + ")\">编辑</button>&nbsp;&nbsp;<button class=\"btn btn-danger\" id=\"btnDelete\" onclick=\"goDelete(" + obj.AliasDegeeID + ")\">删除</button>";
                    return htm;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'optionType', value: $("#drpOptionType").val() });
            d.push({ name: 'optionText', value: $("#drpOptionText").val() });
        }
    });
});

/* 弹窗 */
goAdd = function () {
    aliasID = 0;
    $("#addOption_dialog .heading h3").html("新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addOption_dialog",
        inline: true
    });
    $.colorbox.resize();
}
goEdit = function (id) {
    aliasID = id;
    $("#addOption_dialog .heading h3").html("编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetOptionById", { optionId: id }, function (res) {
        if (res.IsPass) {
            var info = res.Obj[0];
            $("#selOption").val(info.ValueDegeeType);
            $("#selValue").val(info.AliasID);
            $("#selTargetValue").val(info.AliasID2);
            BindSelectName(info.AliasID);
            $("#txtSort").val(info.Sort);
            $("#selOption").attr("disabled", 'disabled');
            $("#selValue").attr("disabled", 'disabled');
            $("#btnResetCancel").removeAttr("disabled");
            $("#btnSave").removeAttr("disabled");
            $("#splashyAdd").hide();
            Getpara($.parseJSON(info.BasicContent), info.ValueDegeeType);
        }
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addOption_dialog",
        inline: true
    });
    $.colorbox.resize();
}
goDelete = function (v) {
    if (window.confirm('是否确认删除？')) {
        ajax("/BaseData/DeleteSysOption", { optionId: v }, function (res) {
            if (res.IsPass) {
                dt_OptionTable.fnDraw();
                $.dialog(res.MSG);
            } else $.dialog(res.MSG);
        });
    }
}

//清空数据
goClear = function () {
    $("#frmAddOption").children(".dimension-para").remove();
    $("#selValue option:first").attr("selected", 'selected');
    $("#selOption option:first").attr("selected", 'selected');
    $("#btnSave").attr("disabled", 'disabled');
    $("#splashyAdd").show();
    $("#selOption").removeAttr("disabled");
    $("#selValue").removeAttr("disabled");
    $("#txtSort").val(0);
    paracount = 0;

    $('.error-block').html('');
}

/* 页面绑定 */
ViewDataBind = function () {
    BindAddParaBtn();
    BindSelectType();
    BindDrpOptionType();
    BindSelectName();
    BindSelectTargetName();
    $("#btnSave").click(function () {
        BindBtnSave();
    });

    $("#btnSerach").click(function () {
        dt_OptionTable.fnDraw();
    });
    //选择条件 - 选择类型
    $("#drpOptionType").change(function () {
        BindDrpOptionType();
    });
    //选择类型
    $("#selOption").change(function () {
        BindSelectName();
    });
    //选择Name
    $("#selValue").change(function () {
        BindSelectOptionCode();
    });
    //重置
    $("#btnResetCancel").click(function () {
        $("#frmAddOption").children(".dimension-para").remove();
        $("#btnSave").attr("disabled", 'disabled');
        $("#splashyAdd").show();
        $("#selOption").removeAttr("disabled");
        $("#selValue").removeAttr("disabled");
        $(this).attr("disabled", "disabled");
        paracount = 0;
    });
}
//绑定保存事件
BindBtnSave = function () {
    var paraArray = checkPara();
    if (!paraArray) {
        return;
    }
    var addModel = {
        AliasDegeeID: aliasID,
        BasicContent: JSON.stringify(paraArray),
        ValueDegeeType: $("#selOption").val(),
        AliasID: $("#selValue").val(),
        AliasID2: $("#selTargetValue").val(),
        Sort: $("#txtSort").val()
    };
    ajax("/BaseData/SaveSysOption", addModel, function (res) {
        if (res.IsPass) {
            $.colorbox.close();
            dt_OptionTable.fnDraw();
            $.dialog(res.MSG);
        } else $.dialog(res.MSG);
    });
}
//搜索条件中 - 参数名
BindDrpOptionType = function () {
    ajaxSync("/BaseData/GetSysOptionValues", { type: $("#drpOptionType").val() }, function (res) {
        if (res.IsPass) {
            var optionHtml = "";
            for (var i = 0; i < res.Obj[0].length; i++) {
                optionHtml += "<option value='" + res.Obj[0][i].AliasID + "' algTable='" + res.Obj[0][i].DictTableName + "' algDictType='" + res.Obj[0][i].DictTableType + "'>" + res.Obj[0][i].FieldDesc + "</option>";
            }
            $("#drpOptionText").html("<option value=''>全部</option>" + optionHtml);
        } else $.dialog(res.MSG);
    });
}
//编辑界面 - 参数名
BindSelectName = function (v) {
    ajaxSync("/BaseData/GetSysOptionValues", { type: $("#selOption").val(), type2: "Source" }, function (res) {
        if (res.IsPass) {
            var optionHtml = "";
            for (var i = 0; i < res.Obj[0].length; i++) {
                optionHtml += "<option value='" + res.Obj[0][i].AliasID + "' algTable='" + res.Obj[0][i].DictTableName + "' algDictType='" + res.Obj[0][i].DictTableType + "'>" + res.Obj[0][i].FieldDesc + "</option>";
            }
            $("#selValue").html(optionHtml);
            if (v != null) $("#selValue").val(v);
            BindSelectOptionCode();
        } else $.dialog(res.MSG);
    });
}
//编辑界面 - 目标参数名
BindSelectTargetName = function () {
    ajaxSync("/BaseData/GetSysOptionValues", { type: $("#selOption").val(), type2: "Target" }, function (res) {
        if (res.IsPass) {
            var optionHtml = "";
            for (var i = 0; i < res.Obj[0].length; i++) {
                optionHtml += "<option value='" + res.Obj[0][i].AliasID + "' algTable='" + res.Obj[0][i].DictTableName + "' algDictType='" + res.Obj[0][i].DictTableType + "'>" + res.Obj[0][i].FieldDesc + "</option>";
            }
            $("#selTargetValue").html(optionHtml);
        } else $.dialog(res.MSG);
    });
}
//搜索中和编辑界面 - 类型
BindSelectType = function () {
    ajaxSync("/BaseData/GetSysOptionTypes", {}, function (res) {
        if (res.IsPass) {
            var optionHtml = "";
            for (var i = 0; i < res.Obj[0].length; i++) {
                optionHtml += "<option value='" + res.Obj[0][i].OptionValue + "'>" + res.Obj[0][i].OptionText + "</option>";
            }
            $("#selOption").html(optionHtml);
            $("#drpOptionType").html("<option value=''>全部</option>" + optionHtml);
        } else $.dialog(res.MSG);
    });
}
//动态获取 编辑页面中 Type为下拉框的值
BindSelectOptionCode = function () {
    if ($("#selOption").val() != 3) return;
    //table, dictType
    var table = $("#selValue").find("option:selected").attr("algTable"),
        dictType = $("#selValue").find("option:selected").attr("algDictType");
    ajaxSync("/MemSubdivision/GetRightSelectData", { table: table, dictType: dictType }, function (res) {
        if (res) {
            var optionHtml = "";
            for (var i = 0; i < res.length; i++) {
                optionHtml += "<option value='" + res[i].OptionValue + "'>" + res[i].OptionText + "</option>";
            }
            $("#selOptionCode").html(optionHtml);
        } else $.dialog(res.MSG);
    });
}
//绑定控件
BindAddParaBtn = function () {
    $("body").delegate(".splashy-add_small", "click", function () {
        if ($(this).attr("alg") == "splashyAdd") {
            $(this).hide();
            $("#selOption").attr("disabled", "disabled");
            $("#selValue").attr("disabled", "disabled");
            $("#btnResetCancel").removeAttr("disabled");
            $("#btnSave").removeAttr("disabled");
        }
        addParaHTML();
    });

    $("body").delegate(".splashy-remove_minus_sign_small", "click", function () {
        if ($(".dimension-para").length > 4) {
            $(this).parents(".dimension-para").remove();
            //paracount = 0;
        }
    });
}

/* 是否为正浮点型数据 */
isNumber = function (num) {
    var reNum = /^\d*$/;
    var reNum2 = /^\d+(\.(\d{2}|\d{1}))?$/;
    if (reNum.test(num) || reNum2.test(num))
        return true;
    return false;
}

/* 新增参数配置 */
addParaHTML = function () {
    var v = $("#selOption").val();
    paracount += 1;
    var a;
    if (v == 1) {
        a = addParaHTML1(paracount);
    } else if (v == 2) {
        a = addParaHTML2(paracount);
    } else if (v == 3) {
        a = addParaHTML3(paracount);
    }
    if (a != null) a.show();
    $("#frmDiv").before(a);
    //$("#txt_uiindex" + paracount).val(paracount);
}
addParaHTML1 = function (paracount) {
    var a = $("#paraType1").clone();
    a.attr("id", "paraType1" + paracount);
    a.children(".dimension-para-btns").addClass("dimension-btnscount2");
    a.find("#txtMinNum").attr("id", "txtMinNum" + paracount);
    a.find("#txtMaxNum").attr("id", "txtMaxNum" + paracount);
    a.find("#txtIntegralV1").attr("id", "txtIntegralV1" + paracount);
    return a;
}
addParaHTML2 = function (paracount) {
    var a = $("#paraType2").clone();
    a.attr("id", "paraType2" + paracount);
    a.children(".dimension-para-btns").addClass("dimension-btnscount2");
    a.find("#txtStartPercentage").attr("id", "txtStartPercentage" + paracount);
    a.find("#txtEndPercentage").attr("id", "txtEndPercentage" + paracount);
    a.find("#txtIntegralV2").attr("id", "txtIntegralV2" + paracount);
    return a;
}
addParaHTML3 = function (paracount) {
    var a = $("#paraType3").clone();
    a.attr("id", "paraType3" + paracount);
    a.children(".dimension-para-btns").addClass("dimension-btnscount2");
    a.find("#selOptionCode").attr("id", "selOptionCode" + paracount);
    a.find("#txtIntegralV3").attr("id", "txtIntegralV3" + paracount);
    return a;
}

/* 保存参数配置 */
checkPara = function () {
    var v = $("#selOption").val();
    var paracontrol = $("#frmAddOption").children(".dimension-para");
    var paraArray = new Array();
    if (v == 1) {
        paraArray = checkPara1(paracontrol);
    } else if (v == 2) {
        paraArray = checkPara2(paracontrol);
    } else if (v == 3) {
        paraArray = checkPara3(paracontrol);
    }
    return paraArray;
}
checkPara1 = function (paracontrol) {
    var paraArray = new Array();
    for (var i = 0; i < paracontrol.length; i++) {
        var para = {
            ParaIndex: 0,
            ParameterMin: 0,
            ParameterMax: 0,
            ParameterVal: 0
        };
        para.ParaIndex = i + 1;
        para.ParameterMin = paracontrol[i].getElementsByClassName("txtMinNum")[0].value;
        if (para.ParameterMin == "") {
            $.dialog("前置参数不能为空");
            return false;
        }
        if (!isNumber(para.ParameterMin)) {
            $.dialog("请输入前置参数,须为正整数或者2位小数");
            return false;
        }
        para.ParameterMax = paracontrol[i].getElementsByClassName("txtMaxNum")[0].value;
        if (para.ParameterMax == "") {
            $.dialog("后置参数不能为空");
            return false;
        }
        if (!isNumber(para.ParameterMax)) {
            $.dialog("请输入后置参数,须为正整数或者2位小数");
            return false;
        }
        para.ParameterVal = paracontrol[i].getElementsByClassName("txtIntegralV1")[0].value;
        if (para.ParameterVal == "") {
            $.dialog("积分不能为空");
            return false;
        }
        if (!isNumber(para.ParameterVal)) {
            $.dialog("请输入积分,须为正整数或者2位小数");
            return false;
        }
        paraArray.push(para);
    }
    return paraArray;
}
checkPara2 = function (paracontrol) {
    var paraArray = new Array();
    for (var i = 0; i < paracontrol.length; i++) {
        var para = {
            ParaIndex: 0,
            ParameterS1: 0,
            ParameterS2: 0,
            ParameterVal: 0
        };
        para.ParaIndex = i + 1;
        para.ParameterS1 = paracontrol[i].getElementsByClassName("txtStartPercentage")[0].value;
        if (para.ParameterS1 == "") {
            $.dialog("前置参数不能为空");
            return false;
        }
        if (!isNumber(para.ParameterS1)) {
            $.dialog("请输入前置参数,须为正整数或者2位小数");
            return false;
        }
        para.ParameterS1 /= 100.0000;
        para.ParameterS2 = paracontrol[i].getElementsByClassName("txtEndPercentage")[0].value;
        if (para.ParameterS2 == "") {
            $.dialog("后置参数不能为空");
            return false;
        }
        if (!isNumber(para.ParameterS2)) {
            console.log(para.ParameterS2)
            $.dialog("请输入后置参数,须为正整数或者2位小数");
            return false;
        }
        para.ParameterS2 /= 100.0000;
        para.ParameterVal = paracontrol[i].getElementsByClassName("txtIntegralV2")[0].value;
        if (para.ParameterVal == "") {
            $.dialog("积分不能为空");
            return false;
        }
        if (!isNumber(para.ParameterVal)) {
            $.dialog("请输入积分,须为正整数或者2位小数");
            return false;
        }
        paraArray.push(para);
    }
    return paraArray;
}
checkPara3 = function (paracontrol) {
    var paraArray = new Array();
    for (var i = 0; i < paracontrol.length; i++) {
        var para = {
            ParaIndex: 0,
            ParameterOptionVal: "",
            ParameterVal: 0,
        };
        para.ParaIndex = i + 1;
        para.ParameterOptionVal = paracontrol[i].getElementsByClassName("selOptionCode")[0].value;
        if (para.ParameterOptionVal == "") {
            $.dialog("请选择下拉框的值");
            return false;
        }
        para.ParameterVal = paracontrol[i].getElementsByClassName("txtIntegralV3")[0].value;
        if (para.ParameterVal == "") {
            $.dialog("积分不能为空");
            return false;
        }
        if (!isNumber(para.ParameterVal)) {
            $.dialog("请输入积分,须为正整数或者2位小数");
            return false;
        }
        paraArray.push(para);
    }
    return paraArray;
}

/* 为控件赋值 */
Getpara = function (res, v) {
    if (v == "1") {
        Getpara1(res);
    } else if (v == "2") {
        Getpara2(res);
    } else if (v == "3") {
        var table = $("#selValue").find("option:selected").attr("algTable"),
        dictType = $("#selValue").find("option:selected").attr("algDictType");
        ajaxSync("/MemSubdivision/GetRightSelectData", { table: table, dictType: dictType }, function (data) {
            if (data) {
                var optionHtml = "";
                for (var i = 0; i < data.length; i++) {
                    optionHtml += "<option value='" + data[i].OptionValue + "'>" + data[i].OptionText + "</option>";
                }
                $("#selOptionCode").html(optionHtml);
                Getpara3(res);
            } else $.dialog(data.MSG);
        });
    }
}
Getpara1 = function (res) {
    for (var i = 0; i < res.length; i++) {
        paracount += 1;
        var a = addParaHTML1(paracount);
        a.show();
        $("#frmDiv").before(a);
        $("#txtMinNum" + paracount).val(res[i].ParameterMin);
        $("#txtMaxNum" + paracount).val(res[i].ParameterMax);
        $("#txtIntegralV1" + paracount).val(res[i].ParameterVal);
    }
}
Getpara2 = function (res) {
    for (var i = 0; i < res.length; i++) {
        paracount += 1;
        var a = addParaHTML2(paracount);
        a.show();
        $("#frmDiv").before(a);
        $("#txtStartPercentage" + paracount).val(res[i].ParameterS1 * 100);
        $("#txtEndPercentage" + paracount).val(res[i].ParameterS2 * 100);
        $("#txtIntegralV2" + paracount).val(res[i].ParameterVal);
    }
}
Getpara3 = function (res) {
    for (var i = 0; i < res.length; i++) {
        console.log(res[i].ParameterOptionVal);
        console.log(res[i].ParameterVal);
        paracount += 1;
        var a = addParaHTML3(paracount);
        a.show();
        $("#frmDiv").before(a);
        $("#selOptionCode" + paracount).val(res[i].ParameterOptionVal);
        $("#txtIntegralV3" + paracount).val(res[i].ParameterVal);
    }
}



