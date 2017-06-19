var dt_Table;
var reg12 = new RegExp("^\\d{12}$");
var reg5 = new RegExp("/\b\d{5}\b/");
var reg = new RegExp("^[1-9]\\d*$");
var isOddId = '0';
var insertArray = new Array();
identity = 0;
var btnCount = 3;


$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    //if ($(".chzn_a").attr('disabled') == 'disabled') {
    //    $(".chzn_a").next('. $('#box').prop('disabled', false);zn-container').attr('disabled', 'disabled');
    //};
    $('#storeDiv').hide();
    loadInfo();
    loadCompany();
    resetBox();
    resetMappingApply();
});


//选择收货地点
$('#selOutAddress').on('change',function () {
    var outVal = $(this).val();
    var $storeDiv = $('#storeDiv');
    var $store = $('#store');
    var $acceptingUnitDiv = $('#acceptingUnitDiv');
    var $acceptingUnit = $('#acceptingUnit');
    var $applyOddIdDiv = $('#applyOddIdDiv');
    var $applyOddId = $('#applyOddId');
    var $mapping = $('#mappingApply');
    if (outVal=="") {
        $store.empty();
        $storeDiv.hide();
        $acceptingUnitDiv.hide();
        $acceptingUnit.empty();
        $applyOddIdDiv.hide();
        $applyOddId.empty().append('<option value="">==无==</option>')
        $mapping.prop('disabled', true);
    }
    else if (outVal == "1") {
        $store.empty();
        $storeDiv.hide();
        $acceptingUnitDiv.show();
        loadCompany();
        $applyOddIdDiv.hide();
        $applyOddId.empty().append('<option value="">==无==</option>');
        $mapping.prop('disabled', true);
    }
    else {
        $storeDiv.show();
        $acceptingUnitDiv.hide();
        $acceptingUnit.empty();
        loadStore();
        $applyOddId.empty();
        $mapping.prop('disabled', false);
    }
});


//是否关联申请单
$('#mappingApply').on('change', function () {
    var val = $(this).val();
    var $storeDiv = $('#storeDiv');
    var $store = $('#store');
    var $acceptingUnitDiv=$('#acceptingUnitDiv');
    var $acceptingUnit=$('#acceptingUnit');
    var $applyOddIdDiv = $('#applyOddIdDiv');
    var $applyOddId = $('#applyOddId');
    var $address = $('#selOutAddress');
    if (val == "0") {
        $store.empty();
        $storeDiv.hide();
        $acceptingUnitDiv.show();  
        loadCompany();
        $applyOddIdDiv.hide();
        $applyOddId.empty().append('<option value="">==无==</option>');
        $address.val("1");
    } else {  
        $storeDiv.show();
        $acceptingUnitDiv.hide();
        $acceptingUnit.empty();
        loadStore();
        $applyOddId.empty()
        $applyOddIdDiv.show();
        $address.val("2");
    }
    isOddId = val;
});


//筛选盒号
$('#choose').click(function () {
    var postdata;
    var company = $('#acceptingUnit').val();
    var store = $('#store').val();
    //旧版
    //if (isOddId == "1") {
    //    if (store == "") {
    //        $.dialog("请选择收货门店");
    //        return false;
    //    }
    //};
    var selAddress = $('#selOutAddress').val();
    if (selAddress=="") {
        $.dialog("请选择领出地点");
        return false;
    }
    //旧版
   //else if (selAddress == "1") {
   //     var company = $('#acceptingUnit').val();
   //     if (company == "") {
   //         $.dialog("请选择经销商");
   //         return false;
   //     };    
   // }
   // else {
   //     var store = $('#store').val();
   //     if (store == "") {
   //         $.dialog("请选择门店");
   //         return false;
   //     }  
   //};
    //postdata = {
    //    company: company,
    //    store: store
    //};
    postdata = {
        company: "",
        store: ""
    };
    $.post('/Distribution/ChooseBoxNoByTitle', postdata, function (result) {
        var $box = $('#box');
        $box.empty();
        if (result.data.length > 0) {
            var opt = '';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.BoxNo + '">' + data.BoxNo + '<option>'
            });
            $box.append(opt);     
            checkDivMadal("1");
            chooseDisabled("1");
        }
        else {
            $box.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        }
    }, 'json');
});

//查询单号
$('#searchOddId').click(function () {
    var storeCode = $('#store').val();
    if (storeCode == "") {
        $.dialog("请选择门店");
        return false;
    };
    loadApplyOddId(storeCode);
});

//添加
$('#btnInsert').click(function () {
    var sendingUnit = $('#sendingUnit').val().trim();
    var company = $('#acceptingUnit').val();
    var store = $('#store').val();
    var applyOddId = $('#applyOddId').val();
    var createBy = $('#createBy').val();
    var remark = $('#remark').val().trim();
    var box = $('#box').find('option:selected').val();
    var mappingValue = $('#mappingApply').val();
    if (mappingValue == "0") {
        //旧版
        //if (company == "") {
        //    $.dialog("请选择收货单位");
        //    return false;
        //}
        if (store == "") {
            $.dialog("请选择收货门店");
            return false;
        }
        if (box == "") {
            $.dialog("请筛选盒号");
            return false;
        };
    }
    else {
        if (store == "") {
            $.dialog("请选择收货门店");
            return false;
        }
        if (applyOddId == "") {
            $.dialog("请选择单号");
            return false;
        };
    }
    
    var showTr='';
    $.post('/Distribution/GetBoxInfoNew', { boxNo: box }, function (result) {
        var postdata = {
            SendUnit: sendingUnit,
            Company: company,
            Store: store,
            ApplyOddId: applyOddId,
            CreateBy: createBy,
            Remark: remark,
            BoxNo: box,
            IsOddId: isOddId,
            CardTypeCode: result.data[0].CardTypeCode,
            Purpose: result.data[0].Purpose,
            CardNumIn: result.data[0].CardNumIn,
        };
        insertArray.push(postdata); 
        identity++;
        showTr += '<tr id="tr' + box + '"><td>' + result.data[0].CardTypeName + '</td><td>' + purposeShow(result.data[0].Purpose) + '</td><td>' + result.data[0].CardNumIn + '</td>';
        $.post('/Distribution/GetBoxCardInfoNew', { boxNo: box }, function (result) {
            showTr += '<td>' + result.data[0].BoxNo + '</td><td>' + result.data[0].BeginCardNo + '</td><td>' + result.data[0].EndCardNo + '</td><td><button onclick="deleteSelf(' + box + ',this)">删除</button></td></tr>';
            $('#showinfo2').append(showTr);
        }, 'json');
    }, 'json');
    $('#box').find('option:selected').remove();
    $(".chzn_a").trigger("liszt:updated");
});

//保存
$('#btnSave').click(function () {
    var status = "0";
    var info = "保存";
    submitForm(status, info);
});


//保存并审核
$('#btnSaveStatus').click(function () {
    var status = "1";
    var info="保存并审核"
    submitForm(status,info);
});

//重置
$('#btnReset').click(function () {
    resetAll();
});

//折叠明细
$('#BoxDetail').click(function(){
    var op = btnCount % 2;
    var $showCardDetail = $('#showCardDetail');
    if (op==0) {
        $showCardDetail.hide();
    }
    else {
        $showCardDetail.show();
    }
    btnCount++;
})


//加载分公司
function loadCompany() {
    var $acceptingUnit = $('#acceptingUnit');
    $acceptingUnit.empty();
    $.post('/PurchasesNew/LoadCompany', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CompanyCode + '">' + data.CompanyName + '/' + data.CompanyCode + '<option>'
            });
            $acceptingUnit.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $acceptingUnit.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};

//加载门店
function loadStore() {
    var $store = $('#store');
    $store.empty();
    //旧版
    //var postdata = { companyCode: "直营店" };
    var postdata = { companyCode: "" };
    $.post('/Distribution/LoadStore', postdata, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.ShoppeCode + '">' + data.ShoppeName + '/' + data.ShoppeCode + '<option>'
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

//加载默认信息
function loadInfo()
{
    var $user = $('#user');
    $('#sendingUnit').val("总部[9900]");
    $('#createBy').val($user.val());
    $('#modifyBy').val($user.val());
}

//重置接收单位
function resetAcceptingUnit()
{  
    //$('#acceptingUnit').empty();
    $('#acceptingUnit').empty().append('<option value="">==无==<option>');
}

//重置门店
function resetStore()
{
    $('#store').empty().append('<option value="">==无==<option>');
}

//重置盒号
function resetBox()
{
    $('#box').empty().append('<option value="">==无==</option>');
}

//加载单号
function loadApplyOddId(storeCode)
{
    var $applyOddId = $('#applyOddId');
    var storeCode = $('#store').val();
    var postdata = {
        storeCode:storeCode
    }
    $applyOddId.empty();
    $.post('/Distribution/LoadApplyOddId', postdata, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.ApplyOddId + '">' + data.ApplyOddIdNo + '<option>'
            });
            $applyOddId.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $applyOddId.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
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
        if (insertArray == [] || insertArray.length == 0) {
            $.dialog("请先添加");
            return false;
        };
        var postdata = {
            jsonParam: JSON.stringify(insertArray),
            status: status
        };
        showLoading("正在"+info+"中");
        $.post('/Distribution/AddNewCardOutTitle', postdata, function (result) {
            hideLoading();
            if (result.IsPass) {
                $.dialog("新增卡领出成功");
                setTimeout(function () { $('#form').submit(); }, 1800);
            }
            else {
                $.dialog(result.MSG);
                checkDivMadal("0")
                chooseDisabled("0");
            }
        }, 'json');
      
    });
}

function checkDivMadal(status)
{
    var $input = $('#modalAdd input');
    var $select = $('#modalAdd select');
    if (status == "0") {   
        for (var i = 0; i < $input.length; i++) {
            if ($input[i].id == "sendingUnit" || $input[i].id == "createBy" || $input[i].id == "modifyBy") {
                continue;
            }
            $input[i].readonly = false;
        }
        for (var i = 0; i < $select.length; i++) {
            //旧版
            //if ($select[i].id == "box") {
            //    continue;
            //}
            if ($select[i].id == "box" || $select[i].id == "store") {
                continue;
            }
            $select[i].disabled = false;
        }
        $(".chzn_a").trigger("liszt:updated");    
    }
    else {
        //$input.prop('readonly', true);      
        for (var i = 0; i < $input.length-1; i++) {      
            $input[i].readonly = false;
        }
        for (var i = 0; i < $select.length; i++) {
            //旧版
            //if ($select[i].id =="box") {
            //    continue;
            //}
            if ($select[i].id == "box" || $select[i].id == "store") {
                continue;
            }
            $select[i].disabled = true
        }
        $(".chzn_a").trigger("liszt:updated");
    }
}

//删除
function deleteSelf(boxNo, obj) {
    for (var i = 0; i < insertArray.length; i++) {
        if (insertArray[i].BoxNo == boxNo) {
            insertArray.splice(i);
        }
    }
    //insertArray.splice(identity)
    var $tr = $('#tr' + boxNo + '');
    //var boxNo = $tr.attr('boxNo');
    $tr.remove();
    var option = '<option value="' + boxNo + '">' + boxNo + '</option>';
    $('#box').append(option);
    $(".chzn_a").trigger("liszt:updated");
    $(obj).parent().parent().remove();
}

//重置关联清单
function resetMappingApply()
{
    $('#mappingApply').empty().append('<option value="0">否</option><option value="1">是</option>');
}

//重置选择地点
function resetOutAddress()
{
    //旧版
    //var option = '<option value="">==请选择==</option>  <option value="1">经销商</option><option value="2">门店</option>';
    var option = '<option value="">==请选择==</option>  <option value="2">门店</option>';
    $('#selOutAddress').empty().append(option);
}                      
                 
//重置
function resetAll() {
    $('#showinfo2').empty();
    isOddId = '0';
    insertArray = [];
    identity = 0;
    resetStore();
    resetBox();
    //resetAcceptingUnit();
    loadCompany();
    resetMappingApply();
    resetOutAddress();
    $('#storeDiv').hide();
    $('#acceptingUnitDiv').hide();
    $('#applyOddId').empty();
    $('#applyOddIdDiv').hide();
    checkDivMadal("0");
    chooseDisabled("0");
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

function purposeShow(obj)
{
    return obj == "0" ? "发售" : "补发";
}

function chooseDisabled(status)
{
    if (status=="0") {
        $('#choose').prop('disabled',false)
    }
    else {
        $('#choose').prop('disabled', true);
    }
}


