
var dicPromotionCondition = {};
var isSaveAndApprove = false;
var arrCash = ['PayCash', 'PayCash2', 'PayQuantity', 'PayQuantity2'];
//页面初始
$(function () {
    /****************************** main start ***********************************/
    $.ajaxSetup({
        async: false
    });
    $('#btnSaveAndApprove').click(function () {
        isSaveAndApprove = true;
        $(this).submit();
    })
    //注册保存按钮
    $("#ActionForm").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            saveAction();
        }
        else {
            tabActive(1);
        }
    });

    if ($("#txtBillCode").val() == "") {
        $("#btnActive,#btnApprove,#btnCopy").hide();
        $("#li2,#li3").hide();
        //action
        initAction();
    }
    else {
        //$('.canNotEdit').attr('disabled', true);
        var promotionType = $('#selPromotionType').val();
        var typeDesc = $('#selPromotionTypeDesc').val();
        setPromotion(promotionType, typeDesc, function () {
            var condition = $('#selPromotionType').attr('data-condition')
            if (condition != '')
                setConditionBody(condition, showApproveStatus);
            //action
            initAction(function () {
                var action = $('#selPromotionType').attr('data-action');
                if (action != '')
                    setAction(action);
            });
        });
    }


    showExecuteStatus();
    //执行状态
    $("#btnActive").click(function (e) {
        var isWakeup = $(this).prop('value');

        var postUrl = "/MemberSalesPromotion/ActiveRuleById";
        ajax(postUrl, { "ruleId": $("#txtBillID").val(), "IsWakeUp": isWakeup == 1 }, ExecuteStatusCallBack);
    });

    showApproveStatus();
    //审核
    $("#btnApprove").click(function (e) {
        var status = $(this).prop('value');
        var postUrl = "/MemberSalesPromotion/ApproveRuleById";
        ajax(postUrl, { "ruleId": $("#txtBillID").val(), "active": status }, ApproveStatusCallBack);
    });

    //返回
    $("#btnReturn").click(function (e) {
        window.location.href = "/MemberSalesPromotion/Promotion";
    });

    $("#selPromotionType").change(function () {
        var selPromotion = $(this).val();
        $("#li2,#li3").hide();
        if (selPromotion != undefined && selPromotion != '') {
            $("#li2,#li3").show();
            //类型名称
            var text = $('#selPromotionType option[value=' + selPromotion + ']').text();
            setPromotion(selPromotion, text);
        }
    })

    //显示指定的促销类型
    function setPromotion(selPromotion, promotiondesc, fun) {
        //显示对应的Action
        $('.action').removeClass('on').hide();
        $('.action[name="' + promotiondesc + '"]').addClass('on').show();

        if ($.inArray(selPromotion, ['7', '9']) >= 0) {
            $('.LimitQuantity').show();
        }
        else
            $('.LimitQuantity').hide();

        //显示对应的可选条件
        var postUrl = "/MemberSalesPromotion/GetConditionList";
        ajax(postUrl, { "PromotionType": selPromotion }, function (data) {

            $('#tab2 .form_validation_reg').empty();
            $('#tab2 .form_validation_reg').append('<div class="row-fluid conditionhide" style="display:none">\
                            <div class="span2 cx_select">\
                                <select class="choseCondition span12">\
                                    <option value="notSelect" selected></option> \
                                </select>\
                            </div>\
                            <div class="span2 cx_body ">\
                                <input disabled class="span12" />\
                            </div>\
                            <div class="span2 cx_condition">\
                                <a class="addCondition" style="float: left;"></a>\
                                <a class="removeCondition" style="float: left"></a>\
                            </div>\
                        </div>');
            $('.choseCondition').empty();
            $('.choseCondition').append('<option value="notSelect" selected></option>');
            $.each(data, function (n, value) {
                $('.choseCondition').append('<option value="{0}">{1}</option>'.format(value.Code, value.Name));
                dicPromotionCondition[value.Code] = value.Name;
            })

            if (data.length == 0) {
                $('#tab2').attr('disabled', true);
            }
            else {
                $('#tab2').attr('disabled', false);
                //必选项处理
                var hasRequire = false;
                var isSingle = data.length == 1;
                $.each(data, function (n, value) {
                    if (value.IsRequire) {
                        var $new = initConditionRow(true, value.Code, value.Name, isSingle);

                        hasRequire = true;
                    }
                })
                //如果 没有必选项，需要默认项
                if (!hasRequire) {
                    var $row = initConditionRow();
                    $($row).find('.removeCondition').attr('disabled', true);
                }
            }

            if (fun)
                fun();

        });
    }

    /****************************** main end ***********************************/
    //加载日期控件
    $("#txtStartDate,#txtEndDate,#selStartDate,#selEndDate").datepicker();
    //加载时间插件
    $("#txtTime").timepicker();

    //绑定商品
    bindProductTable();
    //小数点2位
    bindInput();
     
});

/****************************** main start ***********************************/
function ExecuteStatusCallBack(data) {
    $.dialog(data.MSG);
    if (data.IsPass) {
        $("#cbExecuteStatus").val(data.Obj);
        showExecuteStatus();
    };
}

function showExecuteStatus() {
    var executeStatus = $("#cbExecuteStatus").val();
    if (executeStatus == '休眠中') {
        $('#btnActive').prop('disabled', false).prop('value', 1).text('唤醒');
    }
    else
        $('#btnActive').prop('disabled', false).prop('value', 0).text('休眠');
}

function ApproveStatusCallBack(data) {
    $.dialog(data.MSG);
    if (data.IsPass) {
        $('#cbApproveStatus').val(data.Obj);
        showApproveStatus();
    };
}
function showApproveStatus() {
    var approveStatus = $('#cbApproveStatus').val();
    $('#btnActive').prop('disabled', true); //执行状态
    $('.canNotEdit').attr('disabled', true);
    $('#btnSave').prop('disabled', true);
    $('#btnSaveAndApprove').prop('disabled', true);
    //条件
    $('#tab2 input,#tab2 select').attr('disabled', true);
    notBindCondition();
    if (approveStatus == '未审核') {
        $('#btnApprove').prop('disabled', false).prop('value', 1).text('审核');
        $('.canNotEdit').attr('disabled', false);
        $('#btnSave').prop('disabled', false);
        $('#btnSaveAndApprove').prop('disabled', false);
        $('#tab2 input,#tab2 select').attr('disabled', false);
        //绑定条件
        bindAddRemoveCondition();
    }
    else if (approveStatus == "已审核") {
        $('#btnApprove').prop('disabled', false).prop('value', 2).text('作废');
        $('#btnActive').prop('disabled', false);
    }
    else {
        $('#btnApprove').prop('disabled', true).prop('value', 1).text('审核');
        $('input,textarea,select').prop('disabled', true);
    }
}

//新增时验证数据
var DataValidator = $("#ActionForm").validate({
    //onSubmit: false,
    rules: {
        selPromotionType: {
            required: true,
        },
        txtRunIndex: {
            required: true,
            number: true,
        },
        txtStartDate: {
            required: true,
        },
        txtEndDate: {
            required: true,
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存
function saveAction() {

    //条件
    var condition = getConditionBody();
    if (!condition) {
        tabActive(2);
        return;
    }
    var action = getAction();
    if (!action) {
        tabActive(3);
        return;
    }


    if ($("#txtRunIndex").val() <= 0) {
        $.dialog('优先级不能小于等于0');
        tabActive(1);
        return;
    }

    var ruleMaster = {
        BillID: $("#txtBillID").val(),
        PromotionType: $("#selPromotionType").val(),
        RunIndex: $("#txtRunIndex").val(),
        Condition: JSON.stringify(condition),
        Action: JSON.stringify(action),
        StartDate: $("#txtStartDate").val(),
        EndDate: $("#txtEndDate").val(),
        //ScheduleType: $("#rdRealTime").attr("checked") ? "realtime" : "cycle", 
        Remark: $("#txtRemark").val(),
    };
   

    //if (isSaveAndApprove)
    //    ruleMaster["ApproveStatus"] = 1;

    if (ruleMaster.PromotionType == '') {
        $.dialog("规则类型不能为空");
        return;
    }
    if (ruleMaster.RunIndex == '') {
        $.dialog("优先级不能为空");
        return;
    }
    if (ruleMaster.StartDate == '') {
        $.dialog("开始日期不能为空");
        return;
    }
    if (ruleMaster.EndDate == '') {
        $.dialog("结束日期不能为空");
        return;
    }
    if (!utility.compareDate(ruleMaster.StartDate, ruleMaster.EndDate)) {
        $.dialog("起始日期不能大于结束日期");
        return;
    }

    //数据限量
    var limit = $('#txtLimitQuantity').val() ;
    ruleMaster["LimitQuantity"] = limit;
    if ($.inArray(ruleMaster.PromotionType, ['7', '9']) >= 0 && ( limit == ''|| limit==0)) {
        $.dialog("礼品促销数量限量不能为空！");
        return;
    }


    var postUrl = "/MemberSalesPromotion/SavePromotion";
    ajax(postUrl, { "ruleMaster": ruleMaster }, function (data) {
        if (data.IsPass) { 
            $("#btnActive,#btnApprove,#btnCopy").show();
            $("#txtBillID").val(data.Obj[0].BillID);
            $("#txtBillCode").val(data.Obj[0].BillCode);
            showExecuteStatus();
            showApproveStatus();
            if (isSaveAndApprove)
                $("#btnApprove").click();
        };
        $.dialog(data.MSG);
    });
};


//激活选项卡
function tabActive(index) {
    $("#li1,#li2,#li3,#tab1,#tab2,#tab3").removeClass("active");
    $("#li" + index + ",#tab" + index).addClass("active");
};


/****************************** main end ***********************************/

/****************************** tab2 start ***********************************/

//初始化等级数据
function initMemberLevel($row) {
    var postUrl = "/MemberSalesPromotion/GetConditionOptions";
    ajax(postUrl, { "Type": "MemberLevel" }, function (data) {
        if (data.IsPass) {
            $($row).find('.cx_level').empty();
            $.each(data.Obj[0], function (n, value) {
                $($row).find('.cx_level').append('<label class="radio_input">\
                                    <input class="input_c" type="checkbox" name="memberlevel" value="{0}" />{1}</label>'
                                    .format(value.Value, value.Text));

            })
        }
        else
            $.dialog(data.MSG);
    });
}
//暂时用 ，需要改成弹出框
function initGoods($row, isMulti) {


    var postUrl = "/MemberSalesPromotion/GetConditionOptions";
    ajax(postUrl, { "Type": "AllGoods" }, function (data) {
        if (data.IsPass) {
            $($row).find('.cx_goods').empty();
            $.each(data.Obj[0], function (n, value) {
                var name = value.Text;
                if (name == null)
                    name = "未知";
                var content = '<label class="radio_input">\
                                    <input class="input_c" type="radio" name="goods" value="{0}" />{1}</label>'
                                    .format(value.Value, name);
                if (isMulti)
                    content = content.replace('type="radio"', 'type="checkbox"');
                $($row).find('.cx_goods').append(content);

            })
        }
        else if (data.MSG)
            $.dialog(data.MSG);
    });
}
//初始化生日
function initBirthday($row) {
    $($row).find('.cx_Birthday').empty();
    $($row).find('.cx_Birthday').append(' <label class="radio_input">\
                                    <input class="input_c" type="radio" name="birthday" value="birthday" />生日</label>\
                                <label class="radio_input">\
                                    <input class="input_c" type="radio" name="birthday" value="birthmonth" />生日月（含当天）</label>\
                                <label class="radio_input">\
                                    <input class="input_c" type="radio" name="birthday" value="birthmonthNotDay" />生日月（不含当天）</label>\
                                <label class="radio_input">\
                                    <input class="input_c" type="radio" name="birthday" value="birthweek" />生日周（含当天）</label>\
                                <label class="radio_input">\
                                    <input class="input_c" type="radio" name="birthday" value="birthweekNotDay" />生日周（不含当天）</label>');
}

function initDayRegion($row) {
    $($row).find('.cx_dataregion').empty();
    $($row).find('.cx_dataregion').append('<div class="z15_width">\
                                    <label class="title-left sepV_a ml10">从<span class="f_req">*</span></label>\
                                </div>\
                                <div  >\
                                    <input type="number" class="span12 input_c startRegion" >\
                                </div>\
                                <div class="z15_width">\
                                    <label class="title-left sepV_a ml10">至<span class="f_req">*</span></label>\
                                </div>\
                                <div >\
                                    <input type="number" class="span12 input_c endRegion" >\
                                </div>');
}

//初始化等级分组
function initMemberLevelGroup($row, $input) {
    //积分倍数<input type="number" name="levelPoint"/>
    //<input type="text" name="levelproduct" readonly/><a class=""><img src="~/img/add_img/browser_small.gif" /></a>
    var postUrl = "/MemberSalesPromotion/GetConditionOptions";
    ajax(postUrl, { "Type": "MemberLevel" }, function (data) {
        if (data.IsPass) {
            $($row).find('.cx_levels').empty();
            $.each(data.Obj[0], function (n, value) {
                $($row).find('.cx_levels').append('<label class="radio_input" value="{0}"><b>{1}</b>\
 <div>{2}</div>\
                                </label>'
                                    .format(value.Value, value.Text, $input));
                $($row).find('input:last').attr('level', value.Value);
            })
        }
        else
            $.dialog(data.MSG);
    });
}


function notBindCondition() {
    $('#tab2').off('click', '.addCondition');
    $('#tab2').off('click', '.removeCondition');
    $('#tab2').off('change', '.choseCondition');
    $('#tab2').off('click', '.selGift,.levelproducts');
}

//注册
function bindAddRemoveCondition() {
    //添加条件
    $('#tab2').on('click', '.addCondition', function () {
        initConditionRow();
    });
    //删除条件
    $('#tab2').on('click', '.removeCondition', function () {
        var $row = $(this).parents('.row-fluid');
        var isDisabled = $(this).attr('disabled');
        if (isDisabled != 'disabled')
            $($row).remove();
    });

    $('#tab2').on('change', '.choseCondition', function () {
        $('.nfocus').removeClass('nfocus');
        $(this).addClass('nfocus');
        var $row = $(this).parents('.row-fluid');
        var select = $(this).val();
        $.each($('.choseCondition:not(.nfocus)'), function (n, single) {
            if (select == $(single).val() && select != 'notSelect') {
                $.dialog("当前条件已经选择！");
                $($row).find('.cx_body').empty().append('<input disabled class="span12" />');
                $($row).find('.choseCondition').val('notSelect');
                select = 'notSelect';
            }
            if ($.inArray(select, arrCash) >= 0 && $.inArray($(single).val(), arrCash) >= 0) {
                $.dialog("购买数量和购买金额条件只能选择一种！");
                $($row).find('.cx_body').empty().append('<input disabled class="span12" />');
                $($row).find('.choseCondition').val('notSelect');
                select = 'notSelect';
            }
        })
       
        if (select != 'notSelect') {
            $(this).find('option[value="notSelect"]').remove();

            initConditionBody($row, select);
        }
    });

    //选择礼品
    $('#tab2').on('click', '.selGift,.levelproducts', function () {
        $('.selGiftDiv').removeClass('selGiftDiv');
        $(this).parent().addClass('selGiftDiv');

        showProduct(function (selProducts, selProductName) {
            if (selProducts) {
                $('.selGiftDiv').find('input').attr('selproducts', JSON.stringify(selProducts));
                $('.selGiftDiv').find('input').val(selProductName);
            }
        })
    })
}

//初始化条件
function initConditionRow(isReadOnly, conditionCode, conditionName, isSingle) {
    var $new = $('.conditionhide').clone().removeClass('conditionhide').show();
    if (isReadOnly) {
        $($new).find('.choseCondition option').remove();
        $($new).find('.choseCondition').append('<option value="{0}">{1}</option>'.format(conditionCode, conditionName));
        $($new).find('.removeCondition').attr('disabled', true);
    }
    if (isSingle) {
        $($new).find('.cx_condition').hide();
    }
    if (conditionCode && conditionCode != null) {
        $($new).find('.choseCondition option[value="{0}"]'.format(conditionCode)).attr('selected', true);
        initConditionBody($new, conditionCode);
    }

    $('#tab2 .form_validation_reg').append($new);
    return $new;
}

//当选择不同条件时，显示不同的内容
function initConditionBody($row, condition) {

    $($row).attr('name', condition);
    $($row).find('.cx_body').empty();
    switch (condition) {
        case "PayCash":
        case "PayCash2":
        case "PayQuantity":
        case "PayQuantity2":
            $($row).find('.cx_body').append('<div class="cx_cash">\
                                    <input type="number" /></div>');
            break;
        case "Goods":
        case "GoodsArea":
            $($row).find('.cx_body').append('<div class="cx_goods"><div>\
                                <input class="selProductInCondition" type="text"  style="float:left;" selproducts="[]" readonly />\
                                <a class="showExtend selProductInCondition" style="float:left;"></a>\
                            </div> </div>');
            $($row).on('click', '.selProductInCondition', function () {
                showProduct(function (selProducts, selProductName) {
                    if (selProducts) {
                        $('input.selProductInCondition').attr('selproducts', JSON.stringify(selProducts));
                        $('input.selProductInCondition').val(selProductName);
                    }
                })
            })
            //initGoods($row, true);
            break;
        case "BuyAmount":
            $($row).find('.cx_body').append('<div class="cx_buy">\
                                    <input type="number" /></div>');
            break;
        case "MemberLevel":
            $($row).find('.cx_body').append('<div class="cx_level"></div>');
            initMemberLevel($row);
            break;
        case "MemberBirthday":
            $($row).find('.cx_body').append('<div class="cx_Birthday"></div>');
            initBirthday($row);
            break;
        case "MemberPointMulti"://会员积分分组 
            $($row).find('.cx_body').append('<div class="cx_levels"></div>');
            initMemberLevelGroup($row, '积分倍数<input type="number" class="levelpoints"/>');
            break;
        case "MemberGiftMulti":
            $($row).find('.cx_body').append('<div class="cx_levels"></div>');
            initMemberLevelGroup($row, '请选择礼品<div  style="display:inline"><a class="showExtend selGift" style="float:right;"></a><input type="text" class="levelproducts" selproducts="[]" readonly style="float:left;" /></div>');
            break;
            //case "Payment": 
            //    break;
            //case "MemberLevelUpDay":
            //    $($row).find('.cx_body').append('<div class="cx_dataregion cx_memberLevelUpDay"></div>');
            //    initDayRegion();
            //    break;
            //case "MemberCardDay": 
            //    $($row).find('.cx_body').append('<div class="cx_dataregion cx_memberCardDay"></div>');
            //    initDayRegion();
            //    break;
    }

    bindInput();
}


function getConditionContent(data, $row, condition) {

    switch (condition) {
        case "PayCash":
            var cash = $($row).find('input').val();
            if (cash == '') {
                $.dialog('请输入条件：购买金额 ');
                return false;
            }
            data["PayCash"] = cash;
            break;
        case "PayCash2":
            var cash = $($row).find('input').val();
            if (cash == '') {
                $.dialog('请输入条件：购买金额(整除) ');
                return false;
            }
            data["PayCash2"] = cash;
            break;
        case "PayQuantity":
            var cash = $($row).find('input').val();
            if (cash == '') {
                $.dialog('请输入条件：购买数量(区间) ');
                return false;
            }
            data["PayQuantity"] = cash;
            break;
        case "PayQuantity2":
            var cash = $($row).find('input').val();
            if (cash == '') {
                $.dialog('请输入条件：购买数量(整除) ');
                return false;
            }
            data["PayQuantity2"] = cash;
            break;
            //单品，暂时不用
            //case "Goods": 
        case "GoodsArea":
            var goods = JSON.parse($('.selProductInCondition').attr('selproducts'));
            if (goods.length == 0) {
                data["GoodsArea"] = { OverAll: true };
            }
            else
                data["GoodsArea"] = { OverAll: false, Goods: goods, GoodsName: $('.selProductInCondition').val() };

            break;
            //case "Payment": 
            //    break;
        case "BuyAmount":
            var buyAmount = $($row).find('input').val();
            if (buyAmount == '') {
                $.dialog('请输入条件：购买数量 ');
                return false;
            }
            data["BuyAmount"] = buyAmount;
            break;
        case "MemberLevel":
            var levels = [];
            $('.cx_level .input_c:checked').each(function (n, value) {
                levels.push($(value).val());
            })

            data["MemberLevel"] = levels;
            break;
        case "MemberBirthday":
            var birthdays = $('.cx_Birthday .input_c:checked').val();

            if (birthdays == '') {
                $.dialog('请输入条件：会员生日 ');
                return false;
            }
            data["MemberBirthday"] = birthdays;
            break;
        case "MemberPointMulti"://会员积分分组 
            {
                var isValid = true;
                var LevelPoints = [];
                $('.levelpoints').each(function (n, single) {
                    var level = $(single).attr('level');

                    var multi = $(single).val();
                    if (multi == '') {
                        isValid = false;
                        return false;
                    }
                    LevelPoints.push({ Level: level, Multi: multi });
                })
                if (!isValid) {
                    $.dialog('数据不全！ ');
                    return false;
                }
                data["MemberPointMulti"] = LevelPoints;
            }
            break;
        case "MemberGiftMulti":
            {
                var isValid = true;
                var LevelPoints = [];
                $('.levelproducts').each(function (n, single) {
                    var level = $(single).attr('level');

                    var ps = $(single).attr('selproducts');
                    var psName = $(single).val();
                    LevelPoints.push({ Level: level, Gifts: JSON.parse(ps), GiftName: psName });
                })
                if (!isValid) {
                    $.dialog('数据不全！ ');
                    return false;
                }
                data["MemberGiftMulti"] = LevelPoints;
            }
            break;
        case "MemberLevelUpDay":
            {
                var start = $($row).find('.startRegion').val();
                var end = $($row).find('.endRegion').val();
                if (start == '' || end == '') {
                    $.dialog('请输入条件：会员升级日 ');
                    return false;
                }

                data["MemberLevelUpDay"] = { Start: start, End: end };
                break;
            }
        case "MemberCardDay":
            {
                var start = $($row).find('.startRegion').val();
                var end = $($row).find('.endRegion').val();
                if (start == '' || end == '') {
                    $.dialog('请输入条件：会员开卡日 ');
                    return false;
                }

                data["MemberCardDay"] = { Start: start, End: end };
                break;
            }
            break;
    }
}


function setConditionContent($row, key, value) {
    if (!isNaN(value) && value < 0)
        value = -value;
    switch (key) {
        case "PayCash":
        case "PayCash2":
        case "PayQuantity":
        case "PayQuantity2":
            $($row).find('input').val(value);
            break;
            //case "Goods":
            //    $.each(value, function (n, v) {
            //        $('.cx_goods .input_c[value="{0}"]'.format(v)).prop('checked', true);
            //    })
            //    break;
        case "GoodsArea":
            if (!value.OverAll) {
                $('.selProductInCondition').val(value.GoodsName);
                $('.selProductInCondition').attr('selproducts', JSON.stringify(value.Goods));
            }
            else {
                $('.selProductInCondition').val('');
                $('.selProductInCondition').attr('selproducts', '[]');
            }
            break;
        case "BuyAmount":
            $($row).find('input').val(value);
            break;
        case "MemberLevel":

            $.each(value, function (n, v) {
                $('.cx_level .input_c[value="{0}"]'.format(v)).prop('checked', true);
            })
            break;
        case "MemberBirthday":
            $('.cx_Birthday .input_c[value="{0}"]'.format(value)).prop('checked', true);

            break;

        case "MemberPointMulti"://会员积分分组 
            {
                $.each(value, function (n, single) {
                    $('.levelpoints[level="{0}"]'.format(single.Level)).val(single.Multi);
                });
            }
            break;
        case "MemberGiftMulti":
            {
                $.each(value, function (n, single) {
                    var ctl = $('.levelproducts[level="{0}"]'.format(single.Level));
                    $(ctl).val(single.GiftName);
                    $(ctl).attr('selproducts', JSON.stringify(single.Gifts));
                });
            }
            break;
            //case "Payment": 
            //    break;
            //case "MemberLevelUpDay":
            //    {
            //        var start = $($row).find('.startRegion').val();
            //        var end = $($row).find('.endRegion').val();
            //        if (start == '' || end == '') {
            //            $.dialog('请输入条件：会员升级日 ');
            //            return false;
            //        }
            //        data["MemberLevelUpDay"] = { Start: start, End: end };
            //        break;
            //    }
            //case "MemberCardDay":
            //    {
            //        var start = $($row).find('.startRegion').val();
            //        var end = $($row).find('.endRegion').val();
            //        if (start == '' || end == '') {
            //            $.dialog('请输入条件：会员开卡日 ');
            //            return false;
            //        }
            //        data["MemberCardDay"] = { Start: start, End: end };
            //        break;
            //    }
            //    break;
    }
}

//保存条件
function getConditionBody() {
    var isConditionValue = true;

    var data = {};
    $('#tab2 .row-fluid:not(.conditionhide)').each(function (n, value) {

        var select = $(value).find('.choseCondition').val();
        if (select == 'notSelect') {
            isConditionValue = false;
            $.dialog('请选择条件！');
            return;
        }
        var r = getConditionContent(data, $(value), select);
        if (r == false) {
            isConditionValue = false;
            return;
        }
    })

    if (!isConditionValue)
        return false;
    return data;
}

//加载条件
var loadData = {};
function setConditionBody(data, endfun) {
    //var test = '{"BuyAmount":"11","Goods":["code2"],"MemberLevel":["v2","v3"]}';
    loadData = JSON.parse(data);
    if ($('#tab2 .row-fluid:not(.conditionhide) .choseCondition:first').val() == 'notSelect')
        $('#tab2 .row-fluid:not(.conditionhide)').remove();
    $.each(loadData, function (key, value) {
        var $row = $('#tab2 .row-fluid[name="{0}"]'.format(key));

        if ($row == null || $row.length == 0)
            $row = initConditionRow(false, key, dicPromotionCondition[key]);

    })
    //等待数据加载完
    setTimeout(function () {
        $('#tab2 .row-fluid:not(.conditionhide)').each(function (n, row) {

            var select = $(row).find('.choseCondition').val();
            var selValue = loadData[select];
            if (selValue)
                setConditionContent(row, select, selValue);

            if (endfun != null)
                endfun();
        })
    }, 1000);
}

/****************************** tab2 end ***********************************/


/****************************** tab3 start ***********************************/
//初始化积分数据
function initPointType() {
    var postUrl = "/MemberSalesPromotion/GetConditionOptions";
    return ajax(postUrl, { "Type": "PointType" }, function (data) {
        if (data.IsPass) {
            $('.pointType').empty();
            $.each(data.Obj[0], function (n, value) {
                $('.pointType').append('<option value="{0}">{1}</option>'
                                    .format(value.Value, value.Text));

            })
        }
        else
            $.dialog(data.MSG);
    });
}
//初始化等级数据
function initCardType() {
    var postUrl = "/MemberSalesPromotion/GetConditionOptions";
    return ajax(postUrl, { "Type": "CardType" }, function (data) {
        if (data.IsPass) {
            $('.cardType').empty();
            $.each(data.Obj[0], function (n, value) {
                $('.cardType').append('<option value="{0}">{1}</option>'
                                    .format(value.Value, value.Text));

            })
        }
        else
            $.dialog(data.MSG);
    });
}


//执行动作
function initAction(endfun) {

    $('#tab3').on('click', '.selGift,.gift', function () {
        showProduct(function (selProducts, selProductName) {
            if (selProducts) {
                $('.gift').attr('selproducts', JSON.stringify(selProducts));
                $('.gift').val(selProductName);
            }
        })
    })

    $.when(initPointType(), initCardType())
    .done(function () {
        if (endfun)
            endfun();
    });
}

function getAction() {
    var data = {};
    var valid = true;
    $('.action.on').find('select,input:not(.notShow)').each(function (n, ctl) {
        var name = $(ctl).attr('name');
        var value = $(ctl).val();
        var type = $(ctl).attr('type');
        if (type == 'checkbox')
            value = $(ctl).prop('checked');
        var products = $(ctl).attr('selproducts');
        if (products != '' && products != undefined)
            value = { Products: JSON.parse(products), Display: value };
        var isRequire = $(ctl).hasClass('isrequire');

        if (isRequire && value == '') {
            $.dialog('先输入必填');
            valid = false;
        }
        if (name == 'PointMulti' && value <= 0) {
            $.dialog('积分倍数必须大于等于0');
            valid = false;
        }
        if (name == 'PointNumber' && value <= 0) {
            $.dialog('积分值必须大于等于0');
            valid = false;
        }
        data[name] = value;
    })
    if (valid)
        return data;
    else
        return false;
}

function setAction(dataAct) {
    var data = JSON.parse(dataAct);
    $('.action.on').find('select,input').each(function (n, ctl) {
        var name = $(ctl).attr('name');
        var type = $(ctl).attr('type');
        if (type == 'checkbox' && data[name])
            $(ctl).prop('checked', true);
        else if (name == "GiftCode") {
            $(ctl).attr('selproducts', JSON.stringify(data[name].Products));
            $(ctl).val(data[name].Display);
        }
        else if (data[name])
            $(ctl).val(data[name]);
        if (name == 'DiscountType') {
            changeDiscountType();
        }
    })

}
//折扣选择
$('.discountType').change(changeDiscountType);

function changeDiscountType() {
    var type = $('.discountType').val();
    $('#txtAmountValue').hide();
    $('#txtDiscountValue').hide();

    $('.notShow').removeClass('notShow');
    if (type == 'amount') {
        $('.discountTitle').html('折扣额<span class="f_req">*</span>');
        $('#txtAmountValue').show();
        $('#txtDiscountValue').addClass('notShow');
    }
    if (type == 'discount') {
        $('.discountTitle').html('折扣率<span class="f_req">*</span>');
        $('#txtDiscountValue').show();
        $('#txtAmountValue').addClass('notShow');
    }
}


/****************************** tab3 end ***********************************/
var dt_ProductTable;
function loadProductTable() {
    if (!dt_ProductTable) {
        dt_ProductTable = $('#dtProduct').dataTable({
            sAjaxSource: '/MemberSalesPromotion/GetProductData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aaSorting: [[1, "asc"]],
            aoColumns: [
                {
                    data: null, title: "操作", sClass: "center", sortable: false, render: function (obj) {
                        return '<input  class="selProduct" goodscode="{0}" goodsname="{1}" type=\"checkbox\" name=\"selproduct\" />'.format(obj.GoodsCode, obj.GoodsName);
                    }
                },
                { data: 'GoodsCode', title: "商品编码", sortable: true },
                { data: 'GoodsName', title: "商品名称", sortable: true },
                { data: 'GoodsSort', title: "商品类型", sortable: true },
                { data: 'GoodsBrand', title: "商品品牌", sortable: true },
                { data: 'GoodsRTLPRC', title: "包装规格", sortable: false },
            ],
            fnFixData: function (d) {
                d.push({ name: 'Code', value: $("#txtProductCode").val() });
                d.push({ name: 'Name', value: $("#txtProductName").val() });
                d.push({ name: 'Sort', value: $("#txtGoodsSort").val() });
                d.push({ name: 'Brand', value: $("#txtGoodsBrand").val() });
            },
            rulesfnInitComplete:function(d){

            }
        });
    }
    else {
        dt_ProductTable.fnDraw();
    }
}

function bindProductTable() {
    loadProductTable();
    //查询
    $("#btnSearchProduct").click(function () {
        loadProductTable();
    })
}

function showProduct(endfun) {
    $('.selProduct:checked').prop('checked', false);
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#select_dialog",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#btnSelectOK").click(function (e) {
                var selProducts = [];
                var selProductName = '';
                $('.selProduct:checked').each(function (n, value) {
                    selProducts.push($(value).attr('goodscode'));
                    selProductName += $(value).attr('goodsname') + ';';
                })
                if (selProducts.length == 0) {
                    $.dialog('请选择产品！');
                    $.colorbox.resize();
                    return;
                }
                if (endfun)
                    endfun(selProducts, selProductName);
                e.preventDefault();
                $.colorbox.close();
            });

            $("#btnSelectCancel").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });
        }
    });
}


$('#allstore').bind('click', function () {
    $('.selStore:checked').prop('checked', false)
    var ids = $('#allstore').attr('value');
    if (ids != undefined)
        $.each(JSON.parse(ids), function (n, value) {
            $('.selStore[ids={0}]'.format(value)).prop('checked', true);
          
        });
    showStore(function (selStores, selStoreIds) {
        //$('#allstore').attr('selStores', JSON.stringify(selStores)); 
        $('#allstore').attr('value', JSON.stringify(selStoreIds));
        $('#txtAllowStore').val('选择{0}家门店'.format(selStoreIds.length));
    }); 
})

var dt_StoreTable;
bindStoreTable();
function bindStoreTable() {
    loadStoreTable();
    //查询
    $("#btnSearchStore").click(function () {
        loadStoreTable();
    })
}

function loadStoreTable(  ) {
    if (!dt_StoreTable) {
        dt_StoreTable = $('#dtStore').dataTable({
            sAjaxSource: '/MemberSalesPromotion/GetStoreData',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aaSorting: [[1, "asc"]],
            aoColumns: [
                {
                    data: null, title: "操作", sClass: "center", sortable: false, render: function (obj) {
                        //var checked = '';
                        //if ($.inArray(obj.BaseDataID, selids))
                        //    checked = ' checked="checked" ';
                        return '<input  class="selStore" code="{0}" ids="{1}" type=\"checkbox\" name=\"selStore\"   />'.format(obj.StoreCode, obj.BaseDataID);
                    }
                },
                { data: 'StoreCode', title: "门店代码", sortable: true },
                { data: 'StoreName', title: "门店名称", sortable: true },
                { data: 'ProvinceStore', title: "所在省", sortable: false },
                { data: 'CityStore', title: "所在市", sortable: false },
            ],
            fnFixData: function (d) {
                d.push({ name: 'Code', value: $("#txtStoreCode").val() });
                d.push({ name: 'Name', value: $("#txtStoreName").val() });
            },
            rulesfnInitComplete: function (d) {
             
            }
        });
    }
    else {
        dt_StoreTable.fnDraw();
    }
}

//选择门店
function showStore(endfun) {
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#select_dialog2",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#select_dialog2 .confirm_yes").click(function (e) {
                var selStores = [];
                var selStoreIds = [];
                $('.selStore:checked').each(function (n, value) {
                    selStores.push($(value).attr('code'));
                    selStoreIds.push($(value).attr('ids'));
                }) 
                if (endfun)
                    endfun(selStores, selStoreIds);
                e.preventDefault();
                $.colorbox.close();
            });

            $("#select_dialog2 .confirm_no").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });
        }
    });
}

function reloadPage(billid) {
    var ruleID = billid;
    $("#hideRuleID").val(ruleID); 
    $("#form1").submit();
}

$('#btnCopy').click(function () {
    showCopyDialog();
})

//选择复制会员促销单
function showCopyDialog() {
    var billid = $('#txtBillID').val();
    if (billid == '') {
        $.dialog('只有保存的会员促销单才能复制！');
        return;
    }
    $('#selStartDate').val($('#txtStartDate').val());
    $('#selEndDate').val($('#txtEndDate').val());
    $('#selStartDate,#selEndDate,#hideRuleID').attr('disabled', false); 
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#select_dialog3",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#select_dialog3 .confirm_yes").click(function (e) {
                var start = $('#selStartDate').val();
                var end = $('#selEndDate').val();
                 
                var postUrl = "/MemberSalesPromotion/CopyPromotion";
                ajax(postUrl, { "billid": billid, "start": start, "end": end }, function (data) {
                    if (data.IsPass) {
                        reloadPage(data.Obj[0].BillID); 
                    };
                    $.dialog(data.MSG);
                });
                e.preventDefault();
                $.colorbox.close();
            });

            $("#select_dialog3 .confirm_no").click(function (e) {
                e.preventDefault();
                $.colorbox.close();
            });
        }
    });
}


//对统一控件绑定
function bindInput() {
    setTimeout(function () {
        $('input:not(.zeroNumber)[type="number"]').each(function (n, value) {
            var keyup = $(value).attr('onkeyup');
            if (keyup == undefined) {
                $(value).attr('onkeyup', "extractNumber(this, 2, false);");
                $(value).attr('onkeypress', " return onlyNumbers(event);");
                $(value).attr('step', "0.01");
            }
        });
        $('.zeroNumber[type="number"]').each(function (n, value) {
            var keyup = $(value).attr('onkeyup');
            if (keyup == undefined) {
                $(value).attr('onkeyup', "extractNumber(this, 0, false);");
                $(value).attr('onkeypress', " return onlyNumbers(event);");
                $(value).attr('step', "1");
            }
        });
    }, 2000);
}
//input 控制 控件 为数字 
function extractNumber(obj, decimalPlaces, allowNegative) {
    var temp = obj.value;

    // avoid changing things if already formatted correctly
    var reg0Str = '[0-9]*';
    if (decimalPlaces > 0) {
        reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
    } else if (decimalPlaces < 0) {
        reg0Str += '\\.?[0-9]*';
    }
    reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
    reg0Str = reg0Str + '$';
    var reg0 = new RegExp(reg0Str);
    if (reg0.test(temp)) return true;

    // first replace all non numbers
    var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
    var reg1 = new RegExp(reg1Str, 'g');
    temp = temp.replace(reg1, '');

    if (allowNegative) {
        // replace extra negative
        var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
        var reg2 = /-/g;
        temp = temp.replace(reg2, '');
        if (hasNegative) temp = '-' + temp;
    }


    if (decimalPlaces != 0) {
        var reg3 = /\./g;
        var reg3Array = reg3.exec(temp);
        if (reg3Array != null) {
            // keep only first occurrence of .
            // and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
            var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
            reg3Right = reg3Right.replace(reg3, '');
            reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
            temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
        }
    }

    obj.value = temp;
}
//控件只能输入数字 
function onlyNumbers(e) {
    var keynum
    var keychar
    var numcheck

    if (window.event) // IE
    {
        keynum = e.keyCode
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    numcheck = /[0-9\.%]/
    return numcheck.test(keychar)
}
//检查数字 
function testNumber(obj) {
    var val = obj.value;
    var value = parseInt(val);
    if (value > 100)
        value = 100;
    if (value < 0 || isNaN(value))
        value = 0;
    $('#txtDiscountValue').val(value + '%');
}