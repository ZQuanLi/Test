using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace Excel
{
    /// <summary>
    /// 列頭信息類,
    /// 注意：是否設置寬（ExcelColumn.IsSetWith)默認值為否。
    /// </summary>
    public class ExcelColumn : ExcelElement
    {
        #region 變量
        private string _columnName;//列名
        private int _with;//列的寬度
        private bool _isSetWith;//是否設置寬
        //private int _columnWith;//列的寬度，_columnWith = _with*256;
        private string _dataPropertyName;//數據的屬性名稱
        private ExcelCellStyle _defaultExcelCellStyle;//單元格樣式
        private ExcelCellStyle _columnExcelCellStyle;//表頭樣式
        private int _columnCount;//列佔的列數

        #endregion

        #region 構造函數

        /// <summary>
        /// 創建一個新的列頭對象
        /// </summary>
        private ExcelColumn()
        {
            _isSetWith = false;
            _columnCount = 1;
            _columnName = string.Empty;
            _dataPropertyName = string.Empty;
            _with = 20;
            _defaultExcelCellStyle =null;// new ExcelCellStyle();
            _columnExcelCellStyle = null;//new ExcelCellStyle();
            //_warpText = false;
            //_index = 0;
        }

        /// <summary>
        /// 創建一個新的列頭對象
        /// </summary>
        /// <param name="columnName">列頭名稱</param>
        public ExcelColumn(string columnName)
            : this()
        {
            _columnName = columnName;
        }

        /// <summary>
        /// 創建一個新的列頭對象
        /// </summary>
        /// <param name="columnName">列頭名稱</param>
        /// <param name="dataPropertyName">數據的屬性名稱</param>
        public ExcelColumn(string columnName, string dataPropertyName)
            : this(columnName)
        {
            _dataPropertyName = dataPropertyName;
        }

        /// <summary>
        /// 創建一個新的列頭對象
        /// </summary>
        /// <param name="columnName">列頭名稱</param>
        /// <param name="dataPropertyName">數據的屬性名稱</param>
        /// <param name="with">列的寬</param>
        public ExcelColumn(string columnName, string dataPropertyName, int with)
            : this(columnName, dataPropertyName)
        {
            _with = with;
            _isSetWith = true;
        }

        /// <summary>
        /// 創建一個新的列頭對象
        /// </summary>
        /// <param name="columnName">列頭名稱</param>
        /// <param name="dataPropertyName">數據的屬性名稱</param>
        /// <param name="with">列的寬</param>
        /// <param name="columnExcelCellStyle">列頭的單元格式</param>
        ///<param name="defaultExcelCellStyle">列值的單元格式</param>
        public ExcelColumn(string columnName, string dataPropertyName, int with, ExcelCellStyle columnExcelCellStyle,ExcelCellStyle defaultExcelCellStyle)
            : this(columnName, dataPropertyName, with)
        {
            _defaultExcelCellStyle = defaultExcelCellStyle ;
            _columnExcelCellStyle = columnExcelCellStyle;
        }

        /// <summary>
        /// 創建一個新的列頭對象
        /// </summary>
        /// <param name="columnCount">列佔用的列數</param>
        /// <param name="columnName">列頭名稱</param>
        /// <param name="dataPropertyName">數據的屬性名稱</param>
        public ExcelColumn(int columnCount, string columnName, string dataPropertyName)
            : this(columnName, dataPropertyName)
        {
            this._columnCount = columnCount;
        }
        #endregion

        #region 屬性

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set { _columnName = value; }
        }

        /// <summary>
        /// 數據屬性名稱
        /// </summary>
        public string DataPropertyName
        {
            get { return _dataPropertyName; }
            set { _dataPropertyName = value; }
        }

        /// <summary>
        /// 列的寬(With=1即一個字符寬度）,隻有IsSetWith為true時，設置才有效
        /// </summary>
        public int With
        {
            get { return _with; }
            set
            {
                if (_with != value)
                {
                    _with = value;
                }
            }
        }

        /// <summary>
        /// 列的實際寬度
        /// </summary>
        public int RealityWith
        {
            get { return _with * 256; }
        }

        /// <summary>
        /// 單元格樣式
        /// </summary>
        public ExcelCellStyle DefaultExcelCellStyle
        {
            get { return _defaultExcelCellStyle; }
            set { _defaultExcelCellStyle = value; }
        }

        /// <summary>
        /// 表頭樣式
        /// </summary>
        public ExcelCellStyle ColumnExcelCellStyle
        {
            get { return _columnExcelCellStyle; }
            set { _columnExcelCellStyle = value; }
        }

        /// <summary>
        /// 獲取或設置列在工作表中佔的列數
        /// </summary>
        public int ColumnCount
        {
            get { return _columnCount; }
            set { _columnCount = value; }
        }

        //internal ICellStyle DefaultCellStyle
        //{
        //    get { return _defaultCellStyle; }
        //    set { _defaultCellStyle = value; }
        //}

        //internal ICellStyle ColumnCellStyle
        //{
        //    get { return _columnCellStyle; }
        //    set { _columnCellStyle = value; }
        //}

        /// <summary>
        /// 獲取或設置是否設置列的寬
        /// </summary>
        public bool IsSetWith
        {
            get { return _isSetWith; }
            set { _isSetWith = value; }
        }
        #endregion
    }
}
