using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel
{
    /// <summary>
    /// 單元格類
    /// </summary>
    public class ExcelCell
    {
        object _value;
        private ExcelRow _owningRow;
        private ExcelColumn _owningColumn;
        
        /// <summary>
        /// 創建一個單元格對象
        /// </summary>
        public ExcelCell()
        {
            _value = null;
            _owningRow = null;
            _owningColumn = null;
        }

        /// <summary>
        /// 單元格的值
        /// </summary>
        public Object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 單元格關聯的列頭對象，如果還沒有建立關系列，則為NULL
        /// </summary>
        public ExcelColumn OwningColumn
        {
            get { return _owningColumn; }
        }

        /// <summary>
        /// 單元格關聯的行對象，如果還沒有建立關系行，則為NULL
        /// </summary>
        public ExcelRow OwningRow
        { get { return _owningRow; } }

        internal ExcelColumn OwningColumnInternal
        {
            set { _owningColumn = value; }
        }

        internal ExcelRow OwningRowInternal
        {
            set { _owningRow = value; }
        }

        /// <summary>
        /// 相對列位置，如果還沒有建立與列的關系，則為-1
        /// </summary>
        public int ColumnIndex
        { 
            get
            {
                if (_owningColumn == null)
                    return -1;
                else
                    return _owningColumn.Index;
            } 
        }

        /// <summary>
        /// 相對行位置，如果還沒有建立與行的關系，則為-1
        /// </summary>
        public int RowIndex
        { 
            get 
            {
                if (_owningRow == null)
                    return -1;
                else
                    return _owningRow.Index;
            }
        }
    }
}

