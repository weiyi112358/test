var boxNo_Table;
var array = new Array();
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
var provinceCode = "";
var customizeOddId = "";

$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    };
    loadAgent();
    loadCardType();
    //loadMathRule();

});

//选择盒号生成规则
$('#boxNoRule').change(function () {
    var ruleVal = $(this).val();
    var $ckVal = $('#ckVal');
    var $boxNoAutoDiv = $('#boxNoAutoDiv');
    var $boxNoManualDiv = $('#boxNoManualDiv');
    var $btnSearch = $('#boxNoManualDiv div');
    if (ruleVal == "autoBox") {     
        $ckVal.empty();
        $boxNoAutoDiv.show(); 
        $boxNoManualDiv.hide();
        $btnSearch.hide();
    }
    else if (ruleVal == "manualBox") {    
        if (boxNo_Table==null) {
            loadBoxNoDataTable();
        }
        else {
            boxNo_Table.fnDraw();
        }
        $boxNoAutoDiv.hide();
        $boxNoManualDiv.show();
        $btnSearch.show()
    } else {
        $ckVal.empty();
        $boxNoAutoDiv.hide();
        $boxNoManualDiv.hide();
        $btnSearch.hide();
    }
});

//选择收货单位
$('#chooseAcceptingUnit').change(function () {
    var val = $(this).val();
    var $companyDiv = $('#companyDiv');
    var $storeDiv = $('#storeDiv');
    //旧版
    //var $store=$('#store');
    //var $company=$('#company');
    if (val == "0") {
        storeDivClear();
        companyDivClear();
        $companyDiv.hide();
        $storeDiv.hide();   
    }
    else if (val == "1") {
        storeDivClear();
        loadCompany();
        $companyDiv.show();
        $storeDiv.hide();     
    }
    else {
        companyDivClear();
        loadStore();
        $storeDiv.show();
        $companyDiv.hide();
    }
    provinceCode = "";
})

//选择收货单位省份by分公司
$('#company').change(function () {
    var $company = $('#company');
    if ($company.val()=="") {
        provinceCode == "";
    }
    else {
        provinceCode = ($company.val().split(','))[1];
    }

});

//选择收货单位省份by门店
$('#store').change(function () {
    var $store = $('#store');
    if ($store.val() == "") {
        provinceCode = "";
    }
    else {
        provinceCode = ($store.val().split(','))[1];
    }
});

//选择卡号生成规则
$('#cardNoRule').change(function () {
    var $beginCardNo = $('#beginCardNo');
    var $endCardNo = $('#endCardNo');
    var $destineNum = $('#destineNum');
    var $chooseAcceptingUnit = $('#chooseAcceptingUnit');
    //旧版
    //var $mathRule = $('#mathRule');
    var $store = $('#store');
    var $company = $('#company');
    var cardNum = $destineNum.val().trim();
    var cardSelect = $(this).val();
    //旧版
    //if (provinceCode == "") {
    //    $.dialog("请选择收货单位");
    //    resetCardRuleSelect();
    //    return false;
    //};
    if (cardNum == "" || reg.test(cardNum) == false || parseInt(cardNum)>=10000) {
        $.dialog("请输入最多4位数字的定卡数量");
        resetCardRuleSelect();
        return false;
    };
    //旧版
    //if ($mathRule.val()=="") {
    //    $.dialog("请选择跳号规则");
    //    resetCardRuleSelect();
    //    return false;
    //}
    if (cardSelect == "autoCard") {
        var postdata = {
            cardNum: cardNum,
            provinceCode: provinceCode,
            //旧版
            //mathRule: $mathRule.val()
            mathRule: "",
        };
            showLoading("正在获取卡号");
        $.post('/PurchasesNew/GetAutoCard', postdata, function (result) {      
            if (result.IsPass) {
                var strArray = result.MSG.split(',');
                $beginCardNo.val(strArray[0]);
                $endCardNo.val(strArray[1]);
                //$beginCardNo.prop('readonly', true);
                //$endCardNo.prop('readonly', true);
                hideLoading();
                lockSelect("1");
            }
            else {
                resetCardRuleSelect();
                hideLoading();
            };
        }, 'json');
    }
    else if (cardSelect == "manualCard") {
        var beginCardNo = $('#beginCardNo').val().trim();
        if (beginCardNo == "") {
            $.dialog("请先输入起始卡号");       
            resetCardRuleSelect();
            return false;
        };
        if (reg12.test(beginCardNo) == false) {
            $.dialog("请您输入12位数字的起始卡号");
            resetCardRuleSelect();
            return false;
        };
        if (beginCardNo.substring(0, 2) != "21") {
            $.dialog("起始卡号以21开头");
            resetCardRuleSelect();
            return false;
        };
        //if (beginCardNo.substring(2,4)!=provinceCode) {
        //    $.dialog("起始卡号的省份编码与所选收货单位不符！");
        //    resetCardRuleSelect();
        //    return false;
        //}
        var postdata = {
            beginCardNo: beginCardNo,
            cardNum: cardNum,
            provinceCode: provinceCode,
            //mathRule: $mathRule.val()
            mathRule: "",
        };
        showLoading("正在获取卡号");
        $.post('/PurchasesNew/GetManualCard', postdata, function (result) {
            if (result.IsPass) {         
                $endCardNo.val(result.MSG);
                //$beginCardNo.prop('readonly', true);
                //$endCardNo.prop('readonly', true);
                //$chooseAcceptingUnit.prop('readonly', true);
                //$store.prop('readonly', true);
                //$company.prop('readonly', true);
                hideLoading();
                lockSelect("1");
            }
            else {
                $.dialog(result.MSG);
                hideLoading();
            };
        }, 'json');
    }
    else {
        lockSelect("0");
    };
});

//盒号查询
$('#btnBoxNoSearch').click(function () {
    var boxNoInput = $('#boxNoInput').val().trim();
    if (boxNoInput != "" && reg.test(boxNoInput) == false) {
        $.dialog("请输入正确的盒号");
        return false;
    }
    boxNo_Table.fnDraw();
});

//重置
$('#btnReset').click(function () {  
    goClear();  
    lockSelect("0");
    boxInfoReset();
    $('#store').val('广州市娇联化妆品有限公司').attr('readonly', true);
});

//保存
$('#btnSave').click(function () {
    var status = "0";
    var info="保存"
    submitForm(status,info);
});

//保存并审核
$('#btnSaveStatus').click(function () {
    var status = "1";
    var info="保存并审核"
    submitForm(status,info);
});


//加载供应商
function loadAgent() {
    var $agent = $('#agent');
    $agent.empty();
    $.post('/PurchasesNew/LoadAgent', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.AgentCode + '">' + data.AgentName + '<option>'
            });
            $agent.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $agent.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};

//加载卡类型
function loadCardType() {
    var $cardType = $('#cardType');
    $cardType.empty();
    $.post('/PurchasesNew/LoadCardType', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CardCode + '">' + data.CardName + '<option>'
            });
            $cardType.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $cardType.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};

//加载跳号规则
//function loadMathRule() {
//    var $mathRule = $('#mathRule');
//    $mathRule.empty();
//    var postdata = { optionType: "MathRule" };
//    $.post('/PurchasesNew/LoadBizOption', postdata, function (result) {
//        if (result.data.length > 0) {
//            var opt = '<option value="">==请选择==<option>';
//            $.each(result.data, function (i, data) {
//                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '<option>'
//            });
//            $mathRule.append(opt);
//            $(".chzn_a").trigger("liszt:updated");
//        }
//        else {
//            $mathRule.append('<option value="">==无==<option>');
//            $(".chzn_a").trigger("liszt:updated");
//        };
//    });
//};




//加载分公司
function loadCompany() {
    var $company = $('#company');
    $company.empty();
    $.post('/PurchasesNew/LoadCompany', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CompanyCode + ',' + data.CompanyProvinceCode + '">' + data.CompanyName + '/' + data.CompanyCode + '<option>'
            });
            $company.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $company.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};

//加载门店
function loadStore() {
    var $store = $('#store');
    $store.empty();
    //旧版
    //var postdata = { storeType: "直营店" };
    var postdata = { storeType: "" };
    $.post('/PurchasesNew/LoadStore', postdata, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.StoreCode + ',' + data.StoreProvinceCode + '">' + data.StoreName + '/' + data.StoreCode + '<option>'
            });
            $store.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $store.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};

//加载盒号datatable
function loadBoxNoDataTable() { 
        boxNo_Table = $('#BoxNoTable').dataTable({
        sAjaxSource: '/PurchasesNew/GetEmptyBoxNo',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        order: [[1, "asc"]],
        aoColumns: [
        {
            data: null, title: '<input type="checkbox"  id="ckALL" onclick="ChooseAll(this)">全选</input>', sortable: false, sClass: "center", render: function (r) {
                if ($("#ckVal").val().indexOf(r.BoxNo) > -1) {
                    var str = '<input type="checkbox" name="txtCK" value="' + r.BoxNo + '"  onclick="checkone(this)" checked=checked />';

                } else {
                    var str = '<input type="checkbox" name="txtCK" value="' + r.BoxNo + '"  onclick="checkone(this)" />';

                }
                return str;
            }
        },
        { data: 'BoxNo', title: "盒号", sortable: false, sClass: "center" },
        ],
        fnFixData: function (d) {
            d.push({ name: 'boxNoInput', value: $("#boxNoInput").val().trim() });
        }
    });
    $('#BoxNoDiv').show();
    $('#BoxNoDiv div').show();
}

//全选
function ChooseAll(obj) {
    $("[name=txtCK]:checkbox").prop("checked", obj.checked);
    if (obj.checked == true) {
        $("[name=txtCK]:checkbox").each(function (a, i) {
            $('#ckVal').val($('#ckVal').val().replace(i.value + ",", ''));
            $('#ckVal').val($('#ckVal').val() + i.value + ",");

        })
    }
    else {
        $("[name=txtCK]:checkbox").each(function (a, i) {
            $('#ckVal').val($('#ckVal').val().replace(i.value + ",", ''));

        });
    }
}

//单选
function checkone(a) {
    if (a.checked) {
        $('#ckVal').val($('#ckVal').val() + a.value + ",");
    }
    else {
        $('#ckVal').val($('#ckVal').val().replace(a.value + ",", ''));
    }
}

//一键清理
function goClear() {
    $('#modalAdd input:text').val('');   
    loadAgent();
    loadCardType();
    //旧版
    //loadMathRule(); 
    resetCardRuleSelect();
    resetBoxRuleSelect();
    acceptingUnitClear();  
    array = [];
};

//重置卡号生成规则下拉框
function resetCardRuleSelect()
{
    var options = '<option value="">==请选择==</option><option value="autoCard">自动生成卡号</option><option value="manualCard">手动生成卡号</option>';
    $('#cardNoRule').empty().append(options);

}

//重置盒号生成规则下拉框
function resetBoxRuleSelect()
{
    var options = '<option value="">==请选择==</option><option value="autoBox">自动生成盒号</option><option value="manualBox">手动生成盒号</option>';
    $('#boxNoRule').empty().append(options);
}

//判断盒数量
function CheckNumDone(count) {
    var num = parseInt(count);
    var result = 0;
    if (num < 250) {
        result = 1;
    }
    else {
        result = num % 250 == 0 ? num / 250 : Math.ceil(num / 250);
    }
    return result
}

//表单提交
function submitForm(status,info)
{
    $.dialog("确认"+info+"吗？", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        var cardType = $('#cardType').val();
        var agent = $('#agent').val();
        var mathRule = $('#mathRule').val();
        var chooseAcceptingUnit = $('#chooseAcceptingUnit').val();
        var destineNum = $('#destineNum').val().trim();
        var beginCardNo = $('#beginCardNo').val().trim();
        var endCardNo = $('#endCardNo').val().trim();
        var cardNoArray = $('#ckVal').val();
        var remark = $('#remark').val().trim();
        var boxModel = $('#boxNoRule').val();
        var boxPurpose = $('#boxPurpose').val();
        var $store = $('#store');
        var $company = $('#company');
        var cardModel = $('#cardNoRule').val();
        //验证
        if (cardType == "" || agent == "" || mathRule == "" || chooseAcceptingUnit == 0 || boxModel == "" || cardModel == "") {
            $.dialog("您有遗漏的选项未选择！");
            return false
        };
        if (chooseAcceptingUnit == "1" && $company.val() == "") {
            $.dialog("请您选择分公司！");
            return false;
        };
        if (chooseAcceptingUnit == "2" && $store.val() == "") {
            $.dialog("请您选择门店！");
            return false;
        };
        if (beginCardNo == "" || reg12.test(beginCardNo) == false) {
            $.dialog("请您输入12位数字的起始卡号");
            return false;
        };
        if (endCardNo == "" || reg12.test(endCardNo) == false) {
            $.dialog("请您获取12位数字的截止卡号");
            return false;
        };
        if (destineNum == "" || reg.test(destineNum) == false || parseInt(destineNum) > 10000) {
            $.dialog("请您输入不大于四位数的全数字定制数量");
            return false;
        };
        if (boxModel == "manualBox") {
            if (cardNoArray == "") {
                $.dialog("请勾选盒号");
                return false;
            }
            else {
                var num = CheckNumDone(destineNum)
                var opArray = new Array();
                opArray = cardNoArray.substring(cardNoArray.length - 1, 1).split(',');
                if (opArray.length < num) {
                    $.dialog("请选择不少于" + num + "个盒号");
                    return false;
                }
            }
        };

        var params = {
            CardType: cardType,
            Agent: agent,
            MathRule: mathRule,
            DestineNum: destineNum,
            BeginCardNo: beginCardNo,
            EndCardNo: endCardNo,
            CardNoArray: cardNoArray,
            //旧版
            //StoreCode: ($store.val().split(','))[0],
            //CompanyCode: ($company.val().split(','))[0],
            StoreCode: $store.val(),
            CompanyCode: "",
            BoxPurPose: boxPurpose,
            Remark: remark
        };
        array.push(params);
        var postdata = { jsonParams: JSON.stringify(array), status: status };
        showLoading("正在"+info+"中");
        $.post('/PurchasesNew/AddCustomize', postdata, function (result) {
            if (result.IsPass) {
                $.dialog(result.MSG);
                hideLoading();
                goClear();
                setTimeout(function () { $('#form').submit(); }, 1800);
            }
            else {
                $.dialog(result.MSG);
                array = [];
                hideLoading();
            };
        }, 'json');
    });
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
}

//遮罩层
function hideLoading() {
    $.closePopupLayer('processing');
}

//分公司隐藏
function companyDivClear()
{
   $('#company').empty().append('<option value="">无</option>');
}

//门店隐藏
function storeDivClear() {
    $('#store').empty().append('<option value="">无</option>');
}

//清理收货单位
function acceptingUnitClear()
{
    //旧版
    //$('#chooseAcceptingUnit').empty().append('<option value="0">==请选择==</option><option value="1">分公司</option><option value="2">门店</option>');
    $('#chooseAcceptingUnit').empty().append('<option value="0">==请选择==</option><option value="2">门店</option>');
    storeDivClear();   
    $('#storeDiv').hide();
    companyDivClear();
    $('#companyDiv').hide();
    provinceCode = "";
}

//锁定卡拉框
function lockSelect(status)
{
    var $input = $('#modalAdd input');
    var $select = $('#modalAdd select');
    if (status == "1") {    
        $input.prop('readonly', true);       
        for (var i = 0; i < $select.length; i++) {
            $select[i].disabled = true;
        }
        $(".chzn_a").trigger("liszt:updated");
        $('#boxNoRule').prop('disabled', false);
        $('#boxPurpose').prop('disabled', false);
        $('#remark').prop('readonly', false);
    }
    else {
        $input.prop('readonly', false);
        for (var i = 0; i < $select.length; i++) {
            $select[i].disabled = false
        }
        $(".chzn_a").trigger("liszt:updated");
    }
}

//重置盒状态
function boxInfoReset() {
    resetBoxRuleSelect();
    $('#boxNoAutoDiv').hide();
    $('#boxNoManualDiv').hide();       
};


