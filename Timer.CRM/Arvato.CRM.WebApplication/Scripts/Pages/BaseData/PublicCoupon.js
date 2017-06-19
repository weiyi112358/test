var dt_StoreDataBox;
$(function () {
    //加载条目类型
    LoadStoreType();
    //加载品牌
    LoadStoreBrand();
    //加载优惠券列表
    LoadCouponList();
    //加载数据表格
    dt_StoreData = $('#dt_CouponData').dataTable({
        sAjaxSource: '/BaseData/GetCouponData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[4, "desc"]],
        aoColumns: [
            { data: 'BatchNo', title: "批号", sortable: true, },
            {
                data: null, title: "券名", sortable: false, render: function (r) {
                    var show = r.CouponName;
                    show = show.length >= 9 ? (show.substr(0, 9) + "...") : show;
                    return "<span title='" + r.CouponName + "'>" + show + "</span>";
                }
            },
            { data: 'CouponType', title: "券类型", sortable: false, },
            { data: 'CouponCounts', title: "生成数量", sortable: false, },
            { data: 'AddedDate', title: "创建时间", sortable: true, },
            {
                data: 'StartDate', title: "有效起始时间", sortable: true, render: function (obj) {
                    //var str = obj == null ? "" : obj.substr(0, 10);
                    //if (str == '1900-01-01') return "";
                    return obj == null ? "" : obj.substr(0, 10);
                }
            },
            {
                data: 'EndDate', title: "有效结束时间", sortable: true, render: function (obj) {
                    //var str = obj == null ? "" : obj.substr(0, 10);
                    //if (str == '9999-12-31') return "";
                    return obj == null ? "" : obj.substr(0, 10);
                }
            },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.Enable)
                        return "<button class=\"btn detail\" id=\"btnModify\"  onclick=\"edit('" + obj.BatchNo + "')\">查看明细</button><button class=\"btn export\" onclick=\"exportCoupon('" + obj.BatchNo + "')\">导出</button><button class=\"btn btn-danger\" onclick=\"Inactive('" + obj.BatchNo + "')\">禁用</button>";
                    else
                        return "<button class=\"btn detail\" id=\"btnModify\"  onclick=\"edit('" + obj.BatchNo + "')\">查看明细</button><button class=\"btn export\" onclick=\"exportCoupon('" + obj.BatchNo + "')\">导出</button><button class=\"btn btn-danger\" onclick=\"active('" + obj.BatchNo + "')\">启用</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'batchNo', value: $.trim($("#txtBatchNo").val()) });
            d.push({ name: 'templetID', value: $("#drpTempletID").val() });
            d.push({ name: 'status', value: $("#status").val() });
            d.push({ name: 'templetName', value: $("#txtTempletName").val() });
        }
    });
    //查询
    $("#btnSearch").click(function () {
        dt_StoreData.fnDraw();
    })
    //新增条目
    $("#frmAddCoupon").submit(function (e) {
        e.preventDefault();
        if (DataValidatorAdd.form()) {
            var prefix = $("#addPrefix").val();
            //var afterfix = $("#addAfterfix").val();
            var couponLength = $("#addCouponLength").val();
            var couponCounts = $("#addCouponCounts").val();
            var couponName = encode($("#addCouponName").val());
            var templetID = $("#addTempletID").val();
            var reg = /^[a-zA-Z](?![a-zA-Z]+$)[a-zA-Z0-9]{2}\d*$/
            if (!prefix.match(reg)) {
                $.dialog("前缀必须以英文字母开头，并包含数字");
                return;
            }
            ajax("/BaseData/AddPublicCouponData", { prefix: prefix, couponLength: couponLength, couponCounts: couponCounts, couponName: couponName, templetID: templetID }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();
                    dt_StoreData.fnDraw();
                    $.dialog(res.MSG);
                } else $.dialog(res.MSG);
            });
        }
    })

    //修改条目
    $("#frmEditStore").submit(function (e) {
        e.preventDefault();
        if (DataValidatorEdit.form()) {
            var store = {
                BaseDataID: $("#editStoreId").val(),
                DataGroupID: $("#txtDataGroupId").val(),
                BaseDataType: $("#txtDataType").val(),
                StoreName: encode($("#editStoreName").val()),
                StoreAddress: encode($("#editAddress").val()),
                StoreCodeSale: $("#editStoreCodeSale").val(),
                StoreCode: $("#txtCode").val(),
                StoreBrandCode: $("#editStoreBrand").val(),
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

})

var length = $("#editStoreId").val();
//新增时验证数据
var DataValidatorAdd = $("#frmAddCoupon").validate({
    //onSubmit: false,
    rules: {
        addPrefix: {
            required: true,
            minlength:3,
            maxlength: 3,
        },
        //addAfterfix: {
        //    required: true,
        //    maxlength: 10,
        //},
        addStoreCodeSale: {
            required: true,
            maxlength: length,
            digits: true,
        },
        addCouponCounts: {
            required: true,
            maxlength: 20,
            digits: true,
            min: 1,
        },
        addCouponLength: {
            required: true,
            min: 5,
            max: 10,
        },
        addCouponName: {
            required: true,
            maxlength: 20,
        },
        addTempletID: {
            required: true
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
            isOnlyLNC: true
        },
        editStoreBrand: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//编辑条目信息
function edit(b) {
    $("#hnBatchNo").val(b);
    if (!dt_StoreDataBox) {
        dt_StoreDataBox = $('#dt_CouponDetailData').dataTable({
            sAjaxSource: '/BaseData/GetCouponDetailData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: 'CouponID', title: "券编号", sortable: true, },
                { data: 'CouponCode', title: "券代码", sortable: false, },
                {
                    data: 'IsUsed', title: "使用状态", sortable: false, render: function (obj) {
                        return obj == true ? "已用" : "未用";
                    }
                },
                { data: 'TempletName', title: "所属模板", sortable: false, sWidth: "250px" }
            ],
            fnFixData: function (d) {
                d.push({ name: 'batchNo', value: $("#hnBatchNo").val() });
            }
        });
    } else {
        dt_StoreDataBox.fnDraw();
    }
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

//导出券
function exportCoupon(b) {
    $('#exportForm')[0].action = "/BaseData/ExportPublicCoupon";
    $('#exportForm #exprBatchNo').val(b);
    $('#exportForm')[0].submit();
}

//启用条目
function active(num) {

    $.dialog("确认启用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/ActiveCoupon", { batchNo: num }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                var start = dt_StoreData.fnSettings()._iDisplayStart;
                var length = dt_StoreData.fnSettings()._iDisplayLength;
                dt_StoreData.fnPageChange(start / length, true);
                //loadRuleList();
            } else { $.dialog(res.MSG); }
        });
    })
}

//禁用条目
function Inactive(num) {
    $.dialog("确认禁用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/InActiveCoupon", { batchNo: num }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                //loadRuleList();

                var start = dt_StoreData.fnSettings()._iDisplayStart;
                var length = dt_StoreData.fnSettings()._iDisplayLength;
                dt_StoreData.fnPageChange(start / length, true);
            } else { $.dialog(res.MSG); }
        });
    })
}


//禁用
//function forbidden(num,bool) {
//    ajax('/BaseData/Forbidden', {exprBatchNo:num,status:bool}, function (res) {
       
//    });
    
    

//}


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
    $("#addTempletID").val('');
    $("#addPrefix").val('');
    //$("#addAfterfix").val('');
    //$("#addCouponLength").val('');
    $("#addCouponCounts").val('');
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
    ajax('/BaseData/GetStroeGroupList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].SubDataGroupID + '>' + res[i].SubDataGroupName + '</option>';
            }
            $('#drpStoreClass').append(opt);
            if (res.length == 1) $('#drpStoreClass,#labstore').hide();
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStoreClass').append(opt);
        }
    });
}

function LoadStatus() {
    

}




//加载门店所属品牌
function LoadStoreBrand() {
    //$('#editStoreBrand').empty();
    //$('#addStoreBrand').empty();
    ajax('/BaseData/GetVehicleBrandList', null, function (res) {
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
//加载优惠券模板
function LoadCouponList() {
    ajax('/BaseData/GetCouponList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].CouponID + '>' + res[i].CouponName + '</option>';
            }
            $('#addTempletID').append(opt);
            $('#drpTempletID').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#addTempletID').append(opt);
            $('#drpTempletID').append(opt);
        }
    });

}