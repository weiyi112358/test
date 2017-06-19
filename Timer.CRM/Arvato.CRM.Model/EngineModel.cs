using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.EF;

namespace Arvato.CRM.Model
{
    //条件组合
    public class Ral
    {
        Ral() { Rfl = new List<SubRal>(); }
        public string r { get; set; }
        public List<SubRal> Rfl { get; set; }
    }

    //子条件组合
    public class SubRal
    {
        SubRal() { srfl = new List<Exp>(); }
        public string r { get; set; }
        public List<Exp> srfl { get; set; }
    }

    //表达式
    public struct Exp
    {
        public string l { get; set; }
        public string e { get; set; }
        public string r { get; set; }
    }

    //public class DynamicRightValue
    //{
    //    public string rightValue { get; set; }
    //    public List<DynamicParameter> parameters { get; set; }
    //}

    //public struct DynamicParameter
    //{
    //    public int parameterIndex { get; set; }
    //    public string parameterValue { get; set; }
    //}

    //会员细分日程
    public struct Schedule
    {
        public string type { get; set; }
        public string cycle { get; set; }
        public string d { get; set; }
        public string ap { get; set; }
    }

    //忠诚度日程
    public struct LoyaltySchedule
    {
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Remark { get; set; }
    }

    //规则用条件结果
    public struct ConditionResult
    {
        public string FieldAlias { get; set; }
        public decimal? Maximum { get; set; }
        public decimal? Minimum { get; set; }
        public string GroupFunc { get; set; }//sum,avg,first,max等SQL汇总计算函数
        public string OffsetExpression { get; set; }//+=,*=
        public decimal? OffsetValue { get; set; }
        public bool IsABS { get; set; }//是否取绝对值
        public string NameCode { get; set; }
    }

    //行为组合
    public struct Act
    {
        public ActLeftValue LeftValue { get; set; }
        public string Expression { get; set; }//+=,=
        public string RightValue { get; set; }
        public string RightValueMax { get; set; }
        public string RightValueMin { get; set; }
        public string RightValueFilterAlias { get; set; }
        public string OffsetExpression { get; set; }//+,*
        public string OffsetValue { get; set; }
        public string OffsetUnit { get; set; }
        public short Sort { get; set; }
        public string FreezeValue { get; set; }//冻结
        public string FreezeUnit { get; set; }//单位
        public string AvailabeValue { get; set; }//有效
        public string AvailabeUnit { get; set; }//单位
        public string OffsetDay { get; set; }
        public string OffsetMonth { get; set; }
    }

    //行为左值扩展
    public struct ActLeftValue
    {
        public string ExtName { get; set; }//{FieldAlias};Account
        public string ExtType { get; set; }//FieldAlias;AccountType
        public List<string> ExtLimitList { get; set; }//none;none,vehicle,registerStore,tradeStore,brand
    }

    //RuleMode
    public class Rules
    {
        public int? RuleID { get; set; }
        public string RuleName { get; set; }
        public int DataGroupID { get; set; }
        public string RuleType { get; set; }
        public string RuleRunType { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Schedule { get; set; }
        public string Condition { get; set; }
        public int RunIndex { get; set; }
        public string ConditionResult { get; set; }
        public string Action { get; set; }
        public Nullable<System.DateTime> LastExcuteTime { get; set; }
        public bool Enable { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
    }
    public class RuleModel
    {
        /// <summary>
        /// 规则编号
        /// </summary>
        public string RuleID { set; get; }

        public int DataGroupID { set; get; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { set; get; }

        /// <summary>
        /// 规则类型
        /// </summary>
        public string RuleType { set; get; }

        /// <summary>
        /// 优先级
        /// </summary>
        //[MaxLength(2)]
        public int RunIndex { set; get; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public string Enable { set; get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { set; get; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? LastExecTime { set; get; }

        /// <summary>
        /// 条件
        /// </summary>
        public string Condition { set; get; }


        public string ConditionResult { get; set; }

        /// <summary>
        /// 行为
        /// </summary>
        public string Actions { set; get; }

        /// <summary>
        /// 调度
        /// </summary>
        public string Schedule { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 调度类型
        /// </summary>
        public string ScheduleType { set; get; }

        /// <summary>
        /// 调度子类型
        /// </summary>
        public string ScheduleSubType { set; get; }

        /// <summary>
        /// 调度日参数
        /// </summary>
        public string ScheduleDay { set; get; }

        /// <summary>
        /// 调度时间参数
        /// </summary>
        public string ScheduleTime { set; get; }

    }

    public class LoyShare
    {
        public string InterfaceName { get; set; }
        public string memberScript { get; set; }
        public string searchTradeDetailSQL { get; set; }
        public string searchTradeSQL { get; set; }
        public bool needPointSMS { get; set; }
    }

}
