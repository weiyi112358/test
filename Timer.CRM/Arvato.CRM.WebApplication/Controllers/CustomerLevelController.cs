using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Model;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class CustomerLevelController : Controller
    {
        //
        // GET: /CustomerLevel/
        public ActionResult Index()
        {
            var rst = Submit("CustomerLevel", "GetCustomersLevelList", false, Auth.DataGroupID);

            var bizOptionModelList = JsonHelper.Deserialize<List<BizOptionModel>>(rst.Obj[0].ToString());
            var selectItemList = bizOptionModelList.Select(bizOptionModel => new SelectListItem { Text = bizOptionModel.OptionText, Value = bizOptionModel.OptionValue}).ToList();
            ViewBag.CustomerLevel = selectItemList;

            var rst1 = Submit("Service", "GetBrands", false);
            var brandList = JsonHelper.Deserialize<List<BizOptionModel>>(rst1.Obj[0].ToString());
            var selectBrandItemList = brandList.Select(bizOptionModel => new SelectListItem { Text = bizOptionModel.OptionText, Value = bizOptionModel.OptionValue}).ToList();
            ViewBag.Brand = selectBrandItemList;

            return View();
        }

        public JsonResult GetBizOptionCustomersLevel(string customerLevel) {
            var rst = Submit("CustomerLevel", "GetCustomersLevelList", false, Auth.DataGroupID);
            return Json(rst.Obj[0].ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerLevels(string customerLevel, string brand)
        {
            var dataParamers = Request.CreateDataTableParameter();
            var rst = Submit("CustomerLevel", "GetCustomersLevels", false, new object[] { JsonHelper.Serialize(dataParamers), customerLevel,brand, Auth.DataGroupID });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult EditCustomerLevel(CustomerLevelModel customLevel)
        {
            var rst = Submit("CustomerLevel", "EditCustomerLevel", false, new object[] { customLevel, Auth.UserID.ToString(), Auth.DataGroupID });
            return Json(rst);
        }

        public JsonResult GetCustomerLevelById(int customerLevelId)
        {
            var rst = Submit("CustomerLevel", "GetCustomerLevelById", false, customerLevelId);
            return Json(rst.Obj[0]);
        }

        public JsonResult DeleteCustomerLevelById(int customerLevelId)
        {
            var rst = Submit("CustomerLevel", "DeleteCustomerLevelById", false, customerLevelId);
            return Json(rst);
        }

    }
}
