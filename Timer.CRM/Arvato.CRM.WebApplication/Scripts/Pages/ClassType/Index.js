var ClassID = '';//标记判断是增加还是编辑
var dt_classtype;
var isedit = false;
var isAdmin = $("#isAdmin").val();//判断用户是否是管理员
var closeRole = false;//是否选择UserID关闭Role
var closeUser = false;//是否选择RoleID关闭User
var RoleShow = "";
var UserShow = "";
$(function () {
    //$("#drp_datagroups").chosen();
    $("#drp_dataroles").chosen();
    $("#drp_datausers").chosen();
    //加载Class类型下拉框
    ajax("/ClassType/GetOptionDataList?optType=ClassType", null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpclasstype').append(opt);
            $('#drp_classtype').append(opt);
            //KPIType = res;
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpclasstype').append(opt);
            $('#drp_classtype').append(opt);
        }
        //加载Role用来中文显示表格中的role
        ajax("/ClassType/GetAllRolesByDataGroupId", { GroupId: $("#uDataGroupID").val() }, function (data) {
            
            RoleShow = data;
            //加载user用来中文显示表格中的user
            ajax("/ClassType/GetUsersList", { GroupId: $("#uDataGroupID").val() }, function (data1) {
                UserShow = data1;

                //加载群组下拉框
                ajax("/ClassType/GetDataGroupList", null, function (obj) {
                    if (obj.length > 0) {
                        var opt = "<option value=''>请选择</option>";
                        for (var i = 0; i < obj.length; i++) {
                            opt += '<option value=' + obj[i].DataGroupID + '>' + obj[i].DataGroupName + '</option>';
                        }
                        //$('#drp_datagroups').append(opt).trigger("liszt:updated");
                        $('#drp_datagroups').append(opt);
                        var URL = "";
                        if (isAdmin == "True") {
                            URL = '/BaseData/GetSysClassListForPage';
                        }
                        else
                            URL = '/BaseData/GetSysClassForPage';
                        //TargetValueType = res;
                        dt_classtype = $('#dt_classtype').dataTable({
                            sAjaxSource: URL,
                            bSort: true,   //不排序
                            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                            bServerSide: true,  //每次请求后台数据
                            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                            bPaginate: true, //显示分页信息
                            iDisplayLength: 8,
                            aoColumns: [
                                { data: 'ClassID', title: "编号", sortable: true },
                                { data: 'ClassName', title: "自定义名称", sortable: false },
                                {
                                    data: null, title: "类别", sortable: false, render: function (r) {
                                        for (var i = 0; i < res.length; i++) {
                                            if (r.ClassType == res[i].OptionValue) {
                                                return res[i].OptionText;
                                            }
                                        }
                                    }
                                },
                                {
                                    data: null, title: "群组", sortable: false, render: function (r) {
                                        for (var i = 0; i < obj.length; i++) {
                                            if (r.DataGroupID == obj[i].DataGroupID) {
                                                return obj[i].DataGroupName;
                                            }
                                        }
                                    }
                                },
                               {
                                   data: null, title: "用户", sortable: false, render: function (r) {
                                       for (var i = 0; i < UserShow.length; i++) {
                                           console.info(r.UserID);
                                           if (r.UserID != null && r.UserID != "") {
                                               if (r.UserID == UserShow[i].UserID) {
                                                   return UserShow[i].UserName;
                                               }
                                           }
                                           else
                                               return "";
                                       }
                                   }
                               },
                               {
                                   data: null, title: "角色", sortable: false, render: function (r) {
                                       for (var i = 0; i < RoleShow.length; i++) {
                                           if (r.RoleID != null && r.RoleID != "") {
                                               if (r.RoleID == RoleShow[i].RoleID) {
                                                   return RoleShow[i].RoleName;
                                               }
                                           }
                                           else
                                               return "";
                                       }
                                   }
                               },
                               { data: 'AddedDate', title: "创建时间", sortable: true },
                               {
                                    data: null, title: "操作", sClass: "center", sortable: false,
                                    render: function (obj) {
                                        return "<button class='btn' id='btnModify'  onclick='goEdit(\"" + obj.ClassID + "\")'>编辑</button> <button class='btn btn-danger' id='btnDelete' onclick='goDelete(\"" + obj.ClassID + "\")'>删除</button>";
                                    }
                                }
                            ],
                            fnFixData: function (d) {
                                d.push({ name: 'className', value: $("#classname").val() });
                                d.push({ name: 'classType', value: $("#drpclasstype").val() });
                                if (isAdmin == "True") {
                                    d.push({ name: 'dataGroupID', value: $("#uDataGroupID").val() });
                                }
                            }
                        });
                    } else {
                        var opt = "";
                        $('#drp_datagroups').append(opt).trigger("liszt:updated");
                    }
                });
            });
        });
    });

    //查询
    $('#btnSearch').click(function () {
        dt_classtype.fnDraw();
    })

    //清空搜索的输入信息
    $('#btnClear').click(function () {
        $('#classname').val('');
        $('#drpclasstype').val('');
    })

    //保存编辑信息
    $("#frmEditClassType").submit(function (e) {
        e.preventDefault();
        goSave();
    })

    //弹窗清空数据
    $('#btn_clear').click(function () {
        $("#txt_classname").val("");
        $("#drp_classtype").val("");
        $("#drp_datagroups").val("").trigger("liszt:updated");
        $("#drp_dataroles").val("").trigger("liszt:updated");
        $("#drp_datausers").val("").trigger("liszt:updated");
    })
})

var isfirst = true
//编辑弹窗
function goEdit(classtypeid) {
    $('#btn_clear').click();//每次弹窗先清空数据
    $("#hd_edit").val(0);
    if (classtypeid != '') {
        $("#hd_edit").val(1);
        $('#table_editattribute .modal-header h3').html('自定义编辑');
        ClassID = classtypeid;
        ajax("/ClassType/GetClassByID", { classId: classtypeid }, function (res) {
            initRoleUser(res);
            $("#txt_classname").val(res.ClassName);
            $("#drp_classtype").val(res.ClassType);
            //$("#drp_datagroups").val(res.DataGroupID).trigger("liszt:updated");
            $("#drp_datagroups").val(res.DataGroupID);
            $('#drp_classtype').attr("disabled", "disabled");
            $('#drp_datagroups').attr("disabled", "disabled");
            $('#btn_clear').attr("disabled", "disabled");
            $('#drp_dataroles').attr("disabled", "disabled").trigger("liszt:updated");
            $('#drp_datausers').attr("disabled", "disabled").trigger("liszt:updated");
            
            isedit = true;
        });
    } else {
        if (isedit == true) {
            $('#drp_classtype').removeAttr("disabled");
            $('#drp_datagroups').removeAttr("disabled");
            $('#btn_clear').removeAttr("disabled");
            $('#drp_dataroles').removeAttr("disabled").trigger("liszt:updated");
            $('#drp_datausers').removeAttr("disabled").trigger("liszt:updated");
            $("#roleanduser").hide();
            isedit = false;
        }
        if (closeRole == true) {
            $('#drp_dataroles').removeAttr("disabled").trigger("liszt:updated");
            closeRole = false;
            $("#roleanduser").hide();
        }
        if (closeUser == true) {
            $('#drp_datausers').removeAttr("disabled").trigger("liszt:updated");
            closeUser = false;
            $("#roleanduser").hide();
        }
        ClassID = ''; $('#btn_clear').click();
    }

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editdimension",
        inline: true
    });
}
//验证数据
var DataValidator = $("#frmEditClassType").validate({
    //onSubmit: false,
    rules: {
        txt_classname: {
            required: true,
        },
        drp_classtype: {
            required: true,
        },
        drp_datagroups: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存新增信息或者修改过的信息
function goSave() {
    if (DataValidator.form()) {
        var CLASS = "";
        console.info($("#drp_dataroles").val());
        if ($("#drp_dataroles").val() != null && $("#drp_dataroles").val() != "")
            CLASS = { className: $("#txt_classname").val(), classType: $("#drp_classtype").val(), roles: $("#drp_dataroles").val() }
        if ($("#drp_datausers").val() != null && $("#drp_datausers").val() != "")
            CLASS = { className: $("#txt_classname").val(), classType: $("#drp_classtype").val(), users: $("#drp_datausers").val() }
        if (($("#drp_datausers").val() == null || $("#drp_datausers").val() == "") && ($("#drp_dataroles").val() == null || $("#drp_dataroles").val() == "")) {
            $.dialog("角色和用户必须二选一");
            return;
        }
        var postUrl = "";
        if (ClassID == '')
            postUrl = "/ClassType/AddClass";
        else {
            postUrl = "/ClassType/UpdateClass";
            CLASS = { classID: ClassID, className: $("#txt_classname").val(), classType: $("#drp_classtype").val() };
        }
        ajax(postUrl, CLASS, function (res) {
            if (res.IsPass) {
                $.colorbox.close();
                var start = dt_classtype.fnSettings()._iDisplayStart;
                var length = dt_classtype.fnSettings()._iDisplayLength;
                dt_classtype.fnPageChange(start / length, true);
                $.dialog(res.MSG);
            } else { $.dialog(res.MSG); }
        });
    }
}
//删除数据
function goDelete(classid) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/ClassType/DeleteClassByID", { classID: classid }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_classtype.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}

//动态加载Roles和Users
function initRoleAndUser() {
    var dataGroupID = $("#drp_datagroups").val();
    if (dataGroupID != null && dataGroupID != "") {
        $("#roleanduser").show();
        var GroupID = { GroupId: dataGroupID };
        ajax("/ClassType/GetAllRolesByDataGroupId", GroupID, function (res) {
            $('#drp_dataroles').empty().trigger("liszt:updated");
            if (res.length > 0) {
                var opt = "";
                for (var i = 0; i < res.length; i++) {
                    
                    opt += "<option value='" + res[i].RoleID + "," + res[i].DataGroupID + "'>" + res[i].RoleName + "</option>";
                }
                $('#drp_dataroles').append(opt).trigger("liszt:updated");
            } else {
                var opt = "";
                $('#drp_dataroles').append(opt).trigger("liszt:updated");
            }
        });
        ajax("/ClassType/GetUsersList", GroupID, function (res) {
            $('#drp_datausers').empty().trigger("liszt:updated");
            if (res.length > 0) {
                var opt = "";
                for (var i = 0; i < res.length; i++) {
                    opt += '<option value=' + res[i].UserID + "," + res[i].DataGroupID + '>' + res[i].UserName + '</option>';
                }
                $('#drp_datausers').append(opt).trigger("liszt:updated");
            } else {
                var opt = "";

                $('#drp_datausers').append(opt).trigger("liszt:updated");
            }
        });
    }
    else
        $("#roleanduser").hide();
}

//编辑时加载RolesAndUsers
function initRoleUser(ress) {
    if (ress.DataGroupID != null && ress.DataGroupID != "") {
        $("#roleanduser").show();
        var GroupID = { GroupId: ress.DataGroupID };
        $('#drp_dataroles').empty().trigger("liszt:updated");
        $('#drp_datausers').empty().trigger("liszt:updated");
        ajax("/ClassType/GetAllRolesByDataGroupId", GroupID, function (res) {
            if (res.length > 0) {
                var opt = "";
                for (var i = 0; i < res.length; i++) {
                    console.info(res[i].RoleID);
                    opt += "<option value='" + res[i].RoleID + "'>" + res[i].RoleName + "</option>";
                }
                $('#drp_dataroles').append(opt).trigger("liszt:updated");
            } else {
                var opt = "";
                $('#drp_dataroles').append(opt).trigger("liszt:updated");
            }
            ajax("/ClassType/GetUsersList", GroupID, function (res) {
                if (res.length > 0) {
                    var opt = "";
                    for (var i = 0; i < res.length; i++) {
                        opt += '<option value=' + res[i].UserID + '>' + res[i].UserName + '</option>';
                    }
                    $('#drp_datausers').append(opt).trigger("liszt:updated");
                } else {
                    var opt = "";
                    $('#drp_datausers').append(opt).trigger("liszt:updated");
                }
                $("#drp_dataroles").val(ress.RoleID).trigger("liszt:updated");
                $("#drp_datausers").val(ress.UserID).trigger("liszt:updated");
                $("#roleanduser").show();

            });

        });
    }
    else
        $("#roleanduser").hide();
}


//角色和用户下拉框二选一
function disableRole() {
    var user = $("#drp_datausers").val();
    if (user != null && user != "") {
        $('#drp_dataroles').attr("disabled", "disabled").trigger("liszt:updated");
        closeRole = true;
    }
    else {
        $('#drp_dataroles').removeAttr("disabled").trigger("liszt:updated");
        closeRole = false;
    }
}
function disableUser() {
    var role = $("#drp_dataroles").val();
    if (role != null && role != "") {
        $('#drp_datausers').attr("disabled", "disabled").trigger("liszt:updated");
        closeUser = true;
    }
    else {
        $('#drp_datausers').removeAttr("disabled").trigger("liszt:updated");
        closeUser = false;
    }
}