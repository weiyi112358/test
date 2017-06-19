using System.Collections.Generic;
using System.Xml.Serialization;

namespace System.Web.Mvc
{
    /// <summary>
    /// 网站设置
    /// </summary>
    [Serializable]
    public class WebSite
    {
        public static WebSite Default
        {
            get { return WebConfiguration.Default.WebSite; }
        }

        [XmlElement("xuaCompatible")]
        public string XUACompatible { set; get; }

        /// <summary>
        /// 网站标题
        /// </summary>
        [XmlElement("DimensionComputeTime")]
        public string DimensionComputeTime { set; get; }

        /// <summary>
        /// 网站标题
        /// </summary>
        [XmlElement("title")]
        public string Title { set; get; }

        ///<summary>
        /// 网站语言
        ///</summary>
        [XmlElement("language")]
        public string Language { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("robots")]
        public string Robots { set; get; }

        /// <summary>
        /// 网站作者
        /// </summary>
        [XmlElement("author")]
        public string Author { set; get; }

        /// <summary>
        /// 网站版权
        /// </summary>
        [XmlElement("copyright")]
        public string CopyRight { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("mssmartTagsPreventParsing")]
        public string MSSmartTagsPreventParsing { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("imageToolBar")]
        public string ImageToolBar { set; get; }

        /// <summary>
        /// 网站描述
        /// </summary>
        [XmlElement("description")]
        public string Description { set; get; }

        /// <summary>
        /// 网站关键字
        /// </summary>
        [XmlElement("keywords")]
        public string KeyWords { set; get; }

        /// <summary>
        /// 登陆用户Session ID
        /// </summary>
        [XmlElement("auth")]
        public string Auth { set; get; }

        /// <summary>
        /// 用户默认密码
        /// </summary>
        [XmlElement("userDefultPwd")]
        public string UserDefultPwd { set; get; }

        /// <summary>
        /// 品牌标识
        /// </summary>
        [XmlElement("brandId")]
        public string BrandID { set; get; }

        /// <summary>
        /// 邮件服务器
        /// </summary>
        [XmlElement("smtpServer")]
        public string SmtpServer { set; get; }

        /// <summary>
        /// 邮件服务器端口
        /// </summary>
        [XmlElement("smtpPort")]
        public int SmtpPort { set; get; }

        /// <summary>
        /// 发件人
        /// </summary>
        [XmlElement("smtpAccount")]
        public string SmtpAccount { set; get; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        [XmlElement("smtpPassword")]
        public string SmtpPassword { set; get; }

        /// <summary>
        /// 发件人邮箱
        /// </summary>
        [XmlElement("smtpFromAddress")]
        public string SmtpFromAddress { set; get; }

        /// <summary>
        /// 发件人别名（中文名）
        /// </summary>
        [XmlElement("smtpFromName")]
        public string SmtpFromName { set; get; }

        /// <summary>
        /// 邮件编码
        /// </summary>
        [XmlElement("smtpEncoding")]
        public string SmtpEncoding { set; get; }

        /// <summary>
        /// 发送短信用户名
        /// </summary>
        [XmlElement("smsAccount")]
        public string SmsAccount { get; set; }

        /// <summary>
        /// 发送短信密码
        /// </summary>
        [XmlElement("smsPassword")]
        public string SmsPassword { get; set; }

        /// <summary>
        /// 发送短信签名
        /// </summary>
        [XmlElement("smsSignature")]
        public string SmsSignature { set; get; }

        /// <summary>
        /// 批量发送短信用户名
        /// </summary>
        [XmlElement("smsAccountBatch")]
        public string SmsAccountBatch { get; set; }

        /// <summary>
        /// 批量发送短信密码
        /// </summary>
        [XmlElement("smsPasswordBatch")]
        public string SmsPasswordBatch { get; set; }

        /// <summary>
        /// 批量发送短信签名
        /// </summary>
        [XmlElement("smsSingatureBatch")]
        public string SmsSignatureBatch { get; set; }

        /// <summary>
        /// Exchange 域
        /// </summary>
        [XmlElement("exchangeDomain")]
        public string ExchangeDomain { get; set; }

        /// <summary>
        /// Exchange 邮件用户
        /// </summary>
        [XmlElement("exchangeUser")]
        public string ExchangeUser { get; set; }

        /// <summary>
        /// Exchange 邮件密码
        /// </summary>
        [XmlElement("exchangePwd")]
        public string ExchangePwd { get; set; }

        /// <summary>
        /// Exchange 服务器地址
        /// </summary>
        [XmlElement("exchangeUri")]
        public string ExchangeUri { set; get; }

        /// <summary>
        /// 要过滤的关键字列表
        /// </summary>
        [XmlArray("foreClose"), XmlArrayItem("word")]
        public List<string> SmsForeCloseWords { get; set; }

        /// <summary>
        /// 过滤开始时间
        /// </summary>
        [XmlIgnore]
        public DateTime SmsForeCloseDate { set; get; }

        /// <summary>
        /// 过滤开始时间
        /// </summary>
        [XmlElement("foreCloseDate")]
        public string ForeCloseDate
        {
            set { SmsForeCloseDate = DateTime.Parse(value); }
            get { return SmsForeCloseDate.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public WebSite()
        {
            XUACompatible = "IE=edge";
            Title = "瑞臣移动";
            Language = "";
            Robots = "all";
            Author = "";
            CopyRight = "© 2010-2012 arvato system All rights reserved.";
            MSSmartTagsPreventParsing = "yes";
            ImageToolBar = "false";
            Description = "";
            KeyWords = "";
            Auth = "_auth_";
            UserDefultPwd = "123456789";
            BrandID = "YellowHat";

            SmtpServer = "127.0.0.1";
            SmtpPort = 21;
            SmtpAccount = "admin";
            SmtpPassword = "123456";
            SmtpFromAddress = "admin@admin.com";
            SmtpFromName = "测试用户";
            SmtpEncoding = "Utf-8";

            SmsAccount = "sdk_saisc";
            SmsPassword = "saisc@123";
            SmsSignature = "【城建投资】";

            SmsAccountBatch = "sdk_yellowhatyx";
            SmsPasswordBatch = "yellowhat@123";
            SmsSignatureBatch = "【城建投资】";

            ExchangeUser = "yh-crm-post";
            ExchangePwd = "Password01";
            ExchangeDomain = "saisc";
            ExchangeUri = "https://mail.anji.com/ews/exchange.asmx";


            DimensionComputeTime = "23:00:00";
            SmsForeCloseWords = new List<string>();
            SmsForeCloseDate = DateTime.Now;
        }
    }
}
