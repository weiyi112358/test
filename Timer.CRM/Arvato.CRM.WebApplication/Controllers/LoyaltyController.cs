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
    public class LoyaltyController : Controller
    {
        //
        // GET: /Loyalty/
        #region 会员等级积分计算规则管理
        public ActionResult Rule()
        {
            if (Session["hideRuleName"] == null) Session["hideRuleName"] = "";
            if (Session["hideRuleType"] == null) Session["hideRuleType"] = "";
            if (Session["hideEnable"] == null) Session["hideEnable"] = "";
            return View();

        }
        /// <summary>
        /// 获取会员等级积分计算规则管理（分页）
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="ruleType"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public JsonResult GetRuleData(string ruleName, string ruleType, string enable)
        {
            var dts = Request.CreateDataTableParameter();
            var groupId = Auth.DataGroupID;
            var result = Submit("Loyalty", "GetRuleData", false, new object[] { ruleName, ruleType, enable, JsonHelper.Serialize(dts), groupId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public JsonResult DeleteRuleById(int ruleId)
        {
            var result = Submit("Loyalty", "DeleteRuleById", false, new object[] { ruleId });
            return Json(result);
        }
        /// <summary>
        /// 激活规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult ActiveRuleById(int ruleId)
        {
            var result = Submit("Loyalty", "ActiveRuleById", false, new object[] { ruleId });
            return Json(result);
        }
        /// <summary>
        /// 禁用规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult InActiveRuleById(int ruleId)
        {
            var result = Submit("Loyalty", "InActiveRuleById", false, new object[] { ruleId });
            return Json(result);
        }
        /// <summary>
        /// 获取规则类型列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRuleTypeList()
        {
            string optType = "RuleType";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 会员等级积分计算规则设置
        [HttpPost]
        public ActionResult RuleSetting()
        {
            string RuleID = Request.Form["hideRuleID"] != null ? Request.Form["hideRuleID"].ToString() : "";
            if (Request.Form["hideRuleName"] != null) Session["hideRuleName"] = Request.Form["hideRuleName"].ToString();
            if (Request.Form["hideRuleType"] != null) Session["hideRuleType"] = Request.Form["hideRuleType"].ToString();
            if (Request.Form["hideEnable"] != null) Session["hideEnable"] = Request.Form["hideEnable"].ToString();

            //获取规则下拉列表
            string optType = "RuleType";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            dynamic dyn = JsonHelper.Deserialize<List<object>>(result.Obj[0].ToString());
            List<SelectListItem> listRuleType = new List<SelectListItem>();
            listRuleType.Add(new SelectListItem { Text = "请选择", Value = "" });
            foreach (var b in dyn)
            {
                listRuleType.Add(new SelectListItem { Text = b.OptionText, Value = b.OptionValue });
            }
            ViewData["listRuleType"] = listRuleType;

            //获取可配置成行为的别名
            var res = Submit("Loyalty", "GetActionAliasList", false, new object[] { });
            dynamic dyn1 = JsonHelper.Deserialize<List<object>>(res.Obj[0].ToString());
            List<SelectListItem> listActionLeft = new List<SelectListItem>();
            listActionLeft.Add(new SelectListItem { Text = "请选择", Value = "" });
            foreach (var b in dyn1)
            {
                listActionLeft.Add(new SelectListItem { Text = b.FieldDesc, Value = b.FieldAlias });
            }
            listActionLeft.Add(new SelectListItem { Text = "账户", Value = "Account" });
            ViewData["listActionLeft"] = listActionLeft;

            //数据群组ID下拉列表
            var groupId = Auth.DataGroupID;
            var res1 = Submit("Loyalty", "GetDataGroupList", false, new object[] { groupId });
            dynamic dyn2 = JsonHelper.Deserialize<List<object>>(res1.Obj[0].ToString());
            List<SelectListItem> listDataGroupID = new List<SelectListItem>();
            listDataGroupID.Add(new SelectListItem { Text = "请选择", Value = "" });
            foreach (var b in dyn2)
            {
                listDataGroupID.Add(new SelectListItem { Text = b.SubDataGroupName, Value = b.SubDataGroupID });
            }
            ViewData["listDataGroupID"] = listDataGroupID;

            //编辑
            if (RuleID != "")
            {
                var info = Submit("Loyalty", "GetRuleById", false, new object[] { RuleID });
                var rule = JsonHelper.Deserialize<Rules>(info.Obj[0].ToString());
                var modelRule = new RuleModel()
                {
                    DataGroupID = rule.DataGroupID,
                    RuleName = rule.RuleName,
                    RuleType = rule.RuleType,
                    StartDate = rule.StartDate,
                    EndDate = rule.EndDate,
                    RunIndex = rule.RunIndex,
                    RuleID = rule.RuleID.ToString(),
                    Condition = rule.Condition,
                    ConditionResult=rule.ConditionResult,
                    Actions = rule.Action,
                    Schedule = rule.Schedule,
                    Enable = rule.Enable?"已激活":"未激活",
                    LastExecTime = rule.LastExcuteTime
                };
                return View(modelRule);

            }
            else
            {
                return View();
            }
        }

        public JsonResult GetCouponTempletFieldAliaListByAliasKey(string key)
        {
            var info = Submit("Loyalty", "GetCouponTempletFieldAliaListByAliasKey", false, new object[] { key });
            return Json(info.Obj[0].ToString());
        }

        /// <summary>
        /// 根据id获取单条规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public JsonResult GetRuleById(int ruleId)
        {
            var info = Submit("Loyalty", "GetRuleById", false, new object[] { ruleId });
            return Json(info);
        }
        /// <summary>
        /// 获取会员细分过滤条件所有左值
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRuleLeftValuesAll()
        {
            var info = Submit("Loyalty", "GetRuleLeftValuesAll", false, new object[] { });
            return Json(info.Obj[0].ToString());
        }
        /// <summary>
        /// 获取关键字列表
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public JsonResult GetTradeAliasKeyList(string type1,string type2)
        {
            var info = Submit("Loyalty", "GetTradeAliasKeyList", false, new object[] { type1, type2 });
            return Json(info.Obj[0].ToString());
        }
        /// <summary>
        /// 获取行为右值列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTradeAliasKeyList1()
        {
            var info = Submit("Loyalty", "GetTradeAliasKeyList1", false, new object[] {  });
            return Json(info.Obj[0].ToString());
        }
        /// <summary>
        /// 根据别名value值获取别名desc
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public JsonResult GetAliasDescByValue(string value)
        {
            var info = Submit("Loyalty", "GetAliasDescByValue", false, new object[] { value });
            return Json(info.Obj[0].ToString());
        }
        /// <summary>
        /// 保存规则信息
        /// </summary>
        /// <param name="ruleMaster"></param>
        /// <returns></returns>
        public JsonResult SaveRule(RuleModel ruleMaster)
        {
            var authId= Auth.UserID;
            ruleMaster.RuleName = HttpUtility.HtmlDecode(ruleMaster.RuleName);
            var dataGroupId = ruleMaster.DataGroupID;
            var info = Submit("Loyalty", "SaveRule", false, new object[] { ruleMaster, authId, dataGroupId });
            return Json(info);
        }
        
        #endregion
    }
}
