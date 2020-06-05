using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;
using PushMessage;

namespace DataProcess.Yd.Alarm
{
    internal class Collect
    {
        private static ConcurrentDictionary<long, object> m2mqtt = new ConcurrentDictionary<long, object>();
        //private static SendMessageApi send = new SendMessageApi();//uniPush初始化参数
        public void Run()
        {
            while (true)
            {
                Thread.Sleep(100);/*每100毫秒*/
                v2_alarm_logVModel cc = null;
                if (NCSys.AlarmCollect.TryDequeue(out cc) == true)
                {
                    try
                    {
                        DataProcessBLL bll = new DataProcessBLL(cc.Ledger, Config.Uid);
                        long log_id = bll.AddAlarm(cc);
                        if (cc.IsMqtt == true)
                        {
                            object dy = new { cc = cc, time = DateTime.Now, count = 0 };
                            m2mqtt.TryAdd(log_id, dy);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileLog.Error("内容:" + JsonHelper.Serialize(cc) + "记录告警错误:" + ex.Message + ex.StackTrace);
                    }
                }

                try
                {/*告警信息半个钟推一次，一共需要6次，现场确认下来了*/   /*2020.01.06 最新修改设置 告警信息一分钟推一次，一共需要10次*/
                    bool bIsNext = true;
                    var enumer = m2mqtt.GetEnumerator();
                    while (bIsNext == true)
                    {
                        Thread.Sleep(1000);/*每100毫秒*/
                        bIsNext = enumer.MoveNext();
                        if (bIsNext == false) break;
                        var s0 = enumer.Current;
                        long key = s0.Key;
                        object obj = s0.Value;

                        v2_alarm_logVModel am = CommFunc.GetPropertyValue("cc", obj) as v2_alarm_logVModel;
                        DateTime dd = CommFunc.ConvertDBNullToDateTime(CommFunc.GetPropertyValue("time", obj));
                        int count = CommFunc.ConvertDBNullToInt32(CommFunc.GetPropertyValue("count", obj));
                        bool isUp = false;
                        int Log_id = 0;
                        int Co_id = 0;
                        if (count == 0 || DateTime.Now >= dd.AddMinutes(1)) //一分钟推一次
                        {
                            //权限过滤
                            DataProcessBLL bll = new DataProcessBLL(am.Ledger, Config.Uid);
                            //string AreaPowerStr = "";
                            //bool IsCheckAreaPower = bll.GetAlarmFisrt(am.Ledger, Config.Uid, out AreaPowerStr);
                            //bool qx = false;
                            //string[] sArray = AreaPowerStr.Split(',');
                            var datas = bll.Getv2_alarm_log(CommFunc.ConvertDBNullToInt32(key)); //历史表
                            if (datas.Rows.Count > 0)
                            {
                                Log_id = CommFunc.ConvertDBNullToInt32(datas.Rows[0]["Log_id"].ToString());
                                Co_id = CommFunc.ConvertDBNullToInt32(datas.Rows[0]["Co_id"].ToString());
                                //var Content = datas.Rows[0]["Content"].ToString();
                                //foreach (string i in sArray)
                                //{
                                //    if (Co_id == CommFunc.ConvertDBNullToInt32(i))
                                //    {
                                //        qx = true;
                                //        continue;
                                //    }
                                //}
                            }
                            //if (qx)
                            //{
                                var content = am.Content + "," + Co_id;
                                DataProcess.Business.m2mqtt.Publish(content);
                                //send.send(content,"CID");//uniPush推送
                                count = count + 1;
                                isUp = true;
                                FileLog.WriteLog("Log_id:" + key + " 推送次数:" + count);
                            //}

                            //FileLog.WriteLog("+++++++ Ledger;" + am.Ledger + " Uid;" + Config.Uid + " Log_id:" + key + " 库Log_id:" + am.Log_id + " 库Co_id:" + Co_id + " 权限ID:" + AreaPowerStr + " +++++++");

                        }
                        if (count == 10)
                        {//一共需要10次
                            m2mqtt.TryRemove(key, out obj);
                            FileLog.WriteLog("Log_id:" + key + " 推送:" + count + "移除");
                        }
                        else
                        {
                            if (isUp == true)
                            {
                                object newObj = new { cc = am, time = DateTime.Now, count = count };
                                if (m2mqtt.TryUpdate(key, newObj, obj))
                                    FileLog.WriteLog("Log_id:" + key + " 推送:" + count + " 更新成功！");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //m2mqtt.Clear(); //移除所有的键和值.
                    FileLog.Error("推送mqtt记录内容错误:" + ex.Message + ex.StackTrace);
                }
            }
        }
    }
}
