using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class CardLossAndSolutionController : Controller
    {
        //
        // GET: /CardLossAndSolution/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取卡的信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetCardInfo(string type, string id)
        {
            var result = Submit("CardLossAndSolution", "GetCardInfo", false, new object[] { type, id });
            string name = result.Obj[0].ToString();
            string IDNo = result.Obj[1].ToString();
            var phone = result.Obj[2];
            var tmep = new
            {
                name,
                IDNo,
                phone
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CardPrepare(FormCollection c)
        {

            return View();
        }
        [HttpPost]
        public ActionResult CardChange(FormCollection c)
        {

            return View();
        }
    }
}
