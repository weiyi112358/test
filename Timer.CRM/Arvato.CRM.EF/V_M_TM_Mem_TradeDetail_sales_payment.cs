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
    
    public partial class V_M_TM_Mem_TradeDetail_sales_payment
    {
        public long TradeDetailID { get; set; }
        public string TradeDetailType { get; set; }
        public long TradeID { get; set; }
        public string TradeType { get; set; }
        public string ItemNoPayment { get; set; }
        public string Currency { get; set; }
        public string CardCode { get; set; }
        public string MemoPayment { get; set; }
        public Nullable<decimal> AmountPayment { get; set; }
        public Nullable<decimal> FavAmountPayment { get; set; }
        public Nullable<System.DateTime> RcvTimeSalesPayment { get; set; }
        public Nullable<bool> IsKeepChange { get; set; }
    }
}
