using Newtonsoft.Json;
using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Arvato.CRM.CommunicateEngine
{
    public static class SmsManager
    {
        private static object locker = new object();

        #region old
        static string TrimStart(this string source, string arg)
        {
            return arg + Regex.Replace(source, "^" + arg, "", RegexOptions.Compiled);
        }

        static object ToObject(this string source)
        {
            return JsonConvert.DeserializeObject(source);
        }
        public static void SendByCheXiang()
        {
            //lock (locker)
            //{
            //    int maxSmsQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSmsBatchQueue"].ToString());
            //    string smsUri = ConfigurationManager.AppSettings["SendSmsUrl"].ToString();
            //    string smsAppId = ConfigurationManager.AppSettings["SmsAppId"].ToString();
            //    using (var crm = new CRMEntities())
            //    {
            //        var flg = false;
            //        var needSendSmses = crm.TM_Sys_SMSSendingQueue
            //            .Where(p => !p.IsSent)
            //            .OrderBy(p => new { p.TempletID, p.AddedDate })
            //            .Take(maxSmsQueue)
            //            .ToList();
            //        var tmpIds = needSendSmses.GroupBy(p => p.TempletID).Select(p => p.Key).ToList();
            //        var needTemplates = crm.TM_Act_CommunicationTemplet
            //            .Where(p => tmpIds.Contains(p.TempletID))
            //            .ToList();
            //        Semaphore sema = new Semaphore(5, 10);
            //        var tskCnt = needSendSmses.Count;
            //        var tsks = new Task[tskCnt];
            //        foreach (var sms in needSendSmses)
            //        {
            //            tsks[--tskCnt] = Task.Run(() =>
            //            {
            //                sema.WaitOne();
            //                try
            //                {
            //                    if (!string.IsNullOrWhiteSpace(sms.Mobile))
            //                    {
            //                        var phone = sms.Mobile.TrimStart("+86");
            //                        var paras = sms.MsgPara.ToObject();
            //                        var temp = needTemplates.FirstOrDefault(p => p.TempletID == sms.TempletID);
            //                        if (temp != null)
            //                        {
            //                            var cxMsg = new CheXiangParam
            //                            {
            //                                AppId = smsAppId,
            //                                SchemaId = temp.SchemaId,
            //                                BusinessType = temp.BusinessType,
            //                                DestAddrs = new List<string> { phone },
            //                                DestPhones = new List<string> { phone },
            //                                Params = paras,
            //                            };
            //                            var res = CheXiangSMS.SendBatch(smsUri, cxMsg);
            //                            Log.ServiceLog("Send Result", Util.Serialize(new { Cnt = res }));
            //                            sms.ChannelResult = res.Code.ToString();
            //                            sms.ActSendDate = DateTime.Now;
            //                            sms.IsSent = true;
            //                            flg = true;
            //                        }
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    Log.ServiceLog("Exception", ex.ToString());
            //                }
            //                sema.Release();
            //            });
            //        }
            //        Task.WaitAll(tsks);
            //        if (flg) crm.SaveChanges();
            //    }
            //}
        }
        #endregion
        /// <summary>
        /// 批量发送短信，每次发送100笔短信
        /// </summary>
        public static void Send()
        {
            lock (locker)
            {
                string smsUri = ConfigurationManager.AppSettings["SendSmsUrl"].ToString();
                string spid = ConfigurationManager.AppSettings["SMSSPID"].ToString();
                string sppwd = ConfigurationManager.AppSettings["SMSSPPassword"].ToString();
                var rgx = new Regex(ConfigurationManager.AppSettings["SMSRegex"].ToString());
                var maxTryTimes = int.Parse(ConfigurationManager.AppSettings["MaxTryTimes"]);
                int maxSmsQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSmsBatchQueue"].ToString());
                //QiXingSMS QXSMSService = new QiXingSMS(spid, sppwd, smsUri);
                TimerSMSWebService.wmgwSoapClient QXSMSService = new TimerSMSWebService.wmgwSoapClient();
                #region 批量发送
                //将已经到了预计发送时间的短信发送完成
                int i = 0;
                do
                {
                    i++;
                    using (var db = new CRMEntities())
                    {
                        var needSendSmses = db.TM_Sys_SMSSendingQueue
                            .Where(p => !p.IsSent &&
                                (p.TryTime == null || p.TryTime == 0) &&
                                (p.PlanSendDate <= DateTime.Now || p.PlanSendDate == null))
                            .OrderBy(p => p.AddedDate)
                            .Take(100)
                            .ToList();
                        if (needSendSmses.Count == 0)
                            break;
                        SendSMSBatch(rgx, db, needSendSmses);
                    }

                } while (i * 100 < maxSmsQueue);
                #endregion

                #region 有错误的单独发
                using (var db = new CRMEntities())
                {
                    var needSendSmses = db.TM_Sys_SMSSendingQueue
                        .Where(p => !p.IsSent &&
                            (p.TryTime >= 1 && p.TryTime < maxTryTimes) &&
                            (p.PlanSendDate <= DateTime.Now || p.PlanSendDate == null))
                        .OrderBy(p => p.AddedDate)
                        .ToList();
                    if (needSendSmses.Count == 0)
                        return;
                    SendSMSBatch(rgx,  db, needSendSmses);
                    db.SaveChanges();
                }
                #endregion

            }
        }
        /// <summary>
        /// 批量发送短信，每次发送100笔短信
        /// </summary>
        private static void SendSMSBatch(Regex rgx, CRMEntities db, List<TM_Sys_SMSSendingQueue> needSendSmses)
        {
            string sms = string.Empty;
            foreach (var m in needSendSmses)
            {
                if (!string.IsNullOrWhiteSpace(m.Message) &&
                    !string.IsNullOrWhiteSpace(m.Mobile) &&
                    rgx.IsMatch(m.Mobile))
                {
                    string mobile = m.Mobile;
                    if (!mobile.StartsWith("86"))
                        mobile = "86" + mobile;
                    var str = Encoding.GetEncoding("GBK").GetString(Encoding.Default.GetBytes(m.Message));
                    byte[] bytes = Encoding.Default.GetBytes(str);
                    string strbytes = Convert.ToBase64String(bytes);
                    string msg = string.Format("*|{0}|{1}||||||||||1", m.Mobile, strbytes);
                    sms += msg + ",";
                    m.IsSent = true;
                    m.ActSendDate = DateTime.Now;
                }
                else
                {
                    m.ChannelResult = "Fail";
                    m.TryTime = 99;
                }
            }
            if (sms.Any())
            {
                TimerSMSWebService.wmgwSoapClient ws = new TimerSMSWebService.wmgwSoapClient();
                //var result = QXSMSService.SendSMSBatchX(sms.Substring(0, sms.Length - 1));
                string spid = ConfigurationManager.AppSettings["SMSSPID"].ToString();
                string sppwd = ConfigurationManager.AppSettings["SMSSPPassword"].ToString();
                var result = ws.MongateCsSpMultixMtSend(spid, sppwd, sms.Substring(0, sms.Length - 1));
                Log4netHelper.WriteInfoLog(result.ToString());
                if (result == null || result.Length <= 9)
                {
                    needSendSmses.ForEach(v =>
                    {
                        v.TryTime += 1;
                        v.IsSent = false;
                        v.ActSendDate = null;
                        v.ResultInfo = (result != null && result.Length > 200) ? result.Substring(0, 200) : result;
                    });
                }
                db.SaveChanges();
            }
        }

        public static void SendBatch()
        {
            int maxSmsQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSmsBatchQueue"].ToString());

            string accountBatch = WebSite.Default.SmsAccountBatch;
            string passwordBatch = WebSite.Default.SmsPasswordBatch;
            string signatureBatch = WebSite.Default.SmsSignatureBatch;
            List<string> forecloseWords = WebSite.Default.SmsForeCloseWords;
            DateTime foreCloseDate = WebSite.Default.SmsForeCloseDate;

            var rgx = new Regex(ConfigurationManager.AppSettings["SMSRegex"].ToString());

            using (var db = new CRMEntities())
            {
                var needSendSmses = db.TM_Sys_SMSSendingQueue
                    .Where(p => !p.IsSent && p.Channel == "2" && p.PlanSendDate >= DateTime.Now)
                    .OrderBy(p => p.AddedDate)
                    .Take(maxSmsQueue)
                    .ToList();
                var messages = new List<string>(maxSmsQueue);
                var mobiles = new List<string>(maxSmsQueue);
                var flg = false;
                foreach (var m in needSendSmses)
                {
                    if (foreCloseDate <= DateTime.Now && forecloseWords.Any(p => m.Message.Contains(p)))
                    {
                        m.Channel = "-1";
                        flg = true;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(m.Message) &&
                        !string.IsNullOrWhiteSpace(m.Mobile) &&
                        rgx.IsMatch(m.Mobile))
                    {
                        messages.Add(m.Message + signatureBatch);
                        mobiles.Add(m.Mobile);
                    }
                    m.IsSent = true;
                    m.ActSendDate = DateTime.Now;
                }
                if (messages.Any() && mobiles.Any())
                {
                    var res = SMS.SendBatch(accountBatch, passwordBatch, messages.ToArray(), mobiles.ToArray());
                    Log.ServiceLog("Send Batch Result", Util.Serialize(new { Cnt = res }));

                    foreach (var m in needSendSmses)
                    {
                        m.ChannelResult = res.ToString();
                    }
                    flg = res > 0;
                }
                if (flg) db.SaveChanges();
            }
        }



        public static void Receive()
        {
            string accountDefault = System.Web.Mvc.WebSite.Default.SmsAccount;
            string passwordDefault = System.Web.Mvc.WebSite.Default.SmsPassword;

            string accountBatch = System.Web.Mvc.WebSite.Default.SmsAccountBatch;
            string passwordBatch = System.Web.Mvc.WebSite.Default.SmsPasswordBatch;

            var msgs = SMS.Receive(accountDefault, passwordDefault);
            if (msgs != null)
            {
                using (var db = new CRMEntities())
                {
                    foreach (var msg in msgs)
                    {
                        db.TL_Sys_SMSReceivedList.Add(new TL_Sys_SMSReceivedList
                        {
                            Mobile = msg.Mobile,
                            Message = msg.Message,
                            ReceivedTime = msg.Date,
                        });
                    }
                    db.SaveChanges();
                }
            }

            msgs = SMS.Receive(accountBatch, passwordBatch);
            if (msgs != null)
            {
                using (var db = new CRMEntities())
                {
                    foreach (var msg in msgs)
                    {
                        db.TL_Sys_SMSReceivedList.Add(new TL_Sys_SMSReceivedList
                        {
                            Mobile = msg.Mobile,
                            Message = msg.Message,
                            ReceivedTime = msg.Date,
                        });
                    }
                    db.SaveChanges();
                }
            }

        }

        public static void SendLog()
        {
            int maxSmsQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSmsQueue"].ToString());
            using (var db = new CRMEntities())
            {
                var needSendSmses = (from q in db.TM_Sys_SMSSendingQueue
                                     join utemp in db.V_S_TM_Mem_Master on q.MemberID equals utemp.MemberID
                                     into ut
                                     from u in ut.DefaultIfEmpty()
                                     where q.IsSent == true && (q.IsLogged == null || q.IsLogged == false)
                                     orderby q.AddedDate
                                     select new
                                     {
                                         SMSObject = q,
                                         ParentMemberId = u.ParentMemberID ?? ""
                                     }).Take(maxSmsQueue).ToList();
                foreach (var s in needSendSmses)
                {
                    s.SMSObject.IsLogged = true;

                    db.TL_Act_Communication.Add(new TL_Act_Communication
                    {
                        MemberID = s.SMSObject.MemberID ?? "",
                        CommType = "SMS",
                        ContentDesc = s.SMSObject.Message,
                        TempletID = s.SMSObject.TempletID,
                        ReferenceActID = s.SMSObject.ActInstanceID == null ? "" : s.SMSObject.ActInstanceID.ToString(),
                        Direction = true,
                        OccurTime = DateTime.Now,
                        Status = 1,
                        Source = "ActEngine",
                        AddedDate = DateTime.Now,
                        AddedUser = "ActEngine"
                    });
                    if (!string.IsNullOrEmpty(s.ParentMemberId))
                    {
                        db.TL_Act_Communication.Add(new TL_Act_Communication
                        {
                            MemberID = s.ParentMemberId ?? "",
                            CommType = "SMS",
                            ContentDesc = s.SMSObject.Message,
                            TempletID = s.SMSObject.TempletID,
                            ReferenceActID = s.SMSObject.ActInstanceID == null ? "" : s.SMSObject.ActInstanceID.ToString(),
                            Direction = true,
                            OccurTime = DateTime.Now,
                            Status = 1,
                            Source = "ActEngine",
                            AddedDate = DateTime.Now,
                            AddedUser = "ActEngine"
                        });
                    }
                }
                db.SaveChanges();
            }
        }

        public static void ReceiveLog()
        {
            int maxSmsQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSmsQueue"].ToString());
            using (var db = new CRMEntities())
            {
                var xxxx = (from q in db.TL_Sys_SMSReceivedList
                            select q);

                var needSendSmses = (from q in db.TL_Sys_SMSReceivedList
                                     join utemp in db.V_S_TM_Mem_Master on q.MemberID equals utemp.MemberID
                                     into ut
                                     from u in ut.DefaultIfEmpty()
                                     where q.MemberID != null && q.MemberID != "" && (q.IsLogged == null || q.IsLogged == false)
                                     orderby q.ReceivedTime
                                     select new
                                     {
                                         ReceiveObject = q,
                                         ParentMemberId = u.ParentMemberID ?? ""
                                     }).Take(maxSmsQueue).ToList();
                foreach (var s in needSendSmses)
                {
                    s.ReceiveObject.IsLogged = true;

                    db.TL_Act_Communication.Add(new TL_Act_Communication
                    {
                        MemberID = s.ReceiveObject.MemberID,
                        CommType = "SMS",
                        ContentDesc = s.ReceiveObject.Message,
                        ReferenceActID = s.ReceiveObject.ActInstanceID == null ? "" : s.ReceiveObject.ActInstanceID.ToString(),
                        Direction = false,
                        OccurTime = DateTime.Now,
                        Status = 1,
                        Source = "ActEngine",
                        AddedDate = DateTime.Now,
                        AddedUser = "ActEngine"
                    });
                    if (!string.IsNullOrEmpty(s.ParentMemberId))
                    {
                        db.TL_Act_Communication.Add(new TL_Act_Communication
                        {
                            MemberID = s.ParentMemberId,
                            CommType = "SMS",
                            ContentDesc = s.ReceiveObject.Message,
                            ReferenceActID = s.ReceiveObject.ActInstanceID == null ? "" : s.ReceiveObject.ActInstanceID.ToString(),
                            Direction = false,
                            OccurTime = DateTime.Now,
                            Status = 1,
                            Source = "ActEngine",
                            AddedDate = DateTime.Now,
                            AddedUser = "ActEngine"
                        });
                    }
                }
                db.SaveChanges();
            }
        }


    }
}
