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
    
    public partial class TM_SYS_Class
    {
        public int ClassID { get; set; }
        public string ClassType { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string ClassName { get; set; }
        public short Sort { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public string ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int DataGroupID { get; set; }
    }
}
