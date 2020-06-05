using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Excel
{
    /// <summary>
    /// 單元格集合類
    /// </summary>
    public class ExcelCellCollection : ICollection, IEnumerable
    {
        ArrayList _list;

        /// <summary>
        /// 創建一個單元格集合對象
        /// </summary>
        public ExcelCellCollection()
        {
            _list = new ArrayList();
        }

        //public ExcelCell[] ToArray()
        //{
        //    return (ExcelCell[])_list.ToArray(typeof(ExcelCell));
        //}

        /// <summary>
        /// 獲取單元格對象
        /// </summary>
        /// <param name="index">序號</param>
        /// <returns>返回單元格對象</returns>
        public ExcelCell this[int index]
        {
            get { return (ExcelCell)_list[index]; }
        }

        /// <summary>
        /// 獲取單元格對象
        /// </summary>
        /// <param name="DataPropertyName">數據的屬性名稱</param>
        /// <returns></returns>
        public ExcelCell this[string DataPropertyName]
        {
            get
            {
                foreach (ExcelCell cell in _list)
                {
                    if (cell.OwningColumn.DataPropertyName.Equals(DataPropertyName))
                    {
                        return cell;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 新增一個單元格對象
        /// </summary>
        /// <param name="cell">單元格對象</param>
        public void Add(ExcelCell cell)
        {
            _list.Add(cell);
        }

        /// <summary>
        /// 新增一組單元格對象
        /// </summary>
        /// <param name="cells">單元格集合</param>
        public void AddRange(ExcelCell[] cells)
        {
            _list.AddRange(cells);
        }

        /// <summary>
        /// 移除一個單元格對象
        /// </summary>
        /// <param name="cell">單元格的對象</param>
        public void Remove(ExcelCell cell)
        {
            if (_list.Contains(cell))
                _list.Remove(cell);
        }

        /// <summary>
        /// 移除一個單元格對象
        /// </summary>
        /// <param name="index">序號</param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// 移除所有單元格對象
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        /// <summary>
        /// 包含的單元格數量
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
