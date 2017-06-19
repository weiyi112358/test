var dt_Table;
var reg12 = new RegExp("^\\d{12}$");
var reg5 = new RegExp("/\b\d{5}\b/");
var reg = new RegExp("^[1-9]\\d*$");
$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    }
    loadCustomizeOddIdNo();
});

//保存
$('#btnSave').click(function () {
    var status = "0";
    var info = "保存";
    submitForm(status, info);
});

$('#btnSaveStatus').click(function () {
    var status = "1";
    var info = "保存并审核";
    submitForm(status, info);
});

$('#btnReset').click(function () {
    loadCustomizeOddIdNo();
});


//加载定卡单号下拉框
function loadCustomizeOddIdNo() {
    var $customizeOddIdNo = $('#customizeOddIdNo');
    $customizeOddIdNo.empty();
    $.post('/PurchasesNew/loadCustomizeOddIdNo', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CustomizeOddId + '">' + data.CustomizeOddIdNo + '<option>'
            });
            $customizeOddIdNo.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $customizeOddIdNo.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        }
    })
}

//表单提交
function submitForm(status, info)
{
    $.dialog("确认"+info+"吗？", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        var customizeOddId = $('#customizeOddIdNo').val();
        if (customizeOddId == "") {
            $.dialog("请选择定卡单号");
            return false;
        }
        var postdata = {
            customizeOddId: customizeOddId,
            status: status
        };
        showLoading("正在"+info+"中");
        $.post('/PurchasesNew/AddRetrieve', postdata, function (result) {
            if (result.IsPass) {
                hideLoading();
                $.dialog(result.MSG);
                setTimeout(function () { $('#form').submit(); }, 1800);
                //$('#customizeOddIdNo').find('option:selected').remove();
                //$(".chzn_a").trigger("liszt:updated");
            }
            else {
                hideLoading();
                $.dialog("收卡失败");
            }
        }, 'json');
    })
}

//遮罩层
function showLoading(desc) {
    $("#txtspan").html(desc);
    $("#txtspan").css("color", "#ffffff");


    $.openPopupLayer({
        name: "processing",
        width: 500,
        target: "processingdiv"
    });
};
//遮罩层
function hideLoading() {
    $.closePopupLayer('processing');
};
