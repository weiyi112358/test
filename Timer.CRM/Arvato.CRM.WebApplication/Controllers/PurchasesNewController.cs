using Arvato.CRM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Model;
using System.IO;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class PurchasesNewController : Controller
    {
        //
        // GET: /PurchasesNew/

        #region 定卡

        public ActionResult CustomizeNew()
        {
            return View();
        }

        /// <summary>
        /// 定卡列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="agent"></param>
        /// <param name="boxNumIn"></param>
        /// <param name="createTime"></param>
        /// <param name="CardNumIn"></param>
        /// <returns></returns>
        public JsonResult CustomizeCardList(string oddIdNo, string status, string agent, string boxNumIn, string cardNumIn, string createTime, string isRetrieve)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("PurchasesNew", "CustomizeCardList", false, new object[] { oddIdNo, status, agent, createTime, boxNumIn, cardNumIn, isRetrieve,JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载供应商
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadAgent()
        {
            var result = Submit("PurchasesNew", "LoadAgent", false, new object[] { });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载分公司
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadCompany()
        {
            var result = Submit("PurchasesNew", "LoadCompany", false, new object[] { });
            return Json(result.Obj[0]);
        }

     
        public JsonResult LoadStore(string storeType)
        {
            var result = Submit("PurchasesNew", "LoadStore", false, new object[] { storeType });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载卡类型
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadCardType()
        {
            var result = Submit("PurchasesNew", "LoadCardType", false, new object[] { });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载下拉框
        /// </summary>
        /// <param name="optionType"></param>
        /// <returns></returns>
        public JsonResult LoadBizOption(string optionType)
        {
            var result = Submit("PurchasesNew", "LoadBizOption", false, new object[] { optionType });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 新增页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomizeAddNew()
        {
            return View();
        }

        /// <summary>
        /// 盒号页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateBoxNo()
        {
            return View();
        }

        /// <summary>
        /// 盒号列表
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="status"></param>
        /// <param name="modifyby"></param>
        /// <returns></returns>
        public JsonResult BoxNoList(string boxNo, string status, string modifyby)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("PurchasesNew", "BoxNoList", false, new object[] { boxNo, status, modifyby, JsonHelper.Serialize(pageParamers), Auth.UserID });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 新增盒号
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public JsonResult AddBoxNo(string jsonParam)
        {
            var result = Submit("PurchasesNew", "AddBoxNo", false, new object[] { jsonParam, Auth.UserID });
            return Json(result);
        }

        /// <summary>
        /// 查询空盒
        /// </summary>
        /// <param name="boxNoInput"></param>
        /// <returns></returns>
        public JsonResult GetEmptyBoxNo(string boxNoInput)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("PurchasesNew", "GetEmptyBoxNo", false, new object[] { boxNoInput, Auth.UserID, JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 新增定卡
        /// </summary>
        /// <returns></returns>
        public JsonResult AddCustomize(string jsonParams, string status)
        {        
            var result = Submit("PurchasesNew", "AddCustomize", false, new object[] { jsonParams, status, Auth.UserID });
            return Json(result);
        }

        /// <summary>
        /// 自动获得卡号
        /// </summary>
        /// <param name="cardNum"></param>
        /// <returns></returns>
        public JsonResult GetAutoCard(string cardNum,string provinceCode,string mathRule)
        {
            var result = Submit("PurchasesNew", "GetAutoCard", false, new object[] { cardNum, provinceCode, mathRule });
            return Json(result);
        }

        /// <summary>
        /// 手动获得卡号
        /// </summary>
        /// <param name="beginCardNo"></param>
        /// <param name="cardNum"></param>
        /// <returns></returns>
        public JsonResult GetManualCard(string beginCardNo, string cardNum, string provinceCode, string mathRule)
        {
            var result = Submit("PurchasesNew", "GetManualCard", false, new object[] { beginCardNo, cardNum, provinceCode, mathRule });
            return Json(result);
        }

        /// <summary>
        /// 详情页基本信息
        /// </summary>
        /// <param name="customizeOddId"></param>
        /// <returns></returns>
        public JsonResult CustomizeDetailsParams(string customizeOddId)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("PurchasesNew", "CustomizeDetailsParams", false, new object[] { customizeOddId, JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="customizeOddId"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult CustomizeToExcel(string customizeOddId)
        {
            var result = Submit("PurchasesNew", "CustomizeToExcel", false, new object[] { customizeOddId });         
             var customizeList = JsonHelper.Deserialize<List<CustomizeToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<CustomizeToExcel>> excelColumnFormats = new List<ExcelColumnFormat<CustomizeToExcel>>{
                 new ExcelColumnFormat<CustomizeToExcel>{ColumnName="序号", FuncGetValue=p=>p.Sequence},          
                    new ExcelColumnFormat<CustomizeToExcel>{ColumnName="盒号", FuncGetValue=p=>p.BoxNo},
                   new ExcelColumnFormat<CustomizeToExcel>{ColumnName="起始卡号", FuncGetValue=p=>p.BeginCardNo},
                   new ExcelColumnFormat<CustomizeToExcel>{ColumnName="截止卡号", FuncGetValue=p=>p.EndCardNo}             
            };
            var wordBook = ExcelHelper.DataToExcel(customizeList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "定卡详情导出" + DateTime.Now.ToShortDateString() + ".xls");
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="customizeOddId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult ChangeStatus(string customizeOddId, string status)
        {
            var result = Submit("PurchasesNew", "ChangeStatus", false, new object[] { customizeOddId,status });
            return Json(result);
        }

        /// <summary>
        /// 定卡列表导出Excel
        /// </summary>
        /// <param name="txtExcelOddIdNo"></param>
        /// <param name="txtExcelAgent"></param>
        /// <param name="txtExcelStatus"></param>
        /// <param name="txtExcelBoxNumIn"></param>
        /// <param name="txtExcelCardNumIn"></param>
        /// <param name="txtExcelCreateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult CustomizeListToExcel(string txtExcelOddIdNo, string txtExcelAgent, string txtExcelStatus, string txtExcelBoxNumIn, string txtExcelCardNumIn, string txtExcelCreateTime, string txtExcelIsRetrieve)
        {
            var result = Submit("PurchasesNew", "CustomizeListToExcel", false, new object[] { txtExcelOddIdNo, txtExcelAgent, txtExcelStatus, txtExcelBoxNumIn, txtExcelCardNumIn, txtExcelCreateTime, txtExcelIsRetrieve });
            var applyList = JsonHelper.Deserialize<List<CustomizeListToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<CustomizeListToExcel>> excelColumnFormats = new List<ExcelColumnFormat<CustomizeListToExcel>>{
                 new ExcelColumnFormat<CustomizeListToExcel>{ColumnName="单号", FuncGetValue=p=>p.OddIdNo},          
                    new ExcelColumnFormat<CustomizeListToExcel>{ColumnName="状态", FuncGetValue=p=>p.StatusName},
                   new ExcelColumnFormat<CustomizeListToExcel>{ColumnName="供应商", FuncGetValue=p=>p.Agent},
                   new ExcelColumnFormat<CustomizeListToExcel>{ColumnName="接收单位", FuncGetValue=p=>p.AcceptingUnit},
                     new ExcelColumnFormat<CustomizeListToExcel>{ColumnName="卡数量", FuncGetValue=p=>p.CardNumIn},
                         new ExcelColumnFormat<CustomizeListToExcel>{ColumnName="盒数量", FuncGetValue=p=>p.BoxNumIn},              
                     new ExcelColumnFormat<CustomizeListToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime},             
            };
            var wordBook = ExcelHelper.DataToExcel(applyList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "定卡列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }
        #endregion


        #region 收卡
         public ActionResult RetrieveNew()
         {
             return View();
         }

        /// <summary>
        /// 收卡列表
        /// </summary>
        /// <param name="oddIdNo"></param>
        /// <param name="status"></param>
        /// <param name="agent"></param>
        /// <param name="boxNumIn"></param>
        /// <param name="createTime"></param>
        /// <returns></returns>
         public JsonResult RetrieveCardList(string retrieveOddIdNo, string customizeOddIdNo, string status, string agent, string createTime, string reserveBoxNumber)
         {
             var pageParamers = Request.CreateDataTableParameter();
             var result = Submit("PurchasesNew", "RetrieveCardList", false, new object[] { retrieveOddIdNo, customizeOddIdNo, status, agent, createTime, reserveBoxNumber, JsonHelper.Serialize(pageParamers) });
             return Json(result.Obj[0]);
         }

        
         [HttpPost]
         public ActionResult RetrieveAddNew()
         {
             return View();
         }

        /// <summary>
        /// 加载定卡单号
        /// </summary>
        /// <returns></returns>
         public JsonResult loadCustomizeOddIdNo()
         {
             var result = Submit("PurchasesNew", "loadCustomizeOddIdNo", false, new object[] {});
             return Json(result.Obj[0]);
         }

        /// <summary>
        /// 新增收卡
        /// </summary>
        /// <param name="customizeOddId"></param>
        /// <returns></returns>
         public JsonResult AddRetrieve(string customizeOddId,string status)
         {
             var result = Submit("PurchasesNew", "AddRetrieve", false, new object[] { customizeOddId,status,Auth.UserID });
             return Json(result);
         }

        /// <summary>
        /// 收卡详情
        /// </summary>
        /// <param name="customizeOddId"></param>
        /// <returns></returns>
         public JsonResult RetrieveDetailsParams(string retrieveOddId)
         {
             var pageParamers = Request.CreateDataTableParameter();
             var result = Submit("PurchasesNew", "RetrieveDetailsParams", false, new object[] { retrieveOddId, JsonHelper.Serialize(pageParamers) });
             return Json(result.Obj[0]);
         }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="retrieveOddId"></param>
        /// <returns></returns>
         public JsonResult ChangeStatusRetrieve(string retrieveOddId,string status)
         {
             var result = Submit("PurchasesNew", "ChangeStatusRetrieve", false, new object[] { retrieveOddId,status });
             return Json(result);
         }

         /// <summary>
         /// 收卡列表导出Excel
         /// </summary>
         /// <param name="txtExcelRetrieveOddIdNo"></param>
         /// <param name="txtExcelAgent"></param>
         /// <param name="txtExcelStatus"></param>
         /// <param name="txtExcelCustomizeOddIdNo"></param>
         /// <param name="txtExcelReserveBox"></param>
         /// <param name="txtExcelCreateTime"></param>
         /// <returns></returns>
         [HttpPost]
         public FileResult RetrieveListToExcel(string txtExcelRetrieveOddIdNo, string txtExcelAgent, string txtExcelStatus, string txtExcelCustomizeOddIdNo, string txtExcelReserveBox, string txtExcelCreateTime)
         {
             var result = Submit("PurchasesNew", "RetrieveListToExcel", false, new object[] { txtExcelRetrieveOddIdNo, txtExcelAgent, txtExcelStatus, txtExcelCustomizeOddIdNo, txtExcelReserveBox, txtExcelCreateTime });
             var applyList = JsonHelper.Deserialize<List<RetrieveListToExcel>>(result.Obj[0].ToString());
             List<ExcelColumnFormat<RetrieveListToExcel>> excelColumnFormats = new List<ExcelColumnFormat<RetrieveListToExcel>>{
                  new ExcelColumnFormat<RetrieveListToExcel>{ColumnName="收卡单号", FuncGetValue=p=>p.RetrieveOddIdNo},          
                  new ExcelColumnFormat<RetrieveListToExcel>{ColumnName="状态", FuncGetValue=p=>p.StatusName},
                  new ExcelColumnFormat<RetrieveListToExcel>{ColumnName="供应商", FuncGetValue=p=>p.AgentName},
                  new ExcelColumnFormat<RetrieveListToExcel>{ColumnName="定卡单号", FuncGetValue=p=>p.CustomizeOddIdNo},
                  new ExcelColumnFormat<RetrieveListToExcel>{ColumnName="收盒数量", FuncGetValue=p=>p.ReserveBoxNumber},    
                    new ExcelColumnFormat<RetrieveListToExcel>{ColumnName="收卡数量", FuncGetValue=p=>p.ReserveCardNumber},                         
                      new ExcelColumnFormat<RetrieveListToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime},             
            };
             var wordBook = ExcelHelper.DataToExcel(applyList, excelColumnFormats);
             var stream = ExcelHelper.GetStream(wordBook);
             stream.Seek(0, SeekOrigin.Begin);
             return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "收卡列表导出" + DateTime.Now.ToShortDateString() + ".xls");
         }
        #endregion
    }
}
