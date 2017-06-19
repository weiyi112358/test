using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility.Datatables
{
    /// <summary>
    /// jQuery.DataTablle前台分页数据类型
    /// </summary>
    public class DatatablesSource
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public IList data { set; get; }
    }
}
