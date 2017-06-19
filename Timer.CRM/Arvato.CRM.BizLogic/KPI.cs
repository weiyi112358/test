using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public class KPI
    {
        #region 查询KPI

        public static Result GetKPI(long? kpiID, string kpiName, string kpiType, string targetValueType, int? dataGroupID = null)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from k in db.TM_CRM_KPI
                            select k;

                if (kpiID.HasValue)
                {
                    query = query.Where(k => k.KPIID == kpiID.Value);
                }
                if (!string.IsNullOrEmpty(kpiType))
                {
                    query = query.Where(k => k.KPIType.Trim().Equals(kpiType.Trim()));
                }
                if (!string.IsNullOrEmpty(targetValueType))
                {
                    query = query.Where(k => k.TargetValueType.Trim().Equals(targetValueType.Trim()));
                }
                if (dataGroupID.HasValue)
                {
                    query = query.Where(k => k.DataGroupID == dataGroupID.Value);
                }

                return new Result(true, "", new List<object> { query.ToList() });
            }
        }

        #endregion
    }
}
