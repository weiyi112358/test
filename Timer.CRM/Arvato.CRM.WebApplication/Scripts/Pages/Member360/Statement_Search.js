var dt_StoreData, tableMemberInfo;
$(function () {
    //if (navigator.userAgent.toLowerCase().indexOf("chrome") != -1) {
    //    var inputers = document.getElementsByTagName("input");
    //    for (var i = 0; i < inputers.length; i++) {
    //        if ((inputers[i].type !== "submit") && (inputers[i].type !== "password")) {
    //            inputers[i].disabled = true;
    //        }
    //    }
    //    setTimeout(function () {
    //        for (var i = 0; i < inputers.length; i++) {
    //            if (inputers[i].type !== "submit") {
    //                inputers[i].disabled = false;
    //            }
    //        }
    //    }, 100)
    //}

    $('#dt_StoreData').resize(function () {
        $("#dt_StoreData").css({ "width": "100%" })
    })


    $("#txtDate").datepicker();//{ startDate: '0' }
    $("#txtStartDate,#txtEndDate").datepicker();
    //加载业务类型
    LoadBusinessType();
    loadStore();
    LoadDepartmentByUser()
    //小类联动
    $("#drpBusiType").change(function () {
        LoadBusinessChildType($("#drpBusiType").val());
    })

    //小类联动
    $("#drpBusinessType").change(function () {
        LoadBusinessChildTypeDiv($("#drpBusinessType").val());
    })
    //导出
    $("#btnExport").bind("click", function () {
        $("#exportForm")[0].action = "/Member360/ExportStatementSearch";
        $("#exportForm #exprCardNo").val($("#txtCardNo").val());
        $("#exportForm #exprMemName").val($("#txtName").val());
        $("#exportForm #exprMobile").val($("#txtMobile").val());
        $("#exportForm #exprVinNo").val($("#txtVinNo").val());
        $("#exportForm #exprVehNo").val($("#txtVehNo").val());
        $("#exportForm #exprStatus").val($("#Status").val());
        $("#exportForm #exprBusitype").val($("#drpBusiType").val());
        $("#exportForm #exprChildType").val($("#drpBusiChildType").val());
        $("#exportForm #exprDtStart").val($("#txtStartDate").val());
        $("#exportForm #exprDtEnd").val($("#txtEndDate").val());
        $("#exportForm")[0].submit();
    });
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
    //新增条目
    $("#frmAddStore").submit(function (e) {
        e.preventDefault();

        if ($("#drpVehicle").val() == "") {
            $("#drpVehicle").nextAll("span.error-block").html("请至车辆管理中添加车辆");
            return;
        }

        //if ($("#txtHour").val() == "") {
        //    $("#txtHour").nextAll("span.error-block").html("请填写工时金额");
        //    return;
        //}

        //if ($("#txtParts").val() == "") {
        //    $("#txtParts").nextAll("span.error-block").html("请填写配件金额");
        //    return;
        //}
        if (($("#txtAmount").val() - $("#txtPrice").val()) > 0) {
            $("#txtAmount").nextAll("span.error-block").html("应付金额不能超过单据总金额");
            return;
        }

        if (DataValidatorAdd.form()) {
            var trade = {
                TradeTypeA: $("#drpBusinessType").val(),
                TradeTypeB: $("#drpBusinessChildType").val(),
                Vehicle: $("#drpVehicle").val(),
                MemberId: $("#txtMemId").val(),
                StoreCode: $("#drpStore").val(),
                TradeCode: $("#drpBusinessNo").val(),
                ListDateSales: $("#txtDate").val(),
                Amount: $("#txtAmount").val(),
                StandardAmountSales: $("#txtPrice").val(),
                NotGoldAmountSales: $("#txtNonGold").val(),
                GoldAmountSales: $("#txtGold").val(),
                PayType: $("#dropPayType").val(),
                //PartsAmountSales: $("#txtParts").val(),
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
                ajax("/Member360/EditStatementData", { trade: trade, mid: $("#txtTradeId").val() }, function (res) {
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
        //if ($("#txtNo").val() == '' && $("#txtName").val() == '' && $("#txtMobile").val() == '' && $("#txtStatus").val() == '' && $("#txtStartDate").val() == '' && $("#txtEndDate").val() == ''
        //    && $("#drpBusiType").val() == '' && $("#drpBusiChildType").val() == '') {
        //    $.dialog("至少输入一个条件以供查询");
        //    return;
        //}

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

        LoadTrade();

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
        txtPrice: {
            maxlength: 10,
            number: true,
        },
        txtDate: {
            required: true,
        },

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
            sAjaxSource: '/Member360/GetStatementData_Search',
            sScrollX: "100%",
            sScrollXInner: "100%",
            bScrollCollapse: true,
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            order: [[5, "desc"]],
            aoColumns: [
            { data: 'TradeTypeA', title: "业务类型", sortable: false, },
            { data: 'TradeTypeB', title: "子业务类型", sortable: false, },
            //{ data: 'TradeChannelCode', title: "渠道", sortable: false, },
            { data: 'MemberCardNo', title: "会员卡号", sortable: false, },
            { data: 'PlateNumVehicle', title: "车牌号", sortable: false, },
            { data: 'TradeCode', title: "业务单号", sortable: false, },
            {
                data: 'ListDateSales', title: "发生日期", sortable: true, render: function (obj) {
                    if (obj.length > 10) {
                        return obj.substring(0, 10);
                    } else {
                        return "";
                    }
                }
            },
            { data: 'StandardAmountSales', title: "单据总金额", sortable: false, },
            { data: 'Amount', title: "应付金额", sortable: false, },
            { data: 'GoldAmountSales', title: "金币金额", sortable: false, },

            { data: 'NotGoldAmountSales', title: "非金币金额", sortable: false, },
            //{ data: 'HoursAmountSales', title: "工时金额", sortable: false, },
            //{ data: 'PartsAmountSales', title: "配件金额", sortable: false, },
            { data: 'StatusSalesText', title: "审批状态", sortable: true, },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    if (obj.StatusSales == "1")
                        return "<button class=\"btn btn-default\" onclick=\"Edit(" + obj.TradeID + ")\">查看</button>&nbsp;&nbsp;<button class=\"btn btn-danger\" onclick=\"Delete(" + obj.TradeID + ")\">删除</button>";
                    else
                        return "";

                }
            }

            ],
            fnFixData: function (d) {
                d.push({ name: 'busiType', value: $("#drpBusiType").val() });
                d.push({ name: 'busiChild', value: $("#drpBusiChildType").val() });
                d.push({ name: 'mid', value: $("#txtMemId").val() });
                d.push({ name: 'cardNo', value: $("#txtCardNo").val() });
                d.push({ name: 'name', value: $("#txtName").val() });
                d.push({ name: 'mobile', value: $("#txtMobile").val() });
                d.push({ name: 'status', value: $("#Status").val() });
                d.push({ name: 'startDate', value: $("#txtStartDate").val() });
                d.push({ name: 'endDate', value: $("#txtEndDate").val() });
                d.push({ name: 'vehno', value: $("#txtVehNo").val() });
                d.push({ name: 'vinno', value: $("#txtVinNo").val() });
                d.push({ name: 'department', value: $("#drpDepartment").val() });
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


//加载账户积分现金信息
function loadCashPoint(id) {
    ajax("/Member360/GetMemIsBackAccountInfo", { mid: id }, function (data) {
        $("#stgValidValue1").text(0);

        if (data.length > 0) {
            $("#stgValidValue1").val(data[0].Value2);
            $("#stgValidValue3").val(data[0].Value1);
            //$("#stgValidValue2").text(data[0].NoBackAccount == null ? "0" : data[0].NoBackAccount);
            //$("#txtCash").val(data[0].Total);
            //total = data[0].Total;
        }

        //ajax('/Member360/GetMemberInfoByMid', { mid: $("#txtMemId").val() }, function (res) {
        //    if (res.IsPass) {


        //        $("#stgValidValue2").val(res.Obj[0].MaxIntergral == null ? "" : res.Obj[0].MaxIntergral);
        //        if ($("#txtGold").val() == 0 || $("#txtGold").val() == null) {
        //            txtAmountOnchange($("#txtAmount").val());
        //        }


        //    }
        //});
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

//删除
function Delete(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {

        ajax('/Member360/DeleteItemById', { itemId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_StoreData.fnDraw();
            } else $.dialog(res.MSG);


        });
    })
}


function Edit(v) {
    clearData();

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
            var item = data.Obj[0];
            $("#drpBusinessType").val(item[0].TradeTypeA);
            LoadBusinessChildTypeDiv($("#drpBusinessType").val());
            $("#drpBusinessChildType").val(item[0].TradeTypeB),
            //$("#drpChannel").val(item.TradeChannelCode),
            $("#txtMemId").val(item[0].MemberID),
            $("#drpBusinessNo").val(item[0].TradeCode),
            $("#txtDate").val(item[0].ListDateSales),
            $("#txtAmount").val(item[0].Amount),
            $("#txtPrice").val(item[0].StandardAmountSales),
            $("#txtNonGold").val(item[0].NotGoldAmountSales),
            $("#txtGold").val(item[0].GoldAmountSales),
            $("#txtHour").val(item[0].HoursAmountSales),
            $("#txtParts").val(item[0].PartsAmountSales),
            //$("#drpVehicle").val(item.VehicleId),
            $("#drpStore").val(item[0].StoreCodeSales);
            $("#dropPayType").val(item[0].PayMentTypeSales);
            loadCashPoint(item[0].MemberID),
            LoadVehicleInfo(item[0].VehicleId);

            var d = new Date();
            if ($("#txtDate").val() != d.Format("yyyy-MM-dd")) {
                $("#txtDate").nextAll("span.error-block").html("请注意，发生日期不在当天");
            }

            $("#drpBusinessType").attr("disabled", "disabled");
            $("#drpBusinessChildType").attr("disabled", "disabled");
            $("#drpBusinessNo").attr("disabled", "disabled");
            $("#drpVehicle").attr("disabled", "disabled");
            $("#txtDate").attr("disabled", "disabled");
            $("#txtPrice").attr("disabled", "disabled");
            $("#txtAmount").attr("disabled", "disabled");
            $("#dropPayType").attr("disabled", "disabled");
            $("#txtNonGold").attr("disabled", "disabled");
            $("#txtGold").attr("disabled", "disabled");



        }
    });




}

//新建条目
function add() {
    $("#txtTradeId").val('');
    clearData();
    LoadVehicleInfo();
    if ($("#txtStatus").val() == "0") {
        $.dialog("会员状态为停用，不允许新建结算");
        return;
    }

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
    $("#txtDate").val('');
    $("#txtPrice").val('');
    $("#txtAmount").val('');
    $("#txtNonGold").val('');
    $("#txtGold").val('');
    $("#txtHour").val('');
    $("#txtParts").val('');
    $("#drpStore").val('');
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
function LoadVehicleInfo(v) {
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
            $("#drpVehicle").val(v);
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

function txtPriceOnchange(amount) {
    if ($("#txtAmount").nextAll("span.error-block").html() != "")
        $("#txtAmount").nextAll("span.error-block").html("");


}

function txtAmountOnchange(amount) {
    if ($("#txtAmount").nextAll("span.error-block").html() != "")
        $("#txtAmount").nextAll("span.error-block").html("");
    $("#txtGold").nextAll("span.error-block").html("");
    $("#txtNonGold").val("");




    if ((parseInt(amount) - parseInt($("#stgValidValue2").val())) < "0" && (parseInt(amount) - parseInt($("#stgValidValue3").val())) < "0") {
        $("#txtGold").val(amount);
        $("#txtNonGold").val(parseInt($("#txtAmount").val()) - parseInt(amount));
    }
    else {
        if ((parseInt($("#stgValidValue2").val()) - parseInt($("#stgValidValue3").val()) > "0")) {
            $("#txtGold").val($("#stgValidValue3").val());
            $("#txtNonGold").val(parseInt(amount) - parseInt($("#stgValidValue3").val()));
        }
        else {
            $("#txtGold").val($("#stgValidValue2").val());
            $("#txtNonGold").val(parseInt(amount) - parseInt($("#stgValidValue2").val()));
        }
    }
    //$("#txtHour").val("");
    //$("#txtParts").val("");
}

function txtNonGoldOnchange(amount) {
    if ($("#txtNonGold").nextAll("span.error-block").html() != "")
        $("#txtNonGold").nextAll("span.error-block").html("");
    if ($("#txtGold").nextAll("span.error-block").html() != "")
        $("#txtGold").nextAll("span.error-block").html("");

    if ($("#txtAmount").val() != "" && $("#txtAmount").val() != null && $("#txtNonGold").val() != "" && $("#txtNonGold").val() != null) {
        var a = $("#txtAmount").val() - $("#txtNonGold").val();
        $("#txtGold").val(a);
    }
    if (parseInt(amount) - $("#txtAmount").val() > 0) {
        $("#txtNonGold").nextAll("span.error-block").html("非金币金额不能大于应付金额");
        $("#txtNonGold").val("");
        $("#txtGold").val("");
    }

    if (parseInt(amount) < 0) {
        $("#txtNonGold").nextAll("span.error-block").html("金币金额不能为负数");
        $("#txtGold").val("");
        $("#txtNonGold").val("");
    }
}



function txtGoldOnchange(amount) {
    if ($("#txtGold").nextAll("span.error-block").html() != "")
        $("#txtGold").nextAll("span.error-block").html("");
    if ($("#txtNonGold").nextAll("span.error-block").html() != "")
        $("#txtNonGold").nextAll("span.error-block").html("");

    if (parseInt(amount) < 0) {
        $("#txtGold").nextAll("span.error-block").html("金币金额不能为负数");
        $("#txtGold").val("");
        $("#txtNonGold").val("");
    }

    if (parseInt(amount) - $("#txtAmount").val() > 0) {
        $("#txtNonGold").nextAll("span.error-block").html("非金币金额不能大于应付金额");
        $("#txtNonGold").val("");
        $("#txtGold").val("");
    }


    if ((parseInt(amount) - parseInt($("#stgValidValue3").val())) > "0" || (parseInt(amount) - parseInt($("#stgValidValue2").val())) > "0") {
        if ((parseInt($("#stgValidValue3").val()) - parseInt($("#stgValidValue2").val())) > "0")
            $("#txtGold").nextAll("span.error-block").html("最大可用金币不能超过" + $("#stgValidValue2").val());
        else
            $("#txtGold").nextAll("span.error-block").html("最大可用金币不能超过" + $("#stgValidValue3").val());
        $("#txtGold").val('');
    }

    if ($("#txtAmount").val() != "" && $("#txtAmount").val() != null && $("#txtGold").val() != "" && $("#txtGold").val() != null) {
        var a = $("#txtAmount").val() - $("#txtGold").val();;
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

function LoadDepartmentByUser() {
    ajax("/BaseData/LoadDepartmentByUser", {}, function (res) {
        if (res.IsPass) {
            var optionstring = "<option value=''>请选择</option>";
            var prv = res.Obj[0];
            for (var i in prv) {
                optionstring += "<option value=\"" + prv[i].DepartmentID + "\" >" + prv[i].DepartmentName + "</option>";
            }
            $("#drpDepartment").append(optionstring);
        } else {
            $.dialog(res.MSG);
        }
    })
}