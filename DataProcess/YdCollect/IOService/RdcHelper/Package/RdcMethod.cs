using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using YDS6000.Models;

namespace DataProcess.Rdc.Package
{
    public class RdcMethod
    {
        //private RdcMethod Helper = null;
        /// <summary>
        /// 执行监视的开始时间
        /// </summary>
        //public DateTime? StartTime = null;
        /// <summary>
        /// 资源锁
        /// </summary>
        private object ObjLock = new object();
        internal RdcFunc.RDC_VarOk RdcVar; /*回调函数，给业务层使用*/
        private IntPtr? Handle;

        private string ip;
        private int DataPort = 8109;
        //private int ConfigPort = 11092;

        /// <summary>
        /// 总线程下发命令
        /// </summary>
        /// <param name="ThreadName"></param>
        //private RdcMethod()
        //{
        //}

        public RdcMethod(string ip, int DataPort = 8109)
        {
            this.ip = ip;
            this.DataPort = DataPort;
            //Helper = new RdcMethod();
            try
            {
                lock (ObjLock)
                {
                    this.start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("启动IOServices监听错误:" + ex.Message + ex.StackTrace);
            }
            //return Helper;
        }

        public bool AddVar(List<string> Tags)
        {

            bool rst = false;
            try
            {
                lock (ObjLock)
                {
                    if (this.Handle == null)
                        this.start();
                    rst = this.addVar(Tags);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("增加监听变量错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public int WriteVar(string lpszDbVarName, string dataVal)
        {
            float dd = CommFunc.ConvertDBNullToFloat(dataVal);
            float[] arr = new float[] { dd };
            IntPtr structPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(4);
            System.Runtime.InteropServices.Marshal.Copy(arr, 0, structPtr, 1);
            int rst = RdcFunc.RDC_WriteVarByName(this.Handle.Value, lpszDbVarName, structPtr);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(structPtr);
            return rst;
        }

        private IntPtr pReadEventData = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi("");

        public IOEvent RDC_ReadEventDo()
        {
            // 报警时间 /t事件类型/t时间级别/t事件内容/t归属厂站/t变电所/t变量ID号/t报警变量/t报警区域/报警值/t操作员/t处理时间/t处理过程/t处理结果/t备注/t
            if (this.Handle == null) return null;
            //IntPtr pData = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi("");

            try
            {
                int len = 500;
                int rst = RdcFunc.RDC_ReadEvent(this.Handle.Value, pReadEventData, len);
                if (rst == 0)
                {
                    string str = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(pReadEventData);
                    //string[] temp = Regex.Split(str.ToString(), "/t", RegexOptions.IgnoreCase); // str.ToString().Split(new char[] { "/t" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] ss  = str.Split(new char[] { '\t' }, StringSplitOptions.None);
                    if (ss.Count() < 15)
                    {
                        FileLog.WriteLog("获取告警值数组错误:" + str);
                        return null;
                    }
                    IOEvent cls = new IOEvent();
                    cls.lpszdateTime = CommFunc.ConvertDBNullToDateTime(ss[0]);
                    cls.lpszDbVarName = CommFunc.ConvertDBNullToString(ss[7]);
                    cls.lpszVal = CommFunc.ConvertDBNullToString(ss[9]);
                    cls.lpszType = CommFunc.ConvertDBNullToString(ss[1]);
                    cls.lpszGrade = CommFunc.ConvertDBNullToString(ss[2]);
                    cls.content = CommFunc.ConvertDBNullToString(ss[3]);
                    return cls;
                }
                else
                {
                    //FileLog.WriteLog("获取告警值返回值错误:" + rst);
                }               
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("获取事件错误:" + ex.Message + ex.StackTrace);
            }
            finally
            {
                //System.Runtime.InteropServices.Marshal.FreeHGlobal(pData);
            }
            return null;
        }

        public bool Close()
        {
            int rst = -1;
            if (this.Handle != null)
            {
                rst = RdcFunc.RDC_StopRun(this.Handle.Value);
                rst = RdcFunc.RDC_Close(this.Handle.Value);
            }
            this.Handle = null;
            return rst == 0 ? true : false;
        }

        private bool addVar(List<string> Tags)
        {
            Thread.Sleep(250);
            int rst = RdcFunc.RDC_StopRun(this.Handle.Value);
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
                rst = RdcFunc.RDC_AddVar(this.Handle.Value, tag, nDataType);
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
            bool isPass = false;
            while (++i <= 3 && exits==true)
            {
                Thread.Sleep(250);
                rst = RdcFunc.RDC_StartRun(this.Handle.Value);
                if (rst != 0)
                {
                    FileLog.WriteLog("IP:" + this.ip + ":" + this.DataPort + "第" + i.ToString() + "次重新启动Rdc监控错误返回值:" + rst);
                    continue;
                    //return false;
                }
                else
                {
                    isPass = true;
                    break;
                }
            }
            return isPass;
        }


        private void start()
        {
            if (string.IsNullOrEmpty(ip) || DataPort == 0)
                throw new Exception("地址或端口不能为空");

            if (this.Handle != null)
            {
                RdcFunc.RDC_StopRun(this.Handle.Value);
                RdcFunc.RDC_Close(this.Handle.Value);
            }
            //
            //RdcVar = new RdcFunc.RDC_VarOk(resultVar); //放到业务层上去
            //打开端口
            this.Handle = RdcFunc.RDC_Open(ip, DataPort, RdcVar, 0);
            //开启监听
            //返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3
            //RdcFunc.RDC_AddVar(RdcHelper.Handle.Value, "R1001_4710.1AA1.Ua", 2);
            int rest = RdcFunc.RDC_StartRun(this.Handle.Value);
            if (rest != 0)
            {
                throw new Exception("Ip:" + ip + ":" + DataPort + "开启监听错误返回值:" + rest);
            }
            else
            {
                FileLog.WriteLog("启动IOServic:" + ip + ":" + DataPort + "服务成功");
            }
        }

        public class IOEvent
        {
            /// <summary>
            /// 告警时间
            /// </summary>
            public DateTime lpszdateTime { get; set; }
            /// <summary>
            /// 变量ID号
            /// </summary>
            public string lpszDbVarName { get; set; }
            /// <summary>
            /// 报警值(采集值)
            /// </summary>
            public string lpszVal { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string lpszType { get; set; }
            /// <summary>
            /// 级别
            /// </summary>
            public string lpszGrade { get; set; }
            /// <summary>
            /// 告警内容
            /// </summary>
            public string content { get; set; }

        }
    }
}
