using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.EF;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Auth.EnableDashBoard == true)
            {
                return View();
            }
            else
            {
                return Redirect(Auth.DefaultPath);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            ////区域 
            //var Areas = Submit("Service", "GetAreas", false);
            //ViewBag.Areas = Areas.Obj[0];
            ////品牌 
            //var Brands = Submit("Service", "GetBrands", false);
            //ViewBag.Brands = Brands.Obj[0];

            HttpContext.Session.Clear();
            return View((object)"请输入用户名和密码");
        }
        [HttpPost]
        public ActionResult GetStoreListByDataGroupID(string groupID)
        {
            var resutl = Submit("BaseData", "GetStoreListByGroupID", false, new object[] { groupID });
            if (resutl.IsPass)
            {
                return Json(resutl.Obj[0]);
            }
            return null;
        }
        [HttpPost]
        public ActionResult Login(FormCollection c)
        {
            //var groupType = c["rdGroupType"];
            //var groupID = c["selBrand"];
            //var storeCode = c["selStore"];
            //ViewBag.SelectItem = string.Format("{0},{1},{2}", groupType, groupID, storeCode);
            //var resutl = Submit("Service", "GetBrands", false);
            //if (resutl.IsPass)
            //{
            //    ViewBag.Brands = resutl.Obj[0];
            //}

            //if (groupType != "1" && string.IsNullOrWhiteSpace(groupID))
            //{
            //    return View((object)"区域/品牌 不能为空");
            //}
            //if (groupType == "3" && string.IsNullOrWhiteSpace(storeCode))
            //{
            //    return View((object)"门店 不能为空");
            //}
            //if (groupType == "1")
            //{
            //    groupID = "";
            //    storeCode = "";
            //}
            //else if (groupType == "2")
            //{
            //    storeCode = "";
            //}

            var rst = Submit("Auth", "CheckUser", true, c["Username"], c["Password"]);
            if (rst.IsPass)
            {
                return Redirect(Auth.DefaultPath);
            }
            else
            {
                return View((object)("error:" + rst.MSG));
            }
        }
        [HttpPost]
        public ActionResult LoginJS(string groupType, string groupID, string storeCode, string userName, string pass)
        {
            //var resutl = Submit("BaseData", "GetAllDataGroupList", false, null);
            //if (resutl.IsPass)
            //{
            //    ViewBag.groupList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TM_SYS_DataGroup>>(resutl.Obj[0].ToString());
            //}
            var resutl = Submit("Service", "GetBrands", false);
            if (resutl.IsPass)
            {
                ViewBag.Brands = resutl.Obj[0];
            }
            if (groupType != "1" && string.IsNullOrWhiteSpace(groupID))
            {
                return View((object)"平台/品牌 不能为空");
            }
            if (groupType == "3" && string.IsNullOrWhiteSpace(storeCode))
            {
                return View((object)"经销商 不能为空");
            }
            if (groupType == "1")
            {
                groupID = "";
                storeCode = "";
            }
            else if (groupType == "2")
            {
                storeCode = "";
            }
            var rst = Submit("Auth", "CheckUser", true, userName, pass, groupType, groupID, storeCode);
            //记录登陆日志
            if (rst.IsPass)
            {
                //return Redirect(Auth.DefaultPath);
                //Submit("Auth", "SaveLoginData", false, Auth.UserID, userName, Request.UserHostAddress, 1, "登陆成功");
            }
            else
            {
                //return View((object)("error:" + rst.MSG));
                //此时登陆不成功，找不到userID
                //Submit("Auth", "SaveLoginData", false, 0, userName, Request.UserHostAddress, 0, rst.MSG);
            }
            return Json(rst);
        }

        public JsonResult getOrderKPI(string KPIID)
        {
            var rst = Submit("Home", "getOrderKPI", false, new object[] { int.Parse(KPIID) });
            string kpiname = rst.Obj[0].ToString();
            string total = rst.Obj[1].ToString();
            var kpidata = rst.Obj[2];
            var tmep = new
            {
                kpiname,
                total,
                kpidata
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMemberIncrease(string KPIID)
        {
            var rst = Submit("Home", "GetMemberIncrease", false, new object[] { int.Parse(KPIID) });
            string kpiname = rst.Obj[0].ToString();
            var kpidata = rst.Obj[1];
            var tmep = new
            {
                kpiname,
                kpidata
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getNewCustomerKPI(string KPIID)
        {
            var rst = Submit("Home", "getNewCustomerKPI", false, new object[] { int.Parse(KPIID) });
            string kpiname = rst.Obj[0].ToString();
            var kpiactdata = rst.Obj[1];
            var kpilastdata = rst.Obj[2];
            var tmep = new
            {
                kpiname,
                kpiactdata,
                kpilastdata
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCustomerPriceKPI(string KPIID1, string KPIID2)
        {
            var rst = Submit("Home", "getCustomerPriceKPI", false, new object[] { int.Parse(KPIID1), int.Parse(KPIID2) });
            string kpiname = rst.Obj[0].ToString();
            string total = rst.Obj[1].ToString();
            var kpisaledata = rst.Obj[2];
            var kpiorderdata = rst.Obj[3];
            var tmep = new
            {
                kpiname,
                total,
                kpisaledata,
                kpiorderdata
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderNum(string KPIID)
        {
            var rst = Submit("Home", "GetOrderNum", false, new object[] { int.Parse(KPIID) });
            string kpiname = rst.Obj[0].ToString();
            string totalall = rst.Obj[1].ToString();
            var totalmonth = rst.Obj[2];
            var tmep = new
            {
                kpiname,
                totalall,
                totalmonth
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvgPrice(string KPIIDOrder, string KPIIDPrice)
        {
            var rst = Submit("Home", "GetAvgPrice", false, new object[] { int.Parse(KPIIDOrder), int.Parse(KPIIDPrice) });
            string kpiname = "平均客单价";
            string result = rst.Obj[0].ToString();
            var tmep = new
            {
                kpiname,
                result
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMemberActive(string KPIID)
        {
            var rst = Submit("Home", "GetMemberActive", false, new object[] { int.Parse(KPIID) });
            string kpiname = rst.Obj[0].ToString();
            string validNUm = rst.Obj[1].ToString();
            var tmep = new
            {
                kpiname,
                validNUm,
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKPIYearAVG(string KPIID)
        {
            var rst = Submit("Home", "GetKPIYearAVG", false, new object[] { int.Parse(KPIID) });
            string kpiname = rst.Obj[0].ToString();
            string validNUm = rst.Obj[1].ToString();
            var tmep = new
            {
                kpiname,
                validNUm,
            };
            return Json(tmep, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取销售占比
        /// </summary>
        /// <param name="KPIID"></param>
        /// <returns></returns>
        public JsonResult GetSalesKPI(string KPIID)
        {
            var rst = Submit("Home", "GetSalesKPI", false, new object[] { int.Parse(KPIID) });
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
