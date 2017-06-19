var tableMemberInfo,tableVehcileInfo;
$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();
    LoadStoreList();
    //搜索
    $("#btnSearch").click(function () {

        loadInvoiceList();

    })
    $("#Status").val('1');
})
//加载会员信息
function loadInvoiceList() {
    if (!tableMemberInfo) {
        tableMemberInfo = $('#tableMemberInfo').dataTable({
            sAjaxSource: '/Member360/GetInvoice_Finance',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 4,
            aaSorting:  [[ 4, "desc" ]],
            aoColumns: [
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerName", title: "会员名称", sortable: false },

                { data: "CustomerMobile", title: "手机", sortable: false },
                //{ data: "PlateNumVehicle", title: "车牌号", sortable: false },
                {
                    data: "AmountInvoice", title: "开票金额", sortable: false
                },
                {
                    data: "InvoiceDate", title: "开票日期", sortable: false
                },
                {
                    data: "InvoiceOrderNo", title: "发票号", sortable: false
                },

                {
                    data: "StatusInvoice", title: "审批状态", sortable: false, render: function (obj) {
                        if (obj == 1) return "未审批"; else return "已审批";
                    }
                },
                //{
                //    data: "InvoiceDrawer", title: "开票方", sortable: false
                //},
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        if (obj.StatusInvoice == "1")
                            return "<button class=\"btn\" onclick=\"vehcile('" + obj.MemberID + "')\">查看车辆</button><button class=\"btn btn-danger\" onclick=\"detail(" + obj.TradeID + ")\">审批</button><button class=\"btn \" onclick=\"edit(" + obj.TradeID +",'"+obj.MemberID+ "')\">编辑</button>";
                        else
                            return "<button class=\"btn\" onclick=\"vehcile('" + obj.MemberID + "')\">查看车辆</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'startdate', value: $("#txtStartDate").val() });
                d.push({ name: 'enddate', value: $("#txtEndDate").val() });
                d.push({ name: 'status', value: $("#Status").val() });
            }
        });
    }
    else {
        tableMemberInfo.fnDraw();
    }
}
//审批
function detail(id) {

    ajax('/Member360/UpdateInvoiceStatus', { tid: id }, function (res) {
        
        $.dialog(res.MSG);
        loadInvoiceList();
       

    })
}
//查看车辆
function vehcile(id) {

    $("#txtHdnMid").val(id);
    loadVehcileList();

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_Vehcile",
        inline: true
    });
}

//编辑
function edit(v,m) {
    clearData();
    $("#txtInvoiceId").val(v);
    $("#txtMemId").val(m);
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
            $("#InvoiceNo").val(data.Obj[0].InvoiceOrderNo);
            $("#txtStore").attr("disabled", true);
            $("#txtCompany").attr("disabled", true);
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




function loadVehcileList() {
    if (!tableVehcileInfo) {
        tableVehcileInfo = $('#tableVehcileInfo').dataTable({
            sAjaxSource: '/Member360/GetMemberCarInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aoColumns: [
                { data: "PlateNumVehicle", title: "车牌号", sortable: false },
                { data: "VINVehicle", title: "车架号", sortable: false },
                {
                    data: "BrandName", title: "品牌", sortable: false
                },
                {
                    data: "SeriesName", title: "车系", sortable: false
                },
                {
                    data: "LevelName", title: "车型", sortable: false
                },
                {
                    data: "ColorName", title: "车辆颜色", sortable: false
                },
                { data: "TrimNameBase", title: "内饰", sortable: false },
                { data: "DriveDistinct", title: "行驶里程", sortable: false },
                {
                    data: "BuyDate", title: "购车时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtHdnMid").val() });
            }
        });
    }
    else {
        tableVehcileInfo.fnDraw();
    }
}
function clearData() {
    $("#InvoiceNo").val('');
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

$("#frmAddStore").submit(function (e) {
    e.preventDefault();
    if ($("#txtInvoice").val() <= 0) {
        $.dialog("开票金额不能为0");
        return;
    }
    if (DataValidatorAdd.form()) {
        var invoice = {
            InvoiceNo: $("#InvoiceNo").val(),
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
        //开票金额不能大于储值金额 验证
        //if ($("#stgValidValue1").text() - $("#txtInvoice").val() < 0) {
        //    $.dialog("开票金额不能超过未开票金额");
        //    return;
        //}
        ajax("/Member360/EditInvoiceData", { invoice: invoice }, function (res) {
            if (res.IsPass) {
                $.colorbox.close();
                loadInvoiceList();
                $.dialog(res.MSG);
            } else $.dialog(res.MSG);
        });
    }
})

//新增时验证数据
var DataValidatorAdd = $("#frmAddStore").validate({
    //onSubmit: false,
    rules: {
        InvoiceNo:{
            isOnlyLN:true,
        },
        txtInvoice: {
            required: true,
        },
        txtCorpName: {
            maxlength: 50,
            isSb: true,
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
            isMobileNo: true,
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


