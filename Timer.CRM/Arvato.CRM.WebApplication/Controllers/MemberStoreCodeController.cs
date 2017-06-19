using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Utility;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class MemberStoreCodeController : Controller
    {
        //
        // GET: /StoreCode/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStoreCodeMembers(string memberName, string stroeName, DateTime? startTime, DateTime? endTime)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("MemberStoreCode", "GetStoreCodeMembers", false, new object[] { JsonHelper.Serialize(dts), Auth.DataGroupID, memberName, stroeName, startTime, endTime });
            return Json(rst.Obj[0].ToString());
        }

    }
}
