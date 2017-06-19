var array = new Array();
var dt_Table;
var identity = 0;
var destineNum = 0;
var boxNo_Table;
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
$(document).ready(function () {
    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/CardProduction/GetBatchCardList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[8, "desc"]],
        aoColumns: [
        { data: 'CardId', title: "单号", sortable: true, bVisible: false },
         {
             data: 'OddIdNo', title: "单号", sortable: true, sClass: "center"
         },
           {
               data: 'Status', title: "状态", sortable: false, sClass: "center",render: function (d) {
                   var msg = "";
                   if (d == "1") {
                       msg = "已通过";
                   }
                   else if (d == "2") {
                       msg = "已失效";
                   }
                   else {
                       msg = "未审核";
                   }
                   return msg
               }
           },
           {
               data: 'IsExecute', title: "执行状态", sortable: false, sClass: "center", render: function (d) {
                   var msg = "";
                   if (d == "0") {
                       msg = "未交货";
                   }    
                   else if (d == "1") {
                       msg = "待交货";
                   }
                   else if (d == "2") {
                       msg = "部分交货";
                   }
                   else if (d == "3") {
                       msg = "全部交货";
                   }
                   else {
                       msg = "拒绝交货";
                   }
                   return msg
               }
           },         
        { data: 'CardTypeId', title: "卡类型", sortable: false, sClass: "center" },
        { data: 'BeginCardNum', title: "开始卡号", sortable: false, sClass: "center" },
        { data: 'EndCardNum', title: "截止卡号", sortable: false, sClass: "center" },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },
          {
              data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {
                  var html = '<button  onclick="goDetail(\'' + r.CardId + '\');" >详情</button>';
                  return html;
              }
          },
        ],
        fnFixData: function (d) {
            d.push({ name: 'oddId', value: $("#OddId").val().trim() });
            d.push({ name: 'IsExecute', value: $("#ExecuteStatus").find('option:selected').val() });
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() });       
            d.push({ name: 'beginCardNo', value: $("#BeginCardNo1").val().trim() });
            d.push({ name: 'endCardNo', value: $("#EndCardNo1").val().trim() });
            d.push({ name: 'modifyBy', value: $("#ModifyTime1").val().trim() });
            d.push({ name: 'type', value: $("#Code1").find('option:selected').val() });
        }
    });


    $("#ArriveTime").datepicker({ dateFormat: "yyyy-MM-dd", startDate: "-1" });
    $("#ModifyTime1").datepicker({ dateFormat: "yyyy-MM-dd" });
    
    $('#search').click(function () {
        var beginCardNo = $("#BeginCardNo1").val().trim();
        var endCardNo = $("#EndCardNo1").val().trim();
        if (beginCardNo != "" && reg12.test(beginCardNo)==false) {
            $.dialog("请输入正确的起始卡号");
            return false;
        }
        if (endCardNo != "" && reg12.test(endCardNo) == false) {
            $.dialog("请输入正确的截止卡号");
            return false;
        }
        var oddId = $('#OddId').val().trim();
        if (oddId != "" && reg.test(oddId) == false) {
            $.dialog("请输入正确的单号");
            return false;
        }
        dt_Table.fnDraw();
    });


    $('.Code').empty();
    $.ajax({
        type: 'post',
        url: '/Distribution/LoadCardType',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                //var opt = "<option value=''>请选择城市</option>";
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].Code + '>' + result.data[i].Name + '</option>';
                }
                $('#Code').append(opt);
                $('#Code1').append(opt);
            } else {
                var opt = "<option value=''>无</option>";
                $('#Code').append(opt);             
            }

        },
        error: function (e) {

        }
    });

    


    $('.BoxPurpose').empty();
    $.ajax({
        type: 'post',
        url: '/Distribution/LoadPurpose',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                //var opt = "<option value=''>请选择城市</option>";
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].OptionValue + '>' + result.data[i].OptionText + '</option>';
                }
                $('#BoxPurpose').append(opt);
            } else {
                var opt = "<option value=''>无</option>";
                $('#BoxPurpose').append(opt);
            }
        },
        error: function (e) {

        }
    });


    $('#MathRule').empty();
    $.ajax({
        type: 'post',
        url: '/CardProduction/LoadMathRule',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].OptionValue + '>' + result.data[i].OptionText + '</option>';
                    $('#MathRule').append(opt);
                }
            }
            else {
                opt += "<option value=''>无</option>";
                $('#MathRule').append(opt);
            }
        },
        error: function (e) {
            e.responseText;
        }
    })



    //保存并审核
    $('#btnAddInsert').click(function () {    
        var beginCardNo = $('#BeginCardNo').val().trim();
        var cardNum = $('#ProductionNum').val().trim();
        var endCardNo = $('#EndCardNo').val().trim();
        var arriveTime = $('#ArriveTime').val();
        if (beginCardNo == "" || reg12.test(beginCardNo) == false) {
            $.dialog("请输入正确的起始卡号");
            return false;
        }
        if (cardNum == "" || reg.test(cardNum)== false) {
            $.dialog("请输入正确的卡数量");
            return false;
        }
        if (endCardNo == "" || reg12.test(endCardNo) == false) {
            $.dialog("请输入正确的截止卡号");
            return false;
        }
        if (arriveTime == "") {
            $.dialog("请选择到达时间");
            return false;
        }
          var choose = $('#BoxNoRule').find('option:selected').val();
        if (choose=="") {
            $.dialog("请选择盒号规则");
            return false;
        }
       
        var postdata = {
            beginCardNo: beginCardNo,
            cardNum: cardNum,
            endCardNo: endCardNo
        };
        $.post('/CardProduction/GetCardInfo', postdata, function (result) {
            if (result.IsPass == false) {
                $.dialog(result.MSG);
                //$('#BeginCardNo').val('');
                //$('#ProductionNum').val('');
                //$('#EndCardNo').val('');
                goClear();
                return false;
            }
            else {
                if (choose == "A") {
                    var code = $('#Code').val().trim();
                    var purpose = $('#BoxPurpose').find('option:selected').val();
                    var data = {
                        Code: code,
                        ProductionNum: cardNum,
                        BeginCardNo: beginCardNo,
                        EndCardNo: endCardNo,
                        ArriveTime: arriveTime,
                        Purpose: purpose,
                        ArrayBoxNo: ""
                    };
                    array.push(data);
                    //identity++;
                    showLoading("正在制卡");
                    $.post('/CardProduction/AddCard', { jsonParam: JSON.stringify(array), status: 1 }, function (result1) {
                        if (result1.IsPass) {
                            hideLoading();
                            $.dialog("制发成功");                   
                            goClear();
                            dt_Table.fnDraw();
                            $.colorbox.close();
                        }
                        else {
                            hideLoading();
                            $('#ProductionNum').val('');
                            $.dialog(result1.MSG);
                        }
                    }, 'json')
                }
                if (choose == "M") {
                    var pum = parseInt($('#ProductionNum').val().trim());
                    var num = CheckNumDone(pum)
                    var countNum = $('#ckVal').val();
                    if (countNum == "") {
                        $.dialog("请勾选盒号");
                        return false
                    }
                    var opArray = new Array()
                    opArray = countNum.substring(countNum.length - 1, 1).split(',');
                    if (opArray.length < num) {
                        $.dialog("请选择不少于" +Math.ceil(num) + "个盒号");
                        return false;
                    }
                    var code = $('#Code').val().trim();
                    var purpose = $('#BoxPurpose').find('option:selected').val();
                    var data = {
                        Code: code,
                        ProductionNum: cardNum,
                        BeginCardNo: beginCardNo,
                        EndCardNo: endCardNo,
                        ArriveTime: arriveTime,
                        Purpose: purpose,
                        ArrayBoxNo: countNum
                    };
                    array.push(data);               
                    showLoading("正在制卡");
                    $.post('/CardProduction/AddCardByManual', { jsonParam: JSON.stringify(array), status: 1 }, function (result1) {
                        if (result1.IsPass) {
                            hideLoading();
                            $.dialog("制发成功");
                         
                            goClear();
                            dt_Table.fnDraw();
                            $.colorbox.close();
                        }
                        else {
                            hideLoading();
                            $('#ProductionNum').val('');
                            $.dialog(result1.MSG);
                        }
                    }, 'json')
                }
            }
        }, 'json')
    })


    


    //保存
    $('#btnAddSave').click(function () {
        var beginCardNo = $('#BeginCardNo').val().trim();
        var cardNum = $('#ProductionNum').val().trim();
        var endCardNo = $('#EndCardNo').val().trim();
        var arriveTime = $('#ArriveTime').val();
        if (beginCardNo == "" || reg12.test(beginCardNo) == false) {
            $.dialog("请输入正确的起始卡号");
            return false;
        }
        if (cardNum == "" || reg.test(cardNum) == false) {
            $.dialog("请输入正确的卡数量");
            return false;
        }
        if (endCardNo == "" || reg12.test(endCardNo) == false) {
            $.dialog("请输入正确的截止卡号");
            return false;
        }
        if (arriveTime == "") {
            $.dialog("请选择到达时间");
            return false;
        }
        var choose = $('#BoxNoRule').find('option:selected').val();
        if (choose == "") {
            $.dialog("请选择盒号规则");
            return false;
        }

        var postdata = {
            beginCardNo: beginCardNo,
            cardNum: cardNum,
            endCardNo: endCardNo
        };
        $.post('/CardProduction/GetCardInfo', postdata, function (result) {
            if (result.IsPass == false) {
                $.dialog(result.MSG);             
                goClear();
                return false;
            }
            else {
                if (choose == "A") {
                    var code = $('#Code').val().trim();
                    var purpose = $('#BoxPurpose').find('option:selected').val();
                    var data = {
                        Code: code,
                        ProductionNum: cardNum,
                        BeginCardNo: beginCardNo,
                        EndCardNo: endCardNo,
                        ArriveTime: arriveTime,
                        Purpose: purpose,
                        ArrayBoxNo: ""
                    };
                    array.push(data);              
                    showLoading("正在制卡");
                    $.post('/CardProduction/AddCard', { jsonParam: JSON.stringify(array), status: 0 }, function (result1) {
                        if (result1.IsPass) {
                            hideLoading();
                            $.dialog("制发成功");
                            goClear();
                            dt_Table.fnDraw();
                            $.colorbox.close();
                        }
                        else {
                            hideLoading();
                            $('#ProductionNum').val('');
                            $.dialog(result1.MSG);
                        }
                    }, 'json')
                }
                if (choose == "M") {
                    var pum = parseInt($('#ProductionNum').val().trim());
                    var num = CheckNumDone(pum)
                    var countNum = $('#ckVal').val();
                    if (countNum == "") {
                        $.dialog("请勾选盒号");
                        return false
                    }
                    var opArray=new Array()
                    opArray = countNum.substring(countNum.length - 1, 1).split(',');
                    if (opArray.length < num) {
                        $.dialog("请选择不少于" + num + "个盒号");
                        return false;
                    }             
                    var code = $('#Code').val().trim();
                    var purpose = $('#BoxPurpose').find('option:selected').val();
                    var data = {
                        Code: code,
                        ProductionNum: cardNum,
                        BeginCardNo: beginCardNo,
                        EndCardNo: endCardNo,
                        ArriveTime: arriveTime,
                        Purpose: purpose,
                        ArrayBoxNo: countNum
                    };
                    array.push(data);
                    showLoading("正在制卡");
                    $.post('/CardProduction/AddCardByManual', { jsonParam: JSON.stringify(array), status: 0 }, function (result1) {
                        if (result1.IsPass) {
                            hideLoading();
                            $.dialog("制发成功");
                            goClear();
                            dt_Table.fnDraw();
                            $.colorbox.close();
                        }
                        else {
                            hideLoading();
                            $('#ProductionNum').val('');
                            $.dialog(result1.MSG);
                        }
                    }, 'json')
                }
            }
        }, 'json')
    })

    $('#btndelete').click(function () {
        goClear();
    });


    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');
    });


    

  
    $('#BoxNoRule').change(function () {
        var value = $(this).find('option:selected').val();
        if (value == "A") {     
            $('#BoxNoDiv').hide();
            $('#BoxNoDiv div').hide();

        }
        else if (value == "M") {
            //if ($('#ProductionNum').val().trim()=="") {
            //    $.dialog("请输入制发卡数量");
            //    return false;
            //}
         
            if (boxNo_Table !=null) {
                boxNo_Table.fnDraw();
            };
                boxNo_Table= $('#BoxNoTable').dataTable({
                sAjaxSource: '/CardProduction/GetEmptyBoxNo',
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
    })
    


   
    $("input[name=txtCK]:checkbox").click(function () {
        var flag = true;
        $("input[name=txtCK]:checkbox").each(function () {
            if (this.checked == false) {
                flag = false;
            }
        });
        $("#ckALL").prop("checked", flag);
    });

    $('#btnBoxNoSearch').click(function () {
        var boxNoInput = $('#boxNoInput').val();
        if (boxNoInput!=""&&reg.test(boxNoInput)==false) {
            $.dialog("请输入正确的盒号");
            return false;
        }
        boxNo_Table.fnDraw();
    })
})



function goEdit() {
    $("#addBrand_dialog .heading h3").html("批量制发卡新增");
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
    $('BoxNoTable').empty();
}


function goClear() {
    $('#queryCondition input:text').val('');
    $('#modalAdd input:text').val('');
    array = [];
    identity = 0;
}

function deleteSelf(identity, cardNum, obj) {
    array.splice(identity);
    $(obj).parent().parent().remove(); 
}


function ChooseAll(obj)
{
    $("[name=txtCK]:checkbox").prop("checked", obj.checked);
    if (obj.checked==true) {
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


function goDetail(id) {
    window.location.href = '/Distribution/CardCenterDetailPage?pageKey=BatchProduction&id=' + id;
}


function checkone(a) {   
    if (a.checked) {
        $('#ckVal').val($('#ckVal').val() + a.value + ",");
    }
    else {
        $('#ckVal').val($('#ckVal').val().replace(a.value + ",", ''));
    }
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

function CheckNumDone(count)
{
    var num = parseInt(count);
    var result = 0;
    if (num<250) {
        result = 1;
    }
    else {
        result = num % 250 == 0 ? num / 250 : Math.floor(num / 250) + 1;
    } 
    return result
}