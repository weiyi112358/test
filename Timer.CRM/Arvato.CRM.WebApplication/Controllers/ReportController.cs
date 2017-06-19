using Arvato.CRM.Utility;
using Arvato.CRM.WcfFramework.ClientProxy;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Model;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.EF;
//using Arvato.CRM.Model.Report;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SettlementShare()
        {
            //渠道 
            var chan = Submit("Service", "GetStoreChannels", false);
            ViewBag.Channels = chan.Obj[0];
            //大区
            var area = Submit("Service", "GetStoreArea", false);
            ViewBag.Areas = area.Obj[0];
            //店铺
            var city = Submit("Service", "GetReportCity", false);
            ViewBag.City = city.Obj[0];
            //当前日期
            ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd");
            //门店
            var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
            ViewBag.Stores = stores.Obj[0];
            return View();
        }

        /// <summary>
        /// 结算分摊统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSettlementShareList(string store)
        {
            var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
            ViewBag.Stores = stores.Obj[0];
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Report", "GetSettlementShareList", false, new object[] { JsonHelper.Serialize(dts), store });
            return Json(rst.Obj[0].ToString());
        }

        #region 折扣率
        public ActionResult DiscountRate()
        {
            return View();
        }

        public JsonResult DiscountRateQuery(string txtStartDate, string txtEndDate)
        {
            DateTime? begin = DateTime.Now;
            DateTime? end = DateTime.Now;
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Report", "DiscountRateQuery", false, new object[] { JsonHelper.Serialize(dts), begin, end });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 导出折扣率
        /// </summary>
        /// <param name="exprChannel"></param>
        /// <param name="exprArea"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult ExportDiscountRate(string txtStartDate, string txtEndDate)
        {
            DateTime? begin = DateTime.Now;
            DateTime? end = DateTime.Now;
            List<ExcelColumnFormat<sp_Rpt_DiscountRate_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_DiscountRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="充值现金金额", FuncGetValue=p=>p.ID},
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="充值积分", FuncGetValue=p=>p.Name},
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="消费赠送积分", FuncGetValue=p=>p.Sex},  
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="代金券", FuncGetValue=p=>p.Birthday},
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="折扣率", FuncGetValue=p=>p.age},
            };
            try
            {
                var result = Submit("Report", "ExportDiscountRate", false, new object[] { txtStartDate, txtEndDate });
                if (result.IsPass == false)
                {
                    List<sp_Rpt_DiscountRate_Result> list = new List<sp_Rpt_DiscountRate_Result>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<sp_Rpt_DiscountRate_Result> list = JsonHelper.Deserialize<List<sp_Rpt_DiscountRate_Result>>(result.Obj[0].ToString());
                    //HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "折扣率导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "折扣率导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }
        }
        #endregion

        #region 会员首次消费占比
        public ActionResult FirstConsumptionProportion()
        {
            return View();
        }


        public JsonResult FirstConsumptionProportionQuery(string txtMobile, string txtStore, string txtStartDate, string txtEndDate)
        {
            DateTime? begin = DateTime.Now;
            DateTime? end = DateTime.Now;
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Report", "FirstConsumptionProportionQuery", false, new object[] { JsonHelper.Serialize(dts), txtMobile, txtStore, begin, end });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 导出折扣率
        /// </summary>
        /// <param name="exprChannel"></param>
        /// <param name="exprArea"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult ExportFirstConsumptionProportion(string txtMobile, string txtStore, string txtStartDate, string txtEndDate)
        {
            DateTime? begin = DateTime.Now;
            DateTime? end = DateTime.Now;
            List<ExcelColumnFormat<sp_Rpt_DiscountRate_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_DiscountRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="首次消费金额", FuncGetValue=p=>p.ID},
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="充值后金额", FuncGetValue=p=>p.Name},
                    new ExcelColumnFormat<sp_Rpt_DiscountRate_Result>{ColumnName="首次消费占比", FuncGetValue=p=>p.Sex},  
            };
            try
            {
                var result = Submit("Report", "ExportFirstConsumptionProportion", false, new object[] { txtMobile, txtStore, txtStartDate, txtEndDate });
                if (result.IsPass == false)
                {
                    List<sp_Rpt_DiscountRate_Result> list = new List<sp_Rpt_DiscountRate_Result>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<sp_Rpt_DiscountRate_Result> list = JsonHelper.Deserialize<List<sp_Rpt_DiscountRate_Result>>(result.Obj[0].ToString());
                    //HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "会员首次消费占比导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "会员首次消费占比导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }
        }

        #endregion

        #region 会员流失率
        public ActionResult CustomerLostRate()
        {
            return View();
        }

        public JsonResult CustomerLostRateQuery(string txtMobile, string txtStore, string txtYear)
        {
            DateTime? year = Convert.ToDateTime(txtYear);
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Report", "CustomerLostRateQuery", false, new object[] { JsonHelper.Serialize(dts), txtMobile, txtStore, year });
            return Json(rst.Obj[0].ToString());
        }

        [HttpPost]
        public FileResult ExportCustomerLostRate(string txtMobile, string txtStore, string txtYear)
        {
            try
            {
                DateTime? year = Convert.ToDateTime(txtYear);
                List<ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="一月来厂", FuncGetValue=p=>p.ID},
                    new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="二月来厂", FuncGetValue=p=>p.Name},
                    new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="三月来厂", FuncGetValue=p=>p.Sex},  
                     new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="四月来厂", FuncGetValue=p=>p.Name},  
                      new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="五月来厂", FuncGetValue=p=>p.Sex},  
                       new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="六月来厂", FuncGetValue=p=>p.Name},  
                        new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="七月来厂", FuncGetValue=p=>p.Sex},  
                         new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="八月来厂", FuncGetValue=p=>p.Name},  
                          new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="九月来厂", FuncGetValue=p=>p.Sex},  
                           new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="十月来厂", FuncGetValue=p=>p.Name},  
                            new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="十一月来厂", FuncGetValue=p=>p.Sex},  
                             new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="十二月来厂", FuncGetValue=p=>p.Name},  
                    };
                var result = Submit("Report", "ExportCustomerLostRate", false, new object[] { txtMobile, txtStore, txtYear });
                if (result.IsPass == false)
                {
                    List<sp_Rpt_CustomerLostRate_Result> list = new List<sp_Rpt_CustomerLostRate_Result>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<sp_Rpt_CustomerLostRate_Result> list = JsonHelper.Deserialize<List<sp_Rpt_CustomerLostRate_Result>>(result.Obj[0].ToString());
                    //HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "会员流失率导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "会员流失率导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }
        }

        #endregion

        #region 续保
        public ActionResult RenewalInsuranceRate()
        {
            return View();
        }

        public JsonResult RenewalInsuranceRateQuery(string txtMobile, string txtStore, string txtYear)
        {
            DateTime? year = Convert.ToDateTime(txtYear == null ? DateTime.Now.ToString() : txtYear);
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Report", "RenewalInsuranceRateQuery", false, new object[] { JsonHelper.Serialize(dts), txtMobile, txtStore, year });
            return Json(rst.Obj[0].ToString());
        }

        [HttpPost]
        public FileResult ExportRenewalInsuranceRate(string txtMobile, string txtStore, string txtYear)
        {
            try
            {
                DateTime? year = Convert.ToDateTime(txtYear);
                List<ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="1月首保/续保台次", FuncGetValue=p=>p.ID},
                    new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="2月首保/续保台次", FuncGetValue=p=>p.Name},
                    new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="13月首保/续保台次", FuncGetValue=p=>p.Sex},  
                     new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="4月首保/续保台次", FuncGetValue=p=>p.Name},  
                      new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="5月首保/续保台次", FuncGetValue=p=>p.Sex},  
                       new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="6月首保/续保台次", FuncGetValue=p=>p.Name},  
                        new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="7月首保/续保台次", FuncGetValue=p=>p.Sex},  
                         new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="8月首保/续保台次", FuncGetValue=p=>p.Name},  
                          new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="9月首保/续保台次", FuncGetValue=p=>p.Sex},  
                           new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="10月首保/续保台次", FuncGetValue=p=>p.Name},  
                            new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="11月首保/续保台次", FuncGetValue=p=>p.Sex},  
                             new ExcelColumnFormat<sp_Rpt_CustomerLostRate_Result>{ColumnName="12月首保/续保台次", FuncGetValue=p=>p.Name},  
                    };
                var result = Submit("Report", "ExportRenewalInsuranceRate", false, new object[] { txtMobile, txtStore, txtYear });
                if (result.IsPass == false)
                {
                    List<sp_Rpt_CustomerLostRate_Result> list = new List<sp_Rpt_CustomerLostRate_Result>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<sp_Rpt_CustomerLostRate_Result> list = JsonHelper.Deserialize<List<sp_Rpt_CustomerLostRate_Result>>(result.Obj[0].ToString());
                    //HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "续保率导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "续保率导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }
        }
        #endregion

        #region 机油搭载率
        public ActionResult MachineOilCarryRate()
        {
            return View();
        }

        public JsonResult GetMachineOilCarryRate(string txtMobile, string txtStore, string txtStartDate, string txtEndDate)
        {
            DateTime? begin = Convert.ToDateTime(txtStartDate);
            DateTime? end = Convert.ToDateTime(txtEndDate);
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Report", "GetMachineOilCarryRate", false, new object[] { JsonHelper.Serialize(dts), txtMobile, txtStore, begin, end });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 机油搭载率
        /// </summary>
        /// <param name="txtStartDate"></param>
        /// <param name="txtEndDate"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult ExportMachineOilCarryRate(string txtMobile, string txtStore,string txtStartDate, string txtEndDate)
        {
            DateTime? begin = Convert.ToDateTime(txtStartDate == null ? DateTime.Now.ToString() : txtStartDate);
            DateTime? end = Convert.ToDateTime(txtEndDate == null ? DateTime.Now.ToString() : txtStartDate);
            List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="一般维修", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采机油保养台次", FuncGetValue=p=>p.Age},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="事故", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修+保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="其他", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="合计", FuncGetValue=p=>p.Age},

                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="一般维修", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采机油保养台次", FuncGetValue=p=>p.Age},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="事故", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修+保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="其他", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="合计", FuncGetValue=p=>p.Age},

                     new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="一般维修", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采机油保养台次", FuncGetValue=p=>p.Age},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="事故", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修+保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="其他", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="合计", FuncGetValue=p=>p.Age},

                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采保养台次搭载率", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采保养金额搭载率", FuncGetValue=p=>p.Age}
            };
            //表头栏格式
            List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>> headTopSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="进场台次类型"},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,0,6)}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修收入统计"},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,7,13)},  
                     new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="客单价"},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,14,20)},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采保养机油搭载"}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,21,23)},  
            };
            try
            {
                var result = Submit("Report", "ExportMachineOilCarryRate", false, new object[] { txtMobile, txtStore, begin, end });
                if (result.IsPass == false)
                {
                    List<sp_Rpt_MachineOilCarryRate_Result> list = new List<sp_Rpt_MachineOilCarryRate_Result>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<sp_Rpt_MachineOilCarryRate_Result> list = JsonHelper.Deserialize<List<sp_Rpt_MachineOilCarryRate_Result>>(result.Obj[0].ToString());
                    //HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                   // HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                    var strm = ExcelHelper.GetStream(workBook);
                    strm.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(strm, "application/vnd.ms-excel", "机油搭载率导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "机油搭载率导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }
        }
        #endregion

        #region 台次维修收入汇总
        public ActionResult ServiceIncomeCount()
        {
            return View();
        }

        public JsonResult GetServiceIncomeCount(string txtMobile, string txtStore, string txtStartDate, string txtEndDate)
        {
            DateTime? begin = Convert.ToDateTime(txtStartDate);
            DateTime? end = Convert.ToDateTime(txtEndDate);
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Report", "GetServiceIncomeCount", false, new object[] { JsonHelper.Serialize(dts), txtMobile, txtStore, begin, end });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 台次维修收入汇总
        /// </summary>
        /// <param name="txtStartDate"></param>
        /// <param name="txtEndDate"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult ExportServiceIncomeCount(string txtMobile, string txtStore, string txtStartDate, string txtEndDate)
        {
            DateTime? begin = Convert.ToDateTime(txtStartDate == null ? DateTime.Now.ToString() : txtStartDate);
            DateTime? end = Convert.ToDateTime(txtEndDate == null ? DateTime.Now.ToString() : txtStartDate);
            List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="一般维修", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采机油保养台次", FuncGetValue=p=>p.Age},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="事故", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修+保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="其他", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="合计", FuncGetValue=p=>p.Age},

                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="一般维修", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采机油保养台次", FuncGetValue=p=>p.Age},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="事故", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修+保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="其他", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="合计", FuncGetValue=p=>p.Age},

                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="一般维修", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="自采机油保养台次", FuncGetValue=p=>p.Age},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="事故", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修+保养", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="其他", FuncGetValue=p=>p.Age},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="合计", FuncGetValue=p=>p.Age},
            };
            //表头栏格式
            List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>> headTopSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>>{
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="进场台次类型"},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,0,6)}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修收入统计"},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,7,13)}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="维修收入统计"},  
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""},
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName=""}, 
                    new ExcelColumnFormat<sp_Rpt_MachineOilCarryRate_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,14,20)}, 
            };
            try
            {
                var result = Submit("Report", "ExportServiceIncomeCount", false, new object[] { txtMobile, txtStore, begin, end });
                if (result.IsPass == false)
                {
                    List<sp_Rpt_MachineOilCarryRate_Result> list = new List<sp_Rpt_MachineOilCarryRate_Result>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<sp_Rpt_MachineOilCarryRate_Result> list = JsonHelper.Deserialize<List<sp_Rpt_MachineOilCarryRate_Result>>(result.Obj[0].ToString());
                    //HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
                    var strm = ExcelHelper.GetStream(workBook);
                    strm.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(strm, "application/vnd.ms-excel", "台次维修收入汇总导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "台次维修收入汇总导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }
        }
        #endregion
        #region MyRegion
        
        //#region 会员人数统计

        ///// <summary>
        ///// 会员人数统计
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MemCount()
        //{
        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    //店铺
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    //当前日期
        //    ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd");
        //    //门店
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult GetStoreByCity(string cityCode)
        //{

        //    var rst = Submit("Report", "GetStoreByCity", false, new object[] { cityCode });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 会员人数统计
        ///// </summary>
        ///// <param name="channel"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult GetMemCount(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    if (store == null)
        //    {
        //        store = "";
        //    }

        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMemCount", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, dateStart, dateEnd });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出excel
        ///// </summary>
        ///// <param name="channel"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMemCount(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtStart, DateTime? exprDtEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_Mem_Count_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="渠道明细", FuncGetValue=p=>p.Channel},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="城市", FuncGetValue=p=>p.City},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="店铺", FuncGetValue=p=>p.Store},  
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="普通会员", FuncGetValue=p=>p.Com_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="铜卡", FuncGetValue=p=>p.Copper_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="银卡", FuncGetValue=p=>p.Silver_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="金卡", FuncGetValue=p=>p.Gold_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="白金会员", FuncGetValue=p=>p.Platinum_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="会员总数", FuncGetValue=p=>p.Total_Mem}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="普通会员", FuncGetValue=p=>p.Com_Mem_New},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="铜卡", FuncGetValue=p=>p.Copper_Mem_New},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="银卡", FuncGetValue=p=>p.Silver_Mem_New},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="金卡", FuncGetValue=p=>p.Gold_Mem_New},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="白金会员", FuncGetValue=p=>p.Platinum_Mem_New},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="会员总数", FuncGetValue=p=>p.Total_Mem_New},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="会员增长率", FuncGetValue=p=>p.Percent_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="活跃会员量", FuncGetValue=p=>p.Active_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="潜在流失会员量", FuncGetValue=p=>p.WillLose_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="流失会员量", FuncGetValue=p=>p.Lose_Mem}
        //    };
        //    //表头栏格式
        //    List<ExcelColumnFormat<sp_Rpt_Mem_Count_Result>> headTopSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,0,2)},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="会员"},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},  
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="", Region=new ExcelDataRegion(0,0,3,8)},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="新增会员"},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},  
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName="", Region=new ExcelDataRegion(0,0,9,14)},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_Count_Result>{ColumnName=""}, 
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportMemCount", false, new object[] { exprChannel, exprArea, exprCity, exprStore, exprDtStart, exprDtEnd });
        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_Mem_Count_Result> list = new List<sp_Rpt_Mem_Count_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_Mem_Count_Result> list = JsonHelper.Deserialize<List<sp_Rpt_Mem_Count_Result>>(result.Obj[0].ToString());
        //            foreach (var spRptMemCountResult in list)
        //            {
        //                if (spRptMemCountResult.Total_Mem_New == "0" || spRptMemCountResult.Total_Mem_New == null)
        //                {
        //                    spRptMemCountResult.Percent_Mem = "0.00%";
        //                }
        //                else if (spRptMemCountResult.Total_Mem_New == spRptMemCountResult.Total_Mem)
        //                {
        //                    spRptMemCountResult.Percent_Mem = "100.00%";
        //                }
        //                else
        //                {
        //                    double rate = Convert.ToDouble(spRptMemCountResult.Total_Mem_New) / ((Convert.ToDouble(spRptMemCountResult.Total_Mem) - Convert.ToDouble(spRptMemCountResult.Total_Mem_New)));
        //                    spRptMemCountResult.Percent_Mem = Math.Round(rate * 100, 2).ToString() + "%";
        //                }
        //            }
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员人数统计导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员人数统计导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }

        //}
        //#endregion

        //#region 会员招募统计

        ///// <summary>
        ///// 会员招募统计
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MemRecruitCount()
        //{
        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];

        //    //门店
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];

        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];

        //    return View();
        //}

        ///// <summary>
        ///// 获取会员招募统计
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">店铺</param>
        ///// <param name="dateReg">注册日</param>
        ///// <param name="dateRegType">注册时间类型</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetMemRecruitCount(string channel, string area, string city, string store, DateTime? dateReg, string dateRegType)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMemRecruitCount", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, dateReg, dateRegType });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 会员招募统计导出excel
        ///// </summary>
        ///// <param name="channel"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMemRecruitCount(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtReg, string exprDtRegType)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="渠道明细", FuncGetValue=p=>p.Channel},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="城市", FuncGetValue=p=>p.City},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="店铺", FuncGetValue=p=>p.Store}, 
        //            //new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="会员招募目标", FuncGetValue=p=>p.RecruitTarget_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="实际招聘人数", FuncGetValue=p=>p.Actual_Recruitnum_Mem},
        //            //new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="完成率", FuncGetValue=p=>p.Completion_rate_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="环比增幅", FuncGetValue=p=>p.Same_Increase_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="同比增幅", FuncGetValue=p=>p.Lastyear_Increase_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_Recruit_Count_Result>{ColumnName="店铺区域占比", FuncGetValue=p=>p.Area_ratio_Mem},
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportMemRecruitCount", false, new object[] { exprChannel, exprArea, exprCity, exprStore, exprDtReg, exprDtRegType });


        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_Mem_Recruit_Count_Result> list = new List<sp_Rpt_Mem_Recruit_Count_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_Mem_Recruit_Count_Result> list = JsonHelper.Deserialize<List<sp_Rpt_Mem_Recruit_Count_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员招募统计导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员招募统计导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }

        //}

        //#endregion

        //#region 会员升级分析

        ///// <summary>
        ///// 会员升级分析
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MemUpGrade()
        //{

        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    //店铺
        //    var city = Submit("Service", "GetReportCity", false);

        //    ViewBag.City = city.Obj[0];
        //    ////大区  可能需要更改
        //    //var area = Submit("Service", "GetBizOptionsByType", false, new object[] { "StoreArea", true });
        //    //ViewBag.Areas = area.Obj[0];

        //    //var city = Submit("Service", "GetReportCity", false);
        //    //ViewBag.City = city.Obj[0];

        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    return View();
        //}

        ///// <summary>
        ///// 会员升级分析查询
        ///// </summary>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult GetMemUpGrade(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMemUpGrade", false, new object[] { JsonHelper.Serialize(dts),channel, area, city, store, dateStart, dateEnd });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 会员升级分析导出
        ///// </summary>
        ///// <param name="exprChannel"></param>
        ///// <param name="exprArea"></param>
        ///// <param name="exprCity"></param>
        ///// <param name="exprStore"></param>
        ///// <param name="exprDtStart"></param>
        ///// <param name="exprDtEnd"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMemUpGrade(string exprchannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtStart, DateTime? exprDtEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_MemUpGrade>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MemUpGrade>>{
        //            new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="渠道明细", FuncGetValue=p=>p.Channel},
        //            new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="城市", FuncGetValue=p=>p.City},
        //            new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="店铺", FuncGetValue=p=>p.Store}, 
        //            new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="升铜卡", FuncGetValue=p=>p.ComToCopper},
        //            //new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="累计普卡升铜卡", FuncGetValue=p=>p.ActComToCopper},
        //            new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="升银卡", FuncGetValue=p=>p.CopperToSilver},
        //            //new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="累计铜卡升银卡", FuncGetValue=p=>p.ActCopperToSilver},
        //            new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="升金卡", FuncGetValue=p=>p.SilverToGold},
        //            //new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="累计银卡升金卡", FuncGetValue=p=>p.ActSilverToGold},
        //            new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="升白金卡", FuncGetValue=p=>p.GoldToPlatinum},
        //            //new ExcelColumnFormat<sp_Rpt_MemUpGrade>{ColumnName="累计金卡升白金卡", FuncGetValue=p=>p.ActSilverToGold} ,
        //    };

        //    try
        //    {
        //        var result = Submit("Report", "ExportMemUpGrade", false, new object[] { exprchannel,exprArea, exprCity, exprStore, exprDtStart, exprDtEnd });


        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_MemUpGrade> list = new List<sp_Rpt_MemUpGrade>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_MemUpGrade> list = JsonHelper.Deserialize<List<sp_Rpt_MemUpGrade>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员升级分析导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员升级i分析导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }

        //}

        //#endregion

        //#region 会员月消费区间统计

        ///// <summary>
        ///// 会员月消费区间统计
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MemMonthConsum()
        //{
        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    //店铺
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    //门店
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    //当前日期
        //    ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd");
        //    return View();
        //}

        ///// <summary>
        ///// 会员月消费区间统计
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult GetMemMonthConsum(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMemMonthConsum", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, dateStart, dateEnd });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 会员月消费区间导出
        ///// </summary>
        ///// <param name="exprChannel"></param>
        ///// <param name="exprArea"></param>
        ///// <param name="exprCity"></param>
        ///// <param name="exprStore"></param>
        ///// <param name="exprDtStart"></param>
        ///// <param name="exprDtEnd"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMemMonthConsum(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtStart, DateTime? exprDtEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_MemMonthConsum>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MemMonthConsum>>{
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="统计年月", FuncGetValue=p=>p.StatTime},
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="会员类型", FuncGetValue=p=>p.MemType},
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="1-100元", FuncGetValue=p=>p.Cnt1}, 
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="101-300元", FuncGetValue=p=>p.Cnt2},
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="301-500元", FuncGetValue=p=>p.Cnt3},
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="501-1000元", FuncGetValue=p=>p.Cnt4},
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="1001-2000元", FuncGetValue=p=>p.Cnt5},
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="2001-5000元", FuncGetValue=p=>p.Cnt6},
        //            new ExcelColumnFormat<sp_Rpt_MemMonthConsum>{ColumnName="5000元以上", FuncGetValue=p=>p.Cnt7}, 
        //    };

        //    try
        //    {
        //        var result = Submit("Report", "ExportMemMonthConsum", false, new object[] { exprChannel, exprArea, exprCity, exprStore, exprDtStart, exprDtEnd });


        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_MemMonthConsum> list = new List<sp_Rpt_MemMonthConsum>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_MemMonthConsum> list = JsonHelper.Deserialize<List<sp_Rpt_MemMonthConsum>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员月消费区间导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员月消费区间异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }

        //}

        //#endregion

        //#region 会员消费频次统计
        ///// <summary>
        ///// 会员消费频次统计
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MemConsumptionFreCount()
        //{
        //    //当前日期
        //    ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd");
        //    //品牌
        //    var brand = Submit("Report", "GetBrand", false);
        //    ViewBag.Brand = brand.Obj[0];

        //    //品类  
        //    var category = Submit("Report", "GetCategory", false);
        //    ViewBag.category = category.Obj[0];

        //    //渠道  
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];


        //    return View();
        //}

        ///// <summary>
        /////  查询会员消费频次统计
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="brand">品牌</param>
        ///// <param name="category">品类</param>
        ///// <param name="dateConsumptionStart">消费日期起期</param>
        ///// <param name="dateConsumptionEnd">消费日期止期</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetMemConsumptionFreCount(string channel, string brand, string category, DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMemConsumptionFreCount", false, new object[] { JsonHelper.Serialize(dts), channel, brand, category, dateConsumptionStart, dateConsumptionEnd });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出会员消费频次统计
        ///// </summary>
        ///// <param name="exprChannel">渠道</param>
        ///// <param name="exprBrand">品牌</param>
        ///// <param name="exprCategory">品类</param>
        ///// <param name="expDtConsumptionStart">消费起期</param>
        ///// <param name="expDtConsumptionEnd">消费止期</param>
        //[HttpPost]
        //public FileResult ExportMemConsumptionFreCount(string exprChannel, string exprBrand, string exprCategory, DateTime? expDtConsumptionStart, DateTime? expDtConsumptionEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="会员", FuncGetValue=p=>p.member},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次=0", FuncGetValue=p=>p.FrequencyEqZero},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次=1", FuncGetValue=p=>p.FrequencyEqOne}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次=2", FuncGetValue=p=>p.FrequencyEqTwo},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次=3", FuncGetValue=p=>p.FrequencyEqThree},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次=4", FuncGetValue=p=>p.FrequencyEqFour},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次=5", FuncGetValue=p=>p.FrequencyEqFive},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次=6-10", FuncGetValue=p=>p.FrequencyEqFiveGoTen},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="频次>10", FuncGetValue=p=>p.FrequencyThanTen}

        //    };
        //    //表头栏格式
        //    List<ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>> headTopSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName="消费频次",Region=new ExcelDataRegion(0,0,1,8)},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>{ColumnName=""}
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportMemConsumptionFreCount", false, new object[] { exprChannel, exprBrand, exprCategory, expDtConsumptionStart, expDtConsumptionEnd });


        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_Mem_ConsumptionFrequency_Count_Result> list = new List<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_Mem_ConsumptionFrequency_Count_Result> list = JsonHelper.Deserialize<List<sp_Rpt_Mem_ConsumptionFrequency_Count_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headTopSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员消费频次统计导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员消费频次统计导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        //#region  会员积分发放和消费统计
        //public ActionResult MemPointIssuingConsumption()
        //{
        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    //店铺
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    return View();
        //}
        //[HttpPost]
        //public JsonResult GetMemIssuingConsumption(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var result = Submit("Report", "GetMemIssuingConsumption", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, dateStart, dateEnd });
        //    return Json(result.Obj[0].ToString());
        //}
        //[HttpPost]
        //public FileResult ExportMemIssuingConsumption(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtStart, DateTime? exprDtEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="渠道明细", FuncGetValue=p=>p.Channel},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="城市", FuncGetValue=p=>p.City},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="店铺", FuncGetValue=p=>p.Store}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="普通会员", FuncGetValue=p=>p.Com_Mem_Get},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="铜卡", FuncGetValue=p=>p.Copper_Mem_Get},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="银卡", FuncGetValue=p=>p.Silver_Mem_Get},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="金卡", FuncGetValue=p=>p.Gold_Mem_Get},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="白金卡会员", FuncGetValue=p=>p.Platinum_Mem_Get},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="合计", FuncGetValue=p=>p.Total_Mem_Get},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="普通会员", FuncGetValue=p=>p.Com_Mem_Cost},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="铜卡", FuncGetValue=p=>p.Copper_Mem_Cost},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="银卡", FuncGetValue=p=>p.Silver_Mem_Cost},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="金卡", FuncGetValue=p=>p.Gold_Mem_Cost},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="白金卡会员", FuncGetValue=p=>p.Platinum_Mem_Cost},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="合计", FuncGetValue=p=>p.Total_Mem_Cost},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="普通会员", FuncGetValue=p=>p.Com_Mem_Left},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="铜卡", FuncGetValue=p=>p.Copper_Mem_Left},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="银卡", FuncGetValue=p=>p.Silver_Mem_Left},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="金卡", FuncGetValue=p=>p.Gold_Mem_Left},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="白金卡会员", FuncGetValue=p=>p.Platinum_Mem_Left},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="合计", FuncGetValue=p=>p.Total_Mem_Left},
        //    };
        //    //表头栏格式
        //    List<ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>> headTopSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="",Region=new ExcelDataRegion(0,0,0,2)},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="会员积分发送总量"},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="", Region=new ExcelDataRegion(0,0,3,8)},
        //              new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="会员积分消费总量"},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="", Region=new ExcelDataRegion(0,0,9,14)},
        //                new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="会员积分剩余总量"},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName=""},
        //            new ExcelColumnFormat<sp_Rpt_Mem_IssuingConsumption_Result>{ColumnName="", Region=new ExcelDataRegion(0,0,15,20)}
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportMemIssuingConsumption", false, new object[] { exprChannel, exprArea, exprCity, exprStore, exprDtStart, exprDtEnd });
        //        List<sp_Rpt_Mem_IssuingConsumption_Result> list = result.IsPass ? JsonHelper.Deserialize<List<sp_Rpt_Mem_IssuingConsumption_Result>>(result.Obj[0].ToString())
        //            : new List<sp_Rpt_Mem_IssuingConsumption_Result>();
        //        string excelName = result.IsPass ? "会员积分发放和消费统计" : result.MSG;
        //        return ExportToExcel(list, mainSheetFormats, headTopSheetFormats, excelName);
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员积分发放和消费导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        //#region 会员消费明细
        //public ActionResult MemConsumerDetails()
        //{

        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];

        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];

        //    //城市
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];

        //    return View();
        //}

        ///// <summary>
        ///// 查询消费明细
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">门店</param>
        ///// <param name="dateConsumptionStart">消费起期</param>
        ///// <param name="dateConsumptionEnd">消费止期</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetMemConsumerDetails(string channel, string area, string city, string store, DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMemConsumerDetails", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, dateConsumptionStart, dateConsumptionEnd });
        //    Log4netHelper.WriteInfoLog(rst.Obj[0].ToString());
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出消费明细
        ///// </summary>
        ///// <param name="exprChannel"></param>
        ///// <param name="exprArea"></param>
        ///// <param name="exprCity"></param>
        ///// <param name="exprStore"></param>
        ///// <param name="exprDtReg"></param>
        ///// <param name="exprDtRegMon"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMemConsumerDetails(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtReg, DateTime? exprDtRegMon)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="消费日期", FuncGetValue=p=>p.ConsumptionDate},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="消费店铺", FuncGetValue=p=>p.ConsumptionStore},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="消费渠道", FuncGetValue=p=>p.ConsumptionChannel}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="手机号", FuncGetValue=p=>p.Mobile},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="姓名", FuncGetValue=p=>p.Name},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="单据号", FuncGetValue=p=>p.No},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="数量", FuncGetValue=p=>p.Num},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="标准金额", FuncGetValue=p=>p.StandardAmountSales},
        //           new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="订单金额", FuncGetValue=p=>p.TradeAmoutSales},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="应付金额", FuncGetValue=p=>p.PayMoney},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="折扣金额", FuncGetValue=p=>p.DiscountMoney},
        //            new ExcelColumnFormat<sp_Rpt_Mem_ConsumerDetails_Result>{ColumnName="结算金额", FuncGetValue=p=>p.SettleMoney},

        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportMemConsumerDetails", false, new object[] { exprChannel, exprArea, exprCity, exprStore, exprDtReg, exprDtRegMon });


        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_Mem_ConsumerDetails_Result> list = new List<sp_Rpt_Mem_ConsumerDetails_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_Mem_ConsumerDetails_Result> list = JsonHelper.Deserialize<List<sp_Rpt_Mem_ConsumerDetails_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员消费明细导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员消费明细导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }

        //}
        //#endregion

        //#region 会员贡献率统计

        //public ActionResult MemContributionRate()
        //{
        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    //店铺
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    //当前日期
        //    ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd");
        //    //门店
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    var customerLevels = Submit("Member360", "GetBizOptionsMemberLevel", false, new object[] { "CustomerLevel", Auth.DataGroupID, true });
        //    ViewBag.CustomerLevels = customerLevels.Obj[0];
        //    return View();
        //}

        //public JsonResult GetContributionRate(DateTime? yearDate, DateTime? monthDate, string channel, string area, string city, string store, string customerlevel)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var result = Submit("Report", "GetContributionRate", false, new object[] { JsonHelper.Serialize(dts), yearDate, monthDate, channel, area, city, store, customerlevel });
        //    return Json(result.Obj[0]);
        //}
        //[HttpPost]
        //public FileResult ExportContributionRate(DateTime? exprMonthDate, DateTime? exprYearDate, string channel, string area, string city, string store, string customerlevel)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="会员等级", FuncGetValue=p=>p.MemberLevel},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="消费额", FuncGetValue=p=>p.Spending},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="消费额同比", FuncGetValue=p=>p.SpendingTB}, 
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="消费额环比", FuncGetValue=p=>p.SpendingHB},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="客单价", FuncGetValue=p=>p.GuestUnitPrice},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="客单价同比", FuncGetValue=p=>p.GuestUnitPriceTB},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="客单价环比", FuncGetValue=p=>p.GuestUnitCountHB},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="客单量", FuncGetValue=p=>p.GuestUnitCount},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="客单量同比", FuncGetValue=p=>p.GuestUnitCountTB},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="客单价环比", FuncGetValue=p=>p.GuestUnitCountHB},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="消费增长率", FuncGetValue=p=>p.SpendingRiseRate},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="线下消费贡献率(占会员)", FuncGetValue=p=>p.ContributionRateMember},
        //            new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="线下消费贡献率(占总体)", FuncGetValue=p=>p.ContributionRateTotal}

        //    };
        //    //new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="总计消费额", FuncGetValue=p=>p.SpendingTotal},
        //    //new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="总计消费增长率", FuncGetValue=p=>p.SpendingTotalRiseRate},
        //    //new ExcelColumnFormat<sp_Rpt_MemContributionRate_Result>{ColumnName="总计消费贡献率", FuncGetValue=p=>p.SpendingTotalConRate}};

        //    try
        //    {
        //        var result = Submit("Report", "ExportContributionRate", false, new object[] { exprYearDate, exprMonthDate, channel, area, city, store, customerlevel });
        //        List<sp_Rpt_MemContributionRate_Result> list = result.IsPass ? JsonHelper.Deserialize<List<sp_Rpt_MemContributionRate_Result>>(result.Obj[0].ToString())
        //            : new List<sp_Rpt_MemContributionRate_Result>();
        //        string excelName = result.IsPass ? "会员贡献率统计导出" : result.MSG;
        //        return ExportToExcel(list, mainSheetFormats, null, excelName);
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员贡献率统计异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        public FileResult ExportToExcel<T>(IList<T> list, List<ExcelColumnFormat<T>> mainSheetFormats,
            List<ExcelColumnFormat<T>> headSheetFormats, string excelName) where T : class
        {
            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, headSheetFormats);
            var stream = ExcelHelper.GetStream(workBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, excelName + DateTime.Now.ToShortDateString() + ".xls");
        }

        //#region 会员重复消费统计


        //public ActionResult MemRepeatedConsumption()
        //{
        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    //城市
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd");
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult GetMemRepeatedConsumption(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var result = Submit("Report", "GetMemRepeatedConsumption", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, dateStart, dateEnd });
        //    return Json(result.Obj[0]);
        //}

        ///// <summary>
        ///// 导出
        ///// </summary>
        ///// <param name="exprDtStart"></param>
        ///// <param name="exprDtEnd"></param>
        ///// <param name="exprChannel"></param>
        ///// <param name="exprArea"></param>
        ///// <param name="exprCity"></param>
        ///// <param name="exprStore"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportRepeatedConsumption(DateTime? exprDtStart, DateTime? exprDtEnd, string exprChannel, string exprArea, string exprCity, string exprStore)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>> mainSheetFormats = new List
        //        <ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>>
        //    {
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="月份",FuncGetValue =p=>p.MonthDate},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="首次消费金额",FuncGetValue =p=>p.FirstSpendMoney},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="首次消费交易数",FuncGetValue =p=>p.FirstSpendTradeNumber},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="重复消费金额",FuncGetValue =p=>p.RepeatedSpendMoney},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="重复消费交易数",FuncGetValue =p=>p.RepeatedSpendTradeNumber},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="重复消费人数",FuncGetValue =p=>p.RepeatedSpendPeople},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="总消费金额",FuncGetValue =p=>p.TotalSpendMoney},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="总交易人数（去重）",FuncGetValue =p=>p.TotalTradePeople},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="总交易数",FuncGetValue =p=>p.TotalTradeNumber},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="首次消费客单价",FuncGetValue =p=>p.FristTradeGuestUnitPrice},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="重复消费客单价",FuncGetValue =p=>p.RepeatedTradeGuestUnitPrice},
        //        new ExcelColumnFormat<sp_Rpt_MemRepeatedConsumption_Result>{ColumnName ="重复消费占比",FuncGetValue =p=>p.RepeatedTradeProportion}
        //        };
        //    try
        //    {
        //        var result = Submit("Report", "ExportRepeatedConsumption", false, new object[] { exprDtStart, exprDtEnd, exprChannel, exprArea, exprCity, exprStore });
        //        List<sp_Rpt_MemRepeatedConsumption_Result> list = result.IsPass ? JsonHelper.Deserialize<List<sp_Rpt_MemRepeatedConsumption_Result>>(result.Obj[0].ToString()) : new List<sp_Rpt_MemRepeatedConsumption_Result>();
        //        string excelName = result.IsPass ? "会员重复消费统计导出" : result.MSG;
        //        return ExportToExcel(list, mainSheetFormats, null, excelName);
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员重复消费统计异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        //#region 会员非会员销售占比统计
        //public ActionResult MemToNonMemSalesDutyCount()
        //{
        //    return View();
        //}

        ///// <summary>
        ///// 查询会员非会员销售占比统计
        ///// </summary>
        ///// <param name="dateConsumptionStart">消费起期</param>
        ///// <param name="dateConsumptionEnd">消费止期</param>
        ///// <param name="customerSource">消费来源</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetMemToNonMemSalesDutyCount(DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd, string customerSource)
        //{
        //    if (dateConsumptionEnd.HasValue)
        //    {
        //        dateConsumptionEnd = dateConsumptionEnd.Value.AddMonths(1).AddSeconds(-1);//设置为下月的第一天 查询小于此日期的
        //    }
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMemToNonMemSalesDutyCount", false, new object[] { JsonHelper.Serialize(dts), dateConsumptionStart, dateConsumptionEnd, customerSource });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出会员非会员销售占比统计
        ///// </summary>
        ///// <param name="expDtConsumptionStart">消费起期</param>
        ///// <param name="expDtConsumptionEnd">消费止期</param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMemToNonMemSalesDutyCount(DateTime? expDtConsumptionStart, DateTime? expDtConsumptionEnd, string expdrCustomeSource)
        //{
        //    if (expDtConsumptionEnd.HasValue)
        //    {
        //        expDtConsumptionEnd = expDtConsumptionEnd.Value.AddMonths(1).AddSeconds(-1);//设置为下月的第一天 查询小于此日期的
        //    }

        //    List<ExcelColumnFormat<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>{ColumnName="种类", FuncGetValue=p=>p.Type_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>{ColumnName="消费额", FuncGetValue=p=>p.Expendture_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>{ColumnName="消费贡献率（占总体）", FuncGetValue=p=>p.ConsumptionContribution_Mem}, 
        //            new ExcelColumnFormat<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>{ColumnName="总计消费额", FuncGetValue=p=>p.ExpendtureSum_Mem},
        //            new ExcelColumnFormat<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>{ColumnName="总计消费贡献率（占总体）", FuncGetValue=p=>p.ConsumptionContributionSum_Mem}
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportMemToNonMemSalesDutyCount", false, new object[] { expDtConsumptionStart, expDtConsumptionEnd, expdrCustomeSource });

        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result> list = new List<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result> list = JsonHelper.Deserialize<List<sp_Rpt_Mem_MemToNonMemSalesDutyCount_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员非会员销售占比统计导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员非会员销售占比统计异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        //#region 门店消费占比统计
        //public ActionResult StoreConsumptionDutyCount()
        //{


        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];

        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];

        //    //城市
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];

        //    return View();
        //}

        ///// <summary>
        ///// 门店消费占比统计
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetStoreConsumptionDutyCount(string channel, string area, string city, string store, DateTime? searchdateStart, DateTime? searchdateEnd)
        //{
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetStoreConsumptionDutyCount", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, searchdateStart, searchdateEnd });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出门店消费占比统计
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportStoreConsumptionDutyCount(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtStart, DateTime? exprDtEnd)
        //{

        //    List<ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>{ColumnName="城市", FuncGetValue=p=>p.City},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>{ColumnName="店铺", FuncGetValue=p=>p.Store},
        //            //new ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>{ColumnName="会员消费金额", FuncGetValue=p=>p.Expendture_Mem}, 
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>{ColumnName="会员消费金额（区间）", FuncGetValue=p=>p.ConsumptionMoneySum_Mem},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>{ColumnName="门店会员占比", FuncGetValue=p=>p.DutyTotal},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>{ColumnName="总消费金额", FuncGetValue=p=>p.ConsumptionTotalMoney},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionDuty_Count_Result>{ColumnName="门店会员贡献率", FuncGetValue=p=>p.DutyExpendture}
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportStoreConsumptionDutyCount", false, new object[] { exprChannel, exprArea, exprCity, exprStore, exprDtStart, exprDtEnd });

        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_StoreConsumptionDuty_Count_Result> list = new List<sp_Rpt_StoreConsumptionDuty_Count_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_StoreConsumptionDuty_Count_Result> list = JsonHelper.Deserialize<List<sp_Rpt_StoreConsumptionDuty_Count_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "门店消费占比统计导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "门店消费占比统计异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        //#region 门店消费月统计

        ///// <summary>
        ///// 门店消费月统计
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult GetStoreConsumptionMonthlyCount()
        //{
        //    //渠道 
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    //店铺
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    //渠道 
        //    //var chan = Submit("Service", "GetBizOptionsByType", false, new object[] { "StoreChannel", true });
        //    //ViewBag.Channels = chan.Obj[0];
        //    ////大区
        //    //var area = Submit("Service", "GetBizOptionsByType", false, new object[] { "StoreArea", true });
        //    //ViewBag.Areas = area.Obj[0];

        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];

        //    //城市
        //    //  var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];

        //    return View();
        //}

        ///// <summary>
        ///// 门店消费月统计
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">门店</param>
        ///// <param name="consumptiondateStart">消费起期</param>
        ///// <param name="consumptiondateEnd">消费止期</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetStoreConsumptionMonthlyCount(string channel, string area, string city, string store, DateTime? consumptiondateStart, DateTime? consumptiondateEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetStoreConsumptionMonthlyCount", false, new object[] { JsonHelper.Serialize(dts), channel, area, city, store, consumptiondateStart, consumptiondateEnd });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出门店消费统计
        ///// </summary>
        ///// <param name="exprChannel"></param>
        ///// <param name="exprArea"></param>
        ///// <param name="exprCity"></param>
        ///// <param name="exprStore"></param>
        ///// <param name="exprDtConsumptionStart"></param>
        ///// <param name="exprDtConsumptionEnd"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportStoreConsumptionMonthlyCount(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtConsumptionStart, DateTime? exprDtConsumptionEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="城市", FuncGetValue=p=>p.City},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="店铺", FuncGetValue=p=>p.Store},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="总销售额", FuncGetValue=p=>p.TotalSales}, 
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="会员销售额", FuncGetValue=p=>p.AmountOfSales_Mem},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="会员销售贡献率", FuncGetValue=p=>p.SalesContribution_Mem},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="会员销售环比", FuncGetValue=p=>p.SalesAmplificationHB_Mem},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="会员销售同比", FuncGetValue=p=>p.TurnoverRate_Mem},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="会员客单价", FuncGetValue=p=>p.GuestPrice_Mem},
        //            new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="会员客单量", FuncGetValue=p=>p.GuestOrders_Mem}
        //            //,
        //            //new ExcelColumnFormat<sp_Rpt_StoreConsumptionMonthly_Count_Result>{ColumnName="会员交易额增长率", FuncGetValue=p=>p.TurnoverRate_Mem}
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportStoreConsumptionMonthlyCount", false, new object[] { exprChannel, exprArea, exprCity, exprStore, exprDtConsumptionStart, exprDtConsumptionEnd });

        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_StoreConsumptionMonthly_Count_Result> list = new List<sp_Rpt_StoreConsumptionMonthly_Count_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_StoreConsumptionMonthly_Count_Result> list = JsonHelper.Deserialize<List<sp_Rpt_StoreConsumptionMonthly_Count_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "门店消费月统计导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "门店消费月统计异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}

        //#endregion

        //#region  价格段分布（线下）
        //public ActionResult PriceSegmentDistributionOffline()
        //{

        //    int dataGroupId = Convert.ToInt32(Auth.DataGroupID);
        //    var channels = Submit("Service", "GetBaseDataChannel", false, new object[] { dataGroupId });
        //    ViewBag.Channels = channels.Obj[0];
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult GetPriceSegmentDistributionOffline(DateTime? dateStart, DateTime? dateEnd, string channel, string area, string city, string store)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var result = Submit("Report", "GetPriceSegmentDistributionOffline", false, new object[] { dateStart, dateEnd, channel, area, city, store ,JsonHelper.Serialize(dts) });
        //    return Json(result.Obj[0]);
        //}

        //[HttpPost]
        //public FileResult ExportPriceSegmentDistributionOffline(string exprChannel, DateTime? exprDtStart, DateTime? exprDtEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>>
        //    {
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="区域",FuncGetValue =p=>p.Area},
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="100以下",FuncGetValue =p=>p.UnderHundred},
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="100-300",FuncGetValue =p=>p.HundredOneToThree},
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="300-600",FuncGetValue =p=>p.HundredThreeToSix},
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="600-1200",FuncGetValue =p=>p.HundredSixToTwelve},
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="1200-2000",FuncGetValue =p=>p.HundredTwelveToTwenty},
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="2000-4000",FuncGetValue =p=>p.HundredTwentyToForty},
        //        new  ExcelColumnFormat<sp_Rpt_PriceSegmentDistributionOffline_Result>{ColumnName ="4000以上",FuncGetValue =p=>p.OverFortyHundred},
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportPriceSegmentDistributionOffline", false, new object[] { exprDtStart, exprDtEnd, exprChannel });
        //        List<sp_Rpt_PriceSegmentDistributionOffline_Result> list = result.IsPass ? JsonHelper.Deserialize<List<sp_Rpt_PriceSegmentDistributionOffline_Result>>(result.Obj[0].ToString())
        //            : new List<sp_Rpt_PriceSegmentDistributionOffline_Result>();
        //        string excelName = result.IsPass ? "价格段分布（线下）导出" : result.MSG;
        //        return ExportToExcel(list, mainSheetFormats, null, excelName);
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "价格段分布（线下）导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        //#region 优惠券使用统计

        ///// <summary>
        ///// 优惠券使用统计
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult UseCouponCount()
        //{
        //    //券名称 
        //    var coupon = Submit("Report", "GetBizCouponOptionsByDataGroupAndType", false, new object[] { "Coupon", Auth.DataGroupID });
        //    ViewBag.Coupon = coupon.Obj[0];
        //    //大区
        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    return View();
        //}

        ///// <summary>
        /////优惠券使用统计
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetUseCouponCount(string area, string city, string store, string couponname, DateTime? sendCoupondateStart, DateTime? sendCoupondateEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetUseCouponCount", false, new object[] { JsonHelper.Serialize(dts), area, city, store, couponname, sendCoupondateStart, sendCoupondateEnd });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出优惠券使用统计
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportUseCouponCount(string exprArea, string exprCity, string exprStore, string exprCouponName, DateTime? exprDtSendCouponStart, DateTime? exprDtSendCouponEnd)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_UseCoupon_Count_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_UseCoupon_Count_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_UseCoupon_Count_Result>{ColumnName="券名称", FuncGetValue=p=>p.CouponName},
        //            new ExcelColumnFormat<sp_Rpt_UseCoupon_Count_Result>{ColumnName="发送数量", FuncGetValue=p=>p.SendNumber},
        //            new ExcelColumnFormat<sp_Rpt_UseCoupon_Count_Result>{ColumnName="领用数量", FuncGetValue=p=>p.ReceiveNumber}, 
        //            new ExcelColumnFormat<sp_Rpt_UseCoupon_Count_Result>{ColumnName="使用数量", FuncGetValue=p=>p.UseNumber}, 
        //            new ExcelColumnFormat<sp_Rpt_UseCoupon_Count_Result>{ColumnName="使用率", FuncGetValue=p=>p.UseRate},
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportUseCouponCount", false, new object[] { exprArea, exprCity, exprStore, exprCouponName, exprDtSendCouponStart, exprDtSendCouponEnd });

        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_UseCoupon_Count_Result> list = new List<sp_Rpt_UseCoupon_Count_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_UseCoupon_Count_Result> list = JsonHelper.Deserialize<List<sp_Rpt_UseCoupon_Count_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "优惠券使用统计导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "优惠券使用统计异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}
        //#endregion

        //#region 市场活动跟踪

        ///// <summary>
        ///// 市场活动跟踪
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MarketActivityTracking()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult GetMarketActivityTracking(DateTime? startDate, DateTime? endDate)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMarketActivityTracking", false, new object[] { JsonHelper.Serialize(dts), startDate, endDate });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出市场活动跟踪
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMarketActivityTracking(DateTime? expStartDate, DateTime? expEndedDate)
        //{
        //    List<ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>> mainSheetFormats = new List<ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>>{
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="活动时间", FuncGetValue=p=>p.ActDate},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="活动名称", FuncGetValue=p=>p.ActName},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="细分会员群数量", FuncGetValue=p=>p.ConsumerNumber}, 
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="短息推送数量", FuncGetValue=p=>p.SmsNumber},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="回归销售额累计", FuncGetValue=p=>p.TotActMoney},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="占总体销售额比（会员和非会员）", FuncGetValue=p=>p.TotActRate},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="占总体买单数比", FuncGetValue=p=>p.TradeRate},
        //            //new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="总体买单数环比", FuncGetValue=p=>p.OccupyOverallPayHB},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="男性会员数", FuncGetValue=p=>p.ManNumber},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="女性会员数", FuncGetValue=p=>p.WomenNumber},
        //                    new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="保密会员数", FuncGetValue=p=>p.SecretNumber},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="18-22周岁会员数", FuncGetValue=p=>p.Age1Number},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="23-29周岁会员数", FuncGetValue=p=>p.Age2Number},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="30-35周岁会员数", FuncGetValue=p=>p.Age3Number},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="35-39周岁会员数", FuncGetValue=p=>p.Age4Number},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="40+周岁会员数", FuncGetValue=p=>p.Age5Number},
        //            new ExcelColumnFormat<sp_Rpt_MarketActivityTracking_Result>{ColumnName="未知年龄会员数", FuncGetValue=p=>p.UnknownNumber},
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportMarketActivityTracking", false, new object[] { expStartDate, expEndedDate });

        //        if (result.IsPass == false)
        //        {
        //            List<sp_Rpt_MarketActivityTracking_Result> list = new List<sp_Rpt_MarketActivityTracking_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<sp_Rpt_MarketActivityTracking_Result> list = JsonHelper.Deserialize<List<sp_Rpt_MarketActivityTracking_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "市场活动跟踪导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "市场活动跟踪导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}

        //#endregion

        //#region 会员签到统计
        //public ActionResult WeChartSignin()
        //{
        //    ViewBag.Actions = Submit("Report", "GetWXAction", false, new object[] { }).Obj[0];
        //    return View();
        //}

        //public JsonResult GetWXReport(string StartTime, string EndTime, string ActCode)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetWXReport", false, new object[] { JsonHelper.Serialize(dts), StartTime, EndTime, ActCode });
        //    return Json(rst.Obj[0].ToString());
        //}

        //public JsonResult GetWXbyActionCode(string startDate, string endDate, string ActionCode)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetWXbyActionCode", false, new object[] { JsonHelper.Serialize(dts), startDate, endDate, ActionCode });
        //    return Json(rst.Obj[0].ToString());
        //}

        //[HttpPost]
        //public FileResult ExportWxSignDetail(string expractionCode, string exprstartdate, string exprenddate)
        //{
        //    List<ExcelColumnFormat<WxSign>> mainSheetFormats = new List<ExcelColumnFormat<WxSign>>{
        //            new ExcelColumnFormat<WxSign>{ColumnName="手机号", FuncGetValue=p=>p.Mobile},
        //            new ExcelColumnFormat<WxSign>{ColumnName="微信号", FuncGetValue=p=>p.OpenID}, 
        //            new ExcelColumnFormat<WxSign>{ColumnName="签到时间", FuncGetValue=p=>p.SignDate},
        //            new ExcelColumnFormat<WxSign>{ColumnName="是否会员", FuncGetValue=p=>p.IsMem},
        //            new ExcelColumnFormat<WxSign>{ColumnName="是否本次活动", FuncGetValue=p=>p.IsAttendActive}
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "GetWxSignDetail", false, new object[] { expractionCode, exprstartdate, exprenddate });
        //        if (result.IsPass == false)
        //        {
        //            List<WxSign> list = new List<WxSign>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<WxSign> list = JsonHelper.Deserialize<List<WxSign>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员签到详情导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "会员签到详情导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }

        //}

        //#endregion

        //#region 数据中心

        //#region 产品数据分析

        //public ActionResult ProductDataAnalyze()
        //{
        //    return View();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetProductDataAnalyze(string ProductName, string ProductCode, string BrandName, string LineName1, string LineName2)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetProductDataAnalyze", false, new object[] { JsonHelper.Serialize(dts), ProductName, ProductCode, BrandName, LineName1, LineName2 });
        //    return Json(rst.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 导出
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportProductDataAnalyze(string ProductName, string ProductCode, string BrandName, string LineName1, string LineName2)
        //{
        //    List<ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>> mainSheetFormats = new List<ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>>{
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品代码", FuncGetValue=p=>p.ProductCode},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品名称", FuncGetValue=p=>p.ProductName},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="pos产品编号", FuncGetValue=p=>p.ProductCode_IPOS}, 
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品线1代码", FuncGetValue=p=>p.ProductLineCode1},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品线1名称", FuncGetValue=p=>p.ProductLineName1Base},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品线2代码", FuncGetValue=p=>p.ProductLineCode2},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品线2名称", FuncGetValue=p=>p.ProductLineName2Base},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品线3代码", FuncGetValue=p=>p.ProductLineCode3},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="产品线3名称", FuncGetValue=p=>p.ProductLineName3},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="大类代码", FuncGetValue=p=>p.CategoryCode},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="大类名称", FuncGetValue=p=>p.CategoryName},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="子类代码", FuncGetValue=p=>p.SubCategoryCode},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="子类名称", FuncGetValue=p=>p.SubCategoryName},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="原始售价", FuncGetValue=p=>p.OrginalSellPrice},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="停止使用", FuncGetValue=p=>p.StopUsageFlag},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="品牌代码", FuncGetValue=p=>p.ProductBrandCode},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="品牌名称", FuncGetValue=p=>p.ProductBrandNameBase},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="状态代码", FuncGetValue=p=>p.ProductStatusCode},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="状态名称", FuncGetValue=p=>p.ProductStatusNameBase},
        //            new ExcelColumnFormat<Rpt_ProductDataAnalyze_Result>{ColumnName="同步标志", FuncGetValue=p=>p.ProductSyncFlag}
        //    };
        //    try
        //    {
        //        var result = Submit("Report", "ExportProductDataAnalyze", false, new object[] { ProductName, ProductCode, BrandName, LineName1, LineName2 });

        //        if (result.IsPass == false)
        //        {
        //            List<Rpt_ProductDataAnalyze_Result> list = new List<Rpt_ProductDataAnalyze_Result>();
        //            var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            List<Rpt_ProductDataAnalyze_Result> list = JsonHelper.Deserialize<List<Rpt_ProductDataAnalyze_Result>>(result.Obj[0].ToString());
        //            HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "产品数据分析导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MemoryStream strm = new MemoryStream();
        //        return File(strm, "application/vnd.ms-excel", "产品数据分析导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
        //    }
        //}

        //#endregion

        //#endregion

        //#region 优惠券明细

        ///// <summary>
        ///// 优惠券明细
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult CouponDetail()
        //{

        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    var grade = Submit("Service", "GetMemberGrade", false);
        //    ViewBag.Grade = grade.Obj[0];
        //    return View();


        //}

        ///// <summary>
        /////优惠券明细
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetCouponDetail(string area, string city, string store, string grade, string channel, DateTime? sendCoupondateStart, DateTime? sendCoupondateEnd)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetCouponDetail", false, new object[] { JsonHelper.Serialize(dts), area, city, store, grade, channel, sendCoupondateStart, sendCoupondateEnd });
        //    return Json(rst.Obj[0].ToString());
        //}


        //#endregion

        //#region 市场活动明细

        ///// <summary>
        ///// 市场活动明细
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MarketActivityDetail()
        //{

        //    var area = Submit("Service", "GetStoreArea", false);
        //    ViewBag.Areas = area.Obj[0];
        //    var city = Submit("Service", "GetReportCity", false);
        //    ViewBag.City = city.Obj[0];
        //    var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
        //    ViewBag.Stores = stores.Obj[0];
        //    var chan = Submit("Service", "GetStoreChannels", false);
        //    ViewBag.Channels = chan.Obj[0];
        //    return View();


        //}

        ///// <summary>
        /////市场活动明细
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetMarketActivityDetail(string area, string city, string store, string channel, DateTime? marStarttime, DateTime? marEndtime, DateTime? comStarttime, DateTime? comEndtime)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Report", "GetMarketActivityDetail", false, new object[] { JsonHelper.Serialize(dts), area, city, store, channel, marStarttime, marEndtime, comStarttime, comEndtime });
        //    return Json(rst.Obj[0].ToString());
        //}


        //#endregion
    }
}

        #endregion