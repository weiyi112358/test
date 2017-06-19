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
    public class CouponUseRuleController : Controller
    {
        #region 购物券使用规则
        
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit()
        {
            string ID = Request.Form["hideID"] != null ? Request.Form["hideID"].ToString() : "";
            string Info = Request.Form["hideInfo"] != null ? Request.Form["hideInfo"].ToString() : "";
            ViewBag.ID = ID;
            ViewBag.Info = Info;
            return View();       
        }

        /// <summary>
        /// 加载购物券列表
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <param name="CouponsType"></param>
        /// <returns></returns>
        public ActionResult LoadCouponUseRule(string strmodel)
        {
            var dts = Request.CreateDataTableParameter();
            CouponUseRuleModel model = JsonHelper.Deserialize<CouponUseRuleModel>(strmodel);
            var result = Submit("CouponUseRule", "LoadCouponUseRule", false, new object[] { model, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 添加购物券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddCoupon(CouponUseRuleModel model)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "AddCoupon", false, new object[] { model, User });
            return Json(result);
        }
        /// <summary>
        /// 根据Id获取购物券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCouponsById(int ID)
        {
            var result = Submit("CouponUseRule", "GetCouponById", false, new object[] { ID });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 编辑购物券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateCoupon(CouponUseRuleModel model)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "UpdateCoupon", false, new object[] { model, User });
            return Json(result);
        }
        /// <summary>
        /// 改变购物券的执行状态
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ChangeCouponStatus(int Id)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "ChangeCouponStatus", false, new object[] { Id, User });
            return Json(result);
        }
        /// <summary>
        /// 删除购物券
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteCouponById(int Id)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "DeleteCouponById", false, new object[] { Id, User });
            return Json(result);
        }
        /// <summary>
        /// 改变购物券的审核状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public ActionResult ApproveCouponById(int Id, string active)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "ApproveCouponById", false, new object[] { Id, active,User });
            return Json(result);       
        }

        #endregion

        #region 购物券派送规则


        public ActionResult SendIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEdit()
        {
            string ID = Request.Form["hideID"] != null ? Request.Form["hideID"].ToString() : "";
            string Info = Request.Form["hideInfo"] != null ? Request.Form["hideInfo"].ToString() : "";
            ViewBag.ID = ID;
            ViewBag.Info = Info;
            return View();
        }

        /// <summary>
        /// 加载购物券列表
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <param name="CouponsType"></param>
        /// <returns></returns>
        public ActionResult LoadCouponSendRule(string strmodel)
        {
            var dts = Request.CreateDataTableParameter();
            CouponSendRuleModel model = JsonHelper.Deserialize<CouponSendRuleModel>(strmodel);
            var result = Submit("CouponUseRule", "LoadCouponSendRule", false, new object[] { model, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 添加购物券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddCouponSendRule(CouponSendRuleModel model)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "AddCouponSendRule", false, new object[] { model, User });
            return Json(result);
        }
        /// <summary>
        /// 根据Id获取购物券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCouponSendRuleById(int ID)
        {
            var result = Submit("CouponUseRule", "GetCouponSendRuleById", false, new object[] { ID });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 编辑购物券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateCouponSendRule(CouponSendRuleModel model)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "UpdateCouponSendRule", false, new object[] { model, User });
            return Json(result);
        }
        /// <summary>
        /// 改变购物券的审核状态
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ChangeCouponSendRuleStatus(int Id)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "ChangeCouponSendRuleStatus", false, new object[] { Id, User });
            return Json(result);
        }
        /// <summary>
        /// 删除购物券
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteCouponSendRuleById(int Id)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "DeleteCouponSendRuleById", false, new object[] { Id, User });
            return Json(result);
        }
        /// <summary>
        /// 改变购物券的审核状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public ActionResult ApproveCouponSendById(int Id, string active)
        {
            var User = Auth.UserID;
            var result = Submit("CouponUseRule", "ApproveCouponSendById", false, new object[] { Id, active, User });
            return Json(result);
        }

        #endregion

        #region 公共方法
        
        
        /// <summary>
        /// 加载产品列表
        /// </summary>
        /// <param name="productArr"></param>
        /// <returns></returns>
        public ActionResult LoadProduct(string productArr)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("CouponUseRule", "LoadProduct", false, new object[] { productArr, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
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
                string goodscode = "";
                foreach (DataRow r in dt.Rows)
                {
                    if (r["产品代码"] != null)
                    {
                        if (!string.IsNullOrEmpty(r["产品代码"].ToString()))
                        {
                            goodscode += r["产品代码"].ToString().Trim() + ",";
                        }
                    }
                }
                return goodscode;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 判断该产品代码是否存在
        /// </summary>
        /// <param name="GoodsCode"></param>
        /// <returns></returns>
        public ActionResult CheckGoodsCode(string GoodsCode)
        {
            var result = Submit("CouponUseRule", "CheckGoodsCode",false,new object[]{GoodsCode});
            return Json(result);        
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UploadNewsFile(string uploadType)
        {
            var file = Request.Files[0];
            string filePath = string.Empty;
            string path = FileUploadHelper.UpLoadFile(uploadType, file, ref filePath);
            return filePath;
        }
        /// <summary>
        /// 加载商品——弹窗
        /// </summary>
        /// <param name="productArr"></param>
        /// <returns></returns>
        public ActionResult LoadProducts(string Code,string Name)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("CouponUseRule", "LoadProducts", false, new object[] { Code,Name, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        #endregion
    }
}
