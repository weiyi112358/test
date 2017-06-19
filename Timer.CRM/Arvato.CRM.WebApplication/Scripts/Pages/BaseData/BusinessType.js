$(function () {
    LoadStoreType();
    dt_BusinessType = $('#dt_BusinessType').dataTable({
        sAjaxSource: '/BaseData/GetBusinessType',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumnDefs: [
            {"bVisible": true, "aTargets": [ 0 ] },
        ] ,
        aoColumns: [
            
            { data: 'BaseDataID', title: "编号", sortable: false },
            { data: 'TypeANameBase', title: "类型名称", sortable: false },
            { data: 'TypeACodeBase', title: "类型代码", sortable: false },
            { data: 'SubDataGroupName', title: "所属群组", sortable: false },
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                    return htm;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'typeName', value: $.trim($("#txt_BusinessType").val()) });
            d.push({ name: 'groupId', value: $("#drpStoreClass").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_BusinessType.fnDraw();
    });

    //保存数据
    $("#frmAddType").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var type = {
                Name: encode($("#txtTypeName").val()),
                Code: $("#txtTypeCode").val(),
                
            }
            //增加
            if ($("#txtTypeId").val() == '') {
                ajax("/BaseData/AddType", { TypeCode: $("#txtTypeCode").val(), TypeName: $("#txtTypeName").val(), GroupId: $("#editStoreClass").val() }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_BusinessType.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
            else { //编辑
                ajax("/BaseData/UpdateType", { TypeCode: $("#txtTypeCode").val(), TypeName: $("#txtTypeName").val(), GroupId: $("#editStoreClass").val(), BaseDataId: $("#txtTypeId").val() }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();

                        var start = dt_BusinessType.fnSettings()._iDisplayStart;
                        var length = dt_BusinessType.fnSettings()._iDisplayLength;
                        dt_BusinessType.fnPageChange(start / length, true);
                        //dt_BrandTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    });
});

//验证数据
var DataValidator = $("#frmAddType").validate({
    //onSubmit: false,
    rules: {
        txtTypeName: {
            required: true,
            maxlength: 20,
        },
        txtTypeCode: {
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
    $("#addType_dialog .heading h3").html("大类新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addType_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addType_dialog .heading h3").html("大类编辑");
    
    ajax("/BaseData/GetTypeById", { Id: id }, function (res) {
        
        $("#txtTypeName").val(res[0].TypeANameBase);
        $("#txtTypeCode").val(res[0].TypeACodeBase);
        $("#txtTypeId").val(res[0].BaseDataID);
        
        
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addType_dialog",
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
        ajax("/BaseData/DeleteType", { BaseDataId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_BusinessType.fnDraw();
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
            dt_BrandTable.fnDraw();
        } else {
            $.dialog(res.MSG);
        }
    });
}

//清空数据
function goClear() {
    $("#txtTypeId").val('');
    $("#txtTypeName").val('');
    $("#txtTypeCode").val('');
    

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


