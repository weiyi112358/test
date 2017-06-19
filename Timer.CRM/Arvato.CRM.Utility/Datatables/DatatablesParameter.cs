using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility.Datatables
{
    public class DatatablesParameter
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
        /// 前台DataTable显示列
        /// </summary>
        public List<string> sColumns { set; get; }

        /// <summary>
        /// 排序列
        /// </summary>
        public List<string> SortCols { set; get; }

        /// <summary>
        /// 排序列排列顺序
        /// </summary>
        public List<string> SortDirs { set; get; }

        /// <summary>
        /// 排序列数
        /// </summary>
        public int iSortingCols { set; get; }
    }
}
