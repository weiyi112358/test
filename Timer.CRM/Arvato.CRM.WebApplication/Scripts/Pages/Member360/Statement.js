var dt_StoreData, tableMemberInfo;
$(function () {
    $("#txtDate").datepicker();//{ startDate: '0' }

    $("#txtStartDate,#txtEndDate").datepicker();
    //加载业务类型
    LoadBusinessType();
    //加载门店
    loadStore();

    

    $('#availableCoupon').chosen();


    //loadSalesman();
    //小类联动
    $("#drpBusiType").change(function () {
        LoadBusinessChildType($("#drpBusiType").val());
    })

    //小类联动
    $("#drpBusinessType").change(function () {
        LoadBusinessChildTypeDiv($("#drpBusinessType").val());
    })

    //LoadBusinessChildType();
    //LoadBusinessChildTypeDiv();
    LoadPayType();
    //加载业务类型
    //LoadChannelType();

    $("#btnSearchTrade").click(function () {
        if ($("#txtMemId").val() == "") {
            $.dialog("请先选择会员");
            return;
        }
        if ($("#spnCardStat").text() == '停用') {
            $.dialog("该会员已停用,请联系管理员！");
            return;
        }
        LoadTrade();
    })
    $("#btnInActive").click(function () {
        if ($("#txtPassword").val() == "") {
            $.dialog("支付密码不能为空");
            return;
        }
        ajax("/Member360/InActiveItemById", { itemId: $("#txtHideTrd").val(), password: $("#txtPassword").val() }, function (res) {
            if (res.IsPass) {
                $.colorbox.close();
                $.dialog(res.MSG);
                var start = dt_StoreData.fnSettings()._iDisplayStart;
                var length = dt_StoreData.fnSettings()._iDisplayLength;
                dt_StoreData.fnPageChange(start / length, true);
            } else { $.dialog(res.MSG); }
        });
    })

    $("#btnCoupon").click(function () {
        if (DataValidatorAdd.form()) {
            var trade = {
                TradeTypeA: $("#drpBusinessType").val(),
                TradeTypeB: $("#drpBusinessChildType").val(),
                Vehicle: $("#drpVehicle").val(),
                MemberId: $("#txtMemId").val(),
                StoreCode: $("#txtMemId").val(),
                TradeCode: $("#drpBusinessNo").val(),
                ListDateSales: $("#txtDate").val(),
                Amount: $("#txtAmount").val(),
                StandardAmountSales: $("#txtPrice").val(),
                NotGoldAmountSales: $("#txtNonGold").val(),
                GoldAmountSales: $("#txtGold").val(),
                Remark: $("#txtRemark").val(),//优惠原因
                Sales: $("#txtSales").val(),//服务顾问
                Created: $("#txtSalesName").val(),//开单人
                StoreCode: $("#drpStore").val(),
                AvailableCoupon: $("#drpRolePages").val(),

            }
            if ($("#txtTradeId").val() == "") {
                ajax("/Member360/AddStatementData", { trade: trade }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        LoadTrade();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {
                ajax("/Member360/EditStatementData", { trade: trade, mid: $("#txtTradeId").val(), isFinance: false }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        LoadTrade();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
        
    })


    //新增条目
    $("#frmAddStore").submit(function (e) {
        e.preventDefault();

        if ($("#drpVehicle").val() == "") {
            $("#drpVehicle").nextAll("span.error-block").html("请至车辆管理中添加车辆");
            return;
        }
        else
            $("#drpVehicle").nextAll("span.error-block").html("");

        if ($("#drpBusinessType").val() == "") {
            $("#drpBusinessType").nextAll("span.error-block").html("请选择业务类型");
            return;
        }
        else
            $("#drpBusinessType").nextAll("span.error-block").html("");

        if ($("#drpBusinessChildType").val() == "") {
            $("#drpBusinessChildType").nextAll("span.error-block").html("请选择子业务类型");
            return;
        }
        if ($("#drpStore").val() == '') {
            $("#drpStore").nextAll("span.error-block").html("请选择门店");
            return;
        }
        else
            $("#drpBusinessChildType").nextAll("span.error-block").html("");

        if (($("#txtAmount").val() - $("#txtPrice").val()) > 0) {
            $("#txtPrice").nextAll("span.error-block").html("应付金额不能超过单据总金额");
            return;
        }
        
        if (($("#txtAmount").val() - $("#txtPrice").val()) < 0) {
            if ($("#txtRemark").val() == '') {
                $("#txtRemark").nextAll("span.error-block").html("应付金额小于单据总金额，须填写优惠原因");
                return;
            }
        } else {
            $("#txtRemark").val('');
        }
        //var couponList = $("#availableCoupon").val().join();
       
       

        if (DataValidatorAdd.form()) {
            var trade = {
                TradeTypeA: $("#drpBusinessType").val(),
                TradeTypeB: $("#drpBusinessChildType").val(),
                Vehicle: $("#drpVehicle").val(),
                MemberId: $("#txtMemId").val(),
                StoreCode: $("#txtMemId").val(),
                TradeCode: $("#drpBusinessNo").val(),
                ListDateSales: $("#txtDate").val(),
                Amount: $("#txtAmount").val(),
                StandardAmountSales: $("#txtPrice").val(),
                Remark: $("#txtRemark").val(),//优惠原因
                Sales: $("#txtSales").val(),//服务顾问
                Created: $("#txtSalesName").val(),//开单人
                StoreCode: $("#drpStore").val(),
                AvailableCoupon: $("#drpRolePages").val(),
                NotGoldAmountSales: $("#txtNonGold").val(),
                GoldAmountSales: $("#txtGold").val(),
                
            }
            if ($("#txtTradeId").val() == "") {
                ajax("/Member360/AddStatementData", { trade: trade }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        LoadTrade();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {
                ajax("/Member360/EditStatementData", { trade: trade, mid: $("#txtTradeId").val(),isFinance:false }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        LoadTrade();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }

    })

    //业务类型变化
    //$("#drpBusiType").change(function () {
    //    var a = $("#drpBusiType").val();
    //    //加载品牌
    //    LoadBusinessChildType(a);
    //})
    ////业务类型变化--弹窗
    //$("#drpBusinessType").change(function () {
    //    var b = $("#drpBusinessType").val();
    //    //加载品牌
    //    LoadBusinessChildTypeDiv(b);
    //})


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
        //$.colorbox.resize();


        $("#txtPrice").change(function () {
            txtPriceOnchange($("#txtPrice").val());
        });

        $("#txtAmount").change(function () {
            txtAmountOnchange($("#txtAmount").val());
        });

        $("#txtGold").change(function () {
            txtGoldOnchange($("#txtGold").val());
        });

        $("#txtNonGold").change(function () {
            txtNonGoldOnchange($("#txtNonGold").val());
        });

        $("#txtHour").change(function () {
            txtHourOnchange($("#txtHour").val());
        });

        $("#txtParts").change(function () {
            txtPartsOnchange($("#txtParts").val());
        });

    })
})
//新增时验证数据
var DataValidatorAdd = $("#frmAddStore").validate({
    //onSubmit: false,
    rules: {
        drpBusinessNo: {
            required: true,
            maxlength: 50,
        },
        txtDate: {
            required: true,
        },

        txtAmount: {
            required: true,
            min:0,

        },
        txtPrice: {
            required: true,
            maxlength: 10,
            number: true,
            min:0,

        },
        //drpStore: {
        //    required: true,
        //},
        txtSales: {
            required: true,
        }

    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
function LoadTrade() {
    if (!dt_StoreData) {
        //加载数据表格
        dt_StoreData = $('#dt_StoreData').dataTable({
            sAjaxSource: '/Member360/GetStatementData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            order: [[4, "desc"]],
            aoColumns: [
            { data: 'TradeTypeA', title: "业务类型", sortable: true, },
            { data: 'TradeTypeB', title: "子业务类型", sortable: false, },
            { data: 'StoreName', title: "门店", sortable: false, sWidth: "100", },
            { data: 'MemberCardNo', title: "会员卡号", sortable: false, },
            { data: 'TradeCode', title: "业务单号", sortable: false, },
            {
                data: 'ListDateSales', title: "发生日期", sortable: false, render: function (obj) {
                    if (obj.length > 10) {
                        return obj.substring(0, 10);
                    } else {
                        return "";
                    }
                }
            },
            { data: 'StandardAmountSales', title: "单据总金额", sortable: false, },
            //{ data: 'GoldAmountSales', title: "金币金额", sortable: false, },
            { data: 'Amount', title: "应付金额", sortable: false, },
            //{ data: 'NotGoldAmountSales', title: "非金币金额", sortable: false, },
            //{ data: 'HoursAmountSales', title: "工时金额", sortable: false, },
            //{ data: 'PartsAmountSales', title: "配件金额", sortable: false, },
            { data: 'StatusSalesText', title: "审批状态", sortable: false, },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.StatusSales == "1")
                        return "<button class=\"btn btn-default\" onclick=\"Edit(" + obj.TradeID + ")\">编辑</button>&nbsp;&nbsp;<button class=\"btn btn-default\" onclick=\"editCoupon(" + obj.TradeID + ")\">优惠券</button>";
                    else
                        return "";

                }
            }

            ],
            fnFixData: function (d) {
                d.push({ name: 'busiType', value: $("#drpBusiType").val() });
                d.push({ name: 'busiChild', value: $("#drpBusiChildType").val() });
                d.push({ name: 'mid', value: $("#txtMemId").val() });
                d.push({ name: 'startDate', value: $("#txtStartDate").val() });
                d.push({ name: 'endDate', value: $("#txtEndDate").val() });
            }
        });
    } else {
        dt_StoreData.fnDraw();
    }
}
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
                { data: "MemberCardNo", title: "会员卡号", sortable: false, sWidth: "25%" },

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
    $("#divsearch").show();
    $("#txtMemId").val(id);
    ajax('/Member360/GetMemberInfoByMid', { mid: id }, function (res) {
        if (res.IsPass) {
            var mid = res.Obj[0].MemberID;
            memberId = mid;
            $("#txtStatus").val(res.Obj[0].CustomerStatus);
            //给页面上详细信息栏赋值
            $("#spnName").text(res.Obj[0].CustomerName);
            $("#txtMemCode").val(res.Obj[0].MemberCode);
            $("#spnGender").text(res.Obj[0].Gender == null ? "" : res.Obj[0].Gender);
            $("#spnLevel").text(res.Obj[0].CustomerLevel == null ? "" : res.Obj[0].CustomerLevelText);
            $("#spnCardNo").text(res.Obj[0].MemberCardNo == null ? "" : res.Obj[0].MemberCardNo);
            $("#spnCardStat").text(res.Obj[0].CustomerStatus == 1 ? "正常" : "停用");
            $("#spnMobile").text(res.Obj[0].CustomerMobile == null ? "" : res.Obj[0].CustomerMobile);
            $("#stgValidValue2").text(res.Obj[0].MaxIntergral == null ? "" : res.Obj[0].MaxIntergral);
            

        }
    })
    //加载账户积分现金信息
    loadCashPoint(id);

    
    
    

    $("#btnadd").bind("click", function () { add(id) } );

    

}

//加载账户积分现金信息
function loadCashPoint(id) {
    ajaxSync("/Member360/GetMemIsBackAccountInfo", { mid: id }, function (data) {
        $("#stgValidValue1").text(0);

        if (data.length > 0) {
            $("#stgValidValue1").text(data[0].Value2);
            $("#stgValidValue3").text(data[0].Value1);
            //$("#stgValidValue2").text(data[0].NoBackAccount == null ? "0" : data[0].NoBackAccount);
            //$("#txtCash").val(data[0].Total);
            //total = data[0].Total;
        }
    });
}


//审批
function Inactive(id) {
    $.dialog("确认通过审批吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        $("#txtPassword").val("");
        $("#txtHideTrd").val(id);
        //弹窗输入支付密码
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            overlayClose: false,
            opacity: '0.3',
            //title: '素材',
            href: "#password_dialog",
            inline: true
        });
    })
}

function Edit(v) {
    clearData();
    LoadVehicleInfo();
    loadSalesman();
    
    //refreshMultiSelect(v);
    
    $("#txtTradeId").val(v);
    if ($("#txtStatus").val() == "0") {
        $.dialog("会员状态为停用，不允许编辑结算");
        return;
    }
    $("#addStore_dialog .heading h3").html("编辑结算");
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
    ajax("/Member360/GetStatementEditInfo", { mid: v }, function (data) {
        if (data != null) {
            console.log(data);
            var item = data.Obj[0][0];
            loadCashPoint(item.MemberID);
            var gold, nonGold, couponGold;
            couponGold = item.CouponGold == null ? 0 : item.CouponGold
            if (item.Amount - couponGold <= 0) {
                gold = 0;
                nonGold = 0;
            }
            else {
                if (parseInt($("#stgValidValue3").text()) - (item.Amount - couponGold) <= 0) {
                    gold = parseInt($("#stgValidValue3").text());
                    nonGold = item.Amount - couponGold - gold;
                }
                else {
                    gold = item.Amount - couponGold;
                    nonGold = 0;
                }
            }
            $("#drpBusinessType").val(item.TradeTypeA);
            LoadBusinessChildTypeDiv($("#drpBusinessType").val());
            $("#drpBusinessChildType").val(item.TradeTypeB),
            //$("#dropPayType").val(item.TradeChannelCode),
            $("#txtMemId").val(item.MemberID),
            $("#drpBusinessNo").val(item.TradeCode),
            $("#txtDate").val(item.ListDateSales.substring(0, 10)),
            $("#txtAmount").val(item.Amount),
            $("#txtPrice").val(item.StandardAmountSales),
            $("#txtNonGold").val(nonGold),
            $("#txtGold").val(gold),
            $("#txtHour").val(item.HoursAmountSales),
            $("#txtParts").val(item.PartsAmountSales),
            $("#drpVehicle").val(item.VehicleId);
            $("#txtRemark").val(item.Reason);//优惠原因
            $("#txtSales").val(item.Consultant);//服务顾问
            $("#drpStore").val(item.StoreCodeSales);
            $("#dropPayType").val(item.PayMentTypeSales);


            $("#couponGold").val(couponGold);

            $("#drpRolePages").val(item.CouponIds).multiSelect("refresh");//.init();
            
            

            
        }
    });
}




//新建条目
function add(id) {
    $("#txtTradeId").val('');
    clearData();
    LoadVehicleInfo();
    loadSalesman();
    loadStatementStatus(id)
    if ($("#txtStatementStatus").val() == 1) {
        $.dialog("该会员仍有单未结算，请先完成结算后再新建")
        return;
    }
        
    //LoadAvailableCoupon(0);
    //console.log($("#drpRolePages")[0]);
    
    initPagesMultiSelect();
    //$("#drpRolePages").val("").multiSelect("refresh");//.init();
    
    if ($("#txtStatus").val() == "0") {
        $.dialog("会员状态为停用，不允许新建结算");
        return;
    }
    
    

    $("#addStore_dialog .heading h3").html("新建结算");
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

//清空数据
function clearData() {

    $("#drpBusinessType").val('');
    $("#drpBusinessChildType").val('');
    //$("#drpChannel").val('');
    $("#drpBusinessNo").val('');
    $("#drpVehicle").val('');
    $("#txtPrice").val('');
    $("#txtAmount").val('');
    $("#txtNonGold").val('');
    $("#txtGold").val('');
    $("#txtHour").val('');
    $("#txtParts").val('');
    $("#drpStore").val('');
    $("#txtRemark").val('');//优惠原因
    //$("#txtSales").val('');//服务顾问
    $('.error-block').html('');
}


//加载业务类型
function LoadBusinessType() {
    ajax('/BaseData/GetTypeAList', {}, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].TypeACodeBase + '>' + res[i].TypeANameBase + '</option>';
            }
            $('#drpBusiType').append(opt);
            $('#drpBusinessType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpBusiType').append(opt);
            $('#drpBusinessType').append(opt);
        }
    });
}




//加载子业务类型
function LoadBusinessChildType(typeA) {
    ajaxSync('/BaseData/GetTypeBList', { typeA: typeA }, function (res) {
        $('#drpBusiChildType').html("");
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].TypeBCodeBase + '>' + res[i].TypeBNameBase + '</option>';
            }
            $('#drpBusiChildType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpBusiChildType').append(opt);
        }
    });
}
//加载子业务类型---弹窗
function LoadBusinessChildTypeDiv(typeA) {
    ajaxSync('/BaseData/GetTypeBList', { typeA: typeA }, function (res) {
        $('#drpBusinessChildType').html("");
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].TypeBCodeBase + '>' + res[i].TypeBNameBase + '</option>';
            }
            $('#drpBusinessChildType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpBusinessChildType').append(opt);
        }
    });
}

//加载支付类型
function LoadPayType() {
    ajaxSync('/BaseData/GetOptionDataList', { optType: "PayType" }, function (res) {
        $('#dropPayType').html("");
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#dropPayType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#dropPayType').append(opt);
        }
    });
}

//加载车辆信息
function LoadVehicleInfo() {
    $('#drpVehicle').html("");
    ajax('/BaseData/GetMemVehicle', { mid: $("#txtMemId").val() }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                if (res[i].PlateNumVehicle == null) {
                    opt += '<option value=' + res[i].MemberSubExtID + '>' + '无车牌号码' + '</option>';
                }
                else {
                    opt += '<option value=' + res[i].MemberSubExtID + '>' + res[i].PlateNumVehicle + '</option>';
                }
            }
            $('#drpVehicle').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpVehicle').append(opt);
        }
    });
}
//加载渠道类型
function LoadChannelType() {
    ajax('/BaseData/GetOptionDataList', { optType: "Channel" }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpChannelType').append(opt);
            $('#drpChannel').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpChannelType').append(opt);
            $('#drpChannel').append(opt);
        }
    });
}

function txtAmountOnchange(amount) {
    if ($("#txtPrice").nextAll("span.error-block").html() != "")
        $("#txtPrice").nextAll("span.error-block").html("");


}

function txtPriceOnchange(amount) {
    if ($("#txtPrice").nextAll("span.error-block").html() != "")
        $("#txtPrice").nextAll("span.error-block").html("");
    //$("#txtNonGold").val("");
    //if (parseInt(amount) > parseInt($("#txtPrice").val()))
    //{
    //    $("#txtAmount").nextAll("span.error-block").html("应付金额不能超过单据总金额");
    //}

    //if ((amount - $("#stgValidValue2").text()) < "0" && (amount - $("#stgValidValue3").text()) < "0")
    //    $("#txtGold").val(amount);
    //else {
    //    if (($("#stgValidValue2").text() - $("#stgValidValue3").text()) > "0") {
    //        $("#txtGold").val($("#stgValidValue3").text());
    //        $("#txtNonGold").val(amount - $("#stgValidValue3").text());
    //    }
    //    else {
    //        $("#txtGold").val($("#stgValidValue2").text());
    //        $("#txtNonGold").val(amount - $("#stgValidValue2").text());
    //    }
    //}
    //$("#txtHour").val("");
    //$("#txtParts").val("");

}

function txtNonGoldOnchange(amount) {
    if ($("#txtPrice").val() != "" && $("#txtPrice").val() != null && $("#txtNonGold").val() != "" && $("#txtNonGold").val() != null) {
        var a = $("#txtPrice").val() - $("#txtNonGold").val();;
        $("#txtGold").val(a);
    }
}

//function txtPartsOnchange(amount) {
//    if ($("#txtPrice").val() != "" && $("#txtPrice").val() != null && $("#txtParts").val() != "" && $("#txtParts").val() != null) {
//        var a = $("#txtPrice").val() - $("#txtParts").val();;
//        $("#txtHour").val(a);
//    }
//}

//function txtHourOnchange(amount) {
//    if ($("#txtPrice").val() != "" && $("#txtPrice").val() != null && $("#txtHour").val() != "" && $("#txtHour").val() != null) {
//        var a = $("#txtPrice").val() - $("#txtHour").val();;
//        $("#txtParts").val(a);
//    }
//}


function txtGoldOnchange(amount) {
    if ($("#txtGold").nextAll("span.error-block").html() != "")
        $("#txtGold").nextAll("span.error-block").html("");
    if ((amount - $("#stgValidValue3").text()) > "0" || (amount - $("#stgValidValue2").text()) > "0") {
        if (($("#stgValidValue3").text() - $("#stgValidValue2").text()) > "0")
            $("#txtGold").nextAll("span.error-block").html("最大可用金币不能超过" + $("#stgValidValue2").text());
        else
            $("#txtGold").nextAll("span.error-block").html("最大可用金币不能超过" + $("#stgValidValue3").text());
        $("#txtGold").val('');
    }

    if ($("#txtPrice").val() != "" && $("#txtPrice").val() != null && $("#txtGold").val() != "" && $("#txtGold").val() != null) {
        var a = $("#txtPrice").val() - $("#txtGold").val();;
        $("#txtNonGold").val(a);
    }
}

Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function loadStore() {
    ajax('/Member360/GetStoreListByGroupID', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].StoreCode + '>' + res[i].StoreName + '</option>';
            }
            $('#drpStore').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStore').append(opt);
        }
    });
}

function loadSalesman() {
    ajax("/Member360/GetAuthName", null, function (res) {
        $('#txtSalesName').val(res.UserName);
        $('#txtSales').val(res.LoginName);
    });
}

//加载可用优惠券
function LoadAvailableCoupon(id) {
    ajax('/Member360/GetAvailableCoupon', {tradeId:id}, function (res) {
        if (res.length > 0) {
            var opt = "";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].CouponCode + '>' + res[i].CouponName + '</option>';
            }
            $('#drpRolePages').val('');
            $('#drpRolePages').html(opt);
            //$("#drpRolePages").trigger("liszt:updated");
            
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpRolePages').val('');
            $('#drpRolePages').html(opt);
            
        }

        $("#drpRolePages").val("").multiSelect("refresh");//.init();
        
    });
}

//加载multi-select所有页面
function initPagesMultiSelect() {
    $("#drpRolePages").multiSelect({
        //cssClass: "height: 300px",
        selectableHeader: "<div class='search-header'>可使用优惠券</div>",
        selectionHeader: "<div class='search-selected'>结算单已使用优惠券</div>",
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

//function refreshMultiSelect(v) {
//    $("#drpRolePages").multiSelect({
//        afterSelect: function (e) {
//            Edit(v);
//        },
//        afterDeselect: function (e) {
//            Edit(v);
//        }

//    })

//}


//编辑优惠券
function editCoupon(v) {
    LoadAvailableCoupon(v);
    initPagesMultiSelect();
    $("#txtTradeId").val(v);
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#coupon_dialog",
        inline: true
    });
    ajax("/Member360/GetStatementEditInfo", { mid: v }, function (data) {
        if (data != null) {
            console.log(data);
            var item = data.Obj[0][0];
            $("#drpBusinessType").val(item.TradeTypeA);
            LoadBusinessChildTypeDiv($("#drpBusinessType").val());
            $("#drpBusinessChildType").val(item.TradeTypeB),
            //$("#dropPayType").val(item.TradeChannelCode),
            $("#txtMemId").val(item.MemberID),
            $("#drpBusinessNo").val(item.TradeCode),
            $("#txtDate").val(item.ListDateSales.substring(0, 10)),
            $("#txtAmount").val(item.Amount),
            $("#txtPrice").val(item.StandardAmountSales),
            $("#txtNonGold").val(item.NotGoldAmountSales),
            $("#txtGold").val(item.GoldAmountSales),
            $("#txtHour").val(item.HoursAmountSales),
            $("#txtParts").val(item.PartsAmountSales),
            $("#drpVehicle").val(item.VehicleId);
            $("#txtRemark").val(item.Reason);//优惠原因
            $("#txtSales").val(item.Consultant);//服务顾问
            $("#drpStore").val(item.StoreCodeSales);
            $("#dropPayType").val(item.PayMentTypeSales);


            $("#couponGold").val(item.CouponGold);

            $("#drpRolePages").val(item.CouponIds).multiSelect("refresh");//.init();




        }
    });


}

function loadStatementStatus(mid)
{
    ajaxSync('/Member360/GetStatementStatus', { mid: mid }, function (res) {
        if (res != null)
        {
            $("#txtStatementStatus").val(res.Obj[0]);
        }

    });



}

