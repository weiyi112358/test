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
    
    public partial class TM_Sys_EmailSendingQueue
    {
        public long ID { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string MemberID { get; set; }
        public Nullable<long> ActInstanceID { get; set; }
        public Nullable<System.Guid> WorkflowID { get; set; }
        public Nullable<int> TempletID { get; set; }
        public bool IsSent { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> PlanSendDate { get; set; }
        public Nullable<System.DateTime> ActSendDate { get; set; }
        public Nullable<bool> IsLogged { get; set; }
    }
}
