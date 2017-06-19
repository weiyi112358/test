var KPIID = '';//标记判断是增加还是编辑
var KPIType;
var TargetValueType = null;
var dt_kpi;
$(function () {
    //加载KPI类型下拉框
    ajax("/BaseData/GetOptionDataList?optType=KPIType", null, function (res) {
        if (res.length > 0) {
            var opt = "<option value=''>请选择</option>";
            for (var i = 0; i < res.length; i++) {
                opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
            }
            $('#drpkpitype').append(opt);
            $('#drp_kpitype').append(opt);
            KPIType = res;
        } else {
            var opt = "<option value=''>-无-</option>";
            $('#drpkpitype').append(opt);
            $('#drp_kpitype').append(opt);
        }
        //加载数据类型下拉框
        ajax("/BaseData/GetOptionDataList?optType=TargetValueType", null, function (res) {
            if (res.length > 0) {
                var opt = "<option value='' selected='selected'>请选择</option>";
                for (var i = 0; i < res.length; i++) {
                    opt += '<option value=' + res[i].OptionValue + '>' + res[i].OptionText + '</option>';
                }
                $('#drp_kpitargetvaluetype').append(opt);
                TargetValueType = res;
                dt_kpi = $('#dt_kpi').dataTable({
                    sAjaxSource: '/BaseData/GetKpiList',
                    bSort: true,   //不排序
                    bInfo: true,   //不显示 ‘从 _START_ 到 _END_ 条，共 _TOTAL_ 条记录’
                    bServerSide: true,  //每次请求后台数据
                    bLengthChange: false,//不显示 ‘显示 _MENU_ 条’
                    bPaginate: true, //显示分页信息
                    iDisplayLength: 8,
                    aoColumns: [
                        { data: 'KPIID', title: "编号", sortable: true },
                        { data: 'KPIName', title: "KPI名称", sortable: false },
                        {
                            data: null, title: "KPI类型", sortable: false, render: function (r) {
                                for (var i = 0; i < KPIType.length; i++) {
                                    if (r.KPIType == KPIType[i].OptionValue) {
                                        return KPIType[i].OptionText;
                                    }
                                }
                            }
                        },
                        { data: 'Unit', title: "KPI单位", sortable: false },
                         {
                             data: null, title: "KPI目标值类型", sortable: false, render: function (r) {
                                 for (var i = 0; i < TargetValueType.length; i++) {
                                     if (r.TargetValueType == TargetValueType[i].OptionValue) {
                                         return TargetValueType[i].OptionText;
                                     }
                                 }
                             }
                         },
                        { data: 'AddedDate', title: "创建时间", sortable: true },
                        {
                            data: null, title: "操作", sClass: "center", sortable: false,
                            render: function (obj) {
                                return "<button class='btn' id='btnModify'  onclick='goEdit(\"" + obj.KPIID + "\")'>编辑</button> <button class='btn btn-danger' id='btnDelete' onclick='goDelete(\"" + obj.KPIID + "\")'>删除</button> <button class='btn" + (obj.Enable == true ? " btn-danger" : "") + "' onclick='active(" + obj.KPIID + "," + obj.Enable + ")'>" + (obj.Enable == true ? "停用" : "启用") + "</button>";
                            }
                        }
                    ],
                    fnFixData: function (d) {
                        d.push({ name: 'KPIname', value: $("#kpiname").val() });
                        d.push({ name: 'KPItype', value: $("#drpkpitype").val() });
                    }
                });
            } else {
                var opt = "<option value=''>-无-</option>";
                $('#drp_kpitargetvaluetype').append(opt);
            }
        });
    });

    //查询
    $('#btnSearch').click(function () {
        dt_kpi.fnDraw();
    })

    //清空搜索的输入信息
    $('#btnClear').click(function () {
        $('#kpiname').val('');
        $('#drpkpitype').val('');
    })

    //保存编辑信息
    $("#frmEditKPI").submit(function (e) {
        e.preventDefault();
        goSave();
    })

    //弹窗清空数据
    $('#btn_clear').click(function () {
        $("#txt_kpiname").val('');
        $("#txt_kpiunit").val('');        
        $("#drp_kpitargetvaluetype").val('');
        $("#txt_script").val('');
        if ($("#hd_edit").val() != 1) {
            $("#drp_kpitype").val('');
            $("#drp_kpitype").removeAttr("disabled");
        }
        
    })
})

//编辑弹窗
function goEdit(kpiid) {
    $('#btn_clear').click();//每次弹窗先清空数据
    $("#hd_edit").val(0);
    if (kpiid != '') {
        $("#hd_edit").val(1);
        $("#drp_kpitype").attr("disabled", "disabled");
        $('#table_editattribute .modal-header h3').html('维度编辑');
        KPIID = kpiid;
        ajax("/BaseData/GetKPIById", { KPIID: kpiid }, function (res) {

            $("#txt_kpiname").val(res.KPIName);
            $("#drp_kpitype").val(res.KPIType);
            $("#txt_kpiunit").val(res.Unit);
            $("#drp_kpitargetvaluetype").val(res.TargetValueType);
            $("#txt_script").val(res.ComputeScript);
            $("#hd_addeduser").val(res.AddedUser);
            $("#hd_adddedate").val(res.ModifiedDate);
        });
    } else { KPIID = ''; $('#btn_clear').click(); $('#table_editattribute .modal-header h3').html('新增KPI'); }

    $.colorbox({
        initialHeight: '0',
        initialWidth: '0',
        overlayClose: false,
        opacity: '0.3',
        //title: '素材',
        href: "#table_editdimension",
        inline: true
    });
}
//验证数据
var DataValidator = $("#frmEditKPI").validate({
    //onSubmit: false,
    rules: {
        txt_kpiname: {
            required: true,
        },
        txt_kpiunit: {
            required: true,
        },
        drp_kpitype: {
            required: true,
        },
        drp_kpitargetvaluetype: {
            required: true,
        },
        txt_script: {
            required: true,
        }
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});
//保存新增信息或者修改过的信息
function goSave() {
    if (DataValidator.form()) {
        var KPI = {
            KPIID: KPIID,
            KPIName: $("#txt_kpiname").val(),
            Unit: $("#txt_kpiunit").val(),
            KPIType: $("#drp_kpitype").val(),
            TargetValueType: $("#drp_kpitargetvaluetype").val(),
            ComputeScript: encode($("#txt_script").val()),
            AddedUser: $("#hd_addeduser").val(),
            AddedDate: $("#hd_adddedate").val(),
        }
        var postUrl = "/BaseData/AddorUpdateKPI";
        ajax(postUrl, KPI, function (res) {
            if (res.IsPass) {
                $.colorbox.close();
                var start = dt_kpi.fnSettings()._iDisplayStart;
                var length = dt_kpi.fnSettings()._iDisplayLength;
                dt_kpi.fnPageChange(start / length, true);
                $.dialog(res.MSG);
            } else { $.dialog(res.MSG); }
        });
    }
}
//删除数据
function goDelete(kpiid) {
    $.dialog("确认删除吗?", {
        footer: {
            closebtn: '取消',
            okbtn: '确认'
        }
    }, function () {
        ajax("/BaseData/DeleteKPIById", { KPIID: kpiid }, function (res) {
            if (res.IsPass) {
                $.dialog(res.MSG);
                dt_kpi.fnDraw();
            } else { $.dialog(res.MSG); }
        });
    })
}



//启用或禁用KPI
function active(ID, status) {
    var data = { KPIID: ID, Enable: !status };
    ajax("/BaseData/SetKPIEnable", data, function (d) {
        if (d.IsPass) {
            $.dialog(d.MSG);
            //dtUsers.fnDraw();
            var start = dt_kpi.fnSettings()._iDisplayStart;
            var length = dt_kpi.fnSettings()._iDisplayLength;
            dt_kpi.fnPageChange(start / length, true);
        } else {
            $.dialog(d.MSG);
        }
    });
}
