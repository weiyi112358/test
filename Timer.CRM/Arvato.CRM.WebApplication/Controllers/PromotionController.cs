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
    public class PromotionController : Controller
    {
        //
        // GET: /PromotionSetting/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PromotionBusinessPlan()
        {
            return View();
        }

        public ActionResult PromotionMark()
        {
            return View();
        }

        public ActionResult PromotionDetail()
        {
            return View();
        }

        public JsonResult GetPromotionByid(string pid)
        {
            var rst = Submit("Promotion", "GetPosPromotionByID", false, new object[] { pid }).Obj[0];
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPromotion(string promotionname)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Promotion", "GetPromotion", false, new object[] { promotionname, JsonHelper.Serialize(dts) });
            var kpidata = rst.Obj[0];
            return Json(kpidata, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvaiablePromotion()
        {
            var result = Submit("Promotion", "GetAvaiablePromotion", false, new object[] { Auth.DataGroupID.Value });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAvaiablePromotionForUnlimitedPage()
        {
            DatatablesParameter para = new DatatablesParameter();
            para.iDisplayLength = 0;
            para.iDisplayStart = 0;
            try
            {
                var result = Submit("Promotion", "GetAvaiablePromotion", false, new object[] { Auth.DataGroupID.Value });
                List<PromotionModel> list = JsonHelper.Deserialize<List<PromotionModel>>(JsonHelper.Serialize(result.Obj[0]));
                DatatablesSourceVsPage page = new DatatablesSourceVsPage();

                if (list != null && list.Count > 0)
                {
                    page.aaData = list;
                    page.iDisplayLength = list.Count;
                    page.iDisplayStart = 0;
                    page.iTotalRecords = list.Count;
                    para.iDisplayLength = list.Count;

                    return GenerateAssignedModelList<object>(page, para);
                }
                return DefaultNullModelList(para);
            }
            catch
            {
                return DefaultNullModelList(para);
            }
        }

        public JsonResult GetPromotionWithSysCommonByKeyForUnlimitedPage(string key, string type, string promotionIsEnd, bool isValidDate)
        {
            //DatatablesParameter para = new DatatablesParameter();
            //para.iDisplayLength = 0;
            //para.iDisplayStart = 0;
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Promotion", "GetPromotionWithSysCommonByKey", false, new object[] { key, type, promotionIsEnd, isValidDate, Auth.DataGroupID.Value, JsonHelper.Serialize(dts) });
            //List<PromotionModel> list = JsonHelper.Deserialize<List<PromotionModel>>(JsonHelper.Serialize(result.Obj[0]));
            //DatatablesSourceVsPage page = new DatatablesSourceVsPage();

            //if (list != null && list.Count > 0)
            //{
            //    page.aaData = list;
            //    page.iDisplayLength = list.Count;
            //    page.iDisplayStart = 0;
            //    page.iTotalRecords = list.Count;
            //    para.iDisplayLength = list.Count;

            //    return GenerateAssignedModelList<object>(page, para);
            //}
            //return DefaultNullModelList(para);
            return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPromotionWithSysCommonByKey(string key, string type)
        {
            var result = Submit("Promotion", "GetPromotionWithSysCommonByKey", false, new object[] { key, type, Auth.DataGroupID.Value });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPromotionWithSysCommonByPromotionID(long promotionID, string type)
        {
            var result = Submit("Promotion", "GetPromotionWithSysCommonByPromotionID", false, new object[] { promotionID, type, Auth.DataGroupID.Value });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAvaiablePromotionForPage()
        {
            var pageInfo = Request.CreateDataTableParameter();
            var result = Submit("Promotion", "GetAvaiablePromotionForPage", false, new object[] { Auth.DataGroupID.Value, JsonHelper.Serialize(pageInfo) });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(DefaultNullModelList(new DatatablesParameter()), JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetPromotions(long? baseDataID, string promotionID, string promotionCode, string promotionName, string promotionIsEnd,
            DateTime? startDate, DateTime? endDate)
        {
            var result = Submit("Promotion", "GetPromotions", false, new object[] { baseDataID, promotionID,  promotionCode,  promotionName,  promotionIsEnd,
                startDate,  endDate,Auth.DataGroupID.Value });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPromotionsForPage(long? baseDataID, string promotionID, string promotionCode, string promotionName, string promotionIsEnd,
            DateTime? startDate, DateTime? endDate)
        {
            var pageInfo = Request.CreateDataTableParameter();
            var result = Submit("Promotion", "GetPromotionsForPage", false, new object[] { baseDataID,promotionID,  promotionCode,  promotionName,  promotionIsEnd,
                startDate,  endDate,Auth.DataGroupID.Value, JsonHelper.Serialize(pageInfo) });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(DefaultNullModelList(new DatatablesParameter()), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSysCommonByKey(string key, string type)
        {
            var result = Submit("Promotion", "GetSysCommonByKey", false, new object[] { key, type, Auth.DataGroupID.Value });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSysCommonMarkByID(int actID)
        {
            var result = Submit("Promotion", "GetSysCommonMarkByID", false, new object[] { actID });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSysCommonBusPlanByID(string planID)
        {
            var result = Submit("Promotion", "GetSysCommonBusPlanByID", false, new object[] { planID });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSysCommonByPromotionID(long promotionID, string type)
        {
            var result = Submit("Promotion", "GetSysCommonByPromotionID", false, new object[] { promotionID, type, Auth.DataGroupID.Value });
            if (result.IsPass)
            {
                return Json(result.Obj[0], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InsertBactchPromotionSysCommon(string commonType, string sysCommonList, string key)
        {
            var result = Submit("Promotion", "InsertBactchPromotionSysCommon", false, new object[] { commonType, sysCommonList, key, Auth.DataGroupID.Value });
            return Json(result);
        }

        public JsonResult GetMemSubdivision(string subdivisionname)
        {
            var rst = Submit("Promotion", "GetMemSubdivision", false, new object[] { subdivisionname, Auth.DataGroupID });
            return Json(rst.Obj[0], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessPlan(string busplanname)
        {
            string user = Session[Utility.AppConst.SESSION_AUTH].ToString();
            var rst = Submit("Promotion", "GetBusinessPlan", false, new object[] { busplanname, Auth.DataGroupID });
            return Json(rst.Obj[0], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessPlanApproved(int promotionID)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Promotion", "GetBusinessPlanApproved", false, new object[] { promotionID, JsonHelper.Serialize(dts) });
            var kpidata = rst.Obj[0];
            return Json(kpidata, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarkApproved(int promotionID)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Promotion", "GetMarkApproved", false, new object[] { promotionID, JsonHelper.Serialize(dts) });
            var kpidata = rst.Obj[0];
            return Json(kpidata, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarkActive(string markactname)
        {
            string user = Session[Utility.AppConst.SESSION_AUTH].ToString();
            var rst = Submit("Promotion", "GetMarkActive", false, new object[] { markactname, Auth.DataGroupID });
            return Json(rst.Obj[0], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSysCommonChecked(string promotionID, string type)
        {
            var rst = Submit("Promotion", "GetSysCommonChecked", false, new object[] { int.Parse(promotionID), type });
            return Json(rst.Obj[0], JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSYSCommon(string promotionID, string sysCommons, string type)
        {
            var rst = Submit("Promotion", "SaveSYSCommon", false, new object[] { int.Parse(promotionID), sysCommons, type });
            return Json(rst.Obj[0], JsonRequestBehavior.AllowGet);
        }
    }
}
