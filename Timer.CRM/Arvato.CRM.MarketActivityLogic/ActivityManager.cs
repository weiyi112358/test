using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.EF;
using Arvato.CRM.Utility.WorkFlow;
using System.Configuration;
using Arvato.CRM.Utility;
using System.Data;
using Arvato.CRM.Utility.WorkFlow.Templates;
using System.Text.RegularExpressions;

namespace Arvato.CRM.MarketActivityLogic
{
    public class ActivityManager
    {
        private static double activeInternal = Convert.ToDouble(ConfigurationManager.AppSettings["ActiveInternal"]);
        private static int maxCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRecords"].ToString());
        private static string templetSMS = ConfigurationManager.AppSettings["CouponSMSTemplate"].ToString();
        private static string subjectEmail = ConfigurationManager.AppSettings["CouponMailSubjectTemplate"].ToString();
        private static string templetEmail = ConfigurationManager.AppSettings["CouponMailTemplate"].ToString();

        /// <summary>
        /// 创建市场活动实例
        /// </summary>
        public static void Active()
        {
            using (var db = new CRMEntities())
            {
                DateTime now = DateTime.Now;
                var masterList = db.TM_Act_Master.Where(o => o.Enable
                        && o.PlanStartDate <= now
                        && (o.PlanEndDate == null || o.PlanEndDate >= now)
                        && o.WfRootId != null).ToList();
                //var masterList = db.TM_Act_Master.Where(o => o.Enable
                //        && o.PlanStartDate <= now
                //        && (o.PlanEndDate == null || o.PlanEndDate >= now)
                //        && o.WfRootId != null
                //        && o.ActivityID == 61).ToList();
                foreach (var master in masterList)
                {
                    try
                    {
                        var schedule = Util.Deserialize<Schedule>(master.Schedule);
                        if (schedule != null)
                        {
                            if (schedule.Calc())
                            {
                                double secondes = (schedule.CurRunDate - now).TotalMilliseconds;
                                if (secondes >= (0 - activeInternal / 2) && secondes < activeInternal / 2)
                                {
                                    //string selectType = "";
                                    //if (schedule.CustomerSelectType == CustomerSeltctType.Rate)
                                    //{
                                    //    selectType = "rate";
                                    //}
                                    //else if (schedule.CustomerSelectType == CustomerSeltctType.Amount)
                                    //{
                                    //    selectType = "amount";
                                    //}
                                    db.sp_Act_MarketActivityActive(master.ActivityID, schedule.CustomerSelectType.ToString(), schedule.CustomerRate, schedule.CustomerAmount);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ServiceLog("MarketActivityEngin-Active", "ActivityID:" + master.ActivityID.ToString() + "\n" + ex.ToString());
                        //throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 翻译市场活动步骤
        /// </summary>
        public static void Translate()
        {
            using (var db = new CRMEntities())
            {
                var instanceList = (from i in db.TM_Act_Instance
                                    where i.Enable && !i.IsTranslated && i.Status == 2 && i.WfRootId != null
                                    select i).ToList();
                foreach (var instance in instanceList)
                {
                    try
                    {
                        var step = Util.Deserialize<Activity>(instance.Workflow);
                        Stack<Activity> stack = new Stack<Activity>();
                        step.RunDate = DateTime.Now.AddDays(step.Wait);
                        bool isFirst = true;
                        stack.Push(step);
                        if (step.Category != ActivityCategory.Wait)
                        {
                            db.TM_Act_InstanceStep.Add(new TM_Act_InstanceStep
                            {
                                ActivityInstanceID = instance.ActivityInstanceID,
                                InstanceStepID = step.Id,
                                Category = step.Category.ToString(),
                                Condition = step.Condition,
                                ExpiredDate = step.ExpiredDate,
                                ResultType = step.ResultType.ToString(),
                                RunDate = step.RunDate.Value,
                                SendChannel = step.SendChannel,
                                SendMail = step.SendMail,
                                SendSMS = step.SendSMS,
                                TemplateId = step.TemplateId,
                                ValidDay = step.ValidDay,
                                Wait = step.Wait,
                                AddedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                DataLimit = step.DataLimit,
                                LimitType = step.LimitType,
                                LimitValue = string.Join(",", step.LimitValues)
                            });
                            isFirst = false;
                        }
                        while (stack.Count > 0)
                        {
                            Activity activity = stack.Pop();
                            if (activity.Children != null)
                            {
                                foreach (var child in activity.Children)
                                {
                                    child.RunDate = (activity.RunDate ?? DateTime.Now).AddDays(child.Wait);
                                    if (child.Category == ActivityCategory.Wait)
                                    {
                                        child.Id = activity.Id;
                                    }
                                    else
                                    {
                                        var cStep = new TM_Act_InstanceStep
                                        {
                                            ActivityInstanceID = instance.ActivityInstanceID,
                                            InstanceStepID = child.Id,
                                            Category = child.Category.ToString(),
                                            Condition = child.Condition,
                                            ExpiredDate = child.ExpiredDate,
                                            ResultType = child.ResultType.ToString(),
                                            RunDate = child.RunDate.Value,
                                            SendChannel = child.SendChannel,
                                            SendMail = child.SendMail,
                                            SendSMS = child.SendSMS,
                                            TemplateId = child.TemplateId,
                                            ValidDay = child.ValidDay,
                                            Wait = child.Wait,
                                            AddedDate = DateTime.Now,
                                            ModifiedDate = DateTime.Now,
                                            DataLimit = step.DataLimit,
                                            LimitType = step.LimitType,
                                            LimitValue = string.Join(",", step.LimitValues)
                                        };
                                        if (!isFirst)
                                        {
                                            cStep.ParentInstanceStepID = activity.Id;
                                        }
                                        isFirst = false;
                                        db.TM_Act_InstanceStep.Add(cStep);
                                    }
                                    stack.Push(child);
                                }
                            }
                        }
                        instance.IsTranslated = true;
                        db.Entry(instance).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Log.ServiceLog("MarketActivityEngin-Translate", "ActivityInstanceID:" + instance.ActivityInstanceID.ToString() + "\n" + ex.ToString());
                        //throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 推送活动步骤
        /// </summary>
        public static void Push()
        {
            using (var db = new CRMEntities())
            {
                var instances = db.TM_Act_Instance.Where(o => o.Enable && o.IsTranslated && o.WfRootId != null && o.Status != 3).ToList();
                foreach (var instance in instances)
                {
                    var steps = db.sp_Act_MarketActivityRecordStep(instance.ActivityInstanceID, instance.TableName, 1).ToList();
                    foreach (var step in steps)
                    {
                        try
                        {
                            if (step.ExpiredDate != null && step.ExpiredDate < DateTime.Now)
                            {
                                db.sp_Act_MarketActivityRecordExpired(instance.TableName, step.InstanceStepID, 1);
                            }
                            else
                            {
                                if (step.TemplateId != null)
                                {
                                    ActivityCategory categroy;
                                    if (Enum.TryParse<ActivityCategory>(step.Category, out categroy))
                                    {
                                        switch (categroy)
                                        {
                                            case ActivityCategory.Coupon:
                                                pushCoupon(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.TemplateId.Value, step.SendSMS, step.SendMail);
                                                break;
                                            case ActivityCategory.OB:
                                                pushOB(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.TemplateId.Value);
                                                break;
                                            case ActivityCategory.Question:
                                                pushServey(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.TemplateId.Value, step.SendChannel);
                                                break;
                                            case ActivityCategory.SMS:
                                                pushSMS(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.TemplateId.Value);
                                                break;
                                            case ActivityCategory.Mail:
                                                pushMail(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.TemplateId.Value);
                                                break;
                                            case ActivityCategory.WeChat:
                                                pushWechat(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.TemplateId.Value);
                                                break;
                                            default:
                                                pushNormal(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.TemplateId.Value);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Log.ServiceLog("MarketActivityEngin-Push", "InstanceId:" + instance.ActivityInstanceID + ", SetpId:" + step.InstanceStepID + ", 模板Category错误");
                                    }
                                }
                                else
                                {
                                    Log.ServiceLog("MarketActivityEngin-Push", "InstanceId:" + instance.ActivityInstanceID + ", SetpId:" + step.InstanceStepID + ", 模板为空");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.ServiceLog("MarketActivityEngin-Push", "ActivityInstanceID:" + step.ActivityInstanceID.ToString() + "\n"
                                + "InstanceStepID:" + step.InstanceStepID.ToString() + "\n" + ex.ToString());
                        }
                    }
                }
            }
        }

        private static void pushCoupon(long instanceId, string tableName, Guid wfRootId, int templateId, bool isSMS, bool isEmail)
        {
            using (var db = new CRMEntities())
            {
                db.sp_Act_MarketActivityPushCoupon(instanceId, tableName, wfRootId, templateId, isSMS, isEmail, maxCount);

                //var tpl = db.TM_Act_CommunicationTemplet.FirstOrDefault(p => p.TempletID == templateId && p.Type == "Coupon");

                //if (tpl != null)
                //{
                //    Coupon coupon;
                //    switch (tpl.SubType.ToLower())
                //    {
                //        default:
                //        case "1": //现金券
                //            coupon = Util.Deserialize<VoucherCoupon>(tpl.BasicContent);
                //            break;
                //        case "2": //折扣券
                //            coupon = Util.Deserialize<DiscountCoupon>(tpl.BasicContent);
                //            break;
                //        case "3": //满减券
                //            coupon = Util.Deserialize<ReduceCoupon>(tpl.BasicContent);
                //            break;
                //    }
                //    if (coupon != null)
                //    {
                //        DateTime start = DateTime.Now;
                //        if (coupon.StartDate != null && start < coupon.StartDate)
                //        {
                //            start = coupon.StartDate.Value;
                //        }
                //        DateTime end = DateTime.Now;
                //        //db.sp_Act_MarketActivityPushCoupon(instanceId, tableName, wfRootId, templateId, tpl.SubType ?? "", coupon.StartDate, coupon.EndDate, coupon.OffNumber, coupon.Unit,
                //        //   isSMS, isEmail, templetSMS, subjectEmail, templetEmail, maxCount);
                //    }
                //}
            }
        }

        private static void pushOB(long instanceId, string tableName, Guid wfRootId, int templateId)
        {
            using (var db = new CRMEntities())
            {
                var tpl = db.TM_Act_CommunicationTemplet.FirstOrDefault(p => p.TempletID == templateId && p.Type == "OB");

                if (tpl != null)
                {
                    db.sp_Act_MarketActivityPushOB(instanceId, tableName, wfRootId, tpl.BasicContent, tpl.ForeignTempletID, maxCount);
                }
            }
        }

        private static void pushServey(long instanceId, string tableName, Guid wfRootId, int templateId, string sendChannel)
        {
            using (var db = new CRMEntities())
            {
                var tpl = db.TM_Act_CommunicationTemplet.FirstOrDefault(p => p.TempletID == templateId && p.Type == "Question");

                if (tpl != null)
                {
                    db.sp_Act_MarketActivityPushSurvey(instanceId, tableName, wfRootId, templateId, sendChannel, tpl.BasicContent, maxCount);
                }
            }
        }

        private static void pushSMS(long instanceId, string tableName, Guid wfRootId, int templateId)
        {
            using (var db = new CRMEntities())
            {
                var tpl = db.TM_Act_CommunicationTemplet.FirstOrDefault(p => p.TempletID == templateId && p.Type == "SMS");

                if (tpl != null)
                {
                    //Regex reg = new Regex(@"{[a-zA-Z \u4e00-\u9fa5]+}");
                    //MatchCollection mc = reg.Matches(tpl.BasicContent);
                    //List<Dictionary<string, string>> lstDict = new List<Dictionary<string, string>>();
                    string sendPara = "";
                    //StringBuilder sbPara = new StringBuilder();
                    //if (mc != null && mc.Count > 0)
                    //{
                    //    sbPara.Append("{");
                    //    for (int i = 0; i < mc.Count; i++)
                    //    {
                    //        sbPara.AppendFormat("\"{0}\":\"{{0}}\"", mc[i].Value.ToString().TrimStart('{').TrimEnd('}'));
                    //        if (i != mc.Count - 1)
                    //            sbPara.Append(",");
                    //    }
                    //    sbPara.Append("}");
                    //    //if (sbPara != null)
                    //    //{
                    //    //    sbPara.Remove(sbPara.Length - 1, 1);
                    //    //    sendPara = sbPara.ToString();
                    //    //}
                    //}
                    db.sp_Act_MarketActivityPushSMS(instanceId, tableName, wfRootId, templateId, tpl.BasicContent, sendPara, maxCount);
                }
            }
        }

        private static void pushMail(long instanceId, string tableName, Guid wfRootId, int templateId)
        {
            using (var db = new CRMEntities())
            {
                var tpl = db.TM_Act_CommunicationTemplet.FirstOrDefault(p => p.TempletID == templateId && p.Type == "Mail");

                if (tpl != null)
                {
                    db.sp_Act_MarketActivityPushEmail(instanceId, tableName, wfRootId, templateId, tpl.Topic, tpl.BasicContent, maxCount);
                }
            }
        }

        private static void pushNormal(long instanceId, string tableName, Guid wfRootId, int templateId)
        {
            using (var db = new CRMEntities())
            {
                db.sp_Act_MarketActivityPushNormal(tableName, wfRootId, maxCount);
            }
        }

        private static void pushWechat(long instanceId, string tableName, Guid wfRootId, int templateId)
        {
            using (var db = new CRMEntities())
            {
                var tpl = db.TM_Act_CommunicationTemplet.FirstOrDefault(p => p.TempletID == templateId && p.Type == "WeChat");

                if (tpl != null)
                {
                    db.sp_Act_MarketActivityPushWechat(instanceId, tableName, wfRootId, templateId, tpl.SubType, tpl.BasicContent, maxCount);
                }
            }
        }

        /// <summary>
        /// 获取活动步骤结果
        /// </summary>
        public static void Pull()
        {
            using (var db = new CRMEntities())
            {
                var instances = db.TM_Act_Instance.Where(o => o.Enable && o.IsTranslated && o.WfRootId != null && o.Status != 3).ToList();
                //var instances = db.TM_Act_Instance.Where(o => o.Enable && o.IsTranslated && o.WfRootId != null && o.ActivityInstanceID == 1000051).ToList();
                foreach (var instance in instances)
                {
                    var steps = db.sp_Act_MarketActivityRecordStep(instance.ActivityInstanceID, instance.TableName, 2).ToList();
                    foreach (var step in steps)
                    {
                        try
                        {
                            if (step.ExpiredDate != null && step.ExpiredDate < DateTime.Now)
                            {
                                var nextSteps = steps.Where(o => o.ParentInstanceStepID == step.InstanceStepID).ToList();
                                bool isExpired = true;
                                foreach (var nStep in nextSteps)
                                {
                                    if (nStep.RunDate >= DateTime.Now)
                                    {
                                        isExpired = false;
                                        break;
                                    }
                                }
                                if (isExpired)
                                {
                                    db.sp_Act_MarketActivityRecordExpired(instance.TableName, step.InstanceStepID, 2);
                                }
                            }
                            else
                            {
                                Log4netHelper.WriteInfoLog(instance.ActivityInstanceID.ToString() + instance.TableName + step.InstanceStepID);
                                ActivityCategory categroy;
                                if (Enum.TryParse<ActivityCategory>(step.Category, out categroy))
                                {
                                    switch (categroy)
                                    {
                                        case ActivityCategory.Coupon:
                                            db.sp_Act_MarketActivityPullCoupon(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, maxCount);
                                            break;
                                        case ActivityCategory.OB:
                                            db.sp_Act_MarketActivityPullOB(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, maxCount);
                                            break;
                                        case ActivityCategory.Question:
                                            db.sp_Act_MarketActivityPullSurvey(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, maxCount);
                                            break;
                                        case ActivityCategory.SMS:
                                            StringBuilder sb = new StringBuilder();
                                            var branches = db.TM_Act_InstanceStep.Where(o => o.ActivityInstanceID == instance.ActivityInstanceID && o.ParentInstanceStepID == step.InstanceStepID && o.Category == "Branch").ToList();
                                            if (branches.Count > 0)
                                            {
                                                sb.Append(",");
                                                foreach (var branch in branches)
                                                {
                                                    if (step.ResultType == "Number")
                                                    {
                                                        string[] c = branch.Condition.Split(',');
                                                        int from, to;
                                                        if (c.Length == 1 && int.TryParse(c[0], out from))
                                                        {
                                                            sb.Append(from);
                                                            sb.Append(",");
                                                        }
                                                        else if (c.Length > 1 && int.TryParse(c[0], out from) && int.TryParse(c[c.Length - 1], out to))
                                                        {
                                                            for (int i = from; i <= to; i++)
                                                            {
                                                                sb.Append(i);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sb.Append(branch.Condition);
                                                    }
                                                    sb.Append(",");
                                                }
                                            }
                                            db.sp_Act_MarketActivityPullSMS(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.ResultType, sb.ToString(), maxCount);
                                            break;
                                        case ActivityCategory.Mail:
                                            db.sp_Act_MarketActivityPullEmail(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, maxCount);
                                            break;
                                        case ActivityCategory.WeChat:
                                            db.sp_Act_MarketActivityPullWechat(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, maxCount);
                                            break;
                                        default:
                                            db.sp_Act_MarketActivityPullNormal(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, maxCount);
                                            break;
                                    }
                                    Log4netHelper.WriteInfoLog("finish");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.ServiceLog("MarketActivityEngin-Pull", "ActivityInstanceID:" + step.ActivityInstanceID.ToString() + "\n"
                                + "InstanceStepID:" + step.InstanceStepID.ToString() + "\n" + ex.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成下一步记录
        /// </summary>
        public static void Next()
        {
            using (var db = new CRMEntities())
            {
                var instances = db.TM_Act_Instance.Where(o => o.Enable && o.IsTranslated && o.WfRootId != null && o.Status != 3).ToList();
                foreach (var instance in instances)
                {
                    var steps = db.sp_Act_MarketActivityRecordStep(instance.ActivityInstanceID, instance.TableName, 3).ToList();
                    foreach (var step in steps)
                    {
                        try
                        {
                            db.sp_Act_MarketActivityNextSetp(instance.ActivityInstanceID, instance.TableName, step.InstanceStepID, step.ResultType, maxCount);
                        }
                        catch (Exception ex)
                        {
                            Log.ServiceLog("MarketActivityEngin-Next", "ActivityInstanceID:" + step.ActivityInstanceID.ToString() + "\n"
                                + "InstanceStepID:" + step.InstanceStepID.ToString() + "\n" + ex.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 结束市场活动实例
        /// </summary>
        public static void Finish()
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    db.sp_Act_MarketActivityFinish();
                }
            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-Finish", ex.ToString());
            }
        }


        public static void CreateCoupon()
        {
            using (var db = new CRMEntities())
            {
                var tempIds = db.TM_Mem_CouponCreate.Where(p => p.Status == false).Select(p => new { p.TemplateID, p.DataGroupID }).Distinct().ToList();
                foreach (var tId in tempIds)
                {
                    var tpl = db.TM_Act_CommunicationTemplet.FirstOrDefault(p => p.TempletID == tId.TemplateID && p.DataGroupID == p.DataGroupID && p.Type == "Coupon");
                    if (tpl != null)
                    {
                        Coupon coupon = Util.Deserialize<Coupon>(tpl.BasicContent);
                        if (coupon != null)
                        {
                            if (coupon.StartDate == null)
                            {
                                DateTime curtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                                coupon.StartDate = curtime;
                                switch (coupon.Unit)
                                {
                                    case "year":
                                        coupon.EndDate = curtime.AddYears((int)coupon.OffNumber).AddSeconds(-1);
                                        break;
                                    case "month":
                                        coupon.EndDate = curtime.AddMonths((int)coupon.OffNumber).AddSeconds(-1);
                                        break;
                                    case "day":
                                        coupon.EndDate = curtime.AddDays((int)coupon.OffNumber).AddSeconds(-1);
                                        break;
                                    default:
                                        break;
                                }
                                
                            }
                            db.sp_Act_MarketActivityPushCoupon_Gen(tpl.TempletID, tpl.DataGroupID, coupon.StartDate, coupon.EndDate, templetSMS, maxCount, tpl.Name);
                        }
                    }
                }




            }
        }
    }
}
