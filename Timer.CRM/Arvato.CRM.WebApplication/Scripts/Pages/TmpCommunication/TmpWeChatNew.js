var tmpId = '';//全局变量
$(function () {
    //加载模板树图
    loadDataTree();
    //加载模板类型
    LoadTmpCatg();
    //加载微信类型
    LoadWeChatCatg();
    //加载个性化元素列表
    LoadElementsList();
    $("#drpContentType").change(function () {
        if ($("#drpContentType").val() == "3") {
            $("#txtMaterial").val("");
            $("#txtMaterial").attr("disabled", "disabled");
        }
        else {
            $("#txtMaterial").removeAttr("disabled");
        }
    });
    //新建
    $('#btnClear').click(function () {
        $("#txtTmpCode").val('');
        $("#txtTmpName").val('');
        $("#txtTitle").val('');
        $("#drpTmpCatg").val('1');
        $("#enable").prop("checked", true);
        $("#txtMaterial").val('');//素材
        $("#txtRemark").val('');
        tmpId = '';
    });
    //添加个性化元素到内容中
    $('#slWildcardList').bind("dblclick", function (e) {
        var cur = $("#slWildcardList option:selected").html();
        cur = $("#txtTmpContent").val() + cur;
        $("#txtTmpContent").val(cur);
    });
    //删除
    $('#btnDelete').click(function () {
        if (!tmpId) {
            $.dialog("请选择要删除的短信模板");
            return false;
        }
        ajax('/TmpCommunication/DeleteTmpDataById', { tmpId: tmpId }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                loadDataTree();
            }
        })
    });
    //保存
    $('#btnSave').click(function () {
        var templet = {};
        templet = {
            TempletID: $("#txtTmpCode").val(),
            Name: encode($("#txtTmpName").val()),
            SubType: $("#drpContentType").val(),
            Category: $("#drpTmpCatg").val(),
            Enable: $("#enable").prop("checked"),
            BasicContent: encode($("#txtMaterial").val()),
            Remark: encode($("#txtRemark").val()),
        };
        if (templet.Name.length == 0) {
            $.dialog("模板名称不能为空！");
            return;
        }
        if (templet.SubType != 3) {
            if (templet.BasicContent.length == 0) {
                $.dialog("素材编号不能为空！");
                return;
            }
        }
        else {
            templet.BasicContent = "";
        }

        if (templet.Name.length >= 50) {
            $.dialog("标题字数超过限制！");
            return;
        }
        if (templet.Remark.length >= 200) {
            $.dialog("备注字数超过限制！");
            return;
        }
        ajax('/TmpCommunication/AddOrUpdateTmpWeChatData', templet, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                loadDataTree();
            } else $.dialog(res.MSG);
        })
    });

    //点击搜索的时候重新加载树图
    $('#tmpWechatSearch').submit(function (e) {
        e.preventDefault();
        loadDataTree();
    });
});

//加载模板树图
function loadDataTree() {
    var d = {
        type: 'WeChat',
        key: encode($("#key").val().toLowerCase())
    },
        a = "/TmpCommunication/GetTmpDataList";
    ajax(a, d, function (data) {
        $('.dynatree').dynatree({
            children: data,
            onClick: function (node) {
                if (node.parent.data.children) {
                    TreeItemClick(node);
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
function TreeItemClick(node) {
    var Id = node.data.key.trim();
    if (Id != 0) {
        tmpId = Id;
        ajax('/TmpCommunication/GetTmpDataById', { "tmpId": tmpId }, function (res) {

            if (res != null) {
                $("#txtTmpCode").val(res.TempletID);
                $("#txtTmpName").val(res.Name);
                $("#txtTitle").val(res.Topic);
                $("#drpTmpCatg").val(res.Category);
                $("#enable").prop("checked", res.Enable);
                $("#drpContentType").find("option[value='" + res.SubType + "']").attr("selected", true);
                $("#drpContentType").change();
                $("#txtMaterial").val(res.BasicContent);
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
//加载微信类型
function LoadWeChatCatg() {
    ajax('/TmpCommunication/GetWeChatCatg', null, function (res) {
        $('#drpContentType').empty();
        if (res.length > 0) {
            var opt = "";//"<option value='-1'>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpContentType').append(opt);
        } else {
            var opt = "<option value='1'>-无-</option>";
            $('#drpContentType').append(opt);
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
                opt += '<option>{' + res[i].FieldDesc + '}</option>';
            }
            $('#slWildcardList').append(opt);
        } else {
            var opt = "";//"<option value='1'>-无-</option>";
            $('#slWildcardList').append(opt);
        }
    });
}


