var dt_Table;
var reg = new RegExp("^[1-9]\\d*$");
$(document).ready(function () {
    //goClear();
    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Distribution/GetCardInList',
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
               data: 'Status', title: "状态", sortable: false,sClass: "center", render: function (d) {
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
            d.push({ name: 'status', value: $("#Status").find('option:selected').val()});
            d.push({ name: 'modifyBy', value: $("#ModifyBy").val() });
            //d.push({ name: 'code', value: $("#Code").find('option:selected').val()});
        }
    });

    $("#ModifyBy").datepicker({ dateFormat: "yyyy-MM-dd" });
    $('#search').click(function () {
        var boxNum = $('#BoxNum').val().trim();
        //var cardNum = $('#CardNum').val().trim();
        if (boxNum != "" && reg.test(boxNum) == false) {
            $.dialog("请输入正确的总盒数");
            return false;
        };
        var oddId = $('#OddId').val().trim();
        if (oddId != "" && reg.test(oddId) == false) {
            $.dialog("请输入正确的单号");
            return false;
        }
        dt_Table.fnDraw();
    });

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

    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');
    });
});


function goDetail(id) {
    window.location.href = '/Distribution/CardCenterDetailPage?pageKey=CardIn&id=' + id;
}