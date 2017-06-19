var tmpId = '';//全局变量
$(function () {

    //加载模板树图
    LoadDataTree('');
    loadDrpDGName();
    //加载弹窗中的等级下拉列表
    //loadDrpGrade();
    //下拉列表改变事件
    $('#drpBisGrade').change(function () {
        var dgGrade = $('#drpBisGrade').val();
        if (dgGrade != '') {
            loadDrpDGName(dgGrade);
        } else {
            $('#drpParentDG').html("<option value=''>请选择</option>");
        }
    })
    $(".chzn_a").chosen({
        allow_single_deselect: true
    });
    //点击搜索的时候重新加载树图
    $('#tmpMessageSearch').submit(function (e) {
        e.preventDefault();
        LoadDataTree('');
    })
    //新建
    $('#btnClear').click(function () {

        $("#txtHdnId").val('');
        $("#txtBisName").val('');
        //$("#drpBisGrade").val('');
        $("#drpParentDpt").val('');
        tmpId = '';
        $("#tree_a li").each(function () {
            $(this).children("span").removeClass("dynatree-active");
        })
    })
    //删除
    $('#btnDelete').click(function () {
        if (!tmpId) {
            $.dialog("请选择要删除的业务部门");
            return false;
        }
        $.dialog("确认删除吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax('/BaseData/DeleteBsiDptDataById', { tmpId: tmpId }, function (res) {
                
                if (res.IsPass) {
                    LoadDataTree(curParentNode);
                    $.dialog(res.MSG);
                    $("#txtBisName").val('');
                    loadDrpDGName();
                } else {
                    $.dialog(res.MSG);
                }
            })
        })
    })
    //保存编辑信息
    $("#frmSaveMessage").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            var BisDpt = {
                ID: tmpId,// $("#txtHdnId").val(),
                BisDptName: encode($("#txtBisName").val()),
                //ParentGrade: $("#drpBisGrade").val(),
                ParentBisDpt: $("#drpParentDpt").val(),
            };
            ajax('/BaseData/AddOrUpdateBsiDptData', { bisdpt: BisDpt }, function (res) {
                
                if (res.IsPass) {
                    LoadDataTree(res.Obj[0].toString());
                    $('#btnClear').click();
                    $.dialog(res.MSG);
                    $("#txtBisName").val('');
                    loadDrpDGName();
                } else $.dialog(res.MSG);
            })
        }
    })
})

//验证数据
var DataValidator = $("#frmSaveMessage").validate({
    rules: {
        txtBisName: {
            required: true,
            maxlength: 50,
            isSb: true
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//加载模板树图
function LoadDataTree(snode) {
    var d = { key: encode($("#key").val().toLowerCase()) }, a = '/BaseData/GetBsiDptDataList';
    ajax(a, d, function (data) {
        $('.dynatree').dynatree({
            children: data,
            onClick: function (node) {
                TreeItemClick(node);
                //if (node.parent.data.children) {

                //}
            },
            onActivate: function (node) {
                if (!node.data.isFolder) {
                    curParentNode = node.parent.data.key;
                }
            },
            onPostInit: function (isReloading, isError, dtNode) {
                this.tnRoot.visit(function (dtNode) {
                    if (!utility.isNull(snode) && dtNode.data.key == snode) {
                        if (dtNode.data.isFolder) {
                            dtNode.expand(true);
                        }
                        else {
                            dtNode.focus(true);
                            dtNode.activate(true);
                        }
                    }
                });
            }
        }).dynatree("getTree").reload();
    });
}

//点击树图节点事件
function TreeItemClick(node) {
    var Id = node.data.key.trim();
    if (Id != 0) {
        tmpId = Id;
        ajax('/BaseData/GetBsiDptDataById', { "tmpId": tmpId }, function (res) {

            if (res != null) {
                $("#txtHdnId").val(res.DepartmentID);
                $("#txtBisName").val(res.DepartmentName);
                //$("#drpBisGrade").val(res.DepartmentGrade).change();

                $("#drpParentDpt").val(res.ParentID).trigger("liszt:updated");

            }
        });
    }
}

//加载等级下拉列表
function loadDrpGrade() {
    $('#drpBisGrade').empty();
    ajax('/BaseData/GetBsiDptGrade', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            var j = 0;
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i] + '>' + res[i] + '</option>';
                j = res[i];
            }
            opt += '<option value=' + (j + 1) + '>' + (j + 1) + '</option>';
            $('#drpBisGrade').append(opt);
        } else {
            var opt = "<option value='1'>1</option>";
            $('#drpBisGrade').append(opt);
        }
    });
}

//加载所属群组
function loadDrpDGName() {
    $('#drpParentDpt').empty();
    ajaxSync('/BaseData/GetParentBsiDpt', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].DepartmentID + '>' + res[i].DepartmentName + '</option>';
            }
            $('#drpParentDpt').append(opt);
            $(".chzn_a").trigger("liszt:updated");

        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpParentDpt').append(opt);
        }
    });
}
