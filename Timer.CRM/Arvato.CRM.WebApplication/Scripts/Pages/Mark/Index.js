var dtActivityList,
    dtPromotionList,
    bindActivityID;

$(document).ready(function () {
    $("#drpBusinessPlan").chosen();
    $("body").delegate(".dynamicDate", "focusin", function () {
        $(this).datepicker({ startDate: '0' });
    });
    loadAllBusinessPlan();
    loadActivityList();
    $("#ActivityID").bind("keyup", function () {
        $(this).val($(this).val().replace(/[\D]/g, ""));
    });

    $('#frmSearch').submit(function (e) {
        e.preventDefault();
        if (validate(this)) {
            loadActivityList();
        }
    });

    $('#PlanStartTimeFrom,#PlanEndDateFrom').datepicker({
        format: 'yyyy-mm-dd',
        disableFocus: true,
    });

    $('#PlanEndDateEnd,#PlanStartTimeEnd').datepicker();

    $('.btnAdd').click(function () {
        window.open('/Mark/Edit');
    });

    $("#btnRestSearchActivity").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        resetForm("frmSearch");
    });

    $("#dtList").resize(function () {
        $("#dtList").css({ "width": "110%" });
    });

    $("#btnActivityPromotion").click(function (){
        bindPromotion(bindActivityID);
    });
    //$("#dtAvailablePromotionList").resize(function () {
    //    if (dtPromotionList) {
    //        $("#dtAvailablePromotionList").css({ "width": "100%" });
    //    }
    //});
    
});

function loadActivityList() {
    if (dtActivityList) {
        dtActivityList.fnDestroy();
    }

    dtActivityList = $("#dtList").dataTable({
        sAjaxSource: "/Mark/GetMarketActivities",
        sScrollX: "100%",
        sScrollXInner: "110%",
        bScrollCollapse: true,                       //指定适当的时候缩起滚动视图
        bInfo: true,
        bAutoWidth: false,                                                     //是否自动计算表格各列宽度
        bDestroy: true,
        bRetrieve: true,
        bServerSide: true,
        bLengthChange: false,
        bPaginate: true,
        iDisplayLength: 10,
        aaSorting: [[0, "desc"]],
        aoColumns: [
            { data: 'ActivityID', title: "活动编号", "sClass": "center", sWidth: "8%" },
            {
                data: null, title: "活动名称", "sClass": "center", sWidth: "15%", sortable: false, render: function (r) {
                    var show = r.ActivityName;
                    show = show.length > 10 ? (show.substr(0, 10) + "...") : show;
                    return "<span title='" + r.ActivityName + "'>" + show + "</span>"
                }
            },
            { data: 'BusinessPlanName', title: "所属商业计划", "sClass": "center", sWidth: "15%", sortable: true },
            //{ data: 'StoreName', title: "门店", sWidth: "5%", sortable: false },
            { data: 'PlanStartDate', title: "计划开始时间", "sClass": "center", sortable: true, sWidth: "15%" },
            { data: 'PlanEndDate', title: "计划结束时间", "sClass": "center", sortable: true, sWidth: "15%" },
            //{
            //    data: 'ProStartDate', title: "项目开始时间", "sClass": "center", sortable: false, sWidth: "10%",
            //    render: function (obj) {
            //        if (obj) {
            //            return obj.substr(0, 10);
            //        } else {
            //            return "";
            //        }
            //    }
            //},
            //{
            //    data: 'ProEndDate', title: "项目结束时间", "sClass": "center", sortable: false, sWidth: "10%",
            //    render: function (obj) {
            //        if (obj) {
            //            return obj.substr(0, 10);
            //        } else {
            //            return "";
            //        }
            //    }
            //},
            {
                data: null, title: "激活状态", "sClass": "center", sWidth: "8%", sortable: false,
                render: function (obj) {
                    return obj.Enable ? '激活' : '未激活';
                }
            },
            {
                data: null, title: "活动状态", "sClass": "center", sWidth: "8%", sortable: false,
                render: function (obj) {
                    return obj.Status == "0" ? '提交审批' : '审批通过';
                }
            },
            {
                data: null, title: "操作", sortable: false, "sClass": "center", sWidth: "26%",
                render: function (obj) {
                    var res = "<div style='min-width:260px'>";
                    if (obj.Status == "0") { //提交审批状态
                        //res += "<button class=\"btn bindPromotionButton\" onclick=\"showBindPromotionDialog('" + obj.ActivityID + "')\">关联促销</button>&nbsp;";
                        res += "<button class=\"btn editButton\" onclick=\"edit('" + obj.ActivityID + "')\">编辑</button>";
                        if (!obj.Enable) {      //如果未已经激活(提交审批理论上不可能出现已经激活，但是为了控制严谨加此判断)
                            res += "&nbsp;<button class=\"btn btn-danger\" onclick=\"delConfirm('" + obj.ActivityID + "')\">删除</button>";
                        }
                        res += "&nbsp;<button class=\"approveButton btn btn-info\" onclick=\"doApprove('" + obj.ActivityID + "')\">审批通过</button>";
                    } else {    //审批通过状态
                        res += "<button class=\"btn\" onclick=\"edit('" + obj.ActivityID + "')\">查看</button>";
                        
                        if (!obj.Enable) {
                            res += "&nbsp;<button class=\"btn btn-danger\" onclick=\"delConfirm('" + obj.ActivityID + "')\">删除</button>";
                            res += "&nbsp;<button class=\"approveButton btn btn-info\" onclick=\"refuse('" + obj.ActivityID + "')\">审批驳回</button>";
                        }
                        

                    }
                    res += "</div>";
                    return res;
                }
            }
        ],
        fnFixData: function (d) {
            var list = $('#frmSearch').serializeArray();
            var data = {
                ActivityID: list[0].value,
                ActivityName: list[1].value,
                Enable: list[2].value,
                PlanStartTimeFrom: list[3].value,
                PlanStartTimeEnd: list[4].value,
                PlanEndDateFrom: list[5].value,
                PlanEndDateEnd: list[6].value,
                DataGroupID: $("#selGroup option:selected").val(),
                StoreCode: $("#selStore option:selected").val(),
                BusinessPlanID: $("#drpBusinessPlan").val(),
                Status: parseInt($("#status").val())
            }
            d.push({ name: 'modelStr', value: JSON.stringify(data) });
        }
    });
}

function loadAllBusinessPlan() {
    ajax("/BusinessPlan/GetAllBusinessPlan", null, function (data) {
        $("#drpBusinessPlan").html("");
        var options = "<option value=''>全部</option>";
        if (data && data.length > 1) {
            for (var i = 0; i < data.length; i++) {
                options += "<option value='" + data[i].BusinessPlanID + "'>" + data[i].BusinessPlanName + "</option>";
            }
        }
        $("#drpBusinessPlan").html(options);
        $("#drpBusinessPlan").trigger("liszt:updated");
    });
}

function view(activityId) {
    window.open('/Mark/Detial?activityId=' + activityId);
}

function edit(activityId) {
    window.open('/Mark/Edit?activityId=' + activityId);
}

function loadAvailablePromotionList(activityID) {
    //ajax("/Promotion/GetSysCommonByKey", { "key": activityID, "type": "3" }, function (syslist) {
    //    var selectedPromotionIds = ",";
    //    if (!utility.isNull(syslist)) {
    //        for (var i = 0; i < syslist.length; i++) {
    //            selectedPromotionIds += syslist[i].RelationBigintValue1 + ",";
    //        }
    //    }
    if (dtPromotionList) {
        dtPromotionList.fnDraw();
    }
    else {
        dtPromotionList = $("#dtAvailablePromotionList").dataTable({
            sAjaxSource: "/Promotion/GetPromotionWithSysCommonByKeyForUnlimitedPage",
            bAutoWidth: true,                                                     //是否自动计算表格各列宽度
            bSort: true,   //不排序
            bInfo: false,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: false, //显示分页信息
            //iDisplayLength: 9,
            aaSorting: [[0, "desc"]],
            aoColumns: [
                { data: "BaseDataID", title: "促销ID", "sClass": "center", sWidth: "0", bVisible: false },
                //{ data: "PromotionID", title: "POS促销ID", "sClass": "center", sWidth: "8%" },
                { data: "PromotionCode", title: "促销编号", "sClass": "center", sWidth: "8%" },
                {
                    data: "PromotionName", title: "促销名称", "sClass": "center", sWidth: "20%", sortable: false,
                },
                { data: "PromotionBillType", title: "促销单类型", "sClass": "center", sortable: true, sWidth: "15%" },
                { data: "PromotionType", title: "促销类型", "sClass": "center", sortable: true, sWidth: "10%" },
                {
                    data: null, title: "促销开始时间", "sClass": "center", sortable: false, sWidth: "15%", render: function (r) {
                        if (r.StartDatePromotion != null) {
                            return r.StartDatePromotion.substr(0, 10);
                        }
                        else {
                            return "";
                        }
                    }
                },
                {
                    data: null, title: "促销结束时间", "sClass": "center", sortable: false, sWidth: "15%", render: function (r) {
                        if (r.EndDatePromotion != null) {
                            return r.EndDatePromotion.substr(0, 10);
                        }
                        else {
                            return "";
                        }
                    }
                },
                //{
                //    data: null, title: "绑定生效日期", "sClass": "center", sortable: false, sWidth: "10%", render: function (obj) {
                //        if (utility.isNull(obj.StartDate)) {
                //            return "<div><input type='text' readonly='readonly' id='txtPromotionStartDate_" + obj.BaseDataID.toString() + "' class='input-small dynamicDate' value='' />"
                //            + "<div class='btn-date-clear'></div>"
                //           + "</div>"; txtPromotionStartDate_42663
                //        } else {
                //            return "<div><input type='text' readonly='readonly' id='txtPromotionStartDate_" + obj.BaseDataID.toString() + "' class='input-small dynamicDate' value='" + obj.StartDate.toString().substr(0, 10) + "'/>"
                //            + "<div class='btn-date-clear'></div>"
                //            + "</div>";
                //        }
                //    }
                //},
                //{
                //    data: null, title: "绑定失效日期", "sClass": "center", sortable: false, sWidth: "10%", render: function (obj) {
                //        if (utility.isNull(obj.EndDate)) {
                //            return "<div><input type='text' readonly='readonly' id='txtPromotionEndDate_" + obj.BaseDataID.toString() + "' class='input-small dynamicDate' value='' />"
                //            + "<div class='btn-date-clear'></div>"
                //            + "</div>";
                //        } else {
                //            return "<div><input type='text' readonly='readonly' id='txtPromotionEndDate_" + obj.BaseDataID.toString() + "' class='input-small dynamicDate' value='" + obj.EndDate.toString().substr(0,10) + "'/>"
                //            + "<div class='btn-date-clear'></div>"
                //            + "</div>";
                //        }
                //    }
                //},
                {
                    data: null, title: "操作", sortable: false, "sClass": "center", sWidth: "5%",
                    render: function (obj) {
                        if (parseInt(obj.Key) > 0) {
                            return "<input type='radio' name='chkPromotion' value='" + obj.BaseDataID + "' checked='checked' id='chkPromotion_" + obj.BaseDataID + "'/>";
                        } else {
                            return "<input type='radio' name='chkPromotion' value='" + obj.BaseDataID + "' id='chkPromotion_" + obj.BaseDataID + "'/>";
                        }
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ "name": "key", value: $("#hd_actid").val() });
                d.push({ "name": "type", value: "3" });
                d.push({ "name": "promotionIsEnd", value: "0" });
                d.push({ "name": "isValidDate", value: false });
            }
        });
    }

    
    //});
}

function showBindPromotionDialog(activityID) {
    $("#spanError").html("");
    $("#hd_actid").val(activityID)
    loadAvailablePromotionList(activityID);
    bindActivityID = activityID;
    showEditDialogMessage("divPromotionList");
}

function bindPromotion(activityID) {
    var syslist = [],
        flag = true;
    $("input[name='chkPromotion']:checked").each(function () {
        var currentPromotionID = $(this).val(),
            currentStartDateID = "txtPromotionStartDate_" + currentPromotionID,
            currentEndDateID = "txtPromotionEndDate_" + currentPromotionID,
            tempObject = {
                RelationBigintValue1: currentPromotionID,
                StartDate: null,
                EndDate: null,
                //StartDate: $("#" + currentStartDateID).val(),
                //EndDate: utility.isNull($("#" + currentEndDateID).val()) ? "" : $("#" + currentEndDateID).val() + " 23:59:59",
                RelationBigintValue2: activityID
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
            "commonType": "3",
            "sysCommonList": JSON.stringify(syslist),
            "key": activityID
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

function delConfirm(activityId) {
    $.dialog("确认删除这个活动吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Mark/Delete", { activityId: activityId }, function (res) {
            if (typeof res == "string") {
                res = JSON.parse(res);
            }
            if (res.IsPass) {
                $.dialog("删除成功");
                loadActivityList();
            } else {
                $.dialog("删除失败," + res.MSG);
            }
        });
    });
}

function doApprove(activityID) {
    $.dialog("确认审核通过吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Mark/UpdateActivityStatus", { activityID: activityID, status: "99" }, function (data) {
            if (data && data.IsPass) {
                $.dialog("审核成功");
                loadActivityList();
            } else {
                $.dialog(data.MSG);
            }
        });
    });
}

function refuse(activityID) {
    $.dialog("确认驳回审核吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Mark/UpdateActivityStatus", { activityID: activityID, status: "0" }, function (data) {
            if (data && data.IsPass) {
                $.dialog("审核驳回");
                loadActivityList();
            } else {
                $.dialog(data.MSG);
            }
        });
    });

}



function resetForm(id) {
    $("#" + id + " :input").not(":button, :submit, :reset").val("").removeAttr("checked").removeAttr("selected");
    $("#" + id + " .error").removeClass('error');
    $("#" + id + " .help-block").html('')
}