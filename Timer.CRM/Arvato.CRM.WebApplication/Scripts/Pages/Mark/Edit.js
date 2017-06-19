var templateCollection = [
            { "category": "SMS", "icon": "icon-sms", "resulttype": "Varchar", "title": "短信沟通模板", "remove": 1, "img": "/img/gCons/chat-.png" },
            { "category": "Mail", "icon": "icon-mail", "resulttype": "Varchar", "title": "邮件沟通模板", "remove": 1, "img": "/img/gCons/email.png" },
            { "category": "OB", "icon": "icon-call", "resulttype": "Number", "title": "外呼沟通模板", "remove": 1, "img": "/img/gCons/phone.png" },
            { "category": "Coupon", "icon": "icon-coupon", "resulttype": "Number", "datalimit": 1, "title": "优惠劵模板", "remove": 1, "img": "/img/gCons/tag.png" },
            { "category": "Question", "icon": "icon-coupon", "resulttype": "Number", "title": "调查问卷模板", "remove": 1, "img": "/img/gCons/tag.png" },
            { "category": "WeChat", "icon": 'icon-sms', "resulttype": "Varchar", "title": "微信沟通模板", "remove": 1, "img": "/img/gCons/chat-.png" },
            { "category": "Normal", "icon": 'icon-coupon', "resulttype": "Number", "title": "普通模板", "remove": 1, "img": "/img/gCons/tag.png" }
],
   wf, //工作流对象,
   haveOtherCoupon = false,
   parentWindow;
var refNOExist = false;

$(document).ready(function () {
    parentWindow = window.opener;
    activityID = $("#hidActivityID").val();
    activityEnable = $("#hidActivityEnable").val();
    activityStatus = $("#hidActivityStatus").val();
    hidemenu();//默认隐藏菜单
    getPOSpromotion(activityID)
    var chosedPlanStartTime = $("#hidPlanStartTime").val(),
       chosedPlanEndTime = $("#hidPlanEndTime").val();

    $("#drpBusinessPlan").chosen();

    pageInit();

    $("#btnShowKPI").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        showKPIdialog();
    });

    $("#frmWorkFlow").resize(function () {
        if (utility.isTrue(activityEnable) || activityStatus != "0") { //如果激活状态为激活或者状态不是提交审批的时候不可以编辑
            $("#mask").height($("#mask").next().height() - 42);
            $("#mask").width($("#mask").next().width());
            $("#mask").css({ "top": $("#mask").next()[0].offsetTop });
            $("#mask").show();
        }
    });

    $("#divKPIList").resize(function () {
        if (utility.isTrue(activityEnable) || activityStatus != "0") {//如果激活状态为激活或者状态不是提交审批的时候不可以编辑
            $("#maskkpi").height($("#maskkpi").next().height() + 26);
            $("#maskkpi").width($("#maskkpi").next().width());
            $("#maskkpi").css({ "top": $("#maskkpi").next()[0].offsetTop - 13 });
            $("#maskkpi").show();
        }
    });

    $("#tabwork").click(function () {
        if (wf) {
            wf.init();
            wf.redraw();
        }
    });

    $("#ReferenceNo").blur(function () {
        if ($("#ReferenceNo").val() != "") {
            $.getJSON("/Mark/CheckRefExist?refNO=" + $("#ReferenceNo").val(), function (json) {
                if (json.IsPass) {
                    if (json.Obj[0] && json.Obj[0].length > 0) {
                        $.dialog("该微信活动已被引用");
                        refNOExist = true;
                    }
                    else {
                        refNOExist = false;
                    }
                }
            });
        }
        else {
            refNOExist = false;
        }
    })

    //////////////////////////以上为KPI相关部分
    var dtClientFollow;
    $('#ProStartDate,#ProEndDate,#planDate').datepicker();
    $('#PlanStartDate,#PlanEndDate').datepicker({ startDate: '0' });
    $('#PlanStartTime').timepicker({ defaultTime: utility.isNull(chosedPlanStartTime) ? "current" : chosedPlanStartTime });
    $('#PlanEndTime').timepicker({
        defaultTime: checktime(chosedPlanEndTime)
    });
    $('#planTime,#planTime1').timepicker();

    //初始化流程设置
    wf = $('#holder').workflow();
    //初始化执行时间
    setws(scheduledata);

    $('#subdivisionWord').keyup(function () {
        var rgx = new RegExp($(this).val(), 'i');
        $('.scrollH150 label').each(function () {
            $(this).toggleClass("hide", !rgx.test($(this).text()));
        });
    });

    //激活
    $('#btnActive').click(function (e) {
        e.preventDefault();
        if (validate('#frmBaseInfo')) {
            var d = {
                ActivityID: $('#ActivityID').val() || 0,
                Enable: true,
                Kpi: JSON.stringify(kpi)
            }, a = "/Mark/Edit";

            if (d.ActivityID == 0) {
                processErrs("请先保存活动");
                return;
            }

            ajax(a, d, function (r) {
                if (r.IsPass) {
                    activityEnable = true;
                    pageElementDisabledInit();
                    $.dialog("激活成功", {}, null, function () {
                        actived = true;
                    });
                } else {
                    $.dialog(r.MSG);
                }
            });
        }
    });

    //取消激活
    $('#btnDeactive').click(function (e) {
        e.preventDefault();
        if (validate('#frmBaseInfo')) {
            var d = {
                ActivityID: $('#ActivityID').val() || 0,
                Enable: false,
                Kpi: JSON.stringify(kpi)
            }, a = "/Mark/Edit";
            ajax(a, d, function (r) {
                if (r.IsPass) {
                    activityEnable = false;
                    pageElementDisabledInit();
                    $.dialog("已取消激活", {}, null, function () {
                        actived = false;
                    });
                } else {
                    $.dialog(r.MSG);
                }
            });
        }
    });

    //保存活动
    $('#frmBaseInfo,#frmWorkFlow,#frmTimeSetting,#formKPISettings').submit(function (e) {
        e.preventDefault();

        if (validate('#frmBaseInfo')) {
            if (utility.isNull($('#ActivityName').val())) {
                processErrs(["请填写活动名称"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }
            if (utility.isNull($('#PlanStartDate').val())) {
                processErrs(["请填写计划开始时间"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }
            if (utility.isNull($('#ProStartDate').val())) {
                processErrs(["请填写项目开始时间"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }
            if ($('#ActivityName').val().length > 50) {
                processErrs(["活动名称不能超过50个字"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }
            if ($('#Remark').val().length > 100) {
                processErrs(["备注不能超过100个字"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }
            if ($('#PlanEndDate').val() == "") {
                processErrs(["计划结束时间不能为空"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }
            if ($('#ProEndDate').val() == "") {
                processErrs(["项目结束时间不能为空"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }
            if (refNOExist) {
                processErrs(["该微信活动已被引用"])
                $('.tabbable .nav-tabs a:eq(0)').tab('show');
                return false;
            }

            if (wf.validate()) {
                if (!hasEnoughCouponAmount()) {
                    if (haveOtherCoupon) {
                        $.dialog("细分人群的数量大于异业券的数量,确认要继续保存该数据吗?", {
                            footer: {
                                closebtn: '取消',
                                okbtn: '确认'
                            }
                        }, function () {
                            doSaveSubdivision();
                        });
                        return false;
                    }
                }
                doSaveSubdivision();
            } else {
                $("#tabwork").click();
                return false;
            }
        } else {
            $('.tabbable .nav-tabs a:eq(0)').tab('show');
            return false;
        }
    });

    function doSaveSubdivision() {
        var ws = getws();
        if (validateWs(ws)) {
            if (canInsertActivityKPI()) {
                var d = {
                    ActivityID: $('#ActivityID').val() || 0,
                    DataGroupID: $('#selGroup option:selected').val(),
                    //门店StoreCode: $("#selStore option:selected").val().trim(),
                    ActivityName: encode($('#ActivityName').val()),
                    PlanStartDate: $('#PlanStartDate').val() + " " + $('#PlanStartTime').val(),
                    PlanEndDate: $('#PlanEndDate').val() && $('#PlanEndTime').val() ? $('#PlanEndDate').val() + ' ' + $('#PlanEndTime').val() : '',
                    //PlanEndDate: $('#PlanEndDate').val()+" 23:59:59",
                    ProStartDate: $('#ProStartDate').val(),
                    BusinessPlanID: $('#drpBusinessPlan').val(),
                    ProEndDate: $('#ProEndDate').val() + " 23:59:59",
                    Remark: encode($('#Remark').val()),
                    ReferenceNo: encode($('#ReferenceNo').val()),
                    Workflow: JSON.stringify(wf.getdata()),
                    Schedule: JSON.stringify(ws),
                    Kpi: JSON.stringify(kpi)
                }, a = "/Mark/Edit",
                subdivisionIds = [];

                if (!utility.isNull(subddata)) {
                    for (var j = 0; j < subddata.length; j++) {
                        if (subddata[j].Checked) {
                            subdivisionIds.push(subddata[j].SubdivisionID);
                        }
                    }
                }

                d.SubdivisionIds = subdivisionIds.join(',');
                d.hasSids = true;
                if (!subdivisionIds.length) {
                    processErrs(["请配置会员细分信息"]);
                    $("#tabwork").click();
                } else {
                    ajax(a, d, function (r) {
                        if (r.IsPass) {
                            parentWindow.location.reload();
                            window.close();
                            $.dialog("保存成功", {}, null, function () {
                                $('#ActivityID').val(r.Obj[0].ActivityID);
                                $('#hidActivityID').val(r.Obj[0].ActivityID);
                                activityID = r.Obj[0].ActivityID;
                            });
                        } else {
                            processErrs(r.MSG);
                        }
                    })
                }
            } else {
                $('.tabbable .nav-tabs a:eq(3)').tab('show');
                return false;
            }
        } else {
            $('.tabbable .nav-tabs a:eq(2)').tab('show');
            return false;
        }
    }

    //会员细分查询
    $('#activitySubdivision').submit(function (e) {
        e.preventDefault();
        if (dtClientFollow) {
            dtClientFollow.fnDraw();
        } else {
            dtClientFollow = buildDataTable();
        }
    });

    $('#btnExport').click(function (e) {
        e.preventDefault();
        var d = $('#activitySubdivision').serializeArray(),
            f = $('<form action="/Mark/ActivitySubdivisionExport" method="post" class="hide"></form>').appendTo("body");
        for (var i in d) {
            $('<input type="hidden"/>').attr("name", d[i].name).val(d[i].value).appendTo(f);
        }
        f.submit().remove();
    });

    //缩略图
    $('#zoomIn').click(function (e) {
        e.preventDefault();
        var d = {
            activityJson: JSON.stringify(wf.getdata())
        }, a = "/Mark/GetPic";

        ajax(a, d, {
            dataType: 'text',
            callback: function (r) {
                var img = $('<img style="max-width:none"/>'),
                    dia = $.dialog(img, {
                        header: {
                            title: '流程图',
                            closebtn: '&times;',
                        }
                    }),
                    el = dia.element;

                el.css({
                    width: 'auto',
                    height: 'auto'
                });

                img.bind("resize load", function () {
                    var wh = $(window).height(),
                        ww = $(window).width(),
                        w = el.width(),
                        h = el.height();
                    if (ww < w) {
                        el.css("width", ww - 40);
                    }
                    if (wh < h) {
                        //el.css("height", wh - 40);
                        $('.modal-body', el).css('height', wh - 140)
                    }
                    el.css({
                        top: (wh - el.height()) / 2 - 10,
                        left: (ww - el.width()) / 2 - 10
                    });
                }).attr("src", "data:image/png;base64," + r);
                $('.modal-body', el).css({
                    'overflow-y': 'auto',
                    'overflow-x': 'auto',
                    'max-height': 'none'
                });
            }
        });
    });

    //选择运行方式
    $('input[name=runType]').change(function () {
        var v = $('input[name=runType]:checked').val();
        if (utility.isTrue(activityEnable) || activityStatus != "0") {
            $('#planDate,#planTime').attr('disabled', "disabled");
            $('#runCycle,#planWeek,#planDay,#planTime1,[name=planCycle]').attr('disabled', "disabled");
        } else {
            $('#planDate,#planTime').prop('disabled', v != '1');
            $('#runCycle,#planWeek,#planDay,#planTime1,[name=planCycle]').prop('disabled', v != '0');
        }

    }).change();

    //选择周期
    $('#runCycle').change(function () {
        var v = $(this).val();
        $("#cycleWeek").toggleClass("hide", v != "Week");
        $("#cycleFirstEnd").toggleClass("hide", v != "Month" && v != "Year");
        $('#cycleMonth').toggleClass('hide', v != "Month");
    }).change();

    bindGroup();
    initSubdsMultiSelect();

    $("#btnSelectOK").click(function (e) {
        e.preventDefault();
        var selSubdsData = $("#selSubds").val(),
            dv = $("#divSubd"),
            selIds = [];
        dv.html("");
        ///清空选择项
        if (!utility.isNull(subddata)) {
            for (var j = 0; j < subddata.length; j++) {
                subddata[j].Checked = false;
            }
        }

        if (!utility.isNull(selSubdsData)) {
            for (var i = 0; i < selSubdsData.length; i++) {
                var subdName = "";
                if (!utility.isNull(subddata)) {
                    for (var j = 0; j < subddata.length; j++) {
                        if (subddata[j].SubdivisionID == selSubdsData[i]) {
                            subdName = subddata[j].SubdivisionName
                            subddata[j].Checked = true;
                        }
                    }
                }
                $('<input type="text" class="span12" value="' + subdName + '" itemid="' + selSubdsData[i] + '" />').appendTo(dv);
            }
        }

        if (!utility.isNull(subddata)) {
            for (var j = 0; j < subddata.length; j++) {
                if (subddata[j].Checked) {
                    selIds.push(subddata[j].SubdivisionID)
                }
            }
        }
        //获取活动人数
        if (!utility.isNull(selIds) && selIds.length > 0) {
            getSubdivisionMemCount(selIds)
        }

        $.colorbox.close();
    });

    $("#btnCancelSelect").click(function (e) {
        e.preventDefault();
        $.colorbox.close();
    });

    //$('.btnBack').click(function () {
    //    history.back();
    //});

    $('#rdoCount,#rdoRate').change(function () {
        if ($('#rdoCount').attr("checked") == "checked") {
            $("#txtExRate").val("");
            $("#txtExCount").val("");
            if (utility.isTrue(activityEnable)) {
                $("#txtExRate").attr("disabled", "disabled");
                $("#txtExCount").attr("disabled", "disabled");
            } else {
                $("#txtExRate").attr("disabled", "disabled");
                $("#txtExCount").removeAttr("disabled", "disabled");
            }
        } else {
            $("#txtExRate").val("100");
            $("#txtExCount").val("");
            if (utility.isTrue(activityEnable)) {
                $("#txtExRate").attr("disabled", "disabled");
                $("#txtExCount").attr("disabled", "disabled");
            } else {
                $("#txtExCount").attr("disabled", "disabled");
                $("#txtExRate").removeAttr("disabled", "disabled");
            }
        }
    });

    //$("#selGroup").change(function () {
    //    bindStore($("#selGroup").val());
    //});

    //限制门店改变
    //$("#selStore").change(function () {
    //    var s = $("#selStore").val().trim();
    //    GetSubdivisionList(s);
    //});

    loadBusinessPlan();
});

$.fn.workflow = function () {
    //判断对象是不是数组
    function isarr(obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    }

    //查找指定id的节点
    function getitem(act, id) {
        if (act.id == id) {
            return act;
        }
        for (var i in act.children) {
            var chd = getitem(act.children[i], id);
            if (chd) {
                return chd;
            }
        }
    }

    //查找指定节点下的有效子节点[除去Wait.Branch节点]
    //返回结果：{act:activity,root:true/false}
    //act：返回的节点，root：是否找到有效节点
    function getitemvalid(act, id) {
        if (act.id == id) {
            return {
                act: act,
                root: 0
            };
        }
        for (var i in act.children) {
            var chd = getitemvalid(act.children[i], id);
            if (chd) {
                if (chd.root) {
                    return chd;
                }
                if (act.category != 'Branch' && act.category != 'Wait') {
                    return {
                        act: act,
                        root: 1
                    };
                }
                return chd;
            }
        }
    }

    //查找指定节点的父节点
    //返回结果：{act:activity,root:true/false}
    //act：返回的节点，root：是否找到有效节点
    function getparent(act, id) {
        if (act.id == id) {
            return {
                act: act,
                root: 0
            };
        }
        for (var i in act.children) {
            var tchd = getparent(act.children[i], id);
            if (tchd) {
                if (tchd.root) {
                    return tchd;
                }
                return {
                    act: act,
                    root: 1
                };
            }
        }
    }

    function getItemsByCategory(act, category) {
        if (!utility.isNull(act)) {
            if (act.category == category) {
                couponTempletIds.push(act.templateId);
            }
            if (!utility.isNull(act.children)) {
                for (var i = 0; i < act.children.length; i++) {
                    getItemsByCategory(act.children[i], category);
                }
            }
        }
    }

    //查找当前根节点
    //返回结果：{act:activity,root:true/false}
    //act：返回的节点，root：是否找到有效节点
    //如果找到Branch节点，系统会将该节点作为根节点
    function getroot(act) {
        if (!parentId || parentId == "" || parentId == act.id) {
            var res = {
                act: act,
                root: 0
            };
            if (act.category == "Branch") {
                res.root = 1;
            }
            return res;
        }
        for (var i in act.children) {
            var chd = act.children[i],
                tchd = getroot(chd);
            if (tchd) {
                if (tchd.root) {
                    return tchd;
                } else {
                    if (act.category == "Branch" || act.children.length > 1) {
                        return {
                            act: tchd.act,
                            root: 1
                        };
                    }
                    return {
                        act: act,
                        root: 0
                    };
                }
            }
        }
    }

    //流控制面板重绘
    function redraw(s) {
        var l = s ? s.length : 0,
            n = l > 1 ? l == 2 || l == 4 ? 2 : 3 : 1;
        if (s && s[0] && s[0] != undefined) {
            $('<li><img src="/img/ar' + n + '.png" /></li>').appendTo(that);
            var li = $('<li class="list_node"></li>').appendTo(that);
            if (l > 1) {
                li.addClass('list_node' + n);
            }
            for (var x in s) {
                var md = s[x],
                    m = datas2[md.category],
                    dv = $('<div></div>').appendTo(li);

                dv.data(dataId, md);
                $('<img/>').attr({ src: m.img, alt: m.title }).appendTo(dv);
                $('<span></span>').html(m.title).appendTo(dv);
                m.edit && $('<i class="splashy-documents_edit"></i>').appendTo(dv);
                m.remove && $('<i class="splashy-gem_remove"></i>').appendTo(dv);

                switch (m.category) {
                    case "SMS":
                    case "Mail":
                    case "WeChat":
                    case "OB":
                    case "Coupon":
                    case "Question":
                        var settings = workflowSetting[m.category],
                            tempName = "",
                            tempDay = "";
                        /////////////////////////等待时间一期暂时注释掉
                        //if (md.validday) {
                        //    tempDay = "等待时间：" + md.validday + "天 ";
                        //}
                        for (var i in settings) {
                            if (settings[i].TempletID == md.templateId) {
                                tempName = "使用模板：" + settings[i].Name;
                            }
                        }
                        $('<label></label>').html(tempName + tempDay).appendTo(dv);
                        if (m.datalimit && md.limitType !== '' && md.limitValues[0] !== '') {
                            var msg = '指定到',
                                t = !1,
                                v = !1;
                            for (var i in datalimit) {
                                if (datalimit[i].OptionValue === md.limitType) {
                                    t = datalimit[i];
                                    break;
                                }
                            }
                            if (t) {
                                if (md.limitValues.length === 1) {
                                    for (var i in t.Children) {
                                        if (t.Children[i].Str_Attr_2 === md.limitValues[0]) {
                                            v = ":" + t.Children[i].Str_Attr_1;
                                            break;
                                        }
                                    }
                                }
                                $('<label>指定范围[' + ((md.limitValues.length > 1) ? '多个' : '') + t.OptionText + (v ? v : '') + ']</label>').appendTo(dv);
                            }
                        }
                        break;
                    case "Wait":
                        if (md.wait) {
                            $('<label></label>').html("等待时间：" + md.wait + "天").appendTo(dv);
                        }
                        break;
                    case "Normal":
                        md.remark && $('<label></label>').html(md.remark).appendTo(dv);

                }
            }
        }
        if (s && s[0] && s[0] != undefined && s[0].category == "Branch") {
            return;
        }
        if (l == 1) {
            s && s[0] && s[0].children && redraw(s[0].children);
        }
    }

    //删除指定id的节点，包括该节点的子节点
    function remove(act, id) {
        if (!act.children) {
            return false;
        }
        for (var i in act.children) {
            var chd = act.children[i];
            if (chd.id == id) {
                act.children.splice(i, 1);
                if (act.children.length == 0) {
                    curflow = act;
                }
                return true;
            } else {
                if (remove(chd, id)) {
                    return true;
                }
            }
        }
    }

    //删除节点前逻辑处理
    function removeNode(md) {
        if (md && md.children && md.children.length > 0) {
            $.dialog('删除此节点会删除该节点下的所有子节点，<br/>你确定要删除吗？', {
                header: {
                    closebtn: '&times;',
                    title: '删除提示'
                },
                footer: {
                    closebtn: '取消',
                    okbtn: '删除'
                }
            }, function () {
                that.remove(md.id);
                that.redraw();
            })
        }
        else {
            if (!utility.isNull(md)) {
                if (!utility.isNull(md.id)) {
                    that.remove(md.id);
                }
            }
            that.redraw();
        }
    }

    //活动转化为树形数据
    function gettree(act, id) {
        if (act) {
            var md = datas2[act.category],
                d = {
                    title: md.title,
                    id: act.id,
                    expand: true,
                    iconClass: md.icon
                };
            if (act.id == id) {
                d.select = true;
            }
            if (act.children && act.children.length > 0) {
                d.children = [];
                for (var i in act.children) {
                    d.children.push(gettree(act.children[i], id))
                }
            }
            return d;
        }
    }

    //重绘树目录
    function redrawtree(act, id) {
        var d = activity ? gettree(activity, id) : [];
        $("#tree_workflow").dynatree({
            onDblClick: function (node) {
                parentId = node.data.id;
                curflow = getitem(activity, parentId);
                that.redraw();
            },
            persist: true,
            children: d
        }).dynatree("getTree").reload();
    }

    //获取当前目录下的最后一个节点
    function getlastchild(act) {
        if (!act) {
            return act;
        }
        if (!act.children || act.children.length == 0 || act.category == 'Branch') {
            return act;
        }
        if (act.children.length > 1) {
            return null;
        }
        return getlastchild(act.children[0]);
    }

    function validate(act, errs) {
        if (!act) {
            errs.push("活动节点不能为空！")
            return false;
        }
        var d = datas2[act.category];
        switch (act.category) {
            case "SMS":
            case "Mail":
            case "WeChat":
            case "OB":
            case "Coupon":
            case "Question":
                if (!act.templateId) {
                    errs.push(d.title + " 没有配置模板");
                    return false;
                }
                if (!/^(\d+\.)*\d+$/.test(act.validday)) {
                    errs.push(d.title + " 等待时间的格式不正确");
                    return false;
                }
                break
            case "Wait":
                if (!/^(\d+\.)*\d+$/.test(act.wait) || act.wait <= 0) {
                    errs.push(d.title + " 沟通时间间隔的格式不正确");
                    return false;
                }
                if (!act.children) {
                    errs.push(d.title + " 不能作为最后一个节点");
                    return false;
                }
                break
            case "Branch":
                if (!act.condition || act.condition === ",") {
                    errs.push(d.title + " 条件设置不正确");
                    return false;
                }
                if (!act.children) {
                    errs.push(d.title + " 不能作为最后一个节点");
                    return false;
                }
                break
        }
        for (var i in act.children || []) {
            if (!validate(act.children[i], errs)) {
                return false;
            }
        }
        return true;
    }

    function loadTemplateList() {
        ajax("/BaseData/GetActivityWorkflowTemplateList", null, function (data) {
            if (data != null) {
                var projectTemplateKeyList = data,
                    projectTemplateList = [];
                for (var i = 0; i < templateCollection.length; i++) {
                    var templateObject = templateCollection[i];
                    for (var j = 0; j < projectTemplateKeyList.length; j++) {
                        if (templateObject.category == projectTemplateKeyList[j]) {
                            projectTemplateList.push(templateObject);
                            break;
                        }
                    }
                }
            }
            datas.template.items = projectTemplateList;
            return projectTemplateList;
        });
    }
    //活动配置数据
    var projectTemplateList = loadTemplateList(),
        datas = {
            template: {
                title: '模板',
                items: projectTemplateList
            }
            //},
            //wait: {
            //    title: '等待',
            //    items: [
            //       // { category: 'Wait', icon: 'icon-feedback', title: '推迟执行', remove: 1, img: '/img/gCons/copy-item.png' }
            //    ]
            //},
            //branch: {
            //    title: '分支',
            //    items: [
            //        { category: 'Branch', icon: 'icon-branch', title: '流程分支', edit: 1, remove: 1, img: '/img/gCons/network.png' }
            //    ]
            //}
        },
        that = this,
        curflow = activity = workflowdata || null,
        parentId,
        tmpId = '#ulTemplate',
        dataId = 'workflowdata',
        datas2 = new Object(),
        couponTempletIds = []; //优惠券ID集合

    //返回活动数据
    that.getdata = function () {
        return activity;
    };

    that.validate = function () {
        var errs = [];
        if (validate(activity, errs)) {
            return true;
        }
        processErrs(errs);
        return false;
    };

    //创建节点
    that.create = function () {
        return {
            id: utility.newGuid(),
            state: 'Normal'
        }
    };

    //流程重绘
    that.redraw = function () {
        $(that).html(''),
        $('#nodesetting').html('');
        var root = activity && getroot(activity);
        if (root) {
            root = root.act;
            if (root.category == "Branch") {
                root = root.children;
            }
            if (!isarr(root)) {
                root = [root];
            }
            redraw(root);
        }
        redrawtree(activity, parentId || (activity && activity.id));
    };

    //节点删除
    that.remove = function (id) {
        if (activity.id == id) {
            activity = curflow = null;
        } else {
            var root = getparent(activity, id);
            parentId = root ? root.act.id : "";
            remove(activity, id);
        }
    };

    that.getTempletIdStringByCategory = function (category) {
        var root = activity && getroot(activity),
            Ids = "";

        couponTempletIds = [];

        if (root) {
            root = root.act;
            getItemsByCategory(root, category);
            if (!utility.isNull(couponTempletIds)) {
                for (var index = 0; index < couponTempletIds.length; index++) {
                    Ids += couponTempletIds[index] + "&&";
                }
            }
        }
        return Ids;
    }

    that.getTempletIdByCategory = function (category) {
        var root = activity && getroot(activity);
        couponTempletIds = []
        if (root) {
            root = root.act;
            getItemsByCategory(root, category);
        }
        return couponTempletIds;
    }

    //初始化流程面板
    that.init = function () {
        //生成模板数据
        $(tmpId).html('');
        for (var i in datas) {
            var g = datas[i];
            $('<li class="list_heading"></li>').html(g.title).appendTo(tmpId);
            for (var j in g.items) {
                var l = $('<li class="list_node"></li>').appendTo(tmpId), m = g.items[j];
                if (i === "template" && !workflowSetting[m.category] && m.category != 'Normal') {
                    //l.addClass('disabled');
                }
                l.data(dataId, m);
                m.css && l.addClass(m.css);
                l = $('<div></div>').appendTo(l);
                $('<img/>').attr({ src: m.img, alt: m.title }).appendTo(l);
                $('<span/>').html(m.title).appendTo(l);
                m.edit && $('<i class="splashy-documents_edit"></i>').appendTo(l);
                m.remove && $('<i class="splashy-gem_remove"></i>').appendTo(l);
                datas2[m.category] = m;
            }
        }
        $('.node_list .list_node').mousemove(function () {
            $(this).css("cursor", "move");
        }).mouseout(function () {
            $(this).css("cursor", "none");
        })

        //为模板绑定拖动事件
        $('.node_list .list_node').draggable({
            proxy: 'clone',
            revert: true,
            cursor: 'move',
            onBeforeDrag: function () {
                return !$(this).hasClass('disabled')
            },
            onStartDrag: function () {
                $(this).draggable('proxy').addClass('dp')
            }
        });
        //为接受模板绑定时间
        that.droppable({
            accept: '.list_node',
            onDragEnter: function (e, source) {
                $(this).addClass('over');
            },
            onDragLeave: function (e, source) {
                $(this).removeClass('over');
            },
            onDrop: function (e, ui) {
                var el = $(ui),
                    h = el.html(),
                    d = el.data(dataId),
                    flow = getlastchild(curflow);
                //判断是否在分支后添加子节点
                //Branch节点后不能添加子节点
                if (curflow && (!flow || flow.category == 'Branch' && parentId != flow.id)) {
                    $.dialog('请点击流程分支的编辑按钮');
                    return;
                }
                if (flow) {
                    curflow = flow;
                }
                //判断是否在跟节点下添加Branch，Wait节点
                //根节点下不能添加Branch，Wait节点
                if ((!activity || (curflow && curflow.category == 'Branch' && parentId == curflow.id))
                    && (d.category == "Branch" || d.category == "Wait")) {
                    $.dialog('请先添加模板');
                    return;
                }
                if (d && d.category == "Branch") {
                    var parent = curflow;
                    if (parent.category == 'Wait') {
                        parent = getparent(activity, curflow.id).act;
                    }
                    if (parent.category != "Question" && parent.category != "SMS") {
                        $.dialog('该模板不能添加节点');
                        return;
                    }
                    //添加Branch节点处理
                    //让用户选择添加几个Branch节点[2~6个]
                    var html = $('<div style="padding:10px 30px;"/>'),
                        dlg;
                    for (var x = 2; x <= 6; x++) {
                        $('<a href="javascript:void(0)" class="btn btn-primary">' + x + '个</a>&nbsp;').attr('data-val', x).appendTo(html)
                    }
                    $('.btn', html).click(function () {
                        var val = parseInt($(this).attr('data-val'));
                        if (!curflow.children) {
                            curflow.children = [];
                        }
                        for (var x = 0; x < val; x++) {
                            var tmpflow = that.create();
                            tmpflow.category = d.category;
                            curflow.children.push(tmpflow);
                        }
                        that.redraw();
                        dlg && dlg.remove();
                    });
                    dlg = $.dialog(html, { header: { closebtn: '&times;', title: '子节点数量设置' } });
                } else {
                    //添加模板节点或等待节点
                    var setting = workflowSetting[d.category], tmpflow = that.create();
                    tmpflow.category = d.category;
                    tmpflow.resulttype = d.resulttype;
                    tmpflow.datalimit = !!d.datalimit;
                    if (tmpflow.datalimit) {
                        tmpflow.limitType = '';
                        tmpflow.limitValues = [''];
                    }
                    if (d.category == "Wait") {
                        tmpflow.wait = 0;
                    } else {
                        tmpflow.validday = 3;
                        if (setting) {
                            tmpflow.templateId = setting[setting.length - 1].TempletID;
                        }
                        switch (d.category) {
                            case 'Coupon'://优惠券 -> [发送短信，发送邮件]
                                tmpflow.sendMail = tmpflow.sendSMS = !0;      //************************************新增**********************************//
                                break;
                            case 'Question'://问卷调查 -> [推送渠道]
                                tmpflow.sendChannel = sendChannel ? sendChannel[0].ExtCode : '';
                                break;
                        }
                    }
                    if (!activity) {
                        parentId = "";
                        activity = curflow = tmpflow;
                    } else {
                        if (!curflow.children) curflow.children = [];
                        curflow.children.push(tmpflow);
                    }
                    curflow = tmpflow;
                    that.redraw();
                }
            }
        });

        function setDlv(selItem, selVals) {
            var vals = selVals || [];
            selItem = selItem ||
                {
                    Children: [
                        {
                            Str_Attr_2: '',
                            Str_Attr_1: '全部'
                        }]
                };
            var dlv = $('.dataLimitValues').html('');
            for (var i in selItem.Children) {
                var item = selItem.Children[i],
                    opt = $('<option value="' + item.Str_Attr_2 + '">' + item.Str_Attr_1 + '</option>').appendTo(dlv);
                if (vals.indexOf(item.Str_Attr_2) != -1) {
                    opt.attr('selected', true);
                }
            }
            dlv.trigger("liszt:updated");
        }

        $('#holder').on('click', '.list_node div', function () {
            //设置选中样式
            $('#holder .list_node div').removeClass('active');
            $(this).addClass('active');
            var dv = $(this),
                md = dv.data(dataId);
            if (!md) {
                return;
            }
            var da = getitemvalid(activity, md.id),
                db = getitem(activity, md.id),
                ds = workflowSetting[md.category],
                settingId = '#nodesetting';
            $(settingId).html('');

            //编辑节点属性
            switch (md.category) {
                case 'Wait'://等待反馈 -> [等待时间]
                    var d = $('<div class="span5"></div>').appendTo(settingId);
                    $('<label>推迟时间(天)</label>').appendTo(d);
                    $('<input type="text" class="span12 wait"/>').val(db.wait).appendTo(d);
                    break;
                case 'Branch'://流程分支 -> [条件：字符串，数字]
                    if (da.act.resulttype == "Number") {
                        var condition = (db.condition || ",").split(',');
                        var d = $('<div class="span5"></div>').appendTo(settingId);
                        $('<label>条件范围</label>').appendTo(d);
                        $('<input type="text" class="span12 minval"/>').val(condition[0]).appendTo(d);
                        d = $('<div class="span5"/>').appendTo(settingId);
                        $('<label>&nbsp;</label>').appendTo(d);
                        $('<input type="text" class="span12 maxval"/>').val(condition[1]).appendTo(d);
                    } else {
                        var d = $('<div class="span10"></div>').appendTo(settingId);
                        $('<label>条件范围</label>').appendTo(d);
                        $('<input type="text" class="span12 vals"/>').val(db.condition).appendTo(d);
                    }
                    break;
                case 'SMS'://短信沟通 -> [等待时间，模板]
                case 'Mail'://邮件沟通 -> [等待时间，模板] 
                case "WeChat":
                case 'OB'://外呼沟通 -> [等待时间，模板]
                case 'Coupon'://优惠券 -> [等待时间，模板]
                case 'Question'://问卷调查 -> [等待时间，模板]
                    var d1 = $('<div class="row-fluid"></div>').appendTo(settingId);
                    var d = $('<div class="span5"></div>').appendTo(d1);
                    /////////////////////////等待时间一期暂时注释掉
                    //$('<label>等待时间(天)</label>').appendTo(d);
                    //$('<input type="text" class="span12 validday"/>').val(db.validday).appendTo(d);
                    //d = $('<div class="span5"></div>').appendTo(d1);
                    //$('<label>剩余数量(张)</label>').appendTo(d);
                    //$('<input type="text" class="span12 validday"/>').val(db.validday).appendTo(d);
                    //d = $('<div class="span5"></div>').appendTo(d1);
                    $('<label>模板</label>').appendTo(d);
                    //var s = $('<select class="chzn_b span12 templateId"/>').appendTo(d);
                    var s = $('<select class="chzn_b span12 templateId" onchange="loadTemplateBasicInfo(this,\'' + md.category + '\')"/>').appendTo(d);
                    var categoryArr = [];
                    for (var i in ds) {
                        var isExist = false;
                        if (categoryArr != null && categoryArr.length > 0) {
                            for (var c in categoryArr) {
                                if (categoryArr[c].Category == ds[i].Category)
                                    isExist = true;
                            }
                        }
                        if (!isExist)
                            categoryArr.push({ Category: ds[i].Category, CategoryText: ds[i].CategoryText });
                    }
                    if (categoryArr != null && categoryArr.length > 0) {
                        for (var j in categoryArr) {
                            $('<optgroup value="' + categoryArr[j].Category + '" label="' + categoryArr[j].CategoryText + '"></optgroup>').appendTo(s)
                        }
                    }
                    for (var i in ds) {
                        var selectObj = s.find("optgroup[value='" + ds[i].Category + "']")
                        $('<option value="' + ds[i].TempletID + '">' + ds[i].Name + '</option>').appendTo(selectObj)
                    }
                    s.val(db.templateId).chosen();

                    //if (md.datalimit) {
                    //    d1 = $('<div class="row-fluid"></div>').appendTo(settingId);
                    //    d = $('<div class="span5"></div>').appendTo(d1);
                    //    $('<label>范围</label>').appendTo(d);
                    //    var dlt = $('<select class="chzn_b span12 dataLimitType"></select>').appendTo(d);
                    //    $('<option value="">全部</option>').appendTo(dlt);
                    //    var selItem = !1;
                    //    for (var i in datalimit) {
                    //        var item = datalimit[i];
                    //        var opt = $('<option value="' + item.OptionValue + '">' + item.OptionText + '</option>').data('item', item).appendTo(dlt);
                    //        if (db.limitType === item.OptionValue) {
                    //            opt.attr('selected', true);
                    //            selItem = item;
                    //        }
                    //    }
                    //    dlt.change(function () {
                    //        var optItem = $('option:selected', this).data('item');
                    //        setDlv(optItem, []);
                    //    }).chosen();
                    //    d = $('<div class="span5"></div>').appendTo(d1);
                    //    $('<label>值</label>').appendTo(d);
                    //    var dlv = $('<select class="chzn_b span12 dataLimitValues" data-placeholder="请选择..." multiple></select>').appendTo(d).chosen();
                    //    setDlv(selItem, db.limitValues);
                    //}

                    switch (md.category) {
                        //case 'Coupon'://优惠券 -> [发送短信，发送邮件]

                        //    //加发送方式
                        //    var stype = $('<label>发送方式</label><select class="span12 drpSendType"></select>').appendTo(d);
                        //    loadSendType();
                        //    stype.val(db.sendtype);
                        //    var cnum = $('<label>张数</label><input class="span12 txtNumber" type="text" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,\'\')}else{this.value=this.value.replace(/\\D/g,\'\')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,\'\')}else{this.value=this.value.replace(/\\D/g,\'\')}" value="" />').appendTo(d);
                        //    cnum.val(db.number);

                        //    d1 = $('<div class="row-fluid"></div>').appendTo(settingId);
                        //    d = $('<div class="span5"></div>').appendTo(d1);
                        //    $('<label><input type="checkbox" ' + (db.sendSMS ? 'checked ' : '') + 'id="sendSMS"/>发送短信</label>').appendTo(d);
                        //    d = $('<div class="span5"></div>').appendTo(d1);
                        //    $('<label id="lblOtherIndustryCouponAmount"></label>').appendTo(d);
                        //    loadTemplateBasicInfo(s, "Coupon");
                        //    d = $('<div class="span5"></div>').appendTo(d1);
                        //    $('<label style="display:none;"><input type="checkbox" ' + (db.sendMail ? 'checked ' : '') + 'id="sendMail"/>发送邮件</label>').appendTo(d);
                            //d2 = $('<div class="row-fluid"></div>').appendTo(settingId);
                            //d = $('<div class="span5">起始日期</div>').appendTo(d2);
                            //$('<input type="text" class="span12 couponStartDate" value="" readonly="readonly"/>').datepicker().appendTo(d);
                            //d = $('<div class="span5">结束日期</div>').appendTo(d2);
                            //$('<input type="text" class="span12 couponEndDate" value="" disabled="disabled" />').datepicker().appendTo(d);
                            //break;
                        case 'Question'://问卷调查 -> [推送渠道]
                            var sc = sendChannel;
                            d1 = $('<div class="row-fluid"></div>').appendTo(settingId);
                            d = $('<div class="span5"></div>').appendTo(d1);
                            $('<label>推送渠道</label>').appendTo(d);
                            var s = $('<select class="span12 sendChannel"/>').appendTo(d);
                            for (var i in sc) {
                                $('<option value="' + sc[i].ExtCode + '">' + sc[i].ExtValue + '</option>').appendTo(s)
                            }
                            s.val(db.sendChannel);
                            break;
                    }
                    break;
                case 'Normal':
                    var d = $('<div class="span12"></div>').appendTo(settingId);
                    $('<label>备注</label>').appendTo(d);
                    $('<textarea type="text" class="span10 remark"/>').val(db.remark).appendTo(d);
                    break;
            }
            var d2 = $('<div class="row-fluid"></div>').appendTo(settingId);
            d2 = $('<div class="span10 sepH_c" id="dvSave"></div>').appendTo(d2).toggleClass("hide", !!actived);
            $('<button class="btn btn-info" type="button">保存</button>&nbsp;')
                .appendTo(d2)
                .click(function () {
                    //保存节点属性
                    switch (md.category) {
                        case 'Wait'://等待反馈 -> [等待时间]
                            db.wait = $('.wait', settingId).val();
                            break;
                        case 'Branch'://流程分支 -> [条件：字符串，数字]
                            if (da.act.resulttype == "Number") {
                                db.condition = $('.minval', settingId).val() + ',' + $('.maxval', settingId).val();
                            } else {
                                db.condition = $('.vals', settingId).val();
                            }
                            break;
                        case 'Coupon'://优惠券 -> [等待时间，模板]
                        case 'Question'://问卷调查 -> [等待时间，模板]
                        case 'OB'://外呼沟通 -> [等待时间，模板]
                        case "WeChat":
                        case 'Mail'://邮件沟通 -> [等待时间，模板] 
                        case 'SMS'://短信沟通 -> [等待时间，模板]
                            /////////////////////////等待时间一期暂时注释掉
                            //db.validday = $('.validday', settingId).val();
                            db.validday = 0;
                            db.templateId = $('.templateId', settingId).val();
                            if (md.datalimit) {
                                db.limitType = $('.dataLimitType', settingId).val();
                                db.limitValues = $('.dataLimitValues', settingId).val() || [''];
                            }
                            switch (md.category) {
                                case 'Coupon'://优惠券 -> [发送短信，发送邮件]

                                    db.sendtype = $('.drpSendType', settingId).val();
                                    db.number = $('.txtNumber', settingId).val();

                                    db.sendMail = $('#sendMail').is(':checked');
                                    db.sendSMS = $('#sendSMS').is(':checked');
                                    break;
                                case 'Question'://问卷调查 -> [推送渠道]
                                    db.sendChannel = $('.sendChannel').val();
                                    break;
                            }
                            break;
                        case 'Normal':
                            db.remark = $('.remark').val();
                            break;
                    }
                    $.dialog("修改成功！", {}, null, function () {
                        that.redraw();
                    });
                });
            $('<button class="btn" type="button">删除此节点</button>&nbsp;').appendTo(d2).click(function () {
                removeNode(md);
            });
        }).on('click', '.list_node .splashy-documents_edit', function () {
            //将Branch设置为根节点
            var dv = $(this).closest('div'),
                md = dv.data(dataId);
            parentId = md.id;
            curflow = getitem(activity, parentId);
            that.redraw();
        }).on('click', '.list_node .splashy-gem_remove', function () {
            //删除节点
            var dv = $(this).closest('div'),
                md = dv.data(dataId);
            removeNode(md);
        });
    };

    //that.init();
    //that.redraw();
    return that;
}

function loadTemplateBasicInfo(obj, modelCategory) {
    $("#lblOtherIndustryCouponAmount").html("");
    if (!utility.isNull(obj) && !utility.isNull(workflowSetting) && !utility.isNull(modelCategory)) {
        var list = workflowSetting[modelCategory];
        if (!utility.isNull(obj)) {
            var templateId = $(obj).val();
            for (var i = 0; i < list.length; i++) {
                var tempObj = list[i];
                if (tempObj.TempletID == templateId && JSON.parse(tempObj.BasicContent).isOthers == true) {
                    $("#lblOtherIndustryCouponAmount").html("剩余数量(张):" + tempObj.OtherIndustryCouponAmount);
                    return;
                }
            }
        }
    }
}

function loadSendType() {

    ajaxSync("/BaseData/GetOptionDataList", { optType: "CouponSendType" }, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            //return opt;
            $('.drpSendType').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            //return opt;
            $('.drpSendType').append(opt);
        }
    });
}

function calculateOtherIndustryCouponAmount(ids, category) {
    haveOtherCoupon = false;

    var amount = 0;
    if (!utility.isNull(ids) && !utility.isNull(workflowSetting) && !utility.isNull(category)) {
        var list = workflowSetting[category];
        for (var index = 0; index < ids.length; index++) {
            var templateId = ids[index];
            for (var i = 0; i < list.length; i++) {
                var tempObj = list[i],
                    tempObjBasicContent = JSON.parse(tempObj.BasicContent);
                if (tempObj.TempletID == templateId && !utility.isNull(tempObjBasicContent) && !utility.isNull(tempObjBasicContent.isOthers)
                    && tempObjBasicContent.isOthers == true) {
                    haveOtherCoupon = true;
                    amount += tempObj.OtherIndustryCouponAmount;
                    break;
                }
            }
        }
    }
    return amount;
}
//function loadTemplateBasicInfo(obj, modelCategory) {
//    if (!utility.isNull(obj) && !utility.isNull(workflowSetting) && !utility.isNull(modelCategory)) {
//        var list = workflowSetting[modelCategory];
//        if (!utility.isNull(obj)) {
//            var templateId = $(obj).val();
//            for (var i = 0; i < list.length; i++) {
//                var tempObj = list[i];
//                if (tempObj.TempletID == templateId) {
//                    var couponBasciInfo = JSON.parse(tempObj.BasicContent);
//                    $(".couponStartDate").val(couponBasciInfo.startdate);
//                    $(".couponEndDate").val(couponBasciInfo.enddate);
//                }
//            }
//        }
//    }
//}

function loadBusinessPlan() {
    ajax("/BusinessPlan/GetAllEnableBusinessPlan", null, function (data) {
        $("#drpBusinessPlan").html("");
        var options = "<option value=''>无</option>";
        if (data && data.length > 1) {
            for (var i = 0; i < data.length; i++) {
                options += "<option value='" + data[i].BusinessPlanID + "'>" + data[i].BusinessPlanName + "</option>";
            }
        }
        $("#drpBusinessPlan").html(options);
        $("#drpBusinessPlan").trigger("liszt:updated");
        var businessPlanID = $("#hidBusinessPlanID").val();

        if (!utility.isNull(businessPlanID)) {
            $("#drpBusinessPlan").val(businessPlanID);
        }
    });
}

//绑定群组
function bindGroup() {
    var postUrl = "/MemSubdivision/GetDataGroups";
    ajax(postUrl, null, function (res) {
        if (utility.isNull(res)) {
            return;
        } else {
            var objHtml = "";
            for (var i = 0; i < res.length; i++) {
                if (i == 0) {
                    objHtml += "<option value='" + res[i].SubDataGroupID + "' selected='selected'>" + res[i].SubDataGroupName + "</option>";
                } else {
                    objHtml += "<option value='" + res[i].SubDataGroupID + "'>" + res[i].SubDataGroupName + "</option>";
                }
            }
            $("#selGroup").html(objHtml);

            //bindStore(res[0].SubDataGroupID);
        }
    });
}
//设置执行数据
function setws(d) {
    if (d) {
        d.runType && $('#runType' + d.runType).prop('checked', true);
        d.planDate && $('#planDate').val(d.planDate);
        d.planTime && $('#planTime').val(d.planTime);
        d.runCycle && $('#runCycle').val(d.runCycle);
        d.planCycle && $('#planCycle' + d.planCycle).prop('checked', true);
        d.planDay && $('#planDay').val(d.planDay);
        d.planTime1 && $('#planTime1').val(d.planTime1);
        d.planWeek && $('#planWeek').val(d.planWeek);
        if (utility.isNull(d.CustomerSelectType)) {
            if (d.CustomerSelectType == "Amount") {
                $('#rdoCount').attr("checked", "checked");
                $('#rdoRate').removeAttr("checked");
            } else {
                $('#rdoRate').attr("checked", "checked");
                $('#rdoCount').removeAttr("checked");
            }
        }
        d.CustomerRate && $('#txtExRate').val(d.CustomerRate);
        d.CustomerAmount && $('#txtExCount').val(d.CustomerAmount);
        d.ActAllCount && $('#txtPersonCount').val(d.ActAllCount);
    }
}
//获取执行数据
function getws() {
    return {
        runType: $('[name=runType]:checked').val(),
        planDate: $('#planDate').val(),
        planTime: $('#planTime').val(),
        runCycle: $('#runCycle').val(),
        planCycle: $('[name=planCycle]:checked').val(),
        planDay: $('#planDay').val(),
        planTime1: $('#planTime1').val(),
        planWeek: $('#planWeek').val(),
        CustomerSelectType: $("#rdoRate").attr("checked") == "checked" ? "Rate" : "Amount",
        CustomerRate: $("#txtExRate").val(),
        CustomerAmount: $("#txtExCount").val(),
        ActAllCount: $("#txtPersonCount").val()
    }
}

function validateWs(ws) {
    var errs = [],
        flag = true;
    if ($('#rdoCount').attr("checked") == "checked") {
        if (utility.isNull(ws.CustomerAmount)) {
            errs.push("执行数量不能为空");
            return false;
        }
    }

    if ($('#rdoRate').attr("checked") == "checked") {
        if (utility.isNull(ws.CustomerRate)) {
            errs.push("执行比例不能为空");
            return false;
        }
    }

    if (!utility.isNull(ws.CustomerRate)) {
        if (ws.CustomerRate <= 0 || ws.CustomerRate > 100) {
            errs.push("比例范围错误，只能在0-100之间");
            return false;
        }
    }

    if (!utility.isNull(ws.CustomerAmount)) {
        if (ws.CustomerAmount > ws.ActAllCount) {
            errs.push("执行数量不能大于总数量");
            return false;
        }
        if (ws.CustomerAmount <= 0) {
            errs.push("执行数量不能小于1");
            return false;
        }
    }

    switch (ws.runType) {
        case "1":
            if (!ws.planDate) {
                errs.push("你没有指定日期");
                flag = false;
            }
            if (!ws.planTime) {
                errs.push("你没有指定时间");
                flag = false;
            }
            var pStart = new Date($('#PlanStartDate').val().replace(/\-/g, '/') + ' ' + $('#PlanStartTime').val());
            var spEnd = $('#PlanEndDate').val().replace(/\-/g, '/') + ' ' + $('#PlanEndTime').val();
            var pEnd = spEnd.length < 12 ? null : new Date(spEnd);
            var rStart = new Date(ws.planDate.replace(/\-/g, '/') + ' ' + ws.planTime);
            //console.log(pStart, spEnd, pEnd, rStart)
            if (pStart >= rStart) {
                errs.push("执行时间不能小于等于计划开始时间");
                flag = false;
            }
            if (pEnd && pEnd <= rStart) {
                errs.push("执行时间不能大于等于计划结束时间");
                flag = false;
            }
            break;
        case "0":
            if (!ws.planTime1) {
                errs.push("你没有指定时间");
                flag = false;
            }
            break;
    }
    if (!flag) {
        processErrs(errs);
    }
    return flag;
}

function buildDataTable() {
    return $("#dtClientFollow").dataTable({
        sAjaxSource: '/Mark/GetActivitySubdivision',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'CardNo', title: "会员卡号", sortable: false, sWidth: "100px" },
            { data: 'Name', title: "会员姓名", sortable: false, sWidth: "100px" },
            { data: 'Mobile', title: "手机号", sortable: false, sWidth: "100px" },
            { data: 'Email', title: "邮箱", sortable: true, sWidth: "100px" },
            { data: 'SubdivisionName', title: "会员细分", sortable: true, sWidth: "100px" },
            {
                data: null, title: "跟踪结果", sortable: true, sWidth: "100px", render: function (r) {
                    //return r.Mobile == '' ? '' : r.WfRootId;
                    return r.WfRootId;
                }
            },
            //{ data: 'Templates', title: "关联问卷", sortable: false, sWidth: "100px", bVisible: false },
        ],
        fnFixData: function (d) {
            d.push({ name: 'Instance', value: $("#Instance option:selected").val() });
            d.push({ name: 'SubdivisionId', value: $("#SubdivisionId option:selected").val() });
            d.push({ name: 'Name', value: $("#Name").val() });
            d.push({ name: 'CardNo', value: $("#CardNo").val() });
        }
    });
}

//显示选择会员细分窗口
function showSelSubd() {
    var subids = [];
    if (!utility.isNull(subddata)) {
        for (var i = 0; i < subddata.length; i++) {
            if (subddata[i].Checked) {
                subids.push(subddata[i].SubdivisionID);
            }
        }
    }

    if (!utility.isNull(subids) && subids.length > 0) {
        $("#selSubds").val(subids).multiSelect("refresh");
    } else {
        $("#selSubds").val("").multiSelect("refresh");
    }

    $('#txtSearchSubds').quicksearch($(".ms-elem-selectable", "#ms-selSubds")).on("keydown, input", function (e) {
        if (e.keyCode == 40) {
            $(this).trigger("focusout");
            $("#ms-drpRolePages").focus();
            return false;
        }
        if ($(this).val() == '') {
            $(this).quicksearch($(".ms-elem-selectable", "#ms-selSubds"));
        }
    });
    //$("#drpRolePages").val("").multiSelect("refresh");//.init();//.select("2", "init");
    //$("#drpRolePages").multiSelect("select_all");
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#subds_dialog",
        inline: true
    });
}

//加载multi-select所有页面
function initSubdsMultiSelect() {
    $("#selSubds").multiSelect({
        //cssClass: "height: 300px",
        selectableHeader: "<div class='search-header'><input type='text' class='span12' id='txtSearchSubds' autocomplete='on' placeholder='查找会员细分...'></div>",
        selectionHeader: "<div class='search-selected'>已选会员细分</div>"
    });

    $("#UpdateSelSubdSelectAll").on("click", function () {                            //全选事件
        $("#selSubds").multiSelect("select_all");
        return false;
    });

    $("#UpdateSelSubdDeSelectAll").on("click", function () {                          //取消全选事件
        $("#selSubds").multiSelect("deselect_all");
        return false;
    });
}

//获取活动细分总人数
function getSubdivisionMemCount(ids) {
    var postUrl = "/Mark/GetSubdivisionMemCount";
    ajax(postUrl, { subIds: JSON.stringify(ids) }, function (res) {
        if (utility.isNull(res)) {
            return;
        } else {
            $("#txtPersonCount").val(res)
        }
    });
}

//跳转细分
function goSubd() {
    $.dialog("确认跳转到会员细分页面吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        $.go('/MemSubdivision/Index');
    })
}

//绑定群组
//function bindStore(groupId) {
//    var postUrl = "/MemSubdivision/GetDataGroupStore";
//    ajaxSync(postUrl, { groupId: groupId }, function (res) {
//        $("#selStore").html("");
//        if (utility.isNull(res)) {
//            return;
//        } else {
//            var objHtml = "";
//            if (res.length > 1) {
//                objHtml = "<option value=''>请选择</option>";
//            }
//            for (var i = 0; i < res.length; i++) {
//                objHtml += "<option value='" + res[i].StoreCode + "'>" + res[i].StoreName + "</option>";
//            }
//            $("#selStore").html(objHtml);
//            if (res.length == 1) {
//                GetSubdivisionList(res[0].StoreCode);//过滤会员细分
//            }
//        }
//    });
//}

function GetSubdivisionList(s) {
    $('#selSubds').empty();
    ajax("/Mark/GetMemberSubdivisionListByStore", { store: s }, function (res) {
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value="' + res[i].SubdivisionID + '">' + res[i].SubdivisionName + '</option>';
            }
            $('#selSubds').append(opt).multiSelect("refresh");

            //搜索套餐
            $('#txtSearchSubds').quicksearch($(".ms-elem-selectable", "#ms-selSubds")).on("keydown, input", function (e) {
                if (e.keyCode == 40) {
                    $(this).trigger("focusout");
                    $("#ms-selSubds").focus();
                    return false;
                }
                if ($(this).val() == '') {
                    $(this).quicksearch($(".ms-elem-selectable", "#ms-selSubds"));
                }
            });
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#selSubds').append(opt);
        }
    });
}

function hasEnoughCouponAmount() {
    wf.couponTempletIds = [];
    var personAmount = $("#txtPersonCount").val(),
        templetCouponAmount = calculateOtherIndustryCouponAmount(wf.getTempletIdByCategory("Coupon"), "Coupon");
    if (parseInt(personAmount) <= parseInt(templetCouponAmount)) { //当异业券的数量>人数时返回true;
        return true;
    }
    return false;
}

///////////////////////////////////////////////////KPI相关

var dtkpi,
    dtKpiHistory,
    activityID,
    activityEnable,
    activityStatus,
    activityKpiInfo,
    kpi = [],
    kpiForPage = [];


function pageInit() {
    if (!utility.isNull(activityID)) {
        loadActivityKPI(activityID);
    }

    pageElementDisabledInit();
}

function pageElementDisabledInit() {
    if (activityStatus == "0") {    //如果为提交申请,则可以编辑,不可以激活和取消激活--(理论上不存在激活状态)
        if (utility.isTrue(activityEnable)) {//当前状态为激活,则不可修改
            disableForm("frmBaseInfo");
            disableForm("frmWorkFlow");
            disableForm("frmTimeSetting");
            disableForm("formKPISettings");
        }
        $('.btnBack').removeAttr("disabled");
        $('.btnSave,#dvSave').show();
        $("#btnDeactive").hide();
        $("#btnActive").hide();
    } else {                        //如果为审核通过,则不可以编辑和删除,只能查询、激活和取消激活
        disableForm("frmBaseInfo");
        disableForm("frmWorkFlow");
        disableForm("frmTimeSetting");
        disableForm("formKPISettings");
        $('.btnBack').removeAttr("disabled");
        $("#btnDeactive").hide();
        $("#btnActive").hide();
        $('.btnSave,#dvSave').hide();
        if (utility.isTrue(activityEnable)) {//当前状态为激活则取消激活可用
            $("#btnDeactive").show();
            $('#btnDeactive').removeAttr("disabled");
        } else {
            $("#btnActive").show();
            $('#btnActive').removeAttr("disabled");
        }
    }
}

function loadActivityKPI(activityID) {
    ajax("/Mark/SearchActivityKPITarget", { activityID: activityID }, function (data) {
        if (data == null) {
            return;
        } else {
            for (var i = 0; i < data.length; i++) {
                var o = {
                    "KPIID": data[i].KPIID, "KPIName": data[i].KPIName, "KPIType": data[i].KPIType, "TargetValueType": data[i].TargetValueType, "KPITypeValue": data[i].KPITypeValue,
                    "Unit": data[i].Unit, "IntValue1": data[i].IntValue1, "DecValue1": data[i].DecValue1, "DecValue2": data[i].DecValue2, "StrValue1": data[i].StrValue1
                }
                kpi.push(o);
            }
            loadActivityKPICallback();
        }
    });
};

function loadActivityKPICallback() {
    for (var i = kpi.length - 1; i >= 0; i--) {
        var currentKPI = kpi[i];
        var html = "";
        if (currentKPI.TargetValueType == "2") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.StrValue1 + "\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else if (currentKPI.TargetValueType == "3") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.IntValue1 + "\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        } else if (currentKPI.TargetValueType == "7") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtMinKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.DecValue1 + "\"/>--"
                + "<input type=\"text\" id=\"txtMaxKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.DecValue2 + "\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\" value=\"" + currentKPI.StrValue1 + "\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        }
        $("#divKPIList").append(html);
    }
    kpiForPage = [];
    $.colorbox.close();
}

function loadKPI() {
    if (dtkpi) {
        dtkpi.fnDestroy();
    }

    dtkpi = $('#dt_KPIList').dataTable({
        sAjaxSource: "/Mark/GetKPIData",
        bInfo: true,
        bSort: true,
        bInfo: false,
        searching: true,
        bServerSide: false,
        bLengthChange: false,
        bLengthChange: false,
        bPaginate: true,
        iDisplayLength: 8,
        aoColumns: [
            { data: 'KPIName', title: "指标名称", sortable: false, "sClass": "center", sWidth: "55%" },
            {
                data: null, title: "选择", sortable: false, "sClass": "center", sWidth: "45%", render: function (r) {
                    var isExist = false;
                    for (var i = 0; i < kpi.length; i++) {
                        var currentKPI = kpi[i];
                        if (currentKPI.KPIID == r.KPIID) {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist) {
                        return "<input type=\"checkbox\" onchange=\"addkpi('" + r.KPIID + "','" + r.KPIName
                            + "','" + r.KPIType + "','" + r.Unit + "','" + r.TargetValueType + "')\" />";
                    } else {
                        return "";
                    }
                }
            }
        ]
    });
}

//加载指标列表页面
function showKPIdialog() {
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        title: '选择指标',
        href: "#addKPI_dialog",
        inline: true
    });
    kpiForPage = [];
    loadKPI();
}

//单个勾选
function addkpi(kpiID, kpiName, kpiType, unit, targetValueType) {
    for (var i = 0; i < kpi.length; i++) {
        if (kpi[i].KPIID == kpiID) {
            kpi.splice(i, 1);
            break;
        }
    }

    var o = { "KPIID": kpiID, "KPIName": kpiName, "KPIType": kpiType, "TargetValueType": targetValueType, "KPITypeValue": activityID, "Unit": unit, "IntValue1": "", "DecValue1": "", "DecValue2": "", "StrValue1": "" }
    kpiForPage.push(o);

}

//将选择的指标显示到页面上
function showKPI() {
    for (var i = kpiForPage.length - 1; i >= 0; i--) {
        var currentKPI = kpiForPage[i];
        var html = "";
        if (currentKPI.TargetValueType == "2") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else if (currentKPI.TargetValueType == "3") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        } else if (currentKPI.TargetValueType == "7") {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                + "<div class=\"span2\">"
                + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                + currentKPI.KPIName
                + "(" + currentKPI.Unit + ")"
                + "<span></span>"
                + "</div>"
                + "<div class=\"span10\">"
                + "<input type=\"text\" id=\"txtMinKPI_" + currentKPI.KPIID + "\" required=\"required\"/>--"
                + "<input type=\"text\" id=\"txtMaxKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                + "<span class=\"help-block\"></span>"
                + "</div>"
                + "</div>";
        } else {
            html = "<div id=\"liKPD_" + currentKPI.KPIID + "\" class=\"span12\" style=\"margin-left:0;\">"
                 + "<div class=\"span2\">"
                 + "<i class=\"splashy-gem_cancel_2\" onclick=\"deletekpi(this)\"></i>"
                 + currentKPI.KPIName
                 + "(" + currentKPI.Unit + ")"
                 + "<span></span>"
                 + "</div>"
                 + "<div class=\"span10\">"
                 + "<input type=\"text\" id=\"txtKPI_" + currentKPI.KPIID + "\" required=\"required\"/>"
                 + "<span class=\"help-block\"></span>"
                 + "</div>"
                 + "</div>";
        }

        var flag = false; //如果不存在
        for (var index = 0; index < kpi.length; index++) {
            if (kpi[index].KPIID == currentKPI.KPIID) {
                flag = true;
            }
        }

        if (!flag) {
            kpi.push(currentKPI);
        }

        $("#divKPIList").append(html);
    }
    kpiForPage = [];
    if (kpi.length > 0) {
        $("#btnsave").show();
    }
    $.colorbox.close();
}

function deletekpi(o) {
    if (o.parentNode.parentNode) {
        var kpiID = o.parentNode.parentNode.id;
        if (kpiID) {
            kpiID = kpiID.substr(6);
        }

        $("#liKPD_" + kpiID).remove();
        //o.parentNode.parentNode.remove();

        for (var i = 0; i < kpi.length; i++) {
            if (kpi[i].KPIID == kpiID) {
                kpi.splice(i, 1);
                break;
            }
        }
    }

}

function canInsertActivityKPI() {
    if (kpi.length > 0) {
        for (var i = 0; i < kpi.length; i++) {
            var obj = kpi[i];
            if (obj.TargetValueType == "2") {
                if (utility.isNull($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "不能为空");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }
            } else if (obj.TargetValueType == "3") {
                if (utility.isNull($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "不能为空");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }
                if (!utility.isInt($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "格式错误");
                    $(".tabbable .nav-tabs a:eq(4)").tab("show");
                    return false;
                }
            } else if (obj.TargetValueType == "7") {
                if (utility.isNull($("#txtMinKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最小值不能为空");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }
                if (utility.isNull($("#txtMaxKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最大值不能为空");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }
                if (!utility.isNumber($("#txtMinKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最小值格式错误");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }
                if (!utility.isNumber($("#txtMaxKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "最大值格式错误");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }
                if (parseFloat($("#txtMaxKPI_" + obj.KPIID).val()) < parseFloat($("#txtMinKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "范围值错误");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }

            } else {
                if (utility.isNull($("#txtKPI_" + obj.KPIID).val())) {
                    $.dialog(obj.KPIName + "不能为空");
                    $(".tabbable .nav-tabs a:eq(3)").tab("show");
                    return false;
                }
            }
        }
    }

    if (kpi.length > 0) {
        for (var i = 0; i < kpi.length; i++) {
            var obj = kpi[i];
            obj.KPITypeValue = activityID;
            if (obj.TargetValueType == "2") {
                obj.StrValue1 = $("#txtKPI_" + obj.KPIID).val();
            } else if (obj.TargetValueType == "3") {
                obj.IntValue1 = $("#txtKPI_" + obj.KPIID).val();
            } else if (obj.TargetValueType == "7") {
                obj.DecValue1 = $("#txtMinKPI_" + obj.KPIID).val();
                obj.DecValue2 = $("#txtMaxKPI_" + obj.KPIID).val();
            } else {
                obj.StrValue1 = $("#txtKPI_" + obj.KPIID).val();
            }
        }
    }

    return true;
}

function disableForm(id) {
    $("#" + id + " :input").attr("disabled", "disabled");
}

function activeForm(id) {
    $("#" + id + " :input").removeAttr("disabled");
}

function getPOSpromotion(id) {
    if (id == 0) {
        return;
    }
    $.getJSON("/Promotion/GetSysCommonMarkByID?actID=" + id, function (json) {
        if (json.length == 0) {
            return;
        }
        $("#posbind").show();
        var pos = "";
        for (var i = 0; i < json.length; i++) {
            pos += json[0].PromotionName + "、"
        }
        $("#posbind span").html(pos.substring(0, pos.length - 1));
    })
}

function checktime(time) {
    if (time == "" || utility.isNull(time)) {
        return "23:59";
    }
    else {
        return time;
    }
}