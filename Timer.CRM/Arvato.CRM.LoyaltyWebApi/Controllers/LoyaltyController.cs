using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Arvato.CRM.Utility;
using System.Threading.Tasks;
using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Trigger;
using System.Text;

namespace Arvato.CRM.LoyaltyWebApi.Controllers
{
    public class LoyaltyController : ApiController
    {
        private static Object loyLock = new Object();
        [HttpPost]
        [ActionName("CommonLoyaltyAPI")]
        public async Task<string> CommonLoyaltyAPI()
        {
            string result = await Request.Content.ReadAsStringAsync();
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return "0";
            }
            Log4netHelper.WriteInfoLog(result);
            lock (loyLock)
            {
                using (CRMEntities db = new CRMEntities())
                {
                    DateTime dt = DateTime.Now;
                    try
                    {
                        var ls = JsonHelper.Deserialize<LoyShare>(result);
                        var el = new ExtraLoyalty
                        {
                            InterfaceName = ls.InterfaceName,
                            db = db,
                            DataGroupID = 1,
                            RuleType = "1"
                        };

                        LoyaltyTrigger tr = new LoyaltyTrigger(el);
                        tr.MemberScript = ls.memberScript;
                        tr.Ext.SearchTradeDetailSQL = ls.searchTradeDetailSQL;
                        tr.Ext.SearchTradeSQL = ls.searchTradeSQL;
                        tr.StartTime = dt;
                        tr.Start();
                    }
                    catch (Exception ex)
                    {
                        Log4netHelper.WriteErrorLog(ex.ToString());
                        throw;
                    }
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("Insert into [TM_Sys_SMSSendingQueue]");
                    sql.AppendLine("(");
                    sql.AppendLine("[Mobile],[Message],[MsgPara],[MemberID],[IsSent],[AddedDate],[AddedUser],[IsLogged],[TryTime]");
                    sql.AppendLine(")");
                    sql.AppendLine("select top 1 t2.Mobile,'尊敬的会员,您的会员卡'+t3.CardNo+'发生金币变动：'+cast(cast(sum(ChangeValue) as decimal(18,2))as nvarchar(20))+'当前账户总金币为：'+cast(t4.Value1 as nvarchar(20)),'',t1.MemberID");
                    sql.AppendLine(",0,getdate(),1000,0,0");
                    sql.AppendLine("From tl_mem_accountchange t1");
                    sql.AppendLine("inner join V_U_TM_Mem_Info t2 on t1.memberid = t2.memberid");
                    sql.AppendLine("inner join (select * from tm_mem_card where cardtype=1 and active=1 and lock=0) t3 on t1.memberid = t3.memberid");
                    sql.AppendLine("inner join tm_mem_account t4 on t1.memberid = t4.memberid");
                    sql.AppendLine("where t1.addeddate ='" + dt + "'");
                    sql.AppendLine("group by t1.memberid ,t2.Mobile,t3.CardNo,t4.Value1");
                    Log4netHelper.WriteInfoLog(sql.ToString());
                    db.Database.ExecuteSqlCommand(sql.ToString());



                }
                return "0";
            }
        }
    }
}

