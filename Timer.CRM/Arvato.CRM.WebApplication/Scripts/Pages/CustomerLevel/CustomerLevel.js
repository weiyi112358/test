var dtCustomerLevel;
$(function() {
    $("#dpbrand").change(function() {
        if ($("#dpbrand").val() == "DPAM") {
            $.getJSON("/Customerlevel/GetBizOptionCustomersLevel?customerLevel=CustomerLevel2", function(json) {
                $("#dplevel").empty();
                var opt = "";
                for (var i = 0; i < json.length; i++) {
                    opt += '<option value=' + json[i].OptionValue + '>' + json[i].OptionText + '</option>';
                }
                $("#dplevel").append(opt);
            });
        } else {
            $.getJSON("/Customerlevel/GetBizOptionCustomersLevel?customerLevel=CustomerLevel", function(json) {
                $("#dplevel").empty();
                var opt = "";
                for (var i = 0; i < json.length; i++) {
                    opt += '<option value=' + json[i].OptionValue + '>' + json[i].OptionText + '</option>';
                }
                $("#dplevel").append(opt);
            });
        }
    });
});

function loadCustomerLevelInfo() {
    if (!dtCustomerLevel) {
        dtCustomerLevel = $("#tbCustomerLevel").dataTable({
            sAjaxSource: '/CustomerLevel/GetCustomerLevels',
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 8,
            aoColumns: [
                { data: "DataGroupID", title: "", sortable: false, bVisible: false },
                { data: "SubDataGroupName", title: "所属群组", sortable: false },
                //{ data: "BrandNameCustomerLevel", title: "所属品牌", sortable: false },
                { data: "CustomerLevelNameBase", title: "会员等级", sortable: false },
                { data: "RateCustomerLevel", title: "折扣", sortable: false },
                { data: "MaxIntergral", title: "最大抵用积分", sortable: false },
                { data: "RateMaxUse", title: "当月储值币可用比率", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false, sWidth: "10%",
                    render: function (obj) {
                        return "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"addeditCustomerLevelInfo(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'customerLevel', value: $("#dropCustomerLevel").val() });
                d.push({ name: 'brand', value: $("#dropBrand").val() });
            }
        });
    } else {
        dtCustomerLevel.fnDraw();
    }
}
//查询
$("#btnQuery").on("click", function () {
    loadCustomerLevelInfo();
});


function addeditCustomerLevelInfo(customerLevelId) {
    $("#frmEditCustomerLevel")[0].reset();
    $("#hidCustomLevelId").val(0);
    $("#txtRateMaxUse").val(0);
    if (customerLevelId != "" && customerLevelId != undefined) {
        $("#h3Head").text("修改会员等级");
        ajax("/CustomerLevel/GetCustomerLevelById", { customerLevelId: customerLevelId }, function(rst) {
            $("#hidCustomLevelId").val(rst.BaseDataID),
                $("#dpbrand").val(rst.BrandCodeCustomerLevel),
                $("#dplevel").val(rst.CustomerLevelBase),
                $("#txtRate").val(rst.RateCustomerLevel),
                $("#txtReserverdMoney").val(rst.MaxIntergral),
                $("#txtRateMaxUse").val(rst.RateMaxUse);
            if (rst.BrandCodeCustomerLevel == "DPAM") {
                $.getJSON("/Customerlevel/GetBizOptionCustomersLevel?customerLevel=CustomerLevel2", function(json) {
                    $("#dplevel").empty();
                    var opt = "";
                    for (var i = 0; i < json.length; i++) {
                        if (json[i].OptionValue == rst.CustomerLevelBase) {
                            opt += '<option value=' + json[i].OptionValue + ' selected = "selected">' + json[i].OptionText + '</option>';
                        } else {
                            opt += '<option value=' + json[i].OptionValue + '>' + json[i].OptionText + '</option>';
                        }
                    }
                    $("#dplevel").append(opt);
                });
            } else {
                $.getJSON("/Customerlevel/GetBizOptionCustomersLevel?customerLevel=CustomerLevel", function(json) {
                    $("#dplevel").empty();
                    var opt = "";
                    for (var i = 0; i < json.length; i++) {
                        if (json[i].OptionValue == rst.CustomerLevelBase) {
                            opt += '<option value=' + json[i].OptionValue + ' selected = "selected">' + json[i].OptionText + '</option>';
                        } else {
                            opt += '<option value=' + json[i].OptionValue + '>' + json[i].OptionText + '</option>';
                        }

                    }
                    $("#dplevel").append(opt);
                });
            }
        });
    } else {
        $("#h3Head").text("新增会员等级");
        $.getJSON("/Customerlevel/GetBizOptionCustomersLevel?customerLevel=CustomerLevel", function(json) {
            $("#dplevel").empty();
            var opt = "";
            for (var i = 0; i < json.length; i++) {
                opt += '<option value=' + json[i].OptionValue + '>' + json[i].OptionText + '</option>';
            }
            $("#dplevel").append(opt);
        });
    }
    showModal();
}

function deleteItem(customerLevelId) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/CustomerLevel/DeleteCustomerLevelById", { customerLevelId: customerLevelId }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dtCustomerLevel.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    });
}

//新建条目
function showModal() {
    //显示页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#EditorCustomerLevel",
        inline: true
    });
}

$(function () {
    loadCustomerLevelInfo();
});

$("#frmEditCustomerLevel").submit(function (e) {
    e.preventDefault();
    if (formValidate.form()) {
        var customLevel = {
            ID: $("#hidCustomLevelId").val(),
            DataGroupID: 0,
            CustomerLevel: $("#dplevel").val(),
            CustomerLevelName: $("#dplevel option:selected").text(),
            BrandCodeCustomerLevel: $("#dpbrand").val(),
            BrandNameCustomerLevel: $("#dpbrand option:selected").text(),
            Rate: $("#txtRate").val(),
            MaxIntergral: $("#txtReserverdMoney").val(),
            RateMaxUse: $("#txtRateMaxUse").val()
        };
        ajax("/CustomerLevel/EditCustomerLevel", customLevel, function (d) {
            if (d.IsPass) {
                $.dialog(d.MSG);
                $("#frmEditCustomerLevel")[0].reset();
                dtCustomerLevel.fnDraw();
                $.colorbox.close();
            }
            else {
                $.dialog(d.MSG);
            }
        });
    }
});

var formValidate = $("#frmEditCustomerLevel").validate({
    rules: {
        txtRate: {
            required: true,
            isDecimal: true,
            min: 0.01,
            max: 1.00
        },
        txtReserverdMoney: {
            isDecimal: true,
            min: 0
        },
        txtRateMaxUse: {
            required: true,
            isDecimal: true,
            min: 0.01,
            max: 1.00
        }
    },
    errorClass: 'error-block'
});

