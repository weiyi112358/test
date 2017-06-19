using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System.Data;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class MemberTransformController : Controller
    {
        //
        // GET: /MemberTransform/
        /// <summary>
        /// 卡转移页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TransformView()
        {
            return View();
        }
        /// <summary>
        /// 根据分公司获取门店
        /// </summary>
        /// <param name="company">分公司code</param>
        /// <returns></returns>
        public ActionResult GetStoreList(string company)
        {
            var result = Submit("MemberTransform", "GetStoreByCompany", false, new object[] { company });
            return Json(result.Obj[0]);
        }
        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="Corp">分公司code</param>
        /// <param name="RegisterStoreCode">门店code</param>
        /// <returns></returns>
        public ActionResult GetMemList(string Corp, string RegisterStoreCode)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("MemberTransform", "GetMemList", false, new object[] { Corp, RegisterStoreCode, JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }
        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="Corp">分公司code</param>
        /// <param name="RegisterStoreCode">门店code</param>
        /// <returns></returns>
        public ActionResult GetMemListID(string Corp, string RegisterStoreCode)
        {
            var result = Submit("MemberTransform", "GetMemListID", false, new object[] { Corp, RegisterStoreCode});
            return Json(result.Obj[0]);
        }
        /// <summary>
        /// 卡转移
        /// </summary>
        /// <param name="Corp">分公司code</param>
        /// <param name="RegisterStoreCode">门店code</param>
        /// <param name="MemId">会员ID拼接字符串【,】号拼接，以【,】结尾</param>
        /// <returns></returns>
        public ActionResult SaveTransform(string Corp, string RegisterStoreCode,string MemId)
        {
            Corp = HttpUtility.HtmlDecode(Corp);
            RegisterStoreCode = HttpUtility.HtmlDecode(RegisterStoreCode);
            MemId = HttpUtility.HtmlDecode(MemId);
            var result = Submit("MemberTransform", "SaveTransform", false, new object[] { Corp, RegisterStoreCode, MemId,Auth.UserID });
            return Json(result);
        }
        /// <summary>
        /// 手动导入卡转移列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImportMemExcel()
        {
            try
            {
                var file = Request.Files[0];
                int FileLen = file.ContentLength;
                byte[] input = new byte[FileLen];
                System.IO.Stream MyStream = file.InputStream;
                MyStream.Read(input, 0, FileLen);
                DataTable dt = ExcelHelper.ExcelToDataTable(MyStream);
                string memids = "";
                foreach (DataRow r in dt.Rows)
                {
                    if (r["会员ID"] != null)
                    {
                        if (!string.IsNullOrEmpty(r["会员ID"].ToString()))
                        {
                            memids += r["会员ID"].ToString().Trim() + ",";
                        }
                    }
                }

                return memids;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
