//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Arvato.CRM.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class TD_SYS_VehicleLevel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CarLineID { get; set; }
        public Nullable<int> DataGroupID { get; set; }
        public int Sort { get; set; }
    }
}
