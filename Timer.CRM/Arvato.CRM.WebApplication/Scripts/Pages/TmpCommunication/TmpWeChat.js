var tmpId = '';//全局变量
$(function () {
    //加载模板树图
    LoadDataTree();
    //加载模板类型
    LoadTmpCatg();
    //加载个性化元素列表
    LoadElementsList();

    //选择的是文本还是图片
    $("#drpContentType").bind("change", function () {
        if ($(this).val() == "image") {
            $("#div_txtContent").hide();
            $("#div_picContent").show();

        }
        else if ($(this).val() == "text") {
            $("#div_txtContent").show();
            $("#div_picContent").hide();
        }
    })

    //上传图片
    $("#btnUpFile").click(function () {
        UploadFile();
    })

    ImageUpload.initUpload("/TmpCommunication/UpLoadFileImg", "jpg,png,gif", uploadBack);  //uploadBack回调函数

    //新建
    $('#btnClear').click(function () {
        $("#txtTmpCode").val('');
        $("#txtTmpName").val('');
        $("#txtTitle").val('');
        $("#txtLink").val('');
        $("#drpTmpCatg").val('1');
        $("#enable").prop("checked", true);
        $("#txtTmpContent").val('');
        $("#txtRemark").val('');
        tmpId = '';
    })
    //添加个性化元素到内容中
    $('#slWildcardList').bind("dblclick", function (e) {
        var cur = $("#slWildcardList option:selected").html();
        cur = $("#txtTmpContent").val() + cur;
        $("#txtTmpContent").val(cur);
    })
    //删除
    $('#btnDelete').click(function () {
        if (!tmpId) {
            $.dialog("请选择要删除的短信模板");
            return false;
        }
        ajax('/TmpCommunication/DeleteTmpDataById', { tmpId: tmpId }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                LoadDataTree();
            }
        })
    })
    //保存
    $('#btnSave').click(function () {
        var a = $("#drpContentType").val();
        var templet = {};
        if (a == 'image') {
            var ctn = {
                Link: $("#txtLink").val(),
                Img: $("#materialImg").attr("src"),
                Desc: $("#txtDesc").val(),
            };
            templet = {
                TempletID: $("#txtTmpCode").val(),
                Name: encode($("#txtTmpName").val()),
                SubType: $("#drpContentType").val(),
                Category: $("#drpTmpCatg").val(),
                Topic: $("#txtTitle").val(),
                Enable: $("#enable").prop("checked"),
                BasicContent: encode(JSON.stringify(ctn)),
                Remark: encode($("#txtRemark").val()),
            };
        }
        else if (a == 'text') {
            templet = {
                TempletID: $("#txtTmpCode").val(''),
                Name: encode($("#txtTmpName").val()),
                SubType: $("#drpContentType").val(),
                Category: $("#drpTmpCatg").val(),
                Enable: $("#enable").prop("checked"),
                BasicContent: encode($("#txtTmpContent").val()),
                Remark: encode($("#txtRemark").val()),
            };
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
                LoadDataTree();
            } else $.dialog(res.MSG);
        })
    })
})

//加载模板树图
function LoadDataTree() {
    var d = { type: "WeChat" }, a = '/TmpCommunication/GetTmpDataList';
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
                if (res.SubType == 'image') {
                    $("#txtLink").val(JSON.parse(res.BasicContent).Link);
                    $("#txtDesc").val(JSON.parse(res.BasicContent).Desc);
                    $("#materialImg").attr("src", JSON.parse(res.BasicContent).Img);
                }
                else {
                    $("#txtTmpContent").val(res.BasicContent);
                }
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
                opt += '<option>{' + res[i].FieldDesc + '}</option>';
            }
            $('#slWildcardList').append(opt);
        } else {
            var opt = "";//"<option value='1'>-无-</option>";
            $('#slWildcardList').append(opt);
        }
    });
}

function UploadFile() {
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#import_data",
        overlayClose: false,
        inline: true,
        opacity: '0.3',
        onComplete: function () {
            $("#btnSaveImport").bind("click", function () {
                ImageUpload.startUpload();
            });
        }
    });
}

function uploadBack(data) {
    var title = $("#tbImportName").val();
    var imgUrl = data.response;
    $("#materialImg").attr("src", imgUrl);

    $("#tbFilePath").val("");
    $("#tbImportName").val("");
    $.colorbox.close();
}

// 上传控件
// uploadUrl: 后台处理方法路径，  fileType：文件类型,"xls,xlsx", "jpg,gif",..., fnCallback: 上传成功回调函数
var ImageUpload = (function () {
    var _oUploader = new Object(),
        _uploadUrl = "";
    return {
        initUpload: function (uploadUrl, fileType, fnCallback) {

            this._uploadUrl = uploadUrl;
            this._oUploader = new plupload.Uploader({
                runtimes: 'gears,html4,html5,flash,silverlight,browserplus',
                browse_button: 'pickfiles', //打开文件按钮id
                container: 'container', //dom容器id
                max_file_size: '30mb', //最大文件大小
                //chunk_size: '1mb',  // 上传分块每块的大小，这个值小于服务器最大上传限制的值即可。（文件总大小/chunk_size = 分块数）。
                url: uploadUrl, //上传服务地址
                //resize: { width: 320, height: 240, quality: 90 },
                flash_swf_url: '~/scripts/plugins/plupload/js/plupload.flash.swf',
                silverlight_xap_url: '~/scripts/plugins/plupload/js/plupload.silverlight.xap',
                filters: [
                    { title: "Files", extensions: fileType }
                ]
            });

            //绑定初始化事件
            this._oUploader.bind('Init', function (up, params) {
                $("#tbFilePath").val("");
            });

            //绑定添加文件事件
            this._oUploader.bind('FilesAdded', function (up, files) {
                up.splice(0, up.files.length);

                if (files != null && files.length > 0)
                    var length = files.length;
                for (var i = 0; i < length; i++) {
                    if (files.length > 1) {
                        files.pop();
                    } else {
                        $("#tbFilePath").val(files[0].name);
                    }
                }
            });

            //绑定组件出错事件
            this._oUploader.bind('Error', function (_oUploader, erorr) {
                switch (erorr.code) {
                    case plupload.FILE_EXTENSION_ERROR:
                        $("#importError").html("不支持此类型的文件上传");
                        $("#tbFilePath").val("");
                        this._oUploader.splice(0, _oUploader.files.length);
                        break;
                    case plupload.FILE_SIZE_ERROR:
                        $("#importError").html("最大上传文件大小为10M");
                        $("#tbFilePath").val("");
                        this._oUploader.splice(0, _oUploader.files.length);
                        break;
                    default:
                        $("#importError").html(erorr.message);
                        break;
                }
                $.colorbox.resize();
            });

            //文件上传成功事件
            this._oUploader.bind('FileUploaded', function (_oUploader, file, response) {
                try {
                    var data = eval("(" + response.responseText + ")");
                    if (data.MSG == "导入成功！") {
                        $("#importError").html(data.MSG);
                    }
                    else {
                        $("#importError").html(data.MSG);
                    }
                } catch (ex) { }
                fnCallback(response);
            });

            this._oUploader.init();
        },
        startUpload: function (value) {
            this._oUploader.settings.page_url = this._uploadUrl + '?fileName=' + escape($("#tbImportName").val());
            this._oUploader.settings.url = this._uploadUrl + '?fileName=' + escape($("#tbImportName").val());
            this._oUploader.start();
        },
        startUploadWithArgs: function (args) {
            this._oUploader.settings.page_url = this._uploadUrl + '?fileName=' + escape($("#tbImportName").val());
            this._oUploader.settings.url = this._uploadUrl + '?fileName=' + escape($("#tbImportName").val());

            for (i = 0; i < args.length; i++) {
                this._oUploader.settings.page_url += "&arg" + i + "=" + args[i];
                this._oUploader.settings.url += "&arg" + i + "=" + args[i];
            };
            this._oUploader.start();
        }
    }
})();