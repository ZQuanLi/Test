using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Excel
{
    /// <summary>
    /// 表格列集合類
    /// </summary>
    public class ExcelRowCollection : ICollection, IEnumerable
    {
        ArrayList _list;

        /// <summary>
        /// 創建一個新的表格集合對象
        /// </summary>
        public ExcelRowCollection()
        {
            _list = new ArrayList();
        }

        /// <summary>
        /// 獲取一個表格列對象
        /// </summary>
        /// <param name="index">序號</param>
        /// <returns></returns>
        public ExcelRow this[int index]
        {
            get { return (ExcelRow)_list[index]; }
        }

        private void OnIndexChanged(int index)
        {
            for (int i = index; i < _list.Count; i++)
            {
                this[i].IndexInternal = i;
            }
        }
        /// <summary>
        /// 新增一個表格列對象
        /// </summary>
        /// <param name="row">表格列對象</param>
        public void Add(ExcelRow row)
        {
            foreach (ExcelCell cell in row.Cells)
            {
                cell.OwningRowInternal = row;
            }
            int index = _list.Count;
            _list.Add(row);
            OnIndexChanged(index);
        }

        /// <summary>
        /// 新增一組表格列對象
        /// </summary>
        /// <param name="rows">表格列集合</param>
        public void AddRange(ExcelRow[] rows)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                foreach (ExcelCell cell in rows[i].Cells)
                {
                    cell.OwningRowInternal = rows[i];
                }
            }
            int index = _list.Count;
            _list.AddRange(rows);
            OnIndexChanged(index);
        }

        public void Remove(ExcelRow row)
        {
            int index = _list.IndexOf(row);
            if (_list.Contains(row))
                _list.Remove(row);
            OnIndexChanged(index);
        }

        public bool Contains(ExcelRow row)
        {
            return _list.Contains(row);
        }

        public int IndexOf(ExcelRow row)
        {
            return _list.IndexOf(row);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

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
    }
}
