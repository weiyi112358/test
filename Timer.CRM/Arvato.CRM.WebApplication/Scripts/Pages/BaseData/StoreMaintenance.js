//门店维护
var reg0 = new RegExp("^[0-9]\\d*$");
var dt_StoreTable;
var StoreMaintenance = {
    //门店加载
    LoadStoreMaintenance: function () {
        dt_StoreTable = $('#dt_StoreTable').dataTable({
            sAjaxSource: '/BaseData/GetStoreMaintenanceData',
            bAutoWidth: false,
            bSort: false,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                {
                    data: null, title: "门店名称",sClass: "center", sortable: false, render: function (r) {
                        var show = r.StoreName;
                        if (show == null || show == undefined || show == "") {
                            show = "";
                        }
                        else {
                            show = show.length > 8 ? (show.substr(0, 8) + "...") : show;
                        }
                        return "<span title='" + r.StoreName + "'>" + show + "</span>";

                    }
                },
                {
                    data: null, title: "门店地址", sClass: "center", sortable: false, render: function (r) {
                        var show = r.StoreAddress;
                        if (show == null || show == undefined || show == "") {
                            show = "";
                        }
                        else {
                            show = show.length > 8 ? (show.substr(0, 8) + "...") : show;
                        }
                        return "<span title='" + r.StoreAddress + "'>" + show + "</span>";
                    }
                },
                //{
                //    data: null, title: "经销商", sClass: "center", sortable: false, render: function (r) {
                //        var show = r.StoreFullName;
                //        if (show == null || show == undefined || show == "") {
                //            show = "";
                //        }
                //        else {
                //            show = show.length > 8 ? (show.substr(0, 8) + "...") : show;
                //        }
                //        return "<span title='" + r.StoreFullName + "'>" + show + "</span>";
                //    }
                //},
                  {
                    data: null, title: "经销商", sClass: "center", sortable: false, render: function (r) {
                        var show = r.ChannelNameStore;
                        return "<span title='" + r.ChannelNameStore + "'>" + show + "</span>";
                    }
                },
                {
                    data: "StoreCode", title: "门店代码", sClass: "center", sortable: false
                },
                {
                    data: "AreaNameStore", title: "区域名称", sClass: "center", sortable: false
                },
                {
                    data: "StoreType", title: "门店类型", sClass: "center", sortable: false
                },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button>";
                        //"<button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>"
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'StoreName', value: $.trim($("#txt_StoreName").val()) });
                d.push({ name: 'ChannelCodeStore', value: $("#txt_ChannelCodeStore").val() });
                d.push({ name: 'AddressStore', value: $.trim($("#txt_AddressStore").val()) });
                d.push({ name: 'groupId', value: $("#drpStoreClass").val() }); 
                d.push({ name: 'storeCode', value: $("#txt_StoreCode").val() });
            }
        });
    }
};

$(function () {

    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }

    //加载门店所属群组
    LoadStoreType();
    StoreMaintenance.LoadStoreMaintenance();
    loadStore();

    $("#btnSerach").bind("click", function () {
        var storeCode = $('#txt_StoreCode').val();
        if (storeCode!=""&&(reg0.test(storeCode)==false||storeCode.length>7)) {
            $.dialog("请输入不大于7位的数字代码");
            return false;
        }
        $('#hideDrpStoreClass').val($('#drpStoreClass').val());
        $('#hideStoreName').val($('#txt_StoreName').val());
        $('#hideChannelCodeStore').val($('#txt_ChannelCodeStore').val());
        $('#hideAddressStore').val($('#txt_AddressStore').val());
        $('#hideStoreCode').val($('#txt_StoreCode').val());
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
            var store = {
                BaseDataID: $("#txtStoreId").val(),//门店编号
                StoreCode: $("#txtStoreCode").val(),//门店代码
                StoreName: $("#txtStoreName").val(),//名称
                StoreAddress: $("#txtStoreAddress").val(),//地址
                StoreFullName: $("#txtStoreFullName").val(),//全名
                StoreType: $("#txtStoreType").val(),//类型
                StoreTel: $("#txtStoreTel").val(),//电话
                DataGroupID: $("#editStoreClass").val(),//群组
                AreaCodeStore: $("#txtAreaNameStore").val(),//区域代码
                AreaNameStore: $("#txtAreaNameStore").find("option:selected").text(),//区域名称
                ChannelTypeCodeStore: $("#txtChannerTypeNameStore").val(),//渠道代码
                ChannerTypeNameStore: $("#txtChannerTypeNameStore").find("option:selected").text(),//渠道名称
                ProvinceCodeStore: $("#txtProvinceStore").val(),//省代码
                ProvinceStore: $("#txtProvinceStore").find("option:selected").text(),//省名称
                CityCodeStore: $("#txtCityStore").val(),//市代码
                CityStore: $("#txtCityStore").find("option:selected").text(),//市名称
                StoreBrandCode: $("#txtStorecodeIPOS").val(),
                //,
                BrandStore: $("#txtStorecodeIPOS").find("option:selected").text()
            }
            if ($("#editStoreClass").val() == "") {
                $.dialog("群组为必选项");
                return;
            }
            //if ($("#txtAreaNameStore").val() == "") {
            //    $.dialog("区域为必选项");
            //    return;
            //}
            if ($("#txtChannerTypeNameStore").val() == "") {
                $.dialog("渠道为必选项");
                return;
            }
            //if ($("#txtProvinceStore").val() == "") {
            //    $.dialog("省份为必选项");
            //    return;
            //}
            //if ($("#txtCityStore").val() == "") {
            //    $.dialog("城市为必选项");
            //    return;
            //}
            //if ($("#editStoreBrand").val() == "") {
            //    $.dialog("品牌为必选项");
            //    return;
            //}
            //增加
            if (store.BaseDataID == '') {
                ajax("/BaseData/AddStoreMaintenanceData", { model: store }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_StoreTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateStoreMaintenanceData", { model: store }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        var start = dt_StoreTable.fnSettings()._iDisplayStart;
                        var length = dt_StoreTable.fnSettings()._iDisplayLength;
                        dt_StoreTable.fnPageChange(start / length, true);
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    });

    ////动态获取市
    //$("#txtProvinceStore").bind("change", function () {
    //    getcity()
    //});
});


//清空数据
function goClear() {
    $("#txtStoreId").val(''),//门店编号
        $("#txtStoreCode").val(''),//门店代码
        $("#txtStoreName").val(''),//名称
        $("#txtStoreAddress").val(''),//地址
        $("#txtStoreFullName").val(''),//全名
        $("#txtStoreType").val(''),//类型
        $("#txtStoreTel").val(''),//电话
        $("#editStoreClass").val(''),//群组
        $("#txtAreaNameStore").val(''),//区域代码
        $("#txtChannerTypeNameStore").val(''),//渠道代码
        $("#txtProvinceStore").val(''),//省代码
        $("#txtCityStore").val(''),//市代码
        $("#txtStorecodeIPOS").val('')
    //,
    //$("#editStoreBrand").val('') //品牌

    $('.error-block').html('');
}

//编辑
function edit(id) {
    $("#addStore_dialog .heading h3").html("门店编辑");
    //清空数据
    goClear();
    var model;
    $("#txtStoreCode").attr("disabled", "disabled");
    ajax("/BaseData/GetStoreMaintenanceById", { StoreId: id }, function (res) {
        $('#txtProvinceStore').trigger("change");
        $("#txtStoreId").val(res.BaseDataID),//门店编号
        $("#txtStoreCode").val(res.StoreCode),//门店代码
        $("#txtStoreName").val(res.StoreName),//名称
        $("#txtStoreAddress").val(res.StoreAddress),//地址
        $("#txtStoreFullName").val(res.StoreFullName),//全名
        $("#txtStoreType").val(res.StoreType),//类型
        $("#txtStoreTel").val(res.StoreTel),//电话
        $("#editStoreClass").val(res.DataGroupID),//群组
        $("#txtAreaNameStore").val(res.AreaCodeStore),//区域代码
        $("#txtChannerTypeNameStore").val(res.ChannelTypeCodeStore),//渠道代码
        $("#txtProvinceStore").val(res.ProvinceCodeStore),//省代码
        //$('#txtProvinceStore').trigger("change");
        $("#txtCityStore").val(res.CityCodeStore),//市代码
        $("#txtStorecodeIPOS").val(res.BrandCodeStore)
        //    ,
        //$("#editStoreBrand").val(res.StoreBrandCode) //品牌
        getcity(res.CityCodeStore);
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
        ajax("/BaseData/DeleteStoreMaintenanceById", { storeId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//加载门店所属群组
function LoadStoreType() {
    ajax('/BaseData/GetStroeGroupList', null, function (res) {
        if (res.length > 0) {
            var opt = "";//<option value=''>请选择</option>
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


//加载门店
function loadStore() {
    var $store = $('#txt_StoreName');
    $store.empty();
    var postdata = { companyCode: "直营店" };
    $.post('/Distribution/LoadStore', postdata, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.ShoppeCode + '">' + data.ShoppeName + '/' + data.ShoppeCode + '<option>'
            });
            $store.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $store.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};
