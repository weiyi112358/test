var dtPackageDetail;
$(function () {

    //加载单位下拉框
    GetUnitList();
    //加载条目类型
    GetItemList();
    //加载使用限制
    GetLimitList();
    //初始化多选插件
    $(".chzn_b").chosen();
    GetActLimitBrandList();
    GetActLimitStoreList();
    $(".chzn_brand").chosen();
    $(".chzn_store").chosen();

    //获取IDOS套餐列表
    GetIDOSPackageList(null);
    initPagesMultiSelect();
    //初始化日期时间控件
    $('#txt_StartDate,#txt_EndDate').datepicker();

    dt_package = $('#dt_package').dataTable({
        sAjaxSource: '/BaseData/GetPackageList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 5,
        aoColumns: [
            //{ data: 'PackageID', title: "编号", sortable: true },
            { data: 'PackageName', title: "套餐名称", sortable: false },
            { data: 'Price1', title: "销售价格", sortable: false },
            { data: 'limitName', title: "限定条件", sortable: false },
            //{ data: 'Price2', title: "内部结算价", sortable: false },
            //{
            //    data: 'Proportion', title: "结算比例", sortable: false
            //},
            {
                data: 'Enable', title: "启用状态", sortable: false, render: function (obj) {
                    return obj == true ? "启用" : "未启用";
                }
            },
            {
                data: "StartDate", title: "销售有效起始日期", sortable: true, render: function (obj) {
                    return !obj ? "" : obj.substr(0, 10);
                }
            },
                {
                    data: "EndDate", title: "销售有效结束日期", sortable: true, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
            {
                data: 'AddedDate', title: "创建时间", sortable: true, render: function (obj) {
                    return !obj ? "" : obj.substr(0, 10);
                }
            },
            {
                data: null, title: "操作", sClass: "center", sWidth: "20%", sortable: false,
                render: function (obj) {
                    if (!obj.Enable)
                        return "<button class=\"btn\" onclick=\"goEditPackage(" + obj.PackageID + ")\">编辑</button> <button class=\"btn\" onclick=\"goDeletePackage(" + obj.PackageID + ")\">删除</button><button class=\"btn btn-info\" onclick=\"goPackageActive(" + obj.PackageID + ")\">激活</button><button class=\"btn\" onclick=\"goPackageDetail(" + obj.PackageID + ",'" + obj.limitCode + "')\">明细</button>";
                    else
                        return "<button class=\"btn\" onclick=\"goEditPackage(" + obj.PackageID + ")\">编辑</button> <button class=\"btn\" onclick=\"goDeletePackage(" + obj.PackageID + ")\">删除</button><button class=\"btn btn-danger\" onclick=\"goPackageInActive(" + obj.PackageID + ")\">禁用</button><button class=\"btn\" onclick=\"goPackageDetail(" + obj.PackageID + ",'" + obj.limitCode + "')\">明细</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'pName', value: $("#txtPackageName").val() });
            d.push({ name: 'enable', value: $("#isEnable").val() });
        }
    });
    //搜索查询套餐信息
    $("#btnSearch").click(function () {
        //加载套餐列表
        dt_package.fnDraw();
        //清空明细表格
        //goPackageDetail('');
        $("#dtPackageDetail tbody").html('');
        $("#dtPackageDetail_info").html('暂无记录');
        $("#dtPackageDetail_paginate").html('<ul><li class="prev disabled"><a href="#">上一页</a></li><li class="next disabled"><a href="#">下一页</a></li></ul>');
        //清空套餐明细的id
        $("#txtPackageId").val('');
    })

    //保存套餐信息
    $("#frmValidatePackage").submit(function (e) {
        e.preventDefault();
        if (DataValidatorPackage.form()) {
            var pId = $("#txtPId").val();
            var pro="", p2="";
            if ($("#ClearingType").val() == "1") {
                pro = $("#txtValue1").val() + ":" + $("#txtValue2").val();
            } else if ($("#ClearingType").val() == "2") {
                p2 = $("#txt_Price2").val();
            } else {
                pro = ""; p2 = "";
            }
            var packageModel = {
                PackageID: pId,
                PackageName: encode($("#txt_PackageName").val()),
                PackageDesc: encode($("#txt_PackageDesc").val()),
                StartDate: $("#txt_StartDate").val(),
                EndDate: $("#txt_EndDate").val(),
                AppendQty: $("#txt_AppendQty").val(),
                AppendUnit: $("#drp_Unit").val(),
                Price1: $("#txt_Price1").val(),
                Price2: p2,//内部结算价
                Proportion: pro,
                //MaxSetPrice: $("#txt_MaxPrice").val(),//最高结算价格
                PriceRelation: 'or',// $("#drp_PriceShip").val(),
                Enable: $("#chbEnable").prop('checked'),
            }
            if (!utility.isNull(packageModel.StartDate) && !utility.isNull(packageModel.EndDate)) {
                if (!utility.compareDate(packageModel.StartDate, packageModel.EndDate)) {
                    $.dialog("起始时间不能大于结束时间");
                    return;
                }
            }
            if (pId == '') {//新增
                var postUrl = "/BaseData/AddPackageData";
            } else {//编辑

                var postUrl = "/BaseData/UpdatePackageData";
            }
            //var packageLimit = new Array();
            //$("#drp_Limit").find("option:selected").each(function (i, data) {
            //    var value = data.value;
            //    packageLimit[i] = value;
            //});
            //--------------------------------------------------------------------------
            var actLimit = new Array();
            $("#drp_Limit").find("option:selected").each(function (i, data) {
                var value = data.value;
                actLimit.push({ LiType: value });
            });
            var limitList = new Array();
            if (Enumerable.from(actLimit).where("($.LiType == 'brand')").toArray().length > 0) {
                //限制品牌
                $("#drpLimitBrand").find("option:selected").each(function (i, data) {
                    //var label = $(this).parent("optgroup").attr("label");
                    var value = data.value;
                    limitList.push({ LimitType: "brand", LimitValue: value });
                });
                if (Enumerable.from(limitList).where("($.LimitType == 'brand')").toArray().length <= 0) {
                    $.dialog("请选择限制品牌");
                    return;
                }
            }
            if (Enumerable.from(actLimit).where("($.LiType == 'store')").toArray().length > 0) {
                //限制门店
                $("#drpLimitStore").find("option:selected").each(function (i, data) {
                    //var label = $(this).parent("optgroup").attr("label");
                    var value = data.value;
                    limitList.push({ LimitType: "store", LimitValue: value });
                });
                if (Enumerable.from(limitList).where("($.LimitType == 'store')").toArray().length <= 0) {
                    $.dialog("请选择限制门店");
                    return;
                }
            }
            //------------------------------------------------------------------------
            ajax(postUrl, { model: packageModel, limit: limitList }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();
                    var start = dt_package.fnSettings()._iDisplayStart;
                    var length = dt_package.fnSettings()._iDisplayLength;
                    dt_package.fnPageChange(start / length, true);
                    //dt_dimension.fnDraw();
                    $.dialog(res.MSG);
                } else { $.dialog(res.MSG); }
            });
        }
    })
    //清空套餐弹窗信息
    $("#btnClear").click(function () {
        clearPackageData();
    })
    //保存套餐明细信息
    //$("#btnSaveDetail").click(function () {
    $("#frmValidatePackageDetail").submit(function (e) {
        e.preventDefault();

        if (DataValidatorPackageDetail.form()) {
            var pId = $("#txtPackageId").val();
            if (pId != '') {
                var pacstr = $("#drpRolePages").val();
                var paclist = new Array();
                if (pacstr!=null) {
                    for (var i = 0; i < pacstr.length; i++) {
                        paclist.push({ IDOSPackageCode: pacstr[i].split(',')[0], StoreCOde: pacstr[i].split(',')[1] });
                    }
                }

                //var sd = $("#txtStartDate").val();
                //var ed = $("#txtEndDate").val();
                var pDetailId = $("#txtPDetailId").val();
                var pro = "", p2 = "";
                if ($("#DetailClearingType").val() == "1") {
                    pro = $("#txtDetailValue1").val() + ":" + $("#txtDetailValue2").val();
                } else if ($("#DetailClearingType").val() == "2") {
                    p2 = $("#txt_DetailPrice2").val();
                } else {
                    pro = ""; p2 = "";
                }
                var packageDetail = {
                    PackageDetailID: pDetailId,
                    PackageID: $("#txtPackageId").val(),//点击明细时，带过来的
                    ItemID: $("#drp_Item").val(),
                    ItemName: $("#drp_Item").find('option:selected').text(),
                    ItemDesc: encode($("#txt_ItemDesc").val()),
                    Qty: $("#txt_DetailQty").val(),

                    IDOSPackageMapping: JSON.stringify(paclist),

                    InterPrice: p2,//内部结算价
                    Proportion: pro,//比例
                    MaxSetPrice: $("#txt_DetailMaxPrice").val(),//最高结算价格
                    //IDOSPackageMapping: paclist,

                    //StartDate: $("#txt_DetailStartDate").val(),
                    //EndDate: $("#txt_DetailEndDate").val(),

                    //AppendQty: $("#txt_DetailAppendQty").val(),
                    //AppendUnit: $("#drp_DetailUnit").val(),
                }
                //校验时间
               //if (!utility.isNull(sd) && !utility.isNull(ed) && !utility.isNull(packageDetail.StartDate) && !utility.isNull(packageDetail.EndDate)) {
               //    if (!utility.compareDate(packageDetail.StartDate, packageDetail.EndDate)) {
               //        $.dialog("起始时间不能大于结束时间");
               //        return;
               //    }
               //
               //    if (!utility.compareDate(sd, packageDetail.StartDate) && !utility.compareDate(packageDetail.EndDate, ed)) {
               //        $.dialog("明细的有效时间范围必须在套餐的有效时间范围之内");
               //        return;
               //    }
               //}
                if (pDetailId == '') {//新增
                    var postUrl = "/BaseData/AddPackageDetailData";
                } else {//编辑
                    var postUrl = "/BaseData/UpdatePackageDetailData";
                }


                ajax(postUrl, { model: packageDetail }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        var start = dtPackageDetail.fnSettings()._iDisplayStart;
                        var length = dtPackageDetail.fnSettings()._iDisplayLength;
                        dtPackageDetail.fnPageChange(start / length, true);
                        $.dialog(res.MSG);
                    } else { $.dialog(res.MSG); }
                });
            } else {
                $.dialog('请先选择具体套餐');
            }
        }
    })
    //清空套餐明细弹窗信息
    $("#btnClearDetail").click(function () {
        clearPackageDetailData();
    })

    $("#drpLimitBrand").attr("disabled", true);
    $("#drpLimitStore").attr("disabled", true);
    $("#drp_Limit").change(function () {
        var actLimit = new Array();
        $("#drp_Limit").find("option:selected").each(function (i, data) {
            var value = data.value;
            actLimit.push({ LiType: value });
        });

        if (Enumerable.from(actLimit).where("($.LiType == 'brand')").toArray().length > 0) {
            //限制品牌
            $("#drpLimitBrand").prop("disabled", false).trigger("liszt:updated");;
        } else {
            $("#drpLimitBrand").val('').prop("disabled", true).trigger("liszt:updated");
        }
        if (Enumerable.from(actLimit).where("($.LiType == 'store')").toArray().length > 0) {
            //限制门店
            $("#drpLimitStore").prop("disabled", false).trigger("liszt:updated");;
        } else {
            $("#drpLimitStore").val('').prop("disabled", true).trigger("liszt:updated");
        }
    })

    //多选下拉框
    //$("#drpRolePages").multiSelect({
    //    //cssClass: "height: 300px",
    //    selectableHeader: "<div class='search-header'><input type='text' class='span12' id='txtSearchRolePages' autocomplete='on' placeholder='查找套餐...'></div>",
    //    selectionHeader: "<div class='search-selected'>匹配套餐</div>",
    //    //afterSelect: function (e) {
    //    //    dtElements.fnDraw();
    //    //},
    //    //afterDeselect: function (e) {
    //    //    dtElements.fnDraw();
    //    //}
    //});
    //
    //$("#UpdateRolePageSelectAll").on("click", function () {                            //全选事件
    //    $("#drpRolePages").multiSelect("select_all");
    //    return false;
    //});
    //
    //$("#UpdateRolePageDeSelectAll").on("click", function () {                          //取消全选事件
    //    $("#drpRolePages").multiSelect("deselect_all");
    //    return false;
    //});
    

})
//加载IDOS套餐对应
function GetIDOSPackageList(s) {
    $('#drpRolePages').empty();
    ajax("/BaseData/GetIDOSPackageList", { stores: s }, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].IDOSPackageCode + ',' + res[i].StoreCode + '>' + res[i].PackageDesc + '&nbsp;&nbsp;[' + res[i].StoreName + ']' + '</option>';
            }
            $('#drpRolePages').append(opt).multiSelect("refresh");


        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpRolePages').append(opt);
        }
    });
}

//新增套餐时验证数据
var DataValidatorPackage = $("#frmValidatePackage").validate({
    //onSubmit: false,
    rules: {
        txt_PackageName: {
            required: true,
            maxlength: 50,
        },
        txt_AppendQty: {
            required: true,
            digits: true,
            max: 10,
            min:1,
        },
        drp_Unit: {
            required: true,
        },
        txt_PackageDesc: {
            required: true,
            maxlength: 200,
        },
        txt_Price1: {
            isDecimal: true
        },
        txt_Price2: {
            required: function () {
                if ($('#ClearingType').val() == "2") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true
        },
        txtValue1: {
            required: function () {
                if ($('#ClearingType').val() == "1") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true,
        },
        txtValue2: {
            required: function () {
                if ($('#ClearingType').val() == "1") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//新增套餐明细时验证数据
var DataValidatorPackageDetail = $("#frmValidatePackageDetail").validate({
    //onSubmit: false,
    rules: {
        drp_Item: {
            required: true
        },
        txt_ItemDesc: {
            required: true,
            maxlength: 200,
        },
        txt_DetailQty: {
            required: true,
        },
        txt_DetailMaxPrice: {
            isDecimal: true,
        },
        txt_DetailPrice2: {
            required: function () {
                if ($('#DetailClearingType').val() == "2") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true
        },
        txtDetailValue1: {
            required: function () {
                if ($('#DetailClearingType').val() == "1") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true,
        },
        txtDetailValue1: {
            required: function () {
                if ($('#DetailClearingType').val() == "1") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//编辑套餐信息
function goEditPackage(pId) {
    clearPackageData();//每次弹窗先清空数据
    if (pId == '') {
        $("#table_AddPackage .modal-header h3").html("新增套餐");
    }
    else {
        $("#table_AddPackage .modal-header h3").html("编辑套餐");
    }
    //获取套餐信息
    if (pId != '') {
        ajax("/BaseData/GetPackageById", { packageId: pId }, function (res) {

            $("#txt_PackageName").val(res[0].PackageName);
            $("#txt_PackageDesc").val(res[0].PackageDesc);
            $("#txt_StartDate").val(!res[0].StartDate ? "" : res[0].StartDate.substr(0, 10));
            $("#txt_EndDate").val(!res[0].EndDate ? "" : res[0].EndDate.substr(0, 10));
            $("#txt_AppendQty").val(res[0].AppendQty);
            $("#drp_Unit").val(res[0].AppendUnit);
            $("#txt_Price1").val(res[0].Price1);
            //$("#txt_MaxPrice").val(res[0].MaxSetPrice);
            //$("#drp_PriceShip").val(res[0].PriceRelation);
            $("#chbEnable").prop('checked', res[0].Enable);
            $("#txtPId").val(res[0].PackageID);

            if (res[0].Price2 != null) {
                $('#ClearingType').val('2').change();
                $("#txt_Price2").val(res[0].Price2);
            } else if (res[0].Proportion != null) {
                var pro = res[0].Proportion.split(':');
                $('#ClearingType').val('1').change();
                $("#txtValue1").val(pro[0]);//以：隔开
                $("#txtValue2").val(pro[1]);
            } else {
                $('#ClearingType').val('0').change();
            }

            //使用限制赋值
            var objvalue = new Array();
            var limBrand = new Array();
            var limStore = new Array();
            var j = 0, k = 0, z = 0;
            for (var i in res) {
                objvalue[i] = res[i].LimitType;
                if (res[i].LimitType == "brand") {
                    limBrand[j] = res[i].LimitValue;
                    j++;
                }
                if (res[i].LimitType == "store") {
                    limStore[z] = res[i].LimitValue;
                    z++;
                }
            }
            $("#drp_Limit").val(objvalue);
            $("#drp_Limit").trigger("liszt:updated");
            if (limBrand.length > 0) {
                $("#drpLimitBrand").val(limBrand).prop('disabled', false).trigger("liszt:updated");
            }
            if (limStore.length > 0) {
                $("#drpLimitStore").val(limStore).prop('disabled', false).trigger("liszt:updated");
            }

        });
    } else { $("#txtPId").val(''); clearPackageData(); }
    //弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_AddPackage",
        inline: true
    });
}

//删除套餐信息
function goDeletePackage(pId) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeletePackageById", { packageId: pId }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_package.fnDraw();
                if (dtPackageDetail) {
                    dtPackageDetail.fnDraw();
                }
            } else { $.dialog(res.MSG); }
        });
    })
}
//激活套餐
function goPackageActive(pId) {
    ajax("/BaseData/ActivePackageById", { packageId: pId }, function (res) {
        if (res.IsPass) {
            $.dialog(res.MSG);
            var start = dt_package.fnSettings()._iDisplayStart;
            var length = dt_package.fnSettings()._iDisplayLength;
            dt_package.fnPageChange(start / length, true);
        } else { $.dialog(res.MSG); }
    });
}

//禁用套餐
function goPackageInActive(pId) {
    ajax("/BaseData/InActivePackageById", { packageId: pId }, function (res) {
        if (res.IsPass) {
            $.dialog(res.MSG);
            var start = dt_package.fnSettings()._iDisplayStart;
            var length = dt_package.fnSettings()._iDisplayLength;
            dt_package.fnPageChange(start / length, true);
        } else { $.dialog(res.MSG); }
    });
}
//查看明细
function goPackageDetail(pId,s) {
    $("#txtPackageId").val(pId);
    //重新加载IDOS对应套餐
    var limitstore = s.split(',');
    GetIDOSPackageList(limitstore);
    //ajax("/BaseData/GetPackageById", { packageId: pId }, function (res) {
    //    if (res.length > 0) {
    //        $("#txtStartDate").val(res[0].StartDate.substr(0, 10));
    //        $("#txtEndDate").val(res[0].EndDate.substr(0, 10));
    //    }
    //});

    //加载明细
    if (!dtPackageDetail) {
        dtPackageDetail = $("#dtPackageDetail").dataTable({
            sAjaxSource: '/BaseData/GetPackageDetailList',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                //{ data: "PackageDetailID", title: "套餐明细编号", sortable: false },
                { data: "ItemName", title: "条目名称", sortable: false },
                { data: "ItemDesc", title: "条目描述", sortable: false },
                { data: "Qty", title: "数量", sortable: false },
                //{
                //    data: "StartDate", title: "起始日期", sortable: true, render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                //{
                //    data: "EndDate", title: "到期日期", sortable: true, render: function (obj) {
                //        return !obj ? "" : obj.substr(0, 10);
                //    }
                //},
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        return "<button class=\"btn\" onclick=\"goEditPackageDetail(" + obj.PackageDetailID + ")\">编辑</button> <button class=\"btn\" onclick=\"goDeletePackageDetail(" + obj.PackageDetailID + ")\">删除</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'packageId', value: $("#txtPackageId").val() });
            }
        });
    }
    else {
        dtPackageDetail.fnDraw();
    }

}
//编辑明细信息
function goEditPackageDetail(pDetailId) {
    var pid = $("#txtPackageId").val();
    
    if (pid != '') {
        clearPackageDetailData();//每次弹窗先清空数据
        if (pDetailId == '') {
            $("#table_AddPackageDetail .modal-header h3").html("新增套餐明细");
        }
        else {
            $("#table_AddPackageDetail .modal-header h3").html("编辑套餐明细");
        }
        //获取套餐信息
        if (!utility.isNull(pDetailId)) {
            ajax("/BaseData/GetPackageDetailById", { packageDetailId: pDetailId }, function (res) {

                $("#txtPDetailId").val(res.PackageDetailID);
                $("#txt_DetailMaxPrice").val(res.MaxSetPrice);//价格
                if (res.Proportion != null) {
                    var pro = res.Proportion.split(':');
                    $('#DetailClearingType').val('1').change();
                    $("#txtDetailValue1").val(pro[0]);//以：隔开
                    $("#txtDetailValue2").val(pro[1]);
                } else {
                    $('#DetailClearingType').val('0').change();
                }
                $("#drp_Item").val(res.ItemID);
                $("#txt_ItemDesc").val(res.ItemDesc);
                $("#txt_DetailQty").val(res.Qty);
                $("#txt_DetailStartDate").val(!res.StartDate ? "" : res.StartDate.substr(0, 10));
                $("#txt_DetailEndDate").val(!res.EndDate ? "" : res.EndDate.substr(0, 10));
                $("#txt_DetailAppendQty").val(res.AppendQty);
                $("#drp_DetailUnit").val(res.AppendUnit);
                //$("#drp_Limit").val('').trigger("liszt:updated");;//多选
                if (res.IDOSPackageMapping != null) {
                    var paclist = eval("(" + res.IDOSPackageMapping + ")");
                    var pacstr = [];
                    for (var i = 0; i < paclist.length; i++) {
                        pacstr[i] = paclist[i].IDOSPackageCode + ',' + paclist[i].StoreCOde;
                    }
                    $("#drpRolePages").val(pacstr).multiSelect("refresh");
                } else {
                    $("#drpRolePages").val('').multiSelect("refresh");
                }
                //搜索套餐
                $('#txtSearchRolePages').quicksearch($(".ms-elem-selectable", "#ms-drpRolePages")).on("keydown, input", function (e) {
                    if (e.keyCode == 40) {
                        $(this).trigger("focusout");
                        $("#ms-drpRolePages").focus();
                        return false;
                    }
                    if ($(this).val() == '') {
                        $(this).quicksearch($(".ms-elem-selectable", "#ms-drpRolePages"));
                    }
                });
            });
        } else { $("#txtPDetailId").val(''); clearPackageDetailData(); }

        //搜索套餐
        $('#txtSearchRolePages').quicksearch($(".ms-elem-selectable", "#ms-drpRolePages")).on("keydown, input", function (e) {
            if (e.keyCode == 40) {
                $(this).trigger("focusout");
                $("#ms-drpRolePages").focus();
                return false;
            }
            if ($(this).val() == '') {
                $(this).quicksearch($(".ms-elem-selectable", "#ms-drpRolePages"));
            }
        });
        //弹窗
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            overlayClose: false,
            opacity: '0.3',
            //title: '素材',
            href: "#table_AddPackageDetail",
            inline: true
        });
    } else { $.dialog("请先选择套餐！") }
}

//删除明细信息
function goDeletePackageDetail(pDetailId) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeletePackageDetailById", { packageDetailId: pDetailId }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dtPackageDetail.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//清空套餐弹窗信息
function clearPackageData() {
    $("#txt_PackageName").val('').removeClass('error-block');
    $("#txt_PackageDesc").val('');
    $("#txt_StartDate").val('');
    $("#txt_EndDate").val('');
    $("#txt_AppendQty").val('');
    $("#drp_Unit").val('');
    $("#txt_Price1").val('');
    $("#txt_Price2").val('');
    //$("#txt_MaxPrice").val('');
    //$("#drp_PriceShip").val('or');
    $('#ClearingType option:first').prop('selected', true).change();
    $('#txtInternalPrice').val('');
    $('#txtValue1').val('');
    $('#txtValue2').val('');

    $("#drp_Limit").val('').trigger("liszt:updated");;//多选
    $("#drpLimitVehicle").val('').trigger("liszt:updated");

    $("#drpLimitBrand").val('').trigger("liszt:updated");

    $("#drpLimitStore").val('').trigger("liszt:updated");
    $('.error-block').html('');
}
//清空套餐明细弹窗信息
function clearPackageDetailData() {
    $("#drp_Item").val('');
    $("#txt_ItemDesc").val('').removeClass('error-block');
    $("#txt_DetailQty").val('').removeClass('error-block');
    $("#txt_DetailStartDate").val('');
    $("#txt_DetailEndDate").val('');
    $("#txt_DetailAppendQty").val('');
    $("#drp_DetailUnit").val('');

    $("#drpRolePages").val("").multiSelect("refresh");


}


//加载单位下拉框
function GetUnitList() {

    ajax("/BaseData/GetUnitList", { optType: "DateUnit" }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drp_Unit').append(opt);
            $('#drp_DetailUnit').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_Unit').append(opt);
            $('#drp_DetailUnit').append(opt);
        }
    });
}
//加载条目类型
function GetItemList() {
    //$('#drp_Item').empty();
    ajax("/BaseData/GetPackageItemList", null, function (res) {
        if (res.length > 0) {
            var opt = "<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].BaseDataID + '>' + res[i].ItemName + '</option>';
            }
            $('#drp_Item').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_Item').append(opt);
        }
    });
}

//加载使用限制
function GetLimitList() {
    ajax("/Member360/GetActLimitList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                if (res[i].OptionValue!="vehicle") {
                    opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
                }
            }
            $('#drp_Limit').append(opt);
            $(".chzn_b").trigger("liszt:updated");

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drp_Limit').append(opt);
        }
    });
}
//加载multi-select所有页面
function initPagesMultiSelect() {
    $("#drpRolePages").multiSelect({
        //cssClass: "height: 300px",
        selectableHeader: "<div class='search-header'><input type='text' class='span12' id='txtSearchRolePages' autocomplete='on' placeholder='查找套餐...'></div>",
        selectionHeader: "<div class='search-selected'>匹配套餐</div>",
        afterSelect: function (e) {
            //dtElements.fnDraw();
            //getElements();
        },
        afterDeselect: function (e) {
            //dtElements.fnDraw();
            //getElements();
        }
    });

    $("#UpdateRolePageSelectAll").on("click", function () {                            //全选事件
        $("#drpRolePages").multiSelect("select_all");
        return false;
    });

    $("#UpdateRolePageDeSelectAll").on("click", function () {                          //取消全选事件
        $("#drpRolePages").multiSelect("deselect_all");
        return false;
    });
}

//加载具体品牌使用限制
function GetActLimitBrandList() {
    $('#drpLimitBrand').empty();
    ajax("/Member360/GetActLimitBrandList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].BrandCode + '>' + res[i].BrandName + '</option>';
            }
            $('#drpLimitBrand').append(opt);
            $(".chzn_brand").trigger("liszt:updated");

        } else {
           //var opt = "<option value=''>-无-</option>";
           //$('#drpLimitBrand').append(opt);
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
           //var opt = "<option value=''>-无-</option>";
           //$('#drpLimitStore').append(opt);
        }
    });
}

//计算类型选择
function clearingTypeChg(obj) {
    if ($(obj).val() == "1") {
        $("#div_2").show();
        $("#div_1").hide();
    } else if ($(obj).val() == "2") {
        $("#div_2").hide();
        $("#div_1").show();
    } else {
        $("#div_1").hide();
        $("#div_2").hide();
    }
}
//计算类型选择
function clearingTypeChg1(obj) {
    if ($(obj).val() == "1") {
        $("#div_Detail2").show();
        $("#div_Detail1").hide();
    } else if ($(obj).val() == "2") {
        $("#div_Detail2").hide();
        $("#div_Detail1").show();
    } else {
        $("#div_Detail1").hide();
        $("#div_Detail2").hide();
    }
}

