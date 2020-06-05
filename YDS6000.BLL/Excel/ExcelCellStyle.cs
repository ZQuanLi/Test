using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.HSSF.Record.CF;

namespace Excel
{
    /// <summary>
    /// Excel的單元格格式類
    /// 默認值：字體：Arial，大小：9，字形：普通，水平左對齊，垂直居中,不自動換行。
    /// </summary>
    public class ExcelCellStyle:ICloneable
    {
        private int _fontSize;
        private string _fontName;
        private FontBoldWeight _fontBold;
        private HorizontalAlignment _alignment;
        private VerticalAlignment _verticalAlignment;
        private string _dataFormart;
        private BorderStyle _borderBottom;
        private BorderStyle _borderLeft;
        private BorderStyle _borderTop;
        private BorderStyle _borderRight;
        private bool _styleValueIsChange;
        private ICellStyle _cellStyle;
        private bool _warpText;
        private UnderLine _underline;
        private short _fontColor;
        private bool _isItalic;
        private bool _isStrikeout;
        private short _fontTypeOffset;
        /// <summary>
        /// 創建一個新的單元格格式對象
        /// </summary>
        public ExcelCellStyle()
        {  // ICellStyle e;
  
            _dataFormart = string.Empty;
            _fontSize = 9;
            _fontBold = FontBoldWeight.None;
            _fontName = "Arial";
            _alignment = HorizontalAlignment.LEFT;
            _verticalAlignment = VerticalAlignment.CENTER;
            _borderBottom = BorderStyle.NONE;
            _borderLeft = BorderStyle.NONE;
            _borderRight = BorderStyle.NONE;
            _borderTop = BorderStyle.NONE;
            _styleValueIsChange = true;
            _cellStyle = null;
            _warpText = false;
            _underline = Excel.UnderLine.NONE;
            _fontColor = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            _isItalic = false;
            _isStrikeout = false;
            _fontTypeOffset = 0;
        }

        /// <summary>
        /// 創建一個新的單元格格式對象
        /// </summary>
        /// <param name="fontSize">字體大小</param>
        public ExcelCellStyle(int fontSize)
            : this()
        {
            _fontSize = fontSize;
        }

        //public ExcelCellStyle(int fontSize, string fontName)
        //    : this(fontSize)
        //{
        //    this._fontName = fontName;
        //}

        /// <summary>
        /// 創建一個新的單元格格式對象
        /// </summary>
        /// <param name="fontSize">字體大小</param>
        /// <param name="fontBold">字體字形</param>
        public ExcelCellStyle(int fontSize, FontBoldWeight fontBold)
            : this(fontSize)
        {
            this._fontBold = fontBold;
        }

        /// <summary>
        /// 創建一個新的單元格格式對象
        /// </summary>
        /// <param name="fontSize">字體大小</param>
        /// <param name="fontBold">字體字形</param>
        /// <param name="fontName">字體名稱</param>
        public ExcelCellStyle(int fontSize, FontBoldWeight fontBold, string fontName)
            : this(fontSize, fontBold)
        {
            this.FontName = fontName;
        }

        #region 屬性

        /// <summary>
        /// 設置或獲取字體是否有刪除線
        /// </summary>
        public bool FontIsStrikeout
        {
            get { return _isStrikeout; }
            set
            {
                if (_isStrikeout != value)
                {
                    _isStrikeout = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取字體是否斜體
        /// </summary>
        public bool FontIsItalic
        {
            get { return _isItalic; }
            set
            {
                if (_isItalic != value)
                {
                    _isItalic = value;
                    _styleValueIsChange = true; ;
                }
            }
        }

        /// <summary>
        /// 設置或獲取字體顏色
        /// </summary>
        public short FontColor
        {
            get { return _fontColor; }
            set {
                if (_fontColor != value)
                {
                    _fontColor = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取下邊框
        /// </summary>
        public BorderStyle BorderBottom
        {
            get { return _borderBottom; }
            set
            {
                if (_borderBottom != value)
                {
                    _borderBottom = value;
                    _styleValueIsChange = true;
                }
            }
        }
        /// <summary>
        /// 設置或獲取上邊框
        /// </summary>
        public BorderStyle BorderTop
        {
            get { return _borderTop; }
            set
            {
                if (_borderTop != value)
                {
                    _borderTop = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取左邊框
        /// </summary>
        public BorderStyle BorderLeft
        {
            get { return _borderLeft; }
            set
            {
                if (_borderLeft != value)
                {
                    _borderLeft = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取右邊框
        /// </summary>
        public BorderStyle BorderRight
        {
            get { return _borderRight; }
            set
            {
                if (_borderRight != value)
                {
                    _borderRight = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取字體大小
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    _styleValueIsChange = true;
                }
            }
        }
        /// <summary>
        /// 設置或獲取字體字形
        /// </summary>
        public FontBoldWeight FontBold
        { get { return _fontBold; }
            set
            {
                if (_fontBold != value)
                {
                    _fontBold = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取字體名稱
        /// </summary>
        public string FontName
        {
            get { return _fontName; }
            set
            {
                if(_fontName != value)
                {
                    _fontName = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取單元格的水平對齊方式
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return _alignment; }
            set 
            { 
                if (_alignment != value) 
                { 
                    _alignment = value;
                    _styleValueIsChange = true; 
                } 
            }
        }

        /// <summary>
        /// 設置或獲取單元格的垂直對齊方式
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                if (_verticalAlignment != value)
                {
                    _verticalAlignment = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取格式化格式
        /// </summary>
        public string DataFormart
        {
            get { return _dataFormart; }
            set
            {
                if (_dataFormart != value)
                {
                    _dataFormart = value;
                    _styleValueIsChange = true;
                }
            }
        }

        /// <summary>
        /// 設置或獲取是否自動換行
        /// </summary>
        public bool WarpText
        {
            get { return _warpText; }
            set { _warpText = value;
            _styleValueIsChange = true;
            }
        }

        /// <summary>
        /// 設置或獲取字體的下劃線
        /// </summary>
        public UnderLine UnderLine
        {
            get { return _underline; }
            set {
                _underline = value;
                _styleValueIsChange = true;
            }
        }

        ///// <summary>
        ///// 樣式的值是否改變
        ///// </summary>
        //internal bool StyleValueIsChange
        //{ 
        //    get { return _styleValueIsChange; } 
        //    set{_styleValueIsChange =value;}
        //}

        #endregion

        #region 方法

        internal ICellStyle GetCellStyle(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {
            if (workBook == null)
            {
                throw new ArgumentNullException("workBook參數不能為NULL。");
            }
            
            if (_cellStyle == null || _styleValueIsChange)
            {
                _cellStyle = workBook.CreateCellStyle();
                IFont font= workBook.FindFont((short)_fontBold, _fontColor, (short)(_fontSize * 20), _fontName, _isItalic, _isStrikeout, _fontTypeOffset, (byte)_underline);
                if (font == null)
                {
                    font = workBook.CreateFont();                   
                    font.FontName = this._fontName;
                    font.Boldweight = (short)this._fontBold;
                    font.FontHeightInPoints = (short)this._fontSize;
                    font.Underline = (byte)this.UnderLine;
                    font.IsItalic = _isItalic;
                    font.Color = _fontColor;
                    font.IsStrikeout = _isStrikeout;
                    font.TypeOffset = _fontTypeOffset;
                }
                _cellStyle.SetFont(font);
                IDataFormat format = workBook.CreateDataFormat();
                _cellStyle.BorderBottom = this._borderBottom;
                _cellStyle.BorderLeft = this._borderLeft;
                _cellStyle.BorderRight = this._borderRight;
                _cellStyle.BorderTop = this._borderTop;
                _cellStyle.Alignment = this._alignment;
                _cellStyle.VerticalAlignment = this.VerticalAlignment;
                if(!string.IsNullOrEmpty(_dataFormart))
                    _cellStyle.DataFormat = format.GetFormat(this._dataFormart);
                _cellStyle.WrapText = this._warpText;
                _styleValueIsChange = false;
            }
            return _cellStyle;
        }

        /// <summary>
        /// 設置單元格邊框
        /// </summary>
        /// <param name="left">左邊框</param>
        /// <param name="top">上邊框</param>
        /// <param name="right">右邊框</param>
        /// <param name="bottom">上邊框</param>
        public void SetBorder(BorderStyle left, BorderStyle top, BorderStyle right, BorderStyle bottom)
        {
            _borderLeft = left;
            _borderTop = top;
            _borderRight = right;
            _borderBottom = bottom;
            _styleValueIsChange = true;
        }
        #endregion

        /// <summary>
        /// 克隆一個新的單元格格式對象
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ExcelCellStyle style = new ExcelCellStyle();
            style.BorderBottom = this._borderBottom;
            style.BorderLeft = this._borderLeft;
            style.BorderRight = this._borderRight;
            style.BorderTop = this._borderTop;
            style.HorizontalAlignment = this._alignment;
            style.VerticalAlignment = this._verticalAlignment;
            style.DataFormart = this._dataFormart;
            style.FontBold = this.FontBold;
            style.FontName = this.FontName;
            style.FontSize = this.FontSize;
            style.WarpText = this.WarpText;
            style.FontName = this._fontName;
            style.UnderLine= this.UnderLine;
            style.FontIsItalic = this._isItalic;
            style.FontColor = this._fontColor;
            style.FontIsStrikeout = this._isStrikeout;
            style._fontTypeOffset = this._fontTypeOffset;
            return style;
        }
    }

    public enum UnderLine : byte
    {
        /// <summary>
        /// 沒有下劃線
        /// </summary>
        NONE =0,
        /// <summary>
        /// 單條下劃線
        /// </summary>
        SINGLE=1,
        /// <summary>
        /// 雙條下劃線
        /// </summary>
        DOUBLE=2,
        /// <summary>
        /// 會計單下劃線
        /// </summary>
        SINGLE_ACCOUNTING=33,
        /// <summary>
        /// 會計雙下劃線
        /// </summary>
        DOUBLE_ACCOUNTING=34
    }
}
