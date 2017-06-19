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
    public class BusinessPlan
    {
        #region 商业计划查询

        public static Result GetAllBusinessPlan(int? dataGroupID = null)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from p in db.TM_CRM_BusinessPlan
                            join o in db.TD_SYS_BizOption on new { PlanType = p.PlanType, PlanTypeName = "BusinessPlanType" } equals new { PlanType = o.OptionValue, PlanTypeName = o.OptionType }
                            into p1
                            from planCollection1 in p1.DefaultIfEmpty()
                            join op in db.TD_SYS_BizOption on new { PlanType = p.Status, PlanStatus = "BusinessPlanStatus" } equals new { PlanType = op.OptionValue, PlanStatus = op.OptionType }
                            into p2
                            from planCollection2 in p2.DefaultIfEmpty()
                            select new
                            {
                                BusinessPlanID = p.BusinessPlanID,
                                BusinessPlanName = p.BusinessPlanName,
                                PlanType = p.PlanType,
                                PlanStartTime = p.PlanStartTime,
                                PlanEndTime = p.PlanEndTime,
                                Remark = p.Remark,
                                Status = p.Status,
                                PlanTypeName = planCollection1.OptionText,
                                StatusName = planCollection2.OptionText,
                                AddedDate = p.AddedDate,
                                AddedUser = p.AddedUser,
                                ModifiedDate = p.ModifiedDate,
                                ModifiedUser = p.ModifiedUser,
                                DataGroupID = p.DataGroupID
                            };

                if (dataGroupID.HasValue)
                {
                    query = query.Where(p => p.DataGroupID == dataGroupID.Value);
                }

                return new Result(true, "", new List<object> { query.ToList() });
            }
        }

        public static Result GetAllEnableBusinessPlan(int? dataGroupID = null)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from p in db.TM_CRM_BusinessPlan
                            join o in db.TD_SYS_BizOption on new { PlanType = p.PlanType, PlanTypeName = "BusinessPlanType" } equals new { PlanType = o.OptionValue, PlanTypeName = o.OptionType }
                            into p1
                            from planCollection1 in p1.DefaultIfEmpty()
                            join op in db.TD_SYS_BizOption on new { PlanType = p.Status, PlanStatus = "BusinessPlanStatus" } equals new { PlanType = op.OptionValue, PlanStatus = op.OptionType }
                            into p2
                            from planCollection2 in p2.DefaultIfEmpty()
                            where p.Status.Trim().Equals("2")
                            select new
                            {
                                BusinessPlanID = p.BusinessPlanID,
                                BusinessPlanName = p.BusinessPlanName,
                                PlanType = p.PlanType,
                                PlanStartTime = p.PlanStartTime,
                                PlanEndTime = p.PlanEndTime,
                                Remark = p.Remark,
                                Status = p.Status,
                                PlanTypeName = planCollection1.OptionText,
                                StatusName = planCollection2.OptionText,
                                AddedDate = p.AddedDate,
                                AddedUser = p.AddedUser,
                                ModifiedDate = p.ModifiedDate,
                                ModifiedUser = p.ModifiedUser,
                                DataGroupID = p.DataGroupID
                            };

                if (dataGroupID.HasValue)
                {
                    query = query.Where(p => p.DataGroupID == dataGroupID.Value);
                }

                return new Result(true, "", new List<object> { query.ToList() });
            }
        }

        public static Result GetBusinessPlan(string planName, string planType, DateTime? planStartTime, DateTime? planEndTime,
            string planCode, string status, string page, int? dataGroupID = null)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);

            using (CRMEntities db = new CRMEntities())
            {
                var query = from p in db.TM_CRM_BusinessPlan
                            join o in db.TD_SYS_BizOption on new { PlanType = p.PlanType, PlanTypeName = "BusinessPlanType" } equals new { PlanType = o.OptionValue, PlanTypeName = o.OptionType }
                            into p1
                            from planCollection1 in p1.DefaultIfEmpty()
                            join op in db.TD_SYS_BizOption on new { PlanType = p.Status, PlanStatus = "BusinessPlanStatus" } equals new { PlanType = op.OptionValue, PlanStatus = op.OptionType }
                            into p2
                            from planCollection2 in p2.DefaultIfEmpty()
                            select new
                            {
                                BusinessPlanID = p.BusinessPlanID,
                                BusinessPlanName = p.BusinessPlanName,
                                PlanType = p.PlanType,
                                PlanStartTime = p.PlanStartTime,
                                PlanEndTime = p.PlanEndTime,
                                Remark = p.Remark,
                                Status = p.Status,
                                PlanTypeName = planCollection1.OptionText,
                                StatusName = planCollection2.OptionText,
                                AddedDate = p.AddedDate,
                                AddedUser = p.AddedUser,
                                ModifiedDate = p.ModifiedDate,
                                ModifiedUser = p.ModifiedUser,
                                DataGroupID = p.DataGroupID
                            };

                if (!string.IsNullOrEmpty(planName))
                {
                    query = query.Where(p => p.BusinessPlanName.Trim().Contains(planName.Trim()));
                }
                if (!string.IsNullOrEmpty(planType))
                {
                    query = query.Where(p => p.PlanType.Trim().Equals(planType.Trim()));
                }
                if (planStartTime.HasValue)
                {
                    query = query.Where(p => p.PlanStartTime >= planStartTime.Value);
                }
                if (planEndTime.HasValue)
                {
                    query = query.Where(p => p.PlanStartTime <= planEndTime.Value);
                }
                if (!string.IsNullOrEmpty(planCode))
                {
                    query = query.Where(p => p.BusinessPlanID.Trim().Equals(planCode.Trim()));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status.Trim().Equals(status.Trim()));
                }

                if (dataGroupID.HasValue)
                {
                    query = query.Where(p => p.DataGroupID == dataGroupID.Value);
                }

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(pageInfo) });
            }
        }

        public static Result GetBusinessPlanByPlanID(string planID, int? dataGroupID = null)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from p in db.TM_CRM_BusinessPlan
                            join o in db.TD_SYS_BizOption on new { PlanType = p.PlanType, PlanTypeName = "BusinessPlanType" } equals new { PlanType = o.OptionValue, PlanTypeName = o.OptionType }
                            into p1
                            from planCollection1 in p1.DefaultIfEmpty()
                            join op in db.TD_SYS_BizOption on new { PlanType = p.Status, PlanStatus = "BusinessPlanStatus" } equals new { PlanType = op.OptionValue, PlanStatus = op.OptionType }
                            into p2
                            from planCollection2 in p2.DefaultIfEmpty()
                            select new
                            {
                                BusinessPlanID = p.BusinessPlanID,
                                BusinessPlanName = p.BusinessPlanName,
                                PlanType = p.PlanType,
                                PlanStartTime = p.PlanStartTime,
                                PlanEndTime = p.PlanEndTime,
                                Remark = p.Remark,
                                Status = p.Status,
                                PlanTypeName = planCollection1.OptionText,
                                StatusName = planCollection2.OptionText,
                                AddedDate = p.AddedDate,
                                AddedUser = p.AddedUser,
                                ModifiedDate = p.ModifiedDate,
                                ModifiedUser = p.ModifiedUser,
                                DataGroupID = p.DataGroupID
                            };

                if (dataGroupID.HasValue)
                {
                    query = query.Where(p => p.DataGroupID == dataGroupID.Value);
                }

                if (!string.IsNullOrEmpty(planID))
                {
                    query = query.Where(p => p.BusinessPlanID.Trim().Equals(planID.Trim()));
                }
                else
                {
                    return new Result(false, "", null);
                }

                return new Result(true, "", query.FirstOrDefault());
            }
        }

        public static Result GetBusinessPlanDetailByPlanID(string planID, int? dataGroupID = null)
        {
            if (string.IsNullOrEmpty(planID))
            {
                return new Result(false, "", null);
            }
            else
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var queryKPITarget = from kpt in db.TM_CRM_KPITarget
                                         join kpibase in db.TM_CRM_KPI on kpt.KPIID equals kpibase.KPIID
                                         into kp
                                         from k in kp.DefaultIfEmpty()
                                         where kpt.KPITypeValue.Trim().Equals(planID.Trim()) && kpt.KPIType.Trim().Equals("3")
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
                                             DataGroupID = k.DataGroupID
                                         };


                    var queryKPIResult = from kpr in db.TM_CRM_KPIResult
                                         join kpibase in db.TM_CRM_KPI on kpr.KPIID equals kpibase.KPIID
                                         into kp
                                         from k in kp.DefaultIfEmpty()
                                         join kpt in db.TM_CRM_KPITarget on new { kpr.KPIID, kpr.KPIType, kpr.KPITypeValue } equals new { kpt.KPIID, kpt.KPIType, kpt.KPITypeValue }
                                         into kp2
                                         from k2 in kp2.DefaultIfEmpty()
                                         where kpr.KPITypeValue.Trim().Equals(planID.Trim()) && kpr.KPIType.Trim().Equals("3")
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
                                             DataGroupID = k.DataGroupID
                                         };

                    var queryMaxTime = (from kpr in db.TM_CRM_KPIResult
                                        where kpr.KPIType.Trim().Equals("3") && kpr.KPITypeValue.Trim().Equals(planID.Trim())
                                        select kpr.ComputeTime).Count() > 0 ?
                                        (from kpr in db.TM_CRM_KPIResult
                                         where kpr.KPIType.Trim().Equals("3") && kpr.KPITypeValue.Trim().Equals(planID.Trim())
                                         select kpr.ComputeTime).Max() : DateTime.Now.AddDays(1);

                    var queryResultColl = from r in queryKPIResult
                                          group r by new { r.KPIID, r.KPIType, r.KPITypeValue }
                                              into rstl
                                              from rst in rstl.DefaultIfEmpty()
                                              where rst.ComputeTime.Equals(queryMaxTime)
                                              select new
                                              {
                                                  KPIResultID = rst.KPIResultID,
                                                  KPIID = rst.KPIID,
                                                  KPIName = rst.KPIName,
                                                  TargetValueType = rst.TargetValueType,
                                                  TargetValue = rst.TargetValue,
                                                  CurrentValue = rst.CurrentValue,
                                                  Unit = rst.Unit,
                                                  KPIType = rst.KPIType,
                                                  KPITypeValue = rst.KPITypeValue,
                                                  Year = rst.Year,
                                                  Month = rst.Month,
                                                  Day = rst.Day,
                                                  ComputeTime = (rst.ComputeTime),
                                                  StrValue1 = rst.StrValue1,
                                                  IntValue1 = rst.IntValue1,
                                                  DecValue1 = rst.DecValue1,
                                                  DataGroupID = rst.DataGroupID
                                              };

                    if (dataGroupID.HasValue)
                    {
                        queryKPITarget = queryKPITarget.Where(qkt => qkt.DataGroupID == dataGroupID.Value);
                        queryResultColl = queryResultColl.Where(qrc => qrc.DataGroupID == dataGroupID.Value);
                    }

                    var query = from p in db.TM_CRM_BusinessPlan
                                join o in db.TD_SYS_BizOption on new { PlanType = p.PlanType, PlanTypeName = "BusinessPlanType" } equals new { PlanType = o.OptionValue, PlanTypeName = o.OptionType }
                                into p1
                                from planCollection1 in p1.DefaultIfEmpty()
                                join op in db.TD_SYS_BizOption on new { PlanType = p.Status, PlanStatus = "BusinessPlanStatus" } equals new { PlanType = op.OptionValue, PlanStatus = op.OptionType }
                                into p2
                                from planCollection2 in p2.DefaultIfEmpty()
                                select new
                                {
                                    BusinessPlanID = p.BusinessPlanID,
                                    BusinessPlanName = p.BusinessPlanName,
                                    PlanType = p.PlanType,
                                    PlanStartTime = p.PlanStartTime,
                                    PlanEndTime = p.PlanEndTime,
                                    Remark = p.Remark,
                                    Status = p.Status,
                                    PlanTypeName = planCollection1.OptionText,
                                    StatusName = planCollection2.OptionText,
                                    AddedDate = p.AddedDate,
                                    AddedUser = p.AddedUser,
                                    ModifiedDate = p.ModifiedDate,
                                    ModifiedUser = p.ModifiedUser,
                                    KPIResultList = queryResultColl,
                                    KPITargetList = queryKPITarget,
                                    DataGroupID = p.DataGroupID
                                };

                    if (dataGroupID.HasValue)
                    {
                        query = query.Where(p => p.DataGroupID == dataGroupID.Value);
                    }

                    query = query.Where(p => p.BusinessPlanID.Trim().Equals(planID.Trim()));

                    return new Result(true, "", new List<object> { query.FirstOrDefault() });
                }
            }
        }

        #endregion

        #region 新增、更新商业计划

        public static Result InsertBusinessPlan(string businessPlan, string businessPlanTargetList)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_CRM_BusinessPlan plan = JsonHelper.Deserialize<TM_CRM_BusinessPlan>(businessPlan);
                    List<TM_CRM_KPITarget> targetList = JsonHelper.Deserialize<List<TM_CRM_KPITarget>>(businessPlanTargetList);
                    if (plan != null)
                    {
                        db.Entry<TM_CRM_BusinessPlan>(plan).State = EntityState.Added;
                        if (targetList != null)
                        {
                            foreach (TM_CRM_KPITarget obj in targetList)
                            {
                                obj.KPITypeValue = plan.BusinessPlanID;
                                db.Entry<TM_CRM_KPITarget>(obj).State = EntityState.Added;
                            }
                        }
                        db.SaveChanges();
                        return new Result(true, "", plan.BusinessPlanID);
                    }
                    return new Result(false, "新增失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result UpdateBusinessPlanBase(string businessPlan, int? dataGroupID = null)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_CRM_BusinessPlan plan = JsonHelper.Deserialize<TM_CRM_BusinessPlan>(businessPlan);
                    if (plan != null)
                    {
                        TM_CRM_BusinessPlan planOrigin;

                        if (dataGroupID.HasValue)
                        {
                            planOrigin = db.TM_CRM_BusinessPlan
                                 .Where(p => p.BusinessPlanID.Trim().Equals(plan.BusinessPlanID.Trim()) && p.DataGroupID == dataGroupID.Value).FirstOrDefault();
                        }
                        else
                        {
                            planOrigin = db.TM_CRM_BusinessPlan
                                 .Where(p => p.BusinessPlanID.Trim().Equals(plan.BusinessPlanID.Trim())).FirstOrDefault();
                        }

                        if (EngineHelper.CompareDateTimeIgnoreMillisecond(plan.ModifiedDate, planOrigin.ModifiedDate))
                        {
                            planOrigin.BusinessPlanName = plan.BusinessPlanName;
                            planOrigin.ModifiedDate = DateTime.Now;
                            planOrigin.ModifiedUser = plan.ModifiedUser;
                            planOrigin.PlanStartTime = plan.PlanStartTime;
                            planOrigin.PlanEndTime = plan.PlanEndTime;
                            planOrigin.PlanType = plan.PlanType;
                            planOrigin.Remark = plan.Remark;
                            planOrigin.Status = plan.Status;

                            db.Entry<TM_CRM_BusinessPlan>(planOrigin).State = EntityState.Modified;
                            db.SaveChanges();
                            return new Result(true, "", plan.BusinessPlanID);
                        }

                        return new Result(false, "数据已经被修改,请刷新页面或者重新加载数据进行操作", null);
                    }

                    return new Result(false, "更新失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result UpdateBusinessPlan(string businessPlan, string businessPlanTargetList, int? dataGroupID = null)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_CRM_BusinessPlan plan = JsonHelper.Deserialize<TM_CRM_BusinessPlan>(businessPlan);
                    List<TM_CRM_KPITarget> targetList = JsonHelper.Deserialize<List<TM_CRM_KPITarget>>(businessPlanTargetList);

                    if (plan != null)
                    {
                        TM_CRM_BusinessPlan planOrigin;

                        if (dataGroupID.HasValue)
                        {
                            planOrigin = db.TM_CRM_BusinessPlan
                                 .Where(p => p.BusinessPlanID.Trim().Equals(plan.BusinessPlanID.Trim()) && p.DataGroupID == dataGroupID.Value).FirstOrDefault();
                        }
                        else
                        {
                            planOrigin = db.TM_CRM_BusinessPlan
                                 .Where(p => p.BusinessPlanID.Trim().Equals(plan.BusinessPlanID.Trim())).FirstOrDefault();
                        }

                        if (EngineHelper.CompareDateTimeIgnoreMillisecond(plan.ModifiedDate, planOrigin.ModifiedDate))
                        {
                            planOrigin.BusinessPlanName = plan.BusinessPlanName;
                            planOrigin.ModifiedDate = DateTime.Now;
                            planOrigin.ModifiedUser = plan.ModifiedUser;
                            planOrigin.PlanStartTime = plan.PlanStartTime;
                            planOrigin.PlanEndTime = plan.PlanEndTime;
                            planOrigin.PlanType = plan.PlanType;
                            planOrigin.Remark = plan.Remark;
                            planOrigin.Status = plan.Status;

                            db.Entry<TM_CRM_BusinessPlan>(planOrigin).State = EntityState.Modified;

                            List<TM_CRM_KPITarget> list = db.TM_CRM_KPITarget.
                                Where(t => t.KPITypeValue.Trim().Equals(plan.BusinessPlanID.Trim()) && t.KPIType == "3").ToList();

                            if (list != null)
                            {
                                foreach (TM_CRM_KPITarget deleteObj in list)
                                {
                                    db.Entry<TM_CRM_KPITarget>(deleteObj).State = EntityState.Deleted;
                                }
                            }

                            if (targetList != null)
                            {
                                foreach (TM_CRM_KPITarget obj in targetList)
                                {
                                    db.TM_CRM_KPITarget.Add(obj);
                                }
                            }

                            db.SaveChanges();
                            return new Result(true, "", plan.BusinessPlanID);
                        }

                        return new Result(false, "数据已经被修改,请刷新页面或者重新加载数据进行操作", null);
                    }
                    return new Result(false, "更新失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result UpdateBusinessPlanStatusByID(string businessPlanID, string status, string updateUser, DateTime? updateDate = null, int? dataGroupID = null)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_CRM_BusinessPlan plan;

                    if (dataGroupID.HasValue)
                    {
                        plan = db.TM_CRM_BusinessPlan
                             .Where(p => p.BusinessPlanID.Trim().Equals(businessPlanID.Trim()) && p.DataGroupID == dataGroupID.Value).FirstOrDefault();
                    }
                    else
                    {
                        plan = db.TM_CRM_BusinessPlan
                             .Where(p => p.BusinessPlanID.Trim().Equals(businessPlanID.Trim())).FirstOrDefault();
                    }

                    if (plan != null)
                    {
                        if (EngineHelper.CompareDateTimeIgnoreMillisecond(plan.ModifiedDate, updateDate))
                        {
                            plan.Status = status;
                            plan.ModifiedDate = DateTime.Now;
                            plan.ModifiedUser = updateUser;
                            db.Entry<TM_CRM_BusinessPlan>(plan).State = EntityState.Modified;
                            db.SaveChanges();
                            return new Result(true, "", businessPlanID);
                        }
                        return new Result(false, "数据已经被修改,请刷新页面或者重新加载数据进行操作", null);
                    }
                    return new Result(false, "更新失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        #endregion

        #region 删除商业计划

        public static Result DeleteBusinessPlanByID(string businessPlanID, int? dataGroupID = null)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_CRM_BusinessPlan plan;

                    if (dataGroupID.HasValue)
                    {
                        plan = db.TM_CRM_BusinessPlan
                             .Where(p => p.BusinessPlanID.Trim().Equals(businessPlanID.Trim()) && p.DataGroupID == dataGroupID.Value).FirstOrDefault();
                    }
                    else
                    {
                        plan = db.TM_CRM_BusinessPlan
                             .Where(p => p.BusinessPlanID.Trim().Equals(businessPlanID.Trim())).FirstOrDefault();
                    }


                    if (plan != null)
                    {
                        if (!plan.Status.Trim().Equals("1"))
                        {
                            return new Result(false, "商业计划在非提交审批状态下无法删除", null);
                        }

                        db.Entry<TM_CRM_BusinessPlan>(plan).State = EntityState.Deleted;

                        List<TM_CRM_KPITarget> list = db.TM_CRM_KPITarget.
                            Where(t => t.KPITypeValue.Trim().Equals(plan.BusinessPlanID.Trim()) && t.KPIType == "3").ToList();

                        if (list != null)
                        {
                            foreach (TM_CRM_KPITarget deleteObj in list)
                            {
                                db.Entry<TM_CRM_KPITarget>(deleteObj).State = EntityState.Deleted;
                            }
                        }

                        List<TM_CRM_KPIResult> listResult = db.TM_CRM_KPIResult.
                            Where(t => t.KPITypeValue.Trim().Equals(plan.BusinessPlanID.Trim()) && t.KPIType == "3").ToList();

                        if (listResult != null)
                        {
                            foreach (TM_CRM_KPIResult deleteResultObj in listResult)
                            {
                                db.Entry<TM_CRM_KPIResult>(deleteResultObj).State = EntityState.Deleted;
                            }
                        }

                        db.SaveChanges();
                        return new Result(true, "", businessPlanID);
                    }
                    return new Result(false, "更新失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        #endregion
    }
}
