var array = new Array();
var dt_Table;
var identity = 0;
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
var typeId = "";
var typeText = "";
var Province = "";
var agentCode = "";
var cardTypeArray = new Array();
$(document).ready(function () {
    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Purchases/RetrieveCardList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[7, "desc"]],
        aoColumns: [
        { data: 'RetrieveId', title: "收卡单单号", sortable: true, bVisible: false },
          {
              data: 'OddIdNo', title: "收卡单单号", sortable: true, sClass: "center"
          },
           {
               data: 'Status', title: "状态", sortable: false,sClass: "center", render: function (d) {
                   var msg = "";
                   if (d == "1") {
                       msg = "审核通过";
                   }
                   else if (d == "2") {
                       msg = "审核驳回";
                   }
                   else {
                       msg = "未审核";
                   }
                   return msg
               }
           },
        { data: 'ReserveNumber', title: "收货数量", sortable: false, sClass: "center" },
        { data: 'Agent', title: "供应商", sortable: false, sClass: "center" },
        { data: 'OddId', title: "定卡单单号", sortable: false, sClass: "center" },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },
         {
             data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {
                 var html = '<button  onclick="goDetail(\'' + r.RetrieveId + '\');" >详情</button>';
                 return html;
             }
         },
        ],
        fnFixData: function (d) {
            d.push({ name: 'oddId', value: $("#OddId").val().trim() });
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() });
            d.push({ name: 'agent', value: $("#Agent1").find('option:selected').val() });
            d.push({ name: 'destineNumber', value: $("#DestineNumber").val().trim() });
            d.push({ name: 'retrieveId', value: $("#RetrieveId").val().trim() });
            d.push({ name: 'modifyBy', value: $("#ModifyTime").val().trim() });
        }
    });

    $("#ModifyTime").datepicker({ dateFormat: "yyyy-MM-dd" });
    $('#EndCardNo').prop('readonly', true);

    $('#search').click(function () {
        var destineNumber = $('#DestineNumber').val().trim();
        if (destineNumber != "" && reg.test(destineNumber) == false) {
            $.dialog("请输入正确的收货数量");
            return false;
        }
        var oddId = $('#OddId').val().trim()
        if (oddId != "" && reg.test(oddId) == false) {
            $.dialog("请输入正确的定卡单单号");
            return false;
        }
        var retrieveId = $('#RetrieveId').val().trim()
        if (retrieveId != "" && reg.test(retrieveId) == false) {
            $.dialog("请输入正确的收卡单单号");
            return false;
        }
        dt_Table.fnDraw();
    });


    $('#Agent1').empty();
    $.ajax({
        type: 'post',
        url: '/Purchases/LoadAgent',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                //var opt = '<option value="-1">请选择</option>';
                var opt = '<option value="">请选择</option>';
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].AgentCode + '>' + result.data[i].AgentName + '</option>';
                }
                $('#Agent1').append(opt);
            } else {
                var opt = "<option value=''>无</option>";
                $('#Agent1').append(opt);
            }
        },
        error: function (e) {

        }
    });

    //$('#OddId1').click(function () {
    //    $(this).empty();
    //    $("#addOddId_dialog .heading h3").html("订单号列表");
    //    $.colorbox({
    //        initialHeight: '0',
    //        initialWidth: '0',
    //        overlayClose: false,
    //        opacity: '0.3',
    //        href: "#addOddId_dialog",
    //        inline: true
    //    });
    //    $.colorbox.resize();

    //})


    
   

    //提交
    $('#btnAddInsert').click(function () {
        var retrieveNum = $('#RetrieveNum').val().trim();
        var beginCardNum = $('#BeginCardNo').val().trim();
        if (array.length==0) {
            $.dialog("请先添加");
            return false;
        }
        var postdata = {
            status: 0,
            jsonParam: JSON.stringify(array)
        }
        $.post('/Purchases/RetrieveCard', postdata, function (result) {
            if (result) {
                goClear();
                array = [];
                $('#showinfo').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $('#RetrieveNum').val('');
                $('#BeginCardNo').val('');
                $.colorbox.close();
            }
            else {
                $.dialog("操作失败");
            }
        }, 'json')
    })

    $('#btnAddInsertStatus').click(function () {
        var retrieveNum = $('#RetrieveNum').val().trim();
        var beginCardNum = $('#BeginCardNo').val().trim();
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        }
        var postdata = {
            status: 1,
            jsonParam: JSON.stringify(array)
        }
        $.post('/Purchases/RetrieveCard', postdata, function (result) {
            if (result) {
                goClear();
                array = [];
                $('#showinfo').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $('#RetrieveNum').val('');
                $('#BeginCardNo').val('');
                $.colorbox.close();
            }
            else {
                $.dialog("操作失败");
            }
        }, 'json')
    })


    //添加
 

    $('#btnAddSave').click(function () {
        var retrieveNum = $('#RetrieveNum').val().trim();
        var agent = agentCode;
        var beginCardNum = $('#BeginCardNo').val().trim();
        var oddId = $('#Oddid').find('option:selected').val();

        if (retrieveNum == "" ||  reg.test(retrieveNum)==false) {
            $.dialog("请填写正确的收货数量");
            return false;
        }
        if (beginCardNum == "" || reg12.test(beginCardNum) == false) {
            $.dialog("请填写正确的起始卡号");
            return false;
        }
        if (oddId=="") {
            $.dialog("没有可以收卡的定卡单号");
            return false;
        }
        var postdata = {
            beginCardNo: beginCardNum,
            retrieveNum: retrieveNum,
            oddId:oddId
        };
        var checkEnd = "";
        $.post('/Purchases/CheckBeginCardNo', postdata, function (result) {
            if (result.IsPass) {
                $('#EndCardNo').val(result.MSG);
                checkEnd = result.MSG;
                var endCardNum = $('#EndCardNo').val().trim();
                if (endCardNum == "" || endCardNum != checkEnd) {
                    $.dialog("请输入正确的截止卡号");
                    return false;
                }
                var data = {
                    OddId: oddId,
                    RetrieveNum: retrieveNum,
                    Agent: agent,
                    BeginCardNo: beginCardNum,
                    EndCardNo: endCardNum,
                    Code: typeId
                }
                array.push(data);
                var text = '<tr><td>' + typeText + '</td><td>' + retrieveNum + '</td><td>' + beginCardNum + '</td><td>' + endCardNum + '</td><td><button onclick="deleteSelf(' + identity + ',this)">删除</button></td></tr>';
                $('#showinfo').append(text);
                cardTypeArray.push($('#Oddid').find('option:selected'));
                identity++;
                $('#RetrieveNum').val('');
                $('#BeginCardNo').val('');
                $('#Oddid').find('option:selected').remove();       
            }
            else {
                $.dialog(result.MSG);
                $('#EndCardNo').val('');            
            }
        }, 'json')        
    });

    //清除
    $('#btndelete').click(function () {
        goClear();
    })

    //清除
    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');
    });


   
});



function goEdit() {
    $("#addBrand_dialog .heading h3").html("新增收卡");
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
    $('#Oddid').empty();
    //加载订单号列表
    $.post('/Purchases/GetOddIdList', {}, function (result) {
        if (result.data.length > 0) {
            var opt = "";
            for (var i = 0; i < result.data.length; i++) {
                opt += '<option value=' + result.data[i].OddId + '>' + result.data[i].OddIdNo + '</option>'
            }
            $('#Oddid').append(opt);
            var postdata = { oddId: result.data[0].OddId };
            $.post('/Purchases/GetAgentList', postdata, function (result1) {
                if (result1.data.length > 0) {
                    $('#Agent').text(result1.data[0].AgentName)
                    typeId = result1.data[0].TypeId;
                    typeText = result1.data[0].TypeText;
                    agentCode = result1.data[0].Agent;
                }
                else {
                    $('#Agent').val("无");
                }
            }, 'json')
            $.post('/Purchases/GetProvince', postdata, function (result2) {
                if (result2.data.length > 0) {
                    Province = result2.data[0].Province;
                }
            }, 'json')
        }
        else {
            opt += '<option>无</option>'
            $('#Oddid').append(opt);
        }
    }, 'json')
};


$('#Oddid').change(function () {
    var postdata = { oddId: $('#Oddid').find('option:selected').val() }
    $.post('/Purchases/GetAgentList', postdata, function (result) {
        if (result.data.length > 0) {
            $('#Agent').text(result.data[0].AgentName)
            typeId = result.data[0].TypeId;
            typeText = result.data[0].TypeText;
            agentCode = result.data[0].Agent;
        }
        else {

            $('#Agent').val("无");
        }
    }, 'json');
});


function goClear() {
    $('#queryCondition input:text').val('');
    $('#modalAdd input:text').val('');
    array = [];
    $('#showinfo').empty();
    identity = 0;
    cardTypeArray = [];
}


function deleteSelf(identity,obj) {
    array.splice(identity); 
    $(obj).parent().parent().remove();
    $('#Oddid').append(cardTypeArray[identity]);
    cardTypeArray.splice(identity);
}

function goDetail(id) {
    window.location.href = '/Distribution/CardCenterDetailPage?pageKey=Retrieve&id=' + id;
}