var dataGroupID = '';//全局变量
$(function () {
    $("#txtAddDate").datepicker();

    //加载数据表格
    dt_dataGroup = $('#dt_dataGroup').dataTable({
        sAjaxSource: '/BaseData/GetDataGroup',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'DataGroupID', title: "群组编号", sortable: false },
            { data: 'DataGroupName', title: "群组名称", sortable: false },
            { data: 'DataGroupGrade', title: "群组等级", sortable: true },
            { data: 'FDataGroupName', title: "父群组名称", sortable: false },
            { data: 'AddedDate', title: "创建时间", sortable: true },
        ],
        fnFixData: function (d) {
            d.push({ name: 'groupGrade', value: $("#drpGroupGrade").val() });
            d.push({ name: 'groupName', value: $("#txtGroupName").val() });
            d.push({ name: 'addDate', value: $("#txtAddDate").val() });
        }
    });


    //查询
    $('#btnSearch').click(function () {
        dt_dataGroup.fnDraw();
    })
    //清空筛选信息
    $('#btn_clear').click(function () {
        $('#drpGroupGrade').val('-1');
        $('#txtGroupName').val('');
        $('#txtAddDate').val('');

    })
    //清空弹窗中已输入信息
    $('#btnClear').click(function () {
        $('#txtDGName').val('');
        $('#drpDGGrade').val('');
        $('#drpDGGrade').change();
        $('#drpParentDG').val('');
        $('#drpParentDG').html("<option value=''>请选择</option>");
        dataGroupID = '';
    })

    //删除选中群组信息
    $('#btnDelete').click(function () {
        if (!dataGroupID) {
            $.dialog("请选择要删除的群组项");
            return false;
        }
        $.dialog("确认删除吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax('/BaseData/DeleteDataGroupById', { dataGroupID: dataGroupID }, function (res) {
                if (res.IsPass) {
                    $.dialog(res.MSG);
                    SetDataGroupTree();
                } else { $.dialog(res.MSG); }
            })
        })
    })
    //保存编辑信息
    $("#frmDataGroup").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            var dgName = $('#txtDGName').val();
            var dgGrade = $('#drpDGGrade').val();
            var pgdID = $('#drpParentDG').val();
            ajax('/BaseData/AddOrUpdateDataGroup', { dataGroupName: dgName, dataGroupGrade: dgGrade, pDataGroupID: pgdID, dgId: dataGroupID }, function (res) {
                if (res.IsPass) {
                    $.dialog(res.MSG);
                    SetDataGroupTree();
                    dt_dataGroup.fnDraw();
                    $.colorbox.close();
                } else { $.dialog(res.MSG); }
            })
        }
    })
    //保存群组信息
    //$('#btnSave').click(function () {
    //    var dgName = $('#txtDGName').val();
    //    var dgGrade = $('#drpDGGrade').val();
    //    var pgdID = $('#drpParentDG').val();
    //    ajax('/BaseData/AddOrUpdateDataGroup', { dataGroupName: dgName, dataGroupGrade: dgGrade, pDataGroupID: pgdID, dgId: dataGroupID }, function (res) {
    //        $.dialog(res);
    //        SetDataGroupTree();
    //    })
    //})
    //加载弹窗中的等级下拉列表
    loadDrpGrade();
    //加载搜索中的等级下拉列表
    loadDrpDgGrade();
    //下拉列表改变事件
    $('#drpDGGrade').change(function () {
        var dgGrade = $('#drpDGGrade').val();
        if (dgGrade != '') {
            loadDrpDGName(dgGrade);
        } else {
            $('#drpParentDG').html("<option value=''>请选择</option>");
        }
    })
});
//验证数据
var DataValidator = $("#frmDataGroup").validate({
    //onSubmit: false,
    rules: {
        txtDGName: {
            required: true,
        },
        drpDGGrade: {
            required: true,
        },
        drpParentDG: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//新建按钮点击事件
function goEdit() {
    $('#txtDGName').val('');
    $('#drpDGGrade').val('').removeClass('error-block');
    $('#drpDGGrade').change();
    $('#drpParentDG').val('').removeClass('error-block');

    $('.error-block').html('');
    SetDataGroupTree();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editData",
        inline: true
    });
    $.colorbox.resize();
}

//加载数据群组数据
function SetDataGroupTree() {
    var d = {}, a = '/BaseData/GetDataGroupList';
    ajax(a, d, function (data) {
        $('.dynatree').dynatree({
            children: data,
            onClick: function (node) {
                if (node.parent.data.children) {
                    DataGroupItemClick(node);
                }
            },
            //strings: {
            //    loading: "数据加载中...",
            //    loadError: "数据加载错误!"
            //}
        }).dynatree("getTree").reload();
    });
}

//点击树图节点事件
function DataGroupItemClick(node) {
    var dgId = node.data.key.trim();
    if (dgId != 0) {
        dataGroupID = dgId;
        ajax('/BaseData/GetDataGroupById', { "dataGroupId": dgId }, function (res) {
            //alert(res);
            if (res != null) {
                $('#txtDGName').val(res.DataGroupName);
                $('#drpDGGrade').val(res.DataGroupGrade);
                $('#drpDGGrade').change();
                $('#drpParentDG').val(res.ParentDataGroupID);
            }
        });
    }
}

//加载群组等级下拉列表
function loadDrpGrade() {
    ajax('/BaseData/GetDataGroupGrade', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i] + '>' + res[i] + '</option>';
            }
            $('#drpDGGrade').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpDGGrade').append(opt);
        }
    });
}
//加载群组等级下拉列表
function loadDrpDgGrade() {
    $('#drpGroupGrade').empty();
    ajax('/BaseData/GetDataGroupGrade', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>全部</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i] + '>' + res[i] + '</option>';
            }
            $('#drpGroupGrade').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpGroupGrade').append(opt);
        }
    });
}
//加载所属群组
function loadDrpDGName(dgGrade) {
    $('#drpParentDG').empty();
    ajaxSync('/BaseData/GetParentDataGroup', { "dataGroupGrade": dgGrade }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].DataGroupID + '>' + res[i].DataGroupName + '</option>';
            }
            $('#drpParentDG').append(opt);
            
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpParentDG').append(opt);
        }
    });
}