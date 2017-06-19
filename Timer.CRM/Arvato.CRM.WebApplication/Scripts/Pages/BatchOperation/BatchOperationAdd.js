var array = new Array();
var dt_Table;
var dtCard;
var dtCardList;
var productArr = "";
var codeStr = "";
var reg12 = new RegExp("^\\d{12}$");

$(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    };


    $.ajaxSetup({
        async: false
    });
    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/BatchOperation/LoadWhere',
        bSort: false,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aoColumns: [
            //{
            //    data:null, sClass: "center", title: "卡类型", sortable: false, render: function (obj) {
            //    return "实体卡";
            //} },
             { data: 'CardTypeName', title: "卡类型", sClass: "center", sortable: true },
            { data: 'beginCard', sClass: "center", title: "起始卡号", sortable: false },
            { data: 'endCard', sClass: "center", title: "截止卡号", sortable: false },
            {
                data:null, sClass: "center", title: "卡状态", sortable: false, render: function (obj) {
                    var status = obj.CardStatus;
                    switch (status) {
                        case "2":
                            return "使用中";
                        case "0":
                            return "已发卡";
                        case "1":
                            return "已核对";
                        case "-1":
                            return "已挂失";
                        case "-2":
                            return "已冻结";
                        case "-3":
                            return "已作废";
                        default:
                            return "";
                    }
                }
            },
            {
                data: 'CompanyName', sClass: "center", title: "所属分公司", sortable: false
            },
             {
                 data: 'StoreName', sClass: "center", title: "所属门店", sortable: false
             },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    var result = "";
                    var info = $("#txtInfo").val();
                    if (info != 2) {
                        result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.sort + "')\" >删除</button>";
                    }
                    return result;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'Array', value: JSON.stringify(array) });
        }
    });
    //加载公司下拉框
    $.ajax({
        type: 'post',
        url: '/PurchasesNew/LoadCompany',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                opt += "<option value=''>请选择</option>";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '/' + result.data[i].CompanyCode + '</option>';
                }
                $('#toCompany').append(opt);
                $('#wCompany').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
            else {
                opt = "<option value=''>请选择</option>";
                $('#toCompany').append(opt);
                $('#wCompany').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
        },
        error: function (e) {
            e.responseText;
        }
    })
    //加载卡类型
    $.ajax({
        type: 'post',
        url: '/BatchOperation/GetCardType',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.length > 0) {
                var opt = "";
                opt += "<option value=''>请选择</option>";
                for (var i = 0; i < result.length; i++) {
                    opt += '<option value=' + result[i].CardTypeCodeBase + '>' + result[i].CardTypeNameBase + '</option>';
                }
                $('#CardType').append(opt);
            }
            else {
                opt = "<option value=''>请选择</option>";
                $('#CardType').append(opt);
            }
        },
        error: function (e) {
            e.responseText;
        }
    })
    
    //转移分公司
    $('#toCompany').change(function () {
        $('#toStore').html("");
         var postdata = { companyCode: $('#toCompany').find('option:selected').val() };
        $.post('/Distribution/LoadStore', postdata, function (result) {
            if (result.data.length > 0) {
                var opt = '<option value="">==请选择==<option>';
                $.each(result.data, function (i, data) {
                    opt += '<option value="' + data.ShoppeCode + '">' + data.ShoppeName + '/' + data.ShoppeCode + '<option>'
                });
                $('#toStore').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
            else {
                $('#toStore').append('<option value="">==无==<option>');
                $(".chzn_a").trigger("liszt:updated");
            };
        });

        //$.ajax({
        //    type: 'post',
        //    url: '/MemberTransform/GetStoreList',
        //    dataType: 'json',
        //    data: { company: $('#toCompany').val() },
        //    success: function (result) {
        //        if (result.length > 0) {
        //            var opt = "";
        //            opt += "<option value=''>请选择</option>";
        //            for (var i = 0; i < result.length; i++) {
        //                opt += '<option value=' + result[i].StoreCode + '>' + result[i].StoreName + '</option>';
        //            }
        //            $('#toStore').append(opt);
                
        //        }
        //        else {
        //            opt = "<option value=''>请选择</option>";
        //            $('#toStore').append(opt);

        //        }
        //    },
        //    error: function (e) {
        //        e.responseText;
        //    }
        //})
    });
    //选择条件分公司
    $('#wCompany').change(function () {
        $('#wStore').html("");
        var postdata = { companyCode: $('#wCompany').find('option:selected').val() };
        $.post('/Distribution/LoadStore', postdata, function (result) {
            if (result.data.length > 0) {
                var opt = '<option value="">==请选择==<option>';
                $.each(result.data, function (i, data) {
                    opt += '<option value="' + data.ShoppeCode + '">' + data.ShoppeName + '/' + data.ShoppeCode + '<option>'
                });
                $('#wStore').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
            else {
                $('#wStore').append('<option value="">==无==<option>');
                $(".chzn_a").trigger("liszt:updated");
            };
        });
    });
    var id = $("#txtID").val();
    var info = $("#txtInfo").val();
    if (id == "") {
        $("#hTitleName").html("添加批量操作");
    }
    else {
        if (info == "1") {
            $("#hTitleName").html("编辑批量操作");
        } else {
            $("#hTitleName").html("查看批量操作");
            $("#OperationType").css("display", "none");
            $("#OperationType2").css("display", "block");
            $("#toCompany").css("display", "none");
            $("#toCompany2").css("display", "block");
            $("#toStore").css("display", "none");
            $("#toStore2").css("display", "block");
            $("#Remark").prop("readonly", "readonly");
            $("#btnAdd").css("display", "none");
            $("#btnSearchCard").css("display", "none");
            $("#btnSave").css("display", "none");
            $("#btnPass").css("display", "none");
            $("#CardView").css("display", "block");
            dtCardList = $('#dt_CardList').dataTable({
                sAjaxSource: '/BatchOperation/LoadCardListById',
                bSort: false,   //不排序
                bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true,  //每次请求后台数据
                bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 10,
                aaSorting: [[1, "asc"]],
                aoColumns: [
                     { data: 'CardTypeName', title: "卡类型", sClass: "center", sortable: true },
                    { data: 'CardNo', title: "卡号", sClass: "center", sortable: true },
                    {
                        data: null, title: "卡状态", sClass: "center", sortable: false, render: function (obj) {
                            var status = obj.Statu;
                            switch (status) {
                                case "2":
                                    return "使用中";
                                case "0":
                                    return "已发卡";
                                case "1":
                                    if (obj.IsSalesStatus == 1) {
                                        return " 已补发";
                                    }
                                    if (obj.IsSalesStatus == 0 && obj.IsUsed == false) {
                                        return "已核对"
                                    }
                                    return "";
                                case "-1":
                                    return "已挂失";
                                case "-2":
                                    return "已冻结";
                                case "-3":
                                    return "已作废";
                                default:
                                    return "";
                            }
                        }
                    },
                ],
                fnFixData: function (d) {
                    d.push({ name: 'ID', value: $("#txtID").val() });
                }
            });
        }

        ajax("/BatchOperation/GetOperationById", { ID: id }, function (data) {
            $("#txtOddNumber").val(data.OddNumber);
            $("#OperationType").val(data.OperationType);
            $("#OperationType2").val($("#OperationType").find('option:selected').text());
            $("#toCompany").val(data.CompanyCode); 
            $("#Remark").val(data.Remark);
            $("#toCompany2").val($("#toCompany").find('option:selected').text());
            $("#toStore").val(data.StoreCode);
            if (data.Status=="1") {
                $("#txtStatu").val("已审核");
            }
            $("#txtCreatInfo").val(data.AddedUser + "~" + data.AddedDate);
            $("#txtUpdateInfo").val(data.ModifiedUser + "~" + data.ModifiedDate);
            if (data.OperationType == "1") {
                $("#transformInfo").css("display", "block");
                $.ajax({
                    type: 'post',
                    url: '/MemberTransform/GetStoreList',
                    dataType: 'json',
                    data: { company: $('#toCompany').val() },
                    success: function (result) {
                        if (result.length > 0) {
                            var opt = "";
                            opt += "<option value=''>请选择</option>";
                            for (var i = 0; i < result.length; i++) {
                                if (data.StoreCode == result[i].StoreCode) {
                                    $("#toStore2").val(result[i].StoreName);
                                    opt += '<option selected="selected" value=' + result[i].StoreCode + '>' + result[i].StoreName + '</option>';
                                } else {
                                    opt += '<option value=' + result[i].StoreCode + '>' + result[i].StoreName + '</option>';
                                }
                               
                            }
                            $('#toStore').append(opt);
                        }
                        else {
                            opt = "<option value=''>请选择</option>";
                            $('#toStore').append(opt);
                        }
                    },
                    error: function (e) {
                        e.responseText;
                    }
                })
            }
            for (var i = 0; i < data.OperationWhere.length; i++) {
                var adata = {
                    CardTypeName: data.OperationWhere[i].CardTypeName,
                    beginCard: data.OperationWhere[i].beginCard,
                    endCard: data.OperationWhere[i].endCard,
                    CardStatus: data.OperationWhere[i].CardStatus,
                    CompanyCode: data.OperationWhere[i].CompanyCode,
                    StoreCode: data.OperationWhere[i].StoreCode,
                    CompanyName: data.OperationWhere[i].CompanyName,
                    StoreName: data.OperationWhere[i].StoreName
                };
                array.push(adata);
            }
            dt_Table.fnDraw();
        });
    }
    //单据类型控件
    $("#OperationType").bind("change", function () {
        if ($("#OperationType").val()=="1") {
            $("#transformInfo").css("display", "block");
        } else {
            $("#transformInfo").css("display", "none");
        }
    });
    //审核按钮
    $("#btnPass").bind("click", function () {
        var id = $("#txtID").val();
        $.dialog("确认审核通过此批量操作吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            showLoading("审核中...");
            ajax("/BatchOperation/ApproveOperationById", { Id: id, active: 1 }, function (data) {
                if (data.IsPass) {
                    $.dialog(data.MSG, {
                        footer: {
                            //closebtn: '取消',
                            okbtn: '确认'
                        }
                    }, function () {
                        window.location.href = "/BatchOperation/BatchOperationList";
                       
                    }, function () {
                        window.location.href = "/BatchOperation/BatchOperationList";
                    });
                    
                }
                else {
                    $.dialog(data.MSG);
                };
                hideLoading();
            });
        });
    });
   
    //添加查询条件——弹窗
    $("#btnAdd").bind("click", function () {
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#add_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
        });
        //loadProductInfo();
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
    //添加条件
    $('#btnWhereSave').click(function () {
        var beginCard = $('#txtBeginCard').val();
        var endCard = $('#txtEndCard').val();
        var CardStatus = $('#CardStatus').val();
        var Company = $('#wCompany').val();
        var Store = $('#wStore').val();
        if ($("#CardType").val() == "" ) {
            $.dialog("请选择卡类型");
            return false;
        }
        if (beginCard != "" && reg12.test(beginCard) == false) {
            $.dialog("请输入正确的起始卡号");
            return false;
        }
        if (endCard != "" && reg12.test(endCard) == false) {
            $.dialog("请输入正确的截止卡号");
            return false;
        }    
        if (Store=="") {
            $.dialog("请选择门店");
            return false;
        }
        var data = {
            CardTypeName: $("#CardType").find('option:selected').text(),
            beginCard: beginCard,
            endCard: endCard,
            CardStatus: CardStatus,
            CompanyCode: Company,
            StoreCode: Store,
            CompanyName: $("#wCompany").find('option:selected').text(),
            StoreName: $("#wStore").find('option:selected').text()
        };
        for (var i = 0; i < array.length; i++) {
            if (array[i].CardTypeName == data.CardTypeName && array[i].beginCard == data.beginCard && array[i].endCard == data.endCard
                && array[i].CardStatus == data.CardStatus && array[i].CompanyCode == data.CompanyCode && array[i].StoreCode == data.StoreCode)
            {
                $.dialog("已存在相同的筛选条件，不可添加");
                return false;
            }
        }
        array.push(data);
        $.colorbox.close();
        dt_Table.fnDraw();
    })

    //加载卡列表——弹窗
    $("#btnSearchCard").bind("click", function () {
        //if ($("#OperationType").val()=="") {
        //    $.dialog("请选择单据类型");
        //    return;
        //}
        if (array.length== 0) {
            $.dialog("请添加筛选条件");
            return;
        }
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#selectCard_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
        });
        loadCardInfo();
    });

    //注册保存按钮
    $("#ActionForm").submit(function (e) {
        if (array.length == 0) {
            $.dialog("请添加筛选条件");
            return false;
        } 
            e.preventDefault();
            if (DataValidator.form()) {
                if ($('#OperationType').val() == "1" && ($('#toCompany').val() == "" || $('#toCompany').val() == "")) {
                    $.dialog("卡转移必须选择分公司和门店");
                    return false;
                } 
                    saveAction();
            }
        
    });

  

});
var DataValidator = $("#ActionForm").validate({
    //onSubmit: false,
    rules: {
        OperationType: {
            required: true
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存
function saveAction() {
    
    var model = {
        OperationType: $("#OperationType").val(),
        CompanyCode: $("#toCompany").val(),
        CompanyName: $("#toCompany").find('option:selected').text(),
        StoreCode: $("#toStore").val(),
        StoreName: $("#toStore").find('option:selected').text(),
        Remark: $("#Remark").val(),
        OperationID: $("#txtID").val(),
        OddNumber: $("#txtOddNumber").val(),
        Where: JSON.stringify(array)
    }
    var postUrl;
    if (model.OperationID == "") {
        postUrl = "/BatchOperation/AddOperation";
    }
    else {
        postUrl = "/BatchOperation/UpdateOperation";
    }
    showLoading("正在保存");
    ajax(postUrl, { model: model }, function (data) {
        if (data.MSG == "添加成功") {
            window.location.href = "/BatchOperation/BatchOperationList";
        }
        else {
            $.dialog(data.MSG);
        }
        hideLoading();
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

//删除筛选条件
function goDelete(i) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        array.splice(i,1)
        dt_Table.fnDraw();
    })
}
//加载卡列表信息
function loadCardInfo() {
    //加载数据表格
    dtCard = $('#dtCard').dataTable({
        sAjaxSource: '/BatchOperation/LoadCardList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[1, "asc"]],
        aoColumns: [
            { data: 'CardTypeName', title: "卡类型", sClass: "center", sortable: false },
            { data: 'CardNo', title: "卡号", sClass: "center", sortable: true },
            {
                data: null, title: "卡状态", sClass: "center", sortable: false, render: function (obj) {
                    var status = obj.Status;
                    switch (status) {
                        case "2":
                            return "使用中";
                        case "0":
                            return "已发卡";
                        case "1":
                            if (obj.IsSalesStatus == 1) {
                                return " 已补发";
                            }
                            if (obj.IsSalesStatus == 0 && obj.IsUsed == false) {
                                return "已核对"
                            }
                            return "";
                        case "-1":
                            return "已挂失";
                        case "-2":
                            return "已冻结";
                        case "-3":
                            return "已作废";
                        default:
                            return "";
                    }
                }
            },
        ],
        fnFixData: function (d) {
            d.push({ name: 'Array', value: JSON.stringify(array) });
            d.push({ name: 'OperationType', value: $("#OperationType").val() });
        }
    });
    dtCard.fnDraw();
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