var array = new Array();
var dt_Table;
var identity = 0;
var destineNum = 0;
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
var cardTypeArray = new Array();
$(document).ready(function () {
    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Purchases/CustomizeCardList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[7, "desc"]],
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
                       msg = "审核驳回";
                   }
                   else {
                       msg = "未审核";
                   }
                   return msg
               }
           },
        { data: 'DestineNumber', title: "订货数量", sortable: false, sClass: "center" },
        { data: 'ReserveNumber', title: "收货数量", sortable: false, sClass: "center" },
        { data: 'Agent', title: "供应商", sortable: false, sClass: "center" },
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
            d.push({ name: 'agent', value: $("#Agent1").find('option:selected').val() });
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() });
            d.push({ name: 'destineNumber', value: $("#DestineNumber").val().trim() });
            d.push({ name: 'modifyBy', value: $("#ModifyBy").val() });
        }
    });


    $('#search').click(function () {
        var destineNumber = $('#DestineNumber').val().trim();
        if (destineNumber != "" && reg.test(destineNumber)==false) {
            $.dialog("请输入正确的订货数量");
            return false;
        }
        var oddId = $('#OddId').val().trim()
        if (oddId!=""&&reg.test(oddId)==false) {
            $.dialog("请输入正确的单号");
            return false;
        }
        dt_Table.fnDraw();
    });

    $("#ModifyBy").datepicker({ dateFormat: "yyyy-MM-dd"});

  

 
    $('#Agent').empty();
    $.ajax({
        type: 'post',
        url: '/Purchases/LoadAgent',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                //var opt = "<option value=''>请选择城市</option>";
                //var opt = '<option value="-1">请选择</option>';
                var opt = '';
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].AgentCode + '>' + result.data[i].AgentName + '</option>';
                }
                $('#Agent').append(opt);
            } else {
                var opt = '<option value="">无</option>';
                $('#Agent').append(opt);
            }
        },
        error: function (e) {

        }
    });
    
    $("#ModifyBy").datepicker({ dateFormat: "yyyy-MM-dd" });
    $('#EndCardNo').prop('readonly', true);

    $('#Agent1').empty();
    $.ajax({
        type: 'post',
        url: '/Purchases/LoadAgent',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {           
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


    //$('#BeginCardNo').bind('input propertychange', function () {
    //    var cardnum = $('#CardNum').val();
    //    if (cardnum == "") {
    //        $.dialog("请先输入制卡数量");
    //        $(this).val('');
    //        return false
    //    }
    //    $(this).prop('readonly', false)
    //    var value1 = $('#CardNum').val();
    //    var value2 = $('#BeginCardNo').val();
    //    var value = parseInt(value1 == "" ? 0 : value1) + parseInt(value2 == "" ? 0 : value2);
    //    $('#EndCardNo').val(value);
    //});


    //$('#CardNum').bind('input propertychange', function () {
    //    var value1 = $('#CardNum').val();
    //    var value2 = $('#BeginCardNo').val();
    //    var value = parseInt(value1 == "" ? 0 : value1) + parseInt(value2 == "" ? 0 : value2);
    //    $('#EndCardNo').val(value);
    //});


    $('#AcceptingUnit').empty();
    $.ajax({
        type: 'post',
        url: '/Purchases/LoadCompany',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length>0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '(省份编码：' + result.data[i].ComPanyProvinceCode + ')</option>';
                }
                $('#AcceptingUnit').append(opt);
                $.ajax({
                    type: 'post',
                    url: '/Purchases/LoadProviceCode',
                    data: { provinceName: $('#AcceptingUnit').find('option:selected').val() },
                    dataType: 'json',
                    success: function (result1) {
                        for (var i = 0; i < result1.data.length; i++) {
                            provinceCode = result1.data[i].Code
                        }
                    },
                    error: function () {

                    }
                });
            }
            else {
                opt += "<option value=''>无</option>";
                $('#AcceptingUnit').append(opt);
            }
        },
        error: function (e)
        {
            e.responseText;
        }
    })

    
    var provinceCode = "";
    $('#AcceptingUnit').change(function () {
        $.ajax({
            type: 'post',
            url: '/Purchases/LoadProviceCode',
            dataType: 'json',
            data: { provinceName: $('#AcceptingUnit').find('option:selected').val()},
            success: function (result) {
                if (result.data.length > 0) {
                    provinceCode = result.data[0].Code;
                }
                else {
                
                }
            },
            error: function (e) {
                e.responseText;
            }

        });
    });


    //提交
    $('#btnAddInsert').click(function () {
        var cardNum = $('#CardNum').val().trim();
        var beginCardNo = $('#BeginCardNo').val().trim();
        var endCardNo = $('#EndCardNo').val().trim();
    
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        }
        var postdata = {
            jsonParam: JSON.stringify(array),         
            status: 0,       
        }
        $.post('/Purchases/AddCard', postdata, function (result) {
            if (result) {
                goClear();
                array = [];
                $('#showinfo').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $('#CardNum').val('');
                $('#BeginCardNo').val('');
                $('#EndCardNo').val('');
                $.colorbox.close();
            }
            else {
                $.dialog("操作失败");
            }
        }, 'json')
    })

    //提交并审核
    $('#btnAddInsertStatus').click(function () {
        var cardNum = $('#CardNum').val().trim();
        var beginCardNo = $('#BeginCardNo').val().trim();
        var endCardNo = $('#EndCardNo').val().trim();
        if (array.length == 0) {
            $.dialog("请先添加");
            return false;
        }
        var postdata = {
            jsonParam: JSON.stringify(array),        
            status: 1,    
        }
        $.post('/Purchases/AddCard', postdata, function (result) {
            if (result) {
                goClear();
                array = [];
                $('#showinfo').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $('#CardNum').val('');
                $('#BeginCardNo').val('');
                $('#EndCardNo').val('');
                $.colorbox.close();
            }
            else {
                $.dialog("操作失败");
            }
        }, 'json')
    })

   
   //保存
    $('#btnAddSave').click(function () {
        var acceptingUnit = $('#AcceptingUnit').val().trim();
        var code = $('#Code').find('option:selected').val();
        var agent = $('#Agent').find('option:selected').val();
        var cardNum = $('#CardNum').val().trim();
        var beginCardNo = $('#BeginCardNo').val().trim();
        var endCardNo = $('#EndCardNo').val().trim();
        var mathRule = $('#MathRule').find('option:selected').val();
    
        if (cardNum == "" || reg.test(cardNum)==false) {
            $.dialog("请填写正确的卡数量");
            return false;
        }
        if (reg.test(cardNum)  && parseInt(cardNum)>=10000) {
            $.dialog("卡数量最小于10000张");
            return false;
        }
        if (beginCardNo == "" || reg12.test(beginCardNo) == false) {
            $.dialog("请填写正确的起始卡号");
            return false;
        }
        if (endCardNo == "" || reg12.test(endCardNo) == false) {
            $.dialog("请填写正确的截止卡号");
            return false;
        }
        if ($('#Code option').length == 0) {
            $.dialog("卡类型已选完");
            return false;
        }
  
        destineNum += parseInt(cardNum);
        var data = {
            AcceptingUnit: acceptingUnit,
            Code: code,
            Agent: agent,
            CardNum: cardNum,
            BeginCardNo: beginCardNo,
            EndCardNo: endCardNo,
            MathRule: 4,
            Status: 0,       
        }
        array.push(data);
        var strCode = $('#Code').find('option:selected').text();
        var text = '<tr><td>' + strCode + '</td><td>' + cardNum + '</td><td>' + beginCardNo + '</td><td>' + endCardNo + '</td><td><button onclick="deleteSelf(' + identity + ',' + cardNum + ',this)">删除</button></td></tr>';
        $('#showinfo').append(text);
        $('#DestineNum').val(destineNum);
        cardTypeArray.push($('#Code').find('option:selected'));
        identity++;
        $('#CardNum').val('');
        $('#BeginCardNo').val('');
        $('#EndCardNo').val('');
        $('#autoCard').prop('disabled', false);
        $('#Code').find('option:selected').remove();
    });

    //自动生成卡号
    $('#autoCard').click(function () {
        var cardNum = $('#CardNum').val().trim();
        if (cardNum==""||reg.test(cardNum)==false) {
            $.dialog("请先输入正确的卡数量");      
            return false;
        }
        var postdata = {
            provinceCode: provinceCode,
            cardNum: cardNum
        };
        showLoading("正在自动生成卡号");
        $.post('/Purchases/GetCardNo', postdata, function (result) {
            if (result.IsPass) {              
                var text = result.MSG.split(',')
                $('#BeginCardNo').val(text[0]);
                $('#EndCardNo').val(text[1]);
                hideLoading();
                //$(this).prop('disabled', true);
                $('#autoCard').prop('disabled', true)[0];
            }
            else {
                hideLoading();
                $.dialog("起始卡号或截止卡号错误");
                $('#BeginCardNo').val('');
                $('#EndCardNo').val('');
            }
        }, 'json');
    })

    //手动生成卡号
    $('#BeginCardNo').blur(function () {
        var beginCardNo = $(this).val().trim();      
        if (beginCardNo == "" || reg12.test(beginCardNo) == false) {
            $.dialog("请先输入正确的起始卡号");
            $(this).val('');
            return false;         
        }
        if (beginCardNo.substring(2, 4) != provinceCode) {
            $.dialog("起始卡号省份编码(第三位和第四位)和公司所属省份不匹配");
            return false;
        }
        var checkNo = beginCardNo.substring(0,2);
        if (checkNo!="21") {
            $.dialog("起始卡号以21开头");
            $(this).val('');
            return false;
        }
        var cardNum = $('#CardNum').val().trim();
        if (cardNum == "" || reg.test(cardNum) == false) {
            $.dialog("请先输入正确的卡数量");
            return false;
        }       
        var postdata = {
            beginCardNo:beginCardNo, 
            provinceCode: provinceCode,
            cardNum: cardNum
        };
        showLoading("正在手动生成卡号");
        $.post('/Purchases/GetCardNoManual ', postdata, function (result) {
            if (result.IsPass) {              
                $('#EndCardNo').val(result.MSG);
                hideLoading();
                $('#autoCard').attr('disabled', 'disabled');
            }
            else {
                $.dialog("起始卡号错误");           
                $('#EndCardNo').val('');
                hideLoading();
            }
        }, 'json');
    });
    

    $('#btndelete').click(function () {
        goClear();
    });
    

    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');
    });

})



function goEdit() {
    $("#addBrand_dialog .heading h3").html("定卡新增");
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
    $('#autoCard').prop('disabled', false);
    $('#Code').empty();
    $.ajax({
        type: 'post',
        url: '/Distribution/LoadCardType',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                //var opt = "<option value=''>请选择城市</option>";
                var opt = "";
                var name = ""
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].Code + '>' + result.data[i].Name + '</option>';
                }
                $('#Code').append(opt);

            } else {
                var opt = "<option value=''>无</option>";
                $('#Code').append(opt);
            }
        },
        error: function (e) {

        }
    });
}

function goClear() {
    $('#queryCondition input:text').val('');
    $('#modalAdd input:text').val('');
    array = [];
    $('#showinfo').empty();
    identity = 0;
    destineNum = 0;
    cardTypeArray = [];
}

function deleteSelf(identity,cardNum,obj) {
    array.splice(identity);
    destineNum -= cardNum;
    $(obj).parent().parent().remove();
    $('#DestineNum').val(destineNum);
    $('#Code').append(cardTypeArray[identity]);
    cardTypeArray.splice(identity);
}

function goDetail(id) {

    window.location.href = '/Distribution/CardCenterDetailPage?pageKey=Customize&id=' + id;
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