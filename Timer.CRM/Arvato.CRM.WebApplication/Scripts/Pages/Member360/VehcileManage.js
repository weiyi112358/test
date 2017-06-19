var dt_StoreData, tableMemberInfo;
$(function () {
    $("#txtBuyDate").datepicker();//{ startDate: '0' }
    $(".chzn_a").chosen(
        {
        allow_single_deselect: true
        }
    );
    LoadVehicleBrand();
    $("#btnAdd").click(function () {

        $("#txtVehcileId").val('');
        //清空数据
        goClear();
        $("#addStore_dialog .heading h3").html("新建车辆信息");
        //显示编辑页面弹窗
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            overlayClose: false,
            opacity: '0.3',
            //title: '素材',
            href: "#addStore_dialog",
            inline: true
        });
    })
    //编辑车辆信息
    $("#frmAddStore").submit(function (e) {
        e.preventDefault();

        

        if (DataValidatorAdd.form()) {

            var vechile = {
                VechileId: $("#txtVehcileId").val(),
                MemberID: $("#txtMemId").val(),
                VechileNo: $("#txtVechileNo").val(),
                VechileBrand: $("#drpVechileBrand").val(),
                VechileSerice: $("#drpVechileSerice").val(),
                VechileType: $("#drpVechileType").val(),
                VechileColor: $("#txtVechileColor").val(),
                VechileInner: $("#txtVechileInner").val(),
                Mile: $("#txtMile").val(),
                VinNo: $("#txtVinNo").val(),
                BuyDate: $("#txtBuyDate").val(),
                IsTransfer: $("#isTransfer").val()
            }
            if ($("#drpVechileBrand").val() == "") {
                $.dialog("请输入品牌");
                return;
            }
            if ($("#drpVechileSerice").val() == "") {
                $.dialog("请输入车系");
                return;
            }
            if ($("#drpVechileBrand").val() == "") {
                $.dialog("请输入车型");
                return;
            }
            ajax("/Member360/UpdateVehcileData", { vechile: vechile }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();
                    loadVehcilefoList();
                    $.dialog(res.MSG);
                } else $.dialog(res.MSG);
            });
        }
    })

    //搜索会员
    $("#btnSearch").click(function () {
        //至少有一个条件才能搜索
        if ($("#txtNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '') {
            $.dialog("至少输入一个条件以供查询");
            return;
        }

        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#table_Member",
            inline: true,
            opacity: '0.3',
            onComplete: function () {
                loadMemInfoList();
                $.colorbox.resize();
            }
        });
    })

    //品牌车型车系联动
    $("#drpVechileBrand").change(function () {

        var brandId = $('#drpVechileBrand').val();
        if (brandId != '') {

            LoadSeries(brandId);
        } else {
            $('#drpVechileSerice').html("<option value=''>-无-</option>");
            $('#drpVechileType').html("<option value=''>-无-</option>");
        }
    })
    $("#drpVechileSerice").change(function () {

        var brandId = $('#drpVechileBrand').val();
        var typeId = $('#drpVechileSerice').val();
        if (typeId != '') {
            LoadType(typeId, brandId);
        } else {
            $('#drpVechileType').html("<option value=''>-无-</option>");
        }
    })
})
//新增时验证数据
var DataValidatorAdd = $("#frmAddStore").validate({
    //onSubmit: false,
    rules: {
        txtVechileNo: {
            required: true,
            isVehicleNo: true,
        },
        //drpVechileBrand: {
        //    required: true,
        //},
        //drpVechileSerice: {
        //    required: true,
        //},
        //drpVechileType: {
        //    required: true,
        //},
        txtBuyDate: {
            required: true,
        },
        txtVinNo: {
            required: true,
            isVIN: true
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//加载会员信息
function loadMemInfoList() {
    if (!tableMemberInfo) {
        tableMemberInfo = $('#tableMemberInfo').dataTable({
            sAjaxSource: '/Member360/GetMembersNameByPage',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: "MemberCardNo", title: "会员编号", sortable: false, sWidth: "25%" },

                { data: "CustomerName", title: "会员名称", sortable: false },

                    { data: "CustomerMobile", title: "手机", sortable: false },
                    //{ data: "CustomerLevelText", title: "会员等级", sortable: false },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        var str = '<button class="btn" onclick="detail(\'' + obj.MemberID + '\')">查看</button> ';
                        return str;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'memNo', value: encode($("#txtNo").val()) });
                d.push({ name: 'memName', value: encode($("#txtName").val()) });
                d.push({ name: 'memMobile', value: encode($("#txtMobile").val()) });
            }
        });
    }
    else {
        tableMemberInfo.fnDraw();
    }
}
//查看会员车辆详细信息
function detail(id) {
    $.colorbox.close();

    $("#txtMemId").val(id);
    $("#btnAdd").show();
    $(".memInfoBlock").show();
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            //memberId = mid;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);

            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "v1" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].CustomerStatus == 1 ? "正常" : "停用");
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

        }
    })
    loadCashPoint(id);

    loadVehcilefoList();
}
//加载账户积分现金信息
function loadCashPoint(id) {
    ajax("/Member360/GetMemIsBackAccountInfo", { mid: id }, function (data) {
        $("#stgValidValue1").text(0);
        $("#stgValidValue2").text(0);
        if (data.length > 0) {
            $("#stgValidValue1").text(data[0].Value2);
            $("#stgValidValue3").text(data[0].Value1);
            $("#stgValidValue2").text(data[0].NoBackAccount == null ? "0" : data[0].NoBackAccount);
            $("#txtCash").val(data[0].Total);
            total = data[0].Total;
        }
    });
}
//加载会员信息
function loadVehcilefoList() {
    if (!dt_StoreData) {
        dt_StoreData = $('#dt_StoreData').dataTable({
            sAjaxSource: '/Member360/GetVehcileListByMid',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: "VINVehicle", title: "VIN码", sortable: false },

                { data: "PlateNumVehicle", title: "车牌号", sortable: false },

                    { data: "BrandNameVehicle", title: "品牌", sortable: false },
                    { data: "SeriesNameVehicle", title: "车系", sortable: false },
                    { data: "LevelNameVehicle", title: "车型", sortable: false },
                    { data: "ColorNameVehicle", title: "颜色", sortable: false },
                    { data: "DriveDistinct", title: "里程", sortable: false },
                    {
                        data: "IsPass", title: "是否过户", sortable: false, render: function (obj) {
                            return obj == 1 ? "是" : "否";
                        }
                    },
                    {
                        data: "BuyDate", title: "购买日期", sortable: false, render: function (obj) {
                            return obj ? obj.substring(0, 10) : "";

                        }
                    },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        if (obj.IsPass == 1) {
                            return str = "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.MemberSubExtID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.MemberSubExtID + ")\">删除</button>";
                        } else {
                            return str = "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.MemberSubExtID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.MemberSubExtID + ")\">删除</button><button class=\"btn btn-delete\" id=\"btnTransfer\" onclick=\"transferItem(" + obj.MemberSubExtID + ")\">过户</button>";
                        }
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtMemId").val() });
            }
        });
    }
    else {
        dt_StoreData.fnDraw();
    }
}

//编辑条目信息
function edit(id) {
    $("#txtVehcileId").val(id);

    $("#addStore_dialog .heading h3").html("编辑车辆信息");
    //清空数据
    goClear();
    ajax("/Member360/GetVehcileInfoByid", { id: id }, function (res) {
        $("#txtVechileNo").val(res.PlateNumVehicle);
        $("#drpVechileBrand").val(res.BrandVehicle).change();
        $("#drpVechileSerice").val(res.SeriesVehicle).change();
        $("#drpVechileType").val(res.LevelVehicle).trigger("liszt:updated");
        $("#txtVechileColor").val(res.ColorVehicle);
        $("#txtVechileInner").val(res.TrimVehicle);
        $("#txtMile").val(res.DriveDistinct);
        $("#txtVinNo").val(res.VINVehicle);
        $("#txtBuyDate").val(res.BuyDate==null?'':res.BuyDate.substring(0,10));
        $("#isTransfer").val(res.IsPass);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
    $.colorbox.resize();
}
function goClear() {
    $("#txtVechileNo").val('');
    $("#drpVechileBrand").val('').trigger("liszt:updated");
    $("#drpVechileSerice").val('').trigger("liszt:updated");
    $("#drpVechileType").val('').trigger("liszt:updated");
    $("#txtVechileColor").val('');
    $("#txtVechileInner").val('');
    $("#txtMile").val('');
    $("#txtVinNo").val('');
    $("#txtBuyDate").val('');
    $("#isTransfer").val('');
}
//删除条目
function deleteItem(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Member360/DeleteVehcileData", { id: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreData.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}
//过户
function transferItem(id) {
    $.dialog("确认过户吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Member360/TransferVehcileData", { id: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreData.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//加载车辆品牌
function LoadVehicleBrand() {
    $('#drpVechileBrand').empty();
    ajax('/BaseData/GetVehicleBrandList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drpVechileBrand').append(opt);
            $(".chzn_a").trigger("liszt:updated");
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVechileBrand').append(opt);
        }
    });
}
//加载车系
function LoadSeries(brandId) {
    $('#drpVechileSerice').empty();
    ajaxSync('/BaseData/GetVehicleSeriesList', { brandId: brandId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drpVechileSerice').append(opt);
            $(".chzn_a").trigger("liszt:updated");
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVechileSerice').append(opt);
        }
    });
}
function LoadType(typeId, brandId) {
    $('#drpVechileType').empty();
    ajaxSync('/BaseData/GetVehicleLevelList', { typeId: typeId }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].Code + '>' + res[i].Name + '</option>';
            }
            $('#drpVechileType').append(opt);
            $(".chzn_a").trigger("liszt:updated");
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVechileType').append(opt);
        }
    });
}