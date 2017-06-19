var array = new Array();
var dt_Table;
var identity = 0;
var btnCount = 3;
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    };

    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Distribution/GetCardOutBranchList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[6, "desc"]],
        aoColumns: [
        { data: 'OddId', title: "单号", sortable: true, bVisible: false },
         {
             data: 'OddIdNo', title: "单号", sortable: true, sClass: "center"
         },
           {
               data: 'Status', title: "状态", sortable: false, sClass: "center",render: function (d) {
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
        { data: 'SendingUnit', title: "发送单位", sortable: false, sClass: "center" },
        //{ data: 'AcceptintUnit', title: "接收单位", sortable: false, sClass: "center" },
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
            d.push({ name: 'boxNo', value: $("#BoxNo").val().trim() });
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() });
            //d.push({ name: 'acceptingUnit', value: $("#AcceptingUnit1").find('option:selected').val() });
            d.push({ name: 'modifyBy', value: $("#ModifyBy").val() });
        }
    });

    $("#ModifyBy").datepicker({ dateFormat: "yyyy-MM-dd" });
    $('#search').click(function () {
        var boxNum = $("#BoxNum").val().trim();
        if (boxNum != "" && (reg.test(boxNum) == false||boxNum.length>7)) {
            $.dialog("请输入正确的盒数量");
            return false;
        }
        var oddId = $('#OddId').val().trim();
        if (oddId != "" && (reg.test(oddId) == false||oddId.length>14)) {
            $.dialog("请输入正确的单号");
            return false;
        }
        var boxNo = $('#BoxNo').val().trim();
        if (boxNo != "" && (reg.test(boxNo) == false||boxNo.length>14)) {
            $.dialog("请输入正确的盒号");
            return false;
        }
        var status = $("#Status").val();
        //var acceptingUnit = $("#AcceptingUnit1").val();
        var acceptingUnit ="";
        var createTime = $("#ModifyBy").val();
        $('#txtExcelOddIdNo').val(oddId);
        $('#txtExcelBoxNum').val(boxNum);
        $('#txtExcelStatus').val(status);
        $('#txtExcelAcceptingUnit').val(acceptingUnit);
        $('#txtExcelCreateTime').val(createTime);
        $('#txtExcelBoxNo').val(boxNo);
        dt_Table.fnDraw();
    });


    //$('#AcceptingUnit1').empty();
    //$.ajax({
    //    type: 'post',
    //    url: '/Distribution/LoadStore',
    //    dataType: 'json',
    //    data: {},
    //    success: function (result) {
    //        if (result.data.length > 0) {
    //            var opt = "";
    //            for (var i = 0; i < result.data.length; i++) {
    //                opt += '<option value=' + result.data[i].ShoppeCode + '>' + result.data[i].ShoppeName + '/' + result.data[i].ShoppeCode + '</option>';
    //            }
    //            $('#AcceptingUnit1').append('<option value="">请选择</option>');
    //            $('#AcceptingUnit1').append(opt);
    //            $(".chzn_a").trigger("liszt:updated");
    //        }
    //        else {
    //            opt = "<option value=''>无</option>";
    //            $('#AcceptingUnit1').append(opt);
    //            $(".chzn_a").trigger("liszt:updated");
    //        }

    //    },
    //    error: function (e) {
    //        e.responseText;
    //    }
    //})

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
   

    var companyCode = "";
    $('#SendingUnit').empty();
    $.ajax({
        type: 'post',
        url: '/Purchases/LoadCompany',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '/' + result.data[i].CompanyCode + '</option>';
                }           
                $('#SendingUnit').append(opt);
                $(".chzn_a").trigger("liszt:updated");
                companyCode = result.data[0].CompanyCode
                $('#AcceptingShoppe').empty();
                $.ajax({
                    type: 'post',
                    url: '/Distribution/LoadStore',
                    dataType: 'json',
                    data: { companyCode: companyCode },
                    success: function (result) {
                        if (result.data.length > 0) {
                            var opt = "";
                            for (var i = 0; i < result.data.length; i++) {
                                opt += '<option value=' + result.data[i].ShoppeCode + '>' + result.data[i].ShoppeName + '/' + result.data[i].ShoppeCode + '</option>';
                            }
                            $('#AcceptingShoppe').append(opt);
                            $(".chzn_a").trigger("liszt:updated");
                        }
                        else {
                            var opt = "<option value=''>无</option>";
                            $('#AcceptingShoppe').append(opt);
                            $(".chzn_a").trigger("liszt:updated");
                        }
                    },
                    error: function (e) {
                        e.responseText;
                    }
                })
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

    
    $('#SendingUnit').change(function () {
        var postCode = $(this).find('option:selected').val();
        var postdata = {
            companyCode: postCode
        };
        $('#AcceptingShoppe').empty();
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
                    $('#AcceptingShoppe').append(opt);
                    $(".chzn_a").trigger("liszt:updated");
                }
                else {
                    var opt = "<option value=''>无</option>";
                    $('#AcceptingShoppe').append(opt);
                    $(".chzn_a").trigger("liszt:updated");
                }
            },
            error: function (e) {
                e.responseText;
            }
        })
    })




    //提交
    $('#btnAddInsert').click(function () {
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        };
        var postdata = {
            jsonParam: JSON.stringify(array),
            status: 0,
        };
        showLoading("正在领出中");
        $.post('/Distribution/AddBranchCard', postdata, function (result) {
            if (result) {
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
                $.dialog("操作失败");
            }
        }, 'json')
    })

    //提交并审核
    $('#btnAddInsertStatus').click(function () {
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        };
        var postdata = {
            jsonParam: JSON.stringify(array),
            status: 1,
        };
        showLoading("正在领出中");
        $.post('/Distribution/AddBranchCard', postdata, function (result) {
            if (result) {
                hideLoading();
                goClear();
                array = [];
                $('#showinfo').empty();
                $('#showinfo2').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $.colorbox.close();
                $('#SendingUnit').attr('disabled', false);
                $('#Choose').attr('disabled', false);
            }
            else {
                $('#SendingUnit').attr('disabled', false);
                $('#Choose').attr('disabled', false);
                $.dialog("操作失败");
            }
        }, 'json')
    })


    //添加
    $('#btnAddSave').click(function () {
        var boxNo = $('#box').find('option:selected').text();
        if (boxNo == "无" || boxNo == "") {
            $.dialog("请先筛选盒号");
            return false;
        };

        var create = $('#CreateBy').val().trim();
        var acceptingShoppe = $('#AcceptingShoppe').find('option:selected').val();
        var sendingUnit = $('#SendingUnit').find('option:selected').val();
        var remark = $('#Remark').val().trim();
        var oddId = $('#OddId').find('option:selected').val();
        var boxNoVal = $('#box').find('option:selected').val();
        var isOddId = $('#MappingApply').find('option:selected').val();
        //var oddIdNo = $('#OddId1').val().trim();
        var oddIdNo = "";
        var postdata = { boxNo: boxNo };
        var acceptingStoreName = $('#AcceptingShoppe').find('option:selected').text();
        $.post('/Distribution/GetBoxInfo', postdata, function (result) {
            if (result.data.length > 0) {
                var data = {
                    Create: create,
                    OddId: oddId,
                    AcceptingShoppe: acceptingShoppe,
                    SendingUnit: sendingUnit,
                    Remark: remark,
                    BoxNo: boxNoVal,
                    CardTypeCode: result.data[0].CardTypeCode,
                    Purpose: result.data[0].PurposeId,
                    CardNumIn: result.data[0].CardNumIn,
                    IsOddId: isOddId,
                    OddIdNo: oddIdNo
                };
                array.push(data);            
                var tr = '<tr><td>' + result.data[0].CardTypeName + '</td><td>' + result.data[0].Purpose + '</td><td>' + result.data[0].CardNumIn + '</td><td><button onclick="deleteSelf(' + boxNo + ',this)">删除</button></td></tr>';
                $('#showinfo').append(tr)
                var tr1 = '<tr id="tr' + boxNo + '" boxNo="' + boxNoVal + '"><td>' + result.data[0].BoxNo + '</td><td>' + result.data[0].BeginCardNo + '</td><td>' + result.data[0].EndCardNo + '</td><td>' + acceptingStoreName + '</td></tr>';
                $('#showinfo2').append(tr1);
                identity++;
                $('#box').find('option:selected').remove();
                $(".chzn_a").trigger("liszt:updated");
            }
        }, 'json');
    });





    $('#AcceptingShoppe').prop('readonly', false);
    $('#MappingApply').change(function () {
        var value = $(this).find('option:selected').val();
        $('#box').empty();
        if (value == "1") {
            $.post('/Distribution/GetBranchOddIdList', {}, function (result) {
                if (result.data.length > 0) {
                    var opt = "";
                    for (var i = 0; i < result.data.length; i++) {
                        opt += '<option value=' + result.data[i].OddId + '>' + result.data[i].OddId + '</option>';
                    }
                    $('#oldOddId').append(opt);
                    $('#AcceptingShoppe').empty();
                    $('#AcceptingShoppe').attr('readonly', true);
                }
                else {
                    var opt = '<option >无</option>';
                    $('#oldOddId').append(opt);
                }
            }, 'json')
            $('#oddNumber').show();
            //$('#oddNumber').prop('display', '');
            //$('#AcceptingShoppeDiv').hide();
            $('#AcceptingShoppeDiv input').val('');
        }
        else {
            $('#oddNumber').hide();
            $('#AcceptingShoppeDiv').show();
            $('#AcceptingShoppe').empty();
            $('#AcceptingShoppe').attr('readonly', false);
            $.ajax({
                type: 'post',
                url: '/Purchases/LoadShoppe',
                dataType: 'json',
                data: {},
                success: function (result) {
                    if (result.data.length > 0) {
                        var opt = "";
                        for (var i = 0; i < result.data.length; i++) {
                            opt += '<option value=' + result.data[i].ShoppeCode + '>' + result.data[i].ShoppeName + '</option>';
                        }
                        $('#AcceptingShoppe').append(opt);
                    }
                    else {
                        opt = "<option value=''>无</option>";
                        $('#AcceptingShoppe').append(opt);
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
        if ($('#AcceptingShoppe').prop('readonly') == false) {
            var accept = $('#AcceptingShoppe').find('option:selected').val();
            if (accept == "") {
                $.dialog("请选择接门店");
                return false;
            }
            var post = $('#SendingUnit').find('option:selected').val();
            if (post=="") {
                $.dialog("请选择发货分公司");
                return false;
            }
            var postdata = {
                acceptingUnit: post
            };
            $.post('/Distribution/ChooseBranchOddId', postdata, function (result) {
                $('#box').empty();
                if (result.data.length > 0) {                
                    var opt = "";
                    for (var i = 0; i < result.data.length; i++) {
                        opt += '<option value=' + result.data[i].BoxNo + '>' + result.data[i].BoxNo + '</option>';
                    }
                    $('#box').append(opt);
                    $('#SendingUnit').attr('disabled', true);
                    $('#Choose').attr('disabled', true);
                    $(".chzn_a").trigger("liszt:updated");
                }
                else {
                   var opt = '<option value="0">无</option>';
                   $('#box').append(opt);
                   $(this).attr('disabled', true);
                   $(".chzn_a").trigger("liszt:updated");
                }
            }, 'json')
        }
        if ($('#oddNumber').css('display') == "block") {
            var oddId = $('#oldOddId').find('option:selected').val();
            var postdata = {
                oddId: oddId
            }
            $.post('/Distribution/ChooseBranchOddId1', postdata, function (result) {
                if (result.data.length > 0) {
                    var opt = "";
                    for (var i = 0; i < result.data.length; i++) {
                        opt += '<option value=' + result.data[i].BoxNo + '>' + result.data[i].BoxNo + '</option>';
                    }
                    $('#box').append(opt);              
                    $(".chzn_a").trigger("liszt:updated");
                }
                else {
                    opt = '<option value="0">无</option>';
                    $('#box').append(opt);
                    $(".chzn_a").trigger("liszt:updated");
                }
            }, 'json');
        };
    });



    //$('#Choose').click(function () { 
    //    var accept = $('#SendingUnit').find('option:selected').val();
    //    if (accept == "") {
    //        $.dialog("请选择门店");
    //        return false;
    //    };
    //    var postdata = {
    //        AcceptingShoppe: accept
    //    };
    //    $.post('/Distribution/ChooseOddId', postdata, function (result) {
    //        if (result.data.length > 0) {
    //            var opt = "";
    //            for (var i = 0; i < result.data.length; i++) {
    //                opt += '<option value=' + result.data[i].BoxNo + '>' + result.data[i].BoxNo + '</option>';
    //            }
    //            $('#box').append(opt);
    //        }
    //        else {
    //           var opt = '<option>无</option>';
    //            $('#box').append(opt);
    //        }
            
    //    }, 'json')
    //});


    //$('#btndelete').click(function () {
    //    goClear();
    //});


    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');
    });

    $('#btnAddCancel').click(function () {
        $.colorbox.close();
        $('#modalAdd input').val('');
        $('#showinfo2').empty();
        $('#showinfo1').empty();
        $('#box option').remove();
    })
})



function goEdit() {
    $("#addBrand_dialog .heading h3").html("卡领出新增");
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
    $.colorbox.resize();
    var user = $('#modalAdd').data().username;   
    $('#Status1').val('未审核');
    $('#CreateBy').val(user);
    $('#ModifyBy1').val(user);
    $('#oddNumber').hide();
    $('#box').empty();
    loadCompanyAndStore();
    $('#SendingUnit').prop('disabled', false);
    $('#Choose').attr('disabled', false);
    $('#box').empty().append('<option value="">请筛选盒号</option>');

}

function goClear() {
    $('#queryCondition input:text').val('');
    $('#modalAdd input:text').val('');
    array = [];
    $('#showinfo').empty();
    identity = 0;
}


function deleteSelf(boxNo, obj) {
    for (var i = 0; i < array.length; i++) {
        if (array[i].BoxNo == boxNo)
        {
            array.splice(i);
        }
    }
    //array.splice(identity);
    var $tr = $('#tr' + boxNo + '');
    //var boxNo = $tr.attr('boxNo');
    $tr.remove();
    var option = '<option value="' + boxNo + '">' + boxNo + '</option>';
    $('#box').append(option);
    $(".chzn_a").trigger("liszt:updated");
    $(obj).parent().parent().remove();
}

function goDetail(id) {
    $('#pageKey').val("CardOutBranch");
    $('#id').val(id);
    $('#formDetail').submit();
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


function loadCompanyAndStore()
{
    $('#SendingUnit').empty();
    $(".chzn_a").trigger("liszt:updated");
    $.ajax({
        type: 'post',
        url: '/PurchasesNew/LoadCompany',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '/' + result.data[i].CompanyCode + '</option>';
                }
                $('#SendingUnit').append('<option value="">请选择</option>');
                $('#SendingUnit').append(opt);
                $(".chzn_a").trigger("liszt:updated");
                companyCode = result.data[0].CompanyCode
                $('#AcceptingShoppe').empty();
                $(".chzn_a").trigger("liszt:updated");
                $.ajax({
                    type: 'post',
                    url: '/Distribution/LoadStore',
                    dataType: 'json',
                    data: { companyCode: companyCode },
                    success: function (result) {
                        if (result.data.length > 0) {
                            var opt = "";
                            for (var i = 0; i < result.data.length; i++) {
                                opt += '<option value=' + result.data[i].ShoppeCode + '>' + result.data[i].ShoppeName + '/' + result.data[i].ShoppeCode + '</option>';
                            }
                            $('#AcceptingShoppe').append(opt);
                            $(".chzn_a").trigger("liszt:updated");
                        }
                        else {
                            var opt = "<option value=''>无</option>";
                            $('#AcceptingShoppe').append(opt);
                            $(".chzn_a").trigger("liszt:updated");
                        }
                    },
                    error: function (e) {
                        e.responseText;
                    }
                })
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