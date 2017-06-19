var dt_Table;
var dtCoupon;
var dtProduct;
var productArr = "";


$(function ()
{
    $.ajaxSetup({
        async: false
    });
    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/CouponUseRule/LoadProduct',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aoColumns: [
            { data: 'BaseDataID', sClass: "center", title: "商品编号", sortable: true },
            { data: 'GoodsCode', sClass: "center", title: "商品代码", sortable: false },
            { data: 'GoodsName', sClass: "center", title: "商品名称", sortable: false },
            { data: 'GoodsBrand', sClass: "center", title: "商品品牌", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj)
                {
                    var result = "";
                     var info = $("#txtInfo").val();
                    if (info==0)
                    {
                        result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.GoodsCode + "')\" disabled=\"disabled\">删除</button>";
                    }
                    else
                    {
                        result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.GoodsCode + "')\">删除</button>";
                    }
                    return result;
                }
            }
        ],
        fnFixData: function (d)
        {
            d.push({ name: 'productArr', value: productArr });
        }
    });
    var id = $("#txtID").val();
    var info = $("#txtInfo").val();
    if (id == "")
    {
        $("#hTitleName").html("添加购物券派送规则");
    }
    else
    {
        if (info == 1)
        {
            $("#hTitleName").html("编辑购物券派送规则");
            //绑定时间控件
            $("#StartDate").datepicker('setStartDate', (new Date()).toLocaleDateString());
            $("#EndDate").datepicker('setStartDate', (new Date()).toLocaleDateString());
        }
        else
        {
            $("#hTitleName").html("查看购物券派送规则");
            $("#aCouponNo").prop("disabled", true);
            $("#CouponName").prop("disabled", true);
            $("#CouponValue").prop("disabled", true);         
            $("#CouponRemark").prop("disabled", true);
            $("#IsMember").prop("disabled", true);
            $("#btnImport").hide();
            $("#btnAdd").hide();
            $("#btnSave").hide();
        }       
        ajax("/CouponUseRule/GetCouponSendRuleById", { ID: id }, function (data)
        {
            $("#CouponNo").val(data.CouponNo);
            $("#CouponName").val(data.CouponName);
            $("#CouponValue").val(data.CouponValue);
            $("#StartDate").val(data.StartDate.substr(0, 10));
            $("#EndDate").val(data.EndDate.substr(0, 10));
            $("#CouponRemark").val(data.CouponRemark);
            (data.IsMember == true) ? $("#IsMember").prop("checked", true) : $("#IsMember").prop("checked", false);
            productArr = JsonFormart2(data.LimitValue);
            dt_Table.fnDraw();
        });
    }

    //模板下载
    $("#btnDownLoad").bind("click", function ()
    {
        window.location = '/Upload/购物券使用规则导入模板.xls';
    });
    //导入按钮
    $("#btnImport").bind("click", function ()
    {
        $("#tbFilePath").val("");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#import_data",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
            onComplete: function ()
            {
                $("#btnSaveImport").bind("click", function ()
                {
                    ctrlUpload.startUpload();
                });
            }
        });
    });
    //添加产品——弹窗
    $("#btnAdd").bind("click", function ()
    {
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
    $("#btnProductSearch").bind("click", function ()
    {
        dtProduct.fnDraw();
    });
    //添加产品——保存
    $("#btnProductSave").bind("click", function ()
    {
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
        $("input[name='CheckProductCode']:checked").each(function ()
        {
            productArr += $(this).val() + ",";
        });
        dt_Table.fnDraw();
        $.colorbox.close();
        $.dialog(res.MSG);
    });
    //选择购物券——弹窗
    $("#divCouponNo").bind("click", function ()
    {
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
    $("#btnCouponSearch").bind("click", function ()
    {
        dtCoupon.fnDraw();
    });
    //保存购物券
    $("#btnCouponSave").bind("click", function ()
    {
        var id = $("input[name='CheckCouponNo']:checked").val();
        ajax("/CouponUseRule/GetCouponsById", { ID: id }, function (data)
        {
            $("#CouponNo").val(id);
            $("#CouponName").val(data.CouponName);
            $("#CouponValue").val(data.CouponValue);
            $("#CouponRemark").val(data.CouponRemark);
            (data.IsMember == true) ? $("#IsMember").prop("checked", true) : $("#IsMember").prop("checked", false);
        });
        $.colorbox.close();
    })

    //注册保存按钮
    $("#ActionForm").submit(function (e)
    {
        e.preventDefault();
        if (DataValidator.form())
        {
            saveAction();
        }
    });
    initImportUpload();

    //全选、全不选
    $("#dtProduct").on("click", "#ckALL", function ()
    {
        $("[name='CheckProductCode']:checkbox").prop("checked", this.checked);
    });
    $("#dtProduct").on("click", "input[name='CheckProductCode']:checkbox", function ()
    {
        var flag = true;
        $("input[name='CheckProductCode']:checkbox").each(function ()
        {
            if (this.checked == false)
            {
                flag = false;
            }
        });
        $("#ckALL").prop("checked", flag);
    });

});
//验证数据
jQuery.validator.addMethod("EndLimit", function (value, element)
{
    var returnVal = true;
    var Start = new Date($("#StartDate").val());
    var End = new Date($("#EndDate").val());
    if (Start.getTime() > End.getTime())
    {
        returnVal = false;
    }
    return returnVal;
}, "结束时间必须大于开始时间");
jQuery.validator.addMethod("StartLimit", function (value, element)
{
    var returnVal = true;
    var Start = new Date($("#StartDate").val());
    var End = new Date($("#EndDate").val());
    if (Start.getTime() > End.getTime())
    {
        returnVal = false;
    }
    return returnVal;
}, "开始时间必须小于结束时间");
var DataValidator = $("#ActionForm").validate({
    //onSubmit: false,
    rules: {
        StartDate: {
            required: true,
            StartLimit: true
        },
        EndDate: {
            required: true,
            EndLimit: true
        }
    },
    errorPlacement: function (error, element)
    {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存
function saveAction()
{
    if (productArr == "")
    {
        $.dialog("商品信息不能为空");
        return;
    }
    var model = {
        ID: $("#txtID").val(),
        CouponNo: $("#CouponNo").val(),
        CouponName: $("#CouponName").val(),
        CouponValue: $("#CouponValue").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        CouponRemark: $("#CouponRemark").val(),
        IsMember: ($("#IsMember").attr("checked") == "checked") ? true : false,
        LimitValue: JsonFormart(productArr)
    };
    var postUrl;
    if (model.ID == "")
    {
        postUrl = "/CouponUseRule/AddCouponSendRule";
    }
    else
    {
        postUrl = "/CouponUseRule/UpdateCouponSendRule";
    }
    ajax(postUrl, { model: model }, function (data)
    {
        if (data.MSG == "添加成功")
        {
            window.location.href = "/CouponUseRule/SendIndex";
        }
        else
        {
            $.dialog(data.MSG);
        }
    });
};
//Json格式化
function JsonFormart(productarr)
{
    var arr = productArr.split(",").uniquelize();
    if (arr.length > 0)
    {
        var list = [];
        for (var i = 0; i < arr.length; i++)
        {
            if (arr[i] != "")
            {
                var pro = { GoodsCode: arr[i] };
                list.push(pro);
            }
        }
        return JSON.stringify(list);
    }
    return "";
}
//Json格式化
function JsonFormart2(limitvalue)
{
    var arr = JSON.parse(limitvalue);
    if (arr.length > 0)
    {
        var str = "";
        for (var i = 0; i < arr.length; i++)
        {
            str += arr[i].GoodsCode + ",";
        }
        return str;
    }
    return "";
}
//上传
function initImportUpload()
{
    ctrlUpload.initUpload("/CouponUseRule/ImportExcel", "", uploadBack);
}
//EXCEL上传回调
function uploadBack(data)
{
    var ret = data.response;
    showLoading("正在导入中....");
    productArr += ret;
    dt_Table.fnDraw();
    $.colorbox.close();
    $("#tbFilePath").val("");
    hideLoading();

}
//删除产品
function goDelete(goodscode)
{
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function ()
    {
        var arr = productArr.split(",");
        for (var i = 0; i < arr.length; i++)
        {
            if (arr[i] == goodscode)
            {
                arr.splice(i, 1);
            }
        }
        productArr = arr.toString();
        dt_Table.fnDraw();
    })
}
//加载购物券信息
function loadCouponInfo()
{
    //加载数据表格
    dtCoupon = $('#dtCoupon').dataTable({
        sAjaxSource: '/CouponUseRule/LoadCouponUseRule',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 5,
        aaSorting: [[1, "asc"]],
        aoColumns: [
            {
                data: null, title: "操作", sClass: "center", sortable: false, render: function (obj)
                {
                    return '<input type="radio" name="CheckCouponNo" value="' + obj.ID + '" />';
                }
            },
            { data: 'ID', title: "购物券编号", sClass: "center", sortable: true },
            { data: 'CouponName', title: "购物券名称", sClass: "center", sortable: false },
            { data: 'CouponValue', title: "面额", sClass: "center", sortable: false },
            {
                data: null, title: "开始时间", sClass: "center", sortable: false, render: function (obj)
                {
                    return obj.StartDate.substr(0, 10);
                }
            },
            {
                data: null, title: "结束时间", sClass: "center", sortable: false, render: function (obj)
                {
                    return obj.EndDate.substr(0, 10);
                }
            },
            {
                data: null, title: "是否会员券", sClass: "center", sortable: false,
                render: function (obj)
                {
                    if (obj.IsMember)
                    {
                        return "<span class=\"label label-success\">是</span>";
                    } else
                    {
                        return "<span class=\"label label-danger\">不是</span>";
                    }
                    return result;
                }
            },
            {
                data: null, title: "状态", sClass: "center", sortable: false,
                render: function (obj)
                {
                    if (obj.IsEnble)
                    {
                        return "<span class=\"label label-success\">启用</span>";
                    } else
                    {
                        return "<span class=\"label label-danger\">禁用</span>";
                    }
                    return result;
                }
            }

        ],
        fnFixData: function (d)
        {
            d.push({ name: 'ID', value: $("#txtCouponNo").val() });
            d.push({ name: 'CouponName', value: encode($("#txtCouponName").val()) });
        }
    });
}
//加载商品信息
function loadProductInfo()
{
    //加载数据表格
    dtProduct = $('#dtProduct').dataTable({
        sAjaxSource: '/CouponUseRule/LoadProducts',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 5,
        aaSorting: [[1, "asc"]],
        aoColumns: [
             {
                 data: null, title: '<input type="checkbox"  id="ckALL" />', sClass: "center", sortable: false, render: function (obj)
                 {
                     return '<input type="checkbox" name="CheckProductCode" value="' + obj.GoodsCode + '" />';
                 }
             },
            { data: 'BaseDataID', sClass: "center", title: "商品编号", sortable: true },
            { data: 'GoodsCode', sClass: "center", title: "商品代码", sortable: false },
            { data: 'GoodsName', sClass: "center", title: "商品名称", sortable: false },
            { data: 'GoodsBrand', sClass: "center", title: "商品品牌", sortable: false }
        ],
        fnFixData: function (d)
        {
            d.push({ name: 'Code', value: $("#txtProductCode").val() });
            d.push({ name: 'Name', value: $("#txtProductName").val() });
        }
    });
}
//开启遮罩层
function showLoading(desc)
{

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
function hideLoading()
{
    $.closePopupLayer('processing');
    //$("#processingdiv").hide();
}

//集合去掉重复
Array.prototype.uniquelize = function ()
{
    var tmp = {},
        ret = [];
    for (var i = 0, j = this.length; i < j; i++)
    {
        if (!tmp[this[i]])
        {
            tmp[this[i]] = 1;
            ret.push(this[i]);
        }
    }

    return ret;
}
