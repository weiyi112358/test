using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Arvato.CRM.Utility
{
    /// <summary>
    /// 邮件处理类
    /// </summary>
    public static class Mail
    {


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="smtpServer">邮件服务器</param>
        /// <param name="smtpPort">邮件服务器端口</param>
        /// <param name="fromAccount">发件人</param>
        /// <param name="fromAccountAlias">发件人别名（中文名）</param>
        /// <param name="fromPassword">发件人密码</param>
        /// <param name="toAccount">收件人</param>
        /// <param name="toAccountAlias">收件人别名</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="isBodyHtml">是否以超文本发送</param>
        /// <param name="encoding">邮件编码</param>
        /// <param name="isAuthentication">是否需要服务器验证</param>
        /// <param name="files">邮件附件</param>
        public static void Send(
            string smtpServer,
            int smtpPort,
            string fromAccount,
            string fromPassword,
            string fromMail,
            string fromName,
            string toMail,
            string toName,
            string subject,
            string body,
            bool isBodyHtml,
            Encoding encoding,
            bool isAuthentication,
            params string[] files)
        {
            if (string.IsNullOrWhiteSpace(fromName))
            {
                fromName = fromMail;
            }
            if (string.IsNullOrWhiteSpace(toName))
            {
                toName = toMail;
            }
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; //将smtp的出站方式设为 Network
            //smtpClient.EnableSsl = false;//smtp服务器是否启用SSL加密
            //smtpClient.UseDefaultCredentials = false;//是否请求一起发送

            MailAddress fromAddress = new MailAddress(fromMail, fromName);
            MailAddress toAddress = new MailAddress(toMail, toName);
            MailMessage message = new MailMessage(fromAddress, toAddress);

            message.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;

            message.Subject = subject;
            message.Body = body;

            message.Attachments.Clear();
            if (files != null && files.Length != 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    Attachment attach = new Attachment(files[i]);
                    message.Attachments.Add(attach);
                }
            }

            if (isAuthentication == true)
            {
                smtpClient.Credentials = new NetworkCredential(fromAccount, fromPassword);
            }
            smtpClient.Send(message);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAccount">收件人</param>
        /// <param name="toAccountAlias">收件人别名</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="isBodyHtml">是否以超文本发送</param>
        /// <param name="isAuthentication">是否需要服务器验证</param>
        /// <param name="files">邮件附件</param>
        public static void Send(
            string toAccount,
            string toAccountAlias,
            string subject,
            string body,
            bool isBodyHtml,
            bool isAuthentication,
            params string[] files)
        {
            var encoding = Encoding.GetEncoding(System.Web.Mvc.WebSite.Default.SmtpEncoding);

            string smtpServer = System.Web.Mvc.WebSite.Default.SmtpServer;
            int smtpPort = System.Web.Mvc.WebSite.Default.SmtpPort;
            string fromAccount = System.Web.Mvc.WebSite.Default.SmtpAccount;
            string fromPassword = System.Web.Mvc.WebSite.Default.SmtpPassword;
            string fromMail = System.Web.Mvc.WebSite.Default.SmtpFromAddress;
            string fromName = System.Web.Mvc.WebSite.Default.SmtpFromName;

            Send(smtpServer,
                smtpPort,
                fromAccount,
                fromPassword,
                fromMail,
                fromName,
                toAccount,
                toAccountAlias,
                subject,
                body,
                isBodyHtml,
                encoding,
                isAuthentication,
                files);
        }

        public static void SendExchange(
            string toMail,
            string toName,
            string subject,
            string body,
            bool isBodyHtml)
        {
            var encoding = Encoding.GetEncoding(System.Web.Mvc.WebSite.Default.SmtpEncoding);

            string smtpServer = System.Web.Mvc.WebSite.Default.SmtpServer;
            int smtpPort = System.Web.Mvc.WebSite.Default.SmtpPort;

            string fromAccount = System.Web.Mvc.WebSite.Default.ExchangeUser;
            string fromPassword = System.Web.Mvc.WebSite.Default.ExchangePwd;
            string domain = System.Web.Mvc.WebSite.Default.ExchangeDomain;
            string ewsUrl = System.Web.Mvc.WebSite.Default.ExchangeUri;

            string fromMail = System.Web.Mvc.WebSite.Default.SmtpFromAddress;
            string fromName = System.Web.Mvc.WebSite.Default.SmtpFromName;

            if (string.IsNullOrWhiteSpace(fromName))
            {
                fromName = fromMail;
            }
            if (string.IsNullOrWhiteSpace(toName))
            {
                toName = toMail;
            }

            MailAddress fromAddress = new MailAddress(fromMail, fromName);
            MailAddress toAddress = new MailAddress(toMail, toName);
            MailMessage message = new MailMessage(fromAddress, toAddress);

            message.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;



            message.Subject = subject;
            message.Body = body;

            EWS ews = new EWS(fromAccount, fromPassword, domain, ewsUrl);
            ews.SendMail(message);
        }

        public static void SendExchange(MailMessage message)
        {
            string fromAccount = System.Web.Mvc.WebSite.Default.ExchangeUser;
            string fromPassword = System.Web.Mvc.WebSite.Default.ExchangePwd;
            string domain = System.Web.Mvc.WebSite.Default.ExchangeDomain;
            string ewsUrl = System.Web.Mvc.WebSite.Default.ExchangeUri;
            EWS ews = new EWS(fromAccount, fromPassword, domain, ewsUrl);
            ews.SendMail(message);
        }

        public static void SendExchange(
            string toMail,
            string toName,
            string subject,
            string body,
            bool isBodyHtml,
            List<Attachment> attachments,
            string fromAccount,
            string fromPassword,
            string domain,
            string ewsUrl,
            string fromMail,
            string fromName,
            string smtpEncoding)
        {
            //var encoding = Encoding.GetEncoding(System.Web.Mvc.WebSite.Default.SmtpEncoding);
            var encoding = Encoding.GetEncoding(smtpEncoding);

            //string fromAccount = System.Web.Mvc.WebSite.Default.ExchangeUser;
            //string fromPassword = System.Web.Mvc.WebSite.Default.ExchangePwd;
            //string domain = System.Web.Mvc.WebSite.Default.ExchangeDomain;
            //string ewsUrl = System.Web.Mvc.WebSite.Default.ExchangeUri;

            //string fromMail = System.Web.Mvc.WebSite.Default.SmtpFromAddress;
            //string fromName = System.Web.Mvc.WebSite.Default.SmtpFromName;

            if (string.IsNullOrWhiteSpace(fromName))
            {
                fromName = fromMail;
            }
            if (string.IsNullOrWhiteSpace(toName))
            {
                toName = toMail;
            }

            MailAddress fromAddress = new MailAddress(fromMail, fromName);
            MailAddress toAddress = new MailAddress(toMail, toName);
            MailMessage message = new MailMessage(fromAddress, toAddress);

            foreach (var at in attachments)
            {
                message.Attachments.Add(at);
            }

            message.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;



            message.Subject = subject;
            message.Body = body;

            EWS ews = new EWS(fromAccount, fromPassword, domain, ewsUrl);
            ews.SendMail(message);
        }

        public static void SendExchange(
            string toMail,
            string toName,
            string subject,
            string body,
            bool isBodyHtml,
            Dictionary<string, byte[]> attachments,
            string fromAccount,
            string fromPassword,
            string domain,
            string ewsUrl,
            string fromMail,
            string fromName,
            string smtpEncoding)
        {
            //var encoding = Encoding.GetEncoding(System.Web.Mvc.WebSite.Default.SmtpEncoding);
            var encoding = Encoding.GetEncoding(smtpEncoding);

            //string fromAccount = System.Web.Mvc.WebSite.Default.ExchangeUser;
            //string fromPassword = System.Web.Mvc.WebSite.Default.ExchangePwd;
            //string domain = System.Web.Mvc.WebSite.Default.ExchangeDomain;
            //string ewsUrl = System.Web.Mvc.WebSite.Default.ExchangeUri;

            //string fromMail = System.Web.Mvc.WebSite.Default.SmtpFromAddress;
            //string fromName = System.Web.Mvc.WebSite.Default.SmtpFromName;

            if (string.IsNullOrWhiteSpace(fromName))
            {
                fromName = fromMail;
            }
            if (string.IsNullOrWhiteSpace(toName))
            {
                toName = toMail;
            }

            MailAddress fromAddress = new MailAddress(fromMail, fromName);
            MailAddress toAddress = new MailAddress(toMail, toName);
            MailMessage message = new MailMessage(fromAddress, toAddress);

            message.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;



            message.Subject = subject;
            message.Body = body;

            EWS ews = new EWS(fromAccount, fromPassword, domain, ewsUrl);
            ews.SendMail(message, attachments);
        }
    }
}
