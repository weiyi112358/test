$(function () {
    LoadCampaignParentCatg(0);
    //加载数据表格
    dt_BrandTable = $('#dt_BrandTable').dataTable({
        sAjaxSource: '/BaseData/GetCampaignCatgList',
        bSort: true, //不排序
        bInfo: true, //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
        bServerSide: true, //每次请求后台数据
        bLengthChange: false, //不显示 ‘显示 _MENU_ 条’
        bPaginate: true, //显示分页信息
        iDisplayLength: 8,
        aoColumns: [
            { data: 'BaseDataID', title: "编号", sortable: true },
            { data: 'CampAttrName', title: "名称", sortable: false },
            { data: 'CampGrade', title: "所及级别", sortable: false },
            { data: 'AliaskeyBase', title: "所属父类", sortable: false },
            {
                data: null,
                title: "操作",
                sClass: "center",
                sortable: false,
                render: function (obj) {
                    //var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"enableItem('" + obj.BaseDataID + "','" + (obj.EnableBrand == "undefined" ? "" : obj.EnableBrand) + "')\">" + (obj.EnableBrand == "1" ? "禁用" : "启用") + "</button>";
                    var htm = "<button class=\"btn btn-modify\" id=\"btnModify\"  onclick=\"edit(" + obj.BaseDataID + ")\">编辑</button><button class=\"btn btn-delete\" id=\"btnDelete\" onclick=\"deleteItem(" + obj.BaseDataID + ")\">删除</button>";
                    return htm;
                }
            }
        ],
        fnFixData: function (d) {
            d.push({ name: 'parentCatg', value: $.trim($("#drpPraentCatg").val()) });
        }
    });
    //搜索
    $("#btnSerach").click(function () {
        dt_BrandTable.fnDraw();
    });

    //保存数据
    $("#frmAddBrand").submit(function (e) {
        //$("#btnAddSave").click(function () {
        e.preventDefault();
        if (DataValidator.form()) {
            var campaign = {
                ID: $("#txtCatgId").val(),
                CampGrade: $("#divDrpLevel").val(),
                CampType: $("#divDrpParent").val(),
                AliaskeyBase: $("#divDrpParent").val() + 'child',
                AliasSubkeyBase: $("#txtViewExt").val(),
                CampAttrName: $("#txtCatgName").val()
            }
            //增加
            if (campaign.ID == '') {
                ajax("/BaseData/AddCampaignData", { cam: campaign }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();
                        dt_BrandTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
            else { //编辑
                ajax("/BaseData/UpdateCampaignData", { cam: campaign }, function (res) {
                    if (res.IsPass) {
                        $.colorbox.close();

                        var start = dt_BrandTable.fnSettings()._iDisplayStart;
                        var length = dt_BrandTable.fnSettings()._iDisplayLength;
                        dt_BrandTable.fnPageChange(start / length, true);
                        //dt_BrandTable.fnDraw();
                        $.dialog(res.MSG);
                    } else $.dialog(res.MSG);
                });
            }
        }
    });

    $("#drpLevel").change(function () {
        var level = $("#drpLevel").val();
        if (level == 0) {
            LoadCampaignParentCatg(level);
        } else {
            LoadCampaignParentCatg(level);
        }
    });
    $("#divDrpLevel").change(function () {
        var level = $("#divDrpLevel").val();
        if (level == 0) {
            LoadCampaignParentCatgDiv(level - 1);
        } else {
            LoadCampaignParentCatgDiv(level - 1);
        }
    });
});

//验证数据
var DataValidator = $("#frmAddBrand").validate({
    //onSubmit: false,
    rules: {
        txtViewExt: {
            required: true,
            maxlength: 20,
        },
        txtCatgName: {
            required: true,
            maxlength: 20,
        },
        divDrpParent: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});

//弹窗
function goEdit() {
    $("#addBrand_dialog .heading h3").html("类别新增");
    //清空数据
    goClear();
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        href: "#addBrand_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//编辑条目信息
function edit(id) {

    $("#addBrand_dialog .heading h3").html("类别编辑");
    //清空数据
    goClear();
    ajax("/BaseData/GetCampaignById", { camid: id }, function (res) {
        $("#txtCatgId").val(res.BaseDataID);
        $("#divDrpLevel").val(res.CampGrade).change();
        $("#divDrpParent").val(res.CampType);
        $("#txtViewExt").val(res.AliasSubkeyBase);
        $("#txtCatgName").val(res.CampAttrName);
    });
    //显示编辑页面弹窗
    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#addBrand_dialog",
        inline: true
    });
    $.colorbox.resize();
}

//删除条目
function deleteItem(id) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteCampaignById", { camid: id }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_BrandTable.fnDraw();
            } else {
                $.dialog(res.MSG);
            }
        });
    });
}


//清空数据
function goClear() {
    $("#txtCatgId").val('');
    $("#divDrpLevel").val('1').change();
    $("#divDrpParent").val('');
    $("#txtViewExt").val('');
    $("#txtCatgName").val('');

    $('.error-block').html('');
}

//加载门店所属群组
function LoadCampaignParentCatg(grade) {
    $('#drpPraentCatg').empty();
    ajax('/BaseData/GetCampaignParentCatg', { grade: grade }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].AliaskeyBase + '>' + res[i].CampAttrName + '</option>';
            }
            $('#drpPraentCatg').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpPraentCatg').append(opt);
        }
    });
}
function LoadCampaignParentCatgDiv(grade) {
    $('#divDrpParent').empty();
    ajaxSync('/BaseData/GetCampaignParentCatg', { grade: grade }, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].AliaskeyBase + '>' + res[i].CampAttrName + '</option>';
            }
            $('#divDrpParent').append(opt);
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#divDrpParent').append(opt);
        }
    });
}