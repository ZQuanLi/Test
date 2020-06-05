using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using YDS6000.BLL;
using YDS6000.Models;

namespace DataProcess
{
    static class Program
    {
        private static System.Threading.Mutex mutex;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //处理未捕获的异常   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            new Config();/*初始化配置*/
            //for (int i = 0; i < args.Length; i++)
            //{
            //    if (i == 0 && !string.IsNullOrEmpty(args[i]))
            //        Config.Project = args[i].Trim();
            //};
            //
            string argStr = string.IsNullOrEmpty(Config.Systems) ? "test" : Config.Systems;
            mutex = new System.Threading.Mutex(true, argStr);
            if (!mutex.WaitOne(0, false))
                return;

            bool isStart = false;
            string AppCode = ConfigHelper.GetComputerSn();
            string sn = ConfigHelper.GetAppSettings("sn");
            bool isVrc = GetVrc();
            if (string.IsNullOrEmpty(sn) || !sn.Trim().Equals(ConfigHelper.Encrypt(AppCode.Trim())) || isVrc == false)
            {
                FormAKey form = new FormAKey();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FormSetConnInfo form0 = new FormSetConnInfo();
                    if (form0.ShowDialog() == DialogResult.OK)
                        isStart = true;
                }
            }
            else
            {
                FormSetConnInfo form0 = new FormSetConnInfo();
                if (form0.ShowDialog() == DialogResult.OK)
                    isStart = true;
            }

            if (isStart == false) return;

            new DataProcess.Services.ListenSocket().Start();/*开启监听服务*/
            DataProcess.YdProcess.Helper.Start();// 启动服务

            FileLog.WriteLog("启动成功");

            MainForm mainForm = new MainForm();
            mainForm.Text = mainForm.Text + " -- 项目名称:" + Config.SystemName;
            Application.Run(mainForm);

        }

        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        private static bool GetVrc()
        {
            string dsn = ConfigHelper.GetAppSettings("dsn");
            dsn = ConfigHelper.Decrypt(dsn);
            DateTime dtSn;
            bool isVrc = DateTime.TryParse(dsn, out dtSn);
            if (isVrc == false)
                return false;
            if (DateTime.Now >= dtSn)/*过期了*/
                return false;
            return true;
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            StringBuilder sb1 = new StringBuilder();
            try
            {
                //"系統運行异常：" + DateTime.Now.ToString() + "\r\n"            
                //sb1.AppendLine(string.Format("{0}:{1}", Resources.MainProgram001, Common.CommFunc.ConvertDateTimeToString(DateTime.Now)));
                Exception error = e.Exception as Exception;
                if (error != null)
                {
                    //异常类型:
                    sb1.AppendLine(string.Format("{0}:{1}", "异常类型", error.GetType().Name));
                    //异常消息：
                    sb1.AppendLine(string.Format("{0}:{1}", "异常消息", error.Message));
                    //Source
                    sb1.AppendLine(string.Format("{0}:{1}", "Source", error.Source));
                    //异常详细信息：
                    sb1.AppendLine(string.Format("{0}:{1}", "异常详细信息", error.StackTrace));
                    //內部異常
                    //sb1.AppendLine(string.Format("{0}2:{1}", Resources.MainProgram005, error.InnerException.Message));
                    if (error.InnerException != null)
                    {
                        sb1.AppendLine("Inner Exception: " + error.InnerException.Message);
                    }
                }
                else
                {
                    //应用程序错误：
                    sb1.AppendLine("应用程序错误");
                }
                SetErr(error);
            }
            catch (Exception ex2)
            {
                sb1.Append(ex2.Message + ";\r\n" + ex2.StackTrace);
                SetErr(ex2);
            }
        }

        static void SetErr(Exception error)
        {
            StringBuilder sb1 = new StringBuilder();
            try
            {
                if (error != null)
                {
                    //异常类型:
                    sb1.AppendLine(string.Format("{0}:{1}", "异常类型", error.GetType().Name));
                    //异常消息：
                    sb1.AppendLine(string.Format("{0}:{1}", "异常消息", error.Message));
                    //Source
                    sb1.AppendLine(string.Format("{0}:{1}", "Source", error.Source));
                    //异常详细信息：
                    sb1.AppendLine(string.Format("{0}:{1}", "异常详细信息", error.StackTrace));
                    //內部異常
                    //sb1.AppendLine(string.Format("{0}2:{1}", Resources.MainProgram005, error.InnerException.Message));
                    if (error.InnerException != null)
                    {
                        sb1.AppendLine("Inner Exception: " + error.InnerException.Message);
                    }
                }
                else
                {
                    //应用程序错误：
                    sb1.AppendLine("应用程序错误");
                }
            }
            catch (Exception ex2)
            {
                sb1.Append(ex2.Message + ";\r\n" + ex2.StackTrace);
            }
            if (!string.IsNullOrEmpty(sb1.ToString()))
                WriteLog(sb1.ToString());


        }

        private static void WriteLog(string errorText)
        {
            try
            {
                int iFileLen = 1000 * 1024; // K
                string logPath = AppDomain.CurrentDomain.BaseDirectory + "AppErr.log";

                FileInfo fi = new FileInfo(logPath);
                if (fi.Exists && fi.Length > iFileLen)
                {
                    fi.Delete();
                }

                using (StreamWriter sw = new StreamWriter(logPath, true, Encoding.Unicode))
                {
                    sw.WriteLine(string.Format("{0}-->{1}", DateTime.Now, errorText));
                }
            }
            catch
            {
            }
        }
    }
}
