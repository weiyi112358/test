using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class InventorySearchController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadBoxInfo(string BoxNo,string storeCode)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("InventorySearch", "LoadBoxInfo", false, new object[] { BoxNo, storeCode, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
    }

       
}

