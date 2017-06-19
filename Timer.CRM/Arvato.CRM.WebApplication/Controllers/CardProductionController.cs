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
    public class CardProductionController : Controller
    {
        //
        // GET: /CardProduction/

        #region 批量制发卡
        public ActionResult BatchProduction()
        {
            return View();
        }

        public JsonResult GetBatchCardList(string oddId, string status, string IsExecute, string modifyBy, string beginCardNo, string endCardNo,string type)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("CardProduction", "GetBatchCardList", false, new object[] { oddId, status, IsExecute, modifyBy, beginCardNo, endCardNo, type,JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }

        public JsonResult LoadMathRule()
        {
            var result = Submit("CardProduction", "LoadMathRule", false, new object[] { });
            return Json(result.Obj[0]);
        }

        public JsonResult GetCardInfo(string beginCardNo,string cardNum,string endCardNo)
        {
            var result = Submit("CardProduction", "GetCardInfo", false, new object[] { beginCardNo, cardNum,endCardNo });
            return Json(result);
        }


        public JsonResult AddCard(string jsonParam,string status)
        {
            var result = Submit("CardProduction", "AddCard", false, new object[] { jsonParam, status, Auth.UserID });
            return Json(result);    
        }

        public JsonResult AddCardByManual(string jsonParam, string status)
        {
            var result = Submit("CardProduction", "AddCardByManual", false, new object[] { jsonParam, status, Auth.UserID });
            return Json(result);
        }

        public JsonResult GetAcceptingUnit(string beginCardNo)
        {
            var result = Submit("CardProduction", "GetAcceptingUnit", false, new object[] { beginCardNo});
            return Json(result.IsPass);    
        }

        public JsonResult BoxIDBuild(string boxId,string productionNum,string beginCardNo,string endCardNo)
        {                                                              
            var result = Submit("CardProduction","BoxIDBuild", false, new object[] { boxId,productionNum,beginCardNo,endCardNo });
            return Json(result);    
        }

        public JsonResult GetEmptyBoxNo(string boxNoInput)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("CardProduction", "GetEmptyBoxNo", false, new object[] { boxNoInput, Auth.UserID, JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);    
        }
        #endregion

      
    }
}
