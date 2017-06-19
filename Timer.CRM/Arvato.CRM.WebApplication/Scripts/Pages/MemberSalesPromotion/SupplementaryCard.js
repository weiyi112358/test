var dtCustomerLevel;
$(function () {
    //保存
    $("#btnSave").click(function () {
        var cardAmount = $('#txtCardAmount').val();
        var cardCost = $('#txtCardCost').val();
        var cardType = $('#addCardType').val();
        var companyCode = $('#addCompany').val();
        var cardCostId = $("#txtcardCostId").val();
        if (companyCode == 0) {
            $.dialog("请选择分公司！");           
            return;
        }
        if (cardType==0)
        {
            $.dialog("请选择补卡类型");
            return;
        }
        if (DataValidator.form())
        {
            ajax('/MemberSalesPromotion/AddSupplementaryCard', { CardAmount: cardAmount, CardCost: cardCost, CompanyCode: companyCode, CardType: cardType, CardCostId: cardCostId }, function (res)
            {
                if (res.IsPass)
                {
                    $.dialog(res.MSG);
                    $.colorbox.close();
                    dtCustomerLevel.fnDraw();
                } else
                {
                    $.dialog(res.MSG);
                    dtCustomerLevel.fnDraw();
                }
            });
        }
    });
    //加载补卡信息
    loadCardInfo();
    //购买金额保留两位小数
    $("#txtCardAmount").blur(function ()
    {
        var val = $(this).val();
        if (val != '')
        {
            if (val.indexOf('.')==0)
            {
                val = "0" + val;
            }
            if (utility.isNumber(val))
            {
                $(this).val(formatDecimal(val));
            }
        }
    });
    //补发费用保留两位小数
    $("#txtCardCost").blur(function ()
    {
        var val = $(this).val();
        if (val != '')
        {
            if (val.indexOf('.') == 0)
            {
                val = "0" + val;
            }
            if (utility.isNumber(val))
            {
                $(this).val(formatDecimal(val));
            }
        }
    });
});

var DataValidator = $("#ActionForm").validate({
    //onSubmit: false,
    rules: {
        txtCardAmount: {
            required: true,
            number: true,
            range: [0, 10000]
        },
        txtCardCost: {
            required: true,
            number: true,
            range: [0, 10000]
        }
    },
    errorPlacement: function (error, element)
    {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});


//弹出新建框
function goEdit() {
    $("#table_editData .heading h3").html("添加卡补发规则");
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editData",
        inline: true
    });
    $.colorbox.resize();
}

//加载补卡信息
function loadCardInfo() {
    if (!dtCustomerLevel) {
        dtCustomerLevel = $("#tbCustomerLevel").dataTable({
            sAjaxSource: '/MemberSalesPromotion/GetSupplementaryCard',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,//每页显示8条
            order: [[7, "desc"]],//按第7列排序
            aoColumns: [
                { data: "ID", title: "", sClass: "center", sortable: false, bVisible: false },
                { data: "CardTypeName", title: "补卡类型", sClass: "center", sortable: false },
                { data: "CardAmount", title: "购买金额", sClass: "center", sortable: false },
                { data: "CardCost", title: "补发费用", sClass: "center", sortable: false },
                { data: "CompanyCodeName", sClass: "center", title: "分公司", sortable: false },
                { data: "AddedUser", title: "添加人", sClass: "center", sortable: false },
                { data: "ModifiedUser", title: "更改人", sClass: "center", sortable: false },
                { data: "AddedDate", title: "", sClass: "center", sortable: false, bVisible: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        if (obj.IsEnable)
                            return "<button class=\"btn\" id=\"btnModify\"  onclick=\"modifyRules('" + obj.ID + "')\">编辑</button><button class=\"btn btn-danger\" onclick=\"Inactive('" + obj.ID + "')\">禁用</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteRule('" + obj.ID + "')\">删除</button>";
                        else
                            return "<button class=\"btn btn-info\" onclick=\"active('" + obj.ID + "')\">启用</button> <button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteRule('" + obj.ID + "')\">删除</button>";
                    }
                }
            ],
            //提交参数
            fnFixData: function (d) {
                d.push({ name: 'CompanyCode', value: $("#company").val() });//name:参数名，value：参数值
            }
        });
    } else {
        dtCustomerLevel.fnDraw();
    }
}

//查询
$('#btnSearch').on("click", function () {
    loadCardInfo();
});


//清空数据
function goClear() {
    $("#txtcardCostId").val('');
    $("#addCompany").val('0');
    $("#addCardType").val('0');
    $("#txtCardAmount").val('');
    $("#txtCardCost").val('');
}

//编辑信息
function modifyRules(id) {
    $("#table_editData .heading h3").html("编辑补发卡规则");
    goClear();
    ajax("/MemberSalesPromotion/GetCardCostById", { ID: id }, function (res) {
        $("#txtcardCostId").val(res.ID);
        $("#addCompany").val(res.CompanyCode);
        $("#addCardType").val(res.CardType);
        $("#txtCardAmount").val(res.CardAmount);
        $("#txtCardCost").val(res.CardCost);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editData",
        inline: true
    });
    $.colorbox.resize();
}

//启用
function active(id) {
    $.dialog("确认启用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function() {
        ajax("/MemberSalesPromotion/CardCostById", { ID: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dtCustomerLevel.fnDraw();
            } else {
                $.dialog(res.MSG);
            }
        });
    });
}

//禁用
function Inactive(id) {
    $.dialog("确认禁用吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function() {
        ajax("/MemberSalesPromotion/InCardCostById", { ID: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dtCustomerLevel.fnDraw();
            } else {
                $.dialog(res.MSG);
            }
        });
    });
}

//删除规则
function deleteRule(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function() {
        ajax("/MemberSalesPromotion/DeleteCardCostById", { ID: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dtCustomerLevel.fnDraw();
            } else {
                $.dialog(res.MSG);
            }
        });
    });
}

//保留两位小数
function formatDecimal(x)
{
    var f = parseFloat(x);
    if (isNaN(f))
    {
        return false;
    }
    var f = Math.round(x * 100) / 100;
    var s = f.toString();
    var rs = s.indexOf('.');
    if (rs < 0)
    {
        rs = s.length;
        s += '.';
    }
    while (s.length <= rs + 2)
    {
        s += '0';
    }
    return s;
}

