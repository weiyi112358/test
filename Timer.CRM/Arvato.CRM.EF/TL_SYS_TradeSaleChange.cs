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
    
    public partial class TL_SYS_TradeSaleChange
    {
        public Nullable<long> TradeID { get; set; }
        public string MemID { get; set; }
        public string TradeCode { get; set; }
        public string TradeType { get; set; }
        public Nullable<decimal> StandardAmountSales { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> GoldAmountSales { get; set; }
        public Nullable<decimal> NotGoldAmountSales { get; set; }
        public string PayMentTypeSales { get; set; }
        public Nullable<decimal> His_StandardAmountSales { get; set; }
        public Nullable<decimal> His_Amount { get; set; }
        public Nullable<decimal> His_GoldAmountSales { get; set; }
        public Nullable<decimal> His_NotGoldAmountSales { get; set; }
        public string His_PayMentTypeSales { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int ID { get; set; }
    }
}
