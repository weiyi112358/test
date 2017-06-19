var dtDiscrepantIndustryCouponList,
    dtDiscrepantIndustryCouponDetail,
    uploadedCouponList,
    oUploader;

$(function () {
    $("#txtStartDate,#txtEndDate").datepicker({ startDate: '0' });

    //加载异业优惠券模板列表
    loadDiscrepantIndustryCouponTempletList();
    //加载异业优惠券列表
    loadDiscrepantIndustryCouponList();
    //加载数据表格

    //查询
    $("#btnSearch").click(function () {
        loadDiscrepantIndustryCouponList();
    });

    //导入模板数据
    $("#btnDownloadTemplate").click(function () {
        window.location = "/Upload/异业券导入模板.xls";
    });

    $("#btnClearUploadDiscrepantIndustryCoupon").click(function (e) {
        e.preventDefault();
        $("#btnSaveImport").removeAttr("disabled");
        $("#btnUploadDiscrepantIndustryCoupon").removeAttr("disabled");
        uploadedCouponList = "";
        resetFormByID("divDiscrepantIndustryCouponDialog");
        updateStatus();
    });

    $("#btnUploadDiscrepantIndustryCoupon").click(function (e) {
        e.preventDefault();
        $("#btnUploadDiscrepantIndustryCoupon").attr("disabled", "disabled");
        $("#btnSaveImport").attr("disabled", "disabled");
        bindDiscrepantIndustryCoupon();
    });


    //模板下载
    $("#btnAddDiscrepantIndustryCoupon").click(function () {
        uploadedCouponList = "";
        resetFormByID("divDiscrepantIndustryCouponDialog");
        showEditDialogMessage("divDiscrepantIndustryCouponDialog");
        updateStatus();
    });

    fnInitUpload();
});

//导出券
function exportCoupon(batchNo) {
    $('#exportForm')[0].action = "/BaseData/ExportPublicCoupon";
    $('#exportForm #exprBatchNo').val(batchNo);
    $('#exportForm')[0].submit();
}

function loadDiscrepantIndustryCouponList() {
    if (dtDiscrepantIndustryCouponList) {
        dtDiscrepantIndustryCouponList.fnDestroy();
    }

    dtDiscrepantIndustryCouponList = $("#dtDiscrepantIndustryCouponList").dataTable({
        sAjaxSource: '/BaseData/GetDiscrepantIndustryCouponList',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aaSorting: [[4, "desc"]],
        aoColumns: [
            { data: 'BatchNo', title: "批号", sortable: true, },
            {
                data: null, title: "券名", sortable: false, render: function (r) {
                    var show = r.CouponName;
                    show = show.length >= 9 ? (show.substr(0, 9) + "...") : show;
                    return "<span title='" + r.CouponName + "'>" + show + "</span>";
                }
            },
            { data: 'CouponType', title: "券类型", sortable: false, },
            { data: 'CouponCounts', title: "生成数量", sortable: false, },
            { data: 'AddedDate', title: "创建时间", sortable: true, },
            {
                data: 'StartDate', title: "有效起始时间", sortable: true, render: function (obj) {
                    return obj == null ? "" : obj.substr(0, 10);
                }
            },
            {
                data: 'EndDate', title: "有效结束时间", sortable: true, render: function (obj) {
                    return obj == null ? "" : obj.substr(0, 10);
                }
            },
            {
                data: null, title: "操作", sClass: "center", sortable: false,
                render: function (obj) {
                    return "<div style='width:150px;'><button class='btn detail' id='btnModify'  onclick=\"loadDiscrepantIndustryCouponDetail('" + obj.BatchNo + "')\">查看明细</button>&nbsp;"
                        + "<button class='btn export btn-gebo' onclick=\"exportCoupon('" + obj.BatchNo + "')\">导出</button></div>";
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'batchNo', value: $("#txtBatchNo").val() });
            d.push({ name: 'templetID', value: $("#drpSearchTempletID").val() });
        }
    });
}

//编辑条目信息
function loadDiscrepantIndustryCouponDetail(batchNo) {
    if (dtDiscrepantIndustryCouponDetail) {
        dtDiscrepantIndustryCouponDetail.fnDestroy();
    }
    dtDiscrepantIndustryCouponDetail = $("#dtDiscrepantIndustryCouponDetail").dataTable({
        sAjaxSource: '/BaseData/GetCouponDetailData',
        bSort: true,   //不排序
        bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true,  //每次请求后台数据
        bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 10,
        aoColumns: [
            { data: 'CouponID', title: "券编号", sortable: true, sWidth: "10%" },
            { data: 'CouponCode', title: "券代码", sortable: false, sWidth: "20%" },
            { data: 'Mobile', title: "手机号码", sortable: false, sWidth: "20%" },
            {
                data: 'IsUsed', title: "使用状态", sortable: false, sWidth: "10%", render: function (obj) {
                    return obj == true ? "已用" : "未用";
                }
            },
            { data: 'TempletName', title: "所属模板", sortable: false, sWidth: "40%" }
        ],
        fnFixData: function (d) {
            d.push({ name: "batchNo", value: batchNo });
        }
    });

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#divDiscrepantIndustryCouponDetail",
        inline: true
    });
}

//加载优惠券模板
function loadDiscrepantIndustryCouponTempletList() {
    ajax("/BaseData/GetDiscrepantIndustryCouponTemplet", null, function (data) {
        var opt = "<option value=''>请选择</option>",
            optSearch = "<option value=''>请选择</option>";
        if (!utility.isNull(data)) {
            for (var i = 0; i < data.length; i++) {
                optSearch += "<option value=" + data[i].TempletID + ">" + data[i].Name + "</option>";
                opt += "<option value=" + data[i].TempletID + "&&" + data[i].SubType + ">" + data[i].Name + "</option>";
            }
        }
        $("#drpSearchTempletID").html(optSearch);
        $("#drpTempletID").html(opt);
    });
}

function bindDiscrepantIndustryCoupon() {
    $("#spanError").html("");
    if (utility.isNull($("#drpTempletID").val())) {
        $("#spanError").html("请选择模板");
        $("#btnUploadDiscrepantIndustryCoupon").removeAttr("disabled");
        $("#btnSaveImport").removeAttr("disabled");
        return false;
    }
    if (utility.isNull(uploadedCouponList)) {
        $("#spanError").html("请选择导入的数据Excel");
        $("#btnUploadDiscrepantIndustryCoupon").removeAttr("disabled");
        $("#btnSaveImport").removeAttr("disabled");
        return false;
    }

    if (!utility.isNull($("#txtStartDate").val()) && !utility.isNull($("#txtEndDate").val()) && !utility.compareDate($("#txtStartDate").val(), $("#txtEndDate").val())) {
        $("#spanError").html("失效时间必须大于生效时间");
        $("#btnUploadDiscrepantIndustryCoupon").removeAttr("disabled");
        $("#btnSaveImport").removeAttr("disabled");
        return false;
    }

    var templetID = $("#drpTempletID").val().split("&&")[0],
        couponType = $("#drpTempletID").val().split("&&")[1],
        startDate = $("#txtStartDate").val(),
        endDate = utility.isNull($("#txtEndDate").val()) ? "" : $("#txtEndDate").val() + " 23:59:59",
        obj = {
            "TempletID": templetID,
            "CouponType": couponType,
            "StartDate": startDate,
            "EndDate": endDate,
            "Enable": true,
            "IsUsed": false
        };

    ajax("/BaseData/BatchInsertDiscrepantIndustryCouponPool", { templet: JSON.stringify(obj), "couponList": uploadedCouponList }, function (data) {
        if (data.IsPass) {
            $.colorbox.close();
            $.dialog("导入成功");
            $("#btnUploadDiscrepantIndustryCoupon").removeAttr("disabled");
            $("#btnSaveImport").removeAttr("disabled");
        } else {
            $("#spanError").html(data.MSG);
            $("#btnUploadDiscrepantIndustryCoupon").removeAttr("disabled");
            $("#btnSaveImport").removeAttr("disabled");
        }
    });

    uploadedCouponList = "";
    updateStatus();
    loadDiscrepantIndustryCouponList();
    return;
}

function updateStatus() {
    if (utility.isNull(uploadedCouponList)) {
        $("#spanImportStatus").html("(状态：未导入)");
    } else {
        $("#spanImportStatus").html("(状态：已导入)");
    }
}

//上传js组件初始化
function fnInitUpload() {
    var sUploadUrl = "/BaseData/UploadDiscrepantIndustryCoupon";

    oUploader = new plupload.Uploader({
        runtimes: 'gears,html5,flash,silverlight,browserplus',
        browse_button: 'btnPickFiles', //打开文件按钮id
        container: 'container', //dom容器id
        unique_names: true,
        rename: true,
        max_file_size: '5mb',//最大文件大小
        url: sUploadUrl,//上传服务地址       
        // resize: { width: 320, height: 240, quality: 90 }, //图片压缩
        flash_swf_url: '/gebo/lib/plupload/js/plupload.flash.swf',
        silverlight_xap_url: '/gebo/libplupload/js/plupload.silverlight.xap',
        filters: [
            //{ title: "Image files", extensions: "jpg,gif,png,bmp" },
            //{ title: "Pdf files", extensions: "Pdf" },
            //{ title: "Word files", extensions: "doc,docx" },
            //{ title: "Txt files", extensions: "txt" },
            { title: "Excel files", extensions: "xls,xlsx" }

        ]
    });

    //绑定初始化事件
    oUploader.bind('Init', function (up, params) {
        $("#txtFilePath").val("");
    });

    //绑定添加文件事件
    oUploader.bind('FilesAdded', function (up, files) {
        up.splice(0, up.files.length);

        if (files != null && files.length > 0)
            var length = files.length
        for (var i = 0; i < length; i++) {
            if (files.length > 1) {
                files.pop();
            } else {
                $("#txtFilePath").val(files[0].name);
            }
        }
    });

    //绑定组件出错事件
    oUploader.bind('Error', function (oUploader, erorr) {
        switch (erorr.code) {
            case plupload.FILE_EXTENSION_ERROR:
                $.dialog("不支持此类型的文件上传");
                $("#txtFilePath").val("");
                oUploader.splice(0, oUploader.files.length);
                break;
            case plupload.FILE_SIZE_ERROR:
                $.dialog("最大上传文件大小为10M");
                $("#txtFilePath").val("");
                oUploader.splice(0, oUploader.files.length);
                break;
            default:
                $.dialog(erorr.message);
                break;
        }
    });

    //文件上传成功事件
    oUploader.bind('FileUploaded', function (oUploader, file, result) {
        uploadedCouponList = result.response;
        updateStatus();
       
    });
    //绑定上传按钮事件
    $('#btnSaveImport').bind("click", function () {        
        oUploader.settings.page_url = sUploadUrl;
        oUploader.settings.url = sUploadUrl;
        oUploader.start();
        $("#btnSaveImport").removeAttr("disabled");
    });

    oUploader.init();
}