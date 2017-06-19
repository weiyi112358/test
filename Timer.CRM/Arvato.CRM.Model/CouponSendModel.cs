using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class CouponListModel
    {
        public string CouponListID { get; set; }
        public Nullable<long> OddNumber { get; set; }
        public Nullable<int> Statu { get; set; }
        public Nullable<int> CouponId { get; set; }
        public Nullable<decimal> CouponValue { get; set; }
        public string CouponInfo { get; set; }
        public Nullable<System.DateTime> BeginDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string CouponDesc { get; set; }
        public Nullable<int> SendCount { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string LimitValue { get; set; }
        public string codeStr { get; set; }
        public Nullable<int> SendedCount { get; set; }
    }
    public class StoreAndSendCountModel {
        public string StoreCode { get; set; }
        public int SendCount { get; set; }
    }
}
