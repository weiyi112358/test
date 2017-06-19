using System;
using System.Text.RegularExpressions;

namespace Arvato.CRM.Utility.WorkFlow
{
    //{"runType":"1","planDate":"2014-04-10","planTime":"02:15 AM","runCycle":"Day","planCycle":"First","planDay":"1","planTime1":"","planWeek":"Monday"}
    public class Schedule
    {
        public RunType RunType { set; get; }

        public string PlanDate { set; get; }

        public string PlanTime { set; get; }

        public RunCycle RunCycle { set; get; }

        public PlanCycle PlanCycle { set; get; }

        public int PlanDay { set; get; }

        public string PlanTime1 { set; get; }

        public DayOfWeek PlanWeek { set; get; }

        public CustomerSeltctType CustomerSelectType { get; set; }
        public float? CustomerRate { get; set; }
        public int? CustomerAmount { get; set; }

        public bool Calc()
        {
            switch (RunType)
            {
                case RunType.Cycle:
                    var curDate = DateTime.Now.Date;
                    var curDateStr = curDate.ToString("yyyy-MM-dd ");
                    int hour;
                    if (!int.TryParse(PlanTime1.Split(':')[0], out hour)) return false;
                    if (hour > 12) PlanTime1 = PlanTime1.Substring(0, 5);
                    switch (RunCycle)
                    {
                        case RunCycle.Day:
                            return DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                        case RunCycle.Week:
                            return curDate.DayOfWeek == PlanWeek && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                        case RunCycle.Month:
                            switch (PlanCycle)
                            {
                                case PlanCycle.First:
                                    return curDate.Day == 1 && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                                case PlanCycle.Last:
                                    //return new DateTime(curDate.Year, curDate.Month + 1, 1).AddDays(-1).Day == curDate.Day && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                                    return new DateTime(curDate.AddMonths(1).Year, curDate.AddMonths(1).Month, 1).AddDays(-1).Day == curDate.Day && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                                case PlanCycle.Assign:
                                    return curDate.Day == PlanDay && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                            }
                            return false;
                        case RunCycle.Year:
                            switch (PlanCycle)
                            {
                                case PlanCycle.First:
                                    return curDate.DayOfYear == 1 && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                                case PlanCycle.Last:
                                    //return new DateTime(curDate.Year + 1, 1, 1).AddDays(-1).Day == curDate.DayOfYear && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                                    return new DateTime(curDate.Year + 1, 1, 1).AddDays(-1).DayOfYear == curDate.DayOfYear && DateTime.TryParse(curDateStr + PlanTime1, out curRunDate);
                            }
                            return false;
                    }
                    return false;
                case RunType.Once:
                    int hour1;
                    if (!int.TryParse(PlanTime.Split(':')[0], out hour1)) return false;
                    if (hour1 > 12) PlanTime = PlanTime.Substring(0, 5);
                    return DateTime.TryParse(PlanDate + " " + PlanTime, out curRunDate);
            }
            return false;
        }

        private DateTime curRunDate;

        /// <summary>
        /// 计算出来的运行时间
        /// </summary>
        public DateTime CurRunDate
        {
            get
            {
                return curRunDate;
            }
        }
    }

    /// <summary>
    /// 执行周期
    /// </summary>
    public enum RunType
    {
        /// <summary>
        /// 周期执行
        /// </summary>
        Cycle = 0,
        /// <summary>
        /// 只执行一次
        /// </summary>
        Once
    }

    /// <summary>
    /// 时间间隔
    /// </summary>
    public enum RunCycle
    {
        /// <summary>
        /// 每天
        /// </summary>
        Day,
        /// <summary>
        /// 每周
        /// </summary>
        Week,
        /// <summary>
        /// 每月
        /// </summary>
        Month,
        /// <summary>
        /// 每年
        /// </summary>
        Year
    }

    /// <summary>
    /// 月份的那一天，或年份的那一天
    /// </summary>
    public enum PlanCycle
    {
        /// <summary>
        /// 第一天
        /// </summary>
        First,
        /// <summary>
        /// 最后一天
        /// </summary>
        Last,
        /// <summary>
        /// 指定几号
        /// </summary>
        Assign
    }

    /// <summary>
    /// 执行人数选择方式
    /// </summary>
    public enum CustomerSeltctType
    {
        /// <summary>
        /// 比例
        /// </summary>
        Rate,
        /// <summary>
        /// 数量
        /// </summary>
        Amount
    }
}
