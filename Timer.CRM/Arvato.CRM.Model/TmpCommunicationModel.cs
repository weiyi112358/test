using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class TmpCommunicationModel
    {
        public int? TempletID { get; set; }
        public int? DataGroupID { get; set; }
        public string Name { get; set; }
        //public string SchemaId { get; set; }
        public string Type { get; set; }
        public string BusinessType { get; set; }
        public string SubType { get; set; }
        public string Category { get; set; }
        public string Topic { get; set; }
        public string BasicContent { get; set; }
        public Nullable<decimal> InternalSetPrice { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public string Proportion { get; set; }
        public Nullable<decimal> MaxSetPrice { get; set; }
        public string LimitType { get; set; }
        public string LimitTypeKey { get; set; }
        public Nullable<int> ForeignTempletID { get; set; }
        public string ReferenceNo { get; set; }
        public string Remark { get; set; }
        public short Status { get; set; }
        public bool Enable { get; set; }
        public string AddedUser { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string ModifiedUser { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public List<PacMapping> IDOSPackageMapping { get; set; }
    }
    public class PacMapping
    {
       public string IDOSPackageCode{ get; set; }
       public string StoreCode { get; set; }
    }
    public class voucher_consume 
    {
        public DateTime? startdate {get;set;}
        public DateTime? enddate { get; set; }
        public bool ispublic { get; set; }
        public int? offNumber { get; set; }
        public string unit { get; set; }
        public decimal price { get; set; }
        public bool isOthers { get; set; }//是否是异业券
        public int? maxAvaiCount { get; set; }
        public string getChannel { get; set; }
    }
    public class coupon_consume 
    {
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public bool ispublic { get; set; }
        public int? offNumber { get; set; }
        public string unit { get; set; }
        public decimal discount { get; set; }
        public bool isOthers { get; set; }//是否是异业券
        public int? maxAvaiCount { get; set; }
        public string getChannel { get; set; }

    }
    public class deduction_consume
    {
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public bool ispublic{get;set;}
        public int? offNumber {get;set;}
        public string unit { get; set; }
        public decimal reach { get; set; }
        public decimal reduce { get; set; }
        public bool isOthers { get; set; }//是否是异业券
        public int? maxAvaiCount { get; set; }
        public string getChannel { get; set; }
    }
}
