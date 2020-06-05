using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.Text;
using DataProcess.Rdc.Package;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;

namespace DataProcess.IOService.Collection
{
    public class CollectionHelper
    {
        public static ConcurrentDictionary<string, CollectionHelper> SysCol = new ConcurrentDictionary<string, CollectionHelper>();
        private string ip = "";
        private int dataPort = 8109;

        private bool Start = false;

        private RdcMethod rdc ;
        private CollectionHelper()
        {            
        }
        private void start()
        {
            try
            {
                if (Start == false)
                {
                    //string lpFileName = Config.lpFileName;
                    int cnt = 0;
                    rdc = new RdcMethod(this.ip, this.dataPort);
                    while (cnt < 3)
                    {
                        ++cnt;
                        bool rst = rdc.Close();
                        if (rst == true) break;
                        Thread.Sleep(500);
                    }
                    rdc.RdcVar = new RdcFunc.RDC_VarOk(resultVar); //放到业务层上去
                    //rdc.Instance(ip);/*初始化*/
                    /*开启事件监听*/
                    var ed = new Thread((ThreadStart)delegate { ReadEventDo(); });
                    ed.IsBackground = true;
                    ed.Start();
                    //
                    Start = true;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("初始化Rdc错误:" + ex.Message + ex.StackTrace);
            }
        }

        internal void Close()
        {
            int cnt = 0;
            while (cnt < 3)
            {
                ++cnt;
                bool rst = rdc.Close();
                if (rst == true) break;
                Thread.Sleep(500);
            }
            Start = false;
        }
        //初始化数据
        internal static void Instance(string handledBY, List<string> tag)
        {
            try
            {
                CollectionHelper threadHelper = null;
                if (SysCol.TryGetValue(handledBY, out threadHelper) == false)
                {
                    string[] ar = handledBY.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ar.Count() != 2)
                    {
                        FileLog.WriteLog("handBy:" + handledBY + "格式错误");
                        return;
                    }
                    threadHelper = new CollectionHelper();
                    threadHelper.ip = ar[0];
                    threadHelper.dataPort = CommFunc.ConvertDBNullToInt32(ar[1]);
                    threadHelper.dataPort = threadHelper.dataPort == 0 ? 8109 : threadHelper.dataPort;
                    SysCol.TryAdd(handledBY, threadHelper);
                }
                threadHelper.start();
                if (tag != null && tag.Count > 0)
                    threadHelper.rdc.AddVar(tag);/*增加变量*/
            }
            catch (Exception ex)
            {                
                FileLog.WriteLog("命令新增错误:" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        internal static void WriteVar(CommandVModel cmd)
        {
            CollectionHelper threadHelper = null;            
            //*如有配置地址*
            if (!string.IsNullOrEmpty(Config.IOHandledBY))
                cmd.HandledBY = Config.IOHandledBY;
            //
            if (SysCol.TryGetValue(cmd.HandledBY, out threadHelper) == false)
            {
                FileLog.Error("CollectionHelper对象不存在");
                return;
            }
            bool rst = false;
            AlarmType errCode = AlarmType.Err_Tx;
            string errTxt = "";
            try
            {
                int result = threadHelper.rdc.WriteVar(cmd.LpszDbVarName, cmd.DataValue);
                rst = result == 0 ? true : false;
                errCode = rst == true ? AlarmType.Sue : AlarmType.Err_Tx;
                errTxt = rst == true ? "成功写入数据值:" + cmd.DataValue : "写数值:" + cmd.DataValue + "失败,IOSerivce返回错误码:" + result.ToString();
                if (rst == false)
                    errCode = AlarmType.Err_Sb;
                FileLog.WriteSysLog("电表:" + cmd.ModuleAddr + " 变量:" + cmd.LpszDbVarName + " 值:" + cmd.DataValue + " 执行结果:" + errTxt);
            }
            catch (Exception ex)
            {
                errTxt = ex.Message;
                FileLog.Error("IOService变量" + cmd.LpszDbVarName + "下发命令错误:" + ex.Message + ex.StackTrace);
            }
            cmd.CollectTime = DateTime.Now;
            DataProcess.YdProcess.Helper.IOServiceWriteFail(cmd, DateTime.Now, 0, rst, errCode, errTxt);
        }

        [Obsolete("弃用",true)]
        internal static void WriteVar(int ledger, string handledBY, string lpszDbVarName, string dataVal)
        {
            CollectionHelper threadHelper = null;
            if (SysCol.TryGetValue(handledBY, out threadHelper) == false)
            {
                FileLog.Error("CollectionHelper对象不存在");
                return;
            }
            bool rst = false;
            AlarmType errCode = AlarmType.Err_Tx;
            string errTxt = "";
            try
            {
                int result = threadHelper.rdc.WriteVar(lpszDbVarName, dataVal);
                rst = result == 0 ? true : false;
                errCode = rst == true ? AlarmType.Sue : AlarmType.Err_Tx;
                errTxt = rst == true ? "成功" : "写数据错误";
            }
            catch (Exception ex)
            {
                errTxt = ex.Message;
                FileLog.Error("IOService变量" + lpszDbVarName + "下发命令错误:" + ex.Message + ex.StackTrace);
            }
            //DataProcess.YdProcess.Helper.IOServiceWriteFail(ledger, lpszDbVarName, dataVal, DateTime.Now, 0, rst, errCode, errTxt);
        }
        /// <summary>
        /// 回调函数,获取IOServer数据
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="lpszDbVarName"></param>
        /// <param name="lpszVal"></param>
        /// <param name="lpszdateTime"></param>
        /// <param name="dwUserData"></param>
        /// <returns></returns>
        private int resultVar(IntPtr handle, string lpszDbVarName, string lpszVal, string lpszdateTime, UInt32 dwUserData)
        {
            Thread.Sleep(5);
            try
            {
                DataProcess.YdProcess.Helper.IOServiceSaveCollectCache(lpszDbVarName, lpszVal, CommFunc.ConvertDBNullToDateTime(lpszdateTime), dwUserData, true, AlarmType.Sue, "");
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("变量" + lpszDbVarName + "增加到内存错误:" + ex.Message + ex.StackTrace);
                return 0;
            }
            return 0;
        }

        /// <summary>
        /// 获取事件数据
        /// </summary>
        private void ReadEventDo()
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    RdcMethod.IOEvent result = rdc.RDC_ReadEventDo();
                    if (result != null)
                        DataProcess.YdProcess.Helper.IOServiceEventCollect(result);
                    //if (result != null)
                    //    DataProcess.YdProcess.Helper.IOServiceEventCollect(result.lpszDbVarName, result.lpszVal, result.lpszdateTime, result.content);
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog("获取IOService告警事件错误:" + ex.Message + ex.StackTrace);
                }
            }
        }

    }
}
