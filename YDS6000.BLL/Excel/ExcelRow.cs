using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel
{
    /// <summary>
    /// 表格列類，構造函數修飾為interal，外部要通過ExcelTable.NewRow()去創建表格列對象。
    /// </summary>
    public class ExcelRow : ExcelElement
    {
        private ExcelCellCollection _cells;

        /// <summary>
        /// 創建一個新的表格列對象,內部使用，外部要通過ExcelTable.NewRow()去創建表格列對象。
        /// </summary>
        internal ExcelRow()
        {
            _cells = new ExcelCellCollection();
        }

        /// <summary>
        /// 設置表格列中的單元格集合的值
        /// </summary>
        /// <param name="values">值的集合，值的順序要與單元格集合的順序對應</param>
        public void SetRowValue(object[] values)
        {
            if (values.Length != _cells.Count)
            {
                throw new ArgumentException("values的列數不對.");
            }
            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].Value = values[i];
            }
        }

        /// <summary>
        /// 獲取單元格集合對象
        /// </summary>
        public ExcelCellCollection Cells
        {
            get { return _cells; }
        }

        internal ExcelCellCollection CellsInternal
        {
            set
            {
                _cells = value;
            }
        }

        protected override void OnExcelTableChanged()
        {
            base.OnExcelTableChanged();
        }
    }
}
