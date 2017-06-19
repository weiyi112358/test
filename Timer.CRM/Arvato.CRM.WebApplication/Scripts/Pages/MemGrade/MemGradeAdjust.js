var dt_search;

$(function () {
    $("#txtStartDate, #txtEndDate").datepicker({ startDate: '0' });//
    getAllVipType();
    $("#btnSearch").click(function () {
        var qryopt = {
            vipCode: $("#txtVipCode").val(),
            name: $("#txtName").val(),
            mobileNO: $("#txtMobileNO").val(),
            plateNO: $("#txtPlateNO").val()
        };
        var err = [];
        if (!qryopt.vipCode && !qryopt.name && !qryopt.mobileNO && !qryopt.plateNO) {
            err.push({ txtStartDate: '请至少输入一个查询条件！' });
        }
        if (qryopt.plateNO && qryopt.plateNO.length<5) {
            err.push({ txtStartDate: '车牌号至少输入5位！' });
        }
        if (err.length > 0) {
            processErrs(err);
            return false;
        }

        GetMemGradeAdjust();
    });

    //保存数据
    $("#frmEditItem").submit(function (e) {
        e.preventDefault();
        if (DataValidator.form()) {
            //var corp = {
            befGrade = $("#hdbfGrade").val();
            befStartDate = $("#hdbfStartDate").val();
            befEndDate = $("#hdbfEndDate").val();

            vipCode = $("#hidMemberID").val();
            vipLevel = $("#sltLevel").val();
            updateUser = $("#hidUserID").val();
            startDate = $("#txtStartDate").val();
            endDate = $("#txtEndDate").val();
            reason = $("#txtReason").val();

            if (befGrade == vipLevel) {
                $.dialog("等级相同不能调整");
                return false;
            }
            var myDate = new Date();
            var dateNow = utility.getCurrentDate();//取当前日期
            if (startDate != dateNow && utility.compareDate(startDate, dateNow)){
                $.dialog("会员等级起始时间不能小于当前系统时间");
                return false;
            }
            //if (!utility.compareDate(startDate, endDate) || startDate == endDate) {
                
            if (utility.compareDate(endDate,startDate)) {
                $.dialog("会员等级截至时间必须大于起始时间");
                return false;
            }


           // }
            //增加
            ajax("/MemGradeAdjust/UpdateVipLevelData", { befGrade: befGrade, befStartDate: befStartDate, befEndDate: befEndDate, vipCode: vipCode, vipLevel: vipLevel, updateUser: updateUser, startDate: startDate, endDate: endDate, reason: reason }, function (res) {
                if (res.IsPass) {
                    $.colorbox.close();
                    dt_search.fnDraw();
                    $.dialog(res.MSG);
                } else { $.dialog(res.MSG); }
            });
        }
    })
});

//验证数据
var DataValidator = $("#frmEditItem").validate({
    //onSubmit: false,
    rules: {
        sltLevel:{
            required: true
        },
        txtStartDate: {
            required: true
        },
        txtEndDate: {
            required: true
        },
        txtReason: {
            maxlength: 100
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

function GetMemGradeAdjust() {
    if (!dt_search) {
        dt_search = $('#dt_MemGradeAdjust').dataTable({
            sAjaxSource: '/MemGradeAdjust/GetMemGradeAdjust',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 5,
            aoColumns: [
                { data: "DataGroupName", title: "所属群组", sortable: false },
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerName", title: "会员名称", sortable: false },
                { data: "CustomerMobile", title: "手机号码", sortable: false },
                { data: "OptionText", title: "当前等级", sortable: false },
                { data: "MemberLevelStartDate", title: "会员等级起始时间", sortable: false },
                { data: "MemberLevelEndDate", title: "会员等级截止时间", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        return "<button class=\"btn\" id=\"btnModify\"  onclick=\"edit('" + obj.MemberID + "')\">编辑</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'vipCode', value: $("#txtVipCode").val() });
                d.push({ name: 'name', value: $("#txtName").val() });
                d.push({ name: 'mobileNO', value: $("#txtMobileNO").val() });
                d.push({ name: 'plateNO', value: $("#txtPlateNO").val() });
            }
        });
    }
    else {
        dt_search.fnDraw();
    }
}

//编辑
function edit(carId) {
    $("#txtReason").val('');

    var userID = $("#hidUserID").val();
    var userName = $("#hidUserName").val();
    ajax('/MemGradeAdjust/GetItemById', { itemId: carId }, function (res) {
        $("#hdbfGrade").val(res.CustomerLevel);
        $("#hdbfStartDate").val(res.MemberLevelStartDate.length > 10 ? res.MemberLevelStartDate.substring(0, 10) : res.MemberLevelStartDate);
        $("#hdbfEndDate").val(res.MemberLevelEndDate.length > 10 ? res.MemberLevelEndDate.substring(0, 10) : res.MemberLevelEndDate);
        $("#hidMemberID").val(carId);

        $("#cardNo").html(res.MemberCardNo);
        $("#name").html(res.CustomerName);
        $("#curLGrade").html(res.OptionText);
        $("#sltLevel").val(res.CustomerLevel);
        $("#txtApplyPerson").html(userName);
        $("#txtStartDate").val(res.MemberLevelStartDate.length > 10 ? res.MemberLevelStartDate.substring(0, 10) : res.MemberLevelStartDate);
        $("#txtEndDate").val(res.MemberLevelEndDate.length > 10 ? res.MemberLevelEndDate.substring(0, 10) : res.MemberLevelEndDate);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#editItem_dialog",
        inline: true
    });
}

// 获得用户渠道列表
function getAllVipType() {
    ajax('/MemGradeAdjust/GetAllVipType', null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#sltLevel').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#sltLevel').append(opt);
        }
    });
}
