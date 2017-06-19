using System;
using System.Collections.Generic;
using System.Linq;

namespace Arvato.CRM.Utility.WorkFlow
{
    /// <summary>
    /// 活动模板
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// 执行结果的类型
        /// </summary>
        public ActivityResultType ResultType { set; get; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public Guid Id { set; get; }

        /// <summary>
        /// 活动分类
        /// </summary>
        public ActivityCategory Category { set; get; }

        /// <summary>
        /// 活动状态
        /// 默认为 Normal
        /// </summary>
        public ActivityState State { set; get; }

        /// <summary>
        /// 等待时间
        /// 等待反馈属性
        /// 等待N小时后执行
        /// 数值必须大于等于0
        /// </summary>
        public double Wait { set; get; }

        /// <summary>
        /// 活动执行条件
        /// 流程分支属性
        /// </summary>
        public string Condition { set; get; }

        /// <summary>
        /// 发送邮件
        /// 优惠券属性
        /// </summary>
        public bool SendMail { set; get; }

        /// <summary>
        /// 发送短信
        /// 优惠券属性
        /// </summary>
        public bool SendSMS { set; get; }

        /// <summary>
        /// 发送渠道
        /// 问卷属性
        /// </summary>
        public string SendChannel { set; get; }

        /// <summary>
        /// 子活动
        /// </summary>
        public List<Activity> Children { set; get; }

        /// <summary>
        /// 执行结果，需要逻辑层填写
        /// </summary>
        public string Result { set; get; }

        /// <summary>
        /// 有效期
        /// 0：没有有效期
        /// 默认为 0
        /// </summary>
        public double ValidDay { set; get; }

        /// <summary>
        /// 模板编号
        /// </summary>
        public int TemplateId { set; get; }

        /// <summary>
        /// 预计执行时间
        /// </summary>
        public DateTime? RunDate { set; get; }

        /// <summary>
        /// 执行的时间
        /// </summary>
        public DateTime? ExecutedDate { set; get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpiredDate
        {
            get
            {
                var validday = ValidDay;
                if (validday == 0 || RunDate == null)
                {
                    return null;
                }
                return RunDate.Value.AddDays(ValidDay);
            }
        }

        public bool? DataLimit { set; get; }

        public string LimitType { set; get; }

        public List<string> LimitValues { set; get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public Activity()
        {
            Id = Guid.NewGuid();
            ValidDay = 0;
            Wait = 0;
            //State = ActivityState.Normal;
            //Category = ActivityCategory.SMS;
            Children = new List<Activity>();
            LimitValues = new List<string>();
        }

        /// <summary>
        /// 获取指定活动
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sources">活动列表</param>
        /// <returns></returns>
        public static Activity Find(Func<Activity, bool> predicate, IEnumerable<Activity> sources)
        {
            if (sources.Any(predicate))
            {
                return sources.FirstOrDefault(predicate);
            }
            var chd = sources.SelectMany(p => p.Children);
            if (!chd.Any())
            {
                return null;
            }
            return Find(predicate, chd);
        }
    }
}
