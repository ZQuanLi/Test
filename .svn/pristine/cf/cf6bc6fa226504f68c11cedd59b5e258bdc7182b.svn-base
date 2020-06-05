using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Excel
{
    public class XlsOparete
    {
        public XlsOparete()
        { 
        }
        private IWorkbook hssfworkbook;
        private ISheet sheet;
        private IRow row;

        public System.Data.DataTable XlsToDataTable(string fileName)
        {
            hssfworkbook = null;
            sheet = null;
            row = null;
            System.Data.DataTable dt = new System.Data.DataTable();
            using (System.IO.FileStream file = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                System.Data.DataRow dr;
                hssfworkbook = new HSSFWorkbook(file);
                //hssfworkbook = XSSFWorkbook(file);
                sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                while (rows.MoveNext())
                {
                    row = (HSSFRow)rows.Current;     //TODO::Create DataTable row
                    if (dt.Columns.Count == 0)
                    {
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            dt.Columns.Add(i.ToString().Trim(), typeof(string));
                        }
                    }
                    else
                    {
                        if (dt.Columns.Count < row.LastCellNum)
                        {
                            for (int i = dt.Columns.Count; i < row.LastCellNum; i++)
                            {
                                dt.Columns.Add(i.ToString().Trim(), typeof(string));
                            }
                        }
                    }
                    dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        ICell cell = row.GetCell(i);   //TODO::set cell value to the cell of DataTables    
                        if (cell != null)
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            } 
            return dt;
        }
    }
}
