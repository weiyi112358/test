$(function () {
    LoadBusinessType();
    //加载数据表格
    dt_BusinessSubType = $('#dt_BusinessSubType').dataTable({
        sAjaxSource: '/BaseData/GetBusinessSubType',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: false },
            { data: 'TypeBNameBase', title: "子类型名称", sortable: false },
            { data: 'TypeBCodeBase', title: "子类型代码", sortable: false },
            { data: 'ParentIDTypeA', title: "大类代码", sortable: false },
            //{ data: 'DataGroupID', title: "所属群组", sortable: false },
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    //var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"enableItem('" + obj.BaseDataID + "','" + (obj.EnableBrand == "undefined" ? "" : obj.EnableBrand) + "')\">" + (obj.EnableBrand == "1" ? "禁用" : "启用") + "</button>";
                    var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                    return htm;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'subTypeName', value: $.trim($("#txt_BusinessSubType").val()) });
            d.push({ name: 'typeName', value: $("#txt_BusinessType").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_BusinessSubType.fnDraw();
    });

    //保存数据
    $("#frmAddSubType").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var subType = {
                ID: $("#txtBrandId").val(),
                Type: $("#txtDataType").val(),
                Grade: 1,
                Name: encode($("#txtBrandName").val()),
                Code: $("#txtBrandCode").val(),
               
            }
            //增加
            if ($("#txtTypeId").val() == '') {
                ajax("/BaseData/AddSubType", { TypeCode: $("#addType").val(), SubTypeCode: $("#txtSubTypeCode").val(), SubTypeName: $("#txtSubTypeName").val(), GroupId: 1 }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_BusinessSubType.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
            else { //编辑
                ajax("/BaseData/UpdateSubType", { TypeCode: $("#addType").val(), SubTypeCode: $("#txtSubTypeCode").val(), SubTypeName: $("#txtSubTypeName").val(), GroupId: 1, BaseDataId: $("#txtTypeId").val() }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();

                        var start = dt_BusinessSubType.fnSettings()._iDisplayStart;
                        var length = dt_BusinessSubType.fnSettings()._iDisplayLength;
                        dt_BusinessSubType.fnPageChange(start / length, true);
                        //dt_BrandTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    });
});

//验证数据
var DataValidator = $("#frmAddSubType").validate({
    //onSubmit: false,
    rules: {
        txtBrandName: {
            required: true,
            maxlength: 20,
        },
        txtBrandCode: {
            required: true,
            maxlength: 20,
            isOnlyLN: true,
        },
        //editStoreClass: {
        //    required: true,
        //}
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//弹窗
function goEdit() {
    $("#addSubType_dialog .heading h3").html("子类型新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addSubType_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addSubType_dialog .heading h3").html("子类型编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetSubTypeById", { Id: id }, function (res) {
        $("#addType").val(res[0].ParentIDTypeA);
        $("#txtSubTypeName").val(res[0].TypeBNameBase);
        $("#txtSubTypeCode").val(res[0].TypeBCodeBase);
        $("#txtTypeId").val(res[0].BaseDataID);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addSubType_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//删除条目
function deleteItem(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteSubType", { BaseDataId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_BusinessSubType.fnDraw();
            } else {
                $.dialog(res.MSG);
            }
        });
    });
}

//是否启用
function enableItem(id, v) {
    ajax("/BaseData/EnableBrandById", { currentEnable: v, brandId: id }, function (res) {
        if (res.IsPass) {
            $.dialog(res.MSG);
            dt_BusinessSubType.fnDraw();
        } else {
            $.dialog(res.MSG);
        }
    });
}

//清空数据
function goClear() {
    $("#txtSubTypeCode").val('');
    $("#txtSubTypeName").val('');
    $("#addType").val('');
    $("#txtEnableBrand").val('1');
    $("#editStoreClass").val('');
    $("#txtDataGroupId").val('');
    $("#txtDataType").val('');

    $('.error-block').html('');
}

//加载门店所属群组
function LoadStoreType() {
    //$('#addItemClass').empty();
    ajax('/BaseData/GetStroeGroupList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].SubDataGroupID + '>' + res[i].SubDataGroupName + '</option>';
            }
            $('#drpStoreClass').append(opt);
            $('#addStoreClass').append(opt);
            $("#editStoreClass").append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStoreClass').append(opt);
            $('#addStoreClass').append(opt);
            $("#editStoreClass").append(opt);
        }
    });
}

//加载门店所属群组
function LoadBusinessType() {
    //$('#addItemClass').empty();
    ajax('/BaseData/GetTypeAList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].TypeACodeBase + '>' + res[i].TypeANameBase + '</option>';
            }
            $('#txt_BusinessType').append(opt);
            $('#addType').append(opt);
            
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#txt_BusinessType').append(opt);
            $('#addType').append(opt);
            
        }
    });
}