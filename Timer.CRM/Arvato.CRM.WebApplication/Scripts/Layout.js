//extend
(function ($) {
    function buildParams(prefix, obj, traditional, add) {
        var name;

        if (jQuery.isArray(obj)) {
            // Serialize array item.
            $.each(obj, function (i, v) {
                if (traditional || /\[\]$/.test(prefix)) {
                    // Treat each array item as a scalar.
                    add(prefix, v);

                } else {
                    // Item is non-scalar (array or object), encode its numeric index.
                    buildParams(prefix + (typeof v === "object" ? "[" + i + "]" : ""), v, traditional, add);
                }
            });

        } else if (!traditional && $.type(obj) === "object") {
            // Serialize object item.
            for (name in obj) {
                buildParams(prefix + "." + name, obj[name], traditional, add);
            }

        } else {
            // Serialize scalar item.
            add(prefix, obj);
        }
    };

    $.extend(Date.prototype, {
        format: function (format) {
            var o = {
                "M+": this.getMonth() + 1, //month 
                "d+": this.getDate(), //day 
                "h+": this.getHours(), //hour 
                "m+": this.getMinutes(), //minute 
                "s+": this.getSeconds(), //second 
                "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
                "S": this.getMilliseconds() //millisecond 
            };
            if (/(y+)/.test(format)) {
                format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            }
            for (var k in o) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                }
            }
            return format;
        }
    })

    $.extend(String.prototype, {
        date: function () { //判断字符串是否是日期 
            return /^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$/.test(this);
        },
        mail: function () {    //判断字符串是否是邮件地址
            return /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/.test(this);
        },
        zip: function () {    //判断字符串是否是邮编号码
            return /^[1-9]\d{5}$/.test(this);
        },
        mobile: function () { //判断手机号码
            return /^1[3458]\d{9}$/.test(this);
        },
        empty: function () {    //判断字符串是否是空字符串
            return (this.trim().length == 0);
        },
        startWith: function (str) {// 判断字符串是否以指定的字符串开始
            return this.substr(0, str.length) == str;
        },
        endWith: function (str) {// 判断字符串是否以指定的字符串结束
            return this.substr(this.length - str.length) == str;
        },
        trim: function () {// 去掉字符两端的空白字符
            return this.replace(/(^\s*)|(\s*$)/g, '');
        },
        leftTrim: function () {// 去掉字符左端的的空白字符
            return this.replace(/(^[\\s]*)/g, '');
        },
        rightTrim: function () {// 去掉字符右端的空白字符
            return this.replace(/([\\s]*$)/g, '');
        },
        passWord: function () {//密码验证  数字、字母、特殊字符包含两种
            return (/\d+$/.test(this) && (/[A-Za-z]+/.test(this) || /[!-/:-@\[-`{-~]+/.test(this))) || (/[A-Za-z]+/.test(this) && /[!-/:-@\[-`{-~]+/.test(this));
        },
        userName: function () {//英文字母、数字和'_'(下划线) 
            return /^[A-Za-z0-9_]*$/.test(this);
        },
        vehicleNo: function () {//判断是否是车牌号
            return /^[冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤川青藏琼宁渝京沪津]{1}[A-Za-z]{1}([A-Za-z0-9]{5}|[A-Za-z0-9]{4}[警领]{1})$/.test(this)
            || /^WJ[0-9]{2}[A-Za-z]{2}[0-9]{3}$/.test(this)//武警车牌
            || /^[A-Za-z]{2}[0-9]{5}$/.test(this)//军车车牌
        },
        vin: function () {//判断是否是车架号，17位字母、数字
            return /^[A-HJ-NPR-Za-hj-npr-z0-9]{17}$/.test(this);
        },
        identityNo: function () {//判断是否证件号，字母、数字
            if (!this) return;
            var idno = $.trim(this);
            var ilen = idno == null ? 0 : idno.length;
            var aCity = {
                11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古",
                21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏",
                33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东",
                41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西",
                46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南",
                54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏",
                65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 "
            };
            var iSum = 0;
            var info = "";
            var sBirthday = null;
            if (!/^\d{15}$|^\d{17}[\dx]$/i.test(idno)) return false;
            idno = idno.replace(/x$/i, "a");
            if (aCity[parseInt(idno.substr(0, 2))] == null) return false;  //"Error:非法地区";
            if (ilen == 15) {
                sBirthday = "19" + idno.substr(6, 2) + "/" + Number(idno.substr(10, 2)) + "/" + Number(idno.substr(12, 2));
            } else if (ilen == 18) {
                sBirthday = idno.substr(6, 4) + "/" + Number(idno.substr(10, 2)) + "/" + Number(idno.substr(12, 2));
            }
            var d = new Date(sBirthday)
            if (sBirthday != d.format('yyyy/MM/dd')) return false;  //"Error:非法生日";
            if (ilen == 18) {
                for (var i = 17; i >= 0; i--) iSum += (Math.pow(2, i) % 11) * parseInt(idno.charAt(17 - i), 11);
                if (iSum % 11 != 1) return false;  //"Error:非法证号";
            }
            return true;
        },
        armyIdentityNo: function () { //判断是否是军官证号，1-3位汉字+6-8位数字　
            return /^[\u4E00-\u9FFF]{1,3}\d{6,8}$/.test(this);
        },
        passportNo: function () { //判断是否是护照号　
            return /^1[45][0-9]{7}|G[0-9]{8}|P[0-9]{7}|S[0-9]{7,8}|D[0-9]+$/.test(this);
        },
        telephone: function () {//判断是否电话号
            return /^(\d{3,4}-)?\d{7,8}$/.test(this);
        }
    })

    $.extend({
        cookie: function (h, b, a) {
            if (1 < arguments.length && (!/Object/.test(Object.prototype.toString.call(b)) || null === b || void 0 === b)) {
                a = $.extend({}, a);
                if (null === b || void 0 === b) a.expires = -1;
                if ("number" === typeof a.expires) {
                    var d = a.expires, c = a.expires = new Date;
                    c.setDate(c.getDate() + d)
                }
                b = "" + b;
                return document.cookie = [
                    encodeURIComponent(h),
                    "=",
                    a.raw ? b : encodeURIComponent(b),
                    a.expires ? "; expires=" + a.expires.toUTCString() : "",
                    a.path ? "; path=" + a.path : "",
                    a.domain ? "; domain=" + a.domain : "",
                    a.secure ? "; secure" : ""
                ].join("")
            }
            for (var a = b || {},
                d = a.raw ? function (a) { return a } : decodeURIComponent,
                c = document.cookie.split("; "), e = 0, f;
                f = c[e] && c[e].split("=") ;
                e++)
                if (d(f[0]) === h)
                    return d(f[1] || "");
            return null
        },
        request: function (para) {
            var args = new Object();
            var query = location.search.substring(1);
            var pairs = query.split("&"); // Break at ampersand 
            for (var i = 0; i < pairs.length; i++) {
                var pos = pairs[i].indexOf('=');
                if (pos == -1) continue;
                var argname = pairs[i].substring(0, pos);
                var value = pairs[i].substring(pos + 1);
                value = decodeURIComponent(value);
                args[argname] = value;
            }
            return args[para];
        },
        dialog: function (element, options, callback, close) {
            if (element.hasOwnProperty("indexOf")) {
                if (element.indexOf('{') != -1 && element.indexOf('}') != -1) {
                    var a = $.parseJSON(element)
                    if (!a.IsPass && a.MSG == "系统登陆超时") {
                        location.href = "/Home/Login";
                    }
                }
            }

            var el = {};
            el.hide = function (e) {
                e.preventDefault();
                el.remove()
                el.close && el.close.apply(el);
            };
            el.confirm = function (e) {
                el.callback && el.callback.apply(el);
                el.remove();
            };
            el.remove = function () {
                el.wraper.remove(), el.element.remove()
            };

            var settings = {
                header: {
                    closebtn: '&times;',
                    title: '系统提示'
                },
                footer: null/*{
				closebtn: 'Close',
				okbtn: 'Confirm',
			}*/
            }
            el.options = o = $.extend({}, settings, options || {});
            el.callback = callback || !1;
            el.close = close || !1;
            el.wraper = $('<div class="modal-backdrop fade in"></div>').appendTo("body").bind('click', el.hide);
            el.element = ele = $('<div class="modal modal-mini fade in"></div>').appendTo("body");
            var wh = $(window).height(), ww = $(window).width(), bh = wh - 80;
            if (o.header) bh -= 50;
            if (o.footer) bh -= 60;
            if (o.header) {
                var h = $('<div class="modal-header"></div>').appendTo(ele);
                o.header.closebtn && $('<button type="button" class="close pull-right">' + o.header.closebtn + '</button>').bind('click', el.hide).appendTo(h);
                o.header.title && $('<h3>' + o.header.title + '</h3>').appendTo(h);
            }
            var b = $('<div class="modal-body" style="overflow-y:hidden"></div>').appendTo(ele).html(element);
            if (o.footer) {
                var f = $('<div class="modal-footer" style="position: relative;"></div>').appendTo(ele);
                o.footer.okbtn && $('<button type="button" class="btn btn-primary">' + o.footer.okbtn + '</button>').appendTo(f).bind('click', el.confirm)
                o.footer.closebtn && $('<button type="button" class="btn">' + o.footer.closebtn + '</button>').appendTo(f).bind('click', el.hide);
            }
            if (b.height() >= bh) b.css('height', bh);
            ele.css({ 'top': (wh - ele.height()) / 2, 'left': (ww - ele.width()) / 2, 'margin': '0' })
            return el;
        },
        param: function (a, traditional) {
            var prefix,
                s = [],
                add = function (key, value) {
                    // If value is a function, invoke it and return its value
                    value = $.isFunction(value) ? value() : (value == null ? "" : value);
                    s[s.length] = encodeURIComponent(key) + "=" + encodeURIComponent(value);
                };

            // Set traditional to true for jQuery <= 1.3.2 behavior.
            if (traditional === undefined) {
                traditional = $.ajaxSettings && $.ajaxSettings.traditional;
            }

            // If an array was passed in, assume that it is an array of form elements.
            if ($.isArray(a) || (a.jquery && !$.isPlainObject(a))) {
                // Serialize the form elements
                $.each(a, function () {
                    add(this.name, this.value);
                });

            } else {
                // If traditional, encode the "old" way (the way 1.3.2 or older
                // did it), otherwise encode params recursively.
                for (prefix in a) {
                    buildParams(prefix, a[prefix], traditional, add);
                }
            }

            // Return the resulting serialization
            return s.join("&").replace(/%20/g, "+");
        },
        serializeObject: function (a) {
            var o = {};
            $.each(a, function (i, v) {
                if (!(v.name in o)) {
                    o[v.name] = v.value;
                } else {
                    var _v = o[v.name];
                    if ($.isArray(_v)) o[v.name].push(v.value);
                    else o[v.name] = [_v, v.value];
                }
            });
            return o;
        }
    })

    $.fn.extend({
        actual: function (b, k) {
            var c, d, h, g, f, j, e, i;
            if (!this[b]) {
                throw '$.actual => The jQuery method "' + b + '" you called does not exist';
            }
            h = $.extend({
                absolute: false,
                clone: false,
                includeMargin: undefined
            }, k);
            d = this;
            if (h.clone === true) {
                e = function () {
                    d = d.filter(":first").clone().css({ position: "absolute", top: -1000 }).appendTo("body");
                };
                i = function () { d.remove(); };
            } else {
                e = function () {
                    c = d.parents().andSelf().filter(":hidden");
                    g = h.absolute === true ?
                        { position: "absolute", visibility: "hidden", display: "block" } :
                        { visibility: "hidden", display: "block" };
                    f = [];
                    c.each(function () {
                        var m = {}, l;
                        for (l in g) {
                            m[l] = this.style[l];
                            this.style[l] = g[l];
                        }
                        f.push(m);
                    });
                };
                i = function () {
                    c.each(function (m) {
                        var n = f[m], l;
                        for (l in g) {
                            this.style[l] = n[l];
                        }
                    });
                };
            }
            e();
            j = /(outer)/g.test(b) ? d[b](h.includeMargin) : d[b](); i();
            return j;
        }
    })
})(window.jQuery);

//$.browser init
(function ($) {

    if ($.browser) return;

    $.browser = {};
    $.browser.mozilla = false;
    $.browser.webkit = false;
    $.browser.opera = false;
    $.browser.msie = false;

    var nAgt = navigator.userAgent;
    $.browser.name = navigator.appName;
    $.browser.fullVersion = '' + parseFloat(navigator.appVersion);
    $.browser.majorVersion = parseInt(navigator.appVersion, 10);
    var nameOffset, verOffset, ix;

    // In Opera, the true version is after "Opera" or after "Version" 
    if ((verOffset = nAgt.indexOf("Opera")) != -1) {
        $.browser.opera = true;
        $.browser.name = "Opera";
        $.browser.fullVersion = nAgt.substring(verOffset + 6);
        if ((verOffset = nAgt.indexOf("Version")) != -1)
            $.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
        // In MSIE, the true version is after "MSIE" in userAgent 
    else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
        $.browser.msie = true;
        $.browser.name = "Microsoft Internet Explorer";
        $.browser.fullVersion = nAgt.substring(verOffset + 5);
    }
        // In Chrome, the true version is after "Chrome" 
    else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
        $.browser.webkit = true;
        $.browser.name = "Chrome";
        $.browser.fullVersion = nAgt.substring(verOffset + 7);
    }
        // In Safari, the true version is after "Safari" or after "Version" 
    else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
        $.browser.webkit = true;
        $.browser.name = "Safari";
        $.browser.fullVersion = nAgt.substring(verOffset + 7);
        if ((verOffset = nAgt.indexOf("Version")) != -1)
            $.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
        // In Firefox, the true version is after "Firefox" 
    else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
        $.browser.mozilla = true;
        $.browser.name = "Firefox";
        $.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
        // In most other browsers, "name/version" is at the end of userAgent 
    else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) <
    (verOffset = nAgt.lastIndexOf('/'))) {
        $.browser.name = nAgt.substring(nameOffset, verOffset);
        $.browser.fullVersion = nAgt.substring(verOffset + 1);
        if ($.browser.name.toLowerCase() == jQuery.browser.name.toUpperCase()) {
            $.browser.name = navigator.appName;
        }
    }
    // trim the fullVersion string at semicolon/space if present 
    if ((ix = jQuery.browser.fullVersion.indexOf(";")) != -1)
        $.browser.fullVersion = jQuery.browser.fullVersion.substring(0, ix);
    if ((ix = jQuery.browser.fullVersion.indexOf(" ")) != -1)
        $.browser.fullVersion = jQuery.browser.fullVersion.substring(0, ix);

    $.browser.majorVersion = parseInt('' + jQuery.browser.fullVersion, 10);
    if (isNaN(jQuery.browser.majorVersion)) {
        $.browser.fullVersion = '' + parseFloat(navigator.appVersion);
        $.browser.majorVersion = parseInt(navigator.appVersion, 10);
    }
    $.browser.version = $.browser.majorVersion;
})(window.jQuery);

//dataTable overload
(function ($) {
    if ($.fn.dataTable) {
        var setting = {
            "bFilter": !0,                                                        //是否允许搜索.
            "bAutoWidth": !0,                                                      //是否自动计算表格各列宽度
            "bDestroy": !0,
            "bRetrieve": !0,
            "bSort": !0,                                                          //是否允许排序.
            'bPaginate': !0,                                                      //是否分页.
            'bLengthChange': !0,                                                  //是否允许用户自定义每页显示条数.
            "iDisplayLength": 10,                                                 //每页显示的条数.
            'sPaginationType': 'bootstrap_alt',                                       //分页样式.
            "bInfo": !0,                                                          //是否显示表格的一些信息
            "bStateSave": !1,                                                     //是否打开会员端状态记录功能.这个数据是记录在cookies中的，打开了这个记录后,即使刷新一次页面,或重新打开浏览器，之前的状态都是保存下来的 
            "bProcessing": !0,                                                    //当datatable获取数据时候是否显示正在处理提示信息.
            "bServerSide": !1,                                                    //是否采用服务器端分页,如果设置为true,分页方法要在服务器端实现.
            "bScrollCollapse": !0,                                                //指定适当的时候缩起滚动视图
            "sAjaxSource": "",                                                    //mvc后台ajax调用接口.
            "fnServerData": function (u, d, cb) {                                 //异步请求的方法,一般把后台取数的代码都写在该方法中，sSource就是你设置的servlet或者action.
                var _ = $(this).data("setting") || {};
                if (_.fnFixData) d = _.fnFixData(d) || d;
                $.post(u, d, function (r) { cb(typeof (r) === "string" ? JSON.parse(r) : r); })
            },
            "fnInitComplete": function (oSettings, json) {
                $(this).removeAttr("style");
            },
            "fnDrawCallback": function () {
                setPermission();
            },
            //"fnCreatedRow": function (nRow, aData, iDataIndex) {
            //    $("td:contains('null'),td:contains('undefined')", nRow).html('');
            //},
            "oLanguage": {
                "sProcessing": "数据加载中……",
                "sSearch": "搜索:",
                "sLengthMenu": "显示 _MENU_ 条",
                "sInfo": "从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
                "sZeroRecords": "没有记录",
                "sInfoEmpty": "暂无记录",
                "sInfoFiltered": "(一共有 _MAX_  条记录)",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": " 上一页 ",
                    "sNext": " 下一页 ",
                    "sLast": " 末页 "
                }
            }
        }
        $.fn.dataTableOld = $.fn.dataTable;
        $.fn.dataTable = function (u, c, cb, o) {
            var _ = {};

            if (typeof u === 'string') _.sAjaxSource = u;
            else if ($.type(u) === 'boolean') _.bServerSide = !!u;
            else if ($.type(u) === 'object') $.extend(_, u);

            if ($.isFunction(c)) _.fnFixData = c;
            else if ($.isArray(c)) _.aoColumns = c;
            else if ($.type(c) === 'boolean') _.bServerSide = !!c;
            else if ($.type(c) === 'object') $.extend(_, c);

            if ($.isFunction(cb)) _.fnFixData = cb;
            else if ($.type(cb) === 'boolean') _.bServerSide = !!cb;
            else if ($.type(cb) === 'object') $.extend(_, cb);

            if ($.type(o) === 'object') $.extend(_, o);
            else if (o !== undefined) _.bServerSide = !!o;

            if (_.aoColumns) {
                for (var r in _.aoColumns) {
                    if (!_.aoColumns[r].name && _.aoColumns[r].data)
                        _.aoColumns[r].name = _.aoColumns[r].data;
                    if (!_.aoColumns[r].title && _.aoColumns[r].data)
                        _.aoColumns[r].title = _.aoColumns[r].data;
                }
            }

            if (_.bServerSide) _.bFilter = !1;
            _ = $.extend({}, setting, _);

            var tbl = $(this).data('setting', _).dataTableOld(_);

            _.fnDraw = tbl.fnDraw;

            tbl.fnDraw = function (complete) {
                !_.bServerSide && complete && this.api(!0).ajax.reload();
                _.fnDraw.apply(this);
            };

            return tbl;
        }
    }
})(window.jQuery)

//sidebar
$(function () {
    var antiScroll;

    function init() {
        if ($(window).width() > 979) {
            if (!$('body').hasClass('sidebar_hidden')) {
                if ($.cookie('gebo_sidebar') == "hidden") {
                    $('body').addClass('sidebar_hidden');
                    $('.sidebar_switch').toggleClass('on_switch off_switch').attr('title', '显示菜单');//Show Sidebar
                }
            } else {
                $('.sidebar_switch').toggleClass('on_switch off_switch').attr('title', '显示菜单');//Show Sidebar
            }
        } else {
            $('body').addClass('sidebar_hidden');
            $('.sidebar_switch').removeClass('on_switch').addClass('off_switch');
        }

        info_box();
        //* sidebar visibility switch
        $('.sidebar_switch').click(function () {
            $('.sidebar_switch').removeClass('on_switch off_switch');
            if ($('body').hasClass('sidebar_hidden')) {
                $.cookie('gebo_sidebar', null);
                $('body').removeClass('sidebar_hidden');
                $('.sidebar_switch').addClass('on_switch').show();
                $('.sidebar_switch').attr('title', "隐藏菜单");//Hide Sidebar
            } else {
                $.cookie('gebo_sidebar', 'hidden');
                $('body').addClass('sidebar_hidden');
                $('.sidebar_switch').addClass('off_switch');
                $('.sidebar_switch').attr('title', "显示菜单");//Show Sidebar
            }
            info_box();
            update_scroll();
            $(window).resize();
        });
        //* prevent accordion link click
        $('.sidebar .accordion-toggle').click(function (e) { e.preventDefault() });
    }

    function info_box() {
        var s_box = $('.sidebar_info');
        var s_box_height = s_box.actual('height');
        s_box.css({ 'height': s_box_height });
        $('.push').height(s_box_height);
        $('.sidebar_inner').css({
            'margin-bottom': '-' + s_box_height + 'px',
            'min-height': '100%'
        });
    }

    function make_active() {
        var thisAccordion = $('#side_accordion');
        thisAccordion.find('.accordion-heading').removeClass('sdb_h_active');
        var thisHeading = thisAccordion.find('.accordion-body.in').prev('.accordion-heading');
        if (thisHeading.length) {
            thisHeading.addClass('sdb_h_active');
        }
    }

    function make_scroll() {
        antiScroll = $('.antiScroll').antiscroll().data('antiscroll');
    }

    function update_scroll() {
        if ($('.antiScroll').length) {
            if ($(window).width() > 979) {
                $('.antiscroll-inner,.antiscroll-content').height($(window).height() - 40);
            } else {
                $('.antiscroll-inner,.antiscroll-content').height('400px');
            }
            antiScroll.refresh();
        }
    }

    init(), make_active(), make_scroll(), update_scroll();
});

//menu
$(function () {
    var uri = window.location.pathname.replace("CardLock", "CardChange").replace("CardUnLock", "CardChange").replace("OldToNew", "CardChange").toLowerCase(), curnav, len = 0;
    $(".accordion-body a").each(function (i, a) {
        var nav = $(a), href = nav.attr('href').replace(/\?.*/, '').toLowerCase();
        if (uri.indexOf(href) == 0 && href.length > len) {
            curnav = nav.closest("li");
            len = href.length;
            if (uri == href) len = 9999;
        }
    }).click(function () {
        $('.accordion-body li').removeClass('active');
        $(this).closest('li').addClass('active');
    });
    if (curnav) {
        var $li = curnav || $(".accordion-body a:first").closest("li"), $group = $li.closest(".accordion-group"), $accordion = $group.closest(".accordion");
        if (!$li.hasClass("active")) {
            $(".accordion-heading", $accordion).removeClass("sdb_h_active")
            $("li", $accordion).removeClass("active");
            $(".accordion-heading", $group).addClass("sdb_h_active")
            $(".accordion-body", $group).addClass("in");
            $li.addClass("active");
        }
    }
});

//reload
$(function () {
    // 时间默认设置
    if ($.fn.timepicker) {
        $.fn.timepicker.defaults = {
            "minuteStep": 5,
            "format": 'HH:mm',//时间模式 
            "disableFocus": !0,
            "defaultTime": "current",
            "showMeridian": false,
            "template": "dropdown",
            "templates": {}
        };
        $('input[type=time],.time').prop('type', 'text').timepicker();
    }

    // 日期默认格式
    if ($.fn.datepicker) {
        $.fn.datepicker.defaults = {
            autoclose: !0,
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
        $('input[type=date],.date').prop('type', 'text').datepicker();
    }
});

$(function () {
    //页面元素权限设置
    setPermission();
});

$(function () {
    //清空日期控件
    $("body").delegate(".btn-date-clear", "click", function () {
        $(this).prev().val("");
    });
});

//隐藏菜单
function hidemenu() {
    $('body').addClass('sidebar_hidden');
    $('.sidebar_switch').addClass('off_switch');
    $('.sidebar_switch').attr('title', "显示菜单");//Show Sidebar
}

//为数组加上单引号，用于报表的渠道和店铺多选
function addqout(array) {
    if (array != null) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] != '') {
                array[i] = "'" + array[i] + "'"
            }
        }
    }
    else {
        array = '';
    }
    return array;
}
//报表中比率值乘100取两位小数加百分号
function convertRateData(value) {
    return (100 * value).toFixed(2) + "%";
}
//报表中金额保留两位小数，以千字符形式显示
function convertDecimal(value) {
    if (value != null)
        return value.toFixed(2);
    else
        return null;
}

//获取年月的最后一天
function getLastDay(year, month) {
    var new_year = year; //取当前的年份 
    var new_month = month++;//取下一个月的第一天，方便计算（最后一天不固定） 
    if (month > 12) {
        new_month -= 12; //月份减 
        new_year++; //年份增 
    }
    var new_date = new Date(new_year, new_month, 1); //取当年当月中的第一天 
    return (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate();//获取当月最后一天日期 
}

//数字加千分号
function formatNumber(number, isFixed) {
    if (!number && isFixed) {
        return "0.00";
    }
    if (!number) {
        return 0;
    }
    if (isFixed)
        number = Number(number).toFixed(2);
    number = number.toString();
    //number = number.replace(/,/g, "");
    var digit = number.indexOf("."); // 取得小数点的位置
    var int = number.substr(0, digit); // 取得小数中的整数部分
    var i;
    var mag = new Array();
    var word;
    if (number.indexOf(".") == -1) { // 整数时
        i = number.length; // 整数的个数
        while (i > 0) {
            word = number.substring(i, i - 3); // 每隔3位截取一组数字
            i -= 3;
            mag.unshift(word); // 分别将截取的数字压入数组
        }
        number = mag;
    } else { // 小数时
        i = int.length; // 除小数外，整数部分的个数
        while (i > 0) {
            word = int.substring(i, i - 3); // 每隔3位截取一组数字
            i -= 3;
            mag.unshift(word);
        }
        number = mag + number.substring(digit);
    }

    return number;
}