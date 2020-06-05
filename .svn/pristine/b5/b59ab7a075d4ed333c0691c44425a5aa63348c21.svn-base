using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using YDS6000.Models;

namespace DataProcess
{
    internal class SysPro
    {
        public string ProjectKey = "";
        public string Project = "";
        public int FrMd = 15;
        public int MaxVal = 0;
        public int Auto = 0;
        public EmailUtilities Email = new EmailUtilities();
        public SmsFun Sms = new SmsFun();
        public WxFun Wx = new WxFun();
    }
    internal class CollectVModel
    {
        public int Ledger { get; set; }
        public int Module_id { get; set; }
        public string ModuleAddr { get; set; }
        public int Fun_id { get; set; }
        public string FunType { get; set; }
        public int Action { get; set; }
        public string FunName { get; set; }
        public string CachedKey { get; set; }
        public bool IsDb { get; set; }
        /// <summary>
        /// 更新入库时间
        /// </summary>
        public DateTime UpTime = new DateTime(1900, 1, 1);
    }

    internal class AlarmCollect
    {
        public CommandVModel cmd { get; set; }
        //public string lpszDbVarName { get; set; }
        public string lpszVal { get; set; }
        public DateTime lpszdateTime { get; set; }
        public AlarmType errCode { get; set; }
        public string errTxt { get; set; }
        /// <summary>
        /// 是否发送告警
        /// </summary>
        public int isSend { get; set; }
        ///// <summary>
        ///// 发送间隔时间
        ///// </summary>
        //public int spanTime { get; set; }
    }

    /// <summary>
    /// 设备命令
    /// </summary>
    public class CmdResult
    {
        private IList<string> _mRst = new List<string>();
        private int _errcode = 1;
        private string _errtxt = string.Empty;
        private DateTime _update = DateTime.Now;
        private string funName = string.Empty;
        private string funType = string.Empty;
        public int Ledger { get; set; }
        public int Module_id { get; set; }
        public string ModuleAddr { get; set; }

        public string FunType { get; set; }

        public string FunName { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime Update { get; set; }
        /// <summary>
        /// 数据处理线程
        /// </summary>
        public string BiludBY { get; set; }

        /// <summary>
        /// 返回下发的命令
        /// </summary>
        public IList<string> Result
        {
            get { return _mRst; }
            set { _mRst = value; }
        }

        /// <summary>
        /// 运行状态标示，1成功，其他失败
        /// </summary>
        public int ErrCode
        {
            get { return _errcode; }
            set { _errcode = value; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrTxt
        {
            get { return _errtxt; }
            set { _errtxt = value; }
        }

        public string Value { get; set; }
    }

    /// <summary>
    /// 设备命令
    /// </summary>
    public class CmdResult_Bk
    {
        private IList<string> _mRst = new List<string>();
        private int _errcode = 1;
        private string _errtxt = string.Empty;
        private DateTime _update = DateTime.Now;
        //private int? _ssr = null;
        /// <summary>
        /// 命令运行时的ID
        /// </summary>
        public long Rid { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime Update { get; set; }
        /// <summary>
        /// 数据处理线程
        /// </summary>
        public string BiludBY { get; set; }
        /// <summary>
        /// 命令
        /// </summary>
        public CommandVModel Command { get; set; }

        /// <summary>
        /// 返回下发的命令
        /// </summary>
        public IList<string> Result
        {
            get { return _mRst; }
            set { _mRst = value; }
        }

        /// <summary>
        /// 运行状态标示，1成功，其他失败
        /// </summary>
        public int ErrCode
        {
            get { return _errcode; }
            set { _errcode = value; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrTxt
        {
            get { return _errtxt; }
            set { _errtxt = value; }
        }

        public string Value { get; set; }
    }

    public class Config
    {
        /// <summary>
        /// 账目
        /// </summary>
        public static string Systems = "";
        /// <summary>
        /// 登陆人
        /// </summary>
        public static int Uid = 0;
        /// <summary>
        /// 项目控制
        /// </summary>
        //public static string Project = "";
        public static string SystemName = "";

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

        /// <summary>
        /// IOService的地址
        /// </summary>
        public static string IOHandledBY = "";

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
            Config.Systems = IniHepler.GetConfig(lpFileName, "Automatic", "Systems");
            Config.Uid = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Automatic", "Uid"));
            //Config.Project = IniHepler.GetConfig(lpFileName, "Automatic", "Project");
            Config.SystemName = IniHepler.GetConfig(lpFileName, "Automatic", "SystemName");
            Config.Log_levenl = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Automatic", "Log_levenl"));
            string rdcIp = CommFunc.ConvertDBNullToString(IniHepler.GetConfig(lpFileName, "Rdc", "Ip"));
            int rdcPort = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Rdc", "DataPort"));
            if (!string.IsNullOrEmpty(rdcIp))
                Config.IOHandledBY = rdcIp + ":" + rdcPort;
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
