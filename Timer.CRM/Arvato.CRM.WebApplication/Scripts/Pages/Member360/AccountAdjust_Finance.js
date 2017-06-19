var dtStoreData,tableVehcileInfo;
$(function () {
    $("#txtStartDate,#txtEndDate").datepicker();//{ autoclose: true }
    //搜索会员
    $("#btnSearch").click(function () {
        
        if (checkStr($("#txtCardNo").val())) {
            $.dialog("会员卡号中不能包含特殊字符！");
            return;
        }
        if (checkStr($("#txtName").val())) {
            $.dialog("姓名中不能包含特殊字符！");
            return;
        }

        LoadStoreValueRecord()
    });
    $("#txtStatus").val('1');
});



function LoadStoreValueRecord() {
    if (!dtStoreData) {
        dtStoreData = $('#dt_StoreData').dataTable({
            sAjaxSource: '/Member360/GetAccountChangeRecord_Finance',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aaSorting: [[5, "desc"]],
            aoColumnDefs: [
                //{ "bSearchable": false, "bVisible": false, "aTargets": [ 2 ] },
                //{ "bVisible": false, "aTargets": [ 0 ] }
            ] ,
            aoColumns: [
                
                { data: "MemberCardNo", title: "会员卡号", sortable: false },
                { data: "CustomerName", title: "会员姓名", sortable: false },
                { data: "Mobile", title: "手机号", sortable: false },
                //{ data: "PlateNumVehicle", title: "车牌号", sortable: false },
                { data: "ActChangeValue", title: "调整金额", sortable: false },

                { data: "ActChangeReason", title: "调整原因", sortable: false },
                { data: "AddedDate", title: "添加时间", sortable: true },
                {
                    data: "StatusChange", title: "审批状态", sortable: false, render: function (obj) {
                        if (obj == '1') return "待审批"; else return "已审批";
                    }
                },
                {
                    data: null, title: "操作", sortable: false, render: function (obj) {
                        if (obj.StatusChange == "1")
                            return "<button class=\"btn\" onclick=\"vehcile('" + obj.MemberID + "')\">查看车辆</button><button class=\"btn btn-danger\" onclick=\"Inactive(" + obj.TradeID + ")\">审批</button>";
                        else
                            return "<button class=\"btn\" onclick=\"vehcile('" + obj.MemberID + "')\">查看车辆</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'cardNo', value: $("#txtCardNo").val() });
                d.push({ name: 'name', value: $("#txtName").val() });
                d.push({ name: 'mobile', value: $("#txtMobile").val() });
                d.push({ name: 'status', value: $("#txtStatus").val() });
                d.push({ name: 'startDate', value: $("#txtStartDate").val() });
                d.push({ name: 'endDate', value: $("#txtEndDate").val() });
            }
        });
    }
    else {
        dtStoreData.fnDraw();
    }
}
//查看车辆
function vehcile(id) {

    $("#txtHdnMid").val(id);
    loadVehcileList();

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_Vehcile",
        inline: true
    });
}
function loadVehcileList() {
    if (!tableVehcileInfo) {
        tableVehcileInfo = $('#tableVehcileInfo').dataTable({
            sAjaxSource: '/Member360/GetMemberCarInfo',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aoColumns: [
                { data: "PlateNumVehicle", title: "车牌号", sortable: false },
                { data: "VINVehicle", title: "车架号", sortable: false },
                {
                    data: "BrandName", title: "品牌", sortable: false
                },
                {
                    data: "SeriesName", title: "车系", sortable: false
                },
                {
                    data: "LevelName", title: "车型", sortable: false
                },
                {
                    data: "ColorName", title: "车辆颜色", sortable: false
                },
                { data: "TrimNameBase", title: "内饰", sortable: false },
                { data: "DriveDistinct", title: "行驶里程", sortable: false },
                {
                    data: "BuyDate", title: "购车时间", sortable: false, render: function (obj) {
                        return !obj ? "" : obj.substr(0, 10);
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'mid', value: $("#txtHdnMid").val() });
            }
        });
    }
    else {
        tableVehcileInfo.fnDraw();
    }
}

//审批记录
function Inactive(id) {
    $.dialog("确认通过审批吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/Member360/InActiveActChangeById", { itemId: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                var start = dtStoreData.fnSettings()._iDisplayStart;
                var length = dtStoreData.fnSettings()._iDisplayLength;
                dtStoreData.fnPageChange(start / length, true);
            } else { $.dialog(res.MSG); }
        });
    })
}

function GetDateStr(AddDayCount) {
    var dd = new Date();
    dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1;//获取当前月份的日期
    var d = dd.getDate();
    return y + "-" + m + "-" + d;
}

function checkStr(str) {
    var pattern = new RegExp("[%--`~!#$^&*()=|{}':;',\\[\\].<>/?~！#￥……&*（）——| {}【】‘；：”“'。，、？]");
    return pattern.test(str);
}

