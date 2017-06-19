using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects.SqlClient;

namespace Arvato.CRM.BizLogic
{
    public static class CardInfoSearch
    {
        public static Result LoadCardInfo(string CardNo, string CardStatus, string StoreCode,string BoxNo,string agent ,string dp)
        {
            //Log4netHelper.WriteInfoLog("LoadCardInfo:" + CardStatus);
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = from a in db.TM_Card_CardNo
                                join b in db.TM_Mem_Card on a.CardNo equals b.CardNo into btmp
                                from bt in btmp.DefaultIfEmpty()
                                select new
                                {
                                    bt.MemberID,
                                    a.CardNo,
                                    a.BoxNo,
                                    a.Status,
                                    a.StoreCode,

                                    StoreName = db.V_M_TM_SYS_BaseData_store.Where(t => t.StoreCode == a.StoreCode).FirstOrDefault().StoreName,
                                    a.IsUsed,
                                    a.IsSalesStatus,
                                    CardType = db.V_M_TM_SYS_BaseData_cardType.FirstOrDefault(t => t.CardTypeCodeBase == a.CardType).CardTypeNameBase,                                           Agent=db.V_M_TM_SYS_BaseData_store.Where(n=>n.StoreCode==a.StoreCode).Select(m=>m.ChannelNameStore).FirstOrDefault()
                                };
                    if (!string.IsNullOrEmpty(CardNo)) query = query.Where(i => i.CardNo.Contains(CardNo));
                    if (!string.IsNullOrEmpty(BoxNo)) query = query.Where(i => i.BoxNo.Contains(BoxNo));
                    if (!string.IsNullOrEmpty(agent))
                    {
                        string temp = db.V_M_TM_SYS_BaseData_store.Where(n => n.ChannelCodeStore == agent).Select(m=>m.StoreCode).FirstOrDefault();
                        query = query.Where(i => i.StoreCode == temp);
                    }
                    if (!string.IsNullOrEmpty(CardStatus))
                    {
                        switch (CardStatus)
                        {
                            case "2":
                                query = query.Where(i => i.Status == "2");
                                break;

                            case "1-1":
                                query = query.Where(i => i.Status == "1" && i.IsSalesStatus == 1 && i.IsUsed == false);
                                break;
                            case "1-0":
                                query = query.Where(i => i.Status == "1" && i.IsSalesStatus == 0 && i.IsUsed == false);
                                break;

                            case "0":
                                query = query.Where(i => i.Status == "0" && i.IsSalesStatus == 0 && i.IsUsed == false);
                                break;

                            case "-1":
                                query = query.Where(i => i.Status == "-1");
                                break;

                            case "-2":
                                query = query.Where(i => i.Status == "-2");
                                break;

                            case "-3":
                                query = query.Where(i => i.Status == "-3");
                                break;

                            default:
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(StoreCode)) query = query.Where(i => StoreCode == i.StoreCode);
                    




                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch
            {
                return new Result(false, "", new List<object> { new List<TM_Card_CardNo>().ToDataTableSourceVsPage(myDp) });
            }
        }
    }
}
