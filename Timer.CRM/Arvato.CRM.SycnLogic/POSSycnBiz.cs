using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.EF;
using Arvato.CRM.Utility.WorkFlow;
using System.Configuration;
using Arvato.CRM.Utility;
using System.Data;
using Arvato.CRM.Utility.WorkFlow.Templates;
using System.Text.RegularExpressions;
using System.Xml;
using Arvato.CRM.Trigger;


namespace Arvato.CRM.SycnLogic
{
    public class POSSycnBiz
    {
        private static string ftpIp = ConfigurationManager.AppSettings["ftpIp"].ToString();//@"ftp://192.168.20.31:8001//"
        private static string ftpSort = ConfigurationManager.AppSettings["ftpSort"].ToString();//@"ftp://192.168.20.31:8001//"
        private static string ftpUser = ConfigurationManager.AppSettings["ftpUser"].ToString();//@"ftp://192.168.20.31:8001//"
        private static string ftpPw = ConfigurationManager.AppSettings["ftpPw"].ToString();//@"ftp://192.168.20.31:8001//"
        private static string couponFilePath = ConfigurationManager.AppSettings["couponFilePath"].ToString();//Logo存放路径（绝对路径）
        public static void syncCoupon()
        {
            int i = 1;

            using (CRMEntities db = new CRMEntities())
            {
                Log4netHelper.WriteInfoLog("从HD抽取购物券信息");
                db.Database.ExecuteSqlCommand("exec sp_CRM_ETLCouponToCRM");
                Log4netHelper.WriteInfoLog("完成HD抽取购物券信息");
                var query = db.TE_JPOS_CouponUsed.ToList();
                var querys = db.TM_JPOS_CouponUseRule;
                foreach (var coupon in query)
                {
                    Log4netHelper.WriteInfoLog("开始同步第" + i.ToString() + "条购物券...");
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

                    Log4netHelper.WriteInfoLog("开始同步第" + i.ToString() + "条购物券 下载图片");
                    string path = @"ftp://" + ftpIp + ":" + ftpSort + "//" + logoPath;    //目标路径
                    string ftpip = ftpIp;    //ftp IP地址
                    string username = ftpUser;   //ftp用户名
                    string password = ftpPw;   //ftp密码
                    FtpHelper ftp = new FtpHelper(path, ftpip, username, password);
                    //取到logo中文件名称 fn
                    var files = logoPath.Split('/');
                    var fn = files[files.Count() - 1].ToString();
                    //读取文件路径
                    //string filePath = System.Environment.CurrentDirectory;
                    string filePath = couponFilePath;
                    string UploadPath = filePath + "\\" + logoPath.Substring(0, logoPath.Length - 1 - fn.Length);

                    var re = ftp.Download(fn, UploadPath);
                    if (re)
                        Log4netHelper.WriteInfoLog("完成同步第" + i.ToString() + "条购物券 下载图片");
                    else
                        Log4netHelper.WriteInfoLog("失败同步第" + i.ToString() + "条购物券 下载图片");
                    var c = querys.Where(p => p.CouponNo == coupon.BILLNUMBER).FirstOrDefault();
                    if (c == null)
                    {
                        TM_JPOS_CouponUseRule r = new TM_JPOS_CouponUseRule();
                        r.CouponNo = coupon.BILLNUMBER;
                        r.CouponName = couponName;
                        r.CouponRemark = couponRemark;
                        r.CouponValue = couponValue;
                        r.StartDate = sd;
                        r.EndDate = ed;
                        r.LogoPath = UploadPath;
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
                        c.CouponNo = coupon.BILLNUMBER;
                        c.LogoPath = UploadPath;
                        c.CouponName = couponName;
                        c.CouponRemark = couponRemark;
                        c.CouponValue = couponValue;
                        c.StartDate = sd;
                        c.EndDate = ed;
                        db.SaveChanges();
                        c.ModifiedDate = DateTime.Now;
                    }
                    Log4netHelper.WriteInfoLog("完成第" + i.ToString() + "条购物券同步...");
                    i++;
                }
                db.SaveChanges();
            }



        }


        public static void loy()
        {

            using (var db = new CRMEntities())
            {

                Log4netHelper.WriteInfoLog("执行积分开始");
                db.Database.ExecuteSqlCommand("exec sp_Loy_AccountPointUpdate");
                Log4netHelper.WriteInfoLog("执行积分完成");


                Log4netHelper.WriteInfoLog("执行订单开始");
                db.Database.ExecuteSqlCommand("exec sp_ETL_TradeZLExtract");
                Log4netHelper.WriteInfoLog("执行订单完成");
  

                Log4netHelper.WriteInfoLog("执行升降级开始");
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
                Log4netHelper.WriteInfoLog("执行升降级结束");
            }
        }


    }
}
