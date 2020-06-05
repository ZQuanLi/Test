using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YDS6000.Models;
using System.Data;
using System.Text;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    partial class MonitorHelper
    {
        public APIRst GetYdSmsContent(int co_id,int module_id,string moduleAddr)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdSmsContent(co_id, module_id, moduleAddr);
                string content = "";
                string crmName = "", coName = "", mPhone = "", email = "";
                decimal syVal = 0, syAmt = 0, price = 0;
                DateTime lastTime = new DateTime();
                if (dtSource.Rows.Count > 0)
                {
                    mPhone = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["MPhone"]);
                    email = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Email"]);
                    crmName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CrmName"]);
                    coName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CoName"]);
                    syVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["RdVal"]);
                    price = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["Price"]);
                    lastTime = CommFunc.ConvertDBNullToDateTime(dtSource.Rows[0]["LastTime"]);
                    syAmt = Math.Round(syVal * price, 2, MidpointRounding.AwayFromZero);
                    //尊敬的${CrmName}用户，截止到${DateTime}，您所属的${CoName}号房间剩余金额是${SyAmt}元，剩余电量是${SyVal}，为了不影响您正常的用电使用，请及时到物管中心缴费！谢谢您的合作！
                    content = "尊敬的{0}用户，截止到{1}，您所属的{2}号房间剩余金额是{3}元，剩余电量是{4}，为了不影响您正常的用电使用，请及时到物管中心缴费！谢谢您的合作！";
                    content = string.Format(content, crmName, (lastTime.Year != 1900 ? lastTime.ToString("yyyy-MM-dd") : ""), coName, syAmt, syVal.ToString("f2") + "kWh");
                }
                object obj = new
                {
                    Content = content,
                    MPhone = mPhone,
                    Email = email,
                    CrmName = crmName,
                    LastTime = (lastTime.Year == 1900 || lastTime.Year == 0) ? "" : lastTime.ToString("yyyy-MM-dd"),
                    CoName = coName,
                    SyAmt = syAmt,
                    SyVal = syVal.ToString("f2") + "kWh"
                };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取发送信息错误 (GetYdSmsContent)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst SendYdSmsContent(string mphone,string crmName,string lastTime,string coName,string syAmt,string syVal)
        {
            //string content = "尊敬的{0}用户，截止到{1}，您所属的{2}号房间剩余金额是{3}元，剩余电量是{4}，为了不影响您正常的用电使用，请及时到物管中心缴费！谢谢您的合作！";
            //content = string.Format(content, crmName, lastTime, coName, syAmt, syVal);
            APIRst rst = new APIRst();
            string msg = "";
            if (string.IsNullOrEmpty(mphone))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "无电话号码，不能发送短信";
                return rst;
            }
            
            object obj = new
            {
                CrmName = crmName,
                DateTime = lastTime,
                CoName = coName,
                SyAmt = syAmt,
                SyVal = syVal
            };
            try
            {
                List<string> list = new List<string>();
                list.Add(mphone);
                bool sue = new SmsFun().AiSendMessage(list, JsonHelper.Serialize(obj), out msg);
                ////ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23765435", "53322077bd6695517f97b755cbaded7e");
                //ITopClient client = new DefaultTopClient(Common.SmsConfig.AliSmsConfig.AliUrl, Common.SmsConfig.AliSmsConfig.AliAppKey, Common.SmsConfig.AliSmsConfig.AliAppSecret);
                //AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                //req.Extend = "";
                //req.SmsType = "normal";
                //req.SmsFreeSignName = Common.SmsConfig.AliSmsConfig.AliSignName;// "空调电量监测管理";
                //req.SmsParam = JsonHelper.Serialize(obj); //"{CrmName:'刘超杰',DateTime:'2017-04-21',CoName:'1021',SyAmt:'10',SyVal:'1'}";
                //req.RecNum = mphone;
                //req.SmsTemplateCode = Common.SmsConfig.AliSmsConfig.AliTemplateCode;// "SMS_62975099";
                //AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                //string rst = rsp.Body;
                //bool sue = false;
                //if (rst.Contains("<success>true</success>"))
                //    sue = true;
                rst.rst = sue;
                rst.err.msg = (sue == false ? "发送失败请重试!" : "");
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;                
                FileLog.WriteLog("发送信息错误 (GetYdSmsContent)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst SendYdMailContent(string email,string crmName,DateTime lastTime,string coName,string syAmt,decimal syVal)
        {
            APIRst rst = new APIRst();
            string msg = "";
            if (string.IsNullOrEmpty(email))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "无邮箱地址，不能发送邮件";
                return rst;
            }
            object obj = new
            {
                CrmName = crmName,
                DateTime = lastTime,
                CoName = coName,
                SyAmt = syAmt,
                SyVal = syVal
            };
            try
            {
                EmailConfig cc = new EmailConfig();                
                DataTable dtSource = bll.GetMailInfo();
                if (dtSource.Rows.Count > 0)
                {
                    for(int i=0;i<4;i++)
                    {
                         if(CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfKey"]) == "MailFrom")                    
                               cc.MailFrom = CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfValue"]);
                         if (CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfKey"]) == "MailSmtpHost")
                             cc.MailSmtpHost = CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfValue"]);
                         if (CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfKey"]) == "MailSmtpPassword")
                             cc.MailSmtpPassword = CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfValue"]);
                         if (CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfKey"]) == "MailSmtpUser")
                             cc.MailSmtpUser = CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CfValue"]);
                   
                    }
                }                
                EmailUtilities.EmConfig = cc;

                List<string> list = new List<string>();
                list.Add(email);
                bool sue = this.SendEmail(list, crmName, coName, lastTime, syVal, out msg); 
                rst.rst = sue;
                rst.err.msg = (sue == false ? "发送失败请重试!" : "");
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("发送信息错误 (SendYdMailContent)", ex.Message + ex.StackTrace);
            }
            return rst;
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
                    new YDS6000.Models.EmailUtilities().SendMail(subj1, mailcont1.ToString(), "", listReceipts, null, null);
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
    }
}
