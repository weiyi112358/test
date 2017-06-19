using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class CustomizeNewCard
    {      
           public string Agent { get; set; }
           public string MathRule { get; set; }
           public string CardNoArray { get; set; }
           public string CardType { get; set; }
           public string BeginCardNo { get; set; }
           public string DestineNum { get; set; }
           public string StoreCode { get; set; }
           public string CompanyCode { get; set; }
           public string EndCardNo { get; set; }
           public string Remark { get; set; }
           public string BoxPurpose { get; set; }
    }

    public class CheckCustomizeParams
    {     
        public string CustomizeOddId { get; set; }           
    }

    public class CustomizeToExcel
    {
        public long Sequence { get; set; }     
        public string BoxNo { get; set; }
        public string BeginCardNo { get; set; }
        public string EndCardNo { get; set; }
    }

    
    public class CustomizeExportColumnModel
    {
        public long Sequence { get; set; }  
        public string BoxNo { get; set; }
        public string BeginCardNo { get; set; }
        public string EndCardNo { get; set; }
    }

    public class CustomizeListToExcel
    {
        public string OddIdNo { get; set; }
        public string StatusName { get; set; }
        public string Agent { get; set; }
        public string AcceptingUnit { get; set; }
        public string BoxNumIn { get; set; }
        public string CardNumIn { get; set; }
        public string CreateTime { get; set; }
    }

    public class RetrieveListToExcel
    {
        public string RetrieveOddIdNo { get; set; }
        public string CustomizeOddIdNo { get; set; }
        public string AgentName { get; set; }
        public string StatusName { get; set; }
        public string ReserveBoxNumber { get; set; }
        public string CreateTime { get; set; }
        public string ReserveCardNumber { get; set; }        
    }
}
