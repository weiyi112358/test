var aliasID = '';//标记判断是增加还是编辑
var paracount = 0;
$(function () {
    dt_dimension = $('#dt_dimension').dataTable({
        sAjaxSource: '/BaseData/GetDimensionList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'AliasID', title: "编号", sortable: true },
            //{ data: 'FieldName', title: "字段名称", sortable: false },
            { data: 'FieldAlias', title: "字段别名", sortable: false },
            { data: 'FieldDesc', title: "字段描述", sortable: false },
            { data: 'AliasKey1', title: "字段类型", sortable: false },
            { data: 'AddedDate', title: "创建时间", sortable: true },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"goEdit(" + obj.AliasID + ")\">编辑</button> <button class=\"btn\" id=\"btnDelete\" onclick=\"goDelete(" + obj.AliasID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'fieldDesc', value: $("#txtFieldDesc").val() });
            d.push({ name: 'fieldAlias', value: $("#txtFieldAlias").val() });
            d.push({ name: 'fieldType', value: $("#drpFieldType").val() });
        }
    });

    //查询
    $('#btnSearch').click(function () {
        dt_dimension.fnDraw();
    })
    
    //忠诚度可用时禁止选择动态参数
    $("#ckb_loyal").change(function () {
        if ($("#ckb_loyal").attr("checked") == "checked") {
            $("#ckb_isdynamic").attr("disabled", "disabled")
        }
        else {
            $("#ckb_isdynamic").removeAttr("disabled")
        }
    })

    //自动添加第一个动态参数并禁止选择忠诚度
    $("#ckb_isdynamic").change(function () {
        if ($("#ckb_isdynamic").attr("checked") == "checked") {
            $("#ckb_loyal").attr("disabled", "disabled")
            addParaHTML();
        }
        else {
            $("#ckb_loyal").removeAttr("disabled")
            $(".form-inline").children(".dimension-para").remove();
        }
    })

    //清空搜索的输入信息
    $('#btnClear').click(function () {
        $('#txtFieldDesc').val('');
        $('#txtFieldAlias').val('');
        $('#drpFieldType').val('');
    })

    $("body").delegate(".splashy-add_small", "click", function () {
        addParaHTML();
    });

    $("body").delegate(".splashy-remove_minus_sign_small", "click", function () {
        if ($(".dimension-para").length > 2) {
            $(this).parents(".dimension-para").remove();
            paracount = 0;
        }
    });

    //保存编辑信息
    $("#frmEditDimension").submit(function (e) {
        e.preventDefault();

        goSave();
    })

    //加载字段类型下拉框
    GetFieldType();
    //加载运行类型
    GetRunType();
    //加载表格
    GetRelationship();

    //弹窗清空数据
    $('#btn_clear').click(function () {
        $("#txt_fielddesc").val('');
        $("#txt_fieldalias").val('');
        $("#drp_fieldtype").val('-1');
        $("#drp_controltype").val('input');
        $("#txt_datasource").val('');
        $("#txt_datasourcevalue").val('');
        $("#ckb_subdivision").prop('checked', false);
        $("#ckb_loyal").prop('checked', false);
        $("#txt_reg").val('');
        $("#drp_runtype").val('-1');
        $("#txt_script").val('');
        $("#drp_relationship").val('-1');
        $("#ckb_isdynamic").prop('checked', false);
        $(".form-inline").children(".dimension-para").remove();
        paracount = 0;
    })
})

//编辑弹窗
function goEdit(aid) {
    $('#btn_clear').click();//每次弹窗先清空数据
    if (aid != '') {
        $('#table_editattribute .modal-header h3').html('维度编辑');
        aliasID = aid;
        ajax("/BaseData/GetDimensionById", { aliasId: aid }, function (res) {

            $("#txt_fielddesc").val(res.FieldDesc);
            $("#txt_fieldalias").val(res.FieldAlias);
            $("#drp_fieldtype").val(res.FieldType);
            $("#drp_controltype").val(res.ControlType);
            $("#txt_datasource").val(res.DictTableName);
            $("#txt_datasourcevalue").val(res.DictTableType);
            $("#ckb_subdivision").prop('checked', res.IsFilterBySubdivision);
            $("#ckb_loyal").prop('checked', res.IsFilterByLoyRule);
            $("#txt_reg").val(res.Reg);
            $("#drp_runtype").val(res.RunType);
            $("#txt_script").val(res.ComputeScript);
            $("#drp_relationship").val(res.TableName + ',' + res.AliasType);
            $("#ckb_isdynamic").prop('checked', res.IsDynamicAlias);
            if (res.IsDynamicAlias) {
                Getpara(aid);
            }
            //$("#txt_para").val(res.ParameterCount);
        });
    } else { aliasID = ''; $('#btn_clear').click(); $('#table_editattribute .modal-header h3').html('维度新增'); }

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editdimension",
        inline: true
    });
}
//验证数据
var DataValidator = $("#frmEditDimension").validate({
    //onSubmit: false,
    rules: {
        txt_fielddesc: {
            required: true,
        },
        txt_fieldalias: {
            required: true,
            noWhiteSpaceStr: true,
        },
        drp_fieldtype: {
            required: true,
        },
        drp_controltype: {
            required: true,
        },
        drp_runtype: {
            required: true,
        },
        txt_script: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存新增信息或者修改过的信息
function goSave() {
    var paraArray = checkPara();
    if (!paraArray) {
        return;
    }
    if (DataValidator.form()) {
        var fieldAlias = {
            AliasID: aliasID,
            FieldDesc: $("#txt_fielddesc").val(),
            FieldAlias: $("#txt_fieldalias").val(),
            FieldType: $("#drp_fieldtype").val(),
            ControlType: $("#drp_controltype").val(),
            DictTableType: $("#txt_datasourcevalue").val(),
            DictTableName: $("#txt_datasource").val(),
            IsFilterBySubdivision: $("#ckb_subdivision").prop("checked"),
            IsFilterByLoyRule: $("#ckb_loyal").prop("checked"),
            Reg: encode($("#txt_reg").val()),
            RunType: $("#drp_runtype").val(),
            ComputeScript: encode($("#txt_script").val()),
            IsDynamic: $("#ckb_isdynamic").prop("checked"),
            ParameterCount: paraArray.length,
            FieldPara: paraArray,

            //AliasType: ($("#drp_relationship").val()).split(',')[1],
            //TableName: ($("#drp_relationship").val()).split(',')[0],
        }
        var postUrl = "/BaseData/AddorUpdateDimensionData";
        ajax(postUrl, fieldAlias, function (res) {
            if (res.IsPass) {
                $.colorbox.close();
                var start = dt_dimension.fnSettings()._iDisplayStart;
                var length = dt_dimension.fnSettings()._iDisplayLength;
                dt_dimension.fnPageChange(start / length, true);
                //dt_dimension.fnDraw();
                $.dialog(res.MSG);
            } else { $.dialog(res.MSG); }
        });
    }
}
//删除数据
function goDelete(aid) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteDimensionById", { aliasId: aid }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_dimension.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//获取要维护表格列表
function GetRelationship() {
    $('#drp_relationship').empty();
    ajax("/BaseData/GetRelationshipList", null, function (res) {
        if (res.length > 0) {
            var opt = "<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + ',' + res[i].ReferenceOptionType + '>' + res[i].OptionText + '</option>';
            }
            $('#drp_relationship').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_relationship').append(opt);
        }
    });
}

//获取字段类型——下拉框选项
function GetFieldType() {
    ajax("/BaseData/GetFieldTypeList", null, function (res) {
        if (res.length > 0) {
            var opt = "<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpFieldType').append(opt);
            $('#drp_paratype').append(opt);
            $('#drp_fieldtype').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpFieldType').append(opt);
            $('#drp_fieldtype').append(opt);
        }
    });
}

//获取运行类型
function GetRunType() {
    ajax("/BaseData/GetRunTypeList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drp_runtype').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_runtype').append(opt);
        }
    });
}

function addParaHTML() {
    paracount += 1
    var a = $("#para_control").clone();
    a.attr("id", "para_control" + paracount);
    a.children(".dimension-para-btns").addClass("dimension-btnscount2");
    a.find("#txt_paraname").attr("id", "txt_paraname" + paracount);
    a.find("#txt_parareg").attr("id", "txt_parareg" + paracount);
    a.find("#cb_isrequired").attr("id", "cb_isrequired" + paracount);
    a.find("#drp_paratype").attr("id", "#rp_paratype" + paracount);
    a.find("#drp_paratcontrolype").attr("id", "drp_paratcontrolype" + paracount);
    a.find("#txt_paradicname").attr("id", "txt_paradicname" + paracount);
    a.find("#txt_paradictype").attr("id", "txt_paradictype" + paracount);
    a.find("#txt_uiindex").attr("id", "txt_uiindex" + paracount);
    a.show();
    $("#content_script").before(a);
    $("#txt_uiindex" + paracount).val(paracount);
}

function checkPara() {
    var paracontrol = $(".form-inline .dimension-para");
    var paraArray = new Array();
    for (var i = 0; i < paracontrol.length; i++) {
        var para = {
            ParaIndex: 0,
            ParameterName: "",
            Reg: "",
            FieldType: "",
            ControlType: "",
            DictTableName: "",
            DictTableType: "",
            UIIndex: 0,
            IsRequired:"false"
        }
        para.ParaIndex = i + 1;
        para.ParameterName = paracontrol[i].getElementsByClassName("txt_paraname")[0].value;
        if (para.ParameterName == "") {
            $.dialog("参数名称不能为空");
            return false;
        }
        para.Reg = paracontrol[i].getElementsByClassName("txt_parareg")[0].value;
        para.FieldType = paracontrol[i].getElementsByClassName("drp_paratype")[0].options[paracontrol[i].getElementsByClassName("drp_paratype")[0].selectedIndex].value
        if (para.FieldType == "-1") {
            $.dialog("请选择参数类型");
            return false;
        }
        para.ControlType = paracontrol[i].getElementsByClassName("drp_paratcontrolype")[0].options[paracontrol[i].getElementsByClassName("drp_paratcontrolype")[0].selectedIndex].value
        para.DictTableName = paracontrol[i].getElementsByClassName("txt_paradicname")[0].value;
        para.DictTableType = paracontrol[i].getElementsByClassName("txt_paradictype")[0].value;
        para.UIIndex = paracontrol[i].getElementsByClassName("txt_uiindex")[0].value;
        if (para.UIIndex == "") {
            $.dialog("参数顺序不能为空");
            return false;
        }
        para.IsRequired = paracontrol[i].getElementsByClassName("cb_isrequired")[0].checked;
        paraArray.push(para);
    }
    return paraArray;
}

function Getpara(aliasID) {
    ajax("/BaseData/GetParaList", { aliasID: aliasID }, function (res) {
        for (var i = 0; i < res.length; i++) {
            paracount += 1
            var a = $("#para_control").clone();
            a.attr("id", "para_control" + paracount);
            a.children(".dimension-para-btns").addClass("dimension-btnscount2");
            a.find("#txt_paraname").attr("id", "txt_paraname" + paracount);
            a.find("#txt_parareg").attr("id", "txt_parareg" + paracount);
            a.find("#cb_isrequired").attr("id", "cb_isrequired" + paracount);
            a.find("#drp_paratype").attr("id", "drp_paratype" + paracount); 
            a.find("#drp_paratcontrolype").attr("id", "drp_paratcontrolype" + paracount);
            a.find("#txt_paradicname").attr("id", "txt_paradicname" + paracount);
            a.find("#txt_paradictype").attr("id", "txt_paradictype" + paracount);
            a.find("#txt_uiindex").attr("id", "txt_uiindex" + paracount);
            a.show();
            $("#content_script").before(a);
            $("#txt_paraname" + paracount).val(res[i].ParameterName);
            $("#txt_parareg" + paracount).val(res[i].Reg);
            $("#cb_isrequired" + paracount).prop('checked', res[i].IsRequired);
            $("#drp_paratype" + paracount).val(res[i].FieldType);
            $("#drp_paratcontrolype" + paracount).val(res[i].ControlType);
            $("#txt_paradicname" + paracount).val(res[i].DictTableName);
            $("#txt_paradictype" + paracount).val(res[i].DictTableType); 
            $("#txt_uiindex" + paracount).val(res[i].UIIndex);
        }
    })
}