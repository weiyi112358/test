$(function () {
    //加载数据表格
    dt_CorpTable = $('#dt_CorpTable').dataTable({
        sAjaxSource: '/BaseData/GetCorpData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: true },
            { data: 'CorpName', title: "企业名称", sortable: false },
            { data: 'CorpContract', title: "企业联系人", sortable: false },
            { data: 'CorpPhoneNo', title: "联系电话", sortable: false },
            { data: 'CorpAddress', title: "企业地址", sortable: false },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'corpName', value: $("#txt_CorpName").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_CorpTable.fnDraw();
    })

    //保存数据
    $("#frmSaveCorp").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var corp = {
                BaseDataID: $("#txtCorpId").val(),
                DataGroupID: $("#txtDataGroupId").val(),
                BaseDataType: $("#txtDataType").val(),
                CorpName: encode($("#txtCorpName").val()),
                CorpContract: $("#txtContract").val(),
                CorpPhoneNo: $("#txtPhone").val(),
                CorpAddress: encode($("#txtAddress").val()),
            }
            //增加
            if (corp.BaseDataID == '') {
                ajax("/BaseData/AddCorpData", { model: corp }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_CorpTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else {//编辑
                ajax("/BaseData/UpdateCorpData", { model: corp }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();

                        var start = dt_CorpTable.fnSettings()._iDisplayStart;
                        var length = dt_CorpTable.fnSettings()._iDisplayLength;
                        dt_CorpTable.fnPageChange(start / length, true);
                        //dt_CorpTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    })
})
//验证数据
var DataValidator = $("#frmSaveCorp").validate({
    //onSubmit: false,
    rules: {
        txtCorpName: {
            required: true,
            maxlength: 20,
        },
        txtContract: {
            required: true,
            maxlength: 20,
            isOnlyLC: true,
        },
        txtPhone: {
            required: true,
            maxlength: 11,
            isMobileNo:true,
        },
        txtAddress: {
            required: true,
            maxlength: 100,
            isOnlyLNC:true
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//弹窗
function goEdit() {
    $("#table_Corp .heading h3").html("企业新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#table_Corp",
        inline: true
    });
    $.colorbox.resize();
}
//编辑条目信息
function edit(id) {

    $("#table_Corp .heading h3").html("企业编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetCorpById", { corpId: id }, function (res) {
        $("#txtCorpId").val(res.BaseDataID);
        $("#txtCorpName").val(res.CorpName);
        $("#txtPhone").val(res.CorpPhoneNo);
        $("#txtContract").val(res.CorpContract);
        $("#txtAddress").val(res.CorpAddress);

        $("#txtDataGroupId").val(res.DataGroupID);
        $("#txtDataType").val(res.BaseDataType);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_Corp",
        inline: true
    });
    $.colorbox.resize();
}
//删除条目
function deleteItem(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteCorpById", { corpId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_CorpTable.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}
//清空数据
function goClear() {
    $("#txtCorpId").val('');
    $("#txtCorpName").val('');
    $("#txtPhone").val('');
    $("#txtContract").val('');
    $("#txtAddress").val('');

    $("#txtDataGroupId").val('');
    $("#txtDataType").val('');

    $('.error-block').html('');
}

