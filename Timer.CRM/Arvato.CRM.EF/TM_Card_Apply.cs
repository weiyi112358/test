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
    
    public partial class TM_Card_Apply
    {
        public string Id { get; set; }
        public string ExecuteStatus { get; set; }
        public string Status { get; set; }
        public System.DateTime ArriveTime { get; set; }
        public Nullable<long> MathRule { get; set; }
        public string AcceptingUnit { get; set; }
        public Nullable<int> ApplyNumber { get; set; }
        public Nullable<int> ApproveNumber { get; set; }
        public Nullable<int> DeliverNumber { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<int> ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<long> OddIdNo { get; set; }
    }
}
