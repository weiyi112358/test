$(function() {
    LoadStoreType();
    //加载数据表格
    dt_AreaTable = $('#dt_AreaTable').dataTable({
        sAjaxSource: '/BaseData/GetAreaData',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: true },
            { data: 'AreaNameBase', title: "区域名称", sortable: false },
            { data: 'SubDataGroupName', title: "所属群组", sortable: false },
            { data: 'AreaCodeBase', title: "区域代码", sortable: false },
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button>";
                    return htm;
                }
            }
        ],
        fnFixData: function(d) {
            d.push({ name: 'areaNameBase', value: $.trim($("#txt_AreaNameBase").val()) });
            d.push({ name: 'groupId', value: $("#drpStoreClass").val() });
        }
    });
    //搜索
    $("#btnSerach").click(function() {
        dt_AreaTable.fnDraw();
    });

    //保存数据
    $("#frmAddArea").submit(function(e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var Area = {
                BaseDataID: $("#txtAreaId").val(),
                //DataGroupID: $("#txtDataGroupId").val(),
                DataGroupID: $("#editStoreClass").val(),
                BaseDataType: $("#txtDataType").val(),
                AreaNameBase: encode($("#txtAreaNameBase").val()),
                AreaCodeBase: $("#txtAreaCodeBase").val()
            }
            //增加
            if (Area.BaseDataID == '') {
                ajax("/BaseData/AddAreaData", { model: Area }, function(res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_AreaTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            } else { //编辑
                ajax("/BaseData/UpdateAreaData", { model: Area }, function(res) {
                    if (res.IsPass) {
                        $.colorbox.close();

                        var start = dt_AreaTable.fnSettings()._iDisplayStart;
                        var length = dt_AreaTable.fnSettings()._iDisplayLength;
                        dt_AreaTable.fnPageChange(start / length, true);
                        //dt_AreaTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    });
});

//验证数据
var DataValidator = $("#frmAddArea").validate({
    //onSubmit: false,
    rules: {
        txtAreaNameBase: {
            required: true,
            maxlength: 20,
        },
        txtAreaCodeBase: {
            required: true,
            maxlength: 20,
            isOnlyLN: true,
        },
        editStoreClass: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//弹窗
function goEdit() {
    $("#addArea_dialog .heading h3").html("区域新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addArea_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addArea_dialog .heading h3").html("区域编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetAreaById", { areaId: id }, function (res) {
        $("#txtAreaId").val(res.BaseDataID);
        $("#txtAreaNameBase").val(res.AreaNameBase);
        $("#txtAreaCodeBase").val(res.AreaCodeBase);
        $("#editStoreClass").val(res.DataGroupID);
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
        href: "#addArea_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//清空数据
function goClear() {
    $("#txtAreaId").val('');
    $("#txtAreaNameBase").val('');
    $("#txtAreaCodeBase").val('');
    $("#editStoreClass").val('');
    $("#txtDataGroupId").val('');
    $("#txtDataType").val('');

    $('.error-block').html('');
}

//加载门店所属群组
function LoadStoreType() {
    //$('#addItemClass').empty();
    ajax('/BaseData/GetStroeGroupList', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].SubDataGroupID + '>' + res[i].SubDataGroupName + '</option>';
            }
            $('#drpStoreClass').append(opt);
            $('#addStoreClass').append(opt);
            $("#editStoreClass").append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpStoreClass').append(opt);
            $('#addStoreClass').append(opt);
            $("#editStoreClass").append(opt);
        }
    });
}