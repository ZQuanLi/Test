using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;

namespace YDS6000.Models
{
    public class SmsFun
    {
        /// <summary>
        /// 启动发送短信端口
        /// </summary>
        /// <param name="nPort">端口</param>
        /// <param name="BaudRate">比特率默认38400</param>
        /// <param name="Parity">校验位 2</param>
        /// <param name="DataBits">数据位 8</param>
        /// <param name="StopBits">停止位 0</param>
        /// <param name="FlowControl">流控 0</param>
        /// <param name="csca">csca为短信中心号码，如果直接用卡内已存的短信中心号码则用"card"（小写）</param>
        /// <returns>0失败；非零成功</returns>
        [DllImport("SMSDLL.dll", CharSet = CharSet.Ansi, EntryPoint = "SMSStartService", CallingConvention = CallingConvention.StdCall)]
        internal static extern int SMSStartService(UInt32 nPort, UInt32 BaudRate = 38400, int Parity = 2, int DataBits = 8, int StopBits = 0, int FlowControl = 0, string csca = "card");

        /// <summary>
        /// 发送短消息,返回短消息编号:index，从0开始递增，该函数不会阻塞，立既返回，请用函数SMSQuery(DWORD index)来查询是否发送成功
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="MPhone"></param>
        /// <returns>无阻塞立刻返回短信序号</returns>
        [DllImport("SMSDLL.dll", CharSet = CharSet.Ansi, EntryPoint = "SMSSendMessage", CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 SMSSendMessage(string Msg, string MPhone);

        /// <summary>
        /// 查询指定序号的短信是否发送成功
        /// </summary>
        /// <param name="Index">短信序号</param>
        /// <returns>0失败；1成功；-1表示没有查询到该序号可能正在发送当中</returns>
        [DllImport("SMSDLL.dll", CharSet = CharSet.Ansi, EntryPoint = "SMSQuery", CallingConvention = CallingConvention.StdCall)]
        internal static extern int SMSQuery(UInt32 Index);

        /// <summary>
        /// 停止服务，并关闭串口,0为失败，非0为成功
        /// </summary>
        /// <returns></returns>
        [DllImport("SMSDLL.dll", CharSet = CharSet.Ansi, EntryPoint = "SMSStopSerice", CallingConvention = CallingConvention.StdCall)]
        internal static extern int SMSStopSerice();

        /// <summary>
        /// 发送短信的端口
        /// </summary>
        public ComSmsConfig ComSmsConfig { get; set; }
        /// <summary>
        /// 阿里发送短息类
        /// </summary>
        public static AliSmsConfig AliSmsConfig { get; set; }

        private static bool isStart = false;

        /// <summary>
        /// 异步发送短信(网关使用)
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="MPhone"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendMessage(List<string> listReceipts, string Content, out string msg)
        {
            msg = "";
            int nPort = CommFunc.ConvertDBNullToInt32(ComSmsConfig.ComSms);
            if (nPort == 0)
            {
                msg = "发送短息端口不能为空";
                return false;
            };
            if (isStart == false)
            {
                ChkPort();
                SmsFun.SMSStopSerice();/*关闭服务*/
                int rst = SmsFun.SMSStartService((UInt32)nPort); /*打开服务*/
                msg = rst == 0 ? "启动失败:未连接好设备或者波特率错误" : "";
                isStart = rst == 0 ? false : true;
                if (rst == 0) return false;
            }

            bool sue = false;
            foreach (string mpone in listReceipts)
            {
                System.Threading.Thread.Sleep(50);
                UInt32 index = SmsFun.SMSSendMessage(Content, mpone);
                System.Threading.Thread.Sleep(50);
                int ss = SmsFun.SMSQuery(index);
                int time = 0;
                while (time <= 3000 && ss != 1)
                {
                    System.Threading.Thread.Sleep(1000);
                    ss = SmsFun.SMSQuery(index);
                    time = time + 1000;
                    if (ss == 1)
                        break;
                }
                if (ss == 0)
                {
                    if (!string.IsNullOrEmpty(msg))
                        msg = msg + ",";
                    msg = msg + mpone + "发送短信失败";
                }
                if (sue == false)
                    sue = (ss == 0 ? false : true);
            }
            isStart = sue;
            return sue;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        public bool StopSerice()
        {
            isStart = false;
            int rst = SmsFun.SMSStopSerice();
            return rst == 0 ? false : true;
        }

        /// <summary>
        /// 阿里发送短息
        /// </summary>
        /// <param name="listReceipts"></param>
        /// <param name="content"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AiSendMessage(List<string> listReceipts, string content, out string msg,string smsTemplateCode = "")
        {
            msg = "";
            try
            {
                StringBuilder mphone = new StringBuilder();
                foreach (var item in listReceipts)
                {
                    if (!string.IsNullOrEmpty(mphone.ToString()))
                        mphone.Append(",");
                    mphone.Append(item);
                }
                //ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23765435", "53322077bd6695517f97b755cbaded7e");
                ITopClient client = new DefaultTopClient(AliSmsConfig.AliUrl, AliSmsConfig.AliAppKey, AliSmsConfig.AliAppSecret);
                AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                req.Extend = "";
                req.SmsType = "normal";
                req.SmsFreeSignName = AliSmsConfig.AliSignName;// "空调电量监测管理";
                req.SmsParam = content; //"{CrmName:'刘超杰',DateTime:'2017-04-21',CoName:'1021',SyAmt:'10',SyVal:'1'}";
                req.RecNum = mphone.ToString();
                req.SmsTemplateCode = string.IsNullOrEmpty(smsTemplateCode) ? AliSmsConfig.AliTemplateCode : smsTemplateCode; //"SMS_62975099";
                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                string rst = rsp.Body;
                bool sue = false;
                if (rst.Contains("<success>true</success>"))
                {
                    sue = true;
                }
                else
                {
                    msg = "发送短信失败";
                }
                return sue;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        public bool SmsMessage111(List<string> listReceipts, string content, out string msg, string smsTemplateCode = "")
        {
            msg = "";
            //产品名称:云通信短信API产品,开发者无需替换
            string product = "Dysmsapi";
            //产品域名,开发者无需替换
            string domain = "dysmsapi.aliyuncs.com";
            // TODO 此处需要替换成开发者自己的AK(在阿里云访问控制台寻找)
            String accessKeyId = AliSmsConfig.AliAppKey;
            String accessKeySecret = AliSmsConfig.AliAppSecret;
            StringBuilder mphone = new StringBuilder();
            foreach (var item in listReceipts)
            {
                if (!string.IsNullOrEmpty(mphone.ToString()))
                    mphone.Append(",");
                mphone.Append(item);
            }

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            bool rst = false;
            try
            {

                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = mphone.ToString();//"13710218209,18824867279";
                //必填:短信签名-可在短信控制台中找到
                request.SignName = AliSmsConfig.AliSignName;// "感知万物";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = string.IsNullOrEmpty(smsTemplateCode) ? AliSmsConfig.AliTemplateCode : smsTemplateCode; //"SMS_137410645";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = content; //"{\"moduleName\":\"测试\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "yourOutId";
                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);
                rst = response.Code.ToLower().Equals("ok") ? true : false;
                msg = rst == false ? "发送短信失败" : "";
            }
            catch (ServerException e)
            {
                msg = e.ErrorMessage;
                //Console.WriteLine(e.ErrorCode);
            }
            catch (ClientException e)
            {
                msg = e.ErrorMessage;
                //Console.WriteLine(e.ErrorCode);
            }
            return rst;
        }

        public bool SmsMessage(List<string> listReceipts, string content, out string msg, string smsTemplateCode = "")
        {
            msg = "";
            bool tt = false;
            int cc = 0, ff = 0;
            StringBuilder ffMsg = new StringBuilder();
            StringBuilder ffMps = new StringBuilder();
            foreach (var item in listReceipts)
            {
                bool rst = false;
                try
                {
                    rst = SmsMessage(item, content, out msg, smsTemplateCode);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                if (rst == false)
                {
                    if (!string.IsNullOrEmpty(ffMps.ToString()))
                        ffMps.Append(",");
                    ffMps.Append(item);

                    if (string.IsNullOrEmpty(ffMsg.ToString()))
                        ffMsg.Append(msg);
                }
                tt = rst == true ? true : tt;
                cc = cc + (rst == true ? 1 : 0);
                ff = ff + (rst == false ? 1 : 0);
                System.Threading.Thread.Sleep(1000);/*1秒*/
            }
            if (cc > 0 && ff > 0)
                msg = "失败号码:"+ ffMps.ToString()+" " + ffMsg.ToString();
            return tt;
        }

        public bool SmsMessage(string mphone, string content, out string msg, string smsTemplateCode = "")
        {
            msg = "";
            //产品名称:云通信短信API产品,开发者无需替换
            string product = "Dysmsapi";
            //产品域名,开发者无需替换
            string domain = "dysmsapi.aliyuncs.com";
            // TODO 此处需要替换成开发者自己的AK(在阿里云访问控制台寻找)
            String accessKeyId = AliSmsConfig.AliAppKey;
            String accessKeySecret = AliSmsConfig.AliAppSecret;
          

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            bool rst = false;
            try
            {

                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = mphone.ToString();//"13710218209,18824867279";
                //必填:短信签名-可在短信控制台中找到
                request.SignName = AliSmsConfig.AliSignName;// "感知万物";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = string.IsNullOrEmpty(smsTemplateCode) ? AliSmsConfig.AliTemplateCode : smsTemplateCode; //"SMS_137410645";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = content; //"{\"moduleName\":\"测试\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = new Random(Guid.NewGuid().GetHashCode()).Next(100, 100000).ToString(); //"yourOutId";
                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);
                rst = response.Code.ToLower().Equals("ok") ? true : false;
                msg = rst == false ? "发送短信失败" : "";
            }
            catch (ServerException e)
            {
                msg = e.ErrorMessage;                                
                //Console.WriteLine(e.ErrorCode);
            }
            catch (ClientException e)
            {
                msg = e.ErrorMessage;
                //Console.WriteLine(e.ErrorCode);
            }
            return rst;
        }

        private void ChkPort()
        {
            int nPort = CommFunc.ConvertDBNullToInt32(ComSmsConfig.ComSms);
            System.IO.Ports.SerialPort m_serialPort = new System.IO.Ports.SerialPort();
            m_serialPort.PortName = "COM" + nPort;
            m_serialPort.BaudRate = 38400;
            m_serialPort.Parity = System.IO.Ports.Parity.Even;
            m_serialPort.DataBits = 8;
            m_serialPort.StopBits = System.IO.Ports.StopBits.One;
            if (m_serialPort.IsOpen == false)
                m_serialPort.Open();/*检测端口是否打开*/
            m_serialPort.Close();
        }
    }
}
