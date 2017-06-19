using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using Arvato.CRM.Utility.WorkFlow;
using System.Data.Objects;
using System.Data.Objects.SqlClient;


namespace Arvato.CRM.BizLogic
{
    public class MarketActivity
    {
        public static Result SearchActivity(int? activityID, string activityName, bool? enable,short? status, DateTime? activityMinStartTime, DateTime? activityMaxStartTime,
            DateTime? activityMinEndTime, DateTime? activityMaxEndTime, string businessPlanID, string page, int? dataGroupID = null)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);

            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_Act_Master
                            join d in db.TM_SYS_DataGroup on a.DataGroupID equals d.DataGroupID
                            join p in db.TM_CRM_BusinessPlan on a.BusinessPlanID equals p.BusinessPlanID
                            into act
                            from actColl in act.DefaultIfEmpty()
                            select new
                            {
                                a.ActivityID,
                                a.ActivityName,
                                a.BusinessPlanID,
                                BusinessPlanName = actColl.BusinessPlanName,
                                a.DataGroupID,
                                DataGroupName = d.DataGroupName,
                                a.PlanStartDate,
                                a.PlanEndDate,
                                a.ProStartDate,
                                a.ProEndDate,
                                a.Remark,
                                a.Schedule,
                                a.Status,
                                a.StoreCode,
                                a.WfRootId,
                                a.Workflow,
                                a.Enable,
                                a.ModifiedDate,
                                a.ModifiedUser,
                                a.AddedDate,
                                a.AddedUser
                            };

                if (activityID.HasValue)
                {
                    query = query.Where(a => a.ActivityID == activityID.Value);
                }

                if (!string.IsNullOrEmpty(activityName))
                {
                    query = query.Where(a => a.ActivityName.Trim().Contains(activityName.Trim()));
                }

                if (enable.HasValue)
                {
                    query = query.Where(p => p.Enable.Equals(enable.Value));
                }

                if (activityMinStartTime.HasValue)
                {
                    query = query.Where(p => p.PlanStartDate >= activityMinStartTime.Value);
                }

                if (activityMaxStartTime.HasValue)
                {
                    query = query.Where(p => p.PlanStartDate <= activityMaxStartTime.Value);
                }

                if (activityMinEndTime.HasValue)
                {
                    query = query.Where(p => p.PlanEndDate >= activityMinEndTime.Value);
                }

                if (activityMaxEndTime.HasValue)
                {
                    query = query.Where(p => p.PlanEndDate <= activityMaxEndTime.Value);
                }

                if (!string.IsNullOrEmpty(businessPlanID))
                {
                    query = query.Where(a => a.BusinessPlanID.Trim().Equals(businessPlanID.Trim()));
                }

                if (dataGroupID.HasValue)
                {
                    query = query.Where(p => p.DataGroupID == dataGroupID.Value);
                }

                if (status!=1){
                    if (status.HasValue)
                    {
                        query = query.Where(p => p.Status == status.Value);
                    }
                }

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(pageInfo) });
            }
        }

        public static Result UpdateActivityStatusByID(int activityID, short status, string updateUser, int? dataGroupID = null)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_Act_Master activity;

                    if (dataGroupID.HasValue)
                    {
                        activity = db.TM_Act_Master
                             .Where(a => a.ActivityID == activityID && a.DataGroupID == dataGroupID.Value).FirstOrDefault();
                    }
                    else
                    {
                        activity = db.TM_Act_Master
                             .Where(a => a.ActivityID == activityID).FirstOrDefault();
                    }

                    if (activity != null)
                    {
                        activity.Status = status;
                        activity.ModifiedDate = DateTime.Now;
                        activity.ModifiedUser = updateUser;
                        db.Entry<TM_Act_Master>(activity).State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "", activityID);
                    }
                    return new Result(false, "更新失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        /// <summary>
        /// 获取市场活动列表
        /// </summary>
        /// <param name="id">活动编号</param>
        /// <param name="name">活动名称</param>
        /// <param name="enable">是否可用</param>
        /// <param name="startF">开始时间从</param>
        /// <param name="startE">开始时间到</param>
        /// <param name="endF">结束时间从</param>
        /// <param name="endE">结束时间到</param>
        /// <param name="skipCnt">跳过条数</param>
        /// <param name="takeCnt">显示条数</param>
        /// <returns></returns>
        public static Result GetActivities(int? id, string name, bool? enable, DateTime? startF, DateTime? startE, DateTime? endF, DateTime? endE, int userId, int userGroupId, int? searchGroup, string searchStore, string dp)
        {
            using (CRMEntities db = new CRMEntities())
            {
                DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
                //var qAct = db.TM_Act_Master.Where(p => id.HasValue ? p.ActivityID == id : true).OrderByDescending(o => o.ActivityID).Skip(myDp.iDisplayStart).Take(myDp.iDisplayLength).ToList();

                //return new Result(true, "", new List<object> { qAct.ToDataTableSourceVsPage(myDp) });
                List<string> brand = new List<string>();
                var authStore = Service.GetStoreNameByUserID(userId, ref brand);
                string authBrand = "";
                foreach (string s in brand)
                {
                    authBrand += s;
                    authBrand += ",";
                }
                string authGroups = "";
                var qGroup = db.V_M_TM_SYS_BaseData_brand.Where(o => SqlFunctions.CharIndex(authBrand, o.BrandCodeBase) >= 0).ToList();
                for (int i = 0; i < qGroup.Count; i++)
                {
                    authGroups += qGroup[i].DataGroupID.ToString();
                    authGroups += ",";
                }

                var op = new ObjectParameter("recordCount", typeof(int));
                var res = db.sp_Act_GetSubdivision(id, name, enable, startF, startE, endF, endE, searchGroup, searchStore, userGroupId, authStore, authGroups, myDp.iDisplayStart, myDp.iDisplayLength, op).ToList();
                int count = (int)op.Value;

                var result = new DatatablesSourceVsPage();
                result.iDisplayStart = myDp.iDisplayStart;
                result.iDisplayLength = myDp.iDisplayLength;
                result.iTotalRecords = count;
                result.aaData = res;
                return new Result(true, "", new List<object> { result });
            }
        }

        /// <summary>
        /// 获取活动信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static Result GetActivity(int activityId)
        {
            using (CRMEntities entity = new CRMEntities())
            {
                return new Result(true, "加载成功", entity.TM_Act_Master.FirstOrDefault(p => p.ActivityID == activityId));
            }
        }


        public static Result GetActivityKPIResult(int? activityID, int? dataGroupID = null)
        {
            if (!activityID.HasValue)
            {
                return new Result(false, "", null);
            }
            else
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var queryKPIResult = from kpr in db.TM_CRM_KPIResult
                                         join kpibase in db.TM_CRM_KPI on kpr.KPIID equals kpibase.KPIID
                                         into kp
                                         from k in kp.DefaultIfEmpty()
                                         join kpt in db.TM_CRM_KPITarget on new { kpr.KPIID, kpr.KPIType, kpr.KPITypeValue } equals new { kpt.KPIID, kpt.KPIType, kpt.KPITypeValue }
                                         into kp2
                                         from k2 in kp2.DefaultIfEmpty()
                                         where kpr.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((decimal?)activityID).Trim()) && kpr.KPIType.Trim().Equals("4")
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

                    var queryMaxTime = (from kpr in db.TM_CRM_KPIResult
                                        where kpr.KPIType.Trim().Equals("4") && kpr.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((decimal?)activityID).Trim())
                                        select kpr.ComputeTime).Count() > 0 ?
                                        (from kpr in db.TM_CRM_KPIResult
                                         where kpr.KPIType.Trim().Equals("4") && kpr.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((decimal?)activityID).Trim())
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
                                                  DataGroupID = rst.DataGroupID.Trim()
                                              };
                    if (dataGroupID.HasValue)
                    {
                        queryResultColl = queryResultColl.Where(qrc => qrc.DataGroupID.Trim().Equals(SqlFunctions.StringConvert((decimal?)dataGroupID).Trim()));
                    }
                    return new Result(true, "", new List<object> { queryResultColl.ToList() });
                }

            }

        }

        public static Result GetAcitivityKPITarget(int? activityID, int? dataGroupID = null)
        {
            if (!activityID.HasValue)
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
                                         where kpt.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((decimal?)activityID).Trim()) && kpt.KPIType.Trim().Equals("4")
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
                    return new Result(true, "", new List<object> { queryKPITarget.ToList() });
                }
            }
        }

        /// <summary>
        /// 保存活动信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="subDivisionIds"></param>
        /// <returns></returns>
        public static Result SaveActivity(string actMaster, string subDivisionIdStr, bool hasSids, string activityTargetList = null)
        {
            try
            {
                TM_Act_Master model = JsonHelper.Deserialize<TM_Act_Master>(actMaster);
                List<Guid> subDivisionIds = JsonHelper.Deserialize<List<Guid>>(subDivisionIdStr);

                using (CRMEntities entity = new CRMEntities())
                {
                    if (entity.TM_Act_Master.Any(p => p.ReferenceNo != model.ReferenceNo))
                    {
                        return new Result(false, "该微信活动已被引用", new[] { "该微信活动已被引用" });
                    }

                    if (entity.TM_Act_Master.Any(p => p.ActivityID != model.ActivityID && p.ActivityName == model.ActivityName))
                    {
                        return new Result(false, "市场活动名称已经存在", new[] { "市场活动名称已经存在" });
                    }
                    using (entity.BeginTransaction())
                    {

                        List<TM_CRM_KPITarget> targetList = null;

                        if (model.ActivityID != 0)
                        {
                            entity.TM_Act_Master.Attach(model);
                            var entry = entity.Entry<TM_Act_Master>(model);
                            entry.State = EntityState.Modified;
                            if (hasSids)
                            {
                                entity.Database.ExecuteSqlCommand("DELETE FROM TM_Act_Subdivision WHERE ActivityID = {0}", model.ActivityID);
                            }

                            List<TM_CRM_KPITarget> list = entity.TM_CRM_KPITarget.
                                Where(t => t.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((double?)model.ActivityID).Trim()) && t.KPIType == "4").ToList();

                            foreach (TM_CRM_KPITarget deleteObj in list)
                            {
                                entity.Entry<TM_CRM_KPITarget>(deleteObj).State = EntityState.Deleted;
                            }

                            List<TM_CRM_KPIResult> listResult = entity.TM_CRM_KPIResult.
                                Where(t => t.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((double?)model.ActivityID).Trim()) && t.KPIType == "4").ToList();

                            foreach (TM_CRM_KPIResult deleteResultObj in listResult)
                            {
                                entity.Entry<TM_CRM_KPIResult>(deleteResultObj).State = EntityState.Deleted;
                            }

                            if (!string.IsNullOrEmpty(activityTargetList))
                            {
                                targetList = JsonHelper.Deserialize<List<TM_CRM_KPITarget>>(activityTargetList);

                                foreach (TM_CRM_KPITarget obj in targetList)
                                {
                                    entity.Entry<TM_CRM_KPITarget>(obj).State = EntityState.Added;
                                }
                            }
                            entity.SaveChanges();
                        }
                        else
                        {
                            entity.TM_Act_Master.Add(model);
                            entity.SaveChanges();

                            if (!string.IsNullOrEmpty(activityTargetList))
                            {
                                targetList = JsonHelper.Deserialize<List<TM_CRM_KPITarget>>(activityTargetList);
                                foreach (TM_CRM_KPITarget obj in targetList)
                                {
                                    obj.KPITypeValue = model.ActivityID.ToString().Trim();
                                    entity.Entry<TM_CRM_KPITarget>(obj).State = EntityState.Added;
                                }
                            }

                            entity.SaveChanges();
                        }

                        if (hasSids)
                        {
                            foreach (var id in subDivisionIds)
                            {
                                entity.TM_Act_Subdivision.Add(new TM_Act_Subdivision
                                {
                                    ActivityID = model.ActivityID,
                                    SubdivisionID = id
                                });
                            }
                        }
                        entity.SaveChanges();
                        entity.Commit();
                    }
                    return new Result(true, "保存成功", model);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        /// <summary>
        /// 删除活动信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static Result DelActivity(int activityId)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var cantdel = db.TM_Act_Instance.Any(p => p.ActivityID == activityId);
                    if (cantdel)
                    {
                        return new Result(false, "运行过的市场活动不能删除");
                    }


                    List<TM_CRM_KPITarget> list = db.TM_CRM_KPITarget.
                        Where(t => t.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((double?)activityId).Trim()) && t.KPIType == "4").ToList();

                    if (list != null)
                    {
                        foreach (TM_CRM_KPITarget deleteObj in list)
                        {
                            db.Entry<TM_CRM_KPITarget>(deleteObj).State = EntityState.Deleted;
                        }
                    }

                    List<TM_CRM_KPIResult> listResult = db.TM_CRM_KPIResult.
                        Where(t => t.KPITypeValue.Trim().Equals(SqlFunctions.StringConvert((double?)activityId).Trim()) && t.KPIType == "4").ToList();

                    if (listResult != null)
                    {
                        foreach (TM_CRM_KPIResult deleteResultObj in listResult)
                        {
                            db.Entry<TM_CRM_KPIResult>(deleteResultObj).State = EntityState.Deleted;
                        }
                    }

                    db.TM_Act_Master.Delete(p => p.ActivityID == activityId);
                    db.SaveChanges();
                    return new Result(true, "删除成功", (object)activityId);
                }

            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        /// <summary>
        /// 根据市场活动Id查找会员细分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Result GetMemberSubdivisionByActivityId(int id)
        {
            using (var db = new CRMEntities())
            {
                //查询激活的和人工导入的会员细分
                var query = from ms in db.TM_Mem_Subdivision.Where(o => o.Enable == true || (o.Enable == false && o.SubDevDataType == "2"))
                            join ta in
                                from tas in db.TM_Act_Subdivision
                                where tas.ActivityID == id
                                select tas
                            on ms.SubdivisionID equals ta.SubdivisionID
                            into o
                            orderby ms.AddedDate descending
                            select new { ms.SubdivisionID, ms.SubdivisionName, Checked = o.Any() };
                return new Result(true, "", query.ToList());
            }
        }
        /// <summary>
        /// 按门店获取会员细分
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static Result GetMemberSubdivisionListByStore(string store)
        {
            using (var db = new CRMEntities())
            {
                //查询激活的和人工导入的会员细分
                var query = from ms in db.TM_Mem_Subdivision.Where(o => o.Enable == true || (o.Enable == false && o.SubDevDataType == "2"))
                            //where ms.StoreCode == store
                            orderby ms.AddedDate descending
                            select new { ms.SubdivisionID, ms.SubdivisionName };
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 获取活动关联细分的人数
        /// </summary>
        /// <param name="subIds"></param>
        /// <returns></returns>
        public static Result GetSubdivisionMemCount(string subIds)
        {
            try
            {
                List<Guid> subdivisonList = new List<Guid>();
                subdivisonList = JsonHelper.Deserialize<List<Guid>>(subIds);

                using (var db = new CRMEntities())
                {
                    var activitySubdivisionList = (from a in db.TM_Mem_Subdivision.Where(o => subdivisonList.Any(s => o.SubdivisionID.Equals(s)))
                                                   join b in db.TM_Mem_SubdivisionInstance on a.CurSubdivisionInstanceID equals b.SubdivisionInstanceID
                                                   select b).ToList();
                    if (activitySubdivisionList != null && activitySubdivisionList.Count > 0)
                    {
                        StringBuilder execSQL = new StringBuilder();
                        execSQL.AppendFormat("select count(distinct(MemberID)) ");
                        execSQL.AppendFormat("from ( ");
                        execSQL.AppendFormat("select MemberID from [{0}] ", activitySubdivisionList[0].TableName);
                        for (int index = 1; index < activitySubdivisionList.Count; index++)
                        {
                            var obj = activitySubdivisionList[index];
                            execSQL.AppendFormat("union all ");
                            execSQL.AppendFormat("select MemberID from [{0}] ", obj.TableName);
                        }
                        execSQL.AppendFormat(") t ");
                        var cmd = db.Database.Connection.CreateCommand();
                        cmd.CommandTimeout = 60;
                        cmd.CommandText = execSQL.ToString();
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                        {
                            db.Database.Connection.Open();
                        }
                        int count= Convert.ToInt32(cmd.ExecuteScalar());
                        return new Result(true, "", count);
                    }
                    else
                    {
                        return new Result(false, "", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, 0);
            }

        }

        /// <summary>
        /// 获取分组后的模板
        /// </summary>
        /// <returns></returns>
        public static Result GetGroupedTemplates(string sessionStr)
        {
            Dictionary<string, object> dictSession = JsonHelper.Deserialize<Dictionary<string, object>>(sessionStr);
            AuthModel authModel = JsonHelper.Deserialize<AuthModel>(dictSession["auth"].ToString());
            int dataGroupID = authModel.DataGroupID == null ? 0 : (int)authModel.DataGroupID;
            using (CRMEntities db = new CRMEntities())
            {
                var qGroupID = (from b in db.V_Sys_DataGroupRelation.Where(p => p.DataGroupID == dataGroupID)
                                select b.SubDataGroupID).ToList();
                var couponList = from c in db.TM_Mem_CouponPool
                                 where c.Enable == true && c.IsUsed == false
                                 select c;

                var gRecords = db.TM_Act_CommunicationTemplet.Where(o => qGroupID.Contains(o.DataGroupID)
                        && o.Enable
                        && !o.BasicContent.Contains(",\"ispublic\":true,"))
                    .Join(db.TD_SYS_BizOption.Where(o => o.OptionType == "CommunicationType"), o => o.Category, i => i.OptionValue,
                    (o, i) => new
                    {
                        Type = o.Type,
                        Name = o.Name,
                        TempletID = o.TempletID,
                        Category = o.Category,
                        CategoryText = i.OptionText,
                        BasicContent = o.BasicContent.Trim(),
                        OtherIndustryCouponAmount = couponList.Where(c => c.TempletID == o.TempletID
                            && o.Type == "Coupon"
                            && (string.IsNullOrEmpty(c.MemberID))
                            && o.BasicContent.Contains(",\"isOthers\":true,")).Count()
                    })
                    //.Where(p => p.Enable)
                    .Select(p => new
                    {
                        p.Type,
                        p.TempletID,
                        p.Name,
                        p.Category,
                        p.CategoryText,
                        p.BasicContent,
                        p.OtherIndustryCouponAmount
                    })
                    .GroupBy(p => p.Type).ToList();

                Dictionary<string, object> res = new Dictionary<string, object>();
                foreach (var g in gRecords)
                {
                    res.Add(g.Key, g.ToList());
                }
                return new Result(true, "获取成功", res);
            }
        }

        /// <summary>
        /// 获取会员细分
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static Result GetMemberSubdivisionList(int activityId)
        {
            using (CRMEntities crm = new CRMEntities())
            {
                var query = from a in crm.TM_Mem_Subdivision
                            join b in crm.TM_Mem_SubdivisionInstance on a.SubdivisionID equals b.SubdivisionID
                            join c in crm.TM_Act_InstanceSubdivision on b.SubdivisionInstanceID equals c.SubdivisionInstanceID
                            join d in crm.TM_Act_Instance on c.ActivityInstanceID equals d.ActivityInstanceID
                            where d.ActivityID == activityId
                            select a;
                return new Result(true, "", query.Distinct().OrderByDescending(p => p.AddedDate).ToList());
            }
        }

        /// <summary>
        /// 获取活动实例
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static Result GetActivityInstanceList(int activityId)
        {
            using (CRMEntities crm = new CRMEntities())
            {
                var query = from i in crm.TM_Act_Instance
                            from s in crm.TM_Act_InstanceSubdivision
                            where i.ActivityInstanceID == s.ActivityInstanceID
                            && i.ActivityID == activityId
                            select i;
                return new Result(true, "", query.Distinct().ToList());
            }
        }

        /// <summary>
        /// 获取市场活动细分列表
        /// </summary>
        /// <param name="activityInstanceId">市场活动编号</param>
        /// <param name="cardNo">会员卡号</param>
        /// <param name="memberName">会员名称</param>
        /// <param name="subdivisionId">会员细分编号</param>
        /// <param name="skipCnt">跳过条数</param>
        /// <param name="takeCnt">显示条数</param>
        /// <returns></returns>
        public static Result GetActivitySubdivisionList(long activityInstanceId, string cardNo, string memberName, Guid? subdivisionId, string dp)
        {
            using (CRMEntities crm = new CRMEntities())
            {
                DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
                var mst = crm.TM_Act_Instance.FirstOrDefault(p => p.ActivityInstanceID == activityInstanceId);
                var act = Util.Deserialize<Activity>(mst.Workflow);

                var qTemplateIds = GetQuestionActivity(act).Select(p => p.TemplateId).ToList();
                var qTemplates = crm.TM_Act_CommunicationTemplet.Where(p => qTemplateIds.Contains(p.TempletID)).ToList();

                var couponIds = GetCouponActivity(act).Select(p => p.Id).Cast<Guid?>().ToList();
                var coupons = crm.TM_Mem_CouponPool.Where(p => activityInstanceId == p.ActInstanceID && p.IsUsed && couponIds.Contains(p.WorkflowID)).ToList();

                var op = new ObjectParameter("pagecount", typeof(int));
                var res = crm.sp_Act_MarketActivitySubdivision(activityInstanceId, cardNo, memberName, subdivisionId.ToString(), myDp.iDisplayLength, myDp.iDisplayStart, op).ToList();
                res.ForEach(p =>
                {
                    CombineActiviSubdivision(p, act, qTemplates, coupons, activityInstanceId);
                });
                int count = (int)op.Value;
                var result = new DatatablesSourceVsPage();
                result.iDisplayStart = myDp.iDisplayStart;
                result.iDisplayLength = myDp.iDisplayLength;
                result.iTotalRecords = count;
                result.aaData = res;
                return new Result(true, "", new List<object> { result });
            }
        }

        /// <summary>
        /// 获取市场活动细分列表导出
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="cardNo"></param>
        /// <param name="memberName"></param>
        /// <param name="subdivisionId"></param>
        /// <returns></returns>
        public static Result GetActivitySubdivisionListNoPage(long activityInstanceId, string cardNo, string memberName, Guid? subdivisionId)
        {
            using (CRMEntities crm = new CRMEntities())
            {
                var mst = crm.TM_Act_Instance.FirstOrDefault(p => p.ActivityInstanceID == activityInstanceId);
                var act = Util.Deserialize<Activity>(mst.Workflow);

                var qTemplateIds = GetQuestionActivity(act).Select(p => p.TemplateId).ToList();
                var qTemplates = crm.TM_Act_CommunicationTemplet.Where(p => qTemplateIds.Contains(p.TempletID)).ToList();

                var couponIds = GetCouponActivity(act).Select(p => p.Id).Cast<Guid?>().ToList();
                var coupons = crm.TM_Mem_CouponPool.Where(p => activityInstanceId == p.ActInstanceID && p.IsUsed && couponIds.Contains(p.WorkflowID)).ToList();

                var op = new ObjectParameter("pagecount", typeof(int));
                var res = crm.sp_Act_MarketActivitySubdivision(activityInstanceId, cardNo, memberName, subdivisionId.ToString(), 0, 0, op).ToList();
                res.ForEach(p =>
                {
                    CombineActiviSubdivision(p, act, qTemplates, coupons, activityInstanceId);
                });
                int count = (int)op.Value;
                return new Result(true, "加载成功", res);
            }
        }


        private static List<Activity> GetQuestionActivity(Activity act)
        {
            var lst = new List<Activity>();
            if (act.Category == ActivityCategory.Question || act.Category == ActivityCategory.OB) lst.Add(act);
            foreach (var chd in act.Children)
            {
                lst.AddRange(GetQuestionActivity(chd));
            }
            return lst;
        }

        public static List<Activity> GetCouponActivity(Activity act)
        {
            var lst = new List<Activity>();
            if (act.Category == ActivityCategory.Coupon) lst.Add(act);
            foreach (var chd in act.Children)
            {
                lst.AddRange(GetQuestionActivity(chd));
            }
            return lst;
        }

        /// <summary>
        /// 修正明细名称
        /// </summary>
        /// <param name="masr"></param>
        /// <param name="act"></param>
        private static void CombineActiviSubdivision(sp_Act_MarketActivitySubdivision_Result masr, Activity act, List<TM_Act_CommunicationTemplet> templates, List<TM_Mem_CouponPool> coupons, long actInstanceId)
        {
            masr.SubdivisionName = masr.SubdivisionName.TrimEnd(',');

            var wfIds = masr.WfRootId
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
            StringBuilder wfIdStr = new StringBuilder();
            StringBuilder tmpStr = new StringBuilder();
            foreach (var id in wfIds)
            {
                var o = id.Split('|');
                var tmp = Activity.Find(x => x.Id == Guid.Parse(o[0]), new[] { act });
                if (tmp == null) continue;
                if (tmp.Category == ActivityCategory.Question || tmp.Category == ActivityCategory.OB)
                {
                    var temp = templates.FirstOrDefault(p => p.TempletID == tmp.TemplateId);
                    if (temp != null)
                    {
                        tmpStr.Append("," + temp.Name);
                    }
                }
                if (tmp.Category == ActivityCategory.Coupon)
                {
                    var coupon = coupons.FirstOrDefault(p => p.WorkflowID == tmp.Id && p.MemberID == masr.MemberId);
                    if (coupon != null)
                    {
                        wfIdStr.Append(",已使用优惠券");
                    }
                    else
                    {
                        wfIdStr.Append(",已经生成优惠券");
                    }
                }
                else
                {
                    if (tmp.Category != ActivityCategory.SMS)
                    {
                        if (o[1] == "4") wfIdStr.Append(",已经");
                        else wfIdStr.Append(",未");
                    }
                    switch (tmp.Category)
                    {
                        case ActivityCategory.OB:
                            wfIdStr.Append("打电话"); break;
                        case ActivityCategory.SMS:
                            using (CRMEntities crm = new CRMEntities())
                            {
                                if (Convert.ToInt16(o[1]) >= 2 && Convert.ToInt16(o[1]) < 5)
                                {
                                    var qSend = crm.TM_Sys_SMSSendingQueue.Where(p => p.MemberID == masr.MemberId && p.ActInstanceID == actInstanceId).FirstOrDefault();
                                    if (qSend != null && qSend.IsSent)
                                    {
                                        wfIdStr.Append(",已经");
                                    }
                                    else
                                        wfIdStr.Append(",未");
                                }
                                else wfIdStr.Append(",未");
                            }
                            wfIdStr.Append("发送短信");
                            break;
                        case ActivityCategory.Mail:
                            wfIdStr.Append("发送邮件"); break;
                        case ActivityCategory.Question:
                            wfIdStr.Append("回答问卷调查"); break;
                        case ActivityCategory.Coupon:
                            wfIdStr.Append("发送优惠券"); break;
                        default:
                            wfIdStr.Append("执行"); break;
                    }
                }
            }
            if (wfIdStr.Length > 1) masr.WfRootId = wfIdStr.ToString(1, wfIdStr.Length - 1);
            if (tmpStr.Length > 1) masr.Templates = tmpStr.ToString(1, tmpStr.Length - 1);
        }

        public static Result CheckRefExist(string refNO) {
            using (var db = new CRMEntities())
            {
                var query = (from a in db.TM_Act_Master
                             where a.ReferenceNo == refNO 
                             select new
                             {
                                 exist = 1
                             }).ToList();
                return new Result(true, "", query.ToList());
            }
        }

    }
}
