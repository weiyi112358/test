var dt_Table;
var productArr = "";
$(function ()
{

    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    $('#txtStoreCode').empty();
    loadCompany();
    $.ajax({
        type: 'post',
        url: '/Distribution/LoadStore',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].ShoppeCode + '>' + result.data[i].ShoppeName + '/' + result.data[i].ShoppeCode + '</option>';
                }
                $('#txtStoreCode').append('<option value="">请选择</option>');
                $('#txtStoreCode').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
            else {
                opt = "<option value=''>无</option>";
                $('#txtStoreCode').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }

        },
        error: function (e) {
            e.responseText;
        }
    })



    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/CardInfoSearch/LoadCardInfo',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aoColumns: [
            { data: 'MemberID', title: "会员ID", sClass: "center hide", sortable: false },
            { data: 'CardNo', title: "卡号", sClass: "center", sortable: false },          
            {
                data: null, title: "卡类型", sClass: "center", sortable: false, render: function (obj)
                {
                    return "会员卡";
                }
            },
            { data: 'CardType', title: "卡介质", sClass: "center", sortable: false },
            { data: "BoxNo", title: "卡所属盒", sClass: "center", sortable: false },

            {
                data: null, title: "所属门店", sClass: "center", sortable: false,
                render: function (obj)
                {
                    if (obj.StoreCode != null) {
                        return obj.StoreName + '/' + obj.StoreCode;
                    }
                    else{
                        return '-';
                    }
                }


            },
               { data: 'Agent', title: "代理商", sClass: "center", sortable: false },
            {
                data: null, title: "卡状态", sClass: "cstatus center", sortable: false,
                render: function (obj)
                {
                    var status = obj.Status;
                    switch (status)
                    {
                        case "2":
                            return "使用中";                          
                        case "1":
                            if (obj.IsSalesStatus == 1 && obj.IsUsed == false)
                            {
                                return " 已补发";
                            }
                            if (obj.IsSalesStatus==0&&obj.IsUsed==false)
                            {
                                return "未激活";
                            }
                        case "0":
                            if (obj.IsSalesStatus == 0 && obj.IsUsed == false)
                            {
                                return "已发卡";
                            }                            
                        case "-1":
                            return "已挂失";
                        case "-2":  
                            return "已冻结";
                        case "-3":
                            return "已作废";
                        default:
                            return "";
                    }
                }
            }
        ],
        fnFixData: function (d)
        {
            d.push({ name: 'CardNo', value: $("#txtCardNo").val() });
            d.push({ name: 'CardStatus', value: $("#txtCardStatus").val() });
            d.push({ name: 'StoreCode', value: $("#txtStoreCode").val() });
            d.push({ name: 'BoxNo', value: $("#txtBoxNo").val() });
            d.push({ name: 'agent', value: $("#selAgent").val() == null ? "" : ($("#selAgent").val().split(','))[0] });
        },
        fnDrawCallback: function () {
            $('#dt_Table tbody tr').addClass("rowlink").bind('click', function () {
                var mid = $(this).find('td:first').text();
                var status = $(this).find('.cstatus').text();
                if (status == '使用中' && mid != ""&&mid != null) {
                    window.open("/member360/MemberDetail?mid=" + mid);
                }
            })
        },
    });

    //查询
    $("#btnSerach").bind("click", function ()
    {
        dt_Table.fnDraw();
    });

    



});


function loadCompany() {
    var $company = $('#selAgent');
    $company.empty();
    $.post('/PurchasesNew/LoadCompany', {}, function (result) {
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CompanyCode + ',' + data.CompanyProvinceCode + '">' + data.CompanyName + '/' + data.CompanyCode + '<option>'
            });
            $company.append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $company.append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
};



