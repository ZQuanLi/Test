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
    public class AlarmBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly AlarmDAL dal = null;
        public AlarmBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new AlarmDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取告警设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmConfig()
        {
            return dal.GetAlarmConfig();
        }

        /// <summary>
        /// 获取告警数据
        /// </summary>
        /// <param name="alarmVal"></param>
        /// <returns></returns>
        public DataTable GetAlarmData(decimal alarmVal, string unit)
        {
            return dal.GetAlarmData(alarmVal, unit);
        }

        /// <summary>
        /// 获取告警信息
        /// </summary>
        /// <param name="co_id"></param>
        /// <param name="module_id"></param>
        /// <param name="moduleAddr"></param>
        /// <param name="aType"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public v2_alarm_logVModel GetAlarmModel(int co_id, int module_id, string moduleAddr, string aType, decimal rule)
        {
            return dal.GetAlarmModel(co_id, module_id, moduleAddr, aType, rule);
        }

        public DataTable GetAlarm_log_hand(long log_id, string hdType)
        {
            return dal.GetAlarm_log_hand(log_id, hdType);
        }
        public DataTable GetCrmOfCo(int co_id)
        {
            return dal.GetCrmOfCo(co_id);
        }
        public DataTable GetOpenidList(int co_id, long log_id, string hdType)
        {
            return dal.GetOpenidList(co_id, log_id, hdType);
        }

        //public DataTable GetOpenidList(int co_id)
        //{
        //    return dal.GetOpenidList(co_id);
        //}

        public int AddAlarm(v2_alarm_logVModel am)
        {
            return dal.AddAlarm(am);
        }

        /// <summary>
        /// 拉闸数据
        /// </summary>
        /// <returns></returns>
        public CommandVModel GetSsr1(int module_id)
        {
            return dal.GetSsr1(module_id);
        }

        public string GetSSrIOService(int module_id, string funType, out string dataValue)
        {
            return dal.GetSSrIOService(module_id, funType, out dataValue);
        }

        public int AddAV2_alarm_log_hand(long log_id, int aid, string hdType, string hdKey, string content, int errCode, string errTxt)
        {
            return dal.AddAV2_alarm_log_hand(log_id, aid, hdType, hdKey, content, errCode, errTxt);
        }

    }
}
