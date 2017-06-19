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
    public class BatchOperationController : Controller
    {
        //
        // GET: /BranchOperation/

        public ActionResult BatchOperationList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BatchOperationAdd()
        {
            string ID = Request.Form["hideID"] != null ? Request.Form["hideID"].ToString() : "";
            string Info = Request.Form["hideInfo"] != null ? Request.Form["hideInfo"].ToString() : "";
            ViewBag.ID = ID;
            ViewBag.Info = Info;
            return View();
        }

        /// <summary>
        /// 加载批量操作列表
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <param name="CouponsType"></param>
        /// <returns></returns>
        public ActionResult LoadOperationList(string strmodel)
        {
            var dts = Request.CreateDataTableParameter();
            BatchOperationModel model = JsonHelper.Deserialize<BatchOperationModel>(strmodel);
            var result = Submit("BatchOperation", "LoadOperationList", false, new object[] { model, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 加载筛选条件列表
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        public ActionResult LoadWhere(string Array)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BatchOperation", "LoadWhere", false, new object[] { Array, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 加载卡列表
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <param name="CouponsType"></param>
        /// <returns></returns>
        public ActionResult LoadCardList(string Array, string OperationType)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BatchOperation", "LoadCardList", false, new object[] { Array,OperationType, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 添加批量操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddOperation(BatchOperationModel model)
        {
            var User = Auth.UserID;
            var result = Submit("BatchOperation", "AddOperation", false, new object[] { model, User });
            return Json(result);
        }

        /// <summary>
        /// 编辑批量操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateOperation(BatchOperationModel model)
        {
            var User = Auth.UserID;
            var result = Submit("BatchOperation", "UpdateOperation", false, new object[] { model, User });
            return Json(result);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DeleteOperationById(string Id)
        {
            var User = Auth.UserID;
            var result = Submit("BatchOperation", "DeleteOperationById", false, new object[] { Id, User });
            return Json(result);
        }

        /// <summary>
        /// 根据Id获取批量操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetOperationById(string ID)
        {
            var result = Submit("BatchOperation", "GetOperationById", false, new object[] { ID });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 改变批量操作的审核状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public ActionResult ApproveOperationById(string Id, string active)
        {
            var User = Auth.UserID;
            var result = Submit("BatchOperation", "ApproveOperationById", false, new object[] { Id, active, User });
            return Json(result);
        }
        /// <summary>
        /// 加载查看卡列表
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <param name="CouponsType"></param>
        /// <returns></returns>
        public ActionResult LoadCardListById(string ID)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BatchOperation", "LoadCardListById", false, new object[] { ID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取卡类型
        /// </summary>
        /// <param name="company">分公司code</param>
        /// <returns></returns>
        public ActionResult GetCardType()
        {
            var result = Submit("BatchOperation", "GetCardType", false, new object[] {  });
            return Json(result.Obj[0]);
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

                var pointLists = new List<string>();
                foreach (DataRow r in dt.Rows)
                {
                    if (r["账户调整卡号"] != null)
                    {
                        if (!string.IsNullOrEmpty(r["账户调整卡号"].ToString()))
                        {

                            pointLists.Add(r["账户调整卡号"].ToString() + "," + r["调整数"].ToString());
                        }
                    }
                }

                var result = Submit("Member360", "BatchImportPoint", false, new object[] { JsonHelper.Serialize(pointLists), Auth.UserID });

                return "";
            }
            catch (Exception)
            {

                return "";
            }

        }



        #region 批量操作
        public long ImportExcelBatch(System.Web.HttpPostedFileBase file)
        {
            try
            {
                int FileLen = file.ContentLength;
                byte[] input = new byte[FileLen];
                System.IO.Stream MyStream = file.InputStream;
                MyStream.Read(input, 0, FileLen);
                DataTable dt = ExcelHelper.ExcelToDataTable(MyStream);

                List<string> operation = new List<string>();
                foreach (DataRow r in dt.Rows)
                {
                    string msg = string.Format("{0},{1},{2}", r["卡号"].ToString(), r["门店编码"].ToString(), r["门店名称"].ToString());
                    operation.Add(msg);
                }
                var result = Submit("BatchOperation", "ImportExcelBatch", false, new object[] { JsonHelper.Serialize(operation), Auth.UserID });
                return 1;
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("ImportExcelBatch ：" + ex.ToString());
                throw;
            }
        }       
        #endregion
    }
}