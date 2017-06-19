$(function () {
    //加载门店
    LoadStoreList();
    //加载数据表格
    dt_StoreSetting = $('#dt_StoreSetting').dataTable({
        sAjaxSource: '/BaseData/GetStoreSettingData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: true },
            { data: 'StoreName', title: "门店名称", sortable: false },
            { data: 'OrderMaxPoint', title: "每单积点上限", sortable: false },
            { data: 'PointCashPec', title: "积点现金换算率", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'storeCode', value: $("#drp_Store").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_StoreSetting.fnDraw();
    })

    //保存数据
    $("#frmAddStoreSetting").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var setting = {
                BaseDataID: $("#txtSettingId").val(),
                DataGroupID: $("#txtDataGroupId").val(),
                BaseDataType: $("#txtDataType").val(),

                StoreSettingStoreCode: $("#drpStoreSetting").val(),
                OrderMaxPoint: $("#txtMaxPoint").val(),
                PointCashPec: $("#txtPointCashPec").val(),
            }
            //增加
            if (setting.BaseDataID == '') {
                ajax("/BaseData/AddStoreSettingData", { model: setting }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_StoreSetting.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateStoreSettingData", { model: setting }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        var start = dt_StoreSetting.fnSettings()._iDisplayStart;
                        var length = dt_StoreSetting.fnSettings()._iDisplayLength;
                        dt_StoreSetting.fnPageChange(start / length, true);
                        //dt_StoreSetting.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })
})

//验证数据
var DataValidator = $("#frmAddStoreSetting").validate({
    //onSubmit: false,
    rules: {
        drpStoreSetting: {
            required: true,
        },
        txtMaxPoint: {
            isDecimal:true,
        },
        txtPointCashPec: {
            required: true,
            isDecimal:true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//弹窗
function goEdit() {
    $("#addStoreSetting_dialog .heading h3").html("新增门店设定");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addStoreSetting_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addStoreSetting_dialog .heading h3").html("编辑门店设定");
    //清空数据
    goClear();
    ajax("/BaseData/GetStoreSettingById", { settingId: id }, function (res) {
        $("#txtSettingId").val(res.BaseDataID);

        $("#drpStoreSetting").val(res.StoreSettingStoreCode);
        $("#txtMaxPoint").val(res.OrderMaxPoint);
        $("#txtPointCashPec").val(res.PointCashPec);

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
        href: "#addStoreSetting_dialog",
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
        ajax("/BaseData/DeleteStoreSettingById", { settingId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreSetting.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//清空数据
function goClear() {
    $("#txtSettingId").val('');

    $("#drpStoreSetting").val('');
    $("#txtMaxPoint").val('');
    $("#txtPointCashPec").val('');

    $("#txtDataGroupId").val('');
    $("#txtDataType").val('');

    $('.error-block').html('');
}

//加载门店
function LoadStoreList() {
    $('#drp_Store').empty();
    $('#drpStoreSetting').empty();
    ajax('/BaseData/GetStoreList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>全部</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drp_Store').append(opt);
            $('#drpStoreSetting').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_Store').append(opt);
            $('#drpStoreSetting').append(opt);
        }
    });
}