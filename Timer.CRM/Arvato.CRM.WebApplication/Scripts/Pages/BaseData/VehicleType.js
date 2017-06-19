$(function () {
    $.ajaxSetup({
        async: false
    });
    //加载车牌
    LoadVehicleBrand();
    //加载数据表格
    dt_ModelTable = $('#dt_ModelTable').dataTable({
        sAjaxSource: '/BaseData/GetVehicleTypeInfo',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "编号", sortable: true },
            { data: 'BrandName', title: "所属品牌", sortable: false },
            { data: 'SeriesName', title: "所属车系", sortable: false },
            { data: 'Name', title: "车型名称", sortable: false },
            { data: 'Code', title: "车型代码", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.ID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.ID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'vehicleBrand', value: $("#drp_VehicleBrand").val() });
            d.push({ name: 'vehicleSeries', value: $("#drp_VehicleSeries").val() });
            d.push({ name: 'typeName', value: $("#txt_ModelName").val() });
        }
    });

    //搜索
    $("#btnSerach").click(function () {
        dt_ModelTable.fnDraw();
    })

    //保存数据
    $("#frmAddVehicleModel").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var series = {
                ID: $("#txtModelId").val(),
                //DataGroupID: $("#txtDataGroupId").val(),
                Type: $("#txtDataType").val(),
                Grade: 3,
                //VechileSetBrand: $("#addBrand").val(),
                ParentCode: $("#addSeries").val(),
                Name: encode($("#txtModelName").val()),
                Code: $("#txtModelCode").val(),
            }
            //增加
            if (series.ID == '') {
                ajax("/BaseData/AddVehicleData", { vehicle: series }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_ModelTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateVehicleData", { vehicle: series }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_ModelTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })

    //品牌改变
    $('#drp_VehicleBrand').change(function () {
        var brandId = $('#drp_VehicleBrand').val();
        if (brandId != '') {
            LoadVehicleSeries(brandId);
        } else {
            $('#drpVehicleSeries').html("<option value=''>-无-</option>");
        }
    })
    //品牌改变--弹窗
    $('#addBrand').change(function () {
        var brandId = $('#addBrand').val();
        if (brandId != '') {
            LoadVehicleSeriesDiv(brandId);
        } else {
            $('#addSeries').html("<option value=''>-无-</option>");
        }
    })
})

//验证数据
var DataValidator = $("#frmAddVehicleModel").validate({
    //onSubmit: false,
    rules: {
        addBrand: {
            required: true,
        },
        addSeries: {
            required: true,
        },
        txtModelName: {
            required: true,
            maxlength: 20,
        },
        txtModelCode: {
            required: true,
            maxlength: 20,
            isOnlyLN: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//弹窗
function goEdit() {
    $("#addModel_dialog .heading h3").html("车型新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addModel_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addModel_dialog .heading h3").html("车型编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetVehicleById", { vehicleId: id }, function (res) {
        $("#txtModelId").val(res.ID);

        $("#txtModelName").val(res.Name);
        $("#txtModelCode").val(res.Code);
        $("#txtDataType").val(res.BaseDataType);

        ajax("GetVehicleSiblingList", { code: res.ParentCode }, function (data) {
            //$("#addBrand").val(res.VechileSetBrand).change();
            var brand;
            $('#addSeries').empty();
            if (data.length > 0) {
                brand = data[0].ParentCode
                var opt = "<option value=''>请选择</option>";
                for (var i = 0; i < data.length; i++) {
                    opt += '<option value=' + data[i].Code + '>' + data[i].Name + '</option>';
                }
                $('#addSeries').append(opt);

                $("#addBrand").val(brand);
                $("#addSeries").val(res.ParentCode);
            }
        });

        //$("#txtDataGroupId").val(res.DataGroupID);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addModel_dialog",
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
                dt_ModelTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//清空数据
function goClear() {
    $("#txtModelId").val('');

    $("#txtModelName").val('');
    $("#txtModelCode").val('');
    $("#addBrand").val('').change();
    $("#addType").val('');

    $("#txtDataGroupId").val('');
    $("#txtDataType").val('');

    $('.error-block').html('');
}

//加载车辆品牌
function LoadVehicleBrand() {
    $('#drp_VehicleBrand').empty();
    $('#addBrand').empty();
    $.post('/BaseData/GetVehicleBrandList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drp_VehicleBrand').append(opt);
            $('#addBrand').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_VehicleBrand').append(opt);
            $('#addBrand').append(opt);
        }
    });
}

//加载车型
function LoadVehicleSeries(brandId) {
    $('#drp_VehicleSeries').empty();
    $.post('/BaseData/GetVehicleSeriesList', { brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drp_VehicleSeries').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_VehicleSeries').append(opt);
        }
    });
}
//加载车型--弹窗
function LoadVehicleSeriesDiv(brandId) {
    $('#addSeries').empty();
    $.post('/BaseData/GetVehicleSeriesList', { brandId: brandId }, function (res) {
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