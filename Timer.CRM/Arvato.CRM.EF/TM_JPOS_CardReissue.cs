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
    
    public partial class TM_JPOS_CardReissue
    {
        public string id { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public Nullable<System.DateTime> tranTime { get; set; }
        public string posNo { get; set; }
        public Nullable<int> accountType { get; set; }
        public string accountAccessCode { get; set; }
        public string mobileCheckPsw { get; set; }
        public string xid { get; set; }
        public string CardInfo { get; set; }
        public Nullable<bool> isCancel { get; set; }
        public string cardNumber { get; set; }
        public string referenceNo { get; set; }
        public string reissueType { get; set; }
        public decimal reissueValue { get; set; }
    }
}
