var vechile = [];
var isInit = 1;
$(function () {
    $("#txtBirthday,#txtBuyDate").datepicker();
    $.ajaxSetup({
        async: false
    });
    loadProvince();
    LoadCompany();

    loadOptionalSelectValue();

    $(".chzn_a").chosen({
        allow_single_deselect: true
    });

    $(".chzn_b").chosen();


    $("#drpStore").change(function () {
        var store = $("#drpStore").val();
        if (store) {
            ajax('/Member360/GetStoreByCode', { storeCode: store }, function (res) {

                $("#txtBrand").val(res.BrandCodeStore);
                $("#txtRegion").val(res.AreaCodeStore);
            });
        } else {
            $("#txtBrand").val('');
            $("#txtRegion").val('');
        }
    });
    //$("#txtBrand").val($("#txthidbrand").val());
    //$("#txtRegion").val($("#txthidarea").val());


    //新增条目
    $("#frmAddStore").submit(function (e) {
        e.preventDefault();

        if (DataValidatorAdd.form()) {
            var meminfo = {
                MemState: 1,
                MemName: $("#txtMemName").val(),
                Gender: $("#drpGender").val(),
                Birthday: $("#txtBirthday").val(),
                CertType: $("#drpCertType").val(),
                CertNo: $("#txtCertNo").val(),
                Corp: $("#drpCompany").val(),
                StoreCode: $("#drpStore").val(),
                Mobile: $("#txtMobile").val(),
                Address: $("#txtAddress").val(),
                ProvinceCode: $("#drpProvince").val(),
                CityCode: $("#drpCity").val(),
                DistrictCode: $("#drpDistrict").val(),
                PosNo:$("#txtPosNo").val(),
                Province: $("#drpProvince").val() == "" ? "" : $("#drpProvince").find("option:selected").text(),
                City: $("#drpCity").val() == "" ? "" : $("#drpCity").find("option:selected").text(),
                District: $("#drpDistrict").val() == "" ? "" : $("#drpDistrict").find("option:selected").text(),
                SkinType: $("#skinType").val(),
                SkinCareType: $("#skinCareType").val(),
                MakeupFrequency: $("#makeupFrequency").val(),
                MarriageStatus: $("#marriageStatus").val(),
                HasChild: $("#hasChild").val(),
                Job: $("#job").val(),
                MonthlyIncome: $("#monthlyIncome").val(),
                KnowingFrom: $("#knowingFrom").val(),
                NeededMessage: $("#neededMessage").val()


                //RechargeGold: $("#isOldMem").prop('checked') ? $("#txtRechargeGold").val() : "0",
                //SendGold: $("#isOldMem").prop('checked') ? $("#txtSendGold").val() : "0",
                //TotalGold: $("#isOldMem").prop('checked') ? $("#txtTotalGold").val() : "0",
                //NoInvoiceGold: $("#isOldMem").prop('checked') ? $("#txtNoInvoiceGold").val() : "0",
            }
            $("#btnSave").attr('disabled', true);
            ajax("/Member360/AddMemberData", { mem: meminfo }, function (res) {
                if (res.IsPass) {
                    $("#card_no").text(res.Obj[0].CustomerNo);

                    var settings = {
                        output: "css",                       
                    };

                    $("#barcodeTarget").html("").show().barcode($("#card_no").text(), "code39", settings);


                    $(".vipCard").css("background", "url(../../img/card.jpg) no-repeat center,#000000");
                    $.colorbox({
                        initialHeight: '0',
                        initialWidth: '0',
                        href: '#card_dialog',
                        overlayClose: false,
                        inline: true,
                        opacity: '0.3'
                    });
                    clearData();
                    $("#btnSave").prop('disabled', false);
                } else { $.dialog(res.MSG); $("#btnSave").prop('disabled', false); }
            });
        }
    })
    $("#btnClear").click(function () {
        clearData();
    })

    //省市区联动
    $("#drpProvince").change(function () {
        getRegionCity($("#drpProvince").val());
    })
    $("#drpCity").change(function () {

        getRegionRegion($("#drpCity").val());
    })

    $("#txtCertNo").blur(function () {
        if ($("#drpCertType").val() == '1') { 
            txtCertNoOnChange($("#txtCertNo").val());
        }
    })
    $("#txtCertNo").keyup(function () {
        if ($("#drpCertType").val() == '1' && $("#txtCertNo").val() == '') {
            $("#drpProvince").prop('disabled', false).val('');
            $("#drpCity").prop('disabled', false).val('');
            $("#drpDistrict").prop('disabled', false).val('');
        }
    })
    $("#drpCertType").change(function () {

        $("#txtCertNo").val('');
        $("#drpProvince").prop('disabled', false).val('');
        $("#drpCity").prop('disabled', false).val('');
        $("#drpDistrict").prop('disabled', false).val('');
    })


    //查询条件中的分公司
    $('#drpCompany').change(function () {
        $('#drpStore').html("");
        $.ajax({
            type: 'post',
            url: '/MemberTransform/GetStoreList',
            dataType: 'json',
            data: { company: $('#drpCompany').val() },
            success: function (result) {
                if (result.length > 0) {
                    var opt = "";
                    opt += "<option value=''>请选择</option>";
                    for (var i = 0; i < result.length; i++) {
                        opt += '<option value=' + result[i].StoreCode + '>' + result[i].StoreName + '/' + result[i].StoreCode+ '</option>';
                    }
                    $('#drpStore').append(opt);
                    $(".chzn_a").trigger("liszt:updated");
                }
                else {
                    opt = "<option value=''>请选择</option>";
                    $('#drpStore').append(opt);
                    $(".chzn_a").trigger("liszt:updated");
                }
            },
            error: function (e) {
                e.responseText;
            }
        })
    });

})


//$('#btnCancel').click(function () {
//    $.colorbox.close();
//    clearData();
//})

//加载公司下拉框
function LoadCompany() {
    //$.ajax({
    //    type: 'post',
    //    url: '/Purchases/LoadCompany',
    //    dataType: 'json',
    //    data: {},
    //    success: function (result) {
    //        if (result.data.length > 0) {
    //            var opt = "";
    //            opt += "<option value=''>请选择</option>";
    //            for (var i = 0; i < result.data.length; i++) {
    //                opt += '<option value=' + result.data[i].CompanyCode + '>' + result.data[i].CompanyName + '</option>';
    //            }
    //            $('#drpCompany').append(opt);
    //        }
    //        else {
    //            opt = "<option value=''>请选择</option>";
    //            $('#drpCompany').append(opt);
    //        }
    //    },
    //    error: function (e) {
    //        e.responseText;
    //    }
    //})

    $.post('/PurchasesNew/LoadCompany', {}, function (result) {
        $('#drpCompany').empty();
        if (result.data.length > 0) {
            var opt = '<option value="">==请选择==<option>';
            $.each(result.data, function (i, data) {
                opt += '<option value="' + data.CompanyCode + '">' + data.CompanyName + '/' + data.CompanyCode + '<option>'
            });
            $('#drpCompany').append(opt);
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#drpCompany').append('<option value="">==无==<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}
//新增时验证数据
var DataValidatorAdd = $("#frmAddStore").validate({
    //onSubmit: false,
    rules: {
        txtMemName: {
            required: true,
            maxlength: 20,
            isSb: true,
            isOnlyLNC: true,
        },
        txtCertNo: {
            required: false,
            isSb: true,
            isOnlyLN: true
        },
        txtMobile: {
            required: true,
            isMobileNo: true
        },
        txtBirthday: {
            date: true
        },
        drpCompany: {
            required: true,
        },
        drpStore: {
            required: true,
        },
        //txtTotalGold: {
        //    number: true
        //},
        //txtSendGold: {
        //    number: true
        //},
        //txtRechargeGold: {
        //    number: true
        //},

    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("span.error-block"));
    },
    errorClass: 'error-block',
});


//清空数据
function clearData() {
    $("#txtMemNo").val('');
    $("#drpMemState").val('1');
    $("#drpMemType").val('1').change();
    $("#txtMemName").val('');
    $("#drpGender").val('0');
    $("#txtBirthday").val('');
    $("#drpCertType").val('0');
    $("#txtCertNo").val('');
    $("#txtRegion").val('');
    $("#txtBrand").val('');
    $("#txtCustomerNo").val('');
    $("#txtMobile").val('');
    $("#txtTelephone").val('');
    $("#txtWechat").val('');
    $("#txtAddress").val('');
    $("#txtPostcode").val('');
    $("#drpProvince").val('');
    $("#drpCity").val('');
    $("#drpDistrict").val('');
    $("#drpStore").val('')
    $("#txtVechileNo").val('');
    $("#drpVechileBrand").val('').trigger("liszt:updated");
    $("#drpVechileSerice").val('').trigger("liszt:updated");
    $("#drpVechileType").val('').trigger("liszt:updated");
    $("#txtVechileColor").val('');
    $("#txtVechileInner").val('');
    $("#txtMile").val('');
    $("#txtVinNo").val('');
    $("#txtBuyDate").val('');
    //$("#isTransfer").val('');
    vechile = [];
    $("#dt_Vehcile tbody").html("");
    $('.error-block').html('');

    $("#isOldMem").prop('checked', false);

    $(".olddiv").hide();
    //$("#txtRechargeGold").val('');
    //$("#txtSendGold").val('');
    $("#drpCustomerLevel").val('');
    //$("#txtTotalGold").val('');
    $("#drpDepartment").val('');
    $('#txtPosNo').val('');
    LoadCompany();


    $('#skinType').val("").trigger("liszt:updated");
    $('#skinCareType').val('').trigger("liszt:updated");
    $('#marriageStatus').val('');
    $('#hasChild').val('');
    $('#job').val('');
    $('#monthlyIncome').val('');
    $('#knowingFrom').val('');
    $('#neededMessage').val('');
    $('#makeupFrequency').val('');




}

//加载省份
function loadProvince() {
    ajax("/BaseData/GetRegionByPID", {
        pid: 0,
        grade: 1
    }, function (res) {
        if (res.IsPass) {
            var optionstring = "<option value=''>请选择</option>";
            var prv = res.Obj[0];
            for (var i in prv) {
                optionstring += "<option value=\"" + prv[i].RegionID + "\" >" + prv[i].NameZH + "</option>";
            }
            $("#drpProvince").append(optionstring);
        } else {
            $.dialog(res.MSG);
        }
    })
}



//绑定市信息
function getRegionCity(pId) {
    if (pId != '') {
        postUrl = "/BaseData/GetRegionByPId";
        ajaxSync(postUrl, {
            "pid": pId,
            grade: 2
        }, function (data) {
            if (data.IsPass) {
                $("#drpCity").empty();
                var optionstring = "<option value=''>请选择</option>";
                //$("#drpCity").append(optionstring);
                var rdata = data.Obj[0];
                for (var i in rdata) {
                    optionstring += "<option value=\"" + rdata[i].RegionID + "\" >" + rdata[i].NameZH + "</option>";
                }

                $("#drpCity").append(optionstring);
            } else {
                //var optionstring = "<option value=''>请选择</option>";
                //$("#inp_city").html(optionstring);
                //$("#inp_city").html(optionstring);
            }
        })
    }


}
//function getRegionCityCallBack() {
//    return function (data) {
//        if (data.IsPass) {
//            $("#drpCity").empty();
//            var optionstring = "<option value=''>请选择</option>";
//            //$("#drpCity").append(optionstring);
//            var rdata = data.Obj[0];
//            for (var i in rdata) {
//                optionstring += "<option value=\"" + rdata[i].RegionID + "\" >" + rdata[i].NameZH + "</option>";
//            }

//            $("#drpCity").append(optionstring);
//        }
//    }
//}

//绑定地级区信息
function getRegionRegion(pId) {
    if (pId != '') {
        var postUrl = "/BaseData/GetRegionByPId";
        ajaxSync(postUrl, { "pid": pId, grade: 3 }, getRegionRegionCallBack());
    }
    else {
        //var optionstring = "<option value=''>请选择</option>";
        //$("#drpRegion").html(optionstring);
    }

}
function getRegionRegionCallBack() {
    return function (data) {
        if (data.IsPass) {
            $("#drpDistrict").empty();
            var optionstring = "<option value=''>请选择</option>";
            var rdata = data.Obj[0];
            for (var i in rdata) {
                optionstring += "<option value=\"" + rdata[i].RegionID + "\" >" + rdata[i].NameZH + "</option>";
            }
            $("#drpDistrict").append(optionstring);
        }
    }
}

function inputRegionByCertNo(regionId) {
    if (regionId != '') {
        var postUrl = "/BaseData/InputRegionByCertNo";
        ajax(postUrl, {
            "regionId": regionId
        }, function (res) {
            if (res.IsPass) {
                var prv = res.Obj[0];
                var regionPath = prv[0].RegionPath;
                var pathArray = regionPath.split(',');
                var provinceCode = pathArray[1];
                var cityCode = pathArray[2];
                var regionCode = pathArray[3];

                loadProvince();
                //getRegionCity(provinceCode);
                //getRegionRegion(cityCode);

                //$("#drpProvince").find("option[value='" + provinceCode + "']").attr("selected", true);
                //$("#drpCity").find("option[value='" + cityCode + "']").attr("selected", true);
                //$("#drpDistrict").find("option[value='" + regionCode + "']").attr("selected", true);

                $("#drpProvince").val(provinceCode).change();
                $("#drpCity").val(cityCode).change();
                $("#drpDistrict").val(regionCode);


            } else {
                $.dialog(res.MSG);
            }
        });
    }

}

function txtCertNoOnChange(str) {

    var reStr = /^[0-9]{18}$/;
    if (!reStr.test(str))
        return;

    var regionCode = str.substring(0, 6);
    var birthday = str.substring(6, 10) + "-" + str.substring(10, 12) + "-" + str.substring(12, 14);
    var sexCode = str.substring(16, 17);

    $("#txtBirthday").val(birthday)
    if (sexCode % 2 == 1)
        $("#drpGender").find("option[value='男']").attr("selected", true);
    else {
        $("#drpGender").find("option[value='女']").attr("selected", true);

    }
    //$("#drpProvince").prop('disabled', 'disabled');
    //$("#drpCity").prop('disabled', 'disabled');
    //$("#drpDistrict").prop('disabled', 'disabled');
    inputRegionByCertNo(regionCode);

}


function stripScript(str) {
    var pattern = new RegExp("[%--@`~!#$^&*()=|{}':;',\\[\\].<>/?~！#￥……&*（）——| {}【】‘；：”“'。，、？]")        //格式 RegExp("[在中间定义特殊过滤字符]")
    var s = str.value;
    var rs = "";
    for (var i = 0; i < s.length; i++) {
        rs = rs + s.substr(i, 1).replace(pattern, '');
    }
    str.value = rs;
}


function loadOptionalSelectValue() {
    loadSkinType();
    loadSkinCareType();
    loadMakeupFrequency();
    loadMarriageStatus();
    loadJob();
    loadMonthlyIncome();
    loadKnowingFrom();
    loadNeededMessage();

}


function loadSkinType() {
    $.post('/BaseData/GetOptionDataList', {optType:'SkinType'}, function (result) {
        $('#skinType').empty();
        if (result.length > 0) {
            var opt = '';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'                
            });
            $('#skinType').append(opt);
            
            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#skinType').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}

function loadSkinCareType() {
    $.post('/BaseData/GetOptionDataList', { optType: 'SkinCareType' }, function (result) {
        $('#skinCareType').empty();
        if (result.length > 0) {
            var opt = '';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'
            });
            $('#skinCareType').append(opt);

            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#skinCareType').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}

function loadMakeupFrequency() {
    $.post('/BaseData/GetOptionDataList', { optType: 'MakeupFrequency' }, function (result) {
        $('#makeupFrequency').empty();
        if (result.length > 0) {
            var opt = '<option value="">请选择</option>';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'
            });
            $('#makeupFrequency').append(opt);

            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#makeupFrequency').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}

function loadMarriageStatus() {
    $.post('/BaseData/GetOptionDataList', { optType: 'MarriageStatus' }, function (result) {
        $('#marriageStatus').empty();
        if (result.length > 0) {
            var opt = '<option value="">请选择</option>';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'
            });
            $('#marriageStatus').append(opt);

            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#marriageStatus').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}

function loadJob() {
    $.post('/BaseData/GetOptionDataList', { optType: 'Job' }, function (result) {
        $('#job').empty();
        if (result.length > 0) {
            var opt = '<option value="">请选择</option>';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'
            });
            $('#job').append(opt);

            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#job').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}

function loadMonthlyIncome() {
    $.post('/BaseData/GetOptionDataList', { optType: 'MonthlyIncome' }, function (result) {
        $('#monthlyIncome').empty();
        if (result.length > 0) {
            var opt = '<option value="">请选择</option>';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'
            });
            $('#monthlyIncome').append(opt);

            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#monthlyIncome').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}


function loadKnowingFrom() {
    $.post('/BaseData/GetOptionDataList', { optType: 'KnowingFrom' }, function (result) {
        $('#knowingFrom').empty();
        if (result.length > 0) {
            var opt = '<option value="">请选择</option>';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'
            });
            $('#knowingFrom').append(opt);

            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#knowingFrom').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}

function loadNeededMessage() {
    $.post('/BaseData/GetOptionDataList', { optType: 'NeededMessage' }, function (result) {
        $('#neededMessage').empty();
        if (result.length > 0) {
            var opt = '<option value="">请选择</option>';
            $.each(result, function (i, data) {
                opt += '<option value="' + data.OptionValue + '">' + data.OptionText + '</option>'
            });
            $('#neededMessage').append(opt);

            $(".chzn_a").trigger("liszt:updated");
        }
        else {
            $('#neededMessage').append('<option value="">无<option>');
            $(".chzn_a").trigger("liszt:updated");
        };
    });
}



