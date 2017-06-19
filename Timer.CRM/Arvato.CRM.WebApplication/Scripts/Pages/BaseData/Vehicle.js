$(function () {
    $.ajaxSetup({
        async: false
    });

    //加载车辆品牌列表
    LoadVehicleBrand();
    //加载数据表格
    dt_VehicleTypeTable = $('#dt_VehicleTypeTable').dataTable({
        sAjaxSource: '/BaseData/GetVehicleData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: true },
            { data: 'BrandNameVehicle', title: "品牌", sortable: false },
            { data: 'LevelNameVehicle', title: "车系", sortable: false },
            { data: 'SeriesNameVehicle', title: "车型", sortable: false },
            { data: 'ColorNameVehicle', title: "颜色", sortable: false },
            { data: 'TrimNameVehicle', title: "内饰颜色", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteData(" + obj.BaseDataID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'vehicleBrand', value: $("#drpVehicleBrand").val() });
            d.push({ name: 'vehicleSeries', value: $("#drpVehicleSeries").val() });
            d.push({ name: 'vehicleType', value: $("#drpVehicleType").val() });
        }
    });
    //搜索
    $("#btnSearch").click(function () {
        dt_VehicleTypeTable.fnDraw();
    })

    //保存数据
    $("#frmAddVehicle").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var vehicle = {
                BaseDataID: $("#ItemCode").val(),
                DataGroupID: $("#txtDataGroupId").val(),
                BaseDataType: $("#txtDataType").val(),
                BrandNameVehicle: $("#addBrand").find("option:selected").text(),
                BrandCodeVehicle: $("#addBrand").val(),
                SeriesNameVehicle: $("#addSeries").find("option:selected").text(),
                SeriesCodeVehicle: $("#addSeries").val(),
                LevelNameVehicle: $("#addLevel").find("option:selected").text(),
                LevelCodeVehicle: $("#addLevel").val(),
                ColorNameVehicle: encode($("#addColor").val()),
                ColorCodeVehicle: $("#addColorCode").val(),
                TrimNameVehicle: encode($("#addTrimName").val()),
                TrimCodeVehicle: $("#addTrimNameCode").val(),
            }
            //增加
            if (vehicle.BaseDataID == '') {
                ajax("/BaseData/AddVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_VehicleTypeTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateVehicleData", vehicle, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_VehicleTypeTable.fnDraw();
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
    $('#addBrand').change(function () {
        var brandId = $('#addBrand').val();
        if (brandId != '') {

            LoadVehicleLevelDiv(brandId);
        } else {
            $('#addLevel').html("<option value=''>-无-</option>");
            $('#addSeries').html("<option value=''>-无-</option>");
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
    //$('#addItemClass').empty();
    ajax('/BaseData/GetVehicleBrandList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].BrandCode + '>' + res[i].BrandName + '</option>';
            }
            $('#drpVehicleBrand').append(opt);
            $('#addBrand').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVehicleBrand').append(opt);
            $('#addBrand').append(opt);
        }
    });
}
//加载车辆车系
function LoadVehicleSeries(typeId,brandId) {
    $('#drpVehicleSeries').empty();
    ajax('/BaseData/GetVehicleSeriesList', { typeId: typeId,brandId:brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].VechileSetCode + '>' + res[i].VechileSetName + '</option>';
            }
            $('#drpVehicleSeries').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVehicleSeries').append(opt);
        }
    });
}
//加载车辆车系--弹窗
function LoadVehicleSeriesDiv(typeId,brandId) {
    $('#addSeries').empty();
    $.post('/BaseData/GetVehicleSeriesList', { typeId: typeId, brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].VechileSetCode + '>' + res[i].VechileSetName + '</option>';
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
                opt += '<option value=' + res[i].VechileTypeCode + '>' + res[i].VechileTypeName + '</option>';
            }
            $('#drpVehicleType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVehicleType').append(opt);
        }
    });
}
//加载车型--弹窗
function LoadVehicleLevelDiv(brandId) {
    $('#addLevel').empty();
    $.post('/BaseData/GetVehicleLevelList', { brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].VechileTypeCode + '>' + res[i].VechileTypeName + '</option>';
            }
            $('#addLevel').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#addLevel').append(opt);
        }
    });
}
//验证数据
var DataValidator = $("#frmAddVehicle").validate({
    //onSubmit: false,
    rules: {
        addBrand: {
            required: true,
        },
        addSeries: {
            required: true,
        },
        addLevel: {
            required: true,
        },
        addColor: {
            required: true,
            maxlength: 20,
        },
        addColorCode: {
            required: true,
            maxlength: 20,
            number: true,
        },
        addTrimName: {
            required: true,
            maxlength: 20,
        },
        addTrimNameCode: {
            required: true,
            maxlength: 100,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//添加弹窗
function add() {

    $("#addItem_dialog .heading h3").html("新增车款");
    //清空数据
    clearData();
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addItem_dialog",
        inline: true
    });
}
//编辑
function edit(id) {

    $("#addItem_dialog .heading h3").html("编辑车款");
    //清空数据
    clearData();
    ajax("/BaseData/GetVehicleById", { vehicleId: id }, function (res) {
        $("#addBrand").val(res.BrandCodeVehicle).change();

        $("#addLevel").val(res.LevelCodeVehicle).change();
        $("#ItemCode").val(res.BaseDataID);
        $("#addSeries").val(res.SeriesCodeVehicle);
        //$("#addSeriesCode").val(res.SeriesCodeVehicle);
        //$("#addLevelCode").val(res.LevelCodeVehicle);
        $("#addColor").val(res.ColorNameVehicle);
        $("#addColorCode").val(res.ColorCodeVehicle);
        $("#addTrimName").val(res.TrimNameVehicle);
        $("#addTrimNameCode").val(res.TrimCodeVehicle);

        $("#txtDataGroupId").val(res.DataGroupID);
        $("#txtDataType").val(res.BaseDataType);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addItem_dialog",
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
                dt_VehicleTypeTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}
//清空弹窗数据
function clearData() {
    $("#addBrand").val('');
    $("#ItemCode").val('');
    $("#addSeries").val('');
    $("#addSeriesCode").val('');
    $("#addLevel").val('');
    $("#addLevelCode").val('');
    $("#addColor").val('');
    $("#addColorCode").val('');
    $("#addTrimName").val('');
    $("#addTrimNameCode").val('');

    $("#txtDataGroupId").val('');
    $("#txtDataType").val('');

    $('.error-block').html('');
}