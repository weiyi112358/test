using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arvato.CRM.Model
{
    public class MarketActivityFilterModel
    {
        [RegularExpression(@"\d+", ErrorMessage = "{0}格式不正确")]
        [Display(Name = "活动编号")]
        public int? ActivityID { set; get; }

        [Display(Name = "活动名称")]
        public string ActivityName { set; get; }

        [Display(Name = "启用状态")]
        public bool? Enable { set; get; }

        [Display(Name = "群组")]
        public int? DataGroupID { set; get; }

        [Display(Name = "门店")]
        public string StoreCode { set; get; }

        [Display(Name = "计划开始时间")]
        public DateTime? PlanStartTimeFrom { set; get; }

        [Display(Name = "计划开始时间")]
        public DateTime? PlanStartTimeEnd { set; get; }

        [Display(Name = "计划结束时间")]
        public DateTime? PlanEndDateFrom { set; get; }

        [Display(Name = "计划结束时间")]
        public DateTime? PlanEndDateEnd { set; get; }

        [Display(Name = "商业计划")]
        public string BusinessPlanID { set; get; }

        [Display(Name = "审批状态")]
        public short? Status { set; get; }
    }
}
