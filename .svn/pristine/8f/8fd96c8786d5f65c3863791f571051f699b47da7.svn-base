using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;

namespace DataProcess
{
    internal class Alarm
    {
        public void Run()
        {
            DateTime dtAlarm = DateTime.Now.AddDays(-9999); 
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    int scan = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "Scan"));
                    scan = scan == 0 ? (60 * 24) : scan;
                    //
                    //if (SmsTime == null || SmsTime.Value.AddMinutes(15) > DateTime.Now) SmsFun.StopSerice();/*停止端口，让出端口给其他应用使用*/
                    //
                    if (DateTime.Now.Minute != 0 && DateTime.Now.Minute % 15 != 0 && DateTime.Now.Minute % 30 != 0) continue;
                    if (dtAlarm.AddMinutes(scan) > DateTime.Now) continue;

                    int hour = DateTime.Now.Hour;
                    int hh = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "Hour"));
                    int isAlarm = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "IsAlarm"));

                    if (isAlarm == 0) continue;/*总开关*/
                    if (hour < hh) continue;
                    foreach (var s1 in NCSys.Pro)
                    {
                        //new ZpAlarm(s1.Key, s1.Value).GetAlarm();
                    }
                    dtAlarm = DateTime.Now;
                }
                catch (Exception ex)
                {
                    FileLog.Error("告警处理模块错误:" + ex.Message + ex.StackTrace);
                }
            }
        }

    }
}
