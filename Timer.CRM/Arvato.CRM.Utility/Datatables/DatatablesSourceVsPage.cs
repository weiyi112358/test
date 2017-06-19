using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility.Datatables
{
    /// <summary>
    /// jQuery.DataTablle后台分页返回数据类型
    /// </summary>
    public class DatatablesSourceVsPage
    {
        /// <summary>
        /// 从第N条开始显示
        /// </summary>
        public int iDisplayStart { set; get; }

        /// <summary>
        /// 显示M条
        /// </summary>
        public int iDisplayLength { set; get; }

        /// <summary>
        /// 数据总行数
        /// </summary>
        public int iTotalRecords { set; get; }

        /// <summary>
        /// 数据总行数
        /// </summary>
        public int iTotalDisplayRecords { get { return iTotalRecords; } }

        /// <summary>
        /// 数据源
        /// </summary>
        public IList aaData { set; get; }

        public IList header { set; get; }
    }
}
