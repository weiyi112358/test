var dtUsers;
$(function () {
    dtUsers = $('#dt_users').dataTable({
        sAjaxSource: '/Auth/GetSubUsers',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'UserID', title: "用户编号", sortable: true, sWidth: "10%" },
            { data: 'UserName', title: "用户名", sortable: false },
            { data: 'LoginName', title: "登录名", sortable: false, sWidth: "10%" },
            { data: 'Mobile', title: "手机号", sortable: false, sWidth: "10%" },
            { data: 'Email', title: "电子邮箱", sortable: false },
            {
                data: 'Enable', title: "是否启用", sortable: false, sClass: "center", sWidth: "10%", render: function (r) {
                    return r == true ? "启用" : "未启用";
                }
            },
            {
                data: 'AddedDate', title: "创建时间", sortable: true, sWidth: "10%", render: function (obj) {
                    return !obj ? "" : obj.substr(0, 10);
                }
            },
            {
                data: null, title: "操作", sortable: false, sWidth: "25%", render: function (r) {
                    var str = '<button class="btn" onclick="edit(' + r.UserID + ')">编辑</button> ';
                    str += '<button class="btn" onclick="resetPsd(' + r.UserID + ')">重置密码</button> ';
                    str += '<button class="btn" onclick="changeRole(' + r.UserID + ',' + r.DataGroupID + ',' + r.AddedUser + ')">更改角色</button> ';
                    str += '<button class="btn' + (r.Enable == true ? ' btn-danger' : '') + '" onclick="active(' + r.UserID + ',' + r.Enable + ')">' + (r.Enable == true ? '停用' : '启用') + '</button>';
                    return str;
                }
            }
            //{ data: 'DispName', orderData: [0, 2] },
            //{ data: 'Path' },
            //{ data: 'Type', sortable: false, name: 'Type', title: "Type", render: function (r) { return r.Test || ''; } },
            //{ data: 'Nav' }
        ],
        fnFixData: function (d) {
            d.push({ name: 'userName', value: $("#txtUserName").val() });
            d.push({ name: 'userState', value: $("#drpUserState").val() });
            d.push({ name: 'searchGroupId', value: $("#drDataGroups option:selected").val() });
        }
    });

    $("#btnSearch").click(function () {
        dtUsers.fnDraw();
    });

    $(".chzn_select").chosen();

    //重置密码页面验证规则
    var resetPsdValidator = $("#frmResetPsd").validate({
        //onSubmit: false,
        rules: {
            newPassword: {
                required: true,
                rangelength: [6, 20],
                noWhiteSpace: true
            }
        },
        //errorPlacement: function (error, element) {
        //    error.appendTo(element.next("span.error-block"));
        //},
        errorClass: 'error-block',
    });
    //重置密码提交
    $("#frmResetPsd").submit(function (e) {
        e.preventDefault();
        if (resetPsdValidator.form()) {
            var data = { userId: $("#newPsdUserId").val(), password: encode($("#newPassword").val()) };
            ajax("/Auth/ResetPassword", data, function (d) {
                $.dialog(d.MSG);
                clearFormData("frmResetPsd");
                $.colorbox.close();
            });
        }
    });

    //修改用户信息表单验证
    var editValidator = $("#frmEditUser").validate({
        rules: {
            editMobile: {
                isMobileNo: true
            },
            editEmail: {
                email: true
            }
        },
        //errorPlacement: function (error, element) {
        //    error.appendTo(element.next("span.error-block"));
        //},
        errorClass: 'error-block',
    });
    //修改用户信息提交
    $("#frmEditUser").submit(function (e) {
        e.preventDefault();
        if (editValidator.form()) {
            var user = {
                UserID: $("#editUserID").val(),
                UserName: $("#editUserName").val(),
                LoginName: $("#editLoginName").val(),
                Mobile: $("#editMobile").val(),
                Email: $("#editEmail").val()
            }
            var postUrl = "/Auth/SubmitUserEdit";
            ajax(postUrl, user, function (d) {
                if (d.IsPass) {
                    $.dialog(d.MSG);
                    clearFormData("frmEditUser");
                    //dtUsers.fnDraw();
                    var start = dtUsers.fnSettings()._iDisplayStart;
                    var length = dtUsers.fnSettings()._iDisplayLength;
                    dtUsers.fnPageChange(start / length, true);
                    $.colorbox.close();
                }
                else {
                    $.dialog(d.MSG);
                }
            });
        }
    });

    //新建用户表单验证
    var addValidator = $("#frmAddUser").validate({
        rules: {
            addUserName: {
                required: true,
                maxlength: 20,
            },
            addLoginName: {
                required: true,
                rangelength: [4, 20],
                //isLoginName: true
            },
            addPassword: {
                required: true,
                rangelength: [6, 20],
                noWhiteSpace: true
            },
            addMobile: {
                isMobileNo: true
            },
            addEmail: {
                email: true
            }
        },
        //errorPlacement: function (error, element) {
        //    error.appendTo(element.next("span.error-block"));
        //},
        errorClass: 'error-block',
        //errorElement: 'span'
    });
    //新建用户提交
    $("#frmAddUser").submit(function (e) {
        e.preventDefault();
        if (addValidator.form()) {
            var user = {
                UserName: $("#addUserName").val(),
                LoginName: $("#addLoginName").val(),
                Password: $("#addPassword").val(),
                DataGroupID: $("#addDataGroup").val(),
                Mobile: $("#addMobile").val(),
                Email: $("#addEmail").val()
            }
            var postUrl = "/Auth/SubmitUserAdd";
            ajax(postUrl, { userJsonStr: encode(JSON.stringify(user)) }, function (d) {
                if (d.IsPass) {
                    $.dialog(d.MSG);
                    clearFormData("frmAddUser");
                    dtUsers.fnDraw();
                    $.colorbox.close();
                }
                else {
                    $.dialog(d.MSG);
                }
            });
        }
    });

    //修改角色提交
    $("#frmChangeRole").submit(function (e) {
        e.preventDefault();
        var pageRoles = $("#pageRoles").val();
        var dataRoles = $("#dataRoles").val();
        if (!pageRoles) {
            pageRoles = [];
        }
        if (!dataRoles) {
            dataRoles = [];
        }
        ajax("/Auth/SubmitChangeRoles", { userId: $("#changeRoleUserId").val(), pageRoles: pageRoles, dataRoles: dataRoles }, function (d) {
            if (d.IsPass) {
                $.dialog(d.MSG);
                clearFormData("frmChangeRole");
                $.colorbox.close();

                var start = dtUsers.fnSettings()._iDisplayStart;
                var length = dtUsers.fnSettings()._iDisplayLength;
                dtUsers.fnPageChange(start / length, true);
            }
            else {
                $.dialog(d.MSG);
            }
        });
    });
});


//重置密码
function resetPsd(userId) {
    clearFormData("frmResetPsd");
    $("#newPsdUserId").val(userId);

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#resetPsd_dialog",
        inline: true
    });
}

//编辑用户信息
function edit(userId) {
    if (!userId) {
        $.dialog("用户ID不能为空，请刷新页面后再试");
    }
    clearFormData("frmEditUser");
    ajax("/Auth/EditUser", { userId: userId }, function (d) {
        if (d.IsPass) {
            //初始化编辑页面的值
            var user = d.Obj[0];
            $("#editUserID").val(user.UserID);
            $("#editUserName").val(user.UserName);
            $("#editLoginName").val(user.LoginName);
            $("#editDataGroup").val(user.DataGroupID);
            $("#editMobile").val(user.Mobile);
            $("#editEmail").val(user.Email);
            //显示编辑页面弹窗
            $.colorbox({
                initialHeight: '0',
                initialWidth: '0',
                overlayClose: false,
                opacity: '0.3',
                //title: '素材',
                href: "#editUser_dialog",
                inline: true
            });
        }
        else {
            $.dialog(d.MSG);
        }
    })
}

//新建用户
function add() {
    clearFormData("frmAddUser");
    //显示新建页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addUser_dialog",
        inline: true
    });
}

//启用或禁用用户
function active(userId, enable) {
    var data = { userId: userId, enable: !enable };
    ajax("/Auth/ActiveUser", data, function (d) {
        if (d.IsPass) {
            $.dialog(d.MSG);
            //dtUsers.fnDraw();
            var start = dtUsers.fnSettings()._iDisplayStart;
            var length = dtUsers.fnSettings()._iDisplayLength;
            dtUsers.fnPageChange(start / length, true);
        } else {
            $.dialog(d.MSG);
        }
    });
}

//更改角色
function changeRole(userId, dataGroupId,addedUser) {
    clearFormData("frmChangeRole");
    $("#changeRoleUserId").val(userId);




    //加载角色列表
    ajax("/Auth/GetSubRolesByUserId", { userId: userId }, function (data) {
        var pp = '', dp = '';
        for (var i in data) {
            if (data[i].RoleType == 'page') {
                pp += '<option value="' + data[i].RoleID + '">' + data[i].RoleName + '</option>';
            }
            else if (data[i].RoleType == 'data') {
                dp += '<option value="' + data[i].RoleID + '">' + data[i].RoleName + '</option>';
            }
        }
        $("#pageRoles").html(pp);
        $("#dataRoles").html(dp);

        //加载用户角色
        ajax("/Auth/LoadRolesByUserId", { userId: userId }, function (d) {
            for (var i in d) {
                $("#pageRoles option[value=" + d[i].RoleID + "]").attr("selected", true);
                $("#dataRoles option[value=" + d[i].RoleID + "]").attr("selected", true);
            }
            $(".chzn_select").trigger("liszt:updated");
            //显示新建页面弹窗
            $.colorbox({
                initialHeight: '0',
                initialWidth: '0',
                overlayClose: false,
                opacity: '0.3',
                //title: '素材',
                href: "#changeRole_dialog",
                inline: true
            });
        });
    });
}

////更改角色
//function changeRole(userId, dataGroupId, addedUser) {
//    clearFormData("frmChangeRole");
//    $("#changeRoleUserId").val(userId);




//    //加载角色列表
//    ajax("/Auth/GetRolesByAddedUser", { dataGroupId: dataGroupId, roleType: "", addedUser: addedUser }, function (data) {
//        var pp = '', dp = '';
//        for (var i in data) {
//            if (data[i].RoleType == 'page') {
//                pp += '<option value="' + data[i].RoleID + '">' + data[i].RoleName + '</option>';
//            }
//            else if (data[i].RoleType == 'data') {
//                dp += '<option value="' + data[i].RoleID + '">' + data[i].RoleName + '</option>';
//            }
//        }
//        $("#pageRoles").html(pp);
//        $("#dataRoles").html(dp);

//        //加载用户角色
//        ajax("/Auth/GetRolesByUserId", { userId: userId }, function (d) {
//            for (var i in d) {
//                $("#pageRoles option[value=" + d[i].RoleID + "]").attr("selected", true);
//                $("#dataRoles option[value=" + d[i].RoleID + "]").attr("selected", true);
//            }
//            $(".chzn_select").trigger("liszt:updated");
//            //显示新建页面弹窗
//            $.colorbox({
//                initialHeight: '0',
//                initialWidth: '0',
//                overlayClose: false,
//                opacity: '0.3',
//                //title: '素材',
//                href: "#changeRole_dialog",
//                inline: true
//            });
//        });
//    });
//}


//清空表单数据
function clearFormData(formId) {
    var form = $("#" + formId);
    if (form) {
        form.find("input[type!=button]").val("");
        form.find("select").val("");
        form.find("span.error-block").html("");
        form.find(".chzn_select").html("");
        form.find(".chzn_select").trigger("liszt:updated");
    }
}