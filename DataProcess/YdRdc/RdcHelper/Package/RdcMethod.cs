using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using YDS6000.Models;

namespace DataProcess.Rdc.Package
{
    public class RdcMethod
    {
        private static RdcMethod Helper = null;
        /// <summary>
        /// 执行监视的开始时间
        /// </summary>
        public static DateTime? StartTime = null;
        /// <summary>
        /// 资源锁
        /// </summary>
        private static object ObjLock = new object();
        internal static RdcFunc.RDC_VarOk RdcVar; /*回调函数，给业务层使用*/
        private static IntPtr? Handle;

        private static string ip;
        private static int DataPort = 8109;
        private static int ConfigPort = 11092;

        /// <summary>
        /// 总线程下发命令
        /// </summary>
        /// <param name="ThreadName"></param>
        private RdcMethod()
        {
        }

        public static RdcMethod Instance(string ip, int DataPort = 8109)
        {
            RdcMethod.ip = ip;
            RdcMethod.DataPort = DataPort;
            Helper = new RdcMethod();
            try
            {
                lock (ObjLock)
                {
                    RdcMethod.start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("启动IOServices监听错误:" + ex.Message + ex.StackTrace);
            }
            return Helper;
        }

        public static bool AddVar(List<string> Tags)
        {

            bool rst = false;
            try
            {
                lock (ObjLock)
                {
                    if (RdcMethod.Handle == null)
                        RdcMethod.start();
                    rst = RdcMethod.addVar(Tags);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("增加监听变量错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public static bool Close()
        {
            int rst = -1;
            if (RdcMethod.Handle != null)
            {
                rst = RdcFunc.RDC_StopRun(RdcMethod.Handle.Value);
                rst = RdcFunc.RDC_Close(RdcMethod.Handle.Value);
            }
            RdcMethod.Handle = null;
            return rst == 0 ? true : false;
        }

        private static bool addVar(List<string> Tags)
        {
            Thread.Sleep(250);
            int rst = RdcFunc.RDC_StopRun(RdcMethod.Handle.Value);
            if (rst != 0)
            {
                FileLog.WriteLog("停止客户端监控错误:" + rst);
                return false;
            }
            Thread.Sleep(250);
            int nDataType = 2;/*默认是2*/
            bool exits = false;
            /*增加监听的变量*/
            foreach (string tag in Tags)
            {
                rst = RdcFunc.RDC_AddVar(RdcMethod.Handle.Value, tag, nDataType);
                if (rst == 0)
                {
                    exits = true;
                    //msg = "正在监听...";
                    //Yada.Public.FileLog.WriteLog("变量:" + tag+"监视成功");
                }
                else if (rst == 1)
                {
                    FileLog.WriteLog("变量:" + tag + "监听错误errorcode:" + rst);
                }
                else if (rst == 2)
                {
                    exits = true;
                    //msg = "监听进行中...";
                }
                else if (rst == 3)
                {
                    FileLog.WriteLog("变量:" + tag + "连接接口错误errorcode:" + rst);
                }
                else
                {
                    FileLog.WriteLog("变量:" + tag + "监听未知错误errorcode:" + rst);
                }
            }
            int i = 0;
            while (i < 3 && exits==true)
            {
                ++i;
                Thread.Sleep(250);
                rst = RdcFunc.RDC_StartRun(RdcMethod.Handle.Value);
                if (rst != 0)
                {
                    FileLog.WriteLog("第" + i.ToString() + "重新启动客户端监控错误:" + rst);
                    return false;
                }
                else
                {
                    break;
                }
            }
            return true;
        }


        private static void start()
        {
            if (string.IsNullOrEmpty(ip) || DataPort == 0)
                throw new Exception("地址或端口不能为空");

            if (RdcMethod.Handle != null)
            {
                RdcFunc.RDC_StopRun(RdcMethod.Handle.Value);
                RdcFunc.RDC_Close(RdcMethod.Handle.Value);
            }
            //
            //RdcVar = new RdcFunc.RDC_VarOk(resultVar); //放到业务层上去
            //打开端口
            RdcMethod.Handle = RdcFunc.RDC_Open(ip, DataPort, RdcVar, 0);
            //开启监听
            //返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3
            //RdcFunc.RDC_AddVar(RdcHelper.Handle.Value, "R1001_4710.1AA1.Ua", 2);
            int rest = RdcFunc.RDC_StartRun(RdcMethod.Handle.Value);
            if (rest != 0)
            {
                throw new Exception("开启监听错误:" + rest);
            }
        }
    }
}
