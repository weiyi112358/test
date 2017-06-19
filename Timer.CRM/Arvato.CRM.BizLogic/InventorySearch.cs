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
    public static class InventorySearch
    {
        public static Result LoadBoxInfo(string BoxNo, string storeCode, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {

                using (var db = new CRMEntities())
                {
                    var query = from a in db.Tm_Card_CardBoxNew
                                select new
                                {
                                    a.BoxNo,
                                    a.BeginCardNo,
                                    a.EndCardNo,
                                    a.CardNumIn,
                                    UsedCard = db.TM_Card_CardNo.Where(i => i.BoxNo == a.BoxNo && i.IsUsed == true).Count(),
                                    a.BoxInAddress,
                                    StoreName = db.V_M_TM_SYS_BaseData_store.Where(n => n.StoreCode == a.BoxInAddress).Select(m => m.StoreName).FirstOrDefault()
                                };

                    if (!string.IsNullOrEmpty(BoxNo)) query = query.Where(i => i.BoxNo.Contains(BoxNo));
                    if (string.IsNullOrEmpty(storeCode) == false)
                    {
                        string store = (storeCode.Split(','))[0];
                        query = query.Where(n => n.BoxInAddress == store);
                    }
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
