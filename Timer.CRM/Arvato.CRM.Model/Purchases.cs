using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class PurchasesCard
    { 
        public string Agent { get; set; }
        public string MathRule { get; set; }
        public string CardNum { get; set; }
        public string Code { get; set; }
        public string BeginCardNo { get; set; }
        public string EndCardNo { get; set; }
        public string AcceptingUnit { get; set; }      
    }

    public class RetrieveCard
    { 
        public string OddId { get; set; }
        public string RetrieveNum { get; set; }
        public string Agent { get; set; }   
        public string BeginCardNo { get; set; }
        public string EndCardNo { get; set; }
        public string Code { get; set; }
    }

    public class CardNum
    {
        public string BeginCardNo { get; set; }
        public string EndCardNo { get; set; }
    }

    public class CardType
    {
        public string CardTypeCode { get; set; }
    }

    public class AddEmptyBoxNo
    {
        public string CardTypeCode { get; set; }
        public string Purpose { get; set; }
        public string BeginCardBoxNo { get; set; }
        public string CardBoxNum { get; set; }
    }

    
}
