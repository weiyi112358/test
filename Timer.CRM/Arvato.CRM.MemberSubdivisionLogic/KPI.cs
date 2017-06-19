using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System.Data.Objects.SqlClient;

namespace Arvato.CRM.MemberSubdivisionLogic
{
    public static class KPI
    {
        public static Result ComputeAllKPI(DateTime lastComputeTime, DateTime planComputeTime, DateTime curDatetime)
        {
            if (lastComputeTime < planComputeTime && planComputeTime <= curDatetime)
            {
                try
                {
                    using (CRMEntities db = new CRMEntities())
                    {
                        var query1 = from k in db.TM_CRM_KPI
                                     join kt in db.TM_CRM_KPITarget.Where(p => p.KPIType == "4") on new { k.KPIID, k.KPIType } equals new { kt.KPIID, kt.KPIType }
                                     join m in db.TM_Act_Master on kt.KPITypeValue equals SqlFunctions.StringConvert((double)m.ActivityID).Trim()
                                     select new
                                     {
                                         k.KPIID,
                                         kt.KPITypeValue,
                                         kt.KPIType,
                                         k.KPIName,
                                         k.ComputeScript
                                     };
                        var query2 = from k in db.TM_CRM_KPI
                                     join kt in db.TM_CRM_KPITarget.Where(p => p.KPIType == "3") on new { k.KPIID, k.KPIType } equals new { kt.KPIID, kt.KPIType }
                                     join b in db.TM_CRM_BusinessPlan on kt.KPITypeValue equals b.BusinessPlanID
                                     select new
                                     {
                                         k.KPIID,
                                         kt.KPITypeValue,
                                         kt.KPIType,
                                         k.KPIName,
                                         k.ComputeScript
                                     };
                        var query3 = from k in db.TM_CRM_KPI
                                     join kt in db.TM_CRM_KPITarget.Where(p => p.KPIType == "1" || p.KPIType == "2") on new { k.KPIID, k.KPIType } equals new { kt.KPIID, kt.KPIType }
                                     select new
                                     {
                                         k.KPIID,
                                         kt.KPITypeValue,
                                         kt.KPIType,
                                         k.KPIName,
                                         k.ComputeScript
                                     };

                        var query = query1.Union(query2).Union(query3);
                        //var query = query1.Where(p => p.KPITypeValue == "36");
                        var list = query.ToList();
                        string curTime = "'" + curDatetime.AddDays(1).ToString("yyyyMMdd") + "'";
                        foreach (var l in list)
                        {
                           
                            if (!string.IsNullOrEmpty(l.ComputeScript))
                            {
                                var cmd = db.Database.Connection.CreateCommand();
                                cmd.CommandTimeout = 240;
                                cmd.CommandText = l.ComputeScript.Replace("[KPITYPE]", "'" + l.KPIType + "'").Replace("[KPIID]", "'" + l.KPIID + "'").Replace("[KPITypeVALUE]", "'" + l.KPITypeValue + "'").Replace("[DateNow]", "'" + curDatetime.ToString("yyyy-MM-dd") + "'").Replace("[DatetimeNow]", curTime).Replace("[YearNow]", "'" + curDatetime.ToString("yyyy") + "'").Replace("[MonthNow]", "'" + curDatetime.ToString("MM") + "'").Replace("[DayNow]", curDatetime.ToString("dd"));//替换脚本中的一些通配符

                                cmd.CommandType = System.Data.CommandType.Text;
                                if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                                cmd.ExecuteNonQuery();
                                Log4netHelper.WriteInfoLog(l.KPIName + ":" + "执行完毕");
                            }
                        }


                        //db.BeginTransaction(0, 3, 0);
                        //var kpiNeedComputeList = db.TM_CRM_KPITarget.AsNoTracking().ToList();
                        //var kpiDefineList = db.TM_CRM_KPI.ToList();
                        //string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";
                        //foreach (var knc in kpiNeedComputeList)
                        //{
                        //    var kd = kpiDefineList.Single(o => o.KPIID == knc.KPIID && o.KPIType == knc.KPIType);
                        //    if (!string.IsNullOrEmpty(kd.ComputeScript))
                        //    {
                        //        var cmd = db.Database.Connection.CreateCommand();
                        //        cmd.CommandTimeout = 240;
                        //        cmd.CommandText = kd.ComputeScript.Replace("[KPITYPE]", "'" + knc.KPIType + "'").Replace("[KPIID]", "'" + knc.KPIID + "'").Replace("[KPITypeVALUE]", "'" + knc.KPITypeValue + "'").Replace("[DateNow]", "'" + curDatetime.ToString("yyyy-MM-dd") + "'").Replace("[DatetimeNow]", curTime ).Replace("[YearNow]", "'" + curDatetime.ToString("yyyy") + "'").Replace("[MonthNow]", "'" + curDatetime.ToString("MM") + "'").Replace("[DayNow]", curDatetime.ToString("dd"));//替换脚本中的一些通配符
                        //        cmd.CommandType = System.Data.CommandType.Text;
                        //        if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                        //        cmd.ExecuteNonQuery();
                        //    }
                        //}
                        //db.Commit();
                    }
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
                return new Result(true);
            }

            return new Result(true, "It's not time");
        }
    }
}
