using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi
{
    /// <summary>
    /// 系统配置类
    /// </summary>
    public class WebConfig
    {
        private static int _ledger = 0;
        private static string _memcachKey = "IOService" + "->" + _ledger + "->";
        private static string rVer = DateTime.Now.ToString();// System.Configuration.ConfigurationManager.AppSettings["RVersion"];
        private static string _lpFileName = "";
        private static string _project = "YdSep";
        private static string tempXlsPath = "/XTemp/";

        public static int Port { get; set; }

        public static string XlsPath()
        {
            return tempXlsPath;
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public static string RVersion()
        {
            return rVer;
        }

        /// <summary>
        /// 账目
        /// </summary>
        public static int Ledger
        {
            get { return _ledger; }
            set
            {
                WebConfig._ledger = value;
            }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public static string SysProject
        {
            get { return _project; }
            set { _project = value; }
        }

        /// <summary>
        /// Memcach的前置key,区分不同项目跟账号
        /// </summary>
        public static string MemcachKey
        {
            get { return _memcachKey; }
            set { _memcachKey = value; }
        }
        /// <summary>
        /// 配置文件信息
        /// </summary>
        public static string LpFileName
        {
            get { return _lpFileName; }
            set { _lpFileName = value; }
        }

        private static WxConfig _WxConfig = new WxConfig();
        /// <summary>
        /// 微信配置
        /// </summary>
        public static WxConfig WxConfig { get { return _WxConfig; } set { _WxConfig = value; } }

        public static ComSmsConfig ComSmsConfig  { get;set;}
        /// <summary>
        /// 微信配置信息
        /// </summary>
        public static void GetSysConfig()
        {
            AliSmsConfig AliSms = new AliSmsConfig();
            ComSmsConfig ComSms = new ComSmsConfig();
            WxConfig wx = new WxConfig();
            EmailConfig em = new EmailConfig();
            //
            System.Reflection.PropertyInfo[] AliSmsInfo = AliSms.GetType().GetProperties();
            System.Reflection.PropertyInfo[] ComSmsInfo = ComSms.GetType().GetProperties();
            System.Reflection.PropertyInfo[] wxInfo = wx.GetType().GetProperties();
            System.Reflection.PropertyInfo[] emInfo = em.GetType().GetProperties();
            DataTable dtConfig = YDS6000.BLL.WholeBLL.GetSysConfig(WebConfig.Ledger, "");
            foreach (DataRow dr in dtConfig.Rows)
            {
                foreach (System.Reflection.PropertyInfo f_key in AliSmsInfo)
                {
                    if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                        f_key.SetValue(AliSms, dr["CfValue"], null);
                }
                foreach (System.Reflection.PropertyInfo f_key in ComSmsInfo)
                {
                    if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                        f_key.SetValue(ComSms, dr["CfValue"], null);
                }
                foreach (System.Reflection.PropertyInfo f_key in wxInfo)
                {
                    if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                        f_key.SetValue(wx, dr["CfValue"], null);
                }
                foreach (System.Reflection.PropertyInfo f_key in emInfo)
                {
                    if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                        f_key.SetValue(em, dr["CfValue"], null);
                }
            }
            //SmsFun.ComSmsConfig = ComSms;
            WebConfig.ComSmsConfig = ComSms;
            SmsFun.AliSmsConfig = AliSms;
            EmailUtilities.EmConfig = em;
            WebConfig.WxConfig = wx;
            //
            WxPayAPI.WxPayConfig.APPID = wx.WxAPPID;
            WxPayAPI.WxPayConfig.MCHID = wx.WxMCHID;
            WxPayAPI.WxPayConfig.KEY = wx.WxKEY;
            WxPayAPI.WxPayConfig.APPSECRET = wx.WxAPPSECRET;
            WxPayAPI.WxPayConfig.Redirect_uri = wx.WxRedirect_uri;
            WxPayAPI.WxPayConfig.LOG_LEVENL = 3;
        }


        /// <summary>
        /// 获取系统Session 
        /// </summary>
        /// <returns></returns>
        public static CacheUser GetSession()
        {
            string msg = "";
            CacheUser user = GetAuthorizeUser(out msg);
            if (user == null)
            {
                user = new CacheUser();
                user.Ledger = WebConfig.Ledger;
                user.Uid = 0;
                user.CacheKey = "";
            }
            return user;
        }

        public static CacheUser GetAuthorizeUser(out string msg)
        {
            msg = "";
            CacheUser user = null;
            user = HttpContext.Current.Session["CacheUser"] as CacheUser;
            if (user == null)
            {
                if (HttpContext.Current.Request.Headers != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Headers.Get("Ticket")))
                {
                    string ticket = CommFunc.ConvertDBNullToString(HttpContext.Current.Request.Headers.Get("Ticket"));
                    int len = ticket.Length;
                    if (len >= 14)
                    {
                        string flag = CommFunc.ConvertDBNullToString(ticket.Substring(10, 1));
                        int seed = CommFunc.CharToNuner(flag);
                        string kk = CommFunc.ConvertDBNullToString(ticket.Substring(11, len - 11));
                        string[] arr = kk.Split(new string[] { flag }, StringSplitOptions.RemoveEmptyEntries);
                        bool isPass = true;
                        int ledger = 0, uid = 0;
                        if (arr.Count() != 2)
                        {
                            msg = "Ticket信息错误";
                            isPass = false;
                        }
                        if (isPass == true)
                        {
                            ledger = CommFunc.ConvertDBNullToInt32(arr[0]) - seed;
                            uid = CommFunc.ConvertDBNullToInt32(arr[1]) - seed;
                            if (ledger <= 0 && uid < 0)
                            {
                                msg = "Ticket包含错误信息";
                                isPass = false;
                            }
                        }
                        if (isPass == true)
                        {
                            string ccKey = ledger.ToString() + "A" + uid.ToString();
                            string ss = HttpContext.Current.Request.RawUrl;                           
                            if (ss.Contains("api/ExpApp/"))
                                ccKey = ledger.ToString() + "APP" + uid.ToString();
                            else if (ss.Contains("api/ExpAdminApp/"))
                                ccKey = ledger.ToString() + "AdminApp" + uid.ToString();
                            user = HttpRuntime.Cache.Get(ccKey) as CacheUser;
                        }
                        //int seed = CommFunc.ConvertDBNullToInt32(ticket.Substring(10, 3));
                        //int uid = CommFunc.ConvertDBNullToInt32(ticket.Substring(13, 7)) - seed;
                        //user = HttpRuntime.Cache.Get(uid.ToString()) as CacheUser;
                    }
                }
                else
                {
                    msg = "没有Ticket信息";
                }
            }
            //if (user == null && HttpContext.Current.Request.Url.Host.ToLower().Equals("localhost"))
            //{/*本地代码执行*/
            //    user = new CacheUser();
            //    user.Ledger = WebConfig.Ledger;
            //    user.Uid = 1;
            //}
            return user;
        }

        public static bool GetApiShow(System.Web.Http.Controllers.HttpControllerDescriptor actionContext,string project)
        {
            string prefix = "";
            var obj = actionContext.GetCustomAttributes<System.Web.Http.RoutePrefixAttribute>();
            if (obj != null)
            {
                foreach (System.Web.Http.RoutePrefixAttribute md in obj)
                    prefix = md.Prefix;
            }
            return prefix.ToLower().Contains(project.ToLower());
        }

    }
    public class FiledName
    {
        public static string FiledBuildName { get; set; }
        public static string FiledBoomName { get; set; }        
    }


}