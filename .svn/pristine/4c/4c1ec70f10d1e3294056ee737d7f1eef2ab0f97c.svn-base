using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Energy.Monitor
{
    public partial class MonitorBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Energy.Monitor.MonitorDAL dal = null;
        public MonitorBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Energy.Monitor.MonitorDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取最新采集列表数据
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetModuleOfMapCollect(int meter_id)
        {
            return dal.GetModuleOfMapCollect(meter_id);
        }

        /// <summary>
        /// 保存更换电表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="fun_id">采集ID号</param>
        /// <param name="lastVal">最后值</param>
        /// <returns></returns>
        public int UpdateLastVal(int module_id, int fun_id, decimal lastVal)
        {
            return dal.UpdateLastVal(module_id, fun_id, lastVal);
        }
    }
}
