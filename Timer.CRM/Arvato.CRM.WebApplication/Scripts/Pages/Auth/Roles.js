var dtRoles, dtElements;
$(function () {

    $("#chbDashBoradEnable").change(function () {
        var option = "<option value='0'>仪表盘</option>";
        if ($("#chbDashBoradEnable").attr("checked") == "checked") {
            $('#drpDefaultPage').append(option);
            $('#drpDefaultPage').val('0');
        }
        else {
            $("#drpDefaultPage option:last").remove();
            $('#drpDefaultPage').val();
        }
    })

    //角色列表显示初始化
    if (!dtRoles) {
        dtRoles = $('#dt_roles').dataTable({
            sAjaxSource: '/Auth/GetRolesByPage',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "RoleID", title: "角色编号", sortable: true },
                { data: "RoleName", title: "角色名称", sortable: false },
                {
                    data: "RoleType", title: "角色类型", sortable: false, render: function (obj) {
                        return obj == "page" ? "页面角色" : "数据角色";
                    }
                },
                { data: "DataGroupName", title: "数据群组", sortable: false },
                {
                    data: "Enable", title: "是否启用", sortable: false, sClass: "center", sWidth: "10%", render: function (obj) {
                        return obj == true ? "已启用" : "未启用";
                    }
                },
                {
                    data: "AddedDate", title: "创建日期", sortable: true, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                },
                {
                    data: null, title: "操作", sortable: false, sClass: "center", sWidth: "14%", render: function (obj) {
                        var func = "editPageRole";
                        if (obj.RoleType == "data") {
                            func = "editDataRole";
                        }
                        return '<button class="btn" id="btnEdit" onclick="' + func + '(\'' + obj.RoleID + '\')">编辑</button> <button class="btn btn-danger" id="btnResetPwd" onclick="deleteRole(\'' + obj.RoleID + '\')">删除</button>';
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'roleName', value: $("#txtRoleNameSearch").val() });
                d.push({ name: 'roleType', value: $("#drpTypeSearch").val() });
                d.push({ name: 'roleState', value: $("#drpRoleState").val() });
                d.push({ name: 'searchGroupId', value: $("#drDataGroups option:selected").val() });
            }
        });
    }
    else {
        dtRoles.fnDraw();
    }

    initPagesMultiSelect();
    //搜索
    $("#btnSearch").click(function () {
        dtRoles.fnDraw();
    });

    //保存角色页面验证规则
    var saveRoleValidator = $("#frmPageRole").validate({
        rules: {
            txtRoleName: {
                required: true,
                maxlength: 20
            },
            drpDataGroupPage: {
                required: true
            },
            drpDefaultPage: {
                required: true
            },
        },
        //errorPlacement: function (error, element) {
        //    error.appendTo(element.next("span.error-block"));
        //},
        errorClass: 'error-block',
    });

    //保存页面角色的提交
    $("#frmPageRole").submit(function (e) {
        e.preventDefault();
        if (saveRoleValidator.form()) {
            var roleId = $("#txtRoleID").val();
            if (roleId == "") roleId = 0;
            var roleName = $("#txtRoleName").val();
            var dataGroupId = $("#drpDataGroupPage").val();
            var EnableDashBoard = $("#chbDashBoradEnable")[0].checked;
            var DefaultPath = "";
            var pageID = $("#drpDefaultPage").val();
            if (pageID == 0) {
                DefaultPath = "/Home/Index";
            }
            else {
                var allpages = $.parseJSON($("#hdAllPages").val());
                for (var i = 0; i < allpages.length; i++) {
                    if (allpages[i].PageID == pageID) {
                        DefaultPath = allpages[i].Path;
                        break;
                    }
                }
            }
            var enable = $("#chbEnable")[0].checked;
            var role = { RoleID: roleId, RoleName: roleName, DataGroupID: dataGroupId, Enable: enable, DefaultPath: DefaultPath, EnableDashBoard: EnableDashBoard };
            var pageIds = $("#drpRolePages").val();
            var elements = new Array();
            $("#dt_elements tbody tr select").each(function (e) {
                if ($(this).val() != "") {
                    var element = { RoleID: roleId, PageID: $(this).attr("pageid"), ElementKey: $(this).attr("elementkey"), SettingCss: $(this).val() };
                    elements.push(element);
                }
            });
            ajax("/Auth/SubmitPageRoleEdit", { roleJsonStr: JSON.stringify(role), pageIds: pageIds, elementsJsonStr: JSON.stringify(elements) }, function (d) {
                if (d.IsPass) {
                    $.dialog(d.MSG);
                    clearFormData("frmPageRole");
                    //dtRoles.fnDraw();
                    var start = dtRoles.fnSettings()._iDisplayStart;
                    var length = dtRoles.fnSettings()._iDisplayLength;
                    dtRoles.fnPageChange(start / length, true);
                    $.colorbox.close();
                }
                else {
                    $.dialog(d.MSG);
                }
            });
        }
    });

    $("#drpSpecifyPage").chosen();
    $("#drpDataLimitValue").chosen();
    //数据角色弹窗drpDataGroupData的change事件
    $("#drpDataGroupData").change(function () {
        SetDataLimitValueOptions();
    });
    //数据角色弹窗drpDataLimitType的change事件
    $("#drpDataLimitType").change(function () {
        SetDataLimitValueOptions();
    });
    //数据角色弹窗drpDataLimitValue的change事件
    $("#drpDataLimitValue").change(function () {
        if (!$("#drpDataLimitValue").val()) {
            $("#drpDataLimitValue").nextAll("span.error-block").html("必填字段");
        }
        else {
            $("#drpDataLimitValue").nextAll("span.error-block").html("");
        }
    });
    //保存数据页面验证规则
    var saveDataRoleValidator = $("#frmDataRole").validate({
        rules: {
            txtDataRoleName: {
                required: true,
                maxlength: 20
            },
            drpDataGroupData: {
                required: true
            },
            drpDataLimitType: {
                required: true
            }
        },
        //errorPlacement: function (error, element) {
        //    error.appendTo(element.next("span.error-block"));
        //},
        errorClass: 'error-block',
    });
    //数据角色保存提交
    $("#frmDataRole").submit(function (e) {
        e.preventDefault();
        if (saveDataRoleValidator.form()) {
            var roleId = $("#txtDataRoleId").val();
            if (roleId == "") roleId = 0;
            var roleName = $("#txtDataRoleName").val();
            var dataGroupId = $("#drpDataGroupData").val();
            var enable = $("#chbEnableData")[0].checked;
            var role = { RoleID: roleId, RoleName: roleName, DataGroupID: dataGroupId, Enable: enable };
            var limitType = $("#drpDataLimitType").val();
            var limitValues = $("#drpDataLimitValue").val();
            if (!limitValues) {
                $("#drpDataLimitValue").nextAll("span.error-block").html("必填字段");
                return;
            }
            var pageIds = $("#drpSpecifyPage").val();
            ajax("/Auth/SubmitDataRoleEdit", { roleJsonStr: JSON.stringify(role), limitType: limitType, limitValues: limitValues, pageIds: pageIds }, function (d) {
                if (d.IsPass) {
                    $.dialog(d.MSG);
                    clearFormData("frmDataRole");
                    var start = dtRoles.fnSettings()._iDisplayStart;
                    var length = dtRoles.fnSettings()._iDisplayLength;
                    dtRoles.fnPageChange(start / length, true);
                    $.colorbox.close();
                }
                else {
                    $.dialog(d.MSG);
                }
            });
        }
    });
});

//加载multi-select所有页面
function initPagesMultiSelect() {
    $("#drpRolePages").multiSelect({
        //cssClass: "height: 300px",
        selectableHeader: "<div class='search-header'><input type='text' class='span12' id='txtSearchRolePages' autocomplete='on' placeholder='查找页面...'></div>",
        selectionHeader: "<div class='search-selected'>角色可进入页面</div>",
        afterSelect: function (e) {
            //dtElements.fnDraw();
            getElements();
        },
        afterDeselect: function (e) {
            //dtElements.fnDraw();
            getElements();
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

//新建页面角色
function addPageRole(rid) {
    clearFormData("frmPageRole");
    $("#txtRoleID").val("");
    $("#titlePageRole").text("新建页面角色");
    //$("#drpRolePages").html("").multiSelect("refresh");
    $("#drpRolePages").val("").multiSelect("refresh");
    $("#chbDashBoradEnable").attr("checked", true);
    var option = "<option value='0'>仪表盘</option>";
    $('#drpDefaultPage').append(option);
    $('#drpDefaultPage').val('0');
    $('#txtSearchRolePages').quicksearch($(".ms-elem-selectable", "#ms-drpRolePages")).on("keydown, input", function (e) {
        if (e.keyCode == 40) {
            $(this).trigger("focusout");
            $("#ms-drpRolePages").focus();
            return false;
        }
        if ($(this).val() == '') {
            $(this).quicksearch($(".ms-elem-selectable", "#ms-drpRolePages"));
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
        href: "#pageRole_dialog",
        inline: true
    });
    //dtElements.fnDraw();
    getElements();
    $("#cboxLoadedContent").css("background-color", "white");
}

//新建数据角色
function addDataRole() {
    clearFormData("frmDataRole");
    $("#txtDataRoleID").val("");
    $("#titleDataRole").text("新建数据角色");
    SetDataLimitValueOptions();
    $("#drpSpecifyPage").val("").trigger("liszt:updated");

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#dataRole_dialog",
        inline: true
    });
    $("#cboxLoadedContent").css("background-color", "white");
}

//编辑页面角色
function editPageRole(rid) {
    clearFormData("frmPageRole");
    //$("#txtRoleID").val("");
    $("#titlePageRole").text("编辑页面角色");

    ajax("/Auth/GetRoleInfoById", { roleId: rid }, function (d) {
        $("#txtRoleID").val(d.RoleID);
        $("#txtRoleName").val(d.RoleName);
        $("#drpDataGroupPage").val(d.DataGroupID);
        $("#chbEnable").attr("checked", d.Enable);
        //$("#drpRolePages").val(d.PageIds);
        //$("#drpRolePages").multiSelect("refresh");
        $("#drpRolePages").val(d.PageIds).multiSelect("refresh");//.init();
        $("#chbDashBoradEnable").attr("checked", d.EnableDashBoard);
        $('#drpDefaultPage').empty();
        var allpages = $.parseJSON($("#hdAllPages").val());
        var opt = "";
        for (var i = 0; i < d.PageIds.length; i++) {
            for (var j = 0; j < allpages.length; j++) {
                if (allpages[j].PageID == d.PageIds[i]) {
                    opt += "<option value='" + allpages[j].PageID + "'>" + allpages[j].PageDesc + "</option>"
                    break;
                }
            }
        }
        $('#drpDefaultPage').append(opt);
        if (d.DefaultPath == "/Home/Index") {
            var option = "<option value='0'>仪表盘</option>";
            $('#drpDefaultPage').append(option);
            $('#drpDefaultPage').val('0');
        }
        else {
            var allpages = $.parseJSON($("#hdAllPages").val());
            for (var i = 0; i < allpages.length; i++) {
                if (allpages[i].Path == d.DefaultPath) {
                    $("#drpDefaultPage").val(allpages[i].PageID)
                    break;
                }
            }
        }
        $('#txtSearchRolePages').quicksearch($(".ms-elem-selectable", "#ms-drpRolePages")).on("keydown, input", function (e) {
            if (e.keyCode == 40) {
                $(this).trigger("focusout");
                $("#ms-drpRolePages").focus();
                return false;
            }
            if ($(this).val() == '') {
                $(this).quicksearch($(".ms-elem-selectable", "#ms-drpRolePages"));
            }
        });
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            overlayClose: false,
            opacity: '0.3',
            //title: '素材',
            href: "#pageRole_dialog",
            inline: true
        });
        //dtElements.fnDraw();
        getElements();
        $("#cboxLoadedContent").css("background-color", "white");
    });
}
function getElements() {
    authPageChanges();
    //页面元素列表显示初始化
    if (!dtElements) {
        dtElements = $('#dt_elements').dataTable({
            sAjaxSource: '/Auth/GetElementsByCondition',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 1000,
            //sScrollY: 180,
            aoColumns: [
                { data: "PageName", title: "页面名称", sWidth: "50%", sortable: true },
                { data: "ElementText", title: "控件名称", sWidth: "25%", sortable: false },
                {
                    data: null, title: "设定值", sWidth: "25%", sortable: false, render: function (obj) {
                        if (obj.SettingCss) {
                            var items = obj.SettingCss.split(';'),
                                itemSet = obj.SettedCss,
                                item = null,
                                setted = '',
                                tdContetnt = "<select pageid='" + obj.PageID + "' elementkey='" + obj.ElementKey + "' class='span12'><option value=''>请选择</option>";
                            for (var i = 0; i < items.length; i++) {
                                item = items[i].split(',');
                                setted = '';
                                if (item.length == 2) {
                                    if (itemSet != null && itemSet != "" && itemSet == items[i]) {
                                        setted = " selected='selected'";
                                    }
                                    tdContetnt += "<option value='" + items[i] + "'" + setted + ">" + item[1] + "</option>";
                                }
                            }
                            return tdContetnt + "</select>";
                        }
                    }
                }
            ],
            fnFixData: function (d) {
                var roleId = $("#txtRoleID").val();
                var rolePages = $("#drpRolePages").val();
                if (!rolePages) {
                    rolePages = [];
                }
                d.push({ name: 'roleId', value: roleId });
                pushData(d, { name: 'pageIds', value: rolePages });
            }
        });
    }
    else {
        dtElements.fnDraw();
    }
}

//编辑数据角色
function editDataRole(rid) {
    clearFormData("frmDataRole");
    $("#txtDataRoleID").val("");
    $("#titleDataRole").text("编辑数据角色");
    ajax("/Auth/GetDataRoleInfoById", { roleId: rid }, function (data) {
        if (data.IsPass) {
            var role = data.Obj[0];//角色基本信息
            var limitType = data.Obj[1];//维度的值
            var limitValues = data.Obj[2];//维度值的值
            var pageIds = data.Obj[3];//指定页面的值
            var options = data.Obj[4];//维度值的下拉选项
            //设置角色基本信息
            $("#txtDataRoleId").val(role.RoleID);
            $("#txtDataRoleName").val(role.RoleName);
            $("#drpDataGroupData").val(role.DataGroupID);
            $("#chbEnableData").attr("checked", role.Enable);
            //设置角色维度值和指定页面
            if (limitType) {
                $("#drpDataLimitType").val(limitType);
                $("#drpSpecifyPage").val(pageIds).trigger("liszt:updated");
                var opts = "";
                for (var i in options) {
                    if (options[i].BaseDataType == "store")
                        opts += "<option value='" + options[i].Str_Attr_3 + "'>" + options[i].Str_Attr_1 + "</option>";
                    if (options[i].BaseDataType == "brand")
                        opts += "<option value='" + options[i].Str_Attr_2 + "'>" + options[i].Str_Attr_1 + "</option>";
                    if (options[i].BaseDataType == "area")
                        opts += "<option value='" + options[i].Str_Attr_2 + "'>" + options[i].Str_Attr_1 + "</option>";
                }
                $("#drpDataLimitValue").html(opts).val(limitValues).trigger("liszt:updated");
            }
            else {
                SetDataLimitValueOptions();
                $("#drpSpecifyPage").val("").trigger("liszt:updated");
            }
            //显示弹窗
            $.colorbox({
                initialHeight: '0',
                initialWidth: '0',
                overlayClose: false,
                opacity: '0.3',
                //title: '素材',
                href: "#dataRole_dialog",
                inline: true
            });
            $("#cboxLoadedContent").css("background-color", "white");
        }
        else {
            $.dialog(data.MSG);
        }
    });
}

//删除角色
function deleteRole(rid) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Auth/DeleteRoleById", { roleId: rid }, function (data) {
            if (data.IsPass) {
                //dtRoles.fnDraw();
                var start = dtRoles.fnSettings()._iDisplayStart;
                var length = dtRoles.fnSettings()._iDisplayLength;
                dtRoles.fnPageChange(start / length, true);
                $.dialog(data.MSG);
            } else {
                $.dialog(data.MSG);
            }
        });
    });
}

//联动设置维度值字段的选项
function SetDataLimitValueOptions() {
    var dataGroupId = $("#drpDataGroupData").val();
    var dataLimitType = $("#drpDataLimitType").val();
    ajaxSync("/Auth/GetBaseDataByCondition", { dataGroupId: dataGroupId, type: dataLimitType }, function (data) {
        var opts = "";
        if (data.length < 1) {
            $.dialog("此维度下没有可选的值，请更改维度");
            $("#drpDataLimitValue").attr("disabled", true);
        }
        else {
            for (var i in data) {
                if (data[i].BaseDataType == "store")
                    opts += "<option value='" + data[i].Str_Attr_3 + "'>" + data[i].Str_Attr_1 + "</option>";
                if (data[i].BaseDataType == "area")
                    opts += "<option value='" + data[i].Str_Attr_2 + "'>" + data[i].Str_Attr_1 + "</option>";
                if (data[i].BaseDataType == "brand")
                    opts += "<option value='" + data[i].Str_Attr_2 + "'>" + data[i].Str_Attr_1 + "</option>";

            }
            $("#drpDataLimitValue").attr("disabled", false);
        }
        $("#drpDataLimitValue").html(opts).trigger("liszt:updated");
    });
}

//清空表单数据
function clearFormData(formId) {
    var form = $("#" + formId);
    if (form) {
        form.find("input[type!=checkbox]").val("");
        form.find("input[type=checkbox]").attr("checked", false);
        form.find("select").val("");
        form.find("span.error-block").html("");
        form.find("label.error-block").html("");
        form.find(".chzn_select").html("");
        form.find(".chzn_select").trigger("liszt:updated");
        $("#drpRolePages").val("").multiSelect("refresh");
        if ($("#drpDefaultPage option:last").val() == 0) {
            $("#drpDefaultPage option:last").remove();
        }
    }
}

function authPageChanges() {
    var rolePages = $("#drpRolePages").val();
    if (rolePages < 1) {
        return;
    }
    $('#drpDefaultPage').empty();
    var allPages = $.parseJSON($("#hdAllPages").val());
    var opt = "";

    for (var i = 0; i < rolePages.length; i++) {
        for (var j = 0; j < allPages.length; j++) {
            if (allPages[j].PageID == rolePages[i]) {
                opt += "<option value='" + allPages[j].PageID + "'>" + allPages[j].PageDesc + "</option>"
                break;
            }
        }
    }
    if ($("#chbDashBoradEnable").attr("checked") == "checked") {
        opt += "<option value='0' selected='selected'>仪表盘</option>";
    }
    $('#drpDefaultPage').append(opt);

}