var tmpId = '',//全局变量
    curParentNode = '',
    datalimitCollections = {};

$(function () {
    //加载模板树图
    loadDataTree('');
    //加载模板类型
    loadTempletCatgory();
    //加载优惠券类型
    loadSubTypeList();
    //加载单位
    loadUnitList();
    //加载Coupon获取渠道
    loadChannelList();
    //加载优惠券使用限制
    loadCouponLimitList();

    //新增优惠券类型的按钮点击事件
    $("#btnAddType").click(function () {
        //resetFormByID("couponTypeDialog");
        showEditDialogMessage("couponTypeDialog", "btnSaveCouponType", null, insertCouponType);
    });

    $("#drpDataLimit").chosen();
    $("#txtCouponStartDate").datepicker();
    $("#txtCouponEndDate").datepicker();

    //新建按钮事件
    $('#btnClear').click(function () {
        tmpId = "";
        resetFormByID("frmSaveCoupon");
        $("#enable").prop("checked", true);
        $('#drpTmpCatg option:first').prop('selected', true);
        $('#drpSubType option:first').prop('selected', true).change();
        initDataLimitDialog("");
        showSelectedDataLimit();

        $("#tree_a li").each(function () {
            $(this).children("span").removeClass("dynatree-active");
        });
    });

    //删除
    $("#btnDelete").click(function () {
        if (!tmpId) {
            $.dialog("请选择要删除的优惠券模板");
            return false;
        }
        $.dialog("确认删除吗?如果该模板已被市场活动使用，请勿删除！", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax('/TmpCommunication/DeleteTmpDataById', { tmpId: tmpId }, function (res) {
                if (res.IsPass) {
                    $.dialog(res.MSG);
                    loadDataTree(curParentNode);
                    $("#btnClear").click();
                } else {
                    $.dialog(res.MSG);
                }
            });
        });
    });

    //显示对话框
    $("#btnShowDataLimitDialog").click(function (e) {
        e.preventDefault();
        showEditDialogMessage("divDataLimitDialog", "btnSaveDataLimit", null, showSelectedDataLimit);
    });

    $("#drpCouponLimit").change(function (e) {
        e.preventDefault();
        initDataLimitDialog("");
        showSelectedDataLimit();
    });

    $("#frmSaveCoupon").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            if ($("#drpSubType").val() == "1") {
                if (parseFloat($("#txtMeetAmount").val()) <= parseFloat($("#txtDeductAmount").val())) {
                    $.dialog("减扣金额不能大于满足金额");
                    return;
                }
            }
            if (!utility.isNull($("#txtCouponStartDate").val()) && !utility.isNull($("#txtCouponEndDate").val())
                && $("#txtCouponStartDate").val() > $("#txtCouponEndDate").val()) {
                $.dialog("起始时间不能大于结束时间");
                return;
            }

            jsonstr = convertToJSONString();

            var chosedLimitInfo = $("#drpCouponLimit").val(),
                couponLimitList = [],
                chosedLimitType;
            if (!utility.isNull(chosedLimitInfo)) {
                chosedLimitType = chosedLimitInfo.split("&&")[7];
                if (utility.isNull($("#hidSelectedDataLimit").val())) {
                    $.dialog("请选择限制条件");
                    return;
                } else {
                    var valueList = $("#hidSelectedDataLimit").val().split(",");
                    for (var index = 0; index < valueList.length; index++) {
                        var tempObject = {
                            "LimitType": chosedLimitType,
                            "LimitValue": valueList[index]
                        }
                        couponLimitList.push(tempObject);
                    }
                }
            } else {
                chosedLimitType = "";
            }


            var templet = {
                TempletID: $("#txtTmpCode").val(),
                Name: encode($("#txtTmpName").val()),
                Topic: encode($("#txtTmpName").val()),
                Category: $("#drpTmpCatg").val(),
                Enable: $("#enable").prop("checked"),
                BasicContent: encode(jsonstr),
                LimitType: chosedLimitType,//$("#drpCouponLimit").val(),//限制类型
                LimitTypeKey: chosedLimitInfo,
                SubType: $('#drpSubType').val(),
                ReferenceNo: encode($("#txtReferenceNo").val()),
                Remark: encode($("#txtRemark").val())
            };
            ajax('/TmpCommunication/AddOrUpdateTmpCouponData', { templet: templet, couponLimit: JSON.stringify(couponLimitList) }, function (res) {
                if (res.IsPass) {
                    $.dialog(res.MSG);
                    loadDataTree(res.Obj[0].toString());
                    $("#txtTmpCode").val(res.Obj[0].toString())
                } else {
                    $.dialog(res.MSG);
                }
            });
        }
    });
    //点击搜索的时候重新加载树图
    $('#tmpCouponSearch').submit(function (e) {
        e.preventDefault();
        loadDataTree('');
    });
});
//验证数据
var DataValidator = $("#frmSaveCoupon").validate({
    rules: {
        txtTmpName: {
            required: true,
            maxlength: 50,
        },
        BasicContent: {
            required: true,
        },
        txtOffNumber: {
            min: 1,
            number: true,
        },
        drpUnit: {
            required: function () {
                if ($('#txtOffNumber').val() != "") {
                    return true;
                } else {
                    return false;
                }
            },
        },
        txtAmount: {
            required: function () {
                if ($('#drpSubType').val() == "1") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true,
            min: 1
        },
        txtMeetAmount: {
            required: function () {
                if ($('#drpSubType').val() == "3") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true,
            min: 1
        },
        txtDeductAmount: {
            required: function () {
                if ($('#drpSubType').val() == "3") {
                    return true;
                } else {
                    return false;
                }
            },
            isDecimal: true,
            min: 1
        },
        txtDiscount: {
            required: function () {
                if ($('#drpSubType').val() == "2") {
                    return true;
                } else {
                    return false;
                }
            },
            isFloat: true,
            max: 0.99,
            min: 0,
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//加载模板树图
function loadDataTree(snode) {
    var d = {
        type: "Coupon",
        key: encode($("#key").val().toLowerCase())
    }, a = '/TmpCommunication/GetTmpDataList';

    ajax(a, d, function (data) {
        $('.dynatree').dynatree({
            children: data,
            onClick: function (node) {
                if (node.parent.data.children) {
                    TreeItemClick(node);
                }
            },
            onActivate: function (node) {
                if (!node.data.isFolder) {
                    curParentNode = node.parent.data.key;
                }
            },
            onPostInit: function (isReloading, isError, dtNode) {
                this.tnRoot.visit(function (dtNode) {
                    if (!utility.isNull(snode) && dtNode.data.key == snode) {
                        if (dtNode.data.isFolder) {
                            dtNode.expand(true);
                        } else {
                            dtNode.focus(true);
                            dtNode.activate(true);
                        }
                    }
                });
            }
        }).dynatree("getTree").reload();
    });
}

//点击树图节点事件
function TreeItemClick(node) {
    $('#btnClear').click();
    var Id = node.data.key.trim();
    if (Id != 0) {
        tmpId = Id;
        var limitTypeKey = "";
        ajax('/TmpCommunication/GetTmpDataById', { "tmpId": tmpId }, function (res) {
            if (res != null) {
                limitTypeKey = res.LimitTypeKey;
                $("#txtTmpCode").val(res.TempletID);
                $("#txtTmpName").val(res.Name);
                $("#txtTopic").val(res.Topic);
                $("#drpTmpCatg").val(res.Category);
                $("#enable").prop("checked", res.Enable); 
                $("#txtReferenceNo").val(res.ReferenceNo);
                $("#txtRemark").val(res.Remark);

                //加载数据
                data = $.parseJSON(res.BasicContent);
                if (!utility.isNull(data.offNumber) && data.offNumber.length > 0) {
                    $('#txtOffNumber').val(data.offNumber);
                } else {
                    $('#txtOffNumber').val('');
                }
                if (!utility.isNull(data.unit) && data.unit.length > 0) {
                    $('#drpUnit').val(data.unit);
                } else {
                    $('#drpUnit').val('');
                }

                if (res.SubType == "1") {
                    $("#drpSubType").val("1");
                    changeCouponTempletCategory("#drpSubType");
                    $("#txtAmount").val(data.price);

                } else if (res.SubType == "2") {
                    $("#drpSubType").val("2");
                    changeCouponTempletCategory("#drpSubType");
                    $("#txtDiscount").val(data.discount);
                    var objvalue = new Array();
                    for (var i in data.category) {
                        objvalue[i] = data.category[i].value;
                    }
                } else if (res.SubType == "3") {
                    $("#drpSubType").val("3");
                    changeCouponTempletCategory("#drpSubType");
                    $("#txtMeetAmount").val(data.reach);
                    $("#txtDeductAmount").val(data.reduce);
                }

                $("#txtCouponStartDate").val(data.startdate);
                $("#txtCouponEndDate").val(data.enddate);
                $("#ispublic").prop("checked", data.ispublic);
                $("#isOthers").prop("checked", data.isOthers);
                $("#txtMaxAvliableAmount").val(data.maxAvaiCount);
                $("#drpChannel").val(data.getChannel);
            }
            ajaxSync('/TmpCommunication/GetTmpCouponLimit', { "tmpId": tmpId }, function (data) {
                if (data.length > 0) {
                    var limit = new Array();
                    $("#drpCouponLimit").val(limitTypeKey);

                    for (var i = 0; i < data.length; i++) {
                        limit[i] = data[i].LimitValue;
                    }
                    initDataLimitDialog(limit);
                    showSelectedDataLimit();
                }
            });
        });
    }
}

//加载模板类型
function loadTempletCatgory() {
    ajax('/TmpCommunication/GetTmpCatg', null, function (res) {
        $('#drpTmpCatg').empty();
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpTmpCatg').append(opt);
        } else {
            var opt = "<option value='1'>-无-</option>";
            $('#drpTmpCatg').append(opt);
        }
        var objHtml = "";
        ajax("/BaseData/GetSysClass", { "classID": null, "className": "", "classType": 2 }, function (data) {
            if (!utility.isNull(data)) {
                for (var index = 0; index < data.length; index++) {
                    objHtml += "<option value='" + data[index].ClassID + "'>" + data[index].ClassName + "</option>";
                }
                $("#drpTmpCatg").append(objHtml);
                $("#drpTmpCatg").trigger("liszt:updated");
            }
        });
    });
}

//加载优惠券类型
function loadSubTypeList() {
    $('#drpSubType').empty();
    $.post("/TmpCommunication/GetSubTypeList", null, function (res) {
        if (res.length > 0) {
            var opt = "";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpSubType').append(opt);
            changeCouponTempletCategory("#drpSubType");

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpSubType').append(opt);
        }
    });
}

//优惠券模板管理类型切换
function changeCouponTempletCategory(obj) {
    $("#div1").hide();
    $("#div2").hide();
    $("#div3").hide();
    $("#div4").hide();

    if ($(obj).val() == "1") {//1：现金券
        $("#div1").show();
    } else if ($(obj).val() == "2") {//2：折扣券     
        $("#div2").show();
    } else if ($(obj).val() == "3") {//3：满减券
        //$("#div3").show();
        //$("#div4").show();
        //$("#div1").show();
    }
}

//把数据转化为json格式
function convertToJSONString() {
    var obj = {},
        jsonstr = JSON;

    if ($('#drpSubType').val() == "1") {
        obj.price = $("#txtAmount").val();
    } else if ($('#drpSubType').val() == "2") {
        obj.discount = $("#txtDiscount").val();
    } else if ($('#drpSubType').val() == "3") {
        obj.reach = $("#txtMeetAmount").val();
        obj.reduce = $("#txtDeductAmount").val();
    }

    obj.ispublic = $("#ispublic").prop("checked");
    obj.isOthers = $("#isOthers").prop("checked");
    obj.offNumber = $('#txtOffNumber').val();
    obj.unit = $('#drpUnit').val();
    obj.maxAvaiCount = $("#txtMaxAvliableAmount").val();
    obj.getChannel = $("#drpChannel").val();
    obj.startdate = $("#txtCouponStartDate").val();
    obj.enddate = $("#txtCouponEndDate").val();
    jsonstr = JSON.stringify(obj);
    return jsonstr;
}

//加载Coupon券获取渠道
function loadChannelList() {
    ajax("/BaseData/GetOptionDataList", { optType: "CouponChannel" }, function (res) {
        var opt = "<option value=''>无</option>";
        if (res.length > 0) {
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
        }
        $("#drpChannel").append(opt);
    });
}

//加载单位下拉框
function loadUnitList() {
    ajax("/BaseData/GetUnitList", { optType: "DateUnit" }, function (res) {
        var opt = "";
        if (res.length > 0) {
            opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
        } else {
            opt = "<option value=''>-无-</option>";
        }
        $("#drpUnit").append(opt);
    });
}

//加载使用限制
function loadCouponLimitList() {
    ajax("/Loyalty/GetCouponTempletFieldAliaListByAliasKey", { "key": "store,typea,typeb" }, function (result) {
        var listStr = [],
            opt = "<option value=''>无限制</option>";
        if (result.length > 0) {
            for (var i = 0; i < result.length; i++) {
                opt += "<option value=" + result[i].FieldAlias + "&&" + result[i].DictTableName + "&&" + result[i].DictTableType + "&&"
                    + result[i].ControlType + "&&" + result[i].Reg + "&&" + result[i].FieldType + "&&" + result[i].AliasID + "&&" + result[i].DataLimitType + ">"
                    + result[i].FieldDesc + "</option>";
                listStr.push({ "FieldAlias": result[i].FieldAlias, "TableName": result[i].DictTableName, "TableType": result[i].DictTableType });
            }
        }
        $("#drpCouponLimit").append(opt);
        ajax("/MemSubdivision/GetMemSubdRightValues", { "rightValCfgs": JSON.stringify(listStr) }, function (data) {
            datalimitCollections = eval("(" + data.Data + ")");
        });
    });
}

//初始化数据限制弹框页面
function initDataLimitDialog(chosedDataLimitValue) {
    var selectedKey = $("#drpCouponLimit").val();

    $("#btnShowDataLimitDialog").removeAttr("disabled");
    if (utility.isNull(selectedKey)) {
        $("#btnShowDataLimitDialog").attr("disabled", "disabled");
        $("#txtSelectedDataLimit").val("");
        $("#hidSelectedDataLimit").val("");
        return;
    }

    var fieldAlias = selectedKey.split("&&")[0],
        tableName = selectedKey.split("&&")[1],
        tableKeyPair = selectedKey.split("&&")[2],
        controlType = selectedKey.split("&&")[3],
        reg = selectedKey.split("&&")[4],
        tableKey = utility.isNull(tableKeyPair) ? "" : tableKeyPair.split(",")[0],
        tableValue = utility.isNull(tableKeyPair) ? "" : tableKeyPair.split(",")[1];

    if (utility.isNull(controlType)) {
        return;
    }

    $("#divSelectDataLimit,#divInputDataLimit,#divInputDateDataLimit").css("display", "none");

    if (controlType == "select") {
        $("#divSelectDataLimit").css("display", "block");
        var optionHtml = "";
        if (!utility.isNull(datalimitCollections)) {
            var list = datalimitCollections[fieldAlias];
            if (!utility.isNull(list)) {
                for (var index = 0; index < list.length; index++) {
                    var currentObject = list[index];
                    optionHtml += "<option value='" + currentObject.sv + "'>" + currentObject.st + "</option>";
                }

                $("#drpDataLimit").html(optionHtml);
                $("#drpDataLimit").val(chosedDataLimitValue);
                $("#drpDataLimit").trigger("liszt:updated");
                return;
            }
        }
        $("#drpDataLimit").html(optionHtml);
        $("#drpDataLimit").val(chosedDataLimitValue);
        $("#drpDataLimit").trigger("liszt:updated");

    } else if (controlType == "input") {
        $("#divInputDataLimit").css("display", "block");
        $("#divInputDataLimit").val(chosedDataLimitValue);
    } else if (controlType == "date" || controlType == "datetime") {
        $("#divInputDateDataLimit").css("display", "block");
        $("#divInputDateDataLimit").val(chosedDataLimitValue);
    } else {
        doNothing();
    }
}

//选择数据权限展示
function showSelectedDataLimit() {
    var selectedKey = $("#drpCouponLimit").val();
    if (utility.isNull(selectedKey)) {
        $("#txtSelectedDataLimit").val("");
        $("#hidSelectedDataLimit").val("");
        return;
    }

    var controlType = selectedKey.split("&&")[3];
    if (utility.isNull(controlType)) {
        $("#txtSelectedDataLimit").val("");
        $("#hidSelectedDataLimit").val("");
        return;
    }

    if (controlType == "select") {
        $("#hidSelectedDataLimit").val($("#drpDataLimit").val());
        var selectedOtionText = "",
            optionList = $("#drpDataLimit").find("option:selected");
        for (var i = 0; i < optionList.length; i++) {
            var currentObject = optionList[i];
            selectedOtionText += currentObject.innerText + ",";
        }
        $("#txtSelectedDataLimit").val(selectedOtionText.substr(0, selectedOtionText.length - 1));
    } else if (controlType == "input") {
        $("#txtSelectedDataLimit").val($("#txtDataLimit").val());
        $("#hidSelectedDataLimit").val($("#txtDataLimit").val());
    } else if (controlType == "date" || controlType == "datetime") {
        $("#txtSelectedDataLimit").val($("#txtDataLimitDate").val());
        $("#hidSelectedDataLimit").val($("#txtDataLimit").val());
    } else {
        $("#txtSelectedDataLimit").val("");
        $("#hidSelectedDataLimit").val("");
    }
}


function resetFormByID(id) {
    $("#" + id + " :input").not(":button, :submit, :reset").val("").removeAttr("checked").removeAttr("selected");
    $("#" + id + " .error").removeClass('error');
    $("#" + id + " .help-block").html('')
}

function insertCouponType() {
    var postUrl = "/BaseData/InsertSysClass",
        classInfo = {
            ClassName: $("#txtAddCouponType").val(),
            ClassType: "2",
            Sort: 1
        };

    if (utility.isNull($("#txtAddCouponType").val())) {
        $.dialog("请填写细分类型名称");
        return;
    }

    ajax(postUrl, { "classInfo": JSON.stringify(classInfo) }, function (result) {
        if (utility.isNull(result)) {
            $.dialog("新增失败");
            return;
        } else if (!result.IsPass) {
            $.dialog(result.MSG);
            return;
        } else {
            loadTempletCatgory();
            loadDataTree();
        }
    });
}

