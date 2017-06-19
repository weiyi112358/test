$(document).ready(function () {
    $("#txtPlanStartTime,#txtPlanEndTime").datepicker({ dateFormat: "yy-mm-dd" });
    $("body").delegate(".dynamicDate", "focusin", function () {
        $(this).datepicker({ startDate: '0' });
    });

    loadBusinessType();
    loadBusinessPlan(false);

    $("#btnNewBusinessPlan").click(function () {
        window.open("/BusinessPlan/BusinessPlanInsert");
    });

    $("#btnSearch").click(function () {
        loadBusinessPlan(false);
    });

    $("#btnAdvanceSearch").click(function () {
        loadBusinessPlan(true);
        $.colorbox.close();
    });

    $("#btnResetAdvanceSearch").click(function () {
        resetForm("SeniorSearch_dialog");
    });
    $("#dt_BusinessPlan").resize(function () {
        $("#dt_BusinessPlan").css({ "width": "120%" })
    });

    $("#btnBusinessPlanPromotion").click(function () {
        bindPromotion(bindBusinessPlanID);
    });
});

var dtBusinessPlan,
    dtPromotionList,
    bindBusinessPlanID;

function loadBusinessPlan(isAdvanceSearch) {
    //destory datatable资源之后重新加载新资源
    if (dtBusinessPlan) {
        dtBusinessPlan.fnDraw();
    }
    else {
        dtBusinessPlan = $("#dt_BusinessPlan").dataTable({
            sAjaxSource: "/BusinessPlan/SearchBusinessPlan",
            sScrollX: "100%",
            sScrollXInner: "120%",
            bScrollCollapse: true,                       //指定适当的时候缩起滚动视图
            bInfo: true,
            bAutoWidth: true,                                                     //是否自动计算表格各列宽度
            bDestroy: true,
            bRetrieve: true,
            bServerSide: true,
            bLengthChange: false,
            bPaginate: true,
            iDisplayLength: 10,
            aaSorting: [[6, "desc"]],
            aoColumns: [
                { data: "BusinessPlanID", sortable: false, title: "计划代码", "sClass": "center sorting_disabled", sWidth: "15%" },
                {
                    data: null, title: "计划名称", sortable: false, "sClass": "center", sWidth: "15%",
                    render: function (obj) {
                        if (obj) {
                            var show = obj.BusinessPlanName;
                            show = show.length > 10 ? (show.substr(0, 10) + "...") : show;
                            return "<a onclick=\"redirectToDetail('" + obj.BusinessPlanID + "')\" title='" + obj.BusinessPlanName + "'>" + show + "</a>";
                        }
                    }
                },
                { data: "PlanTypeName", title: "计划类型", sortable: false, "sClass": "center", sWidth: "10%" },
                {
                    data: "PlanStartTime", title: "开始时间", sortable: false, "sClass": "center", sWidth: "10%", render: function (obj) {
                        if (obj) {
                            return obj.substr(0, 10);
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "PlanEndTime", title: "结束时间", sortable: false, "sClass": "center", sWidth: "10%", render: function (obj) {
                        if (obj) {
                            return obj.substr(0, 10);
                        } else {
                            return "";
                        }
                    }
                },
                { data: "StatusName", title: "计划状态", sortable: false, "sClass": "center", sWidth: "10%" },
                { data: "AddedDate", title: "添加时间", sortable: false, "sClass": "center", sWidth: "0%", bVisible: false },
                {
                    data: null, title: "操作", sortable: false, "sClass": "center", sWidth: "20%",
                    render: function (obj) {
                        if (obj.Status == "1") {
                            return "<div style='min-width:260px'>"
                                + "<button class=\"btn bindPromotionButton\" onclick=\"showBindPromotionDialog('" + obj.BusinessPlanID + "')\">关联促销</button>&nbsp;"
                                + "<button class=\"btn btn-modify\" onclick=\"redirectToEdit('" + obj.BusinessPlanID + "')\">编辑</button>&nbsp;"
                                + "<button class=\"btn btn-danger btn-delete\" onclick=\"deleteBusinessPlan('" + obj.BusinessPlanID + "')\">删除</button>&nbsp;"
                                + "<button class=\"approveButton btn btn-info\" onclick=\"doApprove('" + obj.BusinessPlanID + "','" + obj.ModifiedDate + "')\">审批通过</button>"
                                + "</div>";
                        } else {
                            return "<div style='min-width:180px'></div>";
                        }
                    }
                }
            ],
            fnFixData: function (d) {
                if (isAdvanceSearch) {
                    d.push({ name: 'planName', value: $("#txtPlanName").val() });
                    d.push({ name: 'planType', value: $("#drpPlanType").val() });
                    d.push({ name: 'planStartTime', value: $("#txtPlanStartTime").val() });
                    d.push({ name: 'planEndTime', value: utility.isNull($("#txtPlanEndTime").val()) ? "" : $("#txtPlanEndTime").val() + " 23:59:59" });
                    d.push({ name: 'planCode', value: $("#txtPlanCode").val() });
                    d.push({ name: 'status', value: "" });
                } else {
                    d.push({ name: 'planName', value: $("#txtName").val() });
                    d.push({ name: 'planName', value: "" });
                    d.push({ name: 'planType', value: "" });
                    d.push({ name: 'planStartTime', value: "" });
                    d.push({ name: 'planEndTime', value: "" });
                    d.push({ name: 'planCode', value: "" });
                    d.push({ name: 'status', value: "" });
                }
            },

        });
    }

   
}

function loadBusinessType() {
    ajax("/BaseData/GetBizOptionsByType", { optionType: "BusinessPlanType", enable: true }, function (data) {
        $("#drpPlanType").html("");
        var options = "";
        if (data && data.length > 1) {
            options += "<option value=''>全部</option>";
            for (var i = 0; i < data.length; i++) {
                options += "<option value='" + data[i].OptionValue + "'>" + data[i].OptionText + "</option>";
            }
            $("#drpPlanType").html(options);
        }
    });
}

function loadAvailablePromotionList(businessPlanID) {
    if (dtPromotionList) {
        dtPromotionList.fnDraw();
    }

    dtPromotionList = $("#dtAvailablePromotionList").dataTable({
        sAjaxSource: "/Promotion/GetPromotionWithSysCommonByKeyForUnlimitedPage",
        bAutoWidth: true,                                                     //是否自动计算表格各列宽度
        bSort: true,   //不排序
        bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: false, //显示分页信息
        //iDisplayLength: 7,
        aaSorting: [[0, "desc"]],
        aoColumns: [
            { data: "BaseDataID", title: "促销ID", "sClass": "center", sWidth: "1%", bVisible: false,sortable:true, },
            { data: "PromotionCode", title: "促销编号", "sClass": "center", sWidth: "8%" },
            {
                data: "PromotionName", title: "促销名称", "sClass": "center", sWidth: "25%", sortable: false, render: function (r) {
                    return "<span style='white-space:nowrap'>" + r + "</span>"
                }
            },
            { data: "PromotionBillType", title: "促销单类型", "sClass": "center", sortable: true, sWidth: "8%" },
            { data: "PromotionType", title: "促销类型", "sClass": "center", sortable: true, sWidth: "8%" },
            {
                data: null, title: "促销开始时间", "sClass": "center", sortable: false, sWidth: "10%", render: function (r) {
                    if (r.StartDatePromotion != null) {
                        return r.StartDatePromotion.substr(0, 10);
                    }
                    else {
                        return "";
                    }
                }
            },
            {
                data: null, title: "促销结束时间", "sClass": "center", sortable: false, sWidth: "10%", render: function (r) {
                    if (r.EndDatePromotion != null) {
                        return r.EndDatePromotion.substr(0, 10);
                    }
                    else {
                        return "";
                    }
                }
            },
            {
                data: null, title: "操作", sortable: false, "sClass": "center", sWidth: "5%",
                render: function (obj) {
                    if (!utility.isNull(obj.Key)) {
                        //return "<input type='hidden' id='"+obj.BaseDataID+"' value='"+obj.Key+"'>"
                        return "<input type='radio' name='ckPromotion' value='" + obj.BaseDataID + "' checked='checked' id='ckPromotion_" + obj.BaseDataID + "'/>";
                    } else {
                        //return "<input type='hidden' id='" + obj.BaseDataID + "' value='" + obj.Key + "'>"
                        return "<input type='radio' name='ckPromotion' value='" + obj.BaseDataID + "' id='ckPromotion_" + obj.BaseDataID + "'/>";
                    }
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ "name": "key", value: $("#hd_busid").val() });
            d.push({ "name": "type", value: "2" });
            d.push({ "name": "promotionIsEnd", value: "0" });
            d.push({ "name": "isValidDate", value: false });
        },
        //fnRowCallback: function (nRow, aData, iDisplayIndex) {
        //    var id = $('td:eq(6)', nRow)[0].firstChild.id;
        //    var value = $('td:eq(6)', nRow)[0].firstChild.value;
        //    if (value=="null") {
        //        $('td:eq(6)', nRow).html("<input type='radio' name='ckPromotion' value='" + id + "' id='ckPromotion_" + id + "'/>")
        //    } else {
        //        $('td:eq(6)', nRow).html("<input type='radio' name='ckPromotion' value='" + id + "' checked='checked' id='ckPromotion_" + id + "'/>")
        //    }
          
        //}
    });
}

function showBindPromotionDialog(businessPlanID) {
    $("#spanError").html("");
    $("#hd_busid").val(businessPlanID);
    loadAvailablePromotionList(businessPlanID);
    bindBusinessPlanID = businessPlanID;
    showEditDialogMessage("divPromotionList");
}

function bindPromotion(businessPlanID) {
    var syslist = [],
        flag = true;
    $("input[name='ckPromotion']:checked").each(function () {
        var currentPromotionID = $(this).val(),
            currentStartDateID = "txtPromotionStartDate_" + currentPromotionID,
            currentEndDateID = "txtPromotionEndDate_" + currentPromotionID,
            tempObject = {
                RelationBigintValue1: currentPromotionID,
                StartDate: null,
                EndDate: null,
                StartDate: $("#" + currentStartDateID).val(),
                EndDate: utility.isNull($("#" + currentEndDateID).val()) ? "" : $("#" + currentEndDateID).val() + " 23:59:59",
                ReferenceStringValue1: businessPlanID
            };
        if (!utility.isNull($("#" + currentStartDateID).val()) && !utility.isNull($("#" + currentEndDateID).val())
                && !utility.compareDate($("#" + currentStartDateID).val(), $("#" + currentEndDateID).val())) {
            flag = false;
        }
        syslist.push(tempObject);
    });
    if (!flag) {
        $("#spanError").html("失效时间必须大于生效时间");
        return false;
    }
    if (utility.isNull(syslist)) {
        $("#spanError").html("请选择要绑定的促销活动");
        return false;
    } else {
        ajax("/Promotion/InsertBactchPromotionSysCommon", {
            "commonType": "2",
            "sysCommonList": JSON.stringify(syslist),
            "key": businessPlanID
        }, function (data) {
            if (data.IsPass) {
                $.colorbox.close();
                $.dialog("绑定成功");
                return;
            } else {
                $("#spanError").html(data.MSG);
                return false;
            }
        });
    }
}

function deleteBusinessPlan(planID) {
    $.dialog("确认删除商业计划吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BusinessPlan/DeleteBusinessPlanByID", { businessPlanID: planID }, function (data) {
            if (data && data.IsPass) {
                $.dialog("删除成功");
                loadBusinessPlan();
            } else {
                $.dialog(data.MSG);
            }
        });
    });
}

function redirectToEdit(planID) {
    window.open("/BusinessPlan/BusinessPlanInsert/" + planID);
}

function redirectToDetail(planID) {
    window.open("/BusinessPlan/BusinessPlanDetail/" + planID);
}

function resetForm(id) {
    $("#" + id + " :input").not(":button, :submit, :reset").val("").removeAttr("checked").removeAttr("selected");
    $("#" + id + " .error").removeClass('error');
    $("#" + id + " .help-block").html('')
}

function showSeniorSearch() {
    resetForm("SeniorSearch_dialog");

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        title: '高级查询',
        href: "#SeniorSearch_dialog",
        inline: true
    });
    //dtkpi.fnDraw();
}

function doApprove(planID, updateDate) {
    $.dialog("确认审核通过吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BusinessPlan/UpdateBusinessPlanStatus", { businessPlanID: planID, status: "2", updateDate: updateDate }, function (data) {
            if (data && data.IsPass) {
                $.dialog("审核成功");
                loadBusinessPlan();
            } else {
                $.dialog(data.MSG);
            }
        });
    });
}


