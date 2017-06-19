using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arvato.CRM.WebApplication.Controllers
{
    /// <summary>
    /// 会员促销规则 
    /// </summary>
    public class MemberSalesPromotionController : Controller
    {
        //
        // GET: /Loyalty/
        #region 会员促销规则管理
        public ActionResult Promotion()
        {
            if (Session["hideCode"] == null) Session["hideCode"] = "";
            if (Session["hideRuleType"] == null) Session["hideRuleType"] = "";
            if (Session["hideApproveStatus"] == null) Session["hideApproveStatus"] = "";
            if (Session["hideExecuteStatus"] == null) Session["hideExecuteStatus"] = "";
            return View();

        }

        public ActionResult PromotionGift()
        {
            if (Session["hideCode"] == null) Session["hideCode"] = "";
            if (Session["hideRuleType"] == null) Session["hideRuleType"] = "";
            if (Session["hideApproveStatus"] == null) Session["hideApproveStatus"] = "";
            if (Session["hideExecuteStatus"] == null) Session["hideExecuteStatus"] = "";
            return View();

        }
        /// <summary>
        /// 获取促销规则管理（分页）
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="ruleType"></param>
        /// <param name="approveStatus"></param>
        /// <returns></returns>
        public JsonResult GetRuleData(string Code, string ruleType, int? approveStatus, int? executeStatus)
        {
            //(string Code, string ruleType, int? approveStatus,int? executeStatus , string dp )
            var dts = Request.CreateDataTableParameter();
            //   var groupId = Auth.DataGroupID;
            var result = Submit("MemberPromotion", "GetRuleData", false, new object[] { Code, ruleType, approveStatus, executeStatus, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetProductData(string Code, string Name, string Sort, string Brand)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("MemberPromotion", "GetProductData", false, new object[] { Code, Name, Sort, Brand, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetStoreData(string Code, string Name)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("MemberPromotion", "GetStoreData", false, new object[] { Code, Name, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public JsonResult DeleteRuleById(string ruleId)
        {
            var authId = Auth.UserID;
            var result = Submit("MemberPromotion", "DeleteRuleById", false, new object[] { ruleId, authId });
            return Json(result);
        }
        /// <summary>
        /// 激活规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult ActiveRuleById(string ruleId, bool IsWakeUp)
        {
            var authId = Auth.UserID;
            var result = Submit("MemberPromotion", "ActiveRuleById", false, new object[] { ruleId, IsWakeUp, authId });
            return Json(result);
        }
        /// <summary>
        /// 审核规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult ApproveRuleById(string ruleId, int active)
        {
            var authId = Auth.UserID;
            var result = Submit("MemberPromotion", "ApproveRuleById", false, new object[] { ruleId, active, authId });
            return Json(result);
        }
        /// <summary>
        /// 获取规则类型列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRuleTypeList()
        {
            string optType = "MemberPromotion";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }

        #endregion

        #region 会员促销规则 条件
        /// <summary>
        /// 获取不同促销规则的条件
        /// </summary>
        /// <param name="PromotionType"></param>
        /// <returns></returns>
        public ActionResult GetConditionList(int PromotionType)
        {
            var lst = PromotionRuleConditionDic.GetPromotionCondition(PromotionType);
            if (lst.Count == 1)
                lst[0].IsRequire = true;
            return Json(lst);
        }

        public ActionResult GetConditionOptions(string Type)
        {
            Result result;
            switch (Type)
            {
                case "MemberLevel":
                    result = Submit("MemberPromotion", "GetMemberLevel", false, new object[] { });
                    return Json(result);
                case "AllGoods":
                    result = Submit("MemberPromotion", "GetAllGoods", false, new object[] { });
                    return Json(result);
                case "PointType":
                    result = Submit("MemberPromotion", "GetPointType", false, new object[] { });
                    return Json(result);

                case "CardType":
                    result = Submit("MemberPromotion", "GetCardType", false, new object[] { });
                    return Json(result);
            }
            return Json(null);
        }

        #endregion

        #region 会员促销规则设置

        [HttpPost]
        public ActionResult PromotionSettingGift()
        {
            string RuleID = Request.Form["hideRuleID"] != null ? Request.Form["hideRuleID"].ToString() : "";
            if (Request.Form["hideRuleName"] != null) Session["hideRuleName"] = Request.Form["hideRuleName"].ToString(); if (Request.Form["hideRuleType"] != null) Session["hideRuleType"] = Request.Form["hideRuleType"].ToString();

            //获取规则下拉列表
            string optType = "MemberPromotion";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            dynamic dyn = JsonHelper.Deserialize<List<object>>(result.Obj[0].ToString());
            List<SelectListItem> listRuleType = new List<SelectListItem>();
            listRuleType.Add(new SelectListItem { Text = "请选择", Value = "" });
            foreach (var b in dyn)
            {
                listRuleType.Add(new SelectListItem { Text = b.OptionText, Value = b.OptionValue });
            }
            ViewData["listRuleType"] = listRuleType;


            //新增
            if (string.IsNullOrEmpty(RuleID))
                return View(new PromotionRule { StartDate = DateTime.Today });
            //编辑
            var info = Submit("MemberPromotion", "GetPromotionById", false, new object[] { RuleID });
            var rule = JsonHelper.Deserialize<PromotionRule>(info.Obj[0].ToString());
            rule.PromotionTypeDesc = listRuleType.Find(v => v.Value == rule.PromotionType).Text;

            var billitem = Submit("MemberPromotion", "GetPromotionItemByBillID", false, new object[] { RuleID });

            if (billitem.Obj != null && billitem.Obj[0] != null)
            {
                var limitQuantity = billitem.Obj[0].ToString();
                int limit = 1;
                if (!string.IsNullOrEmpty(limitQuantity) && int.TryParse(limitQuantity, out limit))
                {
                    rule.LimitQuantity = limit;
                }
            }

            return View(rule);
        }

        [HttpPost]
        public ActionResult PromotionSetting()
        {
            string RuleID = Request.Form["hideRuleID"] != null ? Request.Form["hideRuleID"].ToString() : "";
            if (Request.Form["hideRuleName"] != null) Session["hideRuleName"] = Request.Form["hideRuleName"].ToString();
            if (Request.Form["hideRuleType"] != null) Session["hideRuleType"] = Request.Form["hideRuleType"].ToString();

            //获取规则下拉列表
            string optType = "MemberPromotion";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            dynamic dyn = JsonHelper.Deserialize<List<object>>(result.Obj[0].ToString());
            List<SelectListItem> listRuleType = new List<SelectListItem>();
            listRuleType.Add(new SelectListItem { Text = "请选择", Value = "" });
            foreach (var b in dyn)
            {
                listRuleType.Add(new SelectListItem { Text = b.OptionText, Value = b.OptionValue });
            }
            ViewData["listRuleType"] = listRuleType;


            //新增
            if (string.IsNullOrEmpty(RuleID))
                return View(new PromotionRule { StartDate = DateTime.Today });
            //编辑
            var info = Submit("MemberPromotion", "GetPromotionById", false, new object[] { RuleID });
            var rule = JsonHelper.Deserialize<PromotionRule>(info.Obj[0].ToString());
            rule.PromotionTypeDesc = listRuleType.Find(v => v.Value == rule.PromotionType).Text;

            var billitem = Submit("MemberPromotion", "GetPromotionItemByBillID", false, new object[] { RuleID });

            if (billitem.Obj != null && billitem.Obj[0] != null)
            {
                var limitQuantity = billitem.Obj[0].ToString();
                int limit = 1;
                if (!string.IsNullOrEmpty(limitQuantity) && int.TryParse(limitQuantity, out limit))
                {
                    rule.LimitQuantity = limit;
                }
            }

            return View(rule);
        }

        public ActionResult CopyPromotion(string billid, DateTime start, DateTime end)
        {
            var authId = Auth.UserID;
            var info = Submit("MemberPromotion", "CopyPromotion", false, new object[] { billid, start, end, authId });
            return Json(info);
        }

        /// <summary>
        /// 根据id获取单条规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public JsonResult GetPromotionById(int ruleId)
        {
            var info = Submit("MemberPromotion", "GetPromotionById", false, new object[] { ruleId });
            return Json(info);
        }

        /// <summary>
        /// 保存规则信息
        /// </summary>
        /// <param name="ruleMaster"></param>
        /// <returns></returns>
        public JsonResult SavePromotion(PromotionRule ruleMaster)
        {
            var authId = Auth.UserID;
            var info = Submit("MemberPromotion", "SavePromotion", false, new object[] { ruleMaster, authId });
            return Json(info);
        }

        #endregion

        #region 时间：2017-03-21  姓名：潘荣胜 （卡补发规则）
        /// <summary>
        /// 卡补发规则
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplementaryCard()
        {
            //获取分公司
            var company = Submit("MemberPromotion", "GetCompanyCodeList", false);
            ViewBag.CompanyList = company.Obj;

            //获取补卡类型
            var cardType = Submit("MemberPromotion", "GetCardTypeList", false);
            ViewBag.cardTypeList = cardType.Obj;
            return View();
        }

        /// <summary>
        /// 保存补发卡数据
        /// </summary>
        /// <param name="CardAmount">购买金额</param>
        /// <param name="CardCost">补发费用</param>
        /// <returns></returns>
        public JsonResult AddSupplementaryCard(string CardAmount, string CardCost, string CompanyCode, string CardType, string CardCostId)
        {
            var userId = Auth.UserID;//当前用户ID
            var result = Submit("MemberPromotion", "AddSupplementaryCard", false,
                new object[] { CardAmount, CardCost, CompanyCode, CardType, userId, CardCostId });
            return Json(result);
        }

        /// <summary>
        /// 加载补卡数据
        /// </summary>
        /// <param name="CompanyCode">分公司</param>
        /// <returns></returns>
        public JsonResult GetSupplementaryCard(string CompanyCode)
        {
            var dataParamers = Request.CreateDataTableParameter();
            var rst = Submit("MemberPromotion", "GetSupplementaryCard", false,
                new object[] { JsonHelper.Serialize(dataParamers), CompanyCode });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="ID">补发卡ID</param>
        /// <returns></returns>
        public JsonResult CardCostById(string ID)
        {
            var result = Submit("MemberPromotion", "CardCostById", false, new object[] { ID });
            return Json(result);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="ID">补发卡ID</param>
        /// <returns></returns>
        public JsonResult InCardCostById(string ID)
        {
            var result = Submit("MemberPromotion", "InCardCostById", false, new object[] { ID });
            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID">补发卡ID</param>
        /// <returns></returns>
        public JsonResult DeleteCardCostById(string ID)
        {
            var result = Submit("MemberPromotion", "DeleteCardCostById", false, new object[] { ID });
            return Json(result);
        }

        /// <summary>
        /// 根据id补发卡信息
        /// </summary>
        /// <param name="ID">补发卡ID</param>
        /// <returns></returns>
        public JsonResult GetCardCostById(string ID)
        {
            var result = Submit("MemberPromotion", "GetCardCostById", false, new object[] { ID });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 积分抵现
        public ActionResult IntergralArrival()
        {
            return View();
        }

        public JsonResult GetArrivalList(string status, string name)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("MemberPromotion", "GetArrivalList", false, new object[] { status, name, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }



        #endregion

        #region 商品兑奖规则
        public ActionResult ExchangeGoods()
        {

            return View();

        }

        public ActionResult ExchangeGoods_Add()
        {
            string ID = Request.Form["hideID"] != null ? Request.Form["hideID"].ToString() : "";
            string Info = Request.Form["hideInfo"] != null ? Request.Form["hideInfo"].ToString() : "";
            ViewBag.ID = ID;
            ViewBag.Info = Info;
            return View();
        }

        //加载列表
        public ActionResult LoadRuleList(string strmodel)
        {
            var dts = Request.CreateDataTableParameter();
            ExchangeGooodsRuleListModel model = JsonHelper.Deserialize<ExchangeGooodsRuleListModel>(strmodel);
            var result = Submit("MemberPromotion", "LoadRuleList", false, new object[] { model, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        //加载明细列表
        public ActionResult LoadRuleDetailList(string RuleID, string value)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("MemberPromotion", "LoadRuleDetailList", false, new object[] { RuleID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());

        }


        //load card type list
        public JsonResult LoadCardType()
        {
            var result = Submit("MemberPromotion", "LoadCardType", false);
            return Json(result.Obj[0].ToString());

        }

        //load customer level
        public JsonResult LoadCustomerLevel()
        {
            var result = Submit("MemberPromotion", "LoadCustomerLevel", false);
            return Json(result.Obj[0].ToString());

        }

        public JsonResult SaveDetail(string strmodel)
        {
            var authId = Auth.UserID;

            var result = Submit("MemberPromotion", "SaveDetail", false, new object[] { strmodel, authId });
            return Json(result);

        }

        public JsonResult SaveRule(string strmodel,string detail)
        {
            var authId = Auth.UserID;

            var result = Submit("MemberPromotion", "SaveRule", false, new object[] { strmodel,detail, authId });
            return Json(result);

        }

        public JsonResult GetRuleById(string id)
        {


            var result = Submit("MemberPromotion", "GetRuleById", false, new object[] { id });
            return Json(result.Obj[0].ToString());

        }

        public JsonResult GetRuleLimitById(string id)
        {


            var result = Submit("MemberPromotion", "GetRuleLimitById", false, new object[] { id });
            return Json(result.Obj[0].ToString());

        }

        public JsonResult ApproveExchangeRuleById(string id)
        {


            var result = Submit("MemberPromotion", "ApproveExchangeRuleById", false, new object[] { id });
            return Json(result);

        }

        public JsonResult TruncateTemp()
        {


            var result = Submit("MemberPromotion", "TruncateTemp", false);
            return Json(result);

        }

        public JsonResult DeleteExchangeRuleById(string id)
        {


            var result = Submit("MemberPromotion", "DeleteExchangeRuleById", false, new object[] { id });
            return Json(result);

        }


        public JsonResult DeleteExchangeDetailRuleById(string detailid, string id)
        {


            var result = Submit("MemberPromotion", "DeleteExchangeDetailRuleById", false, new object[] { detailid, id });
            return Json(result);

        }

        public JsonResult LoadGoods()
        {


            var result = Submit("MemberPromotion", "LoadGoods", false, new object[] { });
            return Json(result.Obj[0].ToString());

        }

        public JsonResult AddExchangeRuleById(string Id,string strdetail)
        {

            var authId = Auth.UserID;
            var result = Submit("MemberPromotion", "AddExchangeRuleById", false, new object[] {Id,strdetail,authId });
            return Json(result);

        }






        #endregion








    }


    /// <summary>
    /// 促销条件处理类
    /// </summary>
    public static class PromotionRuleConditionDic
    {
        private static List<PromotionRuleCondition> GetAll()
        {
            return new List<PromotionRuleCondition>
            {
                new PromotionRuleCondition { Code ="PayCash",Name ="购买金额>=(区间)", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Decimal, UsedInWhichPromotion = new List<int> {1,2,3,4,6,7 } },
                new PromotionRuleCondition { Code ="PayCash2",Name ="购买金额=(整除)", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Decimal, UsedInWhichPromotion = new List<int> { 6,7 } },
                new PromotionRuleCondition { Code ="PayQuantity",Name ="购买数量>=(区间)", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Decimal, UsedInWhichPromotion = new List<int> { 6,7 } },
                new PromotionRuleCondition { Code ="PayQuantity2",Name ="购买数量=(整除)", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Decimal, UsedInWhichPromotion = new List<int> { 6,7 } },
                //前置条件
                //new PromotionRuleCondition { Code ="GoodsArea",Name ="商品范围", IsRequire = true, Type = PromotionRuleConditionTypeEnum.Select, UsedInWhichPromotion = new List<int> {  2,5,6,7} },
                new PromotionRuleCondition { Code ="MemberLevel",Name ="会员级别", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Select, UsedInWhichPromotion = new List<int> {5,6,7 } },
                new PromotionRuleCondition { Code ="MemberBirthday",Name ="会员生日", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Birthday, UsedInWhichPromotion = new List<int> {5,6,7 } },
                //new PromotionRuleCondition {Code ="MemberPointMulti",Name="会员级别分组",IsRequire = true , Type = PromotionRuleConditionTypeEnum.MemberLevelMulti,UsedInWhichPromotion  = new List<int> { 8} },
                //new PromotionRuleCondition {Code ="MemberGiftMulti",Name="会员礼品分组",IsRequire = true , Type = PromotionRuleConditionTypeEnum.MemberLevelMulti,UsedInWhichPromotion  = new List<int> { 9} },
                //new PromotionRuleCondition { Code ="Goods",Name ="购买商品", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Select, UsedInWhichPromotion = new List<int> { 5 } },
                //new PromotionRuleCondition { Code ="Payment",Name ="付款方式", IsRequire = false, Type = PromotionRuleConditionTypeEnum.Select, UsedInWhichPromotion = new List<int> { 5,4} },
                //new PromotionRuleCondition { Code ="MemberLevelUpDay",Name ="会员升级日", IsRequire = false, Type = PromotionRuleConditionTypeEnum.DayRegion, UsedInWhichPromotion = new List<int> {5,4,6,7 } },
                //new PromotionRuleCondition { Code ="MemberCardDay",Name ="会员开卡日", IsRequire = false, Type = PromotionRuleConditionTypeEnum.DayRegion, UsedInWhichPromotion = new List<int> {5 ,4,6,7} },
             };
        }

        public static List<PromotionRuleCondition> GetPromotionCondition(int Promotion)
        {
            return GetAll().Where(v => v.UsedInWhichPromotion.Contains(Promotion)).ToList();
        }
    }
}
