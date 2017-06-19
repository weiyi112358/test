var dtRates;
$(function () {
    dtRates = $('#dt_rates').dataTable({
        sAjaxSource: '/CreditTransfer/GetTransferRates',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'DataGroupName', title: "数据群组", sortable: false },
            { data: 'RateValue', title: "转让比例(%)", sortable: true },
            { data: 'Remark', title: "备注", sortable: false },
            {
                data: 'ModifiedDate', title: "修改时间", sortable: true, render: function (obj) {
                    return !obj ? "" : obj.substr(0, 10);
                }
            },
            {
                data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {
                    var str = '<button id="btnEdit" class="btn btnEdit" onclick="edit(' + r.RateId + ')">编辑</button> ';
                    str += '<button id="btnDelete" class="btn btn-danger btnDelete" onclick="deleteRate(' + r.RateId + ')">删除</button>';
                    return str;
                }
            }
        ],
        //fnFixData: function (d) {
        //    d.push({ name: 'name', value: $("#txtChannelName").val() });
        //    d.push({ name: 'type', value: $("#drpChannelType").val() });
        //}
    });

    $("#btnSearch").click(function () {
        dtRates.fnDraw();
    });

    //表单验证
    var addValidator = $("#frmRate").validate({
        rules: {
            drpDataGroup: {
                required: true
            },
            txtRateValue: {
                required: true,
                number: true,
                min: 0,
                max: 100
            }
        },
        errorClass: 'error-block'
    });
    //提交
    $("#frmRate").submit(function (e) {
        e.preventDefault();
        if (addValidator.form()) {
            var id = $("#hdnRateId").val();
            if (!id) {
                id = 0;
            }
            var rate = {
                RateId: id,
                DataGroupId: $("#drpDataGroup").val(),
                RateValue: $("#txtRateValue").val(),
                Remark: $("#txtRemark").val()
            }
            var postUrl = "/CreditTransfer/EditRateSubmit";
            ajax(postUrl, { modelStr: encode(JSON.stringify(rate)) }, function (d) {
                if (d.IsPass) {
                    $.dialog(d.MSG);
                    clearFormData("frmRate");
                    dtRates.fnDraw();
                    $.colorbox.close();
                }
                else {
                    $.dialog(d.MSG);
                }
            });
        }
    });
});

//修改渠道
function edit(id) {
    if (!id) {
        $.dialog("ID不能为空，请刷新页面后再试");
        return;
    }
    clearFormData("frmRate");
    $("#h3Rate").text("修改转让比例");
    ajax("/CreditTransfer/EditRate", { id: id }, function (d) {
        if (d.IsPass) {
            //初始化编辑页面的值
            var rate = d.Obj[0];
            $("#hdnRateId").val(rate.RateId);
            //$("#drpDataGroup").val(rate.DataGroupId);
            $("#txtRateValue").val(rate.RateValue);
            $("#txtRemark").val(rate.Remark);

            loadDataGroupList(rate.DataGroupId);
            //显示编辑页面弹窗
            $.colorbox({
                initialHeight: '0',
                initialWidth: '0',
                overlayClose: false,
                opacity: '0.3',
                //title: '素材',
                href: "#rate_dialog",
                inline: true
            });
        }
        else {
            $.dialog(d.MSG);
        }
    })
}

//加载数据群组列表
function loadDataGroupList(selected) {
    $("#drpDataGroup").empty();
    ajax("/CreditTransfer/GetSubDataGroupList", { selected: selected }, function (data) {
        if (data.IsPass) {
            var list = data.Obj[0];
            var options='';
            if (list.length > 0) {
                options = '<option value="">请选择</option>';
                for (var i in list) {
                    options += '<option value="' + list[i].SubDataGroupID + '">' + list[i].SubDataGroupName + '</option>';
                }
            } else {
                options = '<option value="">无</option>';
            }
            $("#drpDataGroup").html(options);
        }
        $("#drpDataGroup").val(selected);
    });
}

//新建渠道
function add() {
    clearFormData("frmRate");
    $("#h3Rate").text("新建转让比例");
    loadDataGroupList("");
    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#rate_dialog",
        inline: true
    });
}

//删除
function deleteRate(id) {
    if (!id) {
        $.dialog("ID不能为空，请刷新页面后再试");
        return;
    }
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/CreditTransfer/DeleteRate", { id: id }, function (d) {
            if (d.IsPass) {
                $.dialog("删除成功！");
                var start = dtRates.fnSettings()._iDisplayStart;
                var length = dtRates.fnSettings()._iDisplayLength;
                dtRates.fnPageChange(start / length, true);
            } else {
                $.dialog(d.MSG);
            }
        });
    });
}

//清空表单数据
function clearFormData(formId) {
    var form = $("#" + formId);
    if (form) {
        form.find("input[type!=button]").val("");
        form.find("select").val("");
        form.find("span.error-block").html("");
    }
}