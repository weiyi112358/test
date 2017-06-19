using Microsoft.Exchange.WebServices.Data;
//using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Arvato.CRM.Utility
{
    public sealed class EWS
    {
        static EWS()
        {
            ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;
        }

        private static bool CheckValidationResult(object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        private ExchangeService _esb;

        public EWS(string username, string password, string domain, string ewsUrl)
        {
            _esb = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
            _esb.Credentials = new NetworkCredential(username, password, domain);
            _esb.Url = new Uri(ewsUrl);
            _esb.Timeout = 3600 * 1000;
        }

        public bool SendMail(MailMessage mail)
        {
            EmailMessage msg = new EmailMessage(_esb);
            msg.Subject = mail.Subject;
            msg.Body = new MessageBody();
            msg.Body.Text = mail.Body;
            msg.Body.BodyType = mail.IsBodyHtml ? BodyType.HTML : BodyType.Text;
            msg.From = new EmailAddress(mail.From.DisplayName, mail.From.Address);
            foreach (var adr in mail.To)
            {
                msg.ToRecipients.Add(new EmailAddress(adr.DisplayName, adr.Address));
            }
            foreach (var atr in mail.Attachments)
            {
                var b = new byte[(int)atr.ContentStream.Length];
                atr.ContentStream.Read(b, 0, b.Length);
                msg.Attachments.AddFileAttachment(atr.Name, b);
            }
            msg.Send();
            return true;
        }

        public bool SendMail(MailMessage mail, Dictionary<string, byte[]> attachments)
        {
            EmailMessage msg = new EmailMessage(_esb);
            msg.Subject = mail.Subject;
            msg.Body = new MessageBody();
            msg.Body.Text = mail.Body;
            msg.Body.BodyType = mail.IsBodyHtml ? BodyType.HTML : BodyType.Text;
            msg.From = new EmailAddress(mail.From.DisplayName, mail.From.Address);
            foreach (var adr in mail.To)
            {
                msg.ToRecipients.Add(new EmailAddress(adr.DisplayName, adr.Address));
            }

            foreach (var atr in attachments)
            {
                msg.Attachments.AddFileAttachment(atr.Key, atr.Value);
            }
            msg.Send();
            return true;
        }
    }
}