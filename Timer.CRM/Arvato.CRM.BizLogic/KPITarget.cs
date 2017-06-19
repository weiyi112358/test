using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public class KPITarget
    {
        #region 商业计划查询

        public static Result GetKPITarget(long? kpiID, string kpiType, string planID, int? dataGroupID = null)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from kpt in db.TM_CRM_KPITarget
                            join kpibase in db.TM_CRM_KPI on kpt.KPIID equals kpibase.KPIID
                            into kp
                            from k in kp.DefaultIfEmpty()
                            select new
                            {
                                KPIID = kpt.KPIID,
                                KPIName = k.KPIName,
                                TargetValueType = k.TargetValueType,
                                Unit = k.Unit,
                                KPIType = kpt.KPIType,
                                KPITypeValue = kpt.KPITypeValue,
                                StrValue1 = kpt.StrValue1,
                                IntValue1 = kpt.IntValue1,
                                DecValue1 = kpt.DecValue1,
                                DecValue2 = kpt.DecValue2,
                                DataGroupID = SqlFunctions.StringConvert((decimal?)k.DataGroupID).Trim()
                            };

                if (kpiID.HasValue)
                {
                    query = query.Where(k => k.KPIID == kpiID.Value);
                }
                if (!string.IsNullOrEmpty(kpiType))
                {
                    query = query.Where(k => k.KPIType.Trim().Equals(kpiType.Trim()));
                }
                if (!string.IsNullOrEmpty(planID))
                {
                    query = query.Where(k => k.KPITypeValue.Trim().Equals(planID.Trim()));
                }
                if (dataGroupID.HasValue)
                {
                    query = query.Where(k => k.DataGroupID.Trim().Equals(SqlFunctions.StringConvert((decimal?)dataGroupID).Trim()));
                }
                return new Result(true, "", new List<object> { query.ToList() });

            }
        }

        #endregion
    }
}

