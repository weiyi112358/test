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
    
    public partial class TM_CRM_KPI
    {
        public long KPIID { get; set; }
        public string KPIName { get; set; }
        public string KPIType { get; set; }
        public string ComputeScript { get; set; }
        public string Unit { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string TargetValueType { get; set; }
        public int DataGroupID { get; set; }
        public bool Enable { get; set; }
    }
}
