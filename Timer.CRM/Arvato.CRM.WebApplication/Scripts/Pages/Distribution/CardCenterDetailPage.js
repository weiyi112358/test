var btnCount = 3;
$(document).ready(function () {
    $('#title div').hide();
    var key = $('#key')[0].dataset.pagekey;
    var queryId = $('#key')[0].dataset.queryid;  

    if (key == "ApplyCard") {
        $('#ApplyCard').show();
        $.post('/Distribution/ApplyCardDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.CardTypeName + "</td><td>" + item.StoreName + "</td><td>" + item.Purpose + "</td><td>" + item.ApplyNumber + "</td><td>" + item.ApproveNumber + "</td><td></tr>";
                })
                $('#ApplyDetail').append(tr);
            }
            else {

            }
        }, 'json');
        $.post('/Distribution/ApplyCardPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                $('#apply1').text(result.data[0].OddIdNo);
                $('#apply2').text('[9900总部]');
                $('#apply3').text(result.data[0].AcceptingUnit);
                $('#apply4').text(result.data[0].ArriveTime);
                //$('#apply5').text(result.data[0].ArriveTime);
                $('#apply6').text(ExecuteToName(result.data[0].ExecuteStatus));
                $('#apply7').text('[跳4规则]');
                $('#apply8').text(StatusToName(result.data[0].Status));
                $('#apply9').text(result.data[0].CreateBy);
                $('#apply10').text(result.data[0].CreateTime);
                $('#apply11').text(result.data[0].CreateBy);
                $('#apply12').text(result.data[0].CreateTime);
            }
        }, 'json');
    }

    if (key == "VerifyCard") {
        $('#VerifyCard').show();
        //$('#VerifyTrue').css('display', '');
        //$('#VerifyFalse').css('display', '');
        $.post('/Distribution/ApplyCardDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.CardTypeName + "</td><td>" + item.Purpose + "</td><td>" + item.ApplyNumber + "</td><td>" + item.ApproveNumber + "</td><td></tr>";
                })
                $('#VerifyDetail').append(tr);
            }
            else {

            }
        }, 'json');
        $.post('/Distribution/ApplyCardPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                $('#apply111').text(result.data[0].OddIdNo);
                $('#apply112').text('[9900总部]');
                $('#apply113').text(result.data[0].AcceptingUnit);
                $('#apply114').text(result.data[0].OddId);
                $('#apply115').text(result.data[0].ArriveTime);
                //$('#apply116').text(ExecuteToName(result.data[0].IsExecute));
                $('#apply117').text('[跳4规则]');
                $('#apply118').text(StatusToName(result.data[0].Status));
                $('#apply119').text(result.data[0].CreateBy);
                $('#apply120').text(result.data[0].CreateTime);
                $('#apply121').text(result.data[0].CreateBy);
                $('#apply122').text(result.data[0].CreateTime);
                if (result.data[0].Status == "0") {
                    $('#VerifyTrue').css('display', '');
                    $('#VerifyFalse').css('display', '');
                }
            }
        }, 'json');
    }



    if (key == "Customize") {
        $('#Customize').show();      
        $.post('/Distribution/CustomizeDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.CardTypeName + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td><td>" + item.RetrieveNum + "</td></tr>";
                })
                $('#CustomizeDetail').append(tr);
            }
            else {

            }
        }, 'json');
        $.post('/Distribution/CustomizePage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                $('#customize1').text(result.data[0].OddIdNo);
                $('#customize2').text(result.data[0].Agent);
                $('#customize3').text('[跳4规则]');
                $('#customize4').text(result.data[0].AcceptingUnit);            
                $('#customize5').text(result.data[0].DestineNumber);
                $('#customize6').text(result.data[0].ReserveNumber);          
                $('#customize7').text(StatusToName(result.data[0].Status));
                $('#customize8').text(result.data[0].CreateBy);
                $('#customize9').text(result.data[0].CreateTime);
                $('#customize10').text(result.data[0].CreateBy);
                if (result.data[0].Status=="0") {
                    $('#VerifyTrue').css('display', '');
                    $('#VerifyFalse').css('display', '');
                }
            }
        }, 'json');

    }
    
    if (key == "Retrieve") {
        $('#Retrieve').show();    
        $.post('/Distribution/RetrieveDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.CardTypeName + "</td><td>" + item.RetrieveNum + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td></tr>";
                })
                $('#RetrieveDetail').append(tr);
            }
            else {

            }
        }, 'json');
        $.post('/Distribution/RetrievePage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                $('#retrieve1').text(result.data[0].OddIdNo);
                $('#retrieve2').text(result.data[0].OddId);
                $('#retrieve3').text(result.data[0].Agent);
                $('#retrieve4').text(result.data[0].RetrieveNum);
                $('#retrieve5').text(StatusToName(result.data[0].Status));
                $('#retrieve6').text(result.data[0].CreateBy);
                $('#retrieve7').text(result.data[0].CreateTime);
                $('#retrieve8').text(result.data[0].CreateBy);
                if (result.data[0].Status == "0") {
                    $('#VerifyTrue').css('display', '');
                    $('#VerifyFalse').css('display', '');
                }
            }
        }, 'json');
    }


    if (key == "BatchProduction") {
        $('#BatchProduction').show();    
        //$.post('/Distribution/BatchProductionPage', { queryId: queryId }, function (result) {
        //    if (result.data.length > 0) {
        //        var tr = "";
        //        $.each(result.data, function (i, item) {
        //            tr += "<tr><td>" + item.CardTypeName + "</td><td>" + item.RetrieveNum + "</td><td>" + item.CardNumIn + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td></tr>";
        //        })
        //        $('#RetrieveDetail').append(tr);
        //    }
        //    else {

        //    }
        //}, 'json');
        $.post('/Distribution/BatchProductionPage', { queryId: queryId }, function (result) {
            var msg = "";
            if (result.data.length > 0) {
                $.each(result.data[0].BoxNo, function (i,Box) {
                    msg += Box.BoxNoes + ',';
                })
                $('#batchId').text(result.data[0].OddIdNo);
                $('#batchSa').text(StatusToName(result.data[0].Status));
                $('#batch1').text(result.data[0].CardTypeName);
                $('#batch2').text(result.data[0].Production);
                $('#batch3').text(result.data[0].BeginCardNo);
                $('#batch4').text(result.data[0].EndCardNo);
                $('#batch5').text(result.data[0].ArriveTime);
                $('#batch6').text(msg);
                $('#batch7').text(result.data[0].Purpose[0].BoxPurpose == "0" ? "发售" : "补发");
                $('#batch8').text(ExecuteToName(result.data[0].IsExecute));
                $('#batch10').text(result.data[0].LastBoxNo);
                $('#batch9').text(result.data[0].LastCardNo);
                //$('#batch11').text(result.data[0].CreateBy);
                $('#batch12').text(result.data[0].CreateBy);
                $('#batch13').text(result.data[0].CreateBy);
                $('#batch14').text(result.data[0].CreateBy);
                if (result.data[0].Status == "0") {
                    $('#VerifyTrue').css('display', '');
                    $('#VerifyFalse').css('display', '');
                }
            }
        }, 'json');

        $.post('/Distribution/BoxAndCard', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.BoxNo + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td></tr>";
                })
                $('#Batchshowinfo2').append(tr);
            }
            else {

            }
        }, 'json');
    }



    if (key == "CardOutTitle") {
        $('#CardOutTitle').show();      
        $.post('/Distribution/CardOutTitleDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                var tr1 = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.CardTypeName + "</td><td>" + item.Purpose + "</td><td>" + item.BoxNumber + "</td><td>" + item.CardNumIn + "</td></tr>";
                    //tr1 += "<tr><td>" + item.CardTypeName + "</td><td>" + item.BoxNumber + "</td><td>" + item.CardSum + "</td></tr>"
                })
                $('#titleshowinfo').append(tr);
                //$('#titleshowinfo1').append(tr1);
         
            }
            else {

            }
        }, 'json');
        $.post('/Distribution/CardOutTitlePage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                $('#cardOutTitle1').text(result.data[0].OddIdNo);
                $('#cardOutTitle2').text(StatusToName(result.data[0].Status));
                $('#cardOutTitle3').text(result.data[0].SendingUnit);
                $('#cardOutTitle4').text(result.data[0].AcceptintUnit);
                //$('#cardOutTitle5').text('');
                //$('#cardOutTitle6').text('');
                $('#cardOutTitle7').text(result.data[0].CreateBy);
                $('#cardOutTitle8').text(result.data[0].CreateBy);
                $('#cardOutTitle9').text(result.data[0].CreateBy);
                $('#cardOutTitle10').text(IsOddId(result.data[0].IsOddId));
                if (result.data[0].Status == "0") {
                    $('#VerifyTrue').css('display', '');
                    $('#VerifyFalse').css('display', '');
                }
            }
        }, 'json');

        $.post('/Distribution/CardOutTitleDetailPage1', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.BoxNo + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td></tr>";
                })
                $('#titleshowinfo2').append(tr);
            }
            else {

            }
        }, 'json');


    }


    if (key == "CardIn") {
        $('#CardIn').show();
        $.post('/Distribution/CardOutTitleDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                var tr1 = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.CardTypeName + "</td><td>" + item.Purpose + "</td><td>" + item.BoxNumber + "</td><td>" + item.CardNumIn + "</td><td>" + StatusToName(item.Status) + "</td></tr>";
                    //tr1 += "<tr><td>" + item.CardTypeName + "</td><td>" + item.BoxNumber + "</td><td>" + item.CardSum + "</td></tr>"
                })
                $('#cardinshowinfo').append(tr);
                //$('#cardinshowinfo1').append(tr1);

            }
            else {

            }
        }, 'json');
        $.post('/Distribution/CardOutTitlePage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                $('#cardIn1').text(result.data[0].OddIdNo);
                $('#cardIn2').text(StatusToName(result.data[0].Status));
                $('#cardIn3').text(result.data[0].SendingUnit);
                $('#cardIn4').text(result.data[0].AcceptintUnit);
                //$('#cardIn5').text('');
                //$('#cardIn6').text('');
                $('#cardIn7').text(result.data[0].CreateBy);
                $('#cardIn8').text(result.data[0].CreateBy);
                $('#cardIn9').text(result.data[0].CreateBy);
                $('#cardIn10').text(IsOddId(result.data[0].IsOddId));
            }
        }, 'json');

        $.post('/Distribution/CardOutTitleDetailPage1', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.BoxNo + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td></tr>";
                })
                $('#cardinshowinfo2').append(tr);
            }
            else {

            }
        }, 'json');


    }


    if (key == "CardOutBranch") {
        $('#CardOutBranch').show();    
        $.post('/Distribution/CardOutBranchDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                var tr1 = "";
                var cardNum = 0;
                $.each(result.data, function (i, item) {                
                    cardNum += item.CardNumIn;
                })
                tr += "<tr><td>" + result.data[0].CardTypeName + "</td><td>" + result.data[0].Purpose + "</td><td>" + result.data[0].BoxNumber + "</td><td>" + cardNum + "</td></tr>";
                $('#branchshowinfo').append(tr);
                //$('#branchshowinfo1').append(tr1);

            }
            else {

            }
        }, 'json');
        $.post('/Distribution/CardOutBranchPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                $('#cardOutbranch1').text(result.data[0].OddIdNo);
                $('#cardOutbranch2').text(StatusToName(result.data[0].Status));
                $('#cardOutbranch3').text(result.data[0].SendingUnit);
                //$('#cardOutbranch5').text(result.data[0].AcceptintUnit);
                //$('#cardOutbranch5').text('');
                $('#cardOutbranch6').text('');
                $('#cardOutbranch7').text(result.data[0].CreateBy);
                $('#cardOutbranch8').text(result.data[0].CreateBy);
                $('#cardOutbranch9').text(result.data[0].CreateBy);
                $('#cardOutbranch10').text(IsOddId(result.data[0].IsOddId));
                if (result.data[0].Status == "0") {
                    $('#VerifyTrue').css('display', '');
                    $('#VerifyFalse').css('display', '');
                }
            }
        }, 'json');

        $.post('/Distribution/CardOutBranchDetailPage1', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.BoxNo + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td><td>" + item.StoreName + "</td></tr>";
                })
                $('#branchshowinfo2').append(tr);
            }
            else {

            }
        }, 'json');


    }
    if (key == "CardRepeal") {
        $('#CardRepeal').show();
        
        $.post('/Distribution/CardRepealDetailPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                var tr1 = "";
                var card = 0;
                var boxNum = 0;
                $.each(result.data, function (i, item) {                
                    card += parseInt(item.CardNum);
                    boxNum++;
                })
                tr = "<tr><td>" + result.data[0].CardTypeName+ "</td><td>" + result.data[0].Purpose+ "</td><td>" + boxNum + "</td><td>" + card + "</td></tr>";
                //tr1 += "<tr><td>" + result.data[0].CardTypeName + "</td><td>" + result.data.length + "</td><td>" + card + "</td></tr>"
                $('#repealshowinfo').append(tr);
                //$('#repealshowinfo1').append(tr1);

            }
            else {

            }
        }, 'json');
        $.post('/Distribution/CardRepealPage', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                if (result.data[0].Status==0) {
                    $('#VerifyTrue').css('display', '');
                    $('#VerifyFalse').css('display', '');
                }
                if (result.data[0].RepealType==1) {
                    $('#CardRepealTitle').text("分公司卡退领详情");
                    $('#CardRepeal3').text(result.data[0].SendingUnitBranch);
                    $('#CardRepeal4').text(result.data[0].AcceptintUnit);
                } else if (result.data[0].RepealType == 0) {
                    $('#CardRepealTitle').text("门店卡退领详情");
                    $('#CardRepeal3').text(result.data[0].SendingUnitStore);
                    $('#CardRepeal4').text(result.data[0].AcceptintUnitStore == "" ? result.data[0].AcceptintUnit : result.data[0].AcceptintUnitStore);
                } else {
                    $('#CardRepealTitle').text("总部卡退领详情");
                    $('#CardRepeal3').text(result.data[0].SendingUnit);
                    $('#CardRepeal4').text(result.data[0].AcceptintUnitGroup);
                }
                $('#CardRepeal1').text(result.data[0].OddIdNo);
                $('#CardRepeal2').text(StatusToName(result.data[0].Status));

                //$('#cardOutbranch5').text('');
                $('#CardRepeal6').text('');
                $('#CardRepeal7').text(result.data[0].CreateBy);
                $('#CardRepeal8').text(result.data[0].CreateBy);
                $('#CardRepeal9').text(result.data[0].CreateBy);
            }
        }, 'json');

        $.post('/Distribution/CardRepealDetailPage1', { queryId: queryId }, function (result) {
            if (result.data.length > 0) {
                var tr = "";
                $.each(result.data, function (i, item) {
                    tr += "<tr><td>" + item.BoxNo + "</td><td>" + item.BeginCardNo + "</td><td>" + item.EndCardNo + "</td></tr>";
                })
                $('#repealshowinfo2').append(tr);
            }
            else {

            }
        }, 'json');


    }
    $('#VerifyTrue').click(function () {   
        var postdata = {
            key: key,
            queryId: queryId,
            status: 1
        };
        $.post('/Distribution/VerifyTrue', postdata, function (result) {
            if (result.IsPass) {
                $.dialog("操作成功");
                $('#VerifyTrue').prop('disabled', true);
                $('#VerifyFalse').prop('disabled', true);
            }
            else {
                $.dialog("操作失败," + result.MSG);
            }
        }, 'json')
    })
    $('#VerifyFalse').click(function () {
        var postdata = {
            key: key,
            queryId: queryId,
            status: 2
        };
        $.post('/Distribution/VerifyTrue', postdata, function (result) {
            if (result.IsPass) {
                $.dialog("操作成功");
                $('#VerifyTrue').prop('disabled', true);
                $('#VerifyFalse').prop('disabled', true);
            }
            else {
                $.dialog("操作失败,"+result.MSG);
            }
        }, 'json')
    })
})


$('#BoxDetail').click(function () {
    var op = btnCount % 2;
    var $showCardDetail = $('#showCardDetail');
    if (op == 0) {
        $showCardDetail.hide();
    }
    else {
        $showCardDetail.show();
    }
    btnCount++;
})


function StatusToName(status)
{
    var name = "";
    if (status == "0") {
        name = "未审核";
    }
    else if (status == "1") {
        name = "审核通过";
    }
    else {
        name = "已撤销";
    }
    return name;
}

function ExecuteToName(status)
{
    var name = "";
    if (status == "0") {
        name = "未交货";
    }
    else if (status == "1") {
        name = "待交货";
    }
    else if (status == "2") {
        name = "部分交货";
    }
    else if (status == "3") {
        name = "全部交货";
    }
    else {
        name = "拒绝交货";
    }
    return name;
}

function IsOddId(isOddId)
{
    var name = "";
    if (isOddId == "0" || isOddId == null) {
        name = "否";
    }
    else {
        name = "是";
    }
    return name;
}