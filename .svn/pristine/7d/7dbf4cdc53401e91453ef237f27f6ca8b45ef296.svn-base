using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Alarm
{
    partial class AlarmBLL
    {

        public int GetYdAlarmOfChrgType()
        {
            return dal.GetYdAlarmOfChrgType();
        }

        public DataTable GetYdAlarmOfValList(string CoStrcName, string CoName)
        {
            decimal alarmVal = dal.GetYdAlarmOfVal();
            return dal.GetYdAlarmOfValList(CoStrcName, CoName, alarmVal);
        }

        /// <summary>
        /// 获取首页欠费户数
        /// </summary>
        /// <returns></returns>
        public int GetHomeUserAlarm()
        {
            return dal.GetHomeUserAlarm();
        }
    }
}
