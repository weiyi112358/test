var dt_Table;
var identity = 0;
var array = new Array();
var reg = new RegExp("^[1-9]\\d*$");
var reg7 = new RegExp("/\b\d{7}\b/");
$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    };
    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Distribution/GetCardList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[8, "desc"]],
        aoColumns: [
        { data: 'Id', title: "单号", sortable: true,bVisible:false },
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
        //{ data: 'AcceptingUnit', title: "接收单位", sortable: false, sClass: "center" },
          {
              data: 'Channel', title: "所属代理商", sortable: false, sClass: "center"
          },
        { data: 'ApplyNumber', title: "申请数量", sortable: false, sClass: "center" },
        { data: 'ApproveNumber', title: "批准数量", sortable: false, sClass: "center" },
        { data: 'DeliverNumber', title: "交货数量", sortable: false, sClass: "center" },
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },
        {
            data: null, title: "操作", sortable: false, sClass: "center", render: function (r) {            
                var html = '<button  onclick="goDetail(\''+r.Id+'\');" >详情</button>';
                return html;
            }
        },
        ],     
        fnFixData: function (d) {
            d.push({ name: 'id', value: $("#OddNumbers").val() });
            d.push({ name: 'executeStatus', value: $("#ExecuteStatus").find('option:selected').val() });
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() });
            d.push({ name: 'applyNumber', value: $("#ApplyNumber1").val().trim() });
            d.push({ name: 'approveNumber', value: $("#ApproveNumber").val().trim() });
            d.push({ name: 'deliverNumber', value: $("#DeliverNumber").val().trim() });
            d.push({ name: 'modifyTime', value: $("#ModifyTime").val() });
        }
    });

  
    loadCompany();

    
    $('#selChannel').change(function ()
    {
        var channelCode = $(this).val();
        loadStore(channelCode);
    })


    $('#search').click(function () {
        var approveNumber = $('#ApproveNumber').val().trim();
        var applyNumber = $('#ApplyNumber1').val().trim();
        var deliverNumber = $('#DeliverNumber').val().trim();
        var oddId = $('#OddNumbers').val().trim();
        var createTime = $('#ModifyTime').val().trim();
        var executeStatus= $("#ExecuteStatus").val();
        var status=$("#Status").val();   
        if (approveNumber != "" && (reg.test(approveNumber) == false || approveNumber.length > 7)) {
            $.dialog("请输入不大于7位数的数字类型的批准数量");
            return false;
        };
        if (applyNumber != "" && (reg.test(applyNumber) == false || applyNumber.length > 7)) {
            $.dialog("请输入不大于7位数的数字类型的申请数量");
            return false;
        };
        if (deliverNumber != "" && (reg.test(deliverNumber) == false || deliverNumber.length > 7)) {
            $.dialog("请输入不大于7位数的数字类型的交货数量");
            return false;
        };
        if (oddId!=""&&(reg.test(oddId)==false||oddId.length>14)) {
            $.dialog("请输入不大于12位数的数字类型单号");
            return false;
        }
        $('#txtExcelOddNumbers').val(oddId);
        $('#txtExcelExecuteStatus').val(status);
        $('#txtExcelStatus').val(executeStatus);
        $('#txtExcelApplyNumber').val(applyNumber);
        $('#txtExcelApproveNumber').val(approveNumber);
        $('#txtExcelDeliverNumber').val(deliverNumber);
        $('#txtExcelCreateTime').val(createTime);
        dt_Table.fnDraw();
    });

    $("#ArriveTime").datepicker({ dateFormat: "yyyy-MM-dd", startDate: "-1" });
    $("#ModifyTime").datepicker({ dateFormat: "yyyy-MM-dd" });
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
            } else {
                var opt = "<option value=''>无</option>";
                $('#Code').append(opt);
            }

        },
        error: function (e) {

        }
    });

    $('.Purpose').empty();
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
                    opt += '<option value=' + result.data[i].OptionValue + '>' +result.data[i].OptionText + '</option>';
                }
                $('#Purpose').append(opt);
            } else {
                var opt = "<option value=''>无</option>";
                $('#Purpose').append(opt);
            }
        },
        error: function (e) {

        }
    });

   


    $('#btnAddInsert').click(function () {
        if (array.length==0) {
            $.dialog("请先保存新卡数据");
            return false;
        }
   
        var postdata = {
            param: JSON.stringify(array)
        }
        $.post('/Distribution/AddCard', postdata, function (result) {
            if (result) {
                goClear();
                array = [];
                $('#showinfo').empty();
                $.dialog("操作成功");
                dt_Table.fnDraw();
                $.colorbox.close();
            }
            else {
                $.dialog("操作失败");
            }
        }, 'json')
    })

   
    $('#btnAddSave').click(function () {
        var acceptingUnit = $('#AcceptingUnit').find('option:selected').val();
        var acceptText = $('#AcceptingUnit').find('option:selected').text();
        var arriveTime = $('#ArriveTime').val();
        var code = $('#Code').find('option:selected').val();
        var purpose = $('#Purpose').find('option:selected').val();
        var applyNumber = $('#ApplyNumber').val();
        if (acceptingUnit=="") {
            $.dialog("请选择门店");
            return false;
        }
        if (arriveTime=="") {
            $.dialog("请选择到期时间");
            return false;
        }
   
        if (applyNumber == "" || reg.test(applyNumber)==false) {
            $.dialog("请填写正确的申请数量");
            return false;
        }
     
        var data = {
            AcceptingUnit: acceptingUnit,
            ArriveTime: arriveTime,
            Code: code,
            Purpose: purpose,
            ApplyNumber: applyNumber,
            Identity: identity
        }
        array.push(data);
        var strCode = $('#Code').find('option:selected').text();
        var strPurpose = $('#Purpose').find('option:selected').text();
        var text = '<tr id="' + acceptingUnit + ',' + acceptText + '"><td>' + strCode + '</td><td>' + strPurpose + '</td><td>' + applyNumber + '</td><td>' + acceptText + '</td><td><button onclick="deleteSelf(' + identity + ',this)">删除</button></td></tr>';
        $('#showinfo').append(text);
        identity++;     
        $('#selChannel').prop('disabled', true);  
        $('#AcceptingUnit').find('option:selected').remove();
        $(".chzn_a").trigger("liszt:updated");
    })

    $('#btndelete').click(function () {
        goClear();     
    })


    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');       
    })

    
})



function goEdit() {
    $("#addBrand_dialog .heading h3").html("卡片新增");
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
}

function goClear() {
    $('#queryCondition input:text').empty();
    $('#modalAdd input:text').empty();
    array = [];
    $('#showinfo').empty();
    identity = 0;
    loadCompany();
    $('#selChannel').prop('disabled', false);
    $('#AcceptingUnit').empty().append('<option value="">==请选择==</option>')
    $(".chzn_a").trigger("liszt:updated");
}


function deleteSelf(identity,obj)
{
    for (var i = 0; i < array.length; i++) {
        if (array[i].Identity == identity) {
            array.splice(i);
        }
    }
    var info = $(obj).parent().parent().attr('id');
    var strTrArray = info.split(',');
    var option = '<option value="' + strTrArray[0] + '">' + strTrArray[1] + '</option>';
    $('#AcceptingUnit').append(option);
    $(".chzn_a").trigger("liszt:updated");
    $(obj).parent().parent().remove(); 
}


function goDetail(id)
{
    $('#pageKey').val("ApplyCard");
    $('#id').val(id);
    $('#formDetail').submit(); 
}

//加载代理商
function loadCompany() {
    var $acceptingUnit = $('#selChannel');
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
function loadStore(obj) {
    var $store = $('#AcceptingUnit');
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