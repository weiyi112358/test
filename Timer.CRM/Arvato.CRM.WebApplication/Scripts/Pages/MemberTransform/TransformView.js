var array = new Array();
var dt_Table;
var identity = 0;
var destineNum = 0;
var reg12 = new RegExp("/\b\d{12}\b/");
var reg = new RegExp("^[0-9]*$");
$(document).ready(function () {
    dt_Table = $('#MemTable').dataTable({
        sAjaxSource: '/MemberTransform/GetMemList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        order: [[1, "desc"]],
        aoColumns: [
            {
                data: null, title: "操作",width:80,sortable: false, render: function (r) {
                    if ($("#ckVal").val().indexOf(r.MemberID) > -1) {
                        var str = '<input type="checkbox" name="txtCK" value="' + r.MemberID + '" onclick="checkone(this)" checked="checked"/>';
                    } else {
                        var str = '<input type="checkbox" name="txtCK" value="' + r.MemberID + '" onclick="checkone(this)" />';
                    }
                return str;
            }},
        { data: 'MemberCardNo', title: "会员卡号", sortable: true, },
        { data: 'CustomerName', title: "会员姓名", sortable: false, },
        { data: 'CustomerMobile', title: "手机号", sortable: false, },
        { data: 'StoreName', title: "门店", sortable: false, },
        { data: 'CompanyName', title: "分公司", sortable: false, },
        ],
        fnFixData: function (d) {
            d.push({ name: 'Corp', value: $("#searchCompany").val() });
            d.push({ name: 'RegisterStoreCode', value: $("#searchStore").val() });
        }
    });
    function initImportUpload() {
        ctrlUpload.initUpload("/MemberTransform/ImportMemExcel","", uploadBack);
    }
    //EXCEL上传回调
    function uploadBack(data) {
        var ret = data.response;
        var newCompany = $('#newCompany2').val();
        var newStore = $('#newStore2').val();

        if (newCompany == "" || newCompany == null) {
            $.dialog("请选择分公司");
            return false;
        }
        if (newCompany == "" || newCompany == null) {
            $.dialog("请选择门店");
            return false;
        }
        showLoading("正在转移")
        $.post('/MemberTransform/SaveTransform', {
            Corp: newCompany,
            RegisterStoreCode: newStore,
            MemId:ret
        }, function (result) {
            if (result) {
                hideLoading()
                $.dialog("操作成功");
                $.colorbox.close();
                dt_Table.fnDraw();
            }
            else {
                hideLoading()
                $.dialog("操作失败");
            }
        }, 'json')
        $("#tbFilePath").val("");
        
    }


    initImportUpload();
    //导入按钮
    $("#btnImport").click(function () {
        //initImportUpload();
        $("#tbFilePath").val("");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#import_data",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
            onComplete: function () {
                $("#btnSaveImport").bind("click", function () {
                    var newCompany = $('#newCompany2').val();
                    var newStore = $('#newStore2').val();

                    if (newCompany == "" || newCompany == null) {
                        $.dialog("请选择分公司");
                        return false;
                    }
                    if (newCompany == "" || newCompany == null) {
                        $.dialog("请选择门店");
                        return false;
                    }
                    ctrlUpload.startUpload();
                });
            }
        });
    });

        //模板下载
     $("#btnDownTemplate").click(function () {
           window.location = '/Upload/会员卡转移导入模板.xls';
     });

    //查询
    $('#search').click(function () {
        $('#ckVal').val('');
        $("#allVal").val('');
        dt_Table.fnDraw();
    });
    //全选
    $("#ckALL").click(function () {
        $("[name=txtCK]:checkbox").prop("checked", true);
        $("[name=txtCK]:checkbox").each(function (a,i)
        {
                $('#ckVal').val($('#ckVal').val().replace(i.value + ",", ''));
                $('#ckVal').val($('#ckVal').val() + i.value + ",");
                
          
        });
    });
    //全不选
    $("#uckALL").click(function () {
        $("[name=txtCK]:checkbox").prop("checked", false);
        $("[name=txtCK]:checkbox").each(function (a, i) {
            $('#ckVal').val($('#ckVal').val().replace(i.value + ",", ''));
        });
    });
    //全都选
    $("#AckALL").click(function () {
        $.ajax({
            type: 'post',
            url: '/MemberTransform/GetMemListID',
            dataType: 'json',
            data: { Corp: $("#searchCompany").val(), RegisterStoreCode: $("#searchStore").val() },
            success: function (result) {
                if (result) {
                    $("[name=txtCK]:checkbox").prop("checked", true);
                    $('#ckVal').val(result.MemberIDs);
                   
                } else {
                    $.dialog("全都选失败");
                }
            },
            error: function (e) {
                e.responseText;
            }
        })
        
    });
    //加载公司下拉框
    $.ajax({
        type: 'post',
        url: '/Purchases/LoadCompany',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.data.length > 0) {
                var opt = "";
                opt += "<option value=''>请选择</option>";
                for (var i = 0; i < result.data.length; i++) {
                    opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '</option>';
                }
                $('#searchCompany').append(opt);
                $('#newCompany').append(opt);
                $('#newCompany2').append(opt);
            }
            else {
                opt = "<option value=''>请选择</option>";
                $('#searchCompany').append(opt);
                $('#newCompany').append(opt);
                $('#newCompany2').append(opt);
            }
        },
        error: function (e) {
            e.responseText;
        }
    })
    //查询条件中的分公司
    $('#searchCompany').change(function ()
    {
        $('#searchStore').html("");
        $.ajax({
            type: 'post',
            url: '/MemberTransform/GetStoreList',
            dataType: 'json',
            data: {company:$('#searchCompany').val()},
            success: function (result) {
                if (result.length > 0) {
                    var opt = "";
                    opt += "<option value=''>请选择</option>";
                    for (var i = 0; i < result.length; i++) {
                        opt += '<option value=' + result[i].StoreCode + '>' + result[i].StoreName + '</option>';
                    }
                    $('#searchStore').append(opt);
                }
                else {
                    opt = "<option value=''>请选择</option>";
                    $('#searchStore').append(opt);
                }
            },
            error: function (e) {
                e.responseText;
            }
        })
    });
    //选择转移的分公司
    $('#newCompany').change(function () {
        $('#newStore').html("");
        $.ajax({
            type: 'post',
            url: '/MemberTransform/GetStoreList',
            dataType: 'json',
            data: { company: $('#newCompany').val() },
            success: function (result) {
                if (result.length > 0) {
                    var opt = "";
                    opt += "<option value=''>请选择</option>";
                    for (var i = 0; i < result.length; i++) {
                        opt += '<option value=' + result[i].StoreCode + '>' + result[i].StoreName + '</option>';
                    }
                    $('#newStore').append(opt);
                }
                else {
                    opt = "<option value=''>请选择</option>";
                    $('#newStore').append(opt);
                }
            },
            error: function (e) {
                e.responseText;
            }
        })
    });
    //导入的分公司
    $('#newCompany2').change(function () {
        $('#newStore2').html("");
        $.ajax({
            type: 'post',
            url: '/MemberTransform/GetStoreList',
            dataType: 'json',
            data: { company: $('#newCompany2').val() },
            success: function (result) {
                if (result.length > 0) {
                    var opt = "";
                    opt += "<option value=''>请选择</option>";
                    for (var i = 0; i < result.length; i++) {
                        opt += '<option value=' + result[i].StoreCode + '>' + result[i].StoreName + '</option>';
                    }
                    $('#newStore2').append(opt);
                }
                else {
                    opt = "<option value=''>请选择</option>";
                    $('#newStore2').append(opt);
                }
            },
            error: function (e) {
                e.responseText;
            }
        })
    });

    //批量转移按钮
    $('#btnTransform').click(function () {
        if ($("#ckVal").val()==null||$("#ckVal").val()=="") {
            $.dialog("请勾选会员");
            return false;
        }
        $("#addBrand_dialog .heading h3").html("批量转移");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            overlayClose: false,
            opacity: '0.3',
            href: "#addBrand_dialog",
            inline: true
        });
        $.colorbox.resize();
    });
 


    //保存
    $('#btnSave').click(function () {
        var newCompany = $('#newCompany').val();
        var newStore = $('#newStore').val();

        if (newCompany == "" || newCompany == null) {
            $.dialog("请选择分公司");
            return false;
        }
        if (newCompany == "" || newCompany == null) {
            $.dialog("请选择门店");
            return false;
        }
        showLoading("正在转移")
        $.post('/MemberTransform/SaveTransform', {
            Corp: newCompany,
            RegisterStoreCode: newStore,
            MemId:$("#ckVal").val()
        }, function (result) {
            if (result) {
                hideLoading()
                $.dialog("操作成功");
                $.colorbox.close();
                dt_Table.fnDraw();
            }
            else {
                hideLoading()
                $.dialog("操作失败");
            }
        }, 'json')
    });

   


   

    })
    

function checkone(a) {
    //var flag = true;
    //$("[name=txtCK]:checkbox").each(function (a,i) {
    //    if (i.checked == false) {
    //        flag = false;
    //    }
    //});
    //$("#ckALL").prop("checked", flag);
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