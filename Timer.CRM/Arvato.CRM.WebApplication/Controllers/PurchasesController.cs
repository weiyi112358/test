using Arvato.CRM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Model;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class PurchasesController : Controller
    {
        //
        // GET: /Purchases/
   

        #region 定卡
        public ActionResult Customize()
        {
            return View();
        }

        /// <summary>
        ///  定卡列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applyNumber"></param>
        /// <param name="deliverNumber"></param>
        /// <param name="modifyBy"></param>
        /// <param name="status"></param>
        /// <param name="executeStatus"></param>
        /// <param name="page"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult CustomizeCardList(string oddId, string status, string agent, string modifyBy, string destineNumber)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Purchases", "CustomizeCardList", false, new object[] { oddId, status, agent, modifyBy, destineNumber,JsonHelper.Serialize(pageParamers)});
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载供应商
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadAgent()
        {
            var result = Submit("Purchases", "LoadAgent", false, new object[] { });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 添加新定卡单
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="destineNum"></param>
        /// <returns></returns>
        public JsonResult AddCard(string jsonParam, string destineNum, string status)
        {
            var result = Submit("Purchases", "AddCard", false, new object[] { jsonParam,destineNum,status,Auth.UserID});
            return Json(result.IsPass);
        }

        /// <summary>
        /// 加载公司
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadCompany()
        {
            var result = Submit("Purchases", "LoadCompany", false, new object[] { });
            return Json(result.Obj[0]);
        }

        public JsonResult LoadProviceCode(string provinceName)
        {
            var result = Submit("Purchases", "LoadProviceCode", false, new object[] { provinceName });
            return Json(result.Obj[0]);
        }

        public JsonResult GetCardNo(string provinceCode, string cardNum)
        {
            var result = Submit("Purchases", "GetCardNo", false, new object[] { provinceCode, cardNum });
            return Json(result);
        }
       
        public JsonResult GetCardNoManual(string beginCardNo,string provinceCode,string cardNum)
        {
            var result = Submit("Purchases", "GetCardNoManual", false, new object[] { beginCardNo, provinceCode,cardNum});
            return Json(result);
        }

        #endregion


        #region 手动生成盒号
        public ActionResult AutoBoxNo()
        {
            return View();
        }

        public JsonResult BoxNoList(string boxNo,string status,string modifyby)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Purchases", "BoxNoList", false, new object[] { boxNo, status, modifyby, JsonHelper.Serialize(pageParamers), Auth.UserID });
            return Json(result.Obj[0]);
        }

        public JsonResult AddBoxNo(string jsonParam)
        {
            var result = Submit("Purchases", "AddBoxNo", false, new object[] { jsonParam, Auth.UserID });
          return Json(result);
        }
        #endregion

        #region 收卡
        public ActionResult Retrieve()
        {
            return View();
        }

        /// <summary>
        /// 收卡列表
        /// </summary>
        /// <param name="retrieveId"></param>
        /// <param name="status"></param>
        /// <param name="agent"></param>
        /// <param name="modifyBy"></param>
        /// <param name="destineNumber"></param>
        /// <param name="oddId"></param>
        /// <returns></returns>
        public JsonResult RetrieveCardList(string retrieveId, string status, string agent, string modifyBy, string destineNumber, string oddId)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Purchases", "RetrieveCardList", false, new object[] { retrieveId, status, agent, modifyBy, destineNumber, oddId, JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 添加收卡订单
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public JsonResult RetrieveCard(string jsonParam,string status)
        {
            var result = Submit("Purchases", "RetrieveCard", false, new object[] { jsonParam, status, Auth.UserID });
            return Json(result.IsPass);
        }

        /// <summary>
        /// 加载定卡单单号列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOddIdList()
        {
            var result = Submit("Purchases", "GetOddIdList", false, new object[] { });
            return Json(result.Obj[0]);
        }

        public JsonResult GetAgentList(string oddId)
        {
            var result = Submit("Purchases", "GetAgentList", false, new object[] {oddId });     
            return Json(result.Obj[0]);                
        }

        public JsonResult GetProvince(string oddId)
        {
            var result = Submit("Purchases", "GetProvince", false, new object[] { oddId });       
            return Json(result.Obj[0]);
        }

        public JsonResult CheckBeginCardNo(string retrieveNum, string beginCardNo,string oddId)
        {
            var result = Submit("Purchases", "CheckBeginCardNo", false, new object[] { retrieveNum, beginCardNo,oddId });     
            return Json(result);
        }     
     
        #endregion
    }
}
