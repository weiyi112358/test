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

namespace Arvato.CRM.BizLogic
{
    public static class Loyalty
    {
        #region 会员等级积分计算规则管理
        /// <summary>
        /// 获取会员等级积分计算规则管理（分页）
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="ruleType"></param>
        /// <param name="enable"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetRuleData(string ruleName, string ruleType, string enable, string dp, int groupId)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);

            using (CRMEntities db = new CRMEntities())
            {
                var q1 = from r in db.TM_Loy_Rule
                         join o in db.TD_SYS_BizOption on r.RuleType equals o.OptionValue
                         where o.OptionType == "RuleType"
                         select new
                         {
                             r.RuleType,
                             TypeDesc = o.OptionText,
                             r.RuleRunType,
                             r.DataGroupID,
                             r.Enable,
                             r.RunIndex,
                             r.RuleID,
                             r.RuleName,
                             r.StartDate,
                             r.Schedule,
                             r.ModifiedDate,
                             r.AddedDate
                         };
                var query = from r in q1
                            join oo in db.TD_SYS_BizOption on r.RuleRunType equals oo.OptionValue
                            where oo.OptionType == "RuleRunType"
                            join b in db.V_Sys_DataGroupRelation on r.DataGroupID equals b.SubDataGroupID into bb
                            from bcg in bb.DefaultIfEmpty()
                            where bcg.DataGroupID == groupId
                            select new
                            {
                                r.RuleType,
                                r.TypeDesc,
                                r.RuleRunType,
                                SubTypeDesc = oo.OptionText,
                                r.Enable,
                                r.RunIndex,
                                r.RuleID,
                                r.RuleName,
                                r.StartDate,
                                r.Schedule,
                                r.ModifiedDate,
                                r.AddedDate
                            };

                if (!string.IsNullOrEmpty(enable)) query = query.Where(p => p.Enable == (enable == "1" ? true : false));
                if (!string.IsNullOrEmpty(ruleName)) query = query.Where(p => p.RuleName.Contains(ruleName));
                if (!string.IsNullOrEmpty(ruleType)) query = query.Where(p => p.RuleType == ruleType);

                var res = (from r in query.ToList()
                           select new
                           {
                               r.RuleType,
                               r.TypeDesc,
                               r.RuleRunType,
                               r.RunIndex,
                               r.SubTypeDesc,
                               r.Enable,
                               r.RuleID,
                               r.RuleName,
                               r.StartDate,
                               r.AddedDate,
                               r.ModifiedDate,
                               r.Schedule,
                               Cycle = JsonHelper.Deserialize<LoyaltySchedule>(r.Schedule).Type == "cycle" ? convertCycleDesc(JsonHelper.Deserialize<LoyaltySchedule>(r.Schedule).SubType) : ""
                           }).ToList();

                return new Result(true, "", new List<object> { res.ToDataTableSourceVsPage(myDp) });
            }
        }
        private static string convertCycleDesc(string cycle)
        {
            switch (cycle)
            {
                case "day":
                    return "每日";
                case "week":
                    return "每周";
                case "month":
                    return "每月";
                case "year":
                    return "每年";
                default:
                    return "";
            }
        }

        ////日程
        //private struct Schedule
        //{
        //    public string Type { get; set; }
        //    public string SubType { get; set; }
        //    public string Day { get; set; }
        //    public string Time { get; set; }
        //}
        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result DeleteRuleById(int ruleId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Loy_Rule.Where(p => p.RuleID == ruleId).FirstOrDefault();
                if (query != null)
                {
                    if (query.Enable == true) return new Result(false, "已启用的规则不可删除");
                    if (query.LastExcuteTime != null) return new Result(false, "已执行的规则不可删除");
                    db.TM_Loy_Rule.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }
        /// <summary>
        /// 激活规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result ActiveRuleById(int ruleId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Loy_Rule.Where(p => p.RuleID == ruleId).FirstOrDefault();
                if (query != null)
                {
                    query.Enable = true;

                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;

                    db.SaveChanges();
                    return new Result(true, "激活成功");
                }
                return new Result(false, "激活失败");
            }
        }
        /// <summary>
        /// 禁用规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result InActiveRuleById(int ruleId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Loy_Rule.Where(p => p.RuleID == ruleId).FirstOrDefault();
                if (query != null)
                {
                    query.Enable = false;
                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, "禁用成功");
                }
                return new Result(false, "禁用失败");
            }
        }
        #endregion

        #region 会员等级积分计算规则设置

        public static Result GetCouponTempletFieldAliaListByAliasKey(string keys)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_FieldAlias
                            where a.IsFilterByCouponTemplet == true
                            orderby a.FieldDesc
                            select new
                            {
                                a.FieldAlias,
                                a.FieldDesc,
                                a.FieldType,
                                a.AliasKey,
                                a.AliasID,
                                a.AliasType,
                                a.ControlType,
                                a.DictTableName,
                                a.DictTableType,
                                a.Reg,
                                a.DataLimitType,
                            };
                var querys = query.Where(i => i.AliasID == null);
                var queryTemp = query.Where(i => i.AliasID == null);
                if (!string.IsNullOrEmpty(keys.Trim()))
                {
                    var keyArray = keys.Split(',');
                    foreach (var key in keyArray)
                    {
                        queryTemp = query.Where(f => f.AliasKey.Trim().Equals(key.Trim()));
                        querys = querys.Union(queryTemp);
                    }
                    
                }
                return new Result(true, "", querys.ToList());
            }
        }

        /// <summary>
        /// 根据id获取单条规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result GetRuleById(string ruleId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var id = Convert.ToInt32(ruleId);
                var query = db.TM_Loy_Rule.Where(p => p.RuleID == id).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 获取左值列表
        /// </summary>
        /// <returns></returns>
        public static Result GetActionAliasList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_FieldAlias
                            where a.IsFilterByLoyActionLeft
                            select new
                            {
                                a.FieldAlias,
                                a.FieldDesc,
                                a.FieldType,
                                a.AliasKey
                            };
                return new Result(true, "", query.ToList());
            }
        }
        /// <summary>
        /// 获取数据群族列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Result GetDataGroupList(int groupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_Sys_DataGroupRelation
                            where a.DataGroupID == groupId
                            select new
                            {
                                a.SubDataGroupName,
                                a.SubDataGroupID
                            };
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 获取会员细分过滤条件所有左值
        /// </summary>
        /// <returns></returns>
        public static Result GetRuleLeftValuesAll()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_FieldAlias
                            where a.IsFilterByLoyRule
                            orderby a.FieldDesc
                            select new
                            {
                                a.FieldAlias,
                                a.FieldDesc,
                                a.FieldType,
                                a.AliasKey,
                                a.AliasID,
                                a.AliasType,
                                a.ControlType,
                                a.DictTableName,
                                a.DictTableType,
                                a.Reg
                            };
                return new Result(true, "", query.ToList());
            }
        }
        /// <summary>
        /// 获取关键字列表
        /// </summary>
        /// <returns></returns>
        public static Result GetTradeAliasKeyList(string type1, string type2)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var q1 = from b in db.TD_SYS_BizOption
                         where b.OptionType == "DBFieldType"
                         select new
                         {
                             b.OptionValue,
                             b.OptionText
                         };
                var query = from a in db.TD_SYS_FieldAlias
                            join b in q1 on a.FieldType equals b.OptionValue
                            where (new string[] { type1, type2 }).Contains(a.AliasType) && (new string[] { "3", "7", "8" }).Contains(b.OptionValue)
                            select new
                            {
                                a.FieldAlias,
                                a.FieldDesc,
                                a.FieldType,
                                a.AliasKey,
                                a.AliasID,
                                a.AliasType,
                                a.ControlType,
                                a.DictTableName,
                                a.DictTableType,
                                a.Reg,
                                b.OptionValue
                            };
                return new Result(true, "", query.ToList());
            }
        }
        /// <summary>
        /// 获取行为右值列表
        /// </summary>
        /// <returns></returns>
        public static Result GetTradeAliasKeyList1()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var q1 = from b in db.TD_SYS_BizOption
                         where b.OptionType == "DBFieldType"
                         select new
                         {
                             b.OptionValue,
                             b.OptionText
                         };
                var query = from a in db.TD_SYS_FieldAlias
                            join b in q1 on a.FieldType equals b.OptionValue
                            where a.IsFilterByLoyActionRight == true && (new string[] { "3", "7", "8" }).Contains(b.OptionValue)
                            select new
                            {
                                a.FieldAlias,
                                a.FieldDesc,
                                a.FieldType,
                                a.AliasKey,
                                a.AliasID,
                                a.AliasType,
                                a.ControlType,
                                a.DictTableName,
                                a.DictTableType,
                                a.Reg,
                                b.OptionValue
                            };
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 根据别名value值获取别名desc
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result GetAliasDescByValue(string value)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_FieldAlias
                            where a.FieldAlias == value
                            select new
                            {
                                a.FieldAlias,
                                a.FieldDesc,
                            };
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 保存规则
        /// </summary>
        /// <param name="ruleMaster"></param>
        /// <returns></returns>
        public static Result SaveRule(RuleModel ruleMaster, int authId, int dataGroupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                if (!string.IsNullOrEmpty(ruleMaster.RuleID))
                {//修改
                    var id = Convert.ToInt32(ruleMaster.RuleID);

                    if (db.TM_Loy_Rule.Where(p => p.RuleName == ruleMaster.RuleName && p.RuleID != id).Count() != 0) return new Result(false, "规则名称重复");

                    var query = db.TM_Loy_Rule.Where(p => p.RuleID == id).FirstOrDefault();
                    //if (query.Enable) return new Result(false, "已启用的规则不能编辑");
                    //query.AddedDate = DateTime.Now;
                    query.AddedUser = authId.ToString();
                    query.ModifiedDate = DateTime.Now;
                    query.ModifiedUser = authId.ToString();
                    query.Condition = ruleMaster.Condition ?? "";
                    query.ConditionResult = ruleMaster.ConditionResult;
                    query.StartDate = ruleMaster.StartDate;
                    query.EndDate = ruleMaster.EndDate;
                    query.RuleName = ruleMaster.RuleName;
                    query.RunIndex = ruleMaster.RunIndex;
                    query.RuleType = ruleMaster.RuleType;
                    query.RuleRunType = ruleMaster.ScheduleType == "realtime" ? "1" : "2";
                    query.Action = ruleMaster.Actions;
                    query.Schedule = "{Type:'" + ruleMaster.ScheduleType + "',SubType:'" + ruleMaster.ScheduleSubType + "',Date:'" + ruleMaster.ScheduleDay + "',Time:'" + ruleMaster.ScheduleTime + "',Remark:'" + ruleMaster.Remark + "'}";
                    //query.Enable = ruleMaster.Enable == "已激活"?true:false;
                    query.LastExcuteTime = ruleMaster.LastExecTime;
                    query.DataGroupID = dataGroupId;

                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, "修改成功", query.RuleID);
                }
                else
                {//新增 
                    if (db.TM_Loy_Rule.Where(p => p.RuleName == ruleMaster.RuleName).Count() != 0) return new Result(false, "规则名称重复");

                    var rule = new TM_Loy_Rule
                    {
                        AddedDate = DateTime.Now,
                        AddedUser = authId.ToString(),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = authId.ToString(),
                        Condition = ruleMaster.Condition ?? "",
                        ConditionResult = ruleMaster.ConditionResult,
                        StartDate = ruleMaster.StartDate,
                        EndDate = ruleMaster.EndDate,
                        RuleName = ruleMaster.RuleName,
                        RunIndex = ruleMaster.RunIndex,
                        RuleType = ruleMaster.RuleType,
                        RuleRunType = ruleMaster.ScheduleType == "realtime" ? "1" : "2",
                        Action = ruleMaster.Actions,
                        Schedule = "{Type:'" + ruleMaster.ScheduleType + "',SubType:'" + ruleMaster.ScheduleSubType + "',Date:'" + ruleMaster.ScheduleDay + "',Time:'" + ruleMaster.ScheduleTime + "',Remark:'" + ruleMaster.Remark + "'}",
                        Enable = false,
                        LastExcuteTime = ruleMaster.LastExecTime,
                        DataGroupID = dataGroupId,
                    };
                    db.TM_Loy_Rule.Add(rule);
                    db.SaveChanges();

                    var q1 = db.TM_Loy_Rule.Where(p => p.RuleName == rule.RuleName).FirstOrDefault();
                    return new Result(true, "添加成功", q1.RuleID);
                }
            }
        }
        #endregion
    }
}
