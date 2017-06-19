var tmpId = '';//全局变量
var slWildcard = [];
var curParentNode = '';
$(function () {
    //加载模板树图
    LoadDataTree('');
    //加载模板类型
    LoadTmpCatg();
    //加载个性化元素列表
    LoadElementsList();
    //初始化文本编辑器
    initTinymce();
    //新建
    $('#btnClear').click(function () {
        $("#txtTmpCode").val('');
        $("#txtTmpName").val('');
        $("#txtTitle").val('');
        $("#drpTmpCatg").val('1');
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
        //var cur1 = $("#slWildcardList option:selected").val();
        cur = $("#txtTmpContent").val() + cur;//中文
        //cur1 = $("#txtTmpContent").val() + '{' + cur1 + '}';//英文
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
                    $.dialog(res.MSG);
                    LoadDataTree(curParentNode);
                    $('#btnClear').click();
                }
            })
        })
    })
    //保存编辑信息
    $("#frmSaveMail").submit(function (e) {
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
                Topic: encode($("#txtTitle").val()),
                Category: $("#drpTmpCatg").val(),
                Enable: $("#enable").prop("checked"),
                BasicContent: encode(content),
                Remark: encode($("#txtRemark").val()),
            };
            ajax('/TmpCommunication/AddOrUpdateTmpMailData', templet, function (res) {
                if (res.IsPass) {
                    LoadDataTree(res.Obj[0].toString());
                    $.dialog(res.MSG);
                } else $.dialog(res.MSG);

            })
        }
    })
    //点击搜索的时候重新加载树图
    $('#tmpMailSearch').submit(function (e) {
        e.preventDefault();
        LoadDataTree('');
    })
    //保存
    //$('#btnSave').click(function () {
    //    var templet = {
    //        TempletID: $("#txtTmpCode").val(''),
    //        Name: $("#txtTmpName").val(),
    //        Topic: $("#txtTitle").val(),
    //        Category: $("#drpTmpCatg").val(),
    //        Enable: $("#enable").prop("checked"),
    //        BasicContent: $("#txtTmpContent").val(),
    //        Remark: $("#txtRemark").val(),
    //    };
    //    ajax('/TmpCommunication/AddOrUpdateTmpMailData', templet, function (res) {
    //        $.dialog(res);
    //        LoadDataTree();
    //    })
    //})
})
//验证数据
var DataValidator = $("#frmSaveMail").validate({
    //onSubmit: false,
    rules: {
        txtTmpName: {
            required: true,
            maxlength: 50,
        },
        txtTitle: {
            required: true,
            maxlength: 50,
        },
        txtTmpContent: {
            required: true,
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
    var d = { type: "Mail", key: encode($("#key").val().toLowerCase()) }, a = '/TmpCommunication/GetTmpDataList';
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
                $("#txtTitle").val(res.Topic);
                $("#drpTmpCatg").val(res.Category);
                $("#enable").prop("checked", res.Enable);

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

//初始化文本编辑器
function initTinymce() {
    $("#txtTmpContent").tinymce({
        script_url: '/Gebo/lib/tiny_mce/tiny_mce.js',        // Location of TinyMCE script
        theme: "advanced",                                 // General options
        //language: "zh_CN",
        plugins: "autoresize,style,table,advhr,advimage,advlink,emotions,inlinepopups,preview,media,contextmenu,paste,fullscreen,noneditable,xhtmlxtras,template,advlist",
        theme_advanced_buttons1: "undo,redo,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,fontselect,fontsizeselect",// Theme options
        theme_advanced_buttons2: "forecolor,backcolor,|,cut,copy,paste,pastetext,|,bullist,numlist,link,|,code,preview,fullscreen",
        theme_advanced_buttons3: "",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: false,
        font_size_style_values: "8pt,10px,12pt,14pt,18pt,24pt,36pt",
        init_instance_callback: function () {
            function resizeWidth() {
                document.getElementById(tinyMCE.activeEditor.id + '_tbl').style.width = '100%';
            }
            resizeWidth();
            $(window).resize(function () {
                resizeWidth();
            })
        },
        file_browser_callback: function openKCFinder(field_name, url, type, win) {       // file browser
            tinyMCE.activeEditor.windowManager.open({
                title: 'KCFinder',
                width: 500,
                height: 300,
                resizable: "yes",
                inline: true,
                close_previous: "no",
                popup_css: false
            }, {
                window: win,
                input: field_name
            });
            return false;
        }
    });
}