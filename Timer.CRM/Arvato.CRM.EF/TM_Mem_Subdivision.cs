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
    
    public partial class TM_Mem_Subdivision
    {
        public System.Guid SubdivisionID { get; set; }
        public string SubdivisionName { get; set; }
        public string SubdivisionDesc { get; set; }
        public string SubdivisionType { get; set; }
        public Nullable<System.Guid> CurSubdivisionInstanceID { get; set; }
        public int DataGroupID { get; set; }
        public string Condition { get; set; }
        public bool Enable { get; set; }
        public string Schedule { get; set; }
        public string SubDevDataType { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public Nullable<bool> IsArchived { get; set; }
    }
}
