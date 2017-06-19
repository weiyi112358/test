using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arvato.CRM.Model
{
    public class KPIModel
    {
        public long KPIID { get; set; }
        public string KPIName { get; set; }
        public string KPIType { get; set; }
        public string ComputeScript { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string TargetValueType { get; set; }
        public string Unit { get; set; }
        public int DataGroupID { get; set; }
    }
}