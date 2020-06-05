using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace YDS6000.BLL.Exp.Home
{
    public partial class HomeBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private YDS6000.DAL.Exp.Home.HomeDAL dal = null;
        public HomeBLL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new YDS6000.DAL.Exp.Home.HomeDAL(ledger, uid);
        }

        /// <summary>
        /// 获取设备数量
        /// </summary>
        /// <returns></returns>
        public DataTable GetEquCount()
        {
            return dal.GetEquCount();
        }

        public DataTable GetEquAlarm()
        {
            return dal.GetEquAlarm();
        }
        /// <summary>
        /// 获取最新一条告警
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmFisrt()
        {
            return dal.GetAlarmFisrt();
        }
        /// <summary>
        /// 项目导航
        /// </summary>
        /// <returns></returns>
        public DataTable GetProjectList()
        {
            return dal.GetProjectList();
        }

        public DataTable GetAlarmCompare()
        {
            return dal.GetAlarmCompare();
        }
    }
}
