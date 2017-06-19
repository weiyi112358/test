var dt_Table;
var dtCoupon;
var dtProduct;
var productArr = "";
var codeStr = "";


$(function () {
    $.ajaxSetup({
        async: false
    });
    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/CouponSend/LoadStoreList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aoColumns: [
            { data: 'StoreCode', sClass: "center", title: "门店代码", sortable: false },
            { data: 'StoreName', sClass: "center", title: "门店名称", sortable: false },
            {
                data: null, sClass: "center", title: "派送数量", sortable: false, render: function (obj) {
                    var result = "";
                    if (obj.StoreCode != null && codeStr != "") {
                        var str = codeStr.substring(codeStr.indexOf(obj.StoreCode));
                        result += str.substring(str.indexOf(':') + 1, str.indexOf(','));
                    }
                    return "<lable id='ic_" + obj.StoreCode + "' name='indexCount'>" + result + "</lable>";
                }

            },
            {
                data: null, sClass: "center", title: "派送金额", sortable: false, render: function (obj) {
                    if (obj.StoreCode != null && codeStr != "" && $("#CouponValue").val() != null) {
                        var str = codeStr.substring(codeStr.indexOf(obj.StoreCode));
                        var count = str.substring(str.indexOf(':') + 1, str.indexOf(','));
                        return "<lable id='im_" + obj.StoreCode + "' name='indexMoney'>" + parseInt(count) * parseInt($("#CouponValue").val() == "" ? 0 : $("#CouponValue").val()) + "</lable>";
                    }
                    return "<lable id='im_" + obj.StoreCode + "' name='indexMoney'>0</lable>";
                }
            },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    var result = "";
                    var info = $("#txtInfo").val();
                    if (info != 2) {
                        result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.StoreCode + "')\" >删除</button>";
                    }
                    return result;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'storeArr', value: productArr });
        }
    });
    var id = $("#txtID").val();
    var info = $("#txtInfo").val();
    if (id == "") {
        $("#hTitleName").html("添加购物券派送");
    }
    else {
        if (info == "1") {
            $("#hTitleName").html("编辑购物券派送");
        } else {
            $("#hTitleName").html("查看购物券派送");
            $("#ASendCount").prop("readonly", "readonly");
            $("#btnSave").css("display", "none");
            $("#btnAdd").css("display", "none");
            $("#btnPass").css("display", "none");
            $("#aCouponNo").css("display", "none");
            $("#btnDownLoad").css("display", "none");
            $("#btnImport").css("display", "none");
            $("#btnAvgCount").css("display", "none");
        }

        ajax("/CouponSend/GetCouponSendById", { ID: id }, function (data) {
            $("#CouponNo").val(data.CouponId);
            $("#CouponName").val(data.CouponInfo);
            $("#CouponValue").val(data.CouponValue);
            $("#StartDate").val(data.BeginDate == null ? "" : data.BeginDate.substr(0, 10));
            $("#EndDate").val(data.EndDate == null ? "" : data.EndDate.substr(0, 10));
            $("#CouponRemark").val(data.CouponDesc);
            $("#SendedCount").val(data.SendedCount == null ? 0 : data.SendedCount);
            $("#ASendCount").val(data.SendCount);
            productArr = data.LimitValue;
            codeStr = data.codeStr;
            dt_Table.fnDraw();
        });
    }
    //均分券按钮
    $("#btnAvgCount").bind("click", function () {
        var aCount = $("#ASendCount").val();
        if (aCount == "") {
            $.dialog("请输入总派送数量");
            return;
        }
        var pasc = 0;
        var useC = 0;
        $("lable[name=indexCount]").each(function (i, e) {
            if ($(e).text() == "0") {
                pasc++;
            }
            useC += parseInt($(e).text());
        });
        if (pasc == 0) {
            $.dialog("没有可均分券的门店，请导入门店");
            return;
        }
        if (parseInt(aCount) < useC) {
            $.dialog("派送总数量不可小于门店总数量");
            return;
        }
        if ((parseInt(aCount) - useC) % parseInt(pasc) != 0) {
            $.dialog("派送数量不可有小数点，无法平均分配!");
            return;
        }
        var AvgC = (parseInt(aCount) - useC) / parseInt(pasc);
        var code_Str = "";
        var store_Str = "";
        $("lable[name=indexCount]").each(function (i, e) {
            store_Str += $(e).prop("id").substring(3) + ",";
            if ($(e).text() == "0") {
                code_Str += $(e).prop("id").substring(3) + ":" + AvgC + ",";
            } else {
                code_Str += $(e).prop("id").substring(3) + ":" + $(e).text() + ",";
            }
        });
        productArr = store_Str;
        codeStr = code_Str;
        dt_Table.fnDraw();
    });
    //审核按钮
    $("#btnPass").bind("click", function () {
        var id = $("#txtID").val();
        $.dialog("确认审核通过此购物券派送吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax("/CouponSend/ApproveCouponSendById", { Id: id, active: 1 }, function (data) {

                if (data.IsPass) {
                    window.location.href = "/CouponSend/CouponSendList";
                }
                else {
                    $.dialog(data.MSG);
                };

            });
        });
    });
    //撤销审核按钮
    //$("#btnCancelPass").bind("click", function () {
    //    var id = $("#txtID").val();
    //    $.dialog("确认撤销审核此购物券派送吗?", {
    //        footer: {
    //            closebtn: '取消',
    //            okbtn: '确认'
    //        }
    //    }, function () {
    //        ajax("/CouponSend/ApproveCouponSendById", { Id: id, active: 2 }, function (date){

    //            if (data.isPass) {
    //                window.location.href = "/CouponSend/CouponSendList";
    //            }
    //            else {
    //                $.dialog(data.MSG);
    //            };
    //            });
    //    })
    //});
    //模板下载
    $("#btnDownLoad").bind("click", function () {
        window.location = '/Upload/购物券派送门店导入模板.xls';
    });
    //导入按钮
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
    //添加产品——弹窗
    $("#btnAdd").bind("click", function () {
        $("#txtProductCode").val("");
        $("#txtProductName").val("");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#addProduct_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
        });
        loadProductInfo();
    });
    //添加产品——查询
    $("#btnProductSearch").bind("click", function () {
        dtProduct.fnDraw();
    });
    //添加产品——保存
    $("#btnProductSave").bind("click", function () {
        //var goodscode = $.trim($("#GoodsCode").val());
        //if (goodscode == "")
        //{
        //    $.dialog("产品代码不能为空");
        //    return;
        //}
        //ajax("/CouponUseRule/CheckGoodsCode", { GoodsCode: goodscode }, function (res)
        //{
        //    if (res.IsPass)
        //    {
        //        productArr += goodscode + ",";
        //        dt_Table.fnDraw();
        //        $.colorbox.close();
        //        $.dialog(res.MSG);
        //    } else
        //    { $.dialog(res.MSG); }
        //});
        var res = "";
        $("input[name='CheckProductCode']:checked").each(function () {
            var sendc = $("#" + $(this).val()).val();
            var reg = /^\+?[1-9][0-9]*$/

            if (!reg.test(sendc)) {
                res = "派送数量只能为大于0小于等于5000的整数!";

                return
            }
            if (sendc <= 0 || sendc > 5000) {
                res = "派送数量只能为大于0小于等于5000的整数!";
                return
            }
            productArr += $(this).val() + ",";
            codeStr += $(this).val() + ":" + sendc + ",";
        });
        if (res != "") {
            $.dialog(res);
            return;
        }
        dt_Table.fnDraw();
        $.colorbox.close();
    });
    //选择购物券——弹窗
    $("#divCouponNo").bind("click", function () {
        $("#txtCouponNo").val("");
        $("#txtCouponName").val("");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#selectCoupon_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
        });
        loadCouponInfo();
    });
    //查询购物券
    $("#btnCouponSearch").bind("click", function () {
        dtCoupon.fnDraw();
    });
    //保存购物券
    $("#btnCouponSave").bind("click", function () {
        var id = $("input[name='CheckCouponNo']:checked").val();
        if (id == undefined) {
            $.dialog("请选择购物券");
            return;
        }
        ajax("/CouponSend/GetCouponsById", { ID: id }, function (data) {
            $("#CouponNo").val(id);
            $("#CouponName").val(data.CouponName);
            $("#CouponValue").val(data.CouponValue);
            if (data.StartDate != null && data.StartDate != "") {
                $("#StartDate").val(data.StartDate.substr(0, 10));
            }
            if (data.EndDate != null && data.EndDate != "") {
                $("#EndDate").val(data.EndDate.substr(0, 10));
            }
            $("#SendedCount").val(data.SendCount == null ? 0 : data.SendCount);
            $("#CouponRemark").val(data.CouponRemark);
            $("lable[name=indexMoney]").each(function (i, e) {
                var id = $(e).prop("id").substring(3);
                $(e).text(parseInt($("#ic_" + id).text()) * parseFloat(data.CouponValue));
            });
            //(data.IsMember == true) ? $("#IsMember").prop("checked", true) : $("#IsMember").prop("checked", false);
        });
        $.colorbox.close();
    })
    //注册保存按钮
    $("#ActionForm").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            var count = 0;
            var msg = "";
            if ($("#CouponNo").val() == "") {
                $.dialog("请选择购物券");
                return;
            }
            $("lable[name=indexCount]").each(function (i, e) {
                count += parseInt($(e).text());
                if (parseInt($(e).text()) <= 0 || parseInt($(e).text()) > 5000) {
                    msg = "门店派送数量只能为大于0并且小于等于5000的正数";
                }
            });
            if (msg != "") {
                $.dialog(msg);
                return;
            }
            if (count != $("#ASendCount").val()) {
                $.dialog("派送总数量与各门店派送量之和不一致，是否确认将派送总量改为各门店派送量之和【" + count + "】并提交保存?", {
                    footer: {
                        closebtn: '取消',
                        okbtn: '确认'
                    }
                }, function () {
                    $("#ASendCount").val(count);
                    saveAction();
                })
                return;
            }
            saveAction();
        }
    });
    initImportUpload();

    //全选、全不选
    $("#dtProduct").on("click", "#ckALL", function () {
        $("[name='CheckProductCode']:checkbox").prop("checked", this.checked);
    });
    $("#dtProduct").on("click", "input[name='CheckProductCode']:checkbox", function () {
        var flag = true;
        $("input[name='CheckProductCode']:checkbox").each(function () {
            if (this.checked == false) {
                flag = false;
            }
        });
        $("#ckALL").prop("checked", flag);
    });

});
//验证数据
jQuery.validator.addMethod("EndLimit", function (value, element) {
    var returnVal = true;
    var Start = new Date($("#StartDate").val());
    var End = new Date($("#EndDate").val());
    if (Start.getTime() > End.getTime()) {
        returnVal = false;
    }
    return returnVal;
}, "结束时间必须大于开始时间");
jQuery.validator.addMethod("StartLimit", function (value, element) {
    var returnVal = true;
    var Start = new Date($("#StartDate").val());
    var End = new Date($("#EndDate").val());
    if (Start.getTime() > End.getTime()) {
        returnVal = false;
    }
    return returnVal;
}, "开始时间必须小于结束时间");
var DataValidator = $("#ActionForm").validate({
    //onSubmit: false,
    rules: {
        ASendCount: {
            required: true,
            number: true,
            min: 1
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存
function saveAction() {
    if (productArr == "") {
        $.dialog("门店信息不能为空");
        return;
    }
    var model = {
        CouponListID: $("#txtID").val(),
        CouponID: $("#CouponNo").val(),
        CouponInfo: $("#CouponName").val(),
        CouponValue: $("#CouponValue").val(),
        BeginDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        CouponDesc: $("#CouponRemark").val(),
        LimitValue: codeStr,
        SendCount: $("#ASendCount").val()
    }
    var postUrl;
    if (model.CouponListID == "") {
        postUrl = "/CouponSend/AddCouponSend";
    }
    else {
        postUrl = "/CouponSend/UpdateCouponSend";
    }
    ajax(postUrl, { model: model }, function (data) {
        if (data.MSG == "添加成功") {
            window.location.href = "/CouponSend/CouponSendList";
        }
        else {
            $.dialog(data.MSG);
        }
    });
};
//Json格式化
function JsonFormart(productarr) {
    var arr = productArr.split(",").uniquelize();
    if (arr.length > 0) {
        var list = [];
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] != "") {
                var pro = { StoreCode: arr[i] };
                list.push(pro);
            }
        }
        return JSON.stringify(list);
    }
    return "";
}
//Json格式化
function JsonFormart2(limitvalue) {
    var arr = JSON.parse(limitvalue);
    if (arr.length > 0) {
        var str = "";
        for (var i = 0; i < arr.length; i++) {
            str += arr[i].StoreCode + ",";
        }
        return str;
    }
    return "";
}
//上传
function initImportUpload() {
    ctrlUpload.initUpload("/CouponSend/ImportExcel", "", uploadBack);
}
//EXCEL上传回调
function uploadBack(data) {
    var ret = data.response;
    showLoading("正在导入中....");
    if (ret != "") {
        var store = ret.split(',');
        for (var i = 0; i < store.length; i++) {
            if (store[i] != "") {
                if (productArr.indexOf(store[i] + ",") == -1) {
                    productArr += store[i] + ",";
                    codeStr += store[i] + ":0,";
                }
            }
        }
    }

    dt_Table.fnDraw();
    $.colorbox.close();
    $("#tbFilePath").val("");
    hideLoading();

}
//删除产品
function goDelete(goodscode) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        var arr = productArr.split(",");

        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == goodscode) {
                arr.splice(i, 1);
            }
        }
        var str = codeStr.substring(codeStr.indexOf(goodscode));
        var info = str.substring(0, str.indexOf(',') + 1);
        codeStr = codeStr.replace(info, "");
        productArr = arr.toString();
        dt_Table.fnDraw();
    })
}
//加载购物券信息
function loadCouponInfo() {
    //加载数据表格
    dtCoupon = $('#dtCoupon').dataTable({
        sAjaxSource: '/CouponSend/LoadCoupon',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 5,
        aaSorting: [[1, "asc"]],
        aoColumns: [
            {
                data: null, title: "操作", sClass: "center", sortable: false, render: function (obj) {
                    return '<input type="radio" name="CheckCouponNo" value="' + obj.ID + '" />';
                }
            },
            { data: 'ID', title: "购物券编号", sClass: "center", sortable: true },
            { data: 'CouponName', title: "购物券名称", sClass: "center", sortable: false },
            { data: 'CouponValue', title: "面额", sClass: "center", sortable: false },
            {
                data: null, title: "开始时间", sClass: "center", sortable: false, render: function (obj) {
                    return obj.StartDate == null ? "" : obj.StartDate.substr(0, 10);
                }
            },
            {
                data: null, title: "结束时间", sClass: "center", sortable: false, render: function (obj) {
                    return obj.EndDate == null ? "" : obj.EndDate.substr(0, 10);
                }
            },
            {
                data: null, title: "是否会员券", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.IsMember) {
                        return "<span class=\"label label-success\">是</span>";
                    } else {
                        return "<span class=\"label label-danger\">不是</span>";
                    }
                    return result;
                }
            }

        ],
        fnFixData: function (d) {
            d.push({ name: 'ID', value: $("#txtCouponNo").val() });
            d.push({ name: 'CouponName', value: encode($("#txtCouponName").val()) });
        }
    });
}
//加载门店信息
function loadProductInfo() {
    //加载数据表格
    dtProduct = $('#dtProduct').dataTable({
        sAjaxSource: '/CouponSend/LoadStores',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 5,
        aaSorting: [[1, "asc"]],
        aoColumns: [
             {
                 data: null, title: '<input type="checkbox"  id="ckALL" />', sClass: "center", width: "80", sortable: false, render: function (obj) {
                     return '<input type="checkbox" name="CheckProductCode" value="' + obj.StoreCode + '" />';
                 }
             },
            { data: 'StoreCode', sClass: "center", title: "门店编号", sortable: true },
            { data: 'StoreName', sClass: "center", title: "门店名称", sortable: false },
             { data: 'CompanyName', sClass: "center", title: "所属分公司", sortable: false },
            {
                data: null, sClass: "center", title: "派送数量", sortable: false, render: function (obj) {
                    var result = "";
                    result += "<input id='" + obj.StoreCode + "' name='fSendCount' style='width:90px'>";
                    return result;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'Code', value: $("#txtProductCode").val() });
            d.push({ name: 'Name', value: $("#txtProductName").val() });
            d.push({ name: 'codelist', value: productArr });
        }
    });
    dtProduct.fnDraw();
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
//关闭遮罩层
function hideLoading() {
    $.closePopupLayer('processing');
    //$("#processingdiv").hide();
}

//集合去掉重复
Array.prototype.uniquelize = function () {
    var tmp = {},
        ret = [];
    for (var i = 0, j = this.length; i < j; i++) {
        if (!tmp[this[i]]) {
            tmp[this[i]] = 1;
            ret.push(this[i]);
        }
    }

    return ret;
}
//显示结果
function showResult(res) {
    if (res.IsPass) {
        $.dialog(res.MSG);
        var start = dt_Table.fnSettings()._iDisplayStart;
        var length = dt_Table.fnSettings()._iDisplayLength;
        dt_Table.fnPageChange(start / length, true);
    } else { $.dialog(res.MSG); }
}