/*==============================通用function==================================*/
/*
** jQuery Cookie Plugin
** https://github.com/carhartl/jquery-cookie
**
** Copyright 2011, Klaus Hartl
** Dual licensed under the MIT or GPL Version 2 licenses.
** http://www.opensource.org/licenses/mit-license.php
** http://www.opensource.org/licenses/GPL-2.0
**/

/*①、Create expiring cookie, 7 days from then:*/
//$.cookie('the_cookie', 'the_value', { expires: 7 });
/*②、Read cookie:*/
//$.cookie('the_cookie'); // => 'the_value' or Null
/*③、Delete cookie by passing null as value:*/
//$.cookie('the_cookie', null); or $.cookie('the_cookie', '', { expires: -1 });

/*①、Create expiring session, 7 days from then:*/
//$.session('the_cookie', 'the_value', { expires: 7 });
/*②、Read cookie:*/
//$.session('the_cookie'); // => 'the_value' or Null
/*③、Delete cookie by passing null as value:*/
//$.session('the_cookie', null);

+function ($) {
    $.extend({
        cookie: function (key, value, options) {
            // key and at least value given, set cookie...
            if (arguments.length > 1 && (!/Object/.test(Object.prototype.toString.call(value)) || value === null || value === undefined)) {
                options = $.extend({}, options);

                if (value === null || value === undefined) {
                    options.expires = -1;
                }

                if (typeof options.expires === 'number') {
                    var days = options.expires, t = options.expires = new Date();
                    t.setDate(t.getDate() + days);
                }

                value = String(value);

                return (document.cookie = [
                    encodeURIComponent(key), '=', options.raw ? value : encodeURIComponent(value),
                    options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
                    options.path ? '; path=' + options.path : '',
                    options.domain ? '; domain=' + options.domain : '',
                    options.secure ? '; secure' : ''
                ].join(''));
            }

            // key and possibly options given, get cookie...
            options = value || {};
            var decode = options.raw ? function (s) { return s; } : decodeURIComponent;

            var pairs = document.cookie.split('; ');
            for (var i = 0, pair; pair = pairs[i] && pairs[i].split('=') ; i++) {
                if (decode(pair[0]) === key) return decode(pair[1] || ''); // IE saves cookies with empty string as "c; ", e.g. without "=" as opposed to EOMB, thus pair[1] may be undefined
            }
            return null;
        },
        session: function (key, value, options) {
            var get = function () {
                return JSON.parse(sessionStorage['_session_'] || '{}');
            }, set = function (body) {
                sessionStorage['_session_'] = JSON.stringify(body);
            }, body = get();
            // key and at least value given, set cookie...
            if (arguments.length > 1) {
                if (value === null || value === undefined) delete body[key];
                else body[key] = value;
                set(body); return $;
            }
            return body[key] || null;
        },
        storage: function (key, value, options) {
            var get = function () {
                return JSON.parse(localStorage['_session_'] || '{}');
            }, set = function (body) {
                localStorage['_session_'] = JSON.stringify(body);
            }, body = get();
            // key and at least value given, set cookie...
            if (arguments.length > 1) {
                if (value === null || value === undefined) delete body[key];
                else body[key] = value;
                set(body); return $;
            }
            return body[key] || null;
        }
    });
}(jQuery)
/*
** jQuery MD5 hash algorithm function
** 
** 	<code>
** 		Calculate the md5 hash of a String 
** 		String $.md5 ( String str )
** 	</code>
** 
** Calculates the MD5 hash of str using the » RSA Data Security, Inc. MD5 Message-Digest Algorithm, and returns that hash. 
** MD5 (Message-Digest algorithm 5) is a widely-used cryptographic hash function with a 128-bit hash value. MD5 has been employed in a wide variety of security applications, and is also commonly used to check the integrity of data. The generated hash is also non-reversable. Data cannot be retrieved from the message digest, the digest uniquely identifies the data.
** MD5 was developed by Professor Ronald L. Rivest in 1994. Its 128 bit (16 byte) message digest makes it a faster implementation than SHA-1.
** This script is used to process a variable length message into a fixed-length output of 128 bits using the MD5 algorithm. It is fully compatible with UTF-8 encoding. It is very useful when u want to transfer encrypted passwords over the internet. If you plan using UTF-8 encoding in your project don't forget to set the page encoding to UTF-8 (Content-Type meta tag). 
** This function orginally get from the WebToolkit and rewrite for using as the jQuery plugin.
** 
** Example
** 	Code
** 		<code>
** 			$.md5("I'm Persian."); 
** 		</code>
** 	Result
** 		<code>
** 			"b8c901d0f02223f9761016cfff9d68df"
** 		</code>
** @alias Muhammad Hussein Fattahizadeh < muhammad [AT] semnanweb [DOT] com >
** @link http://www.semnanweb.com/jquery-plugin/md5.html
** @see http://www.webtoolkit.info/
** @license http://www.gnu.org/licenses/gpl.html [GNU General Public License]
** @param {jQuery} {md5:function(string))
** @return string
**/
+ function ($) {
    var rotateLeft = function (lValue, iShiftBits) {
        return (lValue << iShiftBits) | (lValue >>> (32 - iShiftBits));
    }

    var addUnsigned = function (lX, lY) {
        var lX4, lY4, lX8, lY8, lResult;
        lX8 = (lX & 0x80000000);
        lY8 = (lY & 0x80000000);
        lX4 = (lX & 0x40000000);
        lY4 = (lY & 0x40000000);
        lResult = (lX & 0x3FFFFFFF) + (lY & 0x3FFFFFFF);
        if (lX4 & lY4) return (lResult ^ 0x80000000 ^ lX8 ^ lY8);
        if (lX4 | lY4) {
            if (lResult & 0x40000000) return (lResult ^ 0xC0000000 ^ lX8 ^ lY8);
            else return (lResult ^ 0x40000000 ^ lX8 ^ lY8);
        } else {
            return (lResult ^ lX8 ^ lY8);
        }
    }

    var F = function (x, y, z) {
        return (x & y) | ((~x) & z);
    }

    var G = function (x, y, z) {
        return (x & z) | (y & (~z));
    }

    var H = function (x, y, z) {
        return (x ^ y ^ z);
    }

    var I = function (x, y, z) {
        return (y ^ (x | (~z)));
    }

    var FF = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(F(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };

    var GG = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(G(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };

    var HH = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(H(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };

    var II = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(I(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };

    var convertToWordArray = function (string) {
        var lWordCount;
        var lMessageLength = string.length;
        var lNumberOfWordsTempOne = lMessageLength + 8;
        var lNumberOfWordsTempTwo = (lNumberOfWordsTempOne - (lNumberOfWordsTempOne % 64)) / 64;
        var lNumberOfWords = (lNumberOfWordsTempTwo + 1) * 16;
        var lWordArray = Array(lNumberOfWords - 1);
        var lBytePosition = 0;
        var lByteCount = 0;
        while (lByteCount < lMessageLength) {
            lWordCount = (lByteCount - (lByteCount % 4)) / 4;
            lBytePosition = (lByteCount % 4) * 8;
            lWordArray[lWordCount] = (lWordArray[lWordCount] | (string.charCodeAt(lByteCount) << lBytePosition));
            lByteCount++;
        }
        lWordCount = (lByteCount - (lByteCount % 4)) / 4;
        lBytePosition = (lByteCount % 4) * 8;
        lWordArray[lWordCount] = lWordArray[lWordCount] | (0x80 << lBytePosition);
        lWordArray[lNumberOfWords - 2] = lMessageLength << 3;
        lWordArray[lNumberOfWords - 1] = lMessageLength >>> 29;
        return lWordArray;
    };

    var wordToHex = function (lValue) {
        var WordToHexValue = "", WordToHexValueTemp = "", lByte, lCount;
        for (lCount = 0; lCount <= 3; lCount++) {
            lByte = (lValue >>> (lCount * 8)) & 255;
            WordToHexValueTemp = "0" + lByte.toString(16);
            WordToHexValue = WordToHexValue + WordToHexValueTemp.substr(WordToHexValueTemp.length - 2, 2);
        }
        return WordToHexValue;
    };

    var uTF8Encode = function (string) {
        string = string.replace(/\x0d\x0a/g, "\x0a");
        var output = "";
        for (var n = 0; n < string.length; n++) {
            var c = string.charCodeAt(n);
            if (c < 128) {
                output += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                output += String.fromCharCode((c >> 6) | 192);
                output += String.fromCharCode((c & 63) | 128);
            } else {
                output += String.fromCharCode((c >> 12) | 224);
                output += String.fromCharCode(((c >> 6) & 63) | 128);
                output += String.fromCharCode((c & 63) | 128);
            }
        }
        return output;
    };

    $.extend({
        md5: function (string) {
            var x = Array();
            var k, AA, BB, CC, DD, a, b, c, d;
            var S11 = 7, S12 = 12, S13 = 17, S14 = 22;
            var S21 = 5, S22 = 9, S23 = 14, S24 = 20;
            var S31 = 4, S32 = 11, S33 = 16, S34 = 23;
            var S41 = 6, S42 = 10, S43 = 15, S44 = 21;
            string = uTF8Encode(string);
            x = convertToWordArray(string);
            a = 0x67452301; b = 0xEFCDAB89; c = 0x98BADCFE; d = 0x10325476;
            for (k = 0; k < x.length; k += 16) {
                AA = a; BB = b; CC = c; DD = d;
                a = FF(a, b, c, d, x[k + 0], S11, 0xD76AA478);
                d = FF(d, a, b, c, x[k + 1], S12, 0xE8C7B756);
                c = FF(c, d, a, b, x[k + 2], S13, 0x242070DB);
                b = FF(b, c, d, a, x[k + 3], S14, 0xC1BDCEEE);
                a = FF(a, b, c, d, x[k + 4], S11, 0xF57C0FAF);
                d = FF(d, a, b, c, x[k + 5], S12, 0x4787C62A);
                c = FF(c, d, a, b, x[k + 6], S13, 0xA8304613);
                b = FF(b, c, d, a, x[k + 7], S14, 0xFD469501);
                a = FF(a, b, c, d, x[k + 8], S11, 0x698098D8);
                d = FF(d, a, b, c, x[k + 9], S12, 0x8B44F7AF);
                c = FF(c, d, a, b, x[k + 10], S13, 0xFFFF5BB1);
                b = FF(b, c, d, a, x[k + 11], S14, 0x895CD7BE);
                a = FF(a, b, c, d, x[k + 12], S11, 0x6B901122);
                d = FF(d, a, b, c, x[k + 13], S12, 0xFD987193);
                c = FF(c, d, a, b, x[k + 14], S13, 0xA679438E);
                b = FF(b, c, d, a, x[k + 15], S14, 0x49B40821);
                a = GG(a, b, c, d, x[k + 1], S21, 0xF61E2562);
                d = GG(d, a, b, c, x[k + 6], S22, 0xC040B340);
                c = GG(c, d, a, b, x[k + 11], S23, 0x265E5A51);
                b = GG(b, c, d, a, x[k + 0], S24, 0xE9B6C7AA);
                a = GG(a, b, c, d, x[k + 5], S21, 0xD62F105D);
                d = GG(d, a, b, c, x[k + 10], S22, 0x2441453);
                c = GG(c, d, a, b, x[k + 15], S23, 0xD8A1E681);
                b = GG(b, c, d, a, x[k + 4], S24, 0xE7D3FBC8);
                a = GG(a, b, c, d, x[k + 9], S21, 0x21E1CDE6);
                d = GG(d, a, b, c, x[k + 14], S22, 0xC33707D6);
                c = GG(c, d, a, b, x[k + 3], S23, 0xF4D50D87);
                b = GG(b, c, d, a, x[k + 8], S24, 0x455A14ED);
                a = GG(a, b, c, d, x[k + 13], S21, 0xA9E3E905);
                d = GG(d, a, b, c, x[k + 2], S22, 0xFCEFA3F8);
                c = GG(c, d, a, b, x[k + 7], S23, 0x676F02D9);
                b = GG(b, c, d, a, x[k + 12], S24, 0x8D2A4C8A);
                a = HH(a, b, c, d, x[k + 5], S31, 0xFFFA3942);
                d = HH(d, a, b, c, x[k + 8], S32, 0x8771F681);
                c = HH(c, d, a, b, x[k + 11], S33, 0x6D9D6122);
                b = HH(b, c, d, a, x[k + 14], S34, 0xFDE5380C);
                a = HH(a, b, c, d, x[k + 1], S31, 0xA4BEEA44);
                d = HH(d, a, b, c, x[k + 4], S32, 0x4BDECFA9);
                c = HH(c, d, a, b, x[k + 7], S33, 0xF6BB4B60);
                b = HH(b, c, d, a, x[k + 10], S34, 0xBEBFBC70);
                a = HH(a, b, c, d, x[k + 13], S31, 0x289B7EC6);
                d = HH(d, a, b, c, x[k + 0], S32, 0xEAA127FA);
                c = HH(c, d, a, b, x[k + 3], S33, 0xD4EF3085);
                b = HH(b, c, d, a, x[k + 6], S34, 0x4881D05);
                a = HH(a, b, c, d, x[k + 9], S31, 0xD9D4D039);
                d = HH(d, a, b, c, x[k + 12], S32, 0xE6DB99E5);
                c = HH(c, d, a, b, x[k + 15], S33, 0x1FA27CF8);
                b = HH(b, c, d, a, x[k + 2], S34, 0xC4AC5665);
                a = II(a, b, c, d, x[k + 0], S41, 0xF4292244);
                d = II(d, a, b, c, x[k + 7], S42, 0x432AFF97);
                c = II(c, d, a, b, x[k + 14], S43, 0xAB9423A7);
                b = II(b, c, d, a, x[k + 5], S44, 0xFC93A039);
                a = II(a, b, c, d, x[k + 12], S41, 0x655B59C3);
                d = II(d, a, b, c, x[k + 3], S42, 0x8F0CCC92);
                c = II(c, d, a, b, x[k + 10], S43, 0xFFEFF47D);
                b = II(b, c, d, a, x[k + 1], S44, 0x85845DD1);
                a = II(a, b, c, d, x[k + 8], S41, 0x6FA87E4F);
                d = II(d, a, b, c, x[k + 15], S42, 0xFE2CE6E0);
                c = II(c, d, a, b, x[k + 6], S43, 0xA3014314);
                b = II(b, c, d, a, x[k + 13], S44, 0x4E0811A1);
                a = II(a, b, c, d, x[k + 4], S41, 0xF7537E82);
                d = II(d, a, b, c, x[k + 11], S42, 0xBD3AF235);
                c = II(c, d, a, b, x[k + 2], S43, 0x2AD7D2BB);
                b = II(b, c, d, a, x[k + 9], S44, 0xEB86D391);
                a = addUnsigned(a, AA);
                b = addUnsigned(b, BB);
                c = addUnsigned(c, CC);
                d = addUnsigned(d, DD);
            }
            var tempValue = wordToHex(a) + wordToHex(b) + wordToHex(c) + wordToHex(d);
            return tempValue.toUpperCase();
        }
    });
}(jQuery);

//表示全局唯一标识符 (GUID)。
function Guid(g) {
    var arr = new Array(); //存放32位数值的数组
    if (typeof (g) == "string") { //如果构造函数的参数为字符串
        InitByString(arr, g);
    } else {
        InitByOther(arr);
    }

    //返回一个值，该值指示 Guid 的两个实例是否表示同一个值。
    this.Equals = function (o) {
        if (o && o.IsGuid) {
            return this.ToString() == o.ToString();
        } else {
            return false;
        }
    }

    //Guid对象的标记
    this.IsGuid = function () { }

    //返回 Guid 类的此实例值的 String 表示形式。
    this.ToString = function (format) {
        if (typeof (format) == "string") {
            if (format == "N" || format == "D" || format == "B" || format == "P") {
                return ToStringWithFormat(arr, format);
            } else {
                return ToStringWithFormat(arr, "D");
            }
        } else {
            return ToStringWithFormat(arr, "D");
        }
    }

    //由字符串加载
    function InitByString(arr, g) {
        g = g.replace(/\{|\(|\)|\}|-/g, "");
        g = g.toLowerCase();
        if (g.length != 32 || g.search(/[^0-9,a-f]/i) != -1) {
            InitByOther(arr);
        } else {
            for (var i = 0; i < g.length; i++) {
                arr.push(g[i]);
            }
        }
    }

    //由其他类型加载
    function InitByOther(arr) {
        var i = 32;
        while (i--) {
            arr.push("0");
        }
    }

    /*
    根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
    N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
    P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)

    */

    function ToStringWithFormat(arr, format) {
        switch (format) {
            case "N":
                return arr.toString().replace(/,/g, "");
            case "D":
                var str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);
                str = str.replace(/,/g, "");
                return str;
            case "B":
                var str = ToStringWithFormat(arr, "D");
                str = "{" + str + "}";
                return str;
            case "P":
                var str = ToStringWithFormat(arr, "D");
                str = "(" + str + ")";
                return str;
            default:
                return new Guid();
        }
    }
}

//Guid 类的默认实例，其值保证均为零。
Guid.Empty = new Guid();

//初始化 Guid 类的一个新实例。
Guid.NewGuid = function () {
    var g = "";
    var i = 32;
    while (i--) {
        g += Math.floor(Math.random() * 16.0).toString(16);
    }
    return new Guid(g).ToString('D');
};


/*
** String Prototype Plugin
**
** Copyright 2014, Leo King

** ①、判断字符串是否是日期
** string.date();
** ②、判断字符串是否是邮件地址
** string.mail();
** ③、判断字符串是否是邮编号码
** string.zip();
** ④、判断手机号码
** string.mobile();
** ⑤、判断字符串是否是空字符串
** string.empty();
** ⑥、判断字符串是否以指定的字符串开始
** string.startWith();
** ⑦、判断字符串是否以指定的字符串结束
** string.endWith();
** ⑧、去掉字符两端的空白字符
** string.trim();
** ⑨、去掉字符左端的的空白字符
** string.leftTrim();
** ⑩、去掉字符右端的空白字符
** string.rightTrim();
**/
+ function ($) {
    $.extend(String.prototype, {
        date: function () {    //判断字符串是否是日期
            return (this.search(/^(?:([0-9]{4}-(?:(?:0?[1,3-9]|1[0-2])-(?:29|30)|((?:0?[13578]|1[02])-31)))|([0-9]{4}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|1\\d|2[0-8]))|(((?:(\\d\\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))-0?2-29)))$/) == 0);
        },
        mail: function () {    //判断字符串是否是邮件地址
            return (this.search(/^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/) == 0);
        },
        zip: function () {    //判断字符串是否是邮编号码
            return (this.search(/^[1-9]\d{5}$/) == 0);
        },
        mobile: function () { //判断手机号码
            return (this.search(/^1[34578]\d{9}$/) == 0);
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
            return this.replace(/(^\s*)|(\s*$)/g, "");
        },
        leftTrim: function () {// 去掉字符左端的的空白字符
            return this.replace(/(^[\\s]*)/g, "");
        },
        rightTrim: function () {// 去掉字符右端的空白字符
            return this.replace(/([\\s]*$)/g, "");
        },
        vehicleNo: function () {//判断是否是车牌号
            if (this.search(/^[冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤川青藏琼宁渝京沪津]{1}[A-Za-z]{1}([A-Za-z0-9]{5}|[A-Za-z0-9]{4}[警领]{1})$/g) == 0)
                return true;
            else if (this.search(/^WJ[0-9]{2}[A-Za-z]{2}[0-9]{3}$/g) == 0)//武警车牌
                return true;
            else if (this.search(/^[A-Za-z]{2}[0-9]{5}$/g) == 0)//军车车牌
                return true;
            else
                return false;
        },
        vin: function () {//判断是否是车架号，17位字母、数字
            return (this.search(/^[A-HJ-NPR-Za-hj-npr-z0-9]{17}$/g) == 0);
        },
        identityNo: function () {//判断是否证件号，字母、数字
            return (this.search(/^[^\_]{9,18}$/g) == 0);
        },
        telephone: function () {//判断是否电话号
            return (this.search(/^(\d{3,4}-)?\d{7,8}$/g) == 0);
        },
        name: function () {//判断是否姓名
            return (this.search(/^[a-zA-Z \u4E00-\u9FA5 ]*$/g) == 0);
        },
        passport: function () {//判断是否护照
            return (this.search(/^1[45][0-9]{7}|G[0-9]{8}|P[0-9]{7}|S[0-9]{7,8}|D[0-9]+$/g) == 0)
        },
        oldMemberCardNo: function () {//判断老会员卡
            return (this.search(/(0101|0202)[0-9]{13}$/g) == 0)
        },
        newMemberCardNo: function () {//判断新会员卡
            return (this.search(/^[168][12][0-9]{7}$/g) == 0)
        },
        giftCardNo: function () {//判断礼品卡
            return (this.search(/^[9][12][0-9]{7}$/g) == 0)
        },
        idCard: function () {//判断身份证
            return (this.search(/^(\d{18,18}|\d{15,15}|\d{17,17}x)$/g) == 0)
        },
        chequeNo: function () {//判断支票
            return (this.search(/^[a-zA-Z0-9]+$/g) == 0);
        }
    });
}(jQuery);

/*
 * 页面按键拦截设置
 * 设置格式为：
 * var HotKeys={
 *	'login.html':{
 *		'Enter':'alert("Enter")',
 *		'Ctrl+Enter':function(){alert('Ctrl+Enter')}
 *	},
 *	'test.html':{
 *		'Enter':function(){alert('Test.html Enter')}
 *	}
 * }
**/
+function (_) {
    var hkp, p;
    function run(e) {
        var key = build(e);
        if (hkp[key]) {
            e.preventDefault(), e.stopPropagation();
            if (hkp[key] instanceof Function) hkp[key]();
            if (typeof hkp[key] == "string") Function(hkp[key])();
        }
    }
    function build(e) {
        var keycode = e.keyCode || e.which || e.charCode, keyname, keys = [];
        e.ctrlKey && keys.push("Ctrl");
        e.altKey && keys.push("Alt");
        e.shiftKey && keys.push("Shift");
        if (keycode >= 64 && keycode <= 90) keyname = String.fromCharCode(keycode);//字符
        else if (keycode >= 48 && keycode <= 57) keyname = String.fromCharCode(keycode);
        else {
            switch (keycode) {
                case 8: keyname = "Backspace"; break;
                case 9: keyname = "Tab"; break;
                case 12: break;//keyname = "Clear"; 
                case 13: keyname = "Enter"; break;
                case 17: break;//Ctrl键
                case 18: break;//Alt键
                case 19: keyname = "Pause"; break;
                case 20: keyname = "CapsLock"; break;
                case 27: keyname = "Esc"; break;
                case 32: keyname = "Space"; break;
                case 33: keyname = "PageUp"; break;
                case 34: keyname = "PageDown"; break;
                case 35: keyname = "End"; break;
                case 36: keyname = "Home"; break;
                case 37: keyname = "Left"; break;
                case 38: keyname = "Up"; break;
                case 39: keyname = "Right"; break;
                case 40: keyname = "Down"; break;
                case 45: keyname = "Insert"; break;
                case 46: keyname = "Delete"; break;
                case 91: break;//keyname = "Win";
                case 96: keyname = "0"; break;
                case 97: keyname = "1"; break;
                case 98: keyname = "2"; break;
                case 99: keyname = "3"; break;
                case 100: keyname = "4"; break;
                case 101: keyname = "5"; break;
                case 102: keyname = "6"; break;
                case 103: keyname = "7"; break;
                case 104: keyname = "8"; break;
                case 105: keyname = "9"; break;
                case 106: keyname = "*"; break;
                case 107: keyname = "+"; break;
                case 108: keyname = "Enter"; break;
                case 109: keyname = "-"; break;
                case 110: keyname = "."; break;
                case 111: keyname = "/"; break;
                case 112: keyname = "F1"; break;//
                case 113: keyname = "F2"; break;
                case 114: keyname = "F3"; break;
                case 115: keyname = "F4"; break;
                case 116: keyname = "F5"; break;
                case 117: keyname = "F6"; break;
                case 118: keyname = "F7"; break;
                case 119: keyname = "F8"; break;
                case 120: keyname = "F9"; break;
                case 121: keyname = "F10"; break;
                case 122: keyname = "F11"; break;
                case 123: keyname = "F12"; break;
                case 144: keyname = 'NumLock'; break;
                case 186: keyname = ";"; break;
                case 187: keyname = "="; break;
                case 188: keyname = ","; break;
                case 189: keyname = "-"; break;
                case 190: keyname = "."; break;
                case 191: keyname = "/"; break;
                case 192: keyname = "`"; break;
                case 219: keyname = "["; break;
                case 220: keyname = "\\"; break;
                case 221: keyname = "]"; break;
                case 222: keyname = "'"; break;
                case 229: break;//Shift键
                case 255: break;//Fn键
            }
        }
        if (keyname) {
            keys.push(keyname)
            return keys.join("+");
        }
    }
    function isArray(obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    }
    function loadScripts(uris) {
        if (!isArray(uris)) return loadScripts([uris]);
        var ret = [];
        for (var i in uris) ret.push(loadScript(uris[i]));
        return $.when.apply($, ret);
    }
    function loadScript(uri) {
        var dtd = $.Deferred();
        var n = document.createElement('script');
        n.src = uri;// + "?_=" + Math.random();
        n.onload =
		n.onreadystatechange = function () {
		    if (!this.readyState     //这是FF的判断语句，因为ff下没有readyState这人值，IE的readyState肯定有值
				|| this.readyState == 'loaded'
				|| this.readyState == 'complete'   // 这是IE的判断语句
				) {
		        dtd.resolve();
		    }
		};
        n.onerror = function () {
            dtd.reject();
        }
        document.head.appendChild(n);
        return dtd.promise();
    }
    loadScripts('scripts/HotKeys.js').done(function () {
        p = window.location.href.replace(/.+\//g, "").toLowerCase(), hkp = (window.HotKeys || {})[p];
        hkp && $(_).bind("keydown", function (e) { run(e) })
    })
}(document);

/*==============================下面是本项目function==================================*/
var navs = [//菜单
    { class: 'saleMan', href: 'Sales.html', text: '销售管理' },
    { class: 'memberMan', href: 'Member.html', text: '会员管理' },
    //{ class: 'stockMan', href: 'stockMG_shipment.html', text: '库存管理' },
    //{ class: 'reportMan', href: '#', text: '报表管理' },
    //{ class: 'salesMan', href: 'salesMG_campaign.html', text: '促销管理' },
    { class: 'home', href: 'welcome.html', text: '首页' },
    { class: 'shop', href: 'shopMG_dutyRoster.html', text: '门店管理' },
    { class: 'message', href: 'Message.html', text: '消息管理' },
];

+function ($) {
    $.extend({
        go: function (uri) { location.href = uri },
        init: function (idx) {
            $(function () {
                //$('header>nav').nav(idx),
                $('.listFoot').inf()
            })
        },
        query: function (arr) { return Enumerable.From(arr) },
    });
    $.fn.extend({
        //go: function (uri, data, options) {
        //    var t = $(this), o = $.extend({}, {
        //        cache: 0,
        //        dataType: 'html',
        //        contentType: 'text/html',  //MVC 不能加这一句
        //        type: 'get',
        //        async: 1
        //    }, options);
        //    $.ajax({
        //        cache: o.cache,
        //        contentType: o.contentType,
        //        type: o.type,
        //        async: o.async,
        //        url: uri,
        //        dataType: o.dataType,
        //        data: data,
        //        success: function (res) { t.html(res); },
        //        error: function (event, request, settings) { console.log(request), alert(event.responseText); }
        //    });
        //},
        nav: function (idx) {
            var ul = $('<ul class="clearFix"></ul>');
            if (idx == undefined) idx = -1;
            for (var i in navs) {
                var n = navs[i];
                var li = $('<li></li>').addClass(n.class).appendTo(ul);
                var a = $('<a></a>').html(n.text).prop('href', n.href).toggleClass('cur', i == idx).appendTo(li);

                //n.inline && a.click(function (e) {
                //    var t = $(this), href = t.prop('href');
                //    e.preventDefault();
                //    $('#content').go(href);
                //    $('a', ul).removeClass('cur');
                //    t.addClass('cur');
                //})
            }
            $(this).html(ul);

            $("nav li").click(function () {
                if (!isIE10()) {
                    showCommonPromptPopup("请用IE浏览器访问，且IE版本需要10及以上！");
                    $.go("Welcome.html");
                    return;
                }

                var pageStr = $(this).find("a").attr("href");
                //return AuthVisitPage(pageStr);
                if (!AuthVisitPage(pageStr))
                    return false;
            });

        },
        inf: function () {
            var ul = $('<ul class="clearFix"></ul>');
            var li = $('<li class="logout"></li>').appendTo(ul);
            var a = $('<a href="javascript:;" onclick="logout()"></a>').appendTo(li);
            $('<img src="images/btn_logout.gif" alt="退出"/>').appendTo(a);
            var userLoginInfo = $.session("_loginInfo");
            //$('<li></li>').html('用户编号：' + userLoginInfo.UserCode).appendTo(ul);
            if (userLoginInfo != null) {
                $('<li></li>').html('用户名称：' + userLoginInfo.UserName).appendTo(ul);
                $('<li></li>').html('门店号：' + userLoginInfo.StoreCode).appendTo(ul);
                $('<li></li>').html('门店名：' + userLoginInfo.StoreName).appendTo(ul);
                $('<li></li>').html('钱箱编号：' + userLoginInfo.CashboxCode).appendTo(ul);
            }

            $(this).html(ul);
        }
    })
}(jQuery)

var msgCount = 0;//推广信息的条数
$(function () {
    dbContext.ready(function () {
        this.Sys_MenuList.load();
        this.Sys_PageList.load();
    });

    var curPageStr = window.location.href;
    curPageStr = curPageStr.toLowerCase().substr(curPageStr.lastIndexOf("/") + 1)
    if (curPageStr.toLowerCase() == "memberinfo.html")
        curPageStr = "Member.html";
    var userLogin = $.session("_loginInfo");
    if ((userLogin == null || userLogin == "") && curPageStr != "login.html") {
        showCommonPromptPopup("请先登录！");
        $.go("login.html");
        return;
    }

    if (curPageStr.toLowerCase() != "login.html" && curPageStr.toLowerCase() != "print.html" && curPageStr.toLowerCase() != "printrepairjs.html") {
        if (navigator.onLine) {
            ajax("/Message/GetMessageCount", { storeID: userLogin.StoreID }, {
                async: false, callback: function (res) {
                    if (!res.IsPass) {
                        return;
                    }
                    msgCount = res.MSG;
                }
            });
        }
        ajax("/Login/GetMenuList", { userID: userLogin.UserID }, {
            async: false, callback: function (res) {
                if (!res.IsPass) {
                    //showCommonErrorPopup(res.MSG);
                    return;
                }

                var ret = res.Obj;
                if (ret != null && ret.length > 0) {
                    var ul = $('<ul class="clearFix"></ul>');
                    for (var i = 0; i < ret.length; i++) {
                        var li = $('<li></li>').appendTo(ul);
                        if (ret[i].DispName == "销售管理")
                            li.addClass("saleMan");
                        else if (ret[i].DispName == "会员管理")
                            li.addClass("memberMan");
                        else if (ret[i].DispName == "首页")
                            li.addClass("home");
                        //else if (ret[i].DispName == "消息管理")
                        //    li.addClass("message");
                        //var a = $('<a></a>').html(res[i].DispName).prop('href', res[i].Path).appendTo(li);
                        if (ret[i].DispName != "消息管理") {
                            var a = $('<a href=\"javascript:;\"></a>').html(ret[i].DispName).attr('itemid', ret[i].Path).appendTo(li);
                        }

                    }
                    var settingLi = $("<li class=\"shop\"><a href=\"javascript:;\">门店管理</a></li>").appendTo(ul);

                    for (var i = 0; i < ret.length; i++) {
                        var li = $('<li></li>').appendTo(ul);
                        if (ret[i].DispName == "消息管理") {
                            li.addClass("message");
                            var msgCountStr = "";
                            if (msgCount > 0)
                                msgCountStr = msgCount.toString();
                            //else
                            //    msgCountStr = "";
                            var a = $('<a href=\"javascript:;\" onclick="readMessage()"></a>').html('<b>' + msgCountStr + '</b>' + ret[i].DispName).attr('itemid', ret[i].Path).appendTo(li);

                        }
                    }

                    $("header nav").html(ul);
                    $("header nav li").click(function () {
                        if (!isIE10()) {
                            showCommonPromptPopup("请用IE浏览器访问，且IE版本需要10及以上！");
                            $.go("Welcome.html");
                            return;
                        }

                        if ($(this).attr("class") == "shop") {
                            showUpdateLoginPassPopup();
                            return true;
                        }
                        else {
                            var pageStr = $(this).find("a").attr("itemid").trim();
                            if (!navigator.onLine) {//离线 
                                var offlinePageStr = pageStr.toLocaleLowerCase().trim();
                                if (offlinePageStr != "addmember.html" && offlinePageStr != "sales.html" && offlinePageStr != "welcome.html" && offlinePageStr != "member.html") {
                                    showCommonPromptPopup("离线不能访问!");
                                    return;
                                }
                            }

                            if (isOnDutyCommon() && pageStr.toLowerCase() != "welcome.html") {
                                showCommonErrorPopup("请先当班！");
                                return;
                            }
                            if (library.isNull(pageStr)) {
                                showCommonPromptPopup("无访问权限！");
                                return;
                            }
                            var flag = AuthVisitPage(pageStr);
                            if (!flag)
                                return;
                            else
                                $.go(pageStr);
                        }
                    });
                }
            }
        });
        ajax("/Login/GetCurPageList", { pageStr: curPageStr }, {
            async: false, callback: function (res) {
                if (!res.IsPass) {
                    //showCommonErrorPopup(res.MSG);
                    return;
                }
                var ret = res.Obj;
                if (ret != null && ret.length > 0) {
                    //var ul = $('<ul class="maxh"></ul>');
                    var ulHtml = "";
                    for (var i = 0; i < ret.length; i++) {
                        if (ret[i].Path.toLowerCase() == curPageStr.toLowerCase()) {
                            ulHtml += "<li class='cur'>";

                            ulHtml += "<a href='javascript:;'  calss='cur'>" + ret[i].DispName + "</a>";
                            ulHtml += "</li>";
                        }
                        else {
                            ulHtml += "<li>";
                            if (ret[i].Path == "" || ret[i].Path == null)
                                ulHtml += "<a href='javascript:;' onclick=\"funcMenuPopup('" + ret[i].DispName + "')\"  calss='cur'>" + ret[i].DispName + "</a>";
                            else
                                ulHtml += "<a href='" + ret[i].Path + "'  calss='cur'>" + ret[i].DispName + "</a>";
                            ulHtml += "</li>";
                        }

                        //ulHtml += "<a href='" + res[i].Path + "'  calss='cur'>" + res[i].DispName + "</a>";
                        //ulHtml += "</li>";

                        //var li = $('<li></li>').appendTo(ul);
                        //if (res[i].Path == curPageStr)
                        //    li.addClass("cur");
                        //var a = $('<a></a>').html(res[i].DispName).prop('href', res[i].Path).appendTo(li);

                    }
                    $("aside ul").html(ulHtml);
                    $("aside li").click(function () {
                        var pageStr = $(this).find("a").attr("href");
                        if (pageStr == "javascript:;")
                            return true;
                        if (!navigator.onLine) {//离线 
                            var offlinePageStr = pageStr.toLocaleLowerCase().trim();
                            if (offlinePageStr != "addmember.html" && offlinePageStr != "sales.html" && offlinePageStr != "welcome.html" && offlinePageStr != "member.html") {
                                showCommonPromptPopup("离线不能访问!");
                                return false;
                            }
                        }

                        var flag = AuthVisitPage(pageStr);
                        if (!flag)
                            return false;
                    });
                    $("header nav li").each(function (index, item) {
                        var menupage = $(this).find("a").attr("itemid");
                        if (!library.isNull(menupage)) {
                            if (menupage.toLowerCase() == ret[0].Path.toLowerCase()) {
                                $(this).find("a").addClass("cur");
                            }
                        }
                    })
                }
            }
        });

    }

});

//点击阅读推广信息按钮，把信息标记为已读
function readMessage() {
    var userLoginInfo = $.session("_loginInfo");
    ajax("/Message/ReadMessage", { storeID: userLoginInfo.StoreID }, null);
}

function logout() {
    if (navigator.onLine) {
        ajax("/Login/LogOut", null, function (res) {
            $.go("login.html");
        })
    }
    else
        $.go("login.html");
}

//获取当日登录用户信息
function GetCurLoginInfo() {
    var userLoginInfo = $.session("_loginInfo");
    if (userLoginInfo == null) {
        $.go("Login.html");
    }
    return userLoginInfo;
}

//获取当前4位流水号
function getCurStartNo() {
    var sessionStartNo = $.storage("_curStartNo");
    return sessionStartNo;
}

//重设流水号
function setCurStartNo(oldStartNo) {
    var sessionStartNoObj = $.storage("_curStartNo");
    var num = parseInt(oldStartNo) + 10000;
    num += 1;
    var startNoStr = num.toString().substr(1, 4);
    if (sessionStartNoObj != null) {
        sessionStartNoObj.StartNo = startNoStr;
    }
    $.storage("_curStartNo", sessionStartNoObj);
}

//页面跳转权限控制
function AuthVisitPage(pageStr) {
    pageStr = pageStr.substr(pageStr.lastIndexOf("/") + 1)
    var retFlag = true;
    if (pageStr == null || pageStr == "" || pageStr == "#" || pageStr.indexOf("javascript") >= 0) {
        return true;
    }
    var userAuthInfo = $.session("_userAuth");
    var userId = userAuthInfo.UserID;
    var data = { userId: userId, pageStr: pageStr };
    ajax('/Login/GetAuth', data, {
        async: false, callback: function (res) {
            if (!res.IsPass) {
                showCommonPromptPopup("无访问权限！");
                retFlag = false;
            }
            var ret = res.Obj;
            if (ret != null && ret.length > 0) {
                retFlag = true;
            }
        }
    });
    return retFlag;
}

//授权控制
function AuthorizeOperation(userCode, password, element, callback) {
    var isHaveAuthorize = false;
    var pageStr = window.location.href;
    pageStr = pageStr.substr(pageStr.lastIndexOf("/") + 1);
    var data = { userCode: userCode, password: password, pageStr: pageStr };
    ajax('/Login/AuthorizeOperation', data, {
        async: false, callback: function (res) {
            var ret = res.Obj;
            if (ret != null && ret.length > 0) {
                for (var i = 0; i < ret.length; i++) {
                    var item = ret[i];
                    if (item.ElementType == "name") {
                        if (item.SettingType == "authorize" && item.ElementValue == element && item.Path == pageStr) {
                            $("button[name='" + item.ElementValue + "']").attr("itemid", item.SettingValue);
                            if (item.SettingValue)
                                isHaveAuthorize = true;
                        }
                    }
                }
            }
        }
    });
    if (!isHaveAuthorize) {
        showCommonPromptPopup("输入账号无权限！");
        return;
    }
    callback(element);
}


//页面权限控制
function AuthVisit() {
    var userAuthInfo = $.session("_userAuth");
    if (!library.isNull(userAuthInfo)) {
        var pageStr = window.location.href;
        pageStr = pageStr.substr(pageStr.lastIndexOf("/") + 1);
        var userId = userAuthInfo.UserID;
        var data = { userId: userId, pageStr: pageStr };
        ajax('/Login/GetAuth', data, { async: false, callback: authVisitCallBack });
    }
}
//页面权限控制 回调方法
function authVisitCallBack(res) {
    var ret = res.Obj;
    if (!res.IsPass) {
        showCommonErrorPopup(res.MSG);
        return false;
    }
    if (res.IsPass && ret != null && ret.length > 0) {
        for (var i = 0; i < ret.length; i++) {
            var item = ret[i];
            if (item.ElementType == "id") {
                if (item.SettingType == "class") {
                    if (item.SettingValue == "hide")
                        $("#" + item.ElementValue).remove();
                    else
                        $("#" + item.ElementValue).addClass(item.SettingValue);
                } else {
                    $("#" + item.ElementValue).attr(item.SettingType, item.SettingValue);
                }
            }
            if (item.ElementType == "css") {
                if (item.SettingType == "class") {
                    if (item.SettingValue == "hide")
                        $("." + item.ElementValue).remove();
                    else
                        $("." + item.ElementValue).addClass(item.SettingValue);
                } else {
                    $("." + item.ElementValue).attr(item.SettingType, item.SettingValue);
                }
            }
            if (item.ElementType == "name") {
                if (item.SettingType == "class") {
                    if (item.SettingValue == "hide")
                        $("." + item.ElementValue).remove();
                    else
                        $("." + item.ElementValue).addClass(item.SettingValue);
                }
                else if (item.SettingType == "authorize") {
                    $("button[name='" + item.ElementValue + "']").attr("itemid", item.SettingValue);
                }
                else {
                    $("." + item.ElementValue).attr(item.SettingType, item.SettingValue);
                }
            }
        }
    }
    else {
        var pageStr = window.location.href;
        pageStr = pageStr.substr(pageStr.lastIndexOf("/") + 1);
        if (pageStr.toLocaleLowerCase().trim() != "welcome.html")
            $.go('welcome.html');
        showCommonErrorPopup("无访问权限！");
        return false;
    }

}
function getObjectByPage(pageObject, pageSize, currentPageNum) {
    var result = new Array();
    if (pageObject == null || pageObject.length < 1) {
        return null;
    } else {
        var totalNum = pageObject.length;
        if (totalNum >= (currentPageNum + 1) * pageSize) {
            for (var i = currentPageNum * pageSize; i < (currentPageNum + 1) * pageSize ; i++) {
                result.push(pageObject[i]);
            }
            return result;
        } else if (totalNum <= (currentPageNum * pageSize)) {
            return null;
        } else {
            for (var i = currentPageNum * pageSize ; i < (currentPageNum) * pageSize + (totalNum % pageSize) ; i++) {
                result.push(pageObject[i]);
            }
            return result;
        }
    }
}
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


function showEditNumber(title, field, func) {
    $(".popupEditQtyDiscount").remove();
    var editDiv = "<div class='loadBg'><div class=\"popup popupM popupEditQtyDiscount\" style=\"display: block;\">" +
        "<div class=\"inner innerDiscountPopup\" style=\"display: block\">" +
        "<header>" +
        "<h2>" + title + "</h2>" +
        "<a class=\"close\" href=\"javascript:;\">关闭</a>" +
        "</header>" +
        "<hr>" +
        "<div class=\"promptMsg alignc\"><p ></p></div>" +
        "<div class=\"popupNew\">" +
        "<label>" + field + "</label><p class=\"inputBox\">" +
        "<input class=\"inputTS\" type=\"number\" value=\"\">" +
        "<span class=\"plus\">+</span><span class=\"minus\">-</span></p>" +
        "<p class=\"btnBox\">" +
        "<button class=\"btnBrown\" type=\"button\">确  认</button></p>" +
        "</div></div></div></div>";
    $("body").append(editDiv);
    $(".popupEditQtyDiscount .popupNew .inputTS").focus();
    var topicMsg = $(".popupEditQtyDiscount header h2").html();
    if (topicMsg.indexOf("折扣") >= 0) {
        $(".promptMsg").find("p").html("折扣输入范围(1-10)，例如：9折输入9，9.5折输入9.5");
    }
    $(".popupEditQtyDiscount .innerDiscountPopup a").bind("click", function () {
        $(".innerDiscountPopup").show().siblings().hide();
        $(".popupEditQtyDiscount").remove();
        $(".loadBg").hide();
    });

    $(".popupEditQtyDiscount .innerDiscountPopup .plus").bind("click", function () {
        var number = $(".popupEditQtyDiscount .popupNew .inputTS").val();
        if (number == "" || isNaN(number))
            number = 0;
        number = Number(number);
        number = number + 1;
        $(".popupEditQtyDiscount .popupNew .inputTS").val(number);
    });

    $(".popupEditQtyDiscount .innerDiscountPopup .minus").bind("click", function () {
        var number = $(".popupEditQtyDiscount .popupNew .inputTS").val();
        if (number == "" || isNaN(number))
            number = 0;
        number = Number(number);
        if (number > 1)
            number = number - 1;
        $(".popupEditQtyDiscount .popupNew .inputTS").val(number);
    });

    $(".popupEditQtyDiscount .popupNew .btnBrown").bind("click", function () {
        var number = $(".popupEditQtyDiscount .popupNew .inputTS").val();
        var topic = $(".popupEditQtyDiscount header h2").html();
        var qtyformat = /^[1-9]{1}[0-9]*$/;
        var disAmtFormat = /^[0-9.]*$/;
        if (topic.indexOf("数量") >= 0) {
            if (Number(number) <= 0) {
                showCommonErrorPopup("输入数值需大于0!");
                return;
            }

            if (!number.match(qtyformat)) {
                showCommonErrorPopup("输入格式错误!");
                return;
            }
        }
        else {

            if (topic.indexOf("折扣") >= 0) {
                if (Number(number) <= 0) {
                    showCommonErrorPopup("输入数值需大于0!");
                    return;
                }
                if (Number(number) > 10) {
                    showCommonErrorPopup("输入数值需小于10");
                    return;
                }
            }

            if (!number.match(disAmtFormat)) {
                showCommonErrorPopup("输入格式错误!");
                return;
            }
            if (topic.indexOf("单品调价") >= 0) {

                if (number == "" || parseInt(number) < 0) {
                    showCommonErrorPopup("输入数值需大于0!");
                    return;
                }
            }
            else {
                if (number == "" || number <= 0) {
                    showCommonErrorPopup("输入数值需大于等于0!");
                    return;
                }
            }
        }
        $(".innerDiscountPopup").hide();
        $(".popupEditQtyDiscount").hide();
        $(".loadBg").hide();
        func();
    });
}

function editNumberCallback() {
    var number = $(".popupEditQtyDiscount .popupNew .inputTS").val();
    return number;
}

//通用一般提示框
function showCommonPromptPopup(content) {
    $(".popupPrompt").remove();
    var errorDiv = "<div class='loadBg'><div class=\"popup popupS popupPrompt\">" +
        "<div class=\"inner innerPrompt\">" +
        "<header><h2>提示</h2><a href=\"javascript:;\" class=\"close\">关闭</a></header><hr />" +
        "<div class=\"error\"><p>" + content + "</p></div></div></div></div>";
    $("body").append(errorDiv);
    $(".innerPrompt").show().siblings().hide();
    $(".popupPrompt").show();

    $(".popupPrompt .innerPrompt a").bind("click", function () {
        $(".innerPrompt").hide();
        $(".loadBg").hide();
        $(".popupPrompt").remove();
        changePageFocus();
    });

}
//通用错误提示框
function showCommonErrorPopup(content) {
    $(".popupError").remove();
    var errorDiv = "<div class='loadBg'><div class=\"popup popupS popupError\">" +
        "<div class=\"inner innerError\">" +
        "<header><h2>错误提示</h2><a href=\"javascript:;\" class=\"close\">关闭</a></header><hr />" +
        "<div class=\"error\"><p>" + content + "</p></div></div></div></div>";
    $("body").append(errorDiv);
    $(".innerError").show().siblings().hide();
    $(".popupError").show();

    $(".popupError .innerError a").bind("click", function () {
        $(".innerError").hide();
        $(".loadBg").hide();
        $(".popupError").remove();
        changePageFocus();
    });

}
//通用成功提示框
function showCommonSuccessPopup(content) {
    $(".popupSuccess").remove();
    var successDiv = "<div class='loadBg'><div class=\"popup popupS popupSuccess\">" +
        "<div class=\"inner innerSuccess\">" +
        "<header><h2>成功提示</h2><a href=\"javascript:;\" class=\"close\">关闭</a></header><hr />" +
        "<div class=\"success\"><p>" + content + "</p></div></div></div></div>";
    $("body").append(successDiv);
    $(".innerSuccess").show().siblings().hide();
    $(".popupSuccess").show();


    $(".popupSuccess .innerSuccess a").bind("click", function () {
        $(".innerSuccess").hide();
        $(".loadBg").hide();
        $(".popupSuccess").remove();
        changePageFocus();
    });
}
//通用esc快捷方法
function escMethodCommon() {
    $(".popup:visible:last .close").click();
}

function redirectSaleMenu() {
    $.go("Sales.html");
}

function redirectMemberMenu() {
    $.go("Member.html");
}

//焦点切换
function changePageFocus() {
    var isExistPopup = false;
    if ($("#bg").css("display") == "none") {
        $(".loadBg").each(function (index, item) {
            if ($(this).css("display") != "none")
                isExistPopup = true;
        });
    }
    else
        isExistPopup = true;

    var curLocation = location.href.toLowerCase();
    if (curLocation.indexOf("sales.html") > 0 || curLocation.indexOf("giftsales.html") > 0 || curLocation.indexOf("presales.html") > 0) {
        if (!isExistPopup)
            $("#txtProductCode").focus();
        else {
            if ($(".popupPays .inner").size() > 0 && $(".popupPays .inner").css("display") != "none")
                $("#txtPaidMoney").focus();
        }
    }
    else if (curLocation.indexOf("returns.html") > 0) {
        if (!isExistPopup)
            $("#txtOrderCode").focus();
        else {
            if ($(".popupPays .inner").size() > 0 && $(".popupPays .inner").css("display") != "none")
                $("#txtPaidMoney").focus();
        }
    }
}

//显示授权窗口
function showAuthorizePopup(element, callback) {
    $(".popupAuthorize").remove();
    var authorizeDiv = "<div class='loadBg'><div class=\"popup popupS popupAuthorize\">" +
        "<div class=\"inner innerAuthorize\">" +
        "<header><h2>授权</h2><a href=\"javascript:;\" class=\"close\">关闭</a></header><hr /><br/>" +
        "<ul class=\"alignc\"><li><label>用户名</label>&nbsp;<input type=\"text\" class=\"inputT1\" name=\"authorizeUserCode\" required /></li><br/>" +
        "<li><label>密码</label>&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"password\" class=\"inputT1\" required name=\"authorizePassword\" /></li>" +
        "<li><p name='errorMsg'>请输入用户名密码授权</p></li>" +
        "<button type=\"button\" class=\"btnBrown\" name=\"authorize\">确定</button></ul></div></div></div>";
    $("body").append(authorizeDiv);
    $(".innerAuthorize").show().siblings().hide();
    $(".popupAuthorize").show();
    $(".popupAuthorize .innerAuthorize").find("input[name='authorizeUserCode']").focus();

    $(".popupAuthorize .innerAuthorize").find("button[name='authorize']").bind("click", function () {
        var authUserCode = $(".popupAuthorize .innerAuthorize").find("input[name='authorizeUserCode']").val();
        var authPass = $(".popupAuthorize .innerAuthorize").find("input[name='authorizePassword']").val();
        if (authUserCode == "") {
            $(".popupAuthorize .innerAuthorize").find("p[name='errorMsg']").html("用户名不能为空！");
            return;
        }
        else if (authPass == "") {
            $(".popupAuthorize .innerAuthorize").find("p[name='errorMsg']").html("密码不能为空！");
            return;
        }
        //$("body").remove(".popupAuthorize");
        $(".popupAuthorize").remove();
        $(".loadBg").hide();
        AuthorizeOperation(authUserCode, authPass, element, callback);
    });

    $(".popupAuthorize .innerAuthorize a").bind("click", function () {
        $(".popupAuthorize").remove();
        $(".loadBg").hide();
    });
}
//显示输入会员卡密码弹窗
function showMemberPassPopup(callback, memberPassword) {
    $(".popupMemberPass").remove();
    var propmsg = "输错3次卡将被锁定";
    var errorTimes = 0;

    var memmberPassDiv = $("<div class='loadBg'><div class=\"popup popupS popupMemberPass\">" +
        "<div class=\"inner innerMemberPass\" style='display:block;'>" +
        "<header><h2>会员卡密码</h2><a href=\"javascript:;\" class=\"close\">关闭</a></header><hr />" +
        "<br/><ul class=\"alignc\"><li><label>密码</label>&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"password\" class=\"inputT1\" required id='memCardPassword' name=\"memCardPassword\"/></li><br/>" +
        "<li><p name=\"errorMsg\">" + propmsg + "</p></li><br/>" +
        "<button type=\"button\" class=\"btnBrown\" name=\"memCardConfirm\">确定</button></ul></div></div></div>").appendTo("body");
    //$("body").append(memmberPassDiv);
    //$(".innerMemberPass").show().siblings().hide();
    $(".popupMemberPass").show();
    $("button[name='memCardConfirm']", memmberPassDiv).bind("click", function () {
        var memCardPass = $(".popupMemberPass .innerMemberPass").find("input[name='memCardPassword']").val();
        if (memCardPass == "") {
            $(".popupAuthorize .innerMemberPass").find("p[name='errorMsg']").html("密码不能为空！");
            return;
        }
        var inputPwd = $.md5(memCardPass);
        if (inputPwd == memberPassword) {
            callback(true);
            $('#txtProductCode').focus();
        }
        else {
            errorTimes += 1;
            if (errorTimes == 1)
                propmsg = "输错3次卡将被锁定，您还有2次机会";
            else if (errorTimes == 2)
                propmsg = "输错3次卡将被锁定，您还有1次机会";
            $(".popupMemberPass .innerMemberPass").find("p[name='errorMsg']").html(propmsg);
            if (errorTimes == 3) {
                callback(false);
                $('#txtProductCode').focus();
            }
        }
        $(".popupMemberPass").remove();
        $(".loadBg").hide();
    });
    $(".popupMemberPass .innerMemberPass a").bind("click", function () {
        $(".popupMemberPass").remove();
        $(".loadBg").hide();
        $('#txtProductCode').focus();
    });
}

//显示修改登录密码弹窗
function showUpdateLoginPassPopup() {
    $(".popupUpdateLoginPass").remove();
    var updatePassDiv = "<div class='loadBg'><div class=\"popup popupS3 popupS popupUpdateLoginPass\">" +
        "<div class=\"inner innerModifyLoginPassword\">" +
        "<header><h2>修改登录密码</h2><a href=\"javascript:;\" class=\"close\">关闭</a></header>" +
        "<hr /><ul class=\"alignc \">" +
        "<li><label>请输入原密码</label><br /><input type=\"password\" class=\"inputT1\" name=\"OrigLoginPass\" /></li>" +
        "<li><label>请输入新密码</label><br /><input type=\"password\" class=\"inputT1\" name=\"NewLoginPass\" /></li>" +
        "<li><label>请确认新密码</label><br /> <input type=\"password\" class=\"inputT1\" name=\"ConfirmLoginPass\"  /></li>" +
        "<br /><br /><br /><li><button type=\"button\" class=\"btnBrown\" name=\"btnSaveLoginPass\" >确认</button></li>" +
        "</ul></div></div></div>";
    $("body").append(updatePassDiv);
    $(".innerModifyLoginPassword").show().siblings().hide();
    $(".popupUpdateLoginPass").show();

    $(".popupUpdateLoginPass").find("button[name='btnSaveLoginPass']").bind("click", function () {
        var origPass = $(".popupUpdateLoginPass").find("input[name='OrigLoginPass']").val();
        var newPass = $(".popupUpdateLoginPass").find("input[name='NewLoginPass']").val();
        var confirmPass = $(".popupUpdateLoginPass").find("input[name='ConfirmLoginPass']").val();
        if (origPass == "") {
            showCommonErrorPopup("请输入原密码！");
            return;
        }
        if (newPass == "") {
            showCommonErrorPopup("请输入新密码！");
            return;
        }
        if (confirmPass == "") {
            showCommonErrorPopup("请输入确认密码！");
            return;
        }
        if (newPass != confirmPass) {
            showCommonErrorPopup("密码两次输入不一致！");
            return;
        }
        if (newPass == origPass) {
            showCommonErrorPopup("新密码和原密码不能相同！");
            return;
        }
        updateLoginPassword(origPass, newPass);
    });

    $(".popupUpdateLoginPass .innerModifyLoginPassword a").bind("click", function () {
        $(".innerModifyLoginPassword").hide();
        $(".popupUpdateLoginPass").remove();
        $(".loadBg").hide();
    });
}

//修改登录密码
function updateLoginPassword(origPass, newPass) {
    var userLoginInfo = GetCurLoginInfo();
    var userId = userLoginInfo.UserID;
    ajax("/Login/UpdateLoginPassword", { userId: userId, oldPass: origPass, newPass: newPass }, function (res) {
        if (res.IsPass) {
            hiddenCommonPopUp(".innerModifyLoginPassword");
            showCommonSuccessPopup(res.MSG + "请重新登录！");
            $.go("login.html");
            return;
        }
        else {
            showCommonErrorPopup(res.MSG);
            return;
        }
    })
}

function showCommonConfrimPopup(content, callback, keyId) {
    $(".confirmPopup").remove();
    var confirmDiv = "<div class='loadBg'><div class=\"popup popupS confirmPopup\">" +
        "<div class=\"inner innerConfirm\">" +
        "<header><h2>确认提示</h2><a href=\"javascript:;\" class=\"close\">关闭</a></header><hr />" +
    "<div class=\"error\"><p id=\"popErrorContent\">" + content + "</p>" +
    "<div style=\"margin-top:50px;\"><input type='button' class='btnBrown' name='confirm' value='确认'/>&nbsp;&nbsp;" +
    "<input type='button' class='btnBrown' name='cancel' value='取消'/></div></div></div></div></div>"
    $("body").append(confirmDiv);
    $(".innerConfirm").show().siblings().hide();
    $(".confirmPopup").show();

    $(".confirmPopup .innerConfirm a").bind("click", function () {
        $(".confirmPopup").remove();
        $(".loadBg").hide();
    });

    $(".confirmPopup .innerConfirm").find("input[name='confirm']").bind("click", function () {
        $(".confirmPopup").remove();
        $(".loadBg").hide();
        if (keyId == null)
            callback(true);
        else
            callback(keyId, true);
    });

    $(".confirmPopup .innerConfirm").find("input[name='cancel']").bind("click", function () {
        $(".confirmPopup").remove();
        $(".loadBg").hide();
        if (keyId == null)
            callback(false);
        else
            callback(keyId, false);
    });
}

//获取时间流水号
function getOrderCodeByTime() {
    var curdate = new Date();
    var year = curdate.getFullYear(),
        month = curdate.getMonth() + 1,
        day = curdate.getDate(),
        hour = curdate.getHours(),
        minute = curdate.getMinutes(),
        second = curdate.getSeconds(),
        millsecond = curdate.getMilliseconds();
    var monthstr = month < 10 ? "0" + month.toString() : month.toString();
    var daystr = day < 10 ? "0" + day.toString() : day.toString();
    var timeStr = year.toString() + "" + monthstr + "" + daystr + hour.toString() + minute.toString() + second.toString() + millsecond.toString();

    return timeStr;
}

//判断是否当班，未当班只能访问主页，必须先当班
function isOnDutyCommon() {
    var userLoginInfo = $.session("_loginInfo");
    if (library.isNull(userLoginInfo))
        return true;
    var retFlag = false;
    var data = { storeID: userLoginInfo.StoreID, cashboxID: userLoginInfo.CashboxID };
    ajax("/Login/IsOnDuty", data, {
        async: false, callback: function (res) {
            if (!res.IsPass) {
                retFlag = false;
            }
            else
                retFlag = true;
        }
    });
    return retFlag;
}

//清楚日历控件方法
function ComonClearCalendar() {
    $("em").bind("click", function () {
        var thisid = $(this).attr("id");
        var obj = $(this).parent().find("input");
        var size = obj.size();
        if (size > 1) {
            for (var i = 0; i < size; i++) {
                if ($(obj).eq(i).attr("id").indexOf(thisid) >= 0)
                    $(obj).eq(i).val("");
            }
        }
        else
            $(obj).val("");
    })
}

//旧卡换卡跳转
function changeCardRedirect(oldCardNo, isConfirm) {
    if (isConfirm) {
        $.session("_curEditMemberID", "");
        $.session("_curEditMemberCardNo", oldCardNo);
        $.go("MemberInfo.html");
    }
}

//开卡留资跳转
function addCardInfoRedirect(cardNo, isConfirm) {
    if (isConfirm) {
        $.go("AddMember.html");
    }
}

//获取打印模板
function getPrintTemplate(storeID, printType) {
    var data = { storeID: storeID, type: printType };
    var templatePage = "";
    ajax("/Share/GetPrintTemplate", data, {
        async: false, callback: function (res) {
            if (res.IsPass) {
                var ret = res.Obj;
                templatePage = ret.TemplatePath;
            }
        }
    });
    return templatePage;
}

//判断IE浏览器 
function isIE10() {
    //return true;
    if (!!window.ActiveXObject || "ActiveXObject" in window) {
        if (navigator.userAgent.indexOf("MSIE 6.0") > 0)
            return false;
        else if (navigator.userAgent.indexOf("MSIE 7.0") > 0)
            return false;
        else if (navigator.userAgent.indexOf("MSIE 8.0") > 0)
            return false;
        else if (navigator.userAgent.indexOf("MSIE 9.0") > 0)
            return false;

        return true;
    }
    else
        return false;
}

//判断chrome浏览器
function isChrome() {
    if (window.navigator.userAgent.indexOf("Chrome") !== -1)
        return true;
    else
        return false;
}

//是否已日结
function isCommonDailyDebit() {
    var retFlag = false;
    var curDate = new Date();
    var data = { storeID: userLoginInfo.StoreID, cashboxID: userLoginInfo.CashboxID };
    ajax("/Login/IsDailyDebit", data, {
        async: false, callback: function (res) {
            if (res.IsPass) {
                var ret = eval('(' + res.Obj + ')');
                var debitDate = ret.LastOnDutyDate; //new Date(res.Obj).format('yyyy-MM-dd');
                var currentDate = ret.CurrentDate;
                if (debitDate == currentDate) {
                    showCommonErrorPopup("已日结不能操作！");
                    retFlag = false;
                }
                else
                    retFlag = true;
            }
            else
                retFlag = true;
        }
    });
    return retFlag;
}
