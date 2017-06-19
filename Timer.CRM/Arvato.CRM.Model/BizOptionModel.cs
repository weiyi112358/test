using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class BizOptionModel
    {
        public int OptionID { get; set; }
        public string OptionType { get; set; }
        public string OptionValue { get; set; }
        public string OptionText { get; set; }
        public int DataGroupID { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }
        public bool Enable { get; set; }
        public string ReferenceOptionType { get; set; }
    }

    public class BrandModel
    {
        public long BaseDataID { get; set; }
        public int DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string BrandName { get; set; }
        public string BrandCode { get; set; }
    }
}
