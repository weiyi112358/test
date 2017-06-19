using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    //public class Member360ViewModel
    //{
    //    public string MemberCardNo { get; set; }
    //    public string CustomerStatus { get; set; }
    //    public string CustomerStatusText { get; set; }
    //    public string CustomerName { get; set; }
    //    public string CustomerMobile { get; set; }
    //    public string CustomerLevel { get; set; }
    //    public string CustomerLevelText { get; set; }
    //    public string CarNo { get; set; }
    //    public DateTime? ValidateDate { get; set; }
    //    public DateTime? InsuranceDate { get; set; }
    //    public DateTime? RegisterDate { get; set; }
    //    public string RegisterStoreCode { get; set; }
    //    public string RegisterStoreName { get; set; }
    //}

    public class Member360SearchModel
    {
        public int DataGroupId { get; set; }
        public string PageIds { get; set; }
        public string DataRoleIds { get; set; }
        public string MemberNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerLevel { get; set; }
        public string CarNo { get; set; }
        public string RegStoreArea { get; set; }
        public string RegStoreChan { get; set; }
        public decimal? ConsumeAmountStart { get; set; }
        public decimal? ConsumeAmountEnd { get; set; }
        public decimal? ConsumePointStart { get; set; }
        public decimal? ConsumePointEnd { get; set; }
        public string CustomerSource { get; set; }
        public string RegisterDateStart { get; set; }
        public string RegisterDateEnd { get; set; }
        public string RegisterStoreCode { get; set; }
    }

    public class AccountLimit
    {
        public string AccountID { get; set; }
        public string ActDetailID { get; set; }
        public string LimitText { get; set; }
    }
    public class CouponLimit
    {
        public long CouponID { get; set; }
        public string LimitText { get; set; }
    }
    //接收某优惠券的类型与值
    public class CouponLimitPair
    {
        public string LimitType { get; set; }
        public string LimitValue { get; set; }
    }

    public class ActLimit
    {
        public string LimitType { get; set; }
        public string LimitValue { get; set; }
    }

    public class PackageModel
    {
        public int PackageId { get; set; }
        public int PackageDetailId { get; set; }
        public string MemberId { get; set; }
        public decimal? Price { get; set; }
        public int Qty { get; set; }
        public bool IsPresented { get; set; }
    }

    public class AccountModel
    {
        public string MemberId { get; set; }
        public string AccountType { get; set; }

        public decimal ChangeValue { get; set; }
    }

    public class CouModel
    {
        public long CouponId { get; set; }
        public string MemberId { get; set; }
        public int Qty { get; set; }
    }

    public class CardChange
    {
        public long LogID { get; set; }

        public string MemberId { get; set; }
        public string CardNo { get; set; }
        public string ChangeType { get; set; }
        public System.DateTime ChangeTime { get; set; }
        public string ChangePlace { get; set; }
        public string OldCardNo { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public string Remark { get; set; }
    }
    /// <summary>
    /// 卡预存信息表
    /// </summary>
    public class CardPrepare
    {
        public string id { get; set; }
        public DateTime time { get; set; }
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string tranId { get; set; }
        public DateTime tranTime { get; set; }
        public string posNo { get; set; }
        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public string mobileCheckPsw { get; set; }
        public string xid { get; set; }

      
        public string lossCardInfo { get; set; }
       
        /// <summary>
        /// 锁卡是否审核 审核 true
        /// </summary>
        public bool isConfirm { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string cardNumber { get; set; }
        /// <summary>
        /// 解锁是否审核 审核 true
        /// </summary>
        public bool isConfirm_Unlock { get; set; }
        /// <summary>
        /// 会员号
        /// </summary>
        public string cardHolder { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string source { get; set; }
        /// <summary>
        /// 代理人
        /// </summary>
        public string agent { get; set; }
        /// <summary>
        /// 代理人身份证号
        /// </summary>
        public string agentCertificateNo { get; set; }
        /// <summary>
        /// 代理人手机号
        /// </summary>
        public string agentMobile { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal? serviceCharge { get; set; }


        
    }
    public class BizOption
    {
        public int OptionID { get; set; }
        public string OptionType { get; set; }
        public string OptionValue { get; set; }
        public string OptionText { get; set; }
        public Nullable<int> DataGroupID { get; set; }
        public short Sort { get; set; }
        public string Remark { get; set; }
        public bool Enable { get; set; }
        public string ReferenceOptionType { get; set; }
    }

    public class CommonSearch
    {
        public string TableName { get; set; }
        public string AliasKey { get; set; }
        public string AliasSubKey { get; set; }
        public List<CommonSearchDetail> s { get; set; }
        public List<ReturnField> f { get; set; }
    }

    public class CommonSearchDetail
    {
        public string FieldAlias { get; set; }
        public string Value { get; set; }
        public string TableName { get; set; }
        public string Condition { get; set; }
        
    }

    public class ReturnField
    {
        public string FieldAlias { get; set; }
        public string FieldAlias2 { get; set; } //如果是子查询型的字段存放code字段
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string IsSource { get; set; }
        public string Grade { get; set; }
        public bool flag { get; set; }
        public string Alias { get; set; }
    }

    public class TableMapping
    {
        public string TableName { get; set; }
        public string RelationType { get; set; }
        public string RelationAlias_1 { get; set; }
        public string RelationAlias_2 { get; set; }
        public string Grade { get; set; }
    }

    public class AutoComputeTable
    {
        public string TableName { get; set; }
        public string TableName_S { get; set; }
        public string TableAlias { get; set; }
        public string TableAlias_S { get; set; }
        public int Grade { get; set; }
        public string ColumnName { get; set; }
        public string FieldAlias { get; set; }
        public string RelationAlias_1 { get; set; }
        public string RelationAlias_2 { get; set; }
    }

    public class CouponModelLimit
    {
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public int TempletID { get; set; }
        public string LimitType { get; set; }
        public string LimitValue { get; set; }
        public long? isAvailable { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class SMSSendModel
    {
        public string MemberId { get; set; }
        public string MemberInfo { get; set; }
        public string SMSInfo { get; set; }
    
    }

    public class ExportMember360Model
    {
        public string CustomerName { get; set; }
        public string MemberCardNo { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerStatus { get; set; }
        public string Gender { get; set; }
        public string CertificateType { get; set; }
        public string CertificateNo { get; set; }
        public string StrMem_bk_1 { get; set; }
        public string StrMem_bk_2 { get; set; }
        public string StrMem_bk_4 { get; set; }
        public string CustomerEmail { get; set; }
        public string ProvinceCodeExt { get; set; }
        public string CityCodeExt { get; set; }
        public string DistrictCodeExt { get; set; }
        public string Address1 { get; set; }
        public string BusinessDepartment { get; set; }
    }

    public class ExportColumnModel
    {
        
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class AbnormalTradeToExcel
    {
        public string TradeID { get; set; }
        public string StoreCode { get; set; }
        public string MemberID  { get; set; }
        public string  CustomerName { get; set; }
        public string Reason { get; set; }
        public string Mobile { get; set; }
        public string AddedDate { get; set; }
        public string AddedUser { get; set; }
    }

    public class DeleteAbnormalTrade
    {
        public string Id { get; set; }
    }
}
