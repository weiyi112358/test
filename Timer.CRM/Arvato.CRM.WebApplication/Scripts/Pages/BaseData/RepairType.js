$(function () {
    //加载门店
    LoadStoreList();
    $(".chzn_store").chosen();
    GetActLimitStoreList();

    //加载数据表格
    table_RepairType = $('#dt_RepairType').dataTable({
        sAjaxSource: '/BaseData/GetRepairTypeData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'ID', title: "编号", sortable: true },
            { data: 'StoreName', title: "所属门店", sortable: false },
            { data: 'RepairTypeName', title: "维修类型", sortable: false },
            {
                data: 'IsApplyToLoyPoint', title: "赠送积分积点", sortable: false, render: function (r) {
                    return r == true ? "启用" : "未启用";
                }
            },
            {
                data: 'IsApplyToLoyDimension', title: "忠诚度维度计算", sortable: false, render: function (r) {
                    return r == true ? "启用" : "未启用";
                }
            },
            {
                data: 'IsApplyToLoyStatus', title: "忠诚度会员状态", sortable: false, render: function (r) {
                    return r == true ? "启用" : "未启用";
                }
            },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.ID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.ID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'repairName', value: $("#txtRepairTypeName").val() });
            d.push({ name: 'storeCode', value: $("#drp_Store").val() });
        }
    });
    //搜索
    $("#btnSearch").click(function () {
        table_RepairType.fnDraw();
    })
    //添加
    $("#btnAdd").click(function () {
        $("#table_RepairType .modal-header h3").html("新增维修类型");
        //清空数据
        goClear();
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            overlayClose: false,
            opacity: '0.3',
            href: "#table_RepairType",
            inline: true
        });
        $.colorbox.resize();
    })

    //保存数据
    $("#frmRepairType").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var limitStore = new Array();
            //限制门店
            $("#drpLimitStore").find("option:selected").each(function (i, data) {
                //var label = $(this).parent("optgroup").attr("label");
                var value = data.value;
                //limitStore[i] = value;
                limitStore.push({ LimitType: "store", LimitValue: value });
            });
            var RepairModel = {
                ID: $("#txtRepairTypeId").val(),
                StoreCode: limitStore,
                RepairTypeName: encode($("#txtRepairType").val()),

                IsApplyToLoyPoint: $("#chbIsPoint").prop('checked'),
                IsApplyToLoyDimension: $("#chbIsDimension").prop('checked'),
                IsApplyToLoyStatus: $("#chbIsStatus").prop('checked'),
            }
            //增加
            if (RepairModel.ID == '') {
                ajax("/BaseData/AddRepairTypeData", { model: RepairModel }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        table_RepairType.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateRepairTypeData", { model: RepairModel }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        var start = table_RepairType.fnSettings()._iDisplayStart;
                        var length = table_RepairType.fnSettings()._iDisplayLength;
                        table_RepairType.fnPageChange(start / length, true);
                        //dt_StoreSetting.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })
})

//验证数据
var DataValidator = $("#frmRepairType").validate({
    //onSubmit: false,
    rules: {
        txtRepairType: {
            required: true,
        },
        drpLimitStore: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});


//编辑条目信息
function edit(id) {

    $("#table_RepairType .modal-header h3").html("修改维修类型");
    //清空数据
    goClear();
    $("#drpLimitStore").prop('disabled', true);
    $("#txtRepairType").prop('disabled', true);
    ajax("/BaseData/GetRepairTypeById", { repairTypeId: id }, function (res) {
        $("#txtRepairTypeId").val(res.ID);
        $("#drpLimitStore").val(res.StoreCode).trigger("liszt:updated");
        $("#txtRepairType").val(res.RepairTypeName);

        $("#chbIsPoint").prop('checked', res.IsApplyToLoyPoint);
        $("#chbIsDimension").prop('checked', res.IsApplyToLoyDimension);
        $("#chbIsStatus").prop('checked', res.IsApplyToLoyStatus);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_RepairType",
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
        ajax("/BaseData/DeleteRepairTypeById", { repairTypeId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                table_RepairType.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}
//清空数据
function goClear() {
    $("#drpLimitStore").prop('disabled', false);
    $("#txtRepairType").prop('disabled', false);
    $("#txtRepairType").val('');
    $("#txtRepairTypeId").val('');
    $("#chbIsPoint,#chbIsDimension,#chbIsStatus").prop('checked', false);
    $("#drpLimitStore").val('').trigger("liszt:updated");

    $('.error-block').html('');
}
//加载门店
function LoadStoreList() {
    ajax('/BaseData/GetStoreList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>全部</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drp_Store').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_Store').append(opt);
        }
    });
}

//加载具体门店使用限制
function GetActLimitStoreList() {
    $('#drpLimitStore').empty();
    ajax("/Member360/GetActLimitStoreList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drpLimitStore').append(opt);
            $(".chzn_store").trigger("liszt:updated");

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpLimitStore').append(opt);
        }
    });
}