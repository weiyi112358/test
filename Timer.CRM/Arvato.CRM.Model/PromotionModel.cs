using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class PromotionModel
    {
        public Int64 BaseDataID { get; set; }
        public string PromotionID { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string PromotionBillType { get; set; }
        public string PromotionType { get; set; }
        public string PromotionRemark { get; set; }
        public string PromotionIsEnd { get; set; }
        public int DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public DateTime? StartDatePromotion { get; set; }
        public DateTime? EndDatePromotion { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Name { get; set; }
        public object Key { get; set; }
    }
}
