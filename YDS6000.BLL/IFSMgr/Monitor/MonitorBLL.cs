using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.IFSMgr.Monitor
{
    public partial class MonitorBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.IFSMgr.Monitor.MonitorDAL dal = null;
        public MonitorBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.IFSMgr.Monitor.MonitorDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取采集点信息
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataTable GetTagInfo(string tags)
        {
            return dal.GetTagInfo(tags);
        }

        /// <summary>
        /// 更新数据值
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public int UpdateMapDataVal(int module_id, int fun_id, string dataValue)
        {
            return dal.UpdateMapDataVal(module_id,fun_id,dataValue);
        }

        public DataTable GetYdPayListResult(string trade_no)
        {
            return dal.GetYdPayListResult(trade_no);
        }
    }
}
