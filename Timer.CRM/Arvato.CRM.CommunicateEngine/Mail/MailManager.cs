using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arvato.CRM.CommunicateEngine
{

    public static class MailManager
    {
        public static void Send()
        {
            int maxMailQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMailQueue"].ToString());
            var rgx = new Regex(ConfigurationManager.AppSettings["MailRegex"].ToString());
            using (var db = new CRMEntities())
            {

                var needSendMails = db.TM_Sys_EmailSendingQueue
                    .Where(p => !p.IsSent)
                    .OrderBy(p => p.AddedDate)
                    .Take(maxMailQueue)
                    .ToList();
                foreach (var m in needSendMails)
                {
                    try
                    {
                        if (rgx.IsMatch(m.Email))
                        {
                            //m.Message = HttpUtility.HtmlDecode(m.Message);
                            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsExchange"].ToString()))
                            {
                                Mail.SendExchange(m.Email, m.Email, m.Subject, m.Message, true);
                            }
                            else
                            {
                                Mail.Send(m.Email, m.Email, m.Subject, m.Message, true, true);
                            }
                        }
                        m.IsSent = true;
                        db.TL_Act_Communication.Add(new TL_Act_Communication
                        {
                            MemberID = m.MemberID,
                            CommType = "Email",
                            ContentDesc = m.Subject,
                            TempletID = m.TempletID,
                            ReferenceActID = m.ActInstanceID == null ? "" : m.ActInstanceID.ToString(),
                            Direction = true,
                            OccurTime = DateTime.Now,
                            Status = 1,
                            Source = "ActEngine",
                            AddedDate = DateTime.Now,
                            AddedUser = "ActEngine"
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.ServiceLog("SendMail", ex.ToString());
                        throw ex;
                    }
                }
                db.SaveChanges();
            }
        }

        public static void SendLog()
        {
            int maxMailQueue = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMailQueue"].ToString());
            using (var db = new CRMEntities())
            {
                var sendMails = (from q in db.TM_Sys_EmailSendingQueue
                                 join utemp in db.V_S_TM_Mem_Master on q.MemberID equals utemp.MemberID
                                 into ut
                                 from u in ut.DefaultIfEmpty()
                                 where q.IsSent == true && (q.IsLogged == null || q.IsLogged == false)
                                 orderby q.AddedDate
                                 select new
                                 {
                                     EmailObject = q,
                                     ParentMemberId = u.ParentMemberID ?? ""
                                 }).Take(maxMailQueue).ToList();
                foreach (var s in sendMails)
                {
                    s.EmailObject.IsLogged = true;

                    db.TL_Act_Communication.Add(new TL_Act_Communication
                    {
                        MemberID = s.EmailObject.MemberID,
                        CommType = "Email",
                        ContentDesc = s.EmailObject.Subject,
                        TempletID = s.EmailObject.TempletID,
                        ReferenceActID = s.EmailObject.ActInstanceID == null ? "" : s.EmailObject.ActInstanceID.ToString(),
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
                            CommType = "Email",
                            ContentDesc = s.EmailObject.Subject,
                            TempletID = s.EmailObject.TempletID,
                            ReferenceActID = s.EmailObject.ActInstanceID == null ? "" : s.EmailObject.ActInstanceID.ToString(),
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
    }
}
