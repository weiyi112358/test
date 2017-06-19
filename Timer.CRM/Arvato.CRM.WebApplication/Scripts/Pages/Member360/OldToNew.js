$(function () {

    //dtCardlistLoad();

    $('#search').submit(function (e) {
        e.preventDefault();
        if (validate(this)) {
            if ($("#MemberCardNo").val() == "" && $("#CustomerName").val() == "" && $("#CustomerMobile").val() == "" && $("#CarNo").val() == "") {
                $.dialog("请至少输入一个查询条件!");
            }
            else {
                dtCardlistLoad();
            }
        }
    });

    $("#btnEnter").click(function () {
        var reason = $("#txtReason").val().length;
        if (reason > 0 && reason <= 50) {
            saveData();
        }
        else {
            $.dialog("请输入调整原因，且长度不超过50个字符!");
        }
    });

});



var dtCardlist;
function dtCardlistLoad() {
    if (dtCardlist) {
        dtCardlist.fnDraw();
    } else {

        dtCardlist = $('#dt_Cardlist').dataTable({
            sAjaxSource: '/Member360/GetCardByCardChange',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            //aaSorting: [[4, "desc"]],
            aoColumns: [
                    { data: "CardNo", title: "会员卡号", sortable: false },
                    //{ data: "CustomerStatusText", title: "会员状态", sortable: false },
                    { data: "Str_Attr_3", title: "姓名", sortable: false },
                    { data: "Str_Attr_4", title: "手机", sortable: false },
                    {
                        data: 'Lock', title: "是否锁定", sortable: false, render: function (obj) {
                            return obj == null ? "未锁定" : obj == true ? "锁定" : "未锁定";
                        }
                    },
                    {
                        data: 'Active', title: "是否失效", sortable: false, render: function (obj) {
                            return obj == null ? "有效" : (obj == true ? "有效" : "失效");
                        }
                    },
                    {
                        data: null, title: "操作", sortable: false, render: function (obj) {
                            return "<button class=\"btn\" id=\"btnActive\" onclick=\"cardChange('" + obj.MemberID + "','" + obj.Str_Attr_3 + "','" + obj.CardNo + "')\">换卡</button>";
                        }
                    }
            ],
            fnFixData: function (d) {
                d.push({ name: 'cardNo', value: $("#MemberNo").val() });
                d.push({ name: 'memName', value: $("#CustomerName").val() });
                d.push({ name: 'memMobile', value: $("#CustomerMobile").val() });
                d.push({ name: 'vehicleNo', value: $("#CarNo").val() });
            }
        });

    }
}


//显示弹出层
function cardChange(id, name, no) {
    //清空数据
    $("#txtNewCardNo").val('');
    $("#txtReason").val('');

    $("#hdMemId").val(id);
    $("#txtName").val(name);
    $("#txtOldCardNo").val(no);

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

//保存信息
function saveData() {
    var newNo = $("#txtNewCardNo").val();
    if (newNo == "") {
        $.dialog("新卡号不能为空");
        return;
    }
    //if (utility.isNumber(newNo)) {
    //    $.dialog("新卡号格式不正确");
    //    return;
    //}
    var list = {
        MemberId: $("#hdMemId").val(),
        CardNo: encode($("#txtNewCardNo").val()),
        OldCardNo: $("#txtOldCardNo").val(),
        Remark: encode($("#txtReason").val()),
    }
    var postUrl = "/Member360/SaveCardChange";
    ajax(postUrl, { card: list }, saveCallBack);
}
function saveCallBack(data) {
    if (data.IsPass) {
        $.dialog("换卡成功");
        //重新绑定
        dtCardlist.fnDraw(false);
        //清空数据并隐藏
        $("#hdMemId").val('');
        $("#txtNewCardNo").val('');
        $("#txtOldCardNo").val('');
        $("#txtName").val('');

        $.colorbox.close();
    } else {
        $.dialog(data.Obj);
    }
}


