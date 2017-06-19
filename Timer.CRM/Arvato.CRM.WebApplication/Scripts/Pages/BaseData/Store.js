$(function () {
    //加载条目类型
    LoadStoreType();
    //加载数据表格
    dt_StoreData = $('#dt_StoreData').dataTable({
        sAjaxSource: '/BaseData/GetStoreData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: true, },
            { data: 'StoreName', title: "门店名称", sortable: false, },
            { data: 'SubDataGroupName', title: "所属群组", sortable: false, },
            { data: 'BrandName', title: "所属品牌", sortable: false, },
            { data: 'StoreCode', title: "售后门店代码", sortable: false, },
            { data: 'StoreCodeSale', title: "销售门店代码", sortable: false, },
            { data: 'StoreAddress', title: "门店地址", sortable: false, },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'storeCode', value: $("#txtStoreCode").val() });
            d.push({ name: 'storeName', value: $("#txtStoreName").val() });
            d.push({ name: 'datagroupId', value: $("#drpStoreClass").val() });
        }
    });
    //查询
    $("#btnSearch").click(function () {
        dt_StoreData.fnDraw();
    })
    //新增条目
    $("#frmAddStore").submit(function (e) {
        e.preventDefault();
        if (DataValidatorAdd.form()) {
            var storeCode = $("#addStoreCode").val();
            var storeFullName = encode($("#addStoreFullName").val());
            var storeName = encode($("#addStoreName").val());
            var storeClass = $("#addStoreClass").val();
            var address = encode($("#addAddress").val());
            var storeBrand = $("#addStoreBrand").val();
            var storeCodeSale = $("#addStoreCodeSale").val();
            var printer = $("#addPrinter").val();
            var code = $("#addSerialCode").val();
            ajax("/BaseData/AddStoreData", { storeCode: storeCode, storeName: storeName, storeDataGroup: storeClass, address: address, storeBrand: storeBrand, storeCodeSale: storeCodeSale, printer: printer, code: code, storeFullName: storeFullName }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();
                    dt_StoreData.fnDraw();
                    $.dialog(res.MSG);
                } else $.dialog(res.MSG);
            });
        }
    })
    //新增条目
    //$("#btnAddSave").click(function () {
    //   var itemCode= $("#addItemCode").val();
    //   var itemName= $("#addItemName").val();
    //   var itemClass= $("#addItemClass").val();
    //   var remark= $("#addRemark").val();

    //   ajax("/BaseData/AddItemData", {itemId:itemCode,itemName:itemName,itemType:itemClass, remark: remark }, function (res) {
    //        $.colorbox.close();
    //        dt_StoreData.fnDraw();
    //        $.dialog(res);
    //    });
    //})
    //修改条目
    $("#frmEditStore").submit(function (e) {
        e.preventDefault();
        if (DataValidatorEdit.form()) {
            var store = {
                BaseDataID: $("#editStoreId").val(),
                DataGroupID: $("#txtDataGroupId").val(),
                BaseDataType: $("#txtDataType").val(),
                StoreFullName: encode($("#editStoreFullName").val()),
                StoreName: encode($("#editStoreName").val()),
                StoreAddress: encode($("#editAddress").val()),
                StoreCodeSale: $("#editStoreCodeSale").val(),
                StoreCode: $("#txtCode").val(),
                StoreBrandCode: $("#editStoreBrand").val(),
                Printer: $("#editPrinter").val(),
                SerialCode: $("#editSerialCode").val()
            }
            ajax("/BaseData/UpdateStoreData", store, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();

                    var start = dt_StoreData.fnSettings()._iDisplayStart;
                    var length = dt_StoreData.fnSettings()._iDisplayLength;
                    dt_StoreData.fnPageChange(start / length, true);
                    //dt_StoreData.fnDraw();
                    $.dialog(res.MSG);
                } else { $.dialog(res.MSG); }
            });
        }
    })
    //修改条目
    //$("#btnEditSave").click(function () {
    //    ajax("/BaseData/UpdateItemData", { itemId: $("#editItemCode").val(), remark: $("#editRemark").val() }, function (res) {
    //        $.colorbox.close();
    //        dt_StoreData.fnDraw();
    //        $.dialog(res);
    //    });
    //})

    $("#addStoreClass").change(function () {
        var a = $("#addStoreClass").val();
        //加载品牌
        LoadStoreBrand(a);
    })
    $("#editStoreClass").change(function () {
        var b = $("#editStoreClass").val();
        //加载品牌
        LoadStoreBrand(b);
    })
})
//新增时验证数据
var DataValidatorAdd = $("#frmAddStore").validate({
    //onSubmit: false,
    rules: {
        addStoreCode: {
            required: true,
            maxlength: 20,
            isOnlyLN: true,
        },
        addStoreCodeSale: {
            required: true,
            maxlength: 20,
            isOnlyLN: true,
        },
        addStoreName: {
            required: true,
            maxlength: 20,
            //isOnlyLNC: true,
        },
        addStoreClass: {
            required: true,
        },
        addStoreBrand: {
            required: true,
        },
        addAddress: {
            required: true,
            maxlength: 100,
        },
        addPrinter: {
            maxlength: 50,
        },
        addSerialCode: {
            maxlength: 10,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
var DataValidatorEdit = $("#frmEditStore").validate({
    //onSubmit: false,
    rules: {
        editAddress: {
            required: true,
            maxlength: 100,
        },
        editStoreBrand: {
            required: true,
        },
        editPrinter: {
            maxlength: 50,
        },
        editSerialCode: {
            maxlength: 10,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//编辑条目信息
function edit(id) {
    ajax("/BaseData/GetStoreById", { storeId: id }, function (res) {
        $("#editStoreId").val(res.BaseDataID);
        $("#editStoreName").val(res.StoreName);
        $("#editStoreClass").val(res.DataGroupID).change();

        $("#editStoreFullName").val(res.StoreFullName);

        //LoadStoreBrand(res.DataGroupID);
        $("#editAddress").val(res.StoreAddress);
        $("#editStoreBrand").val(res.StoreBrandCode);

        $("#editSerialCode").val(res.SerialCode);

        $("#txtCode").val(res.StoreCode);
        $("#txtDataGroupId").val(res.DataGroupID);
        $("#txtDataType").val(res.BaseDataType);
        $("#editStoreCode").val(res.StoreCode);
        $("#editStoreCodeSale").val(res.StoreCodeSale);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#editStore_dialog",
        inline: true
    });
}

//新建条目
function add() {
    clearData();

    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
}

//清空数据
function clearData() {
    $("#addStoreCode").val('');
    $("#addStoreCodeSale").val('');
    $("#addStoreName").val('');
    $("#addStoreFullName").val('');
    $("#addStoreClass").val('').removeClass('error-block');
    $("#addAddress").val('');
    $('.error-block').html('');
}

//删除条目
function deleteItem(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteStoreById", { storeId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreData.fnDraw();
            } else { $.dialog(res.MSG); }
        });
        //showDialogMessage("删除成功");
    })
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
//加载门店所属品牌
function LoadStoreBrand(id) {
    //$('#editStoreBrand').empty();
    //$('#addStoreBrand').empty();
    ajaxSync('/BaseData/GetVehicleBrandList', { groupId: id }, function (res) {
        $('#addStoreBrand').html("");
        $('#editStoreBrand').html("");
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].BrandCode + '>' + res[i].BrandName + '</option>';
            }
            $('#editStoreBrand').append(opt);
            $('#addStoreBrand').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#editStoreBrand').append(opt);
            $('#addStoreBrand').append(opt);
        }
    });
}

