using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Sockets;
using YDS6000.Models;

namespace DataProcess.Services
{
    public class ListenSocket
    {
        public void Start()
        {
            int port = Config.ListenPort;/*5823 端口*/
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(ip, port));
            listenSocket.Listen(100);
            listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), listenSocket);/*需用异步请求*/
        }

        /// <summary>
        /// 获取客户请求的socket
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = ar.AsyncState as Socket;
            DateTime errTime = DateTime.Now;
            try
            {
                Socket m_socketClient;
                if (listener == null) return;
                m_socketClient = listener.EndAccept(ar);
                if (m_socketClient == null) return;
                if (m_socketClient.Connected)
                {
                    List<byte> data = new List<byte>();
                    byte[] buffer = new byte[1024];
                    int length = 0;
                    while ((length = m_socketClient.Receive(buffer)) > 0)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            data.Add(buffer[j]);
                        }
                        if (length < buffer.Length)
                        {
                            break;
                        }
                    }
                    string rst = System.Text.Encoding.Default.GetString(data.ToArray(), 0, data.Count);
                    try
                    {
                        this.AssData(rst);
                    }
                    catch (Exception ex)
                    {
                        FileLog.WriteLog("服务端解析数据错误:" + ex.Message + ex.StackTrace);
                        FileLog.WriteLog("传输值:" + rst);
                    }
                }
                else
                {
                    FileLog.WriteLog("监听客户端连接已断开，获取不到数据..");
                }
                //
            }
            catch (System.Exception ex)
            {
                FileLog.WriteLog("服务端监听出错，错误信息:" + ex.Message);
            }
            finally
            {
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);/*重新接收一个监听*/
            }
        }

        private void AssData(string rst)
        {
            ListenVModel vm = JsonHelper.Deserialize<ListenVModel>(rst);
            if (vm == null)
                FileLog.WriteLog("服务端获取的数据错误:" + rst);

            if (vm.cfun.Trim().ToLower().Equals(ListenCFun.cmd.ToString().ToLower()))
            {
                CommandVModel cmd = JsonHelper.Deserialize<CommandVModel>(vm.content);
                if (cmd.TransferType == 4)
                {/*IOService通信*/
                    DataProcess.IOService.Collection.CollectionHelper.WriteVar(cmd);
                    //DataProcess.IOService.Collection.CollectionHelper.WriteVar(cmd.Ledger, cmd.HandledBY, cmd.LpszDbVarName, cmd.DataValue);
                }
                else
                {
                    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
                }
            }
            else if (vm.cfun.Trim().ToLower().Equals(ListenCFun.collect.ToString().ToLower()))
            {
                CommandVModel cmd = JsonHelper.Deserialize<CommandVModel>(vm.content);
                if (cmd.TransferType == 4)
                {
                    DataProcess.YdProcess.Helper.IOServiceSaveCollectCache(cmd.LpszDbVarName, cmd.DataValue, cmd.CollectTime.Value, 2, true, AlarmType.Sue, "");
                }
                 
            }
            else if (vm.cfun.Trim().ToLower().Equals(ListenCFun.addvar.ToString().ToLower()))
            {
                CommandVModel cmd = JsonHelper.Deserialize<CommandVModel>(vm.content);
                List<string> tag = JsonHelper.Deserialize<List<string>>(cmd.DataValue);
                DataProcess.IOService.Collection.CollectionHelper.Instance(cmd.HandledBY, tag);
            }
            else if (vm.cfun.Trim().ToLower().Equals(ListenCFun.config.ToString().ToLower()))
            {
                NCSys.Reset = false;
            }
            else {
                FileLog.WriteLog("下发命令类型未存在:" + rst);
            }
        }
    }
}
