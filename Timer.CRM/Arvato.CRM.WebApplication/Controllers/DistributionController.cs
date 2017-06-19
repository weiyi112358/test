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
using System.Data.Objects.SqlClient;
using System.IO;


namespace Arvato.CRM.WebApplication.Controllers
{
    public class DistributionController : Controller
    {

        #region 卡申请
        //
        // GET: /Distribution/   
        public ActionResult ApplyCard()
        {
            return View();        
        }


        /// <summary>
        /// 条件查询卡片
        /// </summary>
        /// <param name="Card"></param>
        /// <returns></returns>
        public JsonResult GetCardList(string id, string applyNumber, string deliverNumber, string approveNumber, string status, string executeStatus, string modifyTime, string page, int? dataGroupID = null)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "ApplyCard", false, new object[] { id, applyNumber, deliverNumber, approveNumber, status, executeStatus, modifyTime, JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载门店
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadApplyStore()
        {
            var result = Submit("Distribution", "LoadApplyStore", false, new object[] { });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载卡类型
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadCardType()
        { 
            var result = Submit("Distribution", "LoadCardType", false, new object[] {});
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 加载用途
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadPurpose()
        {
            var result = Submit("Distribution", "LoadPurpose", false, new object[] {});
            return Json(result.Obj[0]);
        }
    
        /// <summary>
        /// 添加卡组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult AddCard(string param)
        {       
            var result = Submit("Distribution", "AddCard", false, new object[] { param, Auth.UserID});
            return Json(result);
        }

        /// <summary>
        /// 条件查询导出Excel
        /// </summary>
        /// <param name="txtExcelOddNumbers"></param>
        /// <param name="txtExcelExecuteStatus"></param>
        /// <param name="txtExcelStatus"></param>
        /// <param name="txtExcelApplyNumber"></param>
        /// <param name="txtExcelApproveNumber"></param>
        /// <param name="txtExcelDeliverNumber"></param>
        /// <param name="txtExcelCreateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult ApplyCardToExcel(string txtExcelOddNumbers, string txtExcelExecuteStatus, string txtExcelStatus, string txtExcelApplyNumber, string txtExcelApproveNumber, string txtExcelDeliverNumber, string txtExcelCreateTime)
        {
            var result = Submit("Distribution", "ApplyCardToExcel", false, new object[] { txtExcelOddNumbers, txtExcelExecuteStatus, txtExcelStatus, txtExcelApplyNumber, txtExcelApproveNumber, txtExcelDeliverNumber, txtExcelCreateTime, });
            var applyList = JsonHelper.Deserialize<List<ApplyCardToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<ApplyCardToExcel>> excelColumnFormats = new List<ExcelColumnFormat<ApplyCardToExcel>>{
                 new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="单号", FuncGetValue=p=>p.OddIdNo},                       
                   new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="状态", FuncGetValue=p=>p.Status},
                   //new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="接收单位", FuncGetValue=p=>p.AcceptingUnit},
                          new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="所属代理商", FuncGetValue=p=>p.Channel},
                     new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="申请数量", FuncGetValue=p=>p.ApplyNumber},
                     new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="批准数量", FuncGetValue=p=>p.ApproveNumber},
                     new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="交货数量", FuncGetValue=p=>p.DeliverNumber},
                     new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="最后修改人", FuncGetValue=p=>p.CreateBy},
                     new ExcelColumnFormat<ApplyCardToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime}
            };
            var wordBook = ExcelHelper.DataToExcel(applyList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "卡申请列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }
        #endregion

        #region 卡批准
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyCard()
        {
            return View();
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        /// <param name="oddNumbers"></param>
        /// <param name="applyNumber"></param>
        /// <param name="deliverNumber"></param>
        /// <param name="modifyBy"></param>
        /// <param name="status"></param>
        /// <param name="executeStatus"></param>
        /// <param name="page"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult GetVerifyList(string id, string applyNumber, string deliverNumber, string modifyBy, string status, string executeStatus, string acceptingUnit, string page, int? dataGroupID = null)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetVerifyList", false, new object[] { id, applyNumber, deliverNumber, modifyBy, status, executeStatus,acceptingUnit ,JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);
        }


        public JsonResult ExecuteVerify(string jsonParam)
        {
            var result = Submit("Distribution", "ExecuteVerify", false, new object[] { jsonParam, Auth.UserID});
            return Json(result);
        }
        
        #endregion

        #region 总部卡领出
        public ActionResult CardOutTitle()
        {
            ViewBag.UserName = Auth.Username;         
            return View();
        }

        /// <summary>
        ///  加载总部卡领出列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="boxNo"></param>
        /// <param name="code"></param>
        /// <param name="cardNo"></param>
        /// <param name="boxNum"></param>
        /// <param name="money"></param>
        /// <param name="modifyBy"></param>
        /// <returns></returns>
        public ActionResult GetCardOutTitleList(string oddId, string status, string acceptingUnit, string boxNum, string modifyBy)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetCardOutTitleList", false, new object[] { oddId, status, acceptingUnit, boxNum, modifyBy, JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);
        }


        public ActionResult GetOddIdList()
        {
            var result = Submit("Distribution", "GetOddIdList", false, new object[] { });
            return Json(result);
        }

        public ActionResult ChooseOddId(string acceptingUnit)
        {       
            var result = Submit("Distribution", "ChooseOddId", false, new object[] { acceptingUnit});
            return Json(result.Obj[0]);
        }

        public ActionResult ChooseOddId1(string oddId)
        {
            var result = Submit("Distribution", "ChooseOddId1", false, new object[] { oddId });
            return Json(result.Obj[0]);
        }

        public ActionResult AddTitleCard(string jsonParam, string status)
        {
            var result = Submit("Distribution", "AddTitleCard", false, new object[] { jsonParam, status, Auth.UserID });
            return Json(result);
        }

        public JsonResult GetBoxInfo(string boxNo)
        {
            var result = Submit("Distribution", "GetBoxInfo", false, new object[] { boxNo });
            return Json(result.Obj[0]);
        }

        public JsonResult LoadApplyId()
        {
            var result = Submit("Distribution", "LoadApplyId", false, new object[] { });
            return Json(result.Obj[0]);
        }

        #endregion
               
        #region 卡领入
        public ActionResult CardIn()
        {
            return View();
        }


        public JsonResult GetCardInList(string oddId, string boxNum, string status, string modifyBy)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetCardInList", false, new object[] { oddId, boxNum, status, modifyBy, JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }
        #endregion

        #region 分公司卡领出
        public ActionResult CardOutBranch()
        {
            ViewBag.UserName = Auth.Username;         
            return View();
        }

        public ActionResult GetCardOutBranchList(string oddId, string status, string acceptingUnit, string boxNum, string modifyBy,string boxNo)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetCardOutBranchList", false, new object[] { oddId, status, acceptingUnit, boxNum, modifyBy, boxNo,JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);
        }


        public ActionResult GetBranchOddIdList()
        {
            var result = Submit("Distribution", "GetBranchOddIdList", false, new object[] { });
            return Json(result.Obj[0]);
        }

        public ActionResult ChooseBranchOddId(string acceptingUnit)
        {
            var result = Submit("Distribution", "ChooseBranchOddId", false, new object[] { acceptingUnit });
            return Json(result.Obj[0]);
        }

        public ActionResult ChooseBranchOddId1(string oddId)
        {
            var result = Submit("Distribution", "ChooseBranchOddId1", false, new object[] { oddId });
            return Json(result.Obj[0]);
        }


        public JsonResult LoadStore(string companyCode)
        {
            var result = Submit("Distribution", "LoadStore", false, new object[] { companyCode });
            return Json(result.Obj[0]);
        }

        public ActionResult AddBranchCard(string jsonParam, string status)
        {
            var result = Submit("Distribution", "AddBranchCard", false, new object[] { jsonParam, status,Auth.UserID});
            return Json(result.IsPass);
        }

        public ActionResult GetOddIdNo()
        {
            var result = Submit("Distribution", "GetOddIdNo", false, new object[] { });
            return Json(result);
        }

        [HttpPost]
        public FileResult CardOutBranchListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelAcceptingUnit, string txtExcelCreateTime, string txtExcelBoxNo)
        {
            var result = Submit("Distribution", "CardOutBranchListToExcel", false, new object[] { txtExcelOddIdNo, txtExcelBoxNum, txtExcelStatus, txtExcelAcceptingUnit, txtExcelCreateTime, txtExcelBoxNo });
            var cardOutTitleList = JsonHelper.Deserialize<List<CardOutBranchListToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<CardOutBranchListToExcel>> excelColumnFormats = new List<ExcelColumnFormat<CardOutBranchListToExcel>>{
                  new ExcelColumnFormat<CardOutBranchListToExcel>{ColumnName="单号", FuncGetValue=p=>p.OddIdNo},          
                  new ExcelColumnFormat<CardOutBranchListToExcel>{ColumnName="状态", FuncGetValue=p=>p.StatusName},
                  new ExcelColumnFormat<CardOutBranchListToExcel>{ColumnName="发货单位", FuncGetValue=p=>p.SendingUnit},          
                  new ExcelColumnFormat<CardOutBranchListToExcel>{ColumnName="总盒数", FuncGetValue=p=>p.BoxNumber},               
                  new ExcelColumnFormat<CardOutBranchListToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime},             
            };
            var wordBook = ExcelHelper.DataToExcel(cardOutTitleList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "分公司卡领出列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }
        #endregion

        #region 分公司卡退领
        public ActionResult RepealCardBranch()
        {
            return View();
        }
        /// <summary>
        ///  加载分公司卡退领列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="boxNo"></param>
        /// <param name="code"></param>
        /// <param name="cardNo"></param>
        /// <param name="boxNum"></param>
        /// <param name="money"></param>
        /// <param name="modifyBy"></param>
        /// <returns></returns>
        public ActionResult GetCardRepealBranchList(string oddId, string status, string boxNo, string modifyBy)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetCardRepealBranchList", false, new object[] { oddId, status, boxNo, modifyBy, JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);
        
        }
        public ActionResult AddCardRepeal(string jsonParam, string status, int repealtype)
        {
            var result = Submit("Distribution", "AddCardRepeal", false, new object[] { jsonParam, status,repealtype, Auth.UserID });
            return Json(result.IsPass);
        }
        public ActionResult ChooseBox(string acceptingUnit)
        {
            var result = Submit("Distribution", "ChooseBox", false, new object[] { acceptingUnit });
            return Json(result.Obj[0]);
        }
        public JsonResult GetBoxReturnInfo(string boxNo)
        {
            var result = Submit("Distribution", "GetBoxReturnInfo", false, new object[] { boxNo });
            return Json(result.Obj[0]);
        }

        [HttpPost]
        public FileResult CardRepealBranchListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelCreateTime)
        {
            var result = Submit("Distribution", "CardRepealBranchListToExcel", false, new object[] { txtExcelOddIdNo, txtExcelBoxNum, txtExcelStatus, txtExcelCreateTime });
            var cardOutTitleList = JsonHelper.Deserialize<List<CardRepealBranchListToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<CardRepealBranchListToExcel>> excelColumnFormats = new List<ExcelColumnFormat<CardRepealBranchListToExcel>>{
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="单号", FuncGetValue=p=>p.OddIdNo},          
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="状态", FuncGetValue=p=>p.StatusName},
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="发送单位", FuncGetValue=p=>p.SendingUnitName},          
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="收货单位", FuncGetValue=p=>p.AcceptingName},                   
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="总盒数", FuncGetValue=p=>p.BoxNumber},                        
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="总数量", FuncGetValue=p=>p.CardNumber},     
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="最后修改人", FuncGetValue=p=>p.CreateBy},
                  new ExcelColumnFormat<CardRepealBranchListToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime}          
            };
            var wordBook = ExcelHelper.DataToExcel(cardOutTitleList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "分公司卡退领列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }

        #endregion

        #region 门店卡退领
        public ActionResult RepealCardStore()
        {
            return View();
        }
        // <summary>
        ///  加载门店卡退领列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="boxNo"></param>
        /// <param name="code"></param>
        /// <param name="cardNo"></param>
        /// <param name="boxNum"></param>
        /// <param name="money"></param>
        /// <param name="modifyBy"></param>
        /// <returns></returns>
        public ActionResult GetCardRepealStoreList(string oddId, string status, string boxNo, string modifyBy)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetCardRepealStoreList", false, new object[] { oddId, status, boxNo, modifyBy, JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);

        }

        [HttpPost]
        public FileResult CardRepealStoreListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelCreateTime)
        {
            var result = Submit("Distribution", "CardRepealStoreListToExcel", false, new object[] { txtExcelOddIdNo, txtExcelBoxNum, txtExcelStatus, txtExcelCreateTime });
            var cardOutTitleList = JsonHelper.Deserialize<List<CardRepealStoreListToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<CardRepealStoreListToExcel>> excelColumnFormats = new List<ExcelColumnFormat<CardRepealStoreListToExcel>>{
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="单号", FuncGetValue=p=>p.OddIdNo},          
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="状态", FuncGetValue=p=>p.StatusName},
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="发送单位", FuncGetValue=p=>p.SendingUnitName},          
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="收货单位", FuncGetValue=p=>p.AcceptingName},                   
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="总盒数", FuncGetValue=p=>p.BoxNumber},                        
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="总数量", FuncGetValue=p=>p.CardNumber},     
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="最后修改人", FuncGetValue=p=>p.CreateBy},
                  new ExcelColumnFormat<CardRepealStoreListToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime}          
            };
            var wordBook = ExcelHelper.DataToExcel(cardOutTitleList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "门店卡退领列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }
        #endregion

        #region 总部卡退领
        public ActionResult RepealCardGroup()
        {
            return View();
        }
        // <summary>
        ///  加载总部卡退领列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="boxNo"></param>
        /// <param name="code"></param>
        /// <param name="cardNo"></param>
        /// <param name="boxNum"></param>
        /// <param name="money"></param>
        /// <param name="modifyBy"></param>
        /// <returns></returns>
        public ActionResult GetCardRepealGroupList(string oddId, string status, string boxNo, string modifyBy)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetCardRepealGroupList", false, new object[] { oddId, status, boxNo, modifyBy, JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);

        }

        public ActionResult ChooseBox2(string acceptingUnit, string sendingUnit)
        {
            var result = Submit("Distribution", "ChooseBox2", false, new object[] { acceptingUnit, sendingUnit });
            return Json(result.Obj[0]);
        }

        [HttpPost]
        public FileResult CardRepealTitleListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelCreateTime)
        {
            var result = Submit("Distribution", "CardRepealTitleListToExcel", false, new object[] { txtExcelOddIdNo, txtExcelBoxNum, txtExcelStatus, txtExcelCreateTime });
            var cardOutTitleList = JsonHelper.Deserialize<List<CardRepealTitleListToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<CardRepealTitleListToExcel>> excelColumnFormats = new List<ExcelColumnFormat<CardRepealTitleListToExcel>>{
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="单号", FuncGetValue=p=>p.OddIdNo},          
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="状态", FuncGetValue=p=>p.StatusName},
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="发送单位", FuncGetValue=p=>p.SendingUnit},          
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="收货单位", FuncGetValue=p=>p.AcceptingUnitName},                   
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="总盒数", FuncGetValue=p=>p.BoxNumber},                        
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="总数量", FuncGetValue=p=>p.CardNumber},     
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="最后修改人", FuncGetValue=p=>p.CreateBy},
                  new ExcelColumnFormat<CardRepealTitleListToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime}          
            };
            var wordBook = ExcelHelper.DataToExcel(cardOutTitleList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "总部卡退领列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }
        #endregion

        #region 卡中心详情页
        [HttpPost]
        public ActionResult CardCenterDetailPage(string pageKey,string id)
        {
            ViewBag.PageKey=pageKey;
            ViewBag.QueryId = id;
            return View();
        }

        public JsonResult ApplyCardPage(string queryId)
        {
            var result = Submit("Distribution", "ApplyCardPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult ApplyCardDetailPage(string queryId)
        {
            var result = Submit("Distribution", "ApplyCardDetailPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CustomizePage(string queryId)
        {
            var result = Submit("Distribution", "CustomizePage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CustomizeDetailPage(string queryId)
        {
            var result = Submit("Distribution", "CustomizeDetailPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult RetrievePage(string queryId)
        {
            var result = Submit("Distribution", "RetrievePage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult RetrieveDetailPage(string queryId)
        {
            var result = Submit("Distribution", "RetrieveDetailPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult BatchProductionPage(string queryId)
        {
            var result = Submit("Distribution", "BatchProductionPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult BoxAndCard(string queryId)
        {
            var result = Submit("Distribution", "BoxAndCard", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardOutTitlePage(string queryId)
        {
            var result = Submit("Distribution", "CardOutTitlePage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardOutTitleDetailPage(string queryId)
        {
            var result = Submit("Distribution", "CardOutTitleDetailPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardOutTitleDetailPage1(string queryId)
        {
            var result = Submit("Distribution", "CardOutTitleDetailPage1", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardOutBranchPage(string queryId)
        {
            var result = Submit("Distribution", "CardOutBranchPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardOutBranchDetailPage(string queryId)
        {
            var result = Submit("Distribution", "CardOutBranchDetailPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardOutBranchDetailPage1(string queryId)
        {
            var result = Submit("Distribution", "CardOutBranchDetailPage1", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardRepealDetailPage(string queryId)
        {
            var result = Submit("Distribution", "CardRepealDetailPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }

        public JsonResult CardRepealDetailPage1(string queryId)
        {
            var result = Submit("Distribution", "CardRepealDetailPage1", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }
        public JsonResult CardRepealPage(string queryId)
        {
            var result = Submit("Distribution", "CardRepealPage", false, new object[] { queryId });
            return Json(result.Obj[0]);
        }
        public JsonResult VerifyTrue(string key, string queryId, string status)
        {
            var result = Submit("Distribution", "VerifyTrue", false, new object[] {key, queryId,status });
            return Json(result);
        }
        #endregion

        #region 总部卡领出(新)
        public ActionResult CardOutTitleNew()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CardOutTitleAddNew()
        {
            ViewBag.User = Auth.Username;
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="acceptingUnit"></param>
        /// <param name="boxNum"></param>
        /// <param name="queryCreateTime"></param>
        /// <returns></returns>
        public ActionResult GetCardOutTitleNewList(string oddId, string status, string acceptingUnit, string boxNum, string createTime,string boxNo,string store)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "GetCardOutTitleNewList", false, new object[] { oddId, status, acceptingUnit, boxNum, createTime,boxNo,store ,JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 详情列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <returns></returns>
        public JsonResult CardOutTitleDetailsParams(string oddId)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("Distribution", "CardOutTitleDetailsParams", false, new object[] { oddId,JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 筛选盒号
        /// </summary>
        /// <param name="acceptingUnit"></param>
        /// <returns></returns>
        public JsonResult ChooseBoxNoByTitle(string company,string store)
        {
            var result = Submit("Distribution", "ChooseBoxNoByTitle", false, new object[] { company,store });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 门店code查询申请单号
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public JsonResult LoadApplyOddId(string storeCode)
        {
            var result = Submit("Distribution", "LoadApplyOddId", false, new object[] { storeCode });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 盒号查询盒信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public JsonResult GetBoxInfoNew(string boxNo)
        {
            var result = Submit("Distribution", "GetBoxInfoNew", false, new object[] { boxNo });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 盒卡关联信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public JsonResult GetBoxCardInfoNew(string boxNo)
        {
            var result = Submit("Distribution", "GetBoxCardInfoNew", false, new object[] { boxNo });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 新增总部卡领出
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult AddNewCardOutTitle(string jsonParam, string status)
        {
            var result = Submit("Distribution", "AddNewCardOutTitle", false, new object[] { jsonParam, status,Auth.UserID });
            return Json(result);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult ChangeStatusCardOutTitle(string oddId, string status)
        {
            var result = Submit("Distribution", "ChangeStatusCardOutTitle", false, new object[] { oddId, status, Auth.UserID });
            return Json(result);
        }

        /// <summary>
        /// 总部卡领出列表导出Excel
        /// </summary>
        /// <param name="txtExcelOddIdNo"></param>
        /// <param name="txtExcelBoxNum"></param>
        /// <param name="txtExcelStatus"></param>
        /// <param name="txtExcelAcceptingUnit"></param>
        /// <param name="txtExcelCreateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult CardOutTitleListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelAcceptingUnit, string txtExcelCreateTime, string txtExcelBoxNo, string txtExcelStore)
        {
            var result = Submit("Distribution", "CardOutTitleListToExcel", false, new object[] { txtExcelOddIdNo, txtExcelBoxNum, txtExcelStatus, txtExcelAcceptingUnit, txtExcelCreateTime, txtExcelBoxNo, txtExcelStore });
            var cardOutTitleList = JsonHelper.Deserialize<List<CardOutTitleListToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<CardOutTitleListToExcel>> excelColumnFormats = new List<ExcelColumnFormat<CardOutTitleListToExcel>>{
                  new ExcelColumnFormat<CardOutTitleListToExcel>{ColumnName="单号", FuncGetValue=p=>p.OddIdNo},          
                  new ExcelColumnFormat<CardOutTitleListToExcel>{ColumnName="状态", FuncGetValue=p=>p.StatusName},
                  new ExcelColumnFormat<CardOutTitleListToExcel>{ColumnName="发货单位", FuncGetValue=p=>p.SendingUnit},          
                  new ExcelColumnFormat<CardOutTitleListToExcel>{ColumnName="总盒数", FuncGetValue=p=>p.BoxNumber},               
                  new ExcelColumnFormat<CardOutTitleListToExcel>{ColumnName="最后修改时间", FuncGetValue=p=>p.CreateTime},             
            };
            var wordBook = ExcelHelper.DataToExcel(cardOutTitleList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "总部卡领出列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }
        #endregion
    }
}
