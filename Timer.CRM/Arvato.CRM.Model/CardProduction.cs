using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
   public class CardProductions
    {
        public string Code { get; set; }
        public string BeginCardNo { get; set; }
        public string EndCardNo { get; set; }      
        public string ProductionNum { get; set; }
        public string ArriveTime { get; set; }
        public string Purpose { get; set; }
        public string ArrayBoxNo { get; set; }
    }

   public class AcceptUnit
   {
       public string AcceptingUnit { get; set; }
   }

   public class IsStatus
   {
       public string Status { get; set; }
       public int ReserveNumber { get; set; }     
   }

   public class GetRetrieve
   {
       public string RetrieveId { get; set; }
   }

   public class EmptyBoxNoInfo
   {
       public long BoxNo { get; set; }
   }

   public class PurchasesNewCard
   {
       public string BeginCardNo { get; set; }
       public string EndCardNo { get; set; }
       public string Status { get; set; }
   }
}
