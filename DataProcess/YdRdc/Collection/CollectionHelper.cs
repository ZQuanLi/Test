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
        private static bool Start = false;

        private CollectionHelper()
        {            
        }

        private static void start()
        {
            try
            {
                if (Start == false)
                {
                    string lpFileName = Config.lpFileName;
                    string ip = YDS6000.Models.IniHepler.GetConfig(lpFileName, "rdc", "Ip");
                    int port = YDS6000.Models.CommFunc.ConvertDBNullToInt32(YDS6000.Models.IniHepler.GetConfig(lpFileName, "rdc", "DataPort"));
                    int cnt = 0;
                    while (cnt < 3)
                    {
                        ++cnt;
                        bool rst = RdcMethod.Close();
                        if (rst == true) break;
                        Thread.Sleep(500);
                    }
                    RdcMethod.RdcVar = new RdcFunc.RDC_VarOk(resultVar); //放到业务层上去
                    RdcMethod.Instance(ip);/*初始化*/
                    Start = true;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("初始化Rdc错误:" + ex.Message + ex.StackTrace);
            }
        }

        internal static void Close()
        {
            int cnt = 0;
            while (cnt < 3)
            {
                ++cnt;
                bool rst = RdcMethod.Close();
                if (rst == true) break;
                Thread.Sleep(500);
            }
            Start = false;
        }
        //初始化数据
        internal static void Instance(List<string> tag)
        {
            try
            {
                start();
                if (tag != null && tag.Count > 0)
                    RdcMethod.AddVar(tag);/*增加变量*/
            }
            catch (Exception ex)
            {                
                FileLog.WriteLog("命令新增错误:" + ex.Message + ex.StackTrace);
                throw ex;
            }
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
        private static int resultVar(IntPtr handle, string lpszDbVarName, string lpszVal, string lpszdateTime, UInt32 dwUserData)
        {
            Thread.Sleep(5);
            try
            {
                DataProcess.YdProcess.Helper.SaveCollectCache(lpszDbVarName, lpszVal, CommFunc.ConvertDBNullToDateTime(lpszdateTime), dwUserData);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("变量" + lpszDbVarName + "增加到内存错误:" + ex.Message + ex.StackTrace);
                return 0;
            }
            return 0;
        }

    }
}
