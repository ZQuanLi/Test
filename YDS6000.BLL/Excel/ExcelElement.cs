using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel
{
    /// <summary>
    /// 單元格的元素類，被ExcelColumn、ExcelRow類繼承
    /// </summary>
    public class ExcelElement
    {
        private ExcelTable _table;
        private int _index = -1;
        // private bool _isRow = false;

        /// <summary>
        /// 創建一個工作表元素對象
        /// </summary>
        public ExcelElement()
        {
            _table = null;
        }
        /// <summary>
        /// 獲取工作表元素對象所屬的工作表表格
        /// </summary>
        public ExcelTable ExcelTable
        {
            get { return _table; }
        }

        internal ExcelTable ExcelTableInternal
        {
            set
            {
                if (_table != value)
                {
                    _table = value;
                    OnExcelTableChanged();
                }
            }
        }

        /// <summary>
        /// 獲取行或列的序號，如果沒有關系工作表表格，則為-1
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }
        }

        internal int IndexInternal
        {
            set { _index = value; }
        }

        protected virtual void OnExcelTableChanged()
        {

        }
    }
}
