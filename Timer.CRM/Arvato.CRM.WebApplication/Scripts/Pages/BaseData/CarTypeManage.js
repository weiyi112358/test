//卡片类型维护

var dt_StoreTable;
var StoreMaintenance = {
    //门店加载
    LoadStoreMaintenance: function () {
        dt_StoreTable = $('#dt_StoreTable').dataTable({
            sAjaxSource: '/BaseData/GetCarTypeData',
            bAutoWidth: false,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: null, title: "卡片类型名称", sortable: false, render: function (r) {
                        var show = r.CardTypeNameBase;
                        if (show == null || show == undefined || show == "") {
                            show = "";
                        }
                        else {
                            show = show.length > 8 ? (show.substr(0, 8) + "...") : show;
                        }
                        return "<span title='" + r.CardTypeNameBase + "'>" + show + "</span>";

                    }
                },
                {
                    data: null, title: "卡片类型代码", sortable: false, render: function (r) {
                        var show = r.CardTypeCodeBase;
                        if (show == null || show == undefined || show == "") {
                            show = "";
                        }
                        else {
                            show = show.length > 8 ? (show.substr(0, 8) + "...") : show;
                        }
                        return "<span title='" + r.CardTypeCodeBase + "'>" + show + "</span>";
                    }
                },
                //{
                //    data: null, title: "卡片状态", sortable: false, render: function (r) {
                //        var show = r.CardTypeStatus;
                //        if (show == null || show == undefined || show == "") {
                //            show = "";
                //        }
                //        else {
                //            if (show == 1) {
                //                show = "可用";
                //            } else {
                //                show = "不可用";
                //            }
                //        }
                //        return "<span title='" + r.CardTypeStatus + "'>" + show + "</span>";
                //    }
                //},
                
                {
                    data: "AddedUser", title: "添加用户", sortable: false
                },
                {
                    data: "AddedDate", title: "添加时间", sortable: false
                },
                {
                    data: "ModifiedUser", title: "修改用户", sortable: false
                },
                {
                    data: "ModifiedDate", title: "修改时间", sortable: false
                },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                        //"<button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>"
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'CarName', value: $.trim($("#txt_cartype").val()) });
                d.push({ name: 'CarCode', value: $.trim($("#txt_carcode").val()) });
             
            }
        });
    }
};
var Id = "";
$(function () {
    //加载数据
    StoreMaintenance.LoadStoreMaintenance();

    $("#btnSerach").bind("click", function () {
        dt_StoreTable.fnDraw();
    });
    var option = "<option value='null'>空</option>"
    
    $("#txt_ChannelCodeStore").append(option);
    //验证数据
    var DataValidator = $("#frmAddStore").validate({
        rules: {
            txtStoreId: {
                required: true,
                maxlength: 20,
            },
            txtStoreName: {
                required: true,
                maxlength: 100,
            },
            txtStoreAddress: {
                required: false,
                maxlength: 100,
            },
            txtStoreFullName: {
                required: false,
                maxlength: 100,
            },
            txtStoreType: {
                required: false,
                maxlength: 100,
            },
            txtStoreCode: {
                required: true,
                maxlength: 100,
            },
            txtStorecodeIPOS: {
                required: true,
            }

        },
        errorPlacement: function (error, element) {
            error.appendTo(element.next("span.error-block"));
        },
        errorClass: 'error-block',
    });

    $("#frmAddStore").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
     
            var carname=$("#txtCarName").val();
            var carcode = $("#txtCarCode").val();
            //var status = $("#status1").val();
            if (carname=="") {
                $.dialog("卡片类型名称不可以为空!");
                return;
            }
            if (carcode == "") {
                $.dialog("卡片类型代码不可以为空!");
                return;
            }
            if (Id == '') {
                ajax("/BaseData/AddCarTypeData", {carName: carname, carCode: carcode }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_StoreTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateCarTypeData", { id: Id, carName: carname, carCode: carcode }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        var start = dt_StoreTable.fnSettings()._iDisplayStart;
                        var length = dt_StoreTable.fnSettings()._iDisplayLength;
                        dt_StoreTable.fnPageChange(start / length, true);
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
            Id = "";
        }
    });
});


//清空数据
function goClear() {
    $("#txtCarName").val(""),//卡片名称
    $("#txtCarCode").val(""),//卡片代码

    $('.error-block').html('');
}

//编辑
function edit(id) {
    $("#addStore_dialog .heading h3").html("门店编辑");
    //清空数据
    goClear();
    Id = id;
    //$("#txtStoreCode").attr("disabled", "disabled");
    ajax("/BaseData/GetCarTypeById", { id: id }, function (res) {
    
        $("#txtCarName").val(res.CardTypeNameBase),//卡片类型名称
        $("#txtCarCode").val(res.CardTypeCodeBase)//卡片类型代码
        $("#status1").val(res.CardTypeStatus)//卡片类型代码
    });

    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addStore_dialog",
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
        ajax("/BaseData/DeleteCarType", { Id: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//弹窗
function goEdit() {
    $("#addStore_dialog .heading h3").html("门店新增");
    //清空数据
    goClear();
    $("#txtStoreType").val("直营店");
    $("#txtStoreCode").attr("disabled", false);
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addStore_dialog",
        inline: true
    });
    $.colorbox.resize();
}

function getcity(CityCodeStore) {
    $("#txtCityStore").empty();

    ajax("/BaseData/GetCityByProvince", { ProvinceCode: $("#txtProvinceStore").val() }, function (data) {
        var opt = "<option value=''>请选择</option>";
        for (var i = 0; i < data.length; i++) {

            opt += '<option value=' + data[i].OptionValue + '>' + data[i].OptionText + '</option>';
        }
        $('#txtCityStore').append(opt)
        if (CityCodeStore) {
            $("#txtCityStore").val(CityCodeStore)
        }
    });
}
