using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class MemberPackageModel
    {
        public long PackageInstanceID { get; set; }
        public string PackageName { get; set; }
        public string PackageDesc { get; set; }
        public List<MemberPackageDetailModel> ItemList { get; set; }
    }


    public class MemberPackageDetailModel
    {
        public long PackageInstanceID { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public int Qty { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
