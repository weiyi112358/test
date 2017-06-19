$(function () {
    //加载条目类型
    LoadItemType();

    //加载数据表格
    dt_ItemData = $('#dt_ItemData').dataTable({
        sAjaxSource: '/BaseData/GetItemData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "条目编号", sortable: false },
            { data: 'ItemName', title: "条目名称", sortable: false },
            { data: 'OptionText', title: "分类", sortable: false },
            { data: 'ItemDesc', title: "说明", sortable: false },
            {
                data: 'ItemEnable', title: "是否启用", sortable: false, render: function (r) {
                    return r == true ? "启用" : "未启用";
                }
            },
            { data: 'ItemAddedTime', title: "创建时间", sortable: true },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.ItemEnable)
                        return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-danger\" onclick=\"Inactive(" + obj.BaseDataID + ")\">禁用</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                    else
                        return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-danger\" onclick=\"active(" + obj.BaseDataID + ")\">启用</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'itemCode', value: $("#txtItemCode").val() });
            d.push({ name: 'itemName', value: $("#txtItemName").val() });
            d.push({ name: 'itemClass', value: $("#drpItemClass").val() });
            d.push({ name: 'itemEnable', value: $("#chbEnable").prop('checked') });
        }
    });
    //查询
    $("#btnSearch").click(function () {
        dt_ItemData.fnDraw();
    })
    //新增条目
    $("#frmAddItem").submit(function (e) {
        e.preventDefault();
        if (DataValidatorAdd.form()) {
            var itemCode = $("#addItemCode").val();
            var itemName = encode($("#addItemName").val());
            var itemClass = $("#addItemClass").val();
            var remark = encode($("#addRemark").val());
            var enable = $("#addChbEnable").prop('checked');

            ajax("/BaseData/AddItemData", { itemId: itemCode, itemName: itemName, itemType: itemClass, remark: remark,enable:enable }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();
                    dt_ItemData.fnDraw();
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
    //        dt_ItemData.fnDraw();
    //        $.dialog(res);
    //    });
    //})
    //修改条目
    $("#frmEditItem").submit(function (e) {
        e.preventDefault();
        if (DataValidatorEdit.form()) {
            var item = {
                BaseDataID: $("#editItemCode").val(),
                ItemName: $("#editItemName").val(),
                ItemType: $("#editItemClass").val(),
                ItemDesc: encode($("#editRemark").val()),
                ItemEnable:$("#editChbEnable").prop('checked'),
                DataGroupID: $("#txtDataGroupId").val(),
                BaseDataType: $("#txtDataType").val(),
                ItemAddedTime: $("#txtAddDate").val(),
            }
            ajax("/BaseData/UpdateItemData", { model: item }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();

                    var start = dt_ItemData.fnSettings()._iDisplayStart;
                    var length = dt_ItemData.fnSettings()._iDisplayLength;
                    dt_ItemData.fnPageChange(start / length, true);
                    //dt_ItemData.fnDraw();
                    $.dialog(res.MSG);
                } else { $.dialog(res.MSG); }
            });
        }
    })
    //修改条目
    //$("#btnEditSave").click(function () {
    //    ajax("/BaseData/UpdateItemData", { itemId: $("#editItemCode").val(), remark: $("#editRemark").val() }, function (res) {
    //        $.colorbox.close();
    //        dt_ItemData.fnDraw();
    //        $.dialog(res);
    //    });
    //})
})
//新增时验证数据
var DataValidatorAdd = $("#frmAddItem").validate({
    //onSubmit: false,
    rules: {
        addItemName: {
            required: true,
            maxlength: 20,
            isOnlyLNC: true,
        },
        addItemClass: {
            required: true,
        },
        addRemark: {
            required: true,
            maxlength: 100,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
var DataValidatorEdit = $("#frmEditItem").validate({
    //onSubmit: false,
    rules: {
        editRemark: {
            required: true,
            maxlength: 100,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//编辑条目信息
function edit(id) {
    ajax("/BaseData/GetItemById", { itemId: id }, function (res) {
        $("#editItemCode").val(res.BaseDataID);
        $("#editItemName").val(res.ItemName);
        $("#editItemClass").val(res.ItemType);
        $("#editRemark").val(res.ItemDesc);
        $("#editChbEnable").prop('checked', res.ItemEnable);

        $("#txtDataGroupId").val(res.DataGroupID);
        $("#txtDataType").val(res.BaseDataType);
        $("#txtAddDate").val(res.ItemAddedTime);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#editItem_dialog",
        inline: true
    });
}

//新建条目
function add() {
    //清空数据
    $("#addItemCode").val('');
    $("#addItemName").val('');
    $("#addItemClass").val('').removeClass('error-block');
    $("#addRemark").val('');
    $("#addChbEnable").prop('checked',false);
    
    $('.error-block').html('');
    //显示新建页面弹窗
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

//启用条目
function active(id) {
    //var item = {
    //    BaseDataID: obj.BaseDataID,
    //    ItemName: obj.ItemName,
    //    ItemType: obj.ItemType,
    //    ItemDesc: obj.ItemDesc,
    //    DataGroupID: obj.DataGroupID,
    //    BaseDataType: obj.BaseDataType,
    //    ItemAddedTime: obj.ItemAddedTime,
    //}
    ajax("/BaseData/ActiveItemById", { itemId: id }, function (res) {
        if (res.IsPass) {
            $.dialog(res.MSG);
            //dt_ItemData.fnDraw();

            var start = dt_ItemData.fnSettings()._iDisplayStart;
            var length = dt_ItemData.fnSettings()._iDisplayLength;
            dt_ItemData.fnPageChange(start / length, true);
        } else { $.dialog(res.MSG); }
    });
    //showDialogMessage("启用成功");
}

//禁用条目
function Inactive(id) {
    //var item = {
    //    BaseDataID: obj.BaseDataID,
    //    ItemName: obj.ItemName,
    //    ItemType: obj.ItemType,
    //    ItemDesc: obj.ItemDesc,
    //    DataGroupID: obj.DataGroupID,
    //    BaseDataType: obj.BaseDataType,
    //    ItemAddedTime: obj.ItemAddedTime,
    //}
    ajax("/BaseData/InActiveItemById", { itemId: id }, function (res) {
        if (res.IsPass) {
            $.dialog(res.MSG);

            var start = dt_ItemData.fnSettings()._iDisplayStart;
            var length = dt_ItemData.fnSettings()._iDisplayLength;
            dt_ItemData.fnPageChange(start / length, true);
            //dt_ItemData.fnDraw();
        } else { $.dialog(res.MSG); }
    });
    //showDialogMessage("停用成功");
}

//删除条目
function deleteItem(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteItemById", { itemId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_ItemData.fnDraw();
            } else { $.dialog(res.MSG); }
        });
        //showDialogMessage("删除成功");
    })
}

//加载条目类型
function LoadItemType() {
    //$('#addItemClass').empty();
    ajax('/BaseData/GetItemTypeList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpItemClass').append(opt);
            $('#addItemClass').append(opt);
            $("#editItemClass").append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpItemClass').append(opt);
            $('#addItemClass').append(opt);
            $("#editItemClass").append(opt);
        }
    });

}