using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arvato.CRM.Model
{
    public class BusinessPlanModel
    {
        public string BusinessPlanID { get; set; }
        public string BusinessPlanName { get; set; }
        public DateTime PlanStartTime { get; set; }
        public Nullable<DateTime> PlanEndTime { get; set; }
        public string PlanType { get; set; }
        public string PlanTypeName { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public int DataGroupID { get; set; }

        public List<KPIResultModel> KPIResultList { get; set; }
        public List<KPITargetModel> KPITargetList { get; set; }
    }
}