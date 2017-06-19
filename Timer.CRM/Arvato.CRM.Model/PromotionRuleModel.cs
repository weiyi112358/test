using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    #region 促销规则 模板
    public class PromotionRuleTemplate
    { 
        public string Name { get;private set; }

        public PromotionRuleBaseInfo BaseInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PromotionRuleCondition RuleCondition  { get; set; }
        /// <summary>
        /// 促销动作
        /// </summary>
        public PromotionRuleActionEnum Action { get; set; }
    }

    #endregion

    #region Enum
    /// <summary>
    /// 执行状态
    /// </summary>
    public enum PromotionExecuteStatus
    {
        未开始 =0,
        休眠中 = 1,
        执行中 =2,
        已结束 =3,
    }
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum PromotionApproveStatus
    {
        未审核 =0,
        已审核 =1,
        已作废 =2 
    }

    #endregion 

    #region 促销 包含的功能定义

    public class PromotionRuleBaseInfo
    { 
        public bool HasStores { get; set; }

        public bool HasLimit { get; set; }
    }

    public enum PromotionRuleConditionTypeEnum
    {
         Decimal ,
         Select ,
         Birthday,
         DayRegion,
         MemberLevelMulti
    }

    /// <summary>
    /// 促销条件
    /// </summary> 
    [Serializable]
    public class PromotionRuleCondition
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PromotionRuleConditionTypeEnum Type { get; set; }
        public bool IsRequire { get; set; } 

        public List<int> UsedInWhichPromotion { get; set; } 
    }


    #endregion

    #region 促销动作

    public enum PromotionRuleActionEnum
    {
        None = 0,
        /// <summary>
        /// 会员开卡
        /// </summary>
        MemberCard = 1,
        /// <summary>
        /// 现金开卡
        /// </summary>
        CashCard = 2, 
        /// <summary>
        /// 补发卡
        /// </summary>
        PatchCard = 4,
        /// <summary>
        /// 赠送积分
        /// </summary>
        PointAmount = 8,
        /// <summary>
        /// 积分倍数
        /// </summary>
        PointMulti = 16,
        /// <summary>
        /// 会员促销（折扣）
        /// </summary>
        MemberDiscount = 32,
        /// <summary>
        /// 礼品促销
        /// </summary>
        MemberGift = 64,

    }
    /// <summary>
    /// 促销动作
    /// </summary>
    public class PromotionAction
    {
        /// <summary>
        /// 使用的模板
        /// </summary>
        public PromotionRuleTemplate Tempalte { get; protected set; }
        /// <summary>
        /// 序列化到Xml
        /// </summary>
        /// <returns></returns>
        public virtual string SerizeToXml()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 会员开卡
    /// </summary>
    public class MemberCardAction: PromotionAction
    { 
        public MemberCardAction()
        {
             
        }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardTypeCode { get; set; }
        /// <summary>
        /// 卡名称
        /// </summary>
        public string CardTypeName { get; set; }
        /// <summary>
        /// 是否计算积分
        /// </summary>
        public bool IsCalculateScore { get; set; }
        /// <summary>
        /// 是否享受会员价
        /// </summary>
        public bool IsMemberPrice { get; set; }
        /// <summary>
        /// 积分倍数
        /// </summary>
        public int?  ScoreMulti { get; set; }
    }

    public class CashCardAction:PromotionAction
    {
        public CashCardAction()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public string CardTypeCodeName { get; set; }
        /// <summary>
        /// 开卡金额 
        /// </summary>
        public decimal CardCost { get; set; } 
    }

    public class PatchCardAction : PromotionAction
    {
        public PatchCardAction()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string CardTypeCodeName { get; set; }
        /// <summary>
        /// 补卡金额 
        /// </summary>
        public decimal CardCost { get; set; }
    }

    public class PointAmountAction : PromotionAction
    {
        public PointAmountAction()
        {

        }

        /// <summary>
        /// 积分类型
        /// select * from td_sys_bizoption
        /// where optiontype = 'AccountChangeType'
        /// </summary>
        public int PointType { get; set; }
        /// <summary>
        /// 积分值  
        /// </summary>
        public int Point { get; set; }
    }

    public class PointMultiAction : PromotionAction
    {
        public PointMultiAction()
        {

        }
         
        /// <summary>
        /// 积分倍数
        /// </summary>
        public int PointMulti { get; set; }
    }

    public class MemberDiscountAction : PromotionAction
    {
        public MemberDiscountAction()
        {

        }

        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal? DiscountRate { get; set; }

        public decimal ? DiscountValue { get; set; }
    }


    public class MemberGiftAction : PromotionAction
    {
        public MemberGiftAction()
        {

        }

        /// <summary>
        /// 礼品：select * from [V_M_TM_SYS_BaseData_goods]
        /// </summary>
        public int GiftID { get; set; } 
    }




    #endregion

    #region 促销规则 

    //RuleMode
    public class PromotionRule
    {


        public string BillID { get; set; }
        public string BillCode { get; set; }
        //public int DataGroupID { get; set; }
        public string PromotionType { get; set; }

        public string PromotionTypeDesc { get; set; }

        //public string RuleRunType { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        public string Schedule { get; set; }
        public bool IsSetSchedule { get; set; }
        public string Condition { get; set; }
        public string AllowStores { get; set; }
        public string AllowGoods { get; set; }

        public int RunIndex { get; set; }
        public string ConditionResult { get; set; }
        public string Action { get; set; }
        public Nullable<System.DateTime> LastExcuteTime { get; set; }

        public PromotionExecuteStatus ExecuteStatus { get; set; }
        public PromotionApproveStatus ApproveStatus { get; set; }


        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string ApproveUser { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 限制数量
        /// </summary>
        public int LimitQuantity { get; set; }
    }



    public class PromotionRuleModel
    {
        public int? BillID { get; set; }
        public string BillCode { get; set; }
        //public int DataGroupID { get; set; }
        public string PromotionType { get; set; }
        //public string RuleRunType { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        public string Schedule { get; set; }
        public string Condition { get; set; }
        public string AllowStores { get; set; }
        public string AllowGoods { get; set; }

        public int RunIndex { get; set; }
        public string ConditionResult { get; set; }
        public string Action { get; set; }
        public Nullable<System.DateTime> LastExcuteTime { get; set; }

        public PromotionExecuteStatus ExecuteStatus { get; set; }
        public PromotionApproveStatus ApproveStatus { get; set; }


        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public System.DateTime ApproveDate { get; set; }
        public string ApproveUser { get; set; }

        public string Remark { get; set; }
    }
    #endregion

}
