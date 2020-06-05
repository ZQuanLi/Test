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
    internal class ZpAlarm
    {
        private AlarmBLL bll = null;
        private SysPro pro = null;
        internal ZpAlarm(int ledger, SysPro pro)
        {
            this.bll = new AlarmBLL(ledger, Config.Uid);
            this.pro = pro;
        }

        internal bool GetAlarm()
        {
            DataTable dtSource = null;
            DataTable dtAlarmConfig = null;
            try
            {
                dtAlarmConfig = bll.GetAlarmConfig();
                int cnt = dtAlarmConfig.Rows.Count;
                decimal alarmVal1 = cnt == 0 ? 0 : CommFunc.ConvertDBNullToDecimal(dtAlarmConfig.Rows[0]["CfValue"]);
                string unit = cnt == 0 ? "" : CommFunc.ConvertDBNullToString(dtAlarmConfig.Rows[0]["Unit"]);
                dtSource = bll.GetAlarmData(alarmVal1, unit);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("获取告警数据错误:" + ex.Message + ex.StackTrace);
            }
            //
            if (dtSource == null) return false;
            Thread.Sleep(100);
            int co_idPrev = -9999;
            foreach (DataRow dr in dtSource.Rows)
            {
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                string coName = CommFunc.ConvertDBNullToString(dr["CoName"]);
                int chrgType = CommFunc.ConvertDBNullToInt32(dr["ChrgType"]);/*<=1 房间 >1电表*/
                int isAlarm = CommFunc.ConvertDBNullToInt32(dr["IsAlarm"]);/*<=1 房间 >1电表*/
                DateTime lastTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                int transferType = CommFunc.ConvertDBNullToInt32(dr["TransferType"]);
                decimal rdVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
                decimal price = CommFunc.ConvertDBNullToDecimal(dr["Price"]);
                decimal rdAmt = CommFunc.ConvertDBNullToDecimal(dr["RdAmt"]);
                int module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                string moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                string moduleName = CommFunc.ConvertDBNullToString(dr["ModuleName"]);
                string cfKey = "", cfUnit = "";
                decimal cfValue = 0;
                decimal cfRule = 1;
                bool isRun = true;
                if (chrgType <= 1 && co_idPrev == co_id)
                    isRun = false;
                co_idPrev = co_id;
                if (isRun == false) continue;
                int ccc = dtAlarmConfig.Rows.Count;
                int crn = 0;
                foreach (DataRow drC in dtAlarmConfig.Rows)
                {/*从大到小排列*/
                    cfKey = CommFunc.ConvertDBNullToString(drC["CfKey"]);
                    cfValue = CommFunc.ConvertDBNullToDecimal(drC["CfValue"]);
                    cfRule = CommFunc.ConvertDBNullToDecimal(drC["Rule"]);
                    cfUnit = CommFunc.ConvertDBNullToString(drC["Unit"]).ToLower();
                    if (pro.Project.Equals("YdXip") && cfKey.Equals("AlarmValOd"))
                    {/*西安培华的拉闸电量*/
                        cfValue = CommFunc.ConvertDBNullToDecimal(dr["OdValue"]);
                    }
                    //if (!cfKey.ToLower().Equals("AlarmValOd".ToLower()))
                    //    cfRule = cfRule == 0 ? 1 : cfRule;
                    decimal comVal = string.IsNullOrEmpty(cfUnit) || cfUnit.Equals("kWh".ToLower()) ? rdVal : rdAmt;
                    decimal toValue = cfValue;
                    if (crn < ccc - 1)
                        toValue = CommFunc.ConvertDBNullToDecimal(dtAlarmConfig.Rows[crn + 1]["CfValue"]);
                    if (cfValue >= comVal && comVal > toValue) break;
                    crn = crn + 1;
                }
                //FileLog.WriteLog("电表ID:" + module_id + "地址:" + moduleAddr + "房间:" + coName + "剩余电量:" + rdVal + "告警类型:" + cfKey + "规则:" + cfRule + "告警值:" + cfValue);
                bool sendAlarm = true;
                if (cfKey.ToLower().Equals("AlarmValOd".ToLower()) && isAlarm==1)
                { /*拉合闸*/
                    if (CommFunc.ConvertDBNullToInt32(cfRule) == 1)
                    { /*发送拉合闸命令*/
                        sendAlarm = false;
                        try
                        {
                            string key = pro.ProjectKey + CommFunc.ConvertDBNullToString(dr["LpszSsr"]);
                            RstVar rstVar = MemcachedMgr.GetVal<RstVar>(key);
                            bool isPass = false;
                            if (rstVar != null && CommFunc.ConvertDBNullToDecimal(rstVar.lpszVal) == 0)
                                isPass = true;

                            if (isPass == true)
                                this.CtorlSsr1(module_id);
                        }
                        catch (Exception ex)
                        {
                            FileLog.Error("透支电量发送拉合闸命令错误:" + ex.Message + ex.StackTrace);
                        }
                    }
                }
                if (sendAlarm == true)
                {/*发送告警*/
                    cfRule = cfRule == 0 ? 1 : cfRule;
                    v2_alarm_logVModel alarmInfo = this.AlarmInfo(dr, cfValue, cfRule);
                    if (isAlarm == 1)
                    {
                        if (!string.IsNullOrEmpty(EmailUtilities.EmConfig.MailFrom) && !string.IsNullOrEmpty(EmailUtilities.EmConfig.MailSmtpHost) &&
                            !string.IsNullOrEmpty(EmailUtilities.EmConfig.MailSmtpUser) && !string.IsNullOrEmpty(EmailUtilities.EmConfig.MailSmtpPassword)
                            )
                        {/*邮件告警*/
                            this.AlarmEmail(alarmInfo.Log_id, co_id, coName, lastTime, rdVal);
                        }
                        if (!string.IsNullOrEmpty(pro.Sms.ComSmsConfig.ComSms) ||
                            (!string.IsNullOrEmpty(SmsFun.AliSmsConfig.AliAppKey) && !string.IsNullOrEmpty(SmsFun.AliSmsConfig.AliAppSecret) && !string.IsNullOrEmpty(SmsFun.AliSmsConfig.AliSignName))
                            )
                        {/*短信告警*/
                            this.AlarmSms(alarmInfo.Log_id, co_id, moduleName, coName, lastTime, rdVal, rdAmt);
                        }
                        if (!string.IsNullOrEmpty(pro.Wx.WxConfig.WxAPPID))
                        {/*微信告警*/
                         //this.AlarmWx(alarmInfo.Log_id, co_id, coName, lastTime, rdVal);
                        }
                    }
                }
            }
            return true;
        }

        private void CtorlSsr1(int module_id)
        {/*先下发命令*/
         //YDS6000.BLL.CmdService.ProcessBLL gwBll = new YDS6000.BLL.CmdService.ProcessBLL(Config.Ledger, Config.Uid);
         //int fun_id = gwBll.GetYdToFun_id(module_id, V0Fun.Ssr);
         //long log_id = gwBll.YdToGwOfAddCmd(module_id, fun_id, V0Fun.Ssr.ToString(), "1");
         //CommandVModel cmd = gwBll.GetYdToGwOfSendCmd(log_id);
            CommandVModel cmd = bll.GetSsr1(module_id);  //ModelHandler<CommandVModel>.FillModel(dtSsr.Rows[0]);
            if (cmd == null)
            {
                FileLog.Error("CtorlSsr1没有数据");
                return;
            }
            cmd.DataValue = "1";
            cmd.IsNDb = true;
            cmd.Create_by = Config.Uid;
            cmd.IsRn = 1;
            if (cmd.TransferType == 4)
            {/*IOService通信*/
                string fs = cmd.FunType + cmd.DataValue;
                string tagName = "", dataValue = "";
                tagName = bll.GetSSrIOService(module_id, fs, out dataValue);
                if (!string.IsNullOrEmpty(tagName) && !string.IsNullOrEmpty(dataValue))
                {
                    cmd.LpszDbVarName = tagName;
                    cmd.DataValue = dataValue;
                }
                DataProcess.IOService.Collection.CollectionHelper.WriteVar(cmd);
                //DataProcess.IOService.Collection.CollectionHelper.WriteVar(cmd.Ledger, cmd.HandledBY, cmd.LpszDbVarName, cmd.DataValue);
            }
            else
            {
                DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
            }
            //DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
        }

        private v2_alarm_logVModel AlarmInfo(DataRow dr, decimal cfValue, decimal cfRule)
        {
            int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
            string coName = CommFunc.ConvertDBNullToString(dr["CoName"]);
            int chrgType = CommFunc.ConvertDBNullToInt32(dr["ChrgType"]);
            DateTime lastTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
            decimal rdVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
            decimal price = CommFunc.ConvertDBNullToDecimal(dr["Price"]);
            decimal rdAmt = Math.Round(rdVal * price, 2, MidpointRounding.AwayFromZero);
            int module_id = 0;
            string moduleAddr = "";
            string moduleName = CommFunc.ConvertDBNullToString(dr["ModuleName"]);
            if (chrgType >= 2)
            {
                module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
            }
            string ss = "你好当前设备：" + moduleName + "余额：" + rdAmt.ToString("f2");
            v2_alarm_logVModel ems = bll.GetAlarmModel(co_id, module_id, moduleAddr, AlarmType.Alarm.ToString(), cfRule);
            if (ems == null)
            {
                ems = new v2_alarm_logVModel();
                ems.Log_id = 0;
                ems.Co_id = co_id;
                ems.Module_id = module_id;
                ems.ModuleAddr = moduleAddr;
                ems.AType = AlarmType.Alarm.ToString();
            }
            ems.Content = ss;
            ems.ErrTxt = "";
            if (ems.Log_id == 0)
                bll.AddAlarm(ems);//增加log
            return ems;
        }

        private void AlarmEmail(long log_id,int co_id,string coName,DateTime lastTime,decimal rdVal)
        {
            List<string> listReceipts = new List<string>();
            List<string> mphone = new List<string>();
            string crmName = "";
            int aid = 0, errCode = 0;
            string errTxt = "";
            try
            {
                DataTable dtSource = bll.GetAlarm_log_hand(log_id, HdType.AL_Email.ToString());
                if (dtSource.Rows.Count > 0)
                {
                    aid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Aid"]);
                    errCode = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["ErrCode"]);
                }
                if (errCode == 1) return;
                DataTable dtCrm = bll.GetCrmOfCo(co_id);
                foreach (DataRow drCrm in dtCrm.Rows)
                {
                    if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["MPhone"])))
                        mphone.Add(CommFunc.ConvertDBNullToString(drCrm["MPhone"]));
                    if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["Email"])))
                        listReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["Email"]));
                    crmName = CommFunc.ConvertDBNullToString(drCrm["CrmName"]);
                }
            }
            catch (Exception ex)
            {
                FileLog.Error("获取告警数据错误:" + ex.Message + ex.StackTrace);
                errCode = -1;
                errTxt = "获取告警用户邮件手机号码错误";
            }
            string msg = "";
            bool sue = false;
            if (listReceipts.Count != 0)
                sue = this.SendEmail(listReceipts, crmName, coName, lastTime, rdVal, out msg);
            else
                msg = "无邮件地址";
            errTxt = msg;
            errCode = (sue == true ? 1 : -1);
            bll.AddAV2_alarm_log_hand(log_id, aid, HdType.AL_Email.ToString(), "", "", errCode, errTxt);
        }

        private void AlarmSms(long log_id, int co_id,string coName, string moduleName, DateTime lastTime, decimal rdVal, decimal rdAmt)
        {
            List<string> listReceipts = new List<string>();
            string crmName = "", content = "";
            int aid = 0, errCode = 0;
            string errTxt = "";
            string msg = "";
            try
            {
                DataTable dtSource = bll.GetAlarm_log_hand(log_id, HdType.AL_Sms.ToString());
                if (dtSource.Rows.Count > 0)
                {
                    aid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Aid"]);
                    errCode = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["ErrCode"]);
                }
                if (errCode == 1) return;

                DataTable dtCrm = bll.GetCrmOfCo(co_id);
                foreach (DataRow drCrm in dtCrm.Rows)
                {
                    if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["MPhone"])))
                    {
                        listReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["MPhone"]));
                    }
                    //if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["MPhone"])))
                    //    mphone.Add(CommFunc.ConvertDBNullToString(drCrm["MPhone"]));
                    //if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["Email"])))
                    //    listReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["Email"]));
                    crmName = CommFunc.ConvertDBNullToString(drCrm["CrmName"]);
                }
                //object obj = new
                //{
                //    CrmName = crmName,
                //    DateTime = lastTime.ToString("yyyy-MM-dd"),
                //    CoName = coName,
                //    SyAmt = rdAmt.ToString("f2"),
                //    SyVal = rdVal.ToString("f2") + "kWh"
                //};
                object obj = new
                {
                    moduleName = moduleName,
                    rdAmt = rdAmt.ToString("f2") + "元",
                };
                content = JsonHelper.Serialize(obj);
            }
            catch (Exception ex)
            {
                FileLog.Error("获取告警数据错误:" + ex.Message + ex.StackTrace);
                errTxt = "获取告警用户邮件手机号码错误";
            }
            ////////////////
            bool sue = false;
            if (listReceipts.Count != 0)
            {
                //【电费通知】尊敬的【房间名】用户，您好，您已欠费【具体金额】，请及时缴费。
                if (!string.IsNullOrEmpty(pro.Sms.ComSmsConfig.ComSms))
                {
                    if (rdAmt < 0)
                        content = "【电费通知】尊敬的【" + coName + "】用户，您好，您已欠费【" + Math.Abs(rdAmt).ToString("f2") + "元】，请及时缴费。";
                    else
                        content = "【电费通知】尊敬的【" + coName + "】用户，您好，您已剩余【" + Math.Abs(rdAmt).ToString("f2") + "元】，请及时缴费。";
                    //content = "你好当前设备：" + moduleName + "余额：" + rdAmt.ToString("f2");
                }
                sue = this.SendSms(listReceipts, content, out msg);
            }
            else
            {
                msg = "无手机号码";
            }
            errTxt = msg;
            errCode = (sue == true ? 1 : -1);
            bll.AddAV2_alarm_log_hand(log_id, aid, HdType.AL_Sms.ToString(), "", "", errCode, errTxt);
        }

        private void AlarmWx(long log_id, int co_id, string coName, DateTime lastTime, decimal rdVal)
        {
            try
            {
                DataTable dtCrm = bll.GetOpenidList(co_id, log_id, HdType.AL_Wx.ToString());
                foreach (DataRow drCrm in dtCrm.Rows)
                {
                    string openid = CommFunc.ConvertDBNullToString(drCrm["OpenID"]);
                    string crmName = CommFunc.ConvertDBNullToString(drCrm["CrmName"]);
                    int aid = CommFunc.ConvertDBNullToInt32(drCrm["Aid"]);
                    int errCode = CommFunc.ConvertDBNullToInt32(drCrm["ErrCode"]);
                    string errTxt = "";
                    if (errCode != 1)
                    {
                        bool rst = this.SendWx(openid, crmName, coName, lastTime, rdVal, out errTxt);
                        errCode = rst == true ? 1 : -1;
                    }
                    bll.AddAV2_alarm_log_hand(log_id, aid, HdType.AL_Wx.ToString(), openid, "", errCode, errTxt);
                }
            }
            catch (Exception ex)
            {
                FileLog.Error("获取微信信息错误:" + ex.Message + ex.StackTrace);
            }
        }

        private bool SendSms(List<string> listReceipts, string content, out string msg)
        {
            msg = "";
            try
            {
                bool sue = false;
                if (!string.IsNullOrEmpty(pro.Sms.ComSmsConfig.ComSms))
                    sue = pro.Sms.SendMessage(listReceipts, content, out msg);
                else if (!string.IsNullOrEmpty(SmsFun.AliSmsConfig.AliAppKey))
                    sue = pro.Sms.SmsMessage(listReceipts, content, out msg, "SMS_137656977");/*感知万物做演示*/
                else
                    msg = "没有发现短信接口";
                //if (sue == true)
                //    SmsTime = DateTime.Now;
                return sue;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        private bool SendEmail(List<string> listReceipts, string crmName, string coName, DateTime lastTime, decimal SyVal, out string msg)
        {
            msg = "";
            string subj1 = "告警通知";
            StringBuilder mailcont1 = new StringBuilder();
            mailcont1.Append("你好:" + crmName.ToString());
            mailcont1.Append("<br>");
            mailcont1.Append("房间名:" + coName);
            mailcont1.Append("<br>");
            mailcont1.Append("统计时间:" + lastTime.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            mailcont1.Append("<br>");
            mailcont1.Append("当前剩余电量:" + SyVal.ToString("f2") + "kWh");
            bool rst = false;
            if (listReceipts.Count > 0)
            {
                try
                {
                    pro.Email.SendMail(subj1, mailcont1.ToString(), "", listReceipts, null, null);
                    rst = true;
                }
                catch (Exception ex)
                {
                    msg = "邮件发送错误" + ex.Message;
                }
            }
            else
            {
                msg = "无邮箱地址";
            }
            return rst;
        }

        private bool SendWx(string opendid, string crmName, string coName, DateTime lastTime, decimal SyVal, out string msg)
        {
            msg = "";
            StringBuilder mailcont1 = new StringBuilder();
            if (SyVal > 0)
                mailcont1.Append("您好" + crmName + "当前剩余电量:" + SyVal.ToString("f2") + "kWh");
            else
                mailcont1.Append("您好" + crmName + "当前透支电量:" + Math.Abs(SyVal).ToString("f2") + "kWh");
            mailcont1.Append("<br>");
            mailcont1.Append("统计时间:" + lastTime.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            if (string.IsNullOrEmpty(opendid))
            {
                msg = "openid为空，微信用户未认证";
                return false;
            }
            bool rst = false;
            try
            {
                rst = pro.Wx.PushAlarm(opendid, "告警通知", mailcont1.ToString(), "", out msg);
                if (rst == true) msg = "发送成功";
                else msg = "发送失败";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FileLog.WriteLog("发送微信错误" + ex.Message);
            }
            return rst;
        }
    }
}
