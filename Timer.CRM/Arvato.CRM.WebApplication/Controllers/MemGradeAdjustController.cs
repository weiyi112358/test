using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility.Datatables;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class MemGradeAdjustController : Arvato.CRM.WebApplication.Controller
    {
        //
        // GET: /MemGradeAdjust/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMemGradeAdjust(string vipCode, string name, string mobileNO, string plateNO)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("MemGradeAdjust", "GetMemGradeAdjust", false, new object[] { Auth.DataGroupID ?? 0, JsonHelper.Serialize(Auth.RoleIDs), JsonHelper.Serialize(dts), vipCode, name, mobileNO, plateNO });
            return Json(rst.Obj[0].ToString());
        }

        [HttpPost]
        public JsonResult GetAllVipType()
        {
            var rst = Submit("Service", "GetAllVipType", false, new object[] { Auth.DataGroupID ?? 0 });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult GetItemById(string itemId)
        {
            var rst = Submit("MemGradeAdjust", "GetItemById", false, new object[] { itemId });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult UpdateVipLevelData(string befGrade, DateTime? befStartDate, DateTime? befEndDate,string vipCode, string vipLevel, string updateUser, DateTime? startDate, DateTime? endDate, string reason)
        {
            var rst = Submit("MemGradeAdjust", "UpdateVipLevelData", false, new object[] { befGrade, befStartDate, befEndDate, vipCode, vipLevel, updateUser, startDate, endDate, reason });
            return Json(rst);
        }
    }
}
