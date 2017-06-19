using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Arvato.CRM.EF;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class CouponSendController : Controller
    {
        //
        // GET: /CouponSend/

        public ActionResult CouponSendList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add()
        {
            string ID = Request.Form["hideID"] != null ? Request.Form["hideID"].ToString() : "";
            string Info = Request.Form["hideInfo"] != null ? Request.Form["hideInfo"].ToString() : "";
            ViewBag.ID = ID;
            ViewBag.Info = Info;
            return View();
        }

        /// <summary>
        /// 加载购物券派送列表
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <param name="CouponsType"></param>
        /// <returns></returns>
        public ActionResult LoadCouponSendList(string strmodel)
        {
            var dts = Request.CreateDataTableParameter();
            CouponListModel model = JsonHelper.Deserialize<CouponListModel>(strmodel);
            var result = Submit("CouponSend", "LoadCouponSend", false, new object[] { model, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 改变购物券派送的审核状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public ActionResult ApproveCouponSendById(string Id, string active)
        {
            var User = Auth.UserID;     
            string path = "/Upload/TXT";
            string mapPath = System.Web.HttpContext.Current.Server.MapPath(path);
            var result = Submit("CouponSend", "ApproveCouponSendById", false, new object[] { Id, active, User, mapPath });
            return Json(result);
        }

        /// <summary>
        /// 删除购物券派送
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteCouponSendById(string Id)
        {
            var User = Auth.UserID;
            var result = Submit("CouponSend", "DeleteCouponSendById", false, new object[] { Id, User });
            return Json(result);
        }

        /// <summary>
        /// 加载门店列表
        /// </summary>
        /// <param name="storeArr">门店参数</param>
        /// <returns></returns>
        public ActionResult LoadStoreList(string storeArr)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("CouponSend", "LoadStoreList", false, new object[] { storeArr, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 门店列表——弹窗
        /// </summary>
        /// <param name="storeArr">门店参数</param>
        /// <returns></returns>
        public ActionResult LoadStores(string Code, string Name,string codelist)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("CouponSend", "LoadStores", false, new object[] { Code, Name,codelist, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据Id获取购物券派送
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCouponSendById(string ID)
        {
            var result = Submit("CouponSend", "GetCouponSendById", false, new object[] { ID });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 加载购物券列表
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <param name="CouponsType"></param>
        /// <returns></returns>
        public ActionResult LoadCoupon( string CouponName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("CouponSend", "LoadCoupon", false, new object[] { CouponName, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据Id获取购物券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCouponsById(int ID)
        {
            var result = Submit("CouponSend", "GetCouponById", false, new object[] { ID });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 添加购物券派送
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddCouponSend(CouponListModel model)
        {
            var User = Auth.UserID;
            var result = Submit("CouponSend", "AddCouponSend", false, new object[] { model, User });
            return Json(result);
        }

        /// <summary>
        /// 编辑购物券派送
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateCouponSend(CouponListModel model)
        {
            var User = Auth.UserID;
            var result = Submit("CouponSend", "UpdateCouponSend", false, new object[] { model, User });
            return Json(result);
        }

        /// <summary>
        /// 导入EXCEL
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImportExcel()
        {

            try
            {
                var file = Request.Files[0];
                int FileLen = file.ContentLength;
                byte[] input = new byte[FileLen];
                System.IO.Stream MyStream = file.InputStream;
                MyStream.Read(input, 0, FileLen);
                DataTable dt = ExcelHelper.ExcelToDataTable(MyStream);
                
                        var codeStr = "";
                        foreach (DataRow r in dt.Rows)
                        {
                            if (r["门店代码"] != null)
                            {
                                if (!string.IsNullOrEmpty(r["门店代码"].ToString()))
                                {

                                    codeStr += r["门店代码"].ToString().Trim() + ",";
                                }
                            }
                        }
                        return codeStr;
                }
                catch (Exception)
                {

                    return  "";
                }
         
        }
    }
}
