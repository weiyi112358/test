using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public static class MemberStoreCode
    {
        public static Result GetStoreCodeMembers(string dataParameters, int dataGroupId, string memberName, string stroeName, DateTime? startTime, DateTime? endTime)
        {
            throw new NotImplementedException();
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dataParameters);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from bds in db.V_M_TM_SYS_BaseData_store
            //                join sc in db.TR_Mem_StoreCode
            //                    on bds.StoreCode equals sc.StoreCode into store
            //                from storecode in store.DefaultIfEmpty()
            //                join memext in db.TM_Mem_Ext on storecode.MemberID equals memext.MemberID
            //                where storecode.Flag == true && bds.DataGroupID == 3 // dataGroupId
            //                orderby storecode.AddedDate descending
            //                select new
            //                {
            //                    bds.StoreName,
            //                    memext.MemberID,
            //                    memext.Str_Attr_2,//卡号
            //                    memext.Str_Attr_3,//xingming
            //                    memext.Str_Attr_4,//手机号码
            //                    memext.Str_Attr_7,//性别
            //                    memext.Str_Attr_100,//邮箱
            //                    storecode.AddedDate,//授权人
            //                    storecode.AddedUser,//授权时间
            //                    RegisterStoreName = db.V_M_TM_SYS_BaseData_store.FirstOrDefault(t => t.StoreCode == memext.Str_Attr_5) != null
            //                               ? db.V_M_TM_SYS_BaseData_store.FirstOrDefault(t => t.StoreCode == memext.Str_Attr_5).StoreName : ""
            //                };

            //    if (!string.IsNullOrEmpty(memberName))
            //    {
            //        query = query.Where(m => m.Str_Attr_3.Contains(memberName));
            //    }
            //    if (!string.IsNullOrEmpty(stroeName))
            //    {
            //        query = query.Where(m => m.StoreName.Contains(stroeName));
            //    }
            //    if (startTime != null)
            //    {
            //        query = query.Where(m => m.AddedDate >= startTime);
            //    }
            //    if (endTime != null)
            //    {
            //        endTime = endTime.Value.AddDays(1);
            //        query = query.Where(m => m.AddedDate <= endTime);
            //    }
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}

        }

    }
}
