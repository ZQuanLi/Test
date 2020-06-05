using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Monitor
{
    public partial class MonitorBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.Exp.Monitor.MonitorDAL dal = null;
        public MonitorBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.Exp.Monitor.MonitorDAL(_project, _ledger, _uid);
        }
   
    }
}
