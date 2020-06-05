using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.Text;
using DataProcess.Rdc.Package;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;
using System.Windows.Forms;

namespace DataProcess.IOService.Collection
{
    public class CollectionHelper
    {
        private static bool Start = false;
        private static Demo.Form1 mainForm = Demo.Form1.mainForm;
        //public static string Ip = "";
        //public static int Port = 0;

        private CollectionHelper()
        {            
        }

        private static void start()
        {
            try
            {
                if (Start == false)
                {                   
                    string ip = mainForm.txtIp.Text;
                    int port = CommFunc.ConvertDBNullToInt32(mainForm.txtPort.Text);
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
                throw ex;
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
                mainForm.textBox2.Text = ex.Message;
            }
        }

        internal static void WriteVar(string lpszDbVarName, string dataVal)
        {
            bool rst = false;
            AlarmType errCode = AlarmType.Err_Qt;
            string errTxt = "";
            try
            {
                int result = RdcMethod.WriteVar(lpszDbVarName, dataVal);
                rst = result == 0 ? true : false;
                errCode = rst == true ? AlarmType.Sue : AlarmType.Err_Qt;
                errTxt = rst == true ? "成功" : "写数据错误";
            }
            catch (Exception ex)
            {
                errTxt = ex.Message;
                FileLog.WriteLog("IOService变量" + lpszDbVarName + "下发命令错误:" + ex.Message + ex.StackTrace);
            }
            //DataProcess.YdProcess.Helper.IOServiceSaveCollectCache(lpszDbVarName, dataVal, DateTime.Now, 0, rst, errCode, errTxt);
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
                if (!string.IsNullOrEmpty(mainForm.textBox2.Text))
                    mainForm.textBox2.Text = mainForm.textBox2.Text + Environment.NewLine;
                mainForm.textBox2.Text = mainForm.textBox2.Text + "值:" + lpszDbVarName + "时间:" + lpszVal;
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
