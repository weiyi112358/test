//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Arvato.CRM.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_M_TM_Mem_Trade_change
    {
        public long TradeID { get; set; }
        public int DataGroupID { get; set; }
        public string TradeCode { get; set; }
        public string TradeType { get; set; }
        public Nullable<long> RefTradeID { get; set; }
        public string RefTradeType { get; set; }
        public string MemberID { get; set; }
        public bool NeedLoyCompute { get; set; }
        public string NoNeedLoyComputeReaseon { get; set; }
        public string AccountID { get; set; }
        public string AccountDetailID { get; set; }
        public string ActChangeReason { get; set; }
        public string ChangeStoreCode { get; set; }
        public string Remark { get; set; }
        public string StatusChange { get; set; }
        public Nullable<decimal> ActChangeValue { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string ActChangeReasonCode { get; set; }
    }
}
