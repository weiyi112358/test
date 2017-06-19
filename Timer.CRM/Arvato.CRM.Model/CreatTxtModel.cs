using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class CreatTxtModel
    {
        public Nullable<DateTime> time { get; set; }

        public retCodeModel retCode { get; set; }

        public object command { get; set; }

        public IdRetCodeModel ldRetCode { get; set; }

        public List<VouchersModel> vouchers { get; set; }
    }
    public class retCodeModel
    {
        public Nullable<int> code { get; set; }

        public string message { get; set; }
    }
    public class IdRetCodeModel
    {
        public string code { get; set; }

        public string message { get; set; }
    }
    public class VouchersModel
    {
        public string voucherNo { get; set; }

        public string voucherName { get; set; }

        public string purchaseRemark { get; set; }

        public string parValue { get; set; }

        public Nullable<DateTime> beginDate { get; set; }

        public Nullable<DateTime> endDate { get; set; }

        public string imageName { get; set; }

    }
}
