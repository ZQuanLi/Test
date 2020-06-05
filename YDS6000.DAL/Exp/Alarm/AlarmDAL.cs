using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YDS6000.DAL.Exp.Alarm
{
    public partial class AlarmDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        public AlarmDAL(string project, int ledger, int uid)
        {
            this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
        }
    }
}
