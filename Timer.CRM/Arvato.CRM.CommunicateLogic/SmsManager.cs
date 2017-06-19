using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Arvato.CRM.CommunicateLogic
{
    public static class SmsManager
    {
        public static void Send()
        {
            int maxSmsQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSmsQueue"].ToString());

            string accountDefault = WebSite.Default.SmsAccount;
            string passwordDefault = WebSite.Default.SmsPassword;
            string signatureDefault = WebSite.Default.SmsSignature;
            List<string> forecloseWords = WebSite.Default.SmsForeCloseWords;
            DateTime foreCloseDate = WebSite.Default.SmsForeCloseDate;

            var rgx = new Regex(ConfigurationManager.AppSettings["SMSRegex"].ToString());

            using (var db = new CRMEntities())
            {
                var needSendSmses = db.TM_Sys_SMSSendingQueue
                    .Where(p => !p.IsSent && p.Channel == "1" && p.PlanSendDate >= DateTime.Now)
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
                        messages.Add(m.Message + signatureDefault);
                        mobiles.Add(m.Mobile);
                    }
                    m.IsSent = true;
                    m.ActSendDate = DateTime.Now;
                }
                if (messages.Any() && mobiles.Any())
                {
                    var res = SMS.SendBatch(accountDefault, passwordDefault, messages.ToArray(), mobiles.ToArray());
                    Log.ServiceLog("Send Result", Util.Serialize(new { Cnt = res }));

                    foreach (var m in needSendSmses)
                    {
                        m.ChannelResult = res.ToString();
                    }
                    flg = res > 0;
                }
                if (flg) db.SaveChanges();
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
                        MemberID = s.SMSObject.MemberID,
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
                            MemberID = s.ParentMemberId,
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

