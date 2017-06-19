var dt_VehicleBrandTable;
var dt_VehicleSeriesTable;
var dt_VehicleTypeTable;
$(function () {
    $.ajaxSetup({
        async: false
    });

    $(".chzn_a").chosen();
    //加载车辆品牌列表
    LoadVehicleBrand();

    //搜索
    $("#btnSearch").click(function () {
        var brand = $("#drpVehicleBrand").val();
        var series = $("#drpVehicleType").val();
        var type = $("#drpVehicleSeries").val();
        if (brand != "" && series != "") {
            loadBrandTable();
            loadSeriesTable();
            loadTypeTable();
        }
        else if (brand != "" && series == "") {
            loadBrandTable();
            loadSeriesTable();
        } else {
            loadBrandTable();
        }
    })



    //保存数据
    $("#frmAddVehicleBrand").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator_Brand.form()) {
            var vehicle = {
                ID: $("#txtBrandId").val(),
                Name: $("#addBrand").val(),
                Code: $("#addBrandCode").val(),
                Grade: 1,
            }
            //增加
            if (vehicle.ID == '') {
                ajax("/BaseData/AddVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        loadBrandTable();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        loadBrandTable();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })

    //保存数据  车系
    $("#frmAddVehicleSeries").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator_Series.form()) {
            var vehicle = {
                ID: $("#txtSeriesId").val(),
                Name: $("#addSeriesName").val(),
                Code: $("#addSeriesCode").val(),
                ParentCode: $("#drpBrand").val(),
                Grade: 2,
            }
            //增加
            if (vehicle.ID == '') {
                ajax("/BaseData/AddVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        //loadSeriesTable();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        //loadSeriesTable();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })

    //保存数据  车型
    $("#frmAddVehicleType").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator_Type.form()) {
            var vehicle = {
                ID: $("#txtTypeId").val(),
                Name: $("#addTypeName").val(),
                Code: $("#addTypeCode").val(),
                ParentCode: $("#drpSeries").val(),
                Grade: 3,
            }
            //增加
            if (vehicle.ID == '') {
                ajax("/BaseData/AddVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        //loadTypeTable();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        //loadTypeTable();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })

    //品牌改变
    $('#drpVehicleBrand').change(function () {
        var brandId = $('#drpVehicleBrand').val();
        if (brandId != '') {

            LoadVehicleLevel(brandId);
        } else {
            $('#drpVehicleSeries').html("<option value=''>-无-</option>");
            $('#drpVehicleType').html("<option value=''>-无-</option>");
        }
    })
    //车型改变
    $('#drpVehicleType').change(function () {
        var brandId = $('#drpVehicleBrand').val();
        var typeId = $('#drpVehicleType').val();
        if (typeId != '') {
            LoadVehicleSeries(typeId, brandId);
        } else {
            $('#drpVehicleSeries').html("<option value=''>-无-</option>");
        }
    })
    //品牌改变--弹窗
    $('#drpBrand_Type').change(function () {
        var brandId = $('#drpBrand_Type').val();
        if (brandId != '') {

            LoadVehicleLevelDiv(brandId);
        } else {
            $('#drpSeries').html("<option value=''>-无-</option>");
        }
    })
    //车型改变--弹窗
    $('#addLevel').change(function () {
        var brandId = $('#addBrand').val();
        var typeId = $('#addLevel').val();
        if (typeId != '') {
            LoadVehicleSeriesDiv(typeId, brandId);
        } else {
            $('#addSeries').html("<option value=''>-无-</option>");
        }
    })


})

//加载车辆品牌
function LoadVehicleBrand() {
    $('#drpVehicleBrand').empty();
    ajax('/BaseData/GetVehicleBrandList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drpVehicleBrand').append(opt).trigger("liszt:updated");;
            $('#drpBrand').append(opt);
            $('#drpBrand_Type').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVehicleBrand').append(opt).trigger("liszt:updated");;
            $('#drpBrand').append(opt);
            $('#drpBrand_Type').append(opt);
        }
    });
}
//加载车辆车系
function LoadVehicleSeries(typeId, brandId) {
    $('#drpVehicleSeries').empty();
    ajax('/BaseData/GetVehicleSeriesList', { typeId: typeId, brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drpVehicleSeries').append(opt).trigger("liszt:updated");;
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVehicleSeries').append(opt).trigger("liszt:updated");;
        }
    });
}
//加载车辆车系--弹窗
function LoadVehicleSeriesDiv(typeId, brandId) {
    $('#addSeries').empty();
    $.post('/BaseData/GetVehicleSeriesList', { typeId: typeId, brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#addSeries').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#addSeries').append(opt);
        }
    });
}
//加载车型
function LoadVehicleLevel(brandId) {
    $('#drpVehicleType').empty();
    ajax('/BaseData/GetVehicleLevelList', { brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drpVehicleType').append(opt).trigger("liszt:updated");;
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVehicleType').append(opt).trigger("liszt:updated");;
        }
    });
}
//加载车型--弹窗
function LoadVehicleLevelDiv(brandId) {
    $('#drpSeries').empty();
    $.post('/BaseData/GetVehicleLevelList', { brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drpSeries').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpSeries').append(opt);
        }
    });
}
//验证数据  品牌
var DataValidator_Brand = $("#frmAddVehicleBrand").validate({
    //onSubmit: false,
    rules: {
        addBrand: {
            required: true,
            maxlength: 20,
        },
        addBrandCode: {
            required: true,
            maxlength: 20,
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//验证数据  车系
var DataValidator_Series = $("#frmAddVehicleSeries").validate({
    //onSubmit: false,
    rules: {
        drpBrand: {
            required: true,
        },
        addSeriesName: {
            required: true,
            maxlength: 20,
        },
        addSeriesCode: {
            required: true,
            maxlength: 20,
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//验证数据  车型
var DataValidator_Type = $("#frmAddVehicleType").validate({
    //onSubmit: false,
    rules: {
        drpBrand_Type: {
            required: true,
        },
        drpSeries: {
            required: true,
        },
        addTypeName: {
            required: true,
            maxlength: 20,
        },
        addTypeCode: {
            required: true,
            maxlength: 20,
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//添加弹窗----新增品牌
function addBrand() {

    $("#addVehicle_Brand .heading h3").html("新增品牌");
    //清空数据
    $("#addBrand").val('');
    $("#addBrandCode").val('');
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addVehicle_Brand",
        inline: true
    });
}

//添加弹窗----新增车系
function addSeries() {

    $("#addVehicle_Series .heading h3").html("新增车系");
    //清空数据
    $("#drpBrand").val('');
    $("#addSeriesName").val('');
    $("#addSeriesCode").val('');
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addVehicle_Series",
        inline: true
    });
}

//添加弹窗----新增车型
function addType() {

    $("#addVehicle_Type .heading h3").html("新增车型");
    //清空数据
    $("#drpBrand_Type").val('');
    $("#drpSeries").val('');
    $("#addTypeName").val('');
    $("#addTypeCode").val('');
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addVehicle_Type",
        inline: true
    });
}

//编辑品牌
function editBrand(id) {

    $("#addVehicle_Brand .heading h3").html("编辑车款");
    //清空数据
    $("#addBrand").val('');
    $("#addBrandCode").val('');

    $("#txtBrandId").val(id);

    ajax("/BaseData/GetVehicleById", { vehicleId: id }, function (res) {
        $("#addBrand").val(res.Name);
        $("#addBrandCode").val(res.Code);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addVehicle_Brand",
        inline: true
    });
}
//编辑品牌
function editSeries(id) {

    $("#addVehicle_Series .heading h3").html("编辑车系");
    //清空数据

    $("#drpBrand").val('');
    $("#addSeriesName").val('');
    $("#addSeriesCode").val('');


    $("#txtSeriesId").val(id);
    ajax("/BaseData/GetVehicleById", { vehicleId: id }, function (res) {

        $("#drpBrand").val(res.ParentCode);
        $("#addSeriesName").val(res.Name);
        $("#addSeriesCode").val(res.Code);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addVehicle_Series",
        inline: true
    });
}

//编辑品牌
function editType(id) {

    $("#addVehicle_Type .heading h3").html("编辑车型");
    //清空数据

    $("#drpBrand_Type").val('');
    $("#drpSeries").val('');
    $("#addTypeName").val('');
    $("#addTypeCode").val('');


    $("#txtTypeId").val(id);
    ajax("/BaseData/GetVehicleById", { vehicleId: id }, function (res) {

        $("#drpBrand_Type").val($("#drpVehicleBrand").val()).change();
        $("#drpSeries").val(res.ParentCode);
        $("#addTypeName").val(res.Name);
        $("#addTypeCode").val(res.Code);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addVehicle_Type",
        inline: true
    });
}
//删除
function deleteData(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteVehicleById", { vehicleId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_VehicleBrandTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}


function loadBrandTable() {
    if (!dt_VehicleBrandTable) {
        //加载品牌数据表格
        dt_VehicleBrandTable = $('#dt_VehicleBrandTable').dataTable({
            sAjaxSource: '/BaseData/GetVehicleBrandData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: 'ID', title: "编号", sortable: true },
                { data: 'Name', title: "品牌名称", sortable: false },
                { data: 'Code', title: "品牌代码", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<button class=\"btn\" id=\"btnModify\"  onclick=\"editBrand(" + obj.ID + ")\">编辑</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteData(" + obj.ID + ")\">删除</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'vehicleBrand', value: $("#drpVehicleBrand").val() });
            }
        });
    } else {
        dt_VehicleBrandTable.fnDraw();
    }
}

function loadSeriesTable() {
    if (!dt_VehicleSeriesTable) {
        dt_VehicleSeriesTable = $('#dt_VehicleSeriesTable').dataTable({
            sAjaxSource: '/BaseData/GetVehicleSeriesInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: 'ID', title: "编号", sortable: true },
                { data: 'Name', title: "车系名称", sortable: false },
                { data: 'Code', title: "车系代码", sortable: false },
                { data: 'ParentCode', title: "所属品牌", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<button class=\"btn\" id=\"btnModify\"  onclick=\"editSeries(" + obj.ID + ")\">编辑</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteData(" + obj.ID + ")\">删除</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'vehicleBrand', value: $("#drpVehicleBrand").val() });
                d.push({ name: 'vehicleSeries', value: $("#drpVehicleType").val() });
            }
        });
    } else {
        dt_VehicleSeriesTable.fnDraw();
    }
}

function loadTypeTable() {
    if (!dt_VehicleTypeTable) {
        dt_VehicleTypeTable = $('#dt_VehicleTypeTable').dataTable({
            sAjaxSource: '/BaseData/GetVehicleTypeInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: 'ID', title: "编号", sortable: true },
                { data: 'Name', title: "车型名称", sortable: false },
                { data: 'Code', title: "车型代码", sortable: false },
                { data: 'ParentCode', title: "所属车系", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<button class=\"btn\" id=\"btnModify\"  onclick=\"editType(" + obj.ID + ")\">编辑</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteData(" + obj.ID + ")\">删除</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'vehicleBrand', value: $("#drpVehicleBrand").val() });
                d.push({ name: 'vehicleSeries', value: $("#drpVehicleType").val() });
                d.push({ name: 'vehicleType', value: $("#drpVehicleSeries").val() });
            }
        });
    } else {
        dt_VehicleTypeTable.fnDraw();
    }
}