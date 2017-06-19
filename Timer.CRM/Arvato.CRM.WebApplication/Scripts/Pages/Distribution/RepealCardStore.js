var array = new Array();
var dt_Table;
var identity = 0;
var destineNum = 0;
var btnCount = 3;
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[0-9]*$");


$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    };

    $("#ModifyBy").datepicker({ dateFormat: "yyyy-MM-dd" });

    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Distribution/GetCardRepealStoreList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[8, "desc"]],
        aoColumns: [
        { data: 'OddId', title: "单号", sClass: "center", bVisible: false },
         {
             data: 'OddIdNo', title: "单号", sortable: true, sClass: "center"
         },
           {
               data: 'Status', title: "状态", sClass: "center", sortable: false, render: function (d) {
                   var msg = "";
                   if (d == "1") {
                       msg = "已通过";
                   }
                   else if (d == "2") {
                       msg = "已撤销";
                   }
                   else {
                       msg = "未审核";
                   }
                   return msg
               }
           },
        { data: 'SendingUnitName', title: "发送单位", sClass: "center", sortable: false, },
        { data: 'AcceptintUnit', title: "接收单位", sortable: false, sClass: "center" },
        { data: 'BoxNumber', title: "总盒数", sortable: false, sClass: "center" },
        { data: 'CardNumber', title: "总数量", sortable: false, sClass: "center" },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: false, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },
          {
              data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {
                  var html = '<button  onclick="goDetail(\'' + r.OddId + '\');" >详情</button>';
                  return html;
              }
          },
        ],
        fnFixData: function (d) {
            d.push({ name: 'oddId', value: $("#OddId").val().trim() });
            d.push({ name: 'boxNo', value: $("#BoxNo").val().trim() });
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() });
            //d.push({ name: 'destineNumber', value: $("#DestineNumber").val() });
            d.push({ name: 'modifyBy', value: $("#ModifyBy").val() });
        }
    });



    $('#BoxDetail').click(function () {
        var op = btnCount % 2;
        var $showCardDetail = $('#showCardDetail');
        if (op == 0) {
            $showCardDetail.hide();
        }
        else {
            $showCardDetail.show();
        }
        btnCount++;
    })


    $('#search').click(function () {
        var boxNo = $("#BoxNo").val().trim();
        if (boxNo != "" && (reg.test(parseInt(boxNo)) == false||boxNo.length>12)) {
            $.dialog("请输入正确的盒号");
            return false;
        }
        var oddId = $('#OddId').val().trim();
        if (oddId != "" && (reg.test(oddId) == false||oddId.length>14)) {
            $.dialog("请输入正确的单号");
            return false;
        }
        var status = $("#Status").val();
        var createTime = $("#ModifyBy").val();
        $('#txtExcelOddIdNo').val(oddId);
        $('#txtExcelBoxNum').val(boxNo);
        $('#txtExcelStatus').val(status);
        $('#txtExcelCreateTime').val(createTime);
        dt_Table.fnDraw();
    });

    $('#showCardDetail').hide();


    var provinceCode = "";
    loadCompany();


  


    //提交
    $('#btnAddInsert').click(function () {
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        }
        var postdata = {
            jsonParam: JSON.stringify(array),
            status: 0,
            repealtype: 0
        }
        $.post('/Distribution/AddCardRepeal', postdata, function (result) {
            if (result) {
                goClear();
                array = [];
                $('#showinfo').empty();
                $('#showinfo2').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $.colorbox.close();
            }
            else {
                $.dialog("操作失败");
            }
        }, 'json')
    })

    //提交并审核
    $('#btnAddInsertStatus').click(function () {
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        }
        var postdata = {
            jsonParam: JSON.stringify(array),
            status: 1,
            repealtype: 0
        }
        $.post('/Distribution/AddCardRepeal', postdata, function (result) {
            if (result) {
                goClear();
                array = [];
                $('#showinfo').empty();
                $('#showinfo2').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                selLock("0");
                $('#Choose').attr('disabled', false);
                $.colorbox.close();
            }
            else {
                $.dialog("操作失败");
                $('#Choose').attr('disabled', false);
                selLock("0");
            }
        }, 'json')
    });



    //添加
    $('#btnAddSave').click(function () {
        var boxNoText = $('#box').find('option:selected').text();
        var boxNo = $('#box').find('option:selected').val();
        if (boxNoText == "无" || boxNoText == "") {
            $.dialog("请先筛选盒号");
            return false;
        }
        var selReturnUnit = $('#selReturnUnit').val();
        if (selReturnUnit == "") {
            $.dialog("请先选择退货地点");
            return false;
        }
        var postdata = { boxNo: boxNo };
        $.post('/Distribution/GetBoxReturnInfo', postdata, function (result) {
            if (result.data.length > 0) {
                var acceptingUnit = $('#AcceptingUnit').val();
                var executeMsg = $('#ExecuteMsg').val();
                var create = $('#CreateBy').val();
                var acceptingShoppe = $('#AcceptingShoppe').val();
                var sendingUnit = $('#SendingUnit').val();
                var remark = $('#Remark').val();
                var oddId = $('#OddId').val();      

                var data = {
                    AcceptingUnit: acceptingUnit,
                    ExecuteMsg: executeMsg,
                    Create: create,
                    OddId: oddId,
                    AcceptingShoppe: acceptingShoppe,
                    SendingUnit: sendingUnit,
                    Remark: remark,
                    BoxNo: boxNo,
                    CardTypeCode: result.data[0].CardTypeCode,
                    Purpose: result.data[0].PurposeId,
                    CardNumIn: result.data[0].CardNumIn,
                    CanReturnCardNumber: result.data[0].CanReturnCardNumber,
                    SelReturnUnit: selReturnUnit
                };
                array.push(data);      
                identity++;
                var tr = '<tr><td>' + result.data[0].CardTypeName + '</td><td>' + result.data[0].Purpose + '</td><td>' + result.data[0].CardNumIn + '</td><td>' + result.data[0].CanReturnCardNumber + '</td><td><button onclick="deleteSelf(' + boxNo + ',this)">删除</button></td></tr>';
                $('#showinfo').append(tr)
                var tr1 = '<tr id="tr' + boxNo + '"><td name="' + result.data[0].BoxNo + '">' + result.data[0].BoxNo + '</td><td>' + result.data[0].BeginCardNo + '</td><td>' + result.data[0].EndCardNo + '</td></tr>';
                $('#showinfo2').append(tr1);
                $("#cardCount1").text(result.data[0].CardTypeName);
                $("#cardCount2").text($("#cardCount2").text()==""?1:parseInt($("#cardCount2").text())+1);
                $("#cardCount3").text($("#cardCount3").text() == "" ? result.data[0].CardNumIn : parseInt($("#cardCount3").text()) + parseInt(result.data[0].CardNumIn));
                $("#cardCount4").text($("#cardCount4").text() == "" ? result.data[0].CanReturnCardNumber : parseInt($("#cardCount4").text()) + parseInt(result.data[0].CanReturnCardNumber));
                //$("#box>option[value='" + boxNo + "']").remove();
                $('#box').find('option:selected').remove();
                $(".chzn_a").trigger("liszt:updated");
            }
        }, 'json');

    })

    //分公司联动门店
    $('#AcceptingUnit').change(function () {
        var company = $('#AcceptingUnit').find('option:selected').val();
        loadStore(company);
    
    });


    $('#Choose').click(function () {
        var send = $('#SendingUnit').val();
        if (send == "") {
            $.dialog("请选择门店");
            return false;
        };
        var selReturnUnit = $('#selReturnUnit').val();
        if (selReturnUnit == "") {
            $.dialog("请先选择退货地点");
            return false;
        }
        var postdata = {
            acceptingUnit: send
        };
        $.post('/Distribution/ChooseBox', postdata, function (result) {
            $('#box').empty();
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    if ($("#showinfo2>tr>td[name='" + result.data[i].BoxNo + "']").length == 0) {
                        opt += '<option value="' + result.data[i].BoxNo + '">' + result.data[i].BoxNo + '</option>';
                    }
                }
                $('#box').append(opt);
                selLock("1");
                $('#Choose').attr('disabled', true);              
                $(".chzn_a").trigger("liszt:updated");

            }
            else {
                opt = '<option value="0">无</option>';
                $('#box').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }

        }, 'json')
    });


    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');
    });

    $('#BoxDetail').click(function () {
        if ($('#showCardDetail').hide()) {
            $('#showCardDetail').show();
        }
        else if ($('#showCardDetail').show()) {
            $('#showCardDetail').hide();
        }
    })



})

//选择退领地点
$('#selReturnUnit').change(function () {
    var unit = $(this).find('option:selected').val();
    var $channel = $('#AcceptingUnit');
    var $store = $('#SendingUnit');
    if (unit == "") {
        $channel.prop('disabled', true);
        $store.prop('disabled', true);
        loadCompany();
        $store.empty().append('<option value="">请选择</option>');
    }
    else if (unit=="1") {
        $channel.prop('disabled', false);
        $store.prop('disabled', false);
        loadCompany();
        $store.empty().append('<option value="">请选择</option>');
    }
    else {
        $channel.prop('disabled', true);
        $store.prop('disabled', false);
        //旧版
        //loadStore('直营店');
        loadStore('');
    }

})

function goEdit() {
    $("#addBrand_dialog .heading h3").html("门店卡退领新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addBrand_dialog",
        inline: true
    });
    var user = $('#modalAdd').data().username;
    $.colorbox.resize();
    //$('#OddId1').val('guid');
    $('#Status1').val('未审核');
    $('#CreateBy').val(user);
    $('#ModifyBy1').val(user);
    $('#oddNumber').hide();
    $('#AcceptingUnit').empty().append('<option value="">请选择</option>').attr('disabled', true);
    $('#SendingUnit').empty().append('<option value="">请选择</option>').attr('disabled', true);
}

function goClear() {
    $('#queryCondition input:text').val('');
    $('#modalAdd input:text').val('');
    array = [];
    $('#showinfo').empty();
    $('#showinfo2').empty();
    identity = 0;
    loadCompany();
    //旧版
    //$('#selReturnUnit').empty().append('<option value="">==请选择==</option><option value="1">经销商</option><option value="2">总部</option>');
    $('#selReturnUnit').empty().append('<option value="">==请选择==</option><option value="2">总部</option>');
    $('#box').empty();
    $('#modalAdd select').attr('disabled', false);
    $('#Choose').attr('disabled', false);
    $('#box').empty().append('<option value="">请筛选盒号</option>');
}


function deleteSelf(boxNo, obj) {
    for (var i = 0; i < array.length; i++) {
        if (array[i].BoxNo == boxNo) {
            array.splice(i);
        }
    }
    var $tr = $('#tr' + boxNo + '');
    //var boxNo = $tr.attr('boxNo');
    $tr.remove();
    var option = '<option value="' + boxNo + '">' + boxNo + '</option>';
    $('#box').append(option);
    $(".chzn_a").trigger("liszt:updated");
    $(obj).parent().parent().remove();
}

function goDetail(id) {
    $('#pageKey').val("CardRepeal");
    $('#id').val(id);
    $('#formDetail').submit();
}

function selLock(obj)
{
    if (obj=="1") {
        $('#AcceptingUnit').prop('disabled', true);
        $('#SendingUnit').prop('disabled', true);
        $('#selReturnUnit').prop('disabled', true);
    }
    else {
        $('#AcceptingUnit').prop('disabled', true);
        $('#SendingUnit').prop('disabled', true);
        $('#selReturnUnit').prop('disabled', true);
    }
}

function loadStore(obj)
{
    var postdata = { companyCode: obj };
    $('#SendingUnit').empty();
    $.ajax({
        type: 'post',
        url: '/Distribution/LoadStore',
        dataType: 'json',
        data: postdata,
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].ShoppeCode + '>' + result.data[i].ShoppeName + '/' + result.data[i].ShoppeCode + '</option>';
                }
                $('#SendingUnit').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
            else {
                var opt = "<option value=''>无</option>";
                $('#SendingUnit').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
        },
        error: function (e) {
            e.responseText;
        }
    })
}

function loadCompany()
{
    $('#AcceptingUnit').empty();
    $.ajax({
        type: 'post',
        url: '/PurchasesNew/LoadCompany',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                opt += "<option value=''>请选择</option>";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '/' + result.data[i].CompanyCode + '</option>';
                }
                $('#AcceptingUnit').append(opt);
                $(".chzn_a").trigger("liszt:updated");
                
            }
            else {
                opt = "<option value=''>请选择</option>";
                $('#AcceptingUnit').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }

        },
        error: function (e) {
            e.responseText;
        }
    })
}