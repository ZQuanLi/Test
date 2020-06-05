using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.PDU.Alarm
{
    public partial class AlarmBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.PDU.Alarm.AlarmDAL dal = null;
        public AlarmBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.PDU.Alarm.AlarmDAL(_ledger, _uid);
        }

        public DataTable GetAlarmData(DateTime start, DateTime end, string moduleName)
        {
            return dal.GetAlarmData(start, end, moduleName);
        }
    }
}
