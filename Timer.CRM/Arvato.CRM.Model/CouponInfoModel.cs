using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{

    public class OutputCouponModel
    {
        public string Str_Attr_1 { get; set; }
        public string Str_Attr_2 { get; set; }
        public string Str_Attr_3 { get; set; }
        public string Str_Attr_4 { get; set; }
        public string Str_Attr_5 { get; set; }
        public string Str_Attr_6 { get; set; }
        public string Str_Attr_7 { get; set; }
        public string Date_Attr_1 { get; set; }
        public string Date_Attr_2 { get; set; }
        public DateTime? Date_Attr_3 { get; set; }
        public long Int_Attr_1 { get; set; }
        public decimal? Dec_Attr_1 { get; set; }
    }

    public class CouponModel
    {
        public int TempletID { get; set; }
        public string CouponName { get; set; }
        public string CouponType { get; set; }
        public string BasicContent { get; set; }
        public bool IsPubilc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    public class JsonCouponModel
    {
        public int TempletID { get; set; }
        public decimal? price { get; set; }
        public bool ispublic { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public string unit { get; set; }
        public int? offNumber { get; set; }
        public bool isOthers { get; set; }
        public int? maxAvaiCount { get; set; }
        public string getChannel { get; set; }
    }

    public class JsonIDOSPackageModel
    {
        public string IDOSPackageCode { get; set; }
        public string StoreCode { get; set; }
    }

    public class OutputPackageModel
    {
        public long Str_Attr_1 { get; set; }
        public long Str_Attr_2 { get; set; }
        public string Str_Attr_3 { get; set; }
        public string Str_Attr_4 { get; set; }
        public int Str_Attr_5 { get; set; }
        public int Str_Attr_6 { get; set; }
        public DateTime? Str_Attr_7 { get; set; }
        public string Str_Attr_8 { get; set; }
        public bool Str_Attr_9 { get; set; }
    
    }

    public class PublicCoupon
    {
        public long CouponID { get; set; }
        public string CouponCode { get; set; }
        public string Mobile { get; set; }
        public bool IsUsed { get; set; }
        public string TempletName { get; set; }
    }

}
