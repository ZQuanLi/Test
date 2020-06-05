using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NPOI.SS.UserModel;

namespace Excel
{
    /// <summary>
    /// 表格類
    /// </summary>
    public class ExcelTable
    {
        ExcelColumnCollection _columns;
        ExcelRowCollection _rows;

        /// <summary>
        /// 創建一個新的表格
        /// </summary>
        public ExcelTable()
        {
            _columns = new ExcelColumnCollection();
            _rows = new ExcelRowCollection();
        }

        /// <summary>
        /// 獲取一個單元格
        /// </summary>
        /// <param name="rowIndex">行序號</param>
        /// <param name="columnIndex">列序號</param>
        /// <returns></returns>
        public object this[int rowIndex,int columnIndex]
        {
            get{return this.Rows[rowIndex].Cells[columnIndex].Value;}
        }

        //public object this[int rowIndex, string datapropertyName]
        //{
        //    get { return this.Rows[rowIndex].Cells[datapropertyName].Value; }
        //}

        /// <summary>
        /// 列頭集合對象
        /// </summary>
        public ExcelColumnCollection Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// 行集合對象
        /// </summary>
        public ExcelRowCollection Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// 創建一個新的行對象
        /// </summary>
        /// <returns></returns>
        public ExcelRow NewRow()
        {
            ExcelRow row = new ExcelRow();
            foreach (ExcelColumn column in _columns)
            {
                ExcelCell cell = new ExcelCell();
                cell.OwningColumnInternal = column;
                cell.OwningRowInternal = row;
                row.Cells.Add(cell);
            }
            return row;
        }

        /// <summary>
        /// 新增一個行對象
        /// </summary>
        /// <param name="values">行中的單元格的值</param>
        public void AddRow(object[] values)
        {
            if (values.Length != Columns.Count)
            {
                throw new ArgumentOutOfRangeException("values的長度與列數不相等。");
            }
            ExcelRow row = new ExcelRow();
            for (int i = 0; i < Columns.Count; i++)
            {
                ExcelCell cell = new ExcelCell();
                cell.OwningColumnInternal = Columns[i];
                cell.Value = values[i];
                cell.OwningRowInternal = row;
                row.Cells.Add(cell);
            }
            Rows.Add(row);
        }

        public int RowCount
        {
            get 
            {
                return _rows.Count; 
            }
        }
    }

}