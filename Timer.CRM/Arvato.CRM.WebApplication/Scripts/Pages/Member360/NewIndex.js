var dtMember360;
var dtSubMember360;
var urlpara;
$(function () {
    $('#dtMember360').resize(function () {
        $('#dtMember360').css({ "width": "130%" });
    })

    //查询操作
    $("#btnSearch").click(function () {
        searchMem();
    });
    //查询操作
    $("#btnAdd").click(function () {
        window.open("/member360/MemberDetailNew?mid=0");
    });
    //加载车辆品牌列表
    LoadVehicleBrand();
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
});
//var columnList;
// 搜索会员
function searchMem() {
    qryopt = {
        MemberNo: $("#txtMemNo").val().trim(),
        CustomerName: $("#txtName").val().trim(),
        CustomerMobile: $("#txtMobile").val().trim()
    };
    if (qryopt.MemberNo == "" && qryopt.CustomerName == "" && qryopt.CustomerMobile == "" ){
        $.dialog("查询条件不能为空！");
        return;
    }
    //ajaxSync("/Member360/GetMembersByPageColumn", { memberNo: "", customerName: "苏洁", customerMobile: "" }, function (res) {
    //    if (res) {
    //        columnList = res;
    //    }
    //});

    if (!dtMember360) {
        dtMember360 = $('#dtMember360').dataTable({
            sAjaxSource: '/Member360/GetMembersByPage',
            sScrollX: "100%",
            sScrollXInner: "130%",
            bScrollCollapse: true,
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aoColumns:
            //    columnList,
            //fnFixData: function (d) {
            //    d.push({ name: 'memberNo', value: ""});
            //    d.push({ name: 'customerName', value: "苏洁" });
            //    d.push({ name: 'customerMobile', value: "" });
            //},
            [
                { data: "MemberCardNo", title: "会员编号", sWidth: "100", sortable: false },
                { data: "CustomerName", title: "姓名", sWidth: "120", sortable: false },
                { data: "CustomerMobile", title: "手机", sWidth: "100", sortable: false },
                { data: "CustomerLevelText", title: "会员等级", sWidth: "80", sortable: false },
                {
                    data: null, title: "所在城市", sWidth: "100", sortable: false, render: function (r) {
                        var show = r.City == null ? "" : r.City;
                        if (r.City != null) {
                            show = show.length > 3 ? (show.substring(0, 3) + "...") : show;
                        }
                        return "<span title='" + r.City + "'>" + show + "</span>"
                    }
                },
                { data: "Channel", title: "入会渠道", sWidth: "100", sortable: false },
                { data: "CustomerSource", title: "来源", sWidth: "100", sortable: false },
                {
                    data: null, title: "注册门店", sWidth: "150", sortable: false, render: function (r) {
                        var show = r.RegisterStoreName == null ? "" : r.RegisterStoreName;
                        if (r.RegisterStoreName != null) {
                            show = show.length > 10 ? (show.substring(0, 10) + "...") : show;
                        }
                        return "<span title='" + r.RegisterStoreName + "'>" + show + "</span>"
                    }
                },
                { data: "AvailPoint", title: "可用积分", sWidth: "100", sortable: true },
                {
                    data: null, title: "注册时间", sWidth: "140", sortable: false, render: function (r) {
                        return r.RegisterDate.substring(0, 10);
                    }
                },
                {
                    data: "ConsumeAmount", title: "消费额", sWidth: "80", sortable: true,
                },
                {
                    data: null, title: "历史消费额", sWidth: "100", sortable: false, render: function (r) {
                        return r.HistoryConsumeAmount == null ? 0 : r.HistoryConsumeAmount;
                    }
                },
                  {
                      data: null, title: "调整金额", sWidth: "100", sortable: false, render: function (r) {
                          return r.HistoryConsumeModify == null ? 0 : r.HistoryConsumeModify;
                      }
                  }
            ],
            fnFixData: function (d) {
                d.push({ name: 'memberNo', value: ""});
                d.push({ name: 'customerName', value: "苏洁" });
                d.push({ name: 'customerMobile', value: "" });
            },
        });
    }
    else {
        dtMember360.fnDraw();
    }
}

//加载车辆品牌
function LoadVehicleBrand() {
    //$('#addItemClass').empty();
    ajax('/BaseData/GetVehicleBrandList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].BrandCode + '>' + res[i].BrandName + '</option>';
            }
            $('#addBrand').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#addBrand').append(opt);
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
                opt += '<option value=' + res[i].VechileSetCode + '>' + res[i].VechileSetName + '</option>';
            }
            $('#addSeries').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#addSeries').append(opt);
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

