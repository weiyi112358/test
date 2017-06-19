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
    //公共服务控制器
    public class ServiceController : Controller
    {
        //
        // GET: /Service/

        /// <summary>
        /// 根据记录ID和类型查询修改历史
        /// </summary>
        /// <param name="mainId">记录ID</param>
        /// <param name="mainType">记录类型</param>
        /// <returns></returns>
        public JsonResult GetLogsByMainIdAndType(string mainId, string mainType, DateTime? start, DateTime? end)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Service", "GetLogsByMainIdAndType", false, new object[] { JsonHelper.Serialize(dts), mainId, mainType, start, end });
            return Json(info.Obj[0]);
        }
    }
}
