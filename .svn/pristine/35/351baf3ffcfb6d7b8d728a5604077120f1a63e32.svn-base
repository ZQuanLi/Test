using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.PDU.Opertion.Alarm
{
    public partial class AlarmHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.PDU.Alarm.AlarmBLL bll = null;
        public AlarmHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.PDU.Alarm.AlarmBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 获取告警信息
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="moduleName">设备名称</param>
        /// <returns></returns>
        public APIRst GetAlarmData(DateTime start, DateTime end, string moduleName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetAlarmData(start, end, moduleName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               PduName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               AlarmTime = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]),
                               Content = CommFunc.ConvertDBNullToString(s1["FunName"]) + "越限",
                               CollectValue = CommFunc.ConvertDBNullToString(s1["CollectValue"]),
                               IaMax = CommFunc.ConvertDBNullToString(s1["IaMax"]),
                               IaMin = CommFunc.ConvertDBNullToString(s1["IaMin"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}