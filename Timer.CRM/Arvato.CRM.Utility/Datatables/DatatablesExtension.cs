using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Dynamic;

namespace Arvato.CRM.Utility.Datatables
{
    /// <summary>
    /// jQuery.DataTables生成数据扩展方法
    /// </summary>
    public static class DatatablesExtension
    {
        /// <summary>
        /// 生成未分页的jQuery.DataTables数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>返回{data:[...]}对象</returns>
        public static DatatablesSource ToDataTableSource<T>(this IQueryable<T> source)
        {
            return new DatatablesSource { data = source.ToList() };
        }

        /// <summary>
        /// 生成未分页的jQuery.DataTables数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>返回{data:[...]}对象</returns>
        public static DatatablesSource ToDataTableSource<T>(this IList<T> source)
        {
            return source.AsQueryable().ToDataTableSource();
        }

        /// <summary>
        /// 生成分页的jQuery.DataTables数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>返回{iDisplayStart:1,iDisplayLength:2,iTotalRecords:3,aaData:[]}对象</returns>
        public static DatatablesSourceVsPage ToDataTableSourceVsPage<T>(this IQueryable<T> source, DatatablesParameter param)
        {
            var t = typeof(T);
            if (param == null) throw new Exception("DataTableParameter对象不能为空");
            //foreach (var col in param.sColumns)
            //{
            //    if (!string.IsNullOrEmpty(col))
            //        if (t.GetProperty(col) == null)
            //            throw new KeyNotFoundException(string.Format("数据源中缺少列“{0}”", col));
            //}

            var res = new DatatablesSourceVsPage();
            res.iDisplayStart = param.iDisplayStart;
            res.iDisplayLength = param.iDisplayLength;
            res.iTotalRecords = source.Count();

            var odrs = new List<string>();
            if (param.iSortingCols > 0)
            {
                for (var i = 0; i < param.iSortingCols; i++)
                {
                    odrs.Add(param.SortCols[i] + " " + param.SortDirs[i]);
                }
            }
            else
            {
                odrs.Add(param.sColumns.Where(p => !string.IsNullOrEmpty(p)).FirstOrDefault());
            }

            res.aaData = source.OrderBy(string.Join(",", odrs)).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            return res;
        }

        /// <summary>
        /// 生成分页的jQuery.DataTables数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>返回{iDisplayStart:1,iDisplayLength:2,iTotalRecords:3,aaData:[]}对象</returns>
        public static DatatablesSourceVsPage ToDataTableSourceVsPage<T>(this IList<T> source, DatatablesParameter param)
        {
            return source.AsQueryable().ToDataTableSourceVsPage(param);
        }

        public static DatatablesParameter CreateDataTableParameter(this HttpRequestBase request)
        {
            var param = new DatatablesParameter
            {
                iDisplayStart = int.Parse(request["iDisplayStart"]),
                iDisplayLength = int.Parse(request["iDisplayLength"]),
                iSortingCols = int.Parse(request["iSortingCols"] ?? "0"),
                sColumns = request["sColumns"].Split(',').ToList()
            };

            param.SortCols = new List<string>();
            param.SortDirs = new List<string>();
            for (var i = 0; i < param.iSortingCols; i++)
            {
                var sortCol = request["iSortCol_" + i];
                var sortDir = request["sSortDir_" + i];
                sortCol = request["mDataProp_" + sortCol];
                param.SortCols.Add(sortCol);
                param.SortDirs.Add(sortDir);
            }

            return param;
        }

        public static string AssembleData(string data)
        {
             return "{\"aaData\":" + data +",\"iDisplayStart\":1,\"iDisplayLength\":1,\"iTotalRecords\":1,\"iTotalDisplayRecords\":1}";
        }
    }
}
