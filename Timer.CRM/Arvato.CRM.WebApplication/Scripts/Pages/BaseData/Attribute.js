var aliasID = '';//标记判断是增加还是编辑
$(function () {
    //$.ajaxSetup({
    //    async: false
    //});

    dt_attribute = $('#dt_attribute').dataTable({
        sAjaxSource: '/BaseData/GetAttributeList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[7, "desc"]],
        aoColumns: [
            { data: 'AliasID', title: "编号", sortable: true },
            //{ data: 'FieldName', title: "字段名称", sortable: false },
            { data: 'AliasKey2', title: "字段类型", sortable: false },
            { data: 'FieldAlias', title: "字段别名", sortable: false },
            { data: 'AliasType', title: "关系", sortable: false },
            { data: 'AliasKey', title: "关系关键字", sortable: false },
            {
                data: 'AliasSubKey', title: "关系子关键字", sortable: false,
            },
            { data: 'FieldDesc', title: "字段描述", sortable: false },
            { data: 'ModifiedDate', title: "修改时间", sortable: true },
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
        dt_attribute.fnDraw();
    })

    //清空搜索的输入信息
    $('#btnClear').click(function () {
        $('#txtFieldDesc').val('');
        $('#txtFieldAlias').val('');
        $('#drpFieldType').val('');
    })

    //保存编辑信息
    $('#btnSave').click(function () {
        goSave();
    })

    //加载字段类型下拉框
    GetFieldType();
    //加载关系列表
    //GetRelationship();
    //加载别名键值
    //GetAliasKeyData();
    //加载关系列表
    GetRelationship();
    GetOptionData('');
    GetOptionSubData('');
    //关系下拉框改变事件
    $('#drp_relationship').change(function () {
        //$('#drp_catgkey').empty();C:\4SCRM\Arvato.CRM\Arvato.CRM.WebApplication\Gebo/
        GetOptionSubData('');
        var relation = $('#drp_relationship').val();
        if (relation != null) {
            var aliasType = relation.split(',')[1];
            GetOptionData(aliasType);
            $('#drp_catgkey').change();
        }
    })
    $('#drp_catgkey').change(function () {
        var relation = $('#drp_catgkey').val();
        if (relation != '-1' && relation != null) {
            var aliasType = relation.split(',')[1];
            if (aliasType)
                GetOptionSubData(aliasType);
        }
    })

    //弹窗清空数据
    $('#btn_clear').click(function () {
        $("#txt_fielddesc").val('');
        $("#txt_fieldalias").val('');
        $("#drp_fieldtype").val('-1');
        $("#drp_controltype").val('input');
        $("#ckb_tmpcommu").prop('checked', false);
        $("#txt_datasource").val('');
        $("#txt_datasourcetype").val('');
        $("#ckb_subdivision").prop('checked', false);
        $("#ckb_loyal").prop('checked', false);
        $("#txt_reg").val('');


        $('#drp_relationship').change();
        $("#drp_catgkey").val('');//??
        $("#drp_catgsubkey").val('');
    })



    //保存编辑信息
    $("#frmEditAttribute").submit(function (e) {
        e.preventDefault();
        goSave();
    })
})

//编辑弹窗
function goEdit(aid) {
    $('#btn_clear').click();//每次弹窗先清空数据
    if (aid != '') {
        $('#table_editattribute .modal-header h3').html('属性编辑');
        aliasID = aid;
        ajax("/BaseData/GetDimensionById", { aliasId: aid }, function (res) {

            $("#txt_fielddesc").val(res.FieldDesc);
            $("#txt_fieldalias").val(res.FieldAlias);
            $("#drp_fieldtype").val(res.FieldType);
            $("#drp_controltype").val(res.ControlType);
            $("#ckb_tmpcommu").prop('checked', res.IsCommunicationTemplet);
            $("#txt_datasource").val(res.DictTableName);
            $("#txt_datasourcetype").val(res.DictTableType);
            $("#txt_reg").val(res.Reg);
            $("#ckb_subdivision").prop('checked', res.IsFilterBySubdivision);
            $("#ckb_loyal").prop('checked', res.IsFilterByLoyRule);

            $("#drp_relationship").val(res.TableName + ',' + res.AliasType).change();

            $("#drp_catgkey").val(res.AliasKey+','+res.ReferenceOptionType).change();

            $("#drp_catgsubkey").val(res.AliasSubKey);
        });
    } else { aliasID = ''; $('#btn_clear').click(); $('#table_editattribute .modal-header h3').html('属性新增'); }
    $('#drp_relationship').change();
    $('#drp_catgkey').change();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editattribute",
        inline: true
    });
}

//验证数据
var DataValidator = $("#frmEditAttribute").validate({
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
        drp_relationship: {
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
    if (DataValidator.form()) {
        var aliasKey = $("#drp_catgkey").val();
        var aliasSubKey = $("#drp_catgsubkey").val();
        var relation = $("#drp_relationship").val();
        var fieldAlias = {
            AliasID: aliasID,
            FieldDesc: $("#txt_fielddesc").val(),
            FieldAlias: $("#txt_fieldalias").val(),
            FieldType: $("#drp_fieldtype").val(),
            ControlType: $("#drp_controltype").val(),
            IsCommunicationTemplet: $("#ckb_tmpcommu").prop("checked"),
            AliasType: relation.split(',')[1],
            TableName: relation.split(',')[0],
            DictTableName: $("#txt_datasource").val(),
            DictTableType: $("#txt_datasourcetype").val(),
            IsFilterBySubdivision: $("#ckb_subdivision").prop("checked"),
            IsFilterByLoyRule: $("#ckb_loyal").prop("checked"),
            Reg: encode($("#txt_reg").val()),
            RunType: ($("#drp_relationship").val()).split(',')[0],
            AliasKey: aliasKey == null ? null : aliasKey.split(',')[0],//有可能没有值
            AliasSubKey: aliasSubKey == null ? null : aliasSubKey.split(',')[0],//有可能没有值
        }
        var postUrl = "/BaseData/AddorUpdateAttributeData";
        ajax(postUrl, fieldAlias, function (res) {
            if (res.IsPass) {
                //$.colorbox.close();
                //$('#btnSearch').click();
                //dt_attribute.fnDraw();
                var start = dt_attribute.fnSettings()._iDisplayStart;
                var length = dt_attribute.fnSettings()._iDisplayLength;
                dt_attribute.fnPageChange(start / length, true);
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
                //$('#btnSearch').click();
                dt_attribute.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
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
            $('#drp_fieldtype').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpFieldType').append(opt);
            $('#drp_fieldtype').append(opt);
        }
    });
}

//获取属性关系列表
function GetRelationship() {
    $('#drp_relationship').empty();
    ajaxSync("/BaseData/GetRelationshipList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + ',' + res[i].ReferenceOptionType + '>' + res[i].OptionText + '</option>';
            }
            $('#drp_relationship').append(opt);

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_relationship').append(opt);
        }
        //GetOptionData($('#drp_relationship').val().split(',')[1]);
    });
}

//获取属性关系三级列表信息
function GetOptionData(type) {
    $('#drp_catgkey').empty();
    ajaxSync("/BaseData/GetOptionDataList", { optType: type }, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + ',' + res[i].ReferenceOptionType + '>' + res[i].OptionText + '</option>';
            }
            //return opt;
            $('#drp_catgkey').append(opt);

        } else {
            var opt = "<option value=''>-无-</option>";
            //return opt;
            $('#drp_catgkey').append(opt);
        }
        //GetOptionSubData($('#drp_catgkey').val().split(',')[1]);
    });
}

//获取属性关系三级列表信息
function GetOptionSubData(type) {
    $('#drp_catgsubkey').empty();
    ajaxSync("/BaseData/GetOptionDataList", { optType: type }, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            //return opt;
            $('#drp_catgsubkey').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            //return opt;
            $('#drp_catgsubkey').append(opt);
        }
    });
}

//获取别名键值列表信息
//function GetAliasKeyData() {
//    $('#txt_aliaskey').empty();
//    ajax("/BaseData/GetOptionDataList", { optType: "BaseDataType" }, function (res) {
//        if (res.length > 0) {
//            var opt = "";//"<option value='-1'>请选择</option>";
//            for (var i = 0; i < res.length; i++) {
//                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
//            }
//            //return opt;
//            $('#txt_aliaskey').append(opt);
//        } else {
//            var opt = "<option value=''>-无-</option>";
//            //return opt;
//            $('#txt_aliaskey').append(opt);
//        }
//    });
//}