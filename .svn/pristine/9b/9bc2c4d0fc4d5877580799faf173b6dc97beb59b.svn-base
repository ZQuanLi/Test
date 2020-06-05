using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YDS6000.Models;

namespace YDS6000.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            Start();
        }
        public override void Init()
        {/*目的增加seesion*/
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            base.Init();
        }
        /// <summary>
        /// 跨域设置  
        /// </summary>
        protected void Application_BeginRequest()
        {
            //OPTIONS请求方法的主要作用：
            //1、获取服务器支持的HTTP请求方法；也是黑客经常使用的方法。
            //2、用来检查服务器的性能。如：AJAX进行跨域请求时的预检，需要向另外一个域名的资源发送一个HTTP OPTIONS请求头，用以判断实际发送的请求是否安全。
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod.ToUpper() == "OPTIONS")
            {
                //表示对输出的内容进行缓冲，执行page.Response.Flush()时，会等所有内容缓冲完毕，将内容发送到客户端。
                //这样就不会出错，造成页面卡死状态，让用户无限制的等下去
                Response.StatusCode = 200;
                Response.SubStatusCode = 200;
                Response.End();
            }
        }

        private void Start()
        {
            try
            {
                if (string.IsNullOrEmpty(WebConfig.LpFileName))
                {
                    //string lpFileName = HttpContext.Current.Request.PhysicalApplicationPath + "web.ini";
                    string lpFileName = AppDomain.CurrentDomain.BaseDirectory + "web.ini";
                    string project = IniHepler.GetConfig(lpFileName, "Automatic", "Project");
                    WebConfig.Ledger = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Automatic", "Ledger"));
                    WebConfig.Port = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(lpFileName, "Automatic", "Port"));
                    WebConfig.MemcachKey = project + "->" + WebConfig.Ledger + "->";
                    string connect = ConfigHelper.GetConnectionStrings("DefaultConnection");
                    string dbHost = IniHepler.GetConfig(lpFileName, "Db", "DbHost");
                    string dbName = IniHepler.GetConfig(lpFileName, "Db", "DbName");
                    string dbUid = IniHepler.GetConfig(lpFileName, "Db", "DbUid");
                    string dbPwd = IniHepler.GetConfig(lpFileName, "Db", "DBPwd");
                    dbPwd = ConfigHelper.Decrypt(dbPwd);
                    //
                    connect = string.Format(connect, dbHost, dbName, dbUid, dbPwd);
                    YDS6000.BLL.WholeBLL.ConnectionString(connect);
                    WebConfig.LpFileName = lpFileName;
                    Automatic.Start();
                    FileLog.WriteLog("启动成功:" + project);
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("启动错误:" + ex.Message + ex.StackTrace);
            }
        }
    }
}
