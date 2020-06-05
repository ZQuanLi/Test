using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.RunReport
{
    public partial class ExpYdSysLogBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.RunReport.ExpYdSysLogDAL dal = null;
        public ExpYdSysLogBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.RunReport.ExpYdSysLogDAL(_ledger, _uid);
        }

        public DataTable GetYdSysLogOfList(string UName, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdSysLogOfCmd(UName, Start, End);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
            }
            return dtSource;
        }

        public DataTable GetYdSysLogCtrl(string UName, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdSysLogCtrl(UName, Start, End);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
            }
            return dtSource;
        }
    }
}
