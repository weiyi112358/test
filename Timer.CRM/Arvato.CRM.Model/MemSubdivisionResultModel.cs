using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    /// <summary>
    /// 会员细分返回结果类
    /// </summary>
    public class MemSubdivisionResultModel
    {
        public string MemberID { get; set; }
        public string ParentMemberID { get; set; }
        public int DataGroupID { get; set; }
        public string DataGroupName { get; set; }
        public string MemberCardNo { get; set; }
        public string CustomerName { get; set; }
        public string RegisterStoreCode { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Gender { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string StoreName { get; set; }
        public int MemberGrade { get; set; }

    }
    public class SubExportColumnModel
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
