using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.BLL.Energy.Report
{
    public partial class ReportBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Energy.Report.ReportDAL dal = null;
        public ReportBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Energy.Report.ReportDAL(_ledger, _uid);
        }
    }
}
