/* [ ---- Gebo js_update for 安正 ---- ] */

    //* bootstrap datepicker
	gebo_datepicker = {
		init: function() {
			$('#dp_single,#dp_single2').datepicker();
			
			
			// disabling dates
			var nowTemp = new Date();
			var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
	
			var checkin = $('#dp_start').datepicker({
			  onRender: function(date) {
				return date.valueOf() < now.valueOf() ? 'disabled' : '';
			  }
			}).on('changeDate', function(ev) {
			  if (ev.date.valueOf() > checkout.date.valueOf()) {
				var newDate = new Date(ev.date)
				newDate.setDate(newDate.getDate() + 1);
				checkout.setValue(newDate);
			  }
			  checkin.hide();
			  $('#dp_end')[0].focus();
			}).data('datepicker');
			var checkout = $('#dp_end').datepicker({
			  onRender: function(date) {
				return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
			  }
			}).on('changeDate', function(ev) {
			  checkout.hide();
			}).data('datepicker');
		}
	};
	
	//* bootstrap timepicker
	gebo_timepicker = {
		init: function() {
			$('#tp_drop').timepicker({
				defaultTime: 'current',
				minuteStep: 1,
				disableFocus: true,
				template: 'dropdown'
			});
		}
	};
	
	//* multiselect
	gebo_multiselect = {
		init: function(){
			
			if($('#public-methods').length) {
                //* public methods
                $('#public-methods').multiSelect();
                $('#select-all').click(function(){
                    $('#public-methods').multiSelect('select_all');
                    return false;
                });
                $('#deselect-all').click(function(){
                    $('#public-methods').multiSelect('deselect_all');
                    return false;
                });
                $('#select-fr').click(function(){
                    $('#public-methods').multiSelect('select', 'fr');
                    return false;
                });
                $('#deselect-fr').click(function(){
                    $('#public-methods').multiSelect('deselect', 'fr');
                    return false;
                });
            }
            if($('#optgroup').length) {
                //* optgroup
                $('#optgroup').multiSelect()
            }
			if($('#custom-headers').length) {
                //* custom headers
                $('#custom-headers').multiSelect({
                    selectableHeader: "<div class='custom-header'>Selectable item</div>",
                    selectionHeader: "<div class='custom-header'>Selected items</div>"
                });
            }
            if($('#searchable').length) {
                //* searchable
                $('#searchable').multiSelect({
                    selectableHeader: '<div class="search-header"><input type="text" class="span12" id="ms-search" autocomplete="off" placeholder="country name"></div>',
                    selectionHeader: "<div class='search-selected'></div>"
                });
            }
            if($('#ms-search').length) {  
                $('#ms-search').quicksearch($('.ms-elem-selectable', '#ms-searchable' )).on('keydown', function(e){
                    if (e.keyCode == 40){
                        $(this).trigger('focusout');
                        $('#ms-searchable').focus();
                        return false;
                    }
                })
            }

		}
	};
	
	//* enhanced select elements
	gebo_chosen = {
		init: function(){
			$(".chzn_a").chosen({
				allow_single_deselect: true
			});
			
			if($(".chzn_a").attr('disabled')=='disabled'){
				$(".chzn_a").next('.chzn-container').attr('disabled','disabled')
			}
			
			$(".chzn_b").chosen();
		}
	};
	
	//* select all rows
	gebo_select_row = {
		init: function() {
			$('.select_rows').click(function () {
				var tableid = $(this).data('tableid');
                $('#'+tableid).find('input[name=row_sel]').attr('checked', this.checked);
			});
		}
	};
	
    //* gallery table view
    gebo_galery_table = {
        init: function() {
           $('#dt_table').dataTable({
				"oLanguage": {
					"sLengthMenu": "每页显示 _MENU_ 条数据",
					"sZeroRecords": "No data to display"
				},
				"sDom": "<'row'<'span5'<'dt_actions'>l><'span5'f>r>t<'row'<'span5'i><'span5'p>>",
				"sPaginationType": "bootstrap",
				"iDisplayLength": 10,
				"aaSorting": [[ 5, "desc" ]],
				"aoColumns": [
					{ "sType": "formatted-num" , 'sWidth': '30px'},
					{ "sType": "string" },
					{ "sType": "string" }
				]
			});
        }
    };
	
	gebo_common_table = {
        init: function() {
           $('#common_table').dataTable({
				"oLanguage": {
					"sLengthMenu": "每页显示 _MENU_ 条数据",
					"sZeroRecords": "No data to display"
				},
				"sDom": "<'row'<'span5'<'dt_actions'>l><'span5'f>r>t<'row'<'span5'i><'span5'p>>",
				"sPaginationType": "bootstrap",
				"iDisplayLength": 10,
				"aaSorting": [[ 5, "desc" ]],
				"aoColumns": [
					{ "sType": "formatted-num" , 'sWidth': '30px'},
					{ "sType": "string" },
					{ "sType": "string" }
				]
			});
        }
    };
	
	/***************************其他自定义脚本*******************************/
	//moreList
	$('.moreList .slide-nav').click(function(){
		if($(this).children('').hasClass('icon-chevron-down')){
			$(this).children('').removeClass('icon-chevron-down').addClass('icon-chevron-up');
			$(this).parent().siblings('.moreHid').slideDown(200);
		}else{
			$(this).children('').removeClass('icon-chevron-up').addClass('icon-chevron-down');
			$(this).parent().siblings('.moreHid').removeClass('firstShow').slideUp(200);
		}
	})
	
	
	//市场活动配置缩略图树
	$(function(){
        if($("#tree_workflow").size()>0) $("#tree_workflow").dynatree({
            onActivate: function(node) {
                // A DynaTreeNode object is passed to the activation handler
                // Note: we also get this event, if persistence is on, and the page is reloaded.
                //alert("You activated " + node.data.title);
            },
            persist: true,
            children: [ // Pass an array of nodes.
                {title: "活动阶段",iconClass:"icon-campaign"},
                {title: "流程分支", iconClass:"icon-branch",expand: true,
                    children: [
                        {title: "优惠券模版",iconClass:"icon-coupon",expand: true,
						    children: [
								{title: "邮件沟通",iconClass:"icon-mail"},
								{title: "等待反馈",iconClass:"icon-feedback",expand: true,
								    children: [
									    {title: "邮件沟通",iconClass:"icon-mail"},
								        {title: "等待反馈",iconClass:"icon-feedback"}
								    ]
								}
							]
						},
                        {title: "短信沟通",iconClass:"icon-sms"}
                    ]
                },
                {title: "外呼沟通",iconClass:"icon-call"},
                {title: "优惠券模版",iconClass:"icon-coupon"},
                {title: "等待反馈",iconClass:"icon-feedback"}
            ]			
			
        });
		//市场活动配置缩略图树的选中效果
		$('.dynatree-node').live('click',function(){
			$('.dynatree-container').find('.dynatree-active-children').removeClass('dynatree-active-children')	
			if($(this).hasClass('dynatree-has-children'))$(this).next('ul').addClass('dynatree-active-children')	
		})
		
		/*loading exmaple*/
		
		$('#showLoad').click(function(){
			//0为手动关闭loading层，1000为1秒后关闭；e.g.此处举例查询模版 按“保存”按钮后显示loading层
			loadLayer=$.loadBg('处理中，请稍后(点此关闭)...',5000);
		})	
		$('.loadText').live("click",function(){
			$.removeLoad(loadLayer);//需要主动关闭loading的层的调用此函数；e.g.此处举例销售页面点击"loadText层后"直接隐藏本loading
		})
	})
	
	var loadLayer,waitTimer;
	
	$.extend({		
		//loading效果插件
		loadBg : function(str,timeout){	
			str = str || '';
			timeout = timeout || 0;
			
			var loadName='load'+$.getRandom(999);
			$("#contentwrapper").append('<div id="'+loadName+'" class="loadBg"><div class="loadText">'+str+'</div></div>');
			var loadObj=$("#"+loadName);
			$.colorbox({
				initialHeight: '0',
				initialWidth: '0',
				overlayClose:false,
				href: "#"+loadName,
				inline: true,
				opacity: '0.3',
				showLoading: false,
				closeButton:false,
				onOpen: function(){
				   $('#cboxClose').css('visibility','hidden');	
				},
				onComplete: function(){
					if(timeout> 0) waitTimer = setTimeout(function(){
						$.removeLoad(loadObj); 						
					}, timeout);					
				},
				onClosed: function(){
					timeout=0;
					$('#cboxClose').css('visibility','visible');
				}
			});
			return loadObj;
			
		},	
		removeLoad: function(obj){
			clearTimeout(waitTimer);
			$.colorbox.close();
			$(document).bind('cbox_closed', function(){ 
				if(obj.size()>0) obj.remove();
				$(document).unbind('cbox_closed');
			});
		},
		getRandom: function(n){
			return Math.floor(Math.random()*n+1)
		}
		
	});
