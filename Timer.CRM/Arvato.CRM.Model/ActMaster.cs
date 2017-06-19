using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class ActMaster
    {
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public int DataGroupID { get; set; }
        public bool Enable { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime? ProEndDate { get; set; }
        public DateTime? ProStartDate { get; set; }
        public string Remark { get; set; }
        public string Schedule { get; set; }
        public short Status { get; set; }
        public Guid? WfRootId { get; set; }
        public string Workflow { get; set; }
        public string Kpi { get; set; }
        public string ReferenceNo { get; set; }
        public string BusinessPlanID { get; set; }

        public List<KPIResultModel> KPIResultList { get; set; }
        public List<KPITargetModel> KPITargetList { get; set; }
    }
}
