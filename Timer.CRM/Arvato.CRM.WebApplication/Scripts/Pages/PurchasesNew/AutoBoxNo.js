var dt_Table;
var reg12 = new RegExp("^\\d{12}$");
var reg = new RegExp("^[1-9]\\d*$");
var array = new Array();
$(document).ready(function () {

    goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/PurchasesNew/BoxNoList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[5, "desc"]],
        aoColumns: [
        { data: 'BoxNo', title: "盒号", sortable: true, sClass: "center" },
           {
               data: 'Status', title: "盒状态", sortable: false, sClass: "center",render: function (d) {
                   var msg = "";
                   if (d == null || d == "") {
                       msg = "未使用";
                   }
                   else {
                       msg = "已使用";
                   };
                   return msg;
               }
           },
        { data: 'CardTypeName', title: "盒内卡类型", sortable: false, sClass: "center" },
        { data: 'CardNumIn', title: "盒內卡数量", sortable: false, sClass: "center" },    
        { data: 'CreateBy', title: "最后修改人", sortable: false, sClass: "center" },
        {
            data: 'CreateTime', title: "最后修改时间", sortable: true, sClass: "center", render: function (d) {
                return d.substring(10, 2);
            }
        },     
        ],
        fnFixData: function (d) {
            d.push({ name: 'boxNo', value: $("#BoxNo").val().trim() });    
            d.push({ name: 'status', value: $("#Status").find('option:selected').val() });       
            d.push({ name: 'modifyBy', value: $("#ModifyBy").val() });
        }
    });

    $('#search').click(function () {   
        var boxNo = $('#BoxNo').val().trim();
        if (boxNo!=""&&reg12.test(boxNo)==false) {
            $.dialog("请输入正确的盒号");
            return false;
        }
        dt_Table.fnDraw();
    });

    $("#ModifyBy").datepicker({ dateFormat: "yyyy-MM-dd" });


    $('#CardType').empty();
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
                $('#CardType').append(opt);

            } else {
                var opt = "<option value=''>无</option>";
                $('#CardType').append(opt);
            }
        },
        error: function (e) {

        }
    });

    $('#Purpose').empty();
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
                $('#Purpose').append(opt);
            } else {
                var opt = "<option value=''>无</option>";
                $('#Purpose').append(opt);
            }
        },
        error: function (e) {

        }
    });
})



$('#btnAddInsert').click(function () {
    var beginCardBoxNo = $('#BeginCardBoxNo').val().trim();
    if (beginCardBoxNo=="" ||reg12.test(beginCardBoxNo)==false) {
        $.dialog("请输入正确的盒号");
        return false;
    }
   
    var cardBoxNum=$('#CardBoxNum').val().trim();
    if (cardBoxNum=="" || reg.test(cardBoxNum)==false) {
        $.dialog("请输入正确的盒数量");
        return false;
    }
    var postdata = {
        CardTypeCode: $('#CardType').find('option:selected').val(),
        Purpose: $('#Purpose').find('option:selected').val(),
        BeginCardBoxNo: beginCardBoxNo,
        CardBoxNum: cardBoxNum
    };
    array.push(postdata);
    showLoading("正在生成盒号");
    $.post('/PurchasesNew/AddBoxNo',{jsonParam:JSON.stringify(array)}, function (result) {
        if (result.IsPass) {
            hideLoading()
            $.dialog("生成盒号成功");
            $.colorbox.close();
            goClear();
            dt_Table.fnDraw();
        }
        else {
            hideLoading()
            $.dialog("生成盒号失败");
        }

    }, 'json')
     
})




$('#btnRemove').click(function () {
    goClear();
});

function goEdit() {
    $("#addBrand_dialog .heading h3").html("盒号新增");
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
    $('#queryCondition input:text').val('');
    $('#modalAdd input:text').val('');
    array = [];
    $('#showinfo').empty();
    identity = 0;
    destineNum = 0;
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