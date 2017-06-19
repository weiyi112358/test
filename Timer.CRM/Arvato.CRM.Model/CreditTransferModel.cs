using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class CreditTransferDetail
    {
        public decimal TransferValue { get; set; }
        public List<string> AccountDetailIds { get; set; }
    }
}
