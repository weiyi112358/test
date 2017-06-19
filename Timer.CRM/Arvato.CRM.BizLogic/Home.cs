using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public class Home
    {
        #region 首页KPI统计

        /// <summary>
        /// 销售数量KPI统计
        /// </summary>
        /// <param name="KPIID"></param>
        /// <returns></returns>
        public static Result getOrderKPI(int KPIID)
        { 
            using (CRMEntities db = new CRMEntities())
            {
                string[] a = getKPINameAndValueType(KPIID);

                DateTime starttime = DateTime.Now.AddDays(-15);//取当前日期前的十二个月
               DateTime endtime= DateTime.Now;//结束时间
                string m = a[1];//获取KPI数值类型
               
                string total = GetKPITotal(KPIID, a[1]);
                var querydata = from i in db.TM_CRM_KPIResult
                                where i.KPIID == KPIID && i.ComputeTime >= starttime && i.ComputeTime < endtime
                                orderby i.ComputeTime descending
                                select new
                                {
                                    Year = i.Year,
                                    Month=i.Month,

                                    Day = SqlFunctions.StringConvert((double)i.Month).Trim() + "-" + SqlFunctions.StringConvert((double)i.Day).Trim(),
                                    Count = (KPIID==10008?((double)(m == "3" ? i.IntValue1 : i.DecValue1))/10000:(double)(m == "3" ? i.IntValue1 : i.DecValue1)),
                                };
                var tag = false;
                 var list =new List<object>();
                for (int i = 15; i >= 0; i--)
                {
                    tag = false;
                    var time = DateTime.Now.AddDays(-i);
                    var Day = time.Month.ToString() +"-"+ time.Day;
                     foreach (var item in querydata.ToList())
                     {
                         if (item.Year == time.Year && item.Month == time.Month && item.Day == Day)
                         {
                             list.Add(item);
                             tag = true;
                         }
                     }
                     if (!tag)
                     {
                         var temp = new
                             {
                                 Year = time.Year,
                                 Month = time.Month,

                                 Day = time.Month+"-"+time.Day.ToString(),
                                 Count = 0,
                             };
                         list.Add(temp);
                     }
                }

                //var querydata = from p in db.TM_CRM_KPIResult//获取KPI每月的数值
                //                where p.KPIID == KPIID && ((p.Year == starttime.Year && p.Month > starttime.Month) || p.Year > starttime.Year)
                //                group p by new { Year = p.Year, Month = p.Month } into g
                //                select new { Month = (SqlFunctions.StringConvert((double)g.Key.Year).Trim() + "-" + SqlFunctions.StringConvert((double)g.Key.Month).Trim()), Count = g.Sum(t => m == "3" ? t.IntValue1 : t.DecValue1), actrulyear = g.Key.Year, actrulmonth = g.Key.Month };
                return new Result(true, "", new List<object> { a[0], total, list });
            }
        }

        /// <summary>
        /// 获取近13个月每月最后一天的会员总数
        /// </summary>
        /// <returns></returns>
        public static Result GetMemberIncrease(int KPIID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string kpiname = getKPINameAndValueType(KPIID)[0];

                DateTime starttime = DateTime.Now.AddMonths(-12);//获取当前日期前的十二个月

                //var queryValidNum = (from p in db.TM_CRM_KPIResult//获取有效会员数
                //                     where p.KPIID == KPIID
                //                     orderby p.ComputeTime descending
                //                     select p.IntValue1).Take(1);
                var querydata = (from q in
                                     (from p in db.TM_CRM_KPIResult//获取每月最后一天
                                      where p.KPIID == KPIID && ((p.Year == starttime.Year && p.Month >= starttime.Month) || p.Year > starttime.Year)
                                      group p by new { Year = p.Year, Month = p.Month } into g
                                      select new { g.Key.Year, g.Key.Month, Day = g.Max(t => t.Day) })
                                 join p in db.TM_CRM_KPIResult
                                 on new { Year = q.Year, Month = q.Month, Day = q.Day } equals new { p.Year, p.Month, p.Day } into temp
                                 from o in temp.DefaultIfEmpty()
                                 where o.KPIID == KPIID
                                 select new
                                 {
                                     Month = SqlFunctions.StringConvert((double)o.Year).Trim() + "-" + SqlFunctions.StringConvert((double)o.Month).Trim(),
                                     oMonth = o.Month,
                                     oYear = o.Year,
                                     Count = o.IntValue1,
                                 }).OrderBy(m => m.oYear).ThenBy(m => m.oMonth);

                return new Result(true, "", new List<object> { "会员增长情况", querydata.ToDataTableSource() });
            }
        }

        /// <summary>
        /// 会员客单价
        /// </summary>
        /// <param name="KPIID"></param>
        /// <returns></returns>
        public static Result getCustomerPriceKPI(int KPIID1, int KPIID2)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string[] a = getKPINameAndValueType(KPIID1);
                string[] b = getKPINameAndValueType(KPIID2);
                DateTime starttime = DateTime.Now.AddMonths(-12);//获取当前日期前的十二个月

                string m = a[1];//获取KPI数值类型
                string n = b[1];
                string total = GetKPITotal(KPIID1, a[1]);
                //获取每月销售总金额
                var querysaledata = from p in db.TM_CRM_KPIResult//获取KPI每月的数值
                                    where p.KPIID == KPIID1 && ((p.Year == starttime.Year && p.Month > starttime.Month) || p.Year > starttime.Year)
                                    group p by new { Year = p.Year, Month = p.Month } into g
                                    select new { Month = (SqlFunctions.StringConvert((double)g.Key.Year).Trim() + "-" + SqlFunctions.StringConvert((double)g.Key.Month).Trim()), Count = g.Sum(t => m == "3" ? t.IntValue1 : t.DecValue1), actrulyear = g.Key.Year, actrulmonth = g.Key.Month };
                //获取每月订单总数量
                var queryorderdata = from p in db.TM_CRM_KPIResult//获取KPI每月的数值
                                     where p.KPIID == KPIID2 && ((p.Year == starttime.Year && p.Month > starttime.Month) || p.Year > starttime.Year)
                                     group p by new { Year = p.Year, Month = p.Month } into g
                                     select new { Month = (SqlFunctions.StringConvert((double)g.Key.Year).Trim() + "-" + SqlFunctions.StringConvert((double)g.Key.Month).Trim()), Count = g.Sum(t => n == "3" ? t.IntValue1 : t.DecValue1), actrulyear = g.Key.Year, actrulmonth = g.Key.Month };
                return new Result(true, "", new List<object> { "会员客单价情况", total, querysaledata.OrderBy(p => p.actrulyear).ThenBy(p => p.actrulmonth).ToDataTableSource(), queryorderdata.OrderBy(p => p.actrulyear).ThenBy(p => p.actrulmonth).ToDataTableSource() });
            }
        }

        /// <summary>
        /// 获取新客数量KPI
        /// </summary>
        /// <param name="KPIID"></param>
        /// <returns></returns>
        public static Result getNewCustomerKPI(int KPIID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string[] a = getKPINameAndValueType(KPIID);
                DateTime lastmonth = DateTime.Now;
                string kpiname = a[0];
                string valuetype = a[1];
                DateTime starttime = DateTime.Now;
                //var queryactdata = (from p in db.TM_CRM_KPIResult//获取KPI最后一天的数值
                //                    where p.KPIID == KPIID && p.Month == starttime.Month && p.Year == starttime.Year
                //                    orderby p.ComputeTime descending
                //                    select new { validNum = valuetype == "3" ? p.IntValue1 : p.DecValue1 }).Take(1);
                //var querylastdata = (from p in db.TM_CRM_KPIResult//获取KPI上月的数值
                //                     where p.KPIID == KPIID && p.Month == lastmonth.Month && p.Year == lastmonth.Year
                //                     orderby p.ComputeTime descending
                //                     select new { validNum = valuetype == "3" ? p.IntValue1 : p.DecValue1 }).Take(1);


                var queryactdata = (from i in db.TM_CRM_KPIResult
                                    where i.Year == lastmonth.Year && i.Month == lastmonth.Month && i.KPIID == KPIID 
                                    group i by new {  Year = i.Year, Month = i.Month } into g
                                    select new {
                                    g.Key.Year,
                                    g.Key.Month,
                                    Count = g.Sum(t => valuetype == "3" ? t.IntValue1 : t.DecValue1) 
                                  }.Count).FirstOrDefault();
                var queryactdata1 = (from i in db.TM_CRM_KPIResult
                                    where i.Year == lastmonth.Year && i.KPIID == KPIID 
                                    group i by new { Year = i.Year } into g
                                    select new
                                    {
                                        g.Key.Year,
                                        Count = g.Sum(t => valuetype == "3" ? t.IntValue1 : t.DecValue1)
                                    }.Count).FirstOrDefault();
               return new Result(true, "", new List<object> { kpiname, queryactdata ,queryactdata1});
            }
        }

        public static Result GetOrderNum(int KPIID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string[] a = getKPINameAndValueType(KPIID);
                string total = GetKPIYearTotal(KPIID, a[1]);
                string KPIValueType = a[1];
                var query = from p in db.TM_CRM_KPIResult//获取KPI当月的总数
                            where p.KPIID == KPIID && p.Year == DateTime.Now.Year && p.Month == DateTime.Now.Month
                            group p by p.KPIID into g
                            select new { total = g.Sum(p => KPIValueType == "3" ? p.IntValue1 : p.DecValue1) };

                return new Result(true, "", new List<object> { a[0], total, query.Count() == 0 ? "" : query.ToList()[0].total.ToString() });
            }
        }

        public static Result GetAvgPrice(int KPIIDOrder, int KPIIDPrice)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string[] order = getKPINameAndValueType(KPIIDOrder);
                string kpiordertype = order[1];
                string[] price = getKPINameAndValueType(KPIIDPrice);
                string kpipricetype = price[1];
                var queryorder = (from p in db.TM_CRM_KPIResult
                                 where p.KPIID == KPIIDOrder && p.Year == DateTime.Now.Year
                                 group p by p.KPIID into g
                                 select new
                                 {
                                     sum = g.Sum(p => kpiordertype == "3" ? p.IntValue1 : p.DecValue1)
                                 }).FirstOrDefault();

                var queryprice = (from p in db.TM_CRM_KPIResult
                                 where p.KPIID == KPIIDPrice && p.Year == DateTime.Now.Year
                                 group p by p.KPIID into g
                                 select new
                                 {
                                     sum = g.Sum(p => kpipricetype == "3" ? p.IntValue1 : p.DecValue1)
                                 }).FirstOrDefault();
                string result = queryorder == null ? "" : (queryorder.sum / queryprice.sum).ToString();
                return new Result(true, "", new List<object> { result });
            }
        }

        /// <summary>
        /// 获取指标最近一天的值
        /// </summary>
        /// <param name="KPIID"></param>
        /// <returns></returns>
        public static Result GetMemberActive(int KPIID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string[] a = getKPINameAndValueType(KPIID);
                string kpiname = a[0];
                string valuetype = a[1];
                var queryValidNum = (from p in db.TM_CRM_KPIResult
                                     where p.KPIID == KPIID
                                     orderby p.ComputeTime descending
                                     select new { validNum = valuetype == "3" ? p.IntValue1 : p.DecValue1 }).Take(1);

                return new Result(true, "", new List<object> { kpiname, queryValidNum.Count() == 0 ? "" : queryValidNum.ToList()[0].validNum.ToString() });
            }

        }

        private static string[] getKPINameAndValueType(int KPIID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var querykpi = from p in db.TM_CRM_KPI//获取KPI名称和数值类型
                               where p.KPIID == KPIID
                               select new { p.TargetValueType, p.KPIName };
                string[] a = new string[2];
                a[0] = querykpi.Count() == 0 ? "" : querykpi.ToList()[0].KPIName;
                a[1] = querykpi.Count() == 0 ? "" : querykpi.ToList()[0].TargetValueType;
                return a;
            }
        }

        private static string GetKPITotal(int KPIID, string KPIValueType)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var querytotal = from p in db.TM_CRM_KPIResult//获取KPI截止日前的总数
                                 where p.KPIID == KPIID
                                 group p by p.KPIID into g
                                 select new { total = g.Sum(p => KPIValueType == "3" ? p.IntValue1 : p.DecValue1) };

                return querytotal.Count() == 0 ? "" : querytotal.ToList()[0].total.ToString();
            }
        }

        private static string GetKPIYearTotal(int KPIID, string KPIValueType)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var querytotal = from p in db.TM_CRM_KPIResult//获取KPI截止当年的总值
                                 where p.KPIID == KPIID && p.Year == DateTime.Now.Year
                                 group p by p.KPIID into g
                                 select new { total = g.Sum(p => KPIValueType == "3" ? p.IntValue1 : p.DecValue1) };

                return querytotal.Count() == 0 ? "" : querytotal.ToList()[0].total.ToString();
            }
        }

        public static Result GetKPIYearAVG(int KPIID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string[] a = getKPINameAndValueType(KPIID);
                string kpiname = a[0];
                string valuetype = a[1];
                var queryValidNum = from p in db.TM_CRM_KPIResult
                                    where p.KPIID == KPIID && p.Year == DateTime.Now.Year
                                    group p by p.KPIID into g
                                    select new { validNum = g.Average(p => valuetype == "3" ? p.IntValue1 : p.DecValue1) };

                return new Result(true, "", new List<object> { kpiname, queryValidNum.Count() == 0 ? "" : queryValidNum.ToList()[0].validNum.ToString() });
            }

        }


        public static Result GetSalesKPI(int KPIID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = new ArrayList();
                var namelist = new ArrayList();
                var valuelist = new ArrayList();
                //获取最新一天的strvalus 数据
                
                var querydata = (from i in db.TM_CRM_KPIResult
                                 where i.KPIID == KPIID && (i.StrValue1 != null || i.StrValue1 != "")
                                 select i).OrderByDescending(i => i.ComputeTime).FirstOrDefault();
                if (querydata != null)
                {
                    if (!string.IsNullOrWhiteSpace(querydata.StrValue1))
                    {
                        var temp = querydata.StrValue1.Split(',');
                        foreach (var item in temp)
                        {
                            var str = item.Split('|');
                            var temps = new
                            {
                                value = Convert.ToDouble(str[1]),
                                name = str[0]
                            };
                            namelist.Add(str[0]);
                            valuelist.Add(temps);

                        }
                    }
                }

                list.Add(namelist);
                list.Add(valuelist);
                var kpiname = getKPINameAndValueType(KPIID);
                return new Result(true, kpiname[0], new List<object> { list });
            }
        }
        #endregion
    }
}
