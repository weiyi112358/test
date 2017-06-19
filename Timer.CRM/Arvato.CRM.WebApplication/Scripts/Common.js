

//同步调用后台方法
// 1. ajaxSync('posturl',{a:'a'},function(res){})
// 2. ajaxSync('posturl',{a:'a'},{'dataType':'json',callback:function(res){}})
function ajaxSync(uri, data, o) {
    if (typeof (o) == "function") o = { callback: o };
    o = $.extend({}, o, { async: false });
    ajax(uri, data, o);
};

// 重写$.ajax()方法
// 1. ajax('posturl',{a:'a'},function(res){})
// 2. ajax('posturl',{a:'a'},{'dataType':'json',callback:function(res){}})
function ajax(uri, data, o) {
    if (typeof (o) == "function") o = { callback: o };
    o = $.extend({}, {
        cache: 0,
        //contentType: 'application/text',  
        dataType: 'json',
        type: 'post',
        async: true
    }, o);
    return $.ajax({
        cache: o.cache,
        contentType: o.contentType,
        type: o.type,
        async: o.async,
        url: uri,
        dataType: o.dataType,
        data: parseData(data),
        success: function (result) {
            if (result && result.IsPass == false && result.MSG == "系统登陆超时") {
                location.href = "/Auth/Login";
                //if (result.Obj && '[object Array]' == Object.prototype.toString.call(result.Obj)) {
                //    for (var d in result.Obj) {
                //        if (result.Obj[d] && result.Obj[d]['Lexp_LoginTimeout']) {
                //            $.dialog(result.Obj[d]['Lexp_LoginTimeout'], {}, null, function () { location.reload() });
                //            return
                //        }
                //    }
                //}
            }
            o.callback && o.callback(result);
        },
        error: function (event, request, settings) {
            if (request == "parsererror") location.href = "/Home/Login"; else $.dialog(event.responseText);         
            //alert(event.responseText);
        }
    });
};

// 将JSON数据转换为有效的页面提交数据
// 修复mvc后台获取不到对象的bug
function parseData(data, prex) {
    var d = [], p = prex || '', n = /^\d+$/, p1 = p;
    if ('[object String]' == Object.prototype.toString.call(data)) {
        if (p1) return [p1, data].join("=");
        return data;
    }
    if (p1 != '') p1 += '.';
    for (var td in data) {
        if (n.test(td))
            d.push(parseData(data[td], p + '[' + td + ']'));
        else {
            switch (Object.prototype.toString.call(data[td])) {
                case '[object Array]': d.push(parseData(data[td], p1 + td)); break;
                case '[object Object]': d.push(parseData(data[td], p1 + td)); break;
                    //default: d.push((p1 + td + '=' + data[td]).replace(/&/ig, '%26')); break;//如果不对 请改成 %38 -- modify by richard 2014-04-26
                default: //d.push((p1 + td + '=' + data[td]).replace(/\+/g, "%2B").replace(/\&/g, "%26")); break;
                    d.push((p1 + td + '=' + data[td]).replace(/\+/g, "%2B").replace(/\&/g, "%26").replace(/\?/g, "%3F")); break;

            }
        }
    }
    return d.join('&');
};

//表格插件装载数据
function pushData(d, oData) {
    var data = oData.value;
    var prex = oData.name;
    convertData(d, data, prex);
}

//表格插件转换数据
function convertData(d, data, prex) {
    var p = prex || '', n = /^\d+$/, p1 = p;
    if ('[object String]' == Object.prototype.toString.call(data)) {
        if (p1) d.push({ name: p1, value: data });
    }
    if (p1 != '') p1 += '.';
    for (var td in data) {
        if (n.test(td))
            d.push({ name: p + '[' + td + ']', value: data[td] });
        else {
            switch (Object.prototype.toString.call(data[td])) {
                case '[object Array]': pushArrayData(d, data[td], p1 + td); break;
                case '[object Object]': pushArrayData(d, data[td], p1 + td); break;
                default: d.push({ name: p1 + td, value: data[td] }); break;
            }
        }
    }
};

//将[{name:'name1',value:'value1'}]转换为{'name1':'value1'}
function fixData(d) {
    var ndata = {};
    for (var i in d) {
        ndata[d[i].name] = d[i].value;
    }
    return ndata;
};

// html文本编码
function encode(d) {
    //return encodeURIComponent(d);
    return $('<div/>').text(d).html();
}

// html文本解码
function decode(d) {
    //return decodeURIComponent(d); 
    return $('<div/>').html(d).text();
}

//转换时间格式
function changeTimeFormat(time, type) {
    var formatTime = "";
    if (time.substr(0, 1) == "0") {
        time = time.substr(1);
    }
    var semicolonIndex = time.indexOf(":");
    var hour = parseInt(time.substr(0, semicolonIndex));
    //由"11:25 AM"->"11:25:00"
    if (type == "1") {
        if (time.indexOf("PM") > 0) {
            hour += 12;
        }
        formatTime = hour + time.substr(semicolonIndex);
        if (hour < 10) {
            formatTime = "0" + formatTime;
        }
        formatTime = formatTime.substr(0, 5) + ":00";
    }
    else {
        //由"11:25:00"->"11:25 AM",type等于2
        var noonFlag = " AM";
        if (hour > 12) {
            hour -= 12;
            noonFlag = " PM";
        }
        formatTime = hour + time.substr(semicolonIndex);
        if (hour < 10) {
            formatTime = "0" + formatTime;
        }
        formatTime = formatTime.substr(0, 5) + noonFlag;
    }
    return formatTime;
}

function setPermission() {
    var esl = JSON.parse(decode($('#pageElementSettingList').val() || '[]'));
    for (var i in esl) {
        var es = esl[i];
        if (es.ElementKey) {
            if (es.SettingProp) {
                var sp = es.SettingProp.split(';')
                for (var j in sp) {
                    if (sp.indexOf(':')) {
                        var p = sp[j].split(':');
                        $(es.ElementKey).prop(p[0], p[1]);
                    }
                }
            }
            if (es.SettingCss) {
                var cmd = es.SettingCss.split(',')[0], el = $(es.ElementKey),el_specil = $(es.ElementKey + " span");
                if (el_specil.length > 0) {
                    if (cmd == "remove") el_specil.css("display", "none");
                }
                else {
                    el && el[cmd] && el[cmd]();
                    $(es.ElementKey)[cmd]();
                }
            }
        }
    }
};

// 处理后台返回错误信息
// errs格式： [{'ElementId1':'ErrorMessage'，'ElementId2':'ErrorMessage'},'ErrorMessage'，{'ElementId3':'ErrorMessage'}]
function processErrs(errs) {
    var errmsg = []
    if ('[object String]' == Object.prototype.toString.call(errs)) {
        $.dialog(errs);
        return;
    }
    for (var i in errs) {
        if (errs[i] == null) continue;
        else if (typeof errs[i] == 'object') {
            for (var j in errs[i]) {
                if (!$('#' + j)
                    .removeClass('valid')
                    .addClass('error1')
                    .next('.help-block')
                    .removeClass('valid')
                    .addClass('error')
                    .css("color","red")
                    .text('' + errs[i][j])
                    .show()
                    .length)
                    errmsg.push(errs[i][j]);
            }
        } else errmsg.push(errs[i]);
    }
    if (errmsg.length > 0) $.dialog(errmsg.join('<br/>'))
}

// 验证表单
function validate(form) {
    return $(form).valid();
}
// 页面跳转
$.go = function (uri) {
    location.href = uri;
}
// 上传控件
var ctrlUpload = (function () {
    var _oUploader = new Object(),
        _uploadUrl = "";
    var _key = "";
    return {
        initUpload: function (uploadUrl, paraKey, fnCallback) {
            this._key = paraKey;
            this._uploadUrl = uploadUrl;
            this._oUploader = new plupload.Uploader({
                runtimes: 'gears,html4,flash,silverlight,browserplus',
                browse_button: 'pickfiles', //打开文件按钮id
                container: 'container', //dom容器id
                max_file_size: '10mb', //最大文件大小
                url: uploadUrl, //上传服务地址
                //resize: { width: 320, height: 240, quality: 90 },
                flash_swf_url: '~/Gebo/lib/plupload/js/plupload.flash.swf',
                silverlight_xap_url: '~/Gebo/lib/plupload/js/plupload.silverlight.xap',
                filters: [
                    { title: "Excel files", extensions: "xls,xlsx" }
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
                //var data = eval("(" + response.response + ")");
                //if (data.MSG == "导入成功！") {
                //    $("#importError").html(data.MSG);
                //}
                //else {
                //    $("#importError").html(data.MSG);
                //}
                fnCallback(response);
            });

            this._oUploader.init();
        },
        startUpload: function (value) {
            this._oUploader.settings.page_url = this._uploadUrl + '?fileName=' + escape($("#tbImportName").val());// + '&key=' + this._key;
            this._oUploader.settings.url = this._uploadUrl + '?fileName=' + escape($("#tbImportName").val());//+ '&key=' + this._key;
            this._oUploader.start();
        }
    }
})();

//页面打印
//uri打印页面模版
//data 页面需要的数据
//callback 页面打印后，执行的回调程序
function printPage(uri, data, callback) {
    var w = window.open(uri);
    !function p() {
        if (!w.loaded) return setTimeout(p, 10);
        callback && $(w).bind('afterprint', function () { callback.call(w) });//支持IE，Firefox
        w.printPage(data);
    }();
}

//设置datepicker格式、语言
if ($.fn.datepicker) {
    $.fn.datepicker.defaults = {
        format: 'yyyy-mm-dd',
        language: 'zn'
    };
    $.fn.datepicker.dates['zn'] = {
        "days": ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
        "daysShort": ['日', '一', '二', '三', '四', '五', '六'],
        "daysMin": ['日', '一', '二', '三', '四', '五', '六'],
        "months": ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
        "monthsShort": ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '十一', '十二']
    };
}

function showEditDialogMessage(dialogID, confirmButtonID, cancelButtonID, successAction, failedAction, para) {     //div内容和标题在相应html中设置
    $("#" + confirmButtonID).unbind();
    $("#" + cancelButtonID).unbind();

    if (successAction == null) {
        successAction = doNothing;
    }
    if (failedAction == null) {
        failedAction = doNothing;
    }

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#" + dialogID,
        overlayClose: false,
        inline: true,
        opacity: '0.85',
        onComplete: function () {
            $("#" + confirmButtonID).click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                $.colorbox.close();
                $(document).bind('cbox_closed', function () {
                    successAction(para);
                    $(document).unbind('cbox_closed');
                });
                return;
            });
            $("#" + cancelButtonID).click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                $.colorbox.close();
                $(document).bind('cbox_closed', function () {
                    failedAction(para);
                    $(document).unbind('cbox_closed');
                });
                return;
            });
        }
    });
}

function doNothing() {

}

function resetFormByID(id) {
    $("#" + id + " :input").not(":button, :submit, :reset").val("").removeAttr("checked").removeAttr("selected");
    $("#" + id + " .error").removeClass('error');
    $("#" + id + " .help-block").html('')
}

//格式化字串
String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
};
