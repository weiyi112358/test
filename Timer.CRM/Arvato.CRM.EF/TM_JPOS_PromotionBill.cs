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
    
    public partial class TM_JPOS_PromotionBill
    {
        public string BillID { get; set; }
        public string BillCode { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Remark { get; set; }
        public string PromotionType { get; set; }
        public string Schedule { get; set; }
        public string Condition { get; set; }
        public int ExecuteStatus { get; set; }
        public int ApproveStatus { get; set; }
        public string AllowStores { get; set; }
        public string AllowGoods { get; set; }
        public int RunIndex { get; set; }
        public string ExpressionXML { get; set; }
        public string ConditionXML { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string ApproveUser { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string Action { get; set; }
    }
}
