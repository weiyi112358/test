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
    public class KPIResult
    {
        #region KPI执行结果

        public static Result GetKPIResult(long? kpiResultID, long? kpiID, string kpiType, string planID,
            DateTime? computeStartTime, DateTime? computeEndTime, bool fetchLatestOne, int? dataGroupID = null)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from kpr in db.TM_CRM_KPIResult
                            join kpibase in db.TM_CRM_KPI on kpr.KPIID equals kpibase.KPIID
                            into kp
                            from k in kp.DefaultIfEmpty()
                            join kpt in db.TM_CRM_KPITarget on new { kpr.KPIID, kpr.KPIType, kpr.KPITypeValue } equals new { kpt.KPIID, kpt.KPIType, kpt.KPITypeValue }
                            into kp2
                            from k2 in kp2.DefaultIfEmpty()
                            select new
                            {
                                KPIResultID = kpr.KPIResultID,
                                KPIID = kpr.KPIID,
                                KPIName = k.KPIName,
                                TargetValueType = k.TargetValueType,
                                TargetValue = k.TargetValueType.Trim().Equals("2") ? k2.StrValue1 :
                                                   (k.TargetValueType.Trim().Equals("3") ? ((k2.IntValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)k2.IntValue1) : "0") :
                                                   (k.TargetValueType.Trim().Equals("7") ? (((k2.DecValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)k2.DecValue1) : "0") + "--" + ((k2.DecValue2.HasValue) ? SqlFunctions.StringConvert((decimal?)k2.DecValue2.Value) : "0")) : null)),
                                CurrentValue = k.TargetValueType.Trim().Equals("2") ? kpr.StrValue1 :
                                             (k.TargetValueType.Trim().Equals("3") ? ((kpr.IntValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)kpr.IntValue1.Value) : "0") :
                                             (k.TargetValueType.Trim().Equals("7") ? (((kpr.DecValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)kpr.DecValue1.Value) : "0")) : null)),
                                Unit = k.Unit,
                                KPIType = kpr.KPIType,
                                KPITypeValue = kpr.KPITypeValue,
                                Year = kpr.Year,
                                Month = kpr.Month,
                                Day = kpr.Day,
                                ComputeTime = kpr.ComputeTime,
                                StrValue1 = kpr.StrValue1,
                                IntValue1 = kpr.IntValue1,
                                DecValue1 = kpr.DecValue1,
                                DataGroupID = SqlFunctions.StringConvert((decimal?)k.DataGroupID).Trim()
                            };

                if (kpiResultID.HasValue)
                {
                    query = query.Where(k => k.KPIResultID == kpiResultID.Value);
                }
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
                if (computeStartTime.HasValue)
                {
                    query = query.Where(k => k.ComputeTime >= computeStartTime.Value);
                }
                if (computeEndTime.HasValue)
                {
                    query = query.Where(k => k.ComputeTime <= computeStartTime.Value);
                }
                if (dataGroupID.HasValue)
                {
                    query = query.Where(k => k.DataGroupID.Trim().Equals(SqlFunctions.StringConvert((decimal?)dataGroupID).Trim()));
                }
                if (fetchLatestOne)
                {
                    return new Result(true, "", new List<object> { query.OrderByDescending(r => r.ComputeTime).FirstOrDefault() });
                }
                else
                {
                    return new Result(true, "", new List<object> { query.ToList() });
                }
            }
        }


        public static Result GetKPIResultForPage(long? kpiResultID, long? kpiID, string kpiType, string planID,
            DateTime? computeStartTime, DateTime? computeEndTime, string pageInfo, int? dataGroupID = null)
        {
            DatatablesParameter pageRelated = JsonHelper.Deserialize<DatatablesParameter>(pageInfo);

            using (CRMEntities db = new CRMEntities())
            {
                var query = from kpr in db.TM_CRM_KPIResult
                            join kpibase in db.TM_CRM_KPI on kpr.KPIID equals kpibase.KPIID
                            into kp
                            from k in kp.DefaultIfEmpty()
                            join kpt in db.TM_CRM_KPITarget on new { kpr.KPIID, kpr.KPIType, kpr.KPITypeValue } equals new { kpt.KPIID, kpt.KPIType, kpt.KPITypeValue }
                            into kp2
                            from k2 in kp2.DefaultIfEmpty()
                            select new
                            {
                                KPIResultID = kpr.KPIResultID,
                                KPIID = kpr.KPIID,
                                KPIName = k.KPIName,
                                TargetValueType = k.TargetValueType,
                                TargetValue = k.TargetValueType.Trim().Equals("2") ? k2.StrValue1 :
                                                   (k.TargetValueType.Trim().Equals("3") ? ((k2.IntValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)k2.IntValue1) : "0") :
                                                   (k.TargetValueType.Trim().Equals("7") ? (((k2.DecValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)k2.DecValue1) : "0") + "--" + ((k2.DecValue2.HasValue) ? SqlFunctions.StringConvert((decimal?)k2.DecValue2.Value) : "0")) : null)),
                                CurrentValue = k.TargetValueType.Trim().Equals("2") ? kpr.StrValue1 :
                                             (k.TargetValueType.Trim().Equals("3") ? ((kpr.IntValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)kpr.IntValue1.Value) : "0") :
                                             (k.TargetValueType.Trim().Equals("7") ? (((kpr.DecValue1.HasValue) ? SqlFunctions.StringConvert((decimal?)kpr.DecValue1.Value) : "0")) : null)),
                                Unit = k.Unit,
                                KPIType = kpr.KPIType,
                                KPITypeValue = kpr.KPITypeValue,
                                Year = kpr.Year,
                                Month = kpr.Month,
                                Day = kpr.Day,
                                ComputeTime = kpr.ComputeTime,
                                StrValue1 = kpr.StrValue1,
                                IntValue1 = kpr.IntValue1,
                                DecValue1 = kpr.DecValue1,
                                DataGroupID = SqlFunctions.StringConvert((decimal?)k.DataGroupID).Trim()
                            };

                if (kpiResultID.HasValue)
                {
                    query = query.Where(k => k.KPIResultID == kpiResultID.Value);
                }
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
                if (computeStartTime.HasValue)
                {
                    query = query.Where(k => k.ComputeTime >= computeStartTime.Value);
                }
                if (computeEndTime.HasValue)
                {
                    query = query.Where(k => k.ComputeTime <= computeStartTime.Value);
                }
                if (dataGroupID.HasValue)
                {
                    query = query.Where(k => k.DataGroupID.Trim().Equals(SqlFunctions.StringConvert((decimal?)dataGroupID).Trim()));
                }

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(pageRelated) });
            }
        }

        #endregion
    }
}
