using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;


namespace YDS6000.DAL.Exp.Monitor
{
    public partial class MonitorDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";

        public MonitorDAL(string project, int ledger, int uid)
        {
            this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
        }

    }
}
