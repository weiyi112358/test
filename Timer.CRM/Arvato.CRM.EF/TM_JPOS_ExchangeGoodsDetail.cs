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
    
    public partial class TM_JPOS_ExchangeGoodsDetail
    {
        public string ExchangeDetailID { get; set; }
        public string ExchangeID { get; set; }
        public string GoodsID { get; set; }
        public string GoodsCode { get; set; }
        public Nullable<int> MaxCounts { get; set; }
        public Nullable<decimal> DiscountValue { get; set; }
        public Nullable<int> MinCounts { get; set; }
        public int Integral { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string InputCode { get; set; }
    }
}