$(function () {
    //页面验证规则
    var validator = $("#frmUserProfile").validate({
        //onSubmit: false,
        rules: {
            Mobile: {
                isMobileNo: true
            },
            Email: {
                email: true
            }
        },
        //errorPlacement: function (error, element) {
        //    error.appendTo(element.next("span.error-block"));
        //},
        errorClass: 'error-block'
    });

    $("#frmUserProfile").submit(function (e) {
        e.preventDefault();
        if (validator.form()) {
            saveUser();
        }
    });
});


//保存信息
function saveUser() {
    var user = {
        UserName: $("#UserName").val(),
        LoginName: $("#LoginName").val(),
        Mobile: $("#Mobile").val(),
        Email: $("#Email").val()
    }
    var postUrl = "/Auth/SubmitMyProfile";
    ajax(postUrl, user, saveUserCallBack);
}

//保存信息callback
function saveUserCallBack(data) {
    $.dialog(data.MSG);
}