var midFrom, midTo, dtFrom, dtTo;

$(function () {
    $("#btnSearchFrom").click(function () {
        getMembersFrom();
    });
    $("#btnSearchTo").click(function () {
        getMembersTo();
    });

    $("#cbkSelectAll").change(function () {
        var isSelected = $("#cbkSelectAll")[0].checked;
        $("#dt_Credit tbody input[name=transferFlag]").attr("checked", isSelected);
        calculate();
    });

    $("#btnTransfer").click(function () {

        saveTransfer();
    });
    $("#btnCancel").click(function () {
        $.dialog("确认放弃转让吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            location.href = "/CreditTransfer/Index";
        });
    });
});

function getMembersFrom() {
    if (!dtFrom) {
        //子会员360列表显示初始化
        dtFrom = $('#dt_From').dataTable({
            sAjaxSource: '/CreditTransfer/GetMembersForCredit',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: "MemberID", title: "会员编号", visible: false },
                {
                    data: null, title: "选中", sortable: false, sClass: "center", render: function (obj) {
                        return '<input type="radio" name="rdoFromMember" onchange="fromMemberSelected(\'' + obj.MemberID + '\',\'' + obj.CustomerName + '\')" />';
                    }
                },
                { data: "CustomerName", title: "姓名", sortable: false },
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerMobile", title: "手机", sortable: false },
                { data: "CertificateNoKey", title: "证件号码", sortable: false },
                //{ data: "CarNo", title: "车牌号", sortable: false },
                //{ data: "VIN", title: "车架号", sortable: false },
                //{
                //    data: null, title: "", sortable: false, render: function (obj) {
                //        return '<button id="btnSelectFrom" class="btn" onclick="fromMemberSelected(\'' + obj.MemberID + '\',\'' + obj.CustomerName + '\')">选中</button>';
                //    }
                //}
            ],
            fnFixData: function (d) {
                d.push({ name: 'mobile', value: $("#txtMobileFrom").val() });
                d.push({ name: 'certificate', value: $("#txtCertificateFrom").val() });
                d.push({ name: 'vehicleNo', value: $("#txtVehicleFrom").val() });
                d.push({ name: 'vin', value: $("#txtVINFrom").val() });
            }
        });
    }
    else {
        dtFrom.fnDraw();
    }
}
function fromMemberSelected(mid, name) {
    midFrom = mid;
    getCreditSummary(midFrom, name);
    getCreditDetails(midFrom);
}
function getCreditSummary(mid, name) {
    $("#stgName").text(name);
    $("#stgTotalCredit").text("");
    $("#stgValidCredit").text("");
    ajax("/CreditTransfer/GetCreditSummary", { memberId: midFrom }, function (data) {
        if (data.IsPass) {
            $("#stgTotalCredit").text(data.Obj[1]);
            $("#stgValidCredit").text(data.Obj[0]);
        }
        else {
            $.dialog(data.MSG);
        }
    });
}
function getCreditDetails(mid) {
    ajax("/CreditTransfer/GetCreditDetails", { memberId: midFrom }, function (res) {
        if (res.IsPass) {
            var tbody = $("#dt_Credit tbody");
            tbody.empty();
            if (!res.Obj) {
                tbody.append('<tr class="odd"><td class="dataTables_empty" valign="top" colspan="6">没有记录</td></tr>');
            }
            else {
                var data = res.Obj[0];
                for (var i in data) {
                    var sDate = !data[i].SpecialDate1 ? "" : data[i].SpecialDate1.substr(0, 10);
                    //var eDate = !data[i].SpecialDate2 ? "" : data[i].SpecialDate2.substr(0, 10);
                    var tr = $('<tr></tr>');

                    tr.append('<td>' + data[i].ValidValue + '</td>')
                    .append('<td>' + (data[i].EndDate == null ? "" : data[i].EndDate) + '</td>')
                    .append('<td>' + data[i].AccountLimit + '</td>')
                    .append('<td align="center"><input type="checkbox" name="transferFlag"/></td>')
                    .append('<td><input type="text" name="transferAmount" max="' + data[i].ValidValue + '"/><input type="hidden" name="transferIds" value="' + data[i].AccountDetailIds + '"/></td>')
                    .appendTo(tbody);
                }
                $("input[name=transferFlag]").change(function () {
                    calculate();
                });
                $("input[name=transferAmount]").change(function () {
                    var newValue = $(this).val();
                    var oldValue = $(this).attr("max");
                    if (newValue && parseFloat(newValue) > parseFloat(oldValue)) {
                        $.dialog("本次转让积点超出总积点，请重新输入");
                        $(this).val("");
                    }
                    else {
                        calculate();
                    }
                });
            }
        }
        else {
            $.dialog(data.MSG);
        }
    });
}

function calculate() {
    var total = 0;
    $("#dt_Credit tbody tr").each(function (i, e) {
        if ($(e).find("input[name=transferFlag]")[0].checked) {
            var vl = $(e).find("input[name=transferAmount]").val();
            if (vl != undefined && vl != null && vl != "") {
                total += parseFloat(vl);
            }
        }
    });
    $("#spnTotal").text(total);
}

function getMembersTo() {
    if (!dtTo) {
        //子会员360列表显示初始化
        dtTo = $('#dt_To').dataTable({
            sAjaxSource: '/CreditTransfer/GetMembersForCredit',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: "MemberID", title: "会员编号", visible: false },
                {
                    data: null, title: "选中", sortable: false, sClass: "center", render: function (obj) {
                        //return '<button id="btnSelectTo" class="btn" onclick="toMemberSelected(\'' + obj.MemberID + '\',\'' + obj.CustomerName + '\')">选中</button>';
                        return '<input type="radio" name="rdoToMember" onchange="toMemberSelected(\'' + obj.MemberID + '\',\'' + obj.CustomerName + '\')" />';
                    }
                },
                { data: "CustomerName", title: "姓名", sortable: false },
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerMobile", title: "手机", sortable: false },
                { data: "CertificateNoKey", title: "证件号码", sortable: false },
                //{ data: "CarNo", title: "车牌号", sortable: false },
                //{ data: "VIN", title: "车架号", sortable: false },
            ],
            fnFixData: function (d) {
                d.push({ name: 'mobile', value: $("#txtMobileTo").val() });
                d.push({ name: 'certificate', value: $("#txtCertificateTo").val() });
                d.push({ name: 'vehicleNo', value: $("#txtVehicleTo").val() });
                d.push({ name: 'vin', value: $("#txtVINTo").val() });
            }
        });
    }
    else {
        dtTo.fnDraw();
    }
}
function toMemberSelected(mid) {
    midTo = mid;
}

function saveTransfer() {
    $.dialog("确认进行本次转让吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        if (!midFrom) {
            $.dialog("必须选择转出账户");
            return;
        }
        if (!midTo) {
            $.dialog("必须选择转入账户");
            return;
        }
        var details = [];
        $("#dt_Credit tbody tr").each(function (i, e) {
            if ($(e).find("input[name=transferFlag]")[0].checked) {
                var vl = $(e).find("input[name=transferAmount]").val();
                var ids = $(e).find("input[name=transferIds]").val().split(",");
                if (!vl || !ids) {
                    return true;
                }
                details.push({ TransferValue: vl, AccountDetailIds: ids });
            }
        });
        if (!details||details.length<=0) {
            $.dialog("请检查'是否转让'是否勾选，或者'本次转让数量'填写格式是否正确");
            return;
        }

        ajax("/CreditTransfer/SubmitCreditTransfer", { fromMemId: midFrom, toMemId: midTo, details: details }, function (data) {
            if (data.IsPass) {
                $.dialog("积点转让成功");
                location.href = "/CreditTransfer/Index";
            }
            else {
                $.dialog(data.MSG);
            }
        });
    });
}