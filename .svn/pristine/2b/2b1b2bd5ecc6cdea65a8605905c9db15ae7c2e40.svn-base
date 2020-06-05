using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YDS6000.DAL.Exp.Alarm;

namespace YDS6000.BLL.Exp.Alarm
{
    public partial class AlarmBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private AlarmDAL dal = null;
        public AlarmBLL(string project, int ledger, int uid)
        {
            this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new AlarmDAL(project, ledger, uid);
        }
    }
}
