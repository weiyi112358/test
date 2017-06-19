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
    public class CardInfoSearchController : Controller
    {
        //
        // GET: /CardInfoSearch/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadCardInfo(string CardNo,string CardStatus,string StoreCode,string BoxNo,string agent)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("CardInfoSearch", "LoadCardInfo", false, new object[] { CardNo, CardStatus, StoreCode,BoxNo,agent,JsonHelper.Serialize(dts) });
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
    }
}
