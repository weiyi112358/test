using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Report.EF;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace Arvato.CRM.BizLogic
{
    public class Report
    {

        #region 结算分摊统计
        /// <summary>
        /// 结算分摊统计 sp_Rpt_StoreConsumptionDuty_Count
        /// </summary>
        /// <param name="dateConsumptionStart">消费起期</param>
        /// <param name="dateConsumptionEnd">消费止期</param>
        /// <returns></returns>
        public static Result GetSettlementShareList(string dp, string store)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    ////调用存储过程
                    var query = db.sp_Rpt_Mem_GoldCoinShareDetail(store,
                         myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = (int)recordCount.Value;
                    res.aaData = query;
                    return new Result(true, "", new List<object> { res });
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }
        }

        /// <summary>
        /// 折扣率列表
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result DiscountRateQuery(string dp, DateTime? begin, DateTime? end)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    ////调用存储过程
                    var query = db.sp_Rpt_DiscountRate(begin, end).ToList();

                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = 1;
                    res.aaData = query;
                    return new Result(true, "", new List<object> { res });
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }
        }

        /// <summary>
        /// 折扣率导出
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result ExportDiscountRate(DateTime? begin, DateTime? end)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    var query = db.sp_Rpt_DiscountRate(begin, end).ToList();
                    return new Result(true, "", query);
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }

        }

        #endregion

        #region 首次消费占比
        /// <summary>
        /// 首次消费占比列表
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result FirstConsumptionProportionQuery(string dp, string txtMobile, string txtGroupID, DateTime? begin, DateTime? end)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    ////调用存储过程
                    var query = db.sp_Rpt_FirstConsumptionProportion(txtGroupID, txtMobile, begin, end).ToList();

                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = 1;
                    res.aaData = query;
                    return new Result(true, "", new List<object> { res });
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }
        }

        /// <summary>
        /// 首次消费占比导出
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result ExportFirstConsumptionProportion(string txtMobile, string txtGroupID, DateTime? begin, DateTime? end)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    var query = db.sp_Rpt_DiscountRate(begin, end).ToList();
                    return new Result(true, "", query);
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }

        }
        #endregion

        #region 会员流失率
        /// <summary>
        /// 会员流失率
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result CustomerLostRateQuery(string dp, string txtMobile, string txtStore, DateTime? year)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    ////调用存储过程
                    var query = db.sp_Rpt_CustomerLostRate(year, txtMobile, txtStore).ToList();

                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = 1;
                    res.aaData = query;
                    return new Result(true, "", new List<object> { res });
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }
        }

        /// <summary>
        /// 会员流失率
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result ExportCustomerLostRate(string txtMobile, string txtStore, DateTime? year)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    var query = db.sp_Rpt_CustomerLostRate(year, txtMobile, txtStore).ToList();
                    return new Result(true, "", query);
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }

        }
        #endregion

        #region 续保
        /// <summary>
        /// 续保
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result RenewalInsuranceRateQuery(string dp, string txtMobile, string txtStore, DateTime? year)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    ////调用存储过程
                    var query = db.sp_Rpt_RenewalInsuranceRate(myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = (int)recordCount.Value;
                    res.aaData = query;
                    return new Result(true, "", new List<object> { res });
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }
        }

        /// <summary>
        /// 续保
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result ExportRenewalInsuranceRate(string txtMobile, string txtStore, DateTime? year)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    //var query = db.sp_Rpt_RenewalInsuranceRate(year, txtMobile, txtStore).ToList();
                    var query = new List<sp_Rpt_RenewalInsuranceRate_Result>();
                    return new Result(true, "", query);
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }

        }
        #endregion

        #region 机油搭载率
        /// <summary>
        /// 机油搭载率
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result GetMachineOilCarryRate(string dp, string txtMobile, string txtStore, DateTime? txtStartDate, DateTime? txtEndDate)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    ////调用存储过程
                    var query = db.sp_Rpt_MachineOilCarryRate(txtStartDate, txtEndDate, txtMobile, txtStore).ToList();

                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = 1;
                    res.aaData = query;
                    return new Result(true, "", new List<object> { res });
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }
        }

        /// <summary>
        /// 机油搭载率
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result ExportMachineOilCarryRate(string txtMobile, string txtStore, DateTime? txtStartDate, DateTime? txtEndDate)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    var query = db.sp_Rpt_MachineOilCarryRate(txtStartDate, txtEndDate, txtMobile, txtStore).ToList();
                    return new Result(true, "", query);
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }

        }
        #endregion

        #region 台次维修收入汇总
        /// <summary>
        /// 台次维修收入汇总
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result GetServiceIncomeCount(string dp, string txtMobile, string txtStore, DateTime? txtStartDate, DateTime? txtEndDate)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    ////调用存储过程
                    var query = db.sp_Rpt_MachineOilCarryRate(txtStartDate, txtEndDate, txtMobile, txtStore).ToList();

                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = 1;
                    res.aaData = query;
                    return new Result(true, "", new List<object> { res });
                }
                catch (Exception ex)
                {

                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }
        }

        /// <summary>
        /// 台次维修收入汇总
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Result ExportServiceIncomeCount(string txtMobile, string txtStore, DateTime? txtStartDate, DateTime? txtEndDate)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                    var query = db.sp_Rpt_MachineOilCarryRate(txtStartDate, txtEndDate, txtMobile, txtStore).ToList();
                    return new Result(true, "", query);
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    throw;
                }
            }

        }
        #endregion

        //#region 数据查询
        ///// <summary>
        ///// 获取优惠券
        ///// </summary>
        ///// <param name="optionType"></param>
        ///// <param name="dataGroupId"></param>
        ///// <returns></returns>
        //public static Result GetBizCouponOptionsByDataGroupAndType(string optionType, int DataGroupID)
        //{

        //    using (var db = new CRMReportEntities())
        //    {
        //        var query = from a in db.TM_Act_CommunicationTemplet
        //                    where a.Type.Equals(optionType) && a.DataGroupID == DataGroupID
        //                    select a;
        //        return new Result(true, "", query.ToList());
        //    }
        //}

        ///// <summary>
        ///// 获取品牌
        ///// </summary>
        ///// <param name="optionType"></param>
        ///// <param name="dataGroupId"></param>
        ///// <returns></returns>
        //public static Result GetBizBrandOptionByDataGroupAndType(string optionType, int DataGroupID)
        //{
        //    using (var db = new CRMReportEntities())
        //    {
        //        var query = from a in db.V_M_TM_SYS_BaseData_brand
        //                    where a.BaseDataType.Equals(optionType) && a.DataGroupID == DataGroupID
        //                    select a;
        //        return new Result(true, "", query.ToList());
        //    }
        //}

        //public static Result GetBrand()
        //{
        //    using (var db = new CRMReportEntities())
        //    {
        //        //var query = from a in db.V_M_TM_SYS_BaseData_product
        //        //            group a by new { a.ProductBrandCode, a.ProductBrandNameBase } into g
        //        //            select new { g.Key.ProductBrandCode, g.Key.ProductBrandNameBase };
        //        var query = (from a in db.V_M_TM_SYS_BaseData_product
        //                     select new { a.ProductBrandCode, a.ProductBrandNameBase }).Distinct();
        //        return new Result(true, "", query.ToList());
        //    }
        //}

        //public static Result GetCategory()
        //{
        //    using (var db = new CRMReportEntities())
        //    {
        //        var query = (from a in db.V_M_TM_SYS_BaseData_product
        //                     select new { a.CategoryCode, a.CategoryName }).Distinct();
        //        return new Result(true, "", query.ToList());
        //    }
        //}
        //#endregion

        //#region 会员人数统计
        ///// <summary>
        ///// 会员人数统计
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">店铺</param>
        ///// <param name="dateStart">开始时间</param>
        ///// <param name="dateEnd">结束时间</param>
        ///// <returns></returns>
        //public static Result GetMemCount(string dp, string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            //调用存储过程 
        //            //分页
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_Count(dateStart, dateEnd, channel, area, city, store, myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}

        ///// <summary>
        ///// 导出会员人数统计
        ///// </summary>
        ///// <param name="channel"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //public static Result ExportMemCount(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_Count(dateStart, dateEnd, channel, area, city, store, 0, 100000, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}

        //#endregion

        //#region 会员招募统计

        ///// <summary>
        ///// 会员招募统计 存储过程名称sp_Rpt_Mem_Recruit_Count
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">店铺</param>
        ///// <param name="dateReg">注册日</param>
        ///// <param name="dateRegMon">注册月</param>
        ///// <returns></returns>
        //public static Result GetMemRecruitCount(string dp, string channel, string area, string city, string store, DateTime? dateReg, string dateRegType)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //调用存储过程
        //            var query = db.sp_Rpt_Mem_Recruit_Count(dateRegType, dateReg, channel, area,
        //                city, store, myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = recordCount.Value == DBNull.Value ? 0 : (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出会员招募统计
        ///// </summary>
        ///// <param name="channel"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="exprDtRegType"></param>
        ///// <returns></returns>
        //public static Result ExportMemRecruitCount(string channel, string area, string city, string store, DateTime? dateStart, string exprDtRegType)
        //{

        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_Recruit_Count(exprDtRegType, dateStart, channel, area, city, store, 0, 20000000, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 会员升级分析

        ///// <summary>
        ///// 会员升级分析查询
        ///// </summary>
        ///// <param name="dp"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //public static Result GetMemUpGrade(string dp, string channel,string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            //调用存储过程 
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //分页
                    
        //            var query = db.sp_Rpt_MemUpGrade(dateStart, dateEnd, channel, area, city, store, myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}

        //public static Result ExportMemUpGrade(string channel,string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_MemUpGrade(dateStart, dateEnd, channel,area, city, store, 0, 100000, recordCount).ToList();

        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}

        //#endregion

        //#region 会员月消费区间统计

        ///// <summary>
        ///// 会员升级分析查询
        ///// </summary>
        ///// <param name="dp"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //public static Result GetMemMonthConsum(string dp, string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            //if (dateEnd != null)
        //            //{
        //            //    dateEnd = ((DateTime)dateEnd).AddMonths(1);
        //            //}
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            //调用存储过程 
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //分页
        //            var query = db.sp_Rpt_MemMonthConsum(dateStart, dateEnd, channel, area, city, store, myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
        //            //var query = db.sp_Rpt_MemMonthConsum(dateStart, dateEnd, channel, area, city, store, 0, 100000, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}

        //public static Result ExportMemMonthConsum(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            if (dateEnd != null)
        //            {
        //                dateEnd = ((DateTime)dateEnd).AddMonths(1);
        //            }
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_MemMonthConsum(dateStart, dateEnd, channel, area, city, store, 0, 100000, recordCount).ToList();

        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}



        //#endregion

        //#region  会员积分发放和消费统计

        //public static Result GetMemIssuingConsumption(string dp, string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_IssuingConsumption_Result(dateStart, dateEnd, channel, area, city, store, myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}


        //public static Result ExportMemIssuingConsumption(string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_IssuingConsumption_Result(dateStart, dateEnd, channel, area, city, store, 0, int.MaxValue, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        //#endregion

        //#region 会员贡献率统计

        //public static Result GetContributionRate(string dp, DateTime? yearDate, DateTime? monthDate,string channel,string area,string city,string store,string customerlevel)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_ContributionRate_Statistics(yearDate, monthDate,channel,area,city,store,customerlevel).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    };
        //}

        //public static Result ExportContributionRate(DateTime? exprYearDate, DateTime? exprMonthDate, string channel, string area, string city, string store, string customerlevel)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var result = db.sp_Rpt_Mem_ContributionRate_Statistics(exprYearDate, exprMonthDate, channel, area, city, store, customerlevel).ToList();
        //            return new Result(true, "", result);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 会员消费频次统计

        //public static Result GetStoreByCity(string cityCode)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        var query = (from a in db.V_M_TM_SYS_BaseData_store
        //                     where (cityCode != "" ? (a.CityCodeStore == cityCode) : true)
        //                     select new
        //                   {
        //                       OptionValue = a.StoreCode,
        //                       OptionText = a.StoreName
        //                   }).ToList();
        //        return new Result(true, "", query);
        //    }
        //}

        ///// <summary>
        ///// 查询会员消费频次统计
        ///// </summary>
        ///// <param name="dp"></param>
        ///// <param name="channel">渠道</param>
        ///// <param name="brand">品牌</param>
        ///// <param name="category">品类</param>
        ///// <param name="dateConsumptionStart">消费起日</param>
        ///// <param name="dateConsumptionEnd">消费止日</param>
        ///// <returns></returns>
        //public static Result GetMemConsumptionFreCount(string dp, string channel, string brand, string category, DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //调用存储过程
        //            var query = db.sp_Rpt_Mem_ConsumptionFrequency_Count(channel, brand, category, dateConsumptionStart,
        //                dateConsumptionEnd).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            //res.iDisplayStart = myDp.iDisplayStart;
        //            //res.iDisplayLength = myDp.iDisplayLength;
        //            //res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出会员消费频次统计
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="brand">品牌</param>
        ///// <param name="category">品类</param>
        ///// <param name="dateConsumptionStart">消费起日</param>
        ///// <param name="dateConsumptionEnd">消费止日</param>
        ///// <returns></returns>
        //public static Result ExportMemConsumptionFreCount(string channel, string brand, string category, DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_ConsumptionFrequency_Count(channel, brand, category, dateConsumptionStart, dateConsumptionEnd).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 会员消费明细
        ///// <summary>
        ///// 会员消费明细
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">店铺</param>
        ///// <param name="dateReg">消费起期</param>
        ///// <param name="dateRegMon">消费止期</param>
        ///// <returns></returns>
        //public static Result GetMemConsumerDetails(string dp, string channel, string area, string city, string store, DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            Log4netHelper.WriteInfoLog("ttt");
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //调用存储过程
        //            var query = db.sp_Rpt_Mem_ConsumerDetails(channel, area, city, store, dateConsumptionStart, dateConsumptionEnd,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            Log4netHelper.WriteInfoLog(query.Count.ToString()+query[0].TradeAmoutSales);
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出会员会员消费明细
        ///// </summary>
        ///// <param name="channel"></param>
        ///// <param name="area"></param>
        ///// <param name="city"></param>
        ///// <param name="store"></param>
        ///// <param name="dateStart"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //public static Result ExportMemConsumerDetails(string channel, string area, string city, string store, DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_ConsumerDetails(channel, area, city, store, dateConsumptionStart, dateConsumptionEnd,
        //                 0, 10000, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 会员重复消费统计

        //public static Result GetMemRepeatedConsumption(string dp, string channel, string area, string city, string store, DateTime? dateStart, DateTime? dateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_MemRepeatedConsumption_Result(dateStart, dateEnd, channel, area, city, store, myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = query.Count;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    };

        //    return new Result(true);
        //}

        //public static Result ExportRepeatedConsumption(DateTime? exprDtStart, DateTime? exprDtEnd, string exprChannel, string exprArea, string exprCity, string exprStore)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_MemRepeatedConsumption_Result(exprDtStart, exprDtEnd, exprChannel, exprArea, exprCity, exprStore,
        //                   0, int.MaxValue, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 会员非会员销售占比统计

        ///// <summary>
        ///// 查询会员与非会员的占比统计 sp_Rpt_Mem_MemToNonMemSalesDutyCount
        ///// </summary>
        ///// <param name="dateConsumptionStart">消费起期</param>
        ///// <param name="dateConsumptionEnd">消费止期</param>
        ///// <param name="customerSource">消费来源</param>
        ///// <returns></returns>
        //public static Result GetMemToNonMemSalesDutyCount(string dp, DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd, string customerSource)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //调用存储过程
        //            var query = db.sp_Rpt_Mem_MemToNonMemSalesDutyCount(dateConsumptionStart, dateConsumptionEnd, customerSource).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出会员与非会员销售占比统计 sp_Rpt_Mem_MemToNonMemSalesDutyCount
        ///// </summary>
        ///// <param name="dateConsumptionStart">消费起期</param>
        ///// <param name="dateConsumptionEnd">消费止期</param>
        ///// <param name="customerSource">会员来源</param>
        ///// <returns></returns>
        //public static Result ExportMemToNonMemSalesDutyCount(DateTime? dateConsumptionStart, DateTime? dateConsumptionEnd, string customerSource)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_MemToNonMemSalesDutyCount(dateConsumptionStart, dateConsumptionEnd, customerSource).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 门店消费占比统计
        ///// <summary>
        ///// 门店消费占比统计 sp_Rpt_StoreConsumptionDuty_Count
        ///// </summary>
        ///// <param name="dateConsumptionStart">消费起期</param>
        ///// <param name="dateConsumptionEnd">消费止期</param>
        ///// <returns></returns>
        //public static Result GetStoreConsumptionDutyCount(string dp, string channel, string area, string city, string store, DateTime? searchdateStart, DateTime? searchdateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            ////调用存储过程
        //            var query = db.sp_Rpt_StoreConsumptionDuty_Count(channel, area, city, store, searchdateStart, searchdateEnd,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 门店消费占比统计 sp_Rpt_StoreConsumptionDuty_Count
        ///// </summary>
        ///// <param name="dateConsumptionStart">消费起期</param>
        ///// <param name="dateConsumptionEnd">消费止期</param>
        ///// <returns></returns>
        //public static Result ExportStoreConsumptionDutyCount(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtStart, DateTime? exprDtEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_StoreConsumptionDuty_Count(exprChannel, exprArea, exprCity, exprStore, exprDtStart, exprDtEnd,
        //                    0, 200000, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region   价格段分布（线下）
        //public static Result GetPriceSegmentDistributionOffline(DateTime? dateStart, DateTime? dateEnd, string channel, string area, string city, string store, string dp)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);

        //    using (var db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var result = db.sp_Rpt_PriceSegmentDistributionOffline_Result(dateStart, dateEnd, channel,area,city,store).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.aaData = result;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        //public static Result ExportPriceSegmentDistributionOffline(DateTime? dateStart, DateTime? dateEnd, string channel, string area, string city, string store)
        //{
        //    using (var db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var result = db.sp_Rpt_PriceSegmentDistributionOffline_Result(dateStart, dateEnd, channel, area, city, store).ToList();
        //            return new Result(true, "", result);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}


        //#endregion

        //#region 门店消费月统计
        ///// <summary>
        ///// 查询门店消费月统计
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">门店</param>
        ///// <param name="consumptiondateStart">消费起期</param>
        ///// <param name="consumptiondateEnd">消费止期</param>
        ///// <returns></returns>
        //public static Result GetStoreConsumptionMonthlyCount(string dp, string channel, string area, string city, string store, DateTime? consumptiondateStart, DateTime? consumptiondateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            ////调用存储过程
        //            var query = db.sp_Rpt_StoreConsumptionMonthly_Count(channel, area, city, store, consumptiondateStart, consumptiondateEnd,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出门店消费月统计
        ///// </summary>
        ///// <param name="exprChannel"></param>
        ///// <param name="exprArea"></param>
        ///// <param name="exprCity"></param>
        ///// <param name="exprStore"></param>
        ///// <param name="exprDtConsumptionStart"></param>
        ///// <param name="exprDtConsumptionEnd"></param>
        ///// <returns></returns>
        //public static Result ExportStoreConsumptionMonthlyCount(string exprChannel, string exprArea, string exprCity, string exprStore, DateTime? exprDtConsumptionStart, DateTime? exprDtConsumptionEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_StoreConsumptionMonthly_Count(exprChannel, exprArea, exprCity, exprStore, exprDtConsumptionStart, exprDtConsumptionEnd,
        //                    0, 200000, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 优惠券使用统计
        ///// <summary>
        ///// 查询门店消费月统计
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">门店</param>
        ///// <param name="consumptiondateStart">消费起期</param>
        ///// <param name="consumptiondateEnd">消费止期</param>
        ///// <returns></returns>
        //public static Result GetUseCouponCount(string dp, string area, string city, string store, string couponname, DateTime? consumptiondateStart, DateTime? consumptiondateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            ////调用存储过程
        //            var query = db.sp_Rpt_UseCoupon_Count(area, city, store, couponname, consumptiondateStart, consumptiondateEnd,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            Log4netHelper.WriteInfoLog(query.ToList().Count.ToString());
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }

        //    }
        //}

        ///// <summary>
        ///// 导出门店消费月统计
        ///// </summary>
        ///// <param name="expCouponname">券名称</param>
        ///// <param name="exprArea">大区</param>
        ///// <param name="exprCity">城市</param>
        ///// <param name="exprStore">店铺</param>
        ///// <param name="exprDtConsumptionStart">发券起期</param>
        ///// <param name="exprDtConsumptionEnd">发券止期</param>
        ///// <returns></returns>
        //public static Result ExportUseCouponCount(string exprArea, string exprCity, string exprStore, string expCouponname, DateTime? exprDtConsumptionStart, DateTime? exprDtConsumptionEnd)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_UseCoupon_Count(exprArea, exprCity, exprStore, expCouponname, exprDtConsumptionStart, exprDtConsumptionEnd,
        //                    0, 200000, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //#region 市场活动跟踪

        ///// <summary>
        ///// 市场活动跟踪sp_Rpt_Mem_Activitycount
        ///// </summary>
        ///// <param name="dp"></param>
        ///// <param name="startDate"></param>
        ///// <param name="endDate"></param>
        ///// <returns></returns>
        //public static Result GetMarketActivityTracking(string dp, DateTime? startDate, DateTime? endDate)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            ////调用存储过程
        //            var query = db.sp_Rpt_Mem_Activitycount(startDate, endDate,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出市场活动跟踪
        ///// </summary>
        ///// <returns></returns>
        //public static Result ExportMarketActivityTracking(DateTime? expStartDate, DateTime? expEndedDate)
        //{
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            var query = db.sp_Rpt_Mem_Activitycount(expStartDate, expEndedDate,
        //               0, int.MaxValue, recordCount).ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        //#endregion

        //#region 产品数据分析
        //public static Result GetProductDataAnalyze(string dp, string ProductName, string ProductCode, string BrandName, string LineName1, string LineName2)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);

        //    using (var db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            var query = from a in db.V_M_TM_SYS_BaseData_product
        //                        where (string.IsNullOrEmpty(ProductName) ? true : (a.ProductName.Contains(ProductName))) &&
        //                        (string.IsNullOrEmpty(ProductCode) ? true : (a.ProductCode.Contains(ProductCode))) &&
        //                        (string.IsNullOrEmpty(BrandName) ? true : (a.ProductBrandNameBase.Contains(BrandName))) &&
        //                        (string.IsNullOrEmpty(LineName1) ? true : (a.ProductLineName1Base.Contains(LineName1))) &&
        //                        (string.IsNullOrEmpty(LineName2) ? true : (a.ProductLineName2Base.Contains(LineName2)))
        //                        select a;

        //            var Skipquery = query.OrderBy(p => p.CategoryName).Skip(myDp.iDisplayStart).Take(myDp.iDisplayLength);

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = query.Count();
        //            res.aaData = Skipquery.ToList();

        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出产品数据
        ///// </summary>
        ///// <returns></returns>
        //public static Result ExportProductDataAnalyze(string ProductName, string ProductCode, string BrandName, string LineName1, string LineName2)
        //{
        //    using (var db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            var query = from a in db.V_M_TM_SYS_BaseData_product
        //                        where (string.IsNullOrEmpty(ProductName) ? true : (a.ProductName.Contains(ProductName))) &&
        //                       (string.IsNullOrEmpty(ProductCode) ? true : (a.ProductCode.Contains(ProductCode))) &&
        //                       (string.IsNullOrEmpty(BrandName) ? true : (a.ProductBrandNameBase.Contains(BrandName))) &&
        //                       (string.IsNullOrEmpty(LineName1) ? true : (a.ProductLineName1Base.Contains(LineName1))) &&
        //                       (string.IsNullOrEmpty(LineName2) ? true : (a.ProductLineName2Base.Contains(LineName2)))
        //                        select a;
        //            return new Result(true, "", query.ToList());
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}
        //#endregion

        //#region 二维码签到统计

        //public static Result GetWXAction()
        //{
        //    using (var db = new CRMEntities())
        //    {
        //        try
        //        {
        //            var query = (from p in db.TM_Act_Instance
        //                        join master in db.TM_Act_Master on p.ActivityID equals master.ActivityID into tempmaster
        //                        from q in tempmaster.DefaultIfEmpty()
        //                        join wxsign in db.TL_WX_MemberSign on q.ReferenceNo equals wxsign.ActionCode into tempwxsign
        //                        from m in tempwxsign.DefaultIfEmpty()
        //                        select new
        //                        {
        //                            m.ActionCode,
        //                            m.ActionName
        //                        }).Distinct().ToList();
        //            //var query = (from p in db.TL_WX_MemberSign select new { p.ActionName, p.ActionCode }).Distinct().ToList();
        //            return new Result(true, "", query);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }

        //}

        //public static Result GetWXReport(string dp, string StartTime, string EndTime, string ActCode)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);

        //    using (var db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //调用存储过程
        //            var query = db.sp_Rpt_Prd_WeChatSign(StartTime, EndTime, ActCode,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = string.IsNullOrEmpty(recordCount.Value.ToString()) ? 0 : (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 查询签到详情
        ///// </summary>
        ///// <param name="dp"></param>
        ///// <param name="ActionCode"></param>
        ///// <returns></returns>
        //public static Result GetWXbyActionCode(string dp, string startDate, string endDate, string ActionCode)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (var db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //调用存储过程
        //            var query = db.sp_Rpt_WXSign_Detail(startDate, endDate, ActionCode,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = string.IsNullOrEmpty(recordCount.Value.ToString()) ? 0 : (int)recordCount.Value;
        //            res.aaData = query;
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {
        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }
                
        //    }
        //}


        ///// <summary>
        ///// 用于导出
        ///// </summary>
        ///// <param name="ActionCode"></param>
        ///// <returns></returns>
        //public static Result GetWxSignDetail(string ActionCode, string startDate, string endDate)
        //{
            
        //    using (var db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            //调用存储过程
        //            var query = db.sp_Rpt_WXSign_Detail(startDate, endDate, ActionCode,
        //                 0, 100000, recordCount).ToList();
                    
        //            return new Result(true, "", new List<object> { query });
        //        }
        //        catch (Exception ex)
        //        {
        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }

        //    }
        //}

        //#endregion


        //#region 优惠券明细
        ///// <summary>
        ///// 查询优惠券明细
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">门店</param>
        ///// <param name="grade">会员等级</param>
        ///// <param name="consumptiondateStart">消费起期</param>
        ///// <param name="consumptiondateEnd">消费止期</param>
        ///// <returns></returns>
        //public static Result GetCouponDetail(string dp, string area, string city, string store, string grade, string channel, DateTime? consumptiondateStart, DateTime? consumptiondateEnd)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            ////调用存储过程
        //            var query = db.sp_Rpt_UsedCoupon_Detail(area, city, store, grade, channel, consumptiondateStart, consumptiondateEnd,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            Log4netHelper.WriteInfoLog(query.ToList().Count.ToString());
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }

        //    }
        //}

       
        //#endregion


        //#region 市场活动明细
        ///// <summary>
        ///// 市场活动明细
        ///// </summary>
        ///// <param name="channel">渠道</param>
        ///// <param name="area">大区</param>
        ///// <param name="city">城市</param>
        ///// <param name="store">门店</param>
        ///// <param name="consumptiondateStart">消费起期</param>
        ///// <param name="consumptiondateEnd">消费止期</param>
        ///// <param name="consumptiondateStart">消费起期</param>
        ///// <param name="consumptiondateEnd">消费止期</param>
        ///// <returns></returns>
        //public static Result GetMarketActivityDetail(string dp, string area, string city, string store, string channel, DateTime? marStarttime, DateTime? marEndtime, DateTime? comStarttime, DateTime? comEndtime)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMReportEntities db = new CRMReportEntities())
        //    {
        //        try
        //        {
        //            ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
        //            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
        //            ////调用存储过程
        //            var query = db.sp_Rpt_ActIntance_Count(area, city, store, channel, marStarttime, marEndtime, comStarttime, comEndtime,
        //                 myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

        //            var res = new DatatablesSourceVsPage();
        //            res.iDisplayStart = myDp.iDisplayStart;
        //            res.iDisplayLength = myDp.iDisplayLength;
        //            res.iTotalRecords = (int)recordCount.Value;
        //            res.aaData = query;
        //            Log4netHelper.WriteInfoLog(query.ToList().Count.ToString());
        //            return new Result(true, "", new List<object> { res });
        //        }
        //        catch (Exception ex)
        //        {

        //            Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
        //            throw;
        //        }

        //    }
        //}


        //#endregion
    }
}
