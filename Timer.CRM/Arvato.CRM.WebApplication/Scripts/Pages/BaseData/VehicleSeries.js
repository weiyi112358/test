$(function () {
    //加载车牌
    LoadVehicleBrand();

    //加载数据表格
    dt_SeriesTable = $('#dt_SeriesTable').dataTable({
        sAjaxSource: '/BaseData/GetVehicleSeriesInfo',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "编号", sortable: true },
            { data: 'BrandName', title: "所属品牌", sortable: false },
            { data: 'Name', title: "车系名称", sortable: false },
            { data: 'Code', title: "车系代码", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.ID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.ID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'vehicleBrand', value: $("#drp_VehicleBrand").val() });
            d.push({ name: 'seriesName', value: $("#txt_SeriesName").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_SeriesTable.fnDraw();
    })

    //保存数据
    $("#frmAddVehicleSeries").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var type = {
                ID: $("#txtSeriesId").val(),
                //DataGroupID: $("#txtDataGroupId").val(),
                Type: $("#txtDataType").val(),
                Grade: 2,
                ParentCode: $("#addBrand").val(),
                Name: encode($("#txtSeriesName").val()),
                Code: $("#txtSeriesCode").val(),
            }
            //增加
            if (type.ID == '') {
                ajax("/BaseData/AddVehicleData", { vehicle: type }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_SeriesTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateVehicleData", { vehicle: type }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_SeriesTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })
})

//验证数据
var DataValidator = $("#frmAddVehicleSeries").validate({
    //onSubmit: false,
    rules: {
        addBrand: {
            required: true,
        },
        txtSeriesName: {
            required: true,
            maxlength: 20,
        },
        txtSeriesCode: {
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
    $("#addSeries_dialog .heading h3").html("车系新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addSeries_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addSeries_dialog .heading h3").html("车系编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetVehicleById", { vehicleId: id }, function (res) {
        $("#txtSeriesId").val(res.ID);
        $("#txtSeriesName").val(res.Name);
        $("#txtSeriesCode").val(res.Code);
        $("#addBrand").val(res.ParentCode);

        //$("#txtDataGroupId").val(res.DataGroupID);
        $("#txtDataType").val(res.Type);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addSeries_dialog",
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
                dt_SeriesTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//清空数据
function goClear() {
    $("#txtSeriesId").val('');
    $("#txtSeriesName").val('');
    $("#txtSeriesCode").val('');
    $("#addBrand").val('');

    $("#txtDataGroupId").val('');
    $("#txtDataType").val('');

    $('.error-block').html('');
}

//加载车辆品牌
function LoadVehicleBrand() {
    $('#drp_VehicleBrand').empty();
    $('#addBrand').empty();
    ajax('/BaseData/GetVehicleBrandList', null, function (res) {
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