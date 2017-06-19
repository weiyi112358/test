using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Arvato.CRM.Utility
{
    public class SMS
    {
        //private static string account = System.Web.Mvc.WebSite.Default.SmsAccount;
        //private static string password = System.Web.Mvc.WebSite.Default.SmsPassword;

        /// <summary>
        /// 批量发送相同的短信
        /// </summary>
        /// <param name="message">短信信息</param>
        /// <param name="mobiles">手机列表</param>
        public static int SendBatch(string account, string password, string message, params string[] mobiles)
        {
            if (mobiles != null && mobiles.Any())
            {
                var distmobile = new StringBuilder();
                foreach (var mobile in mobiles)
                {
                    if (!string.IsNullOrWhiteSpace(mobile)) distmobile.AppendFormat(";{0}", mobile);
                }
                if (distmobile.Length == 0) return -7;//目标号码为空
                var mob = distmobile.ToString(1, distmobile.Length - 1);

                if (string.IsNullOrWhiteSpace(message)) return -6;//短信内容为空

                //using (var client = new BusinessServiceService())
                //{
                //    if (mobiles.Length == 1)
                //    {
                //        return client.sendMessage(account, password, mob, message);
                //    }
                //    return client.sendBatchMessage(account, password, mob, message);
                //}

                throw new NotImplementedException();
            }
            return -5;//其他错误
        }

        /// <summary>
        /// 批量发送相同的短信
        /// </summary>
        /// <param name="messages">信息列表</param>
        /// <param name="mobiles">手机列表</param>
        /// <returns></returns>
        public static int SendBatch(string account, string password, string[] messages, string[] mobiles)
        {
            if (mobiles != null && messages != null && mobiles.Length == messages.Length)
            {
                var distmobile = new StringBuilder();
                int midx = 0, sidx = 0;
                foreach (var mobile in mobiles)
                {
                    if (!string.IsNullOrWhiteSpace(mobile))
                    {
                        midx++;
                        distmobile.AppendFormat("||{0}", mobile);
                    }
                }
                if (distmobile.Length == 0) return -7;//目标号码为空
                var mob = distmobile.ToString(2, distmobile.Length - 2);

                var distmessage = new StringBuilder();
                foreach (var message in messages)
                {
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        sidx++;
                        distmessage.AppendFormat("||{0}", message);
                    }
                }
                if (distmessage.Length == 0) return -6;//短信内容为空
                var msg = distmessage.ToString(2, distmessage.Length - 2);

                if (midx != sidx) return -5;//短信内容和目标号码不匹配
                //SystemLog.Log("SMS_MOB", mob);
                //SystemLog.Log("SMS_MSG", msg);
                //using (var client = new BusinessServiceService())
                //{
                //    if (mobiles.Length == 1)
                //    {
                //        //SystemLog.Log("SMS_MSG", "sendMessage");
                //        return client.sendMessage(account, password, mob, msg);
                //    }
                //    //SystemLog.Log("SMS_MSG", "sendBatchMessage");
                //    return client.sendPersonalMessages(account, password, mob, msg);
                //}
                throw new NotImplementedException();
            }
            return -5;//其他错误

        }

        /// <summary>
        /// 接收上行回复短消息
        /// </summary>
        /// <returns></returns>
        public static List<SmsReciviedMessage> Receive(string account, string password)
        {
            throw new NotImplementedException();
            //using (var client = new BusinessServiceService())
            //{
            //    var msg = client.getReceivedMsg(account, password, 0);
            //    //SystemLog.Log("SMS Receive Log1", msg);
            //    List<SmsReciviedMessage> srmLst = new List<SmsReciviedMessage>();
            //    if (!string.IsNullOrWhiteSpace(msg))
            //    {
            //        var srmstrs = msg.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            //        try
            //        {
            //            foreach (var srmstr in srmstrs)
            //            {
            //                var strs = Regex.Unescape(srmstr).Split(',');
            //                var srm = new SmsReciviedMessage();
            //                srm.Mobile = strs[0];
            //                srm.Message = strs[1];
            //                var format = new DateTimeFormatInfo();
            //                srm.Date = DateTime.ParseExact(strs[2], "yyyyMMddHHmmss", format);

            //                srmLst.Add(srm);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.ServiceLog("SMS Receive Error", ex.Message);
            //            throw ex;
            //        }
            //    }
            //    return srmLst;
            //}
        }
    }

    /// <summary>
    /// 短信回复
    /// </summary>
    public class SmsReciviedMessage
    {
        /// <summary>
        /// 回复手机
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// 回复消息
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime Date { set; get; }
    }
}
