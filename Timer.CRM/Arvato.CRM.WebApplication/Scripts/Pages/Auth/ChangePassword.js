$(function () {
    //页面验证规则
    var validator = $("#frmChangePassword").validate({
        //onSubmit: false,
        rules: {
            txtOldPassword: {
                required: true,
                rangelength: [6, 20],
                noWhiteSpace: true
            },
            txtNewPassword: {
                required: true,
                rangelength: [6, 20],
                noWhiteSpace: true
            },
            txtNewPasswordConfirm: {
                required: true,
                equalTo: "#txtNewPassword"
            }
        },
        //errorPlacement: function (error, element) {
        //    error.appendTo(element.next("span.error-block"));
        //},
        errorClass: 'error-block'
    });

    $("#frmChangePassword").submit(function (e) {
        e.preventDefault();
        if (validator.form()) {
            savePassword();
        }
    });
});


//保存信息
function savePassword() {
    var model = {
        OldPassword: encode($("#txtOldPassword").val()),
        NewPassword: encode($("#txtNewPassword").val()),
        ConfirmPassword: encode($("#txtNewPasswordConfirm").val())
    }
    var postUrl = "/Auth/SubmitChangePassword";
    ajax(postUrl, model, savePasswordCallBack);
}


//保存信息callback
function savePasswordCallBack(data) {
    if (data.IsPass) {
        $.dialog("保存成功");
    } else {
        $.dialog(data.MSG);
    }
}