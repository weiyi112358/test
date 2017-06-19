$(function () {

    //dtCardlistLoad();

    $('#search').submit(function (e) {
        e.preventDefault();
        if (validate(this)) {
            if ($("#txtCardNos").val() == "" && $("#txtPhone").val() == "" && $("#txtMem").val() == "" && $("#txtIdNum").val() == "") {
                $.dialog("请至少输入一个查询条件!");
            }
            else {
                dtCardlistLoad();
            }
        }
    });

    $("#btnLock").click(function () {
        //var reason = $("#txtReason").val().length;
        //if (reason > 0 && reason <= 50) {
            saveLockList();
        //}
        //else {
            //$.dialog("请输入调整原因，且长度不超过50个字符!");
        //}
    });
    $("#btnSave").click(function () {
        //var reason = $("#txtReason").val().length;
        //if (reason > 0 && reason <= 50) {
        savePrepare();
        //}
        //else {
        //    $.dialog("请输入调整原因，且长度不超过50个字符!");
        //}

    });
});



var dtCardlist;
function dtCardlistLoad() {
    if (dtCardlist) {
        dtCardlist.fnDraw();
    } else {

        dtCardlist = $('#dt_Cardlist').dataTable({
            sAjaxSource: '/Member360/SearchCardUnLockList',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            //aaSorting: [[4, "desc"]],
            aoColumns: [
                //{ data: 'LogID', title: "规则名称", sortable: false },
                { data: 'CardNo', title: "会员卡号", sortable: false },
                { data: 'Str_Attr_3', title: "会员姓名", sortable: false },
                { data: 'Str_Attr_4', title: "手机号", sortable: true },
                {
                    data: 'Lock', title: "是否锁定", sortable: false, render: function (obj) {
                        return obj == true ? "锁定" : "未锁定";
                    }
                },
                {
                    data: 'Active', title: "是否失效", sortable: false, render: function (obj) {
                        return obj == true ? "有效" : "失效";
                    }
                },
                //{
                //    data: null, title: "状态", sortable: false, render: function (obj) {
                //        return obj.isConfirm_Unlock == true ? '已审核' : '未审核'
                //    }
                //},
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        //return obj.Active == false && obj.Lock == true ? "<button class=\"btn\" id=\"btnActive\" onclick=\"viewDetail('" + obj.MemberID + "')\">查看明细</button> <button class=\"btn\" id=\"btnLock\" onclick=\"showCardLock('" + obj.CardNo + "','" + obj.Str_Attr_3 + "','" + obj.Active + "','" + obj.Lock + "','" + obj.MemberID + "')\">卡解挂</button> " : "<button class=\"btn\" id=\"btnActive\" onclick=\"viewDetail('" + obj.MemberID + "')\">查看明细</button> "
                        if (obj.Active == false && obj.Lock == true && obj.isConfirm == null)
                            return  "<button class=\"btn\" id=\"btnActive\" onclick=\"viewDetail('" + obj.MemberID + "')\">查看明细</button> <button class=\"btn\" id=\"btnLock\" onclick=\"showCardLock('" + obj.CardNo + "','" + obj.Str_Attr_3 + "','" + obj.Active + "','" + obj.Lock + "','" + obj.MemberID + "')\">卡解挂</button> " 
                        else if (obj.Active == false && obj.Lock == true && obj.isConfirm == false)
                            return "<button class=\"btn\" id=\"btnActive\" onclick=\"viewDetail('" + obj.MemberID + "')\">查看明细</button> <button class=\"btn\" id=\"btnCheck\" onclick=\"lockCheck('" + obj.CardNo + "')\">审核</button> "
                        else
                            return "<button class=\"btn\" id=\"btnActive\" onclick=\"viewDetail('" + obj.MemberID + "')\">查看明细</button>"
                    }
                },
            ],
            fnFixData: function (d) {
                d.push({ name: 'cardNo', value: $("#txtCardNos").val() });
                d.push({ name: 'phone', value: $("#txtPhone").val() });
                d.push({ name: 'mem', value: $("#txtMem").val() });
                d.push({ name: 'idNum', value: $("#txtIdNum").val() });
            }
        });

    }
}


//显示弹出层
function showCardLock(cardNo, name, active, lock, memid) {
    //var aData = dtCardlist.fnGetData(nTr);

    if (lock == "true" && active == "false") {
        $("#name").html(name);
        $("#cardNo").html(cardNo);
        $("#lockBefore").html("锁定");
        $("#lockAfter").html("未锁定");
        $("#hdActive").val(active);
        $("#hdMemId").val(memid);
        // $("#detail").show();
        $("#txtDName").val("");
        $("#txtDId").val("");
        $("#txtDPhone").val("");

        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#table_card",
            inline: true,
            opacity: '0.3',
            onComplete: function () {
            }
        });
    }
    else {
        $.dialog("该卡状态不能解挂!");
    }
}

//cardPrepare保存
function savePrepare() {
    var cid = $("#cardNo").html();
    var active;

    if ($("#hdAvai").val() == "已开") {
        active = true;
    }
    else {
        active = false;
    }
    var list = {
        cardNumber: cid,
        cardHolder: $("#hdMemId").val(),
        agent: $("#txtDName").val(),
        agentCertificateNo: $("#txtDId").val(),
        agentMobile: $("#txtDPhone").val(),
    }
    var postUrl = "/Member360/LockAddCardPrepare";
    ajax(postUrl, { card: list,type:"unlock" }, saveCallBack);
}


//保存信息
function saveLockList() {
    var cid = $("#cardNo").html();
    var active;

    if ($("#hdAvai").val() == "已开") {
        active = true;
    }
    else {
        active = false;
    }
    var mol = {
        cardNumber: cid,
        cardHolder: $("#hdMemId").val(),
        agent: $("#txtDName").val(),
        agentCertificateNo: $("#txtDId").val(),
        agentMobile: $("#txtDPhone").val(),
        serviceCharge: $("#txtMoney").val(),
    }
    var list = {
        CardNo: cid,
        MemberId: $("#hdMemId").val(),
        agent: $("#txtDName").val(),
        agentCertificateNo: $("#txtDId").val(),
        agentMobile: $("#txtDPhone").val(),
        serviceCharge: $("#txtMoney").val(),
        ChangeType: "2"
    }
    var postUrl = "/Member360/LockAddAndCheck";
    ajax(postUrl, { cCard: list, pCard: mol,type:"unlock" }, saveCallBack);
}
function saveCallBack(data) {
    if (data.IsPass) {
        $.dialog("保存或解挂成功");
        //重新绑定
        dtCardlist.fnDraw(false);
        //清空数据并隐藏
        $("#name").html("");
        $("#cardNo").html("");
        $("#lockBefore").html("");
        $("#lockAfter").html("");
        $("#hdActive").val("");
        $("#hdAvai").val("");
        $("#hdMemId").hide();

        $.colorbox.close();
    } else {
        $.dialog(data.Obj);
    }
}

function lockCheck(cardNo) {
    var list = {
        cardNo: cardNo,
        MemberId: $("#hdMemId").val(),
    }
    var postUrl = "/Member360/CardLockCheck";
    ajax(postUrl, { card: list, type: "unlock" }, function (data) {
        if (data.IsPass) {
            $.dialog("审查成功");
            dtCardlist.fnDraw(false);
        }
        else {
            $.dialog(data.Obj);
        }
    });
}


//弹出会员基础信息层 table_dialog
function viewDetail(data) {
    var mid = data;
    $("#txtCardNo").val("");
    $("#txtName").val("");
    $("#txtMobile").val("");
    $("#txtEmail").val("");
    $("#txtCorp").val("");
    $("#txtStore").val("");
    //$("#txtCash1").val("");
    //$("#txtCash2").val("");
    //$("#txtPoint1").val("");
    $("#txtRegister").val("");
    $("#txtIDType").val("");
    $("#txtID").val("");
    if (mid != 0) {
        getMem(mid);
    }
    else {
        $.dialog("卡信息不存在!");
    }

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        href: "#table_dialog",
        inline: true,
        opacity: '0.3',
        onComplete: function () {
        }
    });
}

function getMem(uid) {
    postUrl = "/Member360/GetMemberInfoByMid";
    ajax(postUrl, { mid: uid }, getMemCallBack);
}
function getMemCallBack(data) {
    if (data.IsPass) {
        var mem = data.Obj[0];
        $("#txtCardNo").val(mem.MemberCardNo);
        $("#txtName").val(mem.CustomerName);
        $("#txtMobile").val(mem.CustomerMobile);
        $("#txtEmail").val(mem.CustomerEmail);
        //$("#txtCorp").val(mem.CorpName);
        $("#txtStore").val(mem.RegisterStore);
        //$("#txtCash1").val(mem.Cash1);
        //$("#txtCash2").val(mem.Cash2);
        //$("#txtPoint1").val(mem.Point1);
        $("#txtRegister").val(mem.RegisterDate);
        $("#txtIDType").val(mem.CertificateType);
        $("#txtID").val(mem.CertificateNo);
    }
}

