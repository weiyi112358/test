var dt_Table;
$(function () {
    //加载数据表格
    dt_Table = $('#dt_Table').dataTable({
        sAjaxSource: '/BatchOperation/LoadOperationList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[0, "desc"]],
        aoColumns: [
           {
               data: 'OddNumber', title: "单号", sClass: "center", sortable: true
           },
           {
               data: 'OperationType', title: "单据类型", sClass: "center", sortable: false, render: function (d) {
                   var msg = "";
                   if(d=="0")
                   {
                       msg = "开卡";
                   }
                   else if (d == "1") {
                       msg = "所属组织转移";
                   }
                   else if (d == "2") {
                       msg = "冻结";
                   }
                   else if(d=="3"){
                       msg = "解冻";
                   }
                   else if (d == "4") {
                       msg = "作废";
                   }
                   else {
                       msg = "";
                   }
                   return msg
               }
           },
            {
                data: 'Status', title: "状态", sClass: "center", sortable: false, render: function (d) {
                    var msg = "";
                    if (d == "1") {
                        msg = "已审核";
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
             
           
            {
                data: 'ModifiedUser', title: "最后修改人", sClass: "center", sortable: false
            },
            {
                data: null, title: "最后修改时间", sClass: "center", sortable: false, render: function (obj) {
                    return obj.ModifiedDate.substr(0, 10);
                }
            },
             {
                 data: null, title: "操作", sClass: "center", sortable: false,
                 render: function (obj) {
                     var action = '';
                     //未审核，可以删除
                     if (obj.Status == 0) {
                         action += '<button class=\"btn\ modifyRule" onclick="Edit(\'' + obj.OperationID + '\',1)"  >编辑</button>';
                         action += '<button class=\"btn btn-danger\ deleteRule" billid=\'' + obj.OperationID + '\'  >删除</button>';
                     }
                     else if (obj.Status == 1) {
                         action += '<button class=\"btn\ modifyRule" onclick="Edit(\'' + obj.OperationID + '\',2)"  >查看</button>';
                     }
                     else if (obj.Status == 2) {
                         action += '<button class=\"btn btn-danger\ deleteRule" billid=\'' + obj.OperationID + '\'  >删除</button>';
                     }
                     return action;
                 }
             }
        ],
        fnFixData: function (d) {
            d.push({ name: 'strmodel', value: getSearchModel() });
        }
    });
    $("#ModifyTimeBegin").datepicker({ dateFormat: "yyyy-MM-dd" });
    $("#ModifyTimeEnd").datepicker({ dateFormat: "yyyy-MM-dd" });
    //重置
    $("#btnRemove").bind("click", function () {
        $("#SearchOddNumber").val(''),
        $("#type").selectIndex = 1,
        $("#Status").selectIndex = 1
    });
    //查询
    $("#btnSerach").bind("click", function () {
        dt_Table.fnDraw();
    });
    //新建
    $("#btnAdd").bind("click", function () {
        $("#hideID").val('');
        $("#hideInfo").val('');
        $("#form1").submit();
    });
   
    //删除
    $('#dt_Table').on('click', '.deleteRule', function () {
        var id = $(this).attr('billid');
        $.dialog("确认删除吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax("/BatchOperation/DeleteOperationById", { Id: id }, showResult);
        })
    });

    //导入按钮
    initImportUpload();
    $("#btnImport").bind("click", function () {
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
                    ctrlUpload.startUpload();
                });
            }
        });
    });

    //模板下载
    $("#btnDownLoad").bind("click", function () {
        window.location = '/Upload/卡批量操作导入模板.xls';
    });

});
//获取查询对象
function getSearchModel() {
    var model = {
        OddNumber: $("#SearchOddNumber").val(),
        OperationType: $("#type").val(),
        Status: $("#Status").val(),
        ModifiedUser: $("#ModifyUser").val(),
        ModifyTimeBegin: $("#ModifyTimeBegin").val(),
        ModifyTimeEnd: $("#ModifyTimeEnd").val()
    };
    return JSON.stringify(model);
}
//修改
function Edit(id, isinfo) {
    $("#hideID").val(id);
    $("#hideInfo").val(isinfo);
    $("#form1").submit();
}
//显示结果
function showResult(res) {
    if (res.IsPass) {
        $.dialog(res.MSG);
        var start = dt_Table.fnSettings()._iDisplayStart;
        var length = dt_Table.fnSettings()._iDisplayLength;
        dt_Table.fnPageChange(start / length, true);
    } else { $.dialog(res.MSG); }
}

//上传
function initImportUpload() {
    ctrlUpload.initUpload("/BatchOperation/ImportExcelBatch", "", uploadBack);
}


//EXCEL上传回调
function uploadBack(data) {
    var ret = data.response;
    showLoading("正在导入中....");
    productArr += ret;
    dt_Table.fnDraw();
    $.colorbox.close();
    $("#tbFilePath").val("");
    hideLoading();
}

//关闭遮罩层
function hideLoading() {
    $.closePopupLayer('processing');
    //$("#processingdiv").hide();
}



