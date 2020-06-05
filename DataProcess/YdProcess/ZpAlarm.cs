using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;

namespace DataProcess.YdProcess
{
    internal class ZpAlarm
    {

        /// <summary>
        /// 更新下发命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="lpszVal"></param>
        /// <param name="lpszdateTime"></param>
        /// <param name="errCode"></param>
        /// <param name="errTxt"></param>
        public static void DbCmdRefresh(CommandVModel command, string lpszVal, DateTime lpszdateTime, AlarmType errCode, string errTxt)
        {
            if (command == null || lpszdateTime.Year < 2000) return;
            if (command.Module_id == 0) return;
            DataProcessBLL bll = new DataProcessBLL(command.Ledger, Config.Uid);
            if (command.Log_id != 0)
            {
                //FileLog.WriteLog(command.LpszDbVarName + " 后台Log："+ command.Log_id  + JsonHelper.Serialize(command));
                bll.UpdataCmd(command, (int)errCode, errTxt);
            }
            else
            {
                if (command.IsNDb == true)
                { /*更新记录*/
                    //FileLog.WriteLog(command.LpszDbVarName + " 后台标示IsNDb：true :" + JsonHelper.Serialize(command));
                    bll.AddCmdLog(command, (int)errCode, errTxt);
                }
            }
        }

        public static void DbEventWpf(CommandVModel command, string lpszVal, DateTime lpszdateTime, AlarmType errCode, string errTxt)
        {
            if (lpszVal == null || lpszdateTime.Year < 2000) return;
            if (errCode != AlarmType.Sue) return;

            DataProcessBLL bll = new DataProcessBLL(command.Ledger, Config.Uid);
            SysPro pro;
            NCSys.Pro.TryGetValue(command.Ledger, out pro);
            if (pro == null)
            {
                FileLog.Error("CollectAlarm.DbEventWpf 模块没有项目:" + command.Ledger);
                return;
            }
            string mkey = pro.ProjectKey + command.LpszDbVarName;
            CollectVModel collect = null;
            if (NCSys.Result.TryGetValue(mkey, out collect) == false)
                return;

            RstVar rst = collect.RstVar;// MemcachedMgr.GetVal<RstVar>(mkey);
            bool isAdd = true;
            if (rst != null && !string.IsNullOrEmpty(rst.lpszVal))
            {
                MalignantVModel pp = JsonHelper.Deserialize<MalignantVModel>(rst.lpszVal);
                if (collect.IsDb == false)
                {
                    if (pp.CTime == lpszdateTime) isAdd = false;
                }
                collect.IsDb = false;
            }
            if (isAdd == true)
            { /*增加到数据库中*/
                bll.AddAlarmOnCollect(command.Co_id, command.Module_id, command.ModuleAddr, command.FunType, command.Fun_id, lpszVal, (int)errCode, errTxt, lpszdateTime);
            }
        }

        /// <summary>
        /// 增加告警到数据库
        /// </summary>
        /// <param name="lpszDbVarName"></param>
        /// <param name="lpszVal"></param>
        /// <param name="lpszdateTime"></param>
        /// <param name="errCode"></param>
        /// <param name="errTxt"></param>
        public static void DbAddAlarm(CommandVModel command, string lpszVal, DateTime lpszdateTime, AlarmType errCode, string errTxt, bool isSend)
        {
            DataProcessBLL bll = new DataProcessBLL(command.Ledger, Config.Uid);
            StringBuilder strContent = new StringBuilder();
            DataTable dtSource = bll.GetMapInfo(command.LpszDbVarName);
            int ccc = dtSource.Rows.Count;
            if (ccc == 0)
            {
                FileLog.WriteLog("告警模块变量:" + command.LpszDbVarName + "不存在映射表中");
                return;
            }
            if (ccc > 1)
            {
                FileLog.WriteLog("告警模块变量:" + command.LpszDbVarName + "在映射表中存在多个");
                return;
            }

            int module_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Module_id"]);
            int co_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Co_id"]);
            int fun_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Fun_id"]);
            string moduleAddr = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["ModuleAddr"]);
            string funType = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["FunType"]);

            if (funType.Equals(V0Fun.LeakAlarm.ToString()))
            {
                if (lpszVal.Substring(0, 12).Contains("1"))
                {
                    int cnt = 0;
                    foreach (char c in lpszVal.Substring(0, 12).ToCharArray())
                    {
                        string content = "";
                        ++cnt;
                        if (c.ToString().Equals("1"))
                        {
                            if (cnt <= 8)
                                content = "漏电流" + cnt + "发生告警";
                            else
                                content = "温度" + (cnt - 8) + "发生告警";
                        }
                        if (!string.IsNullOrEmpty(content))
                        {
                            if (!string.IsNullOrEmpty(strContent.ToString()))
                                strContent.Append(";");
                            strContent.Append(content);
                        }
                    }
                }
            }
            else
            {
                string content = "";
                System.Reflection.FieldInfo info = typeof(V0Fun).GetField(funType);
                if (info != null)
                {
                    var obj = info.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (obj != null)
                    {
                        foreach (DisplayAttribute md in obj)
                            content = md.Name;
                    }
                }
                strContent.Append(content);
            }
            long log_id = bll.AddAlarm(co_id, module_id, moduleAddr, errCode.ToString(), fun_id, strContent.ToString(), lpszVal, lpszdateTime, (int)errCode, errTxt);
            if (isSend == true)
            {/*发送告警*/
                SendHd(command.Ledger, log_id, errCode, HdType.AL_Sms, errTxt, module_id);
                SendHd(command.Ledger, log_id, errCode, HdType.AL_Email, errTxt, module_id);
            }
        }

        private static void SendHd(int ledger,long log_id, AlarmType alarmType, HdType hdType, string content, int module_id)
        {
            SysPro pro;
            NCSys.Pro.TryGetValue(ledger, out pro);
            if (pro == null) return;
            DataProcessBLL bll = new DataProcessBLL(ledger, Config.Uid);
            v2_alarm_logHandVModel hand = bll.GetAlarmHand(log_id, hdType.ToString());
            hand.Content = content;

            if (hand.ErrCode == 1)
            {
                int sendSpan = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "SendSpan"));

                if (hand.HdTime.AddMinutes(sendSpan) > DateTime.Now)
                    return;
                else
                    hand.Aid = 0;/*新增一条记录*/
            }
            DataTable dtSource = bll.GetSendInfo(module_id);
            List<string> mpReceipts = new List<string>();
            List<string> emReceipts = new List<string>();

            string coName = "", moduleName = "";
            foreach (DataRow drCrm in dtSource.Rows)
            {
                if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["MPhone"])))
                    mpReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["MPhone"]));
                if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["Email"])))
                    emReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["Email"]));
                coName = CommFunc.ConvertDBNullToString(drCrm["CoName"]);
                moduleName = CommFunc.ConvertDBNullToString(drCrm["ModuleName"]);
            }
            bool sue = false;
            string msg = "";
            try
            {
                if (hdType == HdType.AL_Sms)
                {
                    if (mpReceipts.Count > 0)
                    {
                        if (alarmType == AlarmType.Al_Use)
                        {/*感知万物演示用*/
                            string ccc = JsonHelper.Serialize(new { moduleName = moduleName });
                            if (!string.IsNullOrEmpty(pro.Sms.ComSmsConfig.ComSms))
                                sue = pro.Sms.SendMessage(mpReceipts, ccc, out msg);
                            else if (!string.IsNullOrEmpty(SmsFun.AliSmsConfig.AliAppKey))
                                sue = pro.Sms.SmsMessage(mpReceipts, ccc, out msg, "SMS_137657141");
                            else
                                msg = "没有发现短信接口";
                        }
                        else
                        {
                            msg = "感知演示,暂不开放";
                        }
                    }
                    else
                    {
                        msg = "无手机号码";
                    }
                }
                else if (hdType == HdType.AL_Email)
                {
                    if (mpReceipts.Count > 0)
                    {
                        StringBuilder mailcont1 = new StringBuilder();
                        mailcont1.Append("你好:" + coName);
                        mailcont1.Append("<br>");
                        mailcont1.Append("告警内容:" + hand.Content);
                        mailcont1.Append("<br>");
                        mailcont1.Append("告警时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                        pro.Email.SendMail("告警通知", mailcont1.ToString(), "", emReceipts, null, null);
                        sue = true;
                    }
                    else
                    {
                        msg = "无邮箱地址";
                    }
                }
                hand.ErrCode = sue == true ? 1 : -1;
            }
            catch (Exception ex)
            {
                hand.ErrCode = -1;
                msg = ex.Message;
            }
            hand.ErrTxt = msg;

            bll.UpAlarmHand(hand);
        }
    }
}
