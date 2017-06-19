var promotionID = 0;
var detail, table
$(document).ready(function () {
    //绑定所有的商业计划
    detail = $("#dt_businessplan").dataTable({
        sAjaxSource: "/Promotion/GetBusinessPlan",
        bInfo: false,
        bServerSide: true,
        bLengthChange: false,
        bPaginate: false,
        bFilter: false,
        bSort: true,
        bSortCellsTop: true,
        iDisplayLength: 8,
        aaSorting: [[2, "desc"]],
        aoColumns: [
         {
             data: null, title: "商业计划名称", sortable: false, sWidth: "100", render: function (r) {
                 var show = r.BusinessPlanName.length > 10 ? (r.BusinessPlanName.substring(0, 10) + '...') : r.BusinessPlanName;
                 var str = '<span title="' + r.BusinessPlanName + '">' + show + '</span><input type="hidden" value="' + r.BusinessPlanID + '">';
                 return str;
             }
         },
         //{
         //    data: null, title: "生效时间", sortable: false, sWidth: "156", render: function (r) {
         //        var str = '<span></span> <i class="icon-pencil pencil" onclick="showdatedialog(this)"></i>';
         //        return str;
         //    }
         //},
         //  {
         //      data: null, title: "失效时间", sortable: false, sWidth: "156", render: function (r) {
         //          var str = '<span></span> <i class="icon-pencil pencil" onclick="showdatedialog(this)"></i>';
         //          return str;
         //      }
         //  },
         {
             data: null, sortable: false, title: "", sWidth: "100", render: function (r) {
                 var str = '<input type="checkbox"/>';
                 return str;
             }
         },
          {
              data: 'AddedDate', title: "", sortable: true, bVisible: false
          },
        ],
        fnFixData: function (d) {
            d.push({ name: 'busplanname', value: $("#busplan_name").val() });
        },
        fnDrawCallback: function () {
            if (promotionID != 0) {
                checksyscommon();
            }
        }
    });


    //绑定所有的促销活动
    table = $("#dt_promotion").dataTable({
        sAjaxSource: '/Promotion/GetPromotion',
        bInfo: false,
        bServerSide: true,
        bLengthChange: false,
        bPaginate: true,
        bFilter: false,
        iDisplayLength: 10,
        select: true,
        aaSorting: [[3, "desc"]],
        aoColumns: [
             { data: "PromotionCode", title: "", sWidth: 0, bVisible: false },
            {
                data: null, title: "促销名称", sortable: false, sWidth: "70%", render: function (r) {
                    var show = r.PromotionName.length > 30 ? (r.PromotionName.substring(0, 30) + '...') : r.PromotionName;
                    var str = '<a onclick="showdetail(' + r.BaseDataID + ')" title="' + r.PromotionName + '">' + show + '</a><input type="hidden" value="' + r.BaseDataID + '">';
                    return str;
                }
            },
             {
                 data: null, title: "促销代码", sortable: false, sWidth: "50%", render: function (r) {
                     return "<a href='#' onclick='showPosDetail(\"" + r.PromotionID + "\")'>" + r.PromotionCode + "</a>"
                 }
             },
              {
                  data: "BaseDataID", title: "", sortable: false, sWidth: "0", bVisible: false
              },
        ],
        fnFixData: function (d) {
            d.push({ name: 'promotionname', value: $("#promotionname").val() });
        },
    });

    //点击促销活动时获取该促销活动与会员细分绑定情况
    $('#dt_promotion tbody').on('click', 'tr', function () {
        promotionID = $(this).children()[0].children[1].value;
        if ($(this).hasClass('table-row-select')) {
            //$(this).removeClass('table-row-select');
        }
        else {
            table.$('tr.table-row-select').removeClass('table-row-select');
            $(this).addClass('table-row-select');
            checksyscommon();
        }
    });
})
/******* 起始日期和结束日期
//弹出选择时间对话框
function showdatedialog(o) {
    $("#dt_businessplan").find(".datetime-dialog").remove();
    var a = $(".datetime-dialog").clone();
    var left = $(o).position().left;
    var top = $(o).position().top;
    a.css({ "left": left - 260, "top": top - 45 })
    var time = o.previousElementSibling.innerHTML;
    var hour = "";
    if (time == "") {
        a.children()[0].children[0].value = new Date().format('yyyy-MM-dd');;
        hour = "current";
    }
    else {
        a.children()[0].children[0].value = time.substring(0, 10);
        hour = time.substring(11, time.length);
    }
    a.children()[0].children[0].id = 'time_ymd';
    a.children()[0].children[1].id = 'time_hm';
    a.show();
    $(o).after(a);
    $("#time_ymd").datepicker();
    $("#time_hm").timepicker({
        showMeridian: false,
        defaultTime: hour
    });
}


//隐藏时间对话框
function removetimedialog() {
    $("#dt_businessplan").find(".datetime-dialog").remove();
}

//保存所选时间
function savetime(o) {
    var day = o.parentNode.children[0].value;
    var time = o.parentNode.children[1].value;
    o.parentNode.parentNode.parentNode.children[0].innerHTML = day + " " + time;
    $("#dt_businessplan").find(".datetime-dialog").remove();
}

//清空所选时间
function cleartime(o) {
    o.parentNode.parentNode.children[1].children[0].innerHTML = "";
    o.parentNode.parentNode.children[2].children[0].innerHTML = "";
}
*****/
//保存活动与会员细分关系
function savesyscommon() {
    if (promotionID == 0) {
        $.dialog("请选择促销活动！");
        return;
    }
    var syscommons = new Array();
    var a = $('#dt_businessplan tbody').children();

    for (var i = 0; i < a.length; i++) {
        var syscommon = {
            RelationBigintValue1: "",
            StartDate: null,
            EndDate: null,
            ReferenceStringValue1: ""
        }
        if (a[i].children[1].children[0].checked) {
            syscommon.RelationBigintValue1 = promotionID;
           
            syscommon.ReferenceStringValue1 = a[i].children[0].children[1].value;
            syscommons.push(syscommon);
            //if (syscommon.EndDate != "") {
            //    if (syscommon.EndDate < syscommon.StartDate) {
            //        $.dialog("失效时间不能小于生效时间！");
            //        return;
            //    }
            //}

            //if (syscommon.StartDate == "" && syscommon.EndDate != "") {
            //    $.dialog("请填写完整的生效时间和失效时间！");
            //    return;
            //}
        }
    }
    $.post(
   "/Promotion/SaveSYSCommon", {
       promotionID: promotionID,
       sysCommons: JSON.stringify(syscommons),
       type: 2
   }, function (data, state) {
       $.dialog(data);
   },
   "html"
)
}

//搜索促销活动
function promotionsearch() {
    table.fnDraw();
}

//搜索会员细分
function busplansearch() {
    detail.fnDraw();
}
var dt_approve;
function showdetail(id) {
    $("#hd_promotionid").val(id);
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editdimension",
        inline: true
    });
    if (dt_approve) {
        dt_approve.fnDraw();
    }
    else {
        dt_approve = $("#dt_bp_approved").dataTable({
            sAjaxSource: '/Promotion/GetBusinessPlanApproved',
            bInfo: false,
            bServerSide: true,
            bLengthChange: false,
            bSort: false,
            bPaginate: true,
            bFilter: false,
            iDisplayLength: 8,
            select: true,

            aoColumns: [

             { data: 'BusinessPlanName', title: "商业计划名称", sortable: false, sWidth: "50%" },
            ],
            fnFixData: function (d) {
                d.push({ name: 'promotionID', value: $("#hd_promotionid").val() });
            },
        });
    }


    $("#cboxOverlay").hide();
}

function showPosDetail(pid) {
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_posdetail",
        inline: true
    });
    $.getJSON("/Promotion/GetPromotionByid?pid=" + pid, function (json) {
        $("#PromotionID").html(json.PromotionID);
        $("#PromotionCode").html(json.PromotionCode);
        $("#PromotionName").html(json.PromotionName);
        $("#PromotionBillType").html(json.PromotionBillType);
        $("#PromotionType").html(json.PromotionType);
        $("#PromotionRemark").html(json.PromotionRemark);
        $("#PromotionStartDate").html(json.PromotionStartDate);
        $("#PromotionEndDate").html(json.PromotionEndDate);
        $("#PromotionIsEnd").html(json.PromotionIsEnd == "0" ? "否" : "是");
        $("#StartDatePromotion").html(json.StartDatePromotion);
        $("#EndDatePromotion").html(json.EndDatePromotion);
    });
    $("#cboxOverlay").hide();
}

function checksyscommon() {
    var noCache = Date();
    $.getJSON("/Promotion/GetSysCommonChecked?promotionID=" + promotionID + "&type=2", { "noCache": noCache }, function (json) {
        var a = $('#dt_businessplan tbody').children();
        for (var k = 0; k < a.length; k++) {
            a[k].children[1].children[0].checked = false;
            //a[k].children[1].children[0].innerHTML = "";
            //a[k].children[2].children[0].innerHTML = "";
        }
        for (var j = 0; j < json.data.length; j++) {
            for (var i = 0; i < a.length; i++) {

                if (json.data[j].ReferenceStringValue1 == a[i].children[0].children[1].value) {
                    a[i].children[1].children[0].checked = true;
                    //a[i].children[1].children[0].innerHTML = json.data[j].StartDate == null ? "" : json.data[j].StartDate.substring(0, 16);
                    //a[i].children[2].children[0].innerHTML = json.data[j].EndDate == null ? "" : json.data[j].EndDate.substring(0, 16);
                }
            }
        }
    });
}