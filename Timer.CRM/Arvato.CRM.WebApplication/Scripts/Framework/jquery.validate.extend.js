jQuery.extend(jQuery.validator.messages, {
    required: "必选字段",
    remote: "请修正该字段",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: jQuery.validator.format("请输入一个 长度最多是 {0} 的字符串"),
    minlength: jQuery.validator.format("请输入一个 长度最少是 {0} 的字符串"),
    rangelength: jQuery.validator.format("请输入 一个长度介于 {0} 和 {1} 之间的字符串"),
    range: jQuery.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
    max: jQuery.validator.format("请输入一个最大为{0} 的值"),
    min: jQuery.validator.format("请输入一个最小为{0} 的值")
});

// 邮政编码验证   
jQuery.validator.addMethod("isZipCode", function (value, element) {
    var tel = /^[0-9]{6}$/;
    return this.optional(element) || (tel.test(value));
}, "请正确填写您的邮政编码");

// 手机号码验证   
jQuery.validator.addMethod("isMobileNo", function (value, element) {
    var tel = /^1[3|4|5|7|8][0-9]\d{8}$/;
    return this.optional(element) || (tel.test(value));
}, "请正确填写您的手机号码");

// 用户密码验证   
jQuery.validator.addMethod("noWhiteSpace", function (value, element) {
    var tel = /^\S+$/;
    return this.optional(element) || (tel.test(value));
}, "密码中不能包含空格");

//用户登录名验证
jQuery.validator.addMethod("isLoginName", function (value, element) {
    var tel = /^[a-zA-z][0-9a-zA-Z]+$/;
    return this.optional(element) || (tel.test(value));
}, "登录名只能包含字母和数字，且由字母开头");

// 字符串中是否有空格验证   
jQuery.validator.addMethod("noWhiteSpaceStr", function (value, element) {
    var tel = /^\S+$/;
    return this.optional(element) || (tel.test(value));
}, "字段中不能包含空格");

// 字符串中只能是数字和字母   
jQuery.validator.addMethod("isOnlyLN", function (value, element) {
    var tel = /^[0-9a-zA-z][0-9a-zA-Z]+$/;
    return this.optional(element) || (tel.test(value));
}, "字段中只能包含字母和数字");

// 字符串中只能是数字N/字母L/汉字C
jQuery.validator.addMethod("isOnlyLNC", function (value, element) {
    var tel = /^[0-9a-zA-z\u4e00-\u9fa5][0-9a-zA-Z\u4e00-\u9fa5]+$/;
    return this.optional(element) || (tel.test(value));
}, "字段中只能包含字母/数字/汉字");

// 字符串中只能是/字母L/汉字C
jQuery.validator.addMethod("isOnlyLC", function (value, element) {
    var tel = /^[a-zA-z\u4e00-\u9fa5][a-zA-Z\u4e00-\u9fa5]+$/;
    return this.optional(element) || (tel.test(value));
}, "字段中只能包含字母/汉字");

// 字符串为金额类型
jQuery.validator.addMethod("isDecimal", function (value, element) {
    var tel = /^(-)?(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/;
    return this.optional(element) || (tel.test(value));
}, "请输入合法的金额");

// 字符串为小数
jQuery.validator.addMethod("isFloat", function (value, element) {
    var tel = /^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$/;
    return this.optional(element) || (tel.test(value));
}, "请输入合法的小数");

//身份证号
jQuery.validator.addMethod("isCertificate", function (value, element) {
    var tel = /^[1-9]{1}[0-9]{14}$|^[1-9]{1}[0-9]{16}([0-9]|[xX])$/;
    return this.optional(element) || (tel.test(value));
}, "请输入合法的身份证号");

//车牌号
jQuery.validator.addMethod("isVehicleNo", function (value, element) {
    var tel = /^[\u4e00-\u9fa5]{1}[A-Z]{1}[A-Z_0-9]{5}$/;
    return this.optional(element) || (tel.test(value));
}, "请输入合法的车牌号码");
//车架号
jQuery.validator.addMethod("isVIN", function (value, element) {
    var tel = /^[0-9A-Z]{17}$/;
    return this.optional(element) || (tel.test(value));
}, "请输入合法的车架号");
//不能有特殊字符
jQuery.validator.addMethod("isSb", function (value, element) {
    var pattern = new RegExp("[%`~!#$^&*=|{}':;',[].?~！#￥……&*{}【】‘；：”“'。，、？]");
    return this.optional(element) || (!pattern.test(value));
}, "不允许含有特殊字符");

//银行账号
jQuery.validator.addMethod("isBankAccount", function (value, element) {
    var tel = /^(\d{16}|\d{19})$/;
    return this.optional(element) || (tel.test(value));
}, "请输入合法的开户行账号");


