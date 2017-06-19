$(function () {
    LoadStoreType();
    //加载数据表格
    dt_BrandTable = $('#dt_BrandTable').dataTable({
        sAjaxSource: '/BaseData/GetVehicleBrandInfo',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "编号", sortable: true },
            { data: 'Name', title: "品牌名称", sortable: false },
            //{ data: 'SubDataGroupName', title: "所属群组", sortable: false },
            { data: 'Code', title: "品牌代码", sortable: false },
            //{
            //    data: null,
            //    title: "是否启用",
            //    sortable: false,
            //    render: function (obj) {
            //        return (obj.EnableBrand != null && obj.EnableBrand == "1" ? "启用" : "禁用");
            //    }
            //},
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    //var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"enableItem('" + obj.BaseDataID + "','" + (obj.EnableBrand == "undefined" ? "" : obj.EnableBrand) + "')\">" + (obj.EnableBrand == "1" ? "禁用" : "启用") + "</button>";
                    var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.ID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.ID + ")\">删除</button>";
                    return htm;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'brandName', value: $.trim($("#txt_BrandName").val()) });
            //d.push({ name: 'groupId', value: $("#drpStoreClass").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_BrandTable.fnDraw();
    });

    //保存数据
    $("#frmAddBrand").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var brand = {
                ID: $("#txtBrandId").val(),
                //DataGroupID: $("#txtDataGroupId").val(),
                //DataGroupID: $("#editStoreClass").val(),
                Type: $("#txtDataType").val(),
                Grade: 1,
                Name: encode($("#txtBrandName").val()),
                Code: $("#txtBrandCode").val(),
                //EnableBrand: $("#txtEnableBrand").val()
            }
            //增加
            if (brand.ID == '') {
                ajax("/BaseData/AddVehicleData", { vehicle: brand }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_BrandTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
            else { //编辑
                ajax("/BaseData/UpdateVehicleData", { vehicle: brand }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();

                        var start = dt_BrandTable.fnSettings()._iDisplayStart;
                        var length = dt_BrandTable.fnSettings()._iDisplayLength;
                        dt_BrandTable.fnPageChange(start / length, true);
                        //dt_BrandTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    });
});

//验证数据
var DataValidator = $("#frmAddBrand").validate({
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
    $("#addBrand_dialog .heading h3").html("品牌新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addBrand_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addBrand_dialog .heading h3").html("品牌编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetVehicleById", { vehicleId: id }, function (res) {
        $("#txtBrandId").val(res.ID);
        $("#txtBrandName").val(res.Name);
        $("#txtBrandCode").val(res.Code);
        //$("#editStoreClass").val(res.DataGroupID);
        //$("#txtDataGroupId").val(res.DataGroupID);
        $("#txtDataType").val(res.Type);
        //$("#txtEnableBrand").val(res.EnableBrand);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addBrand_dialog",
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
        ajax("/BaseData/DeleteVehicleById", { vehicleId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_BrandTable.fnDraw();
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
    $("#txtBrandId").val('');
    $("#txtBrandName").val('');
    $("#txtBrandCode").val('');
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