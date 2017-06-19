$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    LoadStoreList();

    //搜索会员
    $("#btnSearch").click(function () {
        //至少有一个条件才能搜索
        if ($("#txtNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '') {
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
    })
    $("#drpMemType").change(function () {
        //$.colorbox.resize();
        $("#drpInvoiceType").val('');
        $("#txtCorpName").val('');
        $("#txtIdentyNo").val('');
        $("#txtCreditCode").val('');
        $("#txtTelephone").val('');
        $("#txtAddress").val('');
        $("#txtBank").val('');//优惠原因
        $("#txtBankCode").val('');//服务顾问
        var memtype = $("#drpMemType").val();
        if (memtype == '0') {

            $(".divcorp").show();
        } else {//个人
            $(".divcorp").hide();
        }
    });

    //新增条目
    $("#frmAddStore").submit(function (e) {
        e.preventDefault();
        if ($("#txtInvoice").val() <= 0) {
            $.dialog("开票金额不能为0");
            return;
        }
        //开票金额不能大于储值金额 验证
        if ($("#stgValidValue1").text() - $("#txtInvoice").val() < 0) {
            $.dialog("开票金额不能超过未开票金额");
            return;
        }
        if (DataValidatorAdd.form()) {
            var invoice = {
                InvoiceId: $("#txtInvoiceId").val(),
                MemberId: $("#txtMemId").val(),
                InvoiceCash: $("#txtInvoice").val(),
                InvoiceDrawer: $("input[name='radInvoice']:checked").val(),
                StoreCode: $("#drpStore2").val(),
                MemType: $("#drpMemType").val(),
                InvoiceType: $("#drpInvoiceType").val(),
                CorpName: $("#txtCorpName").val(),
                IdentyNo: $("#txtIdentyNo").val(),
                CreditCode: $("#txtCreditCode").val(),
                CorpMobile: $("#txtTelephone").val(),
                Address: encode($("#txtAddress").val()),
                Bank: $("#txtBank").val(),//优惠原因
                BankNo: $("#txtBankCode").val(),//服务顾问
            }

            ajax("/Member360/AddOrUpdateInvoiceData", { invoice: invoice }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();
                    LoadTrade();
                    $.dialog(res.MSG);
                } else $.dialog(res.MSG);
            });
        }

    })
    //$("input[name='radInvoice']").click(function () {
    //    $("#drpStore2").val('');
    //    var t = $(this).val();
    //    //if (t == "门店") {
    //    //    $(".divstore").show();

    //    //} else {
    //    //    $(".divstore").hide();
    //    //}
    //})

    $("#btnSearchTrade").click(function () {
        LoadTrade();
    })
})

//新增时验证数据
var DataValidatorAdd = $("#frmAddStore").validate({
    //onSubmit: false,
    rules: {
        txtInvoice: {
            required: true,
        },
        txtCorpName: {
            maxlength: 50,
            isSb:true,
        },
        txtIdentyNo: {
            maxlength: 50,
            isSb: true,
        },
        txtCreditCode: {
            maxlength: 50,
            isSb: true,
        },
        txtAddress: {
            maxlength: 50,
            isSb: true,
        },
        txtTelephone: {
            maxlength: 50,
            isMobileNo:true,
        },
        txtBank: {
            maxlength: 50,
            isSb: true,
        },
        txtBankCode: {
            isBankAccount: true,
            isSb: true,
        },

    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
var dt_StoreData;
function LoadTrade() {
    if (!dt_StoreData) {
        //加载数据表格
        dt_StoreData = $('#dt_StoreData').dataTable({
            sAjaxSource: '/Member360/GetInvoiceData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            order: [[4, "desc"]],
            aoColumns: [
            { data: 'StoreName', title: "申请门店", sortable: false, sWidth: "100", },
            { data: 'InvoiceDrawer', title: "开票方", sortable: false, sWidth: "100", },
            { data: 'MemberCardNo', title: "会员卡号", sortable: false, },
            { data: 'Memtype', title: "会员类型", sortable: false, },
            { data: 'CustomerMobile', title: "手机号", sortable: false, },
            { data: 'AmountInvoice', title: "开票金额", sortable: true, },
            {
                data: 'InvoiceDate', title: "开票日期", sortable: false, render: function (obj) {
                    if (obj.length > 10) {
                        return obj.substring(0, 10);
                    } else {
                        return "";
                    }
                }
            },
            {
                data: 'StatusInvoice', title: "审批状态", sortable: false, render: function (obj) {
                    if (obj == 1) return "未审批"; else return "已审批";
                }
            },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.StatusInvoice == "1")
                        return "<button class=\"btn btn-default\" onclick=\"Edit(" + obj.TradeID + ")\">编辑</button>&nbsp;&nbsp;";
                    else
                        return "<button class=\"btn btn-default\" onclick=\"Check(" + obj.TradeID + ")\">查看</button>&nbsp;&nbsp;";
                }
            }

            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtMemId").val() });
                d.push({ name: 'startDate', value: $("#txtStartDate").val() });
                d.push({ name: 'endDate', value: $("#txtEndDate").val() });
            }
        });
    } else {
        dt_StoreData.fnDraw();
    }
}
function Edit(v) {
    clearData();
    $("#txtInvoiceId").val(v);
    if ($("#txtStatus").val() == "0") {
        $.dialog("会员状态为停用，不允许编辑开票");
        return;
    }
    $("#addStore_dialog .heading h3").html("编辑开票");
    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
    ajax("/Member360/GetInvoiceEditInfo", { tid: v }, function (data) {
        if (data != null) {
            console.log(data);
            $("#txtStore").attr("disabled", false);
            $("#txtCompany").attr("disabled", false);
            $("#txtInvoice").attr("readonly", false);
            $("#drpStore2").attr("disabled", false);
            $("#drpMemType").attr("disabled", false);
            $("#drpInvoiceType").attr("disabled", false);
            $("#txtCorpName").attr("readonly", false);
            $("#txtIdentyNo").attr("readonly", false);
            $("#txtCreditCode").attr("readonly", false);
            $("#txtTelephone").attr("readonly", false);
            $("#txtAddress").attr("readonly", false);
            $("#txtBank").attr("readonly", false);
            $("#txtBankCode").attr("readonly", false);
            $("#btnAddSave").show();

            var item = data.Obj[0];

            $("#txtInvoice").val(item.AmountInvoice);
            if (item.InvoiceDrawer == "门店") {
                $("#drpStore2").val(item.StoreCodeInovice);
                $("#txtStore").attr("checked", true);
                $("#txtCompany").attr("checked", false);
            }
            else {
                $("#drpStore2").val(item.StoreCode2Inovice);
                $("#txtStore").attr("checked", false);
                $("#txtCompany").attr("checked", true);
            }
            $("#drpMemType").val(item.Memtype).change();
            $("#drpInvoiceType").val(item.InvoiceType);
            $("#txtCorpName").val(item.CorpName);

            $("#txtIdentyNo").val(item.IdentityNo);
            $("#txtCreditCode").val(item.CreditCode);
            $("#txtTelephone").val(item.CorpMobile);
            $("#txtAddress").val(item.CorpAddress);
            $("#txtBank").val(item.DepositBank);
            $("#txtBankCode").val(item.BankAccount);


        }
    });
}

function Check(v) {
    clearData();
    $("#txtInvoiceId").val(v);
    if ($("#txtStatus").val() == "0") {
        $.dialog("会员状态为停用，不允许编辑开票");
        return;
    }
    $("#addStore_dialog .heading h3").html("查看开票");
    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
    ajax("/Member360/GetInvoiceEditInfo", { tid: v }, function (data) {
        if (data != null) {
            var item = data.Obj[0];

            $("#txtInvoice").val(item.AmountInvoice);
            $("#drpStore2").val(item.StoreCodeInovice);
            $("#drpMemType").val(item.Memtype).change();
            $("#drpInvoiceType").val(item.InvoiceType);
            $("#txtCorpName").val(item.CorpName);
            $("#txtIdentyNo").val(item.IdentityNo);
            $("#txtCreditCode").val(item.CreditCode);
            $("#txtTelephone").val(item.CorpMobile);
            $("#txtAddress").val(item.CorpAddress);
            $("#txtBank").val(item.DepositBank);
            $("#txtBankCode").val(item.BankAccount);



            $("#txtInvoice").attr("readonly", true);
            $("#drpStore2").attr("disabled", true);
            $("#drpMemType").attr("disabled", true);
            $("#drpInvoiceType").attr("disabled", true);
            $("#txtCorpName").attr("readonly", true);
            $("#txtIdentyNo").attr("readonly", true);
            $("#txtCreditCode").attr("readonly", true);
            $("#txtTelephone").attr("readonly", true);
            $("#txtAddress").attr("readonly", true);
            $("#txtBank").attr("readonly", true);
            $("#txtBankCode").attr("readonly", true);
            $("#txtStore").attr("disabled", true);
            $("#txtCompany").attr("disabled", true);
            $("#btnAddSave").hide();
        }
    });
}


var tableMemberInfo;
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
    $(".divSearch").show();
    $("#txtMemId").val(id);
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);

            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevelText == null ? "v1" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].CustomerStatus == 1 ? "正常" : "停用");
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);
            $("#stgValidValue3").text(res.Obj[0].GoldTotal);
        }
    })
    //加载账户积分现金信息
    //loadCashPoint(id);
    //加载开票金额和未开票金额
    loadInvoiceInfo(id);
    LoadTrade();

}

//加载账户积分现金信息
function loadCashPoint(id) {
    ajax("/Member360/GetMemIsBackAccountInfo", { mid: id }, function (data) {
        $("#stgValidValue1").text(0);
        $("#stgValidValue2").text(0);
        if (data.length > 0) {
            //$("#stgValidValue1").text(data[0].Value2);
            $("#stgValidValue3").text(data[0].Total);
            //$("#stgValidValue2").text(data[0].NoBackAccount == null ? "0" : data[0].NoBackAccount);
            //$("#txtCash").val(data[0].Total);
            //total = data[0].Total;
        }
    });
}

function loadInvoiceInfo(id) {
    ajax("/Member360/GetInvoiceTotal", { mid: id }, function (data) {
        $("#stgValidValue1").text(data.NoInvoiceAct);
        $("#stgValidValue2").text(data.InvoiceAct);
    });
}

//加载门店
function LoadStoreList() {
    ajax('/Member360/GetStoreListByGroupID', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drpStore2').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStore2').append(opt);
        }
    });
}

//新建条目
function add() {
    clearData();
    $("#addStore_dialog .heading h3").html("新建开票");
    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addStore_dialog",
        inline: true
    });
}
function clearData() {
    $("#txtInvoiceId").val('');
    $("#txtInvoice").val('');
    $("#drpStore2").val('');
    $("#drpMemType").val('0').change();
    $("#drpInvoiceType").val('');
    $("#txtCorpName").val('');
    $("#txtIdentyNo").val('');
    $("#txtCreditCode").val('');
    $("#txtTelephone").val('');
    $("#txtAddress").val('');
    $("#txtBank").val('');//优惠原因
    $("#txtBankCode").val('');//服务顾问
    $("#txtStore").attr("disabled", false);
    $("#txtCompany").attr("disabled", false);
    $("#txtInvoice").attr("readonly", false);
    $("#drpStore2").attr("disabled", false);
    $("#drpMemType").attr("disabled", false);
    $("#drpInvoiceType").attr("disabled", false);
    $("#txtCorpName").attr("readonly", false);
    $("#txtIdentyNo").attr("readonly", false);
    $("#txtCreditCode").attr("readonly", false);
    $("#txtTelephone").attr("readonly", false);
    $("#txtAddress").attr("readonly", false);
    $("#txtBank").attr("readonly", false);
    $("#txtBankCode").attr("readonly", false);
    $("#btnAddSave").show();
}