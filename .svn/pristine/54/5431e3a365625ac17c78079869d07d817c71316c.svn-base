using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using YDS6000.Models;

namespace YDS6000.WebApi
{
    /// <summary>
    /// 读取设置另外个进程的类
    /// </summary>
    public class CacheMgr
    {
        private static object staticLocker = new Object();
        private static bool start = false;
        private static string ip = "127.0.0.1";
        private static int port = 5823;

        /// <summary>
        /// 获取一个key数据
        /// </summary>
        /// <param name="strKey">key</param>
        /// <returns>泛型数据</returns>
        public static YDS6000.Models.RstVar GetVal(string strKey)
        {
            string val = YDS6000.Models.ProcessSevice.Client(WebConfig.Port).GetCollectVal(strKey);
            if (string.IsNullOrEmpty(val)) return null;
            YDS6000.Models.RstVar rstVar = YDS6000.Models.JsonHelper.Deserialize<YDS6000.Models.RstVar>(val);
            return rstVar;
        }
        /// <summary>
        /// 发送采集数据
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public static bool SendCollectVal(List<ApiVar> var)
        {
            Automatic.Start();
            string val = JsonHelper.Serialize(var);
            bool rst = YDS6000.Models.ProcessSevice.Client(WebConfig.Port).SendCollectVal(val);
            return rst;
        }

        /// <summary>
        /// 发送采集数据
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SendCollectVal(ListenVModel vm, out string msg)
        {
            bool rst = false;
            msg = "";
            try
            {
                if (start == false)
                {
                    //string ps1 = IniHepler.GetConfig(WebConfig.LpFileName, "Automatic", "Ip");
                    //string[] ar = ps1.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    //if (ar.Count() != 2)
                    //{
                    //    msg = "IP地址格式错误";
                    //    return false;
                    //}
                    //ip = CommFunc.ConvertDBNullToString(ar[0]);
                    //port = CommFunc.ConvertDBNullToInt32(ar[1]);
                    string ps1 = IniHepler.GetConfig(WebConfig.LpFileName, "Automatic", "Port");
                    port = CommFunc.ConvertDBNullToInt32(ps1);
                    start = true;
                }
                /////////////////////////////////////////
                string data = JsonHelper.Serialize(vm);
                IPAddress host = IPAddress.Parse(ip);
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.ReceiveTimeout = 1000;
                clientSocket.SendTimeout = 1000;
                byte[] dd = System.Text.Encoding.Default.GetBytes(data);
                lock (CacheMgr.staticLocker)
                {
                    clientSocket.Connect(host, port);
                    clientSocket.Send(dd);
                    if (clientSocket.Connected)
                        clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
                rst = true;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("网络发送数据错误:" + ex.Message + ex.StackTrace);
                msg = ex.Message;
            }
            return rst;
        }
        /// <summary>
        /// 异步发送请求 
        /// </summary>
        /// <param name="vm"></param>
        public static void BeginSend(ListenVModel vm)
        {
            string msg = "";
            Thread th = new Thread((ThreadStart)delegate { SendCollectVal(vm, out msg); });
            th.Name = "Send";
            th.IsBackground = true;
            th.Start();
        }
    }
}