var dt_Table;
var dtGoods;
var temp_ExchangeDetail = [];
var temp = []
var temp2 = [];
$(function () {

    var d = new Date();
    $("#Code").val(d.format("yyMMddhhmmssS") + Math.ceil(Math.random() * 100))
    $("#CardTypeLimit").chosen();
    $("#LevelLimit").chosen();
    $("#txtStartDate,#txtEndDate").datepicker();


    //loadCardTypeLimit();
    loadCustomerLevelLimit();
    loadGoods();

    $(".chzn_a").chosen({
        allow_single_deselect: true
    });

    $("#AcceptingUnit1").change(function () {
        $("#GoodsCode").val($("#AcceptingUnit1").val())
    })



    //加载数据表格
    
    if ($("#txtID").val() == "") {
        dt_Table = $('#dt_Table').dataTable({
            "bSort": true,   //不排序
            "bInfo": true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’

            "bLengthChange": false,//不显示 ‘显示 _MENU_ 条’
            "bPaginate": true, //显示分页信息
            "iDisplayLength": 10,

            "aaData": temp_ExchangeDetail,
            "aoColumns": [
                { "sTitle": "输入码" },
                { "sTitle": "商品代码" },
                { "sTitle": "最大兑奖次数" },
                { "sTitle": "抵扣价", "sClass": "center" },
                { "sTitle": "最小兑换单元", "sClass": "center" },
                {
                    "data": null, "sTitle": "操作", "sClass": "center", "sortable": false,
                    render: function (obj) {
                        var result = "";
                        var info = $("#txtInfo").val();
                        var id = $("#txtID").val();
                        
                        result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDeleteTemp('" + obj[0] + "')\" >删除</button>";
                        
                        return result;
                    }

                }
            ]

        });
    }

    else {
        temp_ExchangeDetail = [];
        dt_Table = $('#dt_Table').dataTable({
            sAjaxSource: '/MemberSalesPromotion/LoadRuleDetailList',
            //aaData: temp_ExchangeDetail,
            bSort: true,   //不排序
            bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
            bServerSide: true,  //每次请求后台数据
            bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
            bPaginate: true, //显示分页信息
            iDisplayLength: 10,
            aoColumns: [
                { data: 'GoodsID', sClass: "center", title: "输入码", sortable: false },
                { data: 'GoodsCode', sClass: "center", title: "商品代码", sortable: false },
                //{ data: 'GoodsName', sClass: "center", title: "商品名称", sortable: false },
                { data: 'MaxCounts', sClass: "center", title: "最大兑奖次数", sortable: false },
                { data: 'DiscountValue', sClass: "center", title: "抵扣价", sortable: false },
                { data: 'MinCounts', sClass: "center", title: "最小兑换单元", sortable: false },
                {
                    data: null, title: "操作", sClass: "center", sortable: false,
                    render: function (obj) {
                        var result = "";
                        var info = $("#txtInfo").val();
                        var id = $("#txtID").val();
                        var modelDetail = [];
                        modelDetail.push(obj.GoodsID);
                        //modelDetail.push($("#InputCode").val())
                        modelDetail.push(obj.GoodsCode);
                        modelDetail.push(obj.MaxCounts);
                        modelDetail.push(obj.DiscountValue);
                        modelDetail.push(obj.MinCounts);
                        modelDetail.push(obj.ExchangeDetailID);
                        temp_ExchangeDetail.push(modelDetail);
                        
                        if (id != "" && info != '2') {
                            result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.ExchangeDetailID + "')\" >删除</button>";
                            

                        }
                        else if (id != "" && info == '2') {

                        }

                        else {
                            result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.ID + "')\" >删除</button>";
                        }
                        return result;
                    }
                }
            ],
            fnFixData: function (d) {
                d.push({ name: 'RuleID', value: $("#txtID").val() });

            }
        });

    }

    //添加明细——弹窗
    $("#btnAdd").bind("click", function () {

        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#addDetail_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
        });

    });

    //添加明细——保存
    $("#btnDetailSave").bind("click", function () {
        
        if ($("#AcceptingUnit1").val() == "") {
            alert("请输入商品名称");
            return 
        }

        if ($("#MaxCounts").val() == "") {
            alert("请输入最大兑换次数");
            return
        }

        if ($("#DiscountValue").val() == "") {
            alert("请输入抵扣价");
            return
        }

        if ($("#MinCounts").val() == "") {
            alert("请输入最小兑换单元");
            return
        }

        

        var modelDetail = [];
        modelDetail.push($("#AcceptingUnit1").val())
        //modelDetail.push($("#InputCode").val())
        modelDetail.push($("#GoodsCode").val())
        modelDetail.push($("#MaxCounts").val())
        modelDetail.push($("#DiscountValue").val())
        modelDetail.push($("#MinCounts").val())
        temp_ExchangeDetail.push(modelDetail);
        //removeDuplicate()

        if ($("#txtID").val() != "")
        {
            strdetail = JSON.stringify(modelDetail);
            ajax("/MemberSalesPromotion/AddExchangeRuleById", { Id: $("#txtID").val(), strdetail: strdetail }, function (data) {
                
                

            });

        }





        dt_Table.fnDestroy();

        if ($("#txtID").val() == "") {

            dt_Table = $('#dt_Table').dataTable({
                "bSort": true,   //不排序
                "bInfo": true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’

                "bLengthChange": false,//不显示 ‘显示 _MENU_ 条’
                "bPaginate": true, //显示分页信息
                "iDisplayLength": 10,

                "aaData": temp_ExchangeDetail,
                "aoColumns": [
                    { "sTitle": "输入码" },
                    { "sTitle": "商品代码" },
                    { "sTitle": "最大兑奖次数" },
                    { "sTitle": "抵扣价", "sClass": "center" },
                    { "sTitle": "最小兑换单元", "sClass": "center" },
                    {
                        "data": null, "sTitle": "操作", "sClass": "center", "sortable": false,
                        render: function (obj) {
                            var result = "";
                            var info = $("#txtInfo").val();
                            var id = $("#txtID").val();

                            result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDeleteTemp('" + obj[0] + "')\" >删除</button>";

                            return result;
                        }

                    }
                ]

            });
        }
        else {
            dt_Table = $('#dt_Table').dataTable({
                sAjaxSource: '/MemberSalesPromotion/LoadRuleDetailList',
                //aaData: temp_ExchangeDetail,
                bSort: true,   //不排序
                bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                bServerSide: true,  //每次请求后台数据
                bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                bPaginate: true, //显示分页信息
                iDisplayLength: 10,
                aoColumns: [
                    { data: 'GoodsID', sClass: "center", title: "输入码", sortable: false },
                    { data: 'GoodsCode', sClass: "center", title: "商品代码", sortable: false },
                    //{ data: 'GoodsName', sClass: "center", title: "商品名称", sortable: false },
                    { data: 'MaxCounts', sClass: "center", title: "最大兑奖次数", sortable: false },
                    { data: 'DiscountValue', sClass: "center", title: "抵扣价", sortable: false },
                    { data: 'MinCounts', sClass: "center", title: "最小兑换单元", sortable: false },
                    {
                        data: null, title: "操作", sClass: "center", sortable: false,
                        render: function (obj) {
                            var result = "";
                            var info = $("#txtInfo").val();
                            var id = $("#txtID").val();
                            var modelDetail = [];
                            modelDetail.push(obj.GoodsID);
                            //modelDetail.push($("#InputCode").val())
                            modelDetail.push(obj.GoodsCode);
                            modelDetail.push(obj.MaxCounts);
                            modelDetail.push(obj.DiscountValue);
                            modelDetail.push(obj.MinCounts);
                            modelDetail.push(obj.ExchangeDetailID);
                            temp_ExchangeDetail.push(modelDetail);

                            if (id != "" && info != '2') {
                                result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.ExchangeDetailID + "')\" >删除</button>";


                            }
                            else if (id != "" && info == '2') {

                            }

                            else {
                                result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDelete('" + obj.ID + "')\" >删除</button>";
                            }
                            return result;
                        }
                    }
                ],
                fnFixData: function (d) {
                    d.push({ name: 'RuleID', value: $("#txtID").val() });

                }
            });


        }

        $("#AcceptingUnit1").val("");
        $("#MaxCounts").val("") ;
        $("#DiscountValue").val("") ;
        $("#MinCounts").val("") ;
        $.colorbox.close();
    });


    //Total Save
    $("#btnSave").bind("click", function () {

        if ($("#Code").val() == "") {
            alert("请输入单号");           
        }

        else if ($("#ExchangeType").val() == "") {
            alert("请输入兑奖类型");            
        }

        else if ($("#txtStartDate").val() == "") {
            alert("请输入开始时间");           
        }

        else if ($("#txtEndDate").val() == "") {
            alert("请输入结束时间");
        }

        else {

            var model = {
                ExchangeID: $("#txtID").val(),
                ExchangeType: $("#ExchangeType").val(),
                Code: $("#Code").val(),
                Status: $("#Status").val(),
                Remark: $("#Remark").val(),
                StartDate: $("#txtStartDate").val(),
                EndDate: $("#txtEndDate").val(),
                CardTypeLimit: null,//$("#CardTypeLimit").val(),
                LevelLimit: $("#LevelLimit").val(),
            }

            strmodel = JSON.stringify(model);
            strdetail = JSON.stringify(temp_ExchangeDetail);

            ajax('/MemberSalesPromotion/SaveRule', { strmodel: strmodel, detail: strdetail }, function (res) {
                if (res.IsPass) {
                    //$.dialog(res.MSG);
                    window.location.href = "/MemberSalesPromotion/ExchangeGoods";
                }
                else { $.dialog("新增失败") }

            })
        }

        

    })


    $("#btnPass").bind("click", function () {
        var id = $("#txtID").val();
        $.dialog("确认审核通过此商品积分兑换吗?", {
            footer: {
                closebtn: '取消',
                okbtn: '确认'
            }
        }, function () {
            ajax("/MemberSalesPromotion/ApproveExchangeRuleById", { Id: id, active: 1 }, function (data) {

                if (data.IsPass) {
                    window.location.href = "/MemberSalesPromotion/ExchangeGoods";
                }
                else {
                    $.dialog(data.MSG);
                };

            });
        });
    });

    //$("#btnReturn").bind("click", function () {
    //    ajax("/MemberSalesPromotion/TruncateTemp", null);
    //});






    if ($("#txtID").val() == "") {
        $("#hTitleName").html("添加商品积分兑换规则");
    }
    else {
        if ($("#txtInfo").val() == "1") {
            $("#hTitleName").html("编辑商品积分兑换规则");
        }
        else {
            $("#hTitleName").html("查看商品积分兑换规则");
            $("#Code").prop("readonly", "readonly");
            $("#ExchangeType").prop("disabled", "disabled");
            $("#txtStartDate").prop("disabled", "disabled");
            $("#txtEndDate").prop("disabled", "disabled");
            $("#remark").prop("readonly", "readonly");
            $("#CardTypeLimit").prop('disabled', "disabled");
            $("#LevelLimit").prop("disabled", "disabled");




            $("#btnSave").css("display", "none");
            $("#btnAdd").css("display", "none");
            $("#btnPass").css("display", "none");
            $("#aCouponNo").css("display", "none");
            $("#btnDownLoad").css("display", "none");
            $("#btnImport").css("display", "none");
            $("#btnAvgCount").css("display", "none");
        }

        ajax("/MemberSalesPromotion/GetRuleById", { id: $("#txtID").val() }, function (res) {
            var data = res[0];

            $("#Code").val(data.Code);
            $("#ExchangeType").val(data.ExchangeType);
            $("#remark").val(data.remark);
            $("#txtStartDate").val(data.StartDate.substr(0, 10));
            $("#txtEndDate").val(data.EndDate.substr(0, 10));



        });

        ajax("/MemberSalesPromotion/GetRuleLimitById", { ID: $("#txtID").val() }, function (res) {

            var customerLevel = [];
            var cardType = [];

            for (var i = 0; i < res.length; i++) {
                if (res[i].LimitType == "CustomerLevel") {
                    customerLevel.push(res[i].LimitValue);
                }
                else if (res[i].LimitType == "CardType") {
                    cardType.push(res[i].LimitValue);
                }
            }

            $("#CardTypeLimit").val(cardType);
            $("#LevelLimit").val(customerLevel);

            $("#CardTypeLimit").trigger("liszt:updated");
            $("#LevelLimit").trigger("liszt:updated");



        });






    }

    //选择购物券——弹窗
    $("#divGoodsCode").bind("click", function () {
        $("#txtGoodsCode").val("");
        $("#txtGoodsName").val("");
        $.colorbox({
            initialHeight: '0',
            initialWidth: '0',
            href: "#selectGoods_dialog",
            overlayClose: false,
            inline: true,
            opacity: '0.3',
        });
        loadGoodsInfo();
    });

    //查询商品
    $("#btnGoodsSearch").bind("click", function () {
        dtGoods.fnDraw();
    });

    

    


});


function loadCardTypeLimit() {
    ajax('/MemberSalesPromotion/LoadCardType', null, function (res) {
        if (res.length > 0) {
            var opt = "";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $("#CardTypeLimit").append(opt).chosen().trigger("liszt:updated");
        }
    })
}

function loadCustomerLevelLimit() {
    ajax('/MemberSalesPromotion/LoadCustomerLevel', null, function (res) {
        if (res.length > 0) {
            var opt = "";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].CustomerLevelBase + '>' + res[i].CustomerLevelNameBase + '</option>';
            }
            $("#LevelLimit").append(opt).chosen().trigger("liszt:updated");

        }
    })
}

//删除明细
function goDelete(detailid) {
    var id = $("#txtID").val();
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax('/MemberSalesPromotion/DeleteExchangeDetailRuleById', { detailid: detailid, id: id }, function (res) {

        })

        
        temp_ExchangeDetail = [];
        dt_Table.fnDraw();
        
    })
}


function loadGoods() {
    $('#AcceptingUnit1').empty();
    $.ajax({
        type: 'post',
        url: '/MemberSalesPromotion/LoadGoods',
        dataType: 'json',
        data: {},
        success: function (result) {
            if (result.length > 0) {
                var opt = "";
                for (var i = 0; i < result.length; i++) {
                    opt += '<option value=' + result[i].GoodsCode + '>' + result[i].GoodsName+'/'+result[i].GoodsCode + '</option>';
                }
                $('#AcceptingUnit1').append('<option value="">请选择</option>');
                $('#AcceptingUnit1').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }
            else {
                opt = "<option value=''>无</option>";
                $('#AcceptingUnit1').append(opt);
                $(".chzn_a").trigger("liszt:updated");
            }

        },
        error: function (e) {
            e.responseText;
        }
    })


}


function goDeleteTemp(code) {
    removeDuplicate()
    temp_ExchangeDetail.forEach(function (data) {
        if (data[0] == code)
        {
            temp_ExchangeDetail.remove(data);

        }

    })
    
    dt_Table.fnDestroy();
    
    dt_Table = $('#dt_Table').dataTable({
        "bSort": true,   //不排序
        "bInfo": true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’

        "bLengthChange": false,//不显示 ‘显示 _MENU_ 条’
        "bPaginate": true, //显示分页信息
        "iDisplayLength": 10,

        "aaData": temp_ExchangeDetail,
        "aoColumns": [
            { "sTitle": "输入码" },
            { "sTitle": "商品代码" },
            { "sTitle": "最大兑奖次数" },
            { "sTitle": "抵扣价", "sClass": "center" },
            { "sTitle": "最小兑换单元", "sClass": "center" },
            {
                "data": null, "sTitle": "操作", "sClass": "center", "sortable": false,
                render: function (obj) {
                    var result = "";
                    var info = $("#txtInfo").val();
                    var id = $("#txtID").val();

                    result += "<button type=\"button\" class=\"btn btn-danger btndelete\" onclick=\"goDeleteTemp('" + obj[0] + "')\" >删除</button>";

                    return result;
                }

            }
        ]

    });



}


Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }

    var a = 1;
};

function removeDuplicate() {
    temp = []
    temp2 = []
    temp_ExchangeDetail.forEach(function (data) {
        
        if (temp2.indexOf(data[5]) == -1) {
            temp2.push(data[5]);
            temp.push(data);
        }
    })

    temp_ExchangeDetail = temp;


}







