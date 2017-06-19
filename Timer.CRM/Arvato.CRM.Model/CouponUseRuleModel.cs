using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class CouponUseRuleModel
    {
        public Nullable<int> ID { get; set; }

        public string CouponName { get; set; }
        
        public Nullable<decimal> CouponValue { get; set; }

        public Nullable<int> CouponSort { get; set; }

        public string LogoPath { get; set; }

        public string CouponRemark { get; set; }

        public string LimitRemark { get; set; }

        public string LimitValue{ get; set; }

        public Nullable<bool> IsMember { get; set; }

        public Nullable<DateTime> StartDate { get; set; }

        public Nullable<DateTime> EndDate { get; set; }

        public string ApproveStatus { get; set; }

        public string ExecuteStatus { get; set; }
    }

    public class CouponSendRuleModel
    {
        public Nullable<int> ID { get; set; }

        public Nullable<int> CouponNo { get; set; }

        public string CouponName { get; set; }

        public Nullable<decimal> CouponValue { get; set; }
     
        public string CouponRemark { get; set; }

        public string LimitValue { get; set; }
   
        public Nullable<bool> IsMember { get; set; }

        public Nullable<DateTime> StartDate { get; set; }

        public Nullable<DateTime> EndDate { get; set; }

        public string ApproveStatus { get; set; }

        public string ExecuteStatus { get; set; }
    }



}
