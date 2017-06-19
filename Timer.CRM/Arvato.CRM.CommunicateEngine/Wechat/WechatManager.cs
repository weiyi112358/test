using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Arvato.CRM.CommunicateEngine
{
    public static   class WechatManager
    {
      
        public static void Send()
        {
            var ScretKey = ConfigurationManager.AppSettings["ScretKey"].ToString();
            var WechatAPI = ConfigurationManager.AppSettings["WechatAPI"].ToString();
            var DefaultWechat = ConfigurationManager.AppSettings["DefaultWechat"].ToString();
            var MaxWechatBatch = Convert.ToInt32(ConfigurationManager.AppSettings["MaxWechatBatch"].ToString());
            var WechatPointTempateID = ConfigurationManager.AppSettings["WechatPointTempateID"].ToString();
          
            WechatService service = new WechatService();
            service.ScretKey = ScretKey;
            service.WechatAPI = WechatAPI;

            //群发
            //WechatSendMass(DefaultWechat, MaxWechatBatch, service);
            //商品到期提醒/会员等级模板消息/会员等级模板消息接口
            WechatSendMessage(DefaultWechat, MaxWechatBatch, service, "http://dwpserver-sit.timierhouse.com/CRMApi/UpdateRemind.ashx", "1");
            WechatSendMessage(DefaultWechat, MaxWechatBatch, service, "http://dwpserver-sit.timierhouse.com/CRMApi/UpdatePoints.ashx", "2");
            WechatSendMessage(DefaultWechat, MaxWechatBatch, service, "http://dwpserver-sit.timierhouse.com/CRMApi/UpdateGrade.ashx", "3");

            //
            //WechatSendPoint(MaxWechatBatch, service, WechatPointTempateID);
        }

        private static void WechatSendMessage(string DefaultWechat, int MaxWechatBatch, WechatService service,string WechatAPI,string type)
        {
            using (var db = new CRMEntities())
            {
                //ContentType 1：商品到期提醒
                var needSendWechat = db.TM_Sys_WechatSendingQueue
                    .Where(p => !p.IsSent && (p.PlanSendDate <= DateTime.Now || p.PlanSendDate == null) && p.ContentType == type)
                    .OrderBy(p => p.AddedDate)
                    .GroupBy(v => new { v.ContentType, v.ContentPara, v.TempletID })
                    .ToList();
                foreach (var x in needSendWechat)
                {
                    var totalOpenIDs = x.Select(v => new { v.MemberOpenID, v }).ToList();
                    int idx = 0;
                    var forSendWechats = totalOpenIDs.Skip(idx).Take(MaxWechatBatch).ToList();
                    while (forSendWechats.Count() > 0)
                    {
                        var toUsers = forSendWechats.Select(v => v.MemberOpenID).Distinct().ToArray();
                        if (toUsers.Count() == 0)
                            break;
                        if (toUsers.Count() == 1)
                            toUsers = new string[] { DefaultWechat, toUsers[0] };
                        ContentsDetail detail = new ContentsDetail
                        {
                            touser = toUsers,
                            mpnews = new MpnewsDetail { media_id = x.Key.ContentPara },
                            msgtype = "mpnews"
                        };

                        //var result = service.SendWechatMass(detail);
                        // string json = "{\"FCardNum\":\"820000000142\",\"LoginName\":\"class584520\",\"FCount\":\"2\",\"signdate\":\"2017-06-14 11:35:04\",\"secret\":\"13825abff21c477034de705f31bc96ee\"}";
                        string json = detail.mpnews.media_id;//detail.mpnews.ToString();
                        HttpContent httpcontent = new StringContent(json);
                        var httpClient = new HttpClient();

                        var result = httpClient.PostAsync(WechatAPI, httpcontent).Result.Content.ReadAsStringAsync().Result;
                        //Console.WriteLine(responseJson.ToString());
                        //Console.ReadKey();
                        foreach (var one in forSendWechats)
                        {
                            one.v.IsSent = true;
                            one.v.ActSendDate = DateTime.Now;
                            one.v.ResultInfo = result.ToString();
                            if (!string.IsNullOrEmpty(one.v.ResultInfo) && one.v.ResultInfo.Length > 100)
                                one.v.ResultInfo = one.v.ResultInfo.Substring(0, 100);
                        }

                        idx += MaxWechatBatch;
                        forSendWechats = totalOpenIDs.Skip(idx).Take(MaxWechatBatch).ToList();
                    }
                }

                db.SaveChanges();

            }

        }
        //群发
        private static void WechatSendMass(string DefaultWechat, int MaxWechatBatch, WechatService service)
        {
            using (var db = new CRMEntities())
            {
                var needSendWechat = db.TM_Sys_WechatSendingQueue
                    .Where(p => !p.IsSent && "1,2".Contains(p.ContentType))//群发和生日关怀，走群发接口
                    .OrderBy(p => p.AddedDate)
                    .GroupBy(v => new { v.ContentType, v.ContentPara, v.TempletID })
                    .ToList(); 

                foreach (var x in needSendWechat)
                {
                    var totalOpenIDs = x.Select(v => new { v.MemberOpenID, v }).ToList();
                    int idx = 0;
                    var forSendWechats = totalOpenIDs.Skip(idx).Take(MaxWechatBatch).ToList();
                    while (forSendWechats.Count() > 0)
                    {
                        var toUsers = forSendWechats.Select(v => v.MemberOpenID).Distinct().ToArray();
                        if (toUsers.Count() == 0)
                            break;
                        if (toUsers.Count() == 1)
                            toUsers = new string[] { DefaultWechat, toUsers[0] };
                        ContentsDetail detail = new ContentsDetail
                        {
                            touser = toUsers,
                            mpnews = new MpnewsDetail { media_id = x.Key.ContentPara },
                            msgtype = "mpnews"
                        };

                        var result =  service.SendWechatMass(detail);

                        foreach (var one in forSendWechats)
                        {
                            one.v.IsSent = true;
                            one.v.ActSendDate = DateTime.Now;
                            //add Send Result  
                            one.v.ResultInfo = result.Result ;
                            if (!string.IsNullOrEmpty(one.v.ResultInfo) && one.v.ResultInfo.Length > 100)
                                one.v.ResultInfo = one.v.ResultInfo.Substring(0, 100);
                        }

                        idx += MaxWechatBatch;
                        forSendWechats = totalOpenIDs.Skip(idx).Take(MaxWechatBatch).ToList();
                    }
                }

                db.SaveChanges();
            }
        }


        //群发
        private static void WechatSendPoint( int MaxWechatBatch,  WechatService service,string templateID )
        {
            using (var db = new CRMEntities())
            {
                var needSendWechat = (from q in db.TM_Sys_WechatSendingQueue
                            join account in db.TM_Mem_Account on q.MemberID equals account.MemberID
                            join ext in db.V_S_TM_Mem_Ext on q.MemberID equals ext.MemberID
                            join loy in db.V_S_TM_Loy_MemExt on q.MemberID equals loy.MemberID
                            join template in db.TM_Act_CommunicationTemplet on q.TempletID equals template.TempletID
                            where q.MemberID != null && !q.IsSent && q.ContentType == "3"
                            orderby q.AddedDate
                            select new { 
                                queueObject = q,
                                OpenID = q.MemberOpenID,
                                Title = template.Name,
                                CardNumber = ext.MemberCardNo,
                                ExpirePoint = loy.ComeMaturityIntergral,
                                ValidPoint = account.Value1,
                                Remark = template.Remark, 
                            }).Take(MaxWechatBatch)
                            .ToList ();
              
                

                foreach (var x in needSendWechat)
                {
                    ContentDetail detail = new ContentDetail
                    {
                        template_id = templateID,
                        data = new DataDetail
                        {
                            first = new KeyWordDeatil { value = x.Title, color = "#173177" },
                            keyword1 = new KeyWordDeatil { value = x.CardNumber, color = "#173177" },
                            keyword2 = new KeyWordDeatil { value =x.ExpirePoint==null ?"0": x.ExpirePoint.ToString (), color = "#173177" },
                            keyword3 = new KeyWordDeatil { value =x.ValidPoint.ToString (), color = "#173177" },
                            remark = new KeyWordDeatil { value = x.Remark, color = "#173177" }
                        },
                        topcolor = "#FFFFFF",
                        touser = x.OpenID,
                        url = "http://www.foo.com"
                    };
                    var result = service.SendWechatPoint(detail);
                    x.queueObject.IsSent = true;
                    x.queueObject.ActSendDate = DateTime.Now;
                    x.queueObject.ResultInfo = result.Result;
                    if (!string.IsNullOrEmpty(x.queueObject.ResultInfo) && x.queueObject.ResultInfo.Length > 100)
                        x.queueObject.ResultInfo = x.queueObject.ResultInfo.Substring(0, 100);

                }

                db.SaveChanges();
            }
        } 

        public static void SendLog()
        {
            int MaxWechatBatch = Convert.ToInt32(ConfigurationManager.AppSettings["MaxWechatBatch"].ToString());
            using (var db = new CRMEntities())
            {
                var sendMails = (from q in db.TM_Sys_WechatSendingQueue 
                                 where q.IsSent == true && (q.IsLogged == null || q.IsLogged == false)
                                 orderby q.AddedDate
                                 select new
                                 {
                                     queueObject = q,
                                     ParentMemberId =  ""
                                 }).Take(MaxWechatBatch).ToList();
                foreach (var s in sendMails)
                {
                    s.queueObject.IsLogged = true;

                    db.TL_Act_Communication.Add(new TL_Act_Communication
                    {
                        MemberID = s.queueObject.MemberID,
                        CommType = "WeChat",
                        ContentDesc = s.queueObject.ContentPara,
                        TempletID = s.queueObject.TempletID,
                        ReferenceActID = s.queueObject.ActInstanceID == null ? "" : s.queueObject.ActInstanceID.ToString(),
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
                            CommType = "WeChat",
                            ContentDesc = s.queueObject.ContentPara,
                            TempletID = s.queueObject.TempletID,
                            ReferenceActID = s.queueObject.ActInstanceID == null ? "" : s.queueObject.ActInstanceID.ToString(),
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
