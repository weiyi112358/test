using System.Configuration;
using Arvato.CRM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.Utility;
using Arvato.CRM.MemberSubdivisionLogic;
using System.Net.Http;
using Arvato.CRM.Trigger;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Dynamic;
using Arvato.CRM.Model;
using Arvato.CRM.CommunicateEngine;
using Arvato.CRM.Model.Interface;
using Newtonsoft.Json;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Xml;


namespace Arvato.CRM.UtilityTest
{

    public class QueryGrade : Common
    {

        //会员等级
        public string FGradeType { get; set; }
        //尊享返利
        public string FRebate { get; set; }
        //等级金额
        public decimal FGradeAmount { get; set; }
        //等级有效期
        public DateTime? FGradeDate { get; set; }
        //即将升级相差金额
        public decimal FDifferAmount { get; set; }
    }

    public class Common
    {
        //1 成功 其他出错
        public string Status { get; set; }
        //若状态码为1，则为空
        //若状态码为非1，则为错误信息
        public string ErrorMsg { get; set; }
    }
    /// <summary>
    /// 积分明细查询返回结果
    /// </summary>
    public class QueryPointsDetail : Common
    {
        //积分明细列表
        public List<PointDetail> PointDetailClass { get; set; }

        public QueryPointsDetail()
        {
            PointDetailClass = new List<PointDetail>();
        }
    }

    /// <summary>
    /// 积分明细列表
    /// </summary>
    public class PointDetail
    {
        //积分产生日期
        public DateTime? FProdDate { get; set; }
        //积分产生来源
        public string FProdOrg { get; set; }
        //积分类型
        public string FType { get; set; }
        //消费金额
        public decimal FExpendAmount { get; set; }
        //积分数额
        public decimal FIntegralAmount { get; set; }
        //可用积分
        public decimal FUseIntegral { get; set; }
        //快过期积分
        public decimal FFastExpire { get; set; }
        //过期时间
        public DateTime? FFastDate { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                interfaceJpos();

                //string a = "{\"FGradeType\":\"2\",\"FGradeDate\":\"2018-06-08 09:23:04\",\"FRebate\":\"\",\"FGradeAmount\":500.0,\"FDifferAmount\":500.00,\"Status\":1,\"ErrorMsg\":\"成功\"}";
                //string b = "{\"PointDetailClass\":[{\"FProdDate\":null,\"FProdOrg\":null,\"FType\":\"value1\",\"FExpendAmount\":0.0,\"FIntegralAmount\":0.0,\"FUseIntegral\":-417.0000,\"FFastExpire\":0.0,\"FFastDate\":null},{\"FProdDate\":null,\"FProdOrg\":null,\"FType\":\"value1\",\"FExpendAmount\":0.0,\"FIntegralAmount\":0.0,\"FUseIntegral\":-417.0000,\"FFastExpire\":0.0,\"FFastDate\":null},{\"FProdDate\":null,\"FProdOrg\":null,\"FType\":\"value1\",\"FExpendAmount\":0.0,\"FIntegralAmount\":0.0,\"FUseIntegral\":2115.0000,\"FFastExpire\":0.0,\"FFastDate\":null},{\"FProdDate\":null,\"FProdOrg\":null,\"FType\":\"value1\",\"FExpendAmount\":0.0,\"FIntegralAmount\":0.0,\"FUseIntegral\":417.0000,\"FFastExpire\":0.0,\"FFastDate\":null}],\"Status\":1,\"ErrorMsg\":\"成功\"}";
                //var ttt = JsonHelper.Deserialize<QueryPointsDetail>(b);

                //SmsManager.Send();
                WechatManager.Send();


                //string json = "{\"FCardNum\":\"820000000142\",\"Famount\":\"150\",\"FExpireTime\":\"2018-06-14 23:59:59\",\"FDeductAmount\":\"15\",\"FFastExpire\":\"0\",\"signdate\":\"2017-06-14 15:58:25\",\"secret\":\"61951ef362016b0430552e9b84108dc2\"}";
                //HttpContent httpcontent = new StringContent(json);
                //var httpClient = new HttpClient();

                //var responseJson = httpClient.PostAsync("http://dwpserver-sit.timierhouse.com/CRMApi/UpdatePoints.ashx", httpcontent).Result.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(responseJson.ToString());
                //Console.ReadKey();



                //string json = "{\"FCardNum\":\"820000000142\",\"FGradeType\":\"银卡会员\",\"FRebate\":\"0.09\",\"FGradeAmount\":\"15.00\",\"FGradeDate\":\"2018-06-14 15:59:59\",\"FDifferAmount\":\"1985.00\",\"signdate\":\"2017-06-14 15:58:25\",\"secret\":\"61951ef362016b0430552e9b84108dc2\"}";
                //HttpContent httpcontent = new StringContent(json);
                //var httpClient = new HttpClient();

                //var responseJson = httpClient.PostAsync("http://dwpserver-sit.timierhouse.com/CRMApi/UpdateGrade.ashx", httpcontent).Result.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(responseJson.ToString());
                //Console.ReadKey();


                //string json = "{\"FCardNum\":\"820000000142\",\"LoginName\":\"class584520\",\"FCount\":\"10\",\"signdate\":\"2017-06-14 15:55:59\",\"secret\":\"61951ef362016b0430552e9b84108dc2\"}";
                //HttpContent httpcontent = new StringContent(json);
                //var httpClient = new HttpClient();

                //var responseJson = httpClient.PostAsync("http://dwpserver-sit.timierhouse.com/CRMApi/UpdateRemind.ashx", httpcontent).Result.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(responseJson.ToString());
                //Console.ReadKey();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }

        }


        public static void loy()
        {
            Console.WriteLine("输入1计算积分,输入2同步订单");
            var a = Console.ReadLine();
            try
            {
                using (var db = new CRMEntities())
                {
                    if (a == "1")
                    {

                        db.Database.ExecuteSqlCommand("exec sp_Loy_AccountPointUpdate");
                        Console.WriteLine("执行积分完成");
                        Console.ReadKey();
                    }
                    else if (a == "2")
                    {

                        db.Database.ExecuteSqlCommand("exec sp_ETL_TradeZLExtract");
                        Console.WriteLine("执行订单完成");
                        Console.ReadKey();
                    }
                    else if (a == "3")
                    {
                        var el = new ExtraLoyalty
                            {
                                DataGroupID = 1,
                                //RuleRunType ="1",
                                RuleType = "2"
                            };
                        LoyaltyTrigger tr = new LoyaltyTrigger(el);
                        tr.MemberScript = "Select MemberID From TM_Mem_Master where DataGroupID = 1 ";
                        tr.Ext.SearchTradeDetailSQL = "Select TradeDetailID From TM_Mem_TradeDetail inner join TM_Mem_Trade on TM_Mem_TradeDetail.TradeID = TM_Mem_Trade.TradeID and TM_Mem_Trade.Bool_Attr_2=1 and TM_Mem_Trade.NeedLoyCompute = 1 and DataGroupID = 1 ";
                        tr.Ext.SearchTradeSQL = "Select TradeID From TM_Mem_Trade where NeedLoyCompute = 1 and DataGroupID =  1 ";

                        tr.StartTime = DateTime.Now;
                        //tr.Callback = new LoyaltyTrigger.CallBackMethod(gothisway);
                        tr.Start();
                        Console.WriteLine("执行完成");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("输入有误");
                        Console.ReadKey();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("执行错误" + ex.ToString());
                Console.ReadKey();
            }
        }

        public static void etlCouponByHD()
        {
            using (var db = new CRMEntities())
            {
                db.Database.ExecuteSqlCommand("exec sp_CRM_ETLCouponToCRM");
            }

        }


        public static void syncCoupon()
        {
            int i = 1;
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var query = db.TE_JPOS_CouponUsed.ToList();
                    var querys = db.TM_JPOS_CouponUseRule;
                    foreach (var coupon in query)
                    {
                        Console.WriteLine("开始同步第" + i.ToString() + "条购物券...");
                        DateTime? sd = null;
                        DateTime? ed = null;
                        string logoPath = ""; string couponName = ""; string couponNo = ""; string couponRemark = ""; decimal couponValue = 0;


                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(coupon.DOCUMENT);
                        XmlElement root = doc.DocumentElement;
                        var cld = root.ChildNodes[0];
                        while (cld.HasChildNodes)
                        {
                            cld = cld.ChildNodes[0];
                            if (cld.Attributes["class"] == null) continue;
                            var className = cld.Attributes["class"].Value;
                            switch (className)
                            {
                                case "com.hd123.h5.pom.document.condition.general.DateRangeCondition":
                                    if (cld.Attributes["finish"] != null)
                                        sd = Convert.ToDateTime(cld.Attributes["finish"].Value);
                                    if (cld.Attributes["start"] != null)
                                        ed = Convert.ToDateTime(cld.Attributes["start"].Value);
                                    break;
                                case "com.hd123.h5.pom.document.condition.member.voucher.VoucherActivityCondition":
                                    if (cld.Attributes["logo"] != null)
                                        logoPath = cld.Attributes["logo"].Value;
                                    if (cld.Attributes["name"] != null)
                                        couponName = cld.Attributes["name"].Value;
                                    if (cld.Attributes["no"] != null)
                                        couponNo = cld.Attributes["no"].Value;
                                    if (cld.Attributes["parValue"] != null)
                                        couponValue = Convert.ToDecimal(cld.Attributes["parValue"].Value);
                                    if (cld.Attributes["remark"] != null)
                                        couponRemark = cld.Attributes["remark"].Value;
                                    break;
                                case "com.hd123.h5.pom.document.condition.step.StepCondition":

                                    break;
                                default:
                                    break;
                            }
                        }

                        Console.WriteLine("开始同步第" + i.ToString() + "条购物券 下载图片");
                        string path = @"ftp://192.168.20.31:8001//" + logoPath;    //目标路径
                        string ftpip = "192.168.20.31";    //ftp IP地址
                        string username = "crm_ftp";   //ftp用户名
                        string password = "p@ssw0rd";   //ftp密码
                        FtpHelper ftp = new FtpHelper(path, ftpip, username, password);
                        var files = logoPath.Split('/');
                        var fn = files[files.Count() - 1].ToString();
                        string filePath = System.Environment.CurrentDirectory;
                        Console.WriteLine(filePath + "\\DownLoadCouponLogo\\" + logoPath.Substring(0, logoPath.Length - 1 - fn.Length));
                        ftp.Download(fn, filePath + "\\DownLoadCouponLogo\\" + logoPath.Substring(0, logoPath.Length - 1 - fn.Length));
                        Console.WriteLine("完成同步第" + i.ToString() + "条购物券 下载图片");
                        var c = querys.Where(p => p.CouponNo == coupon.BILLNUMBER).FirstOrDefault();
                        if (c == null)
                        {
                            Console.WriteLine("new");
                            TM_JPOS_CouponUseRule r = new TM_JPOS_CouponUseRule();
                            r.CouponNo = coupon.BILLNUMBER;
                            r.CouponName = couponName;
                            r.CouponRemark = couponRemark;
                            r.CouponValue = couponValue;
                            r.StartDate = sd;
                            r.EndDate = ed;

                            if (cld.HasChildNodes)
                            {
                                cld = cld.ChildNodes[0];
                                var className = cld.Attributes["class"].Value;

                            }
                            r.IsEnble = true;
                            r.AddedDate = DateTime.Now;
                            r.IsDelete = false;
                            r.AddedUser = "1000";
                            r.ModifiedDate = DateTime.Now;
                            r.ModifiedUser = "1000";
                            db.TM_JPOS_CouponUseRule.Add(r);
                        }
                        else
                        {
                            Console.WriteLine("old");
                            c.CouponNo = coupon.BILLNUMBER;
                            c.CouponName = couponName;
                            c.CouponRemark = couponRemark;
                            c.CouponValue = couponValue;
                            c.StartDate = sd;
                            c.EndDate = ed;
                            db.SaveChanges();
                            c.ModifiedDate = DateTime.Now;
                        }
                        Console.WriteLine("完成第" + i.ToString() + "条购物券同步...");
                        i++;
                    }


                    db.SaveChanges();

                    Console.ReadKey();




                    //var a = root.ChildNodes[0].Attributes["finish"];



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("同步第" + i.ToString() + "条购物券出错...");
                Console.WriteLine("错误信息：" + ex.ToString());
                Console.ReadLine();
            }


        }


        public static void interfaceJpos()
        {

            var json = "{\"accountAccessCode\":\"211500005106\",\"accountAccessCodeType\":0,\"ruleUuid\":\"4028949f5bf3cfd0015c4872ee5d03a5_4\",\"time\":\"2017-06-14 22:13:55\",\"operator\":\"spos\",\"storeCode\":\"0900039\",\"tranId\":\"16becbb6e48841998c441abdc38ad83a\",\"trantime\":\"2017-06-14 22:13:55\",\"signDate\":\"2017-06-14 22:13:55\",\"secret\":\"f8d81bca0fc7612fa90e53b8630a0e32\"}";
            HttpContent httpcontent = new StringContent(json);
            // httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();

            var responseJson = httpClient.PostAsync("http://localhost:54383/POST/Prize/QueryMbrJoinRuleCount", httpcontent).Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseJson.ToString());
            Console.ReadKey();
            //var responseJson = httpClient.PostAsync("http://106.75.15.210:8028/api/Wechat/QueryGrade", httpcontent).Result.Content.ReadAsStringAsync().Result;


        }

        public string str = "pppppp";

        public void test()
        {
            System.Reflection.Assembly Ay = System.Reflection.Assembly.LoadFrom("Arvato.CRM.EF.dll");
            Type[] tps = Ay.GetExportedTypes();
            object returnValue = null;

            foreach (Type tp in tps)
            {
                if (tp.Name == "V_U_TM_Mem_Info")
                {
                    tp.GetField("Arvato.CRM.EF.V_M_TM_Mem_SubExt_contact");
                }
                //  returnValue = tp.GetMethod(methodName).Invoke(System.Activator.CreateInstance(tp), parameters);
            }
        }

        public static void update()
        {

            using (CRMEntities db = new CRMEntities())
            {
                var master = db.TM_Mem_Master.ToList();
                foreach (var m in master)
                {
                    var str = m.Str_Key_1.Substring(5);
                    string token = ConfigurationSettings.AppSettings["token"].ToString();
                    var pass = ToolsHelper.MD5(str + token);
                    m.Str_Key_3 = pass;

                }
                db.SaveChanges();

            }
        }




    }
}
