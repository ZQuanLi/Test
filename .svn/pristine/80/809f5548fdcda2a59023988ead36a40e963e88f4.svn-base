using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.DataProcess;

namespace YDS6000.BLL.DataProcess
{
    public class ZpSiBLL
    {
        private string Ledgers = "";
        private int SysUid = 0;
        private readonly ZpSiDAL dal = null;
        public ZpSiBLL(string _ledgers, int _uid)
        {
            this.Ledgers = _ledgers;
            this.SysUid = _uid;
            dal = new ZpSiDAL(_ledgers,_uid);
        }
        /// <summary>
        /// 定时拉合闸数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetV1_si_ssr()
        {
            return dal.GetV1_si_ssr();
        }

        /// <summary>
        /// 拉闸数据
        /// </summary>
        /// <returns></returns>
        public CommandVModel GetSsrInfo(int ledger, int module_id,string funType, string dataValue)
        {
            return dal.GetSsrInfo(ledger, module_id, funType, dataValue);
        }

        public int GetTransferType(int ledger, int module_id)
        {
            return dal.GetTransferType(ledger, module_id);
        }

        public DataTable GetSsr1(int ledger, int meter_id, string meterAddr)
        {
            return dal.GetSsr1(ledger, meter_id, meterAddr);
        }

        public DataTable GetSsr0(int ledger, int meter_id)
        {
            return dal.GetSsr0(ledger, meter_id);
        }

        public bool GetMeterQianFei(int ledger, int co_id, int meter_id, string meterAddr)
        {
            return dal.GetMeterQianFei(ledger, co_id, meter_id, meterAddr);
        }

        public bool GetModuleQianFei(int ledger, int co_id, int module_id, string moduleAddr)
        {
            return dal.GetModuleQianFei(ledger, co_id, module_id, moduleAddr);
        }

        public string GetSSrIOService(int ledger,int module_id, string funType, out string dataValue)
        {
            return dal.GetSSrIOService(ledger, module_id, funType, out dataValue);
        }
    }
}
