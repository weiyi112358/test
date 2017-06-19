
var dt_Table;
var reg = new RegExp("^[1-9]\\d*$");
var reg7 = new RegExp("/\b\d{7}\b/");
$(document).ready(function () {
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    if ($(".chzn_a").attr('disabled') == 'disabled') {
        $(".chzn_a").next('.chzn-container').attr('disabled', 'disabled');
    };


    dt_Table = $('#CardTable').dataTable({
        sAjaxSource: '/Distribution/GetVerifyList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[9, "desc"]],
        aoColumns: [
        {
            data: null, title: '<input type="checkbox"  id="ckALL" />', sortable: false, sClass: "center", render: function (r) {
                var str = '<input type="checkbox" name="txtCK" allotId="' + r.Id + '" />';
                return str;
            }
        },     
        { data: 'Id', title: "单号", sortable: false, bVisible: false },
        { data: 'OddIdNo', title: "单号", sortable: false , sClass: "center" },
        //{
        //    data: 'ExecuteStatus', title: "执行状态", sortable: false, sClass: "center", render: function (d) {
        //        var msg = "";
        //        if (d == "0") {
        //            msg = "未交货";
        //        }
        //        else if (d == "1") {
        //            msg = "待交货";
        //        }
        //        else if (d == "2") {
        //            msg = "部分交货";
        //        }
        //        else if (d == "3") {
        //            msg = "全部交货";
        //        }
        //        else {
        //            msg = "拒绝交货";
        //        }
        //        return msg;
        //    }
        //},
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
        { data: 'AcceptingUnit', title: "代理商", sortable: false, sClass: "center" },
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
                 var html = '<button  onclick="goDetail(\'' + r.Id + '\');" >详情</button>';
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
            d.push({ name: 'modifyBy', value: $("#ModifyTime").val() });
            d.push({ name: 'acceptingUnit', value: $("#AcceptingUnit1").find('option:selected').val() });
        }
    });

    $('#AcceptingUnit1').empty();
    loadStore();
    //$.ajax({
    //    type: 'post',
    //    url: '/Purchases/LoadCompany',
    //    dataType: 'json',
    //    data: {},
    //    success: function (result) {
    //        if (result.data.length > 0) {
    //            var opt = '<option value="">请选择</option>';
    //            for (var i = 0; i < result.data.length; i++) {
    //                opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '</option>';
    //            }
    //            $('#AcceptingUnit1').append(opt);
    //        }
    //        else {
    //            opt = '<option value="">无</option>';
    //            $('#AcceptingUnit1').append(opt);
    //        }
    //    },
    //    error: function (e) {
    //        e.responseText;
    //    }
    //})

    $("#ModifyTime").datepicker({ dateFormat: "yyyy-MM-dd" });

    $("#ckALL").click(function () {
        $("[name=txtCK]:checkbox").prop("checked", this.checked);
    });
    $("input[name=txtCK]:checkbox").click(function () {
        var flag = true;
        $("input[name=txtCK]:checkbox").each(function () {
            if (this.checked == false) {
                flag = false;
            }
        });
        $("#ckALL").prop("checked", flag);
    });

    var arrayTrue = new Array();
    $('#btnVerify').click(function () {
        $("input[name=txtCK]:checkbox").each(function () {
            if (this.checked == true) {
                var data = {
                    Id: $(this).attr('allotId'),
                    Status: 1
                }
                arrayTrue.push(data);
            }
        });
        if (arrayTrue.length==0) {
            $.dialog("请勾选单号");
            return false;
        }
        var postdata = {
            jsonParam:JSON.stringify(arrayTrue)
        }
        $.post('/Distribution/ExecuteVerify', postdata, function (result) {
            if (result.IsPass) {
                $.dialog("操作成功");
                arrayTrue = [];
                dt_Table.fnDraw();
            }
            else {
                $.dialog("操作失败");
            }
        },'json')
    });

    var arrayFalse = new Array();
    $('#btnNoVerify').click(function () {
        $("input[name=txtCK]:checkbox").each(function () {
            if (this.checked == true) {
                var data = {
                    Id: $(this).attr('allotId'),
                    Status: 2
                }
                arrayFalse.push(data);
            }
        });
        if (arrayFalse.length == 0) {
            $.dialog("请勾选单号");
            return false;
        }
        var postdata = {
            jsonParam: JSON.stringify(arrayFalse)
        }
        $.post('/Distribution/ExecuteVerify', postdata, function (result) {
            if (result.IsPass) {
                alert("操作成功");
                arrayFalse = [];
                dt_Table.fnDraw();
            }
            else {
                alert("操作失败");
            }
        }, 'json')
    });



    $('#search').click(function () {
        var approveNumber = $('#ApproveNumber').val();
        var applyNumber = $('#ApplyNumber1').val();
        var deliverNumber = $('#DeliverNumber').val();
        var oddNumbers = $("#OddNumbers").val();
        if (approveNumber != "" && (reg.test(approveNumber) == false||approveNumber.length>7)) {
            $.dialog("请输入不大于7位数的数字类型的批准数量");
            return false;
        };
        if (applyNumber != "" && (reg.test(applyNumber) == false||applyNumber.length>7)) {
            $.dialog("请输入不大于7位数的数字类型的申请数量");
            return false;
        };
        if (deliverNumber != "" && (reg.test(deliverNumber) == false||deliverNumber.length>7)) {
            $.dialog("请输入不大于7位数的数字类型的交货数量");
            return false;
        };
        if (oddNumbers != "" && (reg.test(oddNumbers) == false || oddNumbers.length > 14)) {
            $.dialog("请输入不大于12位数的数字类型单号");
            return false;
        };
        dt_Table.fnDraw();
    });



    $('#btnRemove').click(function () {
        $('#queryCondition input:text').val('');
    })
})

function goDetail(id) {
    $('#pageKey').val("VerifyCard");
    $('#id').val(id);
    $('#formDetail').submit();
}

function loadStore() {
    var $store = $('#AcceptingUnit1');
    $store.empty();
    $.post('/Distribution/LoadStore', {}, function (result) {
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


