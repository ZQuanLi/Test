using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using YDS6000.Models;

namespace Demo
{
    public class Config
    {
        /// <summary>
        /// 账目
        /// </summary>
        public static int Ledger = 0;
        /// <summary>
        /// 登陆人
        /// </summary>
        public static int Uid = 0;
        /// <summary>
        /// 项目控制
        /// </summary>
        public static string Project = "";
        /// <summary>
        /// 通讯端口
        /// </summary>
        public static int ListenPort = 5823;
        /// <summary>
        /// 配置地址
        /// </summary>
        public static string lpFileName = "";
        /// <summary>
        /// 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        /// </summary>
        public static int Log_levenl = 0;

        static Config()
        {
            if (string.IsNullOrEmpty(Config.lpFileName))
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string[] aa = path.Split(new Char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                string tt = aa[aa.Length - 1];
                if (tt.ToLower().Equals("Automatic".ToLower()))
                    Config.lpFileName = path.Substring(0, path.Length - (tt.Length + 1)) + "Web.ini";
                else
                    Config.lpFileName = path + "Web.ini";
            }
            string port = IniHepler.GetConfig(lpFileName, "Automatic", "Port");
            if (!string.IsNullOrEmpty(port))
                Config.ListenPort = CommFunc.ConvertDBNullToInt32(port);
            Config.Ledger = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Automatic", "Ledger"));
            Config.Uid = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Automatic", "Uid"));
            Config.Project = IniHepler.GetConfig(lpFileName, "Automatic", "Project");
            Config.Log_levenl = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Automatic", "Log_levenl"));
            //
            string connect = ConfigHelper.GetConnectionStrings("DefaultConnection");
            string dbHost = IniHepler.GetConfig(lpFileName, "Db", "DbHost");
            string dbName = IniHepler.GetConfig(lpFileName, "Db", "DbName");
            string dbUid = IniHepler.GetConfig(lpFileName, "Db", "DbUid");
            string dbPwd = IniHepler.GetConfig(lpFileName, "Db", "DBPwd");
            dbPwd = ConfigHelper.Decrypt(dbPwd);
            //
            connect = string.Format(connect, dbHost, dbName, dbUid, dbPwd);
            YDS6000.BLL.WholeBLL.ConnectionString(connect);
        }

    }
}
