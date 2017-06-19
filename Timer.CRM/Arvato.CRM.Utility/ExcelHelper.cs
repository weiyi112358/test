using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Web;
using System.Data;

namespace Arvato.CRM.Utility
{
    public class ExcelColumnFormat<T>
    {
        /// <summary>
        /// 导出列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 数据格式
        /// </summary>
        public ExcelDataFromat Format { get; set; }

        public Func<T, object> FuncGetValue { get; set; }
        /// <summary>
        /// 合并的单元格
        /// </summary>
        public ExcelDataRegion Region { get; set; }
    }

    public class ExcelDataFromat
    {
        public ExcelDataFromat(string SystemFromat, string CustomFromat)
        {
            if (SystemFromat == null && CustomFromat == null)
            {
                throw new ArgumentNullException("必须输入一个格式");
            }
            else
            {
                this.SystemFromat = SystemFromat;
                this.CustomFromat = CustomFromat;
            }
        }
        /// <summary>
        /// 内嵌格式
        /// </summary>
        public string SystemFromat { get; set; }
        /// <summary>
        /// 自定义样式
        /// </summary>
        public string CustomFromat { get; set; }
    }

    public class ExcelDataRegion
    {
        public ExcelDataRegion(int FirstRow, int LastRow, int FirstColumn, int LastColumn)
        {
            if (FirstRow == LastRow && FirstColumn == LastColumn)
            {
                throw new ArgumentNullException("合并单元格格式错误");
            }
            else
            {
                this.FirstRow = FirstRow;
                this.LastRow = LastRow;
                this.FirstColumn = FirstColumn;
                this.LastColumn = LastColumn;
            }
        }
        /// <summary>
        /// 区域中第一个单元格的行号
        /// </summary>
        public int FirstRow { get; set; }
        /// <summary>
        /// 区域中第一个单元格的列号
        /// </summary>
        public int FirstColumn { get; set; }
        /// <summary>
        /// 区域中最后一个单元格的行号
        /// </summary>
        public int LastRow { get; set; }
        /// <summary>
        /// 区域中最后一个单元格的列号
        /// </summary>
        public int LastColumn { get; set; }
    }

    class InternalColumnFormat<T>
    {
        public InternalColumnFormat(ExcelColumnFormat<T> ColumnFormat, HSSFWorkbook WorkBook)
        {
            ColumnName = ColumnFormat.ColumnName;
            Format = ColumnFormat.Format;
            FuncGetValue = ColumnFormat.FuncGetValue;
            Region = ColumnFormat.Region;
            this.WorkBook = WorkBook;

            #region  标题行样式设置

            var style = (HSSFCellStyle)WorkBook.CreateCellStyle();
            var font = (HSSFFont)WorkBook.CreateFont();
            style.SetFont(font);
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            font.FontHeightInPoints = 10;
            font.FontName = "Arial";
            font.Boldweight = (short)FontBoldWeight.BOLD;
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_25_PERCENT.index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            style.BorderBottom = BorderStyle.THIN;

            HeaderCellStyle = style;

            #endregion

            #region  数据行样式设置

            style = (HSSFCellStyle)WorkBook.CreateCellStyle();
            font = (HSSFFont)WorkBook.CreateFont();
            style.SetFont(font);
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            font.FontHeightInPoints = 9;
            font.FontName = "Arial";
            font.Boldweight = (short)FontBoldWeight.NORMAL;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            style.BorderBottom = BorderStyle.THIN;

            BodyCellStyle = style;

            #endregion
        }
        public Func<T, object> FuncGetValue { get; set; }
        public string ColumnName { get; set; }
        public ExcelDataFromat Format { get; set; }
        public HSSFWorkbook WorkBook { get; set; }
        internal HSSFCellStyle HeaderCellStyle { get; set; }
        internal HSSFCellStyle BodyCellStyle { get; set; }
        internal bool IsInitDataFormat { get; set; }
        public ExcelDataRegion Region { get; set; }
    }

    public class ExcelHelper
    {
        #region private field
        private static ExcelDataFromat StringDefaultFormat = new ExcelDataFromat("", "");
        private static ExcelDataFromat BoolDefaultFormat = new ExcelDataFromat("", "");
        private static ExcelDataFromat DateTimeDefaultFormat = new ExcelDataFromat(null, "yyyy-MM-dd");
        private static ExcelDataFromat DoubleDefaultFormat = new ExcelDataFromat("0.00", "0.00");
        private static ExcelDataFromat IntDefaultFormat = new ExcelDataFromat("", "");
        #endregion

        #region public method

        /// <summary>
        /// 设置默认样式
        /// </summary>
        /// <param name="stringDefaultFormat"></param>
        /// <param name="boolDefaultFormat"></param>
        /// <param name="dateTimeDefaultFormat"></param>
        /// <param name="doubleDefaultFormat"></param>
        public static void SetDefaultFormat(ExcelDataFromat stringDefaultFormat, ExcelDataFromat boolDefaultFormat, ExcelDataFromat dateTimeDefaultFormat, ExcelDataFromat doubleDefaultFormat)
        {
            Action<ExcelDataFromat, ExcelDataFromat> action = (t, s) =>
            {
                if (t != null)
                {
                    s = t;
                }
            };
            action(stringDefaultFormat, StringDefaultFormat);
            action(boolDefaultFormat, BoolDefaultFormat);
            action(dateTimeDefaultFormat, DateTimeDefaultFormat);
            action(doubleDefaultFormat, DoubleDefaultFormat);
        }

        /// <summary>
        /// 将HSSFWorkbook文件写入stream
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <returns></returns>
        public static MemoryStream GetStream(HSSFWorkbook hssfworkbook)
        {
            MemoryStream ms = new MemoryStream();
            hssfworkbook.Write(ms);
            return ms;
        }

        /// <summary>
        /// 将HSSFWorkbookp直接写入Response流
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="Response"></param>
        /// <param name="fileName"></param>
        //public static void WriteDownload(HSSFWorkbook hssfworkbook, HttpResponseBase Response, string fileName)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    hssfworkbook.Write(ms);
        //    byte[] attFile = ms.GetBuffer();
        //    ms.Close();

        //    if (attFile != null)
        //    {
        //        Response.Clear();
        //        Response.Charset = "UTF-8";
        //        Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        //        Response.ContentType = "application/octet-stream";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName + ".xls"));

        //        Response.BinaryWrite(attFile);
        //        Response.Flush();
        //        Response.End();
        //    }
        //}

        /// <summary>
        ///  将IList<T>写入到Excel文件(所有属性)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HSSFWorkbook DataToExcel<T>(IList<T> data) where T : class
        {
            HSSFWorkbook workBook = new HSSFWorkbook();
            var internalColumnFormats = new List<InternalColumnFormat<T>>();
            Type type = typeof(T);
            foreach (PropertyInfo pInfo in type.GetProperties())
            {
                Func<T, object> func = p => { return pInfo.GetValue(p); };
                var columnFormat = new ExcelColumnFormat<T>
                {
                    Format = null,
                    ColumnName = pInfo.Name,
                    FuncGetValue = func
                    //PropertyText = pInfo.Name
                };
                internalColumnFormats.Add(new InternalColumnFormat<T>(columnFormat, workBook));
            }
            return DataToExcel<T>(data, internalColumnFormats);
        }

        /// <summary>
        /// 将IList<T>写入到Excel文件(指定属性)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="columnFormats"></param>
        /// <returns></returns>
        public static HSSFWorkbook DataToExcel<T>(IList<T> data, IList<ExcelColumnFormat<T>> columnFormats, HSSFWorkbook existWorkBook = null, IList<ExcelColumnFormat<T>> headTopColumnFormats = null, IList<ExcelColumnFormat<T>> head2ndRowColumnFormats = null) where T : class
        {
            ExcelColumnFormat<T> s1 = new ExcelColumnFormat<T>();
            Func<T, string> s = p => p.ToString();

            HSSFWorkbook workBook;
            if (existWorkBook == null)
            {
                workBook = new HSSFWorkbook();
            }
            else
            {
                workBook = existWorkBook;
            }
            var internalColumnFormats = new List<InternalColumnFormat<T>>();
            List<InternalColumnFormat<T>> internalTopColumnFormats = null;
            List<InternalColumnFormat<T>> internal2ndRowColumnFormats = null;
            Type type = typeof(T);
            foreach (ExcelColumnFormat<T> c in columnFormats)
            {
                // var pInfo = type.GetProperty(c.ColumnName);
                //type.GetProperty(,
                // if (pInfo != null)
                //  {
                internalColumnFormats.Add(new InternalColumnFormat<T>(c, workBook));
                //  }
                // else
                // {
                //   internalColumnFormats.Add(new InternalColumnFormat<T>(c, workBook, typeof(string)));
                // }
            }
            if (headTopColumnFormats != null)
            {
                internalTopColumnFormats = new List<InternalColumnFormat<T>>();
                foreach (ExcelColumnFormat<T> tc in headTopColumnFormats)
                {
                    internalTopColumnFormats.Add(new InternalColumnFormat<T>(tc, workBook));
                }
            }
            if (head2ndRowColumnFormats != null)
            {
                internal2ndRowColumnFormats = new List<InternalColumnFormat<T>>();
                foreach (ExcelColumnFormat<T> tc in head2ndRowColumnFormats)
                {
                    internal2ndRowColumnFormats.Add(new InternalColumnFormat<T>(tc, workBook));
                }
            }
            return DataToExcel<T>(data, internalColumnFormats, existWorkBook, internalTopColumnFormats, internal2ndRowColumnFormats);
        }

        public static DataTable ExcelToDataTable(Stream fileStream)
        {
            DataTable dt = new DataTable();
            using (fileStream)
            {
                IWorkbook workBook = new HSSFWorkbook(fileStream);

                if (workBook.NumberOfSheets > 0)
                {
                    ISheet sheet = workBook.GetSheetAt(0);

                    if (sheet.PhysicalNumberOfRows > 0)
                    {
                        IRow headerRow = sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;
                        int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                            dt.Columns.Add(column);
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();
                            if (row != null)
                            {
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                    {
                                        dataRow[j] = GetCellValue(row.GetCell(j));
                                    }
                                }
                            }
                            dt.Rows.Add(dataRow);
                        }
                        return dt;
                    }

                }

            }
            return null;
        }

        #endregion

        #region private method

        private static void SetHeaderValue<T>(ICell cell, InternalColumnFormat<T> columnFormat)
        {
            cell.CellStyle = columnFormat.HeaderCellStyle;
            cell.SetCellValue(columnFormat.ColumnName);
        }

        private static void SetBodyCellValue<T>(ICell cell, T row, InternalColumnFormat<T> columnFormat)
        {
            Type propertytype = null;
            object value = null;

            if (columnFormat.FuncGetValue != null)
            {
                var ss = columnFormat.FuncGetValue(row);
                value = columnFormat.FuncGetValue(row);
                if (value != null)
                {
                    propertytype = value.GetType();
                }
            }

            var cellStyle = columnFormat.BodyCellStyle;
            if (value != null)
            {

                if (propertytype.IsValueType)
                {
                    //布尔值
                    if (propertytype.Equals(typeof(bool)))
                    {
                        SetValue(columnFormat, cell, value, typeof(bool));
                    }
                    //时间
                    else if (propertytype.Equals(typeof(DateTime)))
                    {
                        SetValue(columnFormat, cell, value, typeof(DateTime));
                    }
                    //枚举类型 
                    else if (propertytype.IsEnum)
                    {
                        SetValue(columnFormat, cell, value.ToString(), typeof(string));
                    }
                    //其它基元类型(整数，浮点数， decimal)
                    else if (propertytype.IsPrimitive || propertytype.Equals(typeof(decimal)))
                    {
                        if (propertytype.Equals(typeof(Int64))
                            || propertytype.Equals(typeof(int))
                    || propertytype.Equals(typeof(Int16)) ||
                    propertytype.Equals(typeof(uint))
                    || propertytype.Equals(typeof(UInt16))
                    || propertytype.Equals(typeof(UInt64))
                    || propertytype.Equals(typeof(Byte))
                    || propertytype.Equals(typeof(SByte)))
                        {
                            SetValue(columnFormat, cell, Convert.ToDouble(value), typeof(int));
                        }
                        else
                        {
                            SetValue(columnFormat, cell, Convert.ToDouble(value), typeof(double));
                        }
                    }
                    //可空类型
                    else if (propertytype.IsGenericType && propertytype.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        if (value != null)
                        {
                            var tType = propertytype.GetGenericArguments()[0];
                            var z = Nullable.GetUnderlyingType(propertytype).InvokeMember("Value", System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, value, null);

                            if (tType.Equals(typeof(bool)))
                            {
                                SetValue(columnFormat, cell, value, typeof(bool));
                            }
                            //时间
                            else if (tType.Equals(typeof(DateTime)))
                            {

                                SetValue(columnFormat, cell, value, typeof(DateTime));
                            }
                            //其它基元类型(整数，浮点数， decimal)
                            else if (tType.IsPrimitive || propertytype.Equals(typeof(decimal)))
                            {
                                if (tType.Equals(typeof(Int64))
                                   || tType.Equals(typeof(int))
                                   || tType.Equals(typeof(Int16))
                                   || tType.Equals(typeof(uint))
                                   || tType.Equals(typeof(UInt16))
                                   || tType.Equals(typeof(UInt64))
                                   || tType.Equals(typeof(Byte))
                                   || tType.Equals(typeof(SByte)))
                                {
                                    SetValue(columnFormat, cell, Convert.ToDouble(value), typeof(int));
                                }
                                else
                                {
                                    SetValue(columnFormat, cell, Convert.ToDouble(value), typeof(double));
                                }
                            }
                            else
                            {
                                SetValue(columnFormat, cell, value.ToString(), typeof(string));
                            }
                        }
                        else
                        {
                            SetValue(columnFormat, cell, null, typeof(string));
                        }

                    }
                    //自定义结构 
                    else
                    {
                        SetValue(columnFormat, cell, value.ToString(), typeof(string));
                    }

                }
                else if (propertytype.Equals(typeof(string)))
                {
                    SetValue(columnFormat, cell, value, typeof(string));
                }
                else
                {
                    SetValue(columnFormat, cell, string.Empty, typeof(string));
                }
                //  }
                // else
                //{
                // SetValue(columnFormat, cell, string.Empty, typeof(string));
                // }

            }
            else
            {
                SetValue(columnFormat, cell, string.Empty, typeof(string));
            }
        }

        private static short ChangeExcelDataFormatToCellDataFormat(HSSFWorkbook workBook, ExcelDataFromat format, Type type)
        {
            short cellDataFormat;
            //var format = columnFormat.Format;

            if (type.Equals(typeof(string)))
            {
                format = format ?? StringDefaultFormat;
            }
            else if (type.Equals(typeof(double)))
            {
                format = format ?? DoubleDefaultFormat;
            }
            else if (type.Equals(typeof(bool)))
            {
                format = format ?? BoolDefaultFormat;

            }
            else if (type.Equals(typeof(DateTime)))
            {
                format = format ?? DateTimeDefaultFormat;
            }
            else if (type.Equals(typeof(int)))
            {
                format = format ?? IntDefaultFormat;
            }
            else
            {
                format = format ?? StringDefaultFormat;
            }

            if (format.SystemFromat != null)
            {
                cellDataFormat = HSSFDataFormat.GetBuiltinFormat(format.SystemFromat);
            }
            else
            {
                var dataFormat = workBook.CreateDataFormat();
                cellDataFormat = dataFormat.GetFormat(format.CustomFromat);
            }

            return cellDataFormat;
        }

        private static void SetValue<T>(InternalColumnFormat<T> internalColumnFormat, ICell cell, Object value, Type type)
        {
            if (!internalColumnFormat.IsInitDataFormat)
            {
                internalColumnFormat.BodyCellStyle.DataFormat = ChangeExcelDataFormatToCellDataFormat(internalColumnFormat.WorkBook, internalColumnFormat.Format, type);
                internalColumnFormat.IsInitDataFormat = true;
            }
            cell.CellStyle = internalColumnFormat.BodyCellStyle;

            if (value != null)
            {
                if (type.Equals(typeof(string)))
                {
                    cell.SetCellValue(((string)value));
                }
                else if (type.Equals(typeof(double)))
                {
                    cell.SetCellValue(((double)value));

                }
                else if (type.Equals(typeof(bool)))
                {
                    cell.SetCellValue(((bool)value));

                }
                else if (type.Equals(typeof(DateTime)))
                {
                    cell.SetCellValue(((DateTime)value));
                }
                else if (type.Equals(typeof(int)))
                {
                    cell.SetCellValue(((double)value));
                }
            }
            else
            {
                cell.SetCellValue("");
            }
        }

        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                case CellType.Unknown:
                default:
                    return cell.ToString();
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        private static HSSFWorkbook DataToExcel<T>(IList<T> data, IList<InternalColumnFormat<T>> columnFormats, HSSFWorkbook existWorkBook = null, IList<InternalColumnFormat<T>> headTopColumnFormats = null, IList<InternalColumnFormat<T>> head2ndRowColumnFormats = null)
        {
            try
            {
                HSSFWorkbook workBook;
                if (existWorkBook == null)
                {
                    workBook = columnFormats[0].WorkBook;
                }
                else
                {
                    workBook = existWorkBook;
                }
                List<HSSFCellStyle> styleList = new List<HSSFCellStyle>();
                Type type = typeof(T);

                int pageSize = 65535;
                int page = Convert.ToInt32(Math.Ceiling((data != null ? data.Count : 0) / 65535m));
                bool hasCellSpanRegion = false;

                for (int k = 0; k < page || k == 0 && page == 0; k++)
                {
                    var sheet = workBook.CreateSheet();
                    int rowIndex = 0;
                    int columnIndex = 0;
                    int dataRowIndex = 0;
                    int dataRowSpanDiff = 0;

                    #region 标题行
                    var hssfRow = sheet.CreateRow(rowIndex);
                    if (headTopColumnFormats != null)
                    {
                        foreach (var headTopColumnFormat in headTopColumnFormats)
                        {
                            var cell = hssfRow.CreateCell(columnIndex) as HSSFCell;
                            SetHeaderValue(cell, headTopColumnFormat);
                            columnIndex++;

                        }
                        foreach (var headTopColumnFormat in headTopColumnFormats)
                        {
                            if (headTopColumnFormat.Region != null)
                            {
                                ExcelDataRegion rg = headTopColumnFormat.Region;
                                sheet.AddMergedRegion(new CellRangeAddress(rg.FirstRow, rg.LastRow, rg.FirstColumn, rg.LastColumn));
                            }
                        }
                        columnIndex = 0;
                        rowIndex++;
                    }
                    if (head2ndRowColumnFormats != null)
                    {
                        hssfRow = sheet.CreateRow(rowIndex);
                        foreach (var head2ndRowColumnFormat in head2ndRowColumnFormats)
                        {
                            var cell = hssfRow.CreateCell(columnIndex) as HSSFCell;
                            SetHeaderValue(cell, head2ndRowColumnFormat);
                            columnIndex++;

                        }
                        foreach (var head2ndRowColumnFormat in head2ndRowColumnFormats)
                        {
                            if (head2ndRowColumnFormat.Region != null)
                            {
                                ExcelDataRegion rg = head2ndRowColumnFormat.Region;
                                sheet.AddMergedRegion(new CellRangeAddress(rg.FirstRow, rg.LastRow, rg.FirstColumn, rg.LastColumn));
                            }
                        }
                        columnIndex = 0;
                        rowIndex++;
                    }
                    hssfRow = sheet.CreateRow(rowIndex);
                    foreach (var columnFormat in columnFormats)
                    {
                        var cell = hssfRow.CreateCell(columnIndex) as HSSFCell;
                        SetHeaderValue(cell, columnFormat);
                        columnIndex++;
                        if (!hasCellSpanRegion)
                        {
                            if (columnFormat.Region != null)
                            {
                                hasCellSpanRegion = true;
                                dataRowSpanDiff = columnFormat.Region.LastRow - columnFormat.Region.FirstRow + 1;
                            }
                        }
                    }
                    columnIndex = 0;
                    rowIndex++;
                    #endregion

                    #region 数据行
                    if (data != null)
                    {
                        dataRowIndex = rowIndex;
                        foreach (var row in data.Skip(k * pageSize).Take(pageSize))
                        {
                            hssfRow = sheet.CreateRow(rowIndex);
                            foreach (var columnFormat in columnFormats)
                            {
                                var cell = hssfRow.CreateCell(columnIndex) as HSSFCell;
                                SetBodyCellValue(cell, row, columnFormat);
                                columnIndex++;
                            }
                            if (hasCellSpanRegion)
                            {
                                if ((rowIndex - dataRowIndex) % dataRowSpanDiff == 0)
                                {
                                    foreach (var columnFormat in columnFormats)
                                    {
                                        if (columnFormat.Region != null)
                                        {
                                            ExcelDataRegion rg = columnFormat.Region;
                                            sheet.AddMergedRegion(new CellRangeAddress(rowIndex + rg.FirstRow, rowIndex + rg.LastRow, rg.FirstColumn, rg.LastColumn));
                                        }
                                    }
                                }
                            }
                            rowIndex++;
                            columnIndex = 0;
                        }
                    }
                    #endregion
                }

                return workBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            //string fileName = null;
            ISheet sheet = null;
            IWorkbook workbook = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new HSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                            data.Columns.Add(column);
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="excelFileStream">excel的流文件</param>
        /// <returns></returns>
        public static DataTable RenderFromExcel(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);

                ISheet sheet = workbook.GetSheetAt(0);//取第一个表

                DataTable table = new DataTable();

                IRow headerRow = sheet.GetRow(0);//第一行为标题行
                if (headerRow.Cells[0].StringCellValue == "cardId")
                {
                    int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                    int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                    //handling header.
                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                        table.Columns.Add(column);
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dataRow = table.NewRow();

                        if (row != null)
                        {
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    dataRow[j] = GetCellValue(row.GetCell(j));
                            }
                        }

                        table.Rows.Add(dataRow);
                    }
                    return table;
                }
                return null;
            }
        }
        /// <summary>
        /// 验证excel中是否有数据
        /// </summary>
        /// <param name="excelFileStream">excel流文件</param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                if (workbook.NumberOfSheets > 0)
                {
                    ISheet sheet = workbook.GetSheetAt(0);
                    return sheet.PhysicalNumberOfRows > 0;
                }
            }
            return false;
        }
    }
}
