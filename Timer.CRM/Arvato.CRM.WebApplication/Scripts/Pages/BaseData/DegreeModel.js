var degreeID = 0;//标记判断是增加还是编辑
var paracount = 0;

$(function () {
    //绑定新增删除 Para 按钮事件
    ViewDataBind();

    //加载数据表格
    dt_ModelTable = $('#dt_ModelTable').dataTable({
        sAjaxSource: '/BaseData/GetDegreeModelList',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'DegreeModelID', title: "编号", sortable: true },
            { data: 'Name', title: "名称", sortable: false },
            { data: 'AddedDate', title: "添加时间", sortable: true },            
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"goEdit(" + obj.DegreeModelID + ")\">编辑</button>&nbsp;&nbsp;<button class=\"btn btn-danger\" id=\"btnDelete\" onclick=\"goDelete(" + obj.DegreeModelID + ")\">删除</button>";
                    return htm;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'degreeName', value: $("#SearchName").val() });
            
        }
    });
});


ViewDataBind = function () {
    BindAddParaBtn();
    BindSelectOptionCode();
    $("#btnSave").click(function () {
        BindBtnSave();
    });
    $("#btnSerach").click(function () {
        dt_ModelTable.fnDraw();
    });

    //重置
    $("#btnResetCancel").click(function () {
        $("#frmAddOption").children(".dimension-para").remove();
        $("#txtName").val("");
        $("#btnSave").attr("disabled", 'disabled');
        $("#splashyAdd").show();
        $(this).attr("disabled", "disabled");
        paracount = 0;
    });
}


goAdd = function () {
    degreeID = 0;
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
    degreeID = id;
    $("#addOption_dialog .heading h3").html("编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetDegreeModelById", { degreeID: id }, function (res) {
        if (res.IsPass) {
            var info = res.Obj[0];
            
            
            $("#txtName").val(info.Name);
            $("#btnResetCancel").removeAttr("disabled");
            $("#btnSave").removeAttr("disabled");
            $("#splashyAdd").hide();
            Getpara($.parseJSON(info.BasicContent), 3);
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
        ajax("/BaseData/DeleteDegreeModel", { degreeId: v }, function (res) {
            if (res.IsPass) {
                dt_ModelTable.fnDraw();
                $.dialog(res.MSG);
            } else $.dialog(res.MSG);
        });
    }
}

//清空数据
goClear = function () {
    $("#txtName").val("");
    $("#splashyAdd").show();
    $("#frmAddOption").children(".dimension-para").remove();
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


/* 新增参数配置 */
addParaHTML = function () {
    var v = $("#selOption").val();
    paracount += 1;
    var a;
    
    a = addParaHTML3(paracount);
    
    if (a != null) a.show();
    $("#frmDiv").before(a);
    //$("#txt_uiindex" + paracount).val(paracount);
}

addParaHTML3 = function (paracount) {
    var a = $("#paraType3").clone();
    a.attr("id", "paraType3" + paracount);
    //a.children(".dimension-para-btns").addClass("dimension-btnscount2");
    a.find("#selOptionCode").attr("id", "selOptionCode" + paracount);
    a.find("#txtIntegralV3").attr("id", "txtIntegralV3" + paracount);
    return a;
}

//绑定保存事件
BindBtnSave = function () {
    var paraArray = checkPara();
    if (!paraArray) {
        return;
    }
    var addModel = {
        DegreeModelID : degreeID,
        BasicContent: JSON.stringify(paraArray),
        Name: $("#txtName").val(),      
    };
    ajax("/BaseData/SaveModelDegree", addModel, function (res) {
        if (res.IsPass) {
            $.colorbox.close();
            dt_ModelTable.fnDraw();
            $.dialog(res.MSG);
        } else $.dialog(res.MSG);
    });
}

/* 保存参数配置 */
checkPara = function () {
    
    var paracontrol = $("#frmAddOption").children(".dimension-para");
    
    paraArray = checkPara3(paracontrol);
    
    return paraArray;
}



checkPara3 = function (paracontrol) {
    var paraArray = new Array();
    var paraValue = 0;
    var paraOptionVal = [];
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
            $.dialog("百分比不能为空");
            return false;
        }
        if (!isNumber(para.ParameterVal)) {
            $.dialog("请输入百分比,须为正整数或者2位小数");
            return false;
        }

        paraValue += parseInt(paracontrol[i].getElementsByClassName("txtIntegralV3")[0].value);
       

        if ($.inArray(paracontrol[i].getElementsByClassName("selOptionCode")[0].value, paraOptionVal)!=-1)
        {
            $.dialog("不能选择相同的选项");
            return false;
        }

        paraOptionVal.push(paracontrol[i].getElementsByClassName("selOptionCode")[0].value);

        paraArray.push(para);
    }

    if (paraValue != 100)
    {
        $.dialog("所有选项的百分比之和需为100");
        return false;
    }

    return paraArray;
}

Getpara = function (res, v) {
    if (v == "1") {
        Getpara1(res);
    } else if (v == "2") {
        Getpara2(res);
    } else if (v == "3") {
        //var table = $("#selValue").find("option:selected").attr("algTable"),
        //dictType = $("#selValue").find("option:selected").attr("algDictType");
        Getpara3(res);
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

//动态获取 编辑页面中 Type为下拉框的值
BindSelectOptionCode = function () {  
    ajaxSync("/BaseData/GetTargetData", {}, function (data) {
        var res = data.Obj[0];
        if (res) {
            var optionHtml = "";
            for (var i = 0; i < res.length; i++) {
                optionHtml += "<option value='" + res[i].AliasID2 + "'>" + res[i].FieldDesc + "</option>";
            }
            $("#selOptionCode").html(optionHtml);
        } else $.dialog(res.MSG);
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