//门店维护
var isadd = false;
var city;
var dt_StoreTable;
var StoreMaintenance = {
    //门店加载
    LoadStoreMaintenance: function () {
        dt_StoreTable = $('#dt_SupplierTable').dataTable({
            sAjaxSource: '/BaseData/GetSupplierData',
            bAutoWidth: false,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: 'SupplierCode', title: "代码", sortable: false, sClass: "center"
                },
                {
                    data: 'SupplierName', title: "名称", sortable: false, sClass: "center"
                },
                {
                    data: 'SupplierPhone', title: "电话", sortable: false, sClass: "center"
                },

                {
                    data: null, title: "地址", sortable: false,sClass: "center", render: function (obj) {
                        if (obj.SupplierAddress != undefined) {
                            return obj.SupplierAddress.replace(/_/g, "");
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AddedDate", title: "最后修改时间", sortable: false, sClass: "center", render: function (obj) {
                        return obj.substring(0, 10);
                    }
                },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,sClass: "center",
                    render: function (obj) {
                        var text = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"stopChannel(" + obj.BaseDataID + ")\">禁用</button>";
                        return text;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'SupplierCode', value: $.trim($("#txt_Code").val()) });
                d.push({ name: 'SupplierName', value: $("#txt_Name").val() });
            }
        });
    }
};

function SearchDate() {
    if (dt_StoreTable) {
        dt_StoreTable.fnDraw();
    } else {
        StoreMaintenance.LoadStoreMaintenance();
    }
}


$(function () {    
    
    $("#btnSerach").bind("click", function () {
        SearchDate();        
    });
    
    //验证数据
    var DataValidator = $("#frmAddSupplier").validate({
        rules: {
            txtCode: {
                required: true,
                maxlength: 20,
            },
            txtName: {
                required: true,
                maxlength: 100,
            },
            txtPhone: {
                required: false,
                maxlength: 100,
                isMobileNo: true
            },
            txtFax: {
                required: false,
                maxlength: 100,                
            },
            txtProvince: {
                required: false,
                maxlength: 100,
            },           
            txtCity: {
                required: false,
                maxlength: 100,
            }

        },
        errorPlacement: function (error, element) {
            error.appendTo(element.next("span.error-block"));
        },
        errorClass: 'error-block',
    });

    $("#frmAddSupplier").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            
            var address = "";
            if ($("#txtProvince").val() != "") {
                address += $.trim($("#txtProvince").children(":selected").text());
            }
            if ($("#txtCity").val() != "") {
                address += $.trim($("#txtCity").children(":selected").text());
            }
            var supplier = {
                BaseDateID:$("#supplierid").val(),
                SupplierCode: $("#txtCode").val(),
                SupplierName: $("#txtName").val(),
                SupplierPhone: $("#txtPhone").val(),
                SupplierAddress: address,
                SupplierFax: $("#txtFax").val(),
                SupplierContactPerson: $("#txtContactPerson").val()
            }
            if (isadd) {
                ajax("/BaseData/AddSupplierData", { model: supplier }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        //StoreMaintenance.LoadStoreMaintenance();
                        SearchDate();
                        //$.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {
                ajax("/BaseData/UpdateSupplier", { model: supplier }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        //StoreMaintenance.LoadStoreMaintenance();
                        SearchDate();
                        //$.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
            //增加
            //if (store.BaseDataID == '') {
            //    ajax("/BaseData/AddSupplierData", { model: supplier }, function (res) {
            //        if (res.IsPass) {
            //            $.colorbox.close();
            //            dt_StoreTable.fnDraw();
            //            $.dialog(res.MSG);
            //        } else $.dialog(res.MSG);
            //    });
            //} else {//编辑
            //    ajax("/BaseData/UpdateStoreMaintenanceData", { model: supplier }, function (res) {
            //        if (res.IsPass) {
            //            $.colorbox.close();
            //            var start = dt_StoreTable.fnSettings()._iDisplayStart;
            //            var length = dt_StoreTable.fnSettings()._iDisplayLength;
            //            dt_StoreTable.fnPageChange(start / length, true);
            //            $.dialog(res.MSG);
            //        } else $.dialog(res.MSG);
            //    });
            //}
        }
    });

    ////动态获取市
    //$("#txtProvinceStore").bind("change", function () {
    //    getcity()
    //});

    $("#txtProvince").on("change", function () {
       
        $("#txtCity").empty();

        ajax("/BaseData/GetCityByProvince", { ProvinceCode: $("#txtProvince").val() }, function (data) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < data.length; i++) {
                if (isadd == false && data[i].OptionText == city) {
                   opt += '<option value=' + data[i].OptionValue + ' selected>' + data[i].OptionText + '</option>';
                }
                opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
            }
            $('#txtCity').append(opt)

        });

    });
});





//清空数据
function goClear() {
    $("#txtCode").val('');
    $("#txtName").val('');
    $("#txtStoreName").val('');
    $("#txtPhone").val('');
    $("#txtFax").val('');
    $("#txtProvince").val('');
    $("#txtCity").empty().append("<option>请选择</option>");

    $("#txtContactPerson").val('');
    $("#supplierid").val('');
       
    //,
    //$("#editStoreBrand").val('') //品牌

    $('.error-block').html('');
}

//编辑
function edit(id) {
    $("#addBrand_dialog .heading h3").html("供应商编辑");
    //清空数据
    goClear();
    var model;
    ajax("/BaseData/GetSupplierByID", { SupplierId: id }, function (res) {
        if (res.IsPass) {
            res = res.Obj[0];
            $("#supplierid").val(id);
            $('#txtCode').val(res.SupplierCode);
            $("#txtName").val(res.SupplierName);
            $("#txtPhone").val(res.SupplierPhone);
            $("#txtFax").val(res.SupplierFax);
            $("#txtContactPerson").val(res.SupplierContactPerson);
            address = res.SupplierAddress;
            var provice;
            if (address != undefined && address != "") {
                var index = address.indexOf('_');
                if (index == -1) {
                    provice = address;
                } else {
                    provice = address.substr(0, index);
                    if (index < address.length) {
                        city = address.substr(index + 1);
                    }
                }
                                
                
                $("#txtProvince").children().each(function (e) {
                    if ($.trim($(this).text()) == provice) {
                        $(this).attr("selected", "true");
                        $("#txtProvince").trigger("change");
                        return false;
                    }
                });
            }
        } else {
            $.dialog(res.MSG);
        }
        //$("#editStoreBrand").val(res.StoreBrandCode) //品牌
        
    });


    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addBrand_dialog",
        inline: true
    });
    $.colorbox.resize();
    isadd = false;
}

//删除条目
function deleteItem(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteStoreMaintenanceById", { storeId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}


//弹窗
function goEdit() {
    $("#addBrand_dialog .heading h3").html("新增供应商");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addBrand_dialog",
        inline: true
    });
    $.colorbox.resize();
    isadd = true;
}

function stopChannel(basedataID) {
    $.dialog("确认禁用吗？", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        var postdata = { basedataID: basedataID };
        $.post('/BaseData/StopChannel', postdata, function (result) {
            if (result.IsPass) {
                $.dialog("禁用成功");
                dt_StoreTable.fnDraw();
            }
            else {
                $.dialog("禁用失败");
            }
        }, 'json');
    })
}
