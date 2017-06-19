var array = new Array();
var dt_Table;
var identity = 0;
var destineNum = 0;
var idArray = new Array();
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
$(document).ready(function () {
    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Distribution/GetCardOutTitleList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[7, "desc"]],
        aoColumns: [
        { data: 'OddId', title: "单号", sClass: "center", bVisible: false },
          {
              data: 'OddIdNo', title: "单号", sortable: true, sClass: "center"
          },
           {
               data: 'Status', title: "状态", sClass: "center", sortable: false, render: function (d) {
                   var msg = "";
                   if (d == "1") {
                       msg = "审核通过";
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
        { data: 'SendingUnit', title: "发送单位", sClass: "center", sortable: false, },
        { data: 'AcceptintUnit', title: "接收单位", sortable: false, sClass: "center" },
        { data: 'BoxNumber', title: "总盒数", sortable: false, sClass: "center" },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
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
            d.push({ name: 'boxNum', value: $("#BoxNum").val().trim() });
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() }); 
            d.push({ name: 'acceptingUnit', value: $("#AcceptingUnit1").find('option:selected').val() });
            d.push({ name: 'modifyBy', value: $("#ModifyBy").val() });
        }
    });

    $("#ModifyBy").datepicker({ dateFormat: "yyyy-MM-dd" });

    $('#search').click(function () {
        var boxNum = $("#BoxNum").val().trim();
        if (boxNum != "" && reg.test(boxNum) == false) {
            $.dialog("请输入正确的盒号");
            return false;
        }
        var oddId = $('#OddId').val().trim();
        if (oddId != "" && reg.test(oddId) == false) {
            $.dialog("请输入正确的单号");
            return false;
        }
        dt_Table.fnDraw();
    });

    $('#showCardDetail').hide();

    $('#AcceptingUnit').prop('readonly', false);


    var provinceCode = "";
    $('#AcceptingUnit').empty();
    $.ajax({
        type: 'post',
        url: '/Purchases/LoadCompany',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '</option>';
                }
                $('#AcceptingUnit1').append('<option value="">请选择</option>');
                $('#AcceptingUnit1').append(opt);
                $('#AcceptingUnit').append(opt);
            }
            else {
                opt = "<option value=''>无</option>";
                $('#AcceptingUnit1').append(opt);
                $('#AcceptingUnit').append(opt);         
            }
          
        },
        error: function (e) {
            e.responseText;
        }
    })


    //var provinceCode = "";
    //$('#AcceptingUnit').change(function () {
    //    $.ajax({
    //        type: 'post',
    //        url: '/Purchases/LoadCompany',
    //        dataType: 'json',
    //        data: { provinceName: $('#AcceptingUnit').find('option:selected').text() },
    //        success: function (result) {
    //            if (result.data.length > 0) {
    //                var opt = "";
    //                for (var i = 0; i < result.data.length; i++) {
    //                    provinceCode = result.data[i].Code;
    //                }
    //            }
    //            else {

    //            }
    //        },
    //        error: function (e) {
    //            e.responseText;
    //        }

    //    });
    //});


    //提交
    $('#btnAddInsert').click(function () {
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        }
        var postdata = {
            jsonParam: JSON.stringify(array),    
            status: 0,
        }
        showLoading("正在领出中");
        $.post('/Distribution/AddTitleCard', postdata, function (result) {
            if (result.IsPass) {
                hideLoading();
                goClear();
                array = [];
                $('#showinfo').empty();
                $('#showinfo2').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $.colorbox.close();
            }
            else {
                hideLoading();
                $.dialog(result.MSG != "" ? result.MSG : "操作失败");
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
        }
        showLoading("正在领出中");
        $.post('/Distribution/AddTitleCard', postdata, function (result) {
            if (result.IsPass) {
                hideLoading();
                goClear();
                array = [];
                $('#showinfo').empty();
                $('#showinfo2').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $.colorbox.close();
            }
            else {
                hideLoading();
                $.dialog(result.MSG!=""?result.MSG:"操作失败");
            }
        }, 'json')
    });

    

    //添加
    $('#btnAddSave').click(function () {
        var boxNo = $('#box').find('option:selected').text();
        if (boxNo == "无"||boxNo=="") {
            $.dialog("请先筛选盒号");
            return false;
        }
        var chooseOddId1ValueResult=$('#box').find('option:selected').val();
        var arrayValueResult=[];
        arrayValueResult=chooseOddId1ValueResult.split(',');
        var acceptingUnit= $('#AcceptingUnit').find('option:selected').val();
        acceptingUnit = (acceptingUnit == null) ? arrayValueResult[0] : acceptingUnit
        var create = $('#CreateBy').val().trim();
        var acceptingShoppe = $('#AcceptingShoppe').find('option:selected').val();
        var sendingUnit = $('#SendingUnit').val().trim();
        var remark = $('#Remark').val().trim();
        var oddId = $('#OddId').find('option:selected').val();  
        var isOddId = $('#MappingApply').find('option:selected').val();
        var applyId = arrayValueResult[1];
        //var oddIdNo=$('#oldOddId').val();
        var oddIdNo = "";
        var postdata = { boxNo: boxNo };
        $.post('/Distribution/GetBoxInfo', postdata, function (result) {
            if (result.data.length > 0) {              
                var data = {
                    AcceptingUnit: acceptingUnit,
                    OddIdNo: oddIdNo,
                    Create: create,
                    OddId: oddId,
                    AcceptingShoppe: acceptingShoppe,
                    SendingUnit: sendingUnit,
                    Remark: remark,
                    BoxNo: boxNo,
                    CardTypeCode: result.data[0].CardTypeCode,
                    Purpose: result.data[0].PurposeId,
                    CardNumIn: result.data[0].CardNumIn,
                    IsOddId: isOddId,
                    ApplyId: applyId,
                };
                array.push(data);             
                var tr = '<tr><td>' + result.data[0].CardTypeName + '</td><td>' + result.data[0].Purpose + '</td><td>' + result.data[0].CardNumIn + '</td><td><button onclick="deleteSelf(' + identity + ',this)">删除</button></td></tr>';
                $('#showinfo').append(tr)
                var tr1 = '<tr><td>' + result.data[0].BoxNo + '</td><td>' + result.data[0].BeginCardNo + '</td><td>' + result.data[0].EndCardNo + '</td></tr>';
                $('#showinfo2').append(tr1);
                identity++;
            }
        }, 'json');
       
        $('#box').find('option:selected').remove();
    })



    
    
 
    $('#MappingApply').change(function () {
        var value = $(this).find('option:selected').val();
        $('#box').empty();
        if (value == "1") {      
            $('#oddNumber').show();
            //$('#oddNumber').prop('display', '');
            //$('#AcceptingUnitDiv').hide();
            $('#AcceptingUnit').attr('readonly', true);
            $('#AcceptingUnit').empty()
            $('#AcceptingUnitDiv input').val('');
        }
        else {
            $('#oddNumber').hide();
            $('#AcceptingUnitDiv').show();
            $('#AcceptingUnit').empty();
            $('#AcceptingUnit').attr('readonly', false);
            $.ajax({
                type: 'post',
                url: '/Purchases/LoadCompany',
                dataType: 'json',
                data: {},
                success: function (result) {
                    if (result.data.length > 0) {
                        var opt = "";
                        for (var i = 0; i < result.data.length; i++) {
                            opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '</option>';
                        }
                        $('#AcceptingUnit').append(opt);
                    }
                    else {
                       var opt = "<option value=''>无</option>";
                        $('#AcceptingUnit').append(opt);
                    }

                },
                error: function (e) {
                    e.responseText;
                }
            })
        }
    })


    $('#Choose').click(function () {
        $('#box').empty();
        if ($('#AcceptingUnit').prop('readonly') == false) {
            var accept = $('#AcceptingUnit').find('option:selected').val();
            if (accept == "") {
                $.dialog("请选择接收单位");
                return false;
            }
            var postdata = {
                acceptingUnit: accept
            };
            $.post('/Distribution/ChooseOddId', postdata, function (result) {
                if (result.data.length > 0) {
                    var opt = "";
                    for (var i = 0; i < result.data.length; i++) {
                        opt += '<option value=' + result.data[i].BoxNo + '>' + result.data[i].BoxNo + '</option>';
                    }
                    $('#box').append(opt);
                }
                else {
                  var  opt = '<option value="">无</option>';
                    $('#box').append(opt);
                }
            }, 'json')
        }
        if ($('#oddNumber').css('display') == "block") {
            var temp = $('#MadeId').find('option:selected').val();
            var tempArray = [];
            tempArray = temp.split(',');
            var oddId = tempArray[0];
            var postdata = {
                oddId:oddId
            }
            $.post('/Distribution/ChooseOddId1', postdata, function (result) {
                if (result.data.length > 0) {
                    var opt = "";
                    for (var i = 0; i < result.data.length; i++) {
                        opt += '<option value=' + result.data[i].AcceptingUnit + ',' + result.data[i].ApplyId + '>' + result.data[i].BoxNo + '</option>';
                    }
                    $('#box').append(opt);
                }
                else {
                   var opt = '<option value="0">无</option>';
                    $('#box').append(opt);
                }
            }, 'json');
            };     
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

    $('#btnAddCancel').click(function () {
        $.colorbox.close();
        $('#modalAdd input').val('');
        $('#showinfo2').empty();
        $('#showinfo1').empty();
        $('#box option').remove();
    })


    $('#MadeId').change(function () {
        var status = $(this).find('option:selected').val();
        statusArray = [];
        statusArray = status.split(',');
        $('#Status1').val(StatusToName(statusArray[1]));
    })


})



function goEdit() {
    $("#addBrand_dialog .heading h3").html("总部卡领出新增");
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
    var user=$('#modalAdd').data().username;
    $.colorbox.resize();     
    $('#SendingUnit').val('[9900总部]');
    $('#CreateBy').val(user);
    $('#ModifyBy1').val(user);
    $('#oddNumber').hide();
    $('#box').empty();
    
    $('#MadeId').empty();
    $.ajax({
        type: 'post',
        url: '/Distribution/LoadApplyId',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].OddId + ',' + result.data[i].Status + '>' + result.data[i].OddIdNo + '</option>';
                }
                $('#MadeId').append(opt);           
                $('#Status1').val(StatusToName(result.data[0].Status));
            }
            else {
               var opt = "<option value=''>无</option>";
               $('#MadeId').append(opt);
            }

        },
        error: function (e) {
            e.responseText;
        }
    })

    //$.post('/Distribution/GetOddIdList', {}, function (result) {
    //    if (result.IsPass) {
    //        $('#oldOddId').val(result.MSG);
    //    }
    //    else {
    //        $('#oldOddId').val("");
    //    }
    //}, 'json')

}

function goClear() {
    $('#queryCondition input:text').val('');
    $('#modalAdd input:text').val('');
    array = [];
    $('#showinfo').empty();
    identity = 0;
}


function deleteSelf(identity, obj) {
    array.splice(identity)
    $(obj).parent().parent().remove();
}

function goDetail(id) {
    window.location.href = '/Distribution/CardCenterDetailPage?pageKey=CardOutTitle&id=' + id;
}

function StatusToName(status) {
    var name = "";
    if (status == "0") {
        name = "未审核";
    }
    else if (status == "1") {
        name = "审核通过";
    }
    else {
        name = "已撤销";
    }
    return name;
}


function showLoading(desc) {

    //$("body").append("<div id=\"processingdiv\" style=\"display:none;\"><div class=\"popup\"> <div class=\"popup-body\"><div class=\"loading\"><span>" + desc + "</span></div></div></div></div>");
    $("#txtspan").html(desc);
    $("#txtspan").css("color", "#ffffff");
    //alert($("head").html());  

    $.openPopupLayer({
        name: "processing",
        width: 500,
        target: "processingdiv"
    });
}

/** 
 * 关闭loading画面 
 * @param desc 
 * @return 
 */
function hideLoading() {
    $.closePopupLayer('processing');
    //$("#processingdiv").hide();
}