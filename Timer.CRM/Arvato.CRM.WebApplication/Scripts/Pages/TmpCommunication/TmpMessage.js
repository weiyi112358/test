var tmpId = '';//全局变量
var slWildcard = [];
var curParentNode = '';
$(function () {
    //加载模板树图
    LoadDataTree('');
    //加载模板类型
    LoadTmpCatg();
    //加载短消息类型
    LoadTmpBusinessType();
    //加载个性化元素列表
    LoadElementsList();
    //新建
    $('#btnClear').click(function () {
        $("#txtTmpCode").val('');
        $("#txtTmpName").val('');
        //$("#txtTmpSchemaId").val('');
        $("#drpTmpCatg").val('1');
        $("#drpTmpCXBusinessType").val('activity');
        $("#enable").prop("checked", true);
        $("#txtTmpContent").val('');
        $("#txtRemark").val('');
        tmpId = '';
        $("#tree_a li").each(function () {
            $(this).children("span").removeClass("dynatree-active");
        })
    })
    //添加个性化元素到内容中
    $('#slWildcardList').bind("dblclick", function (e) {
        var cur = $("#slWildcardList option:selected").html();
        //var cur = $("#slWildcardList option:selected").val();
        cur = $("#txtTmpContent").val() + cur;
        $("#txtTmpContent").val(cur);
    })
    //删除
    $('#btnDelete').click(function () {
        if (!tmpId) {
            $.dialog("请选择要删除的短信模板");
            return false;
        }
        $.dialog("确认删除吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax('/TmpCommunication/DeleteTmpDataById', { tmpId: tmpId }, function (res) {
                if (res.IsPass) {
                    LoadDataTree(curParentNode);
                    $.dialog(res.MSG);
                    $('#btnClear').click();
                }
            })
        })
    })

    //保存编辑信息
    $("#frmSaveMessage").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            var content = $("#txtTmpContent").val();
            //找到需要替换的
            var regx = /{[\u4e00-\u9fa5]{0,}}/g;
            //var rs = regx.match(content);
            var rs = content.match(regx);
            if (rs != null) {
                for (var i = 0; i < rs.length; i++) {
                    for (var j = 0; j < slWildcard.length; j++) {
                        if (rs[i] == slWildcard[j].fieldDesc) {
                            content = content.replace(rs[i], slWildcard[j].fieldAlias)
                        }
                    }
                }
            }

            var templet = {
                TempletID: $("#txtTmpCode").val(),
                Name: encode($("#txtTmpName").val()),
                Category: $("#drpTmpCatg").val(),
                BusinessType: $("#drpTmpCXBusinessType").val(),
                //SchemaId: encode($("#txtTmpSchemaId").val()),
                Enable: $("#enable").prop("checked"),
                BasicContent: encode(content),
                Remark: encode($("#txtRemark").val()),
            };
            ajax('/TmpCommunication/AddOrUpdateTmpMessageData', templet, function (res) {
                if (res.IsPass) {
                    LoadDataTree(res.Obj[0].toString());
                    $.dialog(res.MSG);
                } else $.dialog(res.MSG);
            })
        }
    })
    //点击搜索的时候重新加载树图
    $('#tmpMessageSearch').submit(function (e) {
        e.preventDefault();
        LoadDataTree('');
    })
    //保存
    //$('#btnSave').click(function () {
    //    var templet = {
    //        TempletID: $("#txtTmpCode").val(''),
    //        Name: $("#txtTmpName").val(),
    //        Category: $("#drpTmpCatg").val(),
    //        Enable: $("#enable").prop("checked"),
    //        BasicContent: $("#txtTmpContent").val(),
    //        Remark: $("#txtRemark").val(),
    //    };
    //    ajax('/TmpCommunication/AddOrUpdateTmpMessageData', templet, function (res) {
    //        $.dialog(res);
    //        LoadDataTree();
    //    })
    //})
})
//验证数据
var DataValidator = $("#frmSaveMessage").validate({
    //onSubmit: false,
    rules: {
        txtTmpName: {
            required: true,
            maxlength: 50,
        },
        txtTmpContent: {
            required: true,
            maxlength: 4000,
        },
        txtRemark: {
            maxlength: 200,
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//加载模板树图
function LoadDataTree(snode) {
    var d = { type: "SMS", key: encode($("#key").val().toLowerCase()) }, a = '/TmpCommunication/GetTmpDataList';
    ajax(a, d, function (data) {
        $('.dynatree').dynatree({
            children: data,
            onClick: function (node) {
                if (node.parent.data.children) {
                    TreeItemClick(node);
                }
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
        ajax('/TmpCommunication/GetTmpDataById', { "tmpId": tmpId }, function (res) {

            if (res != null) {
                $("#txtTmpCode").val(res.TempletID);
                $("#txtTmpName").val(res.Name);
                $("#drpTmpCatg").val(res.Category);
                $("#enable").prop("checked", res.Enable);

                $("#drpTmpCXBusinessType").val(res.BusinessType);
                //$("#txtTmpSchemaId").val(res.SchemaId);
                var content = res.BasicContent;
                //找到需要替换的
                var regx = /{[a-zA-Z]{0,}}/g;
                //var rs = regx.match(content);
                var rs = content.match(regx);
                if (rs != null) {
                    for (var i = 0; i < rs.length; i++) {
                        for (var j = 0; j < slWildcard.length; j++) {
                            if (rs[i] == slWildcard[j].fieldAlias) {
                                content = content.replace(rs[i], slWildcard[j].fieldDesc)
                            }
                        }
                    }
                }
                $("#txtTmpContent").val(content);
                $("#txtRemark").val(res.Remark);
            }
        });
    }
}

//加载模板类型
function LoadTmpCatg() {
    ajax('/TmpCommunication/GetTmpCatg', null, function (res) {
        $('#drpTmpCatg').empty();
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpTmpCatg').append(opt);
        } else {
            var opt = "<option value='1'>-无-</option>";
            $('#drpTmpCatg').append(opt);
        }
    });
}

//加载模板类型
function LoadTmpBusinessType() {
    ajax('/TmpCommunication/GetTmpCXBusinessType', null, function (res) {
        $('#drpTmpCXBusinessType').empty();
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpTmpCXBusinessType').append(opt);
        } else {
            var opt = "<option value='1'>-无-</option>";
            $('#drpTmpCXBusinessType').append(opt);
        }
    });
}

//加载个性化元素列表
function LoadElementsList() {
    ajax('/TmpCommunication/GetElementsList', null, function (res) {
        $('#slWildcardList').empty();
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].FieldAlias + '>{' + res[i].FieldDesc + '}</option>';
                slWildcard.push({ fieldAlias: '{' + res[i].FieldAlias + '}', fieldDesc: '{' + res[i].FieldDesc + '}' });
            }
            $('#slWildcardList').append(opt);
        } else {
            var opt = "";//"<option value='1'>-无-</option>";
            $('#slWildcardList').append(opt);
        }
    });
}