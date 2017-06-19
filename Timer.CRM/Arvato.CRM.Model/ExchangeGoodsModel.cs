using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class ExchangeGooodsRuleListModel
    {
       
        public string ExchangeID { get; set; }
        public string Code { get; set; }
        public Nullable<int> Status { get; set; }
        
        public string ExchangeType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Remark { get; set; }

        public string[] CardTypeLimit { get; set; }

        public string[] LevelLimit { get; set; }
       


    }

    public class RuleDetailModel
    {
        public string ExchangeId { get; set; }
        public string InputCode { get; set; }
        public string GoodsID { get; set; }
        public string GoodsCode { get; set; }
        public Nullable<int> MaxCounts { get; set; }
        public Nullable<decimal> DiscountValue { get; set; }


        public Nullable<int> MinCounts { get; set; }



    }
}
