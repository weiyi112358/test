var tableMemberInfo, dtPackage, dtPackageDetail_P, dtAccountCouponList;
$(function () {
    //日期控件初始化
    $("#txtEditStartDate").datepicker({ startDate: '0' });
    $("#txtEditEndDate").datepicker({ startDate: '0' });
    //loadActLimitList();
    //GetActLimitBrandList();
    //GetActLimitStoreList();
    //$(".chzn_b").chosen();

    //$(".chzn_brand").chosen();

    //$(".chzn_store").chosen();

    //搜索会员
    $("#btnSearch").click(function () {
        //至少有一个条件才能搜索
        if ($("#txtCardNo").val() == '' && $("#txtNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '') {
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
        //$.colorbox.resize();
    })
    //新建账户
    $("#btnAddAct").click(function () {
        $("#divRemark").hide();
        var mid = $("#hdnMemberId").val();
        if (mid != '') {
            clearDisabled();
            $("#table_EditAccount h3").html('新增账户信息');
            $("#actlblname").html('增加积分');


            $("#txtEditAct").val('').prop('disabled', false);
            $("#drpEditActType").val('value1').prop('disabled', false).change();
            $("#txtEditActNumber").val('').prop('disabled', true);
            $("#txtEditStartDate").val('');
            $("#txtEditEndDate").val('');
            $("#txtActDetailId").val('');

            $("#drpEditActOpt").val('add');
            $("#drpEditActOpt").prop('disabled', true);
            //$("#drpActLimit").val('').trigger("liszt:updated");

            //$("#drpLimitBrand").val('').trigger("liszt:updated");
            //$("#drpLimitStore").val('').trigger("liszt:updated");
            $("#txtChangeReason").val('').prop('disabled', true);

            $.colorbox({
                initialHeight: '0',
                initialWidth: '0',
                href: "#table_EditAccount",
                inline: true,
                opacity: '0.3',
                onComplete: function () {
                    $.colorbox.resize();
                }
            });
        } else {
            $.dialog('请先查询并选择会员！');
        }
    })
    //保存调整信息
    $("#btnSaveImproveAct").click(function () {
        $("#btnSaveImproveAct").attr('disabled', 'disabled');
        var mid = $("#hdnMemberId").val();
        var did = $("#txtActDetailId").val();
        var type = $("#drpActType").val();
        var actId = $("#txtActId").val();
        var actType = $("input[name='reg_det']:checked").val();
        var oper = $("#drpActOpt").val();
        var num = $("#txtActNumber").val();
        var total = $("#txtActPoint").val();
        if (num == '') {
            $.dialog('调整数值不能为空！');
            return;
        }
        if (oper == "sub" && parseInt(total) < parseInt(num)) {
            $.dialog('扣减积分数值不能超过积分余额！');
            return;
        }
        ajax("/Member360/SaveImproveActInfo", { mid: mid, detailType: type, oper: oper, num: num, did: did, actId: actId, actType: actType }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                $.colorbox.close();
                loadAccountList($("input[name='reg_det']:checked").val());
                loadCashPoint($("#hdnMemberId").val());
            } else { $.dialog(res.MSG); }
            $("#btnSaveImproveAct").attr('disabled', false);
        })
    })
    $("input[name='reg_det']").click(function () {
        loadAccountList($(this).val());
    })
    //$("#divSDate").remove();
    //帐户类型改变事件
    $("#drpEditActType").change(function () {
        var type = $("#drpEditActType").val();
        if (type == "value1") {
            $("#divSDate").hide();
            $("#divEDate").val('');
        } else {
            $("#divSDate").show();
            $("#divEDate").val('');
            //.before('<div class="span3" id="divSDate"><label id="labDate">解冻日期</label><input type="text" id="txtEditStartDate" class="span11" name="txtEditStartDate" /><span class="error-block"></span></div>');
            //$("#txtEditStartDate").datepicker({ startDate: '0' });
        }
    })


    //导入按钮
    initImportUpload();
    $("#btnImport").bind("click", function () {
        $("#tbFilePath").val("");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#import_data",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
            onComplete: function () {
                $("#btnSaveImport").bind("click", function () {
                    ctrlUpload.startUpload();

                    
                });
            }
        });
    });

    //模板下载
    $("#btnDownLoad").bind("click", function () {
        window.location = '/Upload/积分调整批量导入模板.xls';
    });







    //保存修改信息
    $("#frmEditAccount").submit(function (e) {
        e.preventDefault();
        if (DataValidatorAct.form()) {
            $("#btnSaveEditAct").attr('disabled', 'disabled');
            //$("#btnSaveEditAct").click(function () {
            var id = $("#txtActDetailId").val();
            var actId = $("#txtActId").val();
            var oper = $("#drpEditActOpt").val();
            var changValue = $("#txtEditActNumber").val();
            var num = $("#txtEditAct").val();
            var type = $("#drpEditActType").val();
            var startDate = $("#txtEditStartDate").val() == undefined ? "" : $("#txtEditStartDate").val();
            var endDate = $("#txtEditEndDate").val();

            var actLimit = new Array();
            var actType = $("input[name='reg_det']:checked").val();//新增账户类型
            var mid = $("#hdnMemberId").val();//会员id

            //----------------------------------限制start
            //$("#drpActLimit").find("option:selected").each(function (i, data) {
            //    //var label = $(this).parent("optgroup").attr("label");
            //    var value = data.value;
            //    actLimit.push({ LiType: value });
            //});
            //var limitList = new Array();

            //if (Enumerable.from(actLimit).where("($.LiType == 'brand')").toArray().length > 0) {
            //    //限制品牌
            //    $("#drpLimitBrand").find("option:selected").each(function (i, data) {
            //        //var label = $(this).parent("optgroup").attr("label");
            //        var value = data.value;
            //        limitList.push({ LimitType: "brand", LimitValue: value });
            //    });
            //    if (Enumerable.from(limitList).where("($.LimitType == 'brand')").toArray().length <= 0) {
            //        $.dialog("请选择限制品牌");
            //        return;
            //    }
            //}
            //if (Enumerable.from(actLimit).where("($.LiType == 'store')").toArray().length > 0) {
            //    //限制门店
            //    $("#drpLimitStore").find("option:selected").each(function (i, data) {
            //        //var label = $(this).parent("optgroup").attr("label");
            //        var value = data.value;
            //        limitList.push({ LimitType: "store", LimitValue: value });
            //    });
            //    if (Enumerable.from(limitList).where("($.LimitType == 'store')").toArray().length <= 0) {
            //        $.dialog("请选择限制门店");
            //        return;
            //    }
            //}
            //----------------------------------限制end

            var reason = encode($("#txtChangeReason").val());

            $("#btnSaveEditAct").attr('disabled', false);
            if (mid == '') {
                $.dialog("请先选择会员，然后再添加账户")
                return;
            }
            if (type == 'value2' && endDate != '') {
                if (!utility.compareDate(startDate, endDate)) {
                    $.dialog("起始时间不能大于结束时间");
                    return;
                }
            }

            var reg = /^[1-9]\d*$/;
            if (changValue != "" && reg.test(changValue) == false) {
                $.dialog('调整数值必须为正整数！');
                return;
            }
            
            //if (parseInt(changValue) <= 0) {
            //    $.dialog("调整值必须大于0");
            //    return;
            //}
            if (id != '') {//修改
                if (oper == "sub" && parseInt(num) < parseInt(changValue)) {
                    $.dialog('扣减积分数值不能超过积分余额！');
                    return;
                }

                ajax("/Member360/SaveEditActInfoById", { did: id, detailType: type, num: changValue, startDate: startDate, endDate: endDate, oper: oper, reason: reason }, function (res) {

                    if (res.IsPass) {
                        $.dialog(res.MSG);
                        $.colorbox.close();
                        loadAccountList($("input[name='reg_det']:checked").val());
                        loadCashPoint($("#hdnMemberId").val());
                    } else { $.dialog(res.MSG); }

                })
            } else {//新增

                if (num != "" && reg.test(num) == false) {
                    $.dialog('增加积分必须为正整数！');
                    return;
                }
                if (parseInt(num) <= 0) {
                    $.dialog("新建账户值必须大于0");
                    return;
                }
                ajax("/Member360/SaveAddActDetailInfo", { actId: actId, detailType: type, num: num, startDate: startDate, endDate: endDate, actType: actType, mid: mid }, function (res) {

                    if (res.IsPass) {
                        $.dialog(res.MSG);
                        $.colorbox.close();
                        loadAccountList($("input[name='reg_det']:checked").val());
                        loadCashPoint($("#hdnMemberId").val());
                    } else {
                        $.dialog(res.MSG);
                        $.colorbox.close();
                    }

                })
            }
        }
    })
    //$("#drpLimitBrand").attr("disabled", true);
    //$("#drpLimitStore").attr("disabled", true);
    //$("#drpActLimit").change(function () {
    //    var actLimit = new Array();
    //    $("#drpActLimit").find("option:selected").each(function (i, data) {
    //        var value = data.value;
    //        actLimit.push({ LiType: value });
    //    });


    //    if (Enumerable.from(actLimit).where("($.LiType == 'brand')").toArray().length > 0) {
    //        //限制品牌
    //        $("#drpLimitBrand").prop("disabled", false).trigger("liszt:updated");;
    //    } else {
    //        $("#drpLimitBrand").val('').prop("disabled", true).trigger("liszt:updated");
    //    }
    //    if (Enumerable.from(actLimit).where("($.LiType == 'store')").toArray().length > 0) {
    //        //限制门店
    //        $("#drpLimitStore").prop("disabled", false).trigger("liszt:updated");;
    //    } else {
    //        $("#drpLimitStore").val('').prop("disabled", true).trigger("liszt:updated");
    //    }
    //})
})

//新增时验证数据
var DataValidatorAct = $("#frmEditAccount").validate({
    //onSubmit: false,
    rules: {
        txtEditAct: {
            required: true,
        },
        txtEditActNumber: {
            required: function () {
                if ($('#txtActDetailId').val() == "") {
                    return false;
                } else {
                    return true;
                }
            },
        },
        txtChangeReason: {
            required: true,
            maxlength: 100,
        },
        txtEditStartDate: {
            required: function () {
                if ($('#drpEditActType').val() == "value2") {
                    return true;
                } else {
                    return false;
                }
            },
        }
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
                //d.push({ name: 'cardNo', value: encode($("#txtCardNo").val()) });
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

//查看会员详细信息
function detail(id) {
    $.colorbox.close();
    $(".memInfoBlock").show();
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            $("#txtActId").val('');
            var mid = res.Obj[0].MemberID;
            memberId = mid;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);
            $("#hdnMemberId").val(mid);
            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            //$("#spnCardStat").text(res.Obj[0].StoreName == null ? "" : res.Obj[0].StoreName);
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);

            //$("#radioCash").prop("checked", true);
            loadAccountList($("input[name='reg_det']:checked").val());
        }
    })
    //加载账户积分现金信息
    loadCashPoint(id);
}
//加载账户积分现金信息
function loadCashPoint(id) {
    ajax("/Member360/GetMemAccountInfo", { mid: id }, function (data) {
        $(".stgValidValue").text(0);
        if (data.length > 0) {
            for (var i in data) {
                $("#stgValidValue_" + data[i].AccountType).text(data[i].ValidValue);
            }
            $("#stgValidValue33").text(Math.floor(data[0].FrozenValue));
            $("#stgValidValue88").text(Math.floor(data[0].TotalValue));
        }
        if (parseInt($("#stgValidValue_3").text()) == 0) {
            $("#btnEditAct").attr('disabled', 'disabled');
        } else {
            $("#btnEditAct").attr('disabled', false);
        }
    });

}


//加载会员账户列表
function loadAccountList(type) {
    //$('#txtActType').val(type);
    ajax("/Member360/GetMemActDetails1", { mid: $("#hdnMemberId").val(), accType: type }, function (res) {//  '0010024595984EB291D851C44EF091C6'
        var tbody = $("#dtAccountDetail tbody");
        $('#txtActId').val('');
        tbody.empty();
        var data1 = res.Obj;
        if (data1 == null) {
            tbody.append('<tr class="odd"><td class="dataTables_empty" valign="top" colspan="6">没有记录</td></tr>');
        }
        else {
            var data = data1[0];
            if (data.length > 0) {
                //var accountId = "", type = "", limittd = "", typetd = "", valuetd = "", limitCount = 0, typeCount = 0;
                $('#txtActId').val(data[0].AccountID);
                for (var i in data) {

                    var tr = $('<tr></tr>');
                    var sDate = !data[i].SpecialDate1 ? "" : data[i].SpecialDate1.substr(0, 10);
                    var eDate = !data[i].SpecialDate2 ? "" : data[i].SpecialDate2.substr(0, 10);
                    //tr.append('<td>' + data[i].AccountLimit + '</td>')
                    tr.append('<td>' + data[i].DetailTypeText + '</td>')
                    .append('<td>' + data[i].DetailValue + '</td>')
                    .append('<td>' + sDate + '</td>')
                    .append('<td>' + eDate + '</td>')
                    //.append('<td>' + data[i].AccountChangeType + '</td>')
                    //.append('<td>' + data[i].ChangeReason + '</td>')
                    .append('<td><button class="btn" onclick="editAccountDetail(\''+ data[i].AccountDetailID +'\')">编辑</button></td>').appendTo(tbody);

                    //}
                }
            }
        }
    });
}
//编辑账户数据
function editAccountDetail(detailId) {
    clearData();
    clearDisabled();
    $("#divRemark").show();
    $("#drpEditActType").prop('disabled', true);

    $("#table_EditAccount h3").html('编辑账户信息');
    $("#actlblname").html('可调整积分');
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#table_EditAccount",
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $.colorbox.resize();
        }
    });
    ajax("/Member360/GetActDetailById", { detailId: detailId }, function (res) {

        $("#txtEditAct").val(res[0].DetailValue).prop('disabled', true);
        $("#drpEditActType").val(res[0].AccountDetailType).change();
        $("#txtEditStartDate").val(res[0].SpecialDate1 == null ? "" : res[0].SpecialDate1.substr(0, 10));
        $("#txtEditEndDate").val(res[0].SpecialDate2 == null ? "" : res[0].SpecialDate2.substr(0, 10));
        $("#txtActDetailId").val(detailId);

        //使用限制赋值
        //    var objvalue = new Array();
        //    var limVeh = new Array();
        //    var limBrand = new Array();
        //    var limStore = new Array();
        //    var j = 0, k = 0, z = 0;
        //    for (var i in res) {
        //        objvalue[i] = res[i].LimitType;
        //        if (res[i].LimitType == "brand") {
        //            limBrand[j] = res[i].LimitValue;
        //            j++;
        //        }
        //        if (res[i].LimitType == "vehicle") {
        //            limVeh[k] = res[i].LimitValue;
        //            k++;
        //        }
        //        if (res[i].LimitType == "store") {
        //            limStore[z] = res[i].LimitValue;
        //            z++;
        //        }
        //    }
        //    $("#drpActLimit").val(objvalue);
        //    $("#drpActLimit").trigger("liszt:updated");
        //    if (limBrand.length > 0) {
        //        $("#drpLimitBrand").val(limBrand).prop('disabled', false).trigger("liszt:updated");
        //    }
        //    if (limStore.length > 0) {
        //        $("#drpLimitStore").val(limStore).prop('disabled', false).trigger("liszt:updated");
        //    }
    })
}
//调整账户弹窗
function improveAccountDetail() {
    $("#txtActNumber").val('');
    $("#drpActOpt").val('add');
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#table_AddAccount",
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $.colorbox.resize();
        }
    });
    $("#txtActPoint").val($("#stgValidValue_3").text());
}
//加载账户使用限制
function loadActLimitList() {
    $('#drpActLimit').empty();
    ajax("/Member360/GetActLimitList", null, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpActLimit').append(opt);
            $(".chzn_b").trigger("liszt:updated");

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpActLimit').append(opt);
        }
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
function clearDisabled() {
    $("#txtEditAct").prop('disabled', false);
    $("#drpEditActType").prop('disabled', false);
    $("#txtEditActNumber").prop('disabled', false);
    $("#drpEditActOpt").prop('disabled', false);
    $("#txtChangeReason").prop('disabled', false);
}
function clearData() {
    $("#table_EditAccount h3").html('编辑账户信息');

    $("#txtEditAct").val('').prop('disabled', false);
    $("#drpEditActType").val('value1').prop('disabled', false);
    $("#txtEditActNumber").val('').prop('disabled', true);
    $("#txtEditStartDate").val('');
    $("#txtEditEndDate").val('');
    $("#txtActDetailId").val('');
    $("#drpEditActOpt").val('add');
    $("#drpEditActOpt").prop('disabled', true);
    //$("#drpActLimit").val('').trigger("liszt:updated");


    //$("#drpLimitBrand").val('').trigger("liszt:updated");

    //$("#drpLimitStore").val('').trigger("liszt:updated");
    $("#txtChangeReason").val('').prop('disabled', true);
}

//上传
function initImportUpload() {
    ctrlUpload.initUpload("/Member360/BatchImportPoint", "", uploadBack);
}


//EXCEL上传回调
function uploadBack(data) {
    
    showLoading("正在导入中....");   
    $.colorbox.close();
    $("#tbFilePath").val("");

    

    if (data.response != "") {
        var result = data.response.substring(data.response.indexOf('[') + 1, data.response.indexOf(']'));
        var successNum = result.split(',')[0];
        var failNum = result.split(',')[1];
        $.dialog('导入完成,成功' + successNum + '条,失败' + failNum + '条。');
    }
    else {
        $.dialog('导出出错');
    }

    
    hideLoading();
}

//关闭遮罩层
function hideLoading() {
    $.closePopupLayer('processing');
    //$("#processingdiv").hide();
}

//开启遮罩层
function showLoading(desc) {

    //$("body").append("<div id=\"processingdiv\" style=\"display:none;\"><div class=\"popup\"> <div class=\"popup-body\"><div class=\"loading\"><span>" + desc + "</span></div></div></div></div>");
    $("#txtspan").html(desc);
    $("#txtspan").css("color", "#ffffff");
    //alert($("head").html());  

    $.openPopupLayer({
        name: "processing",
        width: 500,
        target: "processingdiv"
    });
}