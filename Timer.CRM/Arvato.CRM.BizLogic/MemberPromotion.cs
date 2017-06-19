using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Web.Mvc;

namespace Arvato.CRM.BizLogic
{

    public static class MemberPromotion
    {
        #region 会员促销规则管理
        /// <summary>
        /// 获取会员促销规则管理（分页）
        /// </summary>
        /// <param name="Code">单号</param>
        /// <param name="ruleType">模板</param> 
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetRuleData(string Code, string ruleType, int? approveStatus, int? executeStatus, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);

            using (CRMEntities db = new CRMEntities())
            {
                var query = from r in db.TM_JPOS_PromotionBill
                            join o in db.TD_SYS_BizOption on r.PromotionType equals o.OptionValue
                            where o.OptionType == "MemberPromotion"
                            select (new { r, o });

                if (approveStatus != null) query = query.Where(p => p.r.ApproveStatus == approveStatus);
                if (executeStatus != null) query = query.Where(p => p.r.ExecuteStatus == executeStatus);
                if (!string.IsNullOrEmpty(Code)) query = query.Where(p => p.r.BillCode.Contains(Code));
                if (!string.IsNullOrEmpty(ruleType)) query = query.Where(p => p.r.PromotionType == ruleType);

                var userLst = db.TM_AUTH_User.Select(v => new { UserID = v.UserID, UserName = v.UserName }).ToList();
                var userDic = new Dictionary<string, string>();
                userLst.ForEach(v => userDic[v.UserID.ToString()] = v.UserName);
                var lst = query.ToList()
                     .Select(v => new
                     {
                         v.r.BillID,
                         v.r.BillCode,
                         ExecuteStatus = //((PromotionExecuteStatus)v.r.ExecuteStatus).ToString(),//执行状态
                         (v.r.ExecuteStatus != 1 && v.r.ApproveStatus == 1) ? (
                         DateTime.Today > v.r.EndDate ? PromotionExecuteStatus.已结束.ToString() :
                         (DateTime.Today >= v.r.StartDate ? PromotionExecuteStatus.执行中.ToString() : PromotionExecuteStatus.未开始.ToString())
                         ) : ((PromotionExecuteStatus)v.r.ExecuteStatus).ToString(),
                         TypeDesc = v.o.OptionText,//规则模板
                         ApproveStatus = ((PromotionApproveStatus)v.r.ApproveStatus).ToString(),//审核状态
                         Days = v.r.StartDate.ToString("yyyy-MM-dd") + " ~ " +
                             (v.r.EndDate == null ? "不限制" : v.r.EndDate.Value.ToString("yyyy-MM-dd")),
                         v.r.Remark,
                         CreateInfo = userDic[v.r.AddedUser] + " ~ " + v.r.AddedDate.ToString("yyyy-MM-dd"),
                         UpdateInfo = userDic[v.r.ModifiedUser] + " ~ " + (v.r.ModifiedDate == null ? "" : v.r.ModifiedDate.Value.ToString("yyyy-MM-dd"))
                     }).OrderByDescending(v => v.BillCode).ToList();

                return new Result(true, "", new List<object> { lst.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result GetProductData(string Code, string Name, string Sort, string Brand, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);

            using (CRMEntities db = new CRMEntities())
            {
                var query = from r in db.TD_Sys_Product
                            where r.GoodsName.Contains(Name) && r.GoodsCode.Contains(Code)
                            && r.GoodsSort.Contains(Sort) && r.GoodsBrand.Contains(Brand)
                            select r;


                var lst = query.ToList()
                     .Select(v => new
                     {
                         v.GoodsCode,
                         v.GoodsName,
                         v.GoodsSort,
                         v.GoodsBrand,
                         v.GoodsRTLPRC,

                     }).ToList();

                return new Result(true, "", new List<object> { lst.ToDataTableSourceVsPage(myDp) });
            }
        }


        public static Result GetStoreData(string Code, string Name, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);

            using (CRMEntities db = new CRMEntities())
            {
                var query = from r in db.V_M_TM_SYS_BaseData_store
                            where r.StoreName.Contains(Name) && r.StoreCode.Contains(Code)
                            select r;


                var lst = query.ToList()
                     .Select(v => new
                     {
                         v.BaseDataID,
                         v.StoreCode,
                         v.StoreName,
                         v.ProvinceStore,
                         v.CityStore

                     }).ToList();

                return new Result(true, "", new List<object> { lst.ToDataTableSourceVsPage(myDp) });
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

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result DeleteRuleById(string ruleId, int authId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_PromotionBill.Where(p => p.BillID == ruleId).FirstOrDefault();
                if (query != null)
                {
                    if (query.ApproveStatus > 0) return new Result(false, "已经审核过的规则不能删除");
                    db.TM_JPOS_PromotionBill.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }
        /// <summary>
        /// 审核规则 
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result ApproveRuleById(string ruleId, int active, int authId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_PromotionBill.Where(p => p.BillID == ruleId).FirstOrDefault();
                if (query != null)
                {
                    query.ApproveStatus = active;
                    query.ApproveDate = DateTime.Now;
                    query.ApproveUser = authId.ToString();

                    query.ModifiedDate = DateTime.Now;
                    query.ModifiedUser = authId.ToString();
                    if (active == 2 && query.ExecuteStatus != 3)
                        query.ExecuteStatus = 1;//已结束 

                    db.SaveChanges();
                    var msg = "审核成功";
                    if (active == 2)
                        msg = "作废成功";
                    return new Result(true, msg, ((PromotionApproveStatus)query.ApproveStatus).ToString());
                }
                return new Result(false, "操作失败：数据不存在");
            }
        }
        /// <summary>
        /// 休眠或唤醒 规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result ActiveRuleById(string ruleId, bool IsWakeUp, int authId)
        {
            var msg = "休眠";
            if (IsWakeUp)
                msg = "唤醒";
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_PromotionBill.Where(p => p.BillID == ruleId).FirstOrDefault();
                if (query != null)
                {
                    if (!IsWakeUp)
                        query.ExecuteStatus = PromotionExecuteStatus.休眠中.GetHashCode();
                    else if (query.StartDate >= DateTime.Today)
                        query.ExecuteStatus = PromotionExecuteStatus.未开始.GetHashCode();
                    else if (query.EndDate >= DateTime.Today)
                        query.ExecuteStatus = PromotionExecuteStatus.执行中.GetHashCode();
                    else
                        query.ExecuteStatus = PromotionExecuteStatus.已结束.GetHashCode();

                    query.ModifiedDate = DateTime.Now;
                    query.ModifiedUser = authId.ToString();
                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();

                    return new Result(true, msg + "成功", ((PromotionExecuteStatus)query.ExecuteStatus).ToString());
                }
                return new Result(false, msg + "失败");
            }
        }

        /// <summary>
        /// 会员等级
        /// </summary>
        /// <returns></returns>
        public static Result GetMemberLevel()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_customerlevel
                             select new
                             {
                                 Text = a.CustomerLevelNameBase,
                                 Value = a.CustomerLevelBase
                             }).Distinct().ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取商品，礼品
        /// </summary>
        /// <returns></returns>
        public static Result GetAllGoods()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_goods
                             select new
                             {
                                 Text = a.GoodsName,
                                 Value = a.GoodsCode
                             }).Distinct().ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取积分类型
        /// </summary>
        /// <returns></returns>
        public static Result GetPointType()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_BizOption
                             where a.OptionType == "PointType" && a.Enable
                             orderby a.Sort
                             select new
                             {
                                 Text = a.OptionText,
                                 Value = a.OptionValue
                             }).Distinct().ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 会员卡类型
        /// </summary>
        /// <returns></returns>
        public static Result GetCardType()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_cardType
                             where a.CardTypeStatus == "1" && a.CardTypeCodeBase == "1"
                             select new
                             {
                                 Text = a.CardTypeNameBase,
                                 Value = "001"
                             }).ToList();
                return new Result(true, "", query);
            }
        }


        #endregion

        #region 会员促销规则设置


        /// <summary>
        /// 根据id获取单条规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result GetPromotionById(string ruleId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_PromotionBill//.Join(db.TD_SYS_BizOption,u=>u.PromotionType,v=>v.OptionID,(u))
                    .Where(p => p.BillID == ruleId).FirstOrDefault();
                if (query.ApproveStatus == 1 && query.ExecuteStatus != 1)
                {
                    if (DateTime.Today > query.EndDate)
                        query.ExecuteStatus = 3;//结束 
                    if (DateTime.Today >= query.StartDate)
                        query.ExecuteStatus = 2;//执行中
                }
                db.SaveChanges();
                return new Result(true, "", query);
            }
        }

        public static Result GetPromotionItemByBillID(string billid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_PromotionItem.Where(p => p.BillID == billid).FirstOrDefault();


                return new Result(true, "", query.LimitQuantity);
            }
        }

        /// <summary>
        /// 保存规则
        /// </summary>
        /// <param name="ruleMaster"></param>
        /// <returns></returns>
        public static Result SavePromotion(PromotionRule ruleMaster, int authId)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var query = db.TM_JPOS_PromotionBill.Where(p => p.BillID == ruleMaster.BillID).FirstOrDefault();
                    if (string.IsNullOrEmpty(ruleMaster.BillID))
                    {
                        //新增  
                        var code = "";
                        var codePre = DateTime.Today.ToString("yyMMdd");
                        var codeMax = db.TM_JPOS_PromotionBill.Where(v => v.BillCode.StartsWith(codePre)).Max(v => v.BillCode);
                        var codeIdx = 1;
                        if (codeMax != null)
                            codeIdx = int.Parse(codeMax.Substring(6, 5)) + 1;
                        code = codePre + codeIdx.ToString().PadLeft(5, '0');
                        var rule = new TM_JPOS_PromotionBill
                        {
                            BillID = Guid.NewGuid().ToString().Replace("-", ""),
                            AddedDate = DateTime.Now,
                            AddedUser = authId.ToString(),
                            BillCode = code,
                        };
                        query = db.TM_JPOS_PromotionBill.Add(rule);
                    }
                    query.PromotionType = ruleMaster.PromotionType;
                    query.ModifiedDate = DateTime.Now;
                    query.ModifiedUser = authId.ToString();
                    query.Condition = ruleMaster.Condition ?? "";
                    query.StartDate = ruleMaster.StartDate;
                    query.EndDate = ruleMaster.EndDate;
                    query.RunIndex = ruleMaster.RunIndex;
                    query.Remark = ruleMaster.Remark;
                    query.Action = ruleMaster.Action;
                    query.ApproveStatus = ruleMaster.ApproveStatus.GetHashCode();
                    query.AllowStores = ruleMaster.AllowStores;
                    if (ruleMaster.AllowStores == "undefined")
                        query.AllowStores = null;
                    query.AllowGoods = ruleMaster.AllowGoods;//前置条件

                    SavePlan(db, query, authId);
                    SaveToXML(db, query, ruleMaster, authId);
                    db.SaveChanges();
                    var q1 = db.TM_JPOS_PromotionBill.Where(p => p.BillCode == query.BillCode).FirstOrDefault();


                    return new Result(true, "保存成功", q1);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        public static Result CopyPromotion(string billid, DateTime start, DateTime end, int authId)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var query = db.TM_JPOS_PromotionBill.Where(p => p.BillID == billid).FirstOrDefault();

                    //新增  
                    var code = "";
                    var codePre = DateTime.Today.ToString("yyMMdd");
                    var codeMax = db.TM_JPOS_PromotionBill.Where(v => v.BillCode.StartsWith(codePre)).Max(v => v.BillCode);
                    var codeIdx = 1;
                    if (codeMax != null)
                        codeIdx = int.Parse(codeMax.Substring(6, 5)) + 1;
                    code = codePre + codeIdx.ToString().PadLeft(5, '0');
                    var rule = new TM_JPOS_PromotionBill
                    {
                        BillID = Guid.NewGuid().ToString().Replace("-", ""),
                        AddedDate = DateTime.Now,
                        AddedUser = authId.ToString(),
                        BillCode = code,
                    };
                    db.TM_JPOS_PromotionBill.Add(rule);
                    rule.PromotionType = query.PromotionType;
                    rule.ModifiedDate = DateTime.Now;
                    rule.ModifiedUser = authId.ToString();
                    rule.Condition = query.Condition;
                    rule.StartDate = start;
                    rule.EndDate = end;
                    rule.RunIndex = query.RunIndex;
                    rule.Remark = query.Remark;
                    rule.Action = query.Action;
                    rule.AllowStores = query.AllowStores;
                    rule.AllowGoods = query.AllowGoods;//前置条件

                    var plan = db.TM_JPOS_PromotionItem.Where(v => v.BillID == billid).FirstOrDefault();
                    int limit = 0;
                    int.TryParse(plan.LimitQuantity, out limit);
                    SavePlan(db, rule, authId);
                    SaveToXML(db, rule, new PromotionRule { LimitQuantity = limit }, authId);
                    db.SaveChanges();
                    var q1 = db.TM_JPOS_PromotionBill.Where(p => p.BillCode == rule.BillCode).FirstOrDefault();


                    return new Result(true, "保存成功", q1);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        private static void SavePlan(CRMEntities db, TM_JPOS_PromotionBill bill, int authId)
        {
            var plan = db.TM_JPOS_PromotionBillPlan.Where(v => v.BillID == bill.BillID).FirstOrDefault();
            if (plan == null)
            {
                plan = new TM_JPOS_PromotionBillPlan
                {
                    BillID = bill.BillID,
                    BillPlanID = Guid.NewGuid().ToString().Replace("-", ""),
                    AddedDate = DateTime.Now,
                    AddedUser = authId.ToString(),
                };
                db.TM_JPOS_PromotionBillPlan.Add(plan);
            }
            plan.StartDate = bill.StartDate;
            plan.EndDate = bill.EndDate;
            plan.ModifiedDate = DateTime.Now;
            plan.ModifiedUser = authId.ToString();
        }

        private static dynamic GetValueFromDic(Dictionary<string, dynamic> dic, string key, dynamic defaultvalue)
        {
            dynamic value = null;
            if (dic.TryGetValue(key, out value))
                return value;
            return defaultvalue;
        }
        /// <summary>
        /// 保存到XML表
        /// </summary>
        /// <param name="db"></param>
        /// <param name="bill"></param>
        /// <param name="authId"></param>
        private static void SaveToXML(CRMEntities db, TM_JPOS_PromotionBill bill, PromotionRule ruleMaster, int authId)
        {
            try
            {

                var item = db.TM_JPOS_PromotionItem.Where(v => v.BillID == bill.BillID).FirstOrDefault();
                if (item == null)
                {
                    item = new TM_JPOS_PromotionItem
                    {
                        PromotionID = Guid.NewGuid().ToString().Replace("-", ""),
                        BillID = bill.BillID,
                        AddedDate = DateTime.Now,
                        AddedUser = authId.ToString(),
                        ConflictGroup = "Basket",
                        BillType = bill.PromotionType,
                        LimitMode = "",
                        LimitType = "none",
                    };
                    db.TM_JPOS_PromotionItem.Add(item);
                }
                item.LimitQuantity = null;
                item.ModifiedDate = DateTime.Now;
                item.ModifiedUser = authId.ToString();
                item.ProductExpression = "overall";
                item.ProductXML = "<?xml version=\"1.0\" encoding=\"GBK\"?><element class=\"com.hd123.h5.pom.document.condition.product.BasketCondition\" overall=\"true\"/>";
                item.QRC = 0;
                item.ExeCseqGroup = "001";

                var conditions = JsonHelper.Deserialize<List<Dictionary<string, Dictionary<string, dynamic>>>>(bill.Condition);

                if (conditions.Count == 0)
                    return;

                var condition = conditions.FirstOrDefault()["Condition"];// JsonHelper.Deserialize< Dictionary<string, dynamic>>(bill.Condition);
                var action = conditions.FirstOrDefault()["Action"];// JsonHelper.Deserialize<Dictionary<string, dynamic>>(bill.Action);
                //礼品促销
                if ("7,9".Contains(bill.PromotionType))
                {
                    item.LimitType = "buytimes";
                    item.LimitMode = "buyerid";
                    //如果是礼品促销，数量限制  
                    item.LimitQuantity = ruleMaster.LimitQuantity.ToString();
                }
                //会员促销
                if ("6".Contains(bill.PromotionType))
                {
                    item.ConflictGroup = "SingleProduct";
                }
                var header = "<?xml version='1.0' encoding='GBK'?><pom class='com.hd123.h5.pom.document.PomDocument'><element class='com.hd123.h5.pom.document.condition.general.DateRangeCondition' finish='{end}' start='{start}'>"
                          .Replace("{end}", bill.EndDate.Value.ToString("yyyy-MM-dd HH:mm")) //""
                          .Replace("{start}", bill.StartDate.ToString("yyyy-MM-dd HH:mm")) //""
                    ;
                var productxml = "<element class='com.hd123.h5.pom.document.condition.product.BasketCondition' overall='true'>";
                Dictionary<string, dynamic> allowGoods = null;
                if (!string.IsNullOrEmpty(bill.AllowGoods))
                    allowGoods = JsonHelper.Deserialize<Dictionary<string, dynamic>>(bill.AllowGoods);
                if (allowGoods != null && allowGoods["OverAll"] == false)
                {
                    #region 选择产品
                    string p = allowGoods["Goods"][0];
                    var product = db.V_M_TM_SYS_BaseData_goods.Where(v => v.GoodsCode == p).FirstOrDefault();

                    item.ProductExpression = string.Format("p={0}*{1}", product.GoodsCode, 1);

                    productxml = string.Format(@"<element class='com.hd123.h5.pom.document.condition.product.BasketCondition' overall='false'><product mode='range'><condition field='product' operator='equals'><operand><entity code='{0}' measureUnit='{1}' name='{2}' qpc='1' qpcStr='{3}' type='product' uuid='{4}'/></operand></condition></product>", product.GoodsCode, "支", product.GoodsName, product.GoodsRTLPRC, product.GoodsCode);
                    item.ProductXML = "<?xml version='1.0' encoding='GBK'?>" + productxml + "</element>";
                    item.ProductXML = item.ProductXML.Replace("'", "\"");
                    #endregion
                }
                switch (bill.PromotionType)
                {
                    case "1"://交易开卡
                        #region 交易 开卡
                        {
                            string cardType = action["CardType"];
                            var cardtypeName = (from a in db.V_M_TM_SYS_BaseData_cardType
                                                where a.CardTypeStatus == "1" && a.CardTypeCodeBase == cardType
                                                select a.CardTypeNameBase
                                                ).FirstOrDefault();
                            var hasPoint = GetValueFromDic(action, "HasPoint", true);
                            var HasMemberPrice = GetValueFromDic(action, "HasMemberPrice", true);
                            var PointMulti = GetValueFromDic(action, "PointMulti", 1);
                            var PointNumber = GetValueFromDic(action, "PointNumber", 0);
                            var PointType = GetValueFromDic(action, "PointType", "001");
                            string body = @"<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='range' valueType='amount'><case value='{amount}'><element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element calculateScore='{callscore}' cardTypeCode='{cardtypecode}' cardTypeName='{cardtypename}' class='com.hd123.h5.pom.document.execution.member.salecard.TradeSaleCardExecution' function='E#Member#TradeSaleCard' memberPrice='{memberprice}' scoreMultiple='{scoremulti}'><score score='{score}' scoreTypeCode='{scoretypecode}'/></element></element></element></case></element></element></element></pom>".
                            Replace("{amount}", condition["PayCash"].ToString()).//"50"
                            Replace("{callscore}", hasPoint.ToString()).//"true" 
                            Replace("{cardtypecode}", cardType).//"001"
                            Replace("{cardtypename}", cardtypeName).//"婷美小屋会员卡"
                            Replace("{memberprice}", HasMemberPrice.ToString()).//true
                            Replace("{scoremulti}", PointMulti.ToString()).//1
                            Replace("{score}", PointNumber.ToString()).//0.0
                            Replace("{scoretypecode}", PointType.ToString())//001
                            ;

                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;
                    case "2"://现金开卡 
                        #region  现金开卡
                        {
                            item.ProductExpression = "";
                            string cardType = action["CardType"];
                            var cardtypeName = (from a in db.V_M_TM_SYS_BaseData_cardType
                                                where a.CardTypeStatus == "1" && a.CardTypeCodeBase == cardType
                                                select a.CardTypeNameBase
                                                ).FirstOrDefault();
                            string body = @"<element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.member.salecard.CustomerSaleCardExecution' function='E#Member#CustomerSaleCard'><item cardTypeCode='{cardtypecode}' cardTypeName='{cardtypename}' fee='{fee}'><score score='{score}' scoreTypeCode='{scoretypecode}'/></item></element></element></element></element></element></pom>"
                            .Replace("{cardtypecode}", cardType).//"001"
                            Replace("{cardtypename}", cardtypeName).//"婷美小屋会员卡"
                            Replace("{fee}", action["CardCost"].ToString()).// 20.0 
                            Replace("{score}", action["PointNumber"].ToString()).//0.0
                            Replace("{scoretypecode}", action["PointType"].ToString())//001
                            ;
                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;
                    //case "3"://补发卡 
                    //    break;
                    case "4"://赠送积分 
                        #region  赠送积分
                        {
                            string body = @"<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='equal' valueType='amount'><case value='{amount}'><element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.member.score.ScoreExecution' extra='false'><score score='{score}' scoreTypeCode='{scoretypecode}'/></element></element></element></case></element></element></element></pom>".
                            Replace("{amount}", condition["PayCash"].ToString()).//"50" 
                            Replace("{score}", action["PointNumber"].ToString()).//0.0
                            Replace("{scoretypecode}", action["PointType"].ToString())//001
                            ;
                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;
                    case "5"://积分加速
                        #region 积分加速
                        {
                            var body = "<element class='com.hd123.h5.pom.document.condition.member.MemberCondition'>";

                            foreach (var detail in conditions)
                            {
                                var detailCondtion = detail["Condition"];
                                var detailAction = detail["Action"];
                                body += "<case><type code='会员'/>";
                                //检查是否包含生日 
                                if (detailCondtion.ContainsKey("MemberBirthday"))
                                {
                                    string birth = detailCondtion["MemberBirthday"];
                                    switch (birth)
                                    {
                                        case "birthday":
                                            body += "<condition attribute='birthday' operand='true' operator='equals'/>";
                                            break;
                                        case "birthmonth":
                                            body += "<condition attribute='birthmonth' operand='true' operator='equals'/>";
                                            break;
                                        case "birthmonthNotDay":
                                            body += "<condition attribute='birthday' operand='false' operator='equals'/><condition attribute='birthmonth' operand='true' operator='equals'/>";
                                            break;
                                        case "birthweek":
                                            body += "<condition attribute='birthweek' operand='true' operator='equals'/>";
                                            break;
                                        case "birthweekNotDay":
                                            body += "<condition attribute='birthday' operand='false' operator='equals'/><condition attribute='birthweek' operand='true' operator='equals'/>";
                                            break;
                                    }
                                }
                                //检查是否包含等级
                                if (detailCondtion.ContainsKey("MemberLevel"))
                                {
                                    foreach (string level in detailCondtion["MemberLevel"])
                                    {
                                        body += string.Format("<condition attribute='memberGrade' operand='{0}' operator='equals'/>", level);
                                    }
                                }
                                body += string.Format(@"<element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.member.score.ScoreAccelerateExecution' function='E#Member#ScoreAcclerate' scoreMultiple='{0}'/></element></element></case>", detailAction["PointTimes"]);
                            }


                            body += "</element></element></element></pom>";
                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;

                    case "6"://会员促销
                        #region 会员促销
                        {

                            var body = "<element class='com.hd123.h5.pom.document.condition.member.MemberCondition'><case><type code='会员'/>";
                            //检查是否包含生日 
                            if (condition.ContainsKey("MemberBirthday"))
                            {
                                string birth = condition["MemberBirthday"];
                                switch (birth)
                                {
                                    case "birthday":
                                        body += "<condition attribute='birthday' operand='true' operator='equals'/>";
                                        break;
                                    case "birthmonth":
                                        body += "<condition attribute='birthmonth' operand='true' operator='equals'/>";
                                        break;
                                    case "birthmonthNotDay":
                                        body += "<condition attribute='birthday' operand='false' operator='equals'/><condition attribute='birthmonth' operand='true' operator='equals'/>";
                                        break;
                                    case "birthweek":
                                        body += "<condition attribute='birthweek' operand='true' operator='equals'/>";
                                        break;
                                    case "birthweekNotDay":
                                        body += "<condition attribute='birthday' operand='false' operator='equals'/><condition attribute='birthweek' operand='true' operator='equals'/>";
                                        break;
                                }
                            }
                            //检查是否包含等级
                            if (condition.ContainsKey("MemberLevel"))
                            {
                                foreach (string level in condition["MemberLevel"])
                                {
                                    body += string.Format("<condition attribute='memberGrade' operand='{0}' operator='equals'/>", level);

                                }
                            }
                            if (condition.ContainsKey("PayCash"))
                            {
                                body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='range' valueType='amount'><case value='{0}'>",
                                    condition["PayCash"]);
                            }
                            if (condition.ContainsKey("PayCash2"))
                            {
                                body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='equal' valueType='amount'><case value='{0}'>",
                                    condition["PayCash2"]);
                            }
                            if (condition.ContainsKey("PayQuantity"))
                            {
                                body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='range' valueType='quantity'><case value='{0}'>",
                                    condition["PayQuantity"]);
                            }
                            if (condition.ContainsKey("PayQuantity2"))
                            {
                                body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='equal' valueType='quantity'><case value='{0}'>",
                                    condition["PayQuantity2"]);
                            }

                            var discountType = "price";
                            if (action.ContainsKey("DiscountType"))
                                discountType = action["DiscountType"];
                            body += @"<element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.GeneralExecution' form='{0}' value='{1}'/></element></element>"
                            .Replace("{0}", discountType).Replace("{1}", action["DiscountValue"].ToString());//


                            if (condition.ContainsKey("PayCash") || condition.ContainsKey("PayCash2") || condition.ContainsKey("PayQuantity") || condition.ContainsKey("PayQuantity2"))
                                body += "</case></element>";

                            body += "</case></element></element></element></pom>";
                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;

                    case "7"://礼品促销
                        #region 礼品促销
                        {
                            var body = "<element class='com.hd123.h5.pom.document.condition.member.MemberCondition'>";

                            foreach (var detail in conditions)
                            {
                                var detailCondtion = detail["Condition"];
                                var detailAction = detail["Action"];
                                body += "<case><type code='会员'/>";

                                //检查是否包含生日 
                                if (detailCondtion.ContainsKey("MemberBirthday"))
                                {
                                    string birth = detailCondtion["MemberBirthday"];
                                    switch (birth)
                                    {
                                        case "birthday":
                                            body += "<condition attribute='birthday' operand='true' operator='equals'/>";
                                            break;
                                        case "birthmonth":
                                            body += "<condition attribute='birthmonth' operand='true' operator='equals'/>";
                                            break;
                                        case "birthmonthNotDay":
                                            body += "<condition attribute='birthday' operand='false' operator='equals'/><condition attribute='birthmonth' operand='true' operator='equals'/>";
                                            break;
                                        case "birthweek":
                                            body += "<condition attribute='birthweek' operand='true' operator='equals'/>";
                                            break;
                                        case "birthweekNotDay":
                                            body += "<condition attribute='birthday' operand='false' operator='equals'/><condition attribute='birthweek' operand='true' operator='equals'/>";
                                            break;
                                    }
                                }
                                //检查是否包含等级
                                if (detailCondtion.ContainsKey("MemberLevel"))
                                {
                                    foreach (string level in detailCondtion["MemberLevel"])
                                    {
                                        body += string.Format("<condition attribute='memberGrade' operand='{0}' operator='equals'/>", level);

                                    }
                                }

                                if (detailCondtion.ContainsKey("PayCash"))
                                {
                                    body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='range' valueType='amount'><case value='{0}'>",
                                        detailCondtion["PayCash"]);
                                }
                                if (detailCondtion.ContainsKey("PayCash2"))
                                {
                                    body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='equal' valueType='amount'><case value='{0}'>",
                                        detailCondtion["PayCash2"]);
                                }
                                if (detailCondtion.ContainsKey("PayQuantity"))
                                {
                                    body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='range' valueType='quantity'><case value='{0}'>",
                                        detailCondtion["PayQuantity"]);
                                }
                                if (detailCondtion.ContainsKey("PayQuantity2"))
                                {
                                    body += string.Format("<element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='equal' valueType='quantity'><case value='{0}'>",
                                        detailCondtion["PayQuantity2"]);
                                }

                                body += string.Format(@"<element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.GiftExecution' function='E#Gift#Single' needToInput='true'><group quantity='{0}'>", detailAction["GiftCount"]);

                                foreach (string p in detailAction["GiftCode"].Products)
                                {
                                    var product = db.V_M_TM_SYS_BaseData_goods.Where(v => v.GoodsCode == p).FirstOrDefault();

                                    var pxml = string.Format(@"<entity code='{0}' manufactory='' measureUnit='{1}' name='{2}' qpc='1' qpcStr='{3}' specification='100ml' type='product' uuid='{4}'/>"
    , product.GoodsCode, "支", product.GoodsName, product.GoodsRTLPRC, product.GoodsCode);

                                    body += pxml;

                                }
                                body += @"</group></element></element></element>";
                                if (detailCondtion.ContainsKey("PayCash") || detailCondtion.ContainsKey("PayCash2") || detailCondtion.ContainsKey("PayQuantity") || detailCondtion.ContainsKey("PayQuantity2"))
                                    body += "</case></element>";
                                body += "</case>";
                            }

                            body += "</element></element></element></pom>";

                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;
                    case "8"://会员加速 等级
                        #region 会员加速 等级
                        {
                            var body = "<element class='com.hd123.h5.pom.document.condition.member.MemberCondition'>";

                            foreach (var actionItem in condition["MemberPointMulti"])
                            {
                                string levelCondition = @"<case><type code='会员'/><condition attribute='memberGrade' operand='{level}' operator='equals'/><element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.member.score.ScoreAccelerateExecution' function='E#Member#ScoreAcclerate'
                                scoreMultiple='{Multi}'/></element></element></case>"
                                    .Replace("{level}", actionItem.Level.ToString()).Replace("{Multi}", actionItem.Multi.ToString())
                                    ;
                                body += levelCondition;

                            }
                            body += "</element></element></element></pom>";

                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;
                    case "9"://会员等级 礼品
                        #region 会员等级礼品
                        {
                            var body = "<element class='com.hd123.h5.pom.document.condition.member.MemberCondition'>";

                            foreach (var actionItem in condition["MemberGiftMulti"])
                            {
                                string levelCondition = @"<case><type code='会员'/><condition attribute='memberGrade' operand='{level}' operator='equals'/><element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.GiftExecution' function='E#Gift#Or' needToInput='true'><group quantity='1.0'>"
.Replace("{level}", actionItem.Level.ToString());
                                foreach (string p in actionItem["Gifts"])
                                {
                                    var product = db.V_M_TM_SYS_BaseData_goods.Where(v => v.GoodsCode == p).FirstOrDefault();

                                    var pxml = string.Format(@"<entity code='{0}' manufactory='' measureUnit='{1}' name='{2}' qpc='1' qpcStr='{3}' specification='100ml' type='product' uuid='{4}'/>"
, product.GoodsCode, "支", product.GoodsName, product.GoodsRTLPRC, product.GoodsCode);

                                    levelCondition += pxml;

                                }
                                levelCondition += "</group></element></element></element></case>";
                                body += levelCondition;

                            }
                            body += "</element></element></element></pom>";

                            item.DocumentXML = (header + productxml + body).Replace("'", "\"");
                        }
                        #endregion
                        break;
                    case "10":
                        #region 积分抵现
                        {
                            var body = "<element class='com.hd123.h5.pom.document.condition.product.SingleProductCondition'><product mode='single'><condition field='scoreType' operator='equals'><operand><entity code='0001' name='娇兰佳人积分' type='scoreType' uuid='0001'/></operand></condition></product><element class='com.hd123.h5.pom.document.condition.member.MemberCondition'><case><type code='0010'/><element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='range' valueType='amount'><case value='0.0'><element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='ratio' valueType='amount'><case value='10.0'><element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='" + action["PointNumber"] + "'><element class='com.hd123.h5.pom.document.execution.GeneralExecution' form='amount' value='" + action["DeductAmount"] + "'/></element></element></case></element></case></element></case></element></element></element></pom>";

                            item.DocumentXML = (header + body).Replace("'", "\"");
                        }

                        #endregion
                        break;
                    case "11":
                        #region 积分加速抵现
                        {
                            var body = "<element class='com.hd123.h5.pom.document.condition.product.SingleProductCondition'><product mode='single'><condition field='scoreType' operator='equals'><operand><entity code='0001' name='娇兰佳人积分' type='scoreType' uuid='0001'/></operand></condition></product><element class='com.hd123.h5.pom.document.condition.member.MemberCondition'><case><type code='0001'/><element class='com.hd123.h5.pom.document.condition.step.StepCondition' operator='equal' valueType='amount'><case value='" + action["PointNumber"] + "'><element class='com.hd123.h5.pom.document.PomExecutionSet' inputMode='auto'><element class='com.hd123.h5.pom.document.PomExecutionOption' evaluation='70'><element class='com.hd123.h5.pom.document.execution.GeneralExecution' form='amount' value='" + action["DeductAmount"] + "'/></element></element></case></element></case></element></element></element></pom>";

                            item.DocumentXML = (header + body).Replace("'", "\"");
                        }

                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        #endregion

        #region 时间：2017-03-21  姓名：潘荣胜 （卡补发规则）

        /// <summary>
        /// 获取分公司
        /// </summary>
        /// <returns></returns>
        public static Result GetCompanyCodeList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_company
                            select new
                            {
                                a.BaseDataID,
                                a.DataGroupID,
                                a.BaseDataType,
                                a.CompanyCode,
                                a.CompanyName,
                                a.CompanySort,
                                a.CompanyProvinceCode,
                                a.CompanyProvinceName,
                                a.CompanyEnable
                            };

                return new Result(true, "", query.ToList());


            }
        }

        /// <summary>
        /// 获取补卡类型
        /// </summary>
        /// <returns></returns>
        public static Result GetCardTypeList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_cardType
                            select new
                            {
                                a.BaseDataID,
                                a.DataGroupID,
                                a.BaseDataType,
                                a.CardTypeNameBase,
                                a.CardTypeCodeBase,
                                a.CardTypeStatus
                            };

                return new Result(true, "", query.ToList());


            }
        }

        /// <summary>
        /// 加载补卡信息
        /// </summary>
        /// <param name="dataParamersJson"></param>
        /// <param name="CompanyCode">分公司</param>
        /// <returns></returns>
        public static Result GetSupplementaryCard(string dataParamersJson, string CompanyCode)
        {
            DatatablesParameter dataParameters = JsonHelper.Deserialize<DatatablesParameter>(dataParamersJson);
            using (CRMEntities db = new CRMEntities())
            {
                var userLst = db.TM_AUTH_User.Select(v => new { UserID = v.UserID, UserName = v.UserName }).ToList();
                var userDic = new Dictionary<string, string>();
                userLst.ForEach(v => userDic[v.UserID.ToString()] = v.UserName);
                var query = db.TM_JPOS_CardCost.Where(i => 1 == 1);
                if (CompanyCode != "0")
                {
                    query = query.Where(m => m.CompanyCode == CompanyCode);
                }
                var res = query.ToList().Select(i => new
                {
                    i.ID,
                    i.CardType,
                    i.CardAmount,
                    i.CardCost,
                    i.CompanyCode,
                    i.IsEnable,
                    i.AddedDate,
                    i.ModifiedDate,
                    AddedUser = userDic[i.AddedUser],
                    ModifiedUser = userDic[i.ModifiedUser],
                    CardTypeName = db.V_M_TM_SYS_BaseData_cardType.FirstOrDefault(t => t.CardTypeCodeBase == i.CardType) != null ? db.V_M_TM_SYS_BaseData_cardType.FirstOrDefault(t => t.CardTypeCodeBase == i.CardType).CardTypeNameBase : "",
                    CompanyCodeName = db.V_M_TM_SYS_BaseData_company.FirstOrDefault(t => t.CompanyCode == i.CompanyCode) != null ? db.V_M_TM_SYS_BaseData_company.FirstOrDefault(t => t.CompanyCode == i.CompanyCode).CompanyName : ""
                });
                return new Result(true, "", new List<object> { res.ToList().ToDataTableSourceVsPage(dataParameters) });
            }
        }

        /// <summary>
        /// 卡补发规则
        /// </summary>
        /// <param name="CardAmount">购买金额</param>
        /// <param name="CardCost">补发费用</param>
        /// <param name="userID">当前用户ID</param>
        /// <returns></returns>
        public static Result AddSupplementaryCard(string CardAmount, string CardCost, string CompanyCode, string CardType, int userID, string CardCostId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                if (!string.IsNullOrEmpty(CardCostId))
                {
                    var query = db.TM_JPOS_CardCost.Where(p => p.ID == CardCostId).FirstOrDefault();

                    if (query != null)
                    {
                        var q = db.TM_JPOS_CardCost.Where(p => p.CompanyCode == CompanyCode && p.ID != CardCostId).FirstOrDefault();
                        if (q != null) return new Result(false, "一个分公司只能有一张补发卡！");
                        if (CardType == "0")
                        {
                            CardType = "1";
                        }
                        query.CardType = CardType;
                        query.CardAmount = Convert.ToDecimal(CardAmount);
                        query.CardCost = Convert.ToDecimal(CardCost);
                        query.CompanyCode = CompanyCode;
                        query.ModifiedDate = DateTime.Now;
                        query.ModifiedUser = userID.ToString();
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "修改成功");
                    }
                    return new Result(true, "修改失败");
                }
                else
                {
                    var q = db.TM_JPOS_CardCost.Where(p => p.CompanyCode == CompanyCode).FirstOrDefault();
                    if (q != null) return new Result(false, "一个分公司只能有一张补发卡！");
                    if (CardType == "0")
                    {
                        CardType = "1";
                    }
                    string guid = Guid.NewGuid().ToString("N");
                    var rule = new TM_JPOS_CardCost
                    {
                        ID = guid,
                        CardType = CardType,
                        CardAmount = Convert.ToDecimal(CardAmount),
                        CardCost = Convert.ToDecimal(CardCost),
                        CompanyCode = CompanyCode,
                        IsEnable = true,
                        AddedDate = DateTime.Now,
                        AddedUser = userID.ToString(),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = userID.ToString(),
                    };
                    db.TM_JPOS_CardCost.Add(rule);
                    db.SaveChanges();

                    //var q1 = db.TM_JPOS_CardCost.Where(p => p.ID == guid).FirstOrDefault();
                    //return new Result(true, "添加成功",q1.ID);
                    return new Result(true, "添加成功");
                }

            }

        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id">补发卡ID</param>
        /// <returns></returns>
        public static Result CardCostById(string id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_CardCost.Where(p => p.ID == id).FirstOrDefault();
                if (query != null)
                {
                    query.IsEnable = true;

                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;

                    db.SaveChanges();
                    return new Result(true, "启用成功");
                }
                return new Result(false, "启用失败");
            }
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="id">补发卡ID</param>
        /// <returns></returns>
        public static Result InCardCostById(string id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_CardCost.Where(p => p.ID == id).FirstOrDefault();
                if (query != null)
                {
                    query.IsEnable = false;
                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, "禁用成功");
                }
                return new Result(false, "禁用失败");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">补发卡ID</param>
        /// <returns></returns>
        public static Result DeleteCardCostById(string id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_CardCost.Where(p => p.ID == id).FirstOrDefault();
                if (query != null)
                {
                    if (query.IsEnable == true) return new Result(false, "已启用的规则不可删除");
                    db.TM_JPOS_CardCost.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }


        /// <summary>
        /// 根据id补发卡信息
        /// </summary>
        /// <param name="ID">补发卡ID</param>
        /// <returns></returns>
        public static Result GetCardCostById(string id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_CardCost.Where(p => p.ID == id).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        #endregion


        #region 商品兑奖规则

        public static Result LoadRuleList(ExchangeGooodsRuleListModel model, string dp)
        {
            {
                var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
                try
                {
                    using (var db = new CRMEntities())
                    {
                        var userLst = db.TM_AUTH_User.Select(v => new { UserID = v.UserID, UserName = v.UserName }).ToList();
                        var userDic = new Dictionary<string, string>();
                        userLst.ForEach(v => userDic[v.UserID.ToString()] = v.UserName);
                        var query = db.TM_JPOS_ExchangeGoods
                            .Select(i => new
                            {
                                i.ExchangeID,
                                i.ExchangeType,
                                i.StartDate,
                                i.EndDate,
                                i.MaxCounts,
                                i.MaxCountsDaily,
                                i.Rate,
                                i.IsTotal,
                                i.Sort,
                                i.AddedDate,
                                i.AddedUser,
                                i.ModifiedDate,
                                i.ModifiedUser,
                                i.Status,
                                i.Code,

                                i.Remark,
                                //AppStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ApproveStatus" && x.OptionValue == SqlFunctions.StringConvert((double)i.Statu)).Select(x => x.OptionText),

                            });

                        Log4netHelper.WriteInfoLog(JsonHelper.Serialize(query) + "1111");





                        if (model.Status != null) query = query.Where(i => i.Status == model.Status);
                        if (!string.IsNullOrEmpty(model.ExchangeID)) query = query.Where(i => i.ExchangeID.Contains(model.ExchangeID));
                        if (!string.IsNullOrEmpty(model.ExchangeType)) query = query.Where(i => i.ExchangeType == model.ExchangeType);
                        //if (model.OddNumber != null) query = query.Where(i => SqlFunctions.StringConvert((double)i.OddNumber).Contains(SqlFunctions.StringConvert((double)model.OddNumber)));
                        if (!string.IsNullOrEmpty(model.Remark)) query = query.Where(i => i.Remark == model.Remark);


                        if (model.StartDate.ToString() != "0001/1/1 0:00:00") query = query.Where(i => i.StartDate > model.StartDate);
                        if (model.EndDate.ToString() != "0001/1/1 0:00:00") query = query.Where(i => i.EndDate < model.EndDate);



                        var res = query.ToList().Select(i => new
                        {
                            i.ExchangeID,
                            ExchangeType = db.TD_SYS_BizOption.Where(j=>j.OptionType == "ExchangeType" && j.OptionValue == i.ExchangeType).Select(k=>k.OptionText).FirstOrDefault(),
                            i.StartDate,
                            i.EndDate,
                            i.MaxCounts,
                            i.MaxCountsDaily,
                            i.Rate,
                            i.IsTotal,
                            i.Sort,
                            i.AddedDate,
                            i.ModifiedDate,
                            i.Status,
                            i.Code,
                            i.Remark,
                            AddedUser = userDic[i.AddedUser],
                            ModifiedUser = userDic[i.ModifiedUser],


                        });

                        Log4netHelper.WriteInfoLog(JsonHelper.Serialize(query) + "2222");

                        return new Result(true, "", new List<object> { res.ToList().ToDataTableSourceVsPage(myDp) });
                    }
                }
                catch
                {
                    return new Result(false, "", new List<object> { new List<dynamic>().ToDataTableSourceVsPage(myDp) });
                }
            }
        }


        public static Result LoadRuleDetailList(string RuleID, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {


                using (var db = new CRMEntities())
                {

                    if (string.IsNullOrEmpty(RuleID))
                    {
                        var query = db.TM_JPOS_ExchangeGoodsDetailTemp
                        .Select(i => new
                        {
                            i.GoodsID,
                            i.GoodsCode,
                            i.MaxCounts,
                            i.DiscountValue,
                            i.Integral,
                            i.MinCounts,
                            i.ID,


                        }).ToList();

                        return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

                    }
                    else
                    {
                        var query = db.TM_JPOS_ExchangeGoodsDetail.Where(i => i.ExchangeID == RuleID)
                            .Select(i => new
                            {
                                i.GoodsID,
                                i.GoodsCode,
                                i.MaxCounts,
                                i.DiscountValue,
                                i.Integral,
                                i.MinCounts,
                                i.ExchangeDetailID,
                                

                            }).ToList();
                        return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                    }


                }
            }
            catch (Exception)
            {
                return new Result(false, "", new List<object> { new List<dynamic>().ToDataTableSourceVsPage(myDp) });
            }

        }

        public static Result LoadCardType()
        {
            using (var db = new CRMEntities())
            {
                var query = db.TD_SYS_BizOption.Where(i => i.OptionType == "CardType");

                return new Result(true, "", query.ToList());


            }

        }

        public static Result LoadCustomerLevel()
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_customerlevel;

                return new Result(true, "", query.ToList());


            }

        }


        public static Result SaveDetail(string strmodel, int authId)
        {
            var model = JsonHelper.Deserialize<RuleDetailModel>(strmodel);

            try
            {

                using (var db = new CRMEntities())
                {
                    if (string.IsNullOrEmpty(model.ExchangeId))
                    {
                        var detail = new TM_JPOS_ExchangeGoodsDetailTemp();
                        if (string.IsNullOrEmpty(model.GoodsID))
                        {
                            var query = db.TD_Sys_Product.Where(p => p.GoodsCode == model.GoodsCode).FirstOrDefault();
                            detail.GoodsID = query == null ? null : query.Gid;
                        }
                        else
                        {
                            detail.GoodsID = model.GoodsID;
                        }
                        detail.GoodsCode = model.GoodsCode;
                        detail.InputCode = model.InputCode;
                        detail.MaxCounts = model.MaxCounts;
                        detail.DiscountValue = model.DiscountValue;
                        detail.MinCounts = model.MinCounts;
                        detail.AddedDate = DateTime.Now;
                        detail.ModifiedDate = DateTime.Now;
                        detail.AddedUser = authId.ToString();
                        detail.ModifiedUser = authId.ToString();

                        db.TM_JPOS_ExchangeGoodsDetailTemp.Add(detail);


                    }

                    else
                    {
                        //var query = db.TM_JPOS_ExchangeGoodsDetail.Where(i => i.ExchangeID == model.ExchangeId).FirstOrDefault();



                        var detail = new TM_JPOS_ExchangeGoodsDetail();
                        detail.ExchangeDetailID = Guid.NewGuid().ToString("N");
                        detail.ExchangeID = model.ExchangeId;
                        detail.GoodsID = model.GoodsID;
                        detail.GoodsCode = model.GoodsCode;
                        detail.InputCode = model.InputCode; ;
                        detail.MaxCounts = model.MaxCounts;
                        detail.DiscountValue = model.DiscountValue;
                        detail.MinCounts = model.MinCounts;
                        detail.AddedDate = DateTime.Now;
                        detail.ModifiedDate = DateTime.Now;
                        detail.AddedUser = authId.ToString();
                        detail.ModifiedUser = authId.ToString();

                        db.TM_JPOS_ExchangeGoodsDetail.Add(detail);


                        //else
                        //{
                        //    query.GoodsID = model.GoodsID;
                        //    query.GoodsCode = model.GoodsCode;
                        //    query.InputCode = model.InputCode;
                        //    query.MaxCounts = model.MaxCounts;
                        //    query.DiscountValue = model.DiscountValue;
                        //    query.MinCounts = model.MinCounts;
                        //    query.ModifiedDate = DateTime.Now;
                        //    query.ModifiedUser = authId.ToString();


                        //    var entry = db.Entry(query);
                        //    entry.State = EntityState.Modified;
                        //}


                    }
                    db.SaveChanges();

                }
                return new Result(true, "操作成功", null);

            }
            catch (DbEntityValidationException e)
            {
                return new Result(false, e.ToString(), null);

            }


        }


        public static Result SaveRule(string strmodel,string rulelist, int authId)
        {


            try
            {
                var model = JsonHelper.Deserialize<ExchangeGooodsRuleListModel>(strmodel);

                var list = JsonHelper.Deserialize<List<List<string>>>(rulelist);

                using (var db = new CRMEntities())
                {
                    if (string.IsNullOrEmpty(model.ExchangeID))
                    {
                        var gid = Guid.NewGuid().ToString("N");

                        var rule = new TM_JPOS_ExchangeGoods();

                        rule.ExchangeID = gid;
                        rule.Code = model.Code;
                        rule.ExchangeType = model.ExchangeType;
                        rule.StartDate = model.StartDate;
                        rule.EndDate = model.EndDate;
                        rule.Remark = model.Remark;
                        rule.Status = 0;
                        rule.AddedDate = DateTime.Now;
                        rule.ModifiedDate = DateTime.Now;
                        rule.AddedUser = authId.ToString();
                        rule.ModifiedUser = authId.ToString();

                        db.TM_JPOS_ExchangeGoods.Add(rule);




                        foreach (var query in list)
                        {
                            var detail = new TM_JPOS_ExchangeGoodsDetail();
                            detail.ExchangeID = gid;
                            detail.ExchangeDetailID = Guid.NewGuid().ToString("N");

                            detail.GoodsID = query[0];
                            detail.GoodsCode = query[1];
                            detail.InputCode = "";
                            detail.MaxCounts = int.Parse(query[2]);
                            detail.DiscountValue = decimal.Parse(query[3]);
                            detail.MinCounts = int.Parse(query[4]);
                            detail.AddedDate = DateTime.Now;
                            detail.ModifiedDate = DateTime.Now;
                            detail.AddedUser = authId.ToString();
                            detail.ModifiedUser = authId.ToString();
                            db.TM_JPOS_ExchangeGoodsDetail.Add(detail);
                        }


                        if (model.CardTypeLimit != null)
                        {
                            foreach (var i in model.CardTypeLimit)
                            {
                                var limit = new TM_JPOS_ExchangeGoodsLimit();
                                limit.ExchangeLimitID = Guid.NewGuid().ToString("N");
                                limit.ExchangeID = gid;
                                limit.LimitType = "CardType";
                                limit.LimitValue = i;
                                limit.AddedDate = DateTime.Now;
                                db.TM_JPOS_ExchangeGoodsLimit.Add(limit);

                            }
                        }
                        if (model.LevelLimit != null)
                        {
                            foreach (var i in model.LevelLimit)
                            {
                                var limit = new TM_JPOS_ExchangeGoodsLimit();
                                limit.ExchangeLimitID = Guid.NewGuid().ToString("N");
                                limit.ExchangeID = gid;
                                limit.LimitType = "CustomerLevel";
                                limit.LimitValue = i;
                                limit.AddedDate = DateTime.Now;
                                db.TM_JPOS_ExchangeGoodsLimit.Add(limit);

                            }
                        }
                    }

                    else {
                        var goods = db.TM_JPOS_ExchangeGoods.Where(i => i.ExchangeID == model.ExchangeID).FirstOrDefault();
                        if (goods != null)
                        {
                            goods.Code = model.Code;
                            goods.ExchangeType = model.ExchangeType;
                            goods.StartDate = model.StartDate;
                            goods.EndDate = model.EndDate;
                            goods.Remark = model.Remark;
                            goods.ModifiedDate = DateTime.Now;
                            goods.ModifiedUser = authId.ToString();                      
                        }

                        if (model.LevelLimit != null)
                        {
                            var limit_old = db.TM_JPOS_ExchangeGoodsLimit.Where(j => j.ExchangeID == model.ExchangeID).ToList();
                            foreach (var s in limit_old)
                            {
                                db.TM_JPOS_ExchangeGoodsLimit.Remove(s);
                            }
                            

                            foreach (var i in model.LevelLimit)
                            {
                                var limit = new TM_JPOS_ExchangeGoodsLimit();
                                limit.ExchangeLimitID = Guid.NewGuid().ToString("N");
                                limit.ExchangeID = model.ExchangeID;
                                limit.LimitType = "CustomerLevel";
                                limit.LimitValue = i;
                                limit.AddedDate = DateTime.Now;
                                db.TM_JPOS_ExchangeGoodsLimit.Add(limit);

                            }
                        }

                        //var detail_old = db.TM_JPOS_ExchangeGoodsDetail.Where(k => k.ExchangeID == model.ExchangeID).ToList();
                        //foreach (var s in detail_old)
                        //{
                        //    db.TM_JPOS_ExchangeGoodsDetail.Remove(s);
                        //}

                        //foreach (var query in list)
                        //{
                        //    var detail = new TM_JPOS_ExchangeGoodsDetail();
                        //    detail.ExchangeID = model.ExchangeID;
                        //    detail.ExchangeDetailID = Guid.NewGuid().ToString("N");

                        //    detail.GoodsID = query[0];
                        //    detail.GoodsCode = query[1];
                        //    detail.InputCode = "";
                        //    detail.MaxCounts = int.Parse(query[2]);
                        //    detail.DiscountValue = decimal.Parse(query[3]);
                        //    detail.MinCounts = int.Parse(query[4]);
                        //    detail.AddedDate = DateTime.Now;
                        //    detail.ModifiedDate = DateTime.Now;
                        //    detail.AddedUser = authId.ToString();
                        //    detail.ModifiedUser = authId.ToString();
                        //    db.TM_JPOS_ExchangeGoodsDetail.Add(detail);
                        //}

                    
                    
                    }

                    

                    db.SaveChanges();

                    return new Result(true, "新增成功", null);


                }
            }
            catch (DbEntityValidationException e)
            {
                return new Result(false, e.ToString(), null);

            }

        }


        public static Result GetRuleById(string id)
        {
            using (var db = new CRMEntities())
            {
                var query = db.TM_JPOS_ExchangeGoods.Where(i => i.ExchangeID == id);

                return new Result(true, "", query.ToList());
            }
        }


        public static Result GetRuleLimitById(string id)
        {
            using (var db = new CRMEntities())
            {
                var query = db.TM_JPOS_ExchangeGoodsLimit.Where(i => i.ExchangeID == id);

                return new Result(true, "", query.ToList());
            }
        }


        public static Result ApproveExchangeRuleById(string id)
        {
            using (var db = new CRMEntities())
            {
                var query = db.TM_JPOS_ExchangeGoods.Where(i => i.ExchangeID == id).FirstOrDefault();
                query.Status = 1;

                db.SaveChanges();


                return new Result(true, "", null);
            }
        }


        public static Result TruncateTemp()
        {
            using (var db = new CRMEntities())
            {
                db.Database.ExecuteSqlCommand("Truncate table TM_JPOS_ExchangeGoodsDetailTemp");

                db.SaveChanges();


                return new Result(true, "", null);
            }
        }


        public static Result DeleteExchangeRuleById(string id)
        {
            using (var db = new CRMEntities())
            {
                var query = db.TM_JPOS_ExchangeGoods.Where(i => i.ExchangeID == id).FirstOrDefault();
                db.TM_JPOS_ExchangeGoods.Remove(query);
                db.SaveChanges();
                return new Result(true, "删除成功", null);
            }
        }

        public static Result DeleteExchangeDetailRuleById(string detailid, string id)
        {
            using (var db = new CRMEntities())
            {
                if (string.IsNullOrEmpty(id))
                {
                    var detailid2 = int.Parse(detailid);
                    var query = db.TM_JPOS_ExchangeGoodsDetailTemp.Where(i => i.ID == detailid2).FirstOrDefault();
                    db.TM_JPOS_ExchangeGoodsDetailTemp.Remove(query);
                    db.SaveChanges();
                }

                else
                {
                    var query = db.TM_JPOS_ExchangeGoodsDetail.Where(i => i.ExchangeID == id).FirstOrDefault();
                    db.TM_JPOS_ExchangeGoodsDetail.Remove(query);
                    db.SaveChanges();
                }
                return new Result(true, "删除成功", null);
            }
        }

        public static Result LoadGoods()
        {
            using (var db = new CRMEntities())
            {


                var query = db.TD_Sys_Product.AsQueryable();




                return new Result(true, "", new List<object> { query.ToList() });
            }
        }



        public static Result AddExchangeRuleById(string Id, string strdetail, int authId)
        {
            using (var db = new CRMEntities())
            {


                var detail = JsonHelper.Deserialize<List<string>>(strdetail);
                var d = new TM_JPOS_ExchangeGoodsDetail();
                d.ExchangeDetailID = Guid.NewGuid().ToString("N");
                d.ExchangeID = Id;
                d.GoodsID = detail[0];
                d.GoodsCode = detail[1];
                d.InputCode = "";
                d.MaxCounts = int.Parse(detail[2]);
                d.DiscountValue = decimal.Parse(detail[3]);
                d.MinCounts = int.Parse(detail[4]);
                d.AddedDate = DateTime.Now;
                d.ModifiedDate = DateTime.Now;
                d.AddedUser = authId.ToString();
                d.ModifiedUser = authId.ToString();
                db.TM_JPOS_ExchangeGoodsDetail.Add(d);

                db.SaveChanges();

                return new Result(true, "", new List<object> { });
            }
        }



        #endregion


    }
}
