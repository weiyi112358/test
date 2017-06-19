$(function () {
    LoadStoreType();
    //加载数据表格
    dt_ChannelTable = $('#dt_ChannelTable').dataTable({
        sAjaxSource: '/BaseData/GetChannelData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aaSorting: [[0, "asc"]],
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: true },
            { data: 'ChannelNameBase', title: "渠道名称", sortable: false },
            { data: 'ChannelCodeBase', title: "渠道代码", sortable: false },
            { data: 'SubDataGroupName', title: "所属群组", sortable: false },
            //{ data: 'ChannelIsEnableBase', title: "是否可用", sortable: false,
            //    render: function (obj) {
            //        if (obj == "1") {
            //            return "已启用";
            //        } else {
            //            return "已禁用";
            //        }
            //    }
            //},
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    var strbtn = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-danger\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";//
                    //if (obj.ChannelIsEnableBase == "1") {
                    //    strbtn += "<button class=\"btn btn-danger\" id=\"btnEnable\" onclick=\"EnableItem(" + obj.BaseDataID + "," + obj.ChannelIsEnableBase + ")\">禁用</button>";
                    //} else {
                    //    strbtn += "<button class=\"btn btn-info\" id=\"btnEnable\" onclick=\"EnableItem(" + obj.BaseDataID + "," + obj.ChannelIsEnableBase + ")\">启用</button>";
                    //}
                    return strbtn;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'channelName', value: $.trim($("#txt_ChannelName").val()) });
            d.push({ name: 'groupId', value: $("#drpChannelClass").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_ChannelTable.fnDraw();
    });

    //保存数据
    $("#frmAddChannel").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            var channel = {
                BaseDataID: $("#txtChannelId").val(),
                DataGroupID: $("#editChannelClass").val(),
                BaseDataType: $("#txtDataType").val(),
                ChannelName: encode($("#txtChannelName").val()),
                ChannelCode: $("#txtChannelCode").val()
                //,
                //ChannelIsEnableBase: $("#txtIsEnable").val()
            };
            //增加
            if (channel.BaseDataID == '') {
                ajax("/BaseData/AddChannelData", { model: channel }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_ChannelTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else { //编辑
                ajax("/BaseData/UpdateChannelData", { model: channel }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();

                        var start = dt_ChannelTable.fnSettings()._iDisplayStart;
                        var length = dt_ChannelTable.fnSettings()._iDisplayLength;
                        dt_ChannelTable.fnPageChange(start / length, true);
                        //dt_ChannelTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    });
});

//验证数据
var DataValidator = $("#frmAddChannel").validate({
    //onSubmit: false,
    rules: {
        txtChannelName: {
            required: true,
            maxlength: 20,
        },
        txtChannelCode: {
            isOnlyLN: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//弹窗
function goEdit() {
    $("#addChannel_dialog .heading h3").html("渠道新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addChannel_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addChannel_dialog .heading h3").html("渠道编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetChannelById", { channelId: id }, function (res) {
        $("#txtChannelId").val(res.BaseDataID);
        $("#txtChannelName").val(res.ChannelNameBase);
        $("#txtChannelCode").val(res.ChannelCodeBase);
        $("#editChannelClass").val(res.DataGroupID);
        //  $("#txtDataGroupId").val(res.DataGroupID);
        $("#txtDataType").val(res.BaseDataType);
        //$("#txtIsEnable").val(res.ChannelIsEnableBase);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addChannel_dialog",
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
        ajax("/BaseData/DeleteChannelById", { channelId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_ChannelTable.fnDraw();
            } else {
                $.dialog(res.MSG);
            }
        });
    });
}

//修改可用状态
function EnableItem(id, isEnable) {
    var tip = "";
    if (isEnable == "1") {
        tip = "确认禁用吗?";
        isEnable = "0";
    }
    else {
        tip = "确认启用吗?";
        isEnable = "1";
    }
    $.dialog(tip, {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/UpdateChannelEnableById", { channelId: id, isEnable: isEnable }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_ChannelTable.fnDraw();
            } else {
                $.dialog(res.MSG);
            }
        });
    });
}


//清空数据
function goClear() {
    $("#txtChannelId").val('');
    $("#txtChannelName").val('');
    $("#txtChannelCode").val('');
    $("#editChannelClass").val('');
    //  $("#txtDataGroupId").val('');
    $("#txtDataType").val('');
    //$("#txtIsEnable").val('');
    $('.error-block').html('');
}

//加载所属群组
function LoadStoreType() {
    //$('#addItemClass').empty();
    ajax('/BaseData/GetStroeGroupList', null, function (res) {
        if (res.length > 0) {
            var opt = "";//<option value=''>请选择</option>
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].SubDataGroupID + '>' + res[i].SubDataGroupName + '</option>';
            }
            $('#drpChannelClass').append(opt);
            $("#editChannelClass").append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpChannelClass').append(opt);
            $("#editChannelClass").append(opt);
        }
    });
}