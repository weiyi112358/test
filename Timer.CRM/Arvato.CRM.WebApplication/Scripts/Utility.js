var utility = (function () {
    var readTransactionAvailable = false,
    doNothing = function () { },
    emptyArray = [],
    trim = function (string) {
        return string.replace(/^\s+/, "").replace(/\s+$/, "");
    },
    isArray = function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    },
    isUndefined = function (obj) {
        return (typeof (obj) == undefined || typeof (obj) == "undefined");
    };
    return {
        notSupported: "NotSupported",
        defaultFailureCallback: doNothing,
        trim: function (value) {
            return value.replace(/^\s+|\s+$/g, "");
        },
        isNull: function (obj) {
            if (isUndefined(obj) || obj == null) {
                return true;
            } else if (this.getObjectType(obj) == "number") {
                return false;
            } else if (this.getObjectType(obj) == "object") {
                return (obj == null || isUndefined(obj));
            } else if (this.getObjectType(obj) == "array") {
                return (obj == null || obj.length == 0 || obj == "[]");
            } else if (this.getObjectType(obj) == "string") {
                return (obj == "" || this.trim(obj.toString()) == "" || this.wordToUpper(obj.toString()) == "NULL");
            } else {
                return false;
            }
        },
        isTrue: function (obj) {
            if (isUndefined(obj)) {
                return false;
            } else {
                return (obj == true || this.wordToUpper(obj.toString()) == "TRUE");
            }
        },
        isDate: function (dateValue) {
            var format = /^(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)$/;
            //匹配2011-2-3的日期的正则表达式（带平闰年效验）：
            //((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))
            //匹配2011-02-03日期的正则表达式(带平闰年效验)：
            //^(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)$
            return dateValue.match(format);
        },
        isDateTime: function (dateValue) {
            var format = /^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$/;
            return dateValue.match(format);;
        },
        isNumber: function (dateValue) {
            var format = /^(-?\d+)(\.\d+)?$/;
            return dateValue.match(format);
        },
        isMoney: function (dateValue) {
            var format = /^(-)?(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/;
            return dateValue.match(format);
        },
        isInt: function (dateValue) {
            var format = /^(-?\d+)?$/;
            return dateValue.match(format);
        },
        isPostiveNumber: function (dateValue) {
            var format = /^[1-9]\d*$/;
            return dateValue.match(format);
        },
        isRate: function (dateValue) {
            var format = /^(?:\d{1,2}(\.\d{1,2})?|100)$/;
            return dateValue.match(format);
        },
        compareDate: function (beginDate, endDate) {
            if (this.isDate(beginDate) && this.isDate(endDate)) {
                beginDate = beginDate.replace(/-/g, "/");
                endDate = endDate.replace(/-/g, "/");
                return (new Date(beginDate) <= new Date(endDate));
            } else {
                return false;
            }
        },
        compareDateTime: function (beginDate, endDate) {
            if (this.isDateTime(beginDate) && this.isDateTime(endDate)) {
                beginDate = beginDate.replace(/-/g, "/");
                endDate = endDate.replace(/-/g, "/");
                return (new Date(beginDate) <= new Date(endDate));
            } else {
                return false;
            }
        },
        newGuid: function () {
            var guid = "";
            for (var i = 1; i <= 32; i++) {
                var n = Math.floor(Math.random() * 16.0).toString(16);
                guid += n;
                if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) {
                    //guid += "-";
                }
            }
            return guid;
        },
        floatCalculate: function (m, n, op) {
            var a = (m + ""),                                      //将m转化为字符
                b = (n + ""),                                      //将n转化为字符
                x = 1,
                y = 1,
                c = 1;
            if (a.indexOf(".") > 0) {
                x = Math.pow(10, a.length - a.indexOf(".") - 1); //m的小数位位数作为10的幂
            }
            if (b.indexOf(".") > 0) {
                y = Math.pow(10, b.length - b.indexOf(".") - 1); //n的小数位位数作为10的幂
            }

            switch (op) {
                case '+':
                case '-':
                    c = Math.max(x, y);
                    m = Math.round(m * c);
                    n = Math.round(n * c);
                    break;
                case '*':
                    c = x * y
                    m = Math.round(m * x);
                    n = Math.round(n * y);
                    break;
                case '/':
                    c = Math.max(x, y);
                    m = Math.round(m * c);
                    n = Math.round(n * c);
                    c = 1;
                    break;
            }
            return eval("(" + m + op + ' ' + n + ")/" + c);
        },
        wordToLower: function (obj) {
            return obj.toLowerCase();
        },
        wordToUpper: function (obj) {
            return obj.toUpperCase();
        },
        getCurrentDate: function () {
            return this.changeDateFormat(new Date());
        },
        getCurrentTime: function () {
            return this.changeDateTimeFormat(new Date());
        },
        getUrlParameters: function () {
            var vars = [],
                hash,
                hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        },
        changeTimeFormat: function (time, type) {
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
        },
        changeDateTimeFormat: function (date) {
            date = new Date(date);
            if (date && date != '') {
                var year = date.getFullYear(),
                    month = date.getMonth() + 1,
                    day = date.getDate(),
                    hour = date.getHours(),
                    min = date.getMinutes(),
                    sec = date.getSeconds();
                if (month < 10) {
                    month = "0" + month;
                }
                if (day < 10) {
                    day = "0" + day;
                }
                if (hour < 10) {
                    hour = "0" + hour;
                }
                if (min < 10) {
                    min = "0" + min;
                }
                if (sec < 10) {
                    sec = "0" + sec;
                }
                return year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;
            }
        },
        changeDateFormat: function (date) {
            date = new Date(date);
            if (date && date != '') {
                var year = date.getFullYear(),
                    month = date.getMonth() + 1,
                    day = date.getDate(),
                    hour = date.getHours(),
                    min = date.getMinutes(),
                    sec = date.getSeconds();
                if (month < 10) {
                    month = "0" + month;
                }
                if (day < 10) {
                    day = "0" + day;
                }
                if (hour < 10) {
                    hour = "0" + hour;
                }
                if (min < 10) {
                    min = "0" + min;
                }
                if (sec < 10) {
                    sec = "0" + sec;
                }
                return year + "-" + month + "-" + day;
            }
        },
        changeBackStageDateFormat: function (date) {
            date = new Date(parseInt(date.replace("/Date(", "").replace(")/", "").replace(/-/g, "/"), 10));
            if (date && date != '') {
                var year = date.getFullYear(),
                    month = date.getMonth() + 1,
                    day = date.getDate(),
                    hour = date.getHours(),
                    min = date.getMinutes(),
                    sec = date.getSeconds();
                if (month < 10) {
                    month = "0" + month;
                }
                if (day < 10) {
                    day = "0" + day;
                }
                if (hour < 10) {
                    hour = "0" + hour;
                }
                if (min < 10) {
                    min = "0" + min;
                }
                if (sec < 10) {
                    sec = "0" + sec;
                }
                return year + "-" + month + "-" + day;
            }
        },
        getObjectType: function (obj) {
            if (!(obj.constructor.toString().indexOf("Object") < 0)) {
                return "object";
            } else if (!(obj.constructor.toString().indexOf("Array") < 0)) {
                return "array";
            } else if (!(obj.constructor.toString().indexOf("Function") < 0)) {
                return "function";
            } else if (!(obj.constructor.toString().indexOf("String") < 0)) {
                return "string";
            } else if (!(obj.constructor.toString().indexOf("Number") < 0)) {
                return "number";
            } else if (!(obj.constructor.toString().indexOf("Boolean") < 0)) {
                return "boolean";
            } else if (!(obj.constructor.toString().indexOf("RegExp") < 0)) {
                return "regexp";
            } else if (!(obj.constructor.toString().indexOf("Date") < 0)) {
                return "date";
            } else if (!(obj.constructor.toString().indexOf("Function") < 0)) {
                return "function";
            }
        },
        getUrlParameters: function () {
            var vars = [],
                hash,
                hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        },
        setCookie: function (name, value, expiredays) {
            var exp = new Date();
            exp.setTime(exp.getTime() + expiredays * 24 * 60 * 60 * 1000);
            document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
        },
        getCookie: function (cookieName) {
            var allCookie = document.cookie,
                cookiePos = allCookie.indexOf(cookieName);
            if (cookiePos != -1) {
                cookiePos += cookieName.length + 1;
                var cookieEnd = allCookie.indexOf(";", cookiePos);
                if (cookieEnd == -1) {
                    cookieEnd = allCookie.length;
                }
                var value = unescape(allCookie.substring(cookiePos, cookieEnd));
            }
            return value;
        },
        //验证手机号码格式
        checkIdentityCardNo: function (sValue) {
            if (sValue == undefined || sValue == null) {
                return false;
            }
            if (!(/^\d{18}|\d{15}$/.test(sValue))) {
                return true;//非法
            } else return false;//正确
        },
        //验证手机号码格式
        checkMobile: function (sValue) {
            if (sValue == undefined || sValue == null) {
                return false;
            }
            if (!(/^(13[0-9]|15[012356789]|17[0678]|18[0-9]|14[57])[0-9]{8}$/.test(sValue))) {
                return true;//非法
            } else return false;//正确
        },
        //验证邮件格式
        checkEmail: function (sValue) {
            if (sValue == undefined || sValue == null) {
                return false;
            }
            var regu = "^(([0-9a-zA-Z]+)|([0-9a-zA-Z]+[_.0-9a-zA-Z-]*[0-9a-zA-Z-]+))@([a-zA-Z0-9-]+[.])+([a-zA-Z]|net|NET|asia|ASIA|com|COM|gov|GOV|mil|MIL|org|ORG|edu|EDU|int|INT|cn|CN|cc|CC)$";
            var re = new RegExp(regu);
            if (sValue.search(re) == -1) {
                return true; //非法
            }
            return false;//正确

        }
    }
})();