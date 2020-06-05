using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.IO;

namespace Excel
{
    /// <summary>
    /// 匯出Excel操作類，行或列的起始位置都是從0開始計
    /// </summary>
    public class ExcelOparete
    {
        #region 變量
        private HSSFWorkbook _workbook;
        private ISheet _currentSheet;
        //private int _sheetRowCount;
        private ExcelCellStyle _defaultExcelCellStyle;
        //private int _currentSheetIndex;
        private short _defualtColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;//默認顏色值
        #endregion 變量

        /// <summary>
        /// 創建一個新工作表對象，默認空表格。
        /// </summary>
        public ExcelOparete()
        {
            _workbook = new HSSFWorkbook();
            _currentSheet = null;
            //_sheetRowCount = 0;
            _defaultExcelCellStyle = new ExcelCellStyle();
        }

        /// <summary>
        /// 創建一個新的工作表對象，默認有一個表格。
        /// </summary>
        /// <param name="sheetName">工作表格名</param>
        public ExcelOparete(string sheetName)
            : this()
        {
            _currentSheet = _workbook.CreateSheet(sheetName);
        }

        /// <summary>
        /// 創建一個現有的工作表對象
        /// </summary>
        /// <param name="fullFileName">工作表文件完整路徑</param>
        /// <param name="sheetName">指定當前活動表格的表名，如果為空或NULL值，則默認為第一個表格。</param>
        public ExcelOparete(string fullFileName, string sheetName)
        {
            FileStream file = new FileStream(fullFileName, FileMode.Open, FileAccess.Read);
            this._workbook = new HSSFWorkbook(file);
            file.Dispose();
            if (string.IsNullOrEmpty(sheetName))
            {
                this._currentSheet = this._workbook.GetSheetAt(0);
            }
            else
                this._currentSheet = this._workbook.GetSheet(sheetName);
        }

        #region 屬性

        public ISheet this[int index]
        {
            get { return _workbook.GetSheetAt(index); }
        }

        public ISheet this[string sheetName]
        {
            get { return _workbook.GetSheet(sheetName); }
        }

        /// <summary>
        /// 獲取工作表
        /// </summary>
        public HSSFWorkbook WorkBook
        { get { return _workbook; } }

        /// <summary>
        /// 獲取或設置當前活動表
        /// </summary>
        public ISheet Current
        {
            get { return _currentSheet; }
            set { _currentSheet = value; }
        }

        /// <summary>
        /// 表的行數,取行Excel的最大行数
        /// </summary>
        public int SheetRowCount
        {
            get { return this._currentSheet.PhysicalNumberOfRows; }
        }

        /// <summary>
        /// 獲取或設置工作表默認單元格格式
        /// </summary>
        public ExcelCellStyle DefaultExcelCellStyle
        {
            get { return _defaultExcelCellStyle; }
            set { _defaultExcelCellStyle = value; }
        }

        /// <summary>
        /// 獲取表格數量
        /// </summary>
        public int SheetCount
        {
            get { return _workbook.NumberOfSheets; }
        }

        /// <summary>
        /// 獲取當前活動表的序號
        /// </summary>
        public int CurrentSheetIndex
        {
            get { return _workbook.GetSheetIndex(_currentSheet); }
        }

        #endregion 屬性

        #region 公用方法
        /// <summary>
        /// 增加一個新的表格，並將新增的表格作為活動表
        /// </summary>
        /// <param name="sheetName">表名稱</param>
        public void AddSheet(string sheetName)
        {
            _currentSheet = _workbook.CreateSheet(sheetName);
        }

        /// <summary>
        /// 增加一個新的表格，並將新增的表格作為活動表
        /// </summary>
        /// <param name="sheetName">表名稱</param>
        public void CloneSheet(string sheetName, string newSheetName, ref int sheetIndex)
        {
            int i = _workbook.GetSheetIndex(_workbook.GetSheet(sheetName));
            _currentSheet = _workbook.CloneSheet(i);
            sheetIndex = _workbook.GetSheetIndex(_currentSheet);
            _workbook.SetSheetName(_workbook.GetSheetIndex(_currentSheet), newSheetName);
            _defaultExcelCellStyle = new ExcelCellStyle();
        }

        public void SetSheetName(string sheetName, string newSheetName, ref int sheetIndex)
        {
            int i = _workbook.GetSheetIndex(_workbook.GetSheet(sheetName));
            _currentSheet = _workbook.GetSheetAt(i);
            _workbook.SetSheetName(_workbook.GetSheetIndex(_currentSheet), newSheetName);
            _workbook.SetSheetOrder(newSheetName, _workbook.NumberOfSheets);
            _defaultExcelCellStyle = new ExcelCellStyle();
        }

        /// <summary>
        /// 設置標題值,自動合並單元格
        /// </summary>
        /// <param name="titleString">標題文本</param>
        /// <param name="firstRowIndex">開始行位置</param>
        /// <param name="lastRowIndex">結束行位置</param>
        /// <param name="firstColumnIndex">開始的列位置</param>
        /// <param name="lastColumnIndex">結束的列位置</param>
        /// <param name="titleExceCellStyle">單元格樣式，如果為NULL，則為默認（12號字體、加粗）</param>
        public void SetTitleValue(string titleString, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex, ExcelCellStyle titleExceCellStyle)
        {
            if (titleExceCellStyle == null)
            {
                titleExceCellStyle = new ExcelCellStyle(12, FontBoldWeight.BOLD);
                titleExceCellStyle.HorizontalAlignment = HorizontalAlignment.CENTER;
            }
            for (int i = firstRowIndex; i <= lastRowIndex; i++)
            {
                CreateRow(i, firstColumnIndex, lastColumnIndex, titleExceCellStyle);
            }
            IRow row = this._currentSheet.GetRow(firstRowIndex);

            if (lastColumnIndex > firstColumnIndex)
                this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(firstRowIndex, lastRowIndex, firstColumnIndex, lastColumnIndex));
            row.GetCell(firstColumnIndex).SetCellValue(titleString);
        }

        /// <summary>
        /// 設置標題值,自動合並單元格,默認（12號字體、加粗）
        /// </summary>
        /// <param name="titleString">標題文本</param>
        /// <param name="firstRowIndex">開始行位置</param>
        /// <param name="lastRowIndex">結束行位置</param>
        /// <param name="firstColumnIndex">開始的列位置</param>
        /// <param name="lastColumnIndex">結束的列位置</param>
        public void SetTitleValue(string titleString, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            SetTitleValue(titleString, firstRowIndex, lastRowIndex, firstColumnIndex, lastColumnIndex, null);
        }

        /// <summary>
        /// 設置一個對象的值,自動合並列,單元格格為工作表默認的值
        /// </summary>
        /// <param name="value">對象</param>
        /// <param name="rowIndex">行位置</param>
        /// <param name="firstColumnIndex">起始列</param>
        /// <param name="lastColumnIndex">結束列</param>
        /// <param name="cellStyle">如果為NULL，則為工作表默認的值</param>
        public void SetObjectValue(object value, int rowIndex, int firstColumnIndex, int lastColumnIndex, ExcelCellStyle cellStyle)
        {
            IRow row = CreateRow(rowIndex, firstColumnIndex, lastColumnIndex, cellStyle);
            if (lastColumnIndex > firstColumnIndex)
                this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumnIndex, lastColumnIndex));
            SetCellValue(value, row, firstColumnIndex);
        }

        /// <summary>
        /// 設置一個對象的值,自動合並列,單元格格為工作表默認的值
        /// </summary>
        /// <param name="value">對象</param>
        /// <param name="rowIndex">行位置</param>
        /// <param name="firstColumnIndex">起始列</param>
        /// <param name="lastColumnIndex">結束列</param>
        public void SetObjectValue(object value, int rowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            SetObjectValue(value, rowIndex, firstColumnIndex, lastColumnIndex, null);
        }


        public void SetRowValue(List<string> col_name, DataRow dr, int rowIndex, Dictionary<string, Excel.ExcelCellStyle> style_dict)
        {
            int columnIndex = -1;
            foreach (string item in col_name)
            {
                columnIndex++;
                if (style_dict.ContainsKey(item))
                {
                    this.SetObjectValue(dr[item], rowIndex, columnIndex, columnIndex, style_dict[item]);
                }
                else
                {
                    this.SetObjectValue(dr[item], rowIndex, columnIndex, columnIndex);
                }
            }
        }

        //public void setObjectValue(PCM.Common.Excel.ExcelOparete excel, object obj, int rowIndex, ref int columnIndex)
        //{
        //    columnIndex += 1;
        //    excel.SetObjectValue(obj, rowIndex, columnIndex, columnIndex);
        //}

        /// <summary>
        /// 設置一個對象的值,自動合並行列,單元格格為工作表默認的值
        /// </summary>
        /// <param name="value">對象</param>
        /// <param name="rowIndex1">開始行位置</param>
        /// <param name="rowIndex2">結束行位置</param>
        /// <param name="firstColumnIndex">起始列</param>
        /// <param name="lastColumnIndex">結束列</param>
        /// <param name="cellStyle">如果為NULL，則為工作表默認的值</param>
        public void SetObjectValue2(object value, int rowIndex1, int rowIndex2, int firstColumnIndex, int lastColumnIndex, ExcelCellStyle cellStyle)
        {
            IRow row = CreateRow(rowIndex1, firstColumnIndex, lastColumnIndex, cellStyle);
            if (lastColumnIndex > firstColumnIndex || rowIndex2 > rowIndex1)
                this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex1, rowIndex2, firstColumnIndex, lastColumnIndex));
            SetCellValue(value, row, firstColumnIndex);
        }

        /// <summary>
        /// 設置一個對象的值,自動合並列,單元格格為工作表默認的值
        /// </summary>
        /// <param name="value">對象</param>
        /// <param name="rowIndex1">開始行位置</param>
        /// <param name="rowIndex2">結束行位置</param>
        /// <param name="firstColumnIndex">起始列</param>
        /// <param name="lastColumnIndex">結束列</param>
        public void SetObjectValue2(object value, int rowIndex1, int rowIndex2, int firstColumnIndex, int lastColumnIndex)
        {
            SetObjectValue2(value, rowIndex1, rowIndex2, firstColumnIndex, lastColumnIndex, null);
        }

        /// <summary>
        /// 將ExcelTableRows中的單元格值裝載入當前工作表中。
        /// </summary>
        /// <param name="excelTable">數據源</param>
        /// <param name="firstRowIndex">起始行號</param>
        /// <param name="firstColumnIndex">起始列號</param>
        public void SetExcelTableRows(ExcelTable excelTable, int firstRowIndex, int firstColumnIndex)
        {
            if (excelTable == null)
                throw new ArgumentNullException("excelTable參數不能為NULL.");
            SetColumnWithByColumns(excelTable.Columns, firstColumnIndex);
            int curRowIndex = firstRowIndex;
            for (int i = 0; i < excelTable.RowCount; i++)
            {
                int curColumnIndex = firstColumnIndex;
                curRowIndex = firstRowIndex + i;
                IRow row = CreateRow(curRowIndex, firstColumnIndex, excelTable.Columns.RealCount + firstColumnIndex - 1, null);

                foreach (ExcelCell cell in excelTable.Rows[i].Cells)
                {
                    for (int cellCount = curColumnIndex; cellCount < cell.OwningColumn.ColumnCount + curColumnIndex; cellCount++)
                    {
                        if (row.GetCell(cellCount) == null)
                            row.CreateCell(cellCount);
                        row.GetCell(cellCount).CellStyle
                            = cell.OwningColumn.DefaultExcelCellStyle == null ? this._defaultExcelCellStyle.GetCellStyle(this.WorkBook) : cell.OwningColumn.DefaultExcelCellStyle.GetCellStyle(this.WorkBook);
                    }
                    if (cell.OwningColumn.ColumnCount > 1)
                    {
                        this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(curRowIndex, curRowIndex, curColumnIndex, curColumnIndex + cell.OwningColumn.ColumnCount - 1));
                    }

                    SetCellValue(cell.Value, row, curColumnIndex);
                    curColumnIndex += cell.OwningColumn.ColumnCount;
                }
            }

        }

        /// <summary>
        /// 將ExcelColumnCollection表頭值裝載到當前工作表中
        /// </summary>
        /// <param name="columns">數據源</param>
        /// <param name="rowIndex">當前行號</param>
        /// <param name="firstColumnIndex">起始列號</param>
        public void SetColumnName(ExcelColumnCollection columns, int rowIndex, int firstColumnIndex)
        {
            int curColumnIndex = firstColumnIndex;
            SetColumnWithByColumns(columns, firstColumnIndex);
            IRow row = CreateRow(rowIndex, curColumnIndex, curColumnIndex + columns.RealCount - 1, null);
            for (int i = 0; i < columns.Count; i++)
            {
                for (int columnIndex = curColumnIndex; columnIndex < columns[i].ColumnCount + curColumnIndex; columnIndex++)
                {
                    if (row.GetCell(columnIndex) == null)
                    {
                        row.CreateCell(columnIndex);
                    }
                    row.GetCell(columnIndex).CellStyle
                        = columns[i].ColumnExcelCellStyle == null ? this._defaultExcelCellStyle.GetCellStyle(this.WorkBook) : columns[i].ColumnExcelCellStyle.GetCellStyle(this.WorkBook);
                    int kk = this.WorkBook.NumCellStyles;
                }
                if (columns[i].ColumnCount > 1)
                {
                    this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, curColumnIndex, curColumnIndex + columns[i].ColumnCount - 1));
                }
                row.GetCell(curColumnIndex).SetCellValue(columns[i].ColumnName);
                curColumnIndex += columns[i].ColumnCount;
            }
        }

        /// <summary>
        /// 將DataRow[]的值裝載到當前工作表中
        /// </summary>
        /// <param name="columns">數據源對應工作表中的列頭集合</param>
        /// <param name="dataRows">數據源</param>
        /// <param name="rowIndex">起始行號</param>
        /// <param name="firstColumnIndex">起始列號</param>
        public void SetColumnValue(ExcelColumnCollection columns, DataRow[] dataRows, int rowIndex, int firstColumnIndex)
        {
            if (dataRows == null || dataRows.Length <= 0)
                return;
            //SetColumnWithByColumns(columns, firstColumnIndex);
            int curColumnIndex = 0;
            int pageCount = 1;
            string firstSheetName = _currentSheet.SheetName;
            for (int i = 0; i < dataRows.Length; i++, rowIndex++)
            {
                //if (i > 65000)
                //    return;
                curColumnIndex = firstColumnIndex;
                IRow row = CreateRow(rowIndex, firstColumnIndex, columns.RealCount + firstColumnIndex - 1, null);
                for (int cIndex = 0; cIndex < columns.Count; cIndex++)
                {
                    for (int columnIndex = curColumnIndex; columnIndex < columns[cIndex].ColumnCount + curColumnIndex; columnIndex++)
                    {
                        if (row.GetCell(columnIndex) == null)
                            row.CreateCell(columnIndex);
                        row.GetCell(columnIndex).CellStyle
                            = columns[cIndex].DefaultExcelCellStyle == null ? this._defaultExcelCellStyle.GetCellStyle(this.WorkBook) : columns[cIndex].DefaultExcelCellStyle.GetCellStyle(this.WorkBook);
                        int kk = this.WorkBook.NumCellStyles;
                    }
                    if (columns[cIndex].ColumnCount > 1)
                    {
                        this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, curColumnIndex, curColumnIndex + columns[cIndex].ColumnCount - 1));
                    }
                    if (!string.IsNullOrEmpty(columns[cIndex].DataPropertyName))
                        SetCellValue(dataRows[i][columns[cIndex].DataPropertyName], row, curColumnIndex);
                    curColumnIndex += columns[cIndex].ColumnCount;
                }
                if (this._currentSheet.PhysicalNumberOfRows > 65000)
                {
                    AddSheet(firstSheetName + pageCount.ToString());

                    SetColumnName(columns, 0, firstColumnIndex);
                    pageCount += 1;
                    rowIndex = 0;
                }
            }
        }

        /// <summary>
        /// 保存工作表
        /// </summary>
        /// <param name="fileName">工作表的文件名稱</param>
        /// <returns>保存後，返回工作表的完整路徑</returns>
        public string SaveExcel(string fileName)
        {
            string path1 = @"C:\temp";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            string fname1 = path1 + @"\" + fileName + @".xls";
            FileStream fs1 = new FileStream(fname1, FileMode.Create);
            this.WorkBook.Write(fs1);
            fs1.Close();
            return fname1;
        }

        /// <summary>
        /// 保存工作表
        /// </summary>
        /// <param name="fullFileName">工作表的完整路徑+文件名</param>
        public void SaveExcelByFullFileName(string fullFileName)
        {
            FileStream fs1 = new FileStream(fullFileName, FileMode.Create);
            this.WorkBook.Write(fs1);
            fs1.Close();
        }

        /// <summary>
        /// 設置列的寬
        /// </summary>
        /// <param name="columnIndex">列序號</param>
        /// <param name="with">列寬</param>
        public void SetCellWith(int columnIndex, int with)
        {
            this._currentSheet.SetColumnWidth(columnIndex, with * 256);
        }

        /// <summary>
        /// 獲取行對象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IRow GetRow(int index)
        {
            return _currentSheet.GetRow(index);
        }

        /// <summary>
        /// 設置當前活動表格
        /// </summary>
        /// <param name="sheetIndex"></param>
        public void SetActiveSheet(int sheetIndex)
        {
            _currentSheet = _workbook.GetSheetAt(sheetIndex);
        }
        /// <summary>
        /// 設置當前活動表
        /// </summary>
        /// <param name="sheetName"></param>
        public void SetActiveSheet(string sheetName)
        {
            _currentSheet = _workbook.GetSheet(sheetName);
        }

        #endregion 公共方法

        #region 私有方法

        /// <summary>
        /// 設置列的寬
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="firstColumnIndex"></param>
        private void SetColumnWithByColumns(ExcelColumnCollection columns, int firstColumnIndex)
        {
            if (columns == null)
                return;

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].IsSetWith)
                {
                    for (int cellIndex = 0; cellIndex < columns[i].ColumnCount; cellIndex++)
                    {
                        this._currentSheet.SetColumnWidth(cellIndex + firstColumnIndex, columns[i].RealityWith);
                    }
                }
                firstColumnIndex += columns[i].ColumnCount;
            }
        }

        /// <summary>
        /// 更新ExcelCollection對象中的單元格格式，如果不NULL，則為當前默認值。
        /// </summary>
        /// <param name="columns"></param>
        private void UpdateExcelColumnsOfCellStycle(ExcelColumnCollection columns)
        {
            foreach (ExcelColumn column in columns)
            {
                if (column.DefaultExcelCellStyle == null)
                    column.DefaultExcelCellStyle = this._defaultExcelCellStyle;
                if (column.ColumnExcelCellStyle == null)
                    column.ColumnExcelCellStyle = this._defaultExcelCellStyle;
            }
        }
        /// <summary>
        /// 設置單元格的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        private void SetCellValue(object value, IRow row, int columnIndex)
        {
            if (value != null && value != DBNull.Value)
            {
                Type valueType = value.GetType();
                if (valueType == typeof(decimal) || valueType == typeof(int) || valueType == typeof(float) || valueType == typeof(double))
                {
                    row.GetCell(columnIndex).SetCellValue(double.Parse(value.ToString()));
                }
                else if (valueType == typeof(DateTime))
                {
                    DateTime dt1 = DateTime.Parse(value.ToString());
                    if (dt1.Year > 1900)
                    {
                        row.GetCell(columnIndex).SetCellValue(dt1);
                    }
                    else
                    {
                        row.GetCell(columnIndex).SetCellValue("");
                    }
                }
                else if (valueType == typeof(bool) || valueType == typeof(Boolean))
                {
                    row.GetCell(columnIndex).SetCellValue(bool.Parse(value.ToString()));
                }
                else
                    row.GetCell(columnIndex).SetCellValue(value.ToString());
            }
        }

        private IRow CreateRow(int index, int firstColumnIndex, int lastColumnIndex, ExcelCellStyle cellStyle)
        {
            IRow row = _currentSheet.GetRow(index);
            //ExcelCellStyle tempStyle = cellStyle == null ? _defaultExcelCellStyle : cellStyle;
            if (row == null)
            {

                row = this._currentSheet.CreateRow(index);
                for (int i = firstColumnIndex; i <= lastColumnIndex; i++)
                {
                    row.CreateCell(i);
                    row.GetCell(i).CellStyle = cellStyle == null ? _defaultExcelCellStyle.GetCellStyle(this.WorkBook) : cellStyle.GetCellStyle(this.WorkBook);
                }
            }
            else
            {
                for (int i = firstColumnIndex; i <= lastColumnIndex; i++)
                {
                    if (row.GetCell(i) == null)
                    {
                        row.CreateCell(i);
                    }
                    row.GetCell(i).CellStyle = cellStyle == null ? _defaultExcelCellStyle.GetCellStyle(this.WorkBook) : cellStyle.GetCellStyle(this.WorkBook);
                }
            }
            int kk = this.WorkBook.NumCellStyles;
            return row;
        }
        #endregion 私有方法

        #region 設置邊框的方法，注意：如果設置的邊框的單元格總數超過4000，則會報表超過最大樣式的異常。

        /// <summary>
        /// 設置指定區域的四周邊框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        /// <param name="color"></param>
        public void SetEnclosedBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetEnclosedBorderOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, firstColumn, lastColumn), borderStyle, color);

        }

        /// <summary>
        /// 設置指定區域的四周邊框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytyle"></param>
        public void SetEnclosedBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderSytyle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetEnclosedBorderOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, firstColumn, lastColumn), borderSytyle, _defualtColor);
        }

        /// <summary>
        /// 設置指定區域的下邊櫃
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderBottomOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderBottomOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, color);
        }

        /// <summary>
        /// 設置指定區域的下邊櫃
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        public void SetBorderBottomOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderBottomOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, _defualtColor);
        }

        /// <summary>
        /// 設置指定區域的上邊櫃
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderTopOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderTopOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, color);
        }

        /// <summary>
        /// 設置指定區域的上邊櫃
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        public void SetBorderTopOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderTopOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, _defualtColor);
        }

        /// <summary>
        /// 設置指定區域的左邊櫃
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderLeftOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderLeftOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, color);
        }

        /// <summary>
        /// 設置指定區域的左邊櫃
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        public void SetBorderLeftOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderLeftOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, _defualtColor);
        }
        /// <summary>
        /// 設置指定區域的右邊櫃
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderRightOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderRightOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, color);
        }

        /// <summary>
        /// 設置指定區域的右邊櫃
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        /// <param name="?"></param>
        public void SetBorderRightOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderRightOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, _defualtColor);
        }

        /// <summary>
        /// 設置指定區域的內部邊框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        /// <param name="color"></param>
        public void SetInternalBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            for (int i = firstRow; i < lastRow; i++)//設置水平邊線
            {
                s.SetBorderBottomOfRegion(new NPOI.SS.Util.CellRangeAddress(i, i, firstColumn, lastColumn), borderStyle, color);
            }
            for (int i = firstColumn; i < lastColumn; i++)//設置垂直邊線
            {
                s.SetBorderRightOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, i, i), borderStyle, color);
            }
        }
        /// <summary>
        /// 設置指定區域的內部邊框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        public void SetInternalBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle)
        {
            SetInternalBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle, _defualtColor);
        }
        /// <summary>
        /// 設置指定區域所有的邊框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        public void SetAllBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle)
        {
            SetEnclosedBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle);
            SetInternalBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle);
        }

        /// <summary>
        /// 設置指定區域所有的邊框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        /// <param name="color"></param>
        public void SetAllBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle, short color)
        {
            SetEnclosedBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle, color);
            SetInternalBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle, color);
        }
        #endregion

        #region 打印設置方法

        /// <summary>
        /// 設置當工作表所有表格的打印方向和紙張
        /// </summary>
        /// <param name="isLandscape">是否橫向</param>
        /// <param name="paperSize">紙張（9=A4）</param>
        public void SetWorkbookPrint(bool isLandscape, short paperSize)
        {
            if (this._workbook != null)
            {
                if (_workbook.NumberOfSheets > 0)
                {
                    for (int i = 0; i < _workbook.NumberOfSheets; i++)
                    {
                        SetSheetPrint(_workbook.GetSheetAt(i), isLandscape, paperSize);
                    }
                }
            }
        }

        /// <summary>
        /// 設置當工作表所有表格的打印方向和紙張,默認為A4紙
        /// </summary>
        /// <param name="isLandscape">是否橫向</param>
        public void SetWorkbookPrint(bool isLandscape)
        {
            SetWorkbookPrint(isLandscape, 9);
        }

        /// <summary>
        /// 設置所有工作表顯示頁碼
        /// </summary>
        public void SetPrintPageNum()
        {
            if (this._workbook != null)
            {
                if (_workbook.NumberOfSheets > 0)
                {
                    for (int i = 0; i < _workbook.NumberOfSheets; i++)
                    {
                        PrintPageNum(_workbook.GetSheetAt(i));
                    }
                }
            }
        }

        /// <summary>
        /// 設置工作表的打印方向，紙張
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="isLandscape">是否橫向</param>
        /// <param name="paperSize">紙張（9=A4紙）</param>
        private void SetSheetPrint(ISheet sheet, bool isLandscape, short paperSize)
        {
            sheet.PrintSetup.Landscape = isLandscape;
            sheet.PrintSetup.PaperSize = paperSize;
            // sheet.PrintSetup.Scale = 100;
        }

        /// <summary>
        /// 設置當工作表所有表格的打印方向,紙張默認為A4
        /// </summary>
        /// <param name="isLandscape">是否橫向</param>
        public void SetSheetPrint(bool isLandscape)
        {
            _currentSheet.PrintSetup.Landscape = isLandscape;
            _currentSheet.PrintSetup.PaperSize = 9;
            //_currentSheet.PrintSetup.Scale = 100;

        }
        /// <summary>
        /// 設置當工作表所有表格的打印方向,紙張大小
        /// </summary>
        /// <param name="isLandscape">是否橫向打印</param>
        /// <param name="paperSize">紙張大小</param>
        public void SetSheetPrint(bool isLandscape, short paperSize)
        {
            _currentSheet.PrintSetup.Landscape = isLandscape;
            _currentSheet.PrintSetup.PaperSize = paperSize;
        }
        /// <summary>
        /// 設置工作表打印顯示頁碼,頁碼顯示在右上角,格式：(頁碼：1/1)
        /// </summary>
        /// <param name="sheet"></param>
        public void PrintPageNum(ISheet sheet)
        {
            sheet.Header.Right = "頁碼：" + HSSFHeader.Page + @"/" + HSSFHeader.NumPages;
        }

        /// <summary>
        /// 設置頁邊距
        /// </summary>
        /// <param name="leftMargin"></param>
        /// <param name="topMargin"></param>
        /// <param name="rightMargin"></param>
        /// <param name="bottomMargin"></param>
        public void SetPrintMargin(double leftMargin, double topMargin, double rightMargin, double bottomMargin)
        {
            this._currentSheet.SetMargin(MarginType.LeftMargin, leftMargin);
            this._currentSheet.SetMargin(MarginType.TopMargin, topMargin);
            this._currentSheet.SetMargin(MarginType.RightMargin, rightMargin);
            this._currentSheet.SetMargin(MarginType.BottomMargin, bottomMargin);
        }

        /// <summary>
        /// 設置頁眉邊距
        /// </summary>
        /// <param name="headerMargin"></param>
        public void SetPrintHeaderMargin(double headerMargin)
        {
            this._currentSheet.PrintSetup.HeaderMargin = headerMargin;
        }

        /// <summary>
        /// 設置頁腳邊距
        /// </summary>
        /// <param name="footerMargin"></param>
        public void SetPrintFooterMarging(double footerMargin)
        {
            this._currentSheet.PrintSetup.FooterMargin = footerMargin;
        }

        /// <summary>
        /// 設置按比例打印
        /// </summary>
        /// <param name="scale"></param>
        public void SetPrintScale(short scale)
        {
            this._currentSheet.PrintSetup.FitWidth = 0;
            this._currentSheet.PrintSetup.FitHeight = 0;
            this._currentSheet.PrintSetup.Scale = scale;
        }

        /// <summary>
        /// 設置按比例打印
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="scale"></param>
        public void SetPrintScale(int sheetIndex, short scale)
        {
            ISheet s = this._workbook.GetSheetAt(sheetIndex);
            s.PrintSetup.FitWidth = 0;
            s.PrintSetup.FitHeight = 0;
            s.PrintSetup.Scale = scale;
        }
        /// <summary>
        /// 設置打印區域
        /// </summary>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintArea(int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            this._workbook.SetPrintArea(_workbook.GetSheetIndex(_currentSheet), firstColumnIndex, lastColumnIndex, firstRowIndex, lastRowIndex);
        }
        /// <summary>
        /// 設置打印區域
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintArea(int sheetIndex, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            this._workbook.SetPrintArea(sheetIndex, firstColumnIndex, lastColumnIndex, firstRowIndex, lastRowIndex);
        }

        /// <summary>
        /// 設置重復打印區域
        /// </summary>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintRepeatRange(int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            this.WorkBook.SetRepeatingRowsAndColumns(_workbook.GetSheetIndex(_currentSheet), firstColumnIndex, lastColumnIndex, firstRowIndex, lastRowIndex);
        }

        /// <summary>
        /// 設置重復打印區域
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintRepeatRange(int sheetIndex, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            this.WorkBook.SetRepeatingRowsAndColumns(sheetIndex, firstColumnIndex, lastColumnIndex, firstRowIndex, lastRowIndex);
        }
        #endregion 打印設置方法

        public void SetSheetHidden(string sheetName)
        {
            int i = _workbook.GetSheetIndex(_workbook.GetSheet(sheetName));
            if (i > -1)
            {
                _workbook.SetSheetHidden(i, true);
            }
            if (i < _workbook.NumberOfSheets - 1)
            {
                _workbook.SetActiveSheet(i + 1);
            }
            else
            {
                _workbook.SetActiveSheet(i - 1);
            }
        }
    }
}
