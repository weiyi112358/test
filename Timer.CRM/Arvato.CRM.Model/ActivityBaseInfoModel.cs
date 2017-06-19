using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class ActivityBaseInfoModel
    {/// <summary>
        /// 活动编号
        /// </summary>
        [Display(Name = "活动编号")]
        public int ActivityID { set; get; }

        [Display(Name = "群组编号")]
        public int DataGroupID { set; get; }
        /// <summary>
        /// 活动名称
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "活动名称")]
        public string ActivityName { set; get; }


        /// <summary>
        /// 计划开始日期
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "计划开始日期")]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "{0}格式不正确")]
        public string PlanStartDate { set; get; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "计划开始时间")]
        [RegularExpression(@"\d{2}:\d{2} (PM|AM)", ErrorMessage = "{0}格式不正确")]
        public string PlanStartTime { set; get; }

        /// <summary>
        /// 计划结束日期
        /// </summary>
        [Display(Name = "计划结束日期")]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "{0}格式不正确")]
        public string PlanEndDate { set; get; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Display(Name = "计划结束时间")]
        [RegularExpression(@"\d{2}:\d{2} (PM|AM)", ErrorMessage = "{0}格式不正确")]
        public string PlanEndTime { set; get; }

        [Display(Name = "项目开始时间")]
        public string ProStartDate { set; get; }

        [Display(Name = "项目结束时间")]
        public string ProEndDate { set; get; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "引用编号")]
        public string ReferenceNo { get; set; }

        [Display(Name = "商业计划名称")]
        public string BusinessPlanID { set; get; }
    }
}
